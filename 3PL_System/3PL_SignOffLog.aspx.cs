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
                    case "2":
                        Lbl_Type_PLNO.Text = "派工單"; break;
                    case "3":
                        Lbl_Type_PLNO.Text = "成本單"; break;
                    case "4":
                        Lbl_Type_PLNO.Text = "調整單"; break;
                }

                //DataTable SOProcess = _3PLSO.GetSOProcess(Login_Server, Lbl_Status.Text, Txb_PLNO.Text);
                //GV_Process.DataSource = SOProcess;
                //GV_Process.DataBind();

                DataTable SOLog = _3PLSO.GetSOLog(Login_Server, Lbl_Status.Text, Txb_PLNO.Text);
                GV_Log.DataSource = SOLog;
                Paint_div_Log_Graphic(SOLog);
                GV_Log.DataBind();
            }
        }

        private void Paint_div_Log_Graphic(DataTable dt)
        {
            string strContent = "";

            strContent += "<table style='margin-left:30px'>";
            foreach (DataRow dr in dt.Rows)
            {
                strContent += "<tr>";
                strContent += "<td class='Log_Left'>";

                #region 圖形繪製
                strContent += "<table>";
                strContent += "<tr>";
                //繪製圓圈圈
                strContent += "<td>";
                switch (dr["sofp_IsOk"].ToString())
                {
                    case "0":
                        strContent += "<div class='circle_in' style='background-color:red'></div>"; break;
                    case "1":
                        strContent += "<div class='circle_in'></div>"; break;
                    case "N":
                        strContent += "<div class='circle_in'></div>"; break;
                    case "M":
                        strContent += "<div class='circle_in' style='background-color:purple'></div>"; break;
                }
                strContent += "</td>";
                strContent += "<td>" + dr["FinalStatusName"].ToString() + "</td>";
                strContent += "</tr>";
                //繪製箭頭
                strContent += "<tr>";
                strContent += "<td>";

                string strArrow_body_Length = "";
                if (dr["sofp_Reason"].ToString() != "")
                {
                    strArrow_body_Length = ";height:60px";
                }
                switch (dr["sofp_IsOk"].ToString())
                {
                    case "0":
                        strContent += "<div class='Arrow_body' style='background-color:red"+ strArrow_body_Length + "'></div>"; break;
                    case "1":
                        strContent += "<div class='Arrow_body' style='" + strArrow_body_Length + "'></div>"; break;
                    case "N":
                        break;
                    case "M":
                        strContent += "<div class='Arrow_body' style='background-color:purple" + strArrow_body_Length + "'></div>"; break;
                }
                strContent += "</td>";
                strContent += "<td>" + dr["IsOK"].ToString() + "</td>";
                strContent += "</tr>";
                strContent += "</table>";
                #endregion

                strContent += "</td>";

                //繪製Log
                strContent += "<td>";
                //排版用td
                strContent += "<div class='Log_Middle_border'></div>";
                strContent += "<div class='Log_Middle'></div>";
                strContent += "<div class='Log_body'>";
                strContent += "人員：" + dr["sofp_WorkId"].ToString() + "," + dr["sofp_Workname"].ToString() + "<br>";
                strContent += "時間：" + dr["sofp_updateDate"].ToString() + "<br>";
                if (dr["sofp_Reason"].ToString() != "")
                {
                    strContent += "退回原因：" + dr["sofp_Reason"].ToString() + "<br>";
                }
                strContent += "</div>";
                strContent += "</td>";

                strContent += "</tr>";
            }
            strContent += "</table>";

            div_Log_Graphic.InnerHtml = strContent;
        }
    }
}
