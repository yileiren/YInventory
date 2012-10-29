using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using YLR.YInventory.Inventory.retrnToStorage;
using YLR.YMessage;
using YLR.YInventory.Inventory.RetrnToStorage;

namespace YAgileASP.background.inventory.retrnToStorage
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
                        this.hidWarehouseId.Value = warehouseId;

                        this.txtExecuteBegin.Value = DateTime.Now.ToString("yyyy-MM-dd") + " 00:00:00";
                        this.txttExecuteeEnd.Value = DateTime.Now.ToString("yyyy-MM-dd") + " 23:59:59";

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
                RetrnToStorageOperater invOper = RetrnToStorageOperater.createRetrnToStorageOperater(configFile, "SQLServer");
                if (invOper != null)
                {
                    //获取入库单列表
                    List<RetrnToStorageInventoryDetailInfo> goods = invOper.getGoodsCountsByWarehouseId(warehouseId,this.txtExecuteBegin.Value,this.txttExecuteeEnd.Value);
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
        /// 筛选数据。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void butSearch_Click(object sender, EventArgs e)
        {
            this.bindDetails(Convert.ToInt32(this.hidWarehouseId.Value));
        }
    }
}