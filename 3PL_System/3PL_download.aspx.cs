using System;
using ExcelTool;
using _3PL_DAO;
using _3PL_LIB;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace _3PL_System
{
    public partial class _3PL_download : System.Web.UI.Page
    {
        CrExcel crExcel = new CrExcel();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(Request["TableName"]))
            {
                ScriptManager.RegisterClientScriptBlock(this, typeof(string), "alert", "alert('路徑錯誤');", true);
                return;
            }
            if (Session[Request["TableName"]] == null) {
                return;
            }
            string TableName = Request["TableName"],
                    FileName = Request["FileName"];
            DataTable dt1 = (DataTable)Session[Request["TableName"]];
            Session[Request["TableName"]] = null;
            crExcel.CrWebEx(dt1, FileName, FileName);
        }
    }
}