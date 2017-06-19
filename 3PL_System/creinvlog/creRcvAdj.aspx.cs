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
    public partial class creRcvAdj : System.Web.UI.Page
    {
        crePages_DAO crePagesDAO = new crePages_DAO();

        protected void Page_Load(object sender, EventArgs e)
        {
            PushPageVar(Request.QueryString["VarString"]);
            GV_Date_Bind();
        }

        private void GV_Date_Bind()
        {
            DataTable dt1 = crePagesDAO.searchRCVAdj(lbl_siteno.Text, lbl_supdid.Text, lbl_invdate.Text);
            if (dt1.Rows.Count == 0)
                lbl_creAdj.Visible = true;
            else
            {
                GV_Date.DataSource = dt1;
                GV_Date.DataBind();
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

        /// <summary>
        /// 產生進貨調整結果
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btn_creRcvAdj_Click(object sender, EventArgs e)
        {
            bool IsOK=crePagesDAO.creRCVAdj(lbl_siteno.Text, lbl_supdid.Text);
            if (IsOK == false)
            {
                ScriptManager.RegisterClientScriptBlock(this, typeof(string), "alert", "alert('新增失敗')", true);
                return;
            }
            else
            {
                string Path = string.Format("Main.aspx?VarString={0}", Request.QueryString["VarString"]);
                ScriptManager.RegisterClientScriptBlock(this, typeof(string), "alert", "alert('新增成功');location.href='"+Path+"';", true);
            }
            ScriptManager.RegisterClientScriptBlock(Page, typeof(string), "alert", "HideProgressBar();", true);
            
        }

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