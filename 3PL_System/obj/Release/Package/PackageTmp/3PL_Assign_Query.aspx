<%@ Page Language="C#" MasterPageFile="~/3PLMasterPage.Master" AutoEventWireup="true"
    CodeBehind="3PL_Assign_Query.aspx.cs" Inherits="_3PL_System._3PL_Assign_Query" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script type="text/javascript">
        function show_table1() { <%=xtable1.ClientID %>.style.display = 'inline'; }
        function hide_table1() { <%=xtable1.ClientID %>.style.display = 'none'; }
    </script>
    <script type="text/javascript">
        $(document).ready(function() {
            $("#<%=txb_D_qthe_ContractS_Qry.ClientID %>").datepicker({ dateFormat: 'yy/mm/dd' });
            $("#<%=txb_D_qthe_ContractE_Qry.ClientID %>").datepicker({ dateFormat: 'yy/mm/dd' });
        });
    </script>
    <title>派工單查詢</title>
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
                <asp:Label ID="lbl_Quotation_Query" runat="server" Text="派工單資料"></asp:Label>
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
                <td class="EditTD1">派工單號：
                </td>
                <td>
                    <asp:TextBox ID="Txb_Wk_Id_Query" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="EditTD1">預計完工日：
                </td>
                <td>
                    <asp:TextBox ID="txb_D_qthe_ContractS_Qry" runat="server" />
                    至
                    <asp:TextBox ID="txb_D_qthe_ContractE_Qry" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="EditTD1">派工狀態：
                </td>
                <td>
                    <asp:DropDownList ID="DDL_AssignStatusList" runat="server" />
                    <asp:CheckBox ID="Chk_ShowStatusIsZero" runat="server" Text="顯示作廢單據" />
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Button ID="Btn_Query" runat="server" Text="查詢" OnClick="Btn_Query_Click" />
                </td>
            </tr>
        </table>
    </asp:Panel>
    <asp:UpdatePanel ID="UpdPanel_Assign_HeadList" runat="server" UpdateMode="Always">
        <ContentTemplate>
            <table class="tborder" style="width:100%">
                <tr>
                    <td>
                        <asp:GridView ID="GV_Quotation_Query" runat="server" Width="100%" AutoGenerateColumns="False"
                            CssClass="GVStyle" OnRowCommand="GV_Quotation_Query_RowCommand" OnRowDataBound="GV_Quotation_Query_RowDataBound"
                            PageSize="5" AllowPaging="True" OnPageIndexChanging="GV_Quotation_Query_PageIndexChanging">
                            <HeaderStyle CssClass="GVHead" />
                            <RowStyle CssClass="one" />
                            <AlternatingRowStyle CssClass="two" />
                            <PagerStyle CssClass="GVPage" />
                            <EmptyDataRowStyle HorizontalAlign="Center" />
                            <Columns>
                                <asp:TemplateField HeaderText="簽核">
                                    <ItemTemplate>
                                        <asp:Button ID="Btn_SignOffOk" runat="server" Text='<%# Bind("OkbuttonName") %>'
                                            CommandName="SignOffOk"></asp:Button>
                                        <asp:Button ID="Btn_SignOffCancel" runat="server" Text='<%# Bind("NobuttonName") %>'
                                            CommandName="SignOffCancel" OnClientClick="Javascript:show_table1()"></asp:Button>
                                        <asp:HiddenField ID="IsOK" runat="server" Value='<%# Bind("IsOK") %>' />
                                        <asp:HiddenField ID="Status" runat="server" Value='<%# Bind("Status") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="派工單號">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="Lbl_I_qthe_PLNO" runat="server" Text='<%# Bind("Wk_Id") %>' OnClick="Wk_Id_Select"></asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="狀態">
                                    <ItemTemplate>
                                        <asp:Button ID="Lb_StatusName" runat="server" Text='<%# Bind("StatusName") %>' CommandName="StatusName"></asp:Button>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="FreeId" HeaderText="報價單號" />
                                <asp:BoundField DataField="SupID" HeaderText="供應商名稱" />
                                <asp:TemplateField HeaderText="派工日期">
                                    <ItemTemplate>
                                        <asp:Label ID="Label1" runat="server" Text='<%# Bind("Wk_Date", "{0:yyyy/MM/dd}") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="預計完工日">
                                    <ItemTemplate>
                                        <asp:Label ID="Label2" runat="server" Text='<%# Bind("EtaDate", "{0:yyyy/MM/dd}") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="Memo" HeaderText="備註" />
                                <asp:BoundField DataField="DCName" HeaderText="倉別" />
                            </Columns>
                        </asp:GridView>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:UpdatePanel ID="UpdPanel_Assgin_Detail" runat="server" UpdateMode="Always">
        <ContentTemplate>
            <div id="Div_Assign_New" runat="server" visible="false">
                <hr size="5" color="Blue" />
                <asp:Label ID="Label5" Text="派工單單頭" runat="server" ForeColor="Blue"></asp:Label>
                <table class="tborder" id="AssignHead" width="100%">
                    <tr>
                        <td class="EditTD1">報價單號
                        </td>
                        <td>
                            <asp:TextBox ID="Txb_S_qthe_PLNO" runat="server" Enabled="False"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="EditTD1">派工日期
                        </td>
                        <td>
                            <asp:TextBox ID="Txb_Wk_Date" runat="server" Enabled="False"></asp:TextBox>
                        </td>
                        <td class="EditTD1">派工類別
                        </td>
                        <td>
                            <asp:TextBox ID="Txb_Wk_ClassName" runat="server" Enabled="False"></asp:TextBox>
                        </td>
                        <td class="EditTD1">倉別
                        </td>
                        <td>
                            <asp:DropDownList ID="DDL_DC" runat="server" Enabled="False">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td class="EditTD1">預計完工日
                        </td>
                        <td>
                            <asp:TextBox ID="Txb_EtaDate" runat="server" Enabled="False"></asp:TextBox>
                        </td>
                        <td class="EditTD1">派工部門/人員
                        </td>
                        <td>
                            <asp:TextBox ID="Txb_CreateUser" runat="server" Enabled="False"></asp:TextBox>
                        </td>
                        <td class="EditTD1">處理單位
                        </td>
                        <td>
                            <asp:TextBox ID="Txb_Wk_Unit" runat="server" Enabled="False"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="EditTD1">供應商編號
                        </td>
                        <td>
                            <asp:TextBox ID="Txb_SupID" runat="server" Enabled="False"></asp:TextBox>
                        </td>
                        <td class="EditTD1">實際完工日
                        </td>
                        <td>
                            <asp:TextBox ID="Txb_ActDate" runat="server" Enabled="False"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="EditTD1">備註
                        </td>
                        <td colspan="5">
                            <textarea id="txa_Memo" runat="server" style="width: 95%; font-size: large; background-image: linear-gradient(to bottom,#CCCCB2,#E6E6E6); background-color: #CCCCB2;"
                                rows="3" readonly="readonly"></textarea>
                        </td>
                    </tr>
                </table>
                <hr size="5" color="Blue" />
                <asp:Label ID="Label6" Text="派工單內容" runat="server" ForeColor="Blue"></asp:Label>
                <table class="tborder" id="AssignClass" width="100%">
                    <tr>
                        <td>
                            <asp:GridView ID="GV_Quotation_Detail_New" runat="server" Width="100%" AutoGenerateColumns="False"
                                CssClass="GVStyle">
                                <HeaderStyle CssClass="GVHead" />
                                <RowStyle CssClass="one" />
                                <AlternatingRowStyle CssClass="two" />
                                <PagerStyle CssClass="GVPage" />
                                <EmptyDataRowStyle HorizontalAlign="Center" />
                                <Columns>
                                    <asp:BoundField DataField="Seq" HeaderText="序號" />
                                    <asp:BoundField DataField="Wk_ClassNm" HeaderText="分類名稱" />
                                    <asp:BoundField DataField="Qty" HeaderText="派工數量" />
                                    <asp:BoundField DataField="RealQty" HeaderText="完工數量" />
                                    <asp:BoundField DataField="Unit" HeaderText="單位" />
                                    <asp:BoundField DataField="PONO" HeaderText="PO單號" />
                                    <asp:BoundField DataField="itemno" HeaderText="貨號" />
                                </Columns>
                            </asp:GridView>
                        </td>
                    </tr>
                </table>
                <asp:HiddenField ID="hidTotal_I_qthe_seq" runat="server" />
                <asp:Button ID="Btn_Update" runat="server" Text="更新派工單明細內容" OnClick="Btn_Update_Click"
                    Visible="false" />
                <asp:Button ID="Btn_Print" runat="server" Text="列印派工單" OnClick="Btn_Print_Click"
                    Visible="false" />
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:UpdatePanel ID="UpdPanel_Cost_Detail" runat="server" UpdateMode="Always">
        <ContentTemplate>
            <div id="Div_Cost" runat="server" visible="false">
                <hr size="5" color="Blue" />
                <asp:Label ID="Label4" Text="成本單內容" runat="server" ForeColor="Blue"></asp:Label>
                <table class="tborder" id="Table2" width="100%">
                    <tr>
                        <td>
                            <asp:GridView ID="GV_Cost" runat="server" Width="100%" AutoGenerateColumns="False"
                                CssClass="GVStyle">
                                <HeaderStyle CssClass="GVHead" />
                                <RowStyle CssClass="one" />
                                <AlternatingRowStyle CssClass="two" />
                                <PagerStyle CssClass="GVPage" />
                                <EmptyDataRowStyle HorizontalAlign="Center" />
                                <Columns>
                                    <asp:BoundField DataField="CostTypeName" HeaderText="成本類別" />
                                    <asp:BoundField DataField="CostSeq" HeaderText="序號" />
                                    <asp:BoundField DataField="CostName" HeaderText="作業類別" />
                                    <asp:BoundField DataField="UnitIDName" HeaderText="計價單位" />
                                    <asp:BoundField DataField="WorkHr" HeaderText="數量" />
                                    <asp:BoundField DataField="CostFree" HeaderText="成本" />
                                    <asp:BoundField DataField="Total" HeaderText="總計" />
                                </Columns>
                            </asp:GridView>
                        </td>
                    </tr>
                </table>
                <asp:Button ID="Btn_Update_Cost" runat="server" Text="更新成本單明細內容" OnClick="Btn_Update_Cost_Click"
                    Visible="false" />
                <asp:Button ID="Btn_Print_Cost" runat="server" Text="列印成本單" OnClick="Btn_Print_Cost_Click"
                    Visible="false" />
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:UpdatePanel ID="UpdPanel_SignOffBack" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div id="xtable1" class="myDIV" runat="server">
                <div class="myBackDIV">
                    <table class="myMainSubject">
                        <tr>
                            <td class="PageTitle" colspan="4">
                                <asp:Label ID="lbl_TypeFee" runat="server" Text="派工單退回"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="EditTD1">
                                <asp:Label ID="lbl_ObjNo" runat="server" Text="派工單號:"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="txb_ObjNo" runat="server"></asp:Label>
                            </td>
                            <td class="EditTD1">
                                <asp:Label ID="Label7" runat="server" Text="狀態:"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="txb_Status" runat="server" Visible="false"></asp:Label>
                                <asp:Label ID="txb_StatusName" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="EditTD1">
                                <asp:Label ID="lbl_ObjName" runat="server" Text="退回理由:"></asp:Label>
                            </td>
                            <td colspan="3">
                                <asp:TextBox ID="txb_ObjName" runat="server" Height="150px" Width="100%"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Button ID="btn_SignOffback" runat="server" Text="確定" OnClick="btn_SignOffback_Click" />
                                <asp:Button ID="btn_Cancel" runat="server" Text="取消" />
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
