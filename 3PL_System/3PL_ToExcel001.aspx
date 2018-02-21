<%@ Page Language="C#" MasterPageFile="~/3PLMasterPage.Master" AutoEventWireup="true" CodeBehind="3PL_ToExcel001.aspx.cs"
    Inherits="_3PL_System._3PL_ToExcel001" %>
 
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {
            $(".datepicker_yymmdd").datepicker({ dateFormat: 'yy/mm/dd' });
            $(".datepicker_yymm").datepicker({ dateFormat: 'yy/mm' });
        });
    </script>
    <title>匯出Excel001</title>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:Button Text="報價單" BorderStyle="None" ID="Tab1" CssClass="btn-primary" runat="server"
        OnClick="Tab1_Click" />
    <asp:Button Text="派工單" BorderStyle="None" ID="Tab2" CssClass="btn-default" runat="server"
        OnClick="Tab2_Click" />
    <asp:Button Text="成本及收入明細表" BorderStyle="None" ID="Tab3" CssClass="btn-default" runat="server"
        OnClick="Tab3_Click" />
    <br />
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Always">
        <ContentTemplate>
            <asp:MultiView ID="MultiView1" runat="server" ActiveViewIndex="0">
                <asp:View ID="View1" runat="server">
                    <div id="ExportQuotation">
                        <table class="tborder" style="width:100%">
                            <tr>
                                <td class="PageTitle">
                                    報價單匯出Excel
                                </td>
                            </tr>
                            <tr>
                                <td class="HeaderStyle ">
                                   報價單資料
                                </td>
                            </tr>
                        </table>
                        <table>
                            <tr>
                                <td class="EditTD1">
                                    供應商編號：
                                </td>
                                <td>
                                    <asp:TextBox ID="Txb_S_qthe_SupdId" runat="server"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="EditTD1">
                                    <asp:Label ID="Label3" runat="server" ForeColor="Red" Text="*"></asp:Label>
                                   寄倉倉別：
                                </td>
                                <td>
                                    <asp:DropDownList ID="DDL_S_qthe_SiteNo" runat="server">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td class="EditTD1">
                                    報價日期起
                                </td>
                                <td>
                                    <asp:TextBox ID="txb_D_qthe_ContractS_Qry" CssClass="datepicker_yymmdd" runat="server"></asp:TextBox>
                                </td>
                                <td class="EditTD1">
                                    報價日期迄
                                </td>
                                <td>
                                    <asp:TextBox ID="txb_D_qthe_ContractE_Qry" CssClass="datepicker_yymmdd" runat="server"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="EditTD1">
                                   報價單號：
                                </td>
                                <td>
                                    <asp:TextBox ID="txb_Quotation_PLNO" runat="server"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="EditTD1">
                                    
                                </td>
                                <td>
                                    <asp:CheckBox ID="Chk_ShowStatusIsZero" runat="server" Text="顯示作廢單據" />
                                </td>
                            </tr>
                        </table>
                        <asp:Button ID="btn_Output_Quotation" runat="server" Text="匯出報價單" OnClick="btn_Output_Quotation_Click" OnClientClick="ShowProgressBar()"/>
                    </div>
                </asp:View>
                <asp:View ID="View2" runat="server">
                    <div id="ExportAssign">
                        <table class="tborder" style="width:100%">
                            <tr>
                                <td class="PageTitle">
                                    派工單匯出Excel
                                </td>
                            </tr>
                            <tr>
                                <td class="HeaderStyle ">
                                    派工單資料
                                </td>
                            </tr>
                        </table>
                        <table class="tborder">
                            <tr>
                                <td class="EditTD1">
                                    供應商編號：
                                </td>
                                <td>
                                    <asp:TextBox ID="Txb_S_qthe_SupdId2" runat="server"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="EditTD1">
                                    <asp:Label ID="Label7" runat="server" ForeColor="Red" Text="*"></asp:Label>
                                   寄倉倉別：
                                </td>
                                <td>
                                    <asp:DropDownList ID="DDL_S_qthe_SiteNo2" runat="server">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td class="EditTD1">
                                   派工單號：
                                </td>
                                <td>
                                    <asp:TextBox ID="Txb_Wk_Id_Query" runat="server"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="EditTD1">
                                    預計完工日：
                                </td>
                                <td>
                                    <asp:TextBox ID="txb_Bdate_Assign" CssClass="datepicker_yymmdd" runat="server"></asp:TextBox>
                                    <asp:Label ID="Label19" runat="server" Text="至"></asp:Label>
                                    <asp:TextBox ID="txb_Edate_Assign" CssClass="datepicker_yymmdd" runat="server"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="EditTD1">
                                    
                                </td>
                                <td>
                                    <asp:CheckBox ID="Chk_ShowStatusIsZero2" runat="server" Text="顯示作廢單據" />
                                </td>
                            </tr>
                        </table>
                        <asp:Button ID="btn_Output_Assign" runat="server" Text="匯出派工單" OnClick="btn_Output_Assign_Click" OnClientClick="ShowProgressBar()" />
                    </div>
                </asp:View>
                <asp:View ID="View3" runat="server">
                    <div id="ExportCostIncome">
                        <table class="tborder" style="width:100%">
                            <tr>
                                <td class="PageTitle">
                                    成本及收入明細匯出Excel
                                </td>
                            </tr>
                            <tr>
                                <td class="HeaderStyle ">
                                    查詢條件
                                </td>
                            </tr>
                        </table>
                        <table>
                            <tr>
                                <td class="EditTD1">
                                    供應商編號：
                                </td>
                                <td class="auto-style1">
                                    <asp:TextBox ID="txb_SupdId_CostIncome" runat="server"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="EditTD1">
                                    <asp:Label ID="Label15" runat="server" ForeColor="Red" Text="*"></asp:Label>
                                    寄倉倉別：
                                </td>
                                <td class="auto-style1">
                                    <asp:DropDownList ID="DD_CostIncome" runat="server">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td class="EditTD1">
                                   報價單號：
                                </td>
                                <td class="auto-style1">
                                    <asp:TextBox ID="txb_PLNO_CostIncome" runat="server"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="EditTD1">
                                    查詢月份：
                                </td>
                                <td class="auto-style1">
                                    <asp:TextBox ID="txb_Bmonth_CostIncome" CssClass="datepicker_yymm" runat="server"></asp:TextBox>
                                </td>
                            </tr>
                        </table>
                        <asp:Button ID="btn_CostIncome" runat="server" Text="匯出成本及收入明細" OnClick="btn_CostIncome_Click" OnClientClick="ShowProgressBar()" />
                    </div>
                </asp:View>
            </asp:MultiView>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>