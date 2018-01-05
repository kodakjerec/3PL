<%@ Page Language="C#" MasterPageFile="~/3PLMasterPage.Master"
    AutoEventWireup="true" CodeBehind="3PL_BaseCost_Modify.aspx.cs"
    Inherits="_3PL_System._3PL_BaseCost_Modify" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>成本費用設定</title>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table class="tborder" style="width: 100%">
        <tr>
            <td class="PageTitle">成本費用設定
            </td>
        </tr>
        <tr>
            <td class="HeaderStyle">
                <asp:Label runat="server" ID="lbl_PriceSet_Query" Text="成本費用"></asp:Label>
            </td>
        </tr>
    </table>
    <table class="tborder">
        <tr>
            <td class="EditTD1">
                <span style="color: red">*</span>
                寄倉倉別：
            </td>
            <td>
                <asp:DropDownList ID="ddl_SiteNo" runat="server"
                    OnSelectedIndexChanged="ddl_SiteNo_SelectedIndexChanged">
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td class="EditTD1">
                <span style="color: red">*</span>
                報價主類別：
            </td>
            <td>
                <asp:DropDownList ID="ddl_TypeId" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddl_TypeId_SelectedIndexChanged">
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td class="EditTD1">
                <span style="color: red">*</span>
                費用名稱：
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
                <span style="color: red">*</span>
                成本類別：
            </td>
            <td>
                <asp:DropDownList ID="ddl_CostType" runat="server">
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td class="EditTD1">
                <span style="color: red">*</span>
                成本名稱：
            </td>
            <td>
                <asp:TextBox ID="Txb_bascCostName" runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="EditTD1">
                <span style="color: red">*</span>
                單價：
            </td>
            <td>
                <asp:TextBox ID="Txb_Price" runat="server"></asp:TextBox>
            </td>
            <td class="EditTD1" width="18%">
                <span style="color: red">*</span>
                貨幣單位：
            </td>
            <td>
                <asp:TextBox ID="Txb_DollarUnit" runat="server"></asp:TextBox>
            </td>
            <td class="EditTD1">
                <span style="color: red">*</span>
                計價單位：
            </td>
            <td>
                <asp:DropDownList ID="ddl_UnitId" runat="server">
                </asp:DropDownList>
            </td>
        </tr>
    </table>
    <table class="tborder" style="width: 100%">
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
