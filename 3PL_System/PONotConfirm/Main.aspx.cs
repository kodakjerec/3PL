using System;
using System.Web.UI;
using System.Data;
using _3PL_DAO;

namespace _3PL_System.PONotConfirm
{
    public partial class Alarm_PONotConfirm : System.Web.UI.Page
    {
        Alarm_PONotConfirm_DAO alarmDAO = new Alarm_PONotConfirm_DAO();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CB_siteList.SelectedIndex = 0;
                Session["BTime"] = DateTime.Now;
                Btn_Refresh_Click(Btn_Refresh, e);
            }
        }

        protected void Timer1_Tick(object sender, EventArgs e)
        {
            DateTime BTime = (DateTime)Session["BTime"];
            double wasteMin = new TimeSpan(DateTime.Now.Ticks - BTime.Ticks).TotalSeconds;
            int leftSeconds = 3600 - Convert.ToInt32(wasteMin);
            if (leftSeconds >0)
            {
                int leftMinutes = leftSeconds / 60;
                leftSeconds = leftSeconds % 60;
                Label1.Text = "重新整理倒數計時: " + leftMinutes.ToString() + " 分 "+leftSeconds.ToString()+" 秒";
                UpdatePanel1.Update();
            }
            else
            {
                Label1.Text = "Panel refresh!!";  
                Btn_Refresh_Click(Btn_Refresh, e);
            }
        }

        protected void Btn_Refresh_Click(object sender, EventArgs e)
        {
            Session["BTime"] = DateTime.Now;
            Refresh();
            ScriptManager.RegisterClientScriptBlock(Page, typeof(string), "alert", "HideProgressBar();", true);
            
        }
        private void Refresh()
        {
            string DB = CB_siteList.SelectedValue;

            #region 取得今天PO單總明細
            DataTable dt = alarmDAO.CheckList(DB);
            GV_FillForm_Detail.DataSource = dt;
            GV_FillForm_Detail.DataBind();
            #endregion

            #region 取得未驗收完成的細項

            int NotFinishCount=Convert.ToInt32(dt.Rows[0]["Tcount"]) - Convert.ToInt32(dt.Rows[0]["ConfirmCount"]);
            if (NotFinishCount != 0)
            {
                DataTable dt2 = alarmDAO.CheckDetail(DB);
                GV_CheckList.DataSource = dt2;
                GV_CheckList.DataBind();
            }
            #endregion
        }
    }
}