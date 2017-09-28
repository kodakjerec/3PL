<%@ Page Language="C#" MasterPageFile="~/3PLMasterPage.Master" AutoEventWireup="true" CodeBehind="Menu.aspx.cs" Inherits="_3PL_System.Menu" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        $(function () {
            $('iframe').css('height', $(window).height() - $('nav').height());

            $('#dialog_alarm').dialog({
                height: $(window).height() * 0.8,
                width: $(window).width() * 0.8,
                title: '未完成單據'
            });
            $('#dialog_Settings').dialog({
                autoOpen: false,
                height: $(window).height()*0.8,
                width: $(window).width()*0.8,
                title: '網頁設定'
            });

            //視窗調整自動改變iframe
            $(window).resize(function () {
                $('iframe').css('height', $(window).height() - $('nav').height());
            });

            //紀錄click log
            $('.menu_Clicklog').click(function () {
                $.ajax({
                    type: "POST",
                    dataType: 'text/plain',
                    url: 'handler/Handler_Log.ashx',
                    data: 'UserID=' + getCookie('UserID') + '&MenuID=' + $(this).attr('id') + '&MenuName=' + $(this).text() + '&Active=' + 'Click',
                    cache: false,
                    async: false
                });
            });

            $('#btn_Logout').click(function () {
                $.ajax({
                    type: "POST",
                    dataType: 'text/plain',
                    url: 'handler/Handler_Log.ashx',
                    data: 'UserID=' + getCookie('UserID') + '&MenuID=' + $(this).attr('id') + '&MenuName=' + $(this).text() + '&Active=' + 'Logout',
                    cache: false,
                    async: false
                });
            });
        });
        function dialog_alarm_open() {
            $('#dialog_alarm').dialog('open');
        };
        function dialog_alarm_close() {
            $('#dialog_alarm').dialog('close');
        };
        function dialog_Settings_open() {
            $('#txb_Settings_CloseTimer').val(getCookie('CloseTimer'));
            $('#dialog_Settings').dialog('open');
        };
        function dialog_Settings_close() {
            setCookie('CloseTimer', $('#txb_Settings_CloseTimer').val(), 365);
            $('#dialog_Settings').dialog('close');
        };
        function setCookie(cname, cvalue, exdays) {
            var d = new Date();
            d.setTime(d.getTime() + (exdays * 24 * 60 * 60 * 1000));
            var expires = "expires=" + d.toUTCString();
            document.cookie = cname + "=" + cvalue + ";" + expires + ";path=/";
        }
        function getCookie(cname) {
            var name = cname + "=";
            var decodedCookie = decodeURIComponent(document.cookie);
            var ca = decodedCookie.split(';');
            for (var i = 0; i < ca.length; i++) {
                var c = ca[i];
                while (c.charAt(0) == ' ') {
                    c = c.substring(1);
                }
                if (c.indexOf(name) == 0) {
                    return c.substring(name.length, c.length);
                }
            }
            return "";
        }
    </script>
    <style>
        body {
            overflow-y: hidden;
        }

        #iframe_Index, #iframe_Alarm {
            width: 100%;
            height: 100%;
            border: 0px;
        }

        nav {
            margin-bottom: 0px !important;
        }
    </style>
    <title></title>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <nav class="navbar navbar-default">
        <div class="container-fluid" id="tabs_parent">
            <div class="navbar-header">
                <button type="button" class="navbar-toggle" data-toggle="collapse" data-target="#myNavbar">
                    <span class="glyphicon glyphicon-align-justify"></span>
                </button>
            </div>
            <div class='collapse navbar-collapse' id='myNavbar'>
                <div id="tabs" runat="server">
                </div>
                <ul class="nav navbar-nav navbar-right">
                    <li id="lbl_badge" onclick="dialog_alarm_open();">
                        <a href="#" style="padding:0px 0px 0px 0px">
                            <div id="div_Quotation" runat="server">
                                報<asp:Label CssClass="badge" ID="badge_1" runat="server"></asp:Label></div>
                            <div id="div_Assign" runat="server">
                                派<asp:Label CssClass="badge" ID="badge_2" runat="server"></asp:Label></div>
                            <div id="div_Adjust" runat="server">
                                調<asp:Label CssClass="badge" ID="badge_3" runat="server"></asp:Label></div>
                        </a>
                    </li>
                    <li><a href="Menu.aspx">
                        <asp:Label ID="lbl_name" runat="server" Text="Label" CssClass="glyphicon glyphicon-user"></asp:Label></a></li>
                    <li class="dropdown">
                        <a class='dropdown-toggle' data-toggle='dropdown' href='#'><span class="glyphicon glyphicon-align-justify"></span></a>
                        <ul class='dropdown-menu'>
                            <li><a id="btn_Open_Settings" onclick="dialog_Settings_open();"><span class="glyphicon glyphicon-cog"></span>網頁設定</a></li>
                            <li><a id="btn_Logout" href="Login.aspx"><span class="glyphicon glyphicon-log-in"></span>登出</a></li>
                        </ul>
                    </li>
                </ul>
            </div>
        </div>
    </nav>

    <iframe id="iframe_Index" name="iframe_Index" src="Index.aspx"></iframe>
    <div id="dialog_alarm">
        <button id="btn_close_dialog_alarm" type="button" class="btn btn-default btn-block" onclick="dialog_alarm_close();">關閉</button>
        <iframe id="iframe_Alarm" name="iframe_Alarm" src="AlartRp.aspx"></iframe>
    </div>

    <div id="dialog_Settings">
        <button id="btn_close_dialog_Settings" type="button" class="btn btn-default btn-block" onclick="dialog_Settings_close();">儲存並關閉</button>
        <div class="input-group">
            <span class="input-group-addon">視窗關閉時間</span>
            <input id="txb_Settings_CloseTimer" type="text" class="form-control" name="msg" placeholder="請輸入毫秒 ex:60000">
            請輸入毫秒 想要60秒後自動關閉，請輸入60000"
        </div>
    </div>
</asp:Content>
