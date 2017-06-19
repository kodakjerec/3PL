using System.Data;
using _3PL_LIB;
using System.Collections;

namespace _3PL_DAO
{
    public partial class Entry_DAO
    {
        private string DBlink = "LGDC";
        DB_IO IO = new DB_IO();

        /// <summary>
        /// 取得2個月內的盤點廠商
        /// </summary>
        /// <returns></returns>
        public DataTable GetSupdId()
        {
            DataTable CateIdList = new DataTable();

            string Sql_cmd =
            @"Select a.vendor_no, NewAlias=right(convert(varchar,a.inv_date,112),6)+', '+RTRIM(a.vendor_no)+', '+RTRIM(b.alias)
            from (
                select DISTINCT vendor_no,inv_date from ven_inventory_date
                where inv_date between getdate()-70 and getdate()+2
            ) a
            inner join drp.dbo.drp_supplier b on a.vendor_no=b.ID
            order by a.inv_date DESC, a.vendor_no";
            Hashtable ht1 = new Hashtable();
            DataSet ds = IO.SqlQuery(DBlink, Sql_cmd, ht1);
            CateIdList = ds.Tables[0];

            return CateIdList;
        }

        /// <summary>
        /// 取得盤點日期
        /// </summary>
        /// <param name="DBlink">連結資料庫</param>
        /// <param name="CateId">大類代號</param>
        /// <returns></returns>
        public DataTable GetDate(string SupdId)
        {
            DataTable CateIdList = new DataTable();

            string Sql_cmd =
            @"Select a.inv_date,a.site_no, 
            [status]=CASE 
                        WHEN b.[status] is NULL and a.inv_date<'2015/01/01' THEN '舊資料,沒有歷史紀錄'
                        WHEN b.[status] is NULL THEN '未產生' 
                        WHEN b.[status]='0' THEN '已產單' 
                        WHEN b.[status]='9' THEN '關帳' END
            from VEN_INVENTORY_DATE a 
            left join inventory_log b on a.vendor_no=b.vendor_no and a.inv_date=b.inventory_date and a.site_no=b.site_no
            where a.vendor_no=@SupdId 
            group by a.vendor_no, a.inv_date, a.site_no, b.[status]
            order by inv_date desc, a.site_no";
            Hashtable ht1 = new Hashtable();
            ht1.Add("@SupdId", SupdId);
            DataSet ds = IO.SqlQuery(DBlink, Sql_cmd, ht1);
            CateIdList = ds.Tables[0];

            return CateIdList;
        }

        /// <summary>
        /// 取得上次盤點紀錄
        /// </summary>
        /// <param name="SiteNo"></param>
        /// <param name="SupdId"></param>
        /// <param name="InvDate"></param>
        /// <returns></returns>
        public DataTable GetLastInvLog(string SiteNo, string SupdId, string InvDate)
        {
            DataTable CateIdList = new DataTable();

            Hashtable ht1 = new Hashtable();
            ht1.Add("@Step", "7");
            ht1.Add("@vendor_no", SupdId);
            ht1.Add("@Site_no", SiteNo);
            ht1.Add("@No1", "1,"+InvDate);
            Hashtable ht2 = new Hashtable();
            DataSet ds = IO.SqlSp(DBlink, "盤點_產生盤點結果表", ht1, ref ht2);
            CateIdList = ds.Tables[0];

            //加入primaryKey
            DataColumn[] keys = new DataColumn[1];
            keys[0] = CateIdList.Columns["類別"];
            CateIdList.PrimaryKey = keys;

            return CateIdList;
        }
    }
}
