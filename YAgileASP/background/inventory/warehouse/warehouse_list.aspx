<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="warehouse_list.aspx.cs" Inherits="YAgileASP.inventory.warehouse.warehouse_list" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>仓库管理</title>
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
        * 新增仓库。
        * 作者：董帅 创建时间：2012-8-29 23:03:29
        */
        function addWarehouse()
        {
            window.parent.popupsWindow("#popups", "新增仓库", 600, 165, "inventory/warehouse/warehouse_edit.aspx?parentId=" + $("#hidParentId").val(), "icon-add", true, true);
        }

        /*!
        * \brief
        * 修改仓库。
        * 作者：董帅 创建时间：2012-8-30 13:18:11
        */
        function editWarehouse()
        {
            //判断选中
            if ($("input:checked[type='checkbox'][name='chkWare']").length != 1)
            {
                alert("请选中要编辑的仓库，一次只能选择一个！");
                return;
            }

            window.parent.popupsWindow("#popups", "修改仓库", 600, 165, "inventory/warehouse/warehouse_edit.aspx?parentId=" + $("#hidParentId").val() + "&id=" + $("input:checked[type='checkbox'][name='chkWare']").eq(0).val(), "icon-edit", true, true);
        }

        /*!
        * \brief
        * 删除仓库。
        * 作者：董帅 创建时间：2012-8-30 13:36:03
        */
        function deleteWarehouses()
        {
            //判断选中
            if ($("input:checked[type='checkbox'][name='chkWare']").length > 0)
            {
                return confirm("确认要删除选中的仓库？删除时连同子仓库一并删除。");
            }
            else
            {
                alert("请选中要删除的仓库！");
                return false;
            }
        }

        /*!
        * \brief 
        * 返回上级仓库.
        * 作者：董帅 创建时间：2012-8-30 13:08:25
        */

        function returnParent()
        {
            window.location.href = 'warehouse_list.aspx?parentId=' + $("#hidReturnId").val();
        }

        /*!
        * \brief
        * 设置仓库管理员。
        * 作者：董帅 创建时间：2012-8-20 21:30:28
        */
        function userSet()
        {
            //判断选中
            if ($("input:checked[type='checkbox'][name='chkWare']").length != 1)
            {
                alert("请选中要设置的仓库，一次只能选择一个！");
                return;
            }

            //打开编辑页面
            window.parent.popupsWindow("#popups", "选择仓库管理员", 350, 500, "inventory/warehouse/chouseUsers.aspx?id=" + $("input:checked[type='checkbox'][name='chkWare']").eq(0).val(), "icon-user", true, true);
        }
    </script>
</head>
<body style="width:100%;margin:0px;background-color:#EEF5FD;">
    <form id="form1" runat="server" class="easyui-layout" style="width:100%;height:100%;margin:0px;background-color:#EEF5FD;">
    <div region="north" border="true" style="height:28px;background-color:#EEF5FD">
        <div style="width:380px;margin-left:auto;margin-top:0px;margin-right:0px">
            <a id="A2" href="#" class="easyui-linkbutton" iconCls="icon-user" plain="true" onclick="javascript:userSet();">仓库管理员</a>
            <a href="#" class="easyui-linkbutton" iconCls="icon-add" plain="true" onclick="javascript:addWarehouse();">新增</a>
            <a href="#" class="easyui-linkbutton" iconCls="icon-edit" plain="true" onclick="javascript:editWarehouse();">修改</a>
            <a id="A1" href="#" class="easyui-linkbutton" iconCls="icon-cancel" plain="true" onclick="javascript:return deleteWarehouses();" runat="server" onserverclick="butDeleteItems_Click">删除</a>
        </div>
    </div>
    <div id="center" region="center" style="padding:3px;background-color:#EEF5FD"">
    
    <input type="hidden" id="hidParentId" name="hidParentId" runat="server" />
    <input type="hidden" id="hidReturnId" name="hidReturnId" runat="server" />
    <div style="width:600px;height:28px;margin:0px auto">
        <a id="returnButton" href="#" class="easyui-linkbutton" iconCls="icon-back" plain="true" onclick="javascript:returnParent();" runat="server">返回</a>
        <span>当前仓库：</span>
        <span id="spanParentName" runat="server" style="font-size:16px;font-weight:bold"></span>
    </div>
    <div style="width:600px;margin:0px auto">
        <table class="admintable" style="width:100%">
            <tr style="width:100%;height:30px">
                <th class="adminth" style="width:30px">选择</th>
                <th class="adminth" style="width:auto">名称</th>
                <th class="adminth" style="width:150px">说明</th>
                <th class="adminth" style="width:30px">序号</th>
            </tr>
        <asp:Repeater ID="waresList" runat="server">
        <ItemTemplate>
            <tr style="width:100%;height:30px">
                <td class="admincls0" style="text-align:center;"><input type="checkbox" value="<%#Eval("ID") %>" name="chkWare" /></td>
                <td class="admincls0" style="width:auto">
                    <a href="warehouse_list.aspx?parentId=<%#Eval("ID") %>" class="easyui-linkbutton" id="<%#Eval("ID") %>" plain="true" style="width:370px" ><%#Eval("NAME")%></a>
                </td>
                <td class="admincls0" style="text-align:center">
                    <%#Eval("explain")%>
                </td>
                <td class="admincls0" style="text-align:center">
                    <%#Eval("order") %>
                </td>
            </tr>
        </ItemTemplate>
        <AlternatingItemTemplate>
            <tr style="width:100%;height:30px">
                <td class="admincls1" style="text-align:center;"><input type="checkbox" value="<%#Eval("ID") %>" name="chkWare" /></td>
                <td class="admincls1" style="width:auto">
                    <a href="warehouse_list.aspx?parentId=<%#Eval("ID") %>" class="easyui-linkbutton" id="<%#Eval("ID") %>" plain="true" style="width:370px" ><%#Eval("NAME")%></a>
                </td>
                <td class="admincls1" style="text-align:center">
                    <%#Eval("explain")%>
                </td>
                <td class="admincls1" style="text-align:center">
                    <%#Eval("order") %>
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
