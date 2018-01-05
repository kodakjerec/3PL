<%@ Page Language="C#" MasterPageFile="~/3PLMasterPage.Master" AutoEventWireup="true" CodeBehind="3PL_Adjust_Modify.aspx.cs"
    Inherits="_3PL_System._3PL_Adjust_Modify" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script type="text/javascript">
        function show_table_SignOff_Back() { <%=table_SignOff_Back.ClientID %>.style.display = 'inline'; }
        function hide_table_SignOff_Back() { <%=table_SignOff_Back.ClientID %>.style.display = 'none'; }
    </script>
    <title>調整單修改</title>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table class="tborder" style="width: 100%">
        <tr>
            <td class="PageTitle">
                調整單設定
            </td>
        </tr>
        <tr>
            <td class="HeaderStyle ">
                <asp:Label ID="lbl_Quotation_Query" runat="server" Text="調整單新增"></asp:Label>
            </td>
        </tr>
    </table>
    <div id="Quotation_Head_New" runat="server">
        <table class="tborder" style="width: 100%">
            <tr>
                <td class="EditTD1">調整單號
                </td>
                <td>
                    <asp:TextBox ID="lbl_Adj_Id" runat="server" Enabled="False"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="EditTD1">
                    <span style="color: red">*</span>
                    調整單類別：
                </td>
                <td>
                    <asp:DropDownList ID="DDL_Adj_Type" runat="server" OnSelectedIndexChanged="DDL_Adj_Type_SelectedIndexChanged">
                    </asp:DropDownList>
                </td>
                <td class="EditTD1">
                    <span style="color: red">*</span>
                    異動單號
                </td>
                <td>
                    <asp:TextBox ID="lbl_Adj_PageId" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="EditTD1">備註
                </td>
                <td colspan="5">
                    <textarea id="txa_Memo" runat="server" style="width: 95%;"
                        rows="3"></textarea>
                </td>
            </tr>
        </table>
        <asp:Button ID="Btn_QuotationHead_New" runat="server" Text="確定單頭" OnClick="Btn_QuotationHead_New_Click" />
    </div>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Always">
        <ContentTemplate>
            <asp:Button ID="Btn_QuotationHead_Delete" runat="server" CssClass="btn-danger" Text="作廢" OnClick="Btn_QuotationHead_Delete_Click" Visible="false" />
            <div id="DIV_Quotation_Detail_New" runat="server" visible="false">
                <asp:GridView ID="GV_Quotation_Detail_New" runat="server" Width="100%" AutoGenerateColumns="False"
                    CssClass="GVStyle" OnRowCommand="GV_Quotation_Detail_New_RowCommand" AllowPaging="true"
                    PageSize="5" OnPageIndexChanging="GV_Quotation_Detail_New_PageIndexChanging">
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
                        <asp:BoundField DataField="SEQ" HeaderText="序號" />
                        <asp:BoundField DataField="OriginID" HeaderText="異動欄位" />
                        <asp:BoundField DataField="ColName" HeaderText="欄位名稱" />
                        <asp:BoundField DataField="OriginValue" HeaderText="元內容" />
                        <asp:BoundField DataField="newValue" HeaderText="異動內容" />
                    </Columns>
                </asp:GridView>
                <table class="tborder" style="width: 100%">
                    <tr>
                        <td class="EditTD1">
                            序號：
                        </td>
                        <td>
                            <asp:TextBox ID="lbl_SN" runat="server" Enabled="False"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="EditTD1">
                            <asp:Label ID="lbl_TypeIdIm" runat="server" Text="*" ForeColor="Red"></asp:Label>
                            異動欄位：
                        </td>
                        <td>
                            <asp:DropDownList ID="ddl_OriginID" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddl_OriginId_SelectedIndexChanged">
                            </asp:DropDownList>
                            <asp:TextBox ID="lbl_ColName" runat="server" Enabled="False"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="EditTD1">
                            <asp:Label ID="Lbl_PriceIm" runat="server" Text="*" ForeColor="Red"></asp:Label>
                            元內容：
                        </td>
                        <td>
                            <asp:DropDownList ID="ddl_OriginValue" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddl_OriginValue_SelectedIndexChanged" Enabled="false">
                            </asp:DropDownList>
                            <asp:TextBox ID="txb_OriginValue" runat="server" Enabled="false"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="EditTD1">
                            <asp:Label ID="Label1" runat="server" Text="*" ForeColor="Red"></asp:Label>
                            異動內容：
                        </td>
                        <td>
                            <asp:DropDownList ID="ddl_newValue" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddl_newValue_SelectedIndexChanged">
                            </asp:DropDownList>
                            <asp:TextBox ID="txb_newValue" runat="server" Enabled="false"></asp:TextBox>
                        </td>
                    </tr>
                </table>
                <asp:Button ID="Btn_Detail_New" runat="server" Text="新增" OnClick="Btn_Detail_New_Click" />
                <asp:Button ID="Btn_Detail_Upd" runat="server" Text="更新" OnClick="Btn_Detail_Upd_Click"
                    Visible="False" />
            </div>
            <asp:HiddenField ID="hidTotal_I_qthe_seq" runat="server" />
            <asp:HiddenField ID="hidTotal_I_qtde_seq" runat="server" />
            <asp:HiddenField ID="hidTotal_I_qtde_Detailseq" runat="server" />
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
                                <asp:Label ID="lbl_TypeFee" runat="server" Text="調整單退回"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="EditTD1">
                                <asp:Label ID="lbl_ObjNo" runat="server" Text="調整單號:"></asp:Label>
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
