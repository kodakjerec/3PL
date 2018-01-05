using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using _3PL_LIB;
using System.Data;
using System.Collections;

namespace _3PL_DAO
{
    public partial class _3PL_ToExcel
    {
        public string Login_Server = "3PL";
        DB_IO IO = new DB_IO();

        #region 3PL_ToExcel001
        /// <summary>
        /// 取得報價單
        /// </summary>
        /// <returns></returns>
        public DataTable GetQuotationList(string SupdId, string SiteNo, int I_qthe_seq, string DateS, string DateE, string PLNO, UserInf UI, bool bol_Chk_ShowStatusIsZero)
        {
            DataTable QuotationList = new DataTable();

            string Sql_cmd =
            @"Select
            報價單號,
            供應商編號,
            [倉別(寄倉倉別)],
            報價日期起=convert(varchar,報價日期起),  
            報價日期迄=convert(varchar,報價日期迄),  
            建單人員=b.S_qthe_CreateId+', '+ISNULL(c.WorkName,''),
            建單時間=b.D_qthe_CreateDate,
            派工類別,
            計價費用,
            單價,
            單位,
            其他議價單=CASE b.I_qthe_IsSpecial WHEN 1 THEN 'V' ELSE '' END,
            備註=b.S_qthe_Memo,
            明細備註,
            狀態,
            廠商用印日期=convert(varchar,廠商用印日期)
            FROM (
            select
            報價單號,
            供應商編號=供應商代號+','+供應商名稱,
            [倉別(寄倉倉別)]=倉別,
            報價日期起=合約起日,
            報價日期迄=合約迄日,
            派工類別=報價主類別,
            計價費用=計價費用名稱,
            單價,
            單位=計價單位,
            明細備註=報價明細memo,
            狀態=報價單狀態,
            廠商用印日期=ISNULL(廠商用印日期,'')
            from v_報價實收明細表
            group by 報價單號, 倉別,供應商代號,供應商名稱,合約起日,合約迄日,報價主類別,計價費用名稱,單價,計價單位,報價明細memo,報價單狀態,廠商用印日期) a
            inner join [3PL_QuotationHead] b with(nolock) on a.報價單號=b.S_qthe_PLNO
            left join EmpInf c with(nolock) on b.S_qthe_CreateId=c.WorkId
            where 1=1 ";
            Hashtable ht1 = new Hashtable();
            ht1.Add("@UserID", UI.UserID);
            if (SiteNo == "DC01")
            {
                Sql_cmd += " and b.Flag_DC1='1' ";
            }
            else if (SiteNo == "DC02")
            {
                Sql_cmd += " and b.Flag_DC2='1' ";
            }
            else if (SiteNo == "DC03")
            {
                Sql_cmd += " and b.Flag_DC3='1' ";
            }
            //有選擇TypeId
            if (SupdId != "")
            {
                Sql_cmd += " and b.S_qthe_SupdId=@SupdId";
                ht1.Add("@SupdId", SupdId);
            }
            if (I_qthe_seq > 0)
            {
                Sql_cmd += " and b.I_qthe_seq=@I_qthe_seq";
                ht1.Add("@I_qthe_seq", I_qthe_seq);
            }
            if (DateS != "")
            {
                Sql_cmd += " and b.D_qthe_ContractS>=@D_qthe_ContractS";
                ht1.Add("@D_qthe_ContractS", DateS);
            }
            if (DateE != "")
            {
                Sql_cmd += " and b.D_qthe_ContractE>=@D_qthe_ContractE";
                ht1.Add("@D_qthe_ContractE", DateE);
            }
            if (PLNO != "")
            {
                Sql_cmd += " and b.S_qthe_PLNO like @PLNO+'%' ";
                ht1.Add("@PLNO", PLNO);
            }
            //有選擇bol_Chk_ShowStatusIsZero
            if (bol_Chk_ShowStatusIsZero == false)
            {
                Sql_cmd += " and b.[I_qthe_Status]>0";
            }
            Sql_cmd+=" order by a.報價單號, a.[倉別(寄倉倉別)]";

            DataSet ds = IO.SqlQuery(Login_Server, Sql_cmd, ht1);
            QuotationList = ds.Tables[0];

            return QuotationList;
        }

        /// <summary>
        /// 取得派工單
        /// </summary>
        /// <returns></returns>
        public DataTable GetAssignList(string SupdId, string SiteNo, string Wk_ID, UserInf UI, bool bol_Chk_ShowStatusIsZero, string Bdate, string Edate)
        {
            DataTable QuotationList = new DataTable();

            string Sql_cmd =
            @"select 
            派工單號, 
			派工人員=ISNULL(b.CrtUser+', '+c.WorkName,''),
            派工單狀態, 
            供應商代號,
            供應商名稱,
            倉別, 
            派工日期=convert(varchar,派工日期),
            預定完工日=convert(varchar,預定完工日),
            實際完工日=convert(varchar,實際完工日),
            報價主類別, 
            計價費用名稱, 
            派工數量, 
            實際數量, 
            計價單位, 
            PO單號, 貨號, 單價, 金額, 派工單備註
            from v_報價實收明細表 a
			left join AssignHead b on a.派工單號=b.Wk_Id
			left join EmpInf c with(nolock) on b.CrtUser=c.WorkId
            where 1=1";
            Hashtable ht1 = new Hashtable();
            ht1.Add("@UserID", UI.UserID);
            //有選擇SiteNo
            if (SiteNo != "")
            {
                Sql_cmd += " and a.倉別=@SiteNo";
                ht1.Add("@SiteNo", SiteNo);
            }
            //有選擇SupdId
            if (SupdId != "")
            {
                Sql_cmd += " and a.供應商代號=@SupdId";
                ht1.Add("@SupdId", SupdId);
            }
            //有選擇Wk_Id
            if (Wk_ID != "")
            {
                Sql_cmd += " and 派工單號 like @Wk_ID+'%' ";
                ht1.Add("@Wk_ID", Wk_ID);
            }
            //有選擇bol_Chk_ShowStatusIsZero
            if (bol_Chk_ShowStatusIsZero == false)
            {
                Sql_cmd += " and a.[Status]>0";
            }
            if (Bdate != "")
            {
                Sql_cmd += " and a.預定完工日>= @Bdate ";
                ht1.Add("@Bdate", Bdate);
            }
            if (Edate != "")
            {
                Sql_cmd += " and a.預定完工日<= @Edate ";
                ht1.Add("@Edate", Edate);
            }
            Sql_cmd += " order by a.派工單號";

            DataSet ds = IO.SqlQuery(Login_Server, Sql_cmd, ht1);
            QuotationList = ds.Tables[0];

            return QuotationList;
        }

        /// <summary>
        /// 取得成本及收入明細表
        /// </summary>
        /// <returns></returns>
        public DataTable GetCostIncome(string SupdId, string SiteNo, string Wk_Id, UserInf UI, string Bdate, string Edate)
        {
            DataTable QuotationList = new DataTable();

            string Sql_cmd =
            @"select
                [帳務月份]=LEFT(convert(varchar,實際完工日,112),6)
                ,供應商代號
                ,供應商名稱
                ,派工單號
                ,倉別
                ,會計科目代號
                ,會計科目名稱
                ,實際數量=SUM(實際數量)
                ,金額=SUM(金額)
                ,應收=SUM(實際數量*金額)
            from v_報價實收明細表 with(nolock)
            where [Status]=20";
            Hashtable ht1 = new Hashtable();
            ht1.Add("@UserID", UI.UserID);
            //有選擇SiteNo
            if (SiteNo != "")
            {
                Sql_cmd += " and 倉別=@SiteNo";
                ht1.Add("@SiteNo", SiteNo);
            }
            //有選擇SupdId
            if (SupdId != "")
            {
                Sql_cmd += " and 供應商代號=@SupdId";
                ht1.Add("@SupdId", SupdId);
            }
            //有選擇Wk_Id
            if (Wk_Id != "")
            {
                Sql_cmd += " and 報價單號 like @Wk_ID+'%' ";
                ht1.Add("@Wk_ID", Wk_Id);
            }
            if (Bdate != "")
            {
                Sql_cmd += " and 實際完工日>= @Bdate ";
                ht1.Add("@Bdate", Bdate);
            }
            if (Edate != "")
            {
                Sql_cmd += " and 實際完工日<= @Edate ";
                ht1.Add("@Edate", Edate);
            }
            Sql_cmd += 
             @" group by
                    LEFT(convert(varchar, 實際完工日, 112), 6)
	                ,供應商代號
	                ,供應商名稱
	                ,派工單號
	                ,倉別
	                ,會計科目代號
	                ,會計科目名稱
                order by
                    [帳務月份]
	                ,供應商代號
	                ,派工單號";

            DataSet ds = IO.SqlQuery(Login_Server, Sql_cmd, ht1);
            QuotationList = ds.Tables[0];

            return QuotationList;
        }
        #endregion
    }
}
