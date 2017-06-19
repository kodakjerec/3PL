<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Main.aspx.cs" Inherits="_3PL_System.PONotConfirm.Alarm_PONotConfirm" %>

<html xmlns="http://www.w3.org/1999/xhtml" lang="zh-tw">
<head runat="server">
    <meta charset="utf-8">
    <meta name="viewport" content="width=device-width, initial-scale=1">

    <link rel="stylesheet" href="../CSS/style.css" type="text/css" />
    <link rel="stylesheet" href="../js/bootstrap-3.3.7-dist/css/bootstrap.css" type="text/css" />
    <link rel="stylesheet" href="../js/jquery-ui-1.12.1/jquery-ui.css" type="text/css" />

    <script type="text/javascript" src="../js/jquery-ui-1.12.1/jquery-1.12.4.min.js"></script>
    <script type="text/javascript" src="../js/bootstrap-3.3.7-dist/js/bootstrap.min.js"></script>
    <script type="text/javascript" src="../js/jquery-ui-1.12.1/jquery-ui.min.js"></script>
    <script type="text/javascript">
        // 不直接引用 html id，因為伺服器控制項對應的是 ClientID，而ClientID與控制項層次有關，不利程式碼移植
        // 因此盡可能選擇直接傳遞物件，通過 DOM 獲取相關的父控制項和子控制項。
        function CBL_SingleChoice(sender) {
            var container = sender.parentNode;
            if (container.tagName.toUpperCase() == "TD") { // table 布局，否則為span布局
                container = container.parentNode.parentNode; // 層次: <table><tr><td><input />
            }
            var chkList = container.getElementsByTagName("input");

            for (var i = 0; i < chkList.length; i++) {
                chkList[i].checked = false;
            }
            sender.checked = true;
        }
        $(document).ready(function () {
            //讀取小視窗
            $('#progressbar').progressbar({
                value: false
            });
            $('#divProgress').dialog({
                autoOpen: false,
                modal: true
            });
        });
        //讀取小視窗
        function ShowProgressBar() {
            $('#divProgress').dialog('open');
        }
        function HideProgressBar() {
            $('#divProgress').dialog('close');
        }
    </script>
    <style type="text/css">
        .Alarm_Label {
            color: red;
            font-size: x-large;
            font-family: Arial;
        }

        .Btn_refresh {
            font-size: large;
        }
    </style>
    <title>進貨單未確認通知</title>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <table class="tborder" align="center">
                    <tr>
                        <td class="EditTD1">
                            <asp:Label ID="Label3" runat="server" ForeColor="Red" Text="*"></asp:Label>
                            <asp:Label ID="lbl_S_qthe_SiteNo" runat="server" Text="寄倉倉別："></asp:Label>
                        </td>
                        <td>
                            <asp:CheckBoxList ID="CB_siteList" runat="server" RepeatDirection="Horizontal">
                                <asp:ListItem Value="DC01" onclick="CBL_SingleChoice(this);">觀音</asp:ListItem>
                                <asp:ListItem Value="DC03" onclick="CBL_SingleChoice(this);">梧棲</asp:ListItem>
                                <asp:ListItem Value="DC02" onclick="CBL_SingleChoice(this);">岡山</asp:ListItem>
                            </asp:CheckBoxList>
                        </td>
                    </tr>
                    <tr>
                        <td align="center" colspan="2">
                            <asp:Button ID="Btn_Refresh" runat="server" Text="重新整理" OnClick="Btn_Refresh_Click" CssClass="Btn_refresh" OnClientClick="ShowProgressBar()" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <asp:Label ID="Label1" runat="server" Text="重新整理倒數計時: 59 分 59 秒" CssClass="Alarm_Label"></asp:Label>
                            <asp:Timer ID="Timer1" runat="server" Interval="1000" OnTick="Timer1_Tick">
                            </asp:Timer>
                        </td>
                    </tr>
                </table>
                <asp:GridView ID="GV_FillForm_Detail" runat="server" Width="100%" AutoGenerateColumns="False"
                    CssClass="GVStyle">
                    <HeaderStyle CssClass="GVHead" />
                    <RowStyle CssClass="one" />
                    <PagerStyle CssClass="GVPage" />
                    <EmptyDataRowStyle HorizontalAlign="Center" />
                    <Columns>
                        <asp:BoundField DataField="Tcount" HeaderText="三天內總單據" />
                        <asp:BoundField DataField="ConfirmCount" HeaderText="已確認" />
                        <asp:BoundField DataField="CancelCount" HeaderText="日結取消" />
                        <asp:BoundField DataField="leftCount" HeaderText="尚未確認完畢" />
                    </Columns>
                </asp:GridView>
                <p></p>
                <asp:GridView ID="GV_CheckList" runat="server" Width="100%" AutoGenerateColumns="False"
                    CssClass="GVStyle">
                    <HeaderStyle CssClass="GVHead" />
                    <RowStyle CssClass="one" />
                    <PagerStyle CssClass="GVPage" />
                    <EmptyDataRowStyle HorizontalAlign="Center" />
                    <Columns>
                        <asp:BoundField DataField="S_rech_erpid" HeaderText="PO單" />
                        <asp:BoundField DataField="L_recr_id" HeaderText="驗收單" />
                        <asp:BoundField DataField="S_optd_name" HeaderText="單據狀態" />
                        <asp:BoundField DataField="Tqty" HeaderText="已驗收量(PCS)" />
                    </Columns>
                </asp:GridView>
            </ContentTemplate>
        </asp:UpdatePanel>
        <%--讀取小視窗--%>
        <div id="divProgress">
            <div class="progress-label">資料處理中...</div>
            <div id="progressbar"></div>
        </div>
    </form>
</body>
</html>
