using System.Data;
using _3PL_LIB;
using System.Collections;
using System;

#region 修改紀錄
//2015.03.05 其他議價單的簽核跑完後，不帶入主管姓名，改為帶入建單人
#endregion
namespace _3PL_DAO
{
    public partial class _3PL_SignOff_DAO
    {
        DB_IO IO = new DB_IO();

        #region 簽核基本狀態:查詢
        /// <summary>
        /// 簽核流程狀態一覽
        /// </summary>
        /// <param name="Login_Server">資料庫</param>
        /// <param name="PageType">單據類別 0-報價 1-派工 2-成本</param>
        /// <returns></returns>
        public DataTable GetSOStatus(string Login_Server, string PageType)
        {
            DataTable SignOffStatus = new DataTable();

            string Sql_cmd =
            @"Select * from SignOff_Status where PageType=@PageType order by Step";
            Hashtable ht1 = new Hashtable();
            ht1.Add("@PageType", PageType);

            DataSet ds = IO.SqlQuery(Login_Server, Sql_cmd, ht1);
            SignOffStatus = ds.Tables[0];

            //加入primaryKey
            DataColumn[] keys = new DataColumn[1];
            keys[0] = SignOffStatus.Columns["Step"];
            SignOffStatus.PrimaryKey = keys;

            return SignOffStatus;
        }
        public DataTable GetSOStatus(string Login_Server)
        {
            DataTable SignOffStatus = new DataTable();

            string Sql_cmd =
            @"Select * from SignOff_Status order by PageType, Step";
            Hashtable ht1 = new Hashtable();

            DataSet ds = IO.SqlQuery(Login_Server, Sql_cmd, ht1);
            SignOffStatus = ds.Tables[0];

            //加入primaryKey
            DataColumn[] keys = new DataColumn[2];
            keys[0] = SignOffStatus.Columns["PageType"];
            keys[1] = SignOffStatus.Columns["Step"];
            SignOffStatus.PrimaryKey = keys;

            return SignOffStatus;
        }

        /// <summary>
        /// 指定單據的簽核過程
        /// </summary>
        /// <param name="Login_Server">資料庫</param>
        /// <param name="PageType">單據類別 0-報價 1-派工 2-成本</param>
        /// <param name="PageNo">單號</param>
        /// <returns></returns>
        public DataTable GetSOLog(string Login_Server, string PageType, string PageNo)
        {
            DataTable SignOffLog = new DataTable();

            string Sql_cmd =
            @"Select a.[Status],b.StatusName,sofp_WorkId,sofp_Workname, sofp_updateDate,sofp_Reason
            ,sofp_IsOk
            ,IsOk=
            Case When sofp_IsOk='1' THEN b.okbuttonName 
                 WHEN sofp_IsOk='0' THEN b.NobuttonName 
                 WHEN sofp_IsOK='N' THEN '新增' 
                 WHEN sofp_IsOK='M' THEN '調整' END
            ,FinalStatusName=c.StatusName
            from SignOff_Log a
			left join SignOff_Status b on a.pagetype=b.PageType and a.[Status]=b.[Status]
            left join SignOff_Status c on a.pagetype=c.PageType and a.FinalStatus=c.[Status]
            where a.PageType=@PageType and a.PLNO=@pageNo order by sofp_updateDate DESC";
            Hashtable ht1 = new Hashtable();
            ht1.Add("@PageType", PageType);
            ht1.Add("@PageNo", PageNo);

            DataSet ds = IO.SqlQuery(Login_Server, Sql_cmd, ht1);
            SignOffLog = ds.Tables[0];

            //加入primaryKey
            DataColumn[] keys = new DataColumn[1];
            keys[0] = SignOffLog.Columns["sofp_updateDate"];
            SignOffLog.PrimaryKey = keys;

            return SignOffLog;
        }
        #endregion

        #region SignOffStatus.aspx 簽核流程設定
        /// <summary>
        /// 更新簽核設定
        /// </summary>
        /// <param name="Login_Server"></param>
        /// <param name="UserID"></param>
        /// <param name="dr"></param>
        public void SignOff_Update(string Login_Server, string UserID, DataRow dr)
        {
            int SuccessCount = 0;
            string Sql_cmd =
                @"Update SignOff_Status set StatusName=@StatusName, OkbuttonName=@OkbuttonName,NobuttonName=@NobuttonName where Sn=@Sn";

            Hashtable ht1 = new Hashtable();
            ht1.Add("@StatusName", dr["StatusName"]);
            ht1.Add("@OkbuttonName", dr["OkbuttonName"]);
            ht1.Add("@NobuttonName", dr["NobuttonName"]);
            ht1.Add("@Sn", dr["Sn"]);

            IO.SqlUpdate(Login_Server, Sql_cmd, ht1, ref SuccessCount);
        }
        #endregion

        #region SignOffPermission.aspx 權限新增頁面
        #region 查詢
        /// <summary>
        /// 取得權限一覽表
        /// </summary>
        /// <param name="Login_Server"></param>
        /// <returns></returns>
        public DataTable GetSOPermission(string Login_Server)
        {
            DataTable SignOffPermission = new DataTable();

            string Sql_cmd =
            @"SELECT b.Sn, a.PageName, a.StatusName
            ,Class=b.ClassId+','+ISNULL(c1.ClassName,'')
            ,Worker=b.WorkId+','+ISNULL(c2.WorkName,'')
            FROM SignOff_Status a with(nolock)
            inner join SignOff_Permission b with(nolock)
            on a.Sn=b.SignOffSn
            left join ClassInf c1 with(nolock) on b.ClassId=c1.ClassId
            left join EmpInf c2 with(nolock) on b.WorkId=c2.WorkId
            where a.[Status]>0
	        and (
                (a.PageType=1 and a.[Status]<(select max([Status]) from SignOff_Status where PageType=1))
	        or  (a.PageType=2 and a.[Status]<(select max([Status]) from SignOff_Status where PageType=2))
	        or  (a.PageType=3 and a.[Status]<(select max([Status]) from SignOff_Status where PageType=3))
            or  (a.PageType=4 and a.[Status]<(select max([Status]) from SignOff_Status where PageType=4))
            )
            order by a.PageType,a.Sn,b.Sn";
            Hashtable ht1 = new Hashtable();
            DataSet ds = IO.SqlQuery(Login_Server, Sql_cmd, ht1);
            SignOffPermission = ds.Tables[0];

            return SignOffPermission;
        }
        public DataTable GetSOPageType(string Login_Server)
        {
            DataTable SignOffPermission = new DataTable();

            string Sql_cmd =
            @"SELECT Distinct PageType,PageName=convert(varchar,PageType)+','+PageName from [SignOff_Status] with(nolock)";
            Hashtable ht1 = new Hashtable();
            DataSet ds = IO.SqlQuery(Login_Server, Sql_cmd, ht1);
            SignOffPermission = ds.Tables[0];

            return SignOffPermission;
        }
        public DataTable GetSOStatusName(string Login_Server, string PageType)
        {
            DataTable SignOffPermission = new DataTable();

            string Sql_cmd =
            @"SELECT Distinct Sn,StatusName=convert(varchar,Step)+','+StatusName from [SignOff_Status] with(nolock)
            where PageType=@PageType1
	            and Step>0
	            and Step<(select max(Step) from SignOff_Status where PageType=@PageType2)
            order by StatusName";
            Hashtable ht1 = new Hashtable();
            ht1.Add("@PageType1", PageType);
            ht1.Add("@PageType2", PageType);
            DataSet ds = IO.SqlQuery(Login_Server, Sql_cmd, ht1);
            SignOffPermission = ds.Tables[0];

            return SignOffPermission;
        }
        #endregion

        #region 新增/刪除
        public int AddPermission(string Login_Server, string SN, string ClassId, string ClassName, string WorkId, string WorkName)
        {
            int SuccessCountChild = 0;
            string Sql_cmd =
                    @"Insert into SignOff_Permission values(@Sn,@ClassId,@ClassName,@WorkId,@WorkName)";
            Hashtable ht_Agree = new Hashtable();
            ht_Agree.Add("@Sn", SN);
            ht_Agree.Add("@ClassId", ClassId);
            ht_Agree.Add("@ClassName", ClassName);
            ht_Agree.Add("@WorkId", WorkId);
            ht_Agree.Add("@WorkName", WorkName);
            IO.SqlUpdate(Login_Server, Sql_cmd, ht_Agree, ref SuccessCountChild);
            return SuccessCountChild;
        }
        public int DelPermission(string Login_Server, string SN)
        {
            int SuccessCountChild = 0;
            string Sql_cmd =
                    @"Delete from SignOff_Permission where Sn=@Sn";
            Hashtable ht_Agree = new Hashtable();
            ht_Agree.Add("@Sn", SN);
            IO.SqlUpdate(Login_Server, Sql_cmd, ht_Agree, ref SuccessCountChild);
            return SuccessCountChild;
        }
        #endregion
        #endregion

        #region 簽核過程特定步驟會用到的函數

        /// <summary>
        /// 報價單完成後，產生派工單,成本單 
        /// </summary>
        /// <param name="DBlink"></param>
        /// <param name="PLNO"></param>
        /// <param name="UserId"></param>
        /// <returns></returns>
        private int Head_Assign(string DBlink, string PLNO, string UserId)
        {
            string Create_Assign = "[sp_CrtWC_San]";
            Hashtable ht_assign = new Hashtable();
            Hashtable ht_trash = new Hashtable();
            ht_assign.Add("@WCid", PLNO);
            ht_assign.Add("@Usr", UserId);
            IO.SqlSp(DBlink, Create_Assign, ht_assign, ref ht_trash);

            return 1;
        }

        /// <summary>
        /// 派工單,派工人員簽核通過後押上實際完工日
        /// </summary>
        /// <param name="DBlink"></param>
        /// <param name="PLNO"></param>
        /// <returns></returns>
        public int Assign_ActDate(string DBlink, string PLNO)
        {
            int SuccessCountChild = 0;
            string Sql_cmd =
                    @"Update [Assignhead] set ActDate=getdate() where Wk_Id=@PLNO ";
            Hashtable ht_Agree = new Hashtable();
            ht_Agree.Add("@PLNO", PLNO);
            IO.SqlUpdate(DBlink, Sql_cmd, ht_Agree, ref SuccessCountChild);

            return SuccessCountChild;
        }

        /// <summary>
        /// 取的前一個人的退回原因
        /// </summary>
        /// <param name="DBlink"></param>
        /// <param name="PLNO"></param>
        /// <returns></returns>
        public string GetSignOffBackReasonPrevious(string DBlink, string PLNO)
        {
            string BackReason = "";
            string Sql_cmd =
                    @"select top 1 sofp_Reason from SignOff_Log with(nolock)
                    where PLNO=@PLNO and sofp_Reason is not null
                    order by sofp_updateDate DESC";
            Hashtable ht_Agree = new Hashtable();
            ht_Agree.Add("@PLNO", PLNO);
            DataSet ds_reason = IO.SqlQuery(DBlink, Sql_cmd, ht_Agree);
            if (ds_reason.Tables.Count > 0)
                if (ds_reason.Tables[0].Rows.Count > 0)
                    BackReason = ds_reason.Tables[0].Rows[0][0].ToString();

            return BackReason;
        }

        #endregion

        #region 簽核權限

        /// <summary>
        /// 簽核成功/退回(其他議價單,需跑完簽核過程)
        /// </summary>
        /// <param name="Login_Server"></param>
        /// <param name="UserID"></param>
        /// <param name="UserName"></param>
        /// <param name="IsOk"></param>
        /// <param name="NowStatus"></param>
        /// <param name="PLNO"></param>
        /// <returns></returns>
        public int SignOff_Quotation(string Login_Server, string UserID, string UserName, bool IsOk, string NowStatus, string PLNO, string PageType)
        {
            int SuccessCount = 0, SuccessCountChild = 0;

            //下一步單據狀態FutureStatus    簽核同意用
            //上一步單據狀態PreviousStatus    簽核取消用
            //最終單據狀態MaxStatus
            string FutureStatus = "", PreviousStatus = "", MaxStatus = "";
            string Sql_cmd =
            @"select FutureStatus=(select top 1 [status] from SignOff_status with(nolock) where PageType=@pagetype and [Status]>@NowStatus order by [Status] ),
            PreviousStatus=(select top 1 [status] from SignOff_status with(nolock) where PageType=@pagetype and [Status]<@NowStatus order by [Status] desc),
            MaxStatus=(select top 1 [status] from SignOff_status with(nolock) where PageType=@pagetype and [Status]>@NowStatus order by [Status] desc)";
            Hashtable ht_Status = new Hashtable();
            ht_Status.Add("@NowStatus", NowStatus);
            ht_Status.Add("@pagetype", PageType);
            DataTable dt_Status = IO.SqlQuery(Login_Server, Sql_cmd, ht_Status).Tables[0];
            FutureStatus = dt_Status.Rows[0][0].ToString();
            PreviousStatus = dt_Status.Rows[0][1].ToString();
            MaxStatus = dt_Status.Rows[0][2].ToString();

            try
            {

                #region 寫入log
                Sql_cmd =
                @"Insert Into SignOff_Log(pageType,PLNO,[Status],sofp_WorkId,sofp_WorkName,sofp_updateDate,sofp_IsOK,FinalStatus)
            values(@pagetype,@PLNO,@step,@UserId,@UserName,getdate(),@IsOK,@FinalStatus)";
                Hashtable ht1 = new Hashtable();
                ht1.Add("@pagetype", PageType);
                ht1.Add("@PLNO", PLNO);
                ht1.Add("@step", NowStatus);
                ht1.Add("@UserId", UserID);
                ht1.Add("@UserName", UserName);
                ht1.Add("@IsOK", IsOk == true ? 1 : 0);
                ht1.Add("@FinalStatus", IsOk == true ? FutureStatus : PreviousStatus);
                IO.SqlQuery(Login_Server, Sql_cmd, ht1);
                #endregion

                #region 紀錄簽核流程
                Hashtable ht_Agree = new Hashtable();
                if (IsOk)
                {
                    //同意
                    Sql_cmd =
                    @"Update SignOff_Process set sofp_WorkId=@UserId,sofp_WorkName=@UserName,sofp_updateDate=getdate()
                where pagetype=@pagetype and PLNO=@PLNO and [Status]=@step ";
                    ht_Agree.Add("@UserId", UserID);
                    ht_Agree.Add("@UserName", UserName);
                    ht_Agree.Add("@PLNO", PLNO);
                    ht_Agree.Add("@step", NowStatus);
                    ht_Agree.Add("@pagetype", PageType);
                }
                else
                {
                    //取消
                    //取消現在的簽核
                    //取消前一步的簽核
                    Sql_cmd =
                    @"Update SignOff_Process set sofp_WorkId='',sofp_WorkName='',sofp_updateDate=getdate()
                where pagetype=@pagetype and PLNO=@PLNO and [Status] in (@step1,@step2) ";
                    ht_Agree.Add("@PLNO", PLNO);
                    ht_Agree.Add("@step1", NowStatus);
                    ht_Agree.Add("@step2", FutureStatus);
                    ht_Agree.Add("@pagetype", PageType);
                }
                IO.SqlUpdate(Login_Server, Sql_cmd, ht_Agree, ref SuccessCountChild);
                #endregion

                #region 更改單據狀態
                switch (PageType)
                {
                    case "1":
                        Sql_cmd = "Update [3PL_QuotationHead] set I_qthe_Status=@Step,S_qthe_UpdId=@UserID where S_qthe_PLNO=@PLNO and I_qthe_Status=@Step2";
                        break;
                    case "2":
                        Sql_cmd = "Update [AssignHead] set [Status]=@Step,UpdUser=@UserID where Wk_Id=@PLNO and [Status]=@Step2";
                        break;
                    case "4":
                        Sql_cmd = "Update [_3PL_AdjustHead] set [Status]=@Step,UpdUser=@UserID where Adj_Id=@PLNO and [Status]=@Step2";
                        break;
                }

                Hashtable ht_Page = new Hashtable();
                ht_Page.Add("@Step", IsOk == true ? FutureStatus : PreviousStatus);
                ht_Page.Add("@UserID", UserID);
                ht_Page.Add("@PLNO", PLNO);
                ht_Page.Add("@Step2", NowStatus);
                IO.SqlUpdate(Login_Server, Sql_cmd, ht_Page, ref SuccessCountChild);
                SuccessCount += SuccessCountChild;
                #endregion

                #region 報價單 產生派工單,成本單
                if (PageType == "1" && FutureStatus == MaxStatus && IsOk == true)
                {
                    string strHeadAssignUserId = UserID;

                    #region 2015.03.05 其他議價單的簽核跑完後，不帶入主管姓名，改為帶入建單人
                    Hashtable ht_CreUser = new Hashtable();
                    Sql_cmd = "Select S_qthe_CreateId from [3PL_QuotationHead] where S_qthe_PLNO=@PLNO";
                    ht_CreUser.Add("@PLNO", PLNO);
                    DataSet ds1 = IO.SqlQuery(Login_Server, Sql_cmd, ht_CreUser);
                    if (ds1.Tables[0].Rows.Count > 0)
                    {
                        strHeadAssignUserId = ds1.Tables[0].Rows[0][0].ToString();
                    }
                    #endregion

                    Head_Assign(Login_Server, PLNO, strHeadAssignUserId);
                }
                #endregion

                #region 調整單 更改單據資料
                if (PageType == "4" && FutureStatus == MaxStatus && IsOk == true)
                {
                    _3PL_Adjust_DAO _3PLAdjust = new _3PL_Adjust_DAO();
                    _3PLAdjust.SignOffFinish(PLNO, UserID, UserName);
                }
                #endregion

                return SuccessCount;
            }
            catch
            {
                return 0;
            }
        }

        /// <summary>
        /// 簽核成功(其他議價單)
        /// </summary>
        /// <param name="Login_Server"></param>
        /// <param name="UserID"></param>
        /// <param name="UserName"></param>
        /// <param name="IsOk"></param>
        /// <param name="NowStatus"></param>
        /// <param name="PLNO"></param>
        /// <returns></returns>
        public int SignOff_Quotation_NotSpecial(string Login_Server, string UserID, string UserName, bool IsOk, string NowStatus, string PLNO)
        {
            int SuccessCount = 0, SuccessCountChild = 0;

            //下一步單據狀態FutureStatus    簽核同意用
            //上一步單據狀態PreviousStatus    簽核取消用
            //最終單據狀態MaxStatus
            string FutureStatus = "", PreviousStatus = "", MaxStatus = "";
            string Sql_cmd =
            @"select FutureStatus=(select top 1 [status] from SignOff_status with(nolock) where PageType=1 and [Status]>@NowStatus order by [Status] desc),
            PreviousStatus=(select top 1 [status] from SignOff_status with(nolock) where PageType=1 and [Status]<@NowStatus order by [Status] desc),
            MaxStatus=(select top 1 [status] from SignOff_status with(nolock) where PageType=1 and [Status]>@NowStatus order by [Status] desc)";
            Hashtable ht_Status = new Hashtable();
            ht_Status.Add("@NowStatus", NowStatus);
            DataTable dt_Status = IO.SqlQuery(Login_Server, Sql_cmd, ht_Status).Tables[0];
            FutureStatus = dt_Status.Rows[0][0].ToString();
            PreviousStatus = dt_Status.Rows[0][1].ToString();
            MaxStatus = dt_Status.Rows[0][2].ToString();

            try
            {

                #region 寫入log
                Sql_cmd =
                @"Insert Into SignOff_Log(pageType,PLNO,[Status],sofp_WorkId,sofp_WorkName,sofp_updateDate,sofp_IsOK,FinalStatus)
            values(1,@PLNO,@step,@UserId,@UserName,getdate(),@IsOK,@FinalStatus)";
                Hashtable ht1 = new Hashtable();
                ht1.Add("@PLNO", PLNO);
                ht1.Add("@step", NowStatus);
                ht1.Add("@UserId", UserID);
                ht1.Add("@UserName", UserName);
                ht1.Add("@IsOK", IsOk == true ? 1 : 0);
                ht1.Add("@FinalStatus", IsOk == true ? FutureStatus : PreviousStatus);
                IO.SqlQuery(Login_Server, Sql_cmd, ht1);
                #endregion

                #region 紀錄簽核流程
                Hashtable ht_Agree = new Hashtable();
                if (IsOk)
                {
                    //同意
                    Sql_cmd =
                    @"Update SignOff_Process set sofp_WorkId=@UserId,sofp_WorkName=@UserName,sofp_updateDate=getdate()
                where pagetype=1 and PLNO=@PLNO and [Status]>=@step ";
                    ht_Agree.Add("@UserId", UserID);
                    ht_Agree.Add("@UserName", UserName);
                    ht_Agree.Add("@PLNO", PLNO);
                    ht_Agree.Add("@step", NowStatus);
                }
                IO.SqlUpdate(Login_Server, Sql_cmd, ht_Agree, ref SuccessCountChild);
                #endregion

                #region 更改單據狀態
                Hashtable ht_Page = new Hashtable();
                Sql_cmd = "Update [3PL_QuotationHead] set I_qthe_Status=@Step,S_qthe_UpdId=@UserID where S_qthe_PLNO=@PLNO and I_qthe_Status=@Step2";
                ht_Page.Add("@Step", IsOk == true ? FutureStatus : PreviousStatus);
                ht_Page.Add("@UserID", UserID);
                ht_Page.Add("@PLNO", PLNO);
                ht_Page.Add("@Step2", NowStatus);
                IO.SqlUpdate(Login_Server, Sql_cmd, ht_Page, ref SuccessCountChild);
                SuccessCount += SuccessCountChild;
                #endregion

                #region 產生派工單,成本單
                if (IsOk == true && FutureStatus == MaxStatus)
                {
                    Head_Assign(Login_Server, PLNO, UserID);
                }
                #endregion

                return SuccessCount;
            }
            catch
            {
                return 0;
            }
        }

        /// <summary>
        /// 簽核退回
        /// </summary>
        /// <param name="Login_Server"></param>
        /// <param name="UserID"></param>
        /// <param name="NowStatus"></param>
        /// <param name="PLNO"></param>
        /// <param name="Reason"></param>
        /// <returns></returns>
        public int SignOffBackReason(string Login_Server, string UserID, string NowStatus, string PLNO, string Reason, string PageType)
        {
            int SuccessCountChild = 0;
            string Sql_cmd =
                    @"Update SignOff_Process set sofp_Reason=@Reason,sofp_updateDate=getdate()
                where pagetype=@pagetype and PLNO=@PLNO and [Status] in (@step1) ";
            Hashtable ht_Agree = new Hashtable();
            ht_Agree.Add("@PLNO", PLNO);
            ht_Agree.Add("@step1", NowStatus);
            ht_Agree.Add("@Reason", Reason);
            ht_Agree.Add("@pagetype", PageType);
            IO.SqlUpdate(Login_Server, Sql_cmd, ht_Agree, ref SuccessCountChild);

            Sql_cmd =
                    @"Update SignOff_Log set sofp_Reason=@Reason,sofp_updateDate=getdate()
                where pagetype=@pagetype and PLNO=@PLNO and [Status] in (@step1) 
                    and Sn in (Select max(Sn) from SignOff_Log with(nolock) where pagetype=@pagetype and PLNO=@PLNO and [Status] in (@step1))";
            IO.SqlUpdate(Login_Server, Sql_cmd, ht_Agree, ref SuccessCountChild);
            return SuccessCountChild;
        }
        #endregion
    }
}
