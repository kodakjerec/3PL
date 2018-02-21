<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ScOfferPage.aspx.cs" Inherits="_3PL_System.ScOffer.ScOfferPage" %>

<head>
    <script type="text/javascript" src="/3PL/js/CommonLoader.js"></script>
    <script type="text/javascript" src="./ScOfferPage.js"></script>
    <title>SC報價_單據</title>
</head>
<body>
    <table class="tborder" style="width: 100%">
        <tr>
            <td class="PageTitle">SC報價_單據
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
                    <input id="div_search_SupID" placeholder="請輸入代號4碼,ex: 0888 " value="0259" />
                </td>
            </tr>
            <tr>
                <td class="EditTD1">單號</td>
                <td>
                    <input id="div_search_OfferNo" placeholder="請輸入單號" />
                </td>
            </tr>
            <tr>
                <td class="EditTD1">貨號</td>
                <td>
                    <input id="div_search_ItemNo" placeholder="請輸入貨號8碼" />
                </td>
            </tr>
            <tr>
                <td>
                    <button class="btn-default mybutton" onclick="search(1)">查詢</button>
                    <button class="btn-default mybutton" onclick="NewQHEAD()">新增</button>
                </td>
            </tr>
        </table>
    </div>
    <div id="div_Query">
        <table id="jqGrid"></table>
        <div id="jqGridPager"></div>
    </div>
    <div id="div_EditDialog">
        <span style="color: blue">SC報價單頭</span>
        <table class="tborder">
            <tr>
                <td class="EditTD1">單號</td>
                <td>
                    <input type="text" id="div_EditDialog_Offer_No" />
                </td>
                <td class="EditTD1">供應商</td>
                <td>
                    <input type="text" id="div_EditDialog_Object_No" onchange="find_div_EditDialog_Object_Name(this)" />
                    <label id="div_EditDialog_Object_Name" />
                </td>
            </tr>
            <tr>
                <td class="EditTD1">一次性進貨</td>
                <td>
                    <div data-mydom="myCheckbox" id="div_EditDialog_OT_Flg" class="btn btn-default">
                        <input type="checkbox" id="div_EditDialog_OT_Flg_input" />
                        <label for="div_EditDialog_OT_Flg">啟用</label>
                    </div>
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
            <button class="btn mybutton btn-danger" id="div_EditDialog_btn_Dele" onclick="div_EditDialog_btn_Dele()">
                永久<br />
                刪除</button>
            <button class="btn mybutton" id="div_EditDialog_btn_Canc" onclick="div_EditDialog_btn_Canc()">取消</button>
        </div>
        <table id="jqGrid_Detail"></table>
        <div id="jqGrid_Detail_Pager"></div>
    </div>

    <div id="div_EditDialog_LittleWinform">
        <table class="tborder" style="width: 100%">
            <tr>
                <td class="EditTD1"><span style="color: red">*</span>廠編</td>
                <td colspan="3">
                    <label id="div_EditDialog_LittleWinform_Sup_No" />
                </td>
            </tr>
            <tr>
                <td class="EditTD1"><span style="color: red">*</span>貨號</td>
                <td>
                    <textarea id="div_EditDialog_LittleWinform_Object_Code" rows="10" placeholder="Excel整欄複製貼上, 按下'驗證'"></textarea>
                </td>
                <td class="EditTD1">
                    <button class="btn mybutton" id="div_EditDialog_LittleWinform_btn_Object_Code_check" onclick="div_EditDialog_LittleWinform_Object_Code_check()">驗證</button>
                    <p />
                    <button class="btn mybutton" id="div_EditDialog_LittleWinform_btn_Object_Code_clear" onclick="div_EditDialog_LittleWinform_Object_Code_clear()">清空</button>
                </td>
                <td>
                    <textarea id="div_EditDialog_LittleWinform_Object_Code_valid" rows="10" disabled placeholder="驗證後合法的貨號清單"></textarea>
                </td>
            </tr>
            <tr>
                <td class="EditTD1">倉別</td>
                <td colspan="3">
                    <div data-mydom="myCheckbox" id="div_EditDialogg_LittleWinform_Contract_Ares_1" class="btn btn-default">
                        <input type="checkbox" id="div_EditDialogg_LittleWinform_Contract_Ares_1_input" />
                        <label for="div_EditDialogg_LittleWinform_Contract_Ares_1_input">觀音</label>
                    </div>
                    <div data-mydom="myCheckbox" id="div_EditDialogg_LittleWinform_Contract_Ares_2" class="btn btn-default">
                        <input type="checkbox" id="div_EditDialogg_LittleWinform_Contract_Ares_2_input" />
                        <label for="div_EditDialogg_LittleWinform_Contract_Ares_2_input">岡山</label>
                    </div>
                    <div data-mydom="myCheckbox" id="div_EditDialogg_LittleWinform_Contract_Ares_3" class="btn btn-default">
                        <input type="checkbox" id="div_EditDialogg_LittleWinform_Contract_Ares_3_input" />
                        <label for="div_EditDialogg_LittleWinform_Contract_Ares_3_input">梧棲</label>
                    </div>
                </td>
            </tr>
            <tr>
                <td class="EditTD1"><span style="color: red">*</span>計價日期 起</td>
                <td>
                    <input id="div1_input_Bdate" />
                </td>
                <td class="EditTD1">迄</td>
                <td>
                    <input id="div1_input_Edate" />
                </td>
            </tr>
        </table>
        <table id="jqGrid_Fee"></table>
        <table class="tborder" style="width: 100%">
            <tr>
                <td>
                    <button class="btn-default mybutton" onclick="div_EditDialog_LittleWinform_button_save()">儲存</button>
                </td>
                <td>
                    <button class="btn-default mybutton" onclick="div_EditDialog_LittleWinform_button_close()">取消</button>
                </td>
            </tr>
        </table>
    </div>

</body>
