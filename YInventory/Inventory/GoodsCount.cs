using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YLR.YInventory.Goods;

namespace YLR.YInventory.Inventory
{
    /// <summary>
    /// 货物数量实体类。
    /// 作者：董帅 创建时间：2012-9-11 17:02:26
    /// </summary>
    public class GoodsCount
    {
        /// <summary>
        /// 商品。
        /// </summary>
        protected GoodsInfo _goods = new GoodsInfo();

        /// <summary>
        /// 商品。
        /// </summary>
        public GoodsInfo goods
        {
            get { return this._goods; }
            set { this._goods = value; }
        }

        /// <summary>
        /// 货物的库存明细。
        /// </summary>
        protected List<InventoryDetailInfo> _details = new List<InventoryDetailInfo>();

        /// <summary>
        /// 货物库存明细。
        /// </summary>
        public List<InventoryDetailInfo> details
        {
            get { return this._details; }
            set { this._details = value; }
        }
    }
}
