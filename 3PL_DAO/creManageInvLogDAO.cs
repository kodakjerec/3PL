using _3PL_LIB;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace _3PL_DAO
{
    public partial class creManageInvLogDAO
    {
        DB_IO IO = new DB_IO();

        #region Bind
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
            DataSet ds = IO.SqlQuery("LGDC", Sql_cmd, ht1);
            CateIdList = ds.Tables[0];

            return CateIdList;
        }
        #endregion

        #region 查詢結果
        /// <summary>
        /// 查詢管理盤點單(愈跑)(儲位)
        /// </summary>
        /// <returns></returns>
        public DataTable searchDT1(string DBlink, string SupdId, string Filter)
        {
            DataTable CateIdList = new DataTable();

            Hashtable ht1 = new Hashtable();
            ht1.Add("@supdid", SupdId);
            ht1.Add("@update", 0);
            ht1.Add("@filterSlodId", Filter);
            ht1.Add("@Step", 0);
            Hashtable ht2 = new Hashtable();
            DataSet ds = IO.SqlSp(DBlink, "spXSC_物流盤點_產生管理盤點單", ht1, ref ht2);
            CateIdList = ds.Tables[0];

            return CateIdList;
        }
        /// <summary>
        /// 查詢管理盤點單(愈跑)(彙總)
        /// </summary>
        /// <returns></returns>
        public DataTable searchDT2(string DBlink, string SupdId, string Filter)
        {
            DataTable CateIdList = new DataTable();

            Hashtable ht1 = new Hashtable();
            ht1.Add("@supdid", SupdId);
            ht1.Add("@update", 0);
            ht1.Add("@filterSlodId", Filter);
            ht1.Add("@Step", 1);
            Hashtable ht2 = new Hashtable();
            DataSet ds = IO.SqlSp(DBlink, "spXSC_物流盤點_產生管理盤點單", ht1, ref ht2);
            CateIdList = ds.Tables[0];

            return CateIdList;
        }
        #endregion

        #region 產生結果
        /// <summary>
        /// 產生管理盤點單
        /// </summary>
        /// <returns></returns>
        public DataTable createDT(string DBlink, string SupdId, string Filter)
        {
            DataTable CateIdList = new DataTable();

            Hashtable ht1 = new Hashtable();
            ht1.Add("@supdid", SupdId);
            ht1.Add("@update", 1);
            ht1.Add("@filterSlodId", Filter);
            ht1.Add("@Step", 0);
            Hashtable ht2 = new Hashtable();
            DataSet ds = IO.SqlSp(DBlink, "spXSC_物流盤點_產生管理盤點單", ht1, ref ht2);
            CateIdList = ds.Tables[0];

            return CateIdList;
        }

        /// <summary>
        /// 清除SFC81XX
        /// </summary>
        /// <param name="DBlink"></param>
        /// <param name="SupdId"></param>
        public bool ClearSFC81XX(string DBlink, string SupdId)
        {
            bool IsOK = false;
            DataTable CateIdList = new DataTable();

            Hashtable ht1 = new Hashtable();
            ht1.Add("@supdid", SupdId);
            ht1.Add("@update", 2);
            ht1.Add("@filterSlodId", "");
            ht1.Add("@Step", 0);
            Hashtable ht2 = new Hashtable();
            DataSet ds = IO.SqlSp(DBlink, "spXSC_物流盤點_產生管理盤點單", ht1, ref ht2);
            CateIdList = ds.Tables[0];
            if (CateIdList.Rows[0][0].ToString() == "true")
                IsOK = true;

            return IsOK;
        }
        #endregion
    }
}