using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using YLR.YMessage;
using YLR.YInventory.Warehouse;

namespace YAgileASP.background.inventory.warehouse
{
    public partial class chouseUsers : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                try
                {
                    //获取仓库id
                    string userId = Request.QueryString["id"];
                    if (!string.IsNullOrEmpty(userId))
                    {
                        this.hidWareId.Value = userId;
                    }
                    else
                    {
                        YMessageBox.show(this, "获取仓库id失败！");
                        return;
                    }

                    this.bindData();
                }
                catch (Exception ex)
                {
                    YMessageBox.show(this, "系统运行异常！异常信息[" + ex.Message + "]");
                }

            }
        }

        /// <summary>
        /// 绑定数据。
        /// 作者：董帅 创建时间：2012-8-30 21:50:40
        /// </summary>
        private void bindData()
        {
            try
            {
                //获取配置文件路径。
                string configFile = AppDomain.CurrentDomain.BaseDirectory.ToString() + "DataBaseConfig.xml";

                //创建数据库操作对象。
                WarehouseOperater wareOper = WarehouseOperater.createWarehouseOperater(configFile, "SQLServer");
                if (wareOper != null)
                {
                    List<WarehouseAdmin> admins = wareOper.getChouseUser(Convert.ToInt32(this.hidWareId.Value));
                    if (admins != null)
                    {
                        this.userRepeater.DataSource = admins;
                        this.userRepeater.DataBind();
                    }
                    else
                    {
                        YMessageBox.show(this, "查询出错！错误信息：[" + wareOper.errorMessage + "]");
                    }
                }
            }
            catch (Exception ex)
            {
                YMessageBox.show(this, "查询出错！错误信息：[" + ex.Message + "]");
            }
        }

        /// <summary>
        /// 选择用户保存方法。
        /// 作者：董帅 创建时间：2012-8-30 22:21:43
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void butChouse_Click(object sender, EventArgs e)
        {
            try
            {
                string strIds = Request["chkUser"];
                string[] ids = new string[0];
                if (!string.IsNullOrEmpty(strIds))
                {
                    ids = strIds.Split(',');
                }

                //获取配置文件路径。
                string configFile = AppDomain.CurrentDomain.BaseDirectory.ToString() + "DataBaseConfig.xml";

                //创建数据库操作对象。
                WarehouseOperater wareOper = WarehouseOperater.createWarehouseOperater(configFile, "SQLServer");
                if (wareOper != null)
                {

                    //删除数据
                    int[] intIds = new int[ids.Length];
                    for (int i = 0; i < intIds.Length; i++)
                    {
                        intIds[i] = Convert.ToInt32(ids[i]);
                    }

                    if (wareOper.chouseWarehouseUser(Convert.ToInt32(this.hidWareId.Value), intIds))
                    {
                        YMessageBox.showAndResponseScript(this, "选择用户成功！", "window.parent.closePopupsWindow('#popups');", "");
                    }
                    else
                    {
                        YMessageBox.show(this, "选择角色失败！错误信息[" + wareOper.errorMessage + "]");
                    }
                }
                else
                {
                    YMessageBox.show(this, "获取数据库实例失败！");
                }
            }
            catch (Exception ex)
            {
                YMessageBox.show(this, "系统运行异常！异常信息[" + ex.Message + "]");
            }
        }
    }
}