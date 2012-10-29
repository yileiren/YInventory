using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using YLR.YMessage;
using YLR.YInventory.Inventory.RetrnOfGoods;
using YLR.YInventory.Inventory;

namespace YAgileASP.background.inventory.retrnOfGoods
{
    public partial class chouseGoods : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!this.IsPostBack)
                {
                    //获取id
                    string masterId = Request.QueryString["masterId"];
                    if (!string.IsNullOrEmpty(masterId))
                    {
                        this.hidMasterId.Value = masterId;
                    }
                    else
                    {
                        YMessageBox.show(this, "未指定主表id。");
                        return;
                    }


                    string warehouseId = Request.QueryString["warehouseId"];
                    if (!string.IsNullOrEmpty(warehouseId))
                    {
                        this.bindDetails(Convert.ToInt32(warehouseId));
                    }
                    else
                    {
                        YMessageBox.show(this, "未指定仓库。");
                        return;
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
        /// 作者：董帅 创建时间：2012-9-13 22:35:11
        /// <param name="warehouseId">仓库id</param>
        /// </summary>
        public void bindDetails(int warehouseId)
        {
            try
            {
                //获取配置文件路径。
                string configFile = AppDomain.CurrentDomain.BaseDirectory.ToString() + "DataBaseConfig.xml";

                //获取数据库操作对象
                RetrnOfGoodsOperater invOper = RetrnOfGoodsOperater.createRetrnOfGoodsOperater(configFile, "SQLServer");
                if (invOper != null)
                {
                    //获取入库单列表
                    List<GoodsCount> goods = invOper.getGoodsCountsByWarehouseId(warehouseId);
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

        protected void detail_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                Repeater rp = e.Item.FindControl("countsList") as Repeater; //数量数据空间。
                GoodsCount goods = (GoodsCount)(e.Item.DataItem); //插入的数量对象

                goods.details.RemoveAt(0);
                rp.DataSource = goods.details;
                rp.DataBind();
            }
        }
    }
}