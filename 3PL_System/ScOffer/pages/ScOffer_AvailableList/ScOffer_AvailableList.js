var DB_ScOffer_SupMonthPriceMax = 'EDI';

$(
    function init() {
        ////#region Init
        $("#div_searchDate").datepicker({ dateFormat: 'yy-mm-dd' });
        $('#div_searchDate').val(_3PLgetDate(0));

        FormStatus = 0;

        setGrid();
        search();
    });



//#region jqGrid
var PriceTypeList = ''; //計價類別清單
var GridData = '';
function setGrid() {
    $("#jqGrid").jqGrid({
        datatype: 'local',
        data: GridData,
        //建立欄位名稱及屬性
        colNames: ['倉別', '供應商', '貨號', '計價日期起', '迄', '截止日', '單據', '重複單據'],
        colModel:
            [
                { name: 'Ware_Name', index: 'Ware_Name', sorttype: "text" },
                { name: 'Sup_Name', index: 'Sup_Name', sorttype: "text" },
                { name: 'Object_Code', index: 'Object_Code' },
                { name: 'AmountDateS', index: 'AmountDateS', formatter: "date", formatoptions: { newformat: "Y-m-d" } },
                { name: 'AmountDateE', index: 'AmountDateE', formatter: "date", formatoptions: { newformat: "Y-m-d" } },
                { name: 'stop_date', index: 'stop_date', formatter: "date", formatoptions: { newformat: "Y-m-d" } },
                { name: 'Offer_No_Ext', index: 'Offer_No_Ext' },
                { name: 'Offer_No_List', index: 'Offer_No_List' }
            ],
        //建立grid屬性，editurl 是指資料新增、編輯及刪除時的伺服器端程式 
        pager: '#jqGridPager',
        autowidth: true,    //自动匹配宽度  
        height: 'auto',
        rownumbers: true,   //添加左侧行号  
        rownumWidth: 30,
        altRows: true,      //单双行样式不同 
        loadonce: false,
        rowNum: 30,         //每页显示记录数  
        //rowList: [20, 30, 40],  //用于改变显示行数的下拉列表框的元素数组  
        viewrecords: true,  //显示总记录数  
    });
    $("#jqGrid")
    .navGrid('#jqGridPager', { edit: false, add: false, del: false, search: false })
    .jqGrid('filterToolbar', { stringResult: true, searchOnEnter: false, defaultSearch: "cn" });
    ;
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

function search() {
    var sqlcmd = "select a.* ,Ware_Name=a.Ware+','+b.Field_Name,Sup_Name=a.Object_no+','+c.ALIAS "
    + " From [vSC_Offer_AvailableList](@date) a "
    + " INNER JOIN SC_Offer_Field_Name b with(nolock) "
    + " ON a.Ware=b.Field_Code and b.Cate_Code='Contract_Ares' "
    + " INNER JOIN DRP.dbo.DRP_SUPPLIER c with(nolock) "
    + " on a.Object_no=c.ID "

    $.fn.runAjax(
        DB_ScOffer_SupMonthPriceMax
        , "sqlcmd"
        , sqlcmd
        , [
            { Name: '@date', Value: $('#div_searchDate').val() }
            //, { Name: '@OfferNo', Value: OfferNo }
            //, { Name: '@ItemNo', Value: ItemNo }
        ], function (response) {

            $('#jqGrid').jqGrid("clearGridData", true);
            $('#jqGrid').trigger('reloadGrid');

            setGrid_reset(response);

            $('#jqGrid').jqGrid('setGridParam', { data: GridData });
            $('#jqGrid').trigger('reloadGrid');

        });
}