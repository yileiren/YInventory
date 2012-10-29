<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="chouseUsers.aspx.cs" Inherits="YAgileASP.background.inventory.warehouse.chouseUsers" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>选择仓库管理员</title>
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
</head>
<body style="width:100%;margin:0px;background-color:#EEF5FD;">
    <form id="form1" runat="server" class="easyui-layout" style="width:100%;height:100%;margin:0px;background-color:#EEF5FD;">
    <div id="center" region="center" style="padding:3px;background-color:#EEF5FD"">
        <input type="hidden" id="hidWareId" name="hidUserId" runat="server" />
        <div>
            <asp:Repeater ID="userRepeater" runat="server">
                <HeaderTemplate>
                    <table class="admintable" style="width:100%">
                    <tr style="width:100%;height:30px">
                        <th class="adminth" style="width:30px">选择</th>
                        <th class="adminth" style="width:100px">姓名</th>
                        <th class="adminth" style="width:100px">登陆名</th>
                    </tr>
                </HeaderTemplate>
                <ItemTemplate>
                    <tr style="width:100%;height:30px">
                        <td class="admincls0" style="text-align:center">
                            <input type="checkbox" name="chkUser" "<%#Eval("choused").ToString() == "True" ? " checked=\"checked\" " : "" %>" value="<%#Eval("id") %>"/>
                        </td>
                        <td class="admincls0" style="text-align:center"><%#Eval("name") %></td>
                        <td class="admincls0" style="text-align:center"><%#Eval("logName") %></td>
                    </tr>
                </ItemTemplate>
                <AlternatingItemTemplate>
                    <tr style="width:100%;height:30px">
                        <td class="admincls1" style="text-align:center">
                            <input type="checkbox" name="chkUser" "<%#Eval("choused").ToString() == "True" ? " checked=\"checked\" " : "" %>" value="<%#Eval("id") %>"/>
                        </td>
                        <td class="admincls1" style="text-align:center"><%#Eval("name") %></td>
                        <td class="admincls1" style="text-align:center"><%#Eval("logName")%></td>
                    </tr>
                </AlternatingItemTemplate>
                <FooterTemplate>
                    </table>
                </FooterTemplate>
            </asp:Repeater>
        </div>
    </div>
    <div region="south" border="true" style="height:30px;background:#D9E5FD;padding:2px;">
        <div style="width:170px;margin-left:auto;margin-right:5px">
            <a id="A1" href="#" class="easyui-linkbutton" iconCls="icon-ok" runat="server" onclick="javascript:return checkForms();" onserverclick="butChouse_Click">选择</a>
            <a href="#" class="easyui-linkbutton" iconCls="icon-cancel" onclick="javascript:window.parent.closePopupsWindow('#popups')">取消</a>
        </div>
    </div>
    </form>
</body>
</html>
