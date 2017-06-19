<%@ Page Title="" Language="C#" AutoEventWireup="true" CodeBehind="creRcvAdj.aspx.cs" Inherits="_3PL_System.creinvlog.creRcvAdj" %>

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
    <title>產生進貨調整結果</title>
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
        <br />
        <asp:GridView ID="GV_Date" runat="server" Width="100%" AutoGenerateColumns="False"
            CssClass="GVStyle"
            BorderColor="Gray" EnableModelValidation="True">
            <HeaderStyle CssClass="GVHead" />
            <RowStyle CssClass="one" />
            <PagerStyle CssClass="GVPage" />
            <EmptyDataRowStyle HorizontalAlign="Center" />
            <Columns>
                <asp:BoundField DataField="item_no" HeaderText="貨號" />
                <asp:BoundField DataField="adj_qty" HeaderText="調整量" />
            </Columns>
        </asp:GridView>
        <asp:Label ID="lbl_creAdj" runat="server" Text="無資料" ForeColor="Red" Font-Bold="true" Visible="false"></asp:Label>
        <br />
        <asp:Button runat="server" ID="btn_creRcvAdj" Text="產生進貨調整" OnClick="btn_creRcvAdj_Click" OnClientClick="ShowProgressBar()" />
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
