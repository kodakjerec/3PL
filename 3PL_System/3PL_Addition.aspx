<%@ Page Language="C#" MasterPageFile="~/3PLMasterPage.Master" AutoEventWireup="true" CodeBehind="3PL_Addition.aspx.cs" Inherits="_3PL_System._PL_Addition" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {
            $("#<%=txb_CloseDate.ClientID %>").datepicker({ dateFormat: 'yy/mm/dd' });
        });
    </script>
    <title>3PL額外功能</title>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table class="tborder" style="width:100%">
        <tr>
            <td class="PageTitle">
                3PL額外功能
            </td>
        </tr>
        <tr>
            <td class="HeaderStyle ">
               關帳日期
            </td>
        </tr>
    </table>
    <div>
        <table class="tborder">
            <tr>
                <td class="EditTD1">
                    關帳日期：
                </td>
                <td>
                    <asp:TextBox ID="txb_CloseDate" runat="server"></asp:TextBox>
                    <asp:Button ID="btn_CloseDateConfirm" runat="server" Text="確定修改" OnClick="btn_CloseDateConfirm_Click" />
                    <asp:Label ID="lbl_CloseDate" runat="server" ForeColor="Red"></asp:Label>
                </td>

            </tr>
        </table>
    </div>
</asp:Content>
