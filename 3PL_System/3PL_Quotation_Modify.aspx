<%@ Page Language="C#" MasterPageFile="~/3PLMasterPage.Master" AutoEventWireup="true" CodeBehind="3PL_Quotation_Modify.aspx.cs"
    Inherits="_3PL_System._3PL_Quotation_Modify" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script type="text/javascript">
        $(document).ready(function() {
            $("#<%=Txb_D_qthe_ContractS.ClientID %>").datepicker({ dateFormat: 'yy/mm/dd' });
            $("#<%=Txb_D_qthe_ContractE.ClientID %>").datepicker({ dateFormat: 'yy/mm/dd' });
        });
    </script>

    <script type="text/javascript">
        function show_table_SignOff_Back() { <%=table_SignOff_Back.ClientID %>.style.display = 'inline'; }
        function hide_table_SignOff_Back() { <%=table_SignOff_Back.ClientID %>.style.display = 'none'; }
    </script>
    <title>報價單修改</title>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table class="tborder" style="width: 100%">
        <tr>
            <td class="PageTitle">報價單設定
            </td>
        </tr>
        <tr>
            <td class="HeaderStyle ">
                <asp:Label ID="lbl_Quotation_Query" runat="server" Text="報價單新增"></asp:Label>
            </td>
        </tr>
    </table>
    <div id="Quotation_Head_New" runat="server">
        <table class="tborder" style="width: 100%">
            <tr>
                <td class="EditTD1">報價單號
                </td>
                <td>
                    <asp:TextBox ID="Txb_S_qthe_PLNO" runat="server" Enabled="False"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="EditTD1">
                    <span style="color: red">*</span>
                    供應商編號
                </td>
                <td>
                    <asp:TextBox ID="Txb_S_qthe_SupdId_New" runat="server" Width="30%"></asp:TextBox>
                    <asp:TextBox ID="Txb_S_qthe_SupdName_New" runat="server" Width="30%" Enabled="false"></asp:TextBox>
                    <asp:Button ID="Btn_S_qthe_SupdId_New" runat="server" Text="選擇對象" OnClick="Btn_S_qthe_SupdId_New_Click" />
                </td>
            </tr>
            <tr>
                <td class="EditTD1">
                    <span style="color: red">*</span>
                    寄倉倉別
                </td>
                <td>
                    <asp:CheckBoxList ID="CheckBoxList_Quotation" runat="server" RepeatDirection="Horizontal">
                    </asp:CheckBoxList>
                </td>
            </tr>
            <tr>
                <td class="EditTD1">聯絡電話
                </td>
                <td>
                    <asp:TextBox ID="txb_S_qthe_TEL" runat="server" MaxLength="20"></asp:TextBox>
                </td>
                <td class="EditTD1">聯絡窗口
                </td>
                <td>
                    <asp:TextBox ID="txb_S_qthe_BOSS" runat="server" MaxLength="20"></asp:TextBox>
                </td>
                <td class="EditTD1">傳真
                </td>
                <td>
                    <asp:TextBox ID="txb_S_qthe_FAX" runat="server" MaxLength="20"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="EditTD1">
                    <span style="color: red">*</span>
                    報價日期起
                </td>
                <td>
                    <asp:TextBox ID="Txb_D_qthe_ContractS" runat="server"></asp:TextBox>
                </td>
                <td class="EditTD1">
                    <span style="color: red">*</span>
                    報價日期迄
                </td>
                <td>
                    <asp:TextBox ID="Txb_D_qthe_ContractE" runat="server"></asp:TextBox>
                </td>
                <td>
                    <asp:CheckBox ID="Chk_IsSpecial" runat="server" Text="其他議價單"
                        OnCheckedChanged="Chk_IsSpecial_CheckedChanged" AutoPostBack="True" />
                </td>
            </tr>
            <tr>
                <td class="EditTD1">備註
                </td>
                <td colspan="3">
                    <asp:TextBox ID="Txb_S_qthe_Memo" runat="server" Width="100%" MaxLength="100"></asp:TextBox>
                </td>
            </tr>
        </table>
        <asp:Button ID="Btn_QuotationHead_New" runat="server" Text="確定單頭" OnClick="Btn_QuotationHead_New_Click" />
    </div>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Always">
        <ContentTemplate>
            <asp:Button ID="Btn_QuotationHead_Delete" runat="server" CssClass="btn-danger" Text="作廢" OnClick="Btn_QuotationHead_Delete_Click" Visible="false" />
            <hr style="border-top: 5px solid blue;" />
            <div id="DIV_Quotation_Detail_New" runat="server" visible="false">
                <asp:GridView ID="GV_Quotation_Detail_New" runat="server" Width="100%" AutoGenerateColumns="False"
                    CssClass="GVStyle" OnRowCommand="GV_Quotation_Detail_New_RowCommand" AllowPaging="true"
                    PageSize="10" OnPageIndexChanging="GV_Quotation_Detail_New_PageIndexChanging">
                    <HeaderStyle CssClass="GVHead" />
                    <RowStyle CssClass="one" />
                    <AlternatingRowStyle CssClass="two" />
                    <PagerStyle CssClass="GVPage" />
                    <EmptyDataRowStyle HorizontalAlign="Center" />
                    <Columns>
                        <asp:TemplateField HeaderText="操作">
                            <ItemTemplate>
                                <asp:Button ID="UpdateButton" runat="server" Text="更新" CommandName="UpdateButton" />
                                <asp:Button ID="DeleteButton" runat="server" Text="刪除" CommandName="DeleteButton" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="序號">
                            <ItemTemplate>
                                <asp:LinkButton ID="Txb_I_qtde_Detailseq" runat="server" Text='<%# Bind("I_qtde_Detailseq") %>'>LinkButton</asp:LinkButton>
                                <asp:HiddenField ID="hid_I_qtde_seq" runat="server" Value='<%# Bind("I_qtde_seq") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="S_bsda_FieldName" HeaderText="報價主類別" />
                        <asp:BoundField DataField="S_bcse_CostName" HeaderText="計價費用" />
                        <asp:BoundField DataField="I_qtde_Price" HeaderText="單價" DataFormatString="{0:0.##}" />
                        <asp:BoundField DataField="S_bcse_DollarUnit" HeaderText="單位" />
                        <asp:BoundField DataField="S_qtde_PriceMemo" HeaderText="公式" Visible="False" />
                        <asp:BoundField DataField="S_qtde_Memo" HeaderText="備註" />
                        <asp:BoundField DataField="S_qtde_SiteNo" HeaderText="倉別" />
                        <asp:BoundField DataField="I_qtde_HaveMinimum_Name" HeaderText="最低收費" />
                    </Columns>
                </asp:GridView>
                <table class="tborder" style="width: 100%">
                    <tr>
                        <td class="EditTD1">
                            <span style="color: red">*</span>
                            報價主類別：
                        </td>
                        <td>
                            <asp:DropDownList ID="ddl_TypeId" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddl_TypeId_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                        <td class="EditTD1">
                            <span style="color: red">*</span>
                            計價費用：
                        </td>
                        <td>
                            <asp:DropDownList ID="DDL_bcseseq" runat="server" AutoPostBack="True" Enabled="false"
                                OnSelectedIndexChanged="DDL_bcseseq_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td class="EditTD1">
                            <span style="color: red">*</span>
                            單價：
                        </td>
                        <td>
                            <asp:TextBox ID="Txb_Price" runat="server" Enabled="False"></asp:TextBox>
                            <asp:Label ID="Lbl_DollarUnit" runat="server" Text="元"></asp:Label>
                        </td>
                        <td class="EditTD1">計價單位：
                        </td>
                        <td>
                            <asp:Label ID="Lbl_Unit2" runat="server" Text="PCS"></asp:Label>
                        </td>
                    </tr>
                    <%--<tr>
                        <td class="EditTD1">公式：
                        </td>
                        <td>
                            <asp:TextBox ID="Txb_PriceMemo" runat="server" Enabled="False" MaxLength="100"></asp:TextBox>
                        </td>
                        <td>
                            <asp:CheckBox ID="Chk_IsBaseCost" runat="server" Text="套用公式"
                                Enabled="False" />
                        </td>
                    </tr>--%>
                    <tr>
                        <td class="EditTD1">
                            <span style="color: red">*</span>
                            最低收費：
                        </td>
                        <td>
                            <asp:DropDownList ID="ddl_HaveMinimum" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddl_HaveMinimum_SelectedIndexChanged"></asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td class="EditTD1">備註：
                        </td>
                        <td>
                            <asp:TextBox ID="Txb_Memo" Width="100%" Height="48px" TextMode="MultiLine" runat="server" MaxLength="50"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="EditTD1">倉別：
                        </td>
                        <td>
                            <asp:CheckBoxList ID="CheckBoxList_Detail" runat="server" RepeatDirection="Horizontal">
                                <asp:ListItem>觀音</asp:ListItem>
                                <asp:ListItem>岡山</asp:ListItem>
                                <asp:ListItem>台中</asp:ListItem>
                            </asp:CheckBoxList>
                        </td>
                    </tr>
                </table>
                <div id="div_Detail_Upd">
                    <asp:Button ID="Btn_Detail_New" runat="server" Text="新增" OnClick="Btn_Detail_New_Click" />
                    <asp:Button ID="Btn_Detail_Upd" runat="server" Text="更新" Visible="False" OnClick="Btn_Detail_Upd_Click" />
                    <asp:Button ID="Btn_Detail_Upd_Cancel" runat="server" Text="取消" Visible="False" OnClick="Btn_Detail_Upd_Cancel_Click" />
                </div>
                <asp:HiddenField ID="hidTotal_I_qthe_seq" runat="server" />
                <asp:HiddenField ID="hidTotal_I_qtde_seq" runat="server" />
                <asp:HiddenField ID="hidTotal_I_qtde_Detailseq" runat="server" />
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:Button ID="Btn_Detail_New_Update" runat="server" Text="確定提交更新" OnClick="Btn_Detail_New_Update_Click"
        Visible="False" />
    <asp:Button ID="Btn_Detail_New_Update_Leave" runat="server" Text="取消修改單據" OnClick="Btn_Detail_New_Update_Leave_Click"
        Visible="False" />
    <asp:UpdatePanel ID="UpdPanel_SignOffBack" runat="server" UpdateMode="Always">
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
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
