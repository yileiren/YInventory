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
using YLR.YInventory.Inventory.RetrnToStorage;

namespace YLR.YInventory.Inventory.retrnToStorage
{
    /// <summary>
    /// 货物退库数据库处理类。
    /// 作者：董帅 创建时间：2012-9-16 0:10:20
    /// </summary>
    public class RetrnToStorageOperater
    {
        /// <summary>
        /// 退库单数据库。
        /// </summary>
        public YDataBase retrnToStorageOperaterDataBase
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
        /// 创建退库单数据库操作对象。
        /// 作者：董帅 创建时间：2012-9-6 21:57:48
        /// </summary>
        /// <param name="configFilePath">配置文件路径。</param>
        /// <param name="nodeName">节点名。</param>
        /// <returns>成功返回操作对象，否则返回null。</returns>
        public static RetrnToStorageOperater createRetrnToStorageOperater(string configFilePath, string nodeName)
        {
            RetrnToStorageOperater oper = null;

            //获取数据库实例。
            YDataBase orgDb = YDataBaseConfigFile.createDataBase(configFilePath, nodeName);

            if (orgDb != null)
            {
                oper = new RetrnToStorageOperater();
                oper.retrnToStorageOperaterDataBase = orgDb;
            }
            else
            {
                Exception ex = new Exception("创建数据库实例失败！");
                throw ex;
            }

            return oper;
        }

        /// <summary>
        /// 创建退库单。
        /// 作者：董帅 创建时间：2012-9-15 22:11:54
        /// </summary>
        /// <param name="inventoryMaster">退库单。</param>
        /// <returns>退库单id，失败返回-1。</returns>
        public int createNewRetrnToStorage(InventoryMasterInfo inventoryMaster)
        {
            int inventoryMasterId = -1; //创建id。

            try
            {
                if (inventoryMaster == null)
                {
                    this._errorMessage = "不能插入空退库单！";
                }
                else
                {
                    //存入数据库
                    if (this.retrnToStorageOperaterDataBase.connectDataBase())
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

                        DataTable retDt = this.retrnToStorageOperaterDataBase.executeSqlReturnDt(sql);
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
                                this._errorMessage += "错误信息[" + this.retrnToStorageOperaterDataBase.errorText + "]";
                            }
                        }
                    }
                    else
                    {
                        this._errorMessage = "连接数据库出错！错误信息[" + this.retrnToStorageOperaterDataBase.errorText + "]";
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
                this.retrnToStorageOperaterDataBase.disconnectDataBase();
            }

            return inventoryMasterId;
        }

        /// <summary>
        /// 根据DataRow获取退库单。
        /// 作者：董帅 创建时间：2012-9-15 22:21:03
        /// </summary>
        /// <param name="r">退库单数据</param>
        /// <returns>退库单，失败返回null。</returns>
        private InventoryMasterInfo getRetrnToStorageFormDataRow(DataRow r)
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
        /// 获取退库单。
        /// 作者：董帅 创建时间：2012-9-15 22:20:57
        /// </summary>
        /// <param name="id">退库单id</param>
        /// <returns>成功返回退库单，否则返回null。</returns>
        public InventoryMasterInfo getRetrnToStorage(int id)
        {
            InventoryMasterInfo inv = null;

            try
            {
                if (this.retrnToStorageOperaterDataBase != null)
                {
                    //连接数据库
                    if (this.retrnToStorageOperaterDataBase.connectDataBase())
                    {

                        //sql语句，获取退库单
                        string sql = "SELECT * FROM INV_INVENTORYMASTER WHERE ID = " + id.ToString();

                        //获取数据
                        DataTable dt = this.retrnToStorageOperaterDataBase.executeSqlReturnDt(sql);
                        if (dt != null && dt.Rows.Count == 1)
                        {
                            inv = this.getRetrnToStorageFormDataRow(dt.Rows[0]);
                        }
                        else
                        {
                            this._errorMessage = "获取数据失败！错误信息：[" + this.retrnToStorageOperaterDataBase.errorText + "]";
                        }
                    }
                    else
                    {
                        this._errorMessage = "连接数据库失败！错误信息：[" + this.retrnToStorageOperaterDataBase.errorText + "]";
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
                this.retrnToStorageOperaterDataBase.disconnectDataBase();
            }

            return inv;
        }

        /// <summary>
        /// 获取退库单。
        /// 作者：董帅 创建时间：2012-9-15 22:20:43
        /// </summary>
        /// <param name="number">退库单编号,为null获空字符串表示不指定。</param>
        /// <param name="supplierId">供应商id，等于0表示不指定。</param>
        /// <param name="warehouseId">仓库id，等于0表示不指定。</param>
        /// <param name="states">退库单状态，等于0表示不指定。</param>
        /// <param name="createBegin">创建开始时间，为null获空字符串表示不指定。</param>
        /// <param name="createEnd">创建截止时间,为null获空字符串表示不指定。</param>
        /// <returns>成功返回退库单列表，否则返回null。</returns>
        public List<InventoryMasterInfo> getRetrnToStorage(string number, int supplierId, int warehouseId, int states, string createBegin, string createEnd)
        {
            List<InventoryMasterInfo> invs = null;

            try
            {
                if (this.retrnToStorageOperaterDataBase != null)
                {
                    //连接数据库
                    if (this.retrnToStorageOperaterDataBase.connectDataBase())
                    {

                        //sql语句，获取退库单
                        string sql = "SELECT * FROM INV_INVENTORYMASTER WHERE TYPE = 4";

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
                        DataTable dt = this.retrnToStorageOperaterDataBase.executeSqlReturnDt(sql);
                        if (dt != null)
                        {
                            invs = new List<InventoryMasterInfo>();
                            foreach (DataRow r in dt.Rows)
                            {
                                InventoryMasterInfo inv = this.getRetrnToStorageFormDataRow(r);
                                if (inv != null)
                                {
                                    invs.Add(inv);
                                }
                            }
                        }
                        else
                        {
                            this._errorMessage = "获取数据失败！错误信息：[" + this.retrnToStorageOperaterDataBase.errorText + "]";
                        }
                    }
                    else
                    {
                        this._errorMessage = "连接数据库失败！错误信息：[" + this.retrnToStorageOperaterDataBase.errorText + "]";
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
                this.retrnToStorageOperaterDataBase.disconnectDataBase();
            }

            return invs;
        }

        /// <summary>
        /// 修改退库单信息。
        /// 作者：董帅 创建时间：2012-9-15 22:21:29
        /// </summary>
        /// <param name="id">要修改的退库单id。</param>
        /// <param name="number">退库单编号。</param>
        /// <param name="supplierId">供应商id。</param>
        /// <param name="warehousId">仓库id。</param>
        /// <returns>成功返回true，否则返回false。</returns>
        public bool changRetrnToStorage(int id, string number, int supplierId, int warehousId)
        {
            bool bRet = false; //返回值

            try
            {
                if (this.retrnToStorageOperaterDataBase != null)
                {
                    //连接数据库
                    if (this.retrnToStorageOperaterDataBase.connectDataBase())
                    {
                        //sql语句
                        string sql = string.Format("UPDATE INV_INVENTORYMASTER SET NUMBER = '{0}',SUPPLIERORCLIENTID = {1},WAREHOUSEID = {2} WHERE ID = {3}"
                                , number
                                , supplierId
                                , warehousId
                                , id);

                        int retCount = this.retrnToStorageOperaterDataBase.executeSqlWithOutDs(sql);
                        if (retCount == 1)
                        {
                            bRet = true;
                        }
                        else
                        {
                            this._errorMessage = "更新数据失败！";
                            if (retCount != 1)
                            {
                                this._errorMessage += "错误信息[" + this.retrnToStorageOperaterDataBase.errorText + "]";
                            }
                        }
                    }
                    else
                    {
                        this._errorMessage = "连接数据库出错！错误信息[" + this.retrnToStorageOperaterDataBase.errorText + "]";
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
                this.retrnToStorageOperaterDataBase.disconnectDataBase();
            }

            return bRet;
        }

        /// <summary>
        /// 删除退库单。
        /// 作者：董帅 创建时间：2012-9-15 22:28:42
        /// </summary>
        /// <param name="ids">要删除的退库单id。</param>
        /// <returns>成功返回true，否则返回false。</returns>
        public bool deleteRetrnToStorage(int[] ids)
        {
            bool bRet = false; //返回值

            try
            {
                if (this.retrnToStorageOperaterDataBase != null)
                {
                    //连接数据库
                    if (this.retrnToStorageOperaterDataBase.connectDataBase())
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

                        int retCount = this.retrnToStorageOperaterDataBase.executeSqlWithOutDs(sql);
                        if (retCount > 0)
                        {
                            bRet = true;
                        }
                        else
                        {
                            this._errorMessage = "删除数据失败！";
                            this._errorMessage += "错误信息[" + this.retrnToStorageOperaterDataBase.errorText + "]";
                        }
                    }
                    else
                    {
                        this._errorMessage = "连接数据库出错！错误信息[" + this.retrnToStorageOperaterDataBase.errorText + "]";
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
                this.retrnToStorageOperaterDataBase.disconnectDataBase();
            }

            return bRet;
        }

        /// <summary>
        /// 根据DataRow获取出货明细。
        /// 作者：董帅 创建时间：2012-9-16 1:10:36
        /// </summary>
        /// <param name="r">出货明细明细数据</param>
        /// <returns>出货明细，失败返回null。</returns>
        private RetrnToStorageInventoryDetailInfo getPushOutStorageDetailFormDataRow(DataRow r)
        {
            if (r != null)
            {
                //明细
                RetrnToStorageInventoryDetailInfo detail = new RetrnToStorageInventoryDetailInfo();
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
                    detail.surplusCount = Convert.ToInt32(r["COUNT"]);
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

                OrgOperater orgOper = OrgOperater.createOrgOperater(configFile, "SQLServer");

                if (!r.IsNull("EXECUTETIME"))
                {
                    detail.executeTime = Convert.ToDateTime(r["EXECUTETIME"]);
                }
                else
                {
                    detail.executeTime = null;
                }

                if (!r.IsNull("EXECUTEUSER"))
                {
                    detail.executeUser = orgOper.getUser(Convert.ToInt32(r["EXECUTEUSER"]));
                }
                else
                {
                    detail.executeUser = null;
                }

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
        /// <param name="beginTime">执行开始时间</param>
        /// <param name="endTime">执行结束时间</param>
        /// <returns>库存数量。</returns>
        /// </summary>
        public List<RetrnToStorageInventoryDetailInfo> getGoodsCountsByWarehouseId(int warehouseId, string beginTime, string endTime)
        {
            List<RetrnToStorageInventoryDetailInfo> goods = null;

            try
            {
                if (this.retrnToStorageOperaterDataBase != null)
                {
                    //连接数据库
                    if (this.retrnToStorageOperaterDataBase.connectDataBase())
                    {

                        //sql语句，获取剩余库存
                        string sql = "SELECT INV_INVENTORYDETAIL.ID AS ID,INV_INVENTORYDETAIL.GOODSID,INV_INVENTORYDETAIL.COLORID,INV_INVENTORYDETAIL.SIZEID,INV_INVENTORYDETAIL.COUNT,INV_INVENTORYMASTER.EXECUTETIME,INV_INVENTORYMASTER.EXECUTEUSER,INV_INVENTORYDETAIL.UNITPRICE "
                                    + "FROM INV_INVENTORYMASTER,INV_INVENTORYDETAIL "
                                    + "WHERE INV_INVENTORYMASTER.WAREHOUSEID = " + warehouseId.ToString() + " "
                                    + "AND INV_INVENTORYMASTER.TYPE = 2 "
                                    + "AND INV_INVENTORYMASTER.STATE = 2 "
                                    + "AND INV_INVENTORYMASTER.ID = INV_INVENTORYDETAIL.MASTERID ";

                        if (!string.IsNullOrEmpty(endTime))
                        {
                            sql += " AND INV_INVENTORYMASTER.EXECUTETIME > '" + beginTime +"'";
                        }

                        if (!string.IsNullOrEmpty(beginTime))
                        {
                            sql += " AND INV_INVENTORYMASTER.EXECUTETIME < '" + endTime + "'";
                        }

                        sql += " ORDER BY INV_INVENTORYMASTER.EXECUTETIME DESC";

                        //获取数据
                        DataTable dt = this.retrnToStorageOperaterDataBase.executeSqlReturnDt(sql);
                        if (dt != null)
                        {
                            goods = new List<RetrnToStorageInventoryDetailInfo>();

                            foreach (DataRow r in dt.Rows)
                            {

                                RetrnToStorageInventoryDetailInfo detail = this.getPushOutStorageDetailFormDataRow(r);
                                if (detail != null)
                                {
                                    //获取退库数量
                                    sql = "SELECT SUM(COUNT) AS COUNT FROM INV_INVENTORYDETAIL WHERE DETAILID = " + detail.id.ToString();
                                    DataTable dtCount = this.retrnToStorageOperaterDataBase.executeSqlReturnDt(sql);
                                    if (dtCount != null & dtCount.Rows.Count > 0)
                                    {
                                        if (!dtCount.Rows[0].IsNull("COUNT"))
                                        {
                                            detail.surplusCount -= Convert.ToInt32(dtCount.Rows[0]["COUNT"]);
                                        }
                                    }

                                    if (detail.surplusCount > 0)
                                    {
                                        goods.Add(detail);
                                    }
                                }
                            }

                        }
                        else
                        {
                            this._errorMessage = "获取数据失败！错误信息：[" + this.retrnToStorageOperaterDataBase.errorText + "]";
                        }
                    }
                    else
                    {
                        this._errorMessage = "连接数据库失败！错误信息：[" + this.retrnToStorageOperaterDataBase.errorText + "]";
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
                this.retrnToStorageOperaterDataBase.disconnectDataBase();
            }

            return goods;
        }

        /// <summary>
        /// 获取指定出库明细。
        /// 作者：董帅 创建时间：2012-9-16 2:29:06
        /// </summary>
        /// <param name="id">明细id。</param>
        /// <returns>明细对象。</returns>
        public RetrnToStorageInventoryDetailInfo getPushOutStorageDetail(int id)
        {
            RetrnToStorageInventoryDetailInfo detail = null;

            try
            {
                if (this.retrnToStorageOperaterDataBase != null)
                {
                    //连接数据库
                    if (this.retrnToStorageOperaterDataBase.connectDataBase())
                    {

                        //sql语句，获取出库明细
                        string sql = "SELECT INV_INVENTORYDETAIL.ID AS ID,INV_INVENTORYDETAIL.GOODSID,INV_INVENTORYDETAIL.COLORID,INV_INVENTORYDETAIL.SIZEID,INV_INVENTORYDETAIL.COUNT,INV_INVENTORYMASTER.EXECUTETIME,INV_INVENTORYMASTER.EXECUTEUSER,INV_INVENTORYDETAIL.UNITPRICE "
                                    + "FROM INV_INVENTORYMASTER,INV_INVENTORYDETAIL "
                                    + "WHERE INV_INVENTORYDETAIL.ID = " + id.ToString() + " "
                                    + "AND INV_INVENTORYMASTER.TYPE = 2 "
                                    + "AND INV_INVENTORYMASTER.STATE = 2 "
                                    + "AND INV_INVENTORYMASTER.ID = INV_INVENTORYDETAIL.MASTERID ";

                        //获取数据
                        DataTable dt = this.retrnToStorageOperaterDataBase.executeSqlReturnDt(sql);
                        if (dt != null && dt.Rows.Count == 1)
                        {
                            detail = this.getPushOutStorageDetailFormDataRow(dt.Rows[0]);

                            if (detail != null)
                            {
                                //获取退库数量
                                sql = "SELECT SUM(COUNT) AS COUNT FROM INV_INVENTORYDETAIL WHERE DETAILID = " + detail.id.ToString();
                                DataTable dtCount = this.retrnToStorageOperaterDataBase.executeSqlReturnDt(sql);
                                if (dtCount != null & dtCount.Rows.Count > 0)
                                {
                                    if (!dtCount.Rows[0].IsNull("COUNT"))
                                    {
                                        detail.surplusCount -= Convert.ToInt32(dtCount.Rows[0]["COUNT"]);
                                    }
                                }

                                if (detail.surplusCount <= 0)
                                {
                                    detail = null;
                                }
                            }
                        }
                        else
                        {
                            this._errorMessage = "获取数据失败！错误信息：[" + this.retrnToStorageOperaterDataBase.errorText + "]";
                        }
                    }
                    else
                    {
                        this._errorMessage = "连接数据库失败！错误信息：[" + this.retrnToStorageOperaterDataBase.errorText + "]";
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
                this.retrnToStorageOperaterDataBase.disconnectDataBase();
            }

            return detail;
        }

        /// <summary>
        /// 创建退库单明细。
        /// 作者：董帅 创建时间：2012-9-16 2:59:28
        /// </summary>
        /// <param name="detail">明细。</param>
        /// <returns>退货单明细id，失败返回-1。</returns>
        public int createNewRetrnToStorageDetail(RetrnToStorageInventoryDetailInfo detail)
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
                    if (this.retrnToStorageOperaterDataBase.connectDataBase())
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

                        DataTable retDt = this.retrnToStorageOperaterDataBase.executeSqlReturnDt(sql);
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
                                this._errorMessage += "错误信息[" + this.retrnToStorageOperaterDataBase.errorText + "]";
                            }
                        }
                    }
                    else
                    {
                        this._errorMessage = "连接数据库出错！错误信息[" + this.retrnToStorageOperaterDataBase.errorText + "]";
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
                this.retrnToStorageOperaterDataBase.disconnectDataBase();
            }

            return inventoryDetailId;
        }

        /// <summary>
        /// 根据DataRow获取退库单明细。
        /// 作者：董帅 创建时间：2012-9-16 3:09:34
        /// </summary>
        /// <param name="r">图库单明细数据</param>
        /// <returns>退库单明细，失败返回null。</returns>
        private RetrnToStorageInventoryDetailInfo getRetrnToStorageDetailFormDataRow(DataRow r)
        {
            if (r != null)
            {
                //明细
                RetrnToStorageInventoryDetailInfo detail = new RetrnToStorageInventoryDetailInfo();
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
        /// 获取退库单明细。
        /// 作者：董帅 创建时间：2012-9-16 3:11:53
        /// <param name="masterId">退库单id。</param>
        /// <returns>明细。</returns>
        /// </summary>
        public List<RetrnToStorageInventoryDetailInfo> getRetrnToStorageDetailByMasterId(int masterId)
        {
            List<RetrnToStorageInventoryDetailInfo> goods = null;

            try
            {
                if (this.retrnToStorageOperaterDataBase != null)
                {
                    //连接数据库
                    if (this.retrnToStorageOperaterDataBase.connectDataBase())
                    {

                        //sql语句，获取退货单明细
                        string sql = "SELECT * "
                                   + "FROM INV_INVENTORYDETAIL "
                                   + "WHERE MASTERID = " + masterId.ToString() + " "
                                   + "ORDER BY GOODSID,COLORID,SIZEID";

                        //获取数据
                        DataTable dt = this.retrnToStorageOperaterDataBase.executeSqlReturnDt(sql);
                        if (dt != null)
                        {
                            goods = new List<RetrnToStorageInventoryDetailInfo>();


                            foreach (DataRow r in dt.Rows)
                            {

                                RetrnToStorageInventoryDetailInfo detail = this.getRetrnToStorageDetailFormDataRow(r);

                                if (detail != null)
                                {
                                    //获取退库数量
                                    sql = "SELECT * FROM INV_INVENTORYDETAIL WHERE ID = " + detail.detailId.ToString();
                                    DataTable dtCount = this.retrnToStorageOperaterDataBase.executeSqlReturnDt(sql);
                                    if (dtCount != null & dtCount.Rows.Count > 0)
                                    {
                                        if (!dtCount.Rows[0].IsNull("COUNT"))
                                        {
                                            sql = "SELECT SUM(COUNT) AS COUNT FROM INV_INVENTORYDETAIL WHERE DETAILID = " + detail.detailId.ToString() + " AND ID <> " + detail.id.ToString();
                                            DataTable dtReturn = this.retrnToStorageOperaterDataBase.executeSqlReturnDt(sql);

                                            if (dtReturn != null & dtReturn.Rows.Count > 0 & !dtReturn.Rows[0].IsNull("COUNT"))
                                            {
                                                detail.surplusCount = Convert.ToInt32(dtCount.Rows[0]["COUNT"]) - Convert.ToInt32(dtReturn.Rows[0]["COUNT"]);
                                            }
                                            else
                                            {
                                                detail.surplusCount = Convert.ToInt32(dtCount.Rows[0]["COUNT"]);
                                            }
                                            goods.Add(detail);
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            this._errorMessage = "获取数据失败！错误信息：[" + this.retrnToStorageOperaterDataBase.errorText + "]";
                        }
                    }
                    else
                    {
                        this._errorMessage = "连接数据库失败！错误信息：[" + this.retrnToStorageOperaterDataBase.errorText + "]";
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
                this.retrnToStorageOperaterDataBase.disconnectDataBase();
            }

            return goods;
        }

        /// <summary>
        /// 获取退货单明细。
        /// 作者：董帅 创建时间：2012-9-15 23:37:00
        /// <param name="id">明细id。</param>
        /// <returns>明细。</returns>
        /// </summary>
        public RetrnToStorageInventoryDetailInfo getRetrnToStorageDetail(int id)
        {
            RetrnToStorageInventoryDetailInfo detail = null;

            try
            {
                if (this.retrnToStorageOperaterDataBase != null)
                {
                    //连接数据库
                    if (this.retrnToStorageOperaterDataBase.connectDataBase())
                    {

                        //sql语句，获取退货单明细
                        string sql = "SELECT * "
                                   + "FROM INV_INVENTORYDETAIL "
                                   + "WHERE ID = " + id.ToString();

                        //获取数据
                        DataTable dt = this.retrnToStorageOperaterDataBase.executeSqlReturnDt(sql);
                        if (dt != null && dt.Rows.Count == 1)
                        {
                            detail = this.getRetrnToStorageDetailFormDataRow(dt.Rows[0]);

                            if (detail != null)
                            {
                                //获取退库数量
                                sql = "SELECT * FROM INV_INVENTORYDETAIL WHERE ID = " + detail.detailId.ToString();
                                DataTable dtCount = this.retrnToStorageOperaterDataBase.executeSqlReturnDt(sql);
                                if (dtCount != null & dtCount.Rows.Count > 0)
                                {
                                    if (!dtCount.Rows[0].IsNull("COUNT"))
                                    {
                                        sql = "SELECT SUM(COUNT) AS COUNT FROM INV_INVENTORYDETAIL WHERE DETAILID = " + detail.detailId.ToString() + "AND ID <> " +detail.id.ToString();
                                        DataTable dtReturn = this.retrnToStorageOperaterDataBase.executeSqlReturnDt(sql);

                                        if (dtReturn != null & dtReturn.Rows.Count > 0 & !dtReturn.Rows[0].IsNull("COUNT"))
                                        {
                                            detail.surplusCount = Convert.ToInt32(dtCount.Rows[0]["COUNT"]) - Convert.ToInt32(dtReturn.Rows[0]["COUNT"]);
                                        }
                                        else
                                        {
                                            detail.surplusCount = Convert.ToInt32(dtCount.Rows[0]["COUNT"]);
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            this._errorMessage = "获取数据失败！错误信息：[" + this.retrnToStorageOperaterDataBase.errorText + "]";
                        }
                    }
                    else
                    {
                        this._errorMessage = "连接数据库失败！错误信息：[" + this.retrnToStorageOperaterDataBase.errorText + "]";
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
                this.retrnToStorageOperaterDataBase.disconnectDataBase();
            }

            return detail;
        }

        /// <summary>
        /// 修改退货单明细。
        /// </summary>
        /// <param name="detail">明细</param>
        /// <returns>成功返回true，否是返回false。</returns>
        public bool changeRetrnToStorageDetail(RetrnToStorageInventoryDetailInfo detail)
        {
            bool bRet = false; //返回值

            try
            {
                if (this.retrnToStorageOperaterDataBase != null)
                {
                    //连接数据库
                    if (this.retrnToStorageOperaterDataBase.connectDataBase())
                    {
                        //sql语句
                        string sql = string.Format("UPDATE INV_INVENTORYDETAIL SET COUNT = {0} WHERE ID = {1}"
                                , detail.count
                                , detail.id);

                        int retCount = this.retrnToStorageOperaterDataBase.executeSqlWithOutDs(sql);
                        if (retCount == 1)
                        {
                            bRet = true;
                        }
                        else
                        {
                            this._errorMessage = "更新数据失败！";
                            if (retCount != 1)
                            {
                                this._errorMessage += "错误信息[" + this.retrnToStorageOperaterDataBase.errorText + "]";
                            }
                        }
                    }
                    else
                    {
                        this._errorMessage = "连接数据库出错！错误信息[" + this.retrnToStorageOperaterDataBase.errorText + "]";
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
                this.retrnToStorageOperaterDataBase.disconnectDataBase();
            }

            return bRet;
        }

        /// <summary>
        /// 删除退货单明细。
        /// 作者：董帅 创建时间：2012-9-15 23:53:31
        /// </summary>
        /// <param name="ids">要删除的退货单明细id。</param>
        /// <returns>成功返回true，否则返回false。</returns>
        public bool deleteRetrnToStorageDetail(int[] ids)
        {
            bool bRet = false; //返回值

            try
            {
                if (this.retrnToStorageOperaterDataBase != null)
                {
                    //连接数据库
                    if (this.retrnToStorageOperaterDataBase.connectDataBase())
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

                        int retCount = this.retrnToStorageOperaterDataBase.executeSqlWithOutDs(sql);
                        if (retCount > 0)
                        {
                            bRet = true;
                        }
                        else
                        {
                            this._errorMessage = "删除数据失败！";
                            this._errorMessage += "错误信息[" + this.retrnToStorageOperaterDataBase.errorText + "]";
                        }
                    }
                    else
                    {
                        this._errorMessage = "连接数据库出错！错误信息[" + this.retrnToStorageOperaterDataBase.errorText + "]";
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
                this.retrnToStorageOperaterDataBase.disconnectDataBase();
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
        public bool executeRetrnToStorage(int id, UserInfo user)
        {
            bool bRet = false;

            try
            {
                if (this.retrnToStorageOperaterDataBase != null)
                {
                    //连接数据库
                    if (this.retrnToStorageOperaterDataBase.connectDataBase())
                    {
                        //开启事务，并且设置隔离级别
                        this.retrnToStorageOperaterDataBase.beginTransaction(IsolationLevel.RepeatableRead);
                        //修改入库单状态
                        string sql = string.Format("UPDATE INV_INVENTORYMASTER SET STATE = 2 WHERE ID ={0} AND STATE = 1", id);

                        int retCount = this.retrnToStorageOperaterDataBase.executeSqlWithOutDs(sql);
                        if (retCount == 1)
                        {
                            //明细
                            sql = "SELECT * FROM INV_INVENTORYDETAIL WHERE MASTERID = " + id.ToString() + " ORDER BY GOODSID,COLORID,SIZEID";

                            //获取数据
                            DataTable dt = this.retrnToStorageOperaterDataBase.executeSqlReturnDt(sql);
                            if (dt != null)
                            {
                                int i = 0;
                                foreach (DataRow row in dt.Rows)
                                {
                                    //判断出库数量是否小于退库数量
                                    sql = "SELECT * FROM INV_INVENTORYDETAIL WHERE ID = " + row["DETAILID"].ToString();
                                    DataTable dtCount = this.retrnToStorageOperaterDataBase.executeSqlReturnDt(sql);
                                    if (dtCount != null & dtCount.Rows.Count > 0)
                                    {
                                        if (!dtCount.Rows[0].IsNull("COUNT"))
                                        {
                                            sql = "SELECT SUM(COUNT) AS COUNT FROM INV_INVENTORYDETAIL WHERE DETAILID = " + row["DETAILID"].ToString() + "AND ID <> " + row["ID"].ToString();
                                            DataTable dtReturn = this.retrnToStorageOperaterDataBase.executeSqlReturnDt(sql);

                                            if (dtReturn != null & dtReturn.Rows.Count > 0 & !dtReturn.Rows[0].IsNull("COUNT"))
                                            {
                                                if (Convert.ToInt32(dtCount.Rows[0]["COUNT"]) - Convert.ToInt32(dtReturn.Rows[0]["COUNT"]) - Convert.ToInt32(row["COUNT"]) < 0)
                                                {
                                                    break;
                                                }
                                            }
                                            else
                                            {
                                                if (Convert.ToInt32(dtCount.Rows[0]["COUNT"]) - Convert.ToInt32(row["COUNT"]) < 0)
                                                {
                                                    break;
                                                }
                                            }

                                            //更新库存
                                            sql = string.Format("UPDATE INV_INVENTORYCOUNT SET COUNT = COUNT + {0} WHERE DETAILID = {1}", row["COUNT"], dtCount.Rows[0]["DETAILID"]);
                                            retCount = this.retrnToStorageOperaterDataBase.executeSqlWithOutDs(sql);
                                            if (retCount == 1)
                                            {
                                                i++;
                                            }
                                            else
                                            {
                                                break;
                                            }
                                        }
                                    }
                                }

                                //成功，设置执行人
                                if (i == dt.Rows.Count)
                                {
                                    sql = string.Format("UPDATE INV_INVENTORYMASTER SET EXECUTETIME = '{0}',EXECUTEUSER = {1} WHERE ID ={2}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), user.id, id);
                                    retCount = this.retrnToStorageOperaterDataBase.executeSqlWithOutDs(sql);
                                    if (retCount == 1)
                                    {
                                        bRet = true;
                                    }
                                    else
                                    {
                                        this._errorMessage = "更新执行人和执行时间失败！";
                                        this._errorMessage += "错误信息[" + this.retrnToStorageOperaterDataBase.errorText + "]";
                                    }
                                }
                            }
                            else
                            {
                                this._errorMessage = "更新库存失败！";
                                this._errorMessage += "错误信息[" + this.retrnToStorageOperaterDataBase.errorText + "]";
                            }
                        }
                        else
                        {
                            this._errorMessage = "更新入库单状态失败！";
                            this._errorMessage += "错误信息[" + this.retrnToStorageOperaterDataBase.errorText + "]";
                        }
                    }
                    else
                    {
                        this._errorMessage = "连接数据库出错！错误信息[" + this.retrnToStorageOperaterDataBase.errorText + "]";
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
                    this.retrnToStorageOperaterDataBase.commitTransaction();
                }
                else
                {
                    this.retrnToStorageOperaterDataBase.rollbackTransaction();
                }
                this.retrnToStorageOperaterDataBase.disconnectDataBase();
            }

            return bRet;
        }
    }
}
