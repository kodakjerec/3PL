using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using _3PL_DAO;
using _3PL_LIB;
using System.Globalization;

namespace _3PL_System
{
    public partial class _3PL_Quotation_Print : System.Web.UI.Page
    {
        _3PL_CommonQuery _3PLCQ = new _3PL_CommonQuery();
        _3PL_Quotation_DAO _3PLQu = new _3PL_Quotation_DAO();
        private UserInf UI = new UserInf();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

            }
        }

        protected void Btn_Query_Click(object sender, EventArgs e)
        {
            UI.UserID = "115543";
            Session["QuotationList"] = _3PLQu.sp3PL_CreateQuotationPDF(txb_Quotation_PLNO.Text);
            QuotationHead_Refresh();
        }

        private void QuotationHead_Refresh()
        {
            shadowDIV.Visible = false;
            PrintControl.Visible = false;

            DataTable dt = (DataTable)Session["QuotationList"];
            if (dt.Rows.Count <= 0)
                return;

            shadowDIV.Visible = true;
            PrintControl.Visible = true;

            lbl_報價單號.Text = dt.Rows[0]["報價單號"].ToString();
            lbl_供應商代號.Text = dt.Rows[0]["供應商代號"].ToString();
            lbl_供應商簡稱.Text = dt.Rows[0]["供應商簡稱"].ToString();
            lbl_倉別.Text = dt.Rows[0]["倉別"].ToString();
            lbl_統一編號.Text = dt.Rows[0]["統一編號"].ToString();
            lbl_地址.Text = dt.Rows[0]["地址"].ToString();
            lbl_電話.Text = dt.Rows[0]["電話"].ToString();
            lbl_聯絡人.Text = dt.Rows[0]["聯絡人"].ToString();
            lbl_傳真.Text = dt.Rows[0]["傳真"].ToString();
            lbl_單頭備註.Text = dt.Rows[0]["單頭備註"].ToString();
            lbl_報價日期.Text = ToTaiwanDate(dt.Rows[0]["報價區間起"].ToString());
            lbl_報價區間起.Text = ToTaiwanDate(dt.Rows[0]["報價區間起"].ToString());
            lbl_報價區間迄.Text = ToTaiwanDate(dt.Rows[0]["報價區間迄"].ToString());

            #region 產生明細內容
            string strGroup = "";
            string sH = "";
            foreach (DataRow dr in dt.Rows)
            {
                sH += "<tr>";
                if (strGroup != dr["派工類別"].ToString())
                {
                    strGroup = dr["派工類別"].ToString();

                    int Count = 0;
                    foreach (DataRow dr_Group in dt.Rows)
                    {
                        if (strGroup == dr_Group["派工類別"].ToString())
                            Count++;
                    }
                    sH += "<td rowspan="+Count.ToString()+" class='tdbg' style='width:2cm'>" + dr["派工類別"].ToString() + "</td>";
                    
                }
                sH += "<td class='tdbg' style='width:3cm'>" + dr["費用名稱"].ToString() + "</td>";
                sH += "<td style= 'width:6cm;text-align:right'><span style='color:red'> " + Convert.ToDecimal(dr["單價"]).ToString("#.##", CultureInfo.InvariantCulture) + "</span> " + dr["貨幣單位"].ToString() + " </td>";
                sH += "<td style= 'width:5cm'>" + dr["明細備註"].ToString() + "</td>";
                sH += "</tr>";
            }
            sH += "";
            tabs.InnerHtml = sH;
            #endregion
        }

        /// <summary>
        /// 轉換成民國年月日
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        private string ToTaiwanDate(string v)
        {
            DateTime datetime = DateTime.Parse(v);
            TaiwanCalendar taiwanCalendar = new TaiwanCalendar();

            return string.Format("民國 {0} 年 {1} 月 {2} 日",
                taiwanCalendar.GetYear(datetime),
                datetime.Month,
                datetime.Day);
        }
    }
}
