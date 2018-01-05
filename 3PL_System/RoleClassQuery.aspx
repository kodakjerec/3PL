<%@ Page Language="C#" MasterPageFile="~/3PLMasterPage.Master" AutoEventWireup="true" CodeBehind="RoleClassQuery.aspx.cs"
    Inherits="_3PL_System.RoleClassQuery" %>

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
                    身分類別查詢
                </td>
            </tr>
        </table>
        <table class="tborder" style="width:40%">
            <tr>
                <td class="EditTD1">
                    身分類別：
                </td>
                <td>
                    <asp:TextBox ID="txb_Class" runat="server" MaxLength="20"></asp:TextBox>
                </td>
                <td>
                    <asp:Button ID="btn_Query" runat="server" Text="查詢" OnClick="btn_Query_Click" />
                </td>
            </tr>
        </table>
        <table class="tborder" style="width:40%">
            <tr>
                <td>
                    <asp:GridView ID="gv_List" runat="server" AutoGenerateColumns="False" CssClass="GVStyle"
                        Width="100%" AllowPaging="True" OnPageIndexChanging="gv_List_PageIndexChanging"
                        PageSize="20"
                        OnRowCommand="gv_List_RowCommand">
                        <HeaderStyle CssClass="GVHead" />
                        <RowStyle CssClass="one" />
                        <PagerStyle CssClass="GVPage" />
                        <EmptyDataRowStyle HorizontalAlign="Center" />
                        <Columns>
                            <asp:TemplateField HeaderText="身分類別">
                                <ItemStyle HorizontalAlign="Center" />
                                <ItemTemplate>
                                    <asp:HiddenField ID="hid_ClassId" runat="server" Value='<%# Bind("ClassId") %>' />
                                    <asp:LinkButton ID="lbtn_ClassName" runat="server" Text='<%# Bind("ClassName") %>' CommandName="Select"></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="刪除">
                                <ItemStyle HorizontalAlign="Center" Width="5" />
                                <ItemTemplate>
                                    <asp:Button ID="btn_Del" runat="server" Text="刪除" OnClientClick="if(!confirm('是否確定刪除此類別？')){return false;}"
                                        CommandName="Del" />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
