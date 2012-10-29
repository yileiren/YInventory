using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using YLR.YInventory.Goods;
using YLR.YMessage;

namespace YAgileASP.background.inventory.goods
{
    public partial class goods_edit : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                //获取id
                string strId = Request.QueryString["id"];
                if (!string.IsNullOrEmpty(strId))
                {
                    this.hidGoodsId.Value = strId;

                    //获取配置文件路径。
                    string configFile = AppDomain.CurrentDomain.BaseDirectory.ToString() + "DataBaseConfig.xml";

                    //创建操作对象
                    GoodsOperater oper = GoodsOperater.createGoodsOperater(configFile, "SQLServer");
                    if (oper != null)
                    {
                        //获取仓库信息
                        GoodsInfo goodsInfo = oper.getGoods(Convert.ToInt32(strId));
                        if (goodsInfo != null)
                        {
                            this.txtGoodsName.Value = goodsInfo.name;
                            this.txtGoodsNum.Value = goodsInfo.number;
                        }
                        else
                        {
                            YMessageBox.show(this, "获取数据失败！错误信息[" + oper.errorMessage + "]");
                            return;
                        }
                    }
                    else
                    {
                        YMessageBox.show(this, "创建数据库操作对象失败！");
                        return;
                    }
                }
            }
        }

        /// <summary>
        /// 保存数据。
        /// 作者：董帅 创建时间：2012-9-2 16:29:12
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void butSave_Click(object sender, EventArgs e)
        {
            try
            {
                GoodsInfo goodsInfo = new GoodsInfo();

                goodsInfo.name = this.txtGoodsName.Value;
                if (string.IsNullOrEmpty(goodsInfo.name) || goodsInfo.name.Length > 50)
                {
                    YMessageBox.show(this, "名称不合法！");
                    return;
                }

                goodsInfo.number = this.txtGoodsNum.Value;
                if (goodsInfo.number.Length > 50)
                {
                    YMessageBox.show(this, "货号不合法！");
                    return;
                }

                //获取配置文件路径。
                string configFile = AppDomain.CurrentDomain.BaseDirectory.ToString() + "DataBaseConfig.xml";

                //创建操作对象
                GoodsOperater oper = GoodsOperater.createGoodsOperater(configFile, "SQLServer");
                if (oper != null)
                {
                    if (string.IsNullOrEmpty(this.hidGoodsId.Value))
                    {
                        //新增
                        if (oper.createNewGoods(goodsInfo) > 0)
                        {
                            YMessageBox.showAndResponseScript(this, "保存成功！", "window.parent.closePopupsWindow('#popups');", "window.parent.menuButtonOnClick('货物管理','icon-goods','inventory/goods/goods_list.aspx')");
                        }
                        else
                        {
                            YMessageBox.show(this, "创建失败！错误信息：[" + oper.errorMessage + "]");
                            return;
                        }
                    }
                    else
                    {
                        //修改
                        goodsInfo.id = Convert.ToInt32(this.hidGoodsId.Value);
                        if (oper.changeGoods(goodsInfo))
                        {
                            YMessageBox.showAndResponseScript(this, "保存成功！", "window.parent.closePopupsWindow('#popups');", "window.parent.menuButtonOnClick('货物管理','icon-goods','inventory/goods/goods_list.aspx')");
                        }
                        else
                        {
                            YMessageBox.show(this, "修改失败！错误信息：[" + oper.errorMessage + "]");
                            return;
                        }
                    }
                }
                else
                {
                    YMessageBox.show(this, "创建数据库操作对象失败！");
                    return;
                }
            }
            catch (Exception ex)
            {
                YMessageBox.show(this, "程序异常！错误信息[" + ex.Message + "]");
            }
        }
    }
}