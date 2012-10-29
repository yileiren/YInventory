using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YLR.YAdoNet;
using System.Data;

namespace YLR.YInventory.Warehouse
{
    /// <summary>
    /// 仓库数据库操作业务类，封装操作仓库数据库的方法。
    /// 作者：董帅 创建时间：2012-8-30 11:33:58
    /// </summary>
    public class WarehouseOperater
    {
        /// <summary>
        /// 数据字典数据库。
        /// </summary>
        protected YDataBase _warehouseDataBase = null;

        /// <summary>
        /// 数据字典数据库。
        /// </summary>
        public YDataBase warehouseDataBase
        {
            get
            {
                return this._warehouseDataBase;
            }
            set
            {
                this._warehouseDataBase = value;
            }
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
        /// 创建仓库数据库操作对象。
        /// 作者：董帅 创建时间：2012-8-30 11:37:39
        /// </summary>
        /// <param name="configFilePath">配置文件路径。</param>
        /// <param name="nodeName">节点名。</param>
        /// <returns>成功返回操作对象，否则返回null。</returns>
        public static WarehouseOperater createWarehouseOperater(string configFilePath, string nodeName)
        {
            WarehouseOperater wareOper = null;

            //获取数据库实例。
            YDataBase orgDb = YDataBaseConfigFile.createDataBase(configFilePath, nodeName);

            if (orgDb != null)
            {
                wareOper = new WarehouseOperater();
                wareOper.warehouseDataBase = orgDb;
            }
            else
            {
                Exception ex = new Exception("创建数据库实例失败！");
                throw ex;
            }

            return wareOper;
        }

        /// <summary>
        /// 创建新仓库。
        /// </summary>
        /// <param name="ware">新仓库。</param>
        /// <returns>成功返回仓库id，失败返回-1。</returns>
        public int createNewWarehouse(WarehouseInfo ware)
        {
            int wareId = -1; //创建的仓库id。

            try
            {
                if (ware == null)
                {
                    this._errorMessage = "不能插入空仓库！";
                }
                else if (string.IsNullOrEmpty(ware.name) || ware.name.Length > 50)
                {
                    this._errorMessage = "仓库名称不合法！";
                }
                else
                {
                    //存入数据库
                    if (this.warehouseDataBase.connectDataBase())
                    {
                        //新增数据
                        string sql = "";
                        if (ware.parentId == -1)
                        {

                            sql = string.Format("INSERT INTO INV_WAREHOUSE (NAME,EXPLAIN,[ORDER]) VALUES ('{0}','{1}',{2}) SELECT SCOPE_IDENTITY() AS id"
                                , ware.name
                                , ware.explain
                                , ware.order);
                        }
                        else
                        {
                            sql = string.Format("INSERT INTO INV_WAREHOUSE (NAME,EXPLAIN,[ORDER],PARENTID) VALUES ('{0}','{1}',{2},{3}) SELECT SCOPE_IDENTITY() AS id"
                                , ware.name
                                , ware.explain
                                , ware.order
                                , ware.parentId);
                        }

                        DataTable retDt = this.warehouseDataBase.executeSqlReturnDt(sql);
                        if (retDt != null && retDt.Rows.Count > 0)
                        {
                            //获取仓库id
                            wareId = Convert.ToInt32(retDt.Rows[0]["id"]);
                        }
                        else
                        {
                            this._errorMessage = "创建仓库失败！";
                            if (retDt == null)
                            {
                                this._errorMessage += "错误信息[" + this.warehouseDataBase.errorText + "]";
                            }
                        }
                    }
                    else
                    {
                        this._errorMessage = "连接数据库出错！错误信息[" + this.warehouseDataBase.errorText + "]";
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
                this.warehouseDataBase.disconnectDataBase();
            }

            return wareId;
        }

        /// <summary>
        /// 根据DataRow获取仓库对象。
        /// 作者：董帅 创建时间：2012-8-30 12:57:58
        /// </summary>
        /// <param name="r">仓库数据</param>
        /// <returns>仓库，失败返回null。</returns>
        private WarehouseInfo getWarehouseFormDataRow(DataRow r)
        {
            if (r != null)
            {
                //仓库对象
                WarehouseInfo war = new WarehouseInfo();

                //仓库id不能为null，否则返回失败。
                if (!r.IsNull("ID"))
                {
                    war.id = Convert.ToInt32(r["ID"]);
                }
                else
                {
                    return null;
                }

                if (!r.IsNull("NAME"))
                {
                    war.name = r["NAME"].ToString();
                }

                if (!r.IsNull("PARENTID"))
                {
                    war.parentId = Convert.ToInt32(r["PARENTID"]);
                }
                else
                {
                    war.parentId = -1;
                }

                if (!r.IsNull("EXPLAIN"))
                {
                    war.explain = r["EXPLAIN"].ToString();
                }

                if (!r.IsNull("ORDER"))
                {
                    war.order = Convert.ToInt32(r["ORDER"]);
                }
                else
                {
                    war.order = 0;
                }

                return war;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 根据仓库id，获取仓库。
        /// 作者：董帅 创建时间：2012-8-30 13:03:48
        /// </summary>
        /// <param name="id">仓库id。</param>
        /// <returns>仓库，失败返回null。</returns>
        public WarehouseInfo getWarehouse(int id)
        {
            WarehouseInfo wareInfo = null;

            try
            {
                if (this.warehouseDataBase != null)
                {
                    //连接数据库
                    if (this.warehouseDataBase.connectDataBase())
                    {

                        //sql语句，获取数据
                        string sql = "SELECT * FROM INV_WAREHOUSE WHERE ID = " + id.ToString();

                        //获取数据
                        DataTable dt = this.warehouseDataBase.executeSqlReturnDt(sql);
                        if (dt != null && dt.Rows.Count == 1)
                        {
                            wareInfo = this.getWarehouseFormDataRow(dt.Rows[0]);
                        }
                        else
                        {
                            if (dt != null)
                            {
                                this._errorMessage = "获取数据失败！";
                            }
                            else
                            {
                                this._errorMessage = "获取数据失败！错误信息：[" + this.warehouseDataBase.errorText + "]";
                            }
                        }
                    }
                    else
                    {
                        this._errorMessage = "连接数据库失败！错误信息：[" + this.warehouseDataBase.errorText + "]";
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
                this.warehouseDataBase.disconnectDataBase();
            }

            return wareInfo;
        }

        /// <summary>
        /// 通过指定的父id获取仓库列表。
        /// 作者：董帅 创建时间：2012-8-30 12:58:08
        /// </summary>
        /// <param name="pId">父id，如果是顶级仓库，父id为-1</param>
        /// <returns>成功返回仓库列表，否则返回null。</returns>
        public List<WarehouseInfo> getWarehouseByParentId(int pId)
        {
            List<WarehouseInfo> wares = null;

            try
            {
                if (this.warehouseDataBase != null)
                {
                    //连接数据库
                    if (this.warehouseDataBase.connectDataBase())
                    {

                        //sql语句，获取仓库
                        string sql = "";
                        if (pId == -1)
                        {
                            sql = "SELECT * FROM INV_WAREHOUSE WHERE PARENTID IS NULL ORDER BY [ORDER] ASC";
                        }
                        else
                        {
                            sql = "SELECT * FROM INV_WAREHOUSE WHERE PARENTID = " + pId.ToString() + " ORDER BY [ORDER] ASC";
                        }
                        //获取数据
                        DataTable dt = this.warehouseDataBase.executeSqlReturnDt(sql);
                        if (dt != null)
                        {
                            wares = new List<WarehouseInfo>();
                            foreach (DataRow r in dt.Rows)
                            {
                                WarehouseInfo w = this.getWarehouseFormDataRow(r);
                                if (w != null)
                                {
                                    wares.Add(w);
                                }
                            }
                        }
                        else
                        {
                            this._errorMessage = "获取数据失败！错误信息：[" + this.warehouseDataBase.errorText + "]";
                        }
                    }
                    else
                    {
                        this._errorMessage = "连接数据库失败！错误信息：[" + this.warehouseDataBase.errorText + "]";
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
                this.warehouseDataBase.disconnectDataBase();
            }

            return wares;
        }

        /// <summary>
        /// 通过指定用户可以管理的仓库列表。
        /// 作者：董帅 创建时间：2012-9-5 13:24:01
        /// </summary>
        /// <param name="userId">用户id。</param>
        /// <returns>成功返回仓库列表，否则返回null。</returns>
        public List<WarehouseInfo> getWarehouseByUserId(int userId)
        {
            List<WarehouseInfo> wares = null;

            try
            {
                if (this.warehouseDataBase != null)
                {
                    //连接数据库
                    if (this.warehouseDataBase.connectDataBase())
                    {

                        //sql语句，获取仓库
                        string sql = "SELECT INV_WAREHOUSE.* FROM INV_WAREHOUSE,INV_WAREHOUSE_USER WHERE INV_WAREHOUSE_USER.USERID = " + userId.ToString() + " AND INV_WAREHOUSE.ID = INV_WAREHOUSE_USER.WAREHOUSEID ORDER BY INV_WAREHOUSE.[ORDER] ASC"; ;
                        
                        //获取数据
                        DataTable dt = this.warehouseDataBase.executeSqlReturnDt(sql);
                        if (dt != null)
                        {
                            wares = new List<WarehouseInfo>();
                            foreach (DataRow r in dt.Rows)
                            {
                                WarehouseInfo w = this.getWarehouseFormDataRow(r);
                                if (w != null)
                                {
                                    wares.Add(w);
                                }
                            }
                        }
                        else
                        {
                            this._errorMessage = "获取数据失败！错误信息：[" + this.warehouseDataBase.errorText + "]";
                        }
                    }
                    else
                    {
                        this._errorMessage = "连接数据库失败！错误信息：[" + this.warehouseDataBase.errorText + "]";
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
                this.warehouseDataBase.disconnectDataBase();
            }

            return wares;
        }

        /// <summary>
        /// 修改指定仓库内容，通过仓库id匹配。
        /// </summary>
        /// <param name="ware">要修改的仓库。</param>
        /// <returns>成功返回true，否则返回false。</returns>
        public bool changeWarehouse(WarehouseInfo ware)
        {
            bool bRet = false; //返回值

            try
            {
                if (this.warehouseDataBase != null)
                {
                    //连接数据库
                    if (this.warehouseDataBase.connectDataBase())
                    {
                        //sql语句
                        string sql = "";
                        if (ware.parentId == -1)
                        {
                            //顶级仓库
                            sql = string.Format("UPDATE INV_WAREHOUSE SET NAME = '{0}',EXPLAIN = '{1}',[ORDER] = {2} WHERE ID = {3}"
                                , ware.name
                                , ware.explain
                                , ware.order
                                , ware.id);
                        }
                        else
                        {
                            sql = string.Format("UPDATE INV_WAREHOUSE SET NAME = '{0}',EXPLAIN = '{1}',[ORDER] = {2},PARENTID = {3} WHERE ID = {4}"
                                , ware.name
                                , ware.explain
                                , ware.order
                                , ware.parentId
                                , ware.id);
                        }

                        int retCount = this.warehouseDataBase.executeSqlWithOutDs(sql);
                        if (retCount == 1)
                        {
                            bRet = true;
                        }
                        else
                        {
                            this._errorMessage = "更新数据失败！";
                            if (retCount != 1)
                            {
                                this._errorMessage += "错误信息[" + this.warehouseDataBase.errorText + "]";
                            }
                        }
                    }
                    else
                    {
                        this._errorMessage = "连接数据库出错！错误信息[" + this.warehouseDataBase.errorText + "]";
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
                this.warehouseDataBase.disconnectDataBase();
            }

            return bRet;
        }

        /// <summary>
        /// 删除指定的仓库，删除时，连同子仓库一并删除。
        /// 作者：董帅 创建时间：2012-8-30 13:25:14
        /// </summary>
        /// <param name="wareIds">仓库id</param>
        /// <returns>成功返回true，否则返回false。</returns>
        public bool deleteWarehouses(int[] wareIds)
        {
            bool bRet = true;
            try
            {
                //连接数据库
                if (this.warehouseDataBase.connectDataBase())
                {
                    this.warehouseDataBase.beginTransaction(); //开启事务

                    //删除仓库
                    foreach (int i in wareIds)
                    {
                        if (this.deleteChildWarehouses(i))
                        {
                            //删除当前机构
                            string sql = "DELETE INV_WAREHOUSE WHERE ID = " + i.ToString();
                            if (this.warehouseDataBase.executeSqlWithOutDs(sql) != 1)
                            {
                                bRet = false;
                                break;
                            }
                        }
                        else
                        {
                            bRet = false;
                            break;
                        }
                    }
                }
                else
                {
                    this._errorMessage = "连接数据库出错！错误信息[" + this.warehouseDataBase.errorText + "]";
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (bRet)
                {
                    //提交
                    this.warehouseDataBase.commitTransaction();
                }
                else
                {
                    //回滚
                    this.warehouseDataBase.rollbackTransaction();
                }
                this.warehouseDataBase.disconnectDataBase();
            }

            return bRet;
        }

        /// <summary>
        /// 递归删除指定子仓库。调用前需要连接数据库，调用后关闭。
        /// 作者：董帅 创建时间：2012-8-30 13:26:00
        /// </summary>
        /// <param name="wareId">仓库id</param>
        /// <returns>成功返回true，否则返回false。</returns>
        private bool deleteChildWarehouses(int wareId)
        {
            bool bRet = true;

            try
            {
                //获取下级仓库
                List<WarehouseInfo> wares = null;

                string sql = "";
                if (wareId == -1)
                {
                    sql = "SELECT * FROM INV_WAREHOUSE WHERE PARENTID IS NULL ORDER BY [ORDER] ASC";
                }
                else
                {
                    sql = "SELECT * FROM INV_WAREHOUSE WHERE PARENTID = " + wareId.ToString() + " ORDER BY [ORDER] ASC";
                }
                //获取数据
                DataTable dt = this.warehouseDataBase.executeSqlReturnDt(sql);
                if (dt != null)
                {
                    wares = new List<WarehouseInfo>();
                    foreach (DataRow r in dt.Rows)
                    {
                        WarehouseInfo w = this.getWarehouseFormDataRow(r);
                        if (w != null)
                        {
                            wares.Add(w);
                        }
                    }
                }
                else
                {
                    this._errorMessage = "获取数据失败！错误信息：[" + this.warehouseDataBase.errorText + "]";
                }

                if (wares != null)
                {
                    for (int j = 0; j < wares.Count; j++)
                    {
                        //删除下级仓库
                        if (this.deleteChildWarehouses(wares[j].id))
                        {
                            //删除当前仓库
                            sql = "DELETE INV_WAREHOUSE WHERE ID = " + wares[j].id.ToString();
                            if (this.warehouseDataBase.executeSqlWithOutDs(sql) != 1)
                            {
                                bRet = false;
                                break;
                            }
                        }
                        else
                        {
                            bRet = false;
                            break;
                        }
                    }
                }
                else
                {
                    bRet = false;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return bRet;
        }

        /// <summary>
        /// 根据仓库id获取用户，并标记选中的用户。
        /// 作者：董帅 创建时间：2012-8-30 21:37:49
        /// </summary>
        /// <param name="wareId">仓库id</param>
        /// <returns>用户，失败返回null</returns>
        public List<WarehouseAdmin> getChouseUser(int wareId)
        {
            List<WarehouseAdmin> users = new List<WarehouseAdmin>();

            try
            {
                if (this.warehouseDataBase != null)
                {
                    //连接数据库
                    if (this.warehouseDataBase.connectDataBase())
                    {

                        //sql语句，获取除root外所有用户
                        string sql = "SELECT * FROM ORG_USER WHERE ID <> 1 AND ISDELETE = 'N' ORDER BY ORGANIZATIONID ASC,[ORDER] ASC";
                        string chouseUserSql = "SELECT * FROM INV_WAREHOUSE_USER WHERE WAREHOUSEID = " + wareId.ToString();
                        //获取数据
                        DataTable dt = this.warehouseDataBase.executeSqlReturnDt(sql);
                        DataTable chouseUserDt = this.warehouseDataBase.executeSqlReturnDt(chouseUserSql);
                        if (dt != null && chouseUserDt != null)
                        {
                            //获取选中用户
                            foreach (DataRow row in dt.Rows)
                            {
                                WarehouseAdmin ur = new WarehouseAdmin();

                                //设置数据
                                if (!row.IsNull("ID"))
                                {
                                    ur.id = Convert.ToInt32(row["ID"]);
                                }
                                else
                                {
                                    return null;
                                }

                                if (!row.IsNull("NAME"))
                                {
                                    ur.name = row["NAME"].ToString();
                                }

                                if (!row.IsNull("LOGNAME"))
                                {
                                    ur.logName = row["LOGNAME"].ToString();
                                }

                                if (!row.IsNull("LOGPASSWORD"))
                                {
                                    ur.logPassword = row["LOGPASSWORD"].ToString();
                                }

                                if (!row.IsNull("ORGANIZATIONID"))
                                {
                                    ur.organizationId = Convert.ToInt32(row["ORGANIZATIONID"]);
                                }

                                //是否删除
                                if (!row.IsNull("ISDELETE"))
                                {
                                    if ("Y" == row["ISDELETE"].ToString())
                                    {
                                        ur.isDelete = true;
                                    }
                                    else
                                    {
                                        ur.isDelete = false;
                                    }
                                }

                                //序号
                                if (!row.IsNull("ORDER"))
                                {
                                    ur.order = Convert.ToInt32(row["ORDER"]);
                                }

                                if (chouseUserDt.Select("USERID = " + row["ID"].ToString()).Length > 0)
                                {
                                    ur.choused = true;
                                }

                                users.Add(ur);
                            }

                        }

                    }
                    else
                    {
                        this._errorMessage = "连接数据库失败！错误信息：[" + this.warehouseDataBase.errorText + "]";
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
                this.warehouseDataBase.disconnectDataBase();
            }

            return users;
        }

        /// <summary>
        /// 选择用户，将指定仓库id保存到数据库。
        /// 作者：董帅 创建时间：2012-8-30 22:24:49
        /// </summary>
        /// <param name="wareId">仓库id。</param>
        /// <param name="userIds">选中的用户id。</param>
        /// <returns>成功返回true，否则返回false。</returns>
        public bool chouseWarehouseUser(int wareId, int[] userIds)
        {
            bool bRet = false;

            try
            {
                if (this.warehouseDataBase != null)
                {
                    //连接数据库
                    if (this.warehouseDataBase.connectDataBase())
                    {
                        this.warehouseDataBase.beginTransaction();

                        string deleteSql = "DELETE INV_WAREHOUSE_USER WHERE WAREHOUSEID = " + wareId.ToString();

                        if (this.warehouseDataBase.executeSqlWithOutDs(deleteSql) >= 0)
                        {

                            int count = 0;
                            foreach (int uId in userIds)
                            {
                                //sql语句
                                string sql = string.Format("INSERT INTO INV_WAREHOUSE_USER (WAREHOUSEID,USERID) VALUES ({0},{1})", wareId, uId);
                                int retCount = this.warehouseDataBase.executeSqlWithOutDs(sql);

                                if (retCount == 1)
                                {
                                    count++;
                                }
                                else
                                {
                                    break;
                                }
                            }

                            if (count == userIds.Length)
                            {
                                bRet = true;
                            }
                            else
                            {
                                this._errorMessage = "存储数据失败！";
                                this._errorMessage += "错误信息[" + this.warehouseDataBase.errorText + "]";
                            }
                        }
                    }
                    else
                    {
                        this._errorMessage = "连接数据库出错！错误信息[" + this.warehouseDataBase.errorText + "]";
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
                if (bRet)
                {
                    this.warehouseDataBase.commitTransaction();
                }
                else
                {
                    this.warehouseDataBase.rollbackTransaction();
                }

                this.warehouseDataBase.disconnectDataBase();
            }

            return bRet;
        }
    }
}
