using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using _3PL_DAO;
using _3PL_LIB;

namespace _3PL_System
{
    public partial class PubModify : System.Web.UI.Page
    {
        private UserInf UI = new UserInf();
        protected void Page_Load(object sender, EventArgs e)
        {
            ((_3PLMasterPage)Master).SessionCheck(ref UI);
            if (!Page.IsPostBack)
            {
                //string BullNo = (Request.QueryString["BullDay"] == null) ? "" : Request.QueryString["BullDay"].ToString();
                GetData(string.Empty);
            }
        }

        /// <summary>
        /// 查詢
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btn_Query_Click(object sender, EventArgs e)
        {
            Check CK = new Check();
            IndexBull IB = new IndexBull();
            DataTable dtBull = new DataTable();
            string strBullDay = txb_BullDay.Text.Trim();
            string Msg = string.Empty;
            try 
            {
                if (strBullDay.Length>0 && !CK.CkDate(strBullDay))
                {
                    Msg += "發佈日期格式錯誤!!!" + " \\n";
                }
                if (Msg.Length == 0)
                {
                    GetData(strBullDay);
                    txb_BullDay.Text = string.Empty;
                }
                else 
                {
                    ScriptManager.RegisterClientScriptBlock(this, typeof(string), "alert", "alert(' " + Msg + " ');", true);
                }
            }
            catch
            {
                ScriptManager.RegisterClientScriptBlock(this, typeof(string), "alert", "alert(' 系統異常，請洽資訊人員!!! ');", true);
                return;
            }
        }

        /// <summary>
        ///  GridView換頁
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gv_List_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                DataTable dt = new DataTable();
                dt = (DataTable)Session["Bulletin"];
                gv_List.PageIndex = e.NewPageIndex;
                BindGV(dt);
            }
            catch
            { }
        }

        protected void gv_List_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                GridViewRow Row = ((GridViewRow)((WebControl)(e.CommandSource)).NamingContainer);
                string BullNo = ((HiddenField)Row.FindControl("hid_BullNo")).Value;
                if (e.CommandName == "Select")
                {
                    string Path = string.Format("PublicMemo.aspx?BullNo={0}", BullNo);
                    ScriptManager.RegisterClientScriptBlock(this, typeof(string), "", "location.href='" + Path + "';", true);
                }
                else if (e.CommandName == "Del")
                {
                    int idx = Row.DataItemIndex;
                    IndexBull IB = new IndexBull();
                    string UpUser = UI.UserID; 
                    bool blDel = IB.DelBull("3PL", BullNo, "1", UpUser);
                    DataTable dt = new DataTable();
                    dt = (DataTable)Session["Bulletin"];
                    dt.Rows.RemoveAt(idx);
                    BindGV(dt);
                }
            }
            catch
            {
                ScriptManager.RegisterClientScriptBlock(this, typeof(string), "alert", "alert(' 系統異常，請洽資訊人員!!! ');", true);
            }
        }

        protected void gv_List_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                string TempBullDay=e.Row.Cells[0].Text;
                e.Row.Cells[0].Text = Convert.ToDateTime(TempBullDay).ToString("yyyy/MM/dd");
                string TempEffDateS = e.Row.Cells[3].Text;
                e.Row.Cells[3].Text = Convert.ToDateTime(TempEffDateS).ToString("yyyy/MM/dd");
                string TempEffDateE = e.Row.Cells[4].Text;
                e.Row.Cells[4].Text = Convert.ToDateTime(TempEffDateE).ToString("yyyy/MM/dd");
            }
        }

        /// <summary>
        /// 建置公告清單
        /// </summary>
        /// <param name="BullDay"></param>
        private void GetData(string BullDay)
        {
            DataTable dtBull = new DataTable();
            try
            {
                dtBull = dtBullList(BullDay);
                if (dtBull.Rows.Count > 0)
                {
                    Session["Bulletin"] = dtBull;
                    BindGV(dtBull);
                }
            }
            catch
            {
                ScriptManager.RegisterClientScriptBlock(this, typeof(string), "alert", "alert(' 系統異常，請洽資訊人員!!! ');", true);
                return;
            }
        }

        /// <summary>
        /// 取得公告清單
        /// </summary>
        /// <param name="BullDay">公佈日期</param>
        /// <returns></returns>
        private DataTable dtBullList(string BullDay)
        {
            IndexBull Bull = new IndexBull();
            DataTable dt = new DataTable();
            DataSet ds = new DataSet();
            ds = Bull.dsBullList("3PL", BullDay, string.Empty);
            dt = ds.Tables[0];
            return dt;
        }

        /// <summary>
        /// GridView建置
        /// </summary>
        /// <param name="dt"></param>
        public void BindGV(DataTable dt)
        {
            gv_List.DataSource = dt;
            gv_List.DataBind();
        }
    }
}
