$(
    function init() {
        //#region EditDialog
        $('#div_EditDialog').dialog({
            autoOpen: false,
            width: '60%',
            modal: true,
            resizable: true,
            dialogClass: 'dialog_noTitle'
        });
        //$('#div1_input_Month').datepicker({ dateFormat: 'yymm' });

        //#endregion

        //#region 帶入基本資料
        setGrid();
        search();
        //#endregion
        LoadBaseAccounting();
    }
);

var DB_ScOffer_SupMonthPriceMax = '3PL';

//費用類別建檔
var AccountClassNo2_List = [];
function LoadBaseAccounting() {
    getBaseData('AccountClassNo1', undefined)
    .then(function (response) {
        getlist(response, $('#div_EditDialog_Select_S_Acci_ClassNo1'));
    });

    getBaseData('AccountClassNo2', undefined)
   .then(function (response) {
       AccountClassNo2_List = response;
   });
}
function div_EditDialog_Select_S_Acci_ClassNo1_change() {
    //clear
    $('#div_EditDialog_Select_S_Acci_ClassNo2').empty();

    //filter
    // ParnetValue= "3,其他服務"
    // maps =   {Name: "使用物流中心費用", Value: "3-3"}
    //          {Name: "作業處理費用", Value: "3-4"}
    //          {Name: "流通加工服務費用", Value: "3-5"}

    var ParentValue = $('#div_EditDialog_Select_S_Acci_ClassNo1').val();
    var maps = $.map(AccountClassNo2_List, function (value, key) {
        var strsplit = value.Value.split('-');
        var returnJSON = { Name: value.Name, Value: value.Value };

        return strsplit[0] == ParentValue ? returnJSON : null;
    });
    getlist(maps, $('#div_EditDialog_Select_S_Acci_ClassNo2'));
}


//#region jqGrid

//jqGrid
var GridData = '';
function setGrid() {

    var colNames = ['費用分類', '費用類別', '會計科目', '科目名稱', 'I_Acci_seq'];
    var colModel = [
                { name: 'S_Acci_ClassNo1_View', index: 'S_Acci_ClassNo1_View' },
                { name: 'S_Acci_ClassNo2_View', index: 'S_Acci_ClassNo2_View' },
                { name: 'S_Acci_Id', index: 'S_Acci_Id', key: true },
                { name: 'S_Acci_Name', index: 'S_Acci_Name' },
                { name: 'I_Acci_seq', index: 'I_Acci_seq', hidden: true }];

    $("#jqGrid").jqGrid({
        datatype: 'local',
        data: GridData,
        //建立欄位名稱及屬性
        colNames: colNames,
        colModel: colModel,
        //建立grid屬性，editurl 是指資料新增、編輯及刪除時的伺服器端程式 
        pager: '#jqGridPager',
        autowidth: true,    //自动匹配宽度 
        height: 'auto',
        rownumbers: true,   //添加左侧行号  
        rownumWidth: 30,
        altRows: true,      //单双行样式不同 
        loadonce: false,
        rowNum: 50,         //每页显示记录数  
        viewrecords: true,  //显示总记录数  
        multiSort: true
    });
    $("#jqGrid")
    .jqGrid('sortGrid', 'S_Acci_ClassNo1_View', true, "asc")
    .jqGrid('sortGrid', 'S_Acci_ClassNo2_View', true, "asc")
    .navGrid('#jqGridPager', { edit: false, add: false, del: false, search: false })
    .navButtonAdd('#jqGridPager', {
        caption: "新增",
        buttonicon: "ui-icon-plus",
        onClickButton: setGridDetail_NewButtonClick,
        position: "last"
    })
    .navButtonAdd('#jqGridPager', {
        caption: "修改",
        buttonicon: "ui-icon-pencil",
        onClickButton: setGridDetail_EditButtonClick,
        position: "last"
    })
    .navButtonAdd('#jqGridPager', {
        caption: "刪除",
        buttonicon: "ui-icon-trash",
        onClickButton: setGridDetail_DelButtonClick,
        position: "last"
    });
}
function setGrid_reset(result) {
    $('#jqGrid').jqGrid("clearGridData", true);
    $('#jqGrid').trigger('reloadGrid');

    if (result != null) {
        GridData = result;

        $('#jqGrid').jqGrid('setGridParam', { data: GridData });
        $('#jqGrid').trigger('reloadGrid');
    }
}

//New
function setGridDetail_NewButtonClick() {
    $('#div_EditDialog_Select_S_Acci_ClassNo1').val(3);
    div_EditDialog_Select_S_Acci_ClassNo1_change();
    $('#div_EditDialog').dialog('open');
}
//Edit
function setGridDetail_EditButtonClick() {
    var grid = $("#jqGrid");
    var id = grid.jqGrid('getGridParam', 'selrow');
    if (id == undefined) {
        MessageBoxShow('請選擇列');
        return;
    }

    var Row = grid.jqGrid('getRowData', id);

    $('#div_EditDialog_Select_S_Acci_ClassNo1').val(Row.S_Acci_ClassNo1_View.split(',')[0]);
    div_EditDialog_Select_S_Acci_ClassNo1_change()

    $('#div_EditDialog_Select_S_Acci_ClassNo2').val(Row.S_Acci_ClassNo2_View.split(',')[0]);


    $('#div_EditDialog_S_Acci_Id').val(Row.S_Acci_Id);
    $('#div_EditDialog_S_Acci_Name').val(Row.S_Acci_Name);


    $('#div_EditDialog').dialog('open');
}
//Del
function setGridDetail_DelButtonClick() {
    var grid = $("#jqGrid");
    var id = grid.jqGrid('getGridParam', 'selrow');
    if (id == undefined) {
        MessageBoxShow('請選擇列');
        return;
    }
    var Row = grid.jqGrid('getRowData', id);
    ConfirmBoxShow("確定刪除？" + Row.S_Acci_Id + ',' + Row.S_Acci_Name, "", function () {

        $.fn.runAjax(
           "3PL"
           , "sqlcmd"
           , "Update [3PL_BaseAccounting] set I_Acci_DelFlag=1 Where I_Acci_seq=@seq"
           , [{ Name: "@seq", Value: Row.I_Acci_seq }]);
    });
}
//#endregion jqGrid

//#region div_EditDialog

//儲存
function div_EditDialog_button_save() {
    var AccountClassNo1 = $('#div_EditDialog_Select_S_Acci_ClassNo1').val();
    var AccountClassNo2 = $('#div_EditDialog_Select_S_Acci_ClassNo2').val();
    var S_Acci_Id = $('#div_EditDialog_S_Acci_Id').val();
    var S_Acci_Name = $('#div_EditDialog_S_Acci_Name').val();

    var grid = $("#jqGrid");
    var id = grid.jqGrid('getGridParam', 'selrow');
    var Row = grid.jqGrid('getRowData', id);
    var seq = Row.I_Acci_seq;
    $('#div_EditDialog').dialog('close');

    $.fn.runAjax(
       "3PL"
       , "sqlcmd"
       , "Update [3PL_BaseAccounting] set S_Acci_Id=@S_Acci_Id,S_Acci_Name=@S_Acci_Name,S_Acci_ClassNo1=@S_Acci_ClassNo1,S_Acci_ClassNo2=@S_Acci_ClassNo2 Where I_Acci_seq=@seq"
       , [{ Name: "@seq", Value: seq }
           , { Name: "@S_Acci_Id", Value: S_Acci_Id }
           , { Name: "@S_Acci_Name", Value: S_Acci_Name }
           , { Name: "@S_Acci_ClassNo1", Value: AccountClassNo1 }
           , { Name: "@S_Acci_ClassNo2", Value: AccountClassNo2 }
       ]
       , function success() {
           ToastShow('修改成功', function () {
               search();
           });
       }
       , function fail() {
           ToastShow('修改失敗');
       });
}

//取消
function div_EditDialog_button_close() {
    $('#div_EditDialog').dialog('close');
}

//#endregion div_EditDialog

//#region 取得資料

function search() {

    var sqlcmd = "select I_Acci_seq, S_Acci_Id, S_Acci_Name, S_Acci_ClassNo1, b.S_bsda_FieldName AS S_Acci_ClassName1, S_Acci_ClassNo2, b2.S_bsda_FieldName AS S_Acci_ClassName2 "
                + " from [3PL_BaseAccounting] a with(nolock) "
                + " Inner join [3PL_BaseData] b with(nolock) "
                + " on a.S_Acci_ClassNo1=b.S_bsda_FieldId and b.S_bsda_CateId='AccountClassNo1' "
                + " Inner join [3PL_BaseData] b2 with(nolock) "
                + " on a.S_Acci_ClassNo2=b2.S_bsda_FieldId and b2.S_bsda_CateId='AccountClassNo2' "
                + " where I_Acci_DelFlag=0";

    //#endregion
    $.fn.runAjax(
        "3PL"
        , "sqlcmd"
        , sqlcmd
        , [], function (response) {
            $.each(response, function (key, value) {
                value.S_Acci_ClassNo1_View = value.S_Acci_ClassNo1 + ',' + value.S_Acci_ClassName1;
                value.S_Acci_ClassNo2_View = value.S_Acci_ClassNo2 + ',' + value.S_Acci_ClassName2;
            })
            setGrid_reset(response);
        });
}
//#endregion

//#region Form Control
var FormStatus = 0; //0-search, 1-New, 2-Edit, 3-Save, 4-Del, 5-Cancel

//#endregion