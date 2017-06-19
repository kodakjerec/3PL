using System.Data;
using _3PL_LIB;
using System.Collections;
using System;

namespace _3PL_DAO
{
    public partial class _3PL_Quotation_DAO
    {
        public string Login_Server = "3PL";
        DB_IO IO = new DB_IO();

        #region 查詢
        /// <summary>
        /// 取得報價單單頭
        /// </summary>
        /// <param name="I_qthe_seq">單據序號</param>
        /// <returns></returns>
        public DataTable GetQuotationList(int I_qthe_seq, UserInf UI)
        {
            DataTable QuotationList = new DataTable();

            string Sql_cmd =
            @"Select top 50 a.*,[UIStatus]='Unchanged',c.[Step],c.[StatusName],c.[OkbuttonName],c.[NobuttonName],
            供應商=a.S_qthe_SupdId+','+b.alias,
            建單人=a.S_qthe_CreateId+','+d.WorkName,
            IsOk=dbo.[fn3PL_GetSignOffPermission](1,I_qthe_Status,@UserID)
            from [3PL_QuotationHead] a
            left join v_Supplier b on left(a.S_qthe_SupdId,4)=b.ID
            inner join SignOff_Status c on a.I_qthe_Status=c.[Status] and c.pageType=1
            left join EmpInf d on a.S_qthe_CreateId=d.WorkId
            where I_qthe_seq=@I_qthe_seq";
            Hashtable ht1 = new Hashtable();
            ht1.Add("@I_qthe_seq", I_qthe_seq);
            ht1.Add("@UserID", UI.UserID);

            DataSet ds = IO.SqlQuery(Login_Server, Sql_cmd, ht1);
            QuotationList = ds.Tables[0];

            //加入primaryKey
            DataColumn[] keys = new DataColumn[1];
            keys[0] = QuotationList.Columns["S_qthe_PLNO"];
            QuotationList.PrimaryKey = keys;

            return QuotationList;
        }

        /// <summary>
        /// 取得報價單單頭
        /// </summary>
        /// <param name="SupdId">供應商代號</param>
        /// <param name="SiteNo">倉別</param>
        /// <param name="I_qthe_seq">單據序號</param>
        /// <param name="DateS">報價期間起日</param>
        /// <param name="DateE">報價期間迄日</param>
        /// <returns></returns>
        public DataTable GetQuotationList(string SupdId, string SiteNo, int I_qthe_seq, string DateS, string DateE, string PLNO, UserInf UI, bool bol_Chk_ShowStatusIsZero)
        {
            DataTable QuotationList = new DataTable();

            string Sql_cmd =
            @"Select top 50 a.*,[UIStatus]='Unchanged',c.[Step],c.[StatusName],c.[OkbuttonName],c.[NobuttonName],
            供應商=a.S_qthe_SupdId+','+b.alias,
            建單人=a.S_qthe_CreateId+','+d.WorkName,
            IsOk=dbo.[fn3PL_GetSignOffPermission](1,I_qthe_Status,@UserID)
            from [3PL_QuotationHead] a
            left join v_Supplier b on left(a.S_qthe_SupdId,4)=b.ID
            inner join SignOff_Status c on a.I_qthe_Status=c.[Status] and c.pageType=1
            left join EmpInf d on a.S_qthe_CreateId=d.WorkId
            where 1=1 ";
            Hashtable ht1 = new Hashtable();
            ht1.Add("@UserID", UI.UserID);
            if (SiteNo == "DC01")
            {
                Sql_cmd += " and Flag_DC1='1' ";
            }
            else if (SiteNo == "DC02")
            {
                Sql_cmd += " and Flag_DC2='1' ";
            }
            else if (SiteNo == "DC03")
            {
                Sql_cmd += " and Flag_DC3='1' ";
            }
            //有選擇TypeId
            if (SupdId != "")
            {
                Sql_cmd += " and S_qthe_SupdId=@SupdId";
                ht1.Add("@SupdId", SupdId);
            }
            if (I_qthe_seq > 0)
            {
                Sql_cmd += " and I_qthe_seq=@I_qthe_seq";
                ht1.Add("@I_qthe_seq", I_qthe_seq);
            }
            if (DateS != "")
            {
                Sql_cmd += " and D_qthe_ContractS>=@D_qthe_ContractS";
                ht1.Add("@D_qthe_ContractS", DateS);
            }
            if (DateE != "")
            {
                Sql_cmd += " and D_qthe_ContractE>=@D_qthe_ContractE";
                ht1.Add("@D_qthe_ContractE", DateE);
            }
            if (PLNO != "")
            {
                Sql_cmd += " and S_qthe_PLNO like @PLNO+'%' ";
                ht1.Add("@PLNO", PLNO);
            }
            //有選擇bol_Chk_ShowStatusIsZero
            if (bol_Chk_ShowStatusIsZero == false)
            {
                Sql_cmd += " and a.[I_qthe_Status]>0";
            }
            Sql_cmd += " order by S_qthe_PLNO DESC, S_qthe_SupdId";
            DataSet ds = IO.SqlQuery(Login_Server, Sql_cmd, ht1);
            QuotationList = ds.Tables[0];

            //加入primaryKey
            DataColumn[] keys = new DataColumn[1];
            keys[0] = QuotationList.Columns["S_qthe_PLNO"];
            QuotationList.PrimaryKey = keys;

            return QuotationList;
        }

        /// <summary>
        /// 取得報價單單頭_廠商用印
        /// </summary>
        /// <param name="SupdId"></param>
        /// <param name="SiteNo"></param>
        /// <param name="I_qthe_seq"></param>
        /// <param name="DateS"></param>
        /// <param name="DateE"></param>
        /// <param name="PLNO"></param>
        /// <param name="UI"></param>
        /// <param name="bol_Chk_ShowStatusIsZero"></param>
        /// <param name="StampFilter"></param>
        /// <returns></returns>
        public DataTable GetQuotationList(string SupdId, string SiteNo, int I_qthe_seq, string DateS, string DateE, string PLNO, UserInf UI, bool bol_Chk_ShowStatusIsZero, string StampFilter, string QuotationStatusType)
        {
            DataTable QuotationList = new DataTable();

            string Sql_cmd =
            @"Select top 50 a.*,[UIStatus]='Unchanged',c.[Step],c.[StatusName],c.[OkbuttonName],c.[NobuttonName],
            供應商=a.S_qthe_SupdId+','+b.alias,
            建單人=a.S_qthe_CreateId+','+d.WorkName,
            IsOk=dbo.[fn3PL_GetSignOffPermission](1,I_qthe_Status,@UserID)
            from [3PL_QuotationHead] a
            left join v_Supplier b on left(a.S_qthe_SupdId,4)=b.ID
            inner join SignOff_Status c on a.I_qthe_Status=c.[Status] and c.pageType=1
            left join EmpInf d on a.S_qthe_CreateId=d.WorkId
            where 1=1 ";
            Hashtable ht1 = new Hashtable();
            ht1.Add("@UserID", UI.UserID);
            if (SiteNo == "DC01")
            {
                Sql_cmd += " and Flag_DC1='1' ";
            }
            else if (SiteNo == "DC02")
            {
                Sql_cmd += " and Flag_DC2='1' ";
            }
            else if (SiteNo == "DC03")
            {
                Sql_cmd += " and Flag_DC3='1' ";
            }
            //有選擇TypeId
            if (SupdId != "")
            {
                Sql_cmd += " and S_qthe_SupdId=@SupdId";
                ht1.Add("@SupdId", SupdId);
            }
            if (I_qthe_seq > 0)
            {
                Sql_cmd += " and I_qthe_seq=@I_qthe_seq";
                ht1.Add("@I_qthe_seq", I_qthe_seq);
            }
            if (DateS != "")
            {
                Sql_cmd += " and D_qthe_ContractS>=@D_qthe_ContractS";
                ht1.Add("@D_qthe_ContractS", DateS);
            }
            if (DateE != "")
            {
                Sql_cmd += " and D_qthe_ContractE>=@D_qthe_ContractE";
                ht1.Add("@D_qthe_ContractE", DateE);
            }
            if (PLNO != "")
            {
                Sql_cmd += " and S_qthe_PLNO like @PLNO+'%' ";
                ht1.Add("@PLNO", PLNO);
            }
            //有選擇bol_Chk_ShowStatusIsZero
            if (bol_Chk_ShowStatusIsZero == false)
            {
                Sql_cmd += " and a.[I_qthe_Status]>0";
            }
            //廠商用印_未用印
            if (StampFilter == "1")
            {
                Sql_cmd += " and a.[D_qthe_StampDate] is null";
            }
            //廠商用印_已用印
            if (StampFilter == "2")
            {
                Sql_cmd += " and a.[D_qthe_StampDate] is not null";
            }
            //報價狀態_未完成
            if (QuotationStatusType == "1")
            {
                Sql_cmd += " and (a.[I_qthe_Status]>0 and a.[I_qthe_Status]<20)";
            }
            //報價狀態_已完成
            if (QuotationStatusType == "2")
            {
                Sql_cmd += " and a.[I_qthe_Status]=20";
            }
            Sql_cmd += " order by S_qthe_PLNO DESC, S_qthe_SupdId";
            DataSet ds = IO.SqlQuery(Login_Server, Sql_cmd, ht1);
            QuotationList = ds.Tables[0];

            //加入primaryKey
            DataColumn[] keys = new DataColumn[1];
            keys[0] = QuotationList.Columns["S_qthe_PLNO"];
            QuotationList.PrimaryKey = keys;

            return QuotationList;
        }


        /// <summary>
        /// 取得報價單明細
        /// </summary>
        /// <param name="I_qthe_PLNO">報價單單號</param>
        /// <returns></returns>
        public DataTable GetQuotationDetail(string I_qthe_PLNO)
        {
            DataTable QuotationDetail = new DataTable();

            string Sql_cmd =
            @"  SELECT a.*,b.S_bsda_FieldName,c.S_bcse_CostName,c.S_bcse_DollarUnit,[UIStatus]='Unchanged'
                FROM [3PL_QuotationDetail] a
                left join [3PL_BaseData] b on a.I_qtde_TypeId=b.S_bsda_FieldId and b.S_bsda_CateId='TypeId'
                left join [3PL_BaseCostSet] c on a.I_qtde_bcseSeq=c.I_bcse_Seq
                where I_qtde_DelFlag=0 and S_qtde_qthePLNO=@PLNO
                order by I_qtde_Detailseq";
            Hashtable ht1 = new Hashtable();
            ht1.Add("@PLNO", I_qthe_PLNO);
            DataSet ds = IO.SqlQuery(Login_Server, Sql_cmd, ht1);
            QuotationDetail = ds.Tables[0];

            //加入primaryKey
            DataColumn[] keys = new DataColumn[2];
            keys[0] = QuotationDetail.Columns["I_qtde_seq"];
            keys[1] = QuotationDetail.Columns["I_qtde_Detailseq"];
            QuotationDetail.PrimaryKey = keys;

            return QuotationDetail;
        }
        #endregion

        #region 新增/修改/刪除

        /// <summary>
        /// 新增/修改/刪除 單據表頭
        /// </summary>
        /// <param name="DBlink"></param>
        /// <param name="Head"></param>
        /// <param name="Detail"></param>
        /// <param name="PageNo"></param>
        /// <returns></returns>
        public bool InsertQuotation(string DBlink, DataTable Head, DataTable Detail, string PageNo, string UserID)
        {
            int SuccessCount = 0,ModifyCount=0;
            string RowStatus = "";

            #region 表頭
            foreach (DataRow dr in Head.Rows)
            {
                RowStatus = dr["UIStatus"].ToString();

                if (RowStatus == "Added")
                {
                    dr["S_qthe_PLNO"] = PageNo;
                    SuccessCount += Head_Insert(DBlink, dr);
                    ModifyCount++;
                }
                else if (RowStatus == "Modified")
                {
                    SuccessCount += Head_Update(DBlink, dr);
                    ModifyCount++;
                }
                else if (RowStatus == "Deleted")
                {
                    SuccessCount += Head_Delete(DBlink, dr);
                    ModifyCount++;
                }
            }
            #endregion

            #region 明細

            foreach (DataRow dr in Detail.Rows)
            {
                RowStatus = dr["UIStatus"].ToString();

                if (RowStatus == "Added")
                {
                    dr["S_qtde_qthePLNO"] = PageNo;
                    SuccessCount += Detail_Insert(DBlink, dr);
                    ModifyCount++;
                }
                else if (RowStatus == "Modified")
                {
                    SuccessCount += Detail_Update(DBlink, dr);
                    ModifyCount++;
                }
                else if (RowStatus == "Deleted")
                {
                    SuccessCount += Detail_Delete(DBlink, dr);
                    ModifyCount++;
                }

            }
            #endregion
            if (ModifyCount == 0)
                return true;
            else
            {
                if (SuccessCount > 0)
                    return true;
                else
                    return false;
            }
        }

        #region 單據表頭的新刪修
        private int Head_Insert(string DBlink, DataRow dr)
        {
            //1.新增報價單
            int SuccessCount = 0, SuccessCount_head = 0;
            string Insertcmd_Head = @"Insert Into [3PL_QuotationHead](S_qthe_PLNO,S_qthe_SupdId,S_qthe_SiteNo,I_qthe_IsSpecial,
            D_qthe_ContractS,D_qthe_ContractE,S_qthe_Memo,S_qthe_CreateId,S_qthe_UpdId,I_qthe_Status,Flag_DC1,Flag_DC2,Flag_DC3,S_qthe_TEL,S_qthe_BOSS,S_qthe_FAX)
            values(@S_qthe_PLNO,@S_qthe_SupdId,@S_qthe_SiteNo,@I_qthe_IsSpecial,
            @D_qthe_ContractS,@D_qthe_ContractE,@S_qthe_Memo,@S_qthe_CreateId,@S_qthe_UpdId,10,@Flag_DC1,@Flag_DC2,@Flag_DC3,@S_qthe_TEL,@S_qthe_BOSS,@S_qthe_FAX)";

            Hashtable ht1 = new Hashtable();
            ht1.Add("@S_qthe_PLNO", dr["S_qthe_PLNO"]);
            ht1.Add("@S_qthe_SupdId", dr["S_qthe_SupdId"]);
            ht1.Add("@S_qthe_SiteNo", "DC01");
            ht1.Add("@I_qthe_IsSpecial", dr["I_qthe_IsSpecial"]);
            ht1.Add("@D_qthe_ContractS", DateTime.Parse(dr["D_qthe_ContractS"].ToString()).ToString("yyyy/MM/dd"));
            ht1.Add("@D_qthe_ContractE", DateTime.Parse(dr["D_qthe_ContractE"].ToString()).ToString("yyyy/MM/dd"));
            ht1.Add("@S_qthe_Memo", dr["S_qthe_Memo"]);
            ht1.Add("@S_qthe_CreateId", dr["S_qthe_CreateId"]);
            ht1.Add("@S_qthe_UpdId", dr["S_qthe_UpdId"]);
            ht1.Add("@Flag_DC1", dr["Flag_DC1"]);
            ht1.Add("@Flag_DC2", dr["Flag_DC2"]);
            ht1.Add("@Flag_DC3", dr["Flag_DC3"]);
            ht1.Add("@S_qthe_TEL", dr["S_qthe_TEL"]);
            ht1.Add("@S_qthe_BOSS", dr["S_qthe_BOSS"]);
            ht1.Add("@S_qthe_FAX", dr["S_qthe_FAX"]);

            IO.SqlUpdate(Login_Server, Insertcmd_Head, ht1, ref SuccessCount_head);
            SuccessCount += SuccessCount_head;

            if (SuccessCount > 0)
            { 
                Hashtable ht2=new Hashtable();
                Hashtable ht3=new Hashtable();
                ht2.Add("@PLNO",dr["S_qthe_PLNO"]);
                ht2.Add("@UserId",dr["S_qthe_CreateId"]);
                ht2.Add("@PageType",1);
                IO.SqlSp(Login_Server, "[sp3PL_AddSignOff]", ht2, ref ht3);
            }

            return SuccessCount;
        }
        private int Head_Update(string DBlink, DataRow dr)
        {
            int SuccessCount = 0, SuccessCount_head = 0;
            string Updcmd_head =
            @"Update [3PL_QuotationHead] set D_qthe_ContractS=@D_qthe_ContractS,D_qthe_ContractE=@D_qthe_ContractE,S_qthe_Memo=@S_qthe_Memo,I_qthe_IsSpecial=@I_qthe_IsSpecial,S_qthe_UpdId=@S_qthe_UpdId,
            Flag_DC1=@Flag_DC1,Flag_DC2=@Flag_DC2, Flag_DC3=@Flag_DC3,S_qthe_TEL=@S_qthe_TEL,S_qthe_BOSS=@S_qthe_BOSS,S_qthe_FAX=@S_qthe_FAX
            where I_qthe_seq=@I_qthe_seq";
            Hashtable ht1 = new Hashtable();
            ht1.Add("@I_qthe_IsSpecial", dr["I_qthe_IsSpecial"]);
            ht1.Add("@D_qthe_ContractS", DateTime.Parse(dr["D_qthe_ContractS"].ToString()).ToString("yyyy/MM/dd"));
            ht1.Add("@D_qthe_ContractE", DateTime.Parse(dr["D_qthe_ContractE"].ToString()).ToString("yyyy/MM/dd"));
            ht1.Add("@S_qthe_Memo", dr["S_qthe_Memo"]);
            ht1.Add("@S_qthe_UpdId", dr["S_qthe_UpdId"]);
            ht1.Add("@I_qthe_seq", dr["I_qthe_seq"]);
            ht1.Add("@Flag_DC1", dr["Flag_DC1"]);
            ht1.Add("@Flag_DC2", dr["Flag_DC2"]);
            ht1.Add("@Flag_DC3", dr["Flag_DC3"]);
            ht1.Add("@S_qthe_TEL", dr["S_qthe_TEL"]);
            ht1.Add("@S_qthe_BOSS", dr["S_qthe_BOSS"]);
            ht1.Add("@S_qthe_FAX", dr["S_qthe_FAX"]);

            IO.SqlUpdate(Login_Server, Updcmd_head, ht1, ref SuccessCount_head);
            SuccessCount += SuccessCount_head;

            return SuccessCount;
        }
        private int Head_Delete(string DBlink, DataRow dr)
        {
            int SuccessCount = 0, SuccessCount_head = 0;
            string Delcmd_head =
            @"Update [3PL_QuotationHead] 
            set I_qthe_Status=0, S_qthe_UpdId=@S_qthe_UpdId,Del_Memo=@Memo
            where I_qthe_seq=@I_qthe_seq";
            Hashtable ht1 = new Hashtable();
            ht1.Add("@I_qthe_DelFlag", dr["I_qthe_DelFlag"]);
            ht1.Add("@S_qthe_UpdId", dr["S_qthe_UpdId"]);
            ht1.Add("@I_qthe_seq", dr["I_qthe_seq"]);
            ht1.Add("@Memo", dr["Del_Memo"]);

            IO.SqlUpdate(Login_Server, Delcmd_head, ht1, ref SuccessCount_head);
            SuccessCount += SuccessCount_head;

            return SuccessCount;
        }
        #endregion

        #region 單據明細的新刪修
        private int Detail_Insert(string DBlink, DataRow dr)
        {
            int SuccessCount = 0, SuccessCount_Detail = 0;
            string Insertcmd_Detail = @"Insert Into [3PL_QuotationDetail](S_qtde_qthePLNO,I_qtde_Detailseq,I_qtde_TypeId
            ,I_qtde_bcseSeq,S_qtde_Memo,I_qtde_Price,I_qtde_IsBaseCost,S_qtde_PriceMemo,S_qtde_CreateId,S_qtde_UpdId,S_qtde_SiteNo)
            values(@S_qtde_qthePLNO,@I_qtde_Detailseq,@I_qtde_TypeId
            ,@I_qtde_bcseSeq,@S_qtde_Memo,@I_qtde_Price,@I_qtde_IsBaseCost,@S_qtde_PriceMemo,@S_qtde_CreateId,@S_qtde_UpdId,@S_qtde_SiteNo)";

            Hashtable ht1 = new Hashtable();
            ht1.Add("@S_qtde_qthePLNO", dr["S_qtde_qthePLNO"]);
            ht1.Add("@I_qtde_Detailseq", dr["I_qtde_Detailseq"]);
            ht1.Add("@I_qtde_TypeId", dr["I_qtde_TypeId"]);
            ht1.Add("@I_qtde_bcseSeq", dr["I_qtde_bcseSeq"]);
            ht1.Add("@S_qtde_Memo", dr["S_qtde_Memo"]);
            ht1.Add("@I_qtde_Price", dr["I_qtde_Price"]);
            ht1.Add("@I_qtde_IsBaseCost", dr["I_qtde_IsBaseCost"]);
            ht1.Add("@S_qtde_PriceMemo", dr["S_qtde_PriceMemo"]);
            ht1.Add("@S_qtde_CreateId", dr["S_qtde_CreateId"]);
            ht1.Add("@S_qtde_UpdId", dr["S_qtde_UpdId"]);
            ht1.Add("@S_qtde_Siteno", dr["S_qtde_Siteno"]);

            IO.SqlUpdate(Login_Server, Insertcmd_Detail, ht1, ref SuccessCount_Detail);
            SuccessCount += SuccessCount_Detail;

            return SuccessCount;
        }
        private int Detail_Update(string DBlink, DataRow dr)
        {
            int SuccessCount = 0, SuccessCount_Detail = 0;
            string Updcmd_Detail =
            @"Update [3PL_QuotationDetail] set I_qtde_Detailseq=@I_qtde_Detailseq,I_qtde_TypeId=@I_qtde_TypeId,
            I_qtde_bcseSeq=@I_qtde_bcseSeq,S_qtde_Memo=@S_qtde_Memo,I_qtde_Price=@I_qtde_Price,I_qtde_IsBaseCost=@I_qtde_IsBaseCost,
            S_qtde_PriceMemo=@S_qtde_PriceMemo,S_qtde_UpdId=@S_qtde_UpdId,S_qtde_Siteno=@S_qtde_Siteno
            where I_qtde_seq=@I_qtde_seq";

            Hashtable ht1 = new Hashtable();
            ht1.Add("@I_qtde_Detailseq", dr["I_qtde_Detailseq"]);
            ht1.Add("@I_qtde_TypeId", dr["I_qtde_TypeId"]);
            ht1.Add("@I_qtde_bcseSeq", dr["I_qtde_bcseSeq"]);
            ht1.Add("@S_qtde_Memo", dr["S_qtde_Memo"]);
            ht1.Add("@I_qtde_Price", dr["I_qtde_Price"]);
            ht1.Add("@I_qtde_IsBaseCost", dr["I_qtde_IsBaseCost"]);
            ht1.Add("@S_qtde_PriceMemo", dr["S_qtde_PriceMemo"]);
            ht1.Add("@S_qtde_UpdId", dr["S_qtde_UpdId"]);
            ht1.Add("@I_qtde_seq", dr["I_qtde_seq"]);
            ht1.Add("@S_qtde_Siteno", dr["S_qtde_Siteno"]);

            IO.SqlUpdate(Login_Server, Updcmd_Detail, ht1, ref SuccessCount_Detail);
            SuccessCount += SuccessCount_Detail;

            return SuccessCount;
        }
        private int Detail_Delete(string DBlink, DataRow dr)
        {
            //新增後又刪除的資料直接跳過 
            if (dr["I_qtde_seq"].ToString() == "0")
                return 1;

            int SuccessCount = 0, SuccessCount_Detail = 0;
            string Delcmd_Detail =
            @"Update [3PL_QuotationDetail]
            set I_qtde_DelFlag=@I_qtde_DelFlag, S_qtde_UpdId=@S_qtde_UpdId 
            where I_qtde_seq=@I_qtde_seq";

            Hashtable ht1 = new Hashtable();
            ht1.Add("@I_qtde_DelFlag", dr["I_qtde_DelFlag"]);
            ht1.Add("@S_qtde_UpdId", dr["S_qtde_UpdId"]);
            ht1.Add("@I_qtde_seq", dr["I_qtde_seq"]);

            IO.SqlUpdate(Login_Server, Delcmd_Detail, ht1, ref SuccessCount_Detail);
            SuccessCount += SuccessCount_Detail;

            return SuccessCount;
        }
        #endregion

        /// <summary>
        /// 設定廠商用印時間
        /// </summary>
        /// <param name="DBlink"></param>
        /// <param name="PageNo"></param>
        /// <param name="UserID"></param>
        /// <returns></returns>
        public bool UpdateStamp(string DBlink, string PageNo, string Date, string UserID)
        {
            int SuccessCount_head=0;

            string Delcmd_head =
            @"Update [3PL_QuotationHead] 
            set D_qthe_StampDate=@Date, S_qthe_UpdId=@S_qthe_UpdId
            where S_qthe_PLNO=@PageNo";
            Hashtable ht1 = new Hashtable();
            ht1.Add("@Date", Date);
            ht1.Add("@S_qthe_UpdId", UserID);
            ht1.Add("@PageNo", PageNo);

            IO.SqlUpdate(Login_Server, Delcmd_head, ht1, ref SuccessCount_head);
            if (SuccessCount_head > 0)
                return true;
            else
                return false;
        }
        #endregion

        #region 列印
        public DataTable sp3PL_CreateQuotationPDF(string I_qthe_PLNO)
        {
            DataTable QuotationDetail = new DataTable();

            string Sql_cmd =
            @"sp3PL_CreateQuotationPDF";
            Hashtable ht1 = new Hashtable();
            Hashtable ht2 = new Hashtable();
            ht1.Add("@PLNO", I_qthe_PLNO);
            DataSet ds = IO.SqlSp(Login_Server, Sql_cmd, ht1,ref ht2);
            QuotationDetail = ds.Tables[0];

            return QuotationDetail;
        }
        #endregion
    }
}
