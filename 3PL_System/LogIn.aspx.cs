using System;
using System.Data;
using _3PL_DAO;
using _3PL_LIB;
using System.Web;
using System.Web.Security;
using System.Collections.Generic;
using System.Linq;

namespace _3PL_System
{
    public partial class LogIn : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            FormsAuthentication.SignOut();

            //Clear Session1
            Session.RemoveAll();

            //Clear Cookie
            ClearCookies();
        }

        private void ClearCookies()
        {
            if (HttpContext.Current != null)
            {
                int cookieCount = Request.Cookies.Count;
                for (var i = 0; i < cookieCount; i++)
                {
                    var cookie = Request.Cookies[i];
                    if (cookie != null)
                    {
                        switch (cookie.Name) {
                            case "UserID":
                            case "UserClassID":
                                var cookieName = cookie.Name;
                                var expiredCookie = new HttpCookie(cookieName) { Expires = DateTime.Now.AddDays(-1) };
                                Response.Cookies.Add(expiredCookie); // overwrite it
                                break;
                        }
                    }
                }

                // clear cookies server side
                HttpContext.Current.Request.Cookies.Clear();
            }
        }

        protected void btn_Login_Click(object sender, EventArgs e)
        {
            string strAccount = string.Empty;
            string strPsw = string.Empty;
            ACE_EnCode ACE = new ACE_EnCode();
            DataTable dtUser = new DataTable();
            try
            {
                strAccount = tbx_Account.Text.Trim();
                strPsw = ACE.AESEn(tbx_Password.Text.Trim(), "SCSystem");
                string str_Msg = string.Empty;
                if (string.IsNullOrEmpty(strAccount))
                {
                    str_Msg = "請輸入帳號!!\\n";
                }
                if (string.IsNullOrEmpty(strPsw))
                {
                    str_Msg += "請輸入密碼!!\\n";
                }
                if (str_Msg.Length > 0)
                {
                    ((_3PLMasterPage)Master).ShowMessage(str_Msg);
                    return;
                }
                dtUser = GetUser(strAccount, strPsw);
                if (dtUser.Rows.Count > 0)
                {
                    UserInf UI = new UserInf();
                    UI.UserID = strAccount;
                    UI.IP = Request.UserHostAddress;
                    UI.LoginTime = DateTime.Now;
                    UI.ClassId = dtUser.Rows[0]["ClassId"].ToString();
                    UI.UserName = dtUser.Rows[0]["WorkName"] == null ? "" : dtUser.Rows[0]["WorkName"].ToString();
                    UI.DCList = GetDCList(strAccount);
                    Session["UserInf"] = UI;

                    //Cookies
                    //記錄登入資訊
                    FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(1
                        , strAccount
                        , DateTime.Now
                        , DateTime.Now.AddMinutes(30)
                        , true
                        , ""
                        , FormsAuthentication.FormsCookiePath);

                    Response.Cookies.Add(new HttpCookie("UserID", strAccount));
                    //Response.Cookies.Add(new HttpCookie("UserClassID", UI.ClassId));
                    FormsAuthentication.RedirectFromLoginPage(ticket.Name, false);
                }
                else
                {
                    ((_3PLMasterPage)Master).ShowMessage("帳號/密碼輸入錯誤!! \\n 請重新確認!!");
                    return;
                }
            }
            catch
            { }
        }

        /// <summary>
        /// 取得使用者帳號
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="Psw"></param>
        /// <returns></returns>
        private DataTable GetUser(string Id, string Psw)
        {
            EmpInf Emp = new EmpInf();
            DataTable dt = new DataTable();
            DataSet ds = new DataSet();
            try
            {
                ds = Emp.dsEmpInf("3PL", Id, Psw);
                dt = ds.Tables[0];
            }
            catch
            {
            }
            return dt;
        }

        /// <summary>
        /// 取得所歸屬的倉別
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        private DataTable GetDCList(string Id)
        {
            EmpInf Emp = new EmpInf();
            DataTable dt = new DataTable();
            dt.Columns.Add("DC", typeof(string));

            DataSet ds = new DataSet();
            try
            {
                ds = Emp.dsRoleClass_EI("3PL", Id, string.Empty, string.Empty);

                List<DCList> query = (from a in ds.Tables[0].AsEnumerable()
                                      group a by new { DC = a.Field<string>("DC") } into b
                                      orderby b.Key.DC
                                      select new DCList { DC = b.Key.DC }).ToList<DCList>();
                dt = JSONconvert.ListToDataTable<DCList>(query);
            }
            catch
            {
            }
            return dt;
        }

        public class DCList
        {
            public string DC { get; set; }
        }
    }
}
