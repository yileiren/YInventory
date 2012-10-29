using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YLR.YInventory.Goods;
using YLR.YSystem.DataDictionary;

namespace YLR.YInventory.Inventory
{
    /// <summary>
    /// 库存单明细实体类。
    /// 作者：董帅 创建时间：2012-9-9 14:18:35
    /// </summary>
    public class InventoryDetailInfo
    {
        /// <summary>
        /// 明细项id。
        /// </summary>
        protected int _id = -1;

        /// <summary>
        /// 明细项id。
        /// </summary>
        public int id
        {
            get { return this._id; }
            set { this._id = value; }
        }

        /// <summary>
        /// 关联明细项id。
        /// </summary>
        protected int _detailId = -1;

        /// <summary>
        /// 关联明细项id。
        /// </summary>
        public int detailId
        {
            get { return this._detailId; }
            set { this._detailId = value; }
        }

        /// <summary>
        /// 主表id。
        /// </summary>
        protected int _masterId = -1;

        /// <summary>
        /// 主表id。
        /// </summary>
        public int masterId
        {
            get { return this._masterId; }
            set { this._masterId = value; }
        }

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
        /// 颜色。
        /// </summary>
        protected DataDictionaryInfo _color = new DataDictionaryInfo();

        /// <summary>
        /// 颜色。
        /// </summary>
        public DataDictionaryInfo color
        {
            get { return this._color; }
            set { this._color = value; }
        }

        /// <summary>
        /// 尺码。
        /// </summary>
        protected DataDictionaryInfo _size = new DataDictionaryInfo();

        /// <summary>
        /// 尺码。
        /// </summary>
        public DataDictionaryInfo size
        {
            get { return this._size; }
            set { this._size = value; }
        }

        /// <summary>
        /// 数量。
        /// </summary>
        protected int _count = 0;

        /// <summary>
        /// 数量。
        /// </summary>
        public int count
        {
            get { return this._count; }
            set { this._count = value; }
        }

        /// <summary>
        /// 单价。
        /// </summary>
        protected double _unitPrice = 0.0;

        /// <summary>
        /// 单价。
        /// </summary>
        public double unitPrice
        {
            get { return this._unitPrice; }
            set { this._unitPrice = value; }
        }
    }
}
