//#region 小視窗

//關閉所有視窗
function CloseAllMessageBox() {
    if ($('#divToast').length > 0) {
        $('#divToast').dialog('close');
    }
    if ($('#divMessageBox').length > 0) {
        $('#divMessageBox').dialog('close');
    }
    if ($('#divConfirmBox').length > 0) {
        $('#divConfirmBox').dialog('close');
    }
    if ($('#divrunAjaxProgress').length > 0) {
        $('#divrunAjaxProgress').dialog('close');
    }
}

//Toast視窗
function ToastShow(message, AfterCloseFunction) {
    CloseAllMessageBox();
    //小視窗如果不存在, 就新增
    if ($('#divToast').length <= 0) {
        var MessageWindow = '<div id="divToast"><p></p></div>';
        $('body').append(MessageWindow);
        $('#divToast').dialog({
            autoOpen: false,
            modal: true,
            title: '訊息',
            width: '50%',
            dialogClass: 'dialog_noTitle'
        });
    }
    $('#divToast p').html(message);

    $('#divToast').dialog('open');

    setTimeout(function () {
        $('#divToast').dialog('close');
        if (AfterCloseFunction != undefined)
            AfterCloseFunction();
    }, 1000);
}
//訊息視窗
function MessageBoxShow(message, title) {
    CloseAllMessageBox();
    //小視窗如果不存在, 就新增
    if ($('#divMessageBox').length <= 0) {
        var MessageWindow = '<div id="divMessageBox"><p></p></div>';
        $('body').append(MessageWindow);
        $('#divMessageBox').dialog({
            autoOpen: false,
            modal: true,
            title: '訊息',
            width: '50%',

            buttons: [{
                text: "關閉",
                "class": "MessageBox",
                click: function () {
                    $(this).dialog("close");
                }
            }]
        });
    }

    if (title != null && title.length > 0)
        $('.ui-dialog-title').html(title);

    $('#divMessageBox p').html(message);

    $('#divMessageBox').dialog('open');
}
//確認視窗
function ConfirmBoxShow(message, title, YesAction) {
    CloseAllMessageBox();
    //小視窗如果不存在, 就新增
    if ($('#divConfirmBox').length <= 0) {
        var MessageWindow = '<div id="divConfirmBox"><p></p></div>';
        $('body').append(MessageWindow);
        $('#divConfirmBox').dialog({
            autoOpen: false,
            modal: true,
            title: '詢問',
            width: '50%',
            buttons: [
                {
                    text: "是",
                    "class": "ConfirmBox",
                    click: function () {
                        $(this).dialog("close");
                        YesAction();
                    }
                }
            , {
                text: "否",
                "class": "ConfirmBox",
                click: function () {
                    $(this).dialog("close");
                }
            }
            ]
        });
    }

    if (title != null && title.length > 0)
        $('.ui-dialog-title').html(title);

    $('#divConfirmBox p').html(message);

    $('#divConfirmBox').dialog('open');
}
//讀取條
function LoadingProgress(Mode) {
    //小視窗如果不存在, 就新增
    if ($('#divrunAjaxProgress').length <= 0) {
        var LoadingWindow = '<div id="divrunAjaxProgress"><div class="progress-label">資料處理中...</div><div id="progressbar"></div></div>';
        $('body').append(LoadingWindow);
        //小視窗
        $('#progressbar').progressbar({
            value: false
        });
        $('#divrunAjaxProgress').dialog({
            autoOpen: false,
            modal: true,
            dialogClass: 'dialog_noTitle'
        });
    }
    if (Mode == 0)
        $('#divrunAjaxProgress').dialog('open');
    else
        $('#divrunAjaxProgress').dialog('close');
}
//#endregion

//ajax 查詢
$.fn.runAjax = function (server, mode, sqlcmd, params, success, fail) { //與後端串接的共用function
    CloseAllMessageBox();
    //#region Init
    var IsError = false;
    //#endregion

    //分析傳進來的變數
    var serverURL = 'http://' + Global_Server + '/handler/httpService.ashx';
    var obj = 'server=' + server + '&mode=' + mode + '&sqlcmd=' + encodeURIComponent(sqlcmd);
    var FileName = '';
    $.each(params, function (_key, _value) {
        obj += '&' + _value.Name + '=' + encodeURIComponent(_value.Value);
        if (_value.Name == 'FileName')
            FileName = _value.Value;
    });

    //excel 要用 GET
    var myType = '';
    switch (mode) {
        case 'excel':
            myType = 'GET';
            break;
        default:
            myType = 'POST';
            break;
    }
    //ajax原本就帶有Promise
    $.ajax({
        url: serverURL,
        type: myType,
        cache: false,
        data: obj,
        beforeSend: function () {
            LoadingProgress(0);
        },
        complete: function () {
            LoadingProgress(1);
        },
        error: function (xhr, ajaxOptions, thrownError) {

            IsError = true;
            if (xhr.status == 0) {
                MessageBoxShow('連線失敗，請重新輸入!', '錯誤');
            }
            else if (xhr.responseText.length < 10) {
                MessageBoxShow(xhr.responseText, '錯誤');
            }
            else {
                //get ErrorMessage
                var data = xhr.responseText.match('<title>.*<\/title>');
                var errMessage = '';
                if (data != null)
                    errMessage = data[0].replace('<title>', '').replace('<\/title>', '');

                MessageBoxShow(errMessage, '錯誤');
            }
            if (fail != undefined)
                fail(errMessage);
        },
        success: function (response, status, xhr) {
            switch (mode) {
                case 'excel':
                    var $ifrm = $("<iframe style='display:none' />");
                    $ifrm.attr("src", serverURL + '?' + obj);
                    $ifrm.appendTo("body");
                    $ifrm.load(function () {
                        //if the download link return a page
                        //load event will be triggered
                        MessageBoxShow('Failed to download', '錯誤');
                    });

                    //window.location = serverURL + '?' + obj;
                    if (success != undefined)
                        success('[{}]');
                    break;
                default:
                    if (success != undefined)
                        success(response);
                    break;
            }
        }
    });
};

//取得單號
$.fn._3PL_getHeadNo = function (PageType, HeadNo, ZeroCount) {
    var dfd = $.Deferred();

    var sqlcmd = "sp_3PL_GetHeadNo";
    var regex = new RegExp('-', "g");
    var todayDate = _3PLgetDate(0).replace(regex, '');
    var ht1 = [];
    ht1.push({ Name: "@Pagetype", Value: PageType });
    ht1.push({ Name: "@TodayDateStr", Value: todayDate });
    ht1.push({ Name: "@IncreaseType", Value: ZeroCount });
    $.fn.runAjax(
        DB_ScOffer_SupMonthPriceMax
        , "sp"
        , sqlcmd
        , ht1, function (response) {
            if (response != null) {
                var newHeadNo = HeadNo + todayDate + response[0].ID;
                dfd.resolve(newHeadNo);
            }
            else
                dfd.resolve('');
        });

    return dfd.promise();
}