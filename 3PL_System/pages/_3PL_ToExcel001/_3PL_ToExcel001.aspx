<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="_3PL_ToExcel001.aspx.cs"
    Inherits="_3PL_System._3PL_ToExcel001" %>

<head>
    <script type="text/javascript" src="/3PL/js/CommonLoader.js"></script>
    <script type="text/javascript" src="./_3PL_ToExcel001.js"></script>
    <title>單據匯出Excel_001</title>
</head>
<body>
    <table class="tborder" style="width: 100%">
        <tr>
            <td class="PageTitle">單據匯出Excel_001
            </td>
        </tr>
    </table>
    <div id="div_group_btns">
        <button id="btn1" class="btn-default mybutton">報價單</button>
        <button id="btn2" class="btn-default mybutton">派工單</button>
        <button id="btn3" class="btn-default mybutton">月彙總表(海博)</button>
        <button id="btn4" class="btn-default mybutton">月彙總表會計費用</button>
    </div>
    <div id="div_group_divs">
        <div id="div1">
            <table class="tborder">
                <tr>
                    <td class="HeaderStyle " colspan="4">報價單
                    </td>
                </tr>
                <tr>
                    <td class="EditTD1">供應商代號</td>
                    <td>
                        <input id="div1_input_supNo" />
                    </td>
                </tr>
                <tr>
                    <td class="EditTD1">倉別</td>
                    <td>
                        <select id="div1_select_SiteNo"></select>
                    </td>
                </tr>
                <tr>
                    <td class="EditTD1">報價日期起</td>
                    <td>
                        <input id="div1_input_Bdate" />
                    </td>
                    <td class="EditTD1">迄</td>
                    <td>
                        <input id="div1_input_Edate" />
                    </td>
                </tr>
                <tr>
                    <td colspan="4">
                        <button class="btn mybutton" onclick="div1_button_search()">匯出 報價單</button>
                    </td>
                </tr>
            </table>
        </div>
        <div id="div2">
            <table class="tborder">
                <tr>
                    <td class="HeaderStyle " colspan="4">派工單
                    </td>
                </tr>
                <tr>
                    <td class="EditTD1">供應商代號</td>
                    <td>
                        <input id="div2_input_supNo" />
                    </td>
                </tr>
                <tr>
                    <td class="EditTD1">倉別</td>
                    <td>
                        <select id="div2_select_SiteNo"></select>
                    </td>
                </tr>
                <tr>
                    <td class="EditTD1">預定完工日起</td>
                    <td>
                        <input id="div2_input_Bdate" />
                    </td>
                    <td class="EditTD1">迄</td>
                    <td>
                        <input id="div2_input_Edate" />
                    </td>
                </tr>
                <tr>
                    <td colspan="4">
                        <button class="btn mybutton" onclick="div2_button_search()">匯出 派工單</button>
                    </td>
                </tr>
            </table>
        </div>
        <div id="div3">
            <table class="tborder">
                <tr>
                    <td class="HeaderStyle " colspan="2">月彙總表(海博)
                    </td>
                </tr>
                <tr>
                    <td class="EditTD1">供應商代號</td>
                    <td>
                        <input id="div3_input_supNo" />
                    </td>
                </tr>
                 <tr>
                    <td class="EditTD1">倉別</td>
                    <td>
                        <select id="div3_select_SiteNo"></select>
                    </td>
                </tr>
                <tr>
                    <td class="EditTD1">月份</td>
                    <td>
                        <input id="div3_input_BMonth" />
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <button class="btn mybutton" onclick="div3_button_search()">匯出 月彙總表(海博)</button>
                    </td>
                </tr>
            </table>
        </div>
        <div id="div4">
            <table class="tborder">
                <tr>
                    <td class="HeaderStyle" colspan="2">月彙總表會計費用
                    </td>
                </tr>
                <tr>
                    <td class="EditTD1">月份</td>
                    <td>
                        <input id="div4_input_BMonth" />
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <button class="btn mybutton" onclick="div4_button_search()">匯出 月彙總表會計費用</button>
                    </td>
                </tr>
            </table>
        </div>
    </div>
</body>
