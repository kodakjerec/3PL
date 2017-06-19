using _3PL_LIB;
using System;
using System.Collections;
using System.Data;

namespace _3PL_DAO
{
    public class Alarm_PONotConfirm_DAO
    {
        DB_IO IO = new DB_IO();

        public DataTable CheckList(string DB)
        {
            DataTable dt = new DataTable();

            string Sql_cmd =
        @"declare @Bdate date=getdate()-2,
		          @Edate date=getdate()+1;
          declare @Bdatetime datetime=@Bdate,
		          @Edatetime datetime=@Edate;
          select Tcount=count(1),
            ConfirmCount=SUM(Case I_recr_flag WHEN 48 THEN 1 ELSE 0 END),
            CancelCount=SUM(Case I_recr_flag WHEN 49 THEN 1 ELSE 0 END),
            leftCount=SUM(CASE WHEN I_recr_flag<48 THEN 1 ELSE 0 END)
            FROM(
            select L_recr_id ,I_recr_flag
            from reci_record with(nolock)
            inner join recirecord_item with(nolock) on L_recr_id=L_reri_recrid
            where T_recr_creatdate between @Bdatetime and @Edatetime
	            and L_reri_takeqty1>0
            group by L_recr_id ,I_recr_flag) a";
            Hashtable ht1 = new Hashtable();

            dt = IO.SqlQuery(DB, Sql_cmd, ht1).Tables[0];
           
            return dt;
        }

        public DataTable CheckDetail(string DB)
        {
            DataTable dt = new DataTable();

            string Sql_cmd =
        @"declare @Bdate date=getdate()-2,
		          @Edate date=getdate()+1;
          declare @Bdatetime datetime=@Bdate,
		          @Edatetime datetime=@Edate;
            select c.S_rech_erpid, a.L_recr_id, 
            S_optd_name=CASE a.I_recr_flag
				WHEN 45 THEN '未驗收' WHEN 46 THEN '驗收中' WHEN 47 THEN 'SORT投完' WHEN 48 THEN '已確認' WHEN 49 THEN '日結取消' END, Tqty=sum(ISNULL(b.L_reri_takeqty1,0))
            from reci_record a with(nolock)
            inner join recirecord_item b with(nolock) on a.L_recr_id=b.L_reri_recrid
            inner join reci_head c with(nolock) on a.L_recr_rechid=c.L_rech_id
            where a.T_recr_creatdate between @Bdatetime and @Edatetime
	            and a.I_recr_flag!=48
            group by c.S_rech_erpid, a.L_recr_id, a.I_recr_flag
            order by a.I_recr_flag DESC, c.S_rech_erpid, a.L_recr_id";
            Hashtable ht1 = new Hashtable();

            dt = IO.SqlQuery(DB, Sql_cmd, ht1).Tables[0];

            return dt;
        }
    }
}