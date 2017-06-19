using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using _3PL_LIB;
using _3PL_DAO;
using System.Data;

namespace _3PL_System
{
    public partial class _PL_SignOffLog : System.Web.UI.Page
    {
        _3PL_SignOff_DAO _3PLSO = new _3PL_SignOff_DAO();
        private UserInf UI = new UserInf();
        private string Login_Server = "3PL";

        protected void Page_Load(object sender, EventArgs e)
        {
            ((_3PLMasterPage)Master).SessionCheck(ref UI);

            if (Request.QueryString["PLNO"] != null)
            {
                Txb_PLNO.Text = Request.QueryString["PLNO"];
                Lbl_Status.Text = Request.QueryString["Status"];
                switch (Lbl_Status.Text)
                { 
                    case "1":
                        Lbl_Type_PLNO.Text = "報價單"; break;
                    case"2":
                        Lbl_Type_PLNO.Text = "派工單"; break;
                    case "3":
                        Lbl_Type_PLNO.Text = "成本單"; break;
                }

                //DataTable SOProcess = _3PLSO.GetSOProcess(Login_Server, Lbl_Status.Text, Txb_PLNO.Text);
                //GV_Process.DataSource = SOProcess;
                //GV_Process.DataBind();

                DataTable SOLog = _3PLSO.GetSOLog(Login_Server, Lbl_Status.Text, Txb_PLNO.Text);
                GV_Log.DataSource = SOLog;
                GV_Log.DataBind();
            }
            
        }
    }
}
