using System;
using ExcelTool;
using _3PL_DAO;
using _3PL_LIB;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace _3PL_System
{
    public partial class _3PL_ToExcel002_SendBox : System.Web.UI.Page
    {
        _3PL_CommonQuery GetCQ = new _3PL_CommonQuery();
        _3PL_ToExcel_SendBox_DAO _Excel002 = new _3PL_ToExcel_SendBox_DAO();
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
                
                //預設日期
                DateTime dt=DateTime.Now;
                txb_Bdate_Assign.Text = dt.AddDays(-dt.Day + 1).ToString("yyyy/MM/dd");
                txb_Edate_Assign.Text = dt.AddDays(-1).ToString("yyyy/MM/dd");
                txb_D_qthe_ContractS_Qry.Text = dt.AddDays(-dt.Day + 1).ToString("yyyy/MM/dd");
                txb_D_qthe_ContractE_Qry.Text = dt.AddDays(-1).ToString("yyyy/MM/dd");
                //txb_Quotation_PLNO.Text = "P" + DateTime.Now.ToString("yyyyMMdd").Substring(2, 2);
            }

        }

        //投貨收現明細
        protected void btn_Output_Quotation_Click(object sender, EventArgs e)
        {
            btn_Output_Quotation.Enabled = false;

            string SupdId = Txb_S_qthe_SupdId.Text;
            string SiteNo = DDL_S_qthe_SiteNo.SelectedValue;
            string UNIVNO = txb_UNIVNO.Text;
            string DateS = txb_D_qthe_ContractS_Qry.Text,
                    DateE = txb_D_qthe_ContractE_Qry.Text,
                    PLNO = txb_Quotation_PLNO.Text;

            DataTable dt1 = _Excel002.GetDetail(SiteNo, DateS, DateE, SupdId, PLNO, UNIVNO);
            ScriptManager.RegisterClientScriptBlock(Page, typeof(string), "alert", "HideProgressBar();", true);
            Session["AssignList"] = dt1;

            string Path = "3PL_download.aspx?TableName=AssignList&FileName=投貨收現明細";
            Path = "window.open('" + Path + "','作業對象')";
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", Path, true);

            btn_Output_Quotation.Enabled = true;
        }

        //投貨收現總表
        protected void btn_Output_Assign_Click(object sender, EventArgs e)
        {
            btn_Output_Assign.Enabled = false;

            string SupdId = txb_supno2.Text;
            string SiteNo = DDL_S_qthe_SiteNo2.SelectedValue;
            string Bdate = txb_Bdate_Assign.Text;
            string Edate = txb_Edate_Assign.Text;

            DataTable dt1 = _Excel002.GetTotal(SiteNo, Bdate, Edate, SupdId);
            ScriptManager.RegisterClientScriptBlock(Page, typeof(string), "alert", "HideProgressBar();", true);
            Session["AssignList"] = dt1;

            string Path = "3PL_download.aspx?TableName=AssignList&FileName=投貨收現總表";
            Path = "window.open('" + Path + "','作業對象')";
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", Path, true);

            btn_Output_Assign.Enabled = true;
        }

        #region 分頁切換
        protected void Tab1_Click(object sender, EventArgs e)
        {
            Tab1.CssClass = "btn-primary";
            Tab2.CssClass = "btn-default";
            MultiView1.ActiveViewIndex = 0;
        }

        protected void Tab2_Click(object sender, EventArgs e)
        {
            Tab1.CssClass = "btn-default";
            Tab2.CssClass = "btn-primary";
            MultiView1.ActiveViewIndex = 1;
        }
        #endregion
    }
}
