using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YLR.YInventory.Inventory
{
    /// <summary>
    /// 库存单状态类。
    /// 作者：董帅 创建时间：2012-9-7 13:28:23
    /// </summary>
    public class InventoryStates
    {
        /// <summary>
        /// 状态结构。
        /// </summary>
        public struct States
        {
            /// <summary>
            /// 状态值。
            /// </summary>
            public int value
            {
                get;
                set;
            }

            /// <summary>
            /// 状态名称。
            /// </summary>
            public string name
            {
                get;
                set;
            }
        }

        /// <summary>
        /// 状态文本
        /// </summary>
        protected string[] _stateText = new string[] 
                        {
                            "未定义",
                            "创建",
                            "执行",
                            "作废"
                        };

        /// <summary>
        /// 获取所有库存单状态。
        /// 作者：董帅 创建时间：2012-9-7 13:31:39
        /// </summary>
        /// <returns>状态列表</returns>
        public List<States> getAllStates()
        { 
            List<States> s = new List<States>();
            for (int i = 1; i < this._stateText.Length; i++)
            {
                States states = new States();
                states.value = i;
                states.name = this._stateText[i];
                s.Add(states);
            }
            return s;
        }
    }
}
