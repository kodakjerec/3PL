<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ScOffer_AvailableList.aspx.cs" Inherits="_3PL_System.ScOffer.ScOffer_AvailableList" %>

<head>
    <script type="text/javascript" src="/3PL/js/CommonLoader.js"></script>
    <script type="text/javascript" src="./ScOffer_AvailableList.js"></script>
    <title>SC報價_有效合約</title>
</head>
<body>
    <table class="tborder" style="width: 100%">
        <tr>
            <td class="PageTitle">SC報價_有效合約
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
                <td class="EditTD1">日期</td>
                <td>
                    <input id="div_searchDate" onchange="search()" />
                </td>
            </tr>
        </table>
    </div>
    <div id="div_Query">
        <table id="jqGrid"></table>
        <div id="jqGridPager"></div>
    </div>
</body>
