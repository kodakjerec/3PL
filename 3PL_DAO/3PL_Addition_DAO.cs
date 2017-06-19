using System.Data;
using _3PL_LIB;
using System.Collections;
using System;

#region 修改紀錄
//2015.03.05 其他議價單的簽核跑完後，不帶入主管姓名，改為帶入建單人
#endregion
namespace _3PL_DAO
{
    public partial class _3PL_Addition_DAO
    {
        public string Login_Server = "3PL";
        DB_IO IO = new DB_IO();

        #region 關帳日期
        #region 查詢
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

            DataSet ds = IO.SqlQuery(Login_Server, Sql_cmd, ht1);
            SignOffStatus = ds.Tables[0];
            string CloseData = SignOffStatus.Rows[0][0].ToString();
            return CloseData;
        }
        #endregion

        #region 修改
        public bool Addon_UpdCloseData(string CloseData)
        {
            string Sql_cmd =
            @"Update [3PL_BaseData] set S_bsda_Fieldid=@CloseData where S_bsda_CateId='CloseData'";
            Hashtable ht1 = new Hashtable();
            ht1.Add("@CloseData", CloseData);
            int Count1 = 0;
            bool IsOK=false;
            IO.SqlUpdate(Login_Server, Sql_cmd, ht1, ref Count1);
            if (Count1 > 0)
                IsOK = true;
            return IsOK;
        }
        #endregion
        #endregion
    }
}
