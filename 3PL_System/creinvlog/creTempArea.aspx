<%@ Page Title="" Language="C#" AutoEventWireup="true" CodeBehind="creTempArea.aspx.cs" Inherits="_3PL_System.creinvlog.creTempArea" %>

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
    <title>產生暫存區結果</title>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <table>
            <tr>
                <td>倉別  </td>
                <td>
                    <asp:Label ID="lbl_siteno" runat="server" Text="DC1"></asp:Label>
                </td>
                <td>廠商編號</td>
                <td>
                    <asp:Label ID="lbl_supdid" runat="server" Text="0026"></asp:Label>
                </td>
                <td>日期</td>
                <td>
                    <asp:Label ID="lbl_invdate" runat="server" Text="2015/05/08"></asp:Label>
                </td>
            </tr>
        </table>
        <p></p>
        <asp:GridView ID="GV_Date" runat="server" Width="100%" AutoGenerateColumns="False"
            CssClass="GVStyle" AllowPaging="True" PageSize="5" OnPageIndexChanging="GV_Date_PageIndexChanging"
            BorderColor="Gray" EnableModelValidation="True" OnRowCommand="GV_Date_RowCommand">
            <HeaderStyle CssClass="GVHead" />
            <RowStyle CssClass="one" />
            <PagerStyle CssClass="GVPage" />
            <EmptyDataRowStyle HorizontalAlign="Center" />
            <Columns>
                <asp:TemplateField HeaderText="批次">
                    <ItemTemplate>
                        <asp:LinkButton ID="lbl_linkbatch" runat="server" Text='<%# Bind("batch") %>'
                            CommandName="Select_linkbatch"></asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="統計" HeaderText="統計" />
            </Columns>
        </asp:GridView>
        <asp:HiddenField ID="Select_PaperNo" Value="" runat="server" />
        <p></p>
        <asp:GridView ID="GV_Detail" runat="server" Width="100%" AutoGenerateColumns="False"
            CssClass="GVStyle" AllowPaging="True" PageSize="10" OnPageIndexChanging="GV_Date_PageIndexChanging"
            BorderColor="Gray" EnableModelValidation="True">
            <HeaderStyle CssClass="GVHead" />
            <RowStyle CssClass="one" />
            <PagerStyle CssClass="GVPage" />
            <EmptyDataRowStyle HorizontalAlign="Center" />
            <Columns>
                <asp:BoundField DataField="S_slom_merdid" HeaderText="貨號" />
                <asp:BoundField DataField="S_slom_slodid" HeaderText="儲位" />
                <asp:BoundField DataField="Tqty" HeaderText="庫存量" />
                <asp:BoundField DataField="N_merd_name" HeaderText="品名規格" />
            </Columns>
        </asp:GridView>
        <asp:Label ID="lbl_creAdj" runat="server" Text="無資料" ForeColor="Red" Font-Bold="true" Visible="false"></asp:Label>
        <p></p>
        <asp:Button runat="server" ID="btn_creTempArea" Text="產生暫存區結果" OnClick="btn_creTempArea_Click" OnClientClick="ShowProgressBar()" />
        <p></p>
        <asp:Button runat="server" ID="btn_Pre" Text="上一頁" OnClick="btn_Pre_Click" />
        <%--讀取小視窗--%>
        <div id="divProgress">
            <div class="progress-label">資料處理中...</div>
            <div id="progressbar"></div>
        </div>
    </form>
</body>
</html>
