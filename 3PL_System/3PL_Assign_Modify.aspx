<%@ Page Language="C#" MasterPageFile="~/3PLMasterPage.Master" AutoEventWireup="true"
    CodeBehind="3PL_Assign_Modify.aspx.cs" Inherits="_3PL_System._3PL_Assign_Modify" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        $(document).ready(function() {
            $("#<%=Txb_Wk_Date.ClientID %>").datepicker({ dateFormat: 'yy/mm/dd' });
            $("#<%=Txb_EtaDate.ClientID %>").datepicker({ dateFormat: 'yy/mm/dd' });
        });
    </script>
    <script type="text/javascript">
        function show_table_SignOff_Back() { <%=table_SignOff_Back.ClientID %>.style.display = 'inline'; }
        function hide_table_SignOff_Back() { <%=table_SignOff_Back.ClientID %>.style.display = 'none'; }
    </script>

    <title>派工單修改</title>
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
    <div id="Quotation_Head_New" runat="server">
        <hr size="5" color="Blue" />
        <asp:Label ID="Label5" Text="派工單單頭" runat="server" ForeColor="Blue"></asp:Label>
        <table class="tborder" id="AssignHead" style="width:100%">
            <tr>
                <td class="EditTD1">派工單號
                </td>
                <td>
                    <asp:TextBox ID="txb_AssignHead_Wk_Id" runat="server" Enabled="False"></asp:TextBox>
                </td>
                <td class="EditTD1">報價單號
                </td>
                <td>
                    <asp:TextBox ID="Txb_S_qthe_PLNO" runat="server" Enabled="False"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="EditTD1">
                    <asp:Label ID="lbl_TypeIdIm0" runat="server" Text="*" ForeColor="Red"></asp:Label>
                    派工日期
                </td>
                <td>
                    <asp:TextBox ID="Txb_Wk_Date" runat="server" Enabled="False"></asp:TextBox>
                </td>
                <td class="EditTD1">
                    <asp:Label ID="lbl_TypeIdIm1" runat="server" Text="*" ForeColor="Red"></asp:Label>
                    派工類別
                </td>
                <td>
                    <asp:TextBox ID="Txb_Wk_ClassName" runat="server" Enabled="False"></asp:TextBox>
                </td>
                <td class="EditTD1">
                    <asp:Label ID="lbl_TypeIdIm2" runat="server" Text="*" ForeColor="Red"></asp:Label>
                    倉別
                </td>
                <td>
                    <asp:DropDownList ID="DDL_DC" runat="server" Enabled="False">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td class="EditTD1">
                    <asp:Label ID="lbl_TypeIdIm5" runat="server" Text="*" ForeColor="Red"></asp:Label>
                    預計完工日
                </td>
                <td>
                    <asp:TextBox ID="Txb_EtaDate" runat="server"></asp:TextBox>
                </td>
                <td class="EditTD1">
                    <asp:Label ID="lbl_TypeIdIm4" runat="server" Text="*" ForeColor="Red"></asp:Label>
                    派工部門/人員
                </td>
                <td>
                    <asp:TextBox ID="Txb_CreateUser" runat="server" Enabled="False"></asp:TextBox>
                </td>
                <td class="EditTD1">
                    <asp:Label ID="lbl_TypeIdIm3" runat="server" Text="*" ForeColor="Red"></asp:Label>
                    處理單位
                </td>
                <td>
                    <asp:TextBox ID="Txb_Wk_Unit" runat="server" Enabled="False"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="EditTD1">
                    <asp:Label ID="lbl_TypeIdIm6" runat="server" Text="*" ForeColor="Red"></asp:Label>
                    供應商編號
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
                    <textarea id="txa_Memo" runat="server" style="width: 95%; font-size: large;" rows="3"
                        maxlength="1000"></textarea>
                </td>
            </tr>
        </table>
    </div>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Always">
        <ContentTemplate>
            <asp:Button ID="Btn_Assign_Delete" runat="server" Text="作廢" OnClick="Btn_Assign_Delete_Click" />
            <hr size="5" color="Blue" />
            <asp:Label ID="Label1" Text="派工單明細" runat="server" ForeColor="Blue"></asp:Label>
            <div id="DIV_Quotation_Detail_New_POItem" runat="server" visible="false">
                <table class="tborder">
                    <tr>
                        <td class="EditTD1">PO單
                        </td>
                        <td>
                            <asp:TextBox ID="txb_D_qthe_ContractS_Qry" runat="server"></asp:TextBox>
                        </td>
                        <td class="EditTD1">貨號
                        </td>
                        <td>
                            <asp:TextBox ID="txb_D_qthe_ContractE_Qry" runat="server"></asp:TextBox>
                        </td>
                        <td>
                            <asp:Button ID="Btn_Query" runat="server" Text="產生明細" OnClick="Btn_Query_Click" />
                        </td>
                </table>
            </div>
            <div id="DIV_Quotation_Detail_New" runat="server" visible="false">
                <table class="tborder" style="width:100%">
                    <tr>
                        <td>
                            <asp:GridView ID="GV_Quotation_Detail_New" runat="server" Width="100%" AutoGenerateColumns="False"
                                CssClass="GVStyle" OnRowCommand="GV_Quotation_Detail_New_RowCommand">
                                <HeaderStyle CssClass="GVHead" />
                                <RowStyle CssClass="one" />
                                <AlternatingRowStyle CssClass="two" />
                                <PagerStyle CssClass="GVPage" />
                                <EmptyDataRowStyle HorizontalAlign="Center" />
                                <Columns>
                                    <asp:TemplateField HeaderText="操作">
                                        <ItemTemplate>
                                            <asp:Button ID="btn_GV_Quotation_Detail_New_Del" runat="server" Text='刪除' CommandName="EdtSCdata"></asp:Button>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="序號">
                                        <ItemTemplate>
                                            <asp:Label ID="Lbl_Seq" runat="server" Text='<%# Bind("Seq") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="分類名稱">
                                        <ItemTemplate>
                                            <asp:Label ID="Label2" runat="server" Text='<%# Bind("Wk_ClassNm") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="派工數量">
                                        <ItemTemplate>
                                            <asp:Label ID="Lbl_Qty" runat="server" Text='<%# Bind("Qty") %>'></asp:Label>
                                            <asp:TextBox ID="Txb_Qty" runat="server" Text='<%# Bind("Qty") %>'></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="完工數量">
                                        <ItemTemplate>
                                            <asp:Label ID="Lbl_RealQty" runat="server" Text='<%# Bind("RealQty") %>'></asp:Label>
                                            <asp:TextBox ID="Txb_RealQty" runat="server" Text='<%# Bind("RealQty") %>'></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="單位">
                                        <ItemTemplate>
                                            <asp:Label ID="Lbl_Unit" runat="server" Text='<%# Bind("Unit") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="PO單號">
                                        <ItemTemplate>
                                            <asp:Label ID="Lbl_PONO" runat="server" Text='<%# Bind("PONO") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="貨號">
                                        <ItemTemplate>
                                            <asp:Label ID="Lbl_itemno" runat="server" Text='<%# Bind("itemno") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </td>
                    </tr>
                </table>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:Button ID="Btn_Detail_New_Update" runat="server" Text="確定提交更新" OnClick="Btn_Detail_New_Update_Click" OnClientClick="DialogYes()" />
    <asp:Button ID="Btn_Detail_New_Update_Leave" runat="server" Text="取消修改單據" OnClick="Btn_Detail_New_Update_Leave_Click" />
    <asp:HiddenField ID="hidTotal_Wk_Id" runat="server" />
    <asp:HiddenField ID="hidTotal_I_qtde_seq" runat="server" />
    <asp:HiddenField ID="hidTotal_I_qtde_Detailseq" runat="server" />
    <asp:UpdatePanel ID="UpdPanel_SignOff_Back" runat="server" UpdateMode="Always">
        <ContentTemplate>
            <div id="table_SignOff_Back" class="myDIV" runat="server">
                <div class="myBackDIV">
                    <table class="myMainSubject">
                        <tr>
                            <td class="PageTitle" colspan="2">
                                <asp:Label ID="lbl_TypeFee" runat="server" Text="報價單退回"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="EditTD1">
                                <asp:Label ID="lbl_ObjNo" runat="server" Text="報價單號:"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txb_ObjNo" runat="server" Enabled="false"></asp:TextBox>
                                <asp:TextBox ID="txb_Status" runat="server" Visible="false"></asp:TextBox>
                                <asp:TextBox ID="txb_PageType" runat="server" Visible="false"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="EditTD1">
                                <asp:Label ID="lbl_ObjName" runat="server" Text="退回理由:"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txb_ObjName" runat="server" Height="150px" Width="300px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Button ID="btn_SignOff_OK" runat="server" Text="確定" OnClick="btn_SignOff_OK_Click" />
                                <asp:Button ID="btn_SignOff_Cancel" runat="server" Text="取消" />
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
            <!-- 1. 訊息視窗
                     2. 訊息視窗然後跳頁 -->
            <div id="div_Message_Confirm" class="myDIV" runat="server">
                <div class="myBackDIV">
                    <div class="myMainSubject">
                        <asp:Button ID="btn_close_div_Message_Close" Text="關閉" CssClass="btn btn-default btn-block" OnClick="btn_Close_div_Message_Close_Click" runat="server"></asp:Button>
                        <asp:Label CssClass="h3" ID="lbl_Message_YesNo" runat="server" Text="Test"></asp:Label>
                        <asp:Button ID="btn_close_div_Message_Confirm" Text="確定" CssClass="btn btn-default btn-block" OnClick="btn_close_div_Message_Confirm_Click" runat="server"></asp:Button>
                        <asp:HiddenField ID="hid_Confirm_Value" runat="server" Value="" />
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
