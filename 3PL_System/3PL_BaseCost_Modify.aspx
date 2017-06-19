<%@ Page Language="C#" MasterPageFile="~/3PLMasterPage.Master"
    AutoEventWireup="true" CodeBehind="3PL_BaseCost_Modify.aspx.cs"
    Inherits="_3PL_System._3PL_BaseCost_Modify" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>成本費用設定</title>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table class="tborder" width="100%">
        <tr>
            <td class="PageTitle">
                <asp:Label ID="lbl_PriceSet" runat="server" Text="成本費用設定"></asp:Label>
            </td>
        </tr>
        <tr>
            <td class="HeaderStyle">
                <asp:Label ID="lbl_PriceSet_Query" runat="server" Text="成本費用"></asp:Label>
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
                <asp:DropDownList ID="ddl_SiteNo" runat="server"
                    OnSelectedIndexChanged="ddl_SiteNo_SelectedIndexChanged">
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td class="EditTD1">
                <asp:Label ID="lbl_TypeIdIm" runat="server" Text="*" ForeColor="Red"></asp:Label>
                <asp:Label ID="lbl_TypeId" runat="server" Text="報價主類別："></asp:Label>
            </td>
            <td>
                <asp:DropDownList ID="ddl_TypeId" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddl_TypeId_SelectedIndexChanged">
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td class="EditTD1">
                <asp:Label ID="Lbl_bcseCostNameIm" runat="server" Text="*" ForeColor="Red"></asp:Label>
                <asp:Label ID="Lbl_bcseCostName" runat="server" Text="費用名稱："></asp:Label>
            </td>
            <td>
                <asp:DropDownList ID="DDL_CostName" runat="server" AutoPostBack="True" Enabled="False">
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Button ID="Btn_SelectBaseCostSet" runat="server" Text="選擇計價費用" OnClick="Btn_SelectBaseCostSet_Click" Visible="false" />
                <asp:HiddenField ID="hid_Bcse_seq" runat="server" />
            </td>
        </tr>
    </table>
    <table class="tborder">
        <tr>
            <td class="EditTD1">
                <asp:Label ID="lbl_CostTypeIm" runat="server" Text="*" ForeColor="Red"></asp:Label>
                <asp:Label ID="lbl_CostType" runat="server" Text="成本類別："></asp:Label>
            </td>
            <td>
                <asp:DropDownList ID="ddl_CostType" runat="server">
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td class="EditTD1">
                <asp:Label ID="Lbl_CostNameIm" runat="server" Text="*" ForeColor="Red"></asp:Label>
                <asp:Label ID="Lbl_CostName" runat="server" Text="成本名稱："></asp:Label>
            </td>
            <td>
                <asp:TextBox ID="Txb_bascCostName" runat="server"></asp:TextBox>
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
            <td class="EditTD1" width="18%">
                <asp:Label ID="Lbl_DollarUnitIm" runat="server" Text="*" ForeColor="Red"></asp:Label>
                <asp:Label ID="Lbl_DollarUnit" runat="server" Text="貨幣單位："></asp:Label>
            </td>
            <td>
                <asp:TextBox ID="Txb_DollarUnit" runat="server"></asp:TextBox>
            </td>
            <td class="EditTD1">
                <asp:Label ID="Lbl_UnitIdIm" runat="server" Text="*" ForeColor="Red"></asp:Label>
                <asp:Label ID="Lbl_UnitId" runat="server" Text="計價單位："></asp:Label>
            </td>
            <td>
                <asp:DropDownList ID="ddl_UnitId" runat="server">
                </asp:DropDownList>
            </td>
        </tr>
    </table>
    <table class="tborder" width="100%">
        <tr>
            <td>
                <asp:Button ID="Btn_Upd" runat="server" Text="確定" OnClick="Btn_Upd_Click" />
                <asp:Button ID="Btn_Del" runat="server" Text="刪除" OnClick="Btn_Del_Click" />
                <asp:Button ID="Btn_Cancel" runat="server" Text="取消" OnClick="Btn_Cancel_Click" />
            </td>
        </tr>
    </table>
    <asp:HiddenField ID="hid_bascSeq" runat="server" />
</asp:Content>
