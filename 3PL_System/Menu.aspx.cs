using System;
using System.Web.UI.WebControls;
using System.Data;
using _3PL_LIB;
using _3PL_DAO;

namespace _3PL_System
{
    public partial class Menu : System.Web.UI.Page
    {
        private string Login_Server = "3PL";
        private UserInf UI = new UserInf();
        _3PL_CommonQuery _3PLCQ = new _3PL_CommonQuery();

        private string sH;

        protected void Page_Load(object sender, EventArgs e)
        {
            ((_3PLMasterPage)Master).SessionCheck(ref UI);
            if (!IsPostBack)
            {
                _3PLCQ.LOG_Insert("3PL", UI.UserID, "Menu.aspx", "登入", "Login");
                lbl_name.Text = UI.UserName;
                init_tree(UI.UserID);
                GetNotOK_Counts();
            }
        }

        /// <summary>
        /// 取得未完成單據筆數
        /// </summary>
        private void GetNotOK_Counts()
        {
            DataTable dt = _3PLCQ.GetNotOK_Counts(Login_Server, UI);
            int CNT0 = Convert.ToInt32(dt.Rows[0][0]),
                CNT1 = Convert.ToInt32(dt.Rows[0][1]),
                CNT2 = Convert.ToInt32(dt.Rows[0][2]);
            if (CNT0 == 0)
                div_Quotation.Visible = false;
            if (CNT1 == 0)
                div_Assign.Visible = false;
            if (CNT2 == 0)
                div_Adjust.Visible = false;


            badge_1.Text = CNT0.ToString();
            badge_2.Text = CNT1.ToString();
            badge_3.Text = CNT2.ToString();
        }

        /// <summary>
        /// 初始化樹狀選單
        /// </summary>
        /// <param name="p"></param>
        private void init_tree(string profile)
        {
            EmpInf Emp = new EmpInf();
            DataSet dsPgm = new DataSet();
            DataTable dtPgm = new DataTable();
            DataSet dsParent = new DataSet();
            DataTable dtParent = new DataTable();
            try
            {
                dsPgm = Emp.dsPgmTree("3PL", profile);
                dtPgm = dsPgm.Tables[0];
                dsParent = Emp.dsParentsTree("3PL", profile);
                dtParent = dsParent.Tables[0];
                addnode2(dtPgm, dtParent);
            }
            catch
            { }
        }

        /// <summary>
        /// 建立選單節點
        /// </summary>
        /// <param name="tv_treeview">選單</param>
        /// <param name="dt_Pgm">子節點</param>
        /// <param name="dt_Parent">父節點</param>
        /// <param name="dt_WebPage">相關網頁</param>
        private void addnode2(DataTable dt_Pgm, DataTable dt_Parent)
        {
            sH += "<ul class='nav navbar-nav'>";
            foreach (DataRow Parentrow in dt_Parent.Rows)
            {
                sH += "<li class='dropdown'>";
                sH += "<a class='dropdown-toggle' data-toggle='dropdown' href='#'>" + Parentrow["FunNm"].ToString();
                sH += "<span class='caret'></span></a>";
                sH += "<ul class='dropdown-menu'>";
                foreach (DataRow Pgmrow in dt_Pgm.Rows)
                {
                    if (Pgmrow["FunID"].ToString() == Parentrow["FunID"].ToString())
                    {
                        sH += "<li><a class='menu_Clicklog' id='menu_" + Pgmrow["Sn"].ToString()
                            + "' href='"
                            + Pgmrow["PgUrl"].ToString();
                        //Smart-Query另外用新頁面開, 不要用iframe
                        if (Pgmrow["PgUrl"].ToString().IndexOf("Smart-Query") >= 0)
                            sH += "' target='_blank'>";
                        else
                            sH += "' target='iframe_Index'>";

                        sH += Pgmrow["PgNm"].ToString() + "</a></li>";
                    }
                }
                sH += "</ul>";
                sH += "</li>";
            }
            sH += "</ul>";
            tabs.InnerHtml = sH;
        }
    }
}
