<%@ Page Language="C#" MasterPageFile="~/3PLMasterPage.Master" AutoEventWireup="true"
    CodeBehind="3PL_Assign_New.aspx.cs" Inherits="_3PL_System._3PL_Assign_New" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>派工單新增</title>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table class="tborder" style="width:100%">
        <tr>
            <td class="PageTitle">
                <asp:Label ID="lbl_Quotation" runat="server" Text="派工單設定"></asp:Label>
            </td>
        </tr>
        <tr>
            <td class="HeaderStyle ">
                <asp:Label ID="lbl_Quotation_Query" runat="server" Text="派工單新增"></asp:Label>
            </td>
        </tr>
    </table>
    <table class="tborder">
        <tr>
            <td class="EditTD1">
                單號：
            </td>
            <td>
                <asp:TextBox ID="txb_S_qthe_PLNO" runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="EditTD1">
                <asp:Label ID="Label3" runat="server" ForeColor="Red" Text="*"></asp:Label>
                寄倉倉別：
            </td>
            <td>
                <asp:DropDownList ID="ddl_S_qthe_SiteNo" runat="server">
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Button ID="Btn_CrePLNOList" runat="server" Text="查詢報價單" OnClick="Btn_CrePLNOList_Click" />
    </table>
    <div>
        <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Always">
            <ContentTemplate>
                <asp:GridView ID="GV_PriceList_Query" runat="server" Width="100%" AutoGenerateColumns="False"
                    CssClass="GVStyle" AllowPaging="True" OnRowCommand="GV_PriceList_Query_RowCommand"
                    OnPageIndexChanging="GV_PriceList_Query_PageIndexChanging" PageSize="5">
                    <HeaderStyle CssClass="GVHead" />
                    <RowStyle CssClass="one" />
                    <AlternatingRowStyle CssClass="two" />
                    <PagerStyle CssClass="GVPage" />
                    <EmptyDataRowStyle HorizontalAlign="Center" />
                    <Columns>
                        <asp:TemplateField HeaderText="操作">
                            <ItemTemplate>
                                <asp:Button ID="btn_SCdata" runat="server" Text='選擇' CommandName="EdtSCdata"></asp:Button>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="廠編">
                            <ItemTemplate>
                                <asp:Label ID="lbl_SCData_site_no" runat="server" Text='<%# Bind("S_qthe_SupdId") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="單號">
                            <ItemTemplate>
                                <asp:Label ID="lbl_SCData_PLNO" runat="server" Text='<%# Bind("S_qthe_PLNO") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="計價期間起">
                            <ItemTemplate>
                                <asp:Label ID="lbl_SCData_sc_date_s" runat="server" Text='<%# Bind("D_qthe_ContractS", "{0:yyyy/MM/dd}") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="計價期間迄">
                            <ItemTemplate>
                                <asp:Label ID="lbl_SCData_sc_date_e" runat="server" Text='<%# Bind("D_qthe_ContractE", "{0:yyyy/MM/dd}") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="報價主類別">
                            <ItemTemplate>
                                <asp:Label ID="lbl_SCData_TypeIdname" runat="server" Text='<%# Bind("S_bsda_FieldName") %>'></asp:Label>
                                <asp:HiddenField runat="server" ID="Sel_TypeId" Value='<%# Bind("I_qtde_TypeId") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="處理單位">
                            <ItemTemplate>
                                <asp:Label ID="lbl_SCData_ClassName" runat="server" Text='<%# Bind("ClassName") %>'></asp:Label>
                                <asp:HiddenField runat="server" ID="Sel_ClassId" Value='<%# Bind("ClassId") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="倉別">
                            <ItemTemplate>
                                <asp:Label ID="lbl_SCData_SiteNo" runat="server" Text='<%# Bind("S_qtde_SiteNo") %>'></asp:Label>
                                <asp:HiddenField runat="server" ID="Sel_S_qtde_SiteNo" Value='<%# Bind("S_qtde_SiteNo") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Always">
        <ContentTemplate>
            <div id="div_AssignClass_Demo" runat="server" visible="false">
                <hr style="border-top: 5px solid blue;"/>
                <asp:Label runat="server" Text="派工單單頭" ForeColor="Blue"></asp:Label>
                <asp:HiddenField runat="server" ID="hid_PLNO" Value="" />
                <asp:HiddenField runat="server" ID="hid_TypeId" Value="" />
                <asp:HiddenField runat="server" ID="hid_ClassId" Value="" />
                <table class="tborder" id="AssignHead" style="width:100%">
                    <tr>
                        <td class="EditTD1">
                            派工日期
                        </td>
                        <td>
                            <asp:TextBox ID="Txb_Wk_Date" runat="server" Enabled="False"></asp:TextBox>
                        </td>
                        <td class="EditTD1">
                            報價主類別
                        </td>
                        <td>
                            <asp:TextBox ID="Txb_Wk_ClassName" runat="server" Enabled="False"></asp:TextBox>
                        </td>
                        <td class="EditTD1">
                            倉別
                        </td>
                        <td>
                            <asp:DropDownList ID="DDL_DC" runat="server" Enabled="False">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td class="EditTD1">
                            預計完工日
                        </td>
                        <td>
                            <asp:TextBox ID="Txb_EtaDate" runat="server" Enabled="False"></asp:TextBox>
                        </td>
                        <td class="EditTD1">
                            派工部門/人員
                        </td>
                        <td>
                            <asp:TextBox ID="Txb_CreateUser" runat="server" Enabled="False"></asp:TextBox>
                        </td>
                        <td class="EditTD1">
                            處理單位
                        </td>
                        <td>
                            <asp:TextBox ID="Txb_Wk_Unit" runat="server" Enabled="False"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="EditTD1">
                            供應商編號
                        </td>
                        <td>
                            <asp:TextBox ID="Txb_SupID" runat="server" Enabled="False"></asp:TextBox>
                        </td>
                        <td class="EditTD1">
                            實際完工日
                        </td>
                        <td>
                            <asp:TextBox ID="Txb_ActDate" runat="server" Enabled="False"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="EditTD1">
                            備註
                        </td>
                        <td colspan="5">
                            <textarea id="txa_Memo" runat="server" style="width: 95%; font-size: large" rows="3"></textarea>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="6">
                            <asp:GridView ID="GV_Quotation_Detail_New" runat="server" Width="100%" AutoGenerateColumns="False"
                                CssClass="GVStyle">
                                <HeaderStyle CssClass="GVHead" />
                                <RowStyle CssClass="one" />
                                <AlternatingRowStyle CssClass="two" />
                                <PagerStyle CssClass="GVPage" />
                                <EmptyDataRowStyle HorizontalAlign="Center" />
                                <Columns>
                                    <asp:BoundField DataField="S_bcse_CostName" HeaderText="計價費用" />
                                    <asp:BoundField DataField="I_qtde_Price" HeaderText="單價" />
                                    <asp:BoundField DataField="S_bcse_DollarUnit" HeaderText="單位" />
                                    <asp:BoundField DataField="S_bsda_FieldName" HeaderText="計費單位" />
                                </Columns>
                            </asp:GridView>
                        </td>
                    </tr>
                </table>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Always">
        <ContentTemplate>
            <div id="div_AssignClass_Detail_Demo" runat="server" visible="false">
                <hr style="border-top: 5px solid blue;"/>
                <asp:Label runat="server" Text="派工單明細" ForeColor="Blue"></asp:Label>
                <table class="tborder">
                    <tr>
                        <td class="EditTD1">
                            PO單
                        </td>
                        <td>
                            <asp:TextBox ID="txb_D_qthe_ContractS_Qry" runat="server"></asp:TextBox>
                        </td>
                        <td class="EditTD1">
                            貨號
                        </td>
                        <td>
                            <asp:TextBox ID="txb_D_qthe_ContractE_Qry" runat="server"></asp:TextBox>
                        </td>
                        <td>
                            <asp:Button ID="Btn_Query" runat="server" Text="產生明細" OnClick="Btn_Query_Click" />
                </table>
                <asp:GridView ID="GridView1" runat="server" Width="100%" AutoGenerateColumns="False"
                    CssClass="GVStyle" OnRowCommand="GridView1_RowCommand">
                    <HeaderStyle CssClass="GVHead" />
                    <RowStyle CssClass="one" />
                    <AlternatingRowStyle CssClass="two" />
                    <PagerStyle CssClass="GVPage" />
                    <EmptyDataRowStyle HorizontalAlign="Center" />
                    <Columns>
                        <asp:TemplateField HeaderText="操作">
                            <ItemTemplate>
                                <asp:Button ID="btn_GridView1_Del" runat="server" Text='刪除' CommandName="EdtSCdata">
                                </asp:Button>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="S_bcse_CostName" HeaderText="計價費用" />
                        <asp:BoundField DataField="I_qtde_Price" HeaderText="單價" />
                        <asp:BoundField DataField="S_bcse_DollarUnit" HeaderText="單位" />
                        <asp:BoundField DataField="S_bsda_FieldName" HeaderText="計費單位" />
                        <asp:TemplateField HeaderText="派工數量">
                            <ItemTemplate>
                                <asp:TextBox ID="lbl_GridView1_OrdQty" runat="server" Text='<%# Bind("OrdQty") %>'></asp:TextBox>
                                <asp:HiddenField ID="lbl_I_bcse_seq" runat="server" Value='<%# Bind("I_bcse_seq") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="PO單">
                            <ItemTemplate>
                                <asp:Label ID="lbl_GridView1_PO_no" runat="server" Text='<%# Bind("PONO") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="貨號">
                            <ItemTemplate>
                                <asp:Label ID="lbl_GridView1_itemno" runat="server" Text='<%# Bind("itemno") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
                <asp:Button ID="Btn_CreatePage" runat="server" Text="產生派工單" OnClick="Btn_CreatePage_Click" />
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
