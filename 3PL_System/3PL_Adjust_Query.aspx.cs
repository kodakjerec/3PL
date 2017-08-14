using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using _3PL_LIB;
using _3PL_DAO;
using System.Data;

namespace _3PL_System
{
    public partial class _3PL_Adjust_Query : System.Web.UI.Page
    {
        _3PL_CommonQuery _3PLCQ = new _3PL_CommonQuery();
        _3PL_Adjust_DAO _3PLAdjust = new _3PL_Adjust_DAO();
        _3PL_SignOff_DAO _3PLSignOff = new _3PL_SignOff_DAO();
        EmpInf _3PLEmpInf = new EmpInf();

        ControlBind CB = new ControlBind();
        private UserInf UI = new UserInf();
        private string Login_Server = "3PL";

        protected void Page_Load(object sender, EventArgs e)
        {
            ((_3PLMasterPage)Master).SessionCheck(ref UI);

            if (!IsPostBack)
            {
                //產生作業大類下拉式選單
                CB.DropDownListBind(ref DDL_AssignStatusList, _3PLCQ.GetAssignStatusList(), "Value", "Name");
                CB.DropDownListBind(ref DDL_Adj_Type, _3PLCQ.GetFieldList(Login_Server, "AdjType"), "S_bsda_FieldId", "S_bsda_FieldName", "全部", "ALL");

                if (Request.QueryString["VarString"] != null && Request.QueryString["VarString"] != "")
                    PushPageVar(Request.QueryString["VarString"]);
            }
        }

        #region 查詢
        //查詢調整單頭
        protected void Btn_Query_Click(object sender, EventArgs e)
        {
            Btn_Query_Click();
        }
        private void Btn_Query_Click()
        {
            string Adj_type = DDL_Adj_Type.SelectedValue;
            string Wk_Id = Txb_Wk_Id_Query.Text;
            bool bol_Chk_ShowStatusIsZero = Chk_ShowStatusIsZero.Checked;
            string AssignStatusType = DDL_AssignStatusList.SelectedValue;

            Session["QuotationList"] = _3PLAdjust.GetHead(Wk_Id, UI, Adj_type, bol_Chk_ShowStatusIsZero, AssignStatusType);
            GVBind();

            Div_Assign_New.Visible = false;
        }

        //查詢調整單明細
        protected void Wk_Id_Select(object sender, EventArgs e)
        {
            string Wk_Id = ((LinkButton)sender).Text;
            Wk_Id_Select_Act(Wk_Id);
        }
        private void Wk_Id_Select_Act(string Wk_Id)
        {
            Session["QuotationDetail"] = _3PLAdjust.GetDetail(Wk_Id);
            GVBindDetail_Assign();

            BringDetail(Wk_Id);
            Div_Assign_New.Visible = true;
        }
        #endregion

        #region 進入新增頁面
        protected void Btn_New_Click(object sender, EventArgs e)
        {
            string Path = string.Format("3PL_Adjust_Modify.aspx");
            Response.Redirect(Path);
        }
        #endregion

        #region 進入頁面變數
        //收集本頁變數
        private string GetPageVar()
        {
            string VarString = _3PLCQ.Page_AdjustQuery(
                                            DDL_Adj_Type.SelectedValue
                                            , Txb_Wk_Id_Query.Text);
            return VarString;
        }

        //獲得本頁變數
        private void PushPageVar(string VarString)
        {
            //查詢單頭
            string[] VarList = VarString.Split(',');
            DDL_Adj_Type.SelectedValue = VarList[0];
            Txb_Wk_Id_Query.Text = VarList[1];

            Btn_Query_Click();

            //查詢明細
            Wk_Id_Select_Act(VarList[1]);
        }
        #endregion

        #region 綁定
        //綁定派工單單頭
        private void GVBind()
        {
            GV_Quotation_Query.DataSource = Session["QuotationList"];
            GV_Quotation_Query.DataBind();
        }
        //綁定派工單明細 
        private void GVBindDetail_Assign()
        {
            GV_Quotation_Detail_New.DataSource = Session["QuotationDetail"];
            GV_Quotation_Detail_New.DataBind();
        }

        /// <summary>
        /// 帶出調整單明細
        /// </summary>
        /// <param name="Wk_Id"></param>
        private void BringDetail(string Wk_Id)
        {
            DataTable dt = (DataTable)Session["QuotationList"];
            DataRow dr = dt.Rows.Find(Wk_Id);
            if (dr == null)
                return;

            lbl_Adj_Id.Text = dr["Adj_Id"].ToString();
            lbl_Status.Text = dr["StatusName"].ToString();
            lbl_Ady_Type.Text = dr["Adj_Type_Name"].ToString();
            lbl_Adj_PageId.Text = dr["Adj_PageId"].ToString();
            lbl_CrtUser.Text = dr["建單人"].ToString();
            lbl_CrtDate.Text = dr["Crtdate"].ToString();
            lbl_UpdDate.Text = dr["Upddate"].ToString();

            //Memo
            txa_Memo.Value = dr["Memo"].ToString();

            #region 更新明細按鈕是否出現

            if (Convert.ToInt32(dr["Status"]) > 0 && Convert.ToInt32(dr["Status"]) < 20)
            {
                Btn_Update.Visible = true;
            }
            else
            {
                Btn_Update.Visible = false;
            }
            #endregion
        }
        #endregion

        #region GV_Quotation
        /// <summary>
        /// GV_Quotation選擇單據
        /// StatusName=查看明細
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void GV_Quotation_Query_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName != "Page")
            {
                GridViewRow row = ((GridViewRow)((WebControl)(e.CommandSource)).NamingContainer);
                string lbl_Adj_Id = ((LinkButton)row.Cells[1].FindControl("lbl_Adj_Id")).Text;
                //if (e.CommandName == "Select_I_qthe_PLNO")
                //    I_qthe_PLNO_Select_Action(I_qthe_PLNO);
                if (e.CommandName == "StatusName")
                    StatusName_Select(lbl_Adj_Id);
                else if (e.CommandName == "SignOffOk")
                    Btn_SignOffOk_Click(lbl_Adj_Id);
                else if (e.CommandName == "SignOffCancel")
                    Btn_SignOffCancel_Click(lbl_Adj_Id);
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
                    {
                        ((Button)e.Row.FindControl("Btn_SignOffCancel")).Visible = false;
                    }

                    if (((Button)e.Row.FindControl("Btn_SignOffOk")).Text == "X")
                        ((Button)e.Row.FindControl("Btn_SignOffOk")).Visible = false;
                    if (((Button)e.Row.FindControl("Btn_SignOffCancel")).Text == "X")
                        ((Button)e.Row.FindControl("Btn_SignOffCancel")).Visible = false;
                }
            }
        }

        /// <summary>
        /// 換頁
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void GV_Quotation_Query_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GV_Quotation_Query.PageIndex = e.NewPageIndex;
            GVBind();
        }
        #endregion

        #region 更新明細
        //確定更新調整單明細
        protected void Btn_Update_Click(object sender, EventArgs e)
        {
            string Path = string.Format("3PL_Adjust_Modify.aspx?Wk_Id={0}&VarString={1}", lbl_Adj_Id.Text, GetPageVar());
            Response.Redirect(Path);
        }
        #endregion

        #region 簽核相關
        //查詢簽核狀態
        protected void StatusName_Select(string I_qthe_PLNO)
        {
            string Path = "3PL_SignOffLog.aspx?PLNO=" + I_qthe_PLNO + "&Status=4";
            ((_3PLMasterPage)Master).ShowURL(Path);
        }

        //按下簽核按鈕
        protected void Btn_SignOffOk_Click(string PLNO)
        {
            DataTable dt_head = (DataTable)Session["QuotationList"];

            object[] objParam = { PLNO };
            DataRow dr = dt_head.Rows.Find(objParam);

            //需每關做簽核
            _3PLSignOff.SignOff_Quotation(Login_Server, UI.UserID, UI.UserName, true, dr["status"].ToString(), PLNO, "4");

            ((_3PLMasterPage)Master).ShowMessage("簽核執行完成");

            Btn_Query_Click();
            Wk_Id_Select_Act(dr["Adj_Id"].ToString());
        }

        //按下簽核退回按鈕
        protected void Btn_SignOffCancel_Click(string PLNO)
        {
            DataTable dt_head = (DataTable)Session["QuotationList"];

            object[] objParam = { PLNO };
            DataRow dr = dt_head.Rows.Find(objParam);
            //填入資料
            txb_ObjNo.Text = PLNO;
            txb_Status.Text = dr["Status"].ToString();
            txb_StatusName.Text = dr["StatusName"].ToString();
            txb_ObjName.Text = _3PLSignOff.GetSignOffBackReasonPrevious(Login_Server, PLNO);

            #region 畫面顯示
            //顯示table
            ScriptManager.RegisterClientScriptBlock(Page, typeof(string), "c", "show_table1();", true);

            //UpdatePanel更新
            UpdPanel_SignOffBack.Update();
            #endregion
        }
        //簽核退回按鈕實際動作 
        protected void btn_SignOffback_Click(object sender, EventArgs e)
        {
            if (txb_ObjName.Text == "")
            {
                ((_3PLMasterPage)Master).ShowMessage("未輸入退回原因");
                return;
            }

            //簽核取消
            _3PLSignOff.SignOff_Quotation(Login_Server, UI.UserID, UI.UserName, false, txb_Status.Text, txb_ObjNo.Text, "4");

            //寫入原因
            _3PLSignOff.SignOffBackReason(Login_Server, UI.UserID, txb_Status.Text, txb_ObjNo.Text, txb_ObjName.Text, "4");

            ((_3PLMasterPage)Master).ShowMessage("單據簽核已退回");
            Btn_Query_Click();
            Wk_Id_Select_Act(txb_ObjNo.Text);
        }
        #endregion

    }
}
