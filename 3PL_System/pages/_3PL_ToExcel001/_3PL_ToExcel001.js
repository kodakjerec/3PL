$(
    function init() {
        $.when(getBaseData('SiteNo', { 'Value': '', 'Name': '全部' }))
       .then(function (response) {
           getlist(response, $('#div1_select_SiteNo'));
           getlist(response, $('#div2_select_SiteNo'));
           getlist(response, $('#div3_select_SiteNo'));
       });

        //預設日期
        var today = new Date();
        var Bdate = _3PLgetDate(1);
        var Edate = _3PLgetDate(0);
        var BMonth = Bdate.substring(0, 7);
        $('#div1_input_Bdate').val(Bdate);
        $('#div1_input_Edate').val(Edate);
        $('#div2_input_Bdate').val(Bdate);
        $('#div2_input_Edate').val(Edate);
        $('#div3_input_BMonth').val(BMonth);
        $('#div4_input_BMonth').val(BMonth);


        //日期格式
        $("#div1_input_Bdate").datepicker({ dateFormat: 'yy/mm/dd' });
        $("#div1_input_Edate").datepicker({ dateFormat: 'yy/mm/dd' });
        $('#div1_input_Bdate').datepicker({ dateFormat: 'yy/mm/dd' });
        $('#div1_input_Edate').datepicker({ dateFormat: 'yy/mm/dd' });
        $("#div3_input_BMonth").datepicker({ dateFormat: 'yy/mm' });
        $("#div4_input_BMonth").datepicker({ dateFormat: 'yy/mm' });

        //分頁切換
        $('#div_group_btns > button').click(function (e) {
            divShow($(this).attr('id').replace('btn', ''));
        });

        //預設分頁
        divShow(1);
    }
);

//切換頁面
function divShow(index) {
    $('#div_group_btns button').removeClass('btn-primary');
    $('#div_group_btns #btn' + index).addClass('btn-primary');
    $('#div_group_divs > div').hide();
    $('#div_group_divs #div' + index).show();
}

//查詢-報價單
function div1_button_search() {
    var supNo = $('#div1_input_supNo').val();
    var SiteNo = $('#div1_select_SiteNo').val();
    var Bdate = $('#div1_input_Bdate').val();
    var Edate = $('#div1_input_Edate').val();

    $.fn.runAjax(
        "3PL"
        , "excel"
        , "sp3PL_Web_ToExcel001"
        , [
            { Name: '@Mode', Value: '1' }
            , { Name: '@supNo', Value: supNo }
            , { Name: '@SiteNo', Value: SiteNo }
            , { Name: '@Bdate', Value: Bdate }
            , { Name: '@Edate', Value: Edate }
            , { Name: 'FileName', Value: '報價單' }
        ], function (response) {

        });
}

//查詢-派工單
function div2_button_search() {
    var supNo = $('#div2_input_supNo').val();
    var SiteNo = $('#div2_select_SiteNo').val();
    var Bdate = $('#div2_input_Bdate').val();
    var Edate = $('#div2_input_Edate').val();

    $.fn.runAjax(
        "3PL"
        , "excel"
        , "sp3PL_Web_ToExcel001"
        , [
            { Name: '@Mode', Value: '2' }
            , { Name: '@supNo', Value: supNo }
            , { Name: '@SiteNo', Value: SiteNo }
            , { Name: '@Bdate', Value: Bdate }
            , { Name: '@Edate', Value: Edate }
            , { Name: 'FileName', Value: '派工單' }
        ], function (response) {

        });
}

//查詢-月彙總表(海博)
function div3_button_search() {
    var supNo = $('#div3_input_supNo').val();
    var SiteNo = $('#div3_select_SiteNo').val();
    var BMonth = $('#div3_input_BMonth').val();

    //該月第一天
    var Bdate = BMonth + '/01';

    //取得該月最後一天
    var parts = Bdate.split("/");
    var newDate = new Date(parts[0], parts[1], parts[2]);
    newDate.setDate(newDate.getDate() - 1);
    var Edate = newDate.getFullYear() + '/' + (newDate.getMonth() + 1) + '/' + newDate.getDate();

    $.fn.runAjax(
        "3PL"
        , "excel"
        , "sp3PL_Web_ToExcel001"
        , [
            { Name: '@Mode', Value: '3' }
            , { Name: '@supNo', Value: supNo }
            , { Name: '@SiteNo', Value: SiteNo }
            , { Name: '@Bdate', Value: Bdate }
            , { Name: '@Edate', Value: Edate }
            , { Name: 'FileName', Value: '月彙總表(海博)' }
        ], function (response) {

        });
}

//查詢-月彙總表會計費用
function div4_button_search() {
    var BMonth = $('#div4_input_BMonth').val();

    //該月第一天
    var Bdate = BMonth + '/01';

    //取得該月最後一天
    var parts = Bdate.split("/");
    var newDate = new Date(parts[0], parts[1], parts[2]);
    newDate.setDate(newDate.getDate() - 1);
    var Edate = newDate.getFullYear() + '/' + (newDate.getMonth() + 1) + '/' + newDate.getDate();

    $.fn.runAjax(
    "3PL"
    , "excel"
    , "sp3PL_Web_ToExcel001"
    , [
        { Name: '@Mode', Value: '4' }
        , { Name: '@supNo', Value: '' }
        , { Name: '@SiteNo', Value: '' }
        , { Name: '@Bdate', Value: Bdate }
        , { Name: '@Edate', Value: Edate }
        , { Name: 'FileName', Value: '月彙總表會計費用' }
    ], function (response) {

    });
}