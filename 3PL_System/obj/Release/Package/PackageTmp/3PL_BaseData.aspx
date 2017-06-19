<%@ Page Language="C#" MasterPageFile="~/3PLMasterPage.Master" AutoEventWireup="true" CodeBehind="3PL_BaseData.aspx.cs" Inherits="_3PL_System._3PL_BaseData" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>系統大類設定</title>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table class="tborder" width="100%">
        <tr>
            <td class="PageTitle">系統大類設定
            </td>
        </tr>
        <tr>
            <td class="HeaderStyle">系統大類：
                <asp:DropDownList ID="ddl_CateId" runat="server" OnSelectedIndexChanged="ddl_CateId_SelectedIndexChanged"
                    AutoPostBack="True" style="color:black">
                </asp:DropDownList>
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
                            <asp:TextBox ID="TBX_S_bsda_FieldId" runat="server" Text='<%# Bind("S_bsda_FieldId") %>'></asp:TextBox>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <asp:TextBox ID="TBX_INS_S_bsda_FieldId" runat="server"></asp:TextBox>
                        </FooterTemplate>
                        <ItemTemplate>
                            <asp:Label ID="LBL_S_bsda_FieldId" runat="server" Text='<%# Bind("S_bsda_FieldId") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="項目名稱">
                        <EditItemTemplate>
                            <asp:TextBox ID="TBX_S_bsda_Fieldname" runat="server" Text='<%# Bind("S_bsda_FieldName") %>'></asp:TextBox>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <asp:TextBox ID="TBX_INS_S_bsda_Fieldname" runat="server"></asp:TextBox>
                        </FooterTemplate>
                        <ItemTemplate>
                            <asp:Label ID="LBL_S_bsda_Fieldname" runat="server" Text='<%# Bind("S_bsda_FieldName") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="絕對序號">
                        <ItemTemplate>
                            <asp:Label ID="Lbl_I_bsda_seq" runat="server" Text='<%# Bind("I_bsda_seq") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </tr>
    </table>
</asp:Content>
