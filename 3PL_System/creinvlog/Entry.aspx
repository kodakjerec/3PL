<%@ Page Title="" Language="C#" AutoEventWireup="true" CodeBehind="Entry.aspx.cs" Inherits="_3PL_System.creinvlog.Entry" %>

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
    <title>查詢入口</title>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <table>
            <tr>
                <td class="EditTD1">
                    <asp:Label ID="Label2" runat="server" Text="廠商編號"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txb_SupdId" runat="server" Width="88px"></asp:TextBox>
                    <asp:DropDownList ID="DD_SupdId" runat="server" AutoPostBack="True" OnSelectedIndexChanged="DD_SupdId_SelectedIndexChanged"></asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Button ID="btn_Query" runat="server" Text="查詢" OnClick="btn_Query_Click" OnClientClick="ShowProgressBar()" />
                </td>
            </tr>
        </table>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Always">
            <ContentTemplate>
                <div id="div_Date">
                    <asp:GridView ID="GV_Date" runat="server" Width="50%" AutoGenerateColumns="False"
                        CssClass="GVStyle" AllowPaging="True" PageSize="5" OnPageIndexChanging="GV_Date_PageIndexChanging"
                        BorderColor="Gray" OnRowCommand="GV_Date_RowCommand" EnableModelValidation="True">
                        <HeaderStyle CssClass="GVHead" />
                        <RowStyle CssClass="one" />
                        <PagerStyle CssClass="GVPage" />
                        <EmptyDataRowStyle HorizontalAlign="Center" />
                        <Columns>
                            <asp:TemplateField HeaderText="盤點日期">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lbl_linkDate" runat="server" Text='<%# Bind("inv_date", "{0:yyyy/MM/dd}") %>'
                                        CommandName="Select_linkDate" OnClientClick="ShowProgressBar()"></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="倉別">
                                <ItemTemplate>
                                    <asp:Label ID="lbl_siteno" runat="server" Text='<%# Bind("site_no") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="status" HeaderText="關帳狀態" />
                        </Columns>
                    </asp:GridView>
                    <asp:Button ID="btn_gotoMain" runat="server" Text="重新產生盤點結果表" ForeColor="Red" Height="30px" OnClick="btn_gotoMain_Click" Visible="false" />
                </div>
                <asp:HiddenField ID="Select_site_no" Value="" runat="server" />
                <asp:HiddenField ID="Select_inv_date" Value="" runat="server" />
                <div id="div_detail">
                    <asp:GridView ID="GV_Detail" runat="server" Width="50%" AutoGenerateColumns="False"
                        CssClass="GVStyle" AllowPaging="True" PageSize="5" OnPageIndexChanging="GV_Detail_PageIndexChanging"
                        BorderColor="Gray" EnableModelValidation="True">
                        <HeaderStyle CssClass="GVHead" />
                        <RowStyle CssClass="one" />
                        <PagerStyle CssClass="GVPage" />
                        <EmptyDataRowStyle HorizontalAlign="Center" />
                        <Columns>
                            <asp:BoundField DataField="類別" HeaderText="類別" />
                            <asp:BoundField DataField="儲位數量" HeaderText="庫存量(pcs)" />
                        </Columns>
                    </asp:GridView>
                </div>
                <asp:Label ID="lbl_creAdj" runat="server" Text="無資料" ForeColor="Red" Font-Bold="true" Visible="false"></asp:Label>
            </ContentTemplate>
        </asp:UpdatePanel>
        <%--讀取小視窗--%>
        <div id="divProgress">
            <div class="progress-label">資料處理中...</div>
            <div id="progressbar"></div>
        </div>
    </form>
</body>
</html>
