using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Json;

namespace YLR.YInventory.Warehouse
{
    /// <summary>
    /// 仓库实体类。
    /// 作者：董帅 创建时间：2012-8-30 11:28:13
    /// </summary>
    public class WarehouseInfo
    {
        /// <summary>
        /// 仓库id，默认值是-1，表示没有id。
        /// </summary>
        protected int _id = -1;

        /// <summary>
        /// 仓库id，默认值是-1，表示没有id。
        /// </summary>
        public int id
        {
            get { return this._id; }
            set { this._id = value; }
        }

        /// <summary>
        /// 父仓库id，默认值是-1，表示顶级仓库。
        /// </summary>
        protected int _parentId = -1;

        /// <summary>
        /// 父仓库id，默认值是-1，表示顶级仓库。
        /// </summary>
        public int parentId
        {
            get { return this._parentId; }
            set { this._parentId = value; }
        }

        /// <summary>
        /// 仓库名称。
        /// </summary>
        protected string _name = "";

        /// <summary>
        /// 仓库名称。
        /// </summary>
        public string name
        {
            get { return this._name; }
            set { this._name = value; }
        }

        /// <summary>
        /// 仓库说明。
        /// </summary>
        protected string _explain = "";

        /// <summary>
        /// 仓库说明。
        /// </summary>
        public string explain
        {
            get { return this._explain; }
            set { this._explain = value; }
        }

        /// <summary>
        /// 排序序号。
        /// </summary>
        protected int _order = 0;

        /// <summary>
        /// 排序序号。
        /// </summary>
        public int order
        {
            get { return this._order; }
            set { this._order = value; }
        }
    }
}
