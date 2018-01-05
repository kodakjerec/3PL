<%@ Page Language="C#" MasterPageFile="~/3PLMasterPage.Master" AutoEventWireup="true" CodeBehind="3PL_SignOffPermission.aspx.cs"
    Inherits="_3PL_System._3PL_SignOffPermission" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>簽核權限設定</title>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table class="tborder" id="PermissionNew">
        <tr>
            <td class="EditTD1">
                單據種類：
            </td>
            <td>
                <asp:DropDownList ID="DDL_PageType" runat="server" AutoPostBack="True" OnSelectedIndexChanged="DDL_PageType_SelectedIndexChanged">
                </asp:DropDownList>
            </td>
            <td class="EditTD1">
                狀況名稱：
            </td>
            <td>
                <asp:DropDownList ID="DDL_StatusName" runat="server" AutoPostBack="True">
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td class="EditTD1">
               權限指派：
            </td>
            <td>身分類別
                <asp:DropDownList ID="DDL_Class" runat="server" AutoPostBack="True">
                </asp:DropDownList>
            </td>
            <td colspan="2">
                使用者
                <asp:DropDownList ID="DDL_Worker" runat="server" AutoPostBack="True">
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Button ID="Btn_New" runat="server" Text="新增" OnClick="Btn_New_Click" />
            </td>
        </tr>
    </table>
    <asp:UpdatePanel ID="UpdPanel_SignOffPermission" runat="server" UpdateMode="Always">
        <ContentTemplate>
            <table class="tborder" style="width:100%">
                <tr>
                    <td class="PageTitle">
                        簽核權限設定
                    </td>
                </tr>
                <tr>
                    <asp:GridView ID="GV_BaseAccounting" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                        CssClass="GVStyle" OnPageIndexChanging="GV_BaseAccounting_PageIndexChanging"
                        PageSize="20"
                        AllowSorting="True" ShowFooter="True" Width="100%" OnRowCommand="GV_BaseAccounting_RowCommand">
                        <HeaderStyle CssClass="GVHead" />
                        <RowStyle CssClass="one" />
                        <PagerStyle CssClass="GVPage" />
                        <EmptyDataRowStyle HorizontalAlign="Center" />
                        <Columns>
                            <asp:TemplateField HeaderText="動作">
                                <ItemTemplate>
                                    <asp:Button ID="Btn_Del" runat="server" Text='刪除' CommandName="Btn_Del"></asp:Button>
                                    <asp:HiddenField ID="hid_Sn" runat="server" Value='<%# Bind("Sn") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="單據種類">
                                <ItemTemplate>
                                    <asp:Label ID="LBL_PageName" runat="server" Text='<%# Bind("PageName") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="狀況名稱">
                                <ItemTemplate>
                                    <asp:Label ID="LBL_StatusName" runat="server" Text='<%# Bind("StatusName") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="身分類別">
                                <ItemTemplate>
                                    <asp:Label ID="LBL_Step" runat="server" Text='<%# Bind("Class") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="使用者">
                                <ItemTemplate>
                                    <asp:Label ID="LBL_Status" runat="server" Text='<%# Bind("Worker") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
