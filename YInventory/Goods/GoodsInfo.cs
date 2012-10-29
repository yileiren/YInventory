using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YLR.YInventory.Goods
{
    /// <summary>
    /// 货物信息实体类。
    /// 作者：董帅 创建时间：2012-9-2 15:46:48
    /// </summary>
    public class GoodsInfo
    {
        /// <summary>
        /// 货物id。
        /// </summary>
        public int id
        {
            get;
            set;
        }

        /// <summary>
        /// 货物名称。
        /// </summary>
        public string name
        {
            get;
            set;
        }

        /// <summary>
        /// 货物编号。
        /// </summary>
        public string number
        {
            get;
            set;
        }
    }
}
