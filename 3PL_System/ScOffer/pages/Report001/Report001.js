$(
    function init() {

        //預設日期     
        var today = new Date();
        var Bdate = _3PLgetDate(1);
        var Edate = _3PLgetDate(0);
        var BMonth = Bdate.substring(0,7);
        $('#div1_input_Bdate').val(Bdate);
        $('#div1_input_Edate').val(Edate);
        $('#div2_input_BMonth').val(BMonth);
        $('#div3_input_BMonth').val(BMonth);
        $('#div4_input_BMonth').val(BMonth);
        $('#div5_input_BMonth').val(BMonth);
        $('#div6_input_BMonth').val(BMonth);

        //日期格式
        $("#div1_input_Bdate").datepicker({ dateFormat: 'yy-mm-dd' });
        $("#div1_input_Edate").datepicker({ dateFormat: 'yy-mm-dd' });
        $("#div2_input_BMonth").datepicker({ dateFormat: 'yy-mm' });
        $("#div3_input_BMonth").datepicker({ dateFormat: 'yy-mm' });
        $("#div4_input_BMonth").datepicker({ dateFormat: 'yy-mm' });
        $("#div5_input_BMonth").datepicker({ dateFormat: 'yy-mm' });
        $("#div6_input_BMonth").datepicker({ dateFormat: 'yy-mm' });

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

//查詢-SC應收表(日)
function div1_button_search() {
    var supNo = $('#div1_input_supNo').val();
    var Bdate = $('#div1_input_Bdate').val();
    var Edate = $('#div1_input_Edate').val();
    $.fn.runAjax(
        "EDI"
        , "excel"
        , "spEDI_SCreports02_6666"
        , [
            { Name: '@Object_No', Value: supNo }
            , { Name: '@Object_Code', Value: '' }
            , { Name: '@Bdate', Value: Bdate }
            , { Name: '@Edate', Value: Edate }
            , { Name: '@step', Value: '3' }
            , { Name: 'FileName', Value: 'SC應收表(日)' }
        ]);
}

//查詢-SC應收表(月)
function div2_button_search() {
    var supNo = $('#div2_input_supNo').val();

    //取得該月第一天
    var Bdate = $('#div2_input_BMonth').val() + '-' + '01';

    //取得該月最後一天
    var parts = Bdate.split('-');
    var newDate = new Date(parts[0], parts[1], parts[2]);
    newDate.setDate(newDate.getDate() - 1);
    var Edate = newDate.getFullYear() + '-' + (newDate.getMonth() + 1) + '-' + newDate.getDate();
    $.fn.runAjax(
        "EDI"
        , "excel"
        , "spEDI_SCreports02_6666"
        , [
            { Name: '@Object_No', Value: supNo }
            , { Name: '@Object_Code', Value: '' }
            , { Name: '@Bdate', Value: Bdate }
            , { Name: '@Edate', Value: Edate }
            , { Name: '@step', Value: '1' }
            , { Name: 'FileName', Value: 'SC應收表(月)' }
        ]);
}

//查詢-SC應收表(月)0888
function div3_button_search() {
    var supNo = $('#div3_input_supNo').val();

    //取得該月第一天
    var Bdate = $('#div3_input_BMonth').val() + '-' + '01';

    //取得該月最後一天
    var parts = Bdate.split('-');
    var newDate = new Date(parts[0], parts[1], parts[2]);
    newDate.setDate(newDate.getDate() - 1);
    var Edate = newDate.getFullYear() + '-' + (newDate.getMonth() + 1) + '-' + newDate.getDate();
    $.fn.runAjax(
        "EDI"
        , "excel"
        , "spEDI_SCreports02_6666"
        , [
            { Name: '@Object_No', Value: supNo }
            , { Name: '@Object_Code', Value: '' }
            , { Name: '@Bdate', Value: Bdate }
            , { Name: '@Edate', Value: Edate }
            , { Name: '@step', Value: '2' }
            , { Name: 'FileName', Value: 'SC應收表(月)0888' }
        ]);
}

//查詢-SC應收表(月)>=6666
function div4_button_search() {
    var supNo = $('#div4_input_supNo').val();

    //取得該月第一天
    var Bdate = $('#div4_input_BMonth').val() + '-' + '01';

    //取得該月最後一天
    var parts = Bdate.split('-');
    var newDate = new Date(parts[0], parts[1], parts[2]);
    newDate.setDate(newDate.getDate() - 1);
    var Edate = newDate.getFullYear() + '-' + (newDate.getMonth() + 1) + '-' + newDate.getDate();
    $.fn.runAjax(
        "EDI"
        , "excel"
        , "spEDI_SCreports02_6666"
        , [
            { Name: '@Object_No', Value: supNo }
            , { Name: '@Object_Code', Value: '' }
            , { Name: '@Bdate', Value: Bdate }
            , { Name: '@Edate', Value: Edate }
            , { Name: '@step', Value: '0' }
            , { Name: 'FileName', Value: 'SC應收表(月)大於6666' }
        ]);
}

//查詢-SC應收表(月)折扣
function div5_button_search() {
    //取得該月
    var Bdate = $('#div5_input_BMonth').val();

    $.fn.runAjax(
        "EDI"
        , "excel"
        , "spEDI_SCreports02_Discount"
        , [
            { Name: '@Month', Value: Bdate }
            , { Name: 'FileName', Value: 'SC應收表(月)折扣' }
        ]);
}

//查詢-SC應收表(月)計費上限
function div6_button_search() {
    var supNo = $('#div6_input_supNo').val();

    //取得該月第一天
    var Bdate = $('#div6_input_BMonth').val() + '-' + '01';

    //取得該月最後一天
    var parts = Bdate.split('-');
    var newDate = new Date(parts[0], parts[1], parts[2]);
    newDate.setDate(newDate.getDate() - 1);
    var Edate = newDate.getFullYear() + '-' + (newDate.getMonth() + 1) + '-' + newDate.getDate();
    $.fn.runAjax(
        "EDI"
        , "excel"
        , "spEDI_SCreports02_6666"
        , [
            { Name: '@Object_No', Value: supNo }
            , { Name: '@Object_Code', Value: '' }
            , { Name: '@Bdate', Value: Bdate }
            , { Name: '@Edate', Value: Edate }
            , { Name: '@step', Value: '4' }
            , { Name: 'FileName', Value: 'SC應收表(月)計費上限' }
        ]);
}