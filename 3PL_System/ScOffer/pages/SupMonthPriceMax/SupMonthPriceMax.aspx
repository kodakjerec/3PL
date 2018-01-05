<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SupMonthPriceMax.aspx.cs" Inherits="_3PL_System.ScOffer.SupMonthPriceMax" %>

<head>
    <script type="text/javascript" src="/3PL/js/CommonLoader.js"></script>
    <script type="text/javascript" src="./SupMonthPriceMax.js"></script>
    <title>SC報價_供應商月份計費上限</title>
</head>
<body>
    <table class="tborder" style="width: 100%">
        <tr>
            <td class="PageTitle">SC報價_供應商月份計費上限
            </td>
        </tr>
    </table>
    <div id="div_search">
        <table class="tborder">
            <tr>
                <td class="HeaderStyle " colspan="2">查詢
                </td>
            </tr>
            <tr>
                <td class="EditTD1">供應商</td>
                <td>
                    <input id="div_search_SupID" placeholder="全選請輸入'ALL', 或編號" />
                </td>
            </tr>
            <tr>
                <td class="EditTD1">月份</td>
                <td>
                    <input id="div_search_Month" placeholder="全選請輸入'', 或yyyyMM" />
                </td>
            </tr>
            <tr>
                <td class="EditTD1">計費類別</td>
                <td>
                    <select id="div_search_I_acci_seq"></select>
                </td>
            </tr>
            <tr>
                <td>
                    <button class="btn-default mybutton" onclick="search(1)">查詢</button>
                </td>
            </tr>
        </table>
    </div>
    <div id="div_New">
        <table id="jqGrid"></table>
        <div id="jqGridPager"></div>
    </div>
    <div id="div_EditDialog">
        <table class="tborder" style="width: 100%">
            <tr>
                <td class="HeaderStyle" colspan="4">
                    <span id="div_EditDialog_input_Header">新增</span>計費上限
                </td>
            </tr>
            <tr>
                <td class="EditTD1"><span style="color: red">*</span>供應商</td>
                <td colspan="3">
                    <input type="text" id="div_EditDialog_input_supNo" onchange="CalPriority()" placeholder="全選請輸入'ALL', 或編號" />
                </td>
            </tr>
            <tr>
                <td class="EditTD1"><span style="color: red">*</span>月份</td>
                <td>
                    <input type="text" id="div_EditDialog_input_Month" onchange="CalPriority()" placeholder="全選請輸入'', 或yyyyMM" />
                </td>
            </tr>
            <tr>
                <td class="EditTD1"><span style="color: red">*</span>計費類別</td>
                <td>
                    <select id="div_EditDialog_input_I_acci_seq"></select>
                </td>
            </tr>
            <tr>
                <td class="EditTD1"><span style="color: red">*</span>金額上限</td>
                <td>
                    <input type="number" id="div_EditDialog_input_PriceMax" />
                </td>
            </tr>
            <tr>
                <td class="EditTD1">優先權</td>
                <td>
                    <label id="lbl_input_Priority"></label>
                </td>
            </tr>
            <tr>
                <td>
                    <button class="btn-default mybutton" onclick="div_EditDialog_button_save()">儲存</button>
                </td>
                <td>
                    <button class="btn-default mybutton" onclick="div_EditDialog_button_close()">取消</button>
                </td>
            </tr>
        </table>
    </div>
</body>
