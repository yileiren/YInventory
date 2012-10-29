using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using YLR.YMessage;
using YLR.YInventory.Goods;

namespace YAgileASP.background.inventory.goods
{
    public partial class goods_list : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!this.IsPostBack)
                {
                    this.bindGoods();
                }
            }
            catch (Exception ex)
            {
                YMessageBox.show(this, "程序运行出错！错误信息[" + ex.Message + "]");
            }
        }

        /// <summary>
        /// 绑定货物。
        /// 作者：董帅 创建时间：2012-9-2 16:40:40
        /// </summary>
        public void bindGoods()
        {
            try
            {
                //获取配置文件路径。
                string configFile = AppDomain.CurrentDomain.BaseDirectory.ToString() + "DataBaseConfig.xml";

                //获取数据库操作对象
                GoodsOperater oper = GoodsOperater.createGoodsOperater(configFile, "SQLServer");
                if (oper != null)
                {
                    //获取仓库列表
                    List<GoodsInfo> goods = oper.getGoods();
                    if (goods != null)
                    {
                        this.goodsList.DataSource = goods;
                        this.goodsList.DataBind();
                    }
                    else
                    {
                        YMessageBox.show(this, "获取数据失败！错误信息[" + oper.errorMessage + "]");
                    }
                }
                else
                {
                    YMessageBox.show(this, "获取数据库操作对象失败！");
                }
            }
            catch (Exception ex)
            {
                YMessageBox.show(this, "运行错误！错误信息[" + ex.Message + "]");
            }
        }

        /// <summary>
        /// 删除数据
        /// 作者：董帅 创建时间：2012-9-2 16:41:38
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void butDeleteItems_Click(object sender, EventArgs e)
        {
            try
            {
                string s = Request["chkGoods"];
                string[] goodsIds = new string[0];
                if (!string.IsNullOrEmpty(s))
                {
                    goodsIds = s.Split(','); //要删除的id
                }

                if (goodsIds.Length > 0)
                {
                    //获取配置文件路径。
                    string configFile = AppDomain.CurrentDomain.BaseDirectory.ToString() + "DataBaseConfig.xml";

                    //创建数据库操作对象。
                    GoodsOperater oper = GoodsOperater.createGoodsOperater(configFile, "SQLServer");
                    if (oper != null)
                    {

                        //删除
                        int[] goodsIntIds = new int[goodsIds.Length];
                        for (int i = 0; i < goodsIds.Length; i++)
                        {
                            goodsIntIds[i] = Convert.ToInt32(goodsIds[i]);
                        }

                        if (oper.deleteGoods(goodsIntIds))
                        {
                            this.Response.Redirect("goods_list.aspx");
                            //YMessageBox.showAndResponseScript(this, "删除数据成功！", "", "window.location.href='dataDictionary_list.aspx?parentId=" + this.hidParentId.Value + "'");
                        }
                        else
                        {
                            YMessageBox.show(this, "删除数据失败！");
                        }
                    }
                    else
                    {
                        YMessageBox.show(this, "获取数据库实例失败！");
                    }
                }
                else
                {
                    YMessageBox.show(this, "没有选择要删除的数据！");
                }
            }
            catch (Exception ex)
            {
                YMessageBox.show(this, "系统运行异常！异常信息[" + ex.Message + "]");
            }
        }
    }
}