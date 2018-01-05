using System.Data;
using _3PL_LIB;
using System.Collections;

namespace _3PL_DAO
{
    public partial class _3PL_BaseAccounting_DAO
    {
        DB_IO IO = new DB_IO();

        /// <summary>
        /// 取得會計科目詳細資料
        /// </summary>
        /// <param name="strCon"></param>
        /// <param name="FieldId"></param>
        /// <returns></returns>
        public DataTable GetAccList(string Login_Server)
        {
            DataTable AccList = new DataTable();

            string Sql_cmd = @"Select I_Acci_seq,S_Acci_Id,S_Acci_Name
                                    From [3PL_baseAccounting] with(nolock)
                                    where I_acci_DelFlag=0
                                    order by S_Acci_Id";
            Hashtable ht1 = new Hashtable();
            DataSet ds = IO.SqlQuery(Login_Server, Sql_cmd, ht1);
            AccList = ds.Tables[0];

            return AccList;
        }

        #region 更新資料庫
        //更新
        public int AccList_Update(string Login_Server, string UserId, DataRow dr)
        {
            int SuccessCount = 0;
            bool IsSuccess = false;

            string Sql_cmd = @"Update [3PL_BaseAccounting]
                                set S_Acci_Id=@S_Acci_Id,S_Acci_Name=@S_Acci_Name,S_Acci_UpdId=@S_Acci_UpdId
                                where I_Acci_seq=@I_Acci_seq";
            Hashtable ht1 = new Hashtable();
            ht1.Add("@S_Acci_Id", dr["S_Acci_Id"]);
            ht1.Add("@S_Acci_Name", dr["S_Acci_Name"]);
            ht1.Add("@S_Acci_UpdId", UserId);
            ht1.Add("@I_Acci_seq", dr["I_Acci_seq"]);
            IsSuccess = IO.SqlUpdate(Login_Server, Sql_cmd, ht1, ref SuccessCount);
            if (!IsSuccess)
                SuccessCount = 0;
            return SuccessCount;
        }

        //新增
        public int AccList_Insert(string Login_Server, string UserId, DataRow dr)
        {
            int SuccessCount = 0;
            bool IsSuccess = false;

            string Sql_cmd = @"Insert Into [3PL_BaseAccounting] (S_Acci_Id,S_Acci_Name,S_Acci_CreateId,S_Acci_UpdId)
                                values(@S_Acci_Id,@S_Acci_Name,@S_Acci_CreateId,'')";
            Hashtable ht1 = new Hashtable();
            ht1.Add("@S_Acci_Id", dr["S_Acci_Id"]);
            ht1.Add("@S_Acci_Name", dr["S_Acci_Name"]);
            ht1.Add("@S_Acci_CreateId", UserId);
            IsSuccess = IO.SqlUpdate(Login_Server, Sql_cmd, ht1, ref SuccessCount);
            if (!IsSuccess)
                SuccessCount = 0;
            return SuccessCount;
        }
        #endregion
    }
}
