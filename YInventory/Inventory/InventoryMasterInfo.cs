using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YLR.YInventory.SupplierAndClient;
using YLR.YInventory.Warehouse;
using YLR.YSystem.DataDictionary;
using YLR.YSystem.Organization;

namespace YLR.YInventory.Inventory
{
    /// <summary>
    /// 库存单实体类。
    /// 作者：董帅 创建时间：2012-9-6 21:39:16
    /// </summary>
    public class InventoryMasterInfo
    {
        /// <summary>
        /// 库存单id。
        /// </summary>
        protected int _id = -1;

        /// <summary>
        /// 库存单id。
        /// </summary>
        public int id
        {
            get { return this._id; }
            set { this._id = value; }
        }

        /// <summary>
        /// 编号。
        /// </summary>
        protected string _number = "";

        /// <summary>
        /// 编号。
        /// </summary>
        public string number
        {
            get { return this._number; }
            set { this._number = value; }
        }

        /// <summary>
        /// 客户或供应商。
        /// </summary>
        protected SupplierAndClientInfo _supplierAndClient = new SupplierAndClientInfo();

        /// <summary>
        /// 客户或供应商。
        /// </summary>
        public SupplierAndClientInfo supplierAndClient
        {
            get { return this._supplierAndClient; }
            set { this._supplierAndClient = value; }
        }

        /// <summary>
        /// 仓库。
        /// </summary>
        protected WarehouseInfo _warehouse = new WarehouseInfo();

        /// <summary>
        /// 仓库。
        /// </summary>
        public WarehouseInfo warehouse
        {
            get { return this._warehouse; }
            set { this._warehouse = value; }
        }

        /// <summary>
        /// 类型。
        /// </summary>
        protected int _type = 1;

        /// <summary>
        /// 类型。
        /// </summary>
        public int type
        {
            get { return this._type; }
            set { _type = value; }
        }

        /// <summary>
        /// 库存单类型文本。
        /// </summary>
        protected string[] _typeText = new string[] 
                                        {
                                            "未定义",
                                            "入库单",
                                            "出库单",
                                            "退货单",
                                            "退库单"
                                        };

        /// <summary>
        /// 库存单文本。
        /// </summary>
        public string typeText
        {
            get{ return this._typeText[this._type]; }
        }

        /// <summary>
        /// 状态。
        /// </summary>
        protected int _state = 1;

        /// <summary>
        /// 状态。
        /// </summary>
        public int state
        {
            get { return this._state; }
            set { _state = value; }
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
        /// 状态文本
        /// </summary>
        public string stateText
        {
            get { return this._stateText[this._state]; }
        }

        /// <summary>
        /// 创建时间。
        /// </summary>
        protected DateTime? _createTime = null;

        /// <summary>
        /// 创建时间。
        /// </summary>
        public DateTime? createTime
        {
            get { return this._createTime; }
            set { this._createTime = value; }
        }

        /// <summary>
        /// 创建人。
        /// </summary>
        protected UserInfo _createUser = null;

        /// <summary>
        /// 创建人。
        /// </summary>
        public UserInfo createUser
        {
            get { return this._createUser; }
            set { this._createUser = value; }
        }

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
    }
}
