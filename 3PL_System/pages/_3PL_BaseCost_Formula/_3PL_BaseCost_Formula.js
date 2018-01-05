$(
    function init() {
        //#region 帶入基本資料
        getBaseData('SiteNo', { 'Value': '', 'Name': '全部' })
        .then(function (response) {
            getlist(response, $('#div1_select_SiteNo'));
        });
        getBaseData('TypeId', { 'Value': '', 'Name': '全部' })
        .then(function (response) {
            getlist(response, $('#div1_select_TypeID'));
        })
        .then(function (response) {
            search();
        });
        setGrid();
        //#endregion

        //#region EditDialog
        $('#div_EditDialog').dialog({
            autoOpen: false,
            width: '90%',
            modal: true,
            resizable: true,
            dialogClass: 'dialog_noTitle'
        });
        setGridDetail();
        setGrid_AddonPageDefPrice();
        //$('#div1_input_Month').datepicker({ dateFormat: 'yymm' });

        //#endregion
    }
);

var DB_ScOffer_SupMonthPriceMax = '134_JASON_TEST';

//#region jqGrid

//jqGrid
var GridData = '';
function setGrid() {
    $("#jqGrid").jqGrid({
        datatype: 'local',
        data: GridData,
        //建立欄位名稱及屬性
        colNames: ['序號', '倉別', '報價主類別', '費用名稱', '變數', '公式'],
        colModel:
            [
                { name: 'I_bcse_Seq', index: 'I_bcse_Seq', width: 30 },
                { name: 'S_bcse_SiteNo', index: 'S_bcse_SiteNo', width: 30 },
                { name: 'S_bsda_FieldName', index: 'S_bsda_FieldName', width: 90, sorttype: "text" },
                { name: 'S_bcse_CostName', index: 'S_bcse_CostName', width: 90, sorttype: "text" },
                { name: 'Params', index: 'Params', width: 150 },
                { name: 'Formula', index: 'Formula', width: 150 }
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
        caption: "刪除",
        buttonicon: "ui-icon-trash",
        onClickButton: function () {
            EditIndex = $("#jqGrid").jqGrid('getGridParam', 'selrow');
            div_EditDialog_button_Del();
        },
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

//#region Delete
function div_EditDialog_button_Del() {
    if (EditIndex == null) {
        MessageBoxShow('請選擇列');
        return;
    }
    EditData = GridData[EditIndex - 1];
    var msg = '是否刪除此筆費用的所有設定？' + '<br/>'
        + EditData.S_bcse_SiteNo + '.' + EditData.S_bsda_FieldName + '.' + EditData.S_bcse_CostName;
    ConfirmBoxShow(msg, null, div_EditDialog_button_Del_Action);
}
//real del action
function div_EditDialog_button_Del_Action() {
    var sqlcmd_string = '';
    sqlcmd_string = 'Delete from [_3PL_BaseCostSet_Addon] Where I_Addon_bcseSeq=@sn';

    //更新至DB
    $.fn.runAjax(
        DB_ScOffer_SupMonthPriceMax
        , "sqlcmd"
        , sqlcmd_string
        , [
            { Name: '@sn', Value: EditData.I_bcse_Seq }
        ], function (response) {

            if (response.length > 0) {
                div_EditDialog_button_close();
                ToastShow('刪除成功');
            }
            else {
                ToastShow('刪除失敗');
            }

            search(1);
        });
}
//#endregion

//#endregion jqGrid

//#region jqGrid_paramsList
var GridData_paramsList = '';
var lastSelection_params = -1;
//設定
function setGridDetail() {
    $("#jqGrid_paramsList").jqGrid({
        datatype: 'local',
        data: GridData_paramsList,
        //建立欄位名稱及屬性
        colNames: ['變數名稱', '欄位格式', '預設值', '測試數值', '移動'],
        colModel:
            [
                { name: 'Name', index: 'Name', editable: true, width: 200 },
                { name: 'type', index: 'type', editable: true, width: 150, edittype: 'select', editoptions: { value: "text:text;select:select;date:date;calculate:calculate" } },
                { name: 'Value', index: 'Value', editable: true, width: 300 },
                { name: 'TestValue', index: 'TestValue', editable: true, width: 300 },
                { name: 'DisplayOrder', width: 90, index: 'DisplayOrder', editable: false, sorttype: 'number', formatter: setGridDetail_moveButtons }
            ],
        //建立grid屬性，editurl 是指資料新增、編輯及刪除時的伺服器端程式 
        autowidth: true,    //自动匹配宽度  
        shrinkToFit: true,
        height: 'auto',
        rownumWidth: 30,
        altRows: true,      //单双行样式不同 
        loadonce: false,
        rowNum: 20,         //每页显示记录数
        viewrecords: true,  //显示总记录数  
        sortname: 'DisplayOrder',
        sortorder: 'asc',
        onSelectRow: setGridDetail_EditButtonClick
    });
}
//帶入資料
function setGridDetail_reset(result) {

    $('#jqGrid_paramsList').jqGrid("clearGridData", true);
    $('#jqGrid_paramsList').trigger('reloadGrid');

    if (result != null) {
        GridData_paramsList = result;

        $('#jqGrid_paramsList').jqGrid('setGridParam', { data: GridData_paramsList });
        $('#jqGrid_paramsList').trigger('reloadGrid');
    }
}
//按下修改 inline edit
function setGridDetail_EditButtonClick(id) {
    if (id && id !== lastSelection_params) {
        //先存舊資料
        setGridDetail_save()
        .then(function (response) {
            var grid = $("#jqGrid_paramsList");
            grid.jqGrid('editRow', id);
            lastSelection_params = id;
        });
    }
}
//存檔
function setGridDetail_save() {
    var dfd = $.Deferred();

    setTimeout(function () {
        var grid = $("#jqGrid_paramsList");
        grid.jqGrid('saveRow', lastSelection_params, { url: 'clientArray' });

        dfd.resolve();
    }, 0);

    return dfd.promise();
}
//Help
function setGridDetail_help() {
    MessageBoxShow(
          "1. 變數名稱空白狀態下，存檔時自動刪除<br/>"
        + "2. 下拉式選單一定要搭配預設值, 格式: key:value;key:value<br/>"
        + "3. 變數名稱有\"貨號\"時，打單時會自動搜尋變數名稱並帶入數值，這些特殊變數欄位只要決定變數名稱就可以<br/>"
        + "   目前開放：\"品名\"\"入數\"\"板數\"\"條碼\"<br/>"
        + "3. 變數名稱有\"單價\"時，打單時會自動帶入3PL計價費用設定的單價<br/>"
    );
}
//自訂上下移動按鈕
function setGridDetail_moveButtons(cellvalue, options, rowObject) {
    return rowObject.DisplayOrder
          + '<input type="button" value="▲" class="btn-default" onclick="moveButtonAction(' + rowObject.DisplayOrder + ',\'up\')">'
          + '<input type="button" value="▼" class="btn-default" onclick="moveButtonAction(' + rowObject.DisplayOrder + ',\'down\')">';
}
function moveButtonAction(keyDisplayOrder, Action) {
    var grid = $("#jqGrid_paramsList");
    if (keyDisplayOrder <= 0) return;

    var Origindata = grid.jqGrid('getGridParam', 'data');

    //從DisplayOrder找出row
    var rowIndex = -1;
    $.each(Origindata, function (key, value) {
        if (value.DisplayOrder == keyDisplayOrder)
            rowIndex = key;
    });



    //找出上一筆或下一筆的row
    var nextIndex = -1;
    var nextkeyDisplayOrder = -1;
    if (Action == 'down') {
        nextkeyDisplayOrder = keyDisplayOrder + 1;
        $.each(Origindata, function (key, value) {
            if (value.DisplayOrder == nextkeyDisplayOrder)
                nextIndex = key;
        });
        //沒找到
        if (nextIndex < 0)
            return;
    }
    else {
        nextkeyDisplayOrder = keyDisplayOrder - 1;
        $.each(Origindata, function (key, value) {
            if (value.DisplayOrder == nextkeyDisplayOrder)
                nextIndex = key;
        });
        //沒找到
        if (nextIndex < 0)
            return;
    }

    //更換DisplayOrder
    Origindata[rowIndex].DisplayOrder = nextkeyDisplayOrder;
    Origindata[nextIndex].DisplayOrder = keyDisplayOrder;

    console.log(Origindata);
    grid.trigger('reloadGrid');
}

//#endregion jqGrid_paramsList

//#region 預設網頁

var Grid_AddonPageDefPrice = [];
//jqGrid_AddonPageDefPrice
function setGrid_AddonPageDefPrice() {
    var sqlcmd = "select I_bsda_seq, S_bsda_FieldName, S_bsda_FieldId from [3PL_BaseData] with(nolock) where S_bsda_CateId='AddonPageDefPrice' order by S_bsda_FieldName";
    var ht1 = [];

    $.fn.runAjax(
        "3PL"
        , "sqlcmd"
        , sqlcmd
        , ht1, function (response) {
            Grid_AddonPageDefPrice = response;
            var grid = $("#jqGrid_AddonPageDefPrice");
            grid.jqGrid({
                datatype: 'local',
                data: Grid_AddonPageDefPrice,
                //建立欄位名稱及屬性
                colNames: ['網頁', '預設費用代號', 'Seq'],
                colModel:
                    [
                        { name: 'S_bsda_FieldName', index: 'S_bsda_FieldName' },
                        { name: 'S_bsda_FieldId', index: 'S_bsda_FieldId', editable: true },
                        { name: 'I_bsda_seq', index: 'I_bsda_seq', hidden: true }
                    ],
                //建立grid屬性，editurl 是指資料新增、編輯及刪除時的伺服器端程式 
                //autowidth: true,    //自动匹配宽度  
                height: 'auto',
                rownumWidth: 30,
                altRows: true,      //单双行样式不同 
                loadonce: false,
                rowNum: 20,         //每页显示记录数   
                viewrecords: true,  //显示总记录数  
                caption: "AddOn網頁預設計價費用",
                onSelectRow: setGrid_AddonPageDefPrice_EditButtonClick
            });
        });
}
var lastSelection;          //最後一筆修改的資料
//按下修改 inline edit
function setGrid_AddonPageDefPrice_EditButtonClick(id) {
    if (id && id !== lastSelection) {
        var grid = $("#jqGrid_AddonPageDefPrice");
        if (lastSelection != undefined) {
            grid.jqGrid('saveRow', lastSelection, { url: 'clientArray' });

            var sqlcmd = "Update [3PL_BaseData] Set S_bsda_FieldId=@FieldID where I_bsda_seq=@Sn";
            var ht1 = [
                { Name: "@Sn", Value: Grid_AddonPageDefPrice[lastSelection - 1].I_bsda_seq },
                { Name: "@FieldID", Value: Grid_AddonPageDefPrice[lastSelection - 1].S_bsda_FieldId }
            ];

            $.fn.runAjax(
                "3PL"
                , "sqlcmd"
                , sqlcmd
                , ht1, function (response) {

                });
        }
        grid.jqGrid('editRow', id, { keys: true });
        lastSelection = id;
    }
}

//#endregion

//#region 取得資料

function search() {
    //#region get variables
    var SiteNo = $('#div1_select_SiteNo option:selected').val();
    var TypeID = $('#div1_select_TypeID option:selected').val();
    var IsINNERJOIN = $('#div1_checkbox_InnerJoin').prop('checked');

    if (IsINNERJOIN)
        IsINNERJOIN = " INNER";
    else
        IsINNERJOIN = " LEFT";

    var sqlcmd = "select a.S_bcse_SiteNo, c.S_bsda_FieldName, S_bcse_CostName, a.I_bcse_Seq"
        + ",Params=MAX(CASE WHEN b.I_Addon_Type=0 THEN b.S_Addon_Formula ELSE '' END)"
        + ",Formula=MAX(CASE WHEN b.I_Addon_Type=1 THEN b.S_Addon_Formula ELSE '' END)"
        + " from [3PL_BaseCostSet] a with(nolock)"
        + IsINNERJOIN + " JOIN [192.168.100.134].JASON_TEST.[dbo].[_3PL_BaseCostSet_Addon] b with(nolock)"
        + " on a.I_bcse_Seq=b.I_Addon_bcseSeq"
        + " inner join [3PL_BaseData] c with(nolock)"
        + " on a.I_bcse_TypeId=c.S_bsda_FieldId and c.S_bsda_CateId='TypeId'"
        + "where a.I_bcse_DelFlag=0 ";
    if (SiteNo.length > 0)
        sqlcmd += " and a.S_bcse_SiteNo=@SiteNo";
    if (TypeID.length > 0)
        sqlcmd += " and a.I_bcse_TypeId=@TypeId";
    sqlcmd += " GROUP BY a.S_bcse_SiteNo, c.S_bsda_FieldName, S_bcse_CostName, a.I_bcse_Seq";

    //#endregion
    $.fn.runAjax(
        "3PL"
        , "sqlcmd"
        , sqlcmd
        , [
            { Name: '@SiteNo', Value: SiteNo }
            , { Name: '@TypeId', Value: TypeID }
        ], function (response) {
            setGrid_reset(response);
        });
}
//#endregion

//#region 修改小視窗

var EditData = '';  //修改中的物件
var EditIndex = 0;

//#region 介面-params

//params-新增變數
function div_EditDialog_button_AddParam() {
    setGridDetail_save();
    var grid = $("#jqGrid_paramsList");

    var Origindata = grid.jqGrid('getGridParam', 'data');

    grid.jqGrid("addRow", {
        position: "last"
        , initdata: {
            DisplayOrder: Origindata.length + 1
        }
    });
    var id = grid.jqGrid('getGridParam', 'selrow');
    lastSelection_params = id;
}
//#endregion

//#region 介面-Formula

//介面按下計算
function div_EditDialog_button_Calculate() {
    var grid = $("#jqGrid_paramsList");

    setGridDetail_save()
    .then(function (response) {
        var grid = $("#jqGrid_paramsList");
        grid.jqGrid('editRow', lastSelection_params);

        //取得變數清單
        var data0 = grid.jqGrid('getGridParam', 'data');
        var data = [];
        $.each(data0, function (_name, _value) {
            data.push({ Name: _value.Name, Value: _value.TestValue, type: _value.type });
        });

        //取得公式字串
        var formula = $('#div_EditDialog_input_formula').val();
        //回傳結果
        var _result = '';
        _result = Formula_Calculate(formula, data);
        _result = _result.formula + " = " + Math.round(_result.result);
        $('#div_EditDialog_label_formula').text(_result);
    });
}
//Formula-帶入
function div_EditDialog_input_formula_Bring() {
    var formula = JSON.parse(EditData.Formula);
    $('#div_EditDialog_input_formula').val(formula[0].Name);
}
//Formula-計算
function Formula_Calculate(formula, data, Mode) {
    $.each(data, function (key, value) {
        //key   , value
        //倉租費, (PCS/入數/板數)*寄倉天數*8
        var regex = new RegExp(value.Name, "g");
        formula = formula.replace(regex, value.Value);
    })

    //再度檢查是否有需要繼續帶入
    $.each(data, function (key, value) {
        if (formula.indexOf(value.Name) >= 0) {
            formula = Formula_Calculate(formula, data, 1);
        }
    })

    if (Mode == 1) {
        return formula;
    }
    else {
        var result = 0;
        try {
            result = eval(formula).toFixed(2);
        } catch (error) {
            result = 0;
        }

        //帶入結果
        return { formula: formula, result: result.toString() };
    }
}
//#endregion

//#region Edit

function div_EditDialog_LoadData() {
    if (EditIndex == null) {
        MessageBoxShow('請選擇列');
        return;
    }
    if (EditIndex > 0) {

        EditData = GridData[EditIndex - 1];
        $('#div_EditDialog_label_S_bsda_FieldName').text(EditData.S_bsda_FieldName);
        $('#div_EditDialog_label_S_bcse_CostName').text(EditData.S_bcse_CostName);

        if (EditData.Params == '') {
            FormStatus = 1;

            //新增
            $('#div_EditDialog_input_Header').text('新增');

            //params
            setGridDetail_reset(null);
        }
        else {
            FormStatus = 2;

            //修改
            $('#div_EditDialog_input_Header').text('修改');

            //params
            var parentObj = JSON.parse(EditData.Params);
            var resultObj = [];
            $.each(parentObj, function (key, value) {
                resultObj.push({
                    Name: parentObj[key].Name
                    , type: parentObj[key].type
                    , Value: parentObj[key].Value
                    , TestValue: parentObj[key].Value
                    , DisplayOrder: key + 1
                });
            });

            setGridDetail_reset(resultObj);

            //formula
            div_EditDialog_input_formula_Bring();

            div_EditDialog_button_Calculate();
        }
    }

    $('#div_EditDialog').dialog('open');
}
//#endregion

//#region Save
function div_EditDialog_button_save() {
    setGridDetail_save()
    .then(function () {

        //取得變數清單
        var Origindata = $("#jqGrid_paramsList").jqGrid('getGridParam', 'data');
        Origindata = Origindata.sort(function (a, b) {
            return a.DisplayOrder > b.DisplayOrder;
        });

        var saveParams = [];
        $.each(Origindata, function (key, value) {

            if (value.Name != undefined && value.Name.length > 0) {
                saveParams.push({
                    Name: value.Name
                    , type: value.type
                    , Value: value.Value
                });
            }
        });

        EditData.Params = JSON.stringify(saveParams);

        //取得公式
        var saveFormula = [];
        saveFormula.push({ Name: $('#div_EditDialog_input_formula').val() });
        EditData.Formula = JSON.stringify(saveFormula);

        var ht1 = [];
        ht1.push({ Name: '@sn', Value: EditData.I_bcse_Seq }
                , { Name: '@params', Value: EditData.Params }
                , { Name: '@formula', Value: EditData.Formula });

        //更新資料
        var sqlcmd = '';
        if (FormStatus == 1) {
            sqlcmd = 'INSERT INTO [_3PL_BaseCostSet_Addon](I_Addon_bcseSeq, I_Addon_Type, S_Addon_Formula) values';
            sqlcmd += "(@sn,0,@params),(@sn,1,@formula)";

            //更新至DB
            $.fn.runAjax(
                DB_ScOffer_SupMonthPriceMax
                , "sqlcmd"
                , sqlcmd
                , ht1, function (response) {

                    if (response != null) {
                        ToastShow('新增成功');
                    }
                    else {
                        ToastShow('新增失敗');
                    }
                    FormStatus = 0;
                    search();

                    setTimeout(function () {
                        div_EditDialog_button_close();
                    }, 1000);
                });
        }
        else if (FormStatus == 2) {
            sqlcmd = "UPDATE [_3PL_BaseCostSet_Addon] set S_Addon_Formula=@params where I_Addon_bcseSeq=@sn and I_Addon_Type=0;";
            sqlcmd += "UPDATE [_3PL_BaseCostSet_Addon] set S_Addon_Formula=@formula where I_Addon_bcseSeq=@sn and I_Addon_Type=1;";

            //更新至DB
            $.fn.runAjax(
                DB_ScOffer_SupMonthPriceMax
                , "sqlcmd"
                , sqlcmd
                , ht1, function (response) {

                    if (response != null) {
                        ToastShow('更新成功');
                    }
                    else {
                        ToastShow('更新失敗');
                    }
                    FormStatus = 0;
                    search();

                    setTimeout(function () {
                        div_EditDialog_button_close();
                    }, 1000);
                });
        }
    });
}

//#endregion

//#region Exit
function div_EditDialog_button_close() {
    $('#div_EditDialog').dialog('close');
}
//#endregion

//#endregion

//#region Form Control
var FormStatus = 0; //0-search, 1-New, 2-Edit, 3-Save, 4-Del, 5-Cancel

//#endregion