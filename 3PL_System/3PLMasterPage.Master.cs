using _3PL_DAO;
using _3PL_LIB;
using System;
using System.Data;
using System.Threading;
using System.Web;
using System.Web.Security;
using System.Web.UI;

namespace _3PL_System
{
    public partial class _3PLMasterPage : System.Web.UI.MasterPage
    {
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            Page.Header.DataBind();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            HttpCookie timerCookie = Request.Cookies["CloseTimer"];
            if (timerCookie != null)
                setTimerInterval(Convert.ToInt32(timerCookie.Value));
            else
            {
                timerCookie = new HttpCookie("CloseTimer");
                timerCookie.Value = Timer1.Interval.ToString();
                timerCookie.Expires = DateTime.MaxValue;
                Response.Cookies.Add(timerCookie);
            }

        }

        #region SessionCheck
        public void SessionCheck(ref UserInf UI)
        {
            if (Session["UserInf"] == null)
            {
                FormsAuthentication.RedirectToLoginPage();
            }
            else
            {
                UI = (UserInf)Session["UserInf"];
            }
        }
        //測試專用
        public void SessionCheck(ref UserInf UI, int Mode)
        {
            if (Session["UserInf"] == null)
            {
                UI = new UserInf();
                UI.UserID = "115543";
                UI.IP = Request.UserHostAddress;
                UI.LoginTime = DateTime.Now;
                UI.UserName = "簡克達";
                EmpInf Emp = new EmpInf();
                DataTable dt = new DataTable();
                DataSet ds = new DataSet();
                ds = Emp.dsRoleClass_EI("3PL", UI.UserID, string.Empty, string.Empty);
                dt = ds.Tables[0];
                UI.DCList = dt;
                Session["UserInf"] = UI;
            }
            else
            {
                UI = (UserInf)Session["UserInf"];
            }
        }
        #endregion

        #region messageURL
        public void btn_Close_div_URL_Click(object sender, EventArgs e)
        {
            if (div_URL.Style["display"] != "none")
            {
                div_URL.Style.Add("display", "none");
                UpdPanel_Message.Update();
            }
        }
        //Open URL
        public void ShowURL(string Path)
        {
            div_URL.Style.Add("display", "inline");
            frame_MessageBox.Attributes["src"] = Path;
            UpdPanel_Message.Update();
            //Timer1.Enabled = true;
        }
        public void ShowURL(string Path, string Mode)
        {
            switch (Mode.ToLower())
            {
                case "download":
                    frame_MessageBox.Attributes["src"] = Path;
                    UpdPanel_Message.Update();
                    break;
            }
        }
        #endregion

        #region message_Message
        public void btn_Close_div_Message_Click(object sender, EventArgs e)
        {
            if (div_Message.Style["display"] != "none")
            {
                if (hid_RedirectURL.Value != "")
                {
                    Response.Redirect(hid_RedirectURL.Value);
                }
                else
                {
                    div_Message.Style.Add("display", "none");
                }

                UpdPanel_Message.Update();
            }
        }
        //Open Message
        public void ShowMessage(string Path)
        {
            div_Message.Style.Add("display", "inline");
            lbl_Message.Text = Path.Replace(@"\n", "<br/>");
            hid_RedirectURL.Value = "";
            UpdPanel_Message.Update();
            Timer1.Enabled = true;

        }
        public void ShowMessage(string Path, string URL)
        {
            div_Message.Style.Add("display", "inline");
            lbl_Message.Text = Path.Replace(@"\n", "<br/>");
            hid_RedirectURL.Value = URL;
            UpdPanel_Message.Update();
            Timer1.Enabled = true;
        }
        #endregion

        #region Message_Confirm

        #endregion

        #region 島數計時器
        protected void Timer1_Tick(object sender, EventArgs e)
        {
            Timer1.Enabled = false;
            btn_Close_div_URL_Click(btn_Close_div_URL, new EventArgs());
            btn_Close_div_Message_Click(btn_close_div_Message, new EventArgs());
        }
        public void setTimerInterval(int miliseconds)
        {
            Timer1.Interval = miliseconds;
        }
        #endregion


    }
}