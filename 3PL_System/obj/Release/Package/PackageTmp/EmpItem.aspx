<%@ Page Language="C#" MasterPageFile="~/3PLMasterPage.Master" AutoEventWireup="true" CodeBehind="EmpItem.aspx.cs" Inherits="_3PL_System.EmpItem" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>管理設定</title>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div>
        <table class="tborder" style="width:100%">
            <tr>
                <td class="PageTitle">管理設定
                </td>
            </tr>
            <tr>
                <td class="HeaderStyle ">
                    <asp:Label runat="server" ID="lbl_CustInf" Text="使用者建置"></asp:Label>
                </td>
            </tr>
        </table>
        <table class="tborder" width="60%" align="center">
            <tr>
                <td class="EditTD1" width="20%">
                    <asp:Label ID="lbl_1" runat="server" Text="*" ForeColor="Red"></asp:Label>
                    <asp:Label ID="lbl_WorkId" runat="server" Text="帳號:"></asp:Label>
                </td>
                <td width="80%">
                    <asp:TextBox ID="txb_WorkId" runat="server" MaxLength="10"></asp:TextBox>
                    <asp:Label ID="lblWorkId" runat="server" Text=""></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="EditTD1" width="20%">
                    <asp:Label ID="lbl_2" runat="server" Text="*" ForeColor="Red"></asp:Label>
                    <asp:Label ID="lbl_Name" runat="server" Text="人員姓名:"></asp:Label>
                </td>
                <td width="80%">
                    <asp:TextBox ID="txb_Name" runat="server" MaxLength="10"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="EditTD1" width="20%">
                    <asp:Label ID="lbl_3" runat="server" Text="*" ForeColor="Red"></asp:Label>
                    <asp:Label ID="lbl_Class" runat="server" Text="人員類別:"></asp:Label>
                </td>
                <td width="80%">
                    <asp:DropDownList ID="ddl_Class" runat="server">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td class="EditTD1" width="20%">
                    <asp:Label ID="lbl_4" runat="server" Text="*" ForeColor="Red"></asp:Label>
                    <asp:Label ID="lbl_Psw" runat="server" Text="密碼:"></asp:Label>
                </td>
                <td width="80%">
                    <asp:TextBox ID="txb_Psw" runat="server" TextMode="Password" MaxLength="20"
                        Height="19px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="EditTD1" width="20%">
                    <asp:Label ID="lbl_5" runat="server" Text="*" ForeColor="Red"></asp:Label>
                    <asp:Label ID="lbl_RePsw" runat="server" Text="密碼確認:"></asp:Label>
                </td>
                <td width="80%">
                    <asp:TextBox ID="txb_RePsw" runat="server" TextMode="Password" MaxLength="20"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="EditTD1" width="20%">
                    <asp:Label ID="lbl_Mail" runat="server" Text="E-mail:"></asp:Label>
                </td>
                <td width="80%">
                    <asp:TextBox ID="txb_Mail" runat="server" Width="100%" MaxLength="50"></asp:TextBox>
                </td>
            </tr>
        </table>
        <table class="tborder" width="60%" align="center">
            <tr>
                <td align="center">
                    <asp:Button ID="btn_Submit" runat="server" Text="確定" OnClick="btn_Submit_Click" />
                    <%--  style="height: 21px" --%>
                    &nbsp;&nbsp;
                    <asp:Button ID="btn_Deel" runat="server" Text="刪除" OnClick="btn_Deel_Click" Style="display: none;" OnClientClick="if(!confirm('是否確定刪除？')){return false;}" />
                    &nbsp;&nbsp;
                    <asp:Button ID="btn_Cancel" runat="server" Text="取消" OnClick="btn_Cancel_Click" Style="display: none;" />

                </td>
            </tr>
        </table>
    </div>
    <asp:HiddenField ID="hid_Psw" runat="server" />
</asp:Content>
