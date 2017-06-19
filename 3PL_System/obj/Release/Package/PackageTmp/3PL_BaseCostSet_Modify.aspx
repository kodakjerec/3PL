<%@ Page Language="C#" MasterPageFile="~/3PLMasterPage.Master" AutoEventWireup="true" CodeBehind="3PL_BaseCostSet_Modify.aspx.cs"
    Inherits="_3PL_System._3PL_BaseCostSet_Modify" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>計價費用設定</title>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table class="tborder" id="table_Detail" width="100%">
        <tr>
            <td class="PageTitle">
                <asp:Label ID="lbl_PriceSet" runat="server" Text="計價費用設定"></asp:Label>
            </td>
        </tr>
        <tr>
            <td class="HeaderStyle">
                <asp:Label ID="lbl_PriceSet_Query" runat="server" Text="計價費用"></asp:Label>
            </td>
        </tr>
    </table>
    <table class="tborder">
        <tr>
            <td class="EditTD1">
                <asp:Label ID="Label1" runat="server" Text="*" ForeColor="Red"></asp:Label>
                <asp:Label ID="lbl_SiteNo" runat="server" Text="寄倉倉別："></asp:Label>
            </td>
            <td>
                <asp:CheckBoxList ID="CheckBoxList1" runat="server" RepeatDirection="Horizontal">
                </asp:CheckBoxList>
            </td>
        </tr>
        <tr>
            <td class="EditTD1">
                <asp:Label ID="lbl_TypeIdIm" runat="server" Text="*" ForeColor="Red"></asp:Label>
                <asp:Label ID="lbl_TypeId" runat="server" Text="報價主類別："></asp:Label>
            </td>
            <td>
                <asp:DropDownList ID="ddl_TypeId" runat="server" AutoPostBack="True">
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td class="EditTD1">
                <asp:Label ID="Lbl_CostNameIm" runat="server" Text="*" ForeColor="Red"></asp:Label>
                <asp:Label ID="Lbl_CostName" runat="server" Text="費用名稱："></asp:Label>
            </td>
            <td>
                <asp:TextBox ID="Txb_CostName" runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="EditTD1">
                <asp:Label ID="Lbl_PriceIm" runat="server" Text="*" ForeColor="Red"></asp:Label>
                <asp:Label ID="Lbl_Price" runat="server" Text="單價："></asp:Label>
            </td>
            <td>
                <asp:TextBox ID="Txb_Price" runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="EditTD1">
                <asp:Label ID="Lbl_UnitIdIm" runat="server" Text="*" ForeColor="Red"></asp:Label>
                <asp:Label ID="Lbl_UnitId" runat="server" Text="計價單位："></asp:Label>
            </td>
            <td>
                <asp:DropDownList ID="ddl_UnitId" runat="server" AutoPostBack="True">
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td class="EditTD1">
                <asp:Label ID="Lbl_AccIdIm" runat="server" Text="*" ForeColor="Red"></asp:Label>
                <asp:Label ID="Lbl_AccId" runat="server" Text="會計項目："></asp:Label>
            </td>
            <td>
                <asp:DropDownList ID="ddl_AccId" runat="server" AutoPostBack="True">
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td class="EditTD1">
                <asp:Label ID="Label3" runat="server" Text="新增處理單位："></asp:Label>
            </td>
            <td>
                <asp:DropDownList ID="DDL_ClassList" runat="server" AutoPostBack="True" OnSelectedIndexChanged="DDL_ClassList_SelectedIndexChanged">
                </asp:DropDownList>
                <asp:Label ID="Lbl_DDL_ClassList" runat="server" Text="新增狀態下無法操作" ForeColor="Red"
                    Visible="False"></asp:Label>
            </td>
        </tr>
    </table>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
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
                    <asp:TemplateField HeaderText="處理單位名稱">
                        <ItemTemplate>
                            <asp:Label ID="Label1" runat="server" Text='<%# Bind("ClassName") %>'></asp:Label>
                            <asp:HiddenField ID="hid_ClassId" runat="server" Value='<%# Bind("ClassId") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
                <PagerStyle CssClass="GVPage" />
                <EmptyDataRowStyle HorizontalAlign="Center" />
            </asp:GridView>
        </ContentTemplate>
    </asp:UpdatePanel>
    <table class="tborder" id="OtherCommand" width="100%" visible="false">
        <tr>
            <td>
                <asp:CheckBox ID="Chk_IsDBLink" runat="server" Visible="False" />
                <asp:Label ID="Lbl_IsDBLink" runat="server" Text="連結資料庫" Visible="False"></asp:Label>
            </td>
            <td>
                <asp:CheckBox ID="Chk_IsDiscount" runat="server" Visible="False" />
                <asp:Label ID="Lbl_IsDiscount" runat="server" Text="有折扣" Visible="False"></asp:Label>
            </td>
            <td>
                <asp:CheckBox ID="Chk_IsFormula" runat="server" Visible="False" />
                <asp:Label ID="Lbl_IsFormula" runat="server" Text="設定公式" Visible="False"></asp:Label>
            </td>
            <td>
                <asp:CheckBox ID="Chk_IsPeriod" runat="server" Visible="False" />
                <asp:Label ID="Lbl_IsPeriod" runat="server" Text="週期性費用" Visible="False"></asp:Label>
            </td>
        </tr>
    </table>
    <table class="tborder" id="Command" width="100%">
        <tr>
            <td>
                <asp:Button ID="Btn_Upd" runat="server" Text="確定" OnClick="Btn_Upd_Click" />
                <asp:Button ID="Btn_Del" runat="server" Text="刪除" OnClick="Btn_Del_Click" />
                <asp:Button ID="Btn_Cancel" runat="server" Text="取消" OnClick="Btn_Cancel_Click" />
            </td>
        </tr>
    </table>
    <asp:HiddenField ID="hid_bcseSeq" runat="server" />
</asp:Content>
