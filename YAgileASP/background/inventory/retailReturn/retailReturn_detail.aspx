<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="retailReturn_detail.aspx.cs" Inherits="YAgileASP.background.inventory.retailReturn.retailReturn_detail" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>退库单明细</title>
    <meta http-equiv="pragma" content="no-cache" />
    <meta http-equiv="cache-control" content="no-cache" />  
    <meta http-equiv="expires" content="0" />  

    <link href="../../../js/jquery-easyui/themes/icon.css" rel="stylesheet" type="text/css" />
    <link href="../../../js/jquery-easyui/themes/default/easyui.css" rel="stylesheet" type="text/css" />
    <link href="../../../css/table.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        html,body{ height:100%;}
    </style>

    <script type="text/javascript" src="../../../js/jquery/jquery.min.js"></script>
    <script type="text/javascript" src="../../../js/jquery-easyui/jquery.easyui.min.js"></script>
    <script type="text/javascript" src="../../../js/jquery-easyui/locale/easyui-lang-zh_CN.js"></script>
    <script type="text/javascript" src="../../../js/YWindows.js"></script>
    <script type="text/javascript" language="javascript">
        /*!
        * \brief
        * 新增明细。
        * 作者：董帅 创建时间：2012-9-13 22:41:55
        */
        function addDetail()
        {
            window.parent.popupsWindow("#popups", "选择货物", 800, 500, "inventory/retailReturn/chouseGoods.aspx?masterId=<%=this.inv.id %>" + "&warehouseId=" + <%=this.inv.warehouse.id %>, "icon-ok", true, true);
        }

        /*!
        * \brief
        * 修改退库单明细。
        * 作者：董帅 创建时间：2012-9-15 15:14:51
        */
        function editDetail()
        {
            //判断选中
            if ($("input:checked[type='checkbox'][name='chkDetail']").length != 1)
            {
                alert("请选中要编辑的退库单明细，一次只能选择一个！");
                return;
            }

            window.parent.popupsWindow("#popups", "修改明细", 600, 255, "inventory/retailReturn/retailReturn_detail.aspx?masterId=" + $("#hidPutInStorageId").val() + "&id=" + $("input:checked[type='checkbox'][name='chkDetail']").eq(0).val(), "icon-edit", true, true);
        }

        /*!
        * \brief
        * 删除退货单明细。
        * 作者：董帅 创建时间：2012-9-11 22:37:40
        */
        function deleteDetail()
        {
            //判断选中
            if ($("input:checked[type='checkbox'][name='chkDetail']").length > 0)
            {
                return confirm("确认要删除选中的退库单明细？");
            }
            else
            {
                alert("请选中要删除的退库单明细！");
                return false;
            }
        }
    </script>
</head>
<body style="width:100%;margin:0px;background-color:#EEF5FD;">
    <form id="form1" runat="server" class="easyui-layout" style="width:100%;height:100%;margin:0px;background-color:#EEF5FD;">
    <div region="north" border="true" style="height:28px;background-color:#EEF5FD">
        <div style="width:250px;margin-left:auto;margin-top:0px;margin-right:0px">
            <a href="#" id="butAdd" class="easyui-linkbutton" iconCls="icon-add" plain="true" runat="server" onclick="javascript:addDetail();">新增</a>
            <a href="#" id="butEdit" class="easyui-linkbutton" iconCls="icon-edit" plain="true" runat="server" onclick="javascript:editDetail();">修改</a>
            <a href="#" id="butDelete" class="easyui-linkbutton" iconCls="icon-cancel" plain="true" runat="server" onclick="javascript:return deleteDetail();" runat="server" onserverclick="butDeleteItems_Click">删除</a>
            <a href="retrnToStorage_list.aspx" class="easyui-linkbutton" iconCls="icon-back" plain="true">返回</a>
        </div>
        <input type="hidden" id="hidPutInStorageId" name="hidPutInStorageId" runat="server" />
    </div>
    <div id="center" region="center" style="padding:3px;background-color:#EEF5FD">
        <div style="width:1120px;margin:0px auto">
            <table class="admintable" style="width:100%;">
                <tr style="height:30px">
                    <th class="adminth_s2" style="width:80px;text-align:right">编号：</th>
                    <td class="admincls0" style="width:200px">
                        <span id="txtNumber" runat="server"></span>
                    </td>
                    <th class="adminth_s2" style="width:80px;text-align:right">仓库：</th>
                    <td class="admincls0" style="width:200px">
                        <span id="txtWarehouse" runat="server"></span>
                    </td>
                    <th class="adminth_s2" style="width:80px;text-align:right">供应商：</th>
                    <td class="admincls0" style="width:200px">
                        <span id="txtSupplier" runat="server"></span>
                    </td>
                    <th class="adminth_s2" style="width:80px;text-align:right">状态：</th>
                    <td class="admincls0" style="width:200px">
                        <span id="txtState" runat="server"></span>
                    </td>
                </tr>
                <tr style="height:30px">
                    <th class="adminth_s2" style="width:80px;text-align:right">创建时间：</th>
                    <td class="admincls0" style="width:200px">
                        <span id="txtCreateTime" runat="server"></span>
                    </td>
                    <th class="adminth_s2" style="width:80px;text-align:right">创建人：</th>
                    <td class="admincls0" style="width:200px">
                        <span id="txtCreateUser" runat="server"></span>
                    </td>
                    <th class="adminth_s2" style="width:80px;text-align:right">执行时间：</th>
                    <td class="admincls0" style="width:200px">
                        <span id="txtExecuteTime" runat="server"></span>
                    </td>
                    <th class="adminth_s2" style="width:80px;text-align:right">执行人：</th>
                    <td class="admincls0" style="width:200px">
                        <span id="txtExecuteUser" runat="server"></span>
                    </td>
                </tr>
                <tr style="height:30px">
                    <th class="adminth_s2" style="width:80px;text-align:right"></th>
                    <td class="admincls0" style="width:200px">
                    </td>
                    <th class="adminth_s2" style="width:80px;text-align:right"></th>
                    <td class="admincls0" style="width:200px">
                    </td>
                    <th class="adminth_s2" style="width:80px;text-align:right"></th>
                    <td class="admincls0" style="width:200px">
                    </td>
                    <th class="adminth_s2" style="width:80px;text-align:center" colspan="2">
                        <a href="#" id="butExecute" class="easyui-linkbutton" iconCls="icon-play" runat="server" onserverclick="butExecute_Click">执行</a>
                        <a href="#" id="butDustbin" class="easyui-linkbutton" iconCls="icon-dustbin" runat="server" style="display:none">作废</a>
                    </th>
                </tr>
            </table>
            <div class="easyui-panel" title="出库单明细" iconCls="icon-detail" style="width:1120px">
                <table class="admintable" style="width:100%">
                    <tr style="width:100%;height:30px">
                        <th class="adminth" style="width:30px">选择</th>
                        <th class="adminth" style="width:150px">货号</th>
                        <th class="adminth" style="width:150px">名称</th>
                        <th class="adminth" style="width:150px">颜色</th>
                        <th class="adminth" style="width:60px">尺码</th>
                        <th class="adminth" style="width:60px">出库数量</th>
                        <th class="adminth" style="width:60px">退库数量</th>
                        <th class="adminth" style="width:150px">单价</th>
                        <th class="adminth" style="width:150px">总价</th>
                    </tr>
                <asp:Repeater id="repeaterList" runat="server" >
                <ItemTemplate>
                    <tr style="width:100%;height:30px">
                        <td class="admincls0" style="text-align:center;">
                            <%# this.inv.state == 1 ? "<input type=\"checkbox\" value=\"" + Eval("id") + "\" name=\"chkDetail\" />" : ""%>
                        </td>
                        <td class="admincls0" style="text-align:center">
                            <%#Eval("goods.number")%>
                        </td>
                        <td class="admincls0" style="text-align:center">
                            <%#Eval("goods.name")%>
                        </td>
                        <td class="admincls0" style="text-align:center">
                            <%#Eval("color.name")%>
                        </td>
                        <td class="admincls0" style="text-align:center">
                            <%#Eval("size.name")%>
                        </td>
                        <td class="admincls0" style="text-align:center">
                            <%#Eval("surplusCount")%>
                        </td>
                        <td class="admincls0" <%#(Convert.ToInt32(Eval("count")) > Convert.ToInt32(Eval("surplusCount"))) & this.inv.state == 1 ? "style=\"text-align:center;background-color:red\"" : "style=\"text-align:center\""%>>
                            <%#Eval("count")%>
                        </td>
                        <td class="admincls0" style="text-align:center">
                            <%#String.Format("{0:N2}", Convert.ToDouble(Eval("unitPrice")))%>
                        </td>
                        <td class="admincls0" style="text-align:center">
                            <%#String.Format("{0:N2}", Convert.ToDouble(Eval("unitPrice")) * Convert.ToInt32(Eval("count")))%>
                        </td>
                    </tr>
                </ItemTemplate>
                <AlternatingItemTemplate>
                    <tr style="width:100%;height:30px">
                        <td class="admincls1" style="text-align:center;">
                            <%# this.inv.state == 1 ? "<input type=\"checkbox\" value=\"" + Eval("id") + "\" name=\"chkDetail\" />" : ""%>
                        </td>
                        <td class="admincls1" style="text-align:center">
                            <%#Eval("goods.number")%>
                        </td>
                        <td class="admincls1" style="text-align:center">
                            <%#Eval("goods.name")%>
                        </td>
                        <td class="admincls1" style="text-align:center">
                            <%#Eval("color.name")%>
                        </td>
                        <td class="admincls1" style="text-align:center">
                            <%#Eval("size.name")%>
                        </td>
                        <td class="admincls1" style="text-align:center">
                            <%#Eval("surplusCount")%>
                        </td>
                        <td class="admincls1" <%#(Convert.ToInt32(Eval("count")) > Convert.ToInt32(Eval("surplusCount"))) & this.inv.state == 1 ? "style=\"text-align:center;background-color:red\"" : "style=\"text-align:center\""%>">
                            <%#Eval("count")%>
                        </td>
                        <td class="admincls1" style="text-align:center">
                            <%#String.Format("{0:N2}", Convert.ToDouble(Eval("unitPrice")))%>
                        </td>
                         <td class="admincls1" style="text-align:center">
                            <%#String.Format("{0:N2}", Convert.ToDouble(Eval("unitPrice")) * Convert.ToInt32(Eval("count")))%>
                        </td>
                    </tr>
                </AlternatingItemTemplate>
                </asp:Repeater>
                </table>
            </div>
        </div>
    </div>
    </form>
</body>
</html>