<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EDI_Retn_Price.aspx.cs" Inherits="_3PL_System.Other.EDI_Retn_Price.EDI_Retn_Price" %>

<head>
    <script type="text/javascript" src="/3PL/js/CommonLoader.js"></script>
    <script type="text/javascript" src="/3PL/Other/EDI_Retn_Price/EDI_Retn_Price.js"></script>
    <link rel='stylesheet' href="/3PL/Other/EDI_Retn_Price/EDI_Retn_Price.css" type='text/css' />
    <title>逆物流費用設定</title>
</head>
<body>
     <table class="tborder" style="width: 100%">
        <tr>
            <td class="PageTitle">逆物流費用設定
            </td>
        </tr>
    </table>
    <div id="div_search">
        <table class="tborder">
            <tr>
                <td class="HeaderStyle " colspan="4">查詢
                </td>
            </tr>
            <tr>
                <td class="EditTD1">計費年月：</td>
                <td>
                    <input id="div_search_Bill_date" placeholder="yyyyMM" />
                </td>
            </tr>
            <tr>
                <td class="EditTD1">供應商編號：</td>
                <td>
                    <input id="div_search_vendor_no" placeholder="" />
                </td>
            </tr>
            <tr>
                <td class="EditTD1">寄倉倉別：</td>
                <td>
                    <select id="div_search_site_no"></select>
                </td>
            </tr>
            <tr>
                <td class="EditTD1">計費類別：</td>
                <td>
                    <select id="div_search_kind"></select>
                </td>
            </tr>
            <tr>
                    <td class="EditTD1">退廠日期：</td>
                    <td>
                        <input id="div_search_back_date_S" />
                    </td>
                    <td class="EditTD1">迄</td>
                    <td>
                        <input id="div_search_back_date_E" />
                    </td>
                </tr>
            <tr>
                    <td class="EditTD1">退廠單號：</td>
                    <td>
                        <input id="div_search_back_id" />
                    </td>
                    <td class="EditTD1">退廠箱號：</td>
                    <td>
                        <input id="div_search_boxid" />
                    </td>
                </tr>
            <tr>
                <td>
                    <button class="btn-default mybutton" onclick="search(1)">查詢</button>
                </td>
            </tr>
        </table>
    </div>
</body>
