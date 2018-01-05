<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="_3PL_BaseCost_Formula.aspx.cs"
    Inherits="_3PL_System._3PL_BaseCost_Formula" %>

<head>
    <script type="text/javascript" src="/3PL/js/CommonLoader.js"></script>
    <script type="text/javascript" src="./_3PL_BaseCost_Formula.js"></script>
    <title>計價費用公式</title>
</head>
<body>
    <table class="tborder" style="width: 100%">
        <tr>
            <td class="PageTitle">計價費用公式
            </td>
        </tr>
    </table>
    <div id="div1">
        <table class="tborder">
            <tr>
                <td class="HeaderStyle " colspan="2">查詢
                </td>
            </tr>
            <tr>
                <td class="EditTD1">倉別</td>
                <td>
                    <select id="div1_select_SiteNo"></select>
                </td>
            </tr>
            <tr>
                <td class="EditTD1">報價主類別</td>
                <td>
                    <select id="div1_select_TypeID"></select>
                </td>
            </tr>
            <tr>
                <td class="EditTD1">費用公式</td>
                <td>
                    <input type="checkbox" id="div1_checkbox_InnerJoin" value="1" checked>有設定<br>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <button class="btn mybutton" onclick="search(1)">查詢</button>
                </td>
            </tr>
        </table>
    </div>
    <div id="div_New">
        <table id="jqGrid"></table>
        <div id="jqGridPager"></div>
    </div>

    <div id="div_AddonPageDefPrice">
        <table id="jqGrid_AddonPageDefPrice"></table>
        <div id="jqGridPager_AddonPageDefPrice"></div>
    </div>

    <div id="div_EditDialog">
        <table class="tborder" style="width: 100%">
            <tr>
                <th class="HeaderStyle" colspan="2">
                    <span id="div_EditDialog_input_Header">新增</span>計價費用公式
                </th>
            </tr>
            <tr>
                <th class="HeaderStyle" colspan="2">基本資料
                </th>
            </tr>
            <tr>
                <th class="EditTD1">報價主類別</th>
                <th>
                    <label id="div_EditDialog_label_S_bsda_FieldName" style="width: 100%" />
                </th>
            </tr>
            <tr>
                <th class="EditTD1">費用名稱</th>
                <th>
                    <label id="div_EditDialog_label_S_bcse_CostName" />
                </th>
            </tr>
        </table>
        <table class="tborder" style="width: 100%">
            <tr>
                <td class="HeaderStyle">變數
                    <button class="btn-Info mybutton" onclick="setGridDetail_help()">help</button>
                    <button class="btn-default mybutton" onclick="div_EditDialog_button_AddParam()">新增</button>
                </td>
            </tr>
        </table>
        <table id="jqGrid_paramsList"></table>
        <table class="tborder" style="width: 100%">
            <tr>
                <td class="HeaderStyle">公式
                    <button class="btn-default mybutton" onclick="div_EditDialog_button_Calculate()">計算</button>
                </td>
            </tr>
            <tr>
                <td>
                    <input id="div_EditDialog_input_formula" style="width: 100%" />
                </td>
            </tr>
            <tr>
                <td class="HeaderStyle">試算結果
                </td>
            </tr>
            <tr>
                <td>
                    <label id="div_EditDialog_label_formula" />
                </td>
            </tr>
        </table>
        <table class="tborder" style="width: 100%">
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
