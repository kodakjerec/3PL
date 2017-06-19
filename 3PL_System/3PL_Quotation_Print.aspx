<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="3PL_Quotation_Print.aspx.cs" Inherits="_3PL_System._3PL_Quotation_Print" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <style type="text/css">
        table, th, td {
            border: 1px solid black;
            border-collapse: collapse;
        }

        .tdbg {
            font-weight: bold;
            background-color: rgb(235,241,222);
            text-align: center;
        }

        #shadowDIV {
            border: 1px solid black;
            width: 20cm;
            height: 27cm;
        }

            #shadowDIV #printDIV {
                margin-left: 1cm;
            }

        @media print {
            body, body * {
                visibility: hidden;
                box-shadow: none;
            }

            #printDIV, #printDIV * {
                visibility: visible;
            }
                #printDIV input[type="text"] {
                    border:none;
                    background-color:transparent
                }
            #shadowDIV #printDIV {
                position: absolute;
                margin-left: 0cm;
                left: 0;
                top: 0;
            }
        }
    </style>
    <title>報價單列印Web</title>
</head>
<body>
    <form id="form1" runat="server">
        <div id="searchDIV">
            <table>
                <tr>
                    <td class="EditTD1">報價單號：
                    </td>
                    <td>
                        <asp:TextBox ID="txb_Quotation_PLNO" runat="server">SC201704180004</asp:TextBox>
                    </td>
                    <td>
                        <asp:Button ID="Btn_Query" runat="server" Text="查詢" OnClick="Btn_Query_Click" />

                    </td>
                </tr>
            </table>
            <div runat="server" id="PrintControl" visible="false">
                <input type="button" id="btnPrint" value="列印" onclick="window.print();" />
            </div>
        </div>
        <div id="shadowDIV" runat="server" visible="false">
            <div id="printDIV">
                <table style="width: 18cm; height: 1.5cm; border: none">
                    <tr>
                        <td style="width: 5cm; border: none">
                            <img alt="" src="Mark.JPG" style="width: 63px; height: 48px; margin-left: 10px" /></td>
                        <td style="width: 6cm; border: none; text-align: center"><span style="font-weight: bold; font-size: 30px">綜合服務報價單</span></td>
                        <td style="width: 5cm; border: none; text-align: right; vertical-align: bottom">
                            <asp:Label runat="server" ID="lbl_報價單號" /></td>
                    </tr>
                </table>
                <table style="width: 18cm; height: 5cm">
                    <tr>
                        <th rowspan="8" style="width: 2cm; line-height: 25px" class="tdbg">供<br />
                            應<br />
                            商<br />
                            資<br />
                            訊</th>
                        <th class="tdbg" style="width: 3cm">供應商名稱</th>
                        <th class="tdbg" style="width: 6cm">
                            <asp:Label runat="server" ID="lbl_供應商簡稱" /></th>
                        <th class="tdbg" style="width: 5cm">備註</th>
                    </tr>
                    <tr>
                        <td class="tdbg">廠編</td>
                        <td>
                            <asp:Label runat="server" ID="lbl_供應商代號" /></td>
                        <td rowspan="7">
                            <asp:Label runat="server" ID="lbl_單頭備註" /></td>
                    </tr>
                    <tr>
                        <td class="tdbg">寄庫倉別</td>
                        <td>
                            <asp:Label runat="server" ID="lbl_倉別" /></td>
                    </tr>
                    <tr>
                        <td class="tdbg">統一編號</td>
                        <td>
                            <asp:Label runat="server" ID="lbl_統一編號" /></td>
                    </tr>
                    <tr>
                        <td class="tdbg">公司地址</td>
                        <td>
                            <asp:Label runat="server" ID="lbl_地址" /></td>
                    </tr>
                    <tr>
                        <td class="tdbg">連絡電話</td>
                        <td>
                            <asp:Label runat="server" ID="lbl_電話" /></td>
                    </tr>
                    <tr>
                        <td class="tdbg">聯絡窗口</td>
                        <td>
                            <asp:Label runat="server" ID="lbl_聯絡人" /></td>
                    </tr>
                    <tr>
                        <td class="tdbg">傳真</td>
                        <td>
                            <asp:Label runat="server" ID="lbl_傳真" /></td>
                    </tr>
                    <div id="tabs" runat="server">
                    </div>
                    <tr>
                        <td rowspan="2" colspan="2" class="tdbg" style="width: 5cm; height: 2cm">簽核確認</td>
                        <td class="tdbg" style="width: 6cm; height: 25px">客戶簽章(甲方)</td>
                        <td class="tdbg" style="width: 6cm; height: 25px">弘達簽章(乙方)</td>
                    </tr>
                    <tr>
                        <td style="height: 152px"></td>
                        <td style="height: 152px"></td>
                    </tr>
                    <tr>
                        <td colspan="2" class="tdbg">附註說明</td>
                        <td colspan="2">1.棧板規格120cm(長)*100cm(寬)*160cm(高-含棧板高度)。<br />
                            2.綜合服務費用以每月一日至月底為一期計算。<br />
                            3.上述費用不含營業稅。<br />
                            4.報價日期：民國 &nbsp;<asp:Label runat="server" ID="lbl_報價日期" /><br />
                            5.報價有效區間：民國 &nbsp;<asp:Label runat="server" ID="lbl_報價區間起" />至 &nbsp;<asp:Label runat="server" ID="lbl_報價區間迄" />止，惟本報價迄日不得逾「貨品代配送契約書」之契約期限。<br />
                            <span style="color: red">6.甲方就乙方所提供之報價應予保密，不得直接、間接揭露予第三人，並保證其受雇人、代理人遵守本保密義務，如有違反，甲方應完全賠償乙方所生之損害。</span><br />
                            7.綜合費用給付方式依「貨品代配送契約書」第四條規定辦理。<br />
                        </td>
                    </tr>
                </table>

            </div>
        </div>
    </form>
</body>
</html>
