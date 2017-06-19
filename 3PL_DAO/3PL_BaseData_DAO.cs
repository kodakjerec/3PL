using System.Data;
using _3PL_LIB;
using System.Collections;

namespace _3PL_DAO
{
    public class _3PL_BaseData_DAO
    {
        DB_IO IO = new DB_IO();

        #region 查詢
        /// <summary>
        /// 取得基本資料大類
        /// </summary>
        /// <param name="strCon"></param>
        /// <returns></returns>
        public DataTable GetCateId(string Login_Server)
        {
            DataTable CateIdList = new DataTable();

            string Sql_cmd = @"Select Distinct S_bsda_CateId, S_bsda_CateName
                                    From [3PL_baseData]
                                   Where 1=1";
            Hashtable ht1 = new Hashtable();
            DataSet ds = IO.SqlQuery(Login_Server, Sql_cmd, ht1);
            CateIdList = ds.Tables[0];

            return CateIdList;
        }

        /// <summary>
        /// 取得指定大類的欄位詳細資料
        /// </summary>
        /// <param name="strCon"></param>
        /// <param name="FieldId"></param>
        /// <returns></returns>
        public DataTable GetBDList(string Login_Server, string FieldId)
        {
            DataTable BDList = new DataTable();
            string Sql_cmd = @"Select I_bsda_seq,S_bsda_CateId,S_bsda_CateName,S_bsda_FieldId,S_bsda_FieldName
                                    From [3PL_baseData]
                                   Where 1=1 and S_bsda_CateId=@CateId and I_bsda_DelFlag=0
                                    order by S_bsda_FieldId";
            Hashtable ht1 = new Hashtable();
            ht1.Add("@CateId", FieldId);
            DataSet ds = IO.SqlQuery(Login_Server, Sql_cmd, ht1);
            BDList = ds.Tables[0];

            return BDList;
        }
        #endregion

        #region 更新資料庫
        //更新
        public int BDList_Update(string Login_Server, string UserId, DataRow dr)
        {
            int SuccessCount = 0;
            bool IsSuccess = false;

            string Sql_cmd = @"Update [3PL_BaseData]
                                set S_bsda_FieldId=@S_bsda_FieldId,S_bsda_FieldName=@S_bsda_Fieldname,S_bsda_UpdId=@UpdId
                                where I_bsda_seq=@I_bsda_seq";
            Hashtable ht1 = new Hashtable();
            ht1.Add("@S_bsda_FieldId", dr["S_bsda_FieldId"]);
            ht1.Add("@S_bsda_Fieldname", dr["S_bsda_Fieldname"]);
            ht1.Add("@I_bsda_seq", dr["I_bsda_seq"]);
            ht1.Add("@UpdId", UserId);
            IsSuccess = IO.SqlUpdate(Login_Server, Sql_cmd, ht1, ref SuccessCount);
            if (!IsSuccess)
                SuccessCount = 0;
            return SuccessCount;
        }

        //新增
        public int BDList_Insert(string Login_Server, string UserId, DataRow dr)
        {
            int SuccessCount = 0;
            bool IsSuccess = false;

            string Sql_cmd = @"Insert Into [3PL_BaseData] (S_bsda_CateId,S_bsda_CateName,S_bsda_FieldId,S_bsda_FieldName,S_bsda_CreateId)
                                values(@S_bsda_CateId,@S_bsda_CateName,@S_bsda_FieldId,@S_bsda_FieldName,@S_bsda_CreateId)";
            Hashtable ht1 = new Hashtable();
            ht1.Add("@S_bsda_CateId", dr["S_bsda_CateId"]);
            ht1.Add("@S_bsda_CateName", dr["S_bsda_CateName"]);
            ht1.Add("@S_bsda_FieldId", dr["S_bsda_FieldId"]);
            ht1.Add("@S_bsda_FieldName", dr["S_bsda_FieldName"]);
            ht1.Add("@S_bsda_CreateId", UserId);
            IsSuccess = IO.SqlUpdate(Login_Server, Sql_cmd, ht1, ref SuccessCount);
            if (!IsSuccess)
                SuccessCount = 0;
            return SuccessCount;
        }

        //刪除
        public int BDList_Delete(string Login_Server, string UserId, string DelSeq)
        {
            int SuccessCount = 0;
            bool IsSuccess = false;

            string Sql_cmd = @"Update [3PL_BaseData] set I_bsda_DelFlag=1, S_bsda_UpdId=@id, D_bsda_UpdDate=getdate() where I_bsda_seq=@seq";
            Hashtable ht1 = new Hashtable();
            ht1.Add("@seq", DelSeq);
            ht1.Add("@id", UserId);
            IsSuccess = IO.SqlUpdate(Login_Server, Sql_cmd, ht1, ref SuccessCount);
            if (!IsSuccess)
                SuccessCount = 0;
            return SuccessCount;
        }
        #endregion
    }
}
