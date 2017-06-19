using System;
using System.Data;
using _3PL_DAO;
using _3PL_LIB;
using System.Web;

namespace _3PL_System
{
    public partial class LogIn : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //Clear Session
            Session.Clear();

            //Clear Cookie
            //ClearCookies();
        }

        private void ClearCookies() {
            int limit = Request.Cookies.Count; //Get the number of cookies and 
                                               //use that as the limit.
            HttpCookie aCookie;   //Instantiate a cookie placeholder
            string cookieName;

            //Loop through the cookies
            for (int i = 0; i < limit; i++)
            {
                cookieName = Request.Cookies[i].Name;    //get the name of the current cookie
                if (cookieName!= "ASP.NET_SessionId")
                {
                    aCookie = new HttpCookie(cookieName);    //create a new cookie with the same
                                                             // name as the one you're deleting
                    aCookie.Value = "";    //set a blank value to the cookie 
                    aCookie.Expires = DateTime.Now.AddDays(-1);    //Setting the expiration date
                                                                   //in the past deletes the cookie

                    Response.Cookies.Add(aCookie);    //Set the cookie to delete it.
                }
            }
        }

        protected void btn_Login_Click(object sender, EventArgs e)
        {
            string strAccount = string.Empty;
            string strPsw = string.Empty;
            UserInf UI = new UserInf();
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
                    UI.UserID = strAccount;
                    UI.IP = Request.UserHostAddress;
                    UI.LoginTime = DateTime.Now;
                    UI.UserName = dtUser.Rows[0]["WorkName"] == null ? "" : dtUser.Rows[0]["WorkName"].ToString();
                    UI.Class = GetClass(strAccount);
                    Session["UserInf"] = UI;

                    //Cookies
                    //紀錄UserID
                    HttpCookie myCookie = new HttpCookie("UserID",UI.UserID);
                    myCookie.Expires= DateTime.Now.AddDays(1);
                    Response.Cookies.Add(myCookie);

                    Response.Redirect("Menu.aspx");
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
        /// 取得身分類別
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        private DataTable GetClass(string Id)
        {
            EmpInf Emp = new EmpInf();
            DataTable dt = new DataTable();
            DataSet ds = new DataSet();
            try
            {
                ds = Emp.dsRoleClass_EI("3PL", Id, string.Empty, string.Empty);
                dt = ds.Tables[0];
            }
            catch
            {
            }
            return dt;
        }
    }
}
