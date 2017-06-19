<%@ Page Language="C#" MasterPageFile="~/3PLMasterPage.Master" AutoEventWireup="true" CodeBehind="PubModify.aspx.cs" Inherits="_3PL_System.PubModify" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">

        $(document).ready(function () {

            $("#<%=txb_BullDay.ClientID %>").datepicker({ dateFormat: 'yy-mm-dd' });

        });
    </script>

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
                    <asp:Label ID="lbl_CustInf" runat="server" Text="公告異動"></asp:Label>
                </td>
            </tr>
        </table>
        <table class="tborder" width="100%">
            <tr>
                <td class="EditTD1" width="15%">
                    <asp:Label ID="lbl_BullDay" runat="server" Text="發佈日期:"></asp:Label>
                </td>
                <td width="85%">
                    <asp:TextBox ID="txb_BullDay" runat="server" MaxLength="10"></asp:TextBox>
                    <asp:Button ID="btn_Query" runat="server" Text="查詢" OnClick="btn_Query_Click" />
                </td>
            </tr>
        </table>
        <table class="tborder" width="100%">
            <asp:GridView ID="gv_List" runat="server" AutoGenerateColumns="False" CssClass="GVStyle"
                Width="100%" AllowPaging="True" OnPageIndexChanging="gv_List_PageIndexChanging"
                OnRowCommand="gv_List_RowCommand" OnRowDataBound="gv_List_RowDataBound">
                <HeaderStyle CssClass="GVHead" />
                <RowStyle CssClass="one" />
                <PagerStyle CssClass="GVPage" />
                <EmptyDataRowStyle HorizontalAlign="Center" />
                <Columns>
                    <asp:BoundField HeaderText="發佈日期" DataField="Bull_Day">
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:BoundField>
                    <asp:TemplateField HeaderText="發佈內容">
                        <ItemStyle HorizontalAlign="Left" />
                        <ItemTemplate>
                            <asp:HiddenField ID="hid_BullNo" runat="server" Value='<%# Bind("Bull_No") %>' />
                            <asp:LinkButton ID="lbtn_Id" runat="server" Text='<%# Bind("Memo") %>' CommandName="Select"></asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField HeaderText="發佈人員" DataField="Bull_Member">
                        <ItemStyle HorizontalAlign="Left" />
                    </asp:BoundField>
                    <asp:BoundField HeaderText="有效起日" DataField="Eff_DateS">
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:BoundField>
                    <asp:BoundField HeaderText="有效迄日" DataField="Eff_DateE">
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:BoundField>
                    <asp:TemplateField HeaderText="刪除">
                        <ItemStyle HorizontalAlign="Center" Width="5" />
                        <ItemTemplate>
                            <asp:Button ID="btn_Del" runat="server" Text="刪除" OnClientClick="if(!confirm('是否確定刪除此公告？')){return false;}"
                                CommandName="Del" />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </table>
    </div>
</asp:Content>
