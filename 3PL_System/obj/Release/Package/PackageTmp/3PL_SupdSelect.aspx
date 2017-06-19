<%@ Page Language="C#" MasterPageFile="~/3PLMasterPage.Master" AutoEventWireup="true"
    CodeBehind="3PL_SupdSelect.aspx.cs" Inherits="SC_Offer.SCObject" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>作業對象</title>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div>
        <table class="tborder" width="100%">
            <tr>
                <td class="PageTitle">作業對象查詢
                </td>
            </tr>
        </table>
        <asp:UpdatePanel ID="UpdPanel_table1" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <table class="tborder" width="100%">
                    <tr>
                        <td class="EditTD1" width="15%">對象代碼：
                        </td>
                        <td width="35%">
                            <asp:TextBox ID="txb_ObjNo" runat="server" AutoPostBack="True" OnTextChanged="txb_ObjNo_TextChanged"></asp:TextBox>
                        </td>
                        <td class="EditTD1" width="15%">對象名稱：
                        </td>
                        <td width="35%">
                            <asp:TextBox ID="txb_ObjName" runat="server" AutoPostBack="True" OnTextChanged="txb_ObjNo_TextChanged"></asp:TextBox>
                        </td>
                    </tr>
                </table>
                <table class="tborder" style="width: 100%">
                    <tr>
                        <td>
                            <asp:GridView ID="gv_List" runat="server" AutoGenerateColumns="False" CssClass="GVStyle"
                                Width="100%" AllowPaging="True" OnPageIndexChanging="gv_List_PageIndexChanging"
                                PageSize="15" AllowSorting="True" OnRowEditing="gv_List_RowEditing">
                                <HeaderStyle CssClass="GVHead" />
                                <RowStyle CssClass="one" />
                                <PagerStyle CssClass="GVPage" />
                                <EmptyDataRowStyle HorizontalAlign="Center" />
                                <Columns>
                                    <asp:CommandField ButtonType="Button" EditText="選取" ShowEditButton="True">
                                        <ItemStyle HorizontalAlign="Center" Width="5%" />
                                    </asp:CommandField>
                                    <asp:BoundField HeaderText="對象代碼" DataField="ID">
                                        <ItemStyle HorizontalAlign="Center" Width="10%" />
                                    </asp:BoundField>
                                    <asp:BoundField HeaderText="對象名稱" DataField="ALIAS">
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:BoundField>
                                </Columns>
                            </asp:GridView>
                        </td>
                    </tr>
                </table>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>
