using System;
using System.Collections.Generic;
using System.Linq;
using _3PL_LIB;
using System.Data;
using System.Collections;
using System.ComponentModel;

namespace _3PL_DAO
{
    /*
     * 投貨收現專用
     * 
     * 
     * 
     */
    public class colOrigin
    {
        public string PO單號 { get; set; }
        public string 發票日期 { get; set; }
        public string 服務代碼 { get; set; }
        public string 服務說明 { get; set; }
        public string 會計科目 { get; set; }
        public string 交易單號 { get; set; }
        public string 發票號碼 { get; set; }
        public string 倉別代碼 { get; set; }
        public string 倉別名稱 { get; set; }
        public string 廠商代碼 { get; set; }
        public string 廠商名稱 { get; set; }
        public int 現場輸入數量 { get; set; }
        public string 單價 { get; set; }
        public int 含稅金額 { get; set; }
        public string 驗收單號 { get; set; }
        public int 元驗收總箱數 { get; set; }
        public string 元廠商代號 { get; set; }
        public string 元廠商名稱 { get; set; }
        public string 進貨類型 { get; set; }
        public int PO總箱數 { get; set; }
        public int PO總PCS { get; set; }
        public int 元驗收總PCS { get; set; }
    }
    public class colDetail
    {
        public string 發票號碼 { get; set; }
        public string 倉別名稱 { get; set; }
        public string 發票日期 { get; set; }
        public string 進貨類型 { get; set; }
        public string PO單號 { get; set; }
        public string 驗收單號 { get; set; }
        public string 廠商代碼 { get; set; }
        public string 廠商名稱 { get; set; }
        public string 廠商個人 { get; set; }
        public int PO量 { get; set; }
        public int PO總箱數 { get; set; }
        public int 驗收量 { get; set; }
        public int 驗收總箱數 { get; set; }
        public int 發票數量 { get; set; }
        public string 單價 { get; set; }
        public int 金額 { get; set; }
    }
    public class colTotal
    {
        public string 發票日期 { get; set; }
        public string 廠商代碼 { get; set; }
        public string 廠商名稱 { get; set; }
        public int 驗收總箱數 { get; set; }
        public int 發票數量 { get; set; }
        public string 單價 { get; set; }
        public int 金額 { get; set; }
    }

    public class _3PL_ToExcel_SendBox_DAO
    {
        public string Login_Server_SQ208 = "SQ208";
        public string Login_Server_LGDC = "LGDC";
        DB_IO IO = new DB_IO();

        /// <summary>
        /// 投貨收現明細
        /// </summary>
        /// <returns></returns>
        public DataTable GetDetail(string SiteNo, string DateS, string DateE, string SupdId, string PLNO, string UNIVNO)
        {
            DataTable dt0 = GetSQ208(SiteNo, DateS, DateE, SupdId, PLNO, UNIVNO);
            DataTable dt1 = GetLGDC(SiteNo, DateS, DateE, SupdId, PLNO);

            List<colOrigin> colQuery = CombineSQ208AndLGDC(dt0, dt1);
            List<colDetail> query = (from a in colQuery
                                     orderby a.發票日期, a.發票號碼, a.PO單號
                                     select new colDetail
                                     {
                                         發票號碼 = a.發票號碼,
                                         倉別名稱 = a.倉別名稱,
                                         發票日期 = a.發票日期,
                                         進貨類型 = a.進貨類型,
                                         PO單號 = a.PO單號,
                                         驗收單號 = a.驗收單號,
                                         廠商代碼 = a.元廠商代號,
                                         廠商名稱 = a.元廠商名稱,
                                         廠商個人 = a.廠商名稱,
                                         PO量 = a.PO總PCS,
                                         PO總箱數 = a.PO總箱數,
                                         驗收量 = a.元驗收總PCS,
                                         驗收總箱數 = a.元驗收總箱數,
                                         發票數量 = a.現場輸入數量,
                                         單價 = a.單價,
                                         金額 = a.含稅金額
                                     }).ToList<colDetail>();

            DataTable dt2 = ConvertToDataTable(query);
            return dt2;
        }

        /// <summary>
        /// 投貨收現總表
        /// </summary>
        /// <param name="SupdId"></param>
        /// <param name="SiteNo"></param>
        /// <param name="I_qthe_seq"></param>
        /// <param name="DateS"></param>
        /// <param name="DateE"></param>
        /// <param name="PLNO"></param>
        /// <param name="UI"></param>
        /// <param name="bol_Chk_ShowStatusIsZero"></param>
        /// <returns></returns>
        public DataTable GetTotal(string SiteNo, string DateS, string DateE, string SupdId)
        {
            DataTable dt0 = GetSQ208(SiteNo, DateS, DateE, SupdId, "", "");
            DataTable dt1 = GetLGDC(SiteNo, DateS, DateE, SupdId, "");

            List<colOrigin> colQuery = CombineSQ208AndLGDC(dt0, dt1);
            List<colTotal> query = (from a in colQuery
                                    group a by new { a.發票日期, a.元廠商代號, a.元廠商名稱, a.單價 } into b
                                    orderby b.Key.發票日期, b.Key.元廠商代號, b.Key.元廠商名稱
                                    select new colTotal
                                    {
                                        發票日期 = b.Key.發票日期,
                                        廠商代碼 = b.Key.元廠商代號,
                                        廠商名稱 = b.Key.元廠商名稱,
                                        驗收總箱數 = b.Sum(a => a.元驗收總箱數),
                                        發票數量 = b.Sum(a => a.現場輸入數量),
                                        單價 = b.Key.單價,
                                        金額 = b.Sum(a => a.含稅金額)
                                    }
                        ).ToList<colTotal>();
            DataTable dt2 = ConvertToDataTable(query);
            return dt2;
        }

        /// <summary>
        /// oracle查詢
        /// </summary>
        /// <returns></returns>
        private DataTable GetSQ208(string SiteNo, string DateS, string DateE, string SupdId, string PLNO, string UNIVNO)
        {
            switch (SiteNo)
            {
                case "DC01":
                    SiteNo = "711000"; break;
                case "DC02":
                    SiteNo = "712000"; break;
                case "DC03":
                    SiteNo = "713000"; break;
            }

            string querycmd = @"where a.cmpid = ''HD'' and a.rectp <> ''9''
  and a.TrxUid  not in (select recuid from dfgt10 
                        where recuid = a.trxuid and IsRecover = ''Y'')
 and a.depid=c.depid and a.sbjid=b.sbjid ";

            Hashtable ht1 = new Hashtable();
            querycmd += "AND a.RGSTDATE Between ''" + DateS + "'' and ''" + DateE + "'' ";

            if (SiteNo != string.Empty)
            {
                querycmd += @"and a.depid=''" + SiteNo + @"'' ";
            }
            if (SupdId != string.Empty)
            {
                querycmd += @"AND a.SBJID LIKE ''" + @SupdId + @"%'' ";
            }
            if (PLNO != string.Empty)
            {
                querycmd += @"AND a.PORDNO LIKE ''" + @PLNO + @"%'' ";
            }
            if (UNIVNO != string.Empty)
            {
                querycmd += @"AND a.UINVNO LIKE ''" + @UNIVNO + @"%'' ";
            }

            string sqlcmd = @"select *
from OPENQUERY(WHOLE,
'select a.rgstdate
		,a.SVCID
		,a.SVCDESC
		,a.ACCODE
		,a.TRXNO
		,a.uinvno
		,a.depid
		,c.sname as depname
		,a.pordno
		,a.sbjid
		,b.sbjabb
		,a.txqty
		,a.uprc
		,a.lttlamt
from dfgt1a a, bspm10 b, bdpm10 c "
                + querycmd
                + @"')";

            DataTable dt1 = IO.SqlQuery(Login_Server_SQ208, sqlcmd, ht1).Tables[0];

            return dt1;
        }

        /// <summary>
        /// LGDC
        /// </summary>
        /// <returns></returns>
        private DataTable GetLGDC(string SiteNo, string DateS, string DateE, string SupdId, string PLNO)
        {
            switch (SiteNo)
            {
                case "DC01":
                    SiteNo = "DC1"; break;
                case "DC02":
                    SiteNo = "DC2"; break;
                case "DC03":
                    SiteNo = "DC3"; break;
            }

            string querycmd_RC_Detail = "where 1=1 and arrive_date BETWEEN @DateS AND @DateE ";
            string querycmd_EDI = "where 1=1 and b.RCV_DATE BETWEEN @DateS AND @DateE ";

            Hashtable ht1 = new Hashtable();
            ht1.Add("@DateS", DateS);
            ht1.Add("@DateE", DateE);

            if (SiteNo != string.Empty)
            {
                querycmd_RC_Detail += "and site_no=@SiteNo ";
                querycmd_EDI += "and BUYER_ID=@SiteNo ";
                ht1.Add("@SiteNo", SiteNo);
            }
            if (SupdId != string.Empty)
            {
                querycmd_RC_Detail += "and vendor_no like @SupdId+'%' ";
                querycmd_EDI += "and SELLER_ID like @SupdId+'%' ";
                ht1.Add("@SupdId", SupdId);
            }
            if (PLNO != string.Empty)
            {
                querycmd_RC_Detail += "and po_no like @PLNO+'%' ";
                querycmd_EDI += "and a.ID like @PLNO+'%' ";
                ht1.Add("@PLNO", PLNO);
            }

            string sqlcmd = @"Select 
	po_no
	, rv_no
	, vendor_no
	, b.ALIAS
	, DOCK_MODE=(CASE c.DOCK_MODE WHEN 0 THEN '寄庫' WHEN 1 THEN '越庫' ELSE '' END )
	, cs_qty
	, c.cs_qty_ori
    , a.PCSQTY
    , c.PCSQty_ori
FROM
(
select po_no, rv_no, vendor_no, cs_qty=SUM(cs_qty) ,PCSQTY=SUM(rcv_qty)
from RC_Detail  with(nolock)"
+ querycmd_RC_Detail
+ @"
group by po_no, rv_no , vendor_no
) a
LEFT JOIN 
	DRP.dbo.DRP_SUPPLIER b with(nolock)
on 
	a.vendor_no=b.ID
LEFT JOIN
	(
select a.ID, b.DOCK_MODE, cs_qty_ori=SUM(a.QTY/a.CS_QTY), PCSQty_ori=SUM(a.QTY)
from 
	EDI.dbo.EDI_CRP_PO_LINE a with(nolock)
INNER JOIN
	EDI.dbo.EDI_CRP_PO_HEADER b with(nolock)
ON
	a.ID=b.ID "
+ querycmd_EDI
+ @"
group by a.ID, b.DOCK_MODE
) c ON a.po_no=c.ID";

            DataTable dt1 = IO.SqlQuery(Login_Server_LGDC, sqlcmd, ht1).Tables[0];

            return dt1;
        }

        /// <summary>
        /// 結合oracle和LGDC的souce
        /// </summary>
        /// <param name="dt0"></param>
        /// <param name="dt1"></param>
        /// <returns></returns>
        private List<colOrigin> CombineSQ208AndLGDC(DataTable dt0, DataTable dt1)
        {
            List<colOrigin> colQuery = new List<colOrigin>();

            foreach (DataRow dr0 in dt0.Rows)
            {
                colOrigin obj = new colOrigin();

                obj.PO單號 = dr0["PORDNO"].ToString();
                obj.發票日期 = DateTime.Parse(dr0["RGSTDATE"].ToString()).ToString("yyyy/MM/dd");
                obj.服務代碼 = dr0["SVCID"].ToString();
                obj.服務說明 = dr0["SVCDESC"].ToString();
                obj.會計科目 = dr0["ACCODE"].ToString();
                obj.交易單號 = dr0["TRXNO"].ToString();
                obj.發票號碼 = dr0["UINVNO"].ToString();
                obj.倉別代碼 = dr0["DEPID"].ToString();
                obj.倉別名稱 = dr0["DEPNAME"].ToString();
                obj.廠商代碼 = dr0["SBJID"].ToString();
                obj.廠商名稱 = dr0["SBJABB"].ToString();
                obj.現場輸入數量 = Convert.ToInt32(dr0["TXQTY"]);
                obj.單價 = dr0["UPRC"].ToString();
                obj.含稅金額 = Convert.ToInt32(dr0["LTTLAMT"]);

                foreach (DataRow dr1 in dt1.Rows)
                {
                    string a = dr0["PORDNO"].ToString().Replace("/r","").Replace("/n","");
                    string b = dr1["po_no"].ToString();
                }
                var dr1s = (from a in dt1.AsEnumerable()
                            where a.Field<string>("po_no") == dr0["PORDNO"].ToString()
                            select a);

                if (dr1s.Count() > 0)
                {
                    DataRow dr1 = dr1s.First();
                    obj.驗收單號 = dr1["rv_no"].ToString();
                    obj.元驗收總箱數 = Convert.ToInt32(dr1["cs_qty"]);
                    obj.元廠商代號 = dr1["vendor_no"].ToString();
                    obj.元廠商名稱 = dr1["ALIAS"].ToString();
                    obj.進貨類型 = dr1["DOCK_MODE"].ToString();
                    obj.PO總箱數 = Convert.ToInt32(dr1["cs_qty_ori"]);
                    obj.PO總PCS = Convert.ToInt32(dr1["PCSQty_ori"]);
                    obj.元驗收總PCS = Convert.ToInt32(dr1["PCSQty"]);
                }

                colQuery.Add(obj);
            }

            return colQuery;
        }

        /// <summary>
        /// List to DataTable
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <returns></returns>
        public DataTable ConvertToDataTable<T>(IList<T> data)
        {
            PropertyDescriptorCollection properties =
               TypeDescriptor.GetProperties(typeof(T));
            DataTable table = new DataTable();
            foreach (PropertyDescriptor prop in properties)
                table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
            foreach (T item in data)
            {
                DataRow row = table.NewRow();
                foreach (PropertyDescriptor prop in properties)
                    row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
                table.Rows.Add(row);
            }
            return table;

        }
    }
}
