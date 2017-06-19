using _3PL_DAO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace 產生盤點結果表
{
    public partial class Main : System.Web.UI.Page
    {
        Entry_DAO entryDAO = new Entry_DAO();
        crePages_DAO crePagesDAO = new crePages_DAO();

        protected void Page_Load(object sender, EventArgs e)
        {
            PushPageVar(Request.QueryString["VarString"]);
            PutDefaultInvLogValue();
        }

        /// <summary>
        /// 填入盤點結果表數量
        /// </summary>
        private void PutDefaultInvLogValue()
        {
            string ImportantString = "";

            DataTable dt1 = entryDAO.GetLastInvLog(lbl_siteno.Text, lbl_supdid.Text, lbl_invdate.Text);

            DataRow dr1 = dt1.Rows.Find("進貨調整");

            if (dr1 != null)
            {
                ImportantString = dr1["儲位數量"].ToString();
                lbl_creAdj.Text = ImportantString;
            }
            dr1 = dt1.Rows.Find("盤點單");
            if (dr1 != null)
            {
                ImportantString = dr1["儲位數量"].ToString();
                lbl_InvPaper.Text = ImportantString;
            }

            dr1 = dt1.Rows.Find("暫存區");
            if (dr1 != null)
            {
                ImportantString = dr1["儲位數量"].ToString();
                lbl_TempArea.Text = ImportantString;
            }
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

        protected void btn_creRcvAdj_Click(object sender, EventArgs e)
        {
            string Path = string.Format("creRcvAdj.aspx?VarString={0}", Request.QueryString["VarString"]);
            Response.Redirect(Path);
        }

        protected void btn_InvPaper_Click(object sender, EventArgs e)
        {
            string Path = string.Format("creInvPaper.aspx?VarString={0}", Request.QueryString["VarString"]);
            Response.Redirect(Path);
        }

        protected void btn_TempArea_Click(object sender, EventArgs e)
        {
            string Path = string.Format("creTempArea.aspx?VarString={0}", Request.QueryString["VarString"]);
            Response.Redirect(Path);
        }

        protected void btn_CreInvLog_Click(object sender, EventArgs e)
        {
            crePagesDAO.creInvLog(lbl_siteno.Text, lbl_supdid.Text, lbl_invdate.Text);
            string Path = string.Format("Entry.aspx");
            ScriptManager.RegisterClientScriptBlock(this, typeof(string), "alert", "alert('執行完畢，請再次查詢結果是否正確');location.href='" + Path + "';", true);
            ScriptManager.RegisterClientScriptBlock(Page, typeof(string), "alert", "HideProgressBar();", true);
            
        }

        /// <summary>
        /// 上一頁
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btn_Pre_Click(object sender, EventArgs e)
        {
            string Path = string.Format("Entry.aspx");
            Response.Redirect(Path);
        }
    }
}