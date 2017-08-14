using System.Data;
using _3PL_LIB;
using System.Collections;
using System;

#region 修改紀錄

#endregion
namespace _3PL_DAO
{
    public partial class _3PL_Adjust_DAO
    {
        public string Login_Server = "3PL";
        DB_IO IO = new DB_IO();

        #region 檢查
        public string CheckHeadAdjustPageId(string Adj_Type, string Adj_Page_Id)
        {
            string ErrMsg = "";
            string Sql_cmd = "";
            Hashtable ht1 = new Hashtable();
            ht1.Add("@Adj_Page_Id", Adj_Page_Id);

            switch (Adj_Type)
            {
                case "1":
                    Sql_cmd = @"select top 1 0 from [3PL_QuotationHead] with(nolock) where S_qthe_PLNO=@Adj_Page_Id";
                    break;
                case "2":
                    Sql_cmd = @"select top 1 0 from [AssignHead] with(nolock) where Wk_Id=@Adj_Page_Id";
                    break;
                default:
                    ErrMsg = "輸入資料錯誤\\n"; break;
            }
            DataSet ds = IO.SqlQuery(Login_Server, Sql_cmd, ht1);
            DataTable dt = ds.Tables[0];
            if (dt.Rows.Count <= 0)
            {
                ErrMsg = "找不到相關單號\\n";
            }

            return ErrMsg;
        }
        #endregion

        #region 查詢
        /// <summary>
        /// 查詢調整單單頭
        /// </summary>
        /// <param name="wk_Id">單號</param>
        /// <param name="uI">個資</param>
        /// <param name="bol_Chk_ShowStatusIsZero">顯示作廢單據</param>
        /// <param name="assignStatusType">單據狀態篩選</param>
        /// <returns></returns>
        public DataTable GetHead(string Wk_ID, UserInf uI, string Adj_Type, bool bol_Chk_ShowStatusIsZero, string AssignStatusType)
        {
            DataTable QuotationList = new DataTable();

            string Sql_cmd =
            @"select a.*
            ,Adj_Type_Name=d.S_bsda_FieldName
            ,IsOk=dbo.[fn3PL_GetSignOffPermission](4,a.[Status],@UserID)
            ,c.Step,c.[StatusName],c.[OkbuttonName],c.[NobuttonName]
            ,建單人=a.CrtUser+','+e.WorkName
            ,[UIStatus]='Unchanged'
            from _3PL_AdjustHead a with(nolock)
            inner join SignOff_Status c with(nolock) on a.[Status]=c.[Status] and c.pageType=4
            inner join [3PL_BaseData] d with(nolock) on a.Adj_Type=d.S_bsda_FieldId and d.S_bsda_CateId='AdjType'
            left join EmpInf e on a.CrtUser=e.WorkId
            where 1=1 ";
            Hashtable ht1 = new Hashtable();
            ht1.Add("@UserID", uI.UserID);

            //有選擇Wk_Id
            if (Wk_ID != "")
            {
                Sql_cmd += " and Adj_Id like @Wk_ID+'%'";
                ht1.Add("@Wk_ID", Wk_ID);
            }
            //有選擇bol_Chk_ShowStatusIsZero
            if (bol_Chk_ShowStatusIsZero == false)
            {
                Sql_cmd += " and a.[Status]>0";
            }
            //選擇狀態
            switch (Adj_Type)
            {
                case "ALL": break;
                case "1":
                    Sql_cmd += " and a.Adj_Type=1";
                    break;
                case "2":
                    Sql_cmd += " and a.Adj_Type=2";
                    break;
            }
            //選擇狀態
            switch (AssignStatusType)
            {
                case "0": break;
                case "1":
                    Sql_cmd += " and a.[Status]>0 and a.[Status]<20";
                    break;
                case "2":
                    Sql_cmd += " and a.[Status]=20";
                    break;
            }
            Sql_cmd += " order by UpdDate DESC, Adj_Id";
            DataSet ds = IO.SqlQuery(Login_Server, Sql_cmd, ht1);
            QuotationList = ds.Tables[0];

            //加入primaryKey
            DataColumn[] keys = new DataColumn[1];
            keys[0] = QuotationList.Columns["Adj_Id"];
            QuotationList.PrimaryKey = keys;

            return QuotationList;
        }

        /// <summary>
        /// 查詢調整單明細
        /// </summary>
        /// <param name="wk_Id">單號</param>
        /// <returns></returns>
        public DataTable GetDetail(string Wk_ID)
        {
            DataTable QuotationList = new DataTable();

            string Sql_cmd =
            @"Select top 50
            SEQ=ROW_NUMBER() OVER (ORDER BY a.ADJ_ID)
            ,a.*
            ,[ColName]=CASE b.Adj_Type
			            WHEN 1 THEN c1.S_bsda_FieldName
			            WHEN 2 THEN c2.S_bsda_FieldName END
            ,[UIStatus]='Unchanged'
            FROM [_3PL_AdjustDetail] a with (nolock)
            INNER JOIN 
	            _3PL_AdjustHead b with(nolock)
            ON 
	            b.Adj_Id=a.Adj_Id
            LEFT JOIN 
	            [3PL_BaseData] c1 with(nolock)
            ON 
	            a.OriginID=c1.S_bsda_FieldId and c1.S_bsda_CateId='3PL_QuotationHead'
            LEFT JOIN 
	            [3PL_BaseData] c2 with(nolock)
            ON 
	            a.OriginID=c2.S_bsda_FieldId and c2.S_bsda_CateId='AssignHead'
            where a.Adj_Id=@Wk_ID and DelFlag<1 ";
            Hashtable ht1 = new Hashtable();
            ht1.Add("@Wk_ID", Wk_ID);

            DataSet ds = IO.SqlQuery(Login_Server, Sql_cmd, ht1);
            QuotationList = ds.Tables[0];

            DataColumn[] keys = new DataColumn[2];
            keys[0] = QuotationList.Columns["Adj_Id"];
            keys[1] = QuotationList.Columns["SEQ"];
            QuotationList.PrimaryKey = keys;

            return QuotationList;
        }

        /// <summary>
        /// 自動帶出欄位內容
        /// </summary>
        /// <param name="Adj_Type"></param>
        /// <param name="Adj_Page_Id"></param>
        /// <returns></returns>
        public string BringHeadAdjustPageId_Value(string Adj_Type, string Adj_Page_Id, string ColName)
        {
            string ReturnMsg = "";
            string Sql_cmd = "";
            Hashtable ht1 = new Hashtable();
            ht1.Add("@Adj_Page_Id", Adj_Page_Id);
            ht1.Add("@ColName", ColName);

            switch (Adj_Type)
            {
                case "1":
                    Sql_cmd = @"select top 1 " + ColName + " from [3PL_QuotationHead] with(nolock) where S_qthe_PLNO=@Adj_Page_Id";
                    break;
                case "2":
                    Sql_cmd = @"select top 1 " + ColName + " from [AssignHead] with(nolock) where Wk_Id=@Adj_Page_Id";
                    break;
                default:
                    ReturnMsg = "輸入資料錯誤\\n"; break;
            }
            DataSet ds = IO.SqlQuery(Login_Server, Sql_cmd, ht1);
            DataTable dt = ds.Tables[0];
            if (dt.Rows.Count <= 0)
            {
                ReturnMsg = "";
            }
            else
            {
                ReturnMsg = dt.Rows[0][0].ToString();
            }

            return ReturnMsg;
        }
        #endregion

        #region 新刪修
        /// <summary>
        /// 新增資料
        /// </summary>
        /// <param name="login_Server"></param>
        /// <param name="dt_head"></param>
        /// <param name="dt_Detail"></param>
        /// <param name="adjustNo"></param>
        /// <param name="userID"></param>
        /// <returns></returns>
        public bool InsertAdjust(string login_Server, DataTable dt_head, DataTable dt_Detail, string adjustNo, string userID)
        {
            int SuccessCount = 0, ModifyCount = 0;
            string RowStatus = "";

            #region 表頭
            foreach (DataRow dr in dt_head.Rows)
            {
                RowStatus = dr["UIStatus"].ToString();

                if (RowStatus == "Added")
                {
                    dr["Adj_Id"] = adjustNo;
                    SuccessCount += Head_Insert(login_Server, dr);
                    ModifyCount++;
                }
                else if (RowStatus == "Modified")
                {
                    SuccessCount += Head_Update(login_Server, dr);
                    ModifyCount++;
                }
                else if (RowStatus == "Deleted")
                {
                    SuccessCount += Head_Delete(login_Server, dr);
                    ModifyCount++;
                }
            }
            #endregion

            #region 明細

            foreach (DataRow dr in dt_Detail.Rows)
            {
                RowStatus = dr["UIStatus"].ToString();

                if (RowStatus == "Added")
                {
                    dr["Adj_Id"] = adjustNo;
                    SuccessCount += Detail_Insert(login_Server, dr);
                    ModifyCount++;
                }
                else if (RowStatus == "Modified")
                {
                    SuccessCount += Detail_Update(login_Server, dr);
                    ModifyCount++;
                }
                else if (RowStatus == "Deleted")
                {
                    SuccessCount += Detail_Delete(login_Server, dr);
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
            string Insertcmd_Head = @"Insert Into [_3PL_AdjustHead](Adj_Id,Adj_Type,Adj_PageId,Memo,Status,CrtUser,CrtDate,UpdUser,UpdDate)
            values(@Adj_Id,@Adj_Type,@Adj_PageId,@Memo,10,@CrtUser,GETDATE(),@UpdUser,GETDATE())";

            Hashtable ht1 = new Hashtable();
            ht1.Add("@Adj_Id", dr["Adj_Id"]);
            ht1.Add("@Adj_Type", dr["Adj_Type"]);
            ht1.Add("@Adj_PageId", dr["Adj_PageId"]);
            ht1.Add("@Memo", dr["Memo"]);
            ht1.Add("@CrtUser", dr["CrtUser"]);
            ht1.Add("@UpdUser", dr["UpdUser"]);

            IO.SqlUpdate(Login_Server, Insertcmd_Head, ht1, ref SuccessCount_head);
            SuccessCount += SuccessCount_head;

            if (SuccessCount > 0)
            {
                Hashtable ht2 = new Hashtable();
                Hashtable ht3 = new Hashtable();
                ht2.Add("@PLNO", dr["Adj_Id"]);
                ht2.Add("@UserId", dr["CrtUser"]);
                ht2.Add("@PageType", 4);
                IO.SqlSp(Login_Server, "[sp3PL_AddSignOff]", ht2, ref ht3);
            }

            return SuccessCount;
        }
        private int Head_Update(string DBlink, DataRow dr)
        {
            int SuccessCount = 0, SuccessCount_head = 0;
            string Updcmd_head =
            @"Update [_3PL_AdjustHead] set Adj_Type=@Adj_Type,Adj_PageId=@Adj_PageId,Memo=@Memo,UpdUser=@UpdUser,UpdDate=GETDATE()
            where Adj_Id=@Adj_Id";
            Hashtable ht1 = new Hashtable();
            ht1.Add("@Adj_Id", dr["Adj_Id"]);
            ht1.Add("@Adj_Type", dr["Adj_Type"]);
            ht1.Add("@Adj_PageId", dr["Adj_PageId"]);
            ht1.Add("@Memo", dr["Memo"]);
            ht1.Add("@CrtUser", dr["CrtUser"]);
            ht1.Add("@UpdUser", dr["UpdUser"]);

            IO.SqlUpdate(Login_Server, Updcmd_head, ht1, ref SuccessCount_head);
            SuccessCount += SuccessCount_head;

            return SuccessCount;
        }
        private int Head_Delete(string DBlink, DataRow dr)
        {
            int SuccessCount = 0, SuccessCount_head = 0;
            string Delcmd_head =
            @"Update [_3PL_AdjustHead] 
            set Status=0, UpdUser=@UpdUser,Memo=@Memo
            where Adj_Id=@Adj_Id";
            Hashtable ht1 = new Hashtable();
            ht1.Add("@Adj_Id", dr["Adj_Id"]);
            ht1.Add("@Adj_Type", dr["Adj_Type"]);
            ht1.Add("@Adj_PageId", dr["Adj_PageId"]);
            ht1.Add("@Memo", dr["Memo"]);
            ht1.Add("@CrtUser", dr["CrtUser"]);
            ht1.Add("@UpdUser", dr["UpdUser"]);

            IO.SqlUpdate(Login_Server, Delcmd_head, ht1, ref SuccessCount_head);
            SuccessCount += SuccessCount_head;

            return SuccessCount;
        }
        #endregion

        #region 單據明細的新刪修
        private int Detail_Insert(string DBlink, DataRow dr)
        {
            int SuccessCount = 0, SuccessCount_Detail = 0;
            string Insertcmd_Detail = @"Insert Into [_3PL_AdjustDetail](Adj_Id
            ,OriginID
            ,OriginValue
            ,newValue
            ,CrtUser
            ,CrtDate
            ,UpdUser
            ,UpdDate)
                        values(@Adj_Id
            ,@OriginID
            ,@OriginValue
            ,@newValue
            ,@CrtUser
            ,GETDATE()
            ,@UpdUser
            ,GETDATE())";

            Hashtable ht1 = new Hashtable();
            ht1.Add("@Adj_Id", dr["Adj_Id"]);
            ht1.Add("@OriginID", dr["OriginID"]);
            ht1.Add("@OriginValue", dr["OriginValue"]);
            ht1.Add("@newValue", dr["newValue"]);
            ht1.Add("@CrtUser", dr["CrtUser"]);
            ht1.Add("@UpdUser", dr["UpdUser"]);

            IO.SqlUpdate(Login_Server, Insertcmd_Detail, ht1, ref SuccessCount_Detail);
            SuccessCount += SuccessCount_Detail;

            return SuccessCount;
        }
        private int Detail_Update(string DBlink, DataRow dr)
        {
            int SuccessCount = 0, SuccessCount_Detail = 0;
            string Updcmd_Detail =
            @"Update [_3PL_AdjustDetail] 
            set OriginID=@OriginID,OriginValue=@OriginValue,newValue=@newValue,UpdUser=@UpdUser
            where SN=@SN";

            Hashtable ht1 = new Hashtable();
            ht1.Add("@SN", dr["SN"]);
            ht1.Add("@Adj_Id", dr["Adj_Id"]);
            ht1.Add("@OriginID", dr["OriginID"]);
            ht1.Add("@OriginValue", dr["OriginValue"]);
            ht1.Add("@newValue", dr["newValue"]);
            ht1.Add("@CrtUser", dr["CrtUser"]);
            ht1.Add("@UpdUser", dr["UpdUser"]);

            IO.SqlUpdate(Login_Server, Updcmd_Detail, ht1, ref SuccessCount_Detail);
            SuccessCount += SuccessCount_Detail;

            return SuccessCount;
        }
        private int Detail_Delete(string DBlink, DataRow dr)
        {
            //新增後又刪除的資料直接跳過 
            if (dr["SN"].ToString() == "0")
                return 1;

            int SuccessCount = 0, SuccessCount_Detail = 0;
            string Delcmd_Detail =
            @"Update [_3PL_AdjustDetail]
            set DelFlag=@DelFlag, UpdUser=@UpdUser 
            where SN=@SN";

            Hashtable ht1 = new Hashtable();
            ht1.Add("@DelFlag", 1);
            ht1.Add("@UpdUser", dr["UpdUser"]);
            ht1.Add("@SN", dr["SN"]);

            IO.SqlUpdate(Login_Server, Delcmd_Detail, ht1, ref SuccessCount_Detail);
            SuccessCount += SuccessCount_Detail;

            return SuccessCount;
        }
        #endregion
        #endregion

        #region 簽核完畢
        public bool SignOffFinish(string Adj_Id, string UserID, string UserName)
        {
            #region 更改異動單號
            DataTable dt_Source = new DataTable();
            string Sql_Cmd = @"select a.Adj_Type, a.Adj_PageId, b.OriginID, b.OriginValue, b.newValue
            from _3PL_AdjustHead a with(nolock)
            inner join _3PL_AdjustDetail b with(nolock)
            on a.Adj_Id=b.Adj_Id
            where a.Adj_Id=@Adj_Id";
            Hashtable ht1 = new Hashtable();
            ht1.Add("@Adj_Id", Adj_Id);
            dt_Source = IO.SqlQuery(Login_Server, Sql_Cmd, ht1).Tables[0];

            if (dt_Source.Rows.Count > 0)
            {
                ht1.Clear();
                Sql_Cmd = "";

                DataRow dr = dt_Source.Rows[0];
                ht1.Add("@PLNO", dr["Adj_PageId"]);
                ht1.Add("@OriginValue", dr["OriginValue"]);
                ht1.Add("@newValue", dr["newValue"]);

                switch (Convert.ToInt32(dr["Adj_Type"]))
                {
                    case 1:
                        Sql_Cmd = @"Update [3PL_QuotationHead] 
                                    set " + dr["OriginID"].ToString() + @"=@newValue
                                    where S_qthe_PLNO=@PLNO 
                                    and " + dr["OriginID"].ToString() + " =@OriginValue";
                        break;
                    case 2:
                        Sql_Cmd = @"Update [AssignHead] 
                                    set " + dr["OriginID"].ToString() + @"=@newValue
                                    where Wk_Id=@PLNO 
                                    and " + dr["OriginID"].ToString() + " =@OriginValue";
                        break;
                }
                int tempCount = 0;
                IO.SqlUpdate(Login_Server, Sql_Cmd, ht1, ref tempCount);


                #region 寫入log
                ht1.Clear();
                Sql_Cmd = "";

                Sql_Cmd =
                @"Insert Into SignOff_Log(pageType,PLNO,[Status],sofp_WorkId,sofp_WorkName,sofp_updateDate,sofp_IsOK, sofp_Reason, FinalStatus)
                                values(@pagetype,@PLNO,@step,@UserId,@UserName,getdate(), @IsOK, @Memo, @step2)";
                ht1.Add("@pagetype", dr["Adj_Type"]);
                ht1.Add("@PLNO", dr["Adj_PageId"]);
                ht1.Add("@step", dr["OriginValue"]);
                ht1.Add("@UserId", UserID);
                ht1.Add("@UserName", UserName);
                ht1.Add("@IsOK", "M");
                ht1.Add("@Memo", Adj_Id);
                ht1.Add("@step2", dr["newValue"]);
                IO.SqlQuery(Login_Server, Sql_Cmd, ht1);
                #endregion
            }
            #endregion
            return true;
        }
        #endregion
    }
}
