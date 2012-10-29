using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YLR.YSystem.Organization;

namespace YLR.YInventory.Warehouse
{
    /// <summary>
    /// 仓库管理员。
    /// </summary>
    public class WarehouseAdmin : UserInfo
    {
        /// <summary>
        /// 是否选择。
        /// </summary>
        protected bool _choused = false;

        /// <summary>
        /// 是否选择。
        /// </summary>
        public bool choused
        {
            get { return this._choused; }
            set { this._choused = value; }
        }
    }
}
