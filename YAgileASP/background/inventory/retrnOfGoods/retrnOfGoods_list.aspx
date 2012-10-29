<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="retrnOfGoods_list.aspx.cs" Inherits="YAgileASP.background.inventory.retrnOfGoods.retrnOfGoods_list" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>退货单</title>
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
    <script src="../../../js/My97DatePicker/WdatePicker.js" type="text/javascript"></script>
    <script type="text/javascript" language="javascript">
        /*!
        * \brief
        * 新增退货单。
        * 作者：董帅 创建时间：2012-9-15 22:30:37
        */
        function addRetrnOfGoods()
        {
            window.parent.popupsWindow("#popups", "新增退货单", 600, 165, "inventory/retrnOfGoods/retrnOfGoods_edit.aspx", "icon-add", true, true);
        }

        /*!
        * \brief
        * 修改退货单。
        * 作者：董帅 创建时间：2012-9-15 22:31:54
        */
        function editRetrnOfGoods()
        {
            //判断选中
            if ($("input:checked[type='checkbox'][name='chkInv']").length != 1)
            {
                alert("请选中要编辑的退货单，一次只能选择一个！");
                return;
            }

            window.parent.popupsWindow("#popups", "修改退货单", 600, 165, "inventory/retrnOfGoods/retrnOfGoods_edit.aspx?id=" + $("input:checked[type='checkbox'][name='chkInv']").eq(0).val(), "icon-edit", true, true);
        }

        /*!
        * \brief
        * 删除入库单。
        * 作者：董帅 创建时间：2012-9-15 22:32:43
        */
        function deleteRetrnOfGoods()
        {
            //判断选中
            if ($("input:checked[type='checkbox'][name='chkInv']").length > 0)
            {
                return confirm("确认要删除选中的退货单？");
            }
            else
            {
                alert("请选中要删除的退货单！");
                return false;
            }
        }
    </script>
</head>
<body style="width:100%;margin:0px;background-color:#EEF5FD;">
    <form id="form1" runat="server" class="easyui-layout" style="width:100%;height:100%;margin:0px;background-color:#EEF5FD;">
    <div region="north" border="true" style="height:28px;background-color:#EEF5FD">
        <div style="width:200px;margin-left:auto;margin-top:0px;margin-right:0px">
            <a href="#" class="easyui-linkbutton" iconCls="icon-add" plain="true" onclick="javascript:addRetrnOfGoods();">新增</a>
            <a href="#" class="easyui-linkbutton" iconCls="icon-edit" plain="true" onclick="javascript:editRetrnOfGoods();">修改</a>
            <a id="A1" href="#" class="easyui-linkbutton" iconCls="icon-cancel" plain="true" onclick="javascript:return deleteRetrnOfGoods();" runat="server" onserverclick="butDeleteItems_Click">删除</a>
        </div>
    </div>
    <div id="center" region="center" style="padding:3px;background-color:#EEF5FD"">
    <div class="easyui-layout" style="width:100%;height:100%;margin:0px;background-color:#EEF5FD;">
        <div data-options="region:'north',title:'筛选条件',border:true" style="padding:3px;height:95px;background-color:#EEF5FD">
            <table class="admintable" style="width:100%;">
                <tr style="height:30px">
                    <th class="adminth_s2" style="width:80px;text-align:right">编号：</th>
                    <td class="admincls0">
                        <input type="text" id="txtNumber" name="txtNumber" runat="server" style="width:200px" />
                    </td>
                    <th class="adminth_s2" style="width:80px;text-align:right">仓库：</th>
                    <td class="admincls0">
                        <select id="txtWarehouseName" name="txtWarehouseName" runat="server" style="width:200px">
                        </select>
                    </td>
                    <th class="adminth_s2" style="width:80px;text-align:right">供应商：</th>
                    <td class="admincls0">
                        <select id="selSupplier" name="selSupplier" runat="server" style="width:200px">
                        </select>
                    </td>
                </tr>
                <tr style="height:30px">
                    <th class="adminth_s2" style="width:80px;text-align:right">状态：</th>
                    <td class="admincls0">
                        <select id="selStates" name="selStates" runat="server" style="width:200px">
                        </select>
                    </td>
                    <th class="adminth_s2" style="width:80px;text-align:right">创建时间：</th>
                    <td class="admincls0" colspan="2">
                        <input id="txtCreateBegin" class="Wdate" type="text" runat="server" onFocus="WdatePicker({maxDate:'#F{$dp.$D(\'txtCreateEnd\')}',readOnly:true,dateFmt:'yyyy-MM-dd HH:mm:ss'})"/>-<input id="txtCreateEnd" class="Wdate" type="text" runat="server" onFocus="WdatePicker({minDate:'#F{$dp.$D(\'txtCreateBegin\')}',dateFmt:'yyyy-MM-dd HH:mm:ss',readOnly:true})"/>
                    </td>
                    <td class="adminth_s2" style="text-align:center">
                        <a href="#" class="easyui-linkbutton" iconCls="icon-search" runat="server" onserverclick="butSearch_Click">筛选</a>
                    </td>
                </tr>
            </table>
        </div>
        <div id="Div1" data-options="region:'center',border:true" style="padding:3px;background-color:#EEF5FD"">
            <input type="hidden" id="hidParentId" name="hidParentId" runat="server" />
            <input type="hidden" id="hidReturnId" name="hidReturnId" runat="server" />
            <div style="width:1120px;margin:0px auto">
                <table class="admintable">
                    <tr style="width:100%;height:30px">
                        <th class="adminth" style="width:40px">选择</th>
                        <th class="adminth" style="width:150px">编号</th>
                        <th class="adminth" style="width:150px">仓库</th>
                        <th class="adminth" style="width:150px">供应商</th>
                        <th class="adminth" style="width:60px">状态</th>
                        <th class="adminth" style="width:200px">创建时间</th>
                        <th class="adminth" style="width:60px">创建人</th>
                        <th class="adminth" style="width:200px">执行时间</th>
                        <th class="adminth" style="width:60px">执行人</th>
                        <th class="adminth" style="width:60px">明细</th>
                    </tr>
                <asp:Repeater id="repeaterList" runat="server">
                <ItemTemplate>
                    <tr style="width:100%;height:30px">
                        <td class="admincls0" style="text-align:center;">
                            <%#Convert.ToInt32(Eval("state")) == 1 ? "<input type=\"checkbox\" value=\"" + Eval("id") + "\" name=\"chkInv\" />" : ""%>
                        </td>
                        <td class="admincls0" style="text-align:center">
                            <%#Eval("number")%>
                        </td>
                        <td class="admincls0" style="text-align:center">
                            <%#Eval("warehouse.name")%>
                        </td>
                        <td class="admincls0" style="text-align:center">
                            <%#Eval("supplierAndClient.name")%>
                        </td>
                        <td class="admincls0" style="text-align:center">
                            <%#Eval("stateText")%>
                        </td>
                        <td class="admincls0" style="text-align:center">
                            <%#Convert.ToDateTime(Eval("createTime")).ToString("yyyy年MM月dd日 HH:mm:ss")%>
                        </td>
                        <td class="admincls0" style="text-align:center">
                            <%#Eval("createUser.name")%>
                        </td>
                        <td class="admincls0" style="text-align:center">
                            <%#Eval("executeTime") == null ? "" : Convert.ToDateTime(Eval("executeTime")).ToString("yyyy年MM月dd日 HH:mm:ss")%>
                        </td>
                        <td class="admincls0" style="text-align:center">
                            <%#Eval("executeUser.name")%>
                        </td>
                        <td class="admincls0" style="text-align:center">
                            <a href="retrnOfGoods_detail.aspx?id=<%#Eval("id")%>" class="easyui-linkbutton" iconCls="icon-detail" plain="true" onclick="">明细</a>
                        </td>
                    </tr>
                </ItemTemplate>
                <AlternatingItemTemplate>
                    <tr style="width:100%;height:30px">
                        <td class="admincls1" style="text-align:center;">
                            <%#Convert.ToInt32(Eval("state")) == 1 ? "<input type=\"checkbox\" value=\"" + Eval("id") + "\" name=\"chkInv\" />" : ""%>
                        </td>
                        <td class="admincls1" style="text-align:center">
                            <%#Eval("number")%>
                        </td>
                        <td class="admincls1" style="text-align:center">
                            <%#Eval("warehouse.name")%>
                        </td>
                        <td class="admincls1" style="text-align:center">
                            <%#Eval("supplierAndClient.name")%>
                        </td>
                        <td class="admincls1" style="text-align:center">
                            <%#Eval("stateText")%>
                        </td>
                        <td class="admincls1" style="text-align:center">
                            <%#Convert.ToDateTime(Eval("createTime")).ToString("yyyy年MM月dd日 HH:mm:ss")%>
                        </td>
                        <td class="admincls1" style="text-align:center">
                            <%#Eval("createUser.name")%>
                        </td>
                        <td class="admincls1" style="text-align:center">
                            <%#Eval("executeTime") == null ? "" : Convert.ToDateTime(Eval("executeTime")).ToString("yyyy年MM月dd日 HH:mm:ss")%>
                        </td>
                        <td class="admincls1" style="text-align:center">
                            <%#Eval("executeUser.name")%>
                        </td>
                        <td class="admincls1" style="text-align:center">
                            <a href="retrnOfGoods_detail.aspx?id=<%#Eval("id")%>" class="easyui-linkbutton" iconCls="icon-detail" plain="true" onclick="">明细</a>
                        </td>
                    </tr>
                </AlternatingItemTemplate>
                </asp:Repeater>
                </table>
            </div>
        </div>
    </div>
    
    
    </div>
    </form>
</body>
</html>