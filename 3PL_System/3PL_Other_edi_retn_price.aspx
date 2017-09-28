<%@ Page Language="C#" MasterPageFile="~/3PLMasterPage.Master" AutoEventWireup="true" CodeBehind="3PL_Other_edi_retn_price.aspx.cs"
    Inherits="_3PL_System._3PL_Other_edi_retn_price" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {
            $("#<%=txb_Bill_date.ClientID %>").datepicker({ dateFormat: 'yymm' });
            $("#<%=txb_back_date_S.ClientID %>").datepicker({ dateFormat: 'yy/mm/dd' });
            $("#<%=txb_back_date_E.ClientID %>").datepicker({ dateFormat: 'yy/mm/dd' });
        });
    </script>
    <title>逆物流費用設定</title>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table class="tborder" style="width: 100%">
        <tr>
            <td class="PageTitle">
                <asp:Label ID="lbl_Quotation" runat="server" Text="逆物流費用設定"></asp:Label>
            </td>
        </tr>
        <tr>
            <td class="HeaderStyle ">
                <asp:Label ID="lbl_Quotation_Query" runat="server" Text="逆物流費用"></asp:Label>
            </td>
        </tr>
    </table>
    <asp:Panel ID="Pan_Quotation_Query" runat="server">
        <table class="tborder">
            <tr>
                <td class="EditTD1">計費年月：
                </td>
                <td>
                    <asp:TextBox ID="txb_Bill_date" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="EditTD1">供應商編號：
                </td>
                <td>
                    <asp:TextBox ID="txb_Query_vendor_no" runat="server"></asp:TextBox>
                    </td>
                <td>
                    <asp:Button ID="Btn_Query_vendor_no" runat="server" Text="選擇對象" OnClick="Btn_Query_vendor_no_Click" />
                </td>
            </tr>
            <tr>
                <td class="EditTD1">寄倉倉別：
                </td>
                <td>
                    <asp:DropDownList ID="ddl_Query_site_no" runat="server">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td class="EditTD1">計費類別：
                </td>
                <td>
                    <asp:DropDownList ID="ddl_Query_Kind" runat="server">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td class="EditTD1">退廠日期：
                </td>
                <td>
                    <asp:TextBox ID="txb_back_date_S" runat="server" />
                </td>
                <td>至
                </td>
                <td>
                    <asp:TextBox ID="txb_back_date_E" runat="server" />
                </td>
            </tr>
            <tr>
                <td class="EditTD1">退廠單號：
                </td>
                <td>
                    <asp:TextBox ID="txb_Query_back_id" runat="server"></asp:TextBox>
                </td>
                <td class="EditTD1">退廠箱號：
                </td>
                <td>
                    <asp:TextBox ID="txb_Query_boxid" runat="server" />
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Button ID="Button1" runat="server" Text="查詢" OnClick="Btn_Query_Click" />
                </td>
            </tr>
        </table>
        禁止修改關帳日
        <asp:Label ID="lbl_CloseDate" runat="server" Style="color: red"></asp:Label>
        之前的資料
    </asp:Panel>
    <div id="div_Content" runat="server" visible="false">
        <table class="tborder" style="width: 100%">
            <tr>
                <asp:GridView ID="GV_BaseAccounting" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                    CssClass="GVStyle" OnPageIndexChanging="GV_BaseAccounting_PageIndexChanging"
                    AllowSorting="True"
                    Width="100%" PageSize="50" OnRowDataBound="GV_BaseAccounting_RowDataBound">
                    <HeaderStyle CssClass="GVHead" />
                    <RowStyle CssClass="one" />
                    <PagerStyle CssClass="GVPage" />
                    <EmptyDataRowStyle HorizontalAlign="Center" />
                    <Columns>
                        <asp:TemplateField HeaderText="計費年月">
                            <ItemTemplate>
                                <asp:Label ID="lbl_Bill_date" runat="server" Text='<%# Bind("Bill_date") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="倉別">
                            <ItemTemplate>
                                <asp:Label ID="lbl_Siteno" runat="server" Text='<%# Bind("site_no") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="供應商">
                            <ItemTemplate>
                                <asp:Label ID="lbl_vendor_no" runat="server" Text='<%# Bind("vendor_no") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="驗收日期">
                            <ItemTemplate>
                                <asp:Label ID="lbl_rec_date" runat="server" Text='<%#Bind("rec_date","{0:yyyy/MM/dd}") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="退廠日期">
                            <ItemTemplate>
                                <asp:Label ID="lbl_back_date" runat="server" Text='<%# Bind("back_date","{0:yyyy/MM/dd}") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="退廠單號">
                            <ItemTemplate>
                                <asp:Label ID="lbl_back_id" runat="server" Text='<%# Bind("back_id") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="計費類別">
                            <ItemTemplate>
                                <asp:DropDownList ID="ddl_kind" runat="server" Width="100px" OnSelectedIndexChanged="ddl_kind_SelectedIndexChanged"></asp:DropDownList>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="退廠箱號">
                            <ItemTemplate>
                                <asp:Label ID="lbl_boxid" runat="server" Text='<%# Bind("boxid") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="貨號">
                            <ItemTemplate>
                                <asp:Label ID="lbl_item_id" runat="server" Text='<%# Bind("item_id") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="品名規格">
                            <ItemTemplate>
                                <asp:Label ID="lbl_name" runat="server" Text='<%# Bind("name") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="退廠量">
                            <ItemTemplate>
                                <asp:Label ID="lbl_qty" runat="server" Text='<%# Bind("qty") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="計費量">
                            <ItemTemplate>
                                <asp:TextBox ID="txb_qty_real" runat="server" Text='<%# Bind("qty_real") %>' Width="100px" OnTextChanged="txb_qty_real_TextChanged"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </tr>
        </table>
        <asp:Button ID="btn_CloseDateConfirm" runat="server" Text="更新本頁資訊" OnClick="btn_CloseDateConfirm_Click" />
    </div>
    <asp:Label ID="Message" runat="server"></asp:Label>
</asp:Content>
