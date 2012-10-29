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

namespace YLR.YInventory.Inventory.PutInStorage
{
    /// <summary>
    /// 入库单业务类。
    /// 作者：董帅 创建时间：2012年9月6日21:56:50
    /// </summary>
    public class PutInStorageOperater
    {
        /// <summary>
        /// 入库单数据库。
        /// </summary>
        public YDataBase putInStorageOperaterDataBase
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
        /// 创建入库单数据库操作对象。
        /// 作者：董帅 创建时间：2012-9-6 21:57:48
        /// </summary>
        /// <param name="configFilePath">配置文件路径。</param>
        /// <param name="nodeName">节点名。</param>
        /// <returns>成功返回操作对象，否则返回null。</returns>
        public static PutInStorageOperater createPutInStorageOperater(string configFilePath, string nodeName)
        {
            PutInStorageOperater putInStorageOper = null;

            //获取数据库实例。
            YDataBase orgDb = YDataBaseConfigFile.createDataBase(configFilePath, nodeName);

            if (orgDb != null)
            {
                putInStorageOper = new PutInStorageOperater();
                putInStorageOper.putInStorageOperaterDataBase = orgDb;
            }
            else
            {
                Exception ex = new Exception("创建数据库实例失败！");
                throw ex;
            }

            return putInStorageOper;
        }

        /// <summary>
        /// 创建入库单。
        /// 作者：董帅 创建时间：2012-9-6 22:01:56
        /// </summary>
        /// <param name="inventoryMaster">入库单。</param>
        /// <returns>入库单id，失败返回-1。</returns>
        public int createNewPutInStorage(InventoryMasterInfo inventoryMaster)
        {
            int inventoryMasterId = -1; //创建的仓库id。

            try
            {
                if (inventoryMaster == null)
                {
                    this._errorMessage = "不能插入空入库单！";
                }
                else
                {
                    //存入数据库
                    if (this.putInStorageOperaterDataBase.connectDataBase())
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

                        DataTable retDt = this.putInStorageOperaterDataBase.executeSqlReturnDt(sql);
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
                                this._errorMessage += "错误信息[" + this.putInStorageOperaterDataBase.errorText + "]";
                            }
                        }
                    }
                    else
                    {
                        this._errorMessage = "连接数据库出错！错误信息[" + this.putInStorageOperaterDataBase.errorText + "]";
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
                this.putInStorageOperaterDataBase.disconnectDataBase();
            }

            return inventoryMasterId;
        }

        /// <summary>
        /// 根据DataRow获取入库单。
        /// 作者：董帅 创建时间：2012年9月6日22:49:22
        /// </summary>
        /// <param name="r">入库单数据</param>
        /// <returns>入库单，失败返回null。</returns>
        private InventoryMasterInfo getPutInStorageFormDataRow(DataRow r)
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
        /// 获取入库单。
        /// 作者：董帅 创建时间：2012-9-7 22:35:23
        /// </summary>
        /// <param name="id">入库单id</param>
        /// <returns>成功返回入库单，否则返回null。</returns>
        public InventoryMasterInfo getPutInStorage(int id)
        {
            InventoryMasterInfo inv = null;

            try
            {
                if (this.putInStorageOperaterDataBase != null)
                {
                    //连接数据库
                    if (this.putInStorageOperaterDataBase.connectDataBase())
                    {

                        //sql语句，获取入库单
                        string sql = "SELECT * FROM INV_INVENTORYMASTER WHERE ID = " + id.ToString();

                        //获取数据
                        DataTable dt = this.putInStorageOperaterDataBase.executeSqlReturnDt(sql);
                        if (dt != null && dt.Rows.Count == 1)
                        {
                            inv = this.getPutInStorageFormDataRow(dt.Rows[0]);
                        }
                        else
                        {
                            this._errorMessage = "获取数据失败！错误信息：[" + this.putInStorageOperaterDataBase.errorText + "]";
                        }
                    }
                    else
                    {
                        this._errorMessage = "连接数据库失败！错误信息：[" + this.putInStorageOperaterDataBase.errorText + "]";
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
                this.putInStorageOperaterDataBase.disconnectDataBase();
            }

            return inv;
        }

        /// <summary>
        /// 获取入库单。
        /// 作者：董帅 创建时间：2012-9-6 23:07:12
        /// </summary>
        /// <param name="number">入库单编号,为null获空字符串表示不指定。</param>
        /// <param name="supplierId">供应商id，等于0表示不指定。</param>
        /// <param name="warehouseId">仓库id，等于0表示不指定。</param>
        /// <param name="states">入库单状态，等于0表示不指定。</param>
        /// <param name="createBegin">创建开始时间，为null获空字符串表示不指定。</param>
        /// <param name="createEnd">创建截止时间,为null获空字符串表示不指定。</param>
        /// <returns>成功返回入库单列表，否则返回null。</returns>
        public List<InventoryMasterInfo> getPutInStorage(string number, int supplierId, int warehouseId, int states, string createBegin, string createEnd)
        {
            List<InventoryMasterInfo> invs = null;

            try
            {
                if (this.putInStorageOperaterDataBase != null)
                {
                    //连接数据库
                    if (this.putInStorageOperaterDataBase.connectDataBase())
                    {

                        //sql语句，获取入库单
                        string sql = "SELECT * FROM INV_INVENTORYMASTER WHERE TYPE = 1";

                        if (!string.IsNullOrEmpty(number))
                        {
                            sql += string.Format(" AND NUMBER = '{0}'",number);
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
                        DataTable dt = this.putInStorageOperaterDataBase.executeSqlReturnDt(sql);
                        if (dt != null)
                        {
                            invs = new List<InventoryMasterInfo>();
                            foreach (DataRow r in dt.Rows)
                            {
                                InventoryMasterInfo inv = this.getPutInStorageFormDataRow(r);
                                if (inv != null)
                                {
                                    invs.Add(inv);
                                }
                            }
                        }
                        else
                        {
                            this._errorMessage = "获取数据失败！错误信息：[" + this.putInStorageOperaterDataBase.errorText + "]";
                        }
                    }
                    else
                    {
                        this._errorMessage = "连接数据库失败！错误信息：[" + this.putInStorageOperaterDataBase.errorText + "]";
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
                this.putInStorageOperaterDataBase.disconnectDataBase();
            }

            return invs;
        }

        /// <summary>
        /// 修改入库单信息。
        /// 作者：董帅 创建时间：2012-9-7 22:46:40
        /// </summary>
        /// <param name="id">要修改的入库单id。</param>
        /// <param name="number">入库单编号。</param>
        /// <param name="supplierId">供应商id。</param>
        /// <param name="warehousId">仓库id。</param>
        /// <returns>成功返回true，否则返回false。</returns>
        public bool changePutInStorage(int id, string number, int supplierId, int warehousId)
        {
            bool bRet = false; //返回值

            try
            {
                if (this.putInStorageOperaterDataBase != null)
                {
                    //连接数据库
                    if (this.putInStorageOperaterDataBase.connectDataBase())
                    {
                        //sql语句
                        string sql = string.Format("UPDATE INV_INVENTORYMASTER SET NUMBER = '{0}',SUPPLIERORCLIENTID = {1},WAREHOUSEID = {2} WHERE ID = {3}"
                                , number
                                , supplierId
                                , warehousId
                                , id);

                        int retCount = this.putInStorageOperaterDataBase.executeSqlWithOutDs(sql);
                        if (retCount == 1)
                        {
                            bRet = true;
                        }
                        else
                        {
                            this._errorMessage = "更新数据失败！";
                            if (retCount != 1)
                            {
                                this._errorMessage += "错误信息[" + this.putInStorageOperaterDataBase.errorText + "]";
                            }
                        }
                    }
                    else
                    {
                        this._errorMessage = "连接数据库出错！错误信息[" + this.putInStorageOperaterDataBase.errorText + "]";
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
                this.putInStorageOperaterDataBase.disconnectDataBase();
            }

            return bRet;
        }

        /// <summary>
        /// 删除入库单。
        /// 作者：董帅 创建时间：2012-9-7 23:06:59
        /// </summary>
        /// <param name="ids">要删除的入库单id。</param>
        /// <returns>成功返回true，否则返回false。</returns>
        public bool deletePutInStorage(int[] ids)
        {
            bool bRet = false; //返回值

            try
            {
                if (this.putInStorageOperaterDataBase != null)
                {
                    //连接数据库
                    if (this.putInStorageOperaterDataBase.connectDataBase())
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

                        int retCount = this.putInStorageOperaterDataBase.executeSqlWithOutDs(sql);
                        if (retCount > 0)
                        {
                            bRet = true;
                        }
                        else
                        {
                            this._errorMessage = "删除数据失败！";
                            this._errorMessage += "错误信息[" + this.putInStorageOperaterDataBase.errorText + "]";
                        }
                    }
                    else
                    {
                        this._errorMessage = "连接数据库出错！错误信息[" + this.putInStorageOperaterDataBase.errorText + "]";
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
                this.putInStorageOperaterDataBase.disconnectDataBase();
            }

            return bRet;
        }

        /// <summary>
        /// 执行入库单。
        /// 作者：董帅 创建时间：2012-9-12 21:37:14
        /// </summary>
        /// <param name="id">入库单id</param>
        /// <param name="user">用户</param>
        /// <returns>成功返回true，失败返回false,</returns>
        public bool executePutInStorage(int id,UserInfo user)
        { 
            bool bRet = false;

            try
            {
                if (this.putInStorageOperaterDataBase != null)
                {
                    //连接数据库
                    if (this.putInStorageOperaterDataBase.connectDataBase())
                    {
                        //开启事务，并且设置隔离级别
                        this.putInStorageOperaterDataBase.beginTransaction(IsolationLevel.RepeatableRead);
                        //修改入库单状态
                        string sql = string.Format("UPDATE INV_INVENTORYMASTER SET STATE = 2 WHERE ID ={0} AND STATE = 1", id);

                        int retCount = this.putInStorageOperaterDataBase.executeSqlWithOutDs(sql);
                        if (retCount == 1)
                        {
                            //明细
                            sql = "SELECT * FROM INV_INVENTORYDETAIL WHERE MASTERID = " + id.ToString() + " ORDER BY GOODSID,COLORID,SIZEID";

                            //获取数据
                            DataTable dt = this.putInStorageOperaterDataBase.executeSqlReturnDt(sql);
                            if (dt != null)
                            {
                                int i = 0;
                                foreach (DataRow row in dt.Rows)
                                {
                                    sql = string.Format("INSERT INTO INV_INVENTORYCOUNT (DETAILID,COUNT) VALUES ({0},{1})", row["ID"], row["COUNT"]);
                                    retCount = this.putInStorageOperaterDataBase.executeSqlWithOutDs(sql);
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
                                    sql = string.Format("UPDATE INV_INVENTORYMASTER SET EXECUTETIME = '{0}',EXECUTEUSER = {1} WHERE ID ={2}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),user.id,id);
                                    retCount = this.putInStorageOperaterDataBase.executeSqlWithOutDs(sql);
                                    if (retCount == 1)
                                    {
                                        bRet = true;
                                    }
                                    else
                                    {
                                        this._errorMessage = "更新执行人和执行时间失败！";
                                        this._errorMessage += "错误信息[" + this.putInStorageOperaterDataBase.errorText + "]";
                                    }
                                }
                            }
                            else
                            {
                                this._errorMessage = "更新库存失败！";
                                this._errorMessage += "错误信息[" + this.putInStorageOperaterDataBase.errorText + "]";
                            }
                        }
                        else
                        {
                            this._errorMessage = "更新入库单状态失败！";
                            this._errorMessage += "错误信息[" + this.putInStorageOperaterDataBase.errorText + "]";
                        }
                    }
                    else
                    {
                        this._errorMessage = "连接数据库出错！错误信息[" + this.putInStorageOperaterDataBase.errorText + "]";
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
                    this.putInStorageOperaterDataBase.commitTransaction();
                }
                else
                {
                    this.putInStorageOperaterDataBase.rollbackTransaction();
                }
                this.putInStorageOperaterDataBase.disconnectDataBase();
            }

            return bRet;
        }

        /// <summary>
        /// 创建入库单明细。
        /// 作者：董帅 创建时间：2012-9-9 14:17:36
        /// </summary>
        /// <param name="inventoryDetail">明细。</param>
        /// <returns>入库单id，失败返回-1。</returns>
        public int createNewPutInStorageDetail(InventoryDetailInfo inventoryDetail)
        {
            int inventoryDetailId = -1; //创建的id。

            try
            {
                if (inventoryDetail == null)
                {
                    this._errorMessage = "不能插入空明细！";
                }
                else
                {
                    //存入数据库
                    if (this.putInStorageOperaterDataBase.connectDataBase())
                    {
                        //新增数据
                        string sql = "";
                        if (inventoryDetail.detailId == -1)
                        {
                            sql = string.Format("INSERT INTO INV_INVENTORYDETAIL (MASTERID,GOODSID,COLORID,SIZEID,COUNT,UNITPRICE) " +
                                     "VALUES ({0},{1},{2},{3},{4},{5}) SELECT SCOPE_IDENTITY() AS id"
                                     , inventoryDetail.masterId
                                     , inventoryDetail.goods.id
                                     , inventoryDetail.color.id
                                     , inventoryDetail.size.id
                                     , inventoryDetail.count
                                     , inventoryDetail.unitPrice);
                        }
                        else
                        {
                            
                        }

                        DataTable retDt = this.putInStorageOperaterDataBase.executeSqlReturnDt(sql);
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
                                this._errorMessage += "错误信息[" + this.putInStorageOperaterDataBase.errorText + "]";
                            }
                        }
                    }
                    else
                    {
                        this._errorMessage = "连接数据库出错！错误信息[" + this.putInStorageOperaterDataBase.errorText + "]";
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
                this.putInStorageOperaterDataBase.disconnectDataBase();
            }

            return inventoryDetailId;
        }

        /// <summary>
        /// 根据DataRow获取入库单明细。
        /// 作者：董帅 创建时间：2012-9-11 21:56:45
        /// </summary>
        /// <param name="r">入库单明细数据</param>
        /// <returns>入库单明细，失败返回null。</returns>
        private InventoryDetailInfo getPutInStorageDetailFormDataRow(DataRow r)
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
        /// 获取指定的明细。
        /// 作者：董帅 创建时间：2012-9-11 22:00:33
        /// </summary>
        /// <param name="id">明细id。</param>
        /// <returns>明细对象。</returns>
        public InventoryDetailInfo getPutInStorageDetail(int id)
        {
            InventoryDetailInfo detail = null;

            try
            {
                if (this.putInStorageOperaterDataBase != null)
                {
                    //连接数据库
                    if (this.putInStorageOperaterDataBase.connectDataBase())
                    {

                        //sql语句，获取入库单
                        string sql = "SELECT * FROM INV_INVENTORYDETAIL WHERE ID = " + id.ToString();

                        //获取数据
                        DataTable dt = this.putInStorageOperaterDataBase.executeSqlReturnDt(sql);
                        if (dt != null && dt.Rows.Count == 1)
                        {
                            detail = this.getPutInStorageDetailFormDataRow(dt.Rows[0]);
                        }
                        else
                        {
                            this._errorMessage = "获取数据失败！错误信息：[" + this.putInStorageOperaterDataBase.errorText + "]";
                        }
                    }
                    else
                    {
                        this._errorMessage = "连接数据库失败！错误信息：[" + this.putInStorageOperaterDataBase.errorText + "]";
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
                this.putInStorageOperaterDataBase.disconnectDataBase();
            }

            return detail;
        }

        /// <summary>
        /// 获取入库单明细。
        /// 作者：董帅 创建时间：2012-9-9 16:56:05
        /// <param name="masterId">入库单id。</param>
        /// <returns>库存数量。</returns>
        /// </summary>
        public List<GoodsCount> getPutInStorageDetailByMasterId(int masterId)
        {
            List<GoodsCount> goods = null;

            try
            {
                if (this.putInStorageOperaterDataBase != null)
                {
                    //连接数据库
                    if (this.putInStorageOperaterDataBase.connectDataBase())
                    {

                        //sql语句，获取入库单明细
                        string sql = "SELECT * FROM INV_INVENTORYDETAIL WHERE MASTERID = " + masterId.ToString() + " ORDER BY GOODSID,COLORID,SIZEID";

                        //获取数据
                        DataTable dt = this.putInStorageOperaterDataBase.executeSqlReturnDt(sql);
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

                                        InventoryDetailInfo detail = this.getPutInStorageDetailFormDataRow(r);
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
                            this._errorMessage = "获取数据失败！错误信息：[" + this.putInStorageOperaterDataBase.errorText + "]";
                        }
                    }
                    else
                    {
                        this._errorMessage = "连接数据库失败！错误信息：[" + this.putInStorageOperaterDataBase.errorText + "]";
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
                this.putInStorageOperaterDataBase.disconnectDataBase();
            }

            return goods;
        }

        /// <summary>
        /// 修改入库单明细。
        /// </summary>
        /// <param name="detail">明细</param>
        /// <returns>成功返回true，否是返回false。</returns>
        public bool changePutInStorageDetail(InventoryDetailInfo detail)
        {
            bool bRet = false; //返回值

            try
            {
                if (this.putInStorageOperaterDataBase != null)
                {
                    //连接数据库
                    if (this.putInStorageOperaterDataBase.connectDataBase())
                    {
                        //sql语句
                        string sql = string.Format("UPDATE INV_INVENTORYDETAIL SET GOODSID = {0},COLORID = {1},SIZEID = {2},COUNT = {3},UNITPRICE = {4} WHERE ID = {5}"
                                , detail.goods.id
                                , detail.color.id
                                , detail.size.id
                                , detail.count
                                , detail.unitPrice
                                , detail.id);

                        int retCount = this.putInStorageOperaterDataBase.executeSqlWithOutDs(sql);
                        if (retCount == 1)
                        {
                            bRet = true;
                        }
                        else
                        {
                            this._errorMessage = "更新数据失败！";
                            if (retCount != 1)
                            {
                                this._errorMessage += "错误信息[" + this.putInStorageOperaterDataBase.errorText + "]";
                            }
                        }
                    }
                    else
                    {
                        this._errorMessage = "连接数据库出错！错误信息[" + this.putInStorageOperaterDataBase.errorText + "]";
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
                this.putInStorageOperaterDataBase.disconnectDataBase();
            }

            return bRet;
        }

        /// <summary>
        /// 删除入库单明细。
        /// 作者：董帅 创建时间：2012-9-11 22:29:18
        /// </summary>
        /// <param name="ids">要删除的入库单明细id。</param>
        /// <returns>成功返回true，否则返回false。</returns>
        public bool deletePutInStorageDetail(int[] ids)
        {
            bool bRet = false; //返回值

            try
            {
                if (this.putInStorageOperaterDataBase != null)
                {
                    //连接数据库
                    if (this.putInStorageOperaterDataBase.connectDataBase())
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

                        int retCount = this.putInStorageOperaterDataBase.executeSqlWithOutDs(sql);
                        if (retCount > 0)
                        {
                            bRet = true;
                        }
                        else
                        {
                            this._errorMessage = "删除数据失败！";
                            this._errorMessage += "错误信息[" + this.putInStorageOperaterDataBase.errorText + "]";
                        }
                    }
                    else
                    {
                        this._errorMessage = "连接数据库出错！错误信息[" + this.putInStorageOperaterDataBase.errorText + "]";
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
                this.putInStorageOperaterDataBase.disconnectDataBase();
            }

            return bRet;
        }
    }
}
