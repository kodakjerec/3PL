<%@ Page Language="C#" MasterPageFile="~/3PLMasterPage.Master" AutoEventWireup="true" CodeBehind="FunList.aspx.cs" Inherits="_3PL_System.FunList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>管理設定</title>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div>
        <table class="tborder" width="100%">
            <tr>
                <td class="PageTitle">管理設定
                </td>
            </tr>
            <tr>
                <td class="HeaderStyle ">
                    <asp:Label ID="lbl_CustInf" runat="server" Text="選單管理"></asp:Label>
                </td>
            </tr>
        </table>
        <table class="tborder" width="60%">
            <tr>
                <td class="EditTD1" width="15%">
                    <asp:Label ID="lbl_RoleId" runat="server" Text="權限角色"></asp:Label>
                </td>
                <td width="15%">
                    <asp:DropDownList ID="ddl_Role" runat="server">
                    </asp:DropDownList>
                </td>
                <td class="EditTD1" width="15%">
                    <asp:Label ID="lbl_Fun" runat="server" Text="主功能名稱:"></asp:Label>
                </td>
                <td width="15%">
                    <asp:TextBox ID="txb_Fun" runat="server" MaxLength="10"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td colspan="4">
                    <asp:Button ID="btn_Query" runat="server" Text="查詢" OnClick="btn_Query_Click"
                        Style="height: 21px" />
                </td>
            </tr>
        </table>
        <table class="tborder" width="60%">
            <tr>
                <td>
                    <asp:GridView ID="gv_List" runat="server" AutoGenerateColumns="False" CssClass="GVStyle"
                        Width="100%" OnRowDataBound="gv_List_RowDataBound" AllowPaging="True" OnPageIndexChanging="gv_List_PageIndexChanging">
                        <HeaderStyle CssClass="GVHead" />
                        <RowStyle CssClass="one" />
                        <PagerStyle CssClass="GVPage" />
                        <EmptyDataRowStyle HorizontalAlign="Center" />
                        <Columns>
                            <asp:TemplateField>
                                <ItemStyle HorizontalAlign="Left" Width="5" />
                                <ItemTemplate>
                                    <asp:HiddenField ID="hid_FunId" runat="server" Value='<%# Bind("FunId") %>' />
                                    <asp:HiddenField ID="hid_PgId" runat="server" Value='<%# Bind("PgId") %>' />
                                    <asp:HiddenField ID="hid_RoleId" runat="server" Value='<%# Bind("RoleId") %>' />
                                    <asp:CheckBox ID="cbk_FunList" runat="server" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField HeaderText="主功能名稱" DataField="FunNm">
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:BoundField HeaderText="子功能名稱" DataField="PgNm">
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                        </Columns>
                    </asp:GridView>
                </td>
            </tr>
        </table>
        <table class="tborder" width="60%">
            <tr>
                <td align="center">
                    <asp:Button ID="btn_Submit" runat="server" Text="更新本頁資訊" OnClick="btn_Submit_Click" />
                </td>
            </tr>
        </table>
        <asp:HiddenField ID="hid_Role_Id" runat="server" />
    </div>
</asp:Content>
