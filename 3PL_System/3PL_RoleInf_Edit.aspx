<%@ Page Language="C#" MasterPageFile="~/3PLMasterPage.Master" AutoEventWireup="true" CodeBehind="3PL_RoleInf_Edit.aspx.cs" Inherits="_3PL_System._3PL_RoleInf_Edit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>權限角色設定</title>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table class="tborder" style="width:100%">
        <tr>
            <td class="PageTitle">權限角色設定
            </td>
        </tr>
        <tr>
            <asp:GridView ID="GV_BaseData" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                CssClass="GVStyle" OnPageIndexChanging="GV_BaseData_PageIndexChanging" OnRowCancelingEdit="GV_BaseData_RowCancelingEdit"
                OnRowEditing="GV_BaseData_RowEditing" OnRowUpdating="GV_BaseData_RowUpdating"
                AllowSorting="True" ShowFooter="True" Width="100%" OnRowDeleting="GV_BaseData_RowDeleting">
                <HeaderStyle CssClass="GVHead" />
                <RowStyle CssClass="one" />
                <AlternatingRowStyle CssClass="two" />
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
                        <FooterTemplate>
                            <asp:Button ID="BTN_Insert_BaseData" runat="server" OnClick="BTN_Insert_BaseData_Click"
                                Text="新增" />
                        </FooterTemplate>
                        <ItemTemplate>
                            <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="False" CommandName="Edit"
                                Text="編輯"></asp:LinkButton>
                            &nbsp;<asp:LinkButton ID="LinkButton2" runat="server" CausesValidation="False" CommandName="Delete"
                                Text="刪除"></asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="序號">
                        <EditItemTemplate>
                            <asp:TextBox ID="TBX_RoleId" runat="server" Text='<%# Bind("RoleId") %>'></asp:TextBox>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <asp:TextBox ID="TBX_INS_RoleId" runat="server"></asp:TextBox>
                        </FooterTemplate>
                        <ItemTemplate>
                            <asp:Label ID="LBL_RoleId" runat="server" Text='<%# Bind("RoleId") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="項目名稱">
                        <EditItemTemplate>
                            <asp:TextBox ID="TBX_RoleNm" runat="server" Text='<%# Bind("RoleNm") %>'></asp:TextBox>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <asp:TextBox ID="TBX_INS_RoleNm" runat="server"></asp:TextBox>
                        </FooterTemplate>
                        <ItemTemplate>
                            <asp:Label ID="LBL_RoleNm" runat="server" Text='<%# Bind("RoleNm") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="絕對序號">
                        <ItemTemplate>
                            <asp:Label ID="lbl_Sn" runat="server" Text='<%# Bind("Sn") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </tr>
    </table>
</asp:Content>
