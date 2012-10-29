using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YLR.YAdoNet;
using System.Data;

namespace YLR.YInventory.SupplierAndClient
{
    /// <summary>
    /// 客户和供应商业务类，封装客户和供应商的数据库处理方法。
    /// 作者：董帅 创建时间：2012-9-2 9:03:51
    /// </summary>
    public class SupplierAndClientOperater
    {
        /// <summary>
        /// 客户和供应商数据库。
        /// </summary>
        public YDataBase supplierAndClientDataBase
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
        /// 创建供应商和客户数据库操作对象。
        /// 作者：董帅 创建时间：2012-9-2 9:07:13
        /// </summary>
        /// <param name="configFilePath">配置文件路径。</param>
        /// <param name="nodeName">节点名。</param>
        /// <returns>成功返回操作对象，否则返回null。</returns>
        public static SupplierAndClientOperater createSupplierAndClientOperater(string configFilePath, string nodeName)
        {
            SupplierAndClientOperater supplierAndClientOper = null;

            //获取数据库实例。
            YDataBase orgDb = YDataBaseConfigFile.createDataBase(configFilePath, nodeName);

            if (orgDb != null)
            {
                supplierAndClientOper = new SupplierAndClientOperater();
                supplierAndClientOper.supplierAndClientDataBase = orgDb;
            }
            else
            {
                Exception ex = new Exception("创建数据库实例失败！");
                throw ex;
            }

            return supplierAndClientOper;
        }

        /// <summary>
        /// 创建新客户和供应商。
        /// </summary>
        /// <param name="supplierAndClient">客户和供应商。</param>
        /// <returns>成功返回客户和供应商id，失败返回-1。</returns>
        public int createNewSupplierAndClient(SupplierAndClientInfo supplierAndClient)
        {
            int supplierAndClientId = -1; //创建的仓库id。

            try
            {
                if (supplierAndClient == null)
                {
                    this._errorMessage = "不能插入空客户和供应商！";
                }
                else if (string.IsNullOrEmpty(supplierAndClient.name) || supplierAndClient.name.Length > 50)
                {
                    this._errorMessage = "名称不合法！";
                }
                else
                {
                    //存入数据库
                    if (this.supplierAndClientDataBase.connectDataBase())
                    {
                        //新增数据
                        string sql = string.Format("INSERT INTO INV_SUPPLIERANDCLIENT (NAME,NUMBER) VALUES ('{0}','{1}') SELECT SCOPE_IDENTITY() AS id"
                                , supplierAndClient.name
                                , supplierAndClient.number);

                        DataTable retDt = this.supplierAndClientDataBase.executeSqlReturnDt(sql);
                        if (retDt != null && retDt.Rows.Count > 0)
                        {
                            //获取新增数据id
                            supplierAndClientId = Convert.ToInt32(retDt.Rows[0]["id"]);
                        }
                        else
                        {
                            this._errorMessage = "创建失败！";
                            if (retDt == null)
                            {
                                this._errorMessage += "错误信息[" + this.supplierAndClientDataBase.errorText + "]";
                            }
                        }
                    }
                    else
                    {
                        this._errorMessage = "连接数据库出错！错误信息[" + this.supplierAndClientDataBase.errorText + "]";
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
                this.supplierAndClientDataBase.disconnectDataBase();
            }

            return supplierAndClientId;
        }

        /// <summary>
        /// 根据DataRow获取客户或供应商对象。
        /// 作者：董帅 创建时间：2012-9-2 9:30:59
        /// </summary>
        /// <param name="r">客户或供应商数据</param>
        /// <returns>客户或供应商，失败返回null。</returns>
        private SupplierAndClientInfo getSupplierAndClientFormDataRow(DataRow r)
        {
            if (r != null)
            {
                //仓库对象
                SupplierAndClientInfo s = new SupplierAndClientInfo();

                //仓库id不能为null，否则返回失败。
                if (!r.IsNull("ID"))
                {
                    s.id = Convert.ToInt32(r["ID"]);
                }
                else
                {
                    return null;
                }

                if (!r.IsNull("NAME"))
                {
                    s.name = r["NAME"].ToString();
                }



                if (!r.IsNull("NUMBER"))
                {
                    s.number = r["NUMBER"].ToString();
                }

                return s;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 获取客户和供应商列表。
        /// 作者：董帅 创建时间：2012-9-2 9:37:56
        /// </summary>
        /// <returns>成功返回客户和供应商列表，否则返回null。</returns>
        public List<SupplierAndClientInfo> getSupplierAndClient()
        {
            List<SupplierAndClientInfo> supplierAndClients = null;

            try
            {
                if (this.supplierAndClientDataBase != null)
                {
                    //连接数据库
                    if (this.supplierAndClientDataBase.connectDataBase())
                    {

                        //sql语句，获取数据
                        string sql = sql = "SELECT * FROM INV_SUPPLIERANDCLIENT";
                        
                        //获取数据
                        DataTable dt = this.supplierAndClientDataBase.executeSqlReturnDt(sql);
                        if (dt != null)
                        {
                            supplierAndClients = new List<SupplierAndClientInfo>();
                            foreach (DataRow r in dt.Rows)
                            {
                                SupplierAndClientInfo s = this.getSupplierAndClientFormDataRow(r);
                                if (s != null)
                                {
                                    supplierAndClients.Add(s);
                                }
                            }
                        }
                        else
                        {
                            this._errorMessage = "获取数据失败！错误信息：[" + this.supplierAndClientDataBase.errorText + "]";
                        }
                    }
                    else
                    {
                        this._errorMessage = "连接数据库失败！错误信息：[" + this.supplierAndClientDataBase.errorText + "]";
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
                this.supplierAndClientDataBase.disconnectDataBase();
            }

            return supplierAndClients;
        }

        /// <summary>
        /// 根据客户和供应商id，获取客户和供应商。
        /// 作者：董帅 创建时间：2012-9-2 9:49:15
        /// </summary>
        /// <param name="id">客户和供应商id。</param>
        /// <returns>客户和供应商，失败返回null。</returns>
        public SupplierAndClientInfo getSupplierAndClient(int id)
        {
            SupplierAndClientInfo sOrCInfo = null;

            try
            {
                if (this.supplierAndClientDataBase != null)
                {
                    //连接数据库
                    if (this.supplierAndClientDataBase.connectDataBase())
                    {

                        //sql语句，获取数据
                        string sql = "SELECT * FROM INV_SUPPLIERANDCLIENT WHERE ID = " + id.ToString();

                        //获取数据
                        DataTable dt = this.supplierAndClientDataBase.executeSqlReturnDt(sql);
                        if (dt != null && dt.Rows.Count == 1)
                        {
                            sOrCInfo = this.getSupplierAndClientFormDataRow(dt.Rows[0]);
                        }
                        else
                        {
                            if (dt != null)
                            {
                                this._errorMessage = "获取数据失败！";
                            }
                            else
                            {
                                this._errorMessage = "获取数据失败！错误信息：[" + this.supplierAndClientDataBase.errorText + "]";
                            }
                        }
                    }
                    else
                    {
                        this._errorMessage = "连接数据库失败！错误信息：[" + this.supplierAndClientDataBase.errorText + "]";
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
                this.supplierAndClientDataBase.disconnectDataBase();
            }

            return sOrCInfo;
        }

        /// <summary>
        /// 修改指定客户或供应商内容，通过id匹配。
        /// </summary>
        /// <param name="sOrC">要修改的对象。</param>
        /// <returns>成功返回true，否则返回false。</returns>
        public bool changeSupplierAndClient(SupplierAndClientInfo sOrC)
        {
            bool bRet = false; //返回值

            try
            {
                if (this.supplierAndClientDataBase != null)
                {
                    //连接数据库
                    if (this.supplierAndClientDataBase.connectDataBase())
                    {
                        //sql语句
                        string sql = string.Format("UPDATE INV_SUPPLIERANDCLIENT SET NAME = '{0}',NUMBER = '{1}' WHERE ID = {2}"
                                , sOrC.name
                                , sOrC.number
                                , sOrC.id);

                        int retCount = this.supplierAndClientDataBase.executeSqlWithOutDs(sql);
                        if (retCount == 1)
                        {
                            bRet = true;
                        }
                        else
                        {
                            this._errorMessage = "更新数据失败！";
                            if (retCount != 1)
                            {
                                this._errorMessage += "错误信息[" + this.supplierAndClientDataBase.errorText + "]";
                            }
                        }
                    }
                    else
                    {
                        this._errorMessage = "连接数据库出错！错误信息[" + this.supplierAndClientDataBase.errorText + "]";
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
                this.supplierAndClientDataBase.disconnectDataBase();
            }

            return bRet;
        }

        /// <summary>
        /// 删除客户或供应商。
        /// 作者：董帅 创建时间：2012-9-2 9:59:22
        /// </summary>
        /// <param name="ids">要删除的客户或供应商id。</param>
        /// <returns>成功返回true，否则返回false。</returns>
        public bool deleteSupplierAndClient(int[] ids)
        {
            bool bRet = false; //返回值

            try
            {
                if (this.supplierAndClientDataBase != null)
                {
                    //连接数据库
                    if (this.supplierAndClientDataBase.connectDataBase())
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
                        string sql = string.Format("DELETE INV_SUPPLIERANDCLIENT WHERE ID IN ({0})", strIds);

                        int retCount = this.supplierAndClientDataBase.executeSqlWithOutDs(sql);
                        if (retCount > 0)
                        {
                            bRet = true;
                        }
                        else
                        {
                            this._errorMessage = "删除数据失败！";
                            this._errorMessage += "错误信息[" + this.supplierAndClientDataBase.errorText + "]";
                        }
                    }
                    else
                    {
                        this._errorMessage = "连接数据库出错！错误信息[" + this.supplierAndClientDataBase.errorText + "]";
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
                this.supplierAndClientDataBase.disconnectDataBase();
            }

            return bRet;
        }
    }
}
