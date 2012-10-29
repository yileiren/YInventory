using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using YLR.YSystem.Organization;
using YLR.YMessage;
using YLR.YInventory.Warehouse;
using YLR.YInventory.SupplierAndClient;
using YLR.YInventory.Inventory;
using YLR.YInventory.Inventory.retrnToStorage;

namespace YAgileASP.background.inventory.retailReturn
{
    public partial class retailReturn_list : System.Web.UI.Page
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
                        //绑定状态
                        this.bindStates();

                        //绑定退货单
                        this.bindInvs();
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
        /// 作者：董帅 创建时间：2012-9-15 22:25:26
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
        /// 作者：董帅 创建时间：2012-9-15 22:25:33
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
                        //去除非零售客户
                        List<SupplierAndClientInfo> retailSuppliers = new List<SupplierAndClientInfo>();
                        string strSuppliersIds = System.Configuration.ConfigurationManager.AppSettings["retailSupplierId"].ToString();
                        if (!string.IsNullOrEmpty(strSuppliersIds))
                        {
                            string[] suppliersIds = strSuppliersIds.Split(',');

                            foreach (string id in suppliersIds)
                            {
                                foreach (SupplierAndClientInfo supplier in suppliers)
                                {
                                    if (supplier.id == Convert.ToInt32(id))
                                    {
                                        retailSuppliers.Add(supplier);
                                        break;
                                    }
                                }
                            }
                        }

                        this.selSupplier.DataTextField = "name";
                        this.selSupplier.DataValueField = "id";
                        this.selSupplier.DataSource = retailSuppliers;
                        this.selSupplier.DataBind();
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
        /// 绑定状态下拉列表。
        /// 作者：董帅 创建时间：2012-9-15 22:25:45
        /// </summary>
        private void bindStates()
        {
            try
            {
                InventoryStates invStates = new InventoryStates();
                List<InventoryStates.States> states = invStates.getAllStates();

                this.selStates.DataTextField = "name";
                this.selStates.DataValueField = "value";
                this.selStates.DataSource = states;
                this.selStates.DataBind();
                this.selStates.Items.Insert(0, new ListItem("全部", "0"));
            }
            catch (Exception ex)
            {
                YMessageBox.show(this, "运行错误！错误信息[" + ex.Message + "]");
            }
        }

        /// <summary>
        /// 绑定退货单。
        /// 作者：董帅 创建时间：2012-9-15 22:26:03
        /// </summary>
        public void bindInvs()
        {
            try
            {
                //获取配置文件路径。
                string configFile = AppDomain.CurrentDomain.BaseDirectory.ToString() + "DataBaseConfig.xml";

                //获取数据库操作对象
                RetrnToStorageOperater invOper = RetrnToStorageOperater.createRetrnToStorageOperater(configFile, "SQLServer");
                if (invOper != null)
                {
                    //获取入库单列表
                    List<InventoryMasterInfo> invs = invOper.getRetrnToStorage(this.txtNumber.Value,
                                                                                Convert.ToInt32(this.selSupplier.Value),
                                                                                Convert.ToInt32(this.txtWarehouseName.Value),
                                                                                Convert.ToInt32(this.selStates.Value),
                                                                                this.txtCreateBegin.Value,
                                                                                this.txtCreateEnd.Value);
                    if (invs != null)
                    {
                        this.repeaterList.DataSource = invs;
                        this.repeaterList.DataBind();
                    }
                    else
                    {
                        YMessageBox.show(this, "获取数据失败！错误信息[" + invOper.errorMessage + "]");
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
        /// 筛选数据。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void butSearch_Click(object sender, EventArgs e)
        {
            this.bindInvs();
        }

        /// <summary>
        /// 删除数据
        /// 作者：董帅 创建时间：2012-9-13 14:04:45
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void butDeleteItems_Click(object sender, EventArgs e)
        {
            try
            {
                string s = Request["chkInv"];
                string[] invIds = new string[0];
                if (!string.IsNullOrEmpty(s))
                {
                    invIds = s.Split(','); //要删除的仓库id
                }

                if (invIds.Length > 0)
                {
                    //获取配置文件路径。
                    string configFile = AppDomain.CurrentDomain.BaseDirectory.ToString() + "DataBaseConfig.xml";

                    //创建数据库操作对象。
                    RetrnToStorageOperater oper = RetrnToStorageOperater.createRetrnToStorageOperater(configFile, "SQLServer");
                    if (oper != null)
                    {

                        //删除入库单
                        int[] invIntIds = new int[invIds.Length];
                        for (int i = 0; i < invIds.Length; i++)
                        {
                            invIntIds[i] = Convert.ToInt32(invIds[i]);
                        }

                        if (oper.deleteRetrnToStorage(invIntIds))
                        {
                            this.Response.Redirect("retailReturn_list.aspx");
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