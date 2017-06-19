<%@ Page Language="C#" MasterPageFile="~/3PLMasterPage.Master" AutoEventWireup="true" CodeBehind="Index.aspx.cs" Inherits="_3PL_System.Index" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .css_lbl_Bulletin {
            background-color: lightblue;
            color: blue;
            font-size: xx-large;
            font-weight: bold;
            text-align: center;
        }
    </style>
    <title>公告</title>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div align="center">
        <table class="tborder" style="left: 70%; width: 70%; border: 5px groove #cccccc;">
            <tr>
                <td class="css_lbl_Bulletin">
                    <asp:Label ID="lbl_Bulletin" runat="server" Text="=訊息公告="></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:label ID="txb_Bulletin" runat="server" Height="450" Width="100%"></asp:label>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
