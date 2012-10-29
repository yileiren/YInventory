<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="supplierAndClient_list.aspx.cs" Inherits="YAgileASP.background.inventory.supplierAndClient.supplierAndClient_list" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>客户与供应商管理</title>
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
        * 新增客户或供应商。
        * 作者：董帅 创建时间：2012-9-2 9:22:54
        */
        function addSupplierAndClient()
        {
            window.parent.popupsWindow("#popups", "新增客户或供应商", 600, 135, "inventory/supplierAndClient/supplierAndClient_edit.aspx", "icon-add", true, true);
        }

        /*!
        * \brief
        * 修改客户或供应商。
        * 作者：董帅 创建时间：2012-9-2 9:47:10
        */
        function editSupplierAndClient()
        {
            //判断选中
            if ($("input:checked[type='checkbox'][name='chkSOrC']").length != 1)
            {
                alert("请选中要编辑的客户或供应商，一次只能选择一个！");
                return;
            }

            window.parent.popupsWindow("#popups", "修改客户或供应商", 600, 135, "inventory/supplierAndClient/supplierAndClient_edit.aspx?id=" + $("input:checked[type='checkbox'][name='chkSOrC']").eq(0).val(), "icon-edit", true, true);
        }

        /*!
        * \brief
        * 删除仓库。
        * 作者：董帅 创建时间：2012-9-2 10:04:33
        */
        function deleteSupplierAndClients()
        {
            //判断选中
            if ($("input:checked[type='checkbox'][name='chkSOrC']").length > 0)
            {
                return confirm("确认要删除选中的客户和供应商？");
            }
            else
            {
                alert("请选中要删除的客户或供应商！");
                return false;
            }
        }
    </script>
</head>
<body style="width:100%;margin:0px;background-color:#EEF5FD;">
    <form id="form1" runat="server" class="easyui-layout" style="width:100%;height:100%;margin:0px;background-color:#EEF5FD;">
    <div region="north" border="true" style="height:28px;background-color:#EEF5FD">
        <div style="width:200px;margin-left:auto;margin-top:0px;margin-right:0px">
            <a href="#" class="easyui-linkbutton" iconCls="icon-add" plain="true" onclick="javascript:addSupplierAndClient();">新增</a>
            <a href="#" class="easyui-linkbutton" iconCls="icon-edit" plain="true" onclick="javascript:editSupplierAndClient();">修改</a>
            <a id="A1" href="#" class="easyui-linkbutton" iconCls="icon-cancel" plain="true" onclick="javascript:return deleteSupplierAndClients();" runat="server" onserverclick="butDeleteItems_Click">删除</a>
        </div>
    </div>
    <div id="center" region="center" style="padding:3px;background-color:#EEF5FD"">
        <div style="width:600px;margin:0px auto">
            <table class="admintable" style="width:100%">
                <tr style="width:100%;height:30px">
                    <th class="adminth" style="width:30px">选择</th>
                    <th class="adminth" style="width:auto">名称</th>
                    <th class="adminth" style="width:150px">编号</th>
                </tr>
            <asp:Repeater ID="waresList" runat="server">
            <ItemTemplate>
                <tr style="width:100%;height:30px">
                    <td class="admincls0" style="text-align:center;"><input type="checkbox" value="<%#Eval("id") %>" name="chkSOrC" /></td>
                    <td class="admincls0" style="width:auto">
                        <%#Eval("name")%>
                    </td>
                    <td class="admincls0" style="text-align:center">
                        <%#Eval("number")%>
                    </td>
                </tr>
            </ItemTemplate>
            <AlternatingItemTemplate>
                <tr style="width:100%;height:30px">
                    <td class="admincls1" style="text-align:center;"><input type="checkbox" value="<%#Eval("id") %>" name="chkSOrC" /></td>
                    <td class="admincls1" style="width:auto">
                        <%#Eval("name")%>
                    </td>
                    <td class="admincls1" style="text-align:center">
                        <%#Eval("number")%>
                    </td>
                </tr>
            </AlternatingItemTemplate>
            </asp:Repeater>
            </table>
        </div>
    </div>
    </form>
</body>
</html>
