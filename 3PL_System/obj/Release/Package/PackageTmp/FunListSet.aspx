<%@ Page Language="C#" MasterPageFile="~/3PLMasterPage.Master" AutoEventWireup="true" CodeBehind="FunListSet.aspx.cs" Inherits="_3PL_System.FunListSet" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        function hide_tr() {
            var dr_style = document.getElementById('dyn_tr').style;
            var txb_style = document.getElementById('txb_FunNm').style;
            var ddl_style = document.getElementById('ddl_FunName').style;
            dr_style.display = 'none';
            txb_style.display = 'none';
            ddl_style.display = 'inline';
        }

        function show_tr() {
            var dr_style = document.getElementById('dyn_tr').style;
            var txb_style = document.getElementById('txb_FunNm').style;
            var ddl_style = document.getElementById('ddl_FunName').style;
            txb_style.display = 'inline';
            dr_style.display = 'table-row';
            ddl_style.display = 'none';
        }

        function txtKeyNumber() {
            if (!((window.event.keyCode >= 48) && (window.event.keyCode <= 57))) {
                window.event.keyCode = 0;
            }
        }

    </script>

    <title>資訊系統管理</title>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div>
        <table class="tborder" width="100%">
            <tr>
                <td class="PageTitle">資訊系統管理
                </td>
            </tr>
            <tr>
                <td class="HeaderStyle ">
                    <asp:Label ID="lbl_CustInf" runat="server" Text="選單設定"></asp:Label>
                </td>
            </tr>
        </table>
        <table class="tborder" width="100%">
            <tr>
                <td class="EditTD1" width="15%">
                    <asp:Label ID="lbl_Fun" runat="server" Text="主功能名稱:"></asp:Label>
                </td>
                <td width="35%">
                    <asp:DropDownList ID="ddl_Fun" runat="server">
                    </asp:DropDownList>
                </td>
                <td class="EditTD1" width="15%">
                    <asp:Label ID="lbl_Pg" runat="server" Text="子功能名稱:"></asp:Label>
                </td>
                <td width="35%">
                    <asp:TextBox ID="txb_Pg" runat="server" MaxLength="20"></asp:TextBox>
                </td>
            </tr>
        </table>
        <table>
            <tr>
                <td>
                    <asp:Button ID="btn_Query" runat="server" Text="查詢" OnClick="btn_Query_Click" />
                </td>
                <td>
                    <asp:Button ID="btn_Add" runat="server" Text="新增" OnClick="btn_Add_Click" />
                </td>
            </tr>
        </table>
        <%-- <asp:Panel ID="pnl_List" runat="server">--%>
        <table class="tborder" width="100%">
            <tr>
                <td>
                    <asp:GridView ID="gv_List" runat="server" AutoGenerateColumns="False" CssClass="GVStyle"
                        Width="100%" AllowPaging="True"
                        OnPageIndexChanging="gv_List_PageIndexChanging">
                        <HeaderStyle CssClass="GVHead" />
                        <RowStyle CssClass="one" />
                        <PagerStyle CssClass="GVPage" />
                        <EmptyDataRowStyle HorizontalAlign="Center" />
                        <Columns>
                            <asp:TemplateField>
                                <ItemStyle HorizontalAlign="Left" Width="5" />
                                <ItemTemplate>
                                    <asp:HiddenField ID="hid_FunId" runat="server" Value='<%# Bind("FunId") %>' />
                                    <asp:HiddenField ID="hid_PgId" runat="server" Value='<%# Bind("PgId") %>' />
                                    <asp:CheckBox ID="cbk_FunList" runat="server" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="主功能名稱">
                                <ItemStyle HorizontalAlign="Center" Width="140" />
                                <ItemTemplate>
                                    <asp:Label ID="lbl_Fun" runat="server" Text='<%# Bind("FunNm") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="順序">
                                <ItemStyle HorizontalAlign="Center" Width="50" />
                                <ItemTemplate>
                                    <asp:TextBox ID="txb_Ord" runat="server" Width="50" onkeypress="txtKeyNumber();"
                                        MaxLength="5" Text='<%# Bind("PgOrd") %>'></asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="子功能名稱">
                                <ItemStyle HorizontalAlign="Center" Width="150" />
                                <ItemTemplate>
                                    <asp:TextBox ID="txb_PgName" runat="server" Text='<%# Bind("PgNm") %>'></asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="連結路徑">
                                <ItemStyle HorizontalAlign="Center" />
                                <ItemTemplate>
                                    <asp:TextBox ID="txb_Url" runat="server" Width="300" Text='<%# Bind("PgUrl") %>'></asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Button ID="btn_Update" runat="server" Text="更新" OnClick="btn_Update_Click" />
                    &nbsp;<asp:Button ID="btn_Del" runat="server" Text="刪除" OnClick="btn_Del_Click" />
                </td>
            </tr>
        </table>
        <%--        </asp:Panel>--%>
        <asp:Panel ID="pnl_AddFun" runat="server">
            <table class="tborder" width="100%">
                <tr runat="server" id="dyn_tr">
                    <td width="15%" class="EditTD1">
                        <asp:Label ID="lbl_FunID" runat="server" Text="主功能代號："></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txb_FunOrd" runat="server" Width="25" MaxLength="4" onkeyup="if (value.match(/^\d+$/)==null) value='';"
                            onblur="if (value.match(/^\d+$/)==null) value='';"></asp:TextBox>
                        -
                        <asp:TextBox ID="txb_FunId" runat="server" MaxLength="100"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td width="15%" class="EditTD1">
                        <asp:Label ID="lbl_FunNm" runat="server" Text="主功能名稱："></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddl_FunName" runat="server">
                        </asp:DropDownList>
                        <asp:TextBox ID="txb_FunNm" runat="server" MaxLength="50"></asp:TextBox>
                        <asp:Button ID="btn_AddFunId" runat="server" Text="新增" OnClick="btn_AddFun_Click" />
                        &nbsp;<asp:Button ID="btn_CalFunId" runat="server" Text="取消" OnClick="btn_CalFunId_Click" />
                </tr>
                <tr>
                    <td width="15%" class="EditTD1">
                        <asp:Label ID="lbl_PgId" runat="server" Text="順序 - 子功能代號："></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txb_PgOrd" runat="server" Width="25" MaxLength="4" onkeyup="if (value.match(/^\d+$/)==null) value='';"
                            onblur="if (value.match(/^\d+$/)==null) value='';"></asp:TextBox>
                        -
                        <asp:TextBox ID="txb_PgId" runat="server" MaxLength="100"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td width="15%" class="EditTD1">
                        <asp:Label ID="lbl_PgNm" runat="server" Text="子功能名稱："></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txb_PgNm" runat="server" MaxLength="50"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td width="15%" class="EditTD1">
                        <asp:Label ID="lbl_Url" runat="server" Text="路徑："></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txb_Url" runat="server" MaxLength="255"></asp:TextBox>
                    </td>
                </tr>
            </table>
            <table>
            </table>
            <table>
                <tr>
                    <td>
                        <asp:Button ID="btn_Submit" runat="server" Text="確定" OnClick="btn_Submit_Click" />
                    </td>
                    <td>
                        <asp:Button ID="btn_Cancel" runat="server" Text="取消" OnClick="btn_Cancel_Click" />
                    </td>
                </tr>
            </table>
        </asp:Panel>
        <asp:HiddenField ID="hid_Status" runat="server" />
    </div>
</asp:Content>
