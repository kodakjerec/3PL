var DB_ScOffer_SupMonthPriceMax = 'EDI';

$(
    function init() {
        ////#region Init
        $('#div_EditDialog_LittleWinform').dialog({
            autoOpen: false,
            width: '80%',
            modal: true,
            resizable: true,
            title: '新增計價費用'
        });

        $("#div1_input_Bdate").datepicker({ dateFormat: 'yy-mm-dd' });
        $("#div1_input_Edate").datepicker({ dateFormat: 'yy-mm-dd' });

        $('div[data-myDOM="myCheckbox"]').click(function () {

            myCheckbox_ChangeColor($(this), true);
        });

        FormStatus = 0;
        reset();
        setGrid();

        getSC_Offer_SiteNo()
        .then(function (response) {
            setGridDetail();
            setGrid_Fee();
        })
        ;

    });

//變更checkbox顏色
//sender: div               //要變更的div         
//ChangeChecked: boolean    //是否自動變更選取, 啟用:true
function myCheckbox_ChangeColor(sender, ChangeChecked) {
    var obj = '#' + sender.attr('id') + '_input';

    //變更checked
    if (ChangeChecked) {
        if (!$(obj).prop("disabled"))
            $(obj).prop("checked", !$(obj).prop("checked"));
    }

    if ($(obj).prop("checked")) {
        sender.removeClass('btn-default').addClass('btn-primary');
    }
    else {
        sender.removeClass('btn-primary').addClass('btn-default');
    }
}

//#region 取得 費用清單
var SC_Offer_Type_Fee = [];
function getSC_Offer_Type_Fee() {
    var dfd = $.Deferred();

    SC_Offer_Type_Fee = [];

    $.fn.runAjax(
        DB_ScOffer_SupMonthPriceMax
        , "sqlcmd"
        , "Select Checked=0, a.Object_No, a.Charge_Cate, a.Work_Code, a.Work_Type, a.Work_Name, a.Charge_Type, a.Chage_amount "
        + ", Charge_Cate_Name=convert(varchar,a.Charge_Cate)+','+b1.Field_Name "
        + ", Work_Type_Name=a.Work_Type+','+b2.Field_Name "
        + ", Charge_Type_Name=convert(varchar,a.Charge_Type)+','+b3.Field_Name "
        + " FROM SC_Offer_Type_Fee a with(nolock) "
        + " inner join SC_Offer_Field_Name b1 with(nolock) on a.Charge_Cate=b1.Field_Code and b1.Cate_Code='Charge_Cate' "
        + " inner join SC_Offer_Field_Name b2 with(nolock) on a.Work_Type=b2.Field_Code and b2.Cate_Code='Work_Type' "
        + " inner join SC_Offer_Field_Name b3 with(nolock) on a.Charge_Type=b3.Field_Code and b3.Cate_Code='Charge_Type' "
        + " where a.Object_No='0000' or a.Object_No=@ID "
        + " Order By a.Object_No, a.Work_Code"
        , [{ Name: "@ID", Value: $('#div_EditDialog_Object_No').val() }], function (response) {
            SC_Offer_Type_Fee = response;
            dfd.resolve();
        });

    return dfd.promise();
}
//#endregion 取得 費用清單

//#region 取得 倉別清單
var SC_Offer_SiteNo = [];
function getSC_Offer_SiteNo() {
    var dfd = $.Deferred();

    $.fn.runAjax(
        DB_ScOffer_SupMonthPriceMax
        , "sqlcmd"
        , "select Value=Field_Code, Name=Field_Name from SC_Offer_Field_Name with(nolock) where Cate_Code='Contract_Ares'"
        , []
        , function (response) {
            SC_Offer_SiteNo = response;
            dfd.resolve();
        });

    return dfd.promise();
}
//#endregion 取得 倉別清單

//#region jqGrid
var PriceTypeList = ''; //計價類別清單
var GridData = '';
function setGrid() {
    $("#jqGrid").jqGrid({
        datatype: 'local',
        data: GridData,
        //建立欄位名稱及屬性
        colNames: ['單號', '供應商', '名稱', '一次性進貨', '備註', 'Contract_Ares', 'OT_Flg'],
        colModel:
            [
                { name: 'Offer_No', index: 'Offer_No', sorttype: "text" },
                { name: 'Object_No', index: 'Object_No', sorttype: "text" },
                { name: 'ALIAS', index: 'ALIAS' },
                { name: 'OT_Flg_View', index: 'OT_Flg' },
                { name: 'Memo', index: 'Memo' },
                { name: 'Contract_Ares', hidden: true },
                { name: 'OT_Flg', hidden: true },
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
        //rowList: [20, 30, 40],  //用于改变显示行数的下拉列表框的元素数组  
        viewrecords: true,  //显示总记录数  
        onSelectRow: function () {
            EditIndex = $("#jqGrid").jqGrid('getGridParam', 'selrow');
            div_EditDialog_LoadData();
        }
    });
    $("#jqGrid")
    .navGrid('#jqGridPager', { edit: false, add: false, del: false, search: false })
    .jqGrid('filterToolbar', { stringResult: true, searchOnEnter: false, defaultSearch: "cn" })
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
//#endregion

//#region jqGridDetail
var lastSelection = -1;
var GridDataDetail = '';
function setGridDetail() {
    var NewMaxDate = _3PLgetDate(0);

    var colNames = ['貨號'
        , '倉別Value'
        , '倉別'];    //關鍵字
    var colModel = [{ name: 'Object_Code', index: 'Object_Code' }
        , { name: 'Ware', index: 'Ware', hidden: true }
        , { name: 'WareName', index: 'WareName' }];

    colNames.push('計價日期 起');
    colModel.push({ name: 'AmountDateS', edittype: "text" });
    colNames.push('計價日期 迄');
    colModel.push({ name: 'AmountDateE', edittype: "text" });
    colNames.push('截止日');
    colModel.push({ name: 'stop_date', edittype: "text" });
    colNames.push("序號");
    colModel.push({ name: "DisplayOrder", index: "DisplayOrder", key: true });
    colNames.push("狀態");
    colModel.push({ name: "RowStatus", hidden: false, editoptions: { defaultValue: 'Added' } });

    $.each(SC_Offer_Type_Fee, function (_key2, _value2) {
        colNames.push(_value2.Work_Name);
        colModel.push({ name: _value2.Work_Code });
    });

    //設定jqGrid
    $.jgrid.gridUnload("#jqGrid_Detail");

    $("#jqGrid_Detail")
    .jqGrid({
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
        multiSort: true
    })
    .jqGrid('sortGrid', 'Object_Code', true, "asc")
    .jqGrid('sortGrid', 'Ware', true, "asc")
    .navGrid('#jqGrid_Detail_Pager', { edit: false, add: false, del: false, search: false })


    //增加操作按鈕, 不重複產生
    if ($('#setGridDetail_NewButton').length <= 0) {

        $("#jqGrid_Detail")
        .navButtonAdd('#jqGrid_Detail_Pager', {
            caption: "新增",
            buttonicon: "ui-icon-plus",
            onClickButton: setGridDetail_NewButtonClick,
            position: "last",
            id: "setGridDetail_NewButton"
        })
        .navButtonAdd('#jqGrid_Detail_Pager', {
            caption: "刪除",
            buttonicon: "ui-icon-trash",
            onClickButton: setGridDetail_DelButtonClick,
            position: "last",
            id: "setGridDetail_DelButton"
        })
    }
}
//group-head
function setGridDetail_groupHead() {
    var _1stGroupHead = 0, _2ndGroupHead = 0;
    var _1stGroupCols = 0, _2ndGroupCols = 0;
    var _1stSupName = '0000', _2ndSupName = '';

    $.each(SC_Offer_Type_Fee, function (key, value) {
        switch (value.Object_No) {
            case _1stSupName:
                if (_1stGroupHead == 0)
                    _1stGroupHead = value.Work_Code;
                _1stGroupCols++;
                break;
            default:
                if (_2ndGroupHead == 0) {
                    _2ndGroupHead = value.Work_Code;
                    _2ndSupName = value.Object_No;
                }
                _2ndGroupCols++;
                break;
        }
    });

    $('#jqGrid_Detail')
    .jqGrid('setGroupHeaders', {
        useColSpanStyle: false,
        groupHeaders: [
            { startColumnName: _1stGroupHead, numberOfColumns: _1stGroupCols, titleText: _1stSupName }
            , { startColumnName: _2ndGroupHead, numberOfColumns: _2ndGroupCols, titleText: _2ndSupName }
        ]
    })
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

    var Sup_no = $('#div_EditDialog_Object_No').val();
    if (Sup_no.length == 0) {
        ToastShow('請輸入廠編');
        return;
    }

    $('#div_EditDialog_LittleWinform').dialog('open');

    BringSupNo_ItemList(Sup_no)
    .then(function (response) {
        if (response == undefined) {
            $('#div_EditDialog_LittleWinform').dialog('close');
            return;
        }
        div_EditDialog_LittleWinform_reset();
        $('#div_EditDialog_LittleWinform_Sup_No').text(Sup_no);
    })
    ;

}
//按下刪除
function setGridDetail_DelButtonClick() {
    if ($.inArray(FormStatus, [1, 2]) < 0)
        return;

    var grid = $("#jqGrid_Detail");
    var id = grid.jqGrid('getGridParam', 'selrow');
    var updateJSON = {};

    $.each(SC_Offer_Type_Fee, function (_key2, _value2) {
        updateJSON[_value2.Work_Code] = 'N';
    });
    updateJSON['RowStatus'] = 'Modified';


    if (id) {
        grid.jqGrid('setRowData', id, updateJSON);

        setGridDetail_reOrder();
    }
}
//重新排序
function setGridDetail_reOrder() {
    var grid = $("#jqGrid_Detail");

    //篩選
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

//#endregion jqGridDetail

//#region jqGrid_Fee
function setGrid_Fee() {
    $("#jqGrid_Fee").jqGrid({
        datatype: 'local',
        data: SC_Offer_Type_Fee,
        //建立欄位名稱及屬性
        //Checked, Object_No, Charge_Cate, Work_Code, Work_Type, Work_Name, Charge_Type, Chage_amount,Charge_Cate_Name,Work_Type_Name,Charge_Type_Name
        colNames: ['選擇', '供應商', '費用名稱', '單價', '作業大類', '收費類型', '作業類型'],
        colModel:
            [
                { name: 'Checked', index: 'Checked', editable: true, width: 60, formatter: 'checkbox', edittype: 'checkbox', editoptions: { value: '1:0', defaultValue: '0' } },
                { name: 'Object_No', index: 'Object_No' },
                { name: 'Work_Name', index: 'Work_Name' },
                { name: 'Chage_amount', index: 'Chage_amount' },
                { name: 'Charge_Cate_Name', index: 'Charge_Cate_Name' },
                { name: 'Work_Type_Name', index: 'Work_Type_Name' },
                { name: 'Charge_Type_Name', index: 'Charge_Type_Name' }
            ],
        //建立grid屬性，editurl 是指資料新增、編輯及刪除時的伺服器端程式 
        autowidth: true,    //自动匹配宽度  
        height: 'auto',
        rownumbers: true,   //添加左侧行号  
        rownumWidth: 30,
        altRows: true,      //单双行样式不同 
        loadonce: false,
        rowNum: 10,         //每页显示记录数  
        //rowList: [20, 30, 40],  //用于改变显示行数的下拉列表框的元素数组  
        viewrecords: true,  //显示总记录数  
    });
}

function setGrid_Fee_reset() {

    var grid = $('#jqGrid_Fee');

    grid.jqGrid("clearGridData", true).trigger('reloadGrid');
    grid.jqGrid('setGridParam', { data: SC_Offer_Type_Fee }).trigger('reloadGrid');
}

function setGrid_Fee_save() {
    var grid = $("#jqGrid_Fee");
    var idList = grid.jqGrid('getDataIDs');
    $.each(idList, function (key, id) {
        grid.jqGrid('saveRow', id, { url: 'clientArray' });
        grid.jqGrid('editRow', id, true);
    })
}
//#endregion jqGrid_Fee

//#region div_EditDialog_LittleWinform

//產生點選清單

//驗證
function div_EditDialog_LittleWinform_Object_Code_check() {
    var ItemNoList = $('#div_EditDialog_LittleWinform_Object_Code').val().split(/\r\n|\r|\n/g);
    var result = '';
    $.each(ItemNoList, function (_key, _value) {
        $.each(SupNo_ItemNoList, function (_key2, _value2) {
            if (_value == _value2.Value) {
                result += _value + '\r\n';
            }
        })
    })

    $('#div_EditDialog_LittleWinform_Object_Code_valid').val(result);
}

//清空
function div_EditDialog_LittleWinform_Object_Code_clear() {
    $('#div_EditDialog_LittleWinform_Object_Code_valid').val('');
}

//自動帶出廠商關聯貨號清單
var SupNo_ItemNoList = [];
function BringSupNo_ItemList(Sup_no) {
    var dfd = $.Deferred();

    if (SupNo_ItemNoList.length <= 0 || (SupNo_ItemNoList[0].SUP_ID != Sup_no)) {
        $('#div_EditDialog_LittleWinform_Object_Code').empty();
        SupNo_ItemNoList = [];
        $.fn.runAjax(
       DB_ScOffer_SupMonthPriceMax
       , "sqlcmd"
       , "select Value=ITEM_ID, Name=ALIAS, SUP_ID from DRP.dbo.DRP_ITEM with(nolock) where SUP_ID=@SUP_ID Order by ITEM_ID"
       , [{ Name: "@SUP_ID", Value: Sup_no }], function (response) {
           SupNo_ItemNoList = response;

           if (SupNo_ItemNoList.length == 0) {
               ToastShow('廠編找不到對應的貨號清單');
               dfd.reject(undefined);
           }

           dfd.resolve('');
       });
    }

    return dfd.promise();
}

//儲存
function div_EditDialog_LittleWinform_button_save() {
    //整理UI資料
    var Sup_no = $('#div_EditDialog_LittleWinform_Sup_No').text();
    var Item_no_List = $('#div_EditDialog_LittleWinform_Object_Code_valid').val().split(/\r\n|\r|\n/g);
    var Site_No_List = [
        { 'Value': 1, 'Enabled': $('#div_EditDialogg_LittleWinform_Contract_Ares_1_input').prop('checked') }
        , { 'Value': 2, 'Enabled': $('#div_EditDialogg_LittleWinform_Contract_Ares_2_input').prop('checked') }
        , { 'Value': 3, 'Enabled': $('#div_EditDialogg_LittleWinform_Contract_Ares_3_input').prop('checked') }
    ];
    var B_date = $('#div1_input_Bdate').val();
    var E_date = $('#div1_input_Edate').val();

    //檢查
    var ErrMsg = '';

    if (B_date.length == 0 || E_date.length == 0)
        ErrMsg = '計價日期不正確';

    if (ErrMsg.length > 0) {
        MessageBoxShow(ErrMsg);
        return;
    }

    //費用先存檔
    setGrid_Fee_save();
    var Select_FeeList = $('#jqGrid_Fee').jqGrid('getGridParam', 'data');

    //準備開始更新至明細
    var grid = $("#jqGrid_Detail");
    var Item_no = '', Site_no = '', Site_Name = '', Site_Enabled = false;
    //By 貨號
    $.each(Item_no_List, function (_key_Item, _value_Item) {
        if (_value_Item.length > 0) {
            Item_no = _value_Item;

            //By 倉別
            $.each(Site_No_List, function (_key_Site, _value_Site) {

                Site_no = _value_Site.Value;
                Site_Enabled = _value_Site.Enabled;
                //取得倉別名稱
                $.each(SC_Offer_SiteNo, function (_key2, _value2) {
                    if (_value2.Value == Site_no) {
                        Site_Name = _value2.Name;
                    }
                });

                //倉別排序
                if (Site_Enabled) {
                    //檢查本張單據內有沒有相同關鍵字的資料
                    var IsNew_DisplayOrder = -1;
                    var OriginData = grid.jqGrid('getGridParam', 'data');

                    $.each(OriginData, function (_key_OriginData, _value_OriginData) {
                        if (Site_no == _value_OriginData.Ware
                            && Item_no == _value_OriginData.Object_Code) {
                            IsNew_DisplayOrder = _value_OriginData.DisplayOrder;
                        }
                    });

                    if (IsNew_DisplayOrder >= 0) {
                        //如果有, 套用更新

                        //Basic
                        var updateJSON = {
                            AmountDateS: B_date
                           , AmountDateE: E_date
                           , RowStatus: 'Modified'
                        };
                        //費用清單
                        $.each(Select_FeeList, function (key, value) {
                            if (value.Checked == 1)
                                updateJSON[value.Work_Code] = 'Y';
                            else
                                updateJSON[value.Work_Code] = 'N';
                        })
                        console.log(IsNew_DisplayOrder);
                        grid.jqGrid('setRowData', IsNew_DisplayOrder, updateJSON);
                    }
                    else {
                        //如果沒有, 新增

                        //Basic
                        var updateJSON = {
                            Ware: Site_no
                           , WareName: Site_Name
                           , Object_Code: Item_no
                           , AmountDateS: B_date
                           , AmountDateE: E_date
                           , RowStatus: 'Added'
                           , DisplayOrder: OriginData.length + 1
                        };
                        //費用清單
                        $.each(Select_FeeList, function (key, value) {
                            if (value.Checked == 1)
                                updateJSON[value.Work_Code] = 'Y';
                            else
                                updateJSON[value.Work_Code] = 'N';
                        })

                        grid.jqGrid("addRow", {
                            position: "last"
                            , initdata: updateJSON
                        });
                        var id = grid.jqGrid('getGridParam', 'selrow');

                        grid.jqGrid('saveRow', id, { url: 'clientArray' });
                    }
                }
            });
        }
    });

    setGridDetail_reOrder();
}

//取消
function div_EditDialog_LittleWinform_button_close() {
    $('#div_EditDialog_LittleWinform').dialog('close');
}

//RESET
function div_EditDialog_LittleWinform_reset() {
    setGrid_Fee_save();
    $('#div_EditDialog_LittleWinform_Object_Code').val('');
    $('#div_EditDialog_LittleWinform_Object_Code_valid').val('');
}

//#endregion

//#region Head

var EditData = {};  //修改中的物件
var EditIndex = 0;
//#region Loading

//取得SC_Offer_Header
//Mode=0,   要重整jqGrid, Mode=1, 不要
function search(Mode) {
    FormStatus = 0;
    reset();

    //#region get variables
    var SupID = $('#div_search_SupID').val();
    var OfferNo = $('#div_search_OfferNo').val();
    var ItemNo = $('#div_search_ItemNo').val();

    var sqlcmd = "select TOP 100 a.Offer_No, Object_No, b.ALIAS, a.Contract_Ares, a.OT_Flg "
        + ", OT_Flg_View=CASE a.OT_Flg WHEN 0 THEN 'N' WHEN 1 THEN 'Y' END, a.Memo "
        + " from SC_Offer_Header a with(nolock) "
        + " INNER JOIN DRP.dbo.DRP_SUPPLIER b with(nolock) on a.Object_No=b.ID "
        + " where 1=1 and a.Ware=1";
    if (SupID.length > 0)
        sqlcmd += " and Object_No=@SupID";
    if (OfferNo.length > 0)
        sqlcmd += " and Offer_No=@OfferNo";
    if (ItemNo.length > 0) {
        sqlcmd += " and a.Offer_No in ( Select DISTINCT Offer_No From SC_Offer_Detail with(nolock) where Object_Code=@ItemNo )"
    }
    sqlcmd += " order by Offer_No DESC, Object_No";
    //#endregion

    $.fn.runAjax(
        DB_ScOffer_SupMonthPriceMax
        , "sqlcmd"
        , sqlcmd
        , [
            { Name: '@SupID', Value: SupID }
            , { Name: '@OfferNo', Value: OfferNo }
            , { Name: '@ItemNo', Value: ItemNo }
        ], function (response) {

            $('#jqGrid').jqGrid("clearGridData", true);
            $('#jqGrid').trigger('reloadGrid');

            setGrid_reset(response);

            if (Mode == 0) {
                setGrid();
            }
            else {
                $('#jqGrid').jqGrid('setGridParam', { data: GridData });
                $('#jqGrid').trigger('reloadGrid');
            }
        });
}
//

//查詢單頭
function div_EditDialog_LoadData() {
    if (EditIndex == null) {
        MessageBoxShow('請選擇列');
        return;
    }
    if (EditIndex > 0) {
        //修改
        div_EditDialog_LoadData_searchData();

        $('#div_EditDialog_Offer_No').val(EditData.Offer_No);
        $('#div_EditDialog_Object_No').val(EditData.Object_No);
        $('#div_EditDialog_Object_Name').text(EditData.ALIAS);

        //一次性進貨
        $('#div_EditDialog_OT_Flg_input').prop("checked", false);
        if (EditData.OT_Flg == 1) {
            $('#div_EditDialog_OT_Flg_input').prop("checked", true);
        }
        myCheckbox_ChangeColor($('#div_EditDialog_OT_Flg'), false);

        $('#div_EditDialog_Memo').val(EditData.Memo);

        //讀取指定供應商的費用清單
        getSC_Offer_Type_Fee()
        .then(function (response) {
            //重整jqGrid_Detail
            setGridDetail();
            setGridDetail_groupHead();

            //填入jqGrid_Detail
            query_jrGridDetail(EditData.Offer_No);

            //填入jqGrid_Fee
            setGrid_Fee_reset();
            setGrid_Fee_save();
        })
    }
    else {
        //新增
        NewQHEAD();
    }

    $('#div_EditDialog').show();
}
//取得指定的詳細資料 from jqGrid
function div_EditDialog_LoadData_searchData() {
    var Row = $("#jqGrid").jqGrid('getRowData', EditIndex);
    $.each(GridData, function (key, value) {
        if (value.Offer_No == Row.Offer_No) {
            EditData = value;
            return;
        }
    });
}

//查詢明細資料
function query_jrGridDetail(Offer_no) {
    var sqlcmd = "select Ware, Object_Code, Work_Code, AmountDateS, AmountDateE, stop_date,Del_Flg "
    + " from SC_Offer_Detail with(nolock) "
    + " where Offer_No=@No";

    var ht1 = [{ Name: "@No", Value: Offer_no }];

    $.fn.runAjax(
        DB_ScOffer_SupMonthPriceMax
        , "sqlcmd"
        , sqlcmd
        , ht1, function (response) {
            //1. 取得原始資料
            if (response.length <= 0) {
                return;
            }

            var DisplayOrder = 1;    //每一個單據序號

            //2. 整理成 給使用者看的形式,
            //_key 1
            //_Value {Ware: "3", Object_Code: "90760215", Work_Code: 792, AmountDateS: "2018-01-23T00:00:00", AmountDateE: "2018-03-08T00:00:00"}
            var myDataTable = [];
            $.each(response, function (_key, _value) {
                //Row={"791":"Y","792":"Y","793":"Y","Ware":"1","Object_Code":"76210440","AmountDateS":"2018-01-30","AmountDateE":"2018-03-08","RowStatus":"UnModified","DisplayOrder":1}
                var Row = {};
                var IsNewRow = true;    //是否為新增Row, 啟用-true

                //尋找有沒有已存在的資料
                $.each(myDataTable, function (_key3, _value3) {
                    if (_value3.Ware == _value.Ware
                        && _value3.Object_Code == _value.Object_Code) {
                        Row = _value3;
                        IsNewRow = false;
                    }
                })

                //新資料
                if (Row.Ware == undefined) {
                    //日期格式變更
                    _value.AmountDateS = _value.AmountDateS.toString().substr(0, 10);
                    _value.AmountDateE = _value.AmountDateE.toString().substr(0, 10);
                    if (_value.stop_date != null)
                        _value.stop_date = _value.stop_date.toString().substr(0, 10);

                    Row.Ware = _value.Ware;
                    Row.Object_Code = _value.Object_Code;
                    Row.AmountDateS = _value.AmountDateS;
                    Row.AmountDateE = _value.AmountDateE;
                    Row.stop_date = _value.stop_date;

                    //費用帶入 Default
                    $.each(SC_Offer_Type_Fee, function (_key2, _value2) {
                        Row[_value2.Work_Code] = 'N';
                    });

                    //倉別替換
                    $.each(SC_Offer_SiteNo, function (_key2, _value2) {
                        if (_value2.Value == Row.Ware) {
                            Row.WareName = _value2.Name;
                        }
                    });
                }

                //費用帶入
                $.each(SC_Offer_Type_Fee, function (_key2, _value2) {
                    if (_value2.Work_Code == _value.Work_Code && _value.Del_Flg == 0) {
                        Row[_value2.Work_Code] = 'Y';
                    }
                })

                Row.RowStatus = 'UnModified';

                if (IsNewRow) {
                    Row.DisplayOrder = DisplayOrder;
                    DisplayOrder++;
                    myDataTable.push(Row);
                }
            });
            setGridDetail_reset(myDataTable);
        });
}

//#endregion

//#region New
//按下新增
function NewQHEAD() {
    FormStatus = 1;
    reset();
}

//供應商欄位驗證
function find_div_EditDialog_Object_Name(obj) {
    var Sup_No = $(obj).val();

    $.fn.runAjax(
        DB_ScOffer_SupMonthPriceMax
        , "sqlcmd"
        , "Select ALIAS FROM DRP.dbo.DRP_SUPPLIER with(nolock) where ID=@ID "
        , [{ Name: '@ID', Value: Sup_No }], function (response) {
            $('#div_EditDialog_Object_Name').text(response[0].ALIAS);

            //讀取指定供應商的費用清單
            getSC_Offer_Type_Fee()
            .then(function (response) {
                //重整jqGrid_Detail
                setGridDetail();
                setGridDetail_groupHead();

                //填入jqGrid_Detail
                //query_jrGridDetail(EditData.Offer_No);

                //填入jqGrid_Fee
                setGrid_Fee_reset();
                setGrid_Fee_save();
            })
        });
}
//#endregion

//#region Edit
function div_EditDialog_btn_Edit() {
    FormStatus = 2;
    reset();
}
//#endregion Edit

//#region Save
var saveHeadparams = [];
//get parameters
function div_EditDialog_SaveData() {
    EditData.Object_No = $('#div_EditDialog_Object_No').val();
    EditData.OT_Flg = $('#div_EditDialog_OT_Flg_input').prop("checked") == true ? 1 : 0;
    EditData.Memo = $('#div_EditDialog_Memo').val();

    saveHeadparams = [];
    saveHeadparams.push({ Name: "@Sup_No", Value: EditData.Object_No });
    saveHeadparams.push({ Name: "@OT_Flg", Value: EditData.OT_Flg });
    saveHeadparams.push({ Name: "@Memo", Value: EditData.Memo });

}

function div_EditDialog_btn_Save() {
    var ErrMsg = '';
    //檢查
    var grid = $("#jqGrid_Detail");
    var OriginData = grid.jqGrid('getGridParam', 'data');
    if (OriginData.length == 0)
        ErrMsg = "沒有明細";

    if ($('#div_EditDialog_Object_No').val().length == 0)
        ErrMsg = '供應商未填寫';

    if (ErrMsg.length > 0) {
        MessageBoxShow(ErrMsg);
        return;
    }

    //更新資料
    div_EditDialog_SaveData();

    if (FormStatus == 1) {
        //Head新增
        _3PL_SCOfferHead_getNo()
        .then(function (response) {
            //Head存檔
            EditData.Offer_No = response.NO;
            saveHeadparams.push({ Name: "@No", Value: EditData.Offer_No });
            _3PL_SCOfferHead_Addon_New();
            return response.NO;
        })
        .then(function (response) {
            var HeadNo = response;
            //Detail存檔
            _3PL_SCOfferDetail(HeadNo);

            FormStatus = 0;
            reset();

            ToastShow("新增成功");
        })
        .fail(function () {
            ToastShow("新增失敗");
        })
        ;
    }
    else {
        saveHeadparams.push({ Name: "@No", Value: EditData.Offer_No });

        //修改
        _3PL_SCOfferHead_Addon_Edit()
        .then(function () {
            var HeadNo = EditData.Offer_No;
            //明細存檔
            _3PL_SCOfferDetail(HeadNo);

            FormStatus = 3;
            reset();

            div_EditDialog_LoadData();

            ToastShow("修改成功");
        })
        .fail(function () {
            ToastShow("修改失敗");
        })
        ;
    }
}

//#region 取得單號
function _3PL_SCOfferHead_getNo() {
    var dfd = $.Deferred();

    $.fn.runAjax(
        DB_ScOffer_SupMonthPriceMax
        , "sp"
        , "spEDI_SC_getNo"
        , [{}], function (response) {
            dfd.resolve(response[0]);
        }, function (response) {
            dfd.reject();
        });

    return dfd.promise();
}
//#endregion 取得單號

//#region 單據Head

//#region 新增

function _3PL_SCOfferHead_Addon_New() {
    var dfd = $.Deferred();

    var sqlcmd = 'Insert Into [SC_Offer_Header](Offer_No_Ext,Offer_No,Object_No,OT_Flg,Memo,Create_Date,Ware,Contract_Ares)'
                        + ' values(@No,@No,@Sup_No,@OT_Flg,@Memo,GETDATE(),1,123);';

    //#endregion
    $.fn.runAjax(
        DB_ScOffer_SupMonthPriceMax
        , "sqlcmd"
        , sqlcmd
        , saveHeadparams
        , function success(response) {
            dfd.resolve(response);
        }
        , function fail(response) {
            dfd.reject();
        });

    return dfd.promise();
}

//#endregion 新增

//#region 修改
function _3PL_SCOfferHead_Addon_Edit() {
    var dfd = $.Deferred();

    var sqlcmd = "Update [SC_Offer_Header] set OT_Flg=@OT_Flg, Memo=@Memo, Up_Date=GETDATE() where Offer_No=@No; ";

    $.fn.runAjax(
        DB_ScOffer_SupMonthPriceMax
        , "sqlcmd"
        , sqlcmd
        , saveHeadparams
        , function success(response) {
            dfd.resolve(response);
        }
        , function fail(response) {
            dfd.reject();
        });

    return dfd.promise();
}
//#endregion 修改

//#endregion 單據Head

//#region 單據明細

//分類
function _3PL_SCOfferDetail(HeadNo) {

    var grid = $("#jqGrid_Detail");
    var OriginData = grid.jqGrid('getGridParam', 'data');

    $.each(OriginData, function (key, value) {
        switch (value.RowStatus) {
            case "UnModified":
                break;
            case "Added":
                _3PL_SCOfferDetail_Added(value);
                break;
            case "Modified":
                _3PL_SCOfferDetail_Modified(value);
                break;
            case "Deleted":
                _3PL_SCOfferDetail_Deleted(value);
                break;
        }
    });
}

//#region 新增
function _3PL_SCOfferDetail_Added(Row) {
    var sqlcmd0 = '', sqlcmd = '', ht1 = [];

    sqlcmd = "Insert Into SC_Offer_Detail( Offer_No_Ext, Offer_No, Object_No, Charge_Cate, Work_Code, Work_Type, Work_Name, Charge_Type, Chage_amount, Object_Code, Create_Date , Del_Flg , Ware, stop_date, stop_Flag, AmountDateS, AmountDateE, OT_Flg) "
           + " select Offer_No_Ext=@Offer_No, Offer_No=@Offer_No, Object_No=@Object_No, Charge_Cate, Work_Code, Work_Type, Work_Name, Charge_Type, Chage_amount, Object_Code=@Object_Code, Create_Date=GETDATE(), Del_Flg=0, Ware=@Ware, stop_date=NULL, stop_Flag='N', AmountDateS=@AmountDateS, AmountDateE=@AmountDateE, OT_Flg=@OT_Flg "
           + " from SC_Offer_Type_Fee with(nolock) where Work_Code=@Work_Code";

    //費用帶入
    $.each(SC_Offer_Type_Fee, function (_key2, _value2) {
        if (Row[_value2.Work_Code] != null && Row[_value2.Work_Code] == 'Y') {
            ht1 = [];
            ht1.push({ Name: "@Offer_No", Value: EditData.Offer_No });
            ht1.push({ Name: "@Object_No", Value: EditData.Object_No });
            ht1.push({ Name: "@Object_Code", Value: Row.Object_Code });
            ht1.push({ Name: "@Ware", Value: Row.Ware });
            ht1.push({ Name: "@AmountDateS", Value: Row.AmountDateS });
            ht1.push({ Name: "@AmountDateE", Value: Row.AmountDateE });
            ht1.push({ Name: "@OT_Flg", Value: EditData.OT_Flg });
            ht1.push({ Name: "@Work_Code", Value: _value2.Work_Code });

            console.log("NewRow");

            $.fn.runAjax(
               DB_ScOffer_SupMonthPriceMax
               , "sqlcmd"
               , sqlcmd
               , ht1);
        }
    })


}
//#endregion 新增

//#region 修改
function _3PL_SCOfferDetail_Modified(Row) {
    var sqlcmd0 = '', sqlcmd = '', ht1 = [];

    //費用帶入
    $.each(SC_Offer_Type_Fee, function (_key2, _value2) {
        if (Row[_value2.Work_Code] != null) {
            if (Row[_value2.Work_Code] == 'Y') {
                //新增 or 修改
                sqlcmd0 = "Declare @Count int=0;Select @Count=COUNT(1) From SC_Offer_Detail with(nolock) "
                + " where Offer_No=@Offer_No And Object_No=@Object_No And Object_Code=@Object_Code And Ware=@Ware And Work_Code=@Work_Code;"
                + " if (@Count=0) BEGIN "
                        + " Insert Into SC_Offer_Detail( Offer_No_Ext, Offer_No, Object_No, Charge_Cate, Work_Code, Work_Type, Work_Name, Charge_Type, Chage_amount, Object_Code, Create_Date , Del_Flg , Ware, stop_date, stop_Flag, AmountDateS, AmountDateE, OT_Flg) "
                        + " select Offer_No_Ext=@Offer_No, Offer_No=@Offer_No, Object_No=@Object_No, Charge_Cate, Work_Code, Work_Type, Work_Name, Charge_Type, Chage_amount, Object_Code=@Object_Code, Create_Date=GETDATE(), Del_Flg=0, Ware=@Ware, stop_date=NULL, stop_Flag='N', AmountDateS=@AmountDateS, AmountDateE=@AmountDateE, OT_Flg=@OT_Flg "
                        + " from SC_Offer_Type_Fee with(nolock) where Work_Code=@Work_Code"
                + " END "
                + " ELSE BEGIN "
                        + " Update SC_Offer_Detail Set AmountDateS=@AmountDateS, AmountDateE=@AmountDateE, OT_Flg=@OT_Flg,Upd_Date=GETDATE(),Del_Flg=0 "
                        + " where Offer_No=@Offer_No And Object_No=@Object_No And Object_Code=@Object_Code And Ware=@Ware And Work_Code=@Work_Code "
                + "END "

                ht1 = [];
                ht1.push({ Name: "@Offer_No", Value: EditData.Offer_No });
                ht1.push({ Name: "@Object_No", Value: EditData.Object_No });
                ht1.push({ Name: "@Object_Code", Value: Row.Object_Code });
                ht1.push({ Name: "@Ware", Value: Row.Ware });
                ht1.push({ Name: "@AmountDateS", Value: Row.AmountDateS });
                ht1.push({ Name: "@AmountDateE", Value: Row.AmountDateE });
                ht1.push({ Name: "@OT_Flg", Value: EditData.OT_Flg });
                ht1.push({ Name: "@Work_Code", Value: _value2.Work_Code });

                $.fn.runAjax(
                    DB_ScOffer_SupMonthPriceMax
                    , "sqlcmd"
                    , sqlcmd0
                    , ht1);
            }
            else {
                //刪除
                ht1 = [];
                ht1.push({ Name: "@Offer_No", Value: EditData.Offer_No });
                ht1.push({ Name: "@Object_No", Value: EditData.Object_No });
                ht1.push({ Name: "@Object_Code", Value: Row.Object_Code });
                ht1.push({ Name: "@Ware", Value: Row.Ware });
                ht1.push({ Name: "@AmountDateS", Value: Row.AmountDateS });
                ht1.push({ Name: "@AmountDateE", Value: Row.AmountDateE });
                ht1.push({ Name: "@OT_Flg", Value: EditData.OT_Flg });
                ht1.push({ Name: "@Work_Code", Value: _value2.Work_Code });

                sqlcmd = "Update SC_Offer_Detail Set Upd_Date=GETDATE(),Del_Flg=1 "
                + " where Offer_No=@Offer_No And Object_No=@Object_No And Object_Code=@Object_Code And Ware=@Ware And Work_Code=@Work_Code";

                console.log("DeleteRow Inline");
                $.fn.runAjax(
                   DB_ScOffer_SupMonthPriceMax
                   , "sqlcmd"
                   , sqlcmd
                   , ht1);
            }
        }
    });

}
//#endregion 修改

//#region 刪除
function _3PL_SCOfferDetail_Deleted(Row) {
    var sqlcmd = '', ht1 = [];

    sqlcmd = "Update SC_Offer_Detail Set Del_Flg=1 "
            + " where Offer_No=@Offer_No And Object_No=@Object_No And Object_Code=@Object_Code And Ware=@Ware ";

    ht1.push({ Name: "@Offer_No", Value: EditData.Offer_No });
    ht1.push({ Name: "@Object_No", Value: EditData.Object_No });
    ht1.push({ Name: "@Object_Code", Value: Row.Object_Code });
    ht1.push({ Name: "@Ware", Value: Row.Ware });

    console.log("DeleteRow");
    $.fn.runAjax(
       DB_ScOffer_SupMonthPriceMax
       , "sqlcmd"
       , sqlcmd
       , ht1);
}
//#endregion 刪除

//#endregion 單據明細

//#endregion

//#region Delete
function div_EditDialog_btn_Dele() {
    EditData = GridData[EditIndex - 1];

    ConfirmBoxShow("確定刪除？" + EditData.Offer_No, "", function () {

        var sqlcmd = '';
        sqlcmd = 'Delete FROM SC_Offer_Detail Where Offer_No=@No;Delete FROM SC_Offer_Header Where Offer_No=@No';

        //更新至DB
        $.fn.runAjax(
            DB_ScOffer_SupMonthPriceMax
            , "sqlcmd"
            , sqlcmd
            , [
                { Name: '@No', Value: EditData.Offer_No }
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

//#region Head Cancel
function div_EditDialog_btn_Canc() {
    ToastShow("取消");

    FormStatus = 5;
    reset();

    div_EditDialog_LoadData();


}
//#endregion

//#endregion head

//#region Form Control
var FormStatus = 0; //0-search, 1-New, 2-Edit, 3-Save, 4-Del, 5-Cancel
function reset() {

    lastSelection = undefined;
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
            EditIndex = -1;
            $('#div_Query').hide();
            $('#div_EditDialog').show();
            $('#div_EditDialog input').prop('disabled', false);
            $('#div_EditDialog textarea').prop('disabled', false);
            $('#div_EditDialog_Offer_No').prop('disabled', true); //單號

            $('#div_EditDialog_btn_Edit').hide();   //修改
            $('#div_EditDialog_btn_Save').show();   //存檔
            $('#div_EditDialog_btn_Dele').hide();   //作廢
            $('#div_EditDialog_btn_Canc').show();   //取消

            $('#div_EditDialog input').val('');
            $('#div_EditDialog textarea').val('');
            $('#div_EditDialog_Object_Name').text('');

            $('#div_EditDialog_QuotationDate').val(_3PLgetDate(0));

            setGrid_reset(null);
            setGridDetail_reset(null);
            break;
        case 2:
            $('#div_Query').hide();
            $('#div_EditDialog').show();
            $('#div_EditDialog input').prop('disabled', false);
            $('#div_EditDialog textarea').prop('disabled', false);
            $('#div_EditDialog_Offer_No').prop('disabled', true); //單號
            $('#div_EditDialog_Object_No').prop('disabled', true); //供應商

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
    var Object_No = $('#div_EditDialog_Object_No').val();
    var Offer_No = $('#div_EditDialog_Offer_No').val();
    var Contract_Ares = '123';
    var URL = "http://192.168.110.70/Smart-Query//squery.aspx?GUID=&SQ_AutoLogout=true&filename=WMSS008&path=WMSA&Object_No=" + Object_No + "&offer_No_Ext=" + Offer_No + "&Contract_Ares=" + Contract_Ares;
    window.open(URL, '_blank');
}