<%@ Page Title="" Language="C#" AutoEventWireup="true" CodeBehind="creManageInvPaper.aspx.cs" Inherits="_3PL_System.creinvlog.creManageInvPaper" %>

<html xmlns="http://www.w3.org/1999/xhtml" lang="zh-tw">
<head runat="server">
    <meta charset="utf-8">
    <meta name="viewport" content="width=device-width, initial-scale=1">
    <style>
        /*jquery dialog 不要抬頭*/
        .dialog_noTitle .ui-dialog-titlebar {
            Display: none;
        }
    </style>
    <link rel="stylesheet" href="../CSS/style.css" type="text/css" />
    <link rel="stylesheet" href="../js/bootstrap-3.3.7-dist/css/bootstrap.css" type="text/css" />
    <link rel="stylesheet" href="../js/jquery-ui-1.12.1/jquery-ui.css" type="text/css" />

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
    <title>產生管理盤點單(暫存區)</title>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <table width="50%">
            <tr>
                <td class="PageTitle" colspan="2">
                    <asp:Label ID="lbl_Quotation" runat="server" Text="產生管理盤點單(暫存區)"></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="EditTD1">倉別  </td>
                <td>
                    <asp:DropDownList ID="DD_SiteNo" runat="server" AutoPostBack="True">
                        <asp:ListItem Value="DC01">觀音</asp:ListItem>
                        <asp:ListItem Value="DC02">岡山</asp:ListItem>
                        <asp:ListItem Value="DC03">台中</asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td class="EditTD1">廠商編號</td>
                <td>
                    <asp:DropDownList ID="DD_SupdId" runat="server" AutoPostBack="true"></asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td class="EditTD1">只做SFC儲位</td>
                <td>
                    <asp:CheckBox ID="chk_OnlySFC" runat="server" Text=" "
                        OnCheckedChanged="chk_OnlySFC_CheckedChanged" Checked="True" />
                </td>
            </tr>
            <tr>
                <td class="EditTD1">
                    <asp:Label ID="Label3" runat="server" Text="新增不列入管理盤點的儲位編號："></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txb_OutSlodId" runat="server"></asp:TextBox>
                    <asp:Button ID="btn_AddOutSlodId" runat="server" Text="新增" OnClick="btn_AddOutSlodId_Click" OnClientClick="ShowProgressBar()" />
                </td>
            </tr>
            <tr>
                <td colspan="2">

                    <asp:GridView ID="GV_OutSlodId" runat="server" Width="100%" AutoGenerateColumns="False"
                        CssClass="GVStyle" BorderColor="Gray" EnableModelValidation="True" OnRowCommand="GV_OutSlodId_RowCommand">
                        <HeaderStyle CssClass="GVHead" />
                        <RowStyle CssClass="one" />
                        <PagerStyle CssClass="GVPage" />
                        <EmptyDataRowStyle HorizontalAlign="Center" />
                        <Columns>
                            <asp:TemplateField HeaderText="動作">
                                <ItemTemplate>
                                    <asp:Button ID="btn_DelOutSlodId" runat="server" Text='刪除 '
                                        CommandName="DelOutSlodId"></asp:Button>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="排除儲位編號">
                                <ItemTemplate>
                                    <asp:Label ID="lbl_OutSlodId" runat="server" Text='<%# Bind("SlodId") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Button ID="btn_Query" runat="server" Text="查詢管理盤模擬結果" OnClick="btn_Query_Click" OnClientClick="ShowProgressBar()" />
                </td>
            </tr>
        </table>
        <asp:UpdatePanel ID="UpdPanel_Assign_HeadList" runat="server" UpdateMode="Always">
            <ContentTemplate>
                <div id="dv_Date" visible="false" runat="server">
                    <p>
                        <asp:Label ID="Label1" runat="server" Text="預跑管理盤點單明細" ForeColor="Blue"></asp:Label>
                    </p>
                    <asp:GridView ID="GV_Date" runat="server" Width="100%" AutoGenerateColumns="False"
                        CssClass="GVStyle" AllowPaging="True" PageSize="10" OnPageIndexChanging="GV_Date_PageIndexChanging"
                        BorderColor="Gray" EnableModelValidation="True">
                        <HeaderStyle CssClass="GVHead" />
                        <RowStyle CssClass="one" />
                        <PagerStyle CssClass="GVPage" />
                        <EmptyDataRowStyle HorizontalAlign="Center" />
                        <Columns>
                            <asp:BoundField DataField="S_slom_slodid" HeaderText="儲位" />
                            <asp:BoundField DataField="S_slom_merdid" HeaderText="貨號" />
                            <asp:BoundField DataField="L_slom_1qty" HeaderText="庫存量(pcs)" />
                            <asp:BoundField DataField="Unit" HeaderText="單位" />
                        </Columns>
                    </asp:GridView>
                    <p>
                        <asp:Label ID="Label2" runat="server" Text="預跑管理盤點單彙總" ForeColor="Blue"></asp:Label>
                    </p>
                    <asp:GridView ID="GV_Date2" runat="server" Width="100%" AutoGenerateColumns="False"
                        CssClass="GVStyle" AllowPaging="True" PageSize="10" OnPageIndexChanging="GV_Date2_PageIndexChanging"
                        BorderColor="Gray" EnableModelValidation="True">
                        <HeaderStyle CssClass="GVHead" />
                        <RowStyle CssClass="one" />
                        <PagerStyle CssClass="GVPage" />
                        <EmptyDataRowStyle HorizontalAlign="Center" />
                        <Columns>
                            <asp:BoundField DataField="S_slom_merdid" HeaderText="貨號" />
                            <asp:BoundField DataField="Tqty" HeaderText="盤盈虧" />
                        </Columns>
                    </asp:GridView>
                </div>
                <asp:Label ID="lbl_creAdj" runat="server" Text="無資料" ForeColor="Red" Font-Bold="true" Visible="false"></asp:Label>
                <p></p>
                <asp:Button runat="server" ID="btn_creInvPaper" Text="產生管理盤點單結果並清除SFC81XX" Enabled="False" OnClick="btn_creInvPaper_Click" OnClientClick="return confirm('確定執行嗎？')" />
                <p></p>
                <asp:Label ID="Lbl_finalPaper" runat="server" Text="無資料" ForeColor="Red" Font-Bold="true" Visible="false"></asp:Label>
            </ContentTemplate>
        </asp:UpdatePanel>
        <p></p>
        <asp:Button runat="server" ID="btn_Reset" Text="重新查詢" OnClick="btn_Reset_Click" OnClientClick="ShowProgressBar()" />
        <%--讀取小視窗--%>
        <div id="divProgress">
            <div class="progress-label">資料處理中...</div>
            <div id="progressbar"></div>
        </div>
    </form>
</body>
</html>
