using System.Data;
using _3PL_LIB;
using System.Collections;
using System;

namespace _3PL_DAO
{
    public partial class _3PL_CommonQuery
    {
        DB_IO IO = new DB_IO();

        #region 頁面公用變數
        /// <summary>
        /// 派工單查詢
        /// </summary>
        /// <param name="sup_no"></param>
        /// <param name="site_no_select"></param>
        /// <param name="Wk_ID"></param>
        /// <param name="qthe_seq"></param>
        /// <returns></returns>
        public string Page_AssignQuery(string sup_no, string site_no_select, string Wk_ID, string qthe_seq, string Bdate, string Edate)
        {
            string VarString = "";
            VarString += sup_no + ",";
            VarString += site_no_select + ",";
            VarString += Wk_ID + ",";
            VarString += qthe_seq + ",";
            VarString += Bdate + ",";
            VarString += Edate;
            return VarString;
        }

        /// <summary>
        /// 報價單查詢
        /// </summary>
        /// <param name="sup_no"></param>
        /// <param name="site_no_select"></param>
        /// <param name="Bdate"></param>
        /// <param name="Edate"></param>
        /// <param name="PLNO"></param>
        /// <returns></returns>
        public string Page_QuotationQuery(string sup_no, string site_no_select, string Bdate, string Edate, string PLNO)
        {
            string VarString = "";
            VarString += sup_no + ",";
            VarString += site_no_select + ",";
            VarString += Bdate + ",";
            VarString += Edate + ",";
            VarString += PLNO;

            return VarString;
        }

        /// <summary>
        /// 調整單查詢
        /// </summary>
        /// <param name="sel_Adj_Type">單據類別</param>
        /// <param name="sel_Adj_Id">單號</param>
        /// <param name="sel_Adj_Seq">單號絕對序號</param>
        /// <returns></returns>
        public string Page_AdjustQuery(string sel_Adj_Type, string sel_Adj_Id)
        {
            string VarString = "";
            VarString += sel_Adj_Type + ",";
            VarString += sel_Adj_Id;

            return VarString;
        }
        #endregion

        #region 查詢
        /// <summary>
        /// 取得系統大類清單
        /// </summary>
        /// <param name="DBlink">連結資料庫</param>
        /// <param name="CateId">大類代號</param>
        /// <returns></returns>
        public DataTable GetFieldList(string DBlink, string CateId)
        {
            DataTable CateIdList = new DataTable();

            string Sql_cmd = @"Select S_bsda_CateId,S_bsda_CateName,S_bsda_FieldId, S_bsda_FieldName
                                    From [3PL_baseData] with(nolock)
                                   Where S_bsda_CateId=@CateId and I_bsda_DelFlag=0";
            Hashtable ht1 = new Hashtable();
            ht1.Add("@CateId", CateId);
            DataSet ds = IO.SqlQuery(DBlink, Sql_cmd, ht1);
            CateIdList = ds.Tables[0];

            return CateIdList;
        }

        /// <summary>
        /// 取得系統大類清單with倉別權限
        /// </summary>
        /// <param name="DBlink">連結資料庫</param>
        /// <param name="CateId">大類代號</param>
        /// <returns></returns>
        public DataTable GetFieldList(string DBlink, string CateId, UserInf UI)
        {
            DataTable CateIdList = new DataTable();

            string Sql_cmd = @"Select S_bsda_CateId,S_bsda_CateName,S_bsda_FieldId, S_bsda_FieldName
                                    From [3PL_baseData] with(nolock)
                                   Where S_bsda_CateId=@CateId and I_bsda_DelFlag=0";
            Sql_cmd += GetDCList(UI.DCList, "[S_bsda_FieldId]", 0);
            Hashtable ht1 = new Hashtable();
            ht1.Add("@CateId", CateId);
            DataSet ds = IO.SqlQuery(DBlink, Sql_cmd, ht1);
            CateIdList = ds.Tables[0];

            return CateIdList;
        }

        /// <summary>
        /// 取得供應商資料
        /// </summary>
        /// <param name="DBlink">連結資料庫</param>
        /// <param name="SelectType">0=全供應商代號 1=指定供應商詳細資料</param>
        /// <param name="SupdId">供應商代號(SelectType=1才需要)</param>
        /// <returns></returns>
        public DataTable GetSupdId(string DBlink, int SelectType, string SupdId, string SupdName)
        {
            DataTable SupdIdList = new DataTable();
            string Sql_cmd = "";
            DataSet ds = new DataSet();

            Sql_cmd = @"Select Distinct ID, ALIAS, Bl_No, BOSS, TEL, TEL2, FAX, MAIL, ADDRESS
                                    From v_Supplier";
            Hashtable ht1 = new Hashtable();
            ds = IO.SqlQuery(DBlink, Sql_cmd, ht1);
            SupdIdList = ds.Tables[0];

            return SupdIdList;
        }

        /// <summary>
        /// 取得會計科目
        /// </summary>
        /// <param name="DBlink">連結資料庫</param>
        /// <returns></returns>
        public DataTable GetAccId(string DBlink)
        {
            DataTable AccIdList = new DataTable();

            string Sql_cmd = @"Select I_Acci_seq,S_Acci_Id,S_Acci_Name=S_Acci_Id+','+S_Acci_name
                               From [3PL_baseAccounting]
                               where I_Acci_DelFlag=0";
            Hashtable ht1 = new Hashtable();
            DataSet ds = IO.SqlQuery(DBlink, Sql_cmd, ht1);
            AccIdList = ds.Tables[0];

            return AccIdList;
        }

        /// <summary>
        /// 取得最大單號
        /// </summary>
        /// <param name="DBlink">資料庫</param>
        /// <param name="PageType">單據類別 1:報價 2:派工 3:成本</param>
        /// <returns></returns>
        public string GetMaxPageNo(string DBlink, int PageType)
        {
            string ReturnNo = "";

            string Sql_cmd = @"select dbo.fn3PL_GetNo(@PageType,'')";
            Hashtable ht1 = new Hashtable();
            ht1.Add("@PageType", PageType);
            Hashtable ht2 = new Hashtable();
            DataSet ds = IO.SqlQuery(DBlink, Sql_cmd, ht1);
            if (ds.Tables[0].Rows.Count > 0)
                ReturnNo = ds.Tables[0].Rows[0][0].ToString();

            return ReturnNo;
        }

        /// <summary>
        /// 取得單位一覽表 
        /// </summary>
        /// <param name="DBlink"></param>
        /// <returns></returns>
        public DataTable GetClassIdList(string DBlink)
        {
            DataTable AccIdList = new DataTable();

            string Sql_cmd = @"Select ClassId,Classname From [ClassInf] with(nolock) ";
            Hashtable ht1 = new Hashtable();
            DataSet ds = IO.SqlQuery(DBlink, Sql_cmd, ht1);
            AccIdList = ds.Tables[0];

            return AccIdList;
        }

        /// <summary>
        /// 取得未完成報價單+派工單數量 
        /// </summary>
        /// <param name="DBlink"></param>
        /// <returns></returns>
        public DataTable GetNotOK_Counts(string DBlink, UserInf UI)
        {
            DataTable AccIdList = new DataTable();

            string Sql_cmd =
            @"DECLARE @CNT0 int=0,@CNT1 int=0,@CNT2 int=0

	            SELECT @CNT0=COUNT(1)
	              FROM [v_未完成報價單]
	               where [工號]=@UserId" + GetDCList(UI.DCList, "[寄倉倉別代號]", 1) + " GROUP BY 工號";
            Sql_cmd += @"
	            SELECT @CNT1=COUNT(1)
	              FROM [v_未完成派工單]
	               where [工號]=@UserId" + GetDCList(UI.DCList, "[寄倉倉別代號]", 1) + " GROUP BY 工號";
            Sql_cmd += @"
	            SELECT @CNT2=COUNT(1)
	              FROM [v_未完成調整單]
	               where [工號]=@UserId GROUP BY 工號";
            Sql_cmd += @"
	        select 'Quotation'=@CNT0,'Assign'=@CNT1,'Adjust'=@CNT2";
            Hashtable ht1 = new Hashtable();
            ht1.Add("@UserId", UI.UserID);
            DataSet ds = IO.SqlQuery(DBlink, Sql_cmd, ht1);
            AccIdList = ds.Tables[0];

            return AccIdList;
        }

        /// <summary>
        /// 取得未完成報價單一覽表 
        /// </summary>
        /// <param name="DBlink"></param>
        /// <returns></returns>
        public DataTable GetNotOKQuotation(string DBlink, UserInf UI)
        {
            DataTable AccIdList = new DataTable();

            string Sql_cmd = @"SELECT [報價單號]
                  ,[供應商代號]
                  ,[寄倉倉別]
                  ,[單據狀態]
                  ,[工號]
                  ,[I_qthe_IsSpecial]
                  ,[I_qthe_Status]
              FROM [v_未完成報價單] where [工號]=@UserId";
            Sql_cmd += GetDCList(UI.DCList, "[寄倉倉別代號]", 1);
            Sql_cmd += "order by [報價單號]";
            Hashtable ht1 = new Hashtable();
            ht1.Add("@UserId", UI.UserID);
            DataSet ds = IO.SqlQuery(DBlink, Sql_cmd, ht1);
            AccIdList = ds.Tables[0];

            return AccIdList;
        }

        /// <summary>
        /// 取得未完成派工單一覽表 
        /// </summary>
        /// <param name="DBlink"></param>
        /// <returns></returns>
        public DataTable GetNotOKAssign(string DBlink, UserInf UI)
        {
            DataTable AccIdList = new DataTable();

            string Sql_cmd = @"SELECT [派工單號]
                  ,[供應商代號]
                  ,[寄倉倉別]
                  ,[單據狀態]
                  ,[工號]
                  ,[Status]
                  ,[EtaDate]
              FROM [v_未完成派工單] where [工號]=@UserId";
            Sql_cmd += GetDCList(UI.DCList, "[寄倉倉別代號]", 0);
            Sql_cmd += "order by [派工單號]";
            Hashtable ht1 = new Hashtable();
            ht1.Add("@UserId", UI.UserID);
            DataSet ds = IO.SqlQuery(DBlink, Sql_cmd, ht1);
            AccIdList = ds.Tables[0];

            return AccIdList;
        }

        /// <summary>
        /// 取得未完成調整單一覽表
        /// </summary>
        /// <param name="DBlink"></param>
        /// <param name="UI"></param>
        /// <returns></returns>
        public DataTable GetNotOKAdjust(string DBlink, UserInf UI)
        {
            DataTable AccIdList = new DataTable();

            string Sql_cmd = @"SELECT [調整單號],[調整類別],[異動單號],[單據狀態]
              FROM [v_未完成調整單] where [工號]=@UserId ";
            Sql_cmd += "order by [調整單號]";
            Hashtable ht1 = new Hashtable();
            ht1.Add("@UserId", UI.UserID);
            DataSet ds = IO.SqlQuery(DBlink, Sql_cmd, ht1);
            AccIdList = ds.Tables[0];

            return AccIdList;
        }

        /// <summary>
        /// 取的使用者倉別的string,用在sql的查詢中
        /// </summary>
        /// <param name="dt1">倉別清單UI.Class</param>
        /// <param name="ColumnName">欄位名稱</param>
        /// <param name="Type">種類 0=查詢用'=', 1=查詢用like</param>
        /// <returns></returns>
        public string GetDCList(DataTable dt1, string ColumnName, int Type)
        {
            string DCList = "";

            foreach (DataRow dr in dt1.Rows)
            {
                if (DCList != "")
                    DCList += "or ";
                else
                    DCList += " and (";
                if (Type == 0)
                {
                    DCList += ColumnName + "='" + dr["DC"].ToString() + "' ";
                }
                else
                {
                    DCList += ColumnName + " like'%" + dr["DC"].ToString() + "%' ";
                }
            }
            if (DCList != "")
                DCList += ")";

            return DCList;
        }

        /// <summary>
        /// 查詢關帳日期
        /// </summary>
        /// <returns>string 關帳日期</returns>
        public string Addon_GetCloseData()
        {
            DataTable SignOffStatus = new DataTable();

            string Sql_cmd =
            @"Select S_bsda_FieldId from [3PL_BaseData] where S_bsda_CateId='CloseData'";
            Hashtable ht1 = new Hashtable();

            DataSet ds = IO.SqlQuery("3PL", Sql_cmd, ht1);
            SignOffStatus = ds.Tables[0];
            string CloseData = SignOffStatus.Rows[0][0].ToString();
            return CloseData;
        }

        /// <summary>
        /// 下拉式清單:派工單號狀態
        /// </summary>
        /// <returns></returns>
        public DataTable GetAssignStatusList()
        {
            DataTable dt1 = new DataTable();
            dt1.Columns.Add("Name", typeof(string));
            dt1.Columns.Add("Value", typeof(string));
            dt1.Rows.Add(new object[] { "全部", "0" });
            dt1.Rows.Add(new object[] { "未完成", "1" });
            dt1.Rows.Add(new object[] { "已完成", "2" });
            dt1.Rows.Add(new object[] { "作廢", "3" });
            return dt1;
        }

        /// <summary>
        /// 下拉式清單:QuotationStamp
        /// </summary>
        /// <returns></returns>
        public DataTable GetQuotationStampStatusList()
        {
            DataTable dt1 = new DataTable();
            dt1.Columns.Add("Name", typeof(string));
            dt1.Columns.Add("Value", typeof(string));
            dt1.Rows.Add(new object[] { "全部", "0" });
            dt1.Rows.Add(new object[] { "未用印", "1" });
            dt1.Rows.Add(new object[] { "已用印", "2" });
            return dt1;
        }

        /// <summary>
        /// 回傳FunList的URL編號
        /// </summary>
        /// <param name="FunId"></param>
        /// <param name="PgId"></param>
        /// <returns></returns>
        public string GetFunList_PgUrl(string FunId, string PgId)
        {
            DataTable dt_URL = new DataTable();

            string Sql_cmd =
            @"Select PgUrl from [FunList] where FunId=@FunId and PgId=@PgId ";
            Hashtable ht1 = new Hashtable();
            ht1.Add("@FunId", FunId);
            ht1.Add("@PgId", PgId);

            DataSet ds = IO.SqlQuery("3PL", Sql_cmd, ht1);
            dt_URL = ds.Tables[0];
            string URL = dt_URL.Rows[0][0].ToString();
            return URL;
        }
        #endregion

        #region LOG
        public void LOG_Insert(string DBlink, string UserID, string MenuID, string MenuName, string Active)
        {
            int Counts = 0;

            string Sql_cmd = @"Insert Into SYS_USERSLOG(userid, funid, funname, active, crt_date) values(@UserID,@MenuID, @MenuName, @Active, getdate())";
            Hashtable ht1 = new Hashtable();
            ht1.Add("@UserID", UserID);
            ht1.Add("@MenuID", MenuID);
            ht1.Add("@MenuName", MenuName);
            ht1.Add("@Active", Active);
            try
            {
                IO.SqlUpdate(DBlink, Sql_cmd, ht1, ref Counts);
            }
            catch
            {
            }

        }
        #endregion
    }
}
