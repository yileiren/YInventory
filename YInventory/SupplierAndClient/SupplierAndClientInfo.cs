using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YLR.YInventory.SupplierAndClient
{
    /// <summary>
    /// 客户和供应商实体类，记录客户和供应商的信息。
    /// 作者：董帅 创建时间：2012-9-2 8:55:37
    /// </summary>
    public class SupplierAndClientInfo
    {
        /// <summary>
        /// 客户或供应商id。
        /// </summary>
        public int id
        {
            get;
            set;
        }

        /// <summary>
        /// 客户和供应商名称。
        /// </summary>
        public string name
        {
            get;
            set;
        }

        /// <summary>
        /// 客户和供应商编号。
        /// </summary>
        public string number
        {
            get;
            set;
        }
    }
}
