using System.Data;
using _3PL_LIB;
using System.Collections;

namespace _3PL_DAO
{
    public partial class CreInvResult
    {
        DB_IO IO = new DB_IO();

        /// <summary>
        /// 查詢盤點結果表
        /// </summary>
        /// <param name="DBlink"></param>
        /// <param name="vendor_no">供應商編號</param>
        /// <param name="site_no">倉別</param>
        /// <param name="Mode">模式:0-暫存區 1-盤點單 2-進貨調整</param>
        /// <returns></returns>
        public DataTable GetInvList(string DBlink, string vendor_no, string site_no, string Mode)
        {
            DataTable InvList = new DataTable();

            string Sql_cmd = "LGDC.dbo.盤點_產生盤點結果表";// @Step, @vendor_no, @site_no, @Mode";
            Hashtable ht1 = new Hashtable();
            Hashtable ht2 = new Hashtable();
            ht1.Add("@Step", "7");
            ht1.Add("@vendor_no", vendor_no);
            ht1.Add("@Site_no", site_no);
            ht1.Add("@No1", Mode);
            DataSet ds = IO.SqlSp(DBlink, Sql_cmd, ht1,ref ht2);
            InvList = ds.Tables[0];

            return InvList;
        }
    }
}
