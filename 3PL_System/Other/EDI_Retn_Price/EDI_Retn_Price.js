$(
    function init() {

        //預設日期
        var today = new Date();
        var Bdate = today.getFullYear() + '/' + today.getMonth() + '/' + today.getDate();
        var Edate = today.getFullYear() + '/' + (today.getMonth() + 1) + '/' + today.getDate();
        var BMonth = today.getFullYear() + '/' + today.getMonth();
        $('#div_search_back_date_S').val(Bdate);

        //日期格式
        $("#div_search_back_date_S").datepicker({ dateFormat: 'yy/mm/dd' });
        $("#div_search_back_date_E").datepicker({ dateFormat: 'yy/mm/dd' });

        getDCList();
        getkindList();
    }
);

//#region Default-search

//取得倉別清單
function getDCList() {
    var sqlcmd_string = "Select Value=S_bsda_FieldId, Name=S_bsda_FieldName From [3PL_baseData] with(nolock) Where S_bsda_CateId='SiteNo' and I_bsda_DelFlag=0 order by S_bsda_FieldId";
    runAjax(
        "3PL"
        , "sqlcmd"
        , sqlcmd_string
        , [
        ], function (response) {
            $('#div_search_site_no').append($('<option>', {
                value: '',
                text: 'ALL'
            }));
            $.each(response, function (i, item) {
                $('#div_search_site_no').append($('<option>', {
                    value: item.Value,
                    text: item.Name
                }));
            });
        });
}

//取得計費類別
function getkindList() {
    var sqlcmd_string = "select Value=I_bcse_Seq, Name=S_bcse_CostName, Kind=I_bcse_TypeId from [3PL_BaseCostSet] with(nolock) where S_bcse_SiteNo='DC01' and I_bcse_TypeId in (13,14,15,16)";
    runAjax(
        "3PL"
        , "sqlcmd"
        , sqlcmd_string
        , [
        ], function (response) {
            $('#div_search_kind').append($('<option>', {
                value: '',
                text: 'ALL'
            }));
            $.each(response, function (i, item) {
                $('#div_search_kind').append($('<option>', {
                    value: item.Value,
                    text: item.Name
                }));
            });
        });
}

//#endregion

//#region search


//#endregion