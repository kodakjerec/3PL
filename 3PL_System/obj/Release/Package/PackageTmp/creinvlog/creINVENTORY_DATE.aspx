<%@ Page Language="C#" AutoEventWireup="true"
    CodeBehind="creINVENTORY_DATE.aspx.cs" Inherits="_3PL_System.creinvlog.creINVENTORY_DATE" %>

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
    <script type="text/javascript">
        function pageLoad(sender, args) {
            $(document).ready(function () {
                $("#<%=txb_InvDate.ClientID %>").datepicker({
                    dateFormat: 'yy/mm/dd',
                    onSelect: function () { $("#<%=txb_InvDate.ClientID %>").trigger('change'); }
                });
                $("#<%=txb_Back_S.ClientID %>").datepicker({ dateFormat: 'yy/mm/dd' });
                $("#<%=txb_Back_E.ClientID %>").datepicker({ dateFormat: 'yy/mm/dd' });
                $("#<%=txb_In_S.ClientID %>").datepicker({ dateFormat: 'yy/mm/dd' });
                $("#<%=txb_In_E.ClientID %>").datepicker({ dateFormat: 'yy/mm/dd' });
                $("#<%=txb_InDC_S.ClientID %>").datepicker({ dateFormat: 'yy/mm/dd' });
                $("#<%=txb_InDC_S.ClientID %>").datepicker({ dateFormat: 'yy/mm/dd' });
                $("#<%=txb_Get_S.ClientID %>").datepicker({ dateFormat: 'yy/mm/dd' });
                $("#<%=txb_Get_E.ClientID %>").datepicker({ dateFormat: 'yy/mm/dd' });
                $("#<%=txb_Out_S.ClientID %>").datepicker({ dateFormat: 'yy/mm/dd' });
                $("#<%=txb_Out_E.ClientID %>").datepicker({ dateFormat: 'yy/mm/dd' });
                $("#<%=txb_Adj_S.ClientID %>").datepicker({ dateFormat: 'yy/mm/dd' });
                $("#<%=txb_Adj_E.ClientID %>").datepicker({ dateFormat: 'yy/mm/dd' });
                $("#<%=txb_StoreAdj_S.ClientID %>").datepicker({ dateFormat: 'yy/mm/dd' });
                $("#<%=txb_StoreAdj_E.ClientID %>").datepicker({ dateFormat: 'yy/mm/dd' });
                $("#<%=txb_PREORDER_S.ClientID %>").datepicker({ dateFormat: 'yy/mm/dd' });
                $("#<%=txb_PREORDER_E.ClientID %>").datepicker({ dateFormat: 'yy/mm/dd' });
            });
        }
    </script>

    <style type="text/css">
        .bound2 {
            font-size: 80%;
            text-decoration: none;
            text-align: center;
            color: blue;
            background-color: #99CCFF;
            -webkit-border-radious: 2px;
            -moz-border-radious: 2px;
            border-radius: 2px;
        }

            .bound2:hover {
                cursor: pointer;
                color: red;
            }
    </style>
    <title>產生物流廠商盤點日期</title>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Always">
            <ContentTemplate>
                <div style="text-align: center">
                    <table class="tborder" align="center">
                        <tr>
                            <td>
                                <asp:Label runat="server" ID="Cre_ErrMsg" Style="color: Red"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Button runat="server" ID="btn_Show_div_Create" Text="關閉新增小視窗" OnClick="btn_Show_div_Create_Click" />
                            </td>
                        </tr>
                    </table>
                </div>
                <div runat="server" id="div_Create">
                    <table class="tborder" align="center">
                        <tr>
                            <td class="PageTitle" colspan="2">產生物流廠商盤點日期
                            </td>
                        </tr>
                        <tr>
                            <td class="EditTD1">
                                <label style="color: Red">
                                    1</label>倉別
                            </td>
                            <td>
                                <asp:DropDownList ID="DD_SiteNo" runat="server" AutoPostBack="True">
                                    <asp:ListItem Value="DC">全部</asp:ListItem>
                                    <asp:ListItem Value="DC1">觀音</asp:ListItem>
                                    <asp:ListItem Value="DC2">岡山</asp:ListItem>
                                    <asp:ListItem Value="DC3">梧棲</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td class="EditTD1">
                                <label style="color: Red">
                                    2</label>
                                廠商編號
                            </td>
                            <td>
                                <asp:TextBox ID="txb_Vendor_no" runat="server" MaxLength="4"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="EditTD1">
                                <label style="color: Red">
                                    3</label>
                                盤點日期
                            </td>
                            <td>
                                <asp:TextBox ID="txb_InvDate" runat="server" OnTextChanged="txb_InvDate_TextChanged"
                                    AutoPostBack="True"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="EditTD1">限制門市登打退貨日期
                            </td>
                            <td>
                                <asp:TextBox ID="txb_Back_S" runat="server"></asp:TextBox>至<asp:TextBox ID="txb_Back_E"
                                    runat="server"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="EditTD1">限制物流進貨日期(XD)
                            </td>
                            <td>
                                <asp:TextBox ID="txb_In_S" runat="server"></asp:TextBox>至<asp:TextBox ID="txb_In_E"
                                    runat="server"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="EditTD1">限制物流進貨日期(DC)
                            </td>
                            <td>
                                <asp:TextBox ID="txb_InDC_S" runat="server"></asp:TextBox>至<asp:TextBox ID="txb_InDC_E"
                                    runat="server"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="EditTD1">限制物流揀貨日期
                            </td>
                            <td>
                                <asp:TextBox ID="txb_Get_S" runat="server"></asp:TextBox>至<asp:TextBox ID="txb_Get_E"
                                    runat="server"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="EditTD1">限制物流出貨日期
                            </td>
                            <td>
                                <asp:TextBox ID="txb_Out_S" runat="server"></asp:TextBox>至<asp:TextBox ID="txb_Out_E"
                                    runat="server"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="EditTD1">限制物流調撥日期
                            </td>
                            <td>
                                <asp:TextBox ID="txb_Adj_S" runat="server"></asp:TextBox>至<asp:TextBox ID="txb_Adj_E"
                                    runat="server"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="EditTD1">限制門市調撥日期
                            </td>
                            <td>
                                <asp:TextBox ID="txb_StoreAdj_S" runat="server"></asp:TextBox>至<asp:TextBox ID="txb_StoreAdj_E"
                                    runat="server"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="EditTD1">預購停止日
                            </td>
                            <td>
                                <asp:TextBox ID="txb_PREORDER_S" runat="server"></asp:TextBox>至<asp:TextBox ID="txb_PREORDER_E"
                                    runat="server"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" style="text-align: center">
                                <asp:Button runat="server" ID="btn_Cfm_Create" Text="確定新增" OnClick="btn_Cfm_Create_Click" />
                            </td>
                        </tr>
                    </table>
                </div>
                <div id="div_Inventory_Date_list" runat="server">
                    <table class="tborder" width="100%">
                        <tr>
                            <td align="left">
                                <asp:Button ID="btn_Del" runat="server" Text="刪除本頁選擇項目" OnClick="btn_Del_Click" OnClientClick="return confirm('確定刪除本頁選擇項目嗎？');" />
                                <asp:Button ID="btn_ToExcel" runat="server" Text="匯出Excel" OnClick="btn_ToExcel_Click" />
                            </td>
                            <td align="right">可查詢：盤點月份,倉別,廠商編號
                            <asp:TextBox ID="txb_Search" runat="server" placeholder="請輸入查詢內容" />
                                <asp:Button ID="btn_Search" runat="server" Text="搜尋" OnClick="btn_Search_Click" />
                            </td>
                        </tr>
                    </table>
                    <asp:GridView ID="GV_Date" runat="server" Width="100%" AutoGenerateColumns="False"
                        CssClass="GVStyle" AllowPaging="True" PageSize="10" OnPageIndexChanging="GV_Date_PageIndexChanging"
                        BorderColor="Gray" EnableModelValidation="True" OnRowDataBound="GV_Date_RowDataBound">
                        <HeaderStyle CssClass="GVHead" />
                        <RowStyle CssClass="one" />
                        <AlternatingRowStyle CssClass="two" />
                        <PagerStyle CssClass="GVPage" />
                        <EmptyDataRowStyle HorizontalAlign="Center" />
                        <Columns>
                            <asp:TemplateField HeaderText="操作">
                                <ItemTemplate>
                                    <asp:CheckBox runat="server" ID="Chk_Del" Text="刪除" CssClass="bound2" />
                                    <asp:HiddenField runat="server" ID="hid_CheckStr" Value='<%# Bind("CheckStr")%>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="YYMM" HeaderText="盤點月份" />
                            <asp:BoundField DataField="SITE_NO" HeaderText="倉別" />
                            <asp:BoundField DataField="VENDOR_NO" HeaderText="廠商編號" />
                            <asp:BoundField DataField="INV_DATE" HeaderText="盤點日期" />
                            <asp:BoundField DataField="lbl_01" HeaderText="限制門市<br/>登打退貨日期" HtmlEncode="False" />
                            <asp:BoundField DataField="lbl_02" HeaderText="限制物流<br/>進貨日期(XD)" HtmlEncode="False" />
                            <asp:BoundField DataField="lbl_03" HeaderText="限制物流<br/>進貨日期(DC)" HtmlEncode="False" />
                            <asp:BoundField DataField="lbl_04" HeaderText="限制物流<br/>揀貨日期" HtmlEncode="False" />
                            <asp:BoundField DataField="lbl_05" HeaderText="限制物流<br/>出貨日期" HtmlEncode="False" />
                            <asp:BoundField DataField="lbl_06" HeaderText="限制物流<br/>調撥日期" HtmlEncode="False" />
                            <asp:BoundField DataField="lbl_07" HeaderText="限制門市<br/>調撥日期" HtmlEncode="False" />
                            <asp:BoundField DataField="lbl_08" HeaderText="預購<br/>停止日" HtmlEncode="False" />
                        </Columns>
                    </asp:GridView>
                </div>
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
