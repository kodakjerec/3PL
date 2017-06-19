<%@ Page Language="C#" MasterPageFile="~/3PLMasterPage.Master" AutoEventWireup="true" CodeBehind="ChangePass.aspx.cs" Inherits="_3PL_System.ChangePass" ValidateRequest="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>管理設定</title>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div>
        <table class="tborder" width="100%">
            <tr>
                <td class="PageTitle">管理設定
                </td>
            </tr>
            <tr>
                <td class="HeaderStyle ">
                    <asp:Label ID="lbl_CustInf" runat="server" Text="密碼異動"></asp:Label>
                </td>
            </tr>
        </table>
        <table class="tborder" width="280px">
            <tr>
                <td class="EditTD1" width="50%">
                    <asp:Label ID="lbl_1" runat="server" Text="*" ForeColor="Red"></asp:Label>
                    <asp:Label ID="lbl_OldPsw" runat="server" Text="原密碼:"></asp:Label>
                </td>
                <td width="50%">
                    <asp:TextBox ID="txb_OldPsw" runat="server" TextMode="Password" MaxLength="20"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="EditTD1" width="50%">
                    <asp:Label ID="lbl_4" runat="server" Text="*" ForeColor="Red"></asp:Label>
                    <asp:Label ID="lbl_Psw" runat="server" Text="新密碼:"></asp:Label>
                </td>
                <td width="50%">
                    <asp:TextBox ID="txb_Psw" runat="server" TextMode="Password" MaxLength="20" Height="19px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="EditTD1" width="50%">
                    <asp:Label ID="lbl_5" runat="server" Text="*" ForeColor="Red"></asp:Label>
                    <asp:Label ID="lbl_RePsw" runat="server" Text="密碼確認:"></asp:Label>
                </td>
                <td width="50%">
                    <asp:TextBox ID="txb_RePsw" runat="server" TextMode="Password" MaxLength="20"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td colspan="2" align="center">
                    <asp:Button ID="btn_Submit" runat="server" Text="確定" OnClick="btn_Submit_Click" />
                    &nbsp;&nbsp;
                    <asp:Button ID="btn_Cancel" runat="server" Text="取消" OnClientClick="location.href = 'Index.aspx'; return false;" />
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
