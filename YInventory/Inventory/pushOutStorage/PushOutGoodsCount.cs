using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YLR.YInventory.Goods;

namespace YLR.YInventory.Inventory.pushOutStorage
{
    /// <summary>
    /// 出库单货物数量实体类。
    /// 作者：董帅 创建时间：2012-9-14 23:03:45
    /// </summary>
    public class PushOutGoodsCount
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
        protected List<PushOutInventoryDetailInfo> _details = new List<PushOutInventoryDetailInfo>();

        /// <summary>
        /// 货物库存明细。
        /// </summary>
        public List<PushOutInventoryDetailInfo> details
        {
            get { return this._details; }
            set { this._details = value; }
        }
    }
}
