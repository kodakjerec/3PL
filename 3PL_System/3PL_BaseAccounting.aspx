<%@ Page Language="C#" MasterPageFile="~/3PLMasterPage.Master" AutoEventWireup="true" CodeBehind="3PL_BaseAccounting.aspx.cs"
    Inherits="_3PL_System._3PL_BaseAccounting" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>帳務費用設定</title>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table class="tborder" style="width: 100%">
        <tr>
            <td class="PageTitle">
                帳務費用設定
            </td>
        </tr>
        <tr>
            <asp:UpdatePanel ID="UpdPanel_Assign_HeadList" runat="server" UpdateMode="Always">
                <ContentTemplate>
                    <asp:GridView ID="GV_BaseAccounting" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                        CssClass="GVStyle" OnPageIndexChanging="GV_BaseAccounting_PageIndexChanging"
                        OnRowCancelingEdit="GV_BaseAccounting_RowCancelingEdit" OnRowEditing="GV_BaseAccounting_RowEditing"
                        OnRowUpdating="GV_BaseAccounting_RowUpdating" AllowSorting="True" ShowFooter="True" PageSize="20"
                        Width="100%">
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
                                    <asp:Button ID="BTN_Insert_BaseAccounting" runat="server" OnClick="BTN_Insert_BaseAccounting_Click"
                                        Text="新增" />
                                </FooterTemplate>
                                <ItemTemplate>
                                    <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="False" CommandName="Edit"
                                        Text="編輯"></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="會計科目">
                                <EditItemTemplate>
                                    <asp:TextBox ID="TBX_S_Acci_Id" runat="server" Text='<%# Bind("S_Acci_Id") %>'></asp:TextBox>
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <asp:TextBox ID="TBX_INS_S_Acci_Id" runat="server"></asp:TextBox>
                                </FooterTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="LBL_S_Acci_Id" runat="server" Text='<%# Bind("S_Acci_Id") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="科目名稱">
                                <EditItemTemplate>
                                    <asp:TextBox ID="TBX_S_Acci_Name" runat="server" Text='<%# Bind("S_Acci_Name") %>'></asp:TextBox>
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <asp:TextBox ID="TBX_INS_S_Acci_Name" runat="server"></asp:TextBox>
                                </FooterTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="LBL_S_Acci_Name" runat="server" Text='<%# Bind("S_Acci_Name") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </ContentTemplate>
            </asp:UpdatePanel>
        </tr>
    </table>
</asp:Content>
