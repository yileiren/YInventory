using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using YLR.YInventory.SupplierAndClient;
using YLR.YMessage;

namespace YAgileASP.background.inventory.supplierAndClient
{
    public partial class supplierAndClient_edit : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                //获取id
                string strId = Request.QueryString["id"];
                if (!string.IsNullOrEmpty(strId))
                {
                    this.hidSupplierAndClientId.Value = strId;

                    //获取配置文件路径。
                    string configFile = AppDomain.CurrentDomain.BaseDirectory.ToString() + "DataBaseConfig.xml";

                    //创建操作对象
                    SupplierAndClientOperater oper = SupplierAndClientOperater.createSupplierAndClientOperater(configFile, "SQLServer");
                    if (oper != null)
                    {
                        //获取仓库信息
                        SupplierAndClientInfo sOrCInfo = oper.getSupplierAndClient(Convert.ToInt32(strId));
                        if (sOrCInfo != null)
                        {
                            this.txtSupplierAndClientName.Value = sOrCInfo.name;
                            this.txtSupplierAndClientNum.Value = sOrCInfo.number;
                        }
                        else
                        {
                            YMessageBox.show(this, "获取数据失败！错误信息[" + oper.errorMessage + "]");
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
        /// 作者：董帅 创建时间：2012-9-2 9:22:46
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void butSave_Click(object sender, EventArgs e)
        {
            try
            {
                SupplierAndClientInfo supplierAndClient = new SupplierAndClientInfo();

                supplierAndClient.name = this.txtSupplierAndClientName.Value;
                if (string.IsNullOrEmpty(supplierAndClient.name) || supplierAndClient.name.Length > 50)
                {
                    YMessageBox.show(this, "名称不合法！");
                    return;
                }

                supplierAndClient.number = this.txtSupplierAndClientNum.Value;
                if (supplierAndClient.number.Length > 30)
                {
                    YMessageBox.show(this, "编号不合法！");
                    return;
                }

                //获取配置文件路径。
                string configFile = AppDomain.CurrentDomain.BaseDirectory.ToString() + "DataBaseConfig.xml";

                //创建操作对象
                SupplierAndClientOperater oper = SupplierAndClientOperater.createSupplierAndClientOperater(configFile, "SQLServer");
                if (oper != null)
                {
                    if (string.IsNullOrEmpty(this.hidSupplierAndClientId.Value))
                    {
                        //新增
                        if (oper.createNewSupplierAndClient(supplierAndClient) > 0)
                        {
                            YMessageBox.showAndResponseScript(this, "保存成功！", "window.parent.closePopupsWindow('#popups');", "window.parent.menuButtonOnClick('客户和供应商管理','icon-supplier','inventory/supplierAndClient/supplierAndClient_list.aspx')");
                        }
                        else
                        {
                            YMessageBox.show(this, "创建失败！错误信息：[" + oper.errorMessage + "]");
                            return;
                        }
                    }
                    else
                    {
                        //修改
                        supplierAndClient.id = Convert.ToInt32(this.hidSupplierAndClientId.Value);
                        if (oper.changeSupplierAndClient(supplierAndClient))
                        {
                            YMessageBox.showAndResponseScript(this, "保存成功！", "window.parent.closePopupsWindow('#popups');", "window.parent.menuButtonOnClick('客户和供应商管理','icon-supplier','inventory/supplierAndClient/supplierAndClient_list.aspx')");
                        }
                        else
                        {
                            YMessageBox.show(this, "修改失败！错误信息：[" + oper.errorMessage + "]");
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