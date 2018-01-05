<%@ Page Language="C#" MasterPageFile="~/3PLMasterPage.Master" AutoEventWireup="true" CodeBehind="EmpList.aspx.cs" Inherits="_3PL_System.EmpList" %>

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
                   使用者管理
                </td>
            </tr>
        </table>
        <table class="tborder" style="width:100%">
            <tr>
                <td class="EditTD1" width="15%">
                    帳號：
                </td>
                <td width="35%">
                    <asp:TextBox ID="txb_WorkId" runat="server" MaxLength="10"></asp:TextBox>
                </td>
                <td class="EditTD1" width="15%">
                    身分類別：
                </td>
                <td width="35%">
                    <asp:DropDownList ID="ddl_Class" runat="server">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td class="EditTD1" width="15%">
                    姓名：
                </td>
                <td width="35%">
                    <asp:TextBox ID="txb_Name" runat="server" MaxLength="10"></asp:TextBox>
                </td>
            </tr>
        </table>
        <table class="tborder" style="width:100%">
            <tr>
                <td>
                    <asp:Button ID="btn_Submit" runat="server" Text="查詢" OnClick="btn_Submit_Click" Style="height: 21px" />
                </td>
            </tr>
        </table>
        <table class="tborder" style="width:100%" align="center">
            <asp:GridView ID="gv_List" runat="server" AutoGenerateColumns="False" CssClass="GVStyle"
                Width="60%" AllowPaging="True" OnPageIndexChanging="gv_List_PageIndexChanging"
                PageSize="20"
                OnRowCommand="gv_List_RowCommand">
                <HeaderStyle CssClass="GVHead" />
                <RowStyle CssClass="one" />
                <PagerStyle CssClass="GVPage" />
                <EmptyDataRowStyle HorizontalAlign="Center" />
                <Columns>
                    <asp:BoundField HeaderText="帳號" DataField="WorkId">
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:BoundField>
                    <asp:TemplateField HeaderText="姓名">
                        <ItemStyle HorizontalAlign="Center" />
                        <ItemTemplate>
                            <asp:LinkButton ID="lbtn_Id" runat="server" Text='<%# Bind("WorkName") %>' CommandName="Select"></asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField HeaderText="身分類別" DataField="ClassName">
                        <ItemStyle HorizontalAlign="Left" />
                    </asp:BoundField>
                    <asp:TemplateField HeaderText="刪除">
                        <ItemStyle HorizontalAlign="Center" Width="5" />
                        <ItemTemplate>
                            <asp:Button ID="btn_Del" runat="server" Text="刪除" OnClientClick="if(!confirm('是否確定刪除此使用者？')){return false;}"
                                CommandName="Del" />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </table>
    </div>
</asp:Content>
