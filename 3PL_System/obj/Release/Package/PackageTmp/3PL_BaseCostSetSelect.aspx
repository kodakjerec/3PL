<%@ Page Language="C#" MasterPageFile="~/3PLMasterPage.Master" AutoEventWireup="true" CodeBehind="3PL_BaseCostSetSelect.aspx.cs"
    Inherits="SC_Offer._3PL_BaseCostSetSelect" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>作業對象</title>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div>
        <table class="tborder" style="width:100%">
            <tr>
                <td class="PageTitle">
                    <asp:Label ID="lbl_TypeFee" runat="server" Text="作業對象查詢"></asp:Label>
                </td>
            </tr>
        </table>
        <table class="tborder" style="width:100%">
            <tr>
                <td class="EditTD1">
                    <asp:Label ID="Label1" runat="server" Text="*" ForeColor="Red"></asp:Label>
                    <asp:Label ID="lbl_SiteNo" runat="server" Text="寄倉倉別："></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddl_SiteNo" runat="server" AutoPostBack="True">
                    </asp:DropDownList>
                </td>
                <td class="EditTD1">
                    <asp:Label ID="lbl_TypeId" runat="server" Text="報價主類別："></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddl_TypeId" runat="server" AutoPostBack="True">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td align="right">
                    <asp:Button ID="btn_Query" runat="server" Text="查詢" OnClick="btn_Query_Click" />
                </td>
            </tr>
        </table>
        <table class="tborder" style="width:100%">
            <tr>
                <td width="50%">
                    <asp:GridView ID="gv_List" runat="server" AutoGenerateColumns="False" CssClass="GVStyle"
                        Width="100%" AllowPaging="True" OnPageIndexChanging="gv_List_PageIndexChanging"
                        PageSize="15" AllowSorting="True" OnRowEditing="gv_List_RowEditing">
                        <HeaderStyle CssClass="GVHead" />
                        <RowStyle CssClass="one" />
                        <AlternatingRowStyle CssClass="two" />
                        <PagerStyle CssClass="GVPage" />
                        <EmptyDataRowStyle HorizontalAlign="Center" />
                        <Columns>
                            <asp:CommandField ButtonType="Button" EditText="選取" ShowEditButton="True">
                                <ItemStyle HorizontalAlign="Center" Width="5%" />
                            </asp:CommandField>
                            <asp:BoundField HeaderText="計價費用名稱" DataField="S_bcse_CostName">
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:BoundField DataField="AccName" HeaderText="會計費用" />
                            <asp:BoundField DataField="TypeName" HeaderText="報價主類別" />
                            <asp:BoundField HeaderText="計價費用代碼" DataField="I_bcse_seq">
                                <ItemStyle HorizontalAlign="Center" Width="10%" />
                            </asp:BoundField>
                            <asp:BoundField DataField="S_bcse_SiteNo" HeaderText="倉別代碼" />
                            <asp:BoundField DataField="I_bcse_TypeId" HeaderText="主類別代碼" />
                        </Columns>
                    </asp:GridView>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
