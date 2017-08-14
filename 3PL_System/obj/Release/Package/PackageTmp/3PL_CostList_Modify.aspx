<%@ Page Language="C#" MasterPageFile="~/3PLMasterPage.Master" AutoEventWireup="true" CodeBehind="3PL_CostList_Modify.aspx.cs"
    Inherits="_3PL_System._3PL_CostList_Modify" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {
            $("#Txb_D_qthe_ContractS").datepicker({ dateFormat: 'yy/mm/dd' });
            $("#Txb_D_qthe_ContractE").datepicker({ dateFormat: 'yy/mm/dd' });
        });
    </script>

    <title>成本單修改</title>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table class="tborder" style="width:100%">
        <tr>
            <td class="PageTitle">
                <asp:Label ID="lbl_Quotation" runat="server" Text="成本單設定"></asp:Label>
            </td>
        </tr>
        <tr>
            <td class="HeaderStyle ">
                <asp:Label ID="lbl_Quotation_Query" runat="server" Text="成本單新增"></asp:Label>
            </td>
        </tr>
    </table>
    <div id="Quotation_Head_New" runat="server">
        <table class="tborder" id="AssignHead" width="100%">
            <tr>
                <td class="EditTD1">
                    <asp:Label ID="Lbl_Wk_Date" runat="server" Text="派工日期"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="Txb_Wk_Date" runat="server" Enabled="False"></asp:TextBox>
                </td>
                <td class="EditTD1">
                    <asp:Label ID="Lbl_Wk_ClassName" runat="server" Text="派工類別"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="Txb_Wk_ClassName" runat="server" Enabled="False"></asp:TextBox>
                </td>
                <td class="EditTD1">
                    <asp:Label ID="Lbl_DC" runat="server" Text="倉別"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="DDL_DC" runat="server" Enabled="False">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td class="EditTD1">
                    <asp:Label ID="Lbl_EtaDate" runat="server" Text="預計完工日"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="Txb_EtaDate" runat="server" Enabled="False"></asp:TextBox>
                </td>
                <td class="EditTD1">
                    <asp:Label ID="Lbl_CreateUser" runat="server" Text="派工部門/人員"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="Txb_CreateUser" runat="server" Enabled="False"></asp:TextBox>
                </td>
                <td class="EditTD1">
                    <asp:Label ID="Lbl_Wk_Unit" runat="server" Text="處理單位"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="Txb_Wk_Unit" runat="server" Enabled="False"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="EditTD1">
                    <asp:Label ID="Lbl_SupID" runat="server" Text="供應商編號"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="Txb_SupID" runat="server" Enabled="False"></asp:TextBox>
                </td>
                <td class="EditTD1">
                    <asp:Label ID="Lbl_ActDate" runat="server" Text="實際完工日"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="Txb_ActDate" runat="server" Enabled="False"></asp:TextBox>
                </td>
            </tr>
        </table>
    </div>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Always">
        <ContentTemplate>
            <div id="DIV_Quotation_Detail_New" runat="server" visible="false">
                <asp:GridView ID="GV_Quotation_Detail_New" runat="server" Width="100%" AutoGenerateColumns="False"
                    CssClass="GVStyle" AllowPaging="false" PageSize="5" OnPageIndexChanging="GV_Quotation_Detail_New_PageIndexChanging">
                    <HeaderStyle CssClass="GVHead" />
                    <RowStyle CssClass="one" />
                    <AlternatingRowStyle CssClass="two" />
                    <PagerStyle CssClass="GVPage" />
                    <EmptyDataRowStyle HorizontalAlign="Center" />
                    <Columns>
                        <asp:TemplateField HeaderText="成本類別">
                            <ItemTemplate>
                                <asp:Label ID="Lbl_CostTypeName" runat="server" Text='<%# Bind("CostTypeName") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="序號">
                            <ItemTemplate>
                                <asp:Label ID="Txb_CostSeq" runat="server" Text='<%# Bind("CostSeq") %>'>LinkButton</asp:Label>
                                <asp:HiddenField ID="hid_Sn" runat="server" Value='<%# Bind("Sn") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="作業類別">
                            <ItemTemplate>
                                <asp:Label ID="Lbl_CostName" runat="server" Text='<%# Bind("CostName") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="計價單位">
                            <ItemTemplate>
                                <asp:Label ID="Lbl_UnitIDName" runat="server" Text='<%# Bind("UnitIDName") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="數量">
                            <ItemTemplate>
                                <asp:TextBox ID="Txb_WorkHr" runat="server" Text='<%# Bind("WorkHr") %>'></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="成本">
                            <ItemTemplate>
                                <asp:Label ID="Lbl_CostFree" runat="server" Text='<%# Bind("CostFree") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="總計">
                            <ItemTemplate>
                                <asp:Label ID="Lbl_Total" runat="server" Text='<%# Bind("Total") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>
            <asp:Button ID="Btn_Detail_New_Update" runat="server" Text="確定提交更新" OnClick="Btn_Detail_New_Update_Click" />
            <asp:Button ID="Btn_Detail_New_Update_Leave" runat="server" Text="取消修改單據" OnClick="Btn_Detail_New_Update_Leave_Click" />
            <asp:HiddenField ID="hidTotal_I_qthe_seq" runat="server" />
            <asp:HiddenField ID="hidTotal_I_qtde_seq" runat="server" />
            <asp:HiddenField ID="hidTotal_I_qtde_Detailseq" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
