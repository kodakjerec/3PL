using _3PL_LIB;
using System;
using System.Collections;
using System.Data;
using System.Web;

namespace _3PL_DAO
{
    public partial class crePages_DAO
    {
        private string DBlink = "LGDC";
        DB_IO IO = new DB_IO();

        #region 進貨調整
        /// <summary>
        /// 查詢進貨調整
        /// </summary>
        /// <param name="SiteNo"></param>
        /// <param name="SupdId"></param>
        /// <param name="inv_date"></param>
        /// <returns></returns>
        public DataTable searchRCVAdj(string SiteNo, string SupdId, string inv_date)
        {
            DataTable CateIdList = new DataTable();

            Hashtable ht1 = new Hashtable();
            ht1.Add("@Step", "2");
            ht1.Add("@vendor_no", SupdId);
            ht1.Add("@Site_no", SiteNo);
            ht1.Add("@No1", "0");
            Hashtable ht2 = new Hashtable();
            DataSet ds = IO.SqlSp(DBlink, "盤點_產生盤點結果表", ht1, ref ht2);
            CateIdList = ds.Tables[0];

            return CateIdList;
        }

        /// <summary>
        /// 產生進貨調整
        /// </summary>
        /// <param name="SiteNo"></param>
        /// <param name="SupdId"></param>
        /// <returns></returns>
        public bool creRCVAdj(string SiteNo, string SupdId)
        {
            bool IsOK = false;
            DataTable CateIdList = new DataTable();

            Hashtable ht1 = new Hashtable();
            ht1.Add("@Step", "2");
            ht1.Add("@vendor_no", SupdId);
            ht1.Add("@Site_no", SiteNo);
            ht1.Add("@No1", "1");
            Hashtable ht2 = new Hashtable();
            DataSet ds = IO.SqlSp(DBlink, "盤點_產生盤點結果表", ht1, ref ht2);
            CateIdList = ds.Tables[0];

            if (CateIdList.Rows[0][0].ToString()=="true")
                IsOK = true;

            return IsOK;
        }
        #endregion

        #region 盤點單
        /// <summary>
        /// 查詢盤點單
        /// </summary>
        /// <param name="SiteNo"></param>
        /// <param name="SupdId"></param>
        /// <param name="PaperNo"></param>
        /// <returns></returns>
        public DataTable searchInvPaper(string SiteNo, string SupdId, string PaperNo)
        {
            DataTable CateIdList = new DataTable();

            Hashtable ht1 = new Hashtable();
            ht1.Add("@Step", "5");
            ht1.Add("@vendor_no", SupdId);
            ht1.Add("@Site_no", SiteNo);
            ht1.Add("@No1", PaperNo);
            Hashtable ht2 = new Hashtable();
            DataSet ds = IO.SqlSp(DBlink, "盤點_產生盤點結果表", ht1, ref ht2);
            CateIdList = ds.Tables[0];

            return CateIdList;
        }

        /// <summary>
        /// 產生盤點單
        /// </summary>
        /// <param name="SiteNo"></param>
        /// <param name="SupdId"></param>
        /// <returns></returns>
        public bool creInvPaper(string SiteNo, string SupdId, string PaperNo)
        {
            bool IsOK = false;
            DataTable CateIdList = new DataTable();

            Hashtable ht1 = new Hashtable();
            ht1.Add("@Step", "1");
            ht1.Add("@vendor_no", SupdId);
            ht1.Add("@Site_no", SiteNo);
            ht1.Add("@No1", PaperNo);
            Hashtable ht2 = new Hashtable();
            DataSet ds = IO.SqlSp(DBlink, "盤點_產生盤點結果表", ht1, ref ht2);
            CateIdList = ds.Tables[0];

            if (CateIdList.Rows[0][0].ToString() == "true")
                IsOK = true;

            return IsOK;
        }
        #endregion

        #region 暫存區
        /// <summary>
        /// 查詢暫存區
        /// </summary>
        /// <param name="SiteNo"></param>
        /// <param name="SupdId"></param>
        /// <param name="PaperNo"></param>
        /// <returns></returns>
        public DataTable searchTempArea(string SiteNo, string SupdId, string PaperNo)
        {
            DataTable CateIdList = new DataTable();

            Hashtable ht1 = new Hashtable();
            ht1.Add("@Step", "6");
            ht1.Add("@vendor_no", SupdId);
            ht1.Add("@Site_no", SiteNo);
            ht1.Add("@No1", PaperNo);
            Hashtable ht2 = new Hashtable();
            DataSet ds = IO.SqlSp(DBlink, "盤點_產生盤點結果表", ht1, ref ht2);
            CateIdList = ds.Tables[0];

            return CateIdList;
        }


        /// <summary>
        /// 產生暫存區
        /// </summary>
        /// <param name="SiteNo"></param>
        /// <param name="SupdId"></param>
        /// <returns></returns>
        public bool creTempArea(string SiteNo, string SupdId, string PaperNo)
        {
            bool IsOK = false;
            DataTable CateIdList = new DataTable();

            Hashtable ht1 = new Hashtable();
            ht1.Add("@Step", "3");
            ht1.Add("@vendor_no", SupdId);
            ht1.Add("@Site_no", SiteNo);
            ht1.Add("@No1", PaperNo);
            Hashtable ht2 = new Hashtable();
            DataSet ds = IO.SqlSp(DBlink, "盤點_產生盤點結果表", ht1, ref ht2);
            CateIdList = ds.Tables[0];

            if (CateIdList.Rows[0][0].ToString() == "true")
                IsOK = true;

            return IsOK;
        }
        #endregion

        #region 產生盤點單
        /// <summary>
        /// 產生盤點單
        /// </summary>
        /// <param name="SiteNo"></param>
        /// <param name="SupdId"></param>
        /// <param name="inv_date"></param>
        /// <returns></returns>
        public void creInvLog(string SiteNo, string SupdId, string inv_date)
        {
            DataTable CateIdList = new DataTable();

            Hashtable ht1 = new Hashtable();
            ht1.Add("@Step", "4");
            ht1.Add("@vendor_no", SupdId);
            ht1.Add("@Site_no", SiteNo);
            ht1.Add("@No1", "0");
            Hashtable ht2 = new Hashtable();
            DataSet ds = IO.SqlSp(DBlink, "盤點_產生盤點結果表", ht1, ref ht2);
            CateIdList = ds.Tables[0];
        }
        #endregion
    }
}
