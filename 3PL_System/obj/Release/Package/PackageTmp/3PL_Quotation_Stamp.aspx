<%@ Page Language="C#" MasterPageFile="~/3PLMasterPage.Master" AutoEventWireup="true"
    CodeBehind="3PL_Quotation_Stamp.aspx.cs" Inherits="_3PL_System._3PL_Quotation_Stamp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script type="text/javascript">
        $(document).ready(function() {
            $("#<%=txb_D_qthe_ContractS_Qry.ClientID %>").datepicker({ dateFormat: 'yy/mm/dd' });
            $("#<%=txb_D_qthe_ContractE_Qry.ClientID %>").datepicker({ dateFormat: 'yy/mm/dd' });
        });
        function show_table1() { <%=xtable1.ClientID %>.style.display = 'inline'; }
        function hide_table1() { <%=xtable1.ClientID %>.style.display = 'none'; }
        function SetStampDate() { $("#<%=txb_ObjName.ClientID %>").datepicker({ dateFormat: 'yy/mm/dd' }); }
    </script>

    <title>報價單查詢</title>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table class="tborder" width="100%">
        <tr>
            <td class="PageTitle">
                <asp:Label ID="lbl_Quotation" runat="server" Text="報價單設定"></asp:Label>
            </td>
        </tr>
        <tr>
            <td class="HeaderStyle ">
                <asp:Label ID="lbl_Quotation_Query" runat="server" Text="報價單資料"></asp:Label>
            </td>
        </tr>
    </table>
    <asp:Panel ID="Pan_Quotation_Query" runat="server">
        <table class="tborder">
            <tr>
                <td class="EditTD1">供應商編號：
                </td>
                <td>
                    <asp:TextBox ID="Txb_S_qthe_SupdId" runat="server"></asp:TextBox>
                    <asp:Button ID="Btn_S_qthe_SupdId" runat="server" Text="選擇對象" OnClick="Btn_S_qthe_SupdId_Click" />
                </td>
            </tr>
            <tr>
                <td class="EditTD1">
                    <asp:Label ID="Label3" runat="server" ForeColor="Red" Text="*"></asp:Label>
                    寄倉倉別：
                </td>
                <td>
                    <asp:DropDownList ID="DDL_S_qthe_SiteNo" runat="server">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td class="EditTD1">報價單號：
                </td>
                <td>
                    <asp:TextBox ID="txb_Quotation_PLNO" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="EditTD1">報價日期：
                </td>
                <td>
                    <asp:TextBox ID="txb_D_qthe_ContractS_Qry" runat="server"></asp:TextBox>
                    至
                    <asp:TextBox ID="txb_D_qthe_ContractE_Qry" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="EditTD1">廠商用印篩選：
                </td>
                <td>
                    <asp:DropDownList ID="DDL_StampFilter" runat="server" />
                    <asp:CheckBox ID="Chk_ShowStatusIsZero" runat="server" Text="顯示作廢單據" />
                </td>
            </tr>
        </table>
        <asp:Button ID="Btn_Query" runat="server" Text="查詢" OnClick="Btn_Query_Click" />
    </asp:Panel>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div id="Div_Quotation" runat="server" visible="false">
                <table class="tborder" width="100%">
                    <tr>
                        <td>
                            <asp:GridView ID="GV_Quotation_Query" runat="server" Width="100%" AutoGenerateColumns="False"
                                CssClass="GVStyle" AllowPaging="True" PageSize="5" OnPageIndexChanging="GV_Quotation_Query_PageIndexChanging"
                                BorderColor="Gray" OnRowCommand="GV_Quotation_Query_RowCommand">
                                <HeaderStyle CssClass="GVHead" />
                                <RowStyle CssClass="one" />
                                <AlternatingRowStyle CssClass="two" />
                                <PagerStyle CssClass="GVPage" />
                                <EmptyDataRowStyle HorizontalAlign="Center" />
                                <Columns>
                                    <asp:TemplateField HeaderText="廠商用印時間">
                                        <ItemTemplate>
                                            <asp:Label ID="Label_StampDate" runat="server" Text='<%# Bind("D_qthe_StampDate", "{0:yyyy/MM/dd}") %>'></asp:Label>
                                            <asp:Button ID="Btn_Stamp" runat="server" Text='設定時間' CommandName="UseStamp"></asp:Button>
                                            <asp:HiddenField ID="IsOK" runat="server" Value='<%# Bind("IsOK") %>' />
                                            <asp:HiddenField ID="Status" runat="server" Value='<%# Bind("I_qthe_Status") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="報價單號">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="Lbl_I_qthe_PLNO" runat="server" Text='<%# Bind("S_qthe_PLNO") %>'
                                                CommandName="Select_I_qthe_PLNO"></asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="狀態">
                                        <ItemTemplate>
                                            <asp:Button ID="Lb_StatusName" runat="server" Text='<%# Bind("StatusName") %>' CommandName="StatusName"></asp:Button>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="供應商" HeaderText="供應商" />
                                    <asp:TemplateField HeaderText="報價期間起">
                                        <ItemTemplate>
                                            <asp:Label ID="Label1" runat="server" Text='<%# Bind("D_qthe_ContractS", "{0:yyyy/MM/dd}") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="報價期間迄">
                                        <ItemTemplate>
                                            <asp:Label ID="Label2" runat="server" Text='<%# Bind("D_qthe_ContractE", "{0:yyyy/MM/dd}") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="S_qthe_Memo" HeaderText="備註" />
                                    <asp:BoundField DataField="建單人" HeaderText="建單人" />
                                </Columns>
                            </asp:GridView>
                        </td>
                    </tr>
                </table>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Always">
        <ContentTemplate>
            <div id="div_Detail" runat="server" visible="false">
                <hr size="5" color="Blue" />
                <asp:Label ID="Label6" Text="報價單內容" runat="server" ForeColor="Blue"></asp:Label>
                <table class="tborder" width="100%">
                    <tr>
                        <td class="EditTD1">報價單號
                        </td>
                        <td>
                            <asp:TextBox ID="Txb_S_qthe_PLNO" runat="server" Enabled="False"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="EditTD1">供應商編號
                        </td>
                        <td>
                            <asp:TextBox ID="Txb_S_qthe_SupdId_New" runat="server" Enabled="False"></asp:TextBox>
                            <asp:TextBox ID="Txb_S_qthe_SupdName_New" runat="server" Enabled="False"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="EditTD1">寄倉倉別
                        </td>
                        <td>
                            <asp:CheckBoxList ID="CheckBoxList_Quotation" runat="server" Enabled="False" RepeatDirection="Horizontal">
                                <asp:ListItem>觀音</asp:ListItem>
                                <asp:ListItem>岡山</asp:ListItem>
                                <asp:ListItem>台中</asp:ListItem>
                            </asp:CheckBoxList>
                        </td>
                        <td class="EditTD1">建單人
                        </td>
                        <td>
                            <asp:TextBox ID="TxB_CreateNo" runat="server" Enabled="False"></asp:TextBox>
                        </td>
                        <td class="EditTD1">建單時間
                        </td>
                        <td>
                            <asp:TextBox ID="TxB_CreateTime" runat="server" Enabled="False"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="EditTD1">聯絡電話
                        </td>
                        <td>
                            <asp:TextBox ID="txb_S_qthe_TEL" runat="server" Enabled="False"></asp:TextBox>
                        </td>
                        <td class="EditTD1">聯絡窗口
                        </td>
                        <td>
                            <asp:TextBox ID="txb_S_qthe_BOSS" runat="server" Enabled="False"></asp:TextBox>
                        </td>
                        <td class="EditTD1">傳真
                        </td>
                        <td>
                            <asp:TextBox ID="txb_S_qthe_FAX" runat="server" Enabled="False"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="EditTD1">報價日期起
                        </td>
                        <td>
                            <asp:TextBox ID="Txb_D_qthe_ContractS" runat="server" Enabled="False"></asp:TextBox>
                        </td>
                        <td class="EditTD1">報價日期迄
                        </td>
                        <td>
                            <asp:TextBox ID="Txb_D_qthe_ContractE" runat="server" Enabled="False"></asp:TextBox>
                        </td>
                        <td>
                            <asp:CheckBox ID="Chk_IsSpecial" runat="server" Text="其他議價單" Enabled="False" />
                        </td>
                    </tr>
                    <tr>
                        <td class="EditTD1">備註
                        </td>
                        <td colspan="3">
                            <asp:TextBox ID="Txb_S_qthe_Memo" runat="server" Enabled="False" Width="100%"></asp:TextBox>
                        </td>
                    </tr>
                </table>
                <table class="tborder" width="100%">
                    <tr>
                        <td>
                            <asp:GridView ID="GV_Quotation_Detail_New" runat="server" Width="100%" AutoGenerateColumns="False"
                                CssClass="GVStyle" AllowPaging="True" PageSize="5" OnPageIndexChanging="GV_Quotation_Detail_New_PageIndexChanging">
                                <HeaderStyle CssClass="GVHead" />
                                <RowStyle CssClass="one" />
                                <AlternatingRowStyle CssClass="two" />
                                <PagerStyle CssClass="GVPage" />
                                <EmptyDataRowStyle HorizontalAlign="Center" />
                                <Columns>
                                    <asp:BoundField DataField="S_bsda_FieldName" HeaderText="派工類別" />
                                    <asp:BoundField DataField="S_bcse_CostName" HeaderText="計價費用" />
                                    <asp:BoundField DataField="I_qtde_Price" HeaderText="單價" DataFormatString="{0:0.##}" />
                                    <asp:BoundField DataField="S_bcse_DollarUnit" HeaderText="單位" />
                                    <asp:CheckBoxField DataField="I_qtde_IsBaseCost" HeaderText="V" />
                                    <asp:BoundField DataField="S_qtde_PriceMemo" HeaderText="單價備註" />
                                    <asp:BoundField DataField="S_qtde_Memo" HeaderText="明細備註" />
                                    <asp:BoundField DataField="S_qtde_SiteNo" HeaderText="倉別" />
                                </Columns>
                            </asp:GridView>
                        </td>
                    </tr>
                </table>
            </div>
            <asp:HiddenField ID="hidTotal_I_qthe_seq" runat="server" />
            <asp:HiddenField ID="hid_StatusName" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:UpdatePanel ID="UpdPanel_SignOffBack" runat="server" UpdateMode="Always">
        <ContentTemplate>
            <div id="xtable1" class="myDIV" runat="server">
                <div class="myBackDIV">
                    <table class="myMainSubject">
                        <tr>
                            <td class="PageTitle" colspan="4">
                                <asp:Label ID="lbl_TypeFee" runat="server" Text="報價單設定廠商用印時間"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="EditTD1">
                                <asp:Label ID="lbl_ObjNo" runat="server" Text="報價單號:"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="txb_ObjNo" runat="server"></asp:Label>
                            </td>
                            <td class="EditTD1">
                                <asp:Label ID="Label12" runat="server" Text="狀態:"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="txb_Status" runat="server" Visible="false"></asp:Label>
                                <asp:Label ID="txb_StatusName" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="EditTD1">
                                <asp:Label ID="lbl_ObjName" runat="server" Text="廠商用印時間:"></asp:Label>
                            </td>
                            <td colspan="3">
                                <asp:TextBox ID="txb_ObjName" runat="server"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Button ID="btn_UseStamp" runat="server" Text="確定" OnClick="btn_UseStamp_Click" />
                                <asp:Button ID="btn_Cancel" runat="server" Text="取消" />
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
