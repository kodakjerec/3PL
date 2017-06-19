using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using _3PL_DAO;
using System.Data;
using _3PL_LIB;

namespace _3PL_System.creinvlog
{
    public partial class Entry : System.Web.UI.Page
    {
        Entry_DAO entryDAO = new Entry_DAO();
        ControlBind CB = new ControlBind();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //產生作業大類下拉式選單
                CB.DropDownListBind(ref DD_SupdId, entryDAO.GetSupdId(), "vendor_no", "NewAlias", "請選擇", "");
            }
        }

        #region 查詢盤點日期
        protected void btn_Query_Click(object sender, EventArgs e)
        {
            #region 錯誤控制
            string ErrMsg = "";
            if (txb_SupdId.Text == "")
            {
                ErrMsg += "請輸入廠商編號";
            }
            if (ErrMsg != "")
            {
                ScriptManager.RegisterClientScriptBlock(this, typeof(string), "alert", "alert('" + ErrMsg + "')", true);
                return;
            }
            #endregion

            string supdid = txb_SupdId.Text;
            DataTable dt1 = entryDAO.GetDate( supdid);
            Session["dt_Date"] = dt1;
            GV_Date_Bind();
            ScriptManager.RegisterClientScriptBlock(Page, typeof(string), "alert", "HideProgressBar();", true);
        }
        #endregion

        #region GV_Date
        /// <summary>
        /// 綁定GV_Date
        /// </summary>
        private void GV_Date_Bind()
        {
            DataTable dt1 = (DataTable)Session["dt_Date"];
            GV_Date.DataSource = dt1;
            GV_Date.DataBind();
        }

        /// <summary>
        /// GV_Date 換頁
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void GV_Date_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GV_Date.PageIndex = e.NewPageIndex;
            GV_Date_Bind();
        }

        /// <summary>
        /// GV_Date點下內容
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void GV_Date_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName != "Page")
            {
                #region RESET狀態
                lbl_creAdj.Visible = false;
                #endregion

                GridViewRow row = ((GridViewRow)((WebControl)(e.CommandSource)).NamingContainer);
                if (e.CommandName == "Select_linkDate")
                {
                    string inv_date = ((LinkButton)row.Cells[0].FindControl("lbl_linkDate")).Text;
                    string siteno = ((Label)row.Cells[0].FindControl("lbl_siteno")).Text;

                    string IsClose = row.Cells[2].Text;
                    btn_gotoMain.Visible = true;
                    if (IsClose == "關帳" || IsClose == "舊資料,沒有歷史紀錄")
                        btn_gotoMain.Visible = false;

                    string supdid = txb_SupdId.Text;
                    Select_inv_date.Value = inv_date;
                    Select_site_no.Value = siteno;
                    DataTable dt1 = entryDAO.GetLastInvLog(siteno, supdid, inv_date);
                    Session["dt_Detail"] = dt1;
                    if (dt1.Rows.Count == 0)
                        lbl_creAdj.Visible = true;
                    GV_Detail_Bind();
                    ScriptManager.RegisterClientScriptBlock(Page, typeof(string), "alert", "HideProgressBar();", true);
                }
            }
        }

        #endregion

        #region GV_Detail
        /// <summary>
        /// 綁定GV_Detail
        /// </summary>
        private void GV_Detail_Bind()
        {
            DataTable dt1 = (DataTable)Session["dt_Detail"];
            GV_Detail.DataSource = dt1;
            GV_Detail.DataBind();
        }

        /// <summary>
        /// GV_Detail 換頁
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void GV_Detail_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GV_Detail.PageIndex = e.NewPageIndex;
            GV_Detail_Bind();
        }
        #endregion

        #region 移到產生頁面
        /// <summary>
        /// 移到Page:_3PL_System.creinvlog
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btn_gotoMain_Click(object sender, EventArgs e)
        {
            string Path = string.Format("Main.aspx?VarString={0}", GetPageVar());
            Response.Redirect(Path);
        }

        /// <summary>
        /// 收集頁面變數
        /// </summary>
        /// <returns></returns>
        private string GetPageVar()
        {
            string VarString = "";
            VarString += Select_site_no.Value + ",";
            VarString += txb_SupdId.Text + ",";
            VarString += Select_inv_date.Value;

            return VarString;
        }
        #endregion

        /// <summary>
        /// 選擇廠編
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void DD_SupdId_SelectedIndexChanged(object sender, EventArgs e)
        {
            #region RESET狀態
            Session["dt_Date"] = null;
            Session["dt_Detail"] = null;
            GV_Date_Bind();
            GV_Detail_Bind();
            btn_gotoMain.Visible = false;
            lbl_creAdj.Visible = false;
            #endregion

            txb_SupdId.Text = DD_SupdId.SelectedValue;
        }


    }
}