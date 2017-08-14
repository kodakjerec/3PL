<%@ Page Language="C#" MasterPageFile="~/3PLMasterPage.Master" AutoEventWireup="true" CodeBehind="3PL_BaseCost.aspx.cs" Inherits="_3PL_System._3PL_BaseCost" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>成本費用設定</title>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table class="tborder" style="width:100%">
        <tr>
            <td class="PageTitle">
                <asp:Label ID="lbl_PriceSet" runat="server" Text="成本費用設定"></asp:Label>
            </td>
        </tr>
        <tr>
            <td class="HeaderStyle">
                <asp:Label ID="lbl_PriceSet_Query" runat="server" Text="成本費用設定查詢"></asp:Label>
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
                <asp:DropDownList ID="ddl_SiteNo" runat="server" AutoPostBack="True"
                    OnSelectedIndexChanged="ddl_SiteNo_SelectedIndexChanged">
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td class="EditTD1">
                <asp:Label ID="Label2" runat="server" Text="*" ForeColor="Red"></asp:Label>
                <asp:Label ID="lbl_TypeId" runat="server" Text="報價主類別："></asp:Label>
            </td>
            <td>
                <asp:DropDownList ID="ddl_TypeId" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddl_TypeId_SelectedIndexChanged">
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td class="EditTD1">
                <asp:Label ID="Lbl_CostName" runat="server" Text="費用名稱："></asp:Label>
            </td>
            <td>
                <asp:DropDownList ID="DDL_CostName" runat="server" AutoPostBack="True" Enabled="False">
                </asp:DropDownList>
            </td>
        </tr>
    </table>
    <asp:Button ID="Btn_Query" runat="server" Text="查詢" Height="30px" OnClick="Btn_Query_Click" />
    <asp:Button ID="Btn_New" runat="server" Text="新增" Height="30px" OnClick="Btn_New_Click" />

    <table class="tborder" style="width:100%">
        <tr>
            <td colspan="2">
                <asp:GridView ID="GV_PriceList_Query" runat="server" Width="100%" AutoGenerateColumns="False"
                    CssClass="GVStyle" OnRowCommand="GV_PriceList_Query_RowCommand"
                    AllowPaging="True" PageSize="10"
                    OnPageIndexChanging="GV_PriceList_Query_PageIndexChanging">
                    <HeaderStyle CssClass="GVHead" />
                    <RowStyle CssClass="one" />
                    <AlternatingRowStyle CssClass="two" />
                    <PagerStyle CssClass="GVPage" />
                    <EmptyDataRowStyle HorizontalAlign="Center" />
                    <Columns>
                        <asp:TemplateField HeaderText="成本費用名稱">
                            <ItemTemplate>
                                <asp:LinkButton ID="Lbl_CostName" runat="server" Text='<%# Bind("S_basc_CostName") %>'></asp:LinkButton>
                                <asp:HiddenField ID="hid_bascSeq" runat="server" Value='<%# Bind("I_basc_seq") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="TypeIdName" HeaderText="報價主類別" />
                        <asp:BoundField DataField="BaseCostSetName" HeaderText="計價費用" />
                        <asp:BoundField DataField="S_bsda_FieldName" HeaderText="成本類別" />
                        <asp:BoundField DataField="I_basc_Free" HeaderText="單價" />
                        <asp:BoundField DataField="S_basc_DollarUnit" HeaderText="貨幣單位" />
                        <asp:BoundField DataField="S_bsda_FieldName" HeaderText="計價單位" />
                    </Columns>
                </asp:GridView>
            </td>
        </tr>
    </table>
</asp:Content>
