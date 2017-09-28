using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using _3PL_LIB;
using _3PL_DAO;
using System.Data;

namespace _3PL_System
{
    public partial class _3PL_Assign_Query : System.Web.UI.Page
    {
        _3PL_CommonQuery _3PLCQ = new _3PL_CommonQuery();
        _3PL_Assign_DAO _3PLAssign = new _3PL_Assign_DAO();
        _3PL_CostList_DAO _3PLCostList = new _3PL_CostList_DAO();
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
                DateTime tmp = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                txb_D_qthe_ContractS_Qry.Text = tmp.ToString("yyyy/MM/dd");
                tmp = new DateTime(DateTime.Now.AddMonths(1).Year, DateTime.Now.AddMonths(1).Month, 1).AddDays(-1);
                txb_D_qthe_ContractE_Qry.Text = tmp.ToString("yyyy/MM/dd");

                //產生作業大類下拉式選單
                CB.DropDownListBind(ref DDL_DC, _3PLCQ.GetFieldList(Login_Server, "SiteNo",UI), "S_bsda_FieldId", "S_bsda_FieldName", "請選擇", "");
                CB.DropDownListBind(ref DDL_S_qthe_SiteNo, _3PLCQ.GetFieldList(Login_Server, "SiteNo", UI), "S_bsda_FieldId", "S_bsda_FieldName", "ALL", "");
                CB.DropDownListBind(ref DDL_AssignStatusList, _3PLCQ.GetAssignStatusList(), "Value", "Name");

                if (Request.QueryString["VarString"] != null && Request.QueryString["VarString"] != "")
                    PushPageVar(Request.QueryString["VarString"]);
            }
        }

        #region 查詢
        //查詢報價單單頭
        protected void Btn_Query_Click(object sender, EventArgs e)
        {
            Btn_Query_Click();
        }
        private void Btn_Query_Click()
        {
            string SupdId = Txb_S_qthe_SupdId.Text;
            string SiteNo = DDL_S_qthe_SiteNo.SelectedValue;
            string Wk_Id = Txb_Wk_Id_Query.Text;
            bool bol_Chk_ShowStatusIsZero = Chk_ShowStatusIsZero.Checked;
            string BDate = txb_D_qthe_ContractS_Qry.Text;
            string EDate = txb_D_qthe_ContractE_Qry.Text;
            string AssignStatusType = DDL_AssignStatusList.SelectedValue;

            Session["QuotationList"] = _3PLAssign.GetAssignList(SupdId, SiteNo, Wk_Id, UI, bol_Chk_ShowStatusIsZero, BDate, EDate, AssignStatusType);
            GVBind();

            Div_Assign_New.Visible = false;
            Div_Cost.Visible = false;
        }

        //查詢報價單明細
        protected void Wk_Id_Select(object sender, EventArgs e)
        {
            string Wk_Id = ((LinkButton)sender).Text;
            Wk_Id_Select_Act(Wk_Id);
        }
        private void Wk_Id_Select_Act(string Wk_Id)
        {
            Session["QuotationDetail"] = _3PLAssign.GetAssignDetail(Wk_Id);
            Session["CostDetail"] = _3PLCostList.GetCostDetail(Wk_Id);
            GVBindDetail_Assign();
            GVBindDetail_Cost();

            BringDetail(Wk_Id);
            Div_Assign_New.Visible = true;
            Div_Cost.Visible = true;
        }
        #endregion

        #region 進入頁面變數
        //收集本頁變數
        private string GetPageVar()
        {
            string VarString=_3PLCQ.Page_AssignQuery(
                                            Txb_S_qthe_SupdId.Text
                                            , DDL_S_qthe_SiteNo.SelectedValue
                                            , Txb_Wk_Id_Query.Text
                                            , hidTotal_I_qthe_seq.Value
                                            ,txb_D_qthe_ContractS_Qry.Text
                                            ,txb_D_qthe_ContractE_Qry.Text);

            return VarString;
        }

        //獲得本頁變數
        private void PushPageVar(string VarString)
        {
            //查詢單頭
            //VarList[0]=sup_no
            //VarList[1]=site_no_select
            //VarList[2]=Wk_ID
            //VarList[3]=qthe_seq
            //VarList[4]=Bdate
            //VarList[5]=Edate
            string[] VarList = VarString.Split(',');
            Txb_S_qthe_SupdId.Text = VarList[0];
            DDL_S_qthe_SiteNo.SelectedValue = VarList[1];
            Txb_Wk_Id_Query.Text = VarList[2];
            txb_D_qthe_ContractS_Qry.Text = VarList[4];
            txb_D_qthe_ContractE_Qry.Text = VarList[5];

            Btn_Query_Click();

            //查詢明細
            Wk_Id_Select_Act(VarList[3]);
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
        //綁定成本單明細
        private void GVBindDetail_Cost()
        {
            GV_Cost.DataSource = Session["CostDetail"];
            GV_Cost.DataBind();
        }

        private void BringDetail(string Wk_Id)
        {
            DataTable dt = (DataTable)Session["QuotationList"];
            DataRow dr = dt.Rows.Find(Wk_Id);
            if (dr == null)
                return;

            //派工日期
            if (dr["Wk_Date"].ToString() != "")
                Txb_Wk_Date.Text = DateTime.Parse(dr["Wk_Date"].ToString()).ToString("yyyy/MM/dd");
            else
                Txb_Wk_Date.Text = "";
            //報價主類別
            Txb_Wk_ClassName.Text = dr["Wk_ClassName"].ToString();
            //倉別
            DDL_DC.SelectedValue = dr["DC"].ToString();
            //預定完工日
            if (dr["EtaDate"].ToString() != "")
                Txb_EtaDate.Text = DateTime.Parse(dr["EtaDate"].ToString()).ToString("yyyy/MM/dd");
            else
                Txb_EtaDate.Text = "";
            //建單人
            Txb_CreateUser.Text = dr["CrtUser"].ToString() + "," + dr["CrtUserName"].ToString();
            //處理單位
            Txb_Wk_Unit.Text = "";
            Txb_Wk_Unit.Text = dr["Wk_Unit"].ToString() + "," + dr["Wk_UnitName"].ToString();
            //廠商編號
            Txb_SupID.Text = dr["SupID"].ToString();
            //實際完工日
            if (dr["ActDate"].ToString() != "")
                Txb_ActDate.Text = DateTime.Parse(dr["ActDate"].ToString()).ToString("yyyy/MM/dd");
            else
                Txb_ActDate.Text = "";
            //派工單號
            hidTotal_I_qthe_seq.Value = dr["Wk_Id"].ToString();
            Txb_S_qthe_PLNO.Text = hidTotal_I_qthe_seq.Value;

            //Memo
            txa_Memo.Value = dr["Memo"].ToString();

            #region 更新明細按鈕是否出現

            if (dr["IsOK"].ToString() == "1" && (dr["Step"].ToString() == "1" || dr["Step"].ToString() == "4"))
            {
                Btn_Update.Visible = true;
                Btn_Update_Cost.Visible = true;
            }
            else
            {
                Btn_Update.Visible = false;
                Btn_Update_Cost.Visible = false;
            }
            #endregion

            #region 列印按鈕是否出現
            if (dr["Status"].ToString() != "0")
            {
                Btn_Print.Visible = true;
                Btn_Print_Cost.Visible = true;
            }
            else
            {
                Btn_Print.Visible = false;
                Btn_Print_Cost.Visible = false;
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

                string I_qthe_PLNO = ((LinkButton)row.Cells[1].FindControl("Lbl_I_qthe_PLNO")).Text;
                if (e.CommandName == "StatusName")
                    StatusName_Select(I_qthe_PLNO);
                else if (e.CommandName == "SignOffOk")
                    Btn_SignOffOk_Click(I_qthe_PLNO);
                else if (e.CommandName == "SignOffCancel")
                    Btn_SignOffCancel_Click(I_qthe_PLNO);
            }
        }

        /// <summary>
        /// 資料綁定 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void GV_Quotation_Query_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                #region 簽核按鈕顯示
                string IsOK = ((HiddenField)e.Row.FindControl("IsOK")).Value;
                string Status = ((HiddenField)e.Row.FindControl("Status")).Value;
                if (IsOK == "0")
                {
                    //不允許簽核
                    ((Button)e.Row.FindControl("Btn_SignOffOk")).Visible = false;
                    ((Button)e.Row.FindControl("Btn_SignOffCancel")).Visible = false;
                }
                else
                {
                    //允許簽核
                    //Status=10 建單階段不准退回
                    if (Status == "10")
                        ((Button)e.Row.FindControl("Btn_SignOffCancel")).Visible = false;
                    if (((Button)e.Row.FindControl("Btn_SignOffOk")).Text == "X")
                        ((Button)e.Row.FindControl("Btn_SignOffOk")).Visible = false;
                    if (((Button)e.Row.FindControl("Btn_SignOffCancel")).Text == "X")
                        ((Button)e.Row.FindControl("Btn_SignOffCancel")).Visible = false;
                }
                #endregion

                #region Memo斷行, 斷章取義
                if (e.Row.Cells[7].Text.Length > 6)
                {
                    if (e.Row.Cells[7].Text.Length > 20)
                    {
                        e.Row.Cells[7].Text = e.Row.Cells[7].Text.Substring(0, 6) + "..." + e.Row.Cells[7].Text.Substring(14, 6)+"...";
                    }
                    else
                        e.Row.Cells[7].Text = e.Row.Cells[7].Text.Substring(0, 6) + "...";
                }
                #endregion
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
        //確定更新派工單明細
        protected void Btn_Update_Click(object sender, EventArgs e)
        {
            string Path = string.Format("3PL_Assign_Modify.aspx?Wk_Id={0}&VarString={1}", hidTotal_I_qthe_seq.Value, GetPageVar());
            Response.Redirect(Path);
        }

        //確定更新成本單明細
        protected void Btn_Update_Cost_Click(object sender, EventArgs e)
        {
            string Path = string.Format("3PL_CostList_Modify.aspx?Wk_Id={0}&VarString={1}", hidTotal_I_qthe_seq.Value, GetPageVar());
            Response.Redirect(Path);
        }
        #endregion

        #region 列印
        //列印派工單
        protected void Btn_Print_Click(object sender, EventArgs e)
        {
            string PLNO = hidTotal_I_qthe_seq.Value;
            if (PLNO == "")
                return;
            string Path = _3PLCQ.GetFunList_PgUrl("R001", "WMSS002");
            Path += "&Wk_ID=" + PLNO;
            Path = "window.open('" + Path + "','作業對象')";
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", Path, true);
        }

        //列印成本單
        protected void Btn_Print_Cost_Click(object sender, EventArgs e)
        {
            string PLNO = hidTotal_I_qthe_seq.Value;
            if (PLNO == "")
                return;
            string Path = _3PLCQ.GetFunList_PgUrl("R001", "WMSS003");
            Path += "&Wk_ID=" + PLNO;
            Path = "window.open('" + Path + "','作業對象')";
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", Path, true);
        }
        #endregion

        //選擇廠商對象
        protected void Btn_S_qthe_SupdId_Click(object sender, EventArgs e)
        {
            string Path = "3PL_SupdSelect.aspx?ReturnLocation=" + Txb_S_qthe_SupdId.ClientID + "&btnCloseID=" + ((_3PLMasterPage)Master).FindControl("btn_Close_div_URL").ClientID;
            ((_3PLMasterPage)Master).ShowURL(Path);
        }

        #region 簽核相關
        //查詢簽核狀態
        protected void StatusName_Select(string I_qthe_PLNO)
        {
            string Path = "3PL_SignOffLog.aspx?PLNO=" + I_qthe_PLNO + "&Status=2";
            ((_3PLMasterPage)Master).ShowURL(Path);
        }

        //按下簽核按鈕
        protected void Btn_SignOffOk_Click(string PLNO)
        {
            DataTable dt_head = (DataTable)Session["QuotationList"];

            object[] objParam = { PLNO };
            DataRow dr = dt_head.Rows.Find(objParam);

            #region 檢查欄位是否有填
            #region 建單-->業務主管簽核 必填:預定完工日,派工數量
            if (dr["Step"].ToString() == "1")
            {
                if (dr["EtaDate"].ToString() == "")
                {
                    ((_3PLMasterPage)Master).ShowMessage("派工單:預定完工日尚未填寫");
                    return;
                }
                if (!_3PLAssign.CheckAssignClassStep1(dr["Wk_Id"].ToString()))
                {
                    ((_3PLMasterPage)Master).ShowMessage("派工單:派工數量尚未填寫");
                    return;
                }
            }
            #endregion

            #region 派工人員簽核 完工數量要填,工時要填
            if (dr["Step"].ToString() == "4")
            {
                if (!_3PLAssign.CheckAssignClassStep4(dr["Wk_Id"].ToString()))
                {
                    ((_3PLMasterPage)Master).ShowMessage("派工單:完工數量尚未填寫");
                    return;
                }
                if (!_3PLAssign.CheckCostListStep1(dr["Wk_Id"].ToString()))
                {
                    ((_3PLMasterPage)Master).ShowMessage("成本單:工時尚未填寫");
                    return;
                }
                //檢查通過之後，派工單要押上實際完工日
                _3PLSignOff.Assign_ActDate(Login_Server, dr["Wk_Id"].ToString());
            }
            #endregion
            #endregion

            //需每關做簽核
            int SuccessCount = _3PLSignOff.SignOff_Quotation(Login_Server, UI.UserID, UI.UserName, true, dr["status"].ToString(), PLNO, "2");
            if (SuccessCount <= 0)
            {
                ((_3PLMasterPage)Master).ShowMessage("簽核執行失敗");
                return;
            }

            ((_3PLMasterPage)Master).ShowMessage("簽核執行完成");
            
            Btn_Query_Click();
            Wk_Id_Select_Act(dr["Wk_Id"].ToString());
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
            _3PLSignOff.SignOff_Quotation(Login_Server, UI.UserID, UI.UserName, false, txb_Status.Text, txb_ObjNo.Text, "2");

            //寫入原因
            _3PLSignOff.SignOffBackReason(Login_Server, UI.UserID, txb_Status.Text, txb_ObjNo.Text, txb_ObjName.Text, "2");

            ((_3PLMasterPage)Master).ShowMessage("單據簽核已退回");
            Btn_Query_Click();
            Wk_Id_Select_Act(txb_ObjNo.Text);
        }
        #endregion
    }
}
