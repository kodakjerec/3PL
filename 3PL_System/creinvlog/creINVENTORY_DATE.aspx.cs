using _3PL_DAO;
using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web;

namespace _3PL_System.creinvlog
{
    public partial class creINVENTORY_DATE : System.Web.UI.Page
    {
        creINVENTORY_DATE_DAO creInvDateDAO = new creINVENTORY_DATE_DAO();

        protected void Page_Load(object sender, EventArgs e)
        {
            Response.Cache.SetCacheability(HttpCacheability.NoCache);

            if (!IsPostBack)
            {
                //txb_Search.Attributes.Add("OnKeypress", "return clickButton(event,'" + btn_Search.ClientID + "')");
                btn_Search_Click(sender, e);
                Session["dt_Set"] = creInvDateDAO.searchInventory_Date_Set();
            }
        }

        /// <summary>
        /// 輸入盤點日期
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void txb_InvDate_TextChanged(object sender, EventArgs e)
        {
            Cre_ErrMsg.Text = "";
            if (txb_InvDate.Text != "")
            {
                DateTime dt_InvDate = DateTime.Parse(txb_InvDate.Text);
                DataTable dt_Set = (DataTable)Session["dt_Set"];
                if (dt_Set == null)
                {
                    Cre_ErrMsg.Text = "請重新登入，設定檔遺失";
                }
                DataRow dr_Set = dt_Set.Rows[0];
                txb_Back_S.Text = dt_InvDate.AddDays(Convert.ToInt32(dr_Set["STOP_BACK_KEY_S"])).ToString("yyyy/MM/dd");
                txb_Back_E.Text = dt_InvDate.AddDays(Convert.ToInt32(dr_Set["STOP_BACK_KEY_E"])).ToString("yyyy/MM/dd");

                txb_In_S.Text = dt_InvDate.AddDays(Convert.ToInt32(dr_Set["STOP_SITE_IN_S"])).ToString("yyyy/MM/dd");
                txb_In_E.Text = dt_InvDate.AddDays(Convert.ToInt32(dr_Set["STOP_SITE_IN_E"])).ToString("yyyy/MM/dd");

                txb_InDC_S.Text = dt_InvDate.AddDays(Convert.ToInt32(dr_Set["STOP_SITE_IN_S"])).ToString("yyyy/MM/dd");
                txb_InDC_E.Text = dt_InvDate.AddDays(Convert.ToInt32(dr_Set["STOP_SITE_IN_E"])).ToString("yyyy/MM/dd");

                txb_Get_S.Text = dt_InvDate.AddDays(Convert.ToInt32(dr_Set["STOP_DC_GET_S"])).ToString("yyyy/MM/dd");
                txb_Get_E.Text = dt_InvDate.AddDays(Convert.ToInt32(dr_Set["STOP_DC_GET_E"])).ToString("yyyy/MM/dd");

                txb_Out_S.Text = dt_InvDate.AddDays(Convert.ToInt32(dr_Set["STOP_SITE_OUT_S"])).ToString("yyyy/MM/dd");
                txb_Out_E.Text = dt_InvDate.AddDays(Convert.ToInt32(dr_Set["STOP_SITE_OUT_E"])).ToString("yyyy/MM/dd");

                txb_Adj_S.Text = dt_InvDate.AddDays(Convert.ToInt32(dr_Set["STOP_SITE_ADJ_S"])).ToString("yyyy/MM/dd");
                txb_Adj_E.Text = dt_InvDate.AddDays(Convert.ToInt32(dr_Set["STOP_SITE_ADJ_E"])).ToString("yyyy/MM/dd");

                txb_StoreAdj_S.Text = dt_InvDate.AddDays(Convert.ToInt32(dr_Set["STOP_STORE_ADJ_S"])).ToString("yyyy/MM/dd");
                txb_StoreAdj_E.Text = dt_InvDate.AddDays(Convert.ToInt32(dr_Set["STOP_STORE_ADJ_E"])).ToString("yyyy/MM/dd");

                txb_PREORDER_S.Text = dt_InvDate.AddDays(Convert.ToInt32(dr_Set["STOP_PREORDER_S"])).ToString("yyyy/MM/dd");
                txb_PREORDER_E.Text = dt_InvDate.AddDays(Convert.ToInt32(dr_Set["STOP_PREORDER_E"])).ToString("yyyy/MM/dd");
            }
        }

        #region Form_View
        /// <summary>
        /// 開啟/關閉 新增小視窗
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btn_Show_div_Create_Click(object sender, EventArgs e)
        {
            if (btn_Show_div_Create.Text == "開啟新增小視窗")
            {
                div_Create.Visible = true;
                btn_Show_div_Create.Text = "關閉新增小視窗";
            }
            else
            {
                div_Create.Visible = false;
                btn_Show_div_Create.Text = "開啟新增小視窗";
            }
        }
        #endregion

        #region GV_Date
        protected void GV_Date_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GV_Date.PageIndex = e.NewPageIndex;
            GV_Date_Bind();
        }

        /// <summary>
        /// GV_Date綁定
        /// </summary>
        private void GV_Date_Bind()
        {
            DataTable dt1 = (DataTable)Session["dt_InvPaper"];
            GV_Date.DataSource = dt1;
            GV_Date.DataBind();

        }

        protected void GV_Date_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[4].Text = e.Row.Cells[4].Text.Replace("~", "~<br/>");
                e.Row.Cells[5].Text = e.Row.Cells[5].Text.Replace("~", "~<br/>");
                e.Row.Cells[6].Text = e.Row.Cells[6].Text.Replace("~", "~<br/>");
                e.Row.Cells[7].Text = e.Row.Cells[7].Text.Replace("~", "~<br/>");
                e.Row.Cells[8].Text = e.Row.Cells[8].Text.Replace("~", "~<br/>");
                e.Row.Cells[9].Text = e.Row.Cells[9].Text.Replace("~", "~<br/>");
                e.Row.Cells[10].Text = e.Row.Cells[10].Text.Replace("~", "~<br/>");
                e.Row.Cells[11].Text = e.Row.Cells[11].Text.Replace("~", "~<br/>");
                e.Row.Cells[12].Text = e.Row.Cells[12].Text.Replace("~", "~<br/>");
            }
        }
        #endregion

        /// <summary>
        /// 提交更新
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btn_Cfm_Create_Click(object sender, EventArgs e)
        {
            string VendorName = "";
            string ErrMsg = "", SuccessMsg = "";
            Cre_ErrMsg.Text = "";

            #region 錯誤檢查
            if (txb_Vendor_no.Text == "")
            {
                ErrMsg += "未輸入廠編\n";
            }
            else
            {
                DataTable dt1 = creInvDateDAO.CheckVendorNo(txb_Vendor_no.Text);
                if (dt1.Rows.Count <= 0)
                {
                    ErrMsg += "廠編錯誤\n";
                }
                else
                {
                    VendorName = dt1.Rows[0][0].ToString();
                }
            }
            if (txb_InvDate.Text == "")
            {
                ErrMsg += "未輸入盤點日期\n";
            }
            else
            {
                //盤點日七天內禁止新增
                DateTime oldDate = DateTime.Parse(txb_InvDate.Text);
                DateTime newDate = DateTime.Now;
                TimeSpan ts = newDate - oldDate;
                int DifferentDays = ts.Days;
                if (DifferentDays >=-7 && DifferentDays<=0)
                {
                    ErrMsg += "七天內禁止設定盤點日期\n";
                }
            }


            if (ErrMsg != "")
            {
                Cre_ErrMsg.Text = ErrMsg.Replace("\n", "<br/>");
                return;
            }
            #endregion

            #region 新增LGDC
            string YYMM = txb_InvDate.Text.Replace("/", "").Substring(0, 6);
            if (DD_SiteNo.SelectedValue == "DC")
            {
                bool IsOK = false;
                foreach (ListItem li in DD_SiteNo.Items)
                {
                    if (li.Value != "DC")
                    {
                        IsOK = creInvDateDAO.InsertLGDC(YYMM, txb_Vendor_no.Text, li.Value, txb_InvDate.Text
                                        , txb_Back_S.Text, txb_Back_E.Text
                                        , txb_In_S.Text, txb_In_E.Text
                                        , txb_InDC_S.Text, txb_InDC_E.Text
                                        , txb_Get_S.Text, txb_Get_E.Text
                                        , txb_Out_S.Text, txb_Out_E.Text
                                        , txb_Adj_S.Text, txb_Adj_E.Text
                                        , txb_StoreAdj_S.Text, txb_StoreAdj_E.Text
                                        ,txb_PREORDER_S.Text, txb_PREORDER_E.Text);
                        if (!IsOK)
                        {
                            ErrMsg += "新增至" + li.Value + "失敗\n";
                        }
                    }
                }
            }
            else
            {
                bool IsOK = false;
                IsOK = creInvDateDAO.InsertLGDC(YYMM, txb_Vendor_no.Text, DD_SiteNo.SelectedValue, txb_InvDate.Text
                                        , txb_Back_S.Text, txb_Back_E.Text
                                        , txb_In_S.Text, txb_In_E.Text
                                        , txb_InDC_S.Text, txb_InDC_E.Text
                                        , txb_Get_S.Text, txb_Get_E.Text
                                        , txb_Out_S.Text, txb_Out_E.Text
                                        , txb_Adj_S.Text, txb_Adj_E.Text
                                        , txb_StoreAdj_S.Text, txb_StoreAdj_E.Text
                                         , txb_PREORDER_S.Text, txb_PREORDER_E.Text);
                if (!IsOK)
                {
                    ErrMsg += "新增至" + DD_SiteNo.SelectedValue + "失敗\n";
                }
            }

            if (ErrMsg != "")
            {
                Cre_ErrMsg.Text = ErrMsg.Replace("\n", "<br/>");
                return;
            }
            else
            {
                SuccessMsg += "新增至廠商盤點日期成功\n";
            }
            #endregion

            #region 新增XMS_UNDORTNSUP
            bool InsertUNDORTNSUP = false, DoINeedInsert = true;
            DataTable dtUNDORTNSUP = creInvDateDAO.CheckXMS_UNDORTNSUP(txb_Vendor_no.Text);
            if (dtUNDORTNSUP.Rows.Count > 0)
            {
                if (dtUNDORTNSUP.Rows[0][0].ToString() == "永久鎖退")
                {
                    SuccessMsg += "IRIS鎖退已經是永久鎖退\n";
                    DoINeedInsert = false;
                }
            }

            if (DoINeedInsert)
            {
                InsertUNDORTNSUP = creInvDateDAO.InsertXMS_UNDORTNSUP(txb_Vendor_no.Text, txb_Back_S.Text, txb_Back_E.Text);
                if (!InsertUNDORTNSUP)
                {
                    ErrMsg += "新增至IRIS鎖退失敗\n";
                }
                else
                {
                    SuccessMsg += "新增至IRIS鎖退成功\n";
                }
                if (ErrMsg != "")
                {
                    Cre_ErrMsg.Text = ErrMsg.Replace("\n", "<br/>");
                    return;
                }
            }
            #endregion

            Cre_ErrMsg.Text = SuccessMsg.Replace("\n", "<br/>");
            txb_Search.Text = txb_Vendor_no.Text;
            btn_Search_Click(sender, e);
        }

        #region Button功能
        /// <summary>
        /// 查詢報表
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btn_Search_Click(object sender, EventArgs e)
        {
            Session["dt_InvPaper"] = creInvDateDAO.searchInventory_Date(txb_Search.Text);
            GV_Date_Bind();
        }

        /// <summary>
        /// 刪除選擇項目
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btn_Del_Click(object sender, EventArgs e)
        {
            Cre_ErrMsg.Text = "";
            string ErrMsg = "";
            foreach (GridViewRow row in GV_Date.Rows)
            {
                CheckBox chk = (CheckBox)row.Cells[0].FindControl("Chk_Del");
                if (chk.Checked)
                {
                    string CheckStr = ((HiddenField)row.Cells[0].FindControl("hid_CheckStr")).Value;
                    string[] CheckStrList = CheckStr.Split('|');
                    bool IsOK = creInvDateDAO.DeleteLGDC(CheckStrList[0], CheckStrList[1], CheckStrList[2]);
                    if (IsOK)
                    {
                        ErrMsg += CheckStrList[0] + " " + CheckStrList[1] + " " + CheckStrList[2] + " 刪除成功\n";
                    }
                    else
                    {
                        ErrMsg += CheckStrList[0] + " " + CheckStrList[1] + " " + CheckStrList[2] + " 刪除失敗\n";
                    }

                    IsOK=false;
                    IsOK = creInvDateDAO.DeleteXMS_UNDORTNSUP(CheckStrList[1]);
                    if (IsOK)
                    {
                        ErrMsg += CheckStrList[0] + " " + CheckStrList[1] + " " + CheckStrList[2] + " Iris鎖退 刪除成功\n";
                    }
                    else
                    {
                        ErrMsg += CheckStrList[0] + " " + CheckStrList[1] + " " + CheckStrList[2] + " Iris鎖退 刪除失敗\n";
                    }
                }
            }
            Cre_ErrMsg.Text = ErrMsg.Replace("\n", "<br/>");
            btn_Search_Click(sender, e);
        }

        /// <summary>
        /// 匯出Excel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btn_ToExcel_Click(object sender, EventArgs e)
        {
            DataTable dt1 = (DataTable)Session["dt_InvPaper"];
            dt1.Columns["YYMM"].ColumnName = "盤點月份";
            dt1.Columns["SITE_NO"].ColumnName = "倉別";
            dt1.Columns["VENDOR_NO"].ColumnName = "廠商編號";
            dt1.Columns["INV_DATE"].ColumnName = "盤點日期";
            dt1.Columns["lbl_01"].ColumnName = "限制門市登打退貨日期";
            dt1.Columns["lbl_02"].ColumnName = "限制物流進貨日期(XD)";
            dt1.Columns["lbl_03"].ColumnName = "限制物流進貨日期(DC)";
            dt1.Columns["lbl_04"].ColumnName = "限制物流揀貨日期";
            dt1.Columns["lbl_05"].ColumnName = "限制物流出貨日期";
            dt1.Columns["lbl_06"].ColumnName = "限制物流調撥日期";
            dt1.Columns["lbl_07"].ColumnName = "限制門市調撥日期";
            dt1.Columns.Remove("CheckStr");

            ScriptManager.RegisterClientScriptBlock(Page, typeof(string), "alert", "HideProgressBar();", true);
            Session["AssignList"] = dt1;

            string Path = "../3PL_download.aspx?TableName=AssignList&FileName=廠商盤點資料";
            Path = "window.open('" + Path + "','作業對象')";
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", Path, true);
        }
        #endregion
    }
}
