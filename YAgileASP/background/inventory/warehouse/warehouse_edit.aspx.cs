using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using YLR.YMessage;
using YLR.YInventory.Warehouse;

namespace YAgileASP.background.inventory.warehouse
{
    public partial class warehouse_edit : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
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

                //获取id
                string strId = Request.QueryString["id"];
                if (!string.IsNullOrEmpty(strId))
                {
                    this.hidWarehouseId.Value = strId;

                    //获取配置文件路径。
                    string configFile = AppDomain.CurrentDomain.BaseDirectory.ToString() + "DataBaseConfig.xml";

                    //创建操作对象
                    WarehouseOperater wareOper = WarehouseOperater.createWarehouseOperater(configFile, "SQLServer");
                    if (wareOper != null)
                    {
                        //获取仓库信息
                        WarehouseInfo wareInfo = wareOper.getWarehouse(Convert.ToInt32(strId));
                        if (wareInfo != null)
                        {
                            this.txtWarehouseName.Value = wareInfo.name;
                            this.txtWarehouseExplain.Value = wareInfo.explain;
                            this.txtWarehouseOrder.Value = wareInfo.order.ToString();
                        }
                        else
                        {
                            YMessageBox.show(this, "获取仓库信息失败！错误信息[" + wareOper.errorMessage + "]");
                            return;
                        }
                    }
                    else
                    {
                        YMessageBox.show(this, "创建数据库操作对象失败！");
                        return;
                    }
                }
            }
        }

        /// <summary>
        /// 保存数据。
        /// 作者：董帅 创建时间：2012-8-30 11:24:27
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void butSave_Click(object sender, EventArgs e)
        {
            try
            {
                WarehouseInfo wareInfo = new WarehouseInfo();

                wareInfo.name = this.txtWarehouseName.Value;
                if (string.IsNullOrEmpty(wareInfo.name) || wareInfo.name.Length > 50)
                {
                    YMessageBox.show(this, "名称不合法！");
                    return;
                }

                wareInfo.explain = this.txtWarehouseExplain.Value;
                if (wareInfo.explain.Length > 200)
                {
                    YMessageBox.show(this, "名称不合法！");
                    return;
                }

                wareInfo.order = Convert.ToInt32(this.txtWarehouseOrder.Value);
                wareInfo.parentId = Convert.ToInt32(this.hidParentId.Value);

                //获取配置文件路径。
                string configFile = AppDomain.CurrentDomain.BaseDirectory.ToString() + "DataBaseConfig.xml";

                //创建操作对象
                WarehouseOperater wareDicOper = WarehouseOperater.createWarehouseOperater(configFile, "SQLServer");
                if (wareDicOper != null)
                {
                    if (string.IsNullOrEmpty(this.hidWarehouseId.Value))
                    {
                        //新增
                        if (wareDicOper.createNewWarehouse(wareInfo) > 0)
                        {
                            YMessageBox.showAndResponseScript(this, "保存成功！", "window.parent.closePopupsWindow('#popups');", "window.parent.menuButtonOnClick('仓库管理','icon-warehouse','inventory/warehouse/warehouse_list.aspx?parentId=" + this.hidParentId.Value + "')");
                        }
                        else
                        {
                            YMessageBox.show(this, "创建失败！错误信息：[" + wareDicOper.errorMessage + "]");
                            return;
                        }
                    }
                    else
                    {
                        //修改
                        wareInfo.id = Convert.ToInt32(this.hidWarehouseId.Value);
                        if (wareDicOper.changeWarehouse(wareInfo))
                        {
                            YMessageBox.showAndResponseScript(this, "保存成功！", "window.parent.closePopupsWindow('#popups');", "window.parent.menuButtonOnClick('仓库管理','icon-warehouse','inventory/warehouse/warehouse_list.aspx?parentId=" + this.hidParentId.Value + "')");
                        }
                        else
                        {
                            YMessageBox.show(this, "修改失败！错误信息：[" + wareDicOper.errorMessage + "]");
                            return;
                        }
                    }
                }
                else
                {
                    YMessageBox.show(this, "创建数据库操作对象失败！");
                    return;
                }
            }
            catch (Exception ex)
            {
                YMessageBox.show(this, "程序异常！错误信息[" + ex.Message + "]");
            }
        }
    }
}