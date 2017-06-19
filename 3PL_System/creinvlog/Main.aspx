<%@ Page Title="" Language="C#" AutoEventWireup="true" CodeBehind="Main.aspx.cs" Inherits="產生盤點結果表.Main" %>

<html xmlns="http://www.w3.org/1999/xhtml" lang="zh-tw">
<head runat="server">
    <meta charset="utf-8">
    <meta name="viewport" content="width=device-width, initial-scale=1">

    <link rel="stylesheet" href="../CSS/style.css" type="text/css" />
    <link rel="stylesheet" href="../js/bootstrap-3.3.7-dist/css/bootstrap.css" type="text/css" />
    <link rel="stylesheet" href="../js/jquery-ui-1.12.1/jquery-ui.css" type="text/css" />
    <style>
        .dialog_noTitle .ui-dialog-titlebar {
            Display: none;
        }
    </style>
    <script type="text/javascript" src="../js/jquery-ui-1.12.1/jquery-1.12.4.min.js"></script>
    <script type="text/javascript" src="../js/bootstrap-3.3.7-dist/js/bootstrap.min.js"></script>
    <script type="text/javascript" src="../js/jquery-ui-1.12.1/jquery-ui.min.js"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            //讀取小視窗
            $('#progressbar').progressbar({
                value: false
            });
            $('#divProgress').dialog({
                autoOpen: false,
                modal: true,
                dialogClass: 'dialog_noTitle'
            });
        });
        //讀取小視窗
        function ShowProgressBar() {
            $('#divProgress').dialog('open');
        }
        function HideProgressBar() {
            $('#divProgress').dialog('close');
        }
    </script>
    <title>產生盤點結果表</title>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <table>
            <tr>
                <td>倉別  </td>
                <td>
                    <asp:Label ID="lbl_siteno" runat="server" Text="DC1"></asp:Label></td>
                <td>廠商編號</td>
                <td>
                    <asp:Label ID="lbl_supdid" runat="server" Text="0026"></asp:Label></td>
                <td>日期</td>
                <td>
                    <asp:Label ID="lbl_invdate" runat="server" Text="2015/05/08"></asp:Label></td>
            </tr>
        </table>
        <p></p>
        <asp:Button runat="server" ID="btn_creRcvAdj" Text="產生進貨調整結果" OnClick="btn_creRcvAdj_Click" /><asp:Label ID="lbl_creAdj" runat="server" Text="無資料" ForeColor="Red" Font-Bold="true"></asp:Label>
        <p></p>
        <asp:Button runat="server" ID="btn_InvPaper" Text="產生盤點單結果" OnClick="btn_InvPaper_Click" /><asp:Label ID="lbl_InvPaper" runat="server" Text="無資料" ForeColor="Red" Font-Bold="true"></asp:Label>
        <p></p>
        <asp:Button runat="server" ID="btn_TempArea" Text="產生暫存區結果" OnClick="btn_TempArea_Click" /><asp:Label ID="lbl_TempArea" runat="server" Text="無資料" ForeColor="Red" Font-Bold="true"></asp:Label>
        <p></p>
        <br />
        <br />
        <asp:Button runat="server" ID="btn_CreInvLog" Text="產生盤點結果表" ForeColor="Red" OnClick="btn_CreInvLog_Click" OnClientClick="ShowProgressBar()" />
        <p></p>
        <br />
        <asp:Button runat="server" ID="btn_Pre" Text="上一頁" OnClick="btn_Pre_Click" />
        <%--讀取小視窗--%>
        <div id="divProgress">
            <div class="progress-label">資料處理中...</div>
            <div id="progressbar"></div>
        </div>
    </form>
</body>
</html>
