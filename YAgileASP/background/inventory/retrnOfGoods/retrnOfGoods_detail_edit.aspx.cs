using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using YLR.YMessage;
using YLR.YInventory.Inventory.RetrnOfGoods;
using YLR.YInventory.Inventory.pushOutStorage;

namespace YAgileASP.background.inventory.retrnOfGoods
{
    public partial class retrnOfGoods_detail_edit : System.Web.UI.Page
    {
        protected PushOutInventoryDetailInfo detail = null;

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
                    YMessageBox.show(this, "获取出货单主表失败！");
                    return;
                }

                //获取出货单明细id，如果存在表示新增。
                string chouseDetailId = Request.QueryString["chouseDetailId"];
                //获取配置文件路径。
                string configFile = AppDomain.CurrentDomain.BaseDirectory.ToString() + "DataBaseConfig.xml";
                //获取出库数据
                RetrnOfGoodsOperater oper = RetrnOfGoodsOperater.createRetrnOfGoodsOperater(configFile, "SQLServer");

                if (!string.IsNullOrEmpty(chouseDetailId))
                {
                    this.hidChouseDetailId.Value = chouseDetailId;

                    if (oper != null)
                    {
                        this.detail = oper.getPutInStorageDetail(Convert.ToInt32(chouseDetailId));

                        this.txtGoods.InnerText = this.detail.goods.name;
                        this.txtColor.InnerText = this.detail.color.name;
                        this.txtSize.InnerText = this.detail.size.name;
                        this.txtSurplusCount.InnerText = this.detail.surplusCount.ToString();
                    }
                }
                else
                {
                    //获取id
                    string strId = Request.QueryString["id"];
                    if (!string.IsNullOrEmpty(strId))
                    {
                        this.hidId.Value = strId;

                        if (oper != null)
                        {
                            //获取明细
                            this.detail = oper.getRetrnOfGoodsDetail(Convert.ToInt32(strId));
                            if (detail != null)
                            {
                                this.txtGoods.InnerText = detail.goods.name;
                                this.txtColor.InnerText = detail.color.name;
                                this.txtSize.InnerText = detail.size.name;
                                this.txtSurplusCount.InnerText = detail.surplusCount.ToString();
                                this.txtCount.Value = detail.count.ToString();
                                this.txtUnitPrice.Value = string.Format("{0:N2}", detail.unitPrice);
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
        }

        /// <summary>
        /// 保存数据。
        /// 作者：董帅 创建时间：2012-9-14 21:58:25
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void butSave_Click(object sender, EventArgs e)
        {
            try
            {
                string configFile = AppDomain.CurrentDomain.BaseDirectory.ToString() + "DataBaseConfig.xml";

                //获取出库数据
                RetrnOfGoodsOperater oper = RetrnOfGoodsOperater.createRetrnOfGoodsOperater(configFile, "SQLServer");
                if (oper != null)
                {
                    //保存数据
                    if (string.IsNullOrEmpty(this.hidId.Value))
                    {
                        //构建明细数据
                        this.detail = oper.getPutInStorageDetail(Convert.ToInt32(this.hidChouseDetailId.Value));
                        this.detail.masterId = Convert.ToInt32(this.hidMasterId.Value);
                        this.detail.detailId = Convert.ToInt32(this.hidChouseDetailId.Value);
                        this.detail.count = Convert.ToInt32(this.txtCount.Value);
                        this.detail.unitPrice = Convert.ToDouble(this.txtUnitPrice.Value);

                        //新增
                        if (oper.createNewRetrnOfGoodsDetail(this.detail) > 0)
                        {
                            YMessageBox.showAndResponseScript(this, "保存成功！", "window.parent.closePopupsWindow('#popups');", "window.parent.menuButtonOnClick('退货供应商','icon-retrnOfGoods','inventory/retrnOfGoods/retrnOfGoods_detail.aspx?id=" + this.hidMasterId.Value + "')");
                        }
                        else
                        {
                            YMessageBox.show(this, "创建失败！错误信息：[" + oper.errorMessage + "]");
                            return;
                        }
                    }
                    else
                    {
                        //获取明细
                        this.detail = oper.getRetrnOfGoodsDetail(Convert.ToInt32(this.hidId.Value));

                        this.detail.count = Convert.ToInt32(this.txtCount.Value);
                        this.detail.unitPrice = Convert.ToDouble(this.txtUnitPrice.Value);

                        if (oper.changeRetrnOfGoodsDetail(this.detail))
                        {
                            YMessageBox.showAndResponseScript(this, "保存成功！", "window.parent.closePopupsWindow('#popups');", "window.parent.menuButtonOnClick('退货供应商','icon-retrnOfGoods','inventory/retrnOfGoods/retrnOfGoods_detail.aspx?id=" + this.hidMasterId.Value + "')");
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