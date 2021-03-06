﻿<%@ Page Language="C#" MasterPageFile="~/3PLMasterPage.Master" AutoEventWireup="true" CodeBehind="3PL_SignOffLog.aspx.cs"
    Inherits="_3PL_System._PL_SignOffLog" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .circle_in {
            width: 20px;
            height: 20px;
            border-radius: 99em;
            background-color: black;
        }
        .Arrow_body {
            margin-left:7px;
            width: 6px;
            height: 40px;
            background-color:black;
        }
        .Log_Left {
            width:180px;
        }
        .Log_Middle_border {
             width:0px;
            height:0px;
            border-width:10px;
            border-style:solid;
            border-color:transparent black transparent transparent;
            position:absolute;
            left:190px;
        }
        .Log_Middle {
            width:0px;
            height:0px;
            border-width:10px;
            border-style:solid;
            border-color:transparent peachpuff transparent transparent;
            position:absolute;
            left:191px;
        }
        .Log_body {
            left:10px;
            background-color:peachpuff;
            border-width:1px 1px 1px 1px;
            border-style:solid;
            border-color:black;
        }
    </style>
    <title>簽核狀態查詢</title>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table class="tborder" style="width:100%">
        <tr>
            <td class="PageTitle">
                <asp:Label ID="lbl_TypeFee" runat="server" Text="簽核狀態查詢"></asp:Label>
            </td>
        </tr>
        <tr>
    </table>
    <table class="tborder">
        <tr>
            <td class="EditTD1">
                <asp:Label ID="Lbl_PLNO" runat="server" Text="單號："></asp:Label>
            </td>
            <td>
                <asp:TextBox ID="Txb_PLNO" runat="server" Enabled="false"></asp:TextBox>
                <asp:Label ID="Lbl_Type_PLNO" runat="server" Text="報價單"></asp:Label>
                <asp:Label ID="Lbl_Status" runat="server" Text="單據代號" Visible="false"></asp:Label>
            </td>
        </tr>
    </table>

    <asp:Label ID="Label1" runat="server" Text="簽核過程"></asp:Label>
    <asp:GridView ID="GV_Log" runat="server" AllowPaging="True" AutoGenerateColumns="False"
        CssClass="GVStyle"
        AllowSorting="True" Width="100%" PageSize="20">
        <HeaderStyle CssClass="GVHead" />
        <RowStyle CssClass="one" />
        <PagerStyle CssClass="GVPage" />
        <EmptyDataRowStyle HorizontalAlign="Center" />
        <Columns>
            <asp:BoundField DataField="StatusName" HeaderText="狀態" />
            <asp:BoundField DataField="IsOk" HeaderText="簽核動作" />
            <asp:BoundField DataField="FinalStatusName" HeaderText="簽核後狀態" />
            <asp:BoundField DataField="sofp_WorkId" HeaderText="簽核人員工號" />
            <asp:BoundField DataField="sofp_WorkName" HeaderText="名稱" />
            <asp:BoundField DataField="sofp_updateDate" HeaderText="簽核時間" />
            <asp:BoundField DataField="sofp_Reason" HeaderText="退回原因" />
        </Columns>
    </asp:GridView>
    <div id="div_Log_Graphic" runat="server">
    </div>
</asp:Content>
