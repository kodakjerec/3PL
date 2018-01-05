<%@ Page Language="C#" MasterPageFile="~/3PLMasterPage.Master" AutoEventWireup="true" CodeBehind="3PL_SignOffStatus.aspx.cs"
    Inherits="_3PL_System._3PL_SignOffStatus" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>簽核流程設定</title>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table class="tborder" style="width: 100%">
        <tr>
            <td class="PageTitle">簽核流程設定
            </td>
        </tr>
        <tr>
            <asp:GridView ID="GV_BaseAccounting" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                CssClass="GVStyle" OnPageIndexChanging="GV_BaseAccounting_PageIndexChanging"
                OnRowCancelingEdit="GV_BaseAccounting_RowCancelingEdit" OnRowEditing="GV_BaseAccounting_RowEditing"
                OnRowUpdating="GV_BaseAccounting_RowUpdating" AllowSorting="True" ShowFooter="True"
                PageSize="20"
                Width="100%">
                <HeaderStyle CssClass="GVHead" />
                <RowStyle CssClass="one" />
                <PagerStyle CssClass="GVPage" />
                <EmptyDataRowStyle HorizontalAlign="Center" />
                <Columns>
                    <asp:TemplateField ShowHeader="False">
                        <EditItemTemplate>
                            <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="True" CommandName="Update"
                                Text="更新"></asp:LinkButton>
                            &nbsp;<asp:LinkButton ID="LinkButton2" runat="server" CausesValidation="False" CommandName="Cancel"
                                Text="取消"></asp:LinkButton>
                        </EditItemTemplate>
                        <ItemTemplate>
                            <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="False" CommandName="Edit"
                                Text="編輯"></asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="單據代號">
                        <ItemTemplate>
                            <asp:Label ID="LBL_PageType" runat="server" Text='<%# Bind("PageType") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="單據種類">
                        <ItemTemplate>
                            <asp:Label ID="LBL_PageName" runat="server" Text='<%# Bind("PageName") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="步驟">
                        <ItemTemplate>
                            <asp:Label ID="LBL_Step" runat="server" Text='<%# Bind("Step") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="狀況代號">
                        <ItemTemplate>
                            <asp:Label ID="LBL_Status" runat="server" Text='<%# Bind("Status") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="狀況名稱">
                        <EditItemTemplate>
                            <asp:TextBox ID="TBX_StatusName" runat="server" Text='<%# Bind("StatusName") %>'></asp:TextBox>
                        </EditItemTemplate>
                        <ItemTemplate>
                            <asp:Label ID="LBL_StatusName" runat="server" Text='<%# Bind("StatusName") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="確定按鈕名稱">
                        <EditItemTemplate>
                            <asp:TextBox ID="TBX_OkbuttonName" runat="server" Text='<%# Bind("OkbuttonName") %>'></asp:TextBox>
                        </EditItemTemplate>
                        <ItemTemplate>
                            <asp:Label ID="LBL_OkbuttonName" runat="server" Text='<%# Bind("OkbuttonName") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="取消按鈕名稱">
                        <EditItemTemplate>
                            <asp:TextBox ID="TBX_NobuttonName" runat="server" Text='<%# Bind("NobuttonName") %>'></asp:TextBox>
                        </EditItemTemplate>
                        <ItemTemplate>
                            <asp:Label ID="LBL_NobuttonName" runat="server" Text='<%# Bind("NobuttonName") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </tr>
    </table>
</asp:Content>
