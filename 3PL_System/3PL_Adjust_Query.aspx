<%@ Page Language="C#" MasterPageFile="~/3PLMasterPage.Master" AutoEventWireup="true"
    CodeBehind="3PL_Adjust_Query.aspx.cs" Inherits="_3PL_System._3PL_Adjust_Query" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script type="text/javascript">
        function show_table1() { <%=xtable1.ClientID %>.style.display = 'inline'; }
        function hide_table1() { <%=xtable1.ClientID %>.style.display = 'none'; }
    </script>
    <title>調整單查詢</title>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table class="tborder" style="width: 100%">
        <tr>
            <td class="PageTitle">
                <asp:Label ID="lbl_Quotation" runat="server" Text="調整單設定"></asp:Label>
            </td>
        </tr>
        <tr>
            <td class="HeaderStyle ">
                <asp:Label ID="lbl_Quotation_Query" runat="server" Text="調整單資料"></asp:Label>
            </td>
        </tr>
    </table>
    <asp:Panel ID="Pan_Quotation_Query" runat="server">
        <table class="tborder">
            <tr>
                <td class="EditTD1">
                    調整單類別：
                </td>
                <td>
                    <asp:DropDownList ID="DDL_Adj_Type" runat="server">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td class="EditTD1">單號：
                </td>
                <td>
                    <asp:TextBox ID="Txb_Wk_Id_Query" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="EditTD1">狀態：
                </td>
                <td>
                    <asp:DropDownList ID="DDL_AssignStatusList" runat="server" />
                    <asp:CheckBox ID="Chk_ShowStatusIsZero" runat="server" Text="顯示作廢單據" />
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Button ID="Btn_Query" runat="server" Text="查詢" OnClick="Btn_Query_Click" />
                    <asp:Button ID="Btn_New" runat="server" Text="新增" OnClick="Btn_New_Click" />
                </td>
            </tr>
        </table>
    </asp:Panel>
    <asp:UpdatePanel ID="UpdPanel_Assign_HeadList" runat="server" UpdateMode="Always">
        <ContentTemplate>
            <table class="tborder" style="width: 100%">
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
                                <asp:TemplateField HeaderText="調整單號">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lbl_Adj_Id" runat="server" Text='<%# Bind("Adj_Id") %>' OnClick="Wk_Id_Select"></asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="狀態">
                                    <ItemTemplate>
                                        <asp:Button ID="Lb_StatusName" runat="server" Text='<%# Bind("StatusName") %>' CommandName="StatusName"></asp:Button>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="Adj_Type_Name" HeaderText="調整類別" />
                                <asp:BoundField DataField="Adj_PageId" HeaderText="異動單號" />
                                <asp:BoundField DataField="Memo" HeaderText="備註" />
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
                <hr style="color: blue; border-width: 5px" />
                <asp:Label ID="Label5" Text="調整單單頭" runat="server" ForeColor="Blue"></asp:Label>
                <table class="tborder" id="AssignHead" style="width: 100%">
                    <tr>
                        <td class="EditTD1">調整單號
                        </td>
                        <td>
                            <asp:TextBox ID="lbl_Adj_Id" runat="server" Enabled="False"></asp:TextBox>
                        </td>
                        <td class="EditTD1">狀態
                        </td>
                        <td>
                            <asp:TextBox ID="lbl_Status" runat="server" Enabled="False"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="EditTD1">調整類別
                        </td>
                        <td>
                            <asp:TextBox ID="lbl_Ady_Type" runat="server" Enabled="False"></asp:TextBox>
                        </td>
                        <td class="EditTD1">異動單號
                        </td>
                        <td>
                            <asp:TextBox ID="lbl_Adj_PageId" runat="server" Enabled="False"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>

                        <td class="EditTD1">建單人員
                        </td>
                        <td>
                            <asp:TextBox ID="lbl_CrtUser" runat="server" Enabled="False"></asp:TextBox>
                        </td>
                        <td class="EditTD1">建立日期
                        </td>
                        <td>
                            <asp:TextBox ID="lbl_CrtDate" runat="server" Enabled="False"></asp:TextBox>

                        </td>
                        <td class="EditTD1">更新日期
                        </td>
                        <td>
                            <asp:TextBox ID="lbl_UpdDate" runat="server" Enabled="False"></asp:TextBox>
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
                <hr style="color: blue; border-width: 5px" />
                <asp:Label ID="Label6" Text="調整單內容" runat="server" ForeColor="Blue"></asp:Label>
                <table class="tborder" id="AssignClass" style="width: 100%">
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
                                    <asp:BoundField DataField="SEQ" HeaderText="序號" />
                                    <asp:BoundField DataField="OriginID" HeaderText="異動欄位" />
                                    <asp:BoundField DataField="ColName" HeaderText="欄位名稱" />
                                    <asp:BoundField DataField="OriginValue" HeaderText="元內容" />
                                    <asp:BoundField DataField="newValue" HeaderText="異動內容" />
                                </Columns>
                            </asp:GridView>
                        </td>
                    </tr>
                </table>
                <asp:Button ID="Btn_Update" runat="server" Text="更新調整單明細內容" OnClick="Btn_Update_Click"
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
                                <asp:Label ID="lbl_TypeFee" runat="server" Text="調整單退回"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="EditTD1">
                                <asp:Label ID="lbl_ObjNo" runat="server" Text="調整單號:"></asp:Label>
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
