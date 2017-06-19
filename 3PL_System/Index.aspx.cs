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
    public partial class Index : System.Web.UI.Page
    {
        private UserInf UI = new UserInf();
        protected void Page_Load(object sender, EventArgs e)
        {
            ((_3PLMasterPage)Master).SessionCheck(ref UI);
            if (!IsPostBack)
            {
                CrtBull();
            }
        }

        /// <summary>
        /// 產生公佈欄
        /// </summary>
        private void CrtBull()
        {
            DataTable dtBull = new DataTable();
            try
            {
                dtBull = GetBull();
                PublicBull(dtBull);
            }
            catch
            {
                ((_3PLMasterPage)Master).ShowMessage("系統異常，請洽資訊部");
            }
        }

        /// <summary>
        /// 取得公告內容
        /// </summary>
        /// <returns></returns>
        private DataTable GetBull()
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            IndexBull Bull = new IndexBull();
            ds = Bull.dsBull("3PL");
            dt = ds.Tables[0];
            return dt;
        }

        //建置公告欄內容
        private void PublicBull(DataTable dt)
        {
            string Path = "";
            int Index = dt.Rows.Count;
            try
            {
                for (int i = 0; i < Index; i++)
                {
                    string BullDay = dt.Rows[i]["Bull_Day"] == null ? "" : dt.Rows[i]["Bull_Day"].ToString();//公告日期
                    string Memo = dt.Rows[i]["Detail"] == null ? "" : dt.Rows[i]["Detail"].ToString();//公告內容
                    string PubMen = dt.Rows[i]["Bull_Member"] == null ? "" : dt.Rows[i]["Bull_Member"].ToString();//人員
                    Path += BullDay + "\\n" + Memo + "\\n By:" + PubMen + "\\n";
                }
                txb_Bulletin.Text = Path.Replace(@"\n", "<br/>").Replace("\r\n", "<br/>");
            }
            catch
            {

            }
        }
    }
}
