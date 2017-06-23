<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MD5test.aspx.cs" Inherits="_3PL_System.MD5test" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            加密前文字<asp:TextBox ID="TextBox1" runat="server" Width="80%"></asp:TextBox>
            <br />
            <asp:Button ID="Button1" runat="server" Text="Button" OnClick="Button1_Click" />
            <br />
            加密後文字<asp:TextBox ID="TextBox2" runat="server" Width="80%"></asp:TextBox>
            <br />

            <br />
            密文文字<asp:TextBox ID="TextBox3" runat="server" Width="80%"></asp:TextBox>
            <br />
            <asp:Button ID="Button2" runat="server" Text="Button" OnClick="Button2_Click" />
            <br />
            解密後文字<asp:TextBox ID="TextBox4" runat="server" Width="80%"></asp:TextBox>

            <br />
            AES_Decode 密文文字<asp:TextBox ID="TextBox5" runat="server" Width="80%"></asp:TextBox>
            <br />
            <asp:Button ID="Button3" runat="server" Text="Button" OnClick="Button3_Click" />
            <br />
            解密後文字<asp:TextBox ID="TextBox6" runat="server" Width="80%"></asp:TextBox>
        </div>
    </form>
</body>
</html>
