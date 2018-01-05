<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="_3PL_QuotationHead_Addon_00.aspx.cs"
    Inherits="_3PL_System._3PL_QuotationHead_Addon_00" %>

<head>
    <script type="text/javascript" src="/3PL/js/CommonLoader.js"></script>
    <script type="text/javascript" src="./_3PL_QuotationHead_Addon_00.js"></script>
    <title>租借棧板借出收費</title>
</head>
<body>
    <table class="tborder" style="width: 100%">
        <tr>
            <td class="PageTitle">租借棧板借出收費
            </td>
        </tr>
    </table>
    <div id="div1">
        <table class="tborder">
            <tr>
                <td class="HeaderStyle" colspan="4">查詢
                </td>
            </tr>
            <tr>
                <td class="EditTD1">報價日期</td>
                <td>
                    <input type="text" id="div1_select_Bdate" />
                </td>
                <td class="EditTD1">迄</td>
                <td>
                    <input type="text" id="div1_select_Edate" />
                </td>
            </tr>
            <tr>
                <td class="EditTD1">供應商</td>
                <td>
                    <input type="text" id="div1_select_SupdID" />
                </td>
            </tr>
            <tr>
                <td colspan="4">
                    <button class="btn mybutton" onclick="searchHead()">查詢</button>
                    <button class="btn mybutton" onclick="NewQHEAD()">新增</button>
                </td>
            </tr>
        </table>
    </div>
    <div id="div_Query">
        <table id="jqGrid"></table>
        <div id="jqGridPager"></div>
    </div>
    <div id="div_EditDialog">
        <span style="color: blue">租借棧板內容</span>
        <table class="tborder">
            <tr>
                <td class="EditTD1">單號</td>
                <td>
                    <input type="text" id="div_EditDialog_PaperNo" />
                </td>
            </tr>
            <tr>
                <td class="EditTD1">供應商</td>
                <td>
                    <input type="text" id="div_EditDialog_SupdID" />
                </td>
            </tr>
            <tr>
                <td class="EditTD1">窗口</td>
                <td>
                    <input type="text" id="div_EditDialog_BOSS" />
                </td>
                <td class="EditTD1">TEL</td>
                <td>
                    <input type="text" id="div_EditDialog_TEL" />
                </td>
                <td class="EditTD1">FAX</td>
                <td>
                    <input type="text" id="div_EditDialog_FAX" />
                </td>
            </tr>
            <tr>
                <td class="EditTD1">報價日</td>
                <td>
                    <input type="text" id="div_EditDialog_QuotationDate" />
                </td>
                <td class="EditTD1">付款日</td>
                <td>
                    <input type="text" id="div_EditDialog_PayDate" />
                </td>
            </tr>
            <tr>
                <td class="EditTD1">備註</td>
                <td colspan="5">
                    <textarea rows="4" id="div_EditDialog_Memo" style="width: 100%"></textarea>
                </td>
            </tr>
        </table>
        <div id="div_EditDialog_buttons">
            <button class="btn mybutton" id="div_EditDialog_btn_Print" onclick="div_EditDialog_btn_Print()">列印</button>
            <button class="btn mybutton" id="div_EditDialog_btn_Edit" onclick="div_EditDialog_btn_Edit()">修改</button>
            <button class="btn mybutton" id="div_EditDialog_btn_Save" onclick="div_EditDialog_btn_Save()">存檔</button>
            <button class="btn mybutton btn-danger" id="div_EditDialog_btn_Dele" onclick="div_EditDialog_btn_Dele()">作廢</button>
            <button class="btn mybutton" id="div_EditDialog_btn_Canc" onclick="div_EditDialog_btn_Canc()">取消</button>
        </div>
        <table id="jqGrid_Detail"></table>
        <div id="jqGrid_Detail_Pager"></div>
    </div>
</body>
