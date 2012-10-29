using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YLR.YAdoNet;
using System.Data;

namespace YLR.YInventory.Goods
{
    /// <summary>
    /// 获取操作业务类。
    /// 作者：董帅 创建时间：2012-9-2 15:49:43
    /// </summary>
    public class GoodsOperater
    {
        /// <summary>
        /// 数据库连接对象。
        /// </summary>
        public YDataBase goodsDataBase
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
        /// 创建数据库操作对象。
        /// 作者：董帅 创建时间：2012-9-2 15:51:00
        /// </summary>
        /// <param name="configFilePath">配置文件路径。</param>
        /// <param name="nodeName">节点名。</param>
        /// <returns>成功返回操作对象，否则返回null。</returns>
        public static GoodsOperater createGoodsOperater(string configFilePath, string nodeName)
        {
            GoodsOperater GoodsOper = null;

            //获取数据库实例。
            YDataBase orgDb = YDataBaseConfigFile.createDataBase(configFilePath, nodeName);

            if (orgDb != null)
            {
                GoodsOper = new GoodsOperater();
                GoodsOper.goodsDataBase = orgDb;
            }
            else
            {
                Exception ex = new Exception("创建数据库实例失败！");
                throw ex;
            }

            return GoodsOper;
        }

        /// <summary>
        /// 创建获取。
        /// 作者：董帅 创建时间：2012-9-2 15:52:30
        /// </summary>
        /// <param name="goods">获取信息。</param>
        /// <returns>成功返回id，失败返回-1。</returns>
        public int createNewGoods(GoodsInfo goods)
        {
            int goodsId = -1; //创建的id。

            try
            {
                if (goods == null)
                {
                    this._errorMessage = "不能插入空货物！";
                }
                else if (string.IsNullOrEmpty(goods.name) || goods.name.Length > 50)
                {
                    this._errorMessage = "名称不合法！";
                }
                else
                {
                    //存入数据库
                    if (this.goodsDataBase.connectDataBase())
                    {
                        //新增数据
                        string sql = string.Format("INSERT INTO INV_GOODS (NAME,NUMBER) VALUES ('{0}','{1}') SELECT SCOPE_IDENTITY() AS id"
                                , goods.name
                                , goods.number);

                        DataTable retDt = this.goodsDataBase.executeSqlReturnDt(sql);
                        if (retDt != null && retDt.Rows.Count > 0)
                        {
                            //获取新增数据id
                            goodsId = Convert.ToInt32(retDt.Rows[0]["id"]);
                        }
                        else
                        {
                            this._errorMessage = "创建失败！";
                            if (retDt == null)
                            {
                                this._errorMessage += "错误信息[" + this.goodsDataBase.errorText + "]";
                            }
                        }
                    }
                    else
                    {
                        this._errorMessage = "连接数据库出错！错误信息[" + this.goodsDataBase.errorText + "]";
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
                this.goodsDataBase.disconnectDataBase();
            }

            return goodsId;
        }

        /// <summary>
        /// 根据DataRow获取货物对象。
        /// 作者：董帅 创建时间：2012-9-2 15:54:51
        /// </summary>
        /// <param name="r">货物数据</param>
        /// <returns>货物，失败返回null。</returns>
        private GoodsInfo getGoodsFormDataRow(DataRow r)
        {
            if (r != null)
            {
                //仓库对象
                GoodsInfo g = new GoodsInfo();

                //仓库id不能为null，否则返回失败。
                if (!r.IsNull("ID"))
                {
                    g.id = Convert.ToInt32(r["ID"]);
                }
                else
                {
                    return null;
                }

                if (!r.IsNull("NAME"))
                {
                    g.name = r["NAME"].ToString();
                }



                if (!r.IsNull("NUMBER"))
                {
                    g.number = r["NUMBER"].ToString();
                }

                return g;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 获取货物列表。
        /// 作者：董帅 创建时间：2012-9-2 15:55:42
        /// </summary>
        /// <returns>成功返回货物列表，否则返回null。</returns>
        public List<GoodsInfo> getGoods()
        {
            List<GoodsInfo> goods = null;

            try
            {
                if (this.goodsDataBase != null)
                {
                    //连接数据库
                    if (this.goodsDataBase.connectDataBase())
                    {

                        //sql语句，获取数据
                        string sql = sql = "SELECT * FROM INV_GOODS";

                        //获取数据
                        DataTable dt = this.goodsDataBase.executeSqlReturnDt(sql);
                        if (dt != null)
                        {
                            goods = new List<GoodsInfo>();
                            foreach (DataRow r in dt.Rows)
                            {
                                GoodsInfo g = this.getGoodsFormDataRow(r);
                                if (g != null)
                                {
                                    goods.Add(g);
                                }
                            }
                        }
                        else
                        {
                            this._errorMessage = "获取数据失败！错误信息：[" + this.goodsDataBase.errorText + "]";
                        }
                    }
                    else
                    {
                        this._errorMessage = "连接数据库失败！错误信息：[" + this.goodsDataBase.errorText + "]";
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
                this.goodsDataBase.disconnectDataBase();
            }

            return goods;
        }

        /// <summary>
        /// 根据货物id，获取指定的货物。
        /// 作者：董帅 创建时间：2012-9-2 15:59:50
        /// </summary>
        /// <param name="id">货物id。</param>
        /// <returns>货物，失败返回null。</returns>
        public GoodsInfo getGoods(int id)
        {
            GoodsInfo goods = null;

            try
            {
                if (this.goodsDataBase != null)
                {
                    //连接数据库
                    if (this.goodsDataBase.connectDataBase())
                    {

                        //sql语句，获取数据
                        string sql = "SELECT * FROM INV_GOODS WHERE ID = " + id.ToString();

                        //获取数据
                        DataTable dt = this.goodsDataBase.executeSqlReturnDt(sql);
                        if (dt != null && dt.Rows.Count == 1)
                        {
                            goods = this.getGoodsFormDataRow(dt.Rows[0]);
                        }
                        else
                        {
                            if (dt != null)
                            {
                                this._errorMessage = "获取数据失败！";
                            }
                            else
                            {
                                this._errorMessage = "获取数据失败！错误信息：[" + this.goodsDataBase.errorText + "]";
                            }
                        }
                    }
                    else
                    {
                        this._errorMessage = "连接数据库失败！错误信息：[" + this.goodsDataBase.errorText + "]";
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
                this.goodsDataBase.disconnectDataBase();
            }

            return goods;
        }

        /// <summary>
        /// 修改指定货物内容，通过id匹配。
        /// 作者：董帅 创建时间：2012-9-2 16:02:23
        /// </summary>
        /// <param name="goods">要修改的对象。</param>
        /// <returns>成功返回true，否则返回false。</returns>
        public bool changeGoods(GoodsInfo goods)
        {
            bool bRet = false; //返回值

            try
            {
                if (this.goodsDataBase != null)
                {
                    //连接数据库
                    if (this.goodsDataBase.connectDataBase())
                    {
                        //sql语句
                        string sql = string.Format("UPDATE INV_GOODS SET NAME = '{0}',NUMBER = '{1}' WHERE ID = {2}"
                                , goods.name
                                , goods.number
                                , goods.id);

                        int retCount = this.goodsDataBase.executeSqlWithOutDs(sql);
                        if (retCount == 1)
                        {
                            bRet = true;
                        }
                        else
                        {
                            this._errorMessage = "更新数据失败！";
                            if (retCount != 1)
                            {
                                this._errorMessage += "错误信息[" + this.goodsDataBase.errorText + "]";
                            }
                        }
                    }
                    else
                    {
                        this._errorMessage = "连接数据库出错！错误信息[" + this.goodsDataBase.errorText + "]";
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
                this.goodsDataBase.disconnectDataBase();
            }

            return bRet;
        }

        /// <summary>
        /// 删除货物。
        /// 作者：董帅 创建时间：2012-9-2 16:02:10
        /// </summary>
        /// <param name="ids">要删除的客户或供应商id。</param>
        /// <returns>成功返回true，否则返回false。</returns>
        public bool deleteGoods(int[] ids)
        {
            bool bRet = false; //返回值

            try
            {
                if (this.goodsDataBase != null)
                {
                    //连接数据库
                    if (this.goodsDataBase.connectDataBase())
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
                        string sql = string.Format("DELETE INV_GOODS WHERE ID IN ({0})", strIds);

                        int retCount = this.goodsDataBase.executeSqlWithOutDs(sql);
                        if (retCount > 0)
                        {
                            bRet = true;
                        }
                        else
                        {
                            this._errorMessage = "删除数据失败！";
                            this._errorMessage += "错误信息[" + this.goodsDataBase.errorText + "]";
                        }
                    }
                    else
                    {
                        this._errorMessage = "连接数据库出错！错误信息[" + this.goodsDataBase.errorText + "]";
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
                this.goodsDataBase.disconnectDataBase();
            }

            return bRet;
        }
    }
}
