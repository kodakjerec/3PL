$(
    function init() {
        //#region Init
        $('#div_EditDialog').dialog({
            autoOpen: false,
            width: '30%',
            modal: true,
            resizable: true,
            dialogClass: 'dialog_noTitle'
        });

        //$('#div_EditDialog_input_Month').datepicker({ dateFormat: 'yymm' });

        //#endregion

        getPriceTypeList();
    });

var DB_ScOffer_SupMonthPriceMax = '134_JASON_TEST';

//#region 取得計價類別

function getPriceTypeList() {
    $.fn.runAjax(
     "3PL"
     , "sqlcmd"
     , "select I_acci_seq, S_Acci_Id, S_Acci_Name from [3PL_BaseAccounting] with(nolock) where I_acci_seq in (3,33,34)"
     , [
     ], function (response) {

         if (response != '')
             response.unshift({ I_acci_seq: 0, S_Acci_Id: "0", S_Acci_Name: "全部", _name: "" });

         for (i = 0; i < response.length; i++) {
             var _value = response[i].I_acci_seq;
             var _name = response[i].S_Acci_Id + ',' + response[i].S_Acci_Name;
             response[i]._name = _name;
             ////jqGrid
             //if (i > 0)
             //    PriceTypeList += ';';
             //PriceTypeList += _value + ":" + _name;

             //DOM
             $('#div_EditDialog_input_I_acci_seq').append($("<option></option>").attr('value', _value).text(_name));
             $('#div_search_I_acci_seq').append($("<option></option>").attr('value', _value).text(_name));
         }
         PriceTypeList = response;

         search(0);
     });
}
//#endregion

//#region 取得計價上限資料表

//Mode=0,   要重整jqGrid
//Mode=1, 不要
function search(Mode) {
    //#region get variables
    var SupID = $('#div_search_SupID').val();
    var Month = $('#div_search_Month').val();
    var I_acci_seq = $('#div_search_I_acci_seq').val();

    var sqlcmd = "select *,RTRIM(SPACE(50)) as acci_Name from [ScOffer_SupMonthPriceMax] with(nolock) where 1=1";
    if (SupID.length > 0)
        sqlcmd += " and SupID=@SupID";
    if (Month.length > 0)
        sqlcmd += " and Month=@Month";
    if (I_acci_seq > 0)
        sqlcmd += " and I_acci_seq=@I_acci_seq";
    sqlcmd += " order by Priority, SupID, Month, I_acci_seq";
    //#endregion

    $.fn.runAjax(
        DB_ScOffer_SupMonthPriceMax
        , "sqlcmd"
        , sqlcmd
        , [
            { Name: '@SupID', Value: SupID }
            , { Name: '@Month', Value: Month }
            , { Name: '@I_acci_seq', Value: I_acci_seq }
        ], function (response) {

            $('#jqGrid').jqGrid("clearGridData", true);
            $('#jqGrid').trigger('reloadGrid');

            //帶入計價類別中文名稱
            for (i = 0; i < response.length; i++) {
                var _I_acci_seq = response[i].I_acci_seq;
                var _acci_Name = $.map(PriceTypeList, function (val) {
                    return val.I_acci_seq == _I_acci_seq ? val._name : null;
                });
                response[i].acci_Name = _acci_Name[0];
            }
            GridData = response;

            if (Mode == 0) {
                setGrid();
            }
            else {
                $('#jqGrid').jqGrid('setGridParam', { data: GridData });
                $('#jqGrid').trigger('reloadGrid');
            }
        });
}
//#endregion

//#region jqGrid
var PriceTypeList = ''; //計價類別清單
var GridData = '';
function setGrid() {
    $("#jqGrid").jqGrid({
        datatype: 'local',
        data: GridData,
        //建立欄位名稱及屬性
        colNames: ['供應商', '月份', 'I_acci_seq', '計費類別', '金額上限', '優先權'],
        colModel:
            [
                { name: 'SupID', index: 'SupID', width: 90, sorttype: "text" },
                { name: 'Month', index: 'Month', width: 90, sorttype: "date" },
                { name: 'I_acci_seq', index: 'I_acci_seq', width: 150, hidden: true },
                { name: 'acci_Name', index: 'acci_Name', width: 150 },
                { name: 'PriceMax', index: 'PriceMax', width: 60 },
                { name: 'Priority', index: 'Priority', width: 90 }
            ],
        //建立grid屬性，editurl 是指資料新增、編輯及刪除時的伺服器端程式 
        pager: '#jqGridPager',
        autowidth: true,    //自动匹配宽度  
        height: 'auto',
        rownumbers: true,   //添加左侧行号  
        rownumWidth: 30,
        altRows: true,      //单双行样式不同 
        loadonce: false,
        rowNum: 20,         //每页显示记录数  
        rowList: [20, 30, 40],  //用于改变显示行数的下拉列表框的元素数组  
        viewrecords: true,  //显示总记录数  
        onSelectRow: function () {
            EditIndex = $("#jqGrid").jqGrid('getGridParam', 'selrow');
            div_EditDialog_LoadData();
        }
    });
    $("#jqGrid")
    .navGrid('#jqGridPager', { edit: false, add: false, del: false, search: false })
    .navButtonAdd('#jqGridPager', {
        caption: "新增",
        buttonicon: "ui-icon-plus",
        onClickButton: function (postdata) {
            EditIndex = 0;
            div_EditDialog_LoadData();
        },
        position: "last"
    })
    .navButtonAdd('#jqGridPager', {
        caption: "刪除",
        buttonicon: "ui-icon-trash",
        onClickButton: function () {
            EditIndex = $("#jqGrid").jqGrid('getGridParam', 'selrow');
            div_EditDialog_button_Del();
        },
        position: "last"
    });
}
//#endregion

//#region Edit Dialog
var EditData = '';  //修改中的物件
var EditIndex = 0;
//#region Loading
function div_EditDialog_LoadData() {
    if (EditIndex == null) {
        MessageBoxShow('請選擇列');
        return;
    }
    if (EditIndex > 0) {
        //修改
        EditData = GridData[EditIndex - 1];

        $('#div_EditDialog_input_Header').text('修改');
        $('#div_EditDialog_input_supNo').val(EditData.SupID);
        $('#div_EditDialog_input_Month').val(EditData.Month);
        $('#div_EditDialog_input_I_acci_seq').val(EditData.I_acci_seq);
        $('#div_EditDialog_input_PriceMax').val(EditData.PriceMax);
        $('#lbl_input_Priority').text(EditData.Priority);
    }
    else {
        //新增
        EditData = GridData[EditIndex];

        $('#div_EditDialog_input_Header').text('新增');
        $('#div_EditDialog_input_supNo').val('');
        $('#div_EditDialog_input_Month').val('');
        $('#div_EditDialog_input_I_acci_seq').val(PriceTypeList[0].I_acci_seq);
        $('#div_EditDialog_input_PriceMax').val('0');
        $('#lbl_input_Priority').text('0');
    }

    $('#div_EditDialog').dialog('open');
    ////凍結UI
    //$('#jqGridPager').addClass('ui-state-disabled');
}
//#endregion

//#region Save
function div_EditDialog_button_save() {
    //檢查資料
    var IsOk = [false, ''];
    IsOk = CheckLength($('#div_EditDialog_input_supNo').val(), '供應商');
    if (!IsOk[0]) { MessageBoxShow(IsOk[1]); return; }

    div_EditDialog_button_close();

    //更新資料
    EditData.SupID = $('#div_EditDialog_input_supNo').val();
    EditData.Month = $('#div_EditDialog_input_Month').val();
    EditData.I_acci_seq = $('#div_EditDialog_input_I_acci_seq').val();
    var _acci_Name = $.map(PriceTypeList, function (val) {
        return val.I_acci_seq == EditData.I_acci_seq ? val._name : null;
    });
    EditData.acci_Name = _acci_Name[0];
    EditData.PriceMax = $('#div_EditDialog_input_PriceMax').val();
    EditData.Priority = $('#lbl_input_Priority').text();

    var sqlcmd_string = '';
    if (EditIndex == 0) {
        sqlcmd_string = 'Insert Into [ScOffer_SupMonthPriceMax](SupID,Month,I_acci_seq,PriceMax,Priority)'
                        + 'values(@SupID,@Month,@I_acci_seq,@PriceMax,@Priority)';
    }
    else {
        sqlcmd_string = "Update [ScOffer_SupMonthPriceMax] set SupID=@SupID, Month=@Month,I_acci_seq=@I_acci_seq,PriceMax=@PriceMax,Priority=@Priority where sn=@sn";
    }

    //更新至DB
    $.fn.runAjax(
        DB_ScOffer_SupMonthPriceMax
        , "sqlcmd"
        , sqlcmd_string
        , [
            { Name: '@sn', Value: EditData.sn }
            , { Name: '@SupID', Value: EditData.SupID }
            , { Name: '@Month', Value: EditData.Month }
            , { Name: '@I_acci_seq', Value: EditData.I_acci_seq }
            , { Name: '@PriceMax', Value: EditData.PriceMax }
            , { Name: '@Priority', Value: EditData.Priority }
        ], function (response) {

            ToastShow('更新成功');

            search(1);
        }
        , function () {
            ToastShow('更新失敗');
        });
}

//#endregion

//#region Delete
function div_EditDialog_button_Del() {
    EditData = GridData[EditIndex - 1];

    ConfirmBoxShow("確定刪除？", "", function () {

        var sqlcmd_string = '';
        sqlcmd_string = 'Delete from ScOffer_SupMonthPriceMax Where sn=@sn';

        //更新至DB
        $.fn.runAjax(
            DB_ScOffer_SupMonthPriceMax
            , "sqlcmd"
            , sqlcmd_string
            , [
                { Name: '@sn', Value: EditData.sn }
            ], function (response) {
                ToastShow('刪除成功');

                search(1);
            }
            , function () {
                ToastShow('刪除失敗');
            });
    });
}

//#endregion

//#region 檢查

//檢查長度
function CheckLength(value, colname) {
    if (value == 'ALL')
        return [true, ''];

    if (value.length != 4)
        return [false, colname + ': 長度不正確'];
    else
        return [true, ''];
}

//自動帶出Priority
function CalPriority() {
    var SupID = $('#div_EditDialog_input_supNo').val();
    var Month = $('#div_EditDialog_input_Month').val();
    var myPriority = -1;

    if (SupID == 'ALL' && Month.length == 0) {
        myPriority = 0;
    }
    else if (SupID == 'ALL' && Month.length > 0) {
        myPriority = 1;
    }
    else if (SupID != 'ALL' && Month.length == 0) {
        myPriority = 2;
    }
    else {
        myPriority = 3;
    }

    $('#lbl_input_Priority').text(myPriority);
}

//#endregion

//#region Exit
function div_EditDialog_button_close() {
    $('#div_EditDialog').dialog('close');
}
//#endregion

//#endregion

//#region Form Control
function sendMessage(msg) {
    $('#lbl_Message').text(msg);
}
//#endregion