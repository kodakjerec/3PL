using System;
using System.Web.UI;
using System.Data;
using _3PL_DAO;
using _3PL_LIB;

namespace _3PL_System
{
    public partial class PublicMemo : System.Web.UI.Page
    {
        private UserInf UI = new UserInf();
        protected void Page_Load(object sender, EventArgs e)
        {
            ((_3PLMasterPage)Master).SessionCheck(ref UI);
            if (!Page.IsPostBack)
            {
                txb_Member.Text = UI.UserName;

                string BullNo=(Request.QueryString["BullNo"] == null) ? "" : Request.QueryString["BullNo"].ToString();
                if(BullNo.Length>0)
                {
                    hid_BullNo.Value = BullNo;
                    GetBullInf(BullNo);
                }
            }
        }

        protected void btn_Submit_Click(object sender, EventArgs e)
        {
            Check CK=new Check();
            IndexBull IB = new IndexBull();
            bool blMemo = false;
            string PubMeb = string.Empty;
            string CrtUser = string.Empty;
            string BullDay = string.Empty;
            string EffDateS = string.Empty;
            string EffDateE = string.Empty;
            string Memb = string.Empty;
            string Msg = string.Empty;
            string BullNo = hid_BullNo.Value;
            try
            {
                CrtUser = UI.UserID;
                PubMeb = txb_Member.Text.Trim();
                if (PubMeb.Length == 0)
                {
                    Msg += "請輸入公告人員!!!" + " \\n";
                }
                BullDay = txb_BullDay.Text.Trim();
                if (BullDay.Length == 0)
                {
                    Msg += "請輸入發布日期人員!!!" + " \\n";
                }
                else if(!CK.CkDate(BullDay))
                {
                    Msg += "發布日期格式錯誤!!!" + " \\n";
                }
                EffDateS = txb_EffDateS.Text.Trim();
                if (EffDateS.Length == 0)
                {
                    Msg += "請輸入公告效期起日!!!" + " \\n";
                }
                else if (!CK.CkDate(EffDateS))
                {
                    Msg += "公告效期起日格式錯誤!!!" + " \\n";
                }
                EffDateE = txb_EffDateE.Text.Trim();
                if (EffDateE.Length == 0)
                {
                    Msg += "請輸入公告效期迄日!!!" + " \\n";
                }
                else if (!CK.CkDate(EffDateE))
                {
                    Msg += "公告效期迄日格式錯誤!!!" + " \\n";
                }
                Memb = txb_Memo.Text.Trim();
                if (Memb.Length == 0)
                {
                    Msg += "請輸入公告內容!!!" + " \\n";
                }
                if (Msg.Length == 0)
                {
                    if (BullNo.Length == 0)//建新公告
                    {
                        BullNo = GetBullNo();
                        blMemo = IB.InsBull("3PL", BullNo, BullDay, Memb, PubMeb, EffDateS, EffDateE, CrtUser);
                        if (blMemo)
                        {
                            ((_3PLMasterPage)Master).ShowMessage("發佈完成", "PubModify.aspx?BullDay=" + BullDay);
                        }
                        else
                        {
                            ((_3PLMasterPage)Master).ShowMessage("發佈失敗");
                        }
                    }
                    else//異動公告
                    {
                        blMemo = IB.UpBull("3PL", BullNo, BullDay, Memb, PubMeb, EffDateS, EffDateE, CrtUser);
                        if (blMemo)
                        {
                            ((_3PLMasterPage)Master).ShowMessage("更新完成", "PubModify.aspx?BullDay=" + BullDay);
                        }
                        else 
                        {
                            ((_3PLMasterPage)Master).ShowMessage("更新失敗");
                        }
                    }
                }
                else
                {
                    ((_3PLMasterPage)Master).ShowMessage(Msg);
                }
            }
            catch
            {
                ((_3PLMasterPage)Master).ShowMessage("系統異常，請洽資訊部");
            }
        }

        /// <summary>
        /// 取得公告訊息 
        /// </summary>
        /// <param name="BullNo"></param>
        private void GetBullInf(string BullNo)
        {
            IndexBull IB = new IndexBull();
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            try
            {
                ds = IB.dsBullList("3PL", string.Empty, BullNo);
                dt = ds.Tables[0];
                txb_Member.Text = dt.Rows[0]["Bull_Member"] == null ? "" : dt.Rows[0]["Bull_Member"].ToString();
                txb_BullDay.Text = dt.Rows[0]["Bull_Day"] == null ? "" : Convert.ToDateTime(dt.Rows[0]["Bull_Day"]).ToString("yyyy-MM-dd");
                txb_EffDateS.Text = dt.Rows[0]["Eff_DateS"] == null ? "" : Convert.ToDateTime(dt.Rows[0]["Eff_DateS"]).ToString("yyyy-MM-dd");
                txb_EffDateE.Text = dt.Rows[0]["Eff_DateE"] == null ? "" : Convert.ToDateTime(dt.Rows[0]["Eff_DateE"]).ToString("yyyy-MM-dd");
                txb_Memo.Text = dt.Rows[0]["Detail"] == null ? "" : dt.Rows[0]["Detail"].ToString();
            }
            catch
            {
                ScriptManager.RegisterClientScriptBlock(this, typeof(string), "alert", "alert('系統異常，請洽資訊部 !!! ');", true);
            }
        }

        /// <summary>
        /// 取得公告單號
        /// </summary>
        /// <returns></returns>
        private string GetBullNo()
        {
            IndexBull IB = new IndexBull();
            DataSet dsNo = new DataSet();
            DataTable dtNo = new DataTable();
            string BullNo = string.Empty;
            try
            {
                string strToday = DateTime.Now.ToString("yyyyMMdd");
                string tempBullNo = "B" + strToday;
                dsNo = IB.dsBullList("3PL", string.Empty, tempBullNo);
                dtNo = dsNo.Tables[0];
                if (dtNo.Rows.Count == 0)
                {
                    BullNo = tempBullNo + "00001";
                }
                else 
                {
                    tempBullNo = dtNo.Rows[0]["Bull_No"] == null ? "00" : dtNo.Rows[0]["Bull_No"].ToString();
                    long intTempNo = Convert.ToInt64(tempBullNo.Substring(1, tempBullNo.Length - 1))+1;
                    BullNo = "B" + intTempNo.ToString();
                }

            }
            catch
            { }
            return BullNo;
        }

    }
}
