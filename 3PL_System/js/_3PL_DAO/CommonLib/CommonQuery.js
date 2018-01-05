//取得基本資料
function getBaseData(TypeID, FirstObj) {
    var dfd = $.Deferred();

    $.fn.runAjax(
     "3PL"
     , "sqlcmd"
     , "Select Value=S_bsda_FieldId, Name=S_bsda_FieldName From [3PL_baseData] with(nolock) Where S_bsda_CateId=@CateId and I_bsda_DelFlag=0"
     , [
         { Name: '@CateId', Value: TypeID }
     ], function (response) {

         if (FirstObj != undefined)
             response.unshift({ Value: FirstObj.Value, Name: FirstObj.Name });

         dfd.resolve(response);
     });

    return dfd.promise();
}

//綁定select
function getlist(response, obj_List) {
    var dfd = $.Deferred();

    $.each(response, function (_key, _value) {
        var value = _value.Value;
        var name = _value.Value + ',' + _value.Name;

        //DOM
        obj_List.append($("<option></option>").attr('value', value).text(name));
    })
    dfd.resolve();
    return dfd.promise();
}

//取得日期
//Mode: 0 today
//      1 last Month today
//      2 last Month FirstDay
//      3 last Month lastDay
function _3PLgetDate(Mode) {
    var nowdays = new Date();
    var year = nowdays.getFullYear();
    var month = nowdays.getMonth();
    var dday = nowdays.getDate();
    //取lastMonth
    if (Mode > 0) {
        //要減1個月
        if (month == 0) {
            month = 12;
            year = year - 1;
        }
    }
    else {
        month = month + 1;
    }
    if (month < 10) {
        month = '0' + month;
    }
    if (dday < 10) {
        dday = '0' + dday;
    }

    var nowDay = year + "-" + month + "-" + dday;
    var lastDay = year + "-" + month + "-" + dday;
    var lastMonthfirstDay = year + "-" + month + "-" + "01";//上个月的第一天

    var nowdays = new Date();
    nowdays.setDate(1);
    nowdays.setHours(-1);
    lastdday = nowdays.getDate();
    if (lastdday < 10) {
        lastdday = '0' + lastdday;
    }
    var lastMonthlastDay = year + "-" + month + "-" + lastdday;//上个月的最后一天

    switch (Mode) {
        case 0:
            return nowDay; break;
        case 1:
            return lastDay; break;
        case 2:
            return lastMonthfirstDay; break;
        case 3:
            return lastMonthlastDay; break;
    }
}