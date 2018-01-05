$(
    function init() {
        //預設日期
        var today = new Date();
        var Bdate = _3PLgetDate(1);
        var Edate = _3PLgetDate(0);
        var BMonth = Bdate.substring(0, 7);
        $('#div1_select_Bdate').val(Bdate);
        $('#div1_select_Edate').val(Edate);

        $("#div1_select_Bdate").datepicker({ dateFormat: 'yy-mm-dd' });
        $("#div1_select_Edate").datepicker({ dateFormat: 'yy-mm-dd' });
        $("#div_EditDialog_QuotationDate").datepicker({ dateFormat: 'yy-mm-dd' });
        $("#div_EditDialog_PayDate").datepicker({ dateFormat: 'yy-mm-dd' });

        //init
        FormStatus = 0;
        setGrid();
        reset();

        //讀取預設金額
        getDefaultSn()
        .then(function (response) {
            getDefaultSinglePrice();
        })
        .then(function (response) {
            setGridDetail();
        });
    }
);

var DB_ScOffer_SupMonthPriceMax = '134_JASON_TEST';
var DefaultParams = [];     //預設的參數陣列
var DefaultFormula = '';    //預設的公式
var DefaultSinglePrice = 0; //預設的單價
var DefaultSn = 65535;      //預設的費用
var DefaultPageType = 2;    //預設的網頁參考代號

//#region 預設參數
function getDefaultSn() {
    var dfd = $.Deferred();

    var sqlcmd = "select ID=S_bsda_FieldId from [3PL_BaseData] with(nolock) where S_bsda_CateId='AddonPageDefPrice' and S_bsda_FieldName like @PageType ";
    var ht1 = [{ Name: "@PageType", Value: DefaultPageType + '%' }];

    $.fn.runAjax(
        "3PL"
        , "sqlcmd"
        , sqlcmd
        , ht1, function (response) {
            DefaultSn = response[0].ID;
            dfd.resolve();
        });

    return dfd.promise();
}
function getDefaultSinglePrice() {
    var dfd = $.Deferred();

    var sqlcmd = "select Price=I_bcse_Price from [3PL_BaseCostSet] with(nolock) where I_bcse_Seq=@sn ";
    var ht1 = [{ Name: "@sn", Value: DefaultSn }];

    $.fn.runAjax(
        "3PL"
        , "sqlcmd"
        , sqlcmd
        , ht1, function (response) {
            DefaultSinglePrice = response[0].Price;
            dfd.resolve();
        });

    return dfd.promise();
}
//#endregion

//#region 單頭_操作

//#region Search
function searchHead() {
    FormStatus = 0;
    reset();

    //#region get variables
    var ht1 = [];
    var Bdate = $('#div1_select_Bdate').val();
    var Edate = $('#div1_select_Edate').val();
    var SupdID = $('#div1_select_SupdID').val();

    var sqlcmd = "select * from _3PL_QuotationHead_Addon with(nolock) where [Status]>0 and LinkPriceSeq like @LinkPriceSeq";
    ht1.push({ Name: "@LinkPriceSeq", Value: '%' + DefaultSn + '%' });
    if (Bdate.length > 0) {
        sqlcmd += " and QuotationDate>=@Bdate";
        ht1.push({ Name: "@Bdate", Value: Bdate });
    }
    if (Edate.length > 0) {
        sqlcmd += " and QuotationDate<=@Edate";
        ht1.push({ Name: "@Edate", Value: Edate });
    }
    if (SupdID.length > 0) {
        sqlcmd += " and SupdID=@SupdID";
        ht1.push({ Name: "@SupdID", Value: SupdID });
    }
    sqlcmd += " ORDER BY QuotationDate DESC, CreateDate DESC, SupdID";

    //#endregion

    $.fn.runAjax(
        DB_ScOffer_SupMonthPriceMax
        , "sqlcmd"
        , sqlcmd
        , ht1, function (response) {
            setGrid_reset(response);
        });
}

function searchDetail(MasterSeq) {
    var sqlcmd = "select * from _3PL_QuotationDetail_Addon with(nolock) where QHeadSeq=@sn order by DisplayOrder, Seq";
    var ht1 = [{ Name: "@sn", Value: MasterSeq }];
    $.fn.runAjax(
        DB_ScOffer_SupMonthPriceMax
        , "sqlcmd"
        , sqlcmd
        , ht1, function (response) {
            //#region jqGrid generate
            var resultForJQGrid = [];

            //Detail
            $.each(response, function (_key, _value) {
                var obj = '{';

                var params = JSON.parse(_value.params);
                $.each(params, function (_key2, _value2) {
                    if (_key2 > 0)
                        obj += ",";
                    obj += '"' + _value2.Name + "\":\"" + _value2.Value + '"';
                });

                obj += ",\"DetailSeq\":\"" + _value.Seq + '"';
                obj += ",\"DisplayOrder\":\"" + _value.DisplayOrder + '"';
                obj += ",\"Price\":\"" + _value.Price + '"';
                obj += ",\"RowStatus\":\"UnModified\"";
                obj += "}";

                resultForJQGrid.push(JSON.parse(obj));
            });
            //#endregion

            setGridDetail_reset(resultForJQGrid);
        });
}
//#endregion

//#region Common
var EditData = '';
function passDate(strDate) {
    try {
        return strDate.substring(0, 10);
    }
    catch (err) {
        return '';
    }
}
//#endregion

//#region Head Edit
var EditIndex = -1;
function div_EditDialog_LoadData() {
    if (EditIndex < 1)
        return;
    EditData = GridData[EditIndex - 1];
    $('#div_EditDialog').show();
    $('#div_EditDialog_PaperNo').val(EditData.PaperNo);
    $('#div_EditDialog_SupdID').val(EditData.SupdID);
    $('#div_EditDialog_BOSS').val(EditData.BOSS);
    $('#div_EditDialog_TEL').val(EditData.TEL);
    $('#div_EditDialog_FAX').val(EditData.FAX);
    $('#div_EditDialog_QuotationDate').val(passDate(EditData.QuotationDate));
    $('#div_EditDialog_PayDate').val(passDate(EditData.PayDate));
    $('#div_EditDialog_Memo').val(EditData.Memo);

    searchDetail(EditData.PaperNo);
}

function div_EditDialog_btn_Edit() {
    FormStatus = 2;
    reset();
}
//#endregion

//#region Head New

function NewQHEAD() {
    FormStatus = 1;
    reset();
}
//#endregion

//#region Head Save
var saveHeadparams = [];
//get parameters
function div_EditDialog_SaveData() {
    saveHeadparams = [];
    saveHeadparams.push({ Name: "@LinkPriceSeq", Value: DefaultSn });
    saveHeadparams.push({ Name: "@SupdID", Value: $('#div_EditDialog_SupdID').val() });
    saveHeadparams.push({ Name: "@BOSS", Value: $('#div_EditDialog_BOSS').val() });
    saveHeadparams.push({ Name: "@TEL", Value: $('#div_EditDialog_TEL').val() });
    saveHeadparams.push({ Name: "@FAX", Value: $('#div_EditDialog_FAX').val() });
    saveHeadparams.push({ Name: "@QuotationDate", Value: $('#div_EditDialog_QuotationDate').val() });
    if ($('#div_EditDialog_PayDate').val().length <= 0)
        saveHeadparams.push({ Name: "@PayDate", Value: "NULL" }); //
    else
        saveHeadparams.push({ Name: "@PayDate", Value: $('#div_EditDialog_PayDate').val() }); //
    saveHeadparams.push({ Name: "@Memo", Value: $('#div_EditDialog_Memo').val() });
    saveHeadparams.push({ Name: "@UserID", Value: $.cookie('UserID') });
}

function div_EditDialog_btn_Save() {

    //先存檔 for UI
    div_EditDialog_SaveData();
    setGridDetail_save();

    //#region 檢查錯誤
    var ErrMsg = '';
    if ($('#div_EditDialog_SupdID').val().length <= 0)
        ErrMsg = '供應商未填寫'
    if ($('#div_EditDialog_QuotationDate').val().length <= 0)
        ErrMsg = '報價日未填寫'

    var Origindata = $("#jqGrid_Detail").jqGrid('getGridParam', 'data');
    if (Origindata.length <= 0)
        ErrMsg = '沒有明細'

    if (ErrMsg.length > 0) {
        MessageBoxShow(ErrMsg);
        return;
    }
    //#endregion

    //Head
    if (FormStatus == 1) {
        //Head新增
        $.fn._3PL_getHeadNo("Addon01", "SA", 4)
        .then(function (response) {
            saveHeadparams.push({ Name: "@PaperNo", Value: response });
            return response;
        })
        .then(function (response) {
            _3PL_QuotationHead_Addon_New();
            return response;
        })
        .then(function (response) {
            var QheadSeq = response;
            //明細存檔
            _3PL_QuotationDetail_Addon_New(QheadSeq);

            FormStatus = 0;
            reset();

            searchHead();

            ToastShow("新增成功");
        }, function fail() {
            ToastShow("新增失敗");
        });
    }
    else {
        //修改
        _3PL_QuotationHead_Addon_Edit(
            function success() {
                var QheadSeq = EditData.PaperNo;
                //明細存檔
                _3PL_QuotationDetail_Addon_New(QheadSeq);

                FormStatus = 3;
                reset();

                div_EditDialog_LoadData();

                ToastShow("修改成功");
            },
            function fail() {
                ToastShow("修改失敗");
            }
        );
    }
    ;
}
//#region 單據Head

//#region 新增
function _3PL_QuotationHead_Addon_New() {
    var dfd = $.Deferred();

    var sqlcmd = "Insert Into _3PL_QuotationHead_Addon"
        + "(PaperNo,SupdID,BOSS,TEL,FAX,CreateID,CreateDate,UpdateID,UpdateDate,QuotationDate,PayDate,Memo,Status,LinkPriceSeq)";
    sqlcmd += "values"
        + "(@PaperNo,@SupdID,@BOSS,@TEL,@FAX,@UserID,GETDATE(),@UserID,GETDATE(),@QuotationDate,@PayDate,@Memo,10,@LinkPriceSeq)";
    sqlcmd += "; SELECT ID=SCOPE_IDENTITY()";

    //#endregion
    $.fn.runAjax(
        DB_ScOffer_SupMonthPriceMax
        , "sqlcmd"
        , sqlcmd
        , saveHeadparams, function (response) {
            dfd.resolve(response[0].ID);
        }, function (response) {
            dfd.reject();
        });

    return dfd.promise();
}

//#endregion 新增

//#region 修改
function _3PL_QuotationHead_Addon_Edit(success, fail) {
    saveHeadparams.push({ Name: "@SN", Value: EditData.Seq });

    var sqlcmd = "Update _3PL_QuotationHead_Addon "
        + "Set SupdID=@SupdID,BOSS=@BOSS,TEL=@TEL,FAX=@FAX,UpdateID=@UserID,UpdateDate=GETDATE(),QuotationDate=@QuotationDate,PayDate=@PayDate,Memo=@Memo"
        + " WHERE Seq=@SN";

    $.fn.runAjax(
        DB_ScOffer_SupMonthPriceMax
        , "sqlcmd"
        , sqlcmd
        , saveHeadparams, success, fail);
}
//#endregion 修改

//#region 作廢
function _3PL_QuotationHead_Addon_Dele(success, fail) {
    var sqlcmd = "Update _3PL_QuotationHead_Addon set [status]=0 where Seq=@Sn";
    var ht1 = [];
    ht1.push({ Name: "@Sn", Value: EditData.Seq });

    $.fn.runAjax(
        DB_ScOffer_SupMonthPriceMax
        , "sqlcmd"
        , sqlcmd
        , ht1, success, fail);
}
//#endregion 作廢

//#endregion 單據Head

//#region 單據明細
function _3PL_QuotationDetail_Addon_New(QHeadSeq) {

    var Origindata = $("#jqGrid_Detail").jqGrid('getGridParam', 'data');

    //每一個row
    $.each(Origindata, function (key, value) {
        //key, value
        //1   , {日期: "2017-12-19", 單價: "3", 數量: "5", 項目: "無異動", 出/返: "無", …}

        var SQLparams = [];         //變數陣列
        var SQLparams_str = '';     //變數陣列轉為字串
        var SQLformula_str = JSON.stringify([{ Name: DefaultFormula }]);    //公式轉為字串
        var SQLPrice = value.Price; //單價
        var sqlcmd = '', ht1 = [];
        var NeedSendSQL = false;

        switch (value.RowStatus) {
            case 'Added':       //新增
                NeedSendSQL = true;
                sqlcmd = 'Insert Into _3PL_QuotationDetail_Addon'
                        + '(QHeadSeq,DisplayOrder,params,formula,Price ) '
                        + 'values'
                        + '(@QHeadSeq, @DisplayOrder,@params, @formula, @Price)';
                break;
            case 'Modified':    //修改
                NeedSendSQL = true;
                sqlcmd = 'Update _3PL_QuotationDetail_Addon set DisplayOrder=@DisplayOrder, params=@params, formula=@formula, Price=@Price Where Seq=@DetailSeq';
                break;
            case 'Deleted':    //刪除
                NeedSendSQL = true;
                sqlcmd = 'Delete From _3PL_QuotationDetail_Addon Where Seq=@DetailSeq';
                break;
            case 'UnModified':  //無變動
                break;
            default:
                break;
        }

        if (NeedSendSQL) {
            //原始的欄位名稱清單, 回寫到sql的params, 只會紀錄預設的欄位內容
            $.each(DefaultParams, function (key2, value2) {
                //value2
                //{Name: "調單類型", type: "select", Value: "0:進貨驗收單;1:退廠單"}

                value2.Value = '';

                $.each(value, function (colName, colValue) {
                    //colName, colValue
                    //日期   , 2017-12-21
                    if (colName == value2.Name) {
                        value2.Value = colValue;
                    }
                });

                //照公式順序回寫
                SQLparams.push({
                    Name: value2.Name
                        , Value: value2.Value
                });
            });

            SQLparams_str = JSON.stringify(SQLparams);

            ht1 = [];
            ht1.push({ Name: "@QHeadSeq", Value: QHeadSeq });
            ht1.push({ Name: "@DisplayOrder", Value: value.DisplayOrder });
            ht1.push({ Name: "@params", Value: SQLparams_str });
            ht1.push({ Name: "@formula", Value: SQLformula_str });
            ht1.push({ Name: "@Price", Value: SQLPrice });
            ht1.push({ Name: "@DetailSeq", Value: value.DetailSeq });

            //sql
            $.fn.runAjax(
            DB_ScOffer_SupMonthPriceMax
            , "sqlcmd"
            , sqlcmd
            , ht1);
        }
    });
}
//#endregion 單據明細

//#endregion Save

//#region Head Delete
function div_EditDialog_btn_Dele() {
    ConfirmBoxShow("確定刪除？", "", function () {
        _3PL_QuotationHead_Addon_Dele(
            function success() {
                ToastShow("刪除成功");

                FormStatus = 0;
                reset();

                searchHead();
            },
            function fail() {
                ToastShow("刪除失敗");
            }
        );
    });
}
//#endregion 

//#region Head Cancel
function div_EditDialog_btn_Canc() {
    ToastShow("取消");

    FormStatus = 5;
    reset();

    div_EditDialog_LoadData();


}
//#endregion

//#endregion

//#region jqGrid

//jqGrid_New_Detail
var GridData = '';
function setGrid() {
    $("#jqGrid").jqGrid({
        datatype: 'local',
        data: GridData,
        //建立欄位名稱及屬性
        colNames: ['狀態', '單號', '供應商', '報價日', '付款日', '備註'],
        colModel:
            [
                { name: 'Status', index: 'Status', width: 30 },
                { name: 'PaperNo', index: 'PaperNo', width: 60 },
                { name: 'SupdID', index: 'SupdID', width: 30 },
                { name: 'QuotationDate', index: 'QuotationDate', width: 70 },
                { name: 'PayDate', index: 'PayDate', width: 70 },
                { name: 'Memo', index: 'Memo', width: 150 }
            ],
        //建立grid屬性，editurl 是指資料新增、編輯及刪除時的伺服器端程式 
        pager: '#jqGridPager',
        autowidth: true,    //自动匹配宽度  
        height: 'auto',
        rownumbers: true,   //添加左侧行号  
        rownumWidth: 30,
        altRows: true,      //单双行样式不同 
        loadonce: false,
        rowNum: 10,         //每页显示记录数  
        rowList: [10, 20, 30],  //用于改变显示行数的下拉列表框的元素数组  
        viewrecords: true,  //显示总记录数  
        onSelectRow: function () {
            EditIndex = $("#jqGrid").jqGrid('getGridParam', 'selrow');
            div_EditDialog_LoadData();
        }
    });
    $("#jqGrid")
    .navGrid('#jqGridPager', { edit: false, add: false, del: false, search: false });
}
function setGrid_reset(result) {
    $('#jqGrid').jqGrid("clearGridData", true);
    $('#jqGrid').trigger('reloadGrid');

    GridData = '';

    if (result != null) {
        GridData = result;

        $('#jqGrid').jqGrid('setGridParam', { data: GridData });
        $('#jqGrid').trigger('reloadGrid');
    }
}

var GridDataDetail = '';
var lastSelection;          //GridDataDetail最後一筆修改的資料
function setGridDetail() {
    var sqlcmd = "select Params=MAX(CASE WHEN I_Addon_Type=0 THEN S_Addon_Formula ELSE '' END)"
        + " , Formula=MAX(CASE WHEN I_Addon_Type=1 THEN S_Addon_Formula ELSE '' END)"
        + " from _3PL_BaseCostSet_Addon with(nolock) where I_Addon_bcseSeq=@sn ";
    var ht1 = [{ Name: "@sn", Value: DefaultSn }];

    $.fn.runAjax(
        DB_ScOffer_SupMonthPriceMax
        , "sqlcmd"
        , sqlcmd
        , ht1, function (response) {
            var colNames = [];
            var colModel = [];
            //Head
            $.each(response, function (key, value) {
                //#region 各種預設值
                var NewMaxDate = _3PLgetDate(0);
                //#endregion

                DefaultFormula = JSON.parse(value.Formula)[0].Name;
                DefaultParams = JSON.parse(value.Params);
                $.each(DefaultParams, function (key, value) {
                    colNames.push(value.Name);

                    switch (value.type) {
                        case 'text':
                            var defaulteditoptions = {};
                            if (value.Name == '單價')
                                defaulteditoptions.defaultValue = DefaultSinglePrice;
                            else
                                defaulteditoptions.defaultValue = value.Value;

                            colModel.push({
                                name: value.Name
                                , index: value.Name
                                , edittype: "text"
                                , editable: true
                                , editoptions: defaulteditoptions
                            });
                            break;
                        case 'select':
                            colModel.push({
                                name: value.Name
                                , index: value.Name
                                , edittype: "select"
                                , editoptions: {
                                    value: value.Value
                                }
                                , editable: true
                            });
                            break;
                        case 'date':
                            colModel.push({
                                name: value.Name
                                , index: value.Name
                                , edittype: "text"
                                , editoptions: {
                                    defaultValue: NewMaxDate
                                    , dataInit: function (element) {
                                        $(element).datepicker({
                                            dateFormat: 'yy-mm-dd',
                                            orientation: 'auto bottom'
                                        })
                                    }
                                }
                                , editable: true
                            });
                            break;
                        case 'calculate':
                            colModel.push({
                                name: value.Name
                                , index: value.Name
                            });
                            break;
                        default:
                            colModel.push({
                                name: value.Name
                                , index: value.Name
                                , edittype: "text"
                                , editable: true
                            });
                            break;
                    }

                });

                //以下為固定欄位
                colNames.push("序號");
                colModel.push({ name: "DetailSeq", index: "DetailSeq", hidden: true, editoptions: { defaultValue: '-1' } });
                colNames.push("jqGrid專用");
                colModel.push({ name: "id", index: "id", hidden: true });
                colNames.push("金額");
                colModel.push({ name: "Price", index: "Price" });
                colNames.push("狀態");
                colModel.push({ name: "RowStatus", index: "RowStatus", hidden: true, editoptions: { defaultValue: 'Added' } });
                colNames.push("移動");
                colModel.push({ name: "DisplayOrder", index: "DisplayOrder", hidden: false, sorttype: 'number', formatter: setGridDetail_moveButtons });
            });

            //設定jqGrid
            $("#jqGrid_Detail").jqGrid({
                datatype: 'local',
                data: GridDataDetail,
                colNames: colNames,
                colModel: colModel,
                //建立grid屬性，editurl 是指資料新增、編輯及刪除時的伺服器端程式 
                pager: '#jqGrid_Detail_Pager',
                autowidth: true,    //自动匹配宽度  
                height: 'auto',
                rownumWidth: 30,
                altRows: true,      //单双行样式不同 
                loadonce: false,
                rowNum: 30,         //每页显示记录数  
                rowList: [10, 20, 30],  //用于改变显示行数的下拉列表框的元素数组  
                viewrecords: true,  //显示总记录数 
                sortname: 'DisplayOrder',
                sortorder: 'asc',
                onSelectRow: setGridDetail_EditButtonClick
            });
            $("#jqGrid_Detail")
            .navGrid('#jqGrid_Detail_Pager', { edit: false, add: false, del: false, search: false })
            .navButtonAdd('#jqGrid_Detail_Pager', {
                caption: "新增",
                buttonicon: "ui-icon-plus",
                onClickButton: setGridDetail_NewButtonClick,
                position: "last"
            })
            .navButtonAdd('#jqGrid_Detail_Pager', {
                caption: "刪除",
                buttonicon: "ui-icon-trash",
                onClickButton: setGridDetail_DelButtonClick,
                position: "last"
            });
        });
}
function setGridDetail_reset(result) {

    $('#jqGrid_Detail').jqGrid("clearGridData", true);
    $('#jqGrid_Detail').trigger('reloadGrid');

    if (result != null) {
        GridDataDetail = result;

        $('#jqGrid_Detail').jqGrid('setGridParam', { data: GridDataDetail });
        $('#jqGrid_Detail').trigger('reloadGrid');
    }
}
//按下新增
function setGridDetail_NewButtonClick() {
    if ($.inArray(FormStatus, [1, 2]) < 0)
        return;

    //先存舊資料
    setGridDetail_save()
    .then(function (response) {
        var grid = $("#jqGrid_Detail");
        var Origindata = grid.jqGrid('getGridParam', 'data');

        grid.jqGrid("addRow", {
            position: "last"
            , initdata: {
                DisplayOrder: Origindata.length + 1
            }
        });
        var id = grid.jqGrid('getGridParam', 'selrow');
        lastSelection = id;
    });
}
//按下刪除
function setGridDetail_DelButtonClick(postdata) {
    if ($.inArray(FormStatus, [1, 2]) < 0)
        return;

    var grid = $("#jqGrid_Detail");
    var id = grid.jqGrid('getGridParam', 'selrow');
    if (id) {
        grid.jqGrid('setRowData', id, { RowStatus: 'Deleted' });

        var myfilter = {
            groupOp: "AND",
            rules: [
                { field: "RowStatus", op: "ne", data: "Deleted" }
            ]
        }
        grid[0].p.search = myfilter.rules.length > 0;
        $.extend(grid[0].p.postData, { filters: JSON.stringify(myfilter) });
        grid.trigger("reloadGrid", [{ page: 1 }]);
    }
}
//按下修改 inline edit
function setGridDetail_EditButtonClick(id) {
    if ($.inArray(FormStatus, [1, 2]) < 0)
        return;

    if (id && id !== lastSelection) {
        //先存舊資料
        setGridDetail_save()
        .then(function (response) {
            var grid = $("#jqGrid_Detail");
            grid.jqGrid('editRow', id, { keys: true });
            lastSelection = id;
        });
    }
}

//自訂上下移動按鈕
function setGridDetail_moveButtons(cellvalue, options, rowObject) {
    return rowObject.DisplayOrder
          + '<input type="button" value="▲" class="btn-default" onclick="moveButtonAction(' + rowObject.DisplayOrder + ',\'up\')">'
          + '<input type="button" value="▼" class="btn-default" onclick="moveButtonAction(' + rowObject.DisplayOrder + ',\'down\')">';
}
function moveButtonAction(keyDisplayOrder, Action) {
    if ($.inArray(FormStatus, [1, 2]) < 0)
        return;

    setGridDetail_save();

    var grid = $("#jqGrid_Detail");
    if (keyDisplayOrder <= 0) return;

    var Origindata = grid.jqGrid('getGridParam', 'data');

    //從seq找出row
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

    //更換seq
    Origindata[rowIndex].DisplayOrder = nextkeyDisplayOrder;
    if (Origindata[rowIndex].RowStatus == 'UnModified')
        Origindata[rowIndex].RowStatus = 'Modified';
    Origindata[nextIndex].DisplayOrder = keyDisplayOrder;
    if (Origindata[nextIndex].RowStatus == 'UnModified')
        Origindata[nextIndex].RowStatus = 'Modified';

    grid.trigger('reloadGrid');
}

//存檔
function setGridDetail_save() {
    var dfd = $.Deferred();

    var grid = $("#jqGrid_Detail");
    grid.jqGrid('saveRow', lastSelection, { url: 'clientArray' });

    //自動帶入
    EditRows_BringValue(lastSelection)
    .then(function (response) {
        //計算公式
        EditRows_Calculate(lastSelection);
    })
    .then(function (response) {
        dfd.resolve();
    });

    return dfd.promise();
}

//自動帶入 品名, 入數, 板數
function EditRows_BringValue(rowid) {
    var dfd = $.Deferred();

    var grid = $("#jqGrid_Detail");
    var Row = grid.jqGrid('getLocalRow', rowid);

    //預設帶入觀音的貨號資料
    var itemNo = Row.貨號;
    if (itemNo == undefined || itemNo == null || itemNo.length <= 0) {
        return dfd.resolve();
    }

    var sqlcmd =
        "select Name=a.N_merd_name, boxqty=ISNULL(b.I_merp_1qty,1)"
		+ " ,pltqty=ISNULL(b.I_merp_pacti,1)*ISNULL(b.I_merp_pachi,1)"
		+ " ,barcode=ISNULL(b.S_merp_barcode,'')"
		+ " from mer_data a with(nolock)"
		+ " inner join mer_package b with(nolock)"
		+ " on a.L_merd_sysno=b.L_merp_merdsysno and b.I_merp_boxflag=1"
        + " where S_merd_id=@itemNo";
    var ht1 = [{ Name: "@itemNo", Value: itemNo }];

    $.fn.runAjax(
       "DC01"
       , "sqlcmd"
       , sqlcmd
       , ht1, function (response) {
           if (response.length > 0) {
               //尋找欄位名稱相同的帶入
               var updateJSON = {
                   品名: response[0].Name
                   , 入數: response[0].boxqty
                   , 板數: response[0].pltqty
                   , 條碼: response[0].barcode
               };
               grid.jqGrid('setRowData', rowid, updateJSON);
           }
           dfd.resolve();
       });

    return dfd.promise();
}

//修改時自動計算公式
function EditRows_Calculate(rowid) {
    var dfd = $.Deferred();

    setTimeout(function () {

        var grid = $("#jqGrid_Detail");
        var Row = grid.jqGrid('getLocalRow', rowid);
        var updateJSON = {};

        //攻勢
        var formula = DefaultFormula;
        //變數
        var data = [];

        $.each(DefaultParams, function (key2, value2) {
            //value2
            //{Name: "理貨費", Value: undefined, type: "calculate"}

            //先帶入全部變數
            $.each(Row, function (_name, _value) {
                //_name , _value
                //理貨費, 12345

                if (_name == value2.Name && value2.type != 'calculate') {
                    value2.Value = _value;
                }
            });
            data.push({ Name: value2.Name, Value: value2.Value, type: value2.type });
        });

        //替換掉變數清單的內容
        $.each(data, function (key3, value3) {
            //value3
            //{Name: "理貨費", Value: undefined, type: "calculate"}

            if (value3.type == 'calculate') {
                var tempResult = Formula_Calculate(value3.Value, data);

                updateJSON[value3.Name] = tempResult.result;
                value3.Value = tempResult.result;
            }
        });

        //結果
        //回傳結果
        var _result = '';
        _result = Formula_Calculate(formula, data);

        //判斷是新增還是修改
        var myRowStatus = Row.RowStatus;
        if (myRowStatus == 'UnModified')
            myRowStatus = 'Modified';

        updateJSON.Price = Math.round(_result.result);
        updateJSON.RowStatus = myRowStatus;
        grid.jqGrid('setRowData', rowid, updateJSON);

        dfd.resolve();
    }, 0);

    return dfd.promise();
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

//#region Form Control
var FormStatus = 0; //0-search, 1-New, 2-Edit, 3-Save, 4-Del, 5-Cancel
function reset() {
    switch (FormStatus) {
        case 0:
            $('#div_Query').show();
            $('#div_EditDialog').hide();
            $('#div_EditDialog input').prop('disabled', true);
            $('#div_EditDialog textarea').prop('disabled', true);

            $('#div_EditDialog_btn_Edit').show();   //修改
            $('#div_EditDialog_btn_Save').hide();   //存檔
            $('#div_EditDialog_btn_Dele').hide();   //作廢
            $('#div_EditDialog_btn_Canc').hide();   //取消

            setGrid_reset(null);
            setGridDetail_reset(null);
            break;
        case 1:
            $('#div_Query').hide();
            $('#div_EditDialog').show();
            $('#div_EditDialog input').prop('disabled', false);
            $('#div_EditDialog textarea').prop('disabled', false);
            $('#div_EditDialog_PaperNo').prop('disabled', true); //單號

            $('#div_EditDialog_btn_Edit').hide();   //修改
            $('#div_EditDialog_btn_Save').show();   //存檔
            $('#div_EditDialog_btn_Dele').hide();   //作廢
            $('#div_EditDialog_btn_Canc').show();   //取消

            $('#div_EditDialog input').val('');
            $('#div_EditDialog textarea').val('');

            $('#div_EditDialog_QuotationDate').val(_3PLgetDate(0));

            setGrid_reset(null);
            setGridDetail_reset(null);
            break;
        case 2:
            $('#div_Query').hide();
            $('#div_EditDialog').show();
            $('#div_EditDialog input').prop('disabled', false);
            $('#div_EditDialog textarea').prop('disabled', false);
            $('#div_EditDialog_PaperNo').prop('disabled', true); //單號

            $('#div_EditDialog_btn_Edit').hide();   //修改
            $('#div_EditDialog_btn_Save').show();   //存檔
            $('#div_EditDialog_btn_Dele').show();   //作廢
            $('#div_EditDialog_btn_Canc').show();   //取消
            break;
        case 3:
            $('#div_Query').show();
            $('#div_EditDialog').hide();
            $('#div_EditDialog input').prop('disabled', true);
            $('#div_EditDialog textarea').prop('disabled', true);

            $('#div_EditDialog_btn_Edit').show();   //修改
            $('#div_EditDialog_btn_Save').hide();   //存檔
            $('#div_EditDialog_btn_Dele').hide();   //作廢
            $('#div_EditDialog_btn_Canc').hide();   //取消
            break;
        case 4:
            break;
        case 5:
            $('#div_Query').show();
            $('#div_EditDialog').hide();
            $('#div_EditDialog input').prop('disabled', true);
            $('#div_EditDialog textarea').prop('disabled', true);

            $('#div_EditDialog_btn_Edit').show();   //修改
            $('#div_EditDialog_btn_Save').hide();   //存檔
            $('#div_EditDialog_btn_Dele').hide();   //作廢
            $('#div_EditDialog_btn_Canc').hide();   //取消
            break;
    }

}
//#endregion

//列印報表
function div_EditDialog_btn_Print() {


    $.fn.runAjax(
         DB_ScOffer_SupMonthPriceMax
         , "excel"
         , "sp_3PL_QuotationHead_Addon_Report1"
         , [
             { Name: '@QHeadNo', Value: $('#div_EditDialog_PaperNo').val() }
             , { Name: '@pageType', Value: 0 }
             , { Name: 'FileName', Value: $('#div_EditDialog_PaperNo').val() + $('title').text() }
         ], function (response) {

         });
}