using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YLR.YSystem.Organization;

namespace YLR.YInventory.Inventory.RetrnToStorage
{
    /// <summary>
    /// 退库单明细实体类。
    /// 作者：董帅 创建时间：2012-9-16 1:18:06
    /// </summary>
    public class RetrnToStorageInventoryDetailInfo : InventoryDetailInfo
    {
        /// <summary>
        /// 执行时间。
        /// </summary>
        protected DateTime? _executeTime = null;

        /// <summary>
        /// 执行时间。
        /// </summary>
        public DateTime? executeTime
        {
            get { return this._executeTime; }
            set { this._executeTime = value; }
        }

        /// <summary>
        /// 执行人。
        /// </summary>
        protected UserInfo _executeUser = null;

        /// <summary>
        /// 执行人。
        /// </summary>
        public UserInfo executeUser
        {
            get { return this._executeUser; }
            set { this._executeUser = value; }
        }

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
