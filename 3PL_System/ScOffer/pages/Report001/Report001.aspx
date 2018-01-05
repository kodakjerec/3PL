<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Report001.aspx.cs"
    Inherits="_3PL_System_ScOffer.Report001" %>

<head>
    <script type="text/javascript" src="/3PL/js/CommonLoader.js"></script>
    <script type="text/javascript" src="./Report001.js"></script>
    <title>SC報表匯出Excel</title>
</head>
<body>
    <table class="tborder" style="width: 100%">
        <tr>
            <td class="PageTitle">SC報表匯出Excel
            </td>
        </tr>
    </table>
    <div id="div_group_btns">
        <button id="btn1" class="btn-default mybutton">SC應收表(日)</button>
        <button id="btn2" class="btn-default mybutton">SC應收表(月)</button>
        <button id="btn3" class="btn-default mybutton">SC應收表(月)0888</button>
        <button id="btn4" class="btn-default mybutton">SC應收表(月)>=6666</button>
        <button id="btn5" class="btn-default mybutton">SC應收表(月)折扣</button>
        <button id="btn6" class="btn-default mybutton">SC應收表(月)計費上限</button>
    </div>
    <div id="div_group_divs">
        <div id="div1">
            <table class="tborder">
                <tr>
                    <td class="HeaderStyle " colspan="4">SC應收表(日) 查詢
                    </td>
                </tr>
                <tr>
                    <td class="EditTD1">供應商代號</td>
                    <td>
                        <input id="div1_input_supNo" />
                    </td>
                </tr>
                <tr>
                    <td class="EditTD1">合約日期起</td>
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
                        <button class="btn mybutton" onclick="div1_button_search()">匯出 SC應收表(日)</button>
                    </td>
                </tr>
            </table>
        </div>
        <div id="div2">
            <table class="tborder">
                <tr>
                    <td class="HeaderStyle" colspan="2">SC應收表(月) 查詢
                    </td>
                </tr>
                <tr>
                    <td class="EditTD1">供應商代號</td>
                    <td>
                        <input id="div2_input_supNo" value="0888" />
                    </td>
                </tr>
                <tr>
                    <td class="EditTD1">合約日期</td>
                    <td>
                        <input id="div2_input_BMonth" />
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <button class="btn mybutton" onclick="div2_button_search()">匯出 SC應收表(月)</button>
                    </td>
                </tr>
            </table>
        </div>
        <div id="div3">
            <table class="tborder">
                <tr>
                    <td class="HeaderStyle" colspan="2">SC應收表(月)0888 查詢
                    </td>
                </tr>
                <tr>
                    <td class="EditTD1">供應商代號</td>
                    <td>
                        <input id="div3_input_supNo" value="0888" />
                    </td>
                </tr>
                <tr>
                    <td class="EditTD1">合約日期</td>
                    <td>
                        <input id="div3_input_BMonth" />
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <button class="btn mybutton" onclick="div3_button_search()">匯出 SC應收表(月)0888</button>
                    </td>
                </tr>
            </table>
        </div>
        <div id="div4">
            <table class="tborder">
                <tr>
                    <td class="HeaderStyle" colspan="2">SC應收表(月)>=6666 查詢
                    </td>
                </tr>
                <tr>
                    <td class="EditTD1">供應商代號</td>
                    <td>
                        <input id="div4_input_supNo" value="" />
                    </td>
                </tr>
                <tr>
                    <td class="EditTD1">合約日期</td>
                    <td>
                        <input id="div4_input_BMonth" />
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <button class="btn mybutton" onclick="div4_button_search()">匯出 SC應收表(月)>=6666</button>
                    </td>
                </tr>
            </table>
        </div>
        <div id="div5">
            <table class="tborder">
                <tr>
                    <td class="HeaderStyle" colspan="2">SC應收表(月)折扣 查詢
                    </td>
                </tr>
                <tr>
                    <td class="EditTD1">合約日期</td>
                    <td>
                        <input id="div5_input_BMonth" />
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <button class="btn mybutton" onclick="div5_button_search()">匯出 SC應收表(月)折扣</button>
                    </td>
                </tr>
            </table>
        </div>
        <div id="div6">
            <table class="tborder">
                <tr>
                    <td class="HeaderStyle" colspan="2">SC應收表(月)計費上限 查詢
                    </td>
                </tr>
                <tr>
                    <td class="EditTD1">供應商代號</td>
                    <td>
                        <input id="div6_input_supNo" />
                    </td>
                </tr>
                <tr>
                    <td class="EditTD1">合約日期</td>
                    <td>
                        <input id="div6_input_BMonth" />
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <button class="btn mybutton" onclick="div6_button_search()">匯出 SC應收表(月)計費上限</button>
                    </td>
                </tr>
            </table>
        </div>
    </div>
</body>
