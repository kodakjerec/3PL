using _3PL_DAO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace _3PL_System.creinvlog
{
    public partial class creTempArea : System.Web.UI.Page
    {
        crePages_DAO crePagesDAO = new crePages_DAO();

        protected void Page_Load(object sender, EventArgs e)
        {
            PushPageVar(Request.QueryString["VarString"]);

            #region 查詢暫存區批次
            DataTable dt1 = crePagesDAO.searchTempArea(lbl_siteno.Text, lbl_supdid.Text, lbl_invdate.Text);
            Session["dt_TempArea"] = dt1;
            GV_Date_Bind();
            #endregion
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

        #region 查詢暫存區按鈕
        /// <summary>
        /// GV_Date綁定
        /// </summary>
        private void GV_Date_Bind()
        {
            DataTable dt1 = (DataTable)Session["dt_TempArea"];
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
        /// GV_Date選定批次
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void GV_Date_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName != "Page")
            {
                GridViewRow row = ((GridViewRow)((WebControl)(e.CommandSource)).NamingContainer);
                if (e.CommandName == "Select_linkbatch")
                {
                    string PaperNo = ((LinkButton)row.Cells[0].FindControl("lbl_linkbatch")).Text;
                    string siteno = lbl_siteno.Text, supdid = lbl_supdid.Text;
                    Select_PaperNo.Value = PaperNo;
                    DataTable dt1 = crePagesDAO.searchTempArea(siteno, supdid, PaperNo);
                    Session["dt_TempAreaDetail"] = dt1;
                    GV_Detail_Bind();
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
            DataTable dt1 = (DataTable)Session["dt_TempAreaDetail"];
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

        #region 產生暫存區節果
        /// <summary>
        /// 產生暫存區按鈕
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btn_creTempArea_Click(object sender, EventArgs e)
        {
            #region 錯誤控制
            string ErrMsg = "";
            if (Select_PaperNo.Value == "")
            {
                ErrMsg += "請選擇暫存區批次";
            }
            if (ErrMsg != "")
            {
                ScriptManager.RegisterClientScriptBlock(this, typeof(string), "alert", "alert('" + ErrMsg + "')", true);
                return;
            }
            #endregion

            bool IsOK = crePagesDAO.creTempArea(lbl_siteno.Text, lbl_supdid.Text, Select_PaperNo.Value);
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