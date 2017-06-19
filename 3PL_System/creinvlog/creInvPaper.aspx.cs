using _3PL_DAO;
using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace _3PL_System.creinvlog
{
    public partial class creInvPaper : System.Web.UI.Page
    {
        crePages_DAO crePagesDAO = new crePages_DAO();

        protected void Page_Load(object sender, EventArgs e)
        {
            PushPageVar(Request.QueryString["VarString"]);
        }

        /// <summary>
        /// 獲得變數
        /// </summary>
        /// <param name="VarString"></param>
        private void PushPageVar(string VarString)
        {
            //查詢單頭
            string[] VarList = VarString.Split(',');
            lbl_siteno.Text = VarList[0];
            lbl_supdid.Text = VarList[1];
            lbl_invdate.Text = VarList[2];
        }

        #region 查詢盤點單按鈕
        /// <summary>
        /// GV_Date綁定
        /// </summary>
        private void GV_Date_Bind()
        {
            DataTable dt1 = (DataTable)Session["dt_InvPaper"];
            GV_Date.DataSource = dt1;
            GV_Date.DataBind();

        }

        /// <summary>
        /// GV_Date換頁
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void GV_Date_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GV_Date.PageIndex = e.NewPageIndex;
            GV_Date_Bind();
        }

        /// <summary>
        /// 查詢盤點單按鈕
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btn_searchInvPaper_Click(object sender, EventArgs e)
        {
            #region RESET狀態
            lbl_creAdj.Visible = false;
            lbl_SUM.Visible = false;
            Session["dt_InvPaper"] = null;
            GV_Date_Bind();
            #endregion

            #region 錯誤控制
            string ErrMsg = "";
            if (txb_PaperNo.Text == "")
            {
                ErrMsg += "請輸入盤點單號";
            }
            if (ErrMsg != "")
            {
                ScriptManager.RegisterClientScriptBlock(this, typeof(string), "alert", "alert('" + ErrMsg + "')", true);
                return;
            }
            #endregion

            DataTable dt1 = crePagesDAO.searchInvPaper(lbl_siteno.Text, lbl_supdid.Text, txb_PaperNo.Text);

            if (dt1.Rows.Count == 0)
                lbl_creAdj.Visible = true;
            else
            {
                lbl_SUM.Text = dt1.Rows[0]["統計"].ToString();
                lbl_SUM.Visible = true;
            }
            Session["dt_InvPaper"] = dt1;
            GV_Date_Bind();
        }
        #endregion

        #region 產生盤點單節果
        /// <summary>
        /// 產生盤點單節果按鈕
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btn_creInvPaper_Click(object sender, EventArgs e)
        {
            #region 錯誤控制
            string ErrMsg = "";
            if (txb_PaperNo.Text == "")
            {
                ErrMsg += "請輸入盤點單號";
            }
            if (ErrMsg != "")
            {
                ScriptManager.RegisterClientScriptBlock(this, typeof(string), "alert", "alert('" + ErrMsg + "')", true);
                return;
            }
            #endregion

            bool IsOK = crePagesDAO.creInvPaper(lbl_siteno.Text, lbl_supdid.Text, txb_PaperNo.Text);
            if (IsOK == false)
            {
                ScriptManager.RegisterClientScriptBlock(this, typeof(string), "alert", "alert('新增失敗')", true);
                return;
            }
            else
            {
                string Path = string.Format("Main.aspx?VarString={0}", Request.QueryString["VarString"]);
                ScriptManager.RegisterClientScriptBlock(this, typeof(string), "alert", "alert('新增成功');location.href='" + Path + "';", true);
            }
            ScriptManager.RegisterClientScriptBlock(Page, typeof(string), "alert", "HideProgressBar();", true);
            
        }
        #endregion

        /// <summary>
        /// 上一頁
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btn_Pre_Click(object sender, EventArgs e)
        {
            string Path = string.Format("Main.aspx?VarString={0}", Request.QueryString["VarString"]);
            Response.Redirect(Path);
        }
    }
}