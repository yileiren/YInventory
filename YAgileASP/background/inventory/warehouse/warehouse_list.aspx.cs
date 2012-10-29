using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using YLR.YMessage;
using YLR.YInventory.Warehouse;

namespace YAgileASP.inventory.warehouse
{
    public partial class warehouse_list : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!this.IsPostBack)
                {
                    //获取父id
                    string parentId = Request.QueryString["parentId"];
                    if (!string.IsNullOrEmpty(parentId))
                    {
                        this.hidParentId.Value = parentId;
                    }
                    else
                    {
                        this.hidParentId.Value = "-1";
                    }

                    this.bindWaresInfos();
                }
            }
            catch (Exception ex)
            {
                YMessageBox.show(this, "程序运行出错！错误信息[" + ex.Message + "]");
            }
        }

        /// <summary>
        /// 绑定仓库。
        /// 作者：董帅 创建时间：2012-8-30 13:01:32
        /// </summary>
        public void bindWaresInfos()
        {
            try
            {
                //获取配置文件路径。
                string configFile = AppDomain.CurrentDomain.BaseDirectory.ToString() + "DataBaseConfig.xml";

                //获取数据库操作对象
                WarehouseOperater wareOper = WarehouseOperater.createWarehouseOperater(configFile, "SQLServer");
                if (wareOper != null)
                {
                    //获取父仓库
                    if (this.hidParentId.Value == "-1")
                    {
                        this.spanParentName.InnerText = "顶级仓库";
                        this.returnButton.Disabled = true;
                        this.hidReturnId.Value = "";
                    }
                    else
                    {
                        WarehouseInfo ware = wareOper.getWarehouse(Convert.ToInt32(this.hidParentId.Value));
                        this.spanParentName.InnerText = ware.name;
                        this.hidReturnId.Value = ware.parentId.ToString();
                    }

                    //获取仓库列表
                    List<WarehouseInfo> wares = wareOper.getWarehouseByParentId(Convert.ToInt32(this.hidParentId.Value));
                    if (wares != null)
                    {
                        this.waresList.DataSource = wares;
                        this.waresList.DataBind();
                    }
                    else
                    {
                        YMessageBox.show(this, "获取数据失败！错误信息[" + wareOper.errorMessage + "]");
                    }
                }
                else
                {
                    YMessageBox.show(this, "获取数据库操作对象失败！");
                }
            }
            catch (Exception ex)
            {
                YMessageBox.show(this, "运行错误！错误信息[" + ex.Message + "]");
            }
        }

        /// <summary>
        /// 删除数据
        /// 作者：董帅 创建时间：2012-8-30 13:30:02
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void butDeleteItems_Click(object sender, EventArgs e)
        {
            try
            {
                string s = Request["chkWare"];
                string[] wareIds = new string[0];
                if (!string.IsNullOrEmpty(s))
                {
                    wareIds = s.Split(','); //要删除的仓库id
                }

                if (wareIds.Length > 0)
                {
                    //获取配置文件路径。
                    string configFile = AppDomain.CurrentDomain.BaseDirectory.ToString() + "DataBaseConfig.xml";

                    //创建数据库操作对象。
                    WarehouseOperater dicOper = WarehouseOperater.createWarehouseOperater(configFile, "SQLServer");
                    if (dicOper != null)
                    {

                        //删除仓库
                        int[] wareIntIds = new int[wareIds.Length];
                        for (int i = 0; i < wareIds.Length; i++)
                        {
                            wareIntIds[i] = Convert.ToInt32(wareIds[i]);
                        }

                        if (dicOper.deleteWarehouses(wareIntIds))
                        {
                            this.Response.Redirect("warehouse_list.aspx?parentId=" + this.hidParentId.Value);
                            //YMessageBox.showAndResponseScript(this, "删除数据成功！", "", "window.location.href='dataDictionary_list.aspx?parentId=" + this.hidParentId.Value + "'");
                        }
                        else
                        {
                            YMessageBox.show(this, "删除数据失败！可能是仓库正在使用所以不允许删除！");
                        }
                    }
                    else
                    {
                        YMessageBox.show(this, "获取数据库实例失败！");
                    }
                }
                else
                {
                    YMessageBox.show(this, "没有选择要删除的仓库！");
                }
            }
            catch (Exception ex)
            {
                YMessageBox.show(this, "系统运行异常！异常信息[" + ex.Message + "]");
            }
        }
    }
}