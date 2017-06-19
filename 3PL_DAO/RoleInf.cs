using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Data;
using _3PL_LIB;

namespace _3PL_DAO
{
    public class RoleInf
    {
        DB_IO IO = new DB_IO();

        /// <summary>
        /// 刪除身分類別
        /// </summary>
        /// <param name="DB"></param>
        /// <param name="ClassId"></param>
        /// <returns></returns>
        public bool DelClass(string DB, string ClassId)
        {
            bool booDel = false;
            string[] SqlCmd = new string[2];
            Hashtable[] Hs = new Hashtable[2];
            Hashtable hs1 = new Hashtable();
            Hashtable hs2 = new Hashtable();
            try
            {
                SqlCmd[0] = "Delete dbo.ClassInf Where ClassId= @ClassId ";
                hs1.Add("@ClassId", ClassId);

                SqlCmd[1] = "Delete dbo.ClassRole Where ClassId= @ClassId ";
                hs2.Add("@ClassId", ClassId);
                Hs[0] = hs1;
                Hs[1] = hs2;
                booDel = IO.SqlBeginTran("3PL", SqlCmd, Hs);
            }
            catch
            {
                booDel = false;
            }
            return booDel;
        }

        #region RoleList
        /// <summary>
        /// FunList 權限清單
        /// </summary>
        /// <param name="DB"></param>
        /// <param name="RoleID"></param>
        /// <returns></returns>
        public DataSet dsFunRoleList(string DB, string RoleID, string FunNa)
        {
            DataSet ds = new DataSet();
            Hashtable hs = new Hashtable();
            try
            {
                string SqlCom = @"Select FL.FunOrd,FL.FunId,FL.FunNm,FL.PgOrd,FL.PgId,FL.PgNm,FL.PgUrl,isnull(RI.RoleId,'') RoleId,isnull(RI.RoleNm,'') RoleNm from FunList FL
                                Left Join RoleFun RF ON RF.FunId=FL.FunId and RF.PgId=FL.PgId and RF.RoleId=@RoleID
                                Left Join RoleInf RI ON RI.RoleId=RF.RoleId and RI.RoleId=@RoleID
                                Where FL.FunId <>'IT001' ";


                hs.Add("@RoleID", RoleID);
                if (FunNa.Length > 0)
                {
                    SqlCom += " And FunNm like '%'+@FunNm+'%' ";
                    hs.Add("@FunNm", FunNa);
                }
                SqlCom += " Order by FL.FunOrd,FL.PgOrd ";
                ds = IO.SqlQuery(DB, SqlCom, hs);
            }
            catch
            { }
            return ds;
        }

        /// <summary>
        /// 角色清單
        /// </summary>
        /// <param name="DB"></param>
        /// <param name="RoleID"></param>
        /// <returns></returns>
        public DataSet dsRoleList(string DB, string RoleID)
        {
            DataSet ds = new DataSet();
            Hashtable hs = new Hashtable();
            try
            {
                string SqlCom = @"Select RoleId,RoleNm From RoleInf Where RoleId !='admin' ";
                if (RoleID.Length > 0)
                {
                    SqlCom += " RoleId=@RoleId ";
                    hs.Add("@RoleId", RoleID);
                }
                ds = IO.SqlQuery(DB, SqlCom, hs);
            }
            catch
            { }
            return ds;
        }

        #region 3PL_RoleInf_Edit.aspx
        //查詢
        public DataTable RoleInf_Get(string DB)
        {
            DataSet ds = new DataSet();
            Hashtable hs = new Hashtable();

            string SqlCom = @"Select Sn, RoleId, RoleNm From RoleInf with(nolock)";
            ds = IO.SqlQuery(DB, SqlCom, hs);

            return ds.Tables[0];
        }

        //更新
        public int RoleInf_Update(string Login_Server, string UserId, DataRow dr)
        {
            int SuccessCount = 0;
            bool IsSuccess = false;

            string Sql_cmd = @"Update [RoleInf]
                                set RoleId=@S_bsda_FieldId,RoleNm=@S_bsda_Fieldname,UpdUser=@UpdId,UpdDate=getdate()
                                where Sn=@I_bsda_seq";
            Hashtable ht1 = new Hashtable();
            ht1.Add("@S_bsda_FieldId", dr["RoleId"]);
            ht1.Add("@S_bsda_Fieldname", dr["RoleNm"]);
            ht1.Add("@I_bsda_seq", dr["Sn"]);
            ht1.Add("@UpdId", UserId);
            IsSuccess = IO.SqlUpdate(Login_Server, Sql_cmd, ht1, ref SuccessCount);
            if (!IsSuccess)
                SuccessCount = 0;
            return SuccessCount;
        }

        //新增
        public int RoleInf_Insert(string Login_Server, string UserId, DataRow dr)
        {
            int SuccessCount = 0;
            bool IsSuccess = false;

            string Sql_cmd = @"Insert Into [RoleInf] (RoleId,RoleNm,CrtUser,CrtDate)
                                values(@S_bsda_FieldId,@S_bsda_FieldName,@S_bsda_CreateId,getdate())";
            Hashtable ht1 = new Hashtable();

            ht1.Add("@S_bsda_FieldId", dr["RoleId"]);
            ht1.Add("@S_bsda_FieldName", dr["RoleNm"]);
            ht1.Add("@S_bsda_CreateId", UserId);
            IsSuccess = IO.SqlUpdate(Login_Server, Sql_cmd, ht1, ref SuccessCount);
            if (!IsSuccess)
                SuccessCount = 0;
            return SuccessCount;
        }

        //刪除
        public int RoleInf_Delete(string Login_Server, string UserId, string DelSeq)
        {
            int SuccessCount = 0;
            bool IsSuccess = false;

            string Sql_cmd = @"Update [RoleInf] set DelFlag=1, UpdUser=@id, UpdDate=getdate() where Sn=@seq";
            Hashtable ht1 = new Hashtable();
            ht1.Add("@seq", DelSeq);
            ht1.Add("@id", UserId);
            IsSuccess = IO.SqlUpdate(Login_Server, Sql_cmd, ht1, ref SuccessCount);
            if (!IsSuccess)
                SuccessCount = 0;
            return SuccessCount;
        }
        #endregion

        #endregion

        #region RoleFun
        /// <summary>
        /// 新增角色功能選單
        /// </summary>
        /// <param name="DB">資料庫</param>
        /// <param name="RoleId"></param>
        /// <param name="FunId"></param>
        /// <param name="PgId"></param>
        /// <param name="UserId"></param>
        /// <returns></returns>
        public bool AddRoleFun(string DB, string RoleId, string FunId, string PgId, string UserId)
        {
            bool boAddFun = false;
            Hashtable Prm = new Hashtable();
            int Count = 0;
            try
            {
                string SqlCmd = @" Insert Into dbo.RoleFun(RoleId,FunId,PgId,CrtUser,CrtDate,UpdUser,UpdDate) 
                                Values(@RoleId,@FunId,@PgId,@CrtUser,Getdate(),@CrtUser,Getdate())";

                Prm.Add("@RoleId", RoleId);
                Prm.Add("@FunId", FunId);
                Prm.Add("@PgId", PgId);
                Prm.Add("@CrtUser", UserId);
                boAddFun = IO.SqlUpdate(DB, SqlCmd, Prm, ref Count);
            }
            catch
            {
                boAddFun = false;
            }
            return boAddFun;
        }

        /// <summary>
        /// 移除角色功能選單
        /// </summary>
        /// <param name="DB"></param>
        /// <param name="RoleId"></param>
        /// <param name="FunId"></param>
        /// <param name="PgId"></param>
        /// <param name="UserId"></param>
        /// <returns></returns>
        public bool RemoveRoleFun(string DB, string RoleId, string FunId, string PgId, string UserId)
        {
            bool boReFun = false;
            Hashtable Prm = new Hashtable();
            int Count = 0;
            try
            {
                string SqlCmd = @"Delete dbo.RoleFun Where RoleId=@RoleId and FunId=@FunId and PgId=@PgId ";

                Prm.Add("@RoleId", RoleId);
                Prm.Add("@FunId", FunId);
                Prm.Add("@PgId", PgId);
                boReFun = IO.SqlUpdate(DB, SqlCmd, Prm, ref Count);
            }
            catch
            {
                boReFun = false;
            }
            return boReFun;
        }
        #endregion

        #region ClassRole
        /// <summary>
        /// 新增類別角色
        /// </summary>
        /// <param name="DB"></param>
        /// <param name="ClassId"></param>
        /// <param name="RoleId"></param>
        /// <param name="DC"></param>
        /// <param name="UserId"></param>
        /// <returns></returns>
        public bool AddClassRole(string DB, string ClassId, string RoleId, string DC, string UserId)
        {
            bool boClass = false;
            Hashtable Prm = new Hashtable();
            int Count = 0;
            try
            {
                string SqlCmd = @"Insert Into dbo.ClassRole(ClassId,RoleId,DC,CrtUser,CrtDate) Values(@ClassId,@RoleId,@DC,@CrtUser,Getdate())";
                Prm.Add("@ClassId", ClassId);
                Prm.Add("@RoleId", RoleId);
                Prm.Add("@DC", DC);
                Prm.Add("@CrtUser", UserId);
                boClass = IO.SqlUpdate(DB, SqlCmd, Prm, ref Count);
            }
            catch
            {
                boClass = false;
            }
            return boClass;
        }
        #endregion

        #region ClassInf
        /// <summary>
        /// 新增類別身分
        /// </summary>
        /// <param name="DB"></param>
        /// <param name="ClassId"></param>
        /// <param name="ClassName"></param>
        /// <param name="UserId"></param>
        /// <returns></returns>
        public bool AddClassInf(string DB, string ClassId, string ClassName, string UserId)
        {
            bool boClass = false;
            Hashtable Prm = new Hashtable();
            int Count = 0;
            try
            {
                string SqlCmd = @"Insert Into dbo.ClassInf(ClassId,ClassName,CrtUser,CrtDate) Values(@ClassId,@ClassName,@CrtUser,Getdate())";
                Prm.Add("@ClassId", ClassId);
                Prm.Add("@ClassName", ClassName);
                Prm.Add("@CrtUser", UserId);
                boClass = IO.SqlUpdate(DB, SqlCmd, Prm, ref Count);
            }
            catch
            {
                boClass = false;
            }
            return boClass;
        }
        #endregion

        #region FunList
        /// <summary>
        /// 取得FunList的主功能
        /// </summary>
        /// <param name="DB"></param>
        /// <returns></returns>
        public DataSet dsFunMain(string DB)
        {
            DataSet ds = new DataSet();
            Hashtable hs = new Hashtable();
            try
            {
                string SqlCom = @" Select Convert(varchar(10),FunOrd)+'-'+ Convert(varchar(10),FunId) Ord,FunId,FunNm from FunList Group By  FunOrd,FunId,FunNm ";
                ds = IO.SqlQuery(DB, SqlCom, hs);
            }
            catch
            { }
            return ds;
        }

        /// <summary>
        /// FunList清單
        /// </summary>
        /// <param name="DB">資料庫</param>
        /// <param name="strFunId">主功能ID</param>
        /// <param name="strPgId">程式ID</param>
        /// <returns></returns>
        public DataSet dsFunList(string DB, string strFunId, string strPgId, string strPgName, string strPgOrd)
        {
            DataSet ds = new DataSet();
            Hashtable hs = new Hashtable();
            try
            {
                string SqlCom = @" Select  
                                    FunOrd,
                                    FunId,
                                    FunNm,
                                    PgOrd,
                                    PgId,
                                    PgNm,
                                    PgUrl
                                    From dbo.FunList 
                                    Where 1=1 ";

                if (strFunId.Length > 0)
                {
                    SqlCom += " and FunId=@FunId ";
                    hs.Add("@FunId", strFunId);
                }
                if (strPgId.Length > 0)
                {
                    SqlCom += " and PgId=@PgId ";
                    hs.Add("@PgId", strPgId);
                }
                if (strPgName.Length > 0)
                {
                    SqlCom += " and PgNm like '%'+@PgNm+'%' ";
                    hs.Add("@PgNm", strPgName);
                }
                if (strPgOrd.Length > 0)
                {
                    SqlCom += " and PgOrd=@PgOrd ";
                    hs.Add("@PgOrd", strPgOrd);
                }
                SqlCom += " Order by FunOrd,PgOrd ";
                ds = IO.SqlQuery(DB, SqlCom, hs);
            }
            catch
            { }
            return ds;
        }

        /// <summary>
        /// 檢核選單順序
        /// </summary>
        /// <param name="DB">資料庫</param>
        /// <param name="strFunId">主功能Id</param>
        /// <param name="strPgId">程式Id</param>
        /// <param name="strPgName">程式名稱</param>
        /// <param name="strPgOrd"></param>
        /// <returns></returns>
        public DataSet dsCKFunOrd(string DB, string strFunId, string strPgId, string strPgOrd)
        {
            DataSet ds = new DataSet();
            Hashtable hs = new Hashtable();
            try
            {
                string SqlCom = @" Select  
                                    FunOrd,
                                    FunId,
                                    FunNm,
                                    PgOrd,
                                    PgId,
                                    PgNm,
                                    PgUrl
                                    From dbo.FunList 
                                    Where 1=1 ";

                if (strFunId.Length > 0)
                {
                    SqlCom += " and FunId=@FunId ";
                    hs.Add("@FunId", strFunId);
                }
                if (strPgId.Length > 0)
                {
                    SqlCom += " and PgId != @PgId ";
                    hs.Add("@PgId", strPgId);
                }
                if (strPgOrd.Length > 0)
                {
                    SqlCom += " and PgOrd=@PgOrd ";
                    hs.Add("@PgOrd", strPgOrd);
                }
                ds = IO.SqlQuery(DB, SqlCom, hs);
            }
            catch
            { }
            return ds;
        }
        #endregion

        #region 新刪修 程式清單
        /// <summary>
        /// 刪除程式清單
        /// </summary>
        /// <param name="DB">資料庫</param>
        /// <param name="FunId">主功能ID</param>
        /// <param name="PgId">程式ID</param>
        /// <returns></returns>
        public bool DelFunList(string DB, string FunId, string PgId)
        {
            bool boDel = false;
            string[] SQLCom = new string[2];
            Hashtable[] HS = new Hashtable[2];
            Hashtable Prm = new Hashtable();

            try
            {
                SQLCom[0] = "Delete FunList Where FunId =@FunId And PgId=@PgId ";
                Prm.Add("@FunId", FunId);
                Prm.Add("@PgId", PgId);

                SQLCom[1] = "Delete dbo.RoleFun Where FunId =@FunId And PgId=@PgId ";
                HS[0] = Prm;
                HS[1] = Prm;
                boDel = IO.SqlBeginTran("3PL", SQLCom, HS);
            }
            catch
            {
                boDel = false;
            }
            return boDel;
        }

        /// <summary>
        /// 異動程式清單
        /// </summary>
        /// <param name="DB">資料庫</param>
        /// <param name="FgId">程式ID</param>
        /// <param name="FunId">功能ID</param>
        /// <param name="FunName">功能名稱</param>
        /// <param name="PgOrd">程式順序</param>
        /// <param name="PgName">程式名稱</param>
        /// <param name="PgUrl">程式路徑</param>
        /// <returns></returns>
        public bool UpFunList(string DB, string FgId, string FunId, string PgOrd, string PgName, string PgUrl, string UserId)
        {
            bool blUp = false;
            Hashtable Prm = new Hashtable();
            int Count = 0;
            try
            {
                string SqlCmd = @"Update dbo.FunList set 
                                FunId=@FunId,
                                PgOrd=@PgOrd,
                                PgNm=@PgNm,
                                PgUrl=@PgUrl,
                                UpdUser=@UpdUser,
                                UpdDate=Getdate() 
                                Where FunId=@FunId and PgId=@PgId ";

                Prm.Add("@FunId", FunId);
                Prm.Add("@PgOrd", PgOrd);
                Prm.Add("@PgNm", PgName);
                Prm.Add("@PgUrl", PgUrl);
                Prm.Add("@PgId", FgId);
                Prm.Add("@UpdUser", UserId);
                blUp = IO.SqlUpdate(DB, SqlCmd, Prm, ref Count);
            }
            catch
            {
                blUp = false;
            }
            return blUp;
        }

        /// <summary>
        /// 新增程式清單
        /// </summary>
        /// <param name="DB"></param>
        /// <param name="FunOrd"></param>
        /// <param name="FunId"></param>
        /// <param name="FunNm"></param>
        /// <param name="PgOrd"></param>
        /// <param name="PgId"></param>
        /// <param name="PgNm"></param>
        /// <param name="PgUrl"></param>
        /// <param name="UserId"></param>
        /// <returns></returns>
        public bool InsFunList(string DB, string FunOrd, string FunId, string FunNm, string PgOrd, string PgId, string PgNm, string PgUrl, string UserId)
        {
            bool blIns = false;
            string[] SQLCom = new string[2];
            Hashtable[] Hs = new Hashtable[2];

            try
            {
                string SqlCmd1 = @"Insert Into FunList(FunOrd,FunId,FunNm,PgOrd,PgId,PgNm,PgUrl,CrtUser,CrtDate)
                                Values(@FunOrd,@FunId,@FunNm,@PgOrd,@PgId,@PgNm,@PgUrl,@CrtUser,GetDate())";
                Hashtable Prm1 = new Hashtable();
                Prm1.Add("@FunOrd", FunOrd);
                Prm1.Add("@FunId", FunId);
                Prm1.Add("@FunNm", FunNm);
                Prm1.Add("@PgOrd", PgOrd);
                Prm1.Add("@PgId", PgId);
                Prm1.Add("@PgNm", PgNm);
                Prm1.Add("@PgUrl", PgUrl);
                Prm1.Add("@CrtUser", UserId);

                string SqlCmd2 = @"Insert Into dbo.RoleFun(RoleId,FunId,PgId,CrtUser,CrtDate) 
                                    Values('admin',@FunId,@PgId,@CrtUser,GETDATE())";
                Hashtable Prm2 = new Hashtable();
                Prm2.Add("@FunId", FunId);
                Prm2.Add("@PgId", PgId);
                Prm2.Add("@CrtUser", UserId);

                SQLCom[0] = SqlCmd1;
                SQLCom[1] = SqlCmd2;
                Hs[0] = Prm1;
                Hs[1] = Prm2;
                blIns = IO.SqlBeginTran("3PL", SQLCom, Hs);
            }
            catch
            {
                blIns = false;
            }
            return blIns;
        }
        #endregion
    }
}
