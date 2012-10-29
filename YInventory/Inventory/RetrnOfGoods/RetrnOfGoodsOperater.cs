using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YLR.YAdoNet;
using System.Data;
using YLR.YInventory.SupplierAndClient;
using YLR.YInventory.Warehouse;
using YLR.YSystem.Organization;
using YLR.YInventory.Goods;
using YLR.YSystem.DataDictionary;
using YLR.YInventory.Inventory.pushOutStorage;

namespace YLR.YInventory.Inventory.RetrnOfGoods
{
    /// <summary>
    /// 退货单数据库处理类。
    /// 作者：董帅 创建时间：2012-9-15 22:01:53
    /// </summary>
    public class RetrnOfGoodsOperater
    {
        /// <summary>
        /// 退货单数据库。
        /// </summary>
        public YDataBase retrnOfGoodsOperaterDataBase
        {
            get;
            set;
        }

        /// <summary>
        /// 错误信息。
        /// </summary>
        protected string _errorMessage = "";

        /// <summary>
        /// 错误信息。
        /// </summary>
        public string errorMessage
        {
            get
            {
                return this._errorMessage;
            }
        }

        /// <summary>
        /// 创建退货单数据库操作对象。
        /// 作者：董帅 创建时间：2012-9-6 21:57:48
        /// </summary>
        /// <param name="configFilePath">配置文件路径。</param>
        /// <param name="nodeName">节点名。</param>
        /// <returns>成功返回操作对象，否则返回null。</returns>
        public static RetrnOfGoodsOperater createRetrnOfGoodsOperater(string configFilePath, string nodeName)
        {
            RetrnOfGoodsOperater oper = null;

            //获取数据库实例。
            YDataBase orgDb = YDataBaseConfigFile.createDataBase(configFilePath, nodeName);

            if (orgDb != null)
            {
                oper = new RetrnOfGoodsOperater();
                oper.retrnOfGoodsOperaterDataBase = orgDb;
            }
            else
            {
                Exception ex = new Exception("创建数据库实例失败！");
                throw ex;
            }

            return oper;
        }

        /// <summary>
        /// 创建退货单。
        /// 作者：董帅 创建时间：2012-9-15 22:11:54
        /// </summary>
        /// <param name="inventoryMaster">退货单。</param>
        /// <returns>退货单id，失败返回-1。</returns>
        public int createNewRetrnOfGoods(InventoryMasterInfo inventoryMaster)
        {
            int inventoryMasterId = -1; //创建id。

            try
            {
                if (inventoryMaster == null)
                {
                    this._errorMessage = "不能插入空退货单！";
                }
                else
                {
                    //存入数据库
                    if (this.retrnOfGoodsOperaterDataBase.connectDataBase())
                    {
                        //新增数据
                        string sql = string.Format("INSERT INTO INV_INVENTORYMASTER (NUMBER,SUPPLIERORCLIENTID,WAREHOUSEID,TYPE,STATE,CREATEUSER) " +
                                "VALUES ('{0}',{1},{2},{3},{4},{5}) SELECT SCOPE_IDENTITY() AS id"
                                , inventoryMaster.number
                                , inventoryMaster.supplierAndClient.id
                                , inventoryMaster.warehouse.id
                                , inventoryMaster.type
                                , inventoryMaster.state
                                , inventoryMaster.createUser.id);

                        DataTable retDt = this.retrnOfGoodsOperaterDataBase.executeSqlReturnDt(sql);
                        if (retDt != null && retDt.Rows.Count > 0)
                        {
                            //获取新增数据id
                            inventoryMasterId = Convert.ToInt32(retDt.Rows[0]["id"]);
                        }
                        else
                        {
                            this._errorMessage = "创建失败！";
                            if (retDt == null)
                            {
                                this._errorMessage += "错误信息[" + this.retrnOfGoodsOperaterDataBase.errorText + "]";
                            }
                        }
                    }
                    else
                    {
                        this._errorMessage = "连接数据库出错！错误信息[" + this.retrnOfGoodsOperaterDataBase.errorText + "]";
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                //断开数据库连接。
                this.retrnOfGoodsOperaterDataBase.disconnectDataBase();
            }

            return inventoryMasterId;
        }

        /// <summary>
        /// 根据DataRow获取退货单。
        /// 作者：董帅 创建时间：2012-9-15 22:21:03
        /// </summary>
        /// <param name="r">退货单数据</param>
        /// <returns>退货单，失败返回null。</returns>
        private InventoryMasterInfo getRetrnOfGoodsFormDataRow(DataRow r)
        {
            if (r != null)
            {
                //仓库对象
                InventoryMasterInfo inv = new InventoryMasterInfo();

                //仓库id不能为null，否则返回失败。
                if (!r.IsNull("ID"))
                {
                    inv.id = Convert.ToInt32(r["ID"]);
                }
                else
                {
                    return null;
                }

                if (!r.IsNull("NUMBER"))
                {
                    inv.number = r["NUMBER"].ToString();
                }

                //获取配置文件路径。
                string configFile = AppDomain.CurrentDomain.BaseDirectory.ToString() + "DataBaseConfig.xml";

                if (!r.IsNull("SUPPLIERORCLIENTID"))
                {
                    SupplierAndClientOperater supplierOper = SupplierAndClientOperater.createSupplierAndClientOperater(configFile, "SQLServer");
                    inv.supplierAndClient = supplierOper.getSupplierAndClient(Convert.ToInt32(r["SUPPLIERORCLIENTID"]));
                }
                else
                {
                    return null;
                }

                if (!r.IsNull("WAREHOUSEID"))
                {
                    WarehouseOperater warehouseOper = WarehouseOperater.createWarehouseOperater(configFile, "SQLServer");
                    inv.warehouse = warehouseOper.getWarehouse(Convert.ToInt32(r["WAREHOUSEID"]));
                }
                else
                {
                    return null;
                }

                if (!r.IsNull("TYPE"))
                {
                    inv.type = Convert.ToInt32(r["TYPE"]);
                }
                else
                {
                    return null;
                }

                if (!r.IsNull("STATE"))
                {
                    inv.state = Convert.ToInt32(r["STATE"]);
                }
                else
                {
                    return null;
                }

                if (!r.IsNull("CREATETIME"))
                {
                    inv.createTime = Convert.ToDateTime(r["CREATETIME"]);
                }
                else
                {
                    return null;
                }

                OrgOperater orgOper = OrgOperater.createOrgOperater(configFile, "SQLServer");
                if (!r.IsNull("CREATEUSER"))
                {
                    inv.createUser = orgOper.getUser(Convert.ToInt32(r["CREATEUSER"]));
                }
                else
                {
                    return null;
                }

                if (!r.IsNull("EXECUTETIME"))
                {
                    inv.executeTime = Convert.ToDateTime(r["EXECUTETIME"]);
                }
                else
                {
                    inv.executeTime = null;
                }

                if (!r.IsNull("EXECUTEUSER"))
                {
                    inv.executeUser = orgOper.getUser(Convert.ToInt32(r["EXECUTEUSER"]));
                }
                else
                {
                    inv.executeUser = null;
                }

                return inv;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 获取退货单。
        /// 作者：董帅 创建时间：2012-9-15 22:20:57
        /// </summary>
        /// <param name="id">退货单id</param>
        /// <returns>成功返回退货单，否则返回null。</returns>
        public InventoryMasterInfo getRetrnOfGoods(int id)
        {
            InventoryMasterInfo inv = null;

            try
            {
                if (this.retrnOfGoodsOperaterDataBase != null)
                {
                    //连接数据库
                    if (this.retrnOfGoodsOperaterDataBase.connectDataBase())
                    {

                        //sql语句，获取退货单
                        string sql = "SELECT * FROM INV_INVENTORYMASTER WHERE ID = " + id.ToString();

                        //获取数据
                        DataTable dt = this.retrnOfGoodsOperaterDataBase.executeSqlReturnDt(sql);
                        if (dt != null && dt.Rows.Count == 1)
                        {
                            inv = this.getRetrnOfGoodsFormDataRow(dt.Rows[0]);
                        }
                        else
                        {
                            this._errorMessage = "获取数据失败！错误信息：[" + this.retrnOfGoodsOperaterDataBase.errorText + "]";
                        }
                    }
                    else
                    {
                        this._errorMessage = "连接数据库失败！错误信息：[" + this.retrnOfGoodsOperaterDataBase.errorText + "]";
                    }
                }
                else
                {
                    this._errorMessage = "未设置数据库实例！";
                }
            }
            catch (Exception ex)
            {
                this._errorMessage = ex.Message;
            }
            finally
            {
                this.retrnOfGoodsOperaterDataBase.disconnectDataBase();
            }

            return inv;
        }

        /// <summary>
        /// 获取退货单。
        /// 作者：董帅 创建时间：2012-9-15 22:20:43
        /// </summary>
        /// <param name="number">退货单编号,为null获空字符串表示不指定。</param>
        /// <param name="supplierId">供应商id，等于0表示不指定。</param>
        /// <param name="warehouseId">仓库id，等于0表示不指定。</param>
        /// <param name="states">退货单状态，等于0表示不指定。</param>
        /// <param name="createBegin">创建开始时间，为null获空字符串表示不指定。</param>
        /// <param name="createEnd">创建截止时间,为null获空字符串表示不指定。</param>
        /// <returns>成功返回退货单列表，否则返回null。</returns>
        public List<InventoryMasterInfo> getRetrnOfGoods(string number, int supplierId, int warehouseId, int states, string createBegin, string createEnd)
        {
            List<InventoryMasterInfo> invs = null;

            try
            {
                if (this.retrnOfGoodsOperaterDataBase != null)
                {
                    //连接数据库
                    if (this.retrnOfGoodsOperaterDataBase.connectDataBase())
                    {

                        //sql语句，获取退货单
                        string sql = "SELECT * FROM INV_INVENTORYMASTER WHERE TYPE = 3";

                        if (!string.IsNullOrEmpty(number))
                        {
                            sql += string.Format(" AND NUMBER = '{0}'", number);
                        }

                        if (supplierId > 0)
                        {
                            sql += " AND SUPPLIERORCLIENTID = " + supplierId.ToString();
                        }

                        if (warehouseId > 0)
                        {
                            sql += " AND WAREHOUSEID = " + warehouseId.ToString();
                        }

                        if (states > 0)
                        {
                            sql += " AND STATE = " + states.ToString();
                        }

                        if (!string.IsNullOrEmpty(createBegin))
                        {
                            sql += string.Format(" AND CREATETIME >= '{0}'", createBegin);
                        }

                        if (!string.IsNullOrEmpty(createEnd))
                        {
                            sql += string.Format(" AND CREATETIME <= '{0}'", createEnd);
                        }

                        sql += " ORDER BY CREATETIME DESC";

                        //获取数据
                        DataTable dt = this.retrnOfGoodsOperaterDataBase.executeSqlReturnDt(sql);
                        if (dt != null)
                        {
                            invs = new List<InventoryMasterInfo>();
                            foreach (DataRow r in dt.Rows)
                            {
                                InventoryMasterInfo inv = this.getRetrnOfGoodsFormDataRow(r);
                                if (inv != null)
                                {
                                    invs.Add(inv);
                                }
                            }
                        }
                        else
                        {
                            this._errorMessage = "获取数据失败！错误信息：[" + this.retrnOfGoodsOperaterDataBase.errorText + "]";
                        }
                    }
                    else
                    {
                        this._errorMessage = "连接数据库失败！错误信息：[" + this.retrnOfGoodsOperaterDataBase.errorText + "]";
                    }
                }
                else
                {
                    this._errorMessage = "未设置数据库实例！";
                }
            }
            catch (Exception ex)
            {
                this._errorMessage = ex.Message;
            }
            finally
            {
                this.retrnOfGoodsOperaterDataBase.disconnectDataBase();
            }

            return invs;
        }

        /// <summary>
        /// 修改退货单信息。
        /// 作者：董帅 创建时间：2012-9-15 22:21:29
        /// </summary>
        /// <param name="id">要修改的退货单id。</param>
        /// <param name="number">退货单编号。</param>
        /// <param name="supplierId">供应商id。</param>
        /// <param name="warehousId">仓库id。</param>
        /// <returns>成功返回true，否则返回false。</returns>
        public bool changeRetrnOfGoods(int id, string number, int supplierId, int warehousId)
        {
            bool bRet = false; //返回值

            try
            {
                if (this.retrnOfGoodsOperaterDataBase != null)
                {
                    //连接数据库
                    if (this.retrnOfGoodsOperaterDataBase.connectDataBase())
                    {
                        //sql语句
                        string sql = string.Format("UPDATE INV_INVENTORYMASTER SET NUMBER = '{0}',SUPPLIERORCLIENTID = {1},WAREHOUSEID = {2} WHERE ID = {3}"
                                , number
                                , supplierId
                                , warehousId
                                , id);

                        int retCount = this.retrnOfGoodsOperaterDataBase.executeSqlWithOutDs(sql);
                        if (retCount == 1)
                        {
                            bRet = true;
                        }
                        else
                        {
                            this._errorMessage = "更新数据失败！";
                            if (retCount != 1)
                            {
                                this._errorMessage += "错误信息[" + this.retrnOfGoodsOperaterDataBase.errorText + "]";
                            }
                        }
                    }
                    else
                    {
                        this._errorMessage = "连接数据库出错！错误信息[" + this.retrnOfGoodsOperaterDataBase.errorText + "]";
                    }

                }
                else
                {
                    this._errorMessage = "未设置数据库实例！";
                }
            }
            catch (Exception ex)
            {
                this._errorMessage = ex.Message;
            }
            finally
            {
                this.retrnOfGoodsOperaterDataBase.disconnectDataBase();
            }

            return bRet;
        }

        /// <summary>
        /// 删除退货单。
        /// 作者：董帅 创建时间：2012-9-15 22:28:42
        /// </summary>
        /// <param name="ids">要删除的退货单id。</param>
        /// <returns>成功返回true，否则返回false。</returns>
        public bool deleteRetrnOfGoods(int[] ids)
        {
            bool bRet = false; //返回值

            try
            {
                if (this.retrnOfGoodsOperaterDataBase != null)
                {
                    //连接数据库
                    if (this.retrnOfGoodsOperaterDataBase.connectDataBase())
                    {
                        //组件id字符串
                        string strIds = "";
                        for (int i = 0; i < ids.Length; i++)
                        {
                            if (i == 0)
                            {
                                strIds += ids[i].ToString();
                            }
                            else
                            {
                                strIds += "," + ids[i].ToString();
                            }
                        }
                        //sql语句
                        string sql = string.Format("DELETE INV_INVENTORYMASTER WHERE STATE = 1 AND ID IN ({0})", strIds);

                        int retCount = this.retrnOfGoodsOperaterDataBase.executeSqlWithOutDs(sql);
                        if (retCount > 0)
                        {
                            bRet = true;
                        }
                        else
                        {
                            this._errorMessage = "删除数据失败！";
                            this._errorMessage += "错误信息[" + this.retrnOfGoodsOperaterDataBase.errorText + "]";
                        }
                    }
                    else
                    {
                        this._errorMessage = "连接数据库出错！错误信息[" + this.retrnOfGoodsOperaterDataBase.errorText + "]";
                    }
                }
                else
                {
                    this._errorMessage = "未设置数据库实例！";
                }
            }
            catch (Exception ex)
            {
                this._errorMessage = ex.Message;
            }
            finally
            {
                this.retrnOfGoodsOperaterDataBase.disconnectDataBase();
            }

            return bRet;
        }

        /// <summary>
        /// 根据DataRow获取剩余库存。
        /// 作者：董帅 创建时间：2012-9-15 23:18:14
        /// </summary>
        /// <param name="r">剩余库存明细数据</param>
        /// <returns>剩余库存，失败返回null。</returns>
        private InventoryDetailInfo getGoodsCountsFormDataRow(DataRow r)
        {
            if (r != null)
            {
                //明细
                InventoryDetailInfo detail = new InventoryDetailInfo();
                //明细id不能为null，否则返回失败。
                if (!r.IsNull("ID"))
                {
                    detail.id = Convert.ToInt32(r["ID"]);
                }
                else
                {
                    return null;
                }

                //获取配置文件路径。
                string configFile = AppDomain.CurrentDomain.BaseDirectory.ToString() + "DataBaseConfig.xml";
                if (!r.IsNull("GOODSID"))
                {
                    GoodsOperater goodsOper = GoodsOperater.createGoodsOperater(configFile, "SQLServer");
                    detail.goods = goodsOper.getGoods(Convert.ToInt32(r["GOODSID"]));
                }
                else
                {
                    return null;
                }

                DataDicOperater dataDicOper = DataDicOperater.createDataDicOperater(configFile, "SQLServer");
                if (!r.IsNull("COLORID"))
                {
                    detail.color = dataDicOper.getDataDictionary(Convert.ToInt32(r["COLORID"]));
                }
                else
                {
                    return null;
                }

                if (!r.IsNull("SIZEID"))
                {
                    detail.size = dataDicOper.getDataDictionary(Convert.ToInt32(r["SIZEID"]));
                }
                else
                {
                    return null;
                }

                if (!r.IsNull("COUNT"))
                {
                    detail.count = Convert.ToInt32(r["COUNT"]);
                }
                else
                {
                    return null;
                }

                detail.unitPrice = 0.0;

                return detail;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 获取剩余库存。
        /// 作者：董帅 创建时间：2012-9-15 23:18:21
        /// <param name="warehouseId">仓库id</param>
        /// <returns>库存数量。</returns>
        /// </summary>
        public List<GoodsCount> getGoodsCountsByWarehouseId(int warehouseId)
        {
            List<GoodsCount> goods = null;

            try
            {
                if (this.retrnOfGoodsOperaterDataBase != null)
                {
                    //连接数据库
                    if (this.retrnOfGoodsOperaterDataBase.connectDataBase())
                    {

                        //sql语句，获取剩余库存
                        string sql = "SELECT INV_INVENTORYDETAIL.ID AS ID,INV_INVENTORYDETAIL.GOODSID,INV_INVENTORYDETAIL.COLORID,INV_INVENTORYDETAIL.SIZEID,INV_INVENTORYCOUNT.COUNT "
                                    + "FROM INV_INVENTORYMASTER,INV_INVENTORYDETAIL,INV_INVENTORYCOUNT "
                                    + "WHERE INV_INVENTORYMASTER.WAREHOUSEID = " + warehouseId.ToString() + " "
                                    + "AND INV_INVENTORYMASTER.TYPE = 1 "
                                    + "AND INV_INVENTORYMASTER.STATE = 2 "
                                    + "AND INV_INVENTORYMASTER.ID = INV_INVENTORYDETAIL.MASTERID "
                                    + "AND INV_INVENTORYDETAIL.ID = INV_INVENTORYCOUNT.DETAILID "
                                    + "AND INV_INVENTORYCOUNT.COUNT > 0";

                        //获取数据
                        DataTable dt = this.retrnOfGoodsOperaterDataBase.executeSqlReturnDt(sql);
                        if (dt != null)
                        {
                            goods = new List<GoodsCount>();

                            while (dt.Rows.Count > 0)
                            {
                                DataRow[] rows = dt.Select("GOODSID = " + dt.Rows[0]["GOODSID"].ToString());
                                if (rows.Length > 0)
                                {
                                    //货物数量
                                    GoodsCount goodCount = new GoodsCount();

                                    //获取配置文件路径。
                                    string configFile = AppDomain.CurrentDomain.BaseDirectory.ToString() + "DataBaseConfig.xml";

                                    if (!rows[0].IsNull("GOODSID"))
                                    {
                                        GoodsOperater goodsOper = GoodsOperater.createGoodsOperater(configFile, "SQLServer");
                                        goodCount.goods = goodsOper.getGoods(Convert.ToInt32(rows[0]["GOODSID"]));
                                    }
                                    else
                                    {
                                        return null;
                                    }

                                    foreach (DataRow r in rows)
                                    {

                                        InventoryDetailInfo detail = this.getGoodsCountsFormDataRow(r);
                                        if (detail != null)
                                        {
                                            goodCount.details.Add(detail);
                                        }

                                        dt.Rows.Remove(r);
                                    }

                                    goods.Add(goodCount);
                                }
                                else
                                {
                                    break;
                                }
                            }
                        }
                        else
                        {
                            this._errorMessage = "获取数据失败！错误信息：[" + this.retrnOfGoodsOperaterDataBase.errorText + "]";
                        }
                    }
                    else
                    {
                        this._errorMessage = "连接数据库失败！错误信息：[" + this.retrnOfGoodsOperaterDataBase.errorText + "]";
                    }
                }
                else
                {
                    this._errorMessage = "未设置数据库实例！";
                }
            }
            catch (Exception ex)
            {
                this._errorMessage = ex.Message;
            }
            finally
            {
                this.retrnOfGoodsOperaterDataBase.disconnectDataBase();
            }

            return goods;
        }

        /// <summary>
        /// 创建退货单明细。
        /// 作者：董帅 创建时间：2012-9-15 23:27:31
        /// </summary>
        /// <param name="detail">明细。</param>
        /// <returns>退货单明细id，失败返回-1。</returns>
        public int createNewRetrnOfGoodsDetail(PushOutInventoryDetailInfo detail)
        {
            int inventoryDetailId = -1; //创建的id。

            try
            {
                if (detail == null)
                {
                    this._errorMessage = "不能插入空明细！";
                }
                else
                {
                    //存入数据库
                    if (this.retrnOfGoodsOperaterDataBase.connectDataBase())
                    {
                        //新增数据
                        string sql = string.Format("INSERT INTO INV_INVENTORYDETAIL (MASTERID,DETAILID,GOODSID,COLORID,SIZEID,COUNT,UNITPRICE) " +
                                                     "VALUES ({0},{1},{2},{3},{4},{5},{6}) SELECT SCOPE_IDENTITY() AS id"
                                                     , detail.masterId
                                                     , detail.detailId
                                                     , detail.goods.id
                                                     , detail.color.id
                                                     , detail.size.id
                                                     , detail.count
                                                     , detail.unitPrice);

                        DataTable retDt = this.retrnOfGoodsOperaterDataBase.executeSqlReturnDt(sql);
                        if (retDt != null && retDt.Rows.Count > 0)
                        {
                            //获取新增数据id
                            inventoryDetailId = Convert.ToInt32(retDt.Rows[0]["id"]);
                        }
                        else
                        {
                            this._errorMessage = "创建失败！";
                            if (retDt == null)
                            {
                                this._errorMessage += "错误信息[" + this.retrnOfGoodsOperaterDataBase.errorText + "]";
                            }
                        }
                    }
                    else
                    {
                        this._errorMessage = "连接数据库出错！错误信息[" + this.retrnOfGoodsOperaterDataBase.errorText + "]";
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                //断开数据库连接。
                this.retrnOfGoodsOperaterDataBase.disconnectDataBase();
            }

            return inventoryDetailId;
        }

        /// <summary>
        /// 根据DataRow获取退货单明细。
        /// 作者：董帅 创建时间：2012-9-15 23:30:46
        /// </summary>
        /// <param name="r">退货单明细数据</param>
        /// <returns>退货单明细，失败返回null。</returns>
        private PushOutInventoryDetailInfo getRetrnOfGoodsDetailFormDataRow(DataRow r)
        {
            if (r != null)
            {
                //明细
                PushOutInventoryDetailInfo detail = new PushOutInventoryDetailInfo();
                //明细id不能为null，否则返回失败。
                if (!r.IsNull("ID"))
                {
                    detail.id = Convert.ToInt32(r["ID"]);
                }
                else
                {
                    return null;
                }

                if (!r.IsNull("DETAILID"))
                {
                    detail.detailId = Convert.ToInt32(r["DETAILID"]);
                }
                else
                {
                    detail.detailId = -1;
                }

                if (!r.IsNull("MASTERID"))
                {
                    detail.masterId = Convert.ToInt32(r["MASTERID"]);
                }
                else
                {
                    detail.masterId = -1;
                }
                //获取配置文件路径。
                string configFile = AppDomain.CurrentDomain.BaseDirectory.ToString() + "DataBaseConfig.xml";
                if (!r.IsNull("GOODSID"))
                {
                    GoodsOperater goodsOper = GoodsOperater.createGoodsOperater(configFile, "SQLServer");
                    detail.goods = goodsOper.getGoods(Convert.ToInt32(r["GOODSID"]));
                }
                else
                {
                    return null;
                }

                DataDicOperater dataDicOper = DataDicOperater.createDataDicOperater(configFile, "SQLServer");
                if (!r.IsNull("COLORID"))
                {
                    detail.color = dataDicOper.getDataDictionary(Convert.ToInt32(r["COLORID"]));
                }
                else
                {
                    return null;
                }

                if (!r.IsNull("SIZEID"))
                {
                    detail.size = dataDicOper.getDataDictionary(Convert.ToInt32(r["SIZEID"]));
                }
                else
                {
                    return null;
                }

                if (!r.IsNull("COUNT"))
                {
                    detail.count = Convert.ToInt32(r["COUNT"]);
                }
                else
                {
                    return null;
                }

                if (!r.IsNull("SURPLUSCOUNT"))
                {
                    detail.surplusCount = Convert.ToInt32(r["SURPLUSCOUNT"]);
                }
                else
                {
                    return null;
                }

                if (!r.IsNull("UNITPRICE"))
                {
                    detail.unitPrice = Convert.ToDouble(r["UNITPRICE"]);
                }
                else
                {
                    return null;
                }

                return detail;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 获取退货单明细。
        /// 作者：董帅 创建时间：2012-9-14 22:54:15
        /// <param name="masterId">出库单id。</param>
        /// <returns>库存数量。</returns>
        /// </summary>
        public List<PushOutGoodsCount> getRetrnOfGoodsDetailByMasterId(int masterId)
        {
            List<PushOutGoodsCount> goods = null;

            try
            {
                if (this.retrnOfGoodsOperaterDataBase != null)
                {
                    //连接数据库
                    if (this.retrnOfGoodsOperaterDataBase.connectDataBase())
                    {

                        //sql语句，获取退货单明细
                        string sql = "SELECT INV_INVENTORYDETAIL.*,INV_INVENTORYCOUNT.COUNT AS SURPLUSCOUNT "
                                   + "FROM INV_INVENTORYDETAIL,INV_INVENTORYCOUNT "
                                   + "WHERE MASTERID = " + masterId.ToString() + " AND INV_INVENTORYDETAIL.DETAILID = INV_INVENTORYCOUNT.DETAILID "
                                   + "ORDER BY GOODSID,COLORID,SIZEID";

                        //获取数据
                        DataTable dt = this.retrnOfGoodsOperaterDataBase.executeSqlReturnDt(sql);
                        if (dt != null)
                        {
                            goods = new List<PushOutGoodsCount>();

                            while (dt.Rows.Count > 0)
                            {
                                DataRow[] rows = dt.Select("GOODSID = " + dt.Rows[0]["GOODSID"].ToString());
                                if (rows.Length > 0)
                                {
                                    //货物数量
                                    PushOutGoodsCount goodCount = new PushOutGoodsCount();

                                    //获取配置文件路径。
                                    string configFile = AppDomain.CurrentDomain.BaseDirectory.ToString() + "DataBaseConfig.xml";

                                    if (!rows[0].IsNull("GOODSID"))
                                    {
                                        GoodsOperater goodsOper = GoodsOperater.createGoodsOperater(configFile, "SQLServer");
                                        goodCount.goods = goodsOper.getGoods(Convert.ToInt32(rows[0]["GOODSID"]));
                                    }
                                    else
                                    {
                                        return null;
                                    }

                                    foreach (DataRow r in rows)
                                    {

                                        PushOutInventoryDetailInfo detail = this.getRetrnOfGoodsDetailFormDataRow(r);
                                        if (detail != null)
                                        {
                                            goodCount.details.Add(detail);
                                        }

                                        dt.Rows.Remove(r);
                                    }

                                    goods.Add(goodCount);
                                }
                                else
                                {
                                    break;
                                }
                            }
                        }
                        else
                        {
                            this._errorMessage = "获取数据失败！错误信息：[" + this.retrnOfGoodsOperaterDataBase.errorText + "]";
                        }
                    }
                    else
                    {
                        this._errorMessage = "连接数据库失败！错误信息：[" + this.retrnOfGoodsOperaterDataBase.errorText + "]";
                    }
                }
                else
                {
                    this._errorMessage = "未设置数据库实例！";
                }
            }
            catch (Exception ex)
            {
                this._errorMessage = ex.Message;
            }
            finally
            {
                this.retrnOfGoodsOperaterDataBase.disconnectDataBase();
            }

            return goods;
        }

        /// <summary>
        /// 获取指定入库明细。
        /// 作者：董帅 创建时间：2012-9-15 23:34:19
        /// </summary>
        /// <param name="id">明细id。</param>
        /// <returns>明细对象。</returns>
        public PushOutInventoryDetailInfo getPutInStorageDetail(int id)
        {
            PushOutInventoryDetailInfo detail = null;

            try
            {
                if (this.retrnOfGoodsOperaterDataBase != null)
                {
                    //连接数据库
                    if (this.retrnOfGoodsOperaterDataBase.connectDataBase())
                    {

                        //sql语句，获取出库明细
                        string sql = "SELECT INV_INVENTORYDETAIL.*,INV_INVENTORYCOUNT.COUNT AS SURPLUSCOUNT "
                                    + "FROM INV_INVENTORYMASTER,INV_INVENTORYDETAIL,INV_INVENTORYCOUNT "
                                    + "WHERE INV_INVENTORYDETAIL.ID = " + id.ToString() + " "
                                    + "AND INV_INVENTORYMASTER.TYPE = 1 "
                                    + "AND INV_INVENTORYMASTER.STATE = 2 "
                                    + "AND INV_INVENTORYMASTER.ID = INV_INVENTORYDETAIL.MASTERID "
                                    + "AND INV_INVENTORYDETAIL.ID = INV_INVENTORYCOUNT.DETAILID "
                                    + "AND INV_INVENTORYCOUNT.COUNT > 0";

                        //获取数据
                        DataTable dt = this.retrnOfGoodsOperaterDataBase.executeSqlReturnDt(sql);
                        if (dt != null && dt.Rows.Count == 1)
                        {
                            detail = this.getRetrnOfGoodsDetailFormDataRow(dt.Rows[0]);
                        }
                        else
                        {
                            this._errorMessage = "获取数据失败！错误信息：[" + this.retrnOfGoodsOperaterDataBase.errorText + "]";
                        }
                    }
                    else
                    {
                        this._errorMessage = "连接数据库失败！错误信息：[" + this.retrnOfGoodsOperaterDataBase.errorText + "]";
                    }
                }
                else
                {
                    this._errorMessage = "未设置数据库实例！";
                }
            }
            catch (Exception ex)
            {
                this._errorMessage = ex.Message;
            }
            finally
            {
                this.retrnOfGoodsOperaterDataBase.disconnectDataBase();
            }

            return detail;
        }

        /// <summary>
        /// 获取退货单明细。
        /// 作者：董帅 创建时间：2012-9-15 23:37:00
        /// <param name="id">明细id。</param>
        /// <returns>明细。</returns>
        /// </summary>
        public PushOutInventoryDetailInfo getRetrnOfGoodsDetail(int id)
        {
            PushOutInventoryDetailInfo detail = null;

            try
            {
                if (this.retrnOfGoodsOperaterDataBase != null)
                {
                    //连接数据库
                    if (this.retrnOfGoodsOperaterDataBase.connectDataBase())
                    {

                        //sql语句，获取退货单明细
                        string sql = "SELECT INV_INVENTORYDETAIL.*,INV_INVENTORYCOUNT.COUNT AS SURPLUSCOUNT "
                                   + "FROM INV_INVENTORYDETAIL,INV_INVENTORYCOUNT "
                                   + "WHERE INV_INVENTORYDETAIL.ID = " + id.ToString() + " AND INV_INVENTORYDETAIL.DETAILID = INV_INVENTORYCOUNT.DETAILID";

                        //获取数据
                        DataTable dt = this.retrnOfGoodsOperaterDataBase.executeSqlReturnDt(sql);
                        if (dt != null && dt.Rows.Count == 1)
                        {
                            detail = this.getRetrnOfGoodsDetailFormDataRow(dt.Rows[0]);
                        }
                        else
                        {
                            this._errorMessage = "获取数据失败！错误信息：[" + this.retrnOfGoodsOperaterDataBase.errorText + "]";
                        }
                    }
                    else
                    {
                        this._errorMessage = "连接数据库失败！错误信息：[" + this.retrnOfGoodsOperaterDataBase.errorText + "]";
                    }
                }
                else
                {
                    this._errorMessage = "未设置数据库实例！";
                }
            }
            catch (Exception ex)
            {
                this._errorMessage = ex.Message;
            }
            finally
            {
                this.retrnOfGoodsOperaterDataBase.disconnectDataBase();
            }

            return detail;
        }

        /// <summary>
        /// 修改退货单明细。
        /// </summary>
        /// <param name="detail">明细</param>
        /// <returns>成功返回true，否是返回false。</returns>
        public bool changeRetrnOfGoodsDetail(PushOutInventoryDetailInfo detail)
        {
            bool bRet = false; //返回值

            try
            {
                if (this.retrnOfGoodsOperaterDataBase != null)
                {
                    //连接数据库
                    if (this.retrnOfGoodsOperaterDataBase.connectDataBase())
                    {
                        //sql语句
                        string sql = string.Format("UPDATE INV_INVENTORYDETAIL SET COUNT = {0},UNITPRICE = {1} WHERE ID = {2}"
                                , detail.count
                                , detail.unitPrice
                                , detail.id);

                        int retCount = this.retrnOfGoodsOperaterDataBase.executeSqlWithOutDs(sql);
                        if (retCount == 1)
                        {
                            bRet = true;
                        }
                        else
                        {
                            this._errorMessage = "更新数据失败！";
                            if (retCount != 1)
                            {
                                this._errorMessage += "错误信息[" + this.retrnOfGoodsOperaterDataBase.errorText + "]";
                            }
                        }
                    }
                    else
                    {
                        this._errorMessage = "连接数据库出错！错误信息[" + this.retrnOfGoodsOperaterDataBase.errorText + "]";
                    }

                }
                else
                {
                    this._errorMessage = "未设置数据库实例！";
                }
            }
            catch (Exception ex)
            {
                this._errorMessage = ex.Message;
            }
            finally
            {
                this.retrnOfGoodsOperaterDataBase.disconnectDataBase();
            }

            return bRet;
        }

        /// <summary>
        /// 删除退货单明细。
        /// 作者：董帅 创建时间：2012-9-15 23:53:31
        /// </summary>
        /// <param name="ids">要删除的退货单明细id。</param>
        /// <returns>成功返回true，否则返回false。</returns>
        public bool deleteRetrnOfGoodsDetail(int[] ids)
        {
            bool bRet = false; //返回值

            try
            {
                if (this.retrnOfGoodsOperaterDataBase != null)
                {
                    //连接数据库
                    if (this.retrnOfGoodsOperaterDataBase.connectDataBase())
                    {
                        //id字符串
                        string strIds = "";
                        for (int i = 0; i < ids.Length; i++)
                        {
                            if (i == 0)
                            {
                                strIds += ids[i].ToString();
                            }
                            else
                            {
                                strIds += "," + ids[i].ToString();
                            }
                        }
                        //sql语句
                        string sql = string.Format("DELETE INV_INVENTORYDETAIL WHERE ID IN ({0})", strIds);

                        int retCount = this.retrnOfGoodsOperaterDataBase.executeSqlWithOutDs(sql);
                        if (retCount > 0)
                        {
                            bRet = true;
                        }
                        else
                        {
                            this._errorMessage = "删除数据失败！";
                            this._errorMessage += "错误信息[" + this.retrnOfGoodsOperaterDataBase.errorText + "]";
                        }
                    }
                    else
                    {
                        this._errorMessage = "连接数据库出错！错误信息[" + this.retrnOfGoodsOperaterDataBase.errorText + "]";
                    }
                }
                else
                {
                    this._errorMessage = "未设置数据库实例！";
                }
            }
            catch (Exception ex)
            {
                this._errorMessage = ex.Message;
            }
            finally
            {
                this.retrnOfGoodsOperaterDataBase.disconnectDataBase();
            }

            return bRet;
        }

        /// <summary>
        /// 执行退货单。
        /// 作者：董帅 创建时间：2012-9-15 23:54:59
        /// </summary>
        /// <param name="id">退货单id</param>
        /// <param name="user">用户</param>
        /// <returns>成功返回true，失败返回false,</returns>
        public bool executeRetrnOfGoods(int id, UserInfo user)
        {
            bool bRet = false;

            try
            {
                if (this.retrnOfGoodsOperaterDataBase != null)
                {
                    //连接数据库
                    if (this.retrnOfGoodsOperaterDataBase.connectDataBase())
                    {
                        //开启事务，并且设置隔离级别
                        this.retrnOfGoodsOperaterDataBase.beginTransaction(IsolationLevel.RepeatableRead);
                        //修改入库单状态
                        string sql = string.Format("UPDATE INV_INVENTORYMASTER SET STATE = 2 WHERE ID ={0} AND STATE = 1", id);

                        int retCount = this.retrnOfGoodsOperaterDataBase.executeSqlWithOutDs(sql);
                        if (retCount == 1)
                        {
                            //明细
                            sql = "SELECT * FROM INV_INVENTORYDETAIL WHERE MASTERID = " + id.ToString() + " ORDER BY GOODSID,COLORID,SIZEID";

                            //获取数据
                            DataTable dt = this.retrnOfGoodsOperaterDataBase.executeSqlReturnDt(sql);
                            if (dt != null)
                            {
                                int i = 0;
                                foreach (DataRow row in dt.Rows)
                                {
                                    sql = string.Format("UPDATE INV_INVENTORYCOUNT SET COUNT = COUNT - {0} WHERE DETAILID = {1} AND COUNT - {0} >= 0", row["COUNT"], row["DETAILID"]);
                                    retCount = this.retrnOfGoodsOperaterDataBase.executeSqlWithOutDs(sql);
                                    if (retCount == 1)
                                    {
                                        i++;
                                    }
                                    else
                                    {
                                        break;
                                    }
                                }

                                //成功，设置执行人
                                if (i == dt.Rows.Count)
                                {
                                    sql = string.Format("UPDATE INV_INVENTORYMASTER SET EXECUTETIME = '{0}',EXECUTEUSER = {1} WHERE ID ={2}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), user.id, id);
                                    retCount = this.retrnOfGoodsOperaterDataBase.executeSqlWithOutDs(sql);
                                    if (retCount == 1)
                                    {
                                        bRet = true;
                                    }
                                    else
                                    {
                                        this._errorMessage = "更新执行人和执行时间失败！";
                                        this._errorMessage += "错误信息[" + this.retrnOfGoodsOperaterDataBase.errorText + "]";
                                    }
                                }
                            }
                            else
                            {
                                this._errorMessage = "更新库存失败！";
                                this._errorMessage += "错误信息[" + this.retrnOfGoodsOperaterDataBase.errorText + "]";
                            }
                        }
                        else
                        {
                            this._errorMessage = "更新入库单状态失败！";
                            this._errorMessage += "错误信息[" + this.retrnOfGoodsOperaterDataBase.errorText + "]";
                        }
                    }
                    else
                    {
                        this._errorMessage = "连接数据库出错！错误信息[" + this.retrnOfGoodsOperaterDataBase.errorText + "]";
                    }
                }
                else
                {
                    this._errorMessage = "未设置数据库实例！";
                }
            }
            catch (Exception ex)
            {
                this._errorMessage = ex.Message;
            }
            finally
            {
                //提交事务
                if (bRet)
                {
                    this.retrnOfGoodsOperaterDataBase.commitTransaction();
                }
                else
                {
                    this.retrnOfGoodsOperaterDataBase.rollbackTransaction();
                }
                this.retrnOfGoodsOperaterDataBase.disconnectDataBase();
            }

            return bRet;
        }
    }
}
