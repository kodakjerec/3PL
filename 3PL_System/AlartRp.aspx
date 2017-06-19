<%@ Page Language="C#" MasterPageFile="~/3PLMasterPage.Master" AutoEventWireup="true"
    CodeBehind="AlartRp.aspx.cs" Inherits="_3PL_System.AlartRp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>Alarm</title>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div>
        <table class="tborder" width="100%">
            <tr>
                <td class="PageTitle">
                    <asp:Label ID="lbl_PageTital" runat="server" Text="系統通知"></asp:Label>
                </td>
            </tr>
        </table>
        <asp:UpdatePanel ID="UpdPanel_Assign_HeadList" runat="server" UpdateMode="Always">
            <ContentTemplate>
                <table class="tborder" align="center">
                    <tr>
                        <td>
                            <asp:Label ID="Label1" runat="server" Text="未完成報價單" ForeColor="Blue"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:GridView ID="GV_NotOKQuotation" runat="server" Width="100%" AutoGenerateColumns="False"
                                CssClass="GVStyle" PageSize="5" AllowPaging="True"
                                OnPageIndexChanging="GV_NotOKQuotation_PageIndexChanging" ShowFooter="True" OnRowCreated="GV_NotOKQuotation_RowCreated">
                                <HeaderStyle CssClass="GVHead" />
                                <RowStyle CssClass="one" />
                                <AlternatingRowStyle CssClass="two" />
                                <PagerStyle CssClass="GVPage" />
                                <EmptyDataRowStyle HorizontalAlign="Center" />
                                <Columns>
                                    <asp:TemplateField HeaderText="報價單號">
                                        <ItemTemplate>
                                            <asp:HyperLink ID="Lbl_報價單號" runat="server" Text='<%# Bind("報價單號") %>' Target="iframe_Index" NavigateUrl="<%= formatAssignUrl(派工單號) %>"></asp:HyperLink>
                                            <asp:HiddenField runat="server" ID="Hid_I_qthe_IsSpecial" Value='<%# Bind("I_qthe_IsSpecial") %>' />
                                            <asp:HiddenField runat="server" ID="Hid_I_qthe_Status" Value='<%# Bind("I_qthe_Status") %>' />
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:Button ID="btn_SignOff_ThisPageQuotation" runat="server" OnClick="btn_SignOff_ThisPageQuotation_Click"
                                                Text="簽核本頁報價單" />
                                        </FooterTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="供應商代號" HeaderText="供應商代號" />
                                    <asp:BoundField DataField="寄倉倉別" HeaderText="寄倉倉別" />
                                    <asp:BoundField DataField="單據狀態" HeaderText="單據狀態" />
                                </Columns>
                            </asp:GridView>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="Label2" runat="server" Text="未完成派工單" ForeColor="Blue"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:GridView ID="GV_NotOKAssign" runat="server" Width="100%" AutoGenerateColumns="False"
                                PageSize="5" AllowPaging="True" CssClass="GVStyle"
                                OnPageIndexChanging="GV_NotOKAssign_PageIndexChanging" 
                                EnableSortingAndPagingCallbacks="True" ShowFooter="True" OnRowCreated="GV_NotOKAssign_RowCreated">
                                <HeaderStyle CssClass="GVHead" />
                                <RowStyle CssClass="one" />
                                <AlternatingRowStyle CssClass="two" />
                                <PagerStyle CssClass="GVPage" />
                                <EmptyDataRowStyle HorizontalAlign="Center" />
                                <Columns>
                                    <asp:TemplateField HeaderText="派工單號">
                                        <ItemTemplate>
                                            <asp:HyperLink ID="Lbl_派工單號" runat="server"  Text='<%# Bind("派工單號") %>' Target="iframe_Index" NavigateUrl="<%= formatAssignUrl(派工單號) %>"></asp:HyperLink>
                                            <asp:HiddenField runat="server" ID="Hid_Assign_Status" Value='<%# Bind("Status") %>' />
                                            <asp:HiddenField runat="server" ID="Hid_Assign_EtaDate" Value='<%# Bind("EtaDate") %>' />
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:Button ID="btn_SignOff_ThisPageAssign" runat="server" OnClick="btn_SignOff_ThisPageAssign_Click"
                                                Text="簽核本頁派工單" />
                                        </FooterTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="供應商代號" HeaderText="供應商代號" />
                                    <asp:BoundField DataField="寄倉倉別" HeaderText="寄倉倉別" />
                                    <asp:BoundField DataField="單據狀態" HeaderText="單據狀態" />
                                </Columns>
                            </asp:GridView>
                        </td>
                    </tr>
                </table>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>
