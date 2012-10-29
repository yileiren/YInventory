using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using YLR.YInventory.Goods;
using YLR.YMessage;
using YLR.YSystem.DataDictionary;
using YLR.YSystem.Organization;
using YLR.YInventory.Inventory;
using YLR.YInventory.Inventory.PutInStorage;

namespace YAgileASP.background.inventory.putInStorage
{
    public partial class putInStorage_detail_edit : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                //获取库存单主表id
                string masterId = Request.QueryString["masterId"];
                if (!string.IsNullOrEmpty(masterId))
                {
                    this.hidMasterId.Value = masterId;
                }
                else
                {
                    YMessageBox.show(this, "获取入库单主表失败！");
                    return;
                }

                this.bindGoods();
                this.bindColor();
                this.bindSize();

                //获取id
                string strId = Request.QueryString["id"];
                if (!string.IsNullOrEmpty(strId))
                {
                    this.hidId.Value = strId;

                    //获取配置文件路径。
                    string configFile = AppDomain.CurrentDomain.BaseDirectory.ToString() + "DataBaseConfig.xml";

                    //创建操作对象
                    PutInStorageOperater oper = PutInStorageOperater.createPutInStorageOperater(configFile, "SQLServer");
                    if (oper != null)
                    {
                        ////获取字典项信息
                        InventoryDetailInfo detail = oper.getPutInStorageDetail(Convert.ToInt32(strId));
                        if (detail != null)
                        {
                            this.selGoods.Value = detail.goods.id.ToString();
                            this.selColor.Value = detail.color.id.ToString();
                            this.selSize.Value = detail.size.ToString();
                            this.txtCount.Value = detail.count.ToString();
                            this.txtUnitPrice.Value = string.Format("{0:N2}",detail.unitPrice);
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
        /// 绑定货物
        /// 作者：董帅 创建时间：2012-9-9 13:28:54
        /// </summary>
        private void bindGoods()
        {
            try
            {
                //获取配置文件路径。
                string configFile = AppDomain.CurrentDomain.BaseDirectory.ToString() + "DataBaseConfig.xml";

                //获取数据库操作对象
                GoodsOperater oper = GoodsOperater.createGoodsOperater(configFile, "SQLServer");
                if (oper != null)
                {
                    //获取仓库列表
                    List<GoodsInfo> goods = oper.getGoods();
                    if (goods != null)
                    {
                        this.selGoods.DataTextField = "name";
                        this.selGoods.DataValueField = "id";
                        this.selGoods.DataSource = goods;
                        this.selGoods.DataBind();
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
        /// 绑定颜色下拉列表。
        /// 作者：董帅 创建时间：2012-9-9 13:57:53
        /// </summary>
        private void bindColor()
        {
            try
            {
                //获取color字典id
                int colorId = 1;
                string strColorId = System.Configuration.ConfigurationManager.AppSettings["colorId"].ToString();
                if (!string.IsNullOrEmpty(strColorId))
                {
                    colorId = Convert.ToInt32(strColorId);
                }

                //获取配置文件路径。
                string configFile = AppDomain.CurrentDomain.BaseDirectory.ToString() + "DataBaseConfig.xml";

                //获取数据库操作对象
                DataDicOperater oper = DataDicOperater.createDataDicOperater(configFile, "SQLServer");
                if (oper != null)
                {
                    //获取字典项列表
                    List<DataDictionaryInfo> datas = oper.getDataDictionaryByParentId(colorId);
                    if (datas != null)
                    {
                        this.selColor.DataTextField = "name";
                        this.selColor.DataValueField = "id";
                        this.selColor.DataSource = datas;
                        this.selColor.DataBind();
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
        /// 绑定尺码下拉列表。
        /// 作者：董帅 创建时间：2012-9-9 13:58:08
        /// </summary>
        private void bindSize()
        {
            try
            {
                //获取color字典id
                int sizeId = 1;
                string strSizeId = System.Configuration.ConfigurationManager.AppSettings["sizeId"].ToString();
                if (!string.IsNullOrEmpty(strSizeId))
                {
                    sizeId = Convert.ToInt32(strSizeId);
                }

                //获取配置文件路径。
                string configFile = AppDomain.CurrentDomain.BaseDirectory.ToString() + "DataBaseConfig.xml";

                //获取数据库操作对象
                DataDicOperater oper = DataDicOperater.createDataDicOperater(configFile, "SQLServer");
                if (oper != null)
                {
                    //获取字典项列表
                    List<DataDictionaryInfo> datas = oper.getDataDictionaryByParentId(sizeId);
                    if (datas != null)
                    {
                        this.selSize.DataTextField = "name";
                        this.selSize.DataValueField = "id";
                        this.selSize.DataSource = datas;
                        this.selSize.DataBind();
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
        /// 作者：董帅 创建时间：2012-9-9 15:53:55
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

                InventoryDetailInfo inventoryDetailInfo = new InventoryDetailInfo();

                //创建明细单
                inventoryDetailInfo.masterId = Convert.ToInt32(this.hidMasterId.Value);
                inventoryDetailInfo.detailId = Convert.ToInt32(this.hidDetailId.Value);
                inventoryDetailInfo.goods.id = Convert.ToInt32(this.selGoods.Value);
                inventoryDetailInfo.color.id = Convert.ToInt32(this.selColor.Value);
                inventoryDetailInfo.size.id = Convert.ToInt32(this.selSize.Value);
                inventoryDetailInfo.count = Convert.ToInt32(this.txtCount.Value);
                inventoryDetailInfo.unitPrice = Convert.ToDouble(this.txtUnitPrice.Value);

                //获取配置文件路径。
                string configFile = AppDomain.CurrentDomain.BaseDirectory.ToString() + "DataBaseConfig.xml";

                //创建操作对象
                PutInStorageOperater oper = PutInStorageOperater.createPutInStorageOperater(configFile, "SQLServer");
                if (oper != null)
                {
                    if (string.IsNullOrEmpty(this.hidId.Value))
                    {
                        //新增
                        if (oper.createNewPutInStorageDetail(inventoryDetailInfo) > 0)
                        {
                            YMessageBox.showAndResponseScript(this, "保存成功！", "window.parent.closePopupsWindow('#popups');", "window.parent.menuButtonOnClick('入库单','icon-putInStorage','inventory/putInStorage/putInStorage_detail.aspx?id=" + this.hidMasterId.Value + "')");
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
                        inventoryDetailInfo.id = Convert.ToInt32(this.hidId.Value);
                        if (oper.changePutInStorageDetail(inventoryDetailInfo))
                        {
                            YMessageBox.showAndResponseScript(this, "保存成功！", "window.parent.closePopupsWindow('#popups');", "window.parent.menuButtonOnClick('入库单','icon-putInStorage','inventory/putInStorage/putInStorage_detail.aspx?id=" + this.hidMasterId.Value + "')");
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