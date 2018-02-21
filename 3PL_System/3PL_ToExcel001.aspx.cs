using System;
using ExcelTool;
using _3PL_DAO;
using _3PL_LIB;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace _3PL_System
{
    public partial class _3PL_ToExcel001 : System.Web.UI.Page
    {
        _3PL_CommonQuery GetCQ = new _3PL_CommonQuery();
        _3PL_ToExcel _3PLTE = new _3PL_ToExcel();
        CrExcel crExcel = new CrExcel();

        ControlBind CB = new ControlBind();
        private string Login_Server = "3PL";
        private UserInf UI = new UserInf();

        protected void Page_Load(object sender, EventArgs e)
        {
            ((_3PLMasterPage)Master).SessionCheck(ref UI);

            if (!IsPostBack)
            {
                //產生作業大類下拉式選單
                DataTable dt1 = GetCQ.GetFieldList(Login_Server, "SiteNo", UI);
                CB.DropDownListBind(ref DDL_S_qthe_SiteNo, dt1, "S_bsda_FieldId", "S_bsda_FieldName", "ALL", "");
                CB.DropDownListBind(ref DDL_S_qthe_SiteNo2, dt1, "S_bsda_FieldId", "S_bsda_FieldName", "ALL", "");
                CB.DropDownListBind(ref DD_CostIncome, dt1, "S_bsda_FieldId", "S_bsda_FieldName", "ALL", "");

                //預設日期
                DateTime dt = DateTime.Now;
                txb_Bdate_Assign.Text = dt.AddMonths(-1).AddDays(-dt.Day + 1).ToString("yyyy/MM/dd");
                txb_Edate_Assign.Text = dt.AddDays(-dt.Day).ToString("yyyy/MM/dd");
                txb_D_qthe_ContractS_Qry.Text = dt.AddMonths(-1).AddDays(-dt.Day + 1).ToString("yyyy/MM/dd");
                txb_D_qthe_ContractE_Qry.Text = dt.AddDays(-dt.Day).ToString("yyyy/MM/dd");
                txb_Bmonth_CostIncome.Text = dt.AddMonths(-1).AddDays(-dt.Day + 1).ToString("yyyy/MM");
            }

        }

        //報價單
        protected void btn_Output_Quotation_Click(object sender, EventArgs e)
        {
            btn_Output_Quotation.Enabled = false;

            string SupdId = Txb_S_qthe_SupdId.Text;
            string SiteNo = DDL_S_qthe_SiteNo.SelectedValue;
            string DateS = txb_D_qthe_ContractS_Qry.Text,
                    DateE = txb_D_qthe_ContractE_Qry.Text,
                    PLNO = txb_Quotation_PLNO.Text;
            bool bol_Chk_ShowStatusIsZero = Chk_ShowStatusIsZero.Checked;

            DataTable dt1 = _3PLTE.GetQuotationList(SupdId, SiteNo, 0, DateS, DateE, PLNO, UI, bol_Chk_ShowStatusIsZero);
            ScriptManager.RegisterClientScriptBlock(Page, typeof(string), "alert", "HideProgressBar();", true);
            Session["AssignList"] = dt1;

            string Path = "3PL_download.aspx?TableName=AssignList&FileName=報價單明細";
            ((_3PLMasterPage)Master).ShowURL(Path, "download");

            btn_Output_Quotation.Enabled = true;
        }

        //派工單
        protected void btn_Output_Assign_Click(object sender, EventArgs e)
        {
            btn_Output_Assign.Enabled = false;

            string SupdId = Txb_S_qthe_SupdId2.Text;
            string SiteNo = DDL_S_qthe_SiteNo2.SelectedValue;
            string Wk_Id = Txb_Wk_Id_Query.Text;
            string Bdate = txb_Bdate_Assign.Text;
            string Edate = txb_Edate_Assign.Text;
            bool bol_Chk_ShowStatusIsZero = Chk_ShowStatusIsZero.Checked;

            DataTable dt1 = _3PLTE.GetAssignList(SupdId, SiteNo, Wk_Id, UI, bol_Chk_ShowStatusIsZero, Bdate, Edate);
            ScriptManager.RegisterClientScriptBlock(Page, typeof(string), "alert", "HideProgressBar();", true);
            Session["AssignList"] = dt1;

            string Path = "3PL_download.aspx?TableName=AssignList&FileName=派工單明細";
            ((_3PLMasterPage)Master).ShowURL(Path, "download");

            btn_Output_Assign.Enabled = true;
        }

        //成本及收入明細表
        protected void btn_CostIncome_Click(object sender, EventArgs e)
        {
            btn_CostIncome.Enabled = false;

            string SupdId = txb_SupdId_CostIncome.Text;
            string SiteNo = DD_CostIncome.SelectedValue;
            string Wk_Id = txb_PLNO_CostIncome.Text;
            string Bmonth = txb_Bmonth_CostIncome.Text;
            string Bdate = Bmonth + "/01";
            string Edate = DateTime.Parse(Bdate).AddMonths(1).AddDays(-1).ToString("yyyy/MM/dd");

            DataTable dt1 = _3PLTE.GetCostIncome(SupdId, SiteNo, Wk_Id, UI, Bdate, Edate);
            ScriptManager.RegisterClientScriptBlock(Page, typeof(string), "alert", "HideProgressBar();", true);
            Session["AssignList"] = dt1;

            string Path = "3PL_download.aspx?TableName=AssignList&FileName=成本及收入明細表";
            ((_3PLMasterPage)Master).ShowURL(Path, "download");

            btn_CostIncome.Enabled = true;
        }

        #region 分頁切換
        protected void Tab1_Click(object sender, EventArgs e)
        {
            Tab1.CssClass = "btn-primary";
            Tab2.CssClass = "btn-default";
            Tab3.CssClass = "btn-default";
            MultiView1.ActiveViewIndex = 0;
        }

        protected void Tab2_Click(object sender, EventArgs e)
        {
            Tab1.CssClass = "btn-default";
            Tab2.CssClass = "btn-primary";
            Tab3.CssClass = "btn-default";
            MultiView1.ActiveViewIndex = 1;
        }

        protected void Tab3_Click(object sender, EventArgs e)
        {
            Tab1.CssClass = "btn-default";
            Tab2.CssClass = "btn-default";
            Tab3.CssClass = "btn-primary";
            MultiView1.ActiveViewIndex = 2;
        }
        #endregion
    }
}