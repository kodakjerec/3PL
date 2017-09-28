using System.Data;
using _3PL_LIB;
using System.Collections;
using System;
//逆物流費

#region 修改紀錄
//
#endregion
namespace _3PL_DAO
{
    public partial class _3PL_Other_edi_retn_price_DAO
    {
        public string Login_Server = "EDI";
        DB_IO IO = new DB_IO();

        #region 查詢
        /// <summary>
        /// 查詢逆物流費
        /// </summary>
        public DataTable GetData(string site_no
            , string vendor_no
            , string back_id
            , string boxid
            , string back_date_S
            , string back_date_E
            , string Bill_date
            , string Kind)
        {
            DataTable dt = new DataTable();

            string Sql_cmd =
            @"select TOP 500
                edi_retn_price.*,[UIStatus]='Unchanged'
            from edi_retn_price with(nolock)
            where 1=1 ";
            Hashtable ht1 = new Hashtable();
            if (site_no != "") {
                Sql_cmd += " and site_no=@site_no";
                ht1.Add("@site_no", site_no);
            }
            if (vendor_no != "")
            {
                Sql_cmd += " and vendor_no=@vendor_no";
                ht1.Add("@vendor_no",vendor_no);
            }
            if (back_id != "")
            {
                Sql_cmd += " and back_id like @back_id+'%'";
                ht1.Add("@back_id", back_id);
            }
            if (boxid != "")
            {
                Sql_cmd += " and boxid like @boxid+'%'";
                ht1.Add("@boxid", boxid);
            }
            if (back_date_S != "")
            {
                Sql_cmd += " and back_date>=@back_date_S";
                ht1.Add("@back_date_S", back_date_S);
            }
            if (back_date_E != "")
            {
                Sql_cmd += " and back_date<=@back_date_E";
                ht1.Add("@back_date_E", back_date_E);
            }
            if (Bill_date != "")
            {
                Sql_cmd += " and Bill_date=@Bill_date";
                ht1.Add("@Bill_date", Bill_date);
            }
            if (Kind != "")
            {
                Sql_cmd += " and kind=@Kind";
                ht1.Add("@Kind", Kind);
            }

            Sql_cmd += " order by back_date DESC, site_no, vendor_no, back_id DESC, boxid, item_id";

            DataSet ds = IO.SqlQuery(Login_Server, Sql_cmd, ht1);
            dt = ds.Tables[0];

            return dt;
        }

        /// <summary>
        /// 逆物流費的清單
        /// </summary>
        /// <returns></returns>
        public DataTable KindList()
        {
            DataTable dt1 = new DataTable();
            dt1.Columns.Add("Name", typeof(string));
            dt1.Columns.Add("Value", typeof(string));
            dt1.Rows.Add(new object[] { "食安專退", "食安專退" });
            dt1.Rows.Add(new object[] { "食安管制", "食安管制" });
            dt1.Rows.Add(new object[] { "約期下架", "約期下架" });
            dt1.Rows.Add(new object[] { "食安特收", "食安特收" });
            dt1.Rows.Add(new object[] { "管制品", "管制品" });
            dt1.Rows.Add(new object[] { "廠商自行退貨", "廠商自行退貨" });
            dt1.Rows.Add(new object[] { "變規變包", "變規變包" });
            return dt1;
        }

        public DataTable SiteNoList()
        {
            DataTable dt1 = new DataTable();
            dt1.Columns.Add("Name", typeof(string));
            dt1.Columns.Add("Value", typeof(string));
            dt1.Rows.Add(new object[] { "觀音", "觀音" });
            dt1.Rows.Add(new object[] { "岡山", "岡山" });
            dt1.Rows.Add(new object[] { "梧棲", "梧棲" });
            return dt1;
        }

        /// <summary>
        /// 更新資料
        /// </summary>
        /// <param name="dr"></param>
        /// <param name="UI"></param>
        public void UpdateData(DataRow dr, UserInf UI)
        {
            string Sql_cmd =
            @"UPDATE edi_retn_price
            SET kind=@kind,qty_real=@qty_real,edit_date=GETDATE(),edit_user=@User
            WHERE site_no=@site_no
                AND boxid=@boxid
                AND item_id=@item_id";
            Hashtable ht1 = new Hashtable();
            ht1.Add("@kind", dr["kind"]);
            ht1.Add("@qty_real", dr["qty_real"]);
            ht1.Add("@site_no", dr["site_no"]);
            ht1.Add("@boxid", dr["boxid"]);
            ht1.Add("@item_id", dr["item_id"]);
            ht1.Add("@User", UI.UserID);

            DataSet ds = IO.SqlQuery(Login_Server, Sql_cmd, ht1);
        }
        #endregion
    }
}
