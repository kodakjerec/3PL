using System.Data;
using _3PL_LIB;
using System.Collections;
using System;

namespace _3PL_DAO
{
    public partial class _3PL_CostList_DAO
    {
        public string Login_Server = "3PL";
        DB_IO IO = new DB_IO();

        #region 查詢
        /// <summary>
        /// 取得成本單單頭
        /// </summary>
        /// <param name="SupdId">供應商</param>
        /// <param name="SiteNo">倉別</param>
        /// <returns></returns>
        public DataTable GetCostList(string SupdId, string SiteNo, string Wk_ID, string UserID)
        {
            DataTable QuotationList = new DataTable();

            string Sql_cmd =
            @"Select Wk_Id, FreeId, Wk_Date, Wk_Class, DC, EtaDate, ActDate, Wk_Unit, SupID=SupID+','+b.ALIAS,
            a.Memo,a.[Status],Wk_ClassName=c.S_bsda_FieldName,CrtUserName=d.WorkName,a.UpdUser,a.Wk_UnitName,f.ClassName,
            IsOK=dbo.[fn3PL_GetSignOffPermission](3,a.[Status],@UserID),c1.[StatusName],c1.[Step],c1.[NobuttonName],c1.[OkbuttonName]
            from AssignHead a
            left join v_Supplier b on a.SupID=b.ID
			inner join [3PL_BaseData] c on a.Wk_Class=c.S_bsda_FieldId and c.S_bsda_CateId='TypeId'
            inner join SignOff_Status c1 on a.[Status]=c1.[Status] and c1.pageType=2
			left join EmpInf d on a.CrtUser=d.WorkId
            left join ClassInf f on a.Wk_Unit=f.ClassId
            where a.[status]>0 and DC=@SiteNo";
            Hashtable ht1 = new Hashtable();
            ht1.Add("@SiteNo", SiteNo);
            ht1.Add("@UserID", UserID);
            //有選擇TypeId
            if (SupdId != "")
            {
                Sql_cmd += " and a.SupID=@SupdId";
                ht1.Add("@SupdId", SupdId);
            }
            if (Wk_ID != "")
            {
                Sql_cmd += " and Wk_ID like @Wk_ID+'%' ";
                ht1.Add("@Wk_ID", Wk_ID);
            }
            Sql_cmd += " order by FreeId DESC, Wk_Id";
            DataSet ds = IO.SqlQuery(Login_Server, Sql_cmd, ht1);
            QuotationList = ds.Tables[0];

            //加入primaryKey
            DataColumn[] keys = new DataColumn[1];
            keys[0] = QuotationList.Columns["Wk_Id"];
            QuotationList.PrimaryKey = keys;

            return QuotationList;
        }
        public DataTable GetCostList(string Wk_ID, string UserID)
        {
            DataTable QuotationList = new DataTable();

            string Sql_cmd =
            @"Select Wk_Id, FreeId, Wk_Date, Wk_Class, DC, EtaDate, ActDate, Wk_Unit, SupID=SupID+','+b.ALIAS,
            a.Memo,a.[Status],Wk_ClassName=c.S_bsda_FieldName,CrtUserName=d.WorkName,a.UpdUser,a.Wk_UnitName,f.ClassName,
            IsOK=dbo.[fn3PL_GetSignOffPermission](3,a.[Status],@UserID),c1.[StatusName],c1.[Step],c1.[NobuttonName],c1.[OkbuttonName]
            from AssignHead a with(nolock)
            left join v_Supplier b with(nolock) on a.SupID=b.ID
			inner join [3PL_BaseData] c with(nolock) on a.Wk_Class=c.S_bsda_FieldId and c.S_bsda_CateId='TypeId'
            inner join SignOff_Status c1 with(nolock) on a.[Status]=c1.[Status] and c1.pageType=2
			left join EmpInf d with(nolock) on a.CrtUser=d.WorkId
            left join ClassInf f with(nolock) on a.Wk_Unit=f.ClassId
            where a.[Status]>0 and Wk_ID=@Wk_ID ";
            Hashtable ht1 = new Hashtable();
            ht1.Add("@Wk_ID", Wk_ID);
            ht1.Add("@UserID", UserID);

            DataSet ds = IO.SqlQuery(Login_Server, Sql_cmd, ht1);
            QuotationList = ds.Tables[0];

            //加入primaryKey
            DataColumn[] keys = new DataColumn[1];
            keys[0] = QuotationList.Columns["Wk_Id"];
            QuotationList.PrimaryKey = keys;

            return QuotationList;
        }

        /// <summary>
        /// 取得成本單明細
        /// </summary>
        /// <param name="Wk_Id">成本單單號</param>
        /// <returns></returns>
        public DataTable GetCostDetail(string Wk_Id)
        {
            DataTable QuotationDetail = new DataTable();

            string Sql_cmd =
            @"select
            a.Sn,
            a.Wk_Id,
            a.CostID,
            CostTypeName=a1.S_bsda_FieldName,
            a.CostSeq,
            a.CostName,
            a.WorkerNum,a.WorkHr,a.Total,TotalNumHr=a.WorkerNum*a.WorkHr,
            CostFree=Convert(real,a.CostFree),TotalNumHrFree=a.WorkerNum*ISNULL(a.WorkHr,1)*a.CostFree,
            UpdUser,
            a.UnitIDName
            from CostList a with(nolock)
            inner join [3PL_BaseData] a1  with(nolock)
            on a.CostID=a1.S_bsda_FieldId and a1.S_bsda_CateId='CostType'
            where Wk_Id=@Wk_Id
            order by a.CostID,a.CostSeq";
            Hashtable ht1 = new Hashtable();
            ht1.Add("@Wk_Id", Wk_Id);
            DataSet ds = IO.SqlQuery(Login_Server, Sql_cmd, ht1);
            QuotationDetail = ds.Tables[0];

            //加入primaryKey
            DataColumn[] keys = new DataColumn[3];
            keys[0] = QuotationDetail.Columns["Wk_Id"];
            keys[1] = QuotationDetail.Columns["Sn"];
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
        public bool InsertQuotation(string DBlink, DataTable Head, DataTable Detail, string UserID)
        {
            int SuccessCount = 0;

            #region 明細

            foreach (DataRow dr in Detail.Rows)
            {
                SuccessCount += Detail_Update(DBlink, dr);
            }
            #endregion

            if (SuccessCount > 0)
                return true;
            else
                return false;
        }

        #region 單據明細的新刪修
        private int Detail_Update(string DBlink, DataRow dr)
        {
            int SuccessCount = 0, SuccessCount_Detail = 0;
            string Updcmd_Detail =
            @"Update [CostList] set WorkerNum=@WorkerNum,WorkHr=@WorkHr,Total=@Total,UpdUser=@UpdUser, UpdDate=getdate()
            where Sn=@Sn";

            Hashtable ht1 = new Hashtable();
            ht1.Add("@WorkerNum", dr["WorkerNum"]);
            ht1.Add("@WorkHr", dr["WorkHr"]);
            ht1.Add("@Total", dr["Total"]);
            ht1.Add("@UpdUser", dr["UpdUser"]);
            ht1.Add("@Sn", dr["Sn"]);

            IO.SqlUpdate(Login_Server, Updcmd_Detail, ht1, ref SuccessCount_Detail);
            SuccessCount += SuccessCount_Detail;

            return SuccessCount;
        }
        #endregion

        #endregion
    }
}
