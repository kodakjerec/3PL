<%@ Page Language="C#" MasterPageFile="~/3PLMasterPage.Master"
    AutoEventWireup="true" CodeBehind="3PL_BaseCostSet.aspx.cs"
    Inherits="_3PL_System._3PL_BaseCostSet" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>計價費用設定</title>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table class="tborder" width="100%">
        <tr>
            <td class="PageTitle" colspan="4">
                <asp:Label ID="lbl_PriceSet" runat="server" Text="計價費用設定"></asp:Label>
            </td>
        </tr>
        <tr>
            <td class="HeaderStyle" colspan="4">
                <asp:Label ID="lbl_PriceSet_Query" runat="server" Text="計價費用設定查詢"></asp:Label>
            </td>
        </tr>
    </table>
    <table class="tborder">
        <tr>
            <td class="EditTD1">
                <asp:Label ID="Label1" runat="server" Text="*" ForeColor="Red"></asp:Label>
                <asp:Label ID="lbl_SiteNo" runat="server" Text="寄倉倉別："></asp:Label>
            </td>
            <td>
                <asp:DropDownList ID="ddl_SiteNo" runat="server" AutoPostBack="True">
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td class="EditTD1">
                <asp:Label ID="lbl_TypeId" runat="server" Text="報價主類別："></asp:Label>
            </td>
            <td>
                <asp:DropDownList ID="ddl_TypeId" runat="server" AutoPostBack="True">
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Button ID="Btn_Query" runat="server" Text="查詢" Height="30px" OnClick="Btn_Query_Click" />
                <asp:Button ID="Btn_New" runat="server" Text="新增" Height="30px" OnClick="Btn_New_Click" />
            </td>
        </tr>
    </table>
    <table class="tborder" width="100%">
        <tr>
            <td colspan="2">
                <asp:GridView ID="GV_PriceList_Query" runat="server" Width="100%" AutoGenerateColumns="False"
                    CssClass="GVStyle" OnRowCommand="GV_PriceList_Query_RowCommand">
                    <HeaderStyle CssClass="GVHead" />
                    <RowStyle CssClass="one" />
                    <AlternatingRowStyle CssClass="two" />
                    <PagerStyle CssClass="GVPage" />
                    <EmptyDataRowStyle HorizontalAlign="Center" />
                    <Columns>
                        <asp:TemplateField HeaderText="費用名稱">
                            <ItemTemplate>
                                <asp:LinkButton ID="Lbl_CostName" runat="server" Text='<%# Bind("S_bcse_CostName") %>'></asp:LinkButton>
                                <asp:HiddenField ID="hid_bcseSeq" runat="server" Value='<%# Bind("I_bcse_seq") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="I_bcse_Price" HeaderText="單價" />
                        <asp:BoundField DataField="S_bcse_DollarUnit" HeaderText="貨幣單位" />
                        <asp:BoundField DataField="UnitName" HeaderText="計價單位" />
                        <asp:BoundField DataField="AccName" HeaderText="會計名稱" />
                        <asp:BoundField DataField="TypeName" HeaderText="主類別名稱" />
                    </Columns>
                </asp:GridView>
            </td>
        </tr>
    </table>
</asp:Content>
