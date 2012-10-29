<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="chouseGoods.aspx.cs" Inherits="YAgileASP.background.inventory.retrnOfGoods.chouseGoods" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>选择货物</title>
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
        * 选择明细。
        * 作者：董帅 创建时间：2012-9-14 17:31:16
        **/
        function chouseDetail()
        {
            //判断选中
            if ($("input:checked[type='checkbox'][name='chkDetail']").length != 1)
            {
                alert("请选择商品，一次只能选择一个！");
                return;
            }

            //页面跳转
            window.parent.popupsWindow("#popups", "新增明细", 600, 255, "inventory/retrnOfGoods/retrnOfGoods_detail_edit.aspx?masterId=" + $("#hidMasterId").val() + "&chouseDetailId=" + $("input:checked[type='checkbox'][name='chkDetail']").eq(0).val(), "icon-add", true, true);
        }
    </script>
</head>
<body style="width:100%;margin:0px;background-color:#EEF5FD;">
    <form id="form1" runat="server" class="easyui-layout" style="width:100%;height:100%;margin:0px;background-color:#EEF5FD;">
    <div id="center" region="center" style="padding:3px;background-color:#EEF5FD"">
        <input type="hidden" id="hidId" name="hidId" runat="server" />
        <input type="hidden" id="hidMasterId" name="hidMasterId" runat="server" />
        <table class="admintable" style="width:100%">
                    <tr style="width:100%;height:30px">
                        <th class="adminth" style="width:30px">选择</th>
                        <th class="adminth" style="width:150px">货号</th>
                        <th class="adminth" style="width:150px">名称</th>
                        <th class="adminth" style="width:150px">颜色</th>
                        <th class="adminth" style="width:60px">尺码</th>
                        <th class="adminth" style="width:60px">库存数量</th>
                    </tr>
                <asp:Repeater id="repeaterList" runat="server" onitemdatabound="detail_ItemDataBound">
                <ItemTemplate>
                    <tr style="width:100%;height:30px">
                        <td class="admincls0" style="text-align:center;">
                            <input type="checkbox" value="<%#Eval("details[0].id")%>" name="chkDetail" />
                        </td>
                        <td class="admincls0" style="text-align:center" rowspan="<%#Eval("details.count")%>">
                            <%#Eval("goods.number")%>
                        </td>
                        <td class="admincls0" style="text-align:center" rowspan="<%#Eval("details.count")%>">
                            <%#Eval("goods.name")%>
                        </td>
                        <td class="admincls0" style="text-align:center">
                            <%#Eval("details[0].color.name")%>
                        </td>
                        <td class="admincls0" style="text-align:center">
                            <%#Eval("details[0].size.name")%>
                        </td>
                        <td class="admincls0" style="text-align:center">
                            <%#Eval("details[0].count")%>
                        </td>
                    </tr>
                    <asp:Repeater id="countsList" runat="server">
                    <ItemTemplate>
                    <tr style="width:100%;height:30px">
                        <td class="admincls0" style="text-align:center;">
                            <input type="checkbox" value="<%#Eval("id")%>" name="chkDetail" />
                        </td>
                        <td class="admincls0" style="text-align:center">
                            <%#Eval("color.name")%>
                        </td>
                        <td class="admincls0" style="text-align:center">
                            <%#Eval("size.name")%>
                        </td>
                        <td class="admincls0" style="text-align:center">
                            <%#Eval("count")%>
                        </td>
                    </tr>
                    </ItemTemplate>
                    </asp:Repeater>
                </ItemTemplate>
                <AlternatingItemTemplate>
                    <tr style="width:100%;height:30px">
                        <td class="admincls1" style="text-align:center;">
                            <input type="checkbox" value="<%#Eval("details[0].id")%>" name="chkDetail" />
                        </td>
                        <td class="admincls1" style="text-align:center" rowspan="<%#Eval("details.count")%>">
                            <%#Eval("goods.number")%>
                        </td>
                        <td class="admincls1" style="text-align:center" rowspan="<%#Eval("details.count")%>">
                            <%#Eval("goods.name")%>
                        </td>
                        <td class="admincls1" style="text-align:center">
                            <%#Eval("details[0].color.name")%>
                        </td>
                        <td class="admincls1" style="text-align:center">
                            <%#Eval("details[0].size.name")%>
                        </td>
                        <td class="admincls1" style="text-align:center">
                            <%#Eval("details[0].count")%>
                        </td>
                    </tr>
                    <asp:Repeater id="countsList" runat="server">
                    <ItemTemplate>
                    <tr style="width:100%;height:30px">
                        <td class="admincls1" style="text-align:center;">
                            <input type="checkbox" value="<%#Eval("id")%>" name="chkDetail" />
                        </td>
                        <td class="admincls1" style="text-align:center">
                            <%#Eval("color.name")%>
                        </td>
                        <td class="admincls1" style="text-align:center">
                            <%#Eval("size.name")%>
                        </td>
                        <td class="admincls1" style="text-align:center">
                            <%#Eval("count")%>
                        </td>
                    </tr>
                    </ItemTemplate>
                    </asp:Repeater>
                </AlternatingItemTemplate>
                </asp:Repeater>
                </table>
    </div>
    <div region="south" border="true" style="height:30px;background:#D9E5FD;padding:2px;">
        <div style="width:170px;margin-left:auto;margin-right:5px">
            <a id="A1" href="#" class="easyui-linkbutton" iconCls="icon-ok" runat="server" onclick="javascript:return chouseDetail();">选择</a>
            <a href="#" class="easyui-linkbutton" iconCls="icon-cancel" onclick="javascript:window.parent.closePopupsWindow('#popups')">取消</a>
        </div>
    </div>
    </form>
</body>
</html>
