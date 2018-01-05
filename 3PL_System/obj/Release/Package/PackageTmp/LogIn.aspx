<%@ Page Language="C#" MasterPageFile="~/3PLMasterPage.Master" AutoEventWireup="true"
    CodeBehind="LogIn.aspx.cs" Inherits="_3PL_System.LogIn" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        body
        {
        	background-image: linear-gradient(to bottom,#CAD7D9,#E6E6E6);
        	}
        .login_table
        {
            width: 300px;
            margin-left: 40%;
            margin-right: auto;
            margin-top: 10%;
            background-image: linear-gradient(to bottom,#CAD7D9,#E6E6E6);
        }
    </style>
    <title>全聯實業3PL系統</title>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table class="tborder login_table">
        <tr>
            <td colspan="2">
                <img src="PXMART.jpg" style="width: 100%" align="center" />
            </td>
        </tr>
        <tr>
            <td>
                帳號：
            </td>
            <td>
                <asp:TextBox ID="tbx_Account" Width="100px" runat="server" autocomplete="on"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                密碼：
            </td>
            <td>
                <asp:TextBox ID="tbx_Password"  Width="100px" runat="server" TextMode="Password"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td colspan="2" align="center">
                <asp:Button ID="btn_Login" runat="server" Text="登入" OnClick="btn_Login_Click" />
            </td>
        </tr>
        <tr>
            <td colspan="2" style="font-size: small">
                注意事項：<br />
                1.初次登入系統，預設密碼請洽管理者。<br />
                2.密碼需注意大小寫之分。
                <br />
            </td>
        </tr>
    </table>
</asp:Content>
