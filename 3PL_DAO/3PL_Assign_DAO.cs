using System.Data;
using _3PL_LIB;
using System.Collections;
using System;

namespace _3PL_DAO
{
    public partial class _3PL_Assign_DAO
    {
        public string Login_Server = "3PL";
        DB_IO IO = new DB_IO();
        _3PL_CommonQuery _3PLCQ = new _3PL_CommonQuery();

        #region 派工單本體

        #region 查詢
        /// <summary>
        /// 取得派工單單頭
        /// </summary>
        /// <param name="SupdId">供應商</param>
        /// <param name="SiteNo">倉別</param>
        /// <returns></returns>
        public DataTable GetAssignList(string SupdId, string SiteNo, string Wk_ID, UserInf UI, string BDate, string EDate, string AssignStatusList)
        {
            DataTable QuotationList = new DataTable();

            string Sql_cmd =
            @"Select top 50 Wk_Id, FreeId, Wk_Date, Wk_Class, DC, EtaDate, ActDate, Wk_Unit, SupID=SupID+','+b.ALIAS,
            a.Memo,a.[Status],Wk_ClassName=c.S_bsda_FieldName,a.CrtUser,CrtUserName=d.WorkName,a.UpdUser,a.Wk_UnitName,f.ClassName,
            IsOK=dbo.[fn3PL_GetSignOffPermission](2,a.[Status],@UserID),c1.[StatusName],c1.[Step],c1.[NobuttonName],c1.[OkbuttonName],DCName=left(c2.S_bsda_FieldName,2)
            from AssignHead a with(nolock)
            left join v_Supplier b with(nolock) on a.SupID=b.ID
			inner join [3PL_BaseData] c with(nolock) on a.Wk_Class=c.S_bsda_FieldId and c.S_bsda_CateId='TypeId'
            inner join SignOff_Status c1 with(nolock) on a.[Status]=c1.[Status] and c1.pageType=2
			left join EmpInf d with(nolock) on a.CrtUser=d.WorkId
            left join ClassInf f with(nolock) on a.Wk_Unit=f.ClassId
            inner join [3PL_BaseData] c2 with(nolock) on a.DC=c2.S_bsda_FieldId and c2.S_bsda_CateId='SiteNo'
            where 1=1 ";
            Sql_cmd += _3PLCQ.GetDCList(UI.DCList, "[DC]", 0);
            Hashtable ht1 = new Hashtable();
            ht1.Add("@UserID", UI.UserID);
            //有選擇SiteNo
            if (SiteNo != "")
            {
                Sql_cmd += " and DC=@SiteNo";
                ht1.Add("@SiteNo", SiteNo);
            }
            //有選擇SupdId
            if (SupdId != "")
            {
                Sql_cmd += " and SupID=@SupdId";
                ht1.Add("@SupdId", SupdId);
            }
            //有選擇Wk_Id
            if (Wk_ID != "")
            {
                Sql_cmd += " and Wk_ID like @Wk_ID+'%'";
                ht1.Add("@Wk_ID", Wk_ID);
            }
            if (BDate.Length > 0)
            {
                Sql_cmd += " and EtaDate>=@BDate ";
                ht1.Add("@BDate", BDate);
            }
            if (EDate.Length > 0)
            {
                Sql_cmd += " and EtaDate<=@EDate ";
                ht1.Add("@EDate", EDate);
            }
            Sql_cmd += " and a.[Status] in (" + AssignStatusList + ")";

            Sql_cmd += " order by FreeId DESC, Wk_Date DESC, Wk_Id";
            DataSet ds = IO.SqlQuery(Login_Server, Sql_cmd, ht1);
            QuotationList = ds.Tables[0];

            //加入primaryKey
            DataColumn[] keys = new DataColumn[1];
            keys[0] = QuotationList.Columns["Wk_Id"];
            QuotationList.PrimaryKey = keys;

            return QuotationList;
        }
        public DataTable GetAssignList(string Wk_ID, UserInf UI)
        {
            DataTable QuotationList = new DataTable();

            string Sql_cmd =
            @"Select top 50 Wk_Id, FreeId, Wk_Date, Wk_Class, DC, EtaDate, ActDate, Wk_Unit, SupID=SupID+','+b.ALIAS,
            a.Memo,a.[Status],Wk_ClassName=c.S_bsda_FieldName,a.CrtUser,CrtUserName=d.WorkName,a.UpdUser,a.Wk_UnitName,f.ClassName,
            IsOK=dbo.[fn3PL_GetSignOffPermission](2,a.[Status],@UserID),c1.[StatusName],c1.[Step],c1.[NobuttonName],c1.[OkbuttonName]
            from AssignHead a with(nolock)
            left join v_Supplier b with(nolock) on a.SupID=b.ID
			inner join [3PL_BaseData] c with(nolock) on a.Wk_Class=c.S_bsda_FieldId and c.S_bsda_CateId='TypeId'
            inner join SignOff_Status c1 with(nolock) on a.[Status]=c1.[Status] and c1.pageType=2
			left join EmpInf d with(nolock) on a.CrtUser=d.WorkId
            left join ClassInf f with(nolock) on a.Wk_Unit=f.ClassId
            where Wk_ID=@Wk_ID ";
            Hashtable ht1 = new Hashtable();
            ht1.Add("@Wk_ID", Wk_ID);
            ht1.Add("@UserID", UI.UserID);

            DataSet ds = IO.SqlQuery(Login_Server, Sql_cmd, ht1);
            QuotationList = ds.Tables[0];

            //加入primaryKey
            DataColumn[] keys = new DataColumn[1];
            keys[0] = QuotationList.Columns["Wk_Id"];
            QuotationList.PrimaryKey = keys;

            return QuotationList;
        }

        /// <summary>
        /// 取得派工單明細
        /// </summary>
        /// <param name="Wk_Id">派工單單號</param>
        /// <returns></returns>
        public DataTable GetAssignDetail(string Wk_Id)
        {
            DataTable QuotationDetail = new DataTable();

            string Sql_cmd =
            @"select Seq=ROW_NUMBER() OVER(PARTITION by Wk_Id order by Wk_Id,Sn),Wk_Id,Sn,Wk_Class,Wk_ClassNm,Qty,RealQty,RealQty_WMS,Unit,UpdUser,PONO,itemno,DC,[UIStatus]='Unchanged'
            from AssignClass with(nolock)
            where Wk_Id=@Wk_Id";
            Hashtable ht1 = new Hashtable();
            ht1.Add("@Wk_Id", Wk_Id);
            DataSet ds = IO.SqlQuery(Login_Server, Sql_cmd, ht1);
            QuotationDetail = ds.Tables[0];

            //加入primaryKey
            DataColumn[] keys = new DataColumn[2];
            keys[0] = QuotationDetail.Columns["Wk_Id"];
            keys[1] = QuotationDetail.Columns["Seq"];
            QuotationDetail.PrimaryKey = keys;

            return QuotationDetail;
        }
        /// <summary>
        /// 取得PO驗收實績量
        /// </summary>
        /// <param name="site_no"></param>
        /// <param name="Po_No"></param>
        /// <param name="Item_No"></param>
        /// <returns></returns>
        public string AssignDetail_TakingRealQty(string site_no, string Po_No, string Item_No, string Unit)
        {
            string ReturnQty = "";
            DataTable dt = new DataTable();

            string Sql_cmd =
            @"select TQty=ISNULL(sum(final.Tqty),0), TBox=ISNULL(sum(final.TBox),0), TPallet=ISNULL(Ceiling(sum(final.Tpallet)),0)
            from (
            select Tqty=b.L_reci_takeqty,
	            TBox=b.L_reci_takeqty/ISNULL(c.I_merp_1qty,1),
	            Tpallet=b.L_reci_takeqty/(ISNULL(c.I_merp_1qty,1)*c.I_merp_pacti*c.I_merp_pachi*1.00),
				c.I_merp_1qty
            from reci_head a with(nolock)
            inner join reci_item b with(nolock)
            on a.L_rech_id=b.L_reci_rechid
            left join mer_package c with(nolock) on b.L_reci_merdsysno=c.L_merp_merdsysno and c.I_merp_boxflag=1
            where a.S_rech_erpid=@Po_No
	            and b.S_reci_merdid like '%'+@Item_No+'%'
            ) final";
            Hashtable ht1 = new Hashtable();
            ht1.Add("@Po_No", Po_No);
            ht1.Add("@Item_No", Item_No);
            DataSet ds = IO.SqlQuery(site_no, Sql_cmd, ht1);
            if (ds.Tables.Count > 0)
            {
                int i = GetDetailNeedQty(ds.Tables[0].Rows[0], Unit);
                if (i != 0)
                    ReturnQty = i.ToString();
                else
                    ReturnQty = "";
            }
            else
            {
                ReturnQty = "";
            }

            return ReturnQty;
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
        public bool InsertQuotation(string DBlink, DataTable Head, DataTable Detail, string UserID)
        {
            int SuccessCount = 0;

            #region 表頭
            foreach (DataRow dr in Head.Rows)
            {
                SuccessCount += Head_Update(DBlink, dr);
            }
            #endregion

            #region 明細

            foreach (DataRow dr in Detail.Rows)
            {
                if (dr["UIStatus"].ToString() == "Added")
                {
                    SuccessCount += Detail_Insert(DBlink, dr);
                }
                else if (dr["UIStatus"].ToString() == "Modified")
                {
                    SuccessCount += Detail_Update(DBlink, dr);
                }
                else if (dr["UIStatus"].ToString() == "Deleted")
                {
                    SuccessCount += Detail_Delete(DBlink, dr);
                }
            }
            #endregion

            if (SuccessCount > 0)
                return true;
            else
                return false;
        }

        /// <summary>
        /// 單據表頭的更新
        /// </summary>
        /// <param name="DBlink"></param>
        /// <param name="dr"></param>
        /// <returns></returns>
        private int Head_Update(string DBlink, DataRow dr)
        {
            int SuccessCount = 0, SuccessCount_head = 0;
            string Updcmd_head =
            @"Update [AssignHead] set EtaDate=@EtaDate,Wk_Unit=@Wk_Unit, UpdUser=@UpdUser, UpdDate=getdate(), Memo=@Memo
              where Wk_Id=@Wk_Id";
            Hashtable ht1 = new Hashtable();
            ht1.Add("@EtaDate", DateTime.Parse(dr["EtaDate"].ToString()).ToString("yyyy/MM/dd"));
            ht1.Add("@Wk_Unit", dr["Wk_Unit"]);
            ht1.Add("@UpdUser", dr["UpdUser"]);
            ht1.Add("@Memo", dr["Memo"]);
            ht1.Add("@Wk_Id", dr["Wk_Id"]);

            IO.SqlUpdate(Login_Server, Updcmd_head, ht1, ref SuccessCount_head);
            SuccessCount += SuccessCount_head;

            return SuccessCount;
        }

        /// <summary>
        /// 單據明細的更新
        /// </summary>
        /// <param name="DBlink"></param>
        /// <param name="dr"></param>
        /// <returns></returns>
        private int Detail_Update(string DBlink, DataRow dr)
        {
            int SuccessCount = 0, SuccessCount_Detail = 0;
            string Updcmd_Detail =
            @"Update [AssignClass] set Qty=@Qty,RealQty=@RealQty,RealQty_WMS=@RealQty_WMS,UpdUser=@UpdUser, UpdDate=getdate()
            where Sn=@Sn";

            Hashtable ht1 = new Hashtable();
            ht1.Add("@Qty", dr["Qty"]);
            ht1.Add("@RealQty", dr["RealQty"]);
            ht1.Add("@UpdUser", dr["UpdUser"]);
            ht1.Add("@Sn", dr["Sn"]);
            ht1.Add("@RealQty_WMS", dr["RealQty_WMS"]);

            IO.SqlUpdate(Login_Server, Updcmd_Detail, ht1, ref SuccessCount_Detail);
            SuccessCount += SuccessCount_Detail;

            return SuccessCount;
        }

        /// <summary>
        /// 單據明細的新增
        /// </summary>
        /// <param name="DBlink"></param>
        /// <param name="dr"></param>
        /// <returns></returns>
        private int Detail_Insert(string DBlink, DataRow dr)
        {
            int SuccessCount = 0, SuccessCount_Detail = 0;
            string Updcmd_Detail =
            @"Insert Into [AssignClass](Wk_Id,Wk_Class,Wk_ClassNm,Qty,RealQty,Unit,CrtUser,CrtDate,UpdUser,UpdDate,DC,Wk_Unit,PONO,itemno)
            values(@Wk_Id,@Wk_Class,@Wk_ClassNm,@Qty,NULL,@Unit,@CrtUser,getdate(),NULL,NULL,NULL,NULL,@PONO,@itemno)";

            Hashtable ht1 = new Hashtable();
            ht1.Add("@Wk_Id", dr["Wk_Id"]);
            ht1.Add("@Wk_Class", dr["Wk_Class"]);
            ht1.Add("@Wk_ClassNm", dr["Wk_ClassNm"]);
            ht1.Add("@Qty", dr["Qty"]);
            ht1.Add("@Unit", dr["Unit"]);
            ht1.Add("@CrtUser", dr["UpdUser"]);
            ht1.Add("@PONO", dr["PONO"]);
            ht1.Add("@itemno", dr["itemno"]);

            IO.SqlUpdate(Login_Server, Updcmd_Detail, ht1, ref SuccessCount_Detail);
            SuccessCount += SuccessCount_Detail;

            return SuccessCount;
        }

        /// <summary>
        /// 單據明細的刪除
        /// </summary>
        /// <param name="DBlink"></param>
        /// <param name="dr"></param>
        /// <returns></returns>
        private int Detail_Delete(string DBlink, DataRow dr)
        {
            int SuccessCount = 0, SuccessCount_Detail = 0;
            string Updcmd_Detail =
            @"Delete from AssignClass where Sn=@Sn";

            Hashtable ht1 = new Hashtable();
            ht1.Add("@Sn", dr["Sn"]);

            IO.SqlUpdate(Login_Server, Updcmd_Detail, ht1, ref SuccessCount_Detail);
            SuccessCount += SuccessCount_Detail;

            return SuccessCount;
        }

        /// <summary>
        /// 依據計價單位和Datarow, 回傳派工數量
        /// </summary>
        /// <param name="dr">資料列</param>
        /// <param name="Unit">計價單位</param>
        /// <returns>派工數量</returns>
        public int GetDetailNeedQty(DataRow dr, string Unit)
        {
            int strTotqty = 0;
            int TQty = Convert.ToInt32(dr[0]),
                    TBox = Convert.ToInt32(dr[1]),
                    TPallet = Convert.ToInt32(dr[2]);

            switch (Unit)
            {
                case "箱":
                    strTotqty = TBox; break;
                case "板":
                    strTotqty = TPallet; break;
                case "張":
                    strTotqty = TBox * 2; break;
                default:
                    strTotqty = TQty; break;
            }

            return strTotqty;
        }
        #endregion

        #endregion

        #region 檢查
        /// <summary>
        /// 檢查有無填寫派工數量
        /// </summary>
        /// <param name="PLNO"></param>
        /// <returns></returns>
        public bool CheckAssignClassStep1(string PLNO)
        {
            bool IsOK = false;

            string Sql_cmd =
            @"select Cnt=COUNT(1)
            from AssignClass with(nolock)
            where Wk_Id=@PLNO
	            and Qty is null";
            Hashtable ht1 = new Hashtable();
            ht1.Add("@PLNO", PLNO);
            DataSet ds = IO.SqlQuery(Login_Server, Sql_cmd, ht1);
            DataTable dt = ds.Tables[0];
            if (dt.Rows[0][0].ToString() == "0")
                IsOK = true;

            return IsOK;
        }

        /// <summary>
        /// 檢查有無填寫完工數量
        /// </summary>
        /// <param name="PLNO"></param>
        /// <returns></returns>
        public bool CheckAssignClassStep4(string PLNO)
        {
            bool IsOK = false;

            string Sql_cmd =
            @"select Cnt=COUNT(1)
            from AssignClass with(nolock)
            where Wk_Id=@PLNO
	            and RealQty is null";
            Hashtable ht1 = new Hashtable();
            ht1.Add("@PLNO", PLNO);
            DataSet ds = IO.SqlQuery(Login_Server, Sql_cmd, ht1);
            DataTable dt = ds.Tables[0];
            if (dt.Rows[0][0].ToString() == "0")
                IsOK = true;

            return IsOK;
        }

        /// <summary>
        /// 檢查有無填寫工時
        /// </summary>
        /// <param name="PLNO"></param>
        /// <returns></returns>
        public bool CheckCostListStep1(string PLNO)
        {
            bool IsOK = false;

            string Sql_cmd =
            @"select Cnt=COUNT(1)
            from CostList with(nolock)
            where Wk_Id=@PLNO
	            and Total is null";
            Hashtable ht1 = new Hashtable();
            ht1.Add("@PLNO", PLNO);
            DataSet ds = IO.SqlQuery(Login_Server, Sql_cmd, ht1);
            DataTable dt = ds.Tables[0];
            if (dt.Rows[0][0].ToString() == "0")
                IsOK = true;

            return IsOK;
        }

        #endregion
    }
}
