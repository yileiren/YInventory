using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using YLR.YMessage;
using YLR.YInventory.SupplierAndClient;

namespace YAgileASP.background.inventory.supplierAndClient
{
    public partial class supplierAndClient_list : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!this.IsPostBack)
                {
                    this.bindSupplierAndClientInfo();
                }
            }
            catch (Exception ex)
            {
                YMessageBox.show(this, "程序运行出错！错误信息[" + ex.Message + "]");
            }
        }

        /// <summary>
        /// 绑定客户和供应商。
        /// 作者：董帅 创建时间：2012-9-2 9:43:55
        /// </summary>
        public void bindSupplierAndClientInfo()
        {
            try
            {
                //获取配置文件路径。
                string configFile = AppDomain.CurrentDomain.BaseDirectory.ToString() + "DataBaseConfig.xml";

                //获取数据库操作对象
                SupplierAndClientOperater oper = SupplierAndClientOperater.createSupplierAndClientOperater(configFile, "SQLServer");
                if (oper != null)
                {
                    //获取仓库列表
                    List<SupplierAndClientInfo> supplierAndClients = oper.getSupplierAndClient();
                    if (supplierAndClients != null)
                    {
                        this.waresList.DataSource = supplierAndClients;
                        this.waresList.DataBind();
                    }
                    else
                    {
                        YMessageBox.show(this, "获取数据失败！错误信息[" + oper.errorMessage + "]");
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
        /// 作者：董帅 创建时间：2012-9-2 10:01:46
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void butDeleteItems_Click(object sender, EventArgs e)
        {
            try
            {
                string s = Request["chkSOrC"];
                string[] sOrCIds = new string[0];
                if (!string.IsNullOrEmpty(s))
                {
                    sOrCIds = s.Split(','); //要删除的id
                }

                if (sOrCIds.Length > 0)
                {
                    //获取配置文件路径。
                    string configFile = AppDomain.CurrentDomain.BaseDirectory.ToString() + "DataBaseConfig.xml";

                    //创建数据库操作对象。
                    SupplierAndClientOperater oper = SupplierAndClientOperater.createSupplierAndClientOperater(configFile, "SQLServer");
                    if (oper != null)
                    {

                        //删除
                        int[] sOrCIntIds = new int[sOrCIds.Length];
                        for (int i = 0; i < sOrCIds.Length; i++)
                        {
                            sOrCIntIds[i] = Convert.ToInt32(sOrCIds[i]);
                        }

                        if (oper.deleteSupplierAndClient(sOrCIntIds))
                        {
                            this.Response.Redirect("supplierAndClient_list.aspx");
                            //YMessageBox.showAndResponseScript(this, "删除数据成功！", "", "window.location.href='dataDictionary_list.aspx?parentId=" + this.hidParentId.Value + "'");
                        }
                        else
                        {
                            YMessageBox.show(this, "删除数据失败！");
                        }
                    }
                    else
                    {
                        YMessageBox.show(this, "获取数据库实例失败！");
                    }
                }
                else
                {
                    YMessageBox.show(this, "没有选择要删除的数据！");
                }
            }
            catch (Exception ex)
            {
                YMessageBox.show(this, "系统运行异常！异常信息[" + ex.Message + "]");
            }
        }
    }
}