using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Data;
using _3PL_LIB;

namespace _3PL_DAO
{
    public class EmpInf
    {
        /// <summary>
        /// 取得User資訊，包含資訊管理員
        /// </summary>
        /// <param name="DB">資料庫</param>
        /// <param name="strId">帳號</param>
        /// <param name="strPsw">密碼</param>
        /// <returns></returns>
        public DataSet dsEmpInf(string DB, string strId, string strPsw)
        {
            DataSet ds = new DataSet();
            Hashtable hs = new Hashtable();
            DB_IO IO = new DB_IO();
            try
            {
                string SqlCom = @"Select 
                                E.WorkId,
                                E.WorkName,
                                E.WorkPsw,
                                E.Email,
                                C.ClassId,
                                C.ClassName
                                From EmpInf E with(nolock)
                                Join EmpClass EC with(nolock) On EC.WorkId=E.WorkId
                                Join ClassInf C with(nolock) On C.ClassId=EC.ClassId
                                Where E.DelStatus='0' and E.WorkId= @WorkId";

                hs.Add("@WorkId", strId);
                if (strPsw.Length > 0)
                {
                    SqlCom += " And E.WorkPsw= @WorkPsw ";
                    hs.Add("@WorkPsw", strPsw);
                }
                ds = IO.SqlQuery(DB, SqlCom, hs);
            }
            catch
            {
            }
            return ds;
        }

        /// <summary>
        ///  取得User清單資訊，不包含資訊管理員
        /// </summary>
        /// <param name="DB"></param>
        /// <param name="strId"></param>
        /// <returns></returns>
        public DataSet dsEmpList(string DB, string strId,string ClassID,string WorkName)
        {
            DataSet ds = new DataSet();
            Hashtable hs = new Hashtable();
            DB_IO IO = new DB_IO();
            try
            {
                string SqlCom = @"Select 
                                E.WorkId,
                                E.WorkName,
                                E.WorkPsw,
                                E.Email,
                                C.ClassId,
                                C.ClassName
                                From EmpInf E with(nolock)
                                Join EmpClass EC with(nolock) On EC.WorkId=E.WorkId
                                Join ClassInf C with(nolock) On C.ClassId=EC.ClassId
                                Where E.DelStatus='0' and C.ClassId != '000000' ";

                if (strId.Length > 0)
                {
                    SqlCom += " and E.WorkId= @WorkId";
                    hs.Add("@WorkId", strId);
                }
                if (ClassID.Length > 0)
                {
                    SqlCom += " and C.ClassId= @ClassId";
                    hs.Add("@ClassId", ClassID);
                }
                if (WorkName.Length > 0)
                {
                    SqlCom += " and E.WorkName like '%'+@ClassId+ '%' ";
                    hs.Add("@ClassId", WorkName);
                }
                
                ds = IO.SqlQuery(DB, SqlCom, hs);
            }
            catch
            {
            }
            return ds;
        }

        /// <summary>
        /// 使用者角色權限
        /// </summary>
        /// <param name="DB">資料庫</param>
        /// <param name="strID">帳號</param>
        /// <param name="RoleId">權限代碼</param>
        /// <param name="DC">倉別</param>
        /// <returns></returns>
        public DataSet dsRoleClass_EI(string DB, string strID, string RoleId, string DC)
        {
            DataSet ds = new DataSet();
            Hashtable hs = new Hashtable();
            DB_IO IO = new DB_IO();
            try
            {
                string SqlCom = @"Select EI.WorkId,EI.WorkName,CI.ClassId,CI.ClassName,RI.RoleId,RI.RoleNm,CR.DC,WI.DCNm 
                                    From EmpInf EI with(nolock)
                                    Left Join EmpClass EC with(nolock) On EC.WorkId=EI.WorkId
                                    Left Join ClassInf CI with(nolock) On CI.ClassId=EC.ClassId
                                    Left Join ClassRole CR with(nolock) On CR.ClassId=EC.ClassId
                                    Left Join RoleInf RI with(nolock) On RI.RoleId= CR.RoleId
                                    Left Join WareInf WI with(nolock) On WI.DC=CR.DC
                                    Where EI.WorkId=@WorkId ";

                hs.Add("@WorkId", strID);
                if (RoleId.Length > 0)
                {
                    SqlCom += " RI.RoleId=@RoleId ";
                    hs.Add("@RoleId", RoleId);
                }
                if (DC.Length > 0)
                {
                    SqlCom += "  CR.DC = @DC ";
                    hs.Add("@DC", DC);
                }
                ds = IO.SqlQuery(DB, SqlCom, hs);
            }
            catch
            { }
            return ds;
        }

        /// <summary>
        /// 取得MenuTree子功能
        /// </summary>
        /// <param name="DB">資料庫</param>
        /// <param name="strID">帳號</param>
        /// <returns></returns>
        public DataSet dsPgmTree(string DB, string strID)
        {
            DataSet ds = new DataSet();
            Hashtable hs = new Hashtable();
            DB_IO IO = new DB_IO();
            try
            {
                string SqlCom = @"Select MAX(F.Sn) as Sn,F.FunId,F.PgId,convert(varchar(10),F.FunOrd)+'-'+convert(varchar(10),F.PgOrd)+' '+F.PgNm as PgNm,F.PgUrl 
                                From EmpInf EI with(nolock)
                                Left Join dbo.EmpClass EC with(nolock) On EC.WorkId=EI.WorkId
                                Left Join dbo.ClassRole CR with(nolock) On CR.ClassId=EC.ClassId
                                Left Join dbo.RoleFun RF with(nolock) On RF.RoleId=CR.RoleId
                                Left Join dbo.FunList F with(nolock) On F.FunId=RF.FunId and F.PgId=RF.PgId
                                Where EI.WorkId=@WorkId 
                                Group By F.FunId,F.PgId,F.PgNm,F.PgUrl,F.FunOrd,F.PgOrd 
                                Order by F.FunOrd,F.FunId,F.PgOrd ";

                hs.Add("@WorkId", strID);
                ds = IO.SqlQuery(DB, SqlCom, hs);
            }
            catch
            { }
            return ds;
        }

        /// <summary>
        /// 取得MenuTree父抬頭
        /// </summary>
        /// <param name="DB"></param>
        /// <param name="ID"></param>
        /// <returns></returns>
        public DataSet dsParentsTree(string DB, string strID)
        {
            DataSet ds = new DataSet();
            Hashtable hs = new Hashtable();
            DB_IO IO = new DB_IO();
            try
            {
                string SqlCom = @"Select  F.FunId,convert(varchar(10),F.FunOrd)+' '+ F.FunNm as FunNm
                                From EmpInf EI with(nolock)
                                Left Join dbo.EmpClass EC with(nolock) On EC.WorkId=EI.WorkId
                                Left Join dbo.ClassRole CR with(nolock) On CR.ClassId=EC.ClassId
                                Left Join dbo.RoleFun RF with(nolock) On RF.RoleId=CR.RoleId
                                Left Join dbo.FunList F with(nolock) On F.FunId=RF.FunId and F.PgId=RF.PgId
                                Where EI.WorkId=@WorkId 
                                Group By  F.FunId,F.FunNm,F.FunOrd  
                                Order by F.FunOrd, F.FunId ";

                hs.Add("@WorkId", strID);
                ds = IO.SqlQuery(DB, SqlCom, hs);
            }
            catch
            { }
            return ds;
        }

        /// <summary>
        /// 身分類別清單
        /// </summary>
        /// <param name="DB">資料庫</param>
        /// <param name="ClassId">類別代號</param>
        /// <returns></returns>
        public DataSet dsClassInf(string DB, string ClassId,string ClassName)
        {
            DataSet ds = new DataSet();
            Hashtable hs = new Hashtable();
            DB_IO IO = new DB_IO();
            try
            {
                string SqlCom = @" Select ClassId,ClassName 
                                    From ClassInf with(nolock)
                                    Where  ClassId != '000000' ";

                if (ClassId.Length > 0)
                {
                    SqlCom += " and  ClassId = @ClassId ";
                    hs.Add("@ClassId", ClassId);
                }
                if (ClassName.Length > 0)
                {
                    SqlCom += " and ClassName like '%'+@ClassName+'%' ";
                    hs.Add("@ClassName", ClassName);
                }
                ds = IO.SqlQuery(DB, SqlCom, hs);
            }
            catch
            { }
            return ds;
        }

        /// <summary>
        /// 取得最新類別代碼
        /// </summary>
        /// <param name="DB"></param>
        /// <returns></returns>
        public DataSet dsGetClassId(string DB)
        {
            DataSet ds = new DataSet();
            Hashtable hs = new Hashtable();
            DB_IO IO = new DB_IO();
            try
            {
                string SqlCom = @" Select Top 1 ClassId ClassId,ClassName 
                                    From ClassInf with(nolock)
                                    Where  ClassId != '000000' Order by ClassId Desc ";
                ds = IO.SqlQuery(DB, SqlCom, hs);
            }
            catch
            { }
            return ds;
        }

        /// <summary>
        /// 身分類別權限
        /// </summary>
        /// <param name="DB"></param>
        /// <param name="ClassId"></param>
        /// <returns></returns>
        public DataSet dsClassRole(string DB,string ClassId)
        {
            DataSet ds = new DataSet();
            Hashtable hs = new Hashtable();
            DB_IO IO = new DB_IO();
            try 
            {
                string SqlCom = @"Select CI.ClassId,CI.ClassName,RI.RoleId,RI.RoleNm,WI.DC,WI.DCNm 
                                From ClassInf  CI with(nolock)
                                Inner Join ClassRole CR with(nolock) On CR.ClassId=CI.ClassId
                                Inner Join RoleInf RI with(nolock) On RI.RoleId= CR.RoleId
                                Inner Join WareInf WI with(nolock) On WI.DC=CR.DC
                                Where CI.ClassId != '000000' ";
                if (ClassId.Length>0)
                {
                    SqlCom += " And CI.ClassId= @ClassId ";
                    hs.Add("@ClassId", ClassId);
                }
                ds = IO.SqlQuery(DB, SqlCom, hs);
            }
            catch
            {
 
            }
            return ds;
        }

        /// <summary>
        /// 新增人員
        /// </summary>
        /// <param name="DB">資料庫</param>
        /// <param name="ID">帳號</param>
        /// <param name="Name">姓名</param>
        /// <param name="Psw">密碼</param>
        /// <param name="Email">E-Mail</param>
        /// <param name="ClassId">身分類別</param>
        /// <param name="CrtUser">建檔人員</param>
        /// <returns></returns>
        public bool InsEmp(string DB, string ID, string Name, string Psw, string Email, string ClassID, string CrtUser)
        {
            DB_IO IO = new DB_IO();
            bool blIns = false;
            string[] SqlCmd = new string[2];
            Hashtable[] Hs = new Hashtable[2];
            Hashtable hs1 = new Hashtable();
            Hashtable hs2 = new Hashtable();
            try
            {
                SqlCmd[0] = @"Insert Into EmpInf(WorkId,WorkName,WorkPsw,Email,CrtUser,Crtdate,UpdUser,UpdDate)
                                                 Values(@WorkId,@WorkName,@WorkPsw,@Email,@CrtUser,GETDATE(),@UpdUser,GETDATE())";
                hs1.Add("@WorkId", ID);
                hs1.Add("@WorkName", Name);
                hs1.Add("@WorkPsw", Psw);
                hs1.Add("@Email", Email);
                hs1.Add("@CrtUser", CrtUser);
                hs1.Add("@UpdUser", CrtUser);

                SqlCmd[1] = "Insert Into EmpClass(WorkId,ClassId,CrtUser,Crtdate,UpdUser,UpdDate) Values(@WorkId,@ClassId,@CrtUser,GETDATE(),@UpdUser,GETDATE()) ";
                hs2.Add("@WorkId", ID);
                hs2.Add("@ClassID", ClassID);
                hs2.Add("@CrtUser", CrtUser);
                hs2.Add("@UpdUser", CrtUser);
                Hs[0] = hs1;
                Hs[1] = hs2;
                blIns = IO.SqlBeginTran("3PL", SqlCmd, Hs);
            }
            catch
            {
                blIns = false;
            }
            return blIns;
        }

        /// <summary>
        /// 更新人員資料
        /// </summary>
        /// <param name="DB">資料庫</param>
        /// <param name="ID">帳號</param>
        /// <param name="Name">姓名</param>
        /// <param name="Psw">密碼</param>
        /// <param name="Email">E-Mail</param>
        /// <param name="ClassId">身分類別</param>
        /// <param name="CrtUser">建檔人員</param>
        /// <returns></returns>
        public bool UpdEmp(string DB, string ID, string Name, string Psw, string Email, string ClassID, string UpdUser)
        {
            DB_IO IO = new DB_IO();
            bool blUpd = false;
            string[] SqlCmd = new string[2];
            Hashtable[] Hs = new Hashtable[2];
            Hashtable hs1 = new Hashtable();
            Hashtable hs2 = new Hashtable();
            try
            {
                SqlCmd[0] = @"Update EmpInf Set 
                            WorkName=@WorkName,
                            WorkPsw=@WorkPsw,
                            Email=@Email,
                            UpdUser=@UpdUser,
                            UpdDate=GETDATE()
                            Where WorkId=@WorkId ";
                hs1.Add("@WorkId", ID);
                hs1.Add("@WorkName", Name);
                hs1.Add("@WorkPsw", Psw);
                hs1.Add("@Email", Email);
                hs1.Add("@UpdUser", UpdUser);


                SqlCmd[1] = @"Update dbo.EmpClass Set ClassId=@ClassId,UpdUser=@UpdUser,UpdDate=GETDATE()
                            Where WorkId =@WorkId  ";
                hs2.Add("@WorkId", ID);
                hs2.Add("@ClassID", ClassID);
                hs2.Add("@UpdUser", UpdUser);
                Hs[0] = hs1;
                Hs[1] = hs2;
                blUpd = IO.SqlBeginTran("3PL", SqlCmd, Hs);
            }
            catch
            {
                blUpd = false;
            }
            return blUpd;
        }


        public bool UpdPsw(string DB, string ID, string Psw)
        {
            DB_IO IO = new DB_IO();
            bool blUpd = false;
            Hashtable hs = new Hashtable();
            int Cnt=0;

            try 
            {
                string SqlCmd = "Update dbo.EmpInf Set WorkPsw=@WorkPsw Where WorkId=@WorkId ";
                hs.Add("@WorkId", ID);
                hs.Add("@WorkPsw",Psw);
                blUpd = IO.SqlUpdate(DB, SqlCmd, hs, ref Cnt);
            }
            catch
            {
                blUpd = false;
            }
            return blUpd;
        }

        /// <summary>
        /// 刪除使用者
        /// </summary>
        /// <param name="DB"></param>
        /// <param name="ID"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public bool DelEmp(string DB, string ID, string Status, string UpdUser)
        {
            bool booDel = false;
            DB_IO IO = new DB_IO();
            string[] SqlCmd = new string[2];
            Hashtable[] Hs = new Hashtable[2];
            Hashtable hs1 = new Hashtable();
            Hashtable hs2 = new Hashtable();
            try
            {
                SqlCmd[0] = @"Update EmpInf Set 
                            DelStatus=@Status,
                            UpdUser=@UpdUser,
                            UpdDate=GETDATE()
                            Where WorkId=@WorkId ";
                hs1.Add("@WorkId", ID);
                hs1.Add("@Status", Status);
                hs1.Add("@UpdUser", UpdUser);

                SqlCmd[1] = "Delete dbo.EmpClass Where WorkId=@WorkId ";
                hs2.Add("@WorkId", ID);
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

    }
}
