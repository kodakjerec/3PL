<%@ Page Language="C#" MasterPageFile="~/3PLMasterPage.Master" AutoEventWireup="true" CodeBehind="ClassItem.aspx.cs" Inherits="_3PL_System.ClassItem" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>管理設定</title>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div>
        <table class="tborder" style="width:100%">
            <tr>
                <td class="PageTitle">管理設定
                </td>
            </tr>
            <tr>
                <td class="HeaderStyle ">
                    身分類別建置
                </td>
            </tr>
        </table>
        <table class="tborder" style="width:100%" align="center">
            <tr>
                <td class="EditTD1" width="15%">
                    <asp:Label ID="lbl_2" runat="server" Text="*" ForeColor="Red"></asp:Label>
                   類別名稱：
                </td>
                <td width="85%">
                    <asp:TextBox ID="txb_Name" runat="server" MaxLength="10"></asp:TextBox>
                    <asp:Label ID="lblName" runat="server" Text=""></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="EditTD1" width="15%">
                    <asp:Label ID="lbl_3" runat="server" Text="*" ForeColor="Red"></asp:Label>
                    權限角色：
                </td>
                <td width="85%">
                    <asp:DropDownList ID="ddl_Role" runat="server">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td class="EditTD1" width="15%">
                    <asp:Label ID="lbl_4" runat="server" Text="*" ForeColor="Red"></asp:Label>
                    倉別：
                </td>
                <td width="85%">
                    <asp:DropDownList ID="ddl_DC" runat="server">
                    </asp:DropDownList>
                    <asp:Button ID="btn_Submit" runat="server" Text="新增" OnClick="btn_Submit_Click" />
                </td>
            </tr>
        </table>
        <table class="tborder" width="50%">
            <tr>
                <td width="100%" align="center">
                    <asp:GridView ID="gv_List" runat="server" AutoGenerateColumns="False" CssClass="GVStyle"
                        Width="100%" AllowPaging="True" OnPageIndexChanging="gv_List_PageIndexChanging"
                        OnRowCommand="gv_List_RowCommand">
                        <HeaderStyle CssClass="GVHead" />
                        <RowStyle CssClass="one" />
                        <PagerStyle CssClass="GVPage" />
                        <EmptyDataRowStyle HorizontalAlign="Center" />
                        <Columns>
                            <asp:TemplateField HeaderText="權限角色">
                                <ItemStyle HorizontalAlign="Center" />
                                <ItemTemplate>
                                    <asp:Label ID="lbl_Role" runat="server" Text='<%# Bind("RoleName") %>'></asp:Label>
                                    <asp:HiddenField ID="hid_Role" runat="server" Value='<%# Bind("RoleID") %>' />
                                    <asp:HiddenField ID="hid_DC" runat="server" Value='<%# Bind("DC") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField HeaderText="倉別" DataField="DcName">
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:BoundField>
                            <asp:TemplateField HeaderText="移除">
                                <ItemStyle HorizontalAlign="Center" Width="5" />
                                <ItemTemplate>
                                    <asp:Button ID="btn_Del" runat="server" Text="移除" CommandName="Del" />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </td>
            </tr>
        </table>
        <table class="tborder">
            <tr>
                <td>
                    <asp:Button ID="btn_Ent" runat="server" Text="確定" OnClick="btn_Ent_Click" />
                </td>
            </tr>
        </table>
        <asp:HiddenField ID="hid_ClassId" runat="server" />
        <asp:HiddenField ID="hid_Status" runat="server" />
    </div>
</asp:Content>
