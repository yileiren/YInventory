using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YLR.YInventory.Inventory.pushOutStorage
{
    /// <summary>
    /// 出库单使用的库存明细对象，继承库存明细对象，增加剩余库存属相。
    /// 作者：董帅 创建时间：2012-9-14 17:51:23
    /// </summary>
    public class PushOutInventoryDetailInfo : InventoryDetailInfo
    {
        /// <summary>
        /// 剩余数量。
        /// </summary>
        protected int _surplusCount = 0;

        /// <summary>
        /// 剩余数量。
        /// </summary>
        public int surplusCount
        {
            get { return this._surplusCount; }
            set { this._surplusCount = value; }
        }
    }
}
