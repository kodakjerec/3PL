using _3PL_DAO;
using _3PL_LIB;
using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace _3PL_System.creinvlog
{
    public partial class creManageInvPaper : System.Web.UI.Page
    {
        creManageInvLogDAO CMILDAO = new creManageInvLogDAO();
        ControlBind CB = new ControlBind();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //產生作業大類下拉式選單
                CB.DropDownListBind(ref DD_SupdId, CMILDAO.GetSupdId(), "vendor_no", "NewAlias", "請選擇", "");

                RESETPage();
            }
        }

        #region 設定不列入管理盤點的儲位編號
        /// <summary>
        /// 新增不列入管理盤點的儲位編號
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btn_AddOutSlodId_Click(object sender, EventArgs e)
        {
            if (Session["dt_OutSlodId_creInvPaper"] == null)
            {
                ScriptManager.RegisterClientScriptBlock(Page, typeof(string), "alert", "HideProgressBar();", true);
                RESETPage();
                Lbl_finalPaper.Text = "排除儲位出問題，請重新設定";
                Lbl_finalPaper.Visible = true;
                return;
            }
            DataTable dt1 = (DataTable)Session["dt_OutSlodId_creInvPaper"];
            DataRow dr = dt1.NewRow();
            dr["SlodId"] = txb_OutSlodId.Text;
            dt1.Rows.Add(dr);
            dt1.AcceptChanges();
            Session["dt_OutSlodId_creInvPaper"] = dt1;
            GV_OutSlodId_Bind();

            txb_OutSlodId.Text = "";
        }
        #region GV_OutSlodId
        private void GV_OutSlodId_Bind()
        {
            DataTable dt1 = (DataTable)Session["dt_OutSlodId_creInvPaper"];
            GV_OutSlodId.DataSource = dt1;
            GV_OutSlodId.DataBind();
        }
        protected void GV_OutSlodId_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName != "Page")
            {
                GridViewRow row = ((GridViewRow)((WebControl)(e.CommandSource)).NamingContainer);
                string lbl_OutSlodId = ((Label)row.Cells[0].FindControl("lbl_OutSlodId")).Text;
                if (e.CommandName == "DelOutSlodId")
                {
                    DataTable dt1 = (DataTable)Session["dt_OutSlodId_creInvPaper"];
                    dt1.Rows.Find(lbl_OutSlodId).Delete();
                    dt1.AcceptChanges();
                    Session["dt_OutSlodId_creInvPaper"] = dt1;
                    GV_OutSlodId_Bind();
                }
            }
        }
        #endregion
        #endregion

        #region 查詢管理盤點結果(預跑)
        /// <summary>
        /// 查詢管理盤模擬結果
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btn_Query_Click(object sender, EventArgs e)
        {
            string Supdid = DD_SupdId.SelectedValue,
                SiteNo = DD_SiteNo.SelectedValue,
                Filter = "";

            #region 錯誤檢查
            Lbl_finalPaper.Text = "";
            if (Supdid == "")
            {
                Lbl_finalPaper.Text = "沒有選擇廠商";
                Lbl_finalPaper.Visible = true;
                return;
            }
            #endregion

            if (chk_OnlySFC.Checked)
            {
                Filter = "";
            }
            else
            {
                #region 篩選排除儲位
                if (Session["dt_OutSlodId_creInvPaper"] == null)
                {
                    ScriptManager.RegisterClientScriptBlock(Page, typeof(string), "alert", "HideProgressBar();", true);
                    RESETPage();
                    Lbl_finalPaper.Text = "排除儲位出問題，請重新設定";
                    Lbl_finalPaper.Visible = true;
                    return;
                }
                DataTable dt1 = (DataTable)Session["dt_OutSlodId_creInvPaper"];
                foreach (DataRow dr in dt1.Rows)
                {
                    Filter += dr["SlodId"].ToString() + ",";
                }
                #endregion
            }

            btn_creInvPaper.Enabled = false;

            DataTable dt_Date1 = CMILDAO.searchDT1(SiteNo, Supdid, Filter);
            DataTable dt_Date2 = CMILDAO.searchDT2(SiteNo, Supdid, Filter);
            if (dt_Date1.Rows.Count > 0)
            {
                dv_Date.Visible = true;
                lbl_creAdj.Visible = false;

                Session["dt_Date_creInvPaper"] = dt_Date1;
                Session["dt_Date2_creInvPaper"] = dt_Date2;
                GV_Date_Bind();
                GV_Date2_Bind();
            }
            else
            {
                dv_Date.Visible = false;
                lbl_creAdj.Visible = true;
            }
            btn_creInvPaper.Enabled = true;
        }
        protected void GV_Date_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GV_Date.PageIndex = e.NewPageIndex;
            GV_Date_Bind();
        }
        private void GV_Date_Bind()
        {
            DataTable dt1 = (DataTable)Session["dt_Date_creInvPaper"];
            GV_Date.DataSource = dt1;
            GV_Date.DataBind();
        }

        protected void GV_Date2_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GV_Date2.PageIndex = e.NewPageIndex;
            GV_Date2_Bind();
        }
        private void GV_Date2_Bind()
        {
            DataTable dt1 = (DataTable)Session["dt_Date2_creInvPaper"];
            GV_Date2.DataSource = dt1;
            GV_Date2.DataBind();
        }
        #endregion

        #region 產生管理盤點單結果並清除SFC81XX
        /// <summary>
        /// 產生管理盤點單結果並清除SFC81XX
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btn_creInvPaper_Click(object sender, EventArgs e)
        {
            string Supdid = DD_SupdId.SelectedValue,
                SiteNo = DD_SiteNo.SelectedValue,
                Filter = "";

            if (chk_OnlySFC.Checked)
            {
                Filter = "";
            }
            else
            {
                #region 篩選排除儲位
                if (Session["dt_OutSlodId_creInvPaper"] == null)
                {
                    ScriptManager.RegisterClientScriptBlock(Page, typeof(string), "alert", "HideProgressBar();", true);
                    RESETPage();
                    Lbl_finalPaper.Text = "排除儲位出問題，請重新設定";
                    Lbl_finalPaper.Visible = true;
                    return;
                }
                DataTable dt1 = (DataTable)Session["dt_OutSlodId_creInvPaper"];
                foreach (DataRow dr in dt1.Rows)
                {
                    Filter += dr["SlodId"].ToString() + ",";
                }
                #endregion
            }

            string PaperNo = "";
            DataTable dtIsOK = CMILDAO.createDT(SiteNo, Supdid, Filter);

            PaperNo = dtIsOK.Columns[0].ColumnName + ": " + dtIsOK.Rows[0][0].ToString() + " \n" +
                dtIsOK.Columns[1].ColumnName + ": " + dtIsOK.Rows[0][1].ToString() + " \n" +
                dtIsOK.Columns[2].ColumnName + ": " + dtIsOK.Rows[0][2].ToString() + " \n";

            PaperNo += "請至【WMS 9-9盤盈虧認列】繼續操作將盤盈虧儲位歸零\n";
            Lbl_finalPaper.Text = PaperNo.Replace("\n","<br/>");
            Lbl_finalPaper.Visible = true;
            ScriptManager.RegisterClientScriptBlock(Page, typeof(string), "alert", "HideProgressBar();", true);
            
        }
        #endregion

        #region RESET
        protected void btn_Reset_Click(object sender, EventArgs e)
        {
            RESETPage();
        }

        /// <summary>
        /// 重置畫面
        /// </summary>
        private void RESETPage()
        {
            DataTable dt1 = new DataTable();
            dt1.Columns.Add("SlodId");
            DataRow dr1 = dt1.NewRow();
            dr1["SlodId"] = "SFC8101";
            dt1.Rows.Add(dr1);
            DataRow dr2 = dt1.NewRow();
            dr2["SlodId"] = "SFC8102";
            dt1.Rows.Add(dr2);
            //加入primaryKey
            DataColumn[] keys = new DataColumn[1];
            keys[0] = dt1.Columns["SlodId"];
            dt1.PrimaryKey = keys;
            dt1.AcceptChanges();
            Session["dt_OutSlodId_creInvPaper"] = dt1;
            GV_OutSlodId_Bind();

            Session["dt_Date_creInvPaper"] = null;
            Session["dt_Date2_creInvPaper"] = null;
            GV_Date_Bind();
            GV_Date2_Bind();
            btn_creInvPaper.Enabled = false;
            lbl_creAdj.Visible = false;
            Lbl_finalPaper.Visible = false;
            chk_OnlySFC.Checked = true;
        }
        #endregion

        #region 只做SFC儲位
        protected void chk_OnlySFC_CheckedChanged(object sender, EventArgs e)
        {
        }
        #endregion
    }
}