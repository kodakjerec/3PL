<%@ Page Language="C#" MasterPageFile="~/3PLMasterPage.Master" AutoEventWireup="true" CodeBehind="PublicMemo.aspx.cs" Inherits="_3PL_System.PublicMemo" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">

        $(document).ready(function() {

            $("#<%=txb_BullDay.ClientID %>").datepicker({ dateFormat: 'yy-mm-dd' });
            $("#<%=txb_EffDateS.ClientID %>").datepicker({ dateFormat: 'yy-mm-dd' });
            $("#<%=txb_EffDateE.ClientID %>").datepicker({ dateFormat: 'yy-mm-dd' });
        });
    </script>

    <title>管理設定</title>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div>
        <table class="tborder" style="width:100%">
            <tr>
                <td class="PageTitle">
                    管理設定
                </td>
            </tr>
            <tr>
                <td class="HeaderStyle ">
                    <asp:Label ID="lbl_CustInf" runat="server" Text="系統公告"></asp:Label>
                </td>
            </tr>
        </table>
        <table class="tborder" style="width:100%">
            <tr>
                <td class="EditTD1" width="15%">
                    <asp:Label ID="lbl_Member" runat="server" Text="公告人員:"></asp:Label>
                </td>
                <td width="35%">
                    <asp:TextBox ID="txb_Member" runat="server" MaxLength="20"></asp:TextBox>
                </td>
                <td class="EditTD1" width="15%">
                    <asp:Label ID="lbl_BullDay" runat="server" Text="發佈日期:"></asp:Label>
                </td>
                <td width="35%">
                    <asp:TextBox ID="txb_BullDay" runat="server" MaxLength="10"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="EditTD1" width="15%">
                    <asp:Label ID="lbl_EffDate" runat="server" Text="公告效期:"></asp:Label>
                </td>
                <td width="85%" colspan="3">
                    <asp:TextBox ID="txb_EffDateS" runat="server" MaxLength="10"></asp:TextBox>
                    ~
                    <asp:TextBox ID="txb_EffDateE" runat="server" MaxLength="10"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="EditTD1" width="15%">
                    <asp:Label ID="lbl_Memo" runat="server" Text="公告內容:"></asp:Label>
                </td>
                <td width="85%" colspan="3" align="center">
                    <asp:TextBox ID="txb_Memo" runat="server"  Height="350" Width="95%" 
                        TextMode="MultiLine" Font-Size="14pt"></asp:TextBox>
                </td>
            </tr>
            <tr>
             <td  width="100%" align="center"  colspan="4" > 
                 <asp:Button ID="btn_Submit" runat="server" Text="發佈" 
                     onclick="btn_Submit_Click" style="height: 21px" />
             </td>
            </tr>
        </table>
    </div>
    <asp:HiddenField ID="hid_BullNo" runat="server" />
</asp:Content>