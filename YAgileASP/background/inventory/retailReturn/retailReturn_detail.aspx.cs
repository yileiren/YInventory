﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using YLR.YInventory.Inventory;
using YLR.YSystem.Organization;
using YLR.YInventory.Inventory.retrnToStorage;
using YLR.YMessage;
using YLR.YInventory.Inventory.RetrnToStorage;

namespace YAgileASP.background.inventory.retailReturn
{
    public partial class retailReturn_detail : System.Web.UI.Page
    {
        protected InventoryMasterInfo inv = null; //退货单
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
                                this.inv = oper.getRetrnToStorage(Convert.ToInt32(strId));
                                if (this.inv != null)
                                {
                                    this.txtNumber.InnerText = this.inv.number;
                                    this.txtWarehouse.InnerText = this.inv.warehouse.name;
                                    this.txtSupplier.InnerText = this.inv.supplierAndClient.name;
                                    this.txtState.InnerText = this.inv.stateText;
                                    this.txtCreateTime.InnerText = Convert.ToDateTime(this.inv.createTime).ToString("yyyy年MM月dd HH:mm:ss");
                                    this.txtCreateUser.InnerText = this.inv.createUser.name;

                                    if (this.inv.executeTime != null && this.inv.executeUser != null)
                                    {
                                        this.butAdd.Disabled = true;
                                        this.butEdit.Disabled = true;
                                        this.butDelete.Disabled = true;
                                        this.butExecute.Disabled = true;
                                        this.txtExecuteTime.InnerText = Convert.ToDateTime(this.inv.executeTime).ToString("yyyy年MM月dd HH:mm:ss");
                                        this.txtExecuteUser.InnerText = this.inv.executeUser.name;
                                    }
                                    else
                                    {
                                        this.butDustbin.Disabled = true;
                                    }

                                    this.bindDetails();
                                }
                                else
                                {
                                    YMessageBox.show(this, "获取出库单信息失败！错误信息[" + oper.errorMessage + "]");
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
        /// 绑定明细项数据。
        /// 作者：董帅 创建时间：2012-9-14 22:56:01
        /// </summary>
        public void bindDetails()
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
                    List<RetrnToStorageInventoryDetailInfo> goods = invOper.getRetrnToStorageDetailByMasterId(Convert.ToInt32(this.hidPutInStorageId.Value));
                    if (goods != null)
                    {
                        this.repeaterList.DataSource = goods;
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
        /// 删除数据
        /// 作者：董帅 创建时间：2012-9-15 16:12:53
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void butDeleteItems_Click(object sender, EventArgs e)
        {
            try
            {
                string s = Request["chkDetail"];
                string[] detailIds = new string[0];
                if (!string.IsNullOrEmpty(s))
                {
                    detailIds = s.Split(','); //要删除的仓库id
                }

                if (detailIds.Length > 0)
                {
                    //获取配置文件路径。
                    string configFile = AppDomain.CurrentDomain.BaseDirectory.ToString() + "DataBaseConfig.xml";

                    //创建数据库操作对象。
                    RetrnToStorageOperater oper = RetrnToStorageOperater.createRetrnToStorageOperater(configFile, "SQLServer");
                    if (oper != null)
                    {

                        //删除入库单明细
                        int[] detailIntIds = new int[detailIds.Length];
                        for (int i = 0; i < detailIds.Length; i++)
                        {
                            detailIntIds[i] = Convert.ToInt32(detailIds[i]);
                        }

                        if (oper.deleteRetrnToStorageDetail(detailIntIds))
                        {
                            this.Response.Redirect("retailReturn_detail.aspx?id=" + this.hidPutInStorageId.Value);
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

        /// <summary>
        /// 执行出库。
        /// 作者：董帅 创建时间：2012-9-15 16:18:29
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void butExecute_Click(object sender, EventArgs e)
        {
            try
            {
                //获取用户信息。
                UserInfo user = (UserInfo)Session["UserInfo"];

                if (user != null)
                {
                    //获取配置文件路径。
                    string configFile = AppDomain.CurrentDomain.BaseDirectory.ToString() + "DataBaseConfig.xml";

                    //创建数据库操作对象。
                    RetrnToStorageOperater oper = RetrnToStorageOperater.createRetrnToStorageOperater(configFile, "SQLServer");
                    if (oper != null)
                    {
                        //出库单
                        this.inv = oper.getRetrnToStorage(Convert.ToInt32(this.hidPutInStorageId.Value));

                        if (oper.executeRetrnToStorage(Convert.ToInt32(this.hidPutInStorageId.Value), user))
                        {
                            this.Response.Redirect("retailReturn_detail.aspx?id=" + this.hidPutInStorageId.Value);
                            //YMessageBox.showAndResponseScript(this, "删除数据成功！", "", "window.location.href='dataDictionary_list.aspx?parentId=" + this.hidParentId.Value + "'");
                        }
                        else
                        {
                            YMessageBox.show(this, "执行失败！错误信息[" + oper.errorMessage + "]");
                        }
                    }
                    else
                    {
                        YMessageBox.show(this, "获取数据库实例失败！");
                    }
                }
                else
                {
                    YMessageBox.showAndRedirect(this, "用户登陆超时，请重新登陆！", "../../sys/login.aspx");
                }
            }
            catch (Exception ex)
            {
                YMessageBox.show(this, "系统运行异常！异常信息[" + ex.Message + "]");
            }
        }
    }
}