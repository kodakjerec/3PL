<%@ Page Title="" Language="C#" MasterPageFile="~/3PLMasterPage.Master" AutoEventWireup="true" CodeBehind="3PL_Other_edi_retn_price_Settings.aspx.cs" Inherits="_3PL_System._3PL_Other_edi_retn_price_Settings" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>逆物流費用設定_設定濾除</title>
        <style>
        .myMessage {
            color:red;
            font-size:xx-large;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Always">
        <ContentTemplate>
            <asp:Button ID="btn_Supno" runat="server" Text="廠商" CssClass="btn-primary" OnClick="btn_Supno_Click" />
            <asp:Button ID="btn_itemno" runat="server" Text="貨號" CssClass="btn-default" OnClick="btn_Itemno_Click" />
            <br />
            <asp:Label ID="lbl_Message" CssClass="myMessage" runat="server" Text="."></asp:Label>

            <div id="div_Supno" runat="server" visible="true">
                <table class="tborder">
                    <tr>
                        <td class="PageTitle">廠商編號
                        </td>
                    </tr>
                    <tr>
                        <td class="HeaderStyle">新增廠商編號
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:TextBox ID="txb_SupNo_New" runat="server"></asp:TextBox>
                            <asp:Button ID="btn_SupNo_New" runat="server" Text="新增" CssClass="btn-default" OnClick="btn_SupNo_New_Click" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:GridView ID="GV_ClassList" runat="server" CssClass="GVStyle" AutoGenerateColumns="False"
                                OnRowCommand="GV_ClassList_RowCommand">
                                <HeaderStyle CssClass="GVHead" />
                                <RowStyle CssClass="one" />
                                <AlternatingRowStyle CssClass="two" />
                                <Columns>
                                    <asp:TemplateField HeaderText="操作">
                                        <ItemTemplate>
                                            <asp:Button ID="DeleteButton" runat="server" Text="刪除" CommandName="DeleteButton" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="廠商編號">
                                        <ItemTemplate>
                                            <asp:Label ID="lbl_sup_no" Text='<%# Bind("sup_no") %>' runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="del_user" HeaderText="刪除使用者" />
                                    <asp:BoundField DataField="del_date" HeaderText="刪除時間" />
                                    <asp:TemplateField HeaderText="建立時間">
                                        <ItemTemplate>
                                            <asp:Label ID="lbl_create_date" Text='<%# Bind("create_date", "{0:yyyy/MM/dd HH:mm:ss}") %>' runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                                <PagerStyle CssClass="GVPage" />
                            </asp:GridView>
                        </td>
                    </tr>
                </table>

            </div>
            <div id="div_itemno" runat="server" visible="false">
                <table class="tborder">
                    <tr>
                        <td class="PageTitle">貨號
                        </td>
                    </tr>
                    <tr>
                        <td class="HeaderStyle">新增貨號
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:TextBox ID="txb_ItemNo" runat="server"></asp:TextBox>
                            <asp:Button ID="btn_ItemNo_New" runat="server" Text="新增" CssClass="btn-default" OnClick="btn_ItemNo_New_Click" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:GridView ID="GV_ItemNoList" runat="server" CssClass="GVStyle" AutoGenerateColumns="False"
                                OnRowCommand="GV_ItemNoList_RowCommand">
                                <HeaderStyle CssClass="GVHead" />
                                <RowStyle CssClass="one" />
                                <AlternatingRowStyle CssClass="two" />
                                <Columns>
                                    <asp:TemplateField HeaderText="操作">
                                        <ItemTemplate>
                                            <asp:Button ID="DeleteButton" runat="server" Text="刪除" CommandName="DeleteButton" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="貨號">
                                        <ItemTemplate>
                                            <asp:Label ID="lbl_Item_id" Text='<%# Bind("item_id") %>' runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="del_user" HeaderText="刪除使用者" />
                                    <asp:BoundField DataField="del_date" HeaderText="刪除時間" />
                                    <asp:TemplateField HeaderText="建立時間">
                                        <ItemTemplate>
                                            <asp:Label ID="lbl_create_date" Text='<%# Bind("create_date", "{0:yyyy/MM/dd HH:mm:ss}") %>' runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                                <PagerStyle CssClass="GVPage" />
                            </asp:GridView>
                        </td>
                    </tr>
                </table>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
