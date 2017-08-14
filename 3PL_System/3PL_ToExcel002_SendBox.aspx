<%@ Page Language="C#" MasterPageFile="~/3PLMasterPage.Master" AutoEventWireup="true" CodeBehind="3PL_ToExcel002_SendBox.aspx.cs"
    Inherits="_3PL_System._3PL_ToExcel002_SendBox" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {
            $(".datepicker_yymmdd").datepicker({ dateFormat: 'yy/mm/dd' });
            $(".datepicker_yymm").datepicker({ dateFormat: 'yy/mm' });
        });
    </script>

    <title>匯出Excel002</title>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:Button Text="明細表" BorderStyle="None" ID="Tab1" CssClass="btn-primary" runat="server"
        OnClick="Tab1_Click" />
    <asp:Button Text="總表" BorderStyle="None" ID="Tab2" CssClass="btn-default" runat="server"
        OnClick="Tab2_Click" />
    <br />
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Always">
        <ContentTemplate>
            <asp:MultiView ID="MultiView1" runat="server" ActiveViewIndex="0">
                <asp:View ID="View1" runat="server">
                    <div id="ExportQuotation">
                        <table class="tborder" style="width:100%">
                            <tr>
                                <td class="PageTitle">投貨收現明細表
                                </td>
                            </tr>
                        </table>
                        <table>
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
                                <td class="EditTD1">發票日期起
                                </td>
                                <td>
                                    <asp:TextBox ID="txb_D_qthe_ContractS_Qry" CssClass="datepicker_yymmdd" runat="server"></asp:TextBox>
                                    發票日期迄
                                    <asp:TextBox ID="txb_D_qthe_ContractE_Qry" CssClass="datepicker_yymmdd" runat="server"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="EditTD1">供應商編號：
                                </td>
                                <td>
                                    <asp:TextBox ID="Txb_S_qthe_SupdId" runat="server"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="EditTD1">PO單號：
                                </td>
                                <td>
                                    <asp:TextBox ID="txb_Quotation_PLNO" runat="server" Text=""></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="EditTD1">發票號碼：
                                </td>
                                <td>
                                    <asp:TextBox ID="txb_UNIVNO" runat="server"></asp:TextBox>
                                </td>
                            </tr>
                        </table>
                        <asp:Button ID="btn_Output_Quotation" runat="server" Text="匯出投貨收現明細表" OnClick="btn_Output_Quotation_Click"
                            OnClientClick="ShowProgressBar()"/>
                    </div>
                </asp:View>
                <asp:View ID="View2" runat="server">
                    <div id="ExportAssign">
                        <table class="tborder" style="width:100%">
                            <tr>
                                <td class="PageTitle">投貨收現總表
                                </td>
                            </tr>
                        </table>
                        <table>
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
                                <td class="EditTD1">發票日期起：
                                </td>
                                <td>
                                    <asp:TextBox ID="txb_Bdate_Assign" CssClass="datepicker_yymmdd" runat="server"></asp:TextBox>
                                    發票日期迄
                                    <asp:TextBox ID="txb_Edate_Assign" CssClass="datepicker_yymmdd" runat="server"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="EditTD1">供應商編號
                                </td>
                                <td>
                                    <asp:TextBox ID="txb_supno2" runat="server"></asp:TextBox>
                                </td>
                            </tr>
                        </table>
                        <asp:Button ID="btn_Output_Assign" runat="server" Text="匯出投貨收現總表" OnClick="btn_Output_Assign_Click"
                            OnClientClick="ShowProgressBar()" />
                    </div>
                </asp:View>
            </asp:MultiView>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
