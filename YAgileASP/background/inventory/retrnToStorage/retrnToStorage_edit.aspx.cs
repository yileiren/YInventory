using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using YLR.YSystem.Organization;
using YLR.YInventory.Inventory.retrnToStorage;
using YLR.YInventory.Inventory;
using YLR.YMessage;
using YLR.YInventory.Warehouse;
using YLR.YInventory.SupplierAndClient;

namespace YAgileASP.background.inventory.retrnToStorage
{
    public partial class retrnToStorage_edit : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!this.IsPostBack)
                {
                    //获取用户信息。
                    UserInfo user = (UserInfo)Session["UserInfo"];

                    if (user != null)
                    {
                        //绑定仓库
                        this.bindWarehouse(user.id);
                        //绑定供应商
                        this.bindSupplier();

                        //获取id
                        string strId = Request.QueryString["id"];
                        if (!string.IsNullOrEmpty(strId))
                        {
                            this.hidPutInStorageId.Value = strId;

                            //获取配置文件路径。
                            string configFile = AppDomain.CurrentDomain.BaseDirectory.ToString() + "DataBaseConfig.xml";

                            //创建操作对象
                            RetrnToStorageOperater oper = RetrnToStorageOperater.createRetrnToStorageOperater(configFile, "SQLServer");
                            if (oper != null)
                            {
                                //获取入库单
                                InventoryMasterInfo inv = oper.getRetrnToStorage(Convert.ToInt32(strId));
                                if (inv != null)
                                {
                                    this.txtWarehouseName.Value = inv.warehouse.ToString();
                                    this.txtSupplier.Value = inv.supplierAndClient.ToString();
                                    this.txtNumber.Value = inv.number;
                                }
                                else
                                {
                                    YMessageBox.show(this, "获取退库单信息失败！错误信息[" + oper.errorMessage + "]");
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
                    else
                    {
                        YMessageBox.showAndRedirect(this, "用户登陆超时，请重新登陆！", "../../sys/login.aspx");
                    }
                }
            }
            catch (Exception ex)
            {
                YMessageBox.show(this, "程序运行出错！错误信息[" + ex.Message + "]");
            }
        }

        /// <summary>
        /// 绑定仓库方法。
        /// 作者：董帅 创建时间：2012-9-13 13:18:47
        /// </summary>
        /// <param name="userId">用户id</param>
        private void bindWarehouse(int userId)
        {
            try
            {
                //获取配置文件路径。
                string configFile = AppDomain.CurrentDomain.BaseDirectory.ToString() + "DataBaseConfig.xml";

                //获取数据库操作对象
                WarehouseOperater wareOper = WarehouseOperater.createWarehouseOperater(configFile, "SQLServer");
                if (wareOper != null)
                {
                    //获取仓库列表
                    List<WarehouseInfo> wares = wareOper.getWarehouseByUserId(userId);
                    if (wares != null)
                    {
                        this.txtWarehouseName.DataTextField = "name";
                        this.txtWarehouseName.DataValueField = "id";
                        this.txtWarehouseName.DataSource = wares;
                        this.txtWarehouseName.DataBind();
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
        /// 绑定供应商。
        /// 作者：董帅 创建时间：2012-9-13 13:19:00
        /// </summary>
        private void bindSupplier()
        {
            try
            {
                //获取配置文件路径。
                string configFile = AppDomain.CurrentDomain.BaseDirectory.ToString() + "DataBaseConfig.xml";

                //获取数据库操作对象
                SupplierAndClientOperater oper = SupplierAndClientOperater.createSupplierAndClientOperater(configFile, "SQLServer");
                if (oper != null)
                {
                    //获取供应商列表
                    List<SupplierAndClientInfo> suppliers = oper.getSupplierAndClient();
                    if (suppliers != null)
                    {
                        this.txtSupplier.DataTextField = "name";
                        this.txtSupplier.DataValueField = "id";
                        this.txtSupplier.DataSource = suppliers;
                        this.txtSupplier.DataBind();
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
        /// 保存数据。
        /// 作者：董帅 创建时间：2012-9-6 22:29:38
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void butSave_Click(object sender, EventArgs e)
        {
            try
            {
                //获取用户信息。
                UserInfo user = (UserInfo)Session["UserInfo"];

                if (user == null)
                {
                    YMessageBox.showAndRedirect(this, "用户登陆超时，请重新登陆！", "../../sys/login.aspx");
                    return;
                }

                InventoryMasterInfo inventoryMasterInfo = new InventoryMasterInfo();

                //创建库存单
                inventoryMasterInfo.number = this.txtNumber.Value;
                if (string.IsNullOrEmpty(inventoryMasterInfo.number) || inventoryMasterInfo.number.Length > 50)
                {
                    YMessageBox.show(this, "退库单编号不合法！");
                    return;
                }

                inventoryMasterInfo.warehouse.id = Convert.ToInt32(this.txtWarehouseName.Value);
                inventoryMasterInfo.supplierAndClient.id = Convert.ToInt32(this.txtSupplier.Value);
                inventoryMasterInfo.createUser = user;
                inventoryMasterInfo.type = 4;

                //获取配置文件路径。
                string configFile = AppDomain.CurrentDomain.BaseDirectory.ToString() + "DataBaseConfig.xml";

                //创建操作对象
                RetrnToStorageOperater oper = RetrnToStorageOperater.createRetrnToStorageOperater(configFile, "SQLServer");
                if (oper != null)
                {
                    if (string.IsNullOrEmpty(this.hidPutInStorageId.Value))
                    {
                        //新增
                        if (oper.createNewRetrnToStorage(inventoryMasterInfo) > 0)
                        {
                            YMessageBox.showAndResponseScript(this, "保存成功！", "window.parent.closePopupsWindow('#popups');", "window.parent.menuButtonOnClick('退库单','icon-retrnToStorage','inventory/retrnToStorage/retrnToStorage_list.aspx')");
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
                        if (oper.changRetrnToStorage(Convert.ToInt32(this.hidPutInStorageId.Value), this.txtNumber.Value, Convert.ToInt32(this.txtSupplier.Value), Convert.ToInt32(this.txtWarehouseName.Value)))
                        {
                            YMessageBox.showAndResponseScript(this, "保存成功！", "window.parent.closePopupsWindow('#popups');", "window.parent.menuButtonOnClick('退库单','icon-retrnToStorage','inventory/retrnToStorage/retrnToStorage_list.aspx')");
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