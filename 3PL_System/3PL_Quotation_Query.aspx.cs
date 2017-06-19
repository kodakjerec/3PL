using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using _3PL_LIB;
using _3PL_DAO;
using System.Data;

namespace _3PL_System
{
    public partial class _3PL_Quotation : System.Web.UI.Page
    {
        _3PL_CommonQuery _3PLCQ = new _3PL_CommonQuery();
        _3PL_Quotation_DAO _3PLQu = new _3PL_Quotation_DAO();
        _3PL_SignOff_DAO _3PLSignOff = new _3PL_SignOff_DAO();

        ControlBind CB = new ControlBind();
        private UserInf UI = new UserInf();
        private string Login_Server = "3PL";

        protected void Page_Load(object sender, EventArgs e)
        {
            ((_3PLMasterPage)Master).SessionCheck(ref UI);

            if (!IsPostBack)
            {
                //產生作業大類下拉式選單
                CB.DropDownListBind(ref DDL_S_qthe_SiteNo, _3PLCQ.GetFieldList(Login_Server, "SiteNo", UI), "S_bsda_FieldId", "S_bsda_FieldName", "ALL", "");
                CB.CheckBoxListBind(ref CheckBoxList_Quotation, _3PLCQ.GetFieldList(Login_Server, "SiteNo"), "S_bsda_FieldId", "S_bsda_FieldName");
                CB.DropDownListBind(ref DDL_QuotationStatusList, _3PLCQ.GetAssignStatusList(), "Value", "Name");

                if (Request.QueryString["VarString"] != null && Request.QueryString["VarString"] != "")
                    PushPageVar(Request.QueryString["VarString"]);
            }
        }

        #region 查詢
        //查詢報價單單頭
        protected void Btn_Query_Click(object sender, EventArgs e)
        {
            Btn_Query_Click_Query();
        }
        private void Btn_Query_Click_Query()
        {
            string SupdId = Txb_S_qthe_SupdId.Text;
            string SiteNo = DDL_S_qthe_SiteNo.SelectedValue;
            string DateS = txb_D_qthe_ContractS_Qry.Text,
                    DateE = txb_D_qthe_ContractE_Qry.Text,
                    PLNO = txb_Quotation_PLNO.Text;
            bool bol_Chk_ShowStatusIsZero = Chk_ShowStatusIsZero.Checked;
            string QuotationStatusType = DDL_QuotationStatusList.SelectedValue;

            Session["QuotationList"] = _3PLQu.GetQuotationList(SupdId, SiteNo, 0, DateS, DateE, PLNO, UI, bol_Chk_ShowStatusIsZero, "0", QuotationStatusType);
            Div_Quotation.Visible = true;
            GVBind();
            div_Detail.Visible = false;
            UpdatePanel1.Update();
        }

        //查詢報價單明細
        private void I_qthe_PLNO_Select_Action(string I_qthe_PLNO)
        {
            if (I_qthe_PLNO == "")
                return;

            Session["QuotationDetail"] = _3PLQu.GetQuotationDetail(I_qthe_PLNO);
            GVBindDetail();

            BringDetail(I_qthe_PLNO);
            div_Detail.Visible = true;
        }
        #endregion

        #region 進入頁面變數
        //收集本頁變數
        private string GetPageVar()
        {
            string VarString = _3PLCQ.Page_QuotationQuery(
                                                Txb_S_qthe_SupdId.Text
                                                , DDL_S_qthe_SiteNo.SelectedValue
                                                , txb_D_qthe_ContractS_Qry.Text
                                                , txb_D_qthe_ContractE_Qry.Text
                                                , Txb_S_qthe_PLNO.Text);

            return VarString;
        }

        //獲得本頁變數
        private void PushPageVar(string VarString)
        {
            //查詢單頭
            string[] VarList = VarString.Split(',');
            Txb_S_qthe_SupdId.Text = VarList[0];
            DDL_S_qthe_SiteNo.SelectedValue = VarList[1];
            txb_D_qthe_ContractS_Qry.Text = VarList[2];
            txb_D_qthe_ContractE_Qry.Text = VarList[3];
            txb_Quotation_PLNO.Text = VarList[4];

            Btn_Query_Click_Query();

            //查詢明細
            I_qthe_PLNO_Select_Action(VarList[4]);
        }
        #endregion

        #region GV_Quotation
        //GV_Quotation按鈕動作
        protected void GV_Quotation_Query_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName != "Page")
            {
                GridViewRow row = ((GridViewRow)((WebControl)(e.CommandSource)).NamingContainer);
                string I_qthe_PLNO = ((LinkButton)row.Cells[1].FindControl("Lbl_I_qthe_PLNO")).Text;
                if (e.CommandName == "Select_I_qthe_PLNO")
                    I_qthe_PLNO_Select_Action(I_qthe_PLNO);
                else if (e.CommandName == "StatusName")
                    StatusName_Select(I_qthe_PLNO);
                else if (e.CommandName == "SignOffOk")
                    Btn_SignOffOk_Click(I_qthe_PLNO);
                else if (e.CommandName == "SignOffCancel")
                    Btn_SignOffCancel_Click(I_qthe_PLNO);
            }

        }

        //GridView是否顯示簽核方框
        protected void GV_Quotation_Query_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                string IsOK = ((HiddenField)e.Row.FindControl("IsOK")).Value;
                string Status = ((HiddenField)e.Row.FindControl("Status")).Value;
                if (IsOK == "0")
                {
                    ((Button)e.Row.FindControl("Btn_SignOffOk")).Visible = false;
                    ((Button)e.Row.FindControl("Btn_SignOffCancel")).Visible = false;
                }
                else
                {
                    if (Status == "10")
                        ((Button)e.Row.FindControl("Btn_SignOffCancel")).Visible = false;
                    if (((Button)e.Row.FindControl("Btn_SignOffOk")).Text == "X")
                        ((Button)e.Row.FindControl("Btn_SignOffOk")).Visible = false;
                    if (((Button)e.Row.FindControl("Btn_SignOffCancel")).Text == "X")
                        ((Button)e.Row.FindControl("Btn_SignOffCancel")).Visible = false;
                }
            }
        }

        //換頁
        protected void GV_Quotation_Detail_New_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GV_Quotation_Detail_New.PageIndex = e.NewPageIndex;
            GVBindDetail();
        }

        //確定更新明細
        protected void Btn_Update_Click(object sender, EventArgs e)
        {
            if (hidTotal_I_qthe_seq.Value == "")
                return;
            string Path = string.Format("3PL_Quotation_Modify.aspx?I_qthe_seq={0}&VarString={1}", hidTotal_I_qthe_seq.Value, GetPageVar());
            Response.Redirect(Path);
        }

        #endregion

        #region 綁定
        //Head綁定
        protected void GV_Quotation_Query_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GV_Quotation_Query.PageIndex = e.NewPageIndex;
            GVBind();
        }
        private void GVBind()
        {
            GV_Quotation_Query.DataSource = Session["QuotationList"];
            GV_Quotation_Query.DataBind();
        }

        //Detail綁定
        private void GVBindDetail()
        {
            GV_Quotation_Detail_New.DataSource = Session["QuotationDetail"];
            GV_Quotation_Detail_New.DataBind();
        }
        private void BringDetail(string I_qthe_PLNO)
        {
            DataTable dt = (DataTable)Session["QuotationList"];
            DataRow dr = dt.Rows.Find(I_qthe_PLNO);
            if (dr == null)
                return;
            Txb_S_qthe_PLNO.Text = dr["S_qthe_PLNO"].ToString();
            string[] SupdId = dr["供應商"].ToString().Split(',');
            Txb_S_qthe_SupdId_New.Text = SupdId[0];
            Txb_S_qthe_SupdName_New.Text = SupdId[1];
            Txb_D_qthe_ContractS.Text = DateTime.Parse(dr["D_qthe_ContractS"].ToString()).ToString("yyyy/MM/dd");
            Txb_D_qthe_ContractE.Text = DateTime.Parse(dr["D_qthe_ContractE"].ToString()).ToString("yyyy/MM/dd");
            Txb_S_qthe_Memo.Text = dr["S_qthe_Memo"].ToString();
            txb_S_qthe_TEL.Text = dr["S_qthe_TEL"].ToString();
            txb_S_qthe_BOSS.Text = dr["S_qthe_BOSS"].ToString();
            txb_S_qthe_FAX.Text = dr["S_qthe_FAX"].ToString();
            TxB_CreateNo.Text = dr["建單人"].ToString();
            TxB_CreateTime.Text = dr["D_qthe_CreateDate"].ToString();
            Chk_IsSpecial.Checked = (bool)dr["I_qthe_IsSpecial"];

            CheckBoxList_Quotation.Items[0].Selected = dr["Flag_DC1"].ToString() == "1" ? true : false;
            CheckBoxList_Quotation.Items[1].Selected = dr["Flag_DC2"].ToString() == "1" ? true : false;
            CheckBoxList_Quotation.Items[2].Selected = dr["Flag_DC3"].ToString() == "1" ? true : false;

            hidTotal_I_qthe_seq.Value = dr["I_qthe_seq"].ToString();
            if (dr["Step"].ToString() != "1")
                Btn_Update.Visible = false;
            else
                Btn_Update.Visible = true;

            if (dr["I_qthe_Status"].ToString() != "0")
                Btn_Print.Visible = true;
            else
                Btn_Print.Visible = false;
        }
        #endregion

        #region 簽核相關
        //查詢簽核狀態
        protected void StatusName_Select(string I_qthe_PLNO)
        {
            string Path = "3PL_SignOffLog.aspx?PLNO=" + I_qthe_PLNO + "&Status=1";
            ((_3PLMasterPage)Master).ShowURL(Path);
        }

        //按下簽核按鈕
        protected void Btn_SignOffOk_Click(string PLNO)
        {
            DataTable dt_head = (DataTable)Session["QuotationList"];

            object[] objParam = { PLNO };
            DataRow dr = dt_head.Rows.Find(objParam);

            int SuccessCount = 0;
            if (Convert.ToBoolean(dr["I_qthe_IsSpecial"].ToString()))
            {
                //其他議價單,需每關做簽核
                SuccessCount = _3PLSignOff.SignOff_Quotation(Login_Server, UI.UserID, UI.UserName, true, dr["I_qthe_status"].ToString(), PLNO, "1");
            }
            else
            {
                //一般單,直接跳完成
                SuccessCount = _3PLSignOff.SignOff_Quotation_NotSpecial(Login_Server, UI.UserID, UI.UserName, true, dr["I_qthe_status"].ToString(), PLNO);
            }
            if (SuccessCount <= 0)
            {
                ((_3PLMasterPage)Master).ShowMessage("簽核執行失敗");
                return;
            }

            ((_3PLMasterPage)Master).ShowMessage("簽核執行完成");
            //ScriptManager.RegisterClientScriptBlock(this, typeof(string), "alert", "alert('簽核執行完成')", true);

            Btn_Query_Click_Query();
            I_qthe_PLNO_Select_Action(PLNO);

        }

        //按下簽核退回按鈕
        protected void Btn_SignOffCancel_Click(string PLNO)
        {
            DataTable dt_head = (DataTable)Session["QuotationList"];

            object[] objParam = { PLNO };
            DataRow dr = dt_head.Rows.Find(objParam);

            txb_ObjNo.Text = PLNO;
            txb_Status.Text = dr["I_qthe_Status"].ToString();
            txb_StatusName.Text = dr["StatusName"].ToString();
            txb_ObjName.Text = _3PLSignOff.GetSignOffBackReasonPrevious(Login_Server, PLNO); ;
            ScriptManager.RegisterClientScriptBlock(Page, typeof(string), "c", "show_table1();", true);
            UpdPanel_SignOffBack.Update();
        }

        //簽核退回按鈕實際動作 
        protected void btn_SignOffback_Click(object sender, EventArgs e)
        {
            if (txb_ObjName.Text == "")
            {
                ((_3PLMasterPage)Master).ShowMessage("未輸入退回原因");
                //ScriptManager.RegisterClientScriptBlock(Page, typeof(string), "alert", "alert('未輸入退回原因')", true);
                return;
            }

            //簽核取消
            _3PLSignOff.SignOff_Quotation(Login_Server, UI.UserID, UI.UserName, false, txb_Status.Text, txb_ObjNo.Text, "1");

            //寫入原因
            _3PLSignOff.SignOffBackReason(Login_Server, UI.UserID, txb_Status.Text, txb_ObjNo.Text, txb_ObjName.Text, "1");

            ((_3PLMasterPage)Master).ShowMessage("單據簽核已退回");
            //ScriptManager.RegisterClientScriptBlock(Page, typeof(string), "alert", "alert('單據簽核已退回')", true);
            Btn_Query_Click_Query();
            I_qthe_PLNO_Select_Action(txb_ObjNo.Text);
        }
        #endregion

        //列印報價單
        protected void Btn_Print_Click(object sender, EventArgs e)
        {
            string PLNO = Txb_S_qthe_PLNO.Text;
            if (PLNO == "")
                return;
            string Path = _3PLCQ.GetFunList_PgUrl("R001", "WMSS001");
            Path += "&PLNO="+PLNO;
            Path = "window.open('" + Path + "','作業對象')";
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", Path, true);
        }

        //選擇廠商對象
        protected void Btn_S_qthe_SupdId_Click(object sender, EventArgs e)
        {
            string Path = "3PL_SupdSelect.aspx?ReturnLocation=" + Txb_S_qthe_SupdId.ClientID + "&btnCloseID=" + ((_3PLMasterPage)Master).FindControl("btn_Close_div_URL").ClientID;
            ((_3PLMasterPage)Master).ShowURL(Path);
        }

    }
}
