<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="_3PL_BaseAccounting.aspx.cs"
    Inherits="_3PL_System._3PL_BaseAccounting_v2" %>

<head>
    <script type="text/javascript" src="/3PL/js/CommonLoader.js"></script>
    <script type="text/javascript" src="./_3PL_BaseAccounting.js"></script>
    <title>會計科目維護</title>
</head>
<body>
    <table class="tborder" style="width: 100%">
        <tr>
            <td class="PageTitle">會計科目維護
            </td>
        </tr>
    </table>
    <div id="div_New">
        <table id="jqGrid"></table>
        <div id="jqGridPager"></div>
    </div>

    <div id="div_EditDialog">
        <table class="tborder" style="width: 100%">
            <tr>
                <th class="HeaderStyle" colspan="2">
                    <span id="div_EditDialog_input_Header">新增</span>會計科目
                </th>
            </tr>
            <tr>
                <th class="HeaderStyle" colspan="2">基本資料
                </th>
            </tr>
            <tr>
                <td class="EditTD1">費用分類</td>
                <td>
                    <select id="div_EditDialog_Select_S_Acci_ClassNo1" onchange="div_EditDialog_Select_S_Acci_ClassNo1_change()" />
                </td>
            </tr>
            <tr>
                <td class="EditTD1">費用類別</td>
                <td>
                    <select id="div_EditDialog_Select_S_Acci_ClassNo2" />
                </td>
            </tr>
            <tr>
                <td class="EditTD1">會計科目</td>
                <td>
                    <input type="text" id="div_EditDialog_S_Acci_Id" />
                </td>
            </tr>
            <tr>
                <td class="EditTD1">科目名稱</td>
                <td>
                    <input type="text" id="div_EditDialog_S_Acci_Name" />
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
