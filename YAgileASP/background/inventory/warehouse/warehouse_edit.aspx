﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="warehouse_edit.aspx.cs" Inherits="YAgileASP.background.inventory.warehouse.warehouse_edit" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>编辑仓库</title>
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
        * 验证表单。
        * 作者：董帅 创建时间：2012-8-28 17:26:01
        */
        function checkForms()
        {
            if (!$("#txtWarehouseName").validatebox("isValid"))
            {
                return false;
            }

            if (!$("#txtWarehouseOrder").validatebox("isValid"))
            {
                return false;
            }

            return true;
        }
    </script>
</head>
<body style="width:100%;margin:0px;background-color:#EEF5FD;">
    <form id="form1" runat="server" class="easyui-layout" style="width:100%;height:100%;margin:0px;background-color:#EEF5FD;">
    <div id="center" region="center" style="padding:3px;background-color:#EEF5FD"">
        <input type="hidden" id="hidWarehouseId" name="hidDicId" runat="server" />
        <input type="hidden" id="hidParentId" name="hidParentId" runat="server" />
        <table class="admintable" style="width:100%;">
            <tr style="height:30px">
                <th class="adminth_s2" style="width:120px;text-align:right">名称：</th>
                <td bgcolor="#FFFFFF"><input type="text" id="txtWarehouseName" name="txtWarehouseName" class="easyui-validatebox" required="true" maxlength="50" runat="server" style="width:200px" /></td>
            </tr>
            <tr style="height:30px">
                <th class="adminth_s2" style="width:120px;text-align:right">说明：</th>
                <td bgcolor="#FFFFFF"><input type="text" id="txtWarehouseExplain" name="txtWarehouseExplain" maxlength="200" runat="server" style="width:200px" /></td>
            </tr>
            <tr style="height:30px">
                <th class="adminth_s2" style="width:120px;text-align:right">序号：</th>
                <td bgcolor="#FFFFFF"><input type="text" id="txtWarehouseOrder" name="txtWarehouseOrder" class="easyui-numberbox" required="true" min="0" max="50000" precision="0" value="0" runat="server" style="width:200px" /></td>
            </tr>
        </table>
    </div>
    <div region="south" border="true" style="height:30px;background:#D9E5FD;padding:2px;">
        <div style="width:170px;margin-left:auto;margin-right:5px">
            <a id="A1" href="#" class="easyui-linkbutton" iconCls="icon-save" runat="server" onclick="javascript:return checkForms();"  onserverclick="butSave_Click">保存</a>
            <a href="#" class="easyui-linkbutton" iconCls="icon-cancel" onclick="javascript:window.parent.closePopupsWindow('#popups')">取消</a>
        </div>
    </div>
    </form>
</body>
</html>
