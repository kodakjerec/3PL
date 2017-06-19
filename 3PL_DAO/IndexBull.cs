using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Data;
using _3PL_LIB;

namespace _3PL_DAO
{
    public class IndexBull
    {
        /// <summary>
        /// 資訊公告
        /// </summary>
        /// <param name="DB"></param>
        /// <returns></returns>
        public DataSet dsBull(string DB)
        {
            DB_IO IO = new DB_IO();
            DataSet ds = new DataSet();
            Hashtable Hs=new Hashtable();
            try 
            {
                string SqlCom = @"Select convert(varchar(10),Bull_Day,121) Bull_Day,Detail,Bull_Member From dbo.Bulletin 
                                Where  Del_status='0' 
                                and GETDATE() Between Eff_DateS and Eff_DateE
                                Order by Sn Desc";
                ds = IO.SqlQuery(DB, SqlCom, Hs);
            }
            catch
            { }
            return ds;
        }

        /// <summary>
        /// 新增公告
        /// </summary>
        /// <param name="DB">資料庫名稱</param>
        /// <param name="Bull_Day">發佈日期</param>
        /// <param name="Detail">公告內容</param>
        /// <param name="Bull_Member">公告人員</param>
        /// <param name="Eff_DateS">有效起日</param>
        /// <param name="Eff_DateE">有效迄日</param>
        /// <param name="CrtUser">建檔人員</param>
        /// <returns></returns>
        public bool InsBull(string DB,string Bull_No,string Bull_Day,string Detail,string Bull_Member,string Eff_DateS,string Eff_DateE,string CrtUser)
        {
            DB_IO IO = new DB_IO();
            bool blIns = false;
            Hashtable Hs=new Hashtable();
            int intCount=0;
            try 
            {
                string SqlCom = @"Insert Into dbo.Bulletin(Bull_No,Bull_Day,Detail,Bull_Member,Eff_DateS,Eff_DateE,CrtUser,CrtDate,UpdUser,UpdDate)
                                    Values(@BullNo,@Bull_Day,@Detail,@Bull_Member,@Eff_DateS,@Eff_DateE,@CrtUser,GETDATE(),@UpdUser,GETDATE()) ";


                Hs.Add("@BullNo", Bull_No);
                Hs.Add("@Bull_Day",Bull_Day);
                Hs.Add("@Detail",Detail);
                Hs.Add("@Bull_Member",Bull_Member);
                Hs.Add("@Eff_DateS",Eff_DateS);
                Hs.Add("@Eff_DateE",Eff_DateE);
                Hs.Add("@CrtUser",CrtUser);
                Hs.Add("@UpdUser",CrtUser);
                blIns = IO.SqlUpdate(DB, SqlCom, Hs, ref intCount);
                if (intCount > 0)
                {
                    blIns = true;
                }
                else
                {
                    blIns = false;
                }
            }
            catch
            {
                blIns = false;
            }
            return blIns;
        }

        /// <summary>
        /// 查詢公告內容
        /// </summary>
        /// <param name="DB">資料庫名稱</param>
        /// <param name="Bull_Day">發佈日期</param>
        /// <param name="Bull_No">公告編號</param>
        /// <returns></returns>
        public DataSet dsBullList(string DB, string Bull_Day,string Bull_No)
        {
            DB_IO IO = new DB_IO();
            DataSet ds = new DataSet();
            Hashtable Hs = new Hashtable();
            try 
            {
                string SqlCom = @"Select Sn,
                                Bull_No,
                                Bull_Day,
                                Detail,
                                SUBSTRING(Detail,1,7)+'...' Memo,
                                Bull_Member,
                                Eff_DateS,
                                Eff_DateE  
                                From dbo.Bulletin Where  Del_status='0' ";
                if (Bull_Day.Length > 0)
                {
                    SqlCom += "And Bull_Day = @BullDay ";
                    Hs.Add("@BullDay", Bull_Day);
                }
                if (Bull_No.Length > 0)
                {
                    SqlCom += "And Bull_No like @Bull_No+'%' ";
                    Hs.Add("@Bull_No", Bull_No);
                }
                SqlCom += " Order by UpdDate Desc ";
                ds = IO.SqlQuery(DB, SqlCom, Hs);
            }
            catch
            {
            }
            return ds;
        }

        /// <summary>
        /// 更新公告內容
        /// </summary>
        /// <param name="DB">資料庫名稱</param>
        /// <param name="Bull_No">公告編號</param>
        /// <param name="Bull_Day">發佈日期</param>
        /// <param name="Detail">公告內容</param>
        /// <param name="Bull_Member">公告人員</param>
        /// <param name="Eff_DateS">有效起日</param>
        /// <param name="Eff_DateE">有效迄日</param>
        /// <param name="CrtUser"></param>
        /// <returns></returns>
        public bool UpBull(string DB, string Bull_No, string Bull_Day, string Detail, string Bull_Member, string Eff_DateS, string Eff_DateE, string CrtUser)
        {
            DB_IO IO = new DB_IO();
            bool blUpdate = false;
            Hashtable Hs = new Hashtable();
            int intCount = 0;
            try
            {
                string SqlCom = @"Update dbo.Bulletin 
                            Set Bull_Day=@Bull_Day,
                            Detail=@Detail,
                            Bull_Member=@Bull_Member,
                            Eff_DateS=@Eff_DateS,
                            Eff_DateE=@Eff_DateE,
                            UpdUser=@UpdUser,
                            UpdDate=Getdate()  
                            where Bull_No=@Bull_No ";

                Hs.Add("@Bull_Day", Bull_Day);
                Hs.Add("@Detail", Detail);
                Hs.Add("@Bull_Member", Bull_Member);
                Hs.Add("@Eff_DateS", Eff_DateS);
                Hs.Add("@Eff_DateE", Eff_DateE);
                Hs.Add("@UpdUser", CrtUser);
                Hs.Add("@Bull_No", Bull_No);
                blUpdate = IO.SqlUpdate(DB, SqlCom, Hs, ref intCount);
                if (intCount > 0)
                {
                    blUpdate = true;
                }
                else
                {
                    blUpdate = false;
                }
            }
            catch
            {
                blUpdate = false;
            }
            return blUpdate;
        }

        /// <summary>
        ///  刪除公告
        /// </summary>
        /// <param name="DB">資料庫名稱</param>
        /// <param name="BullNo">公告單號</param>
        /// <param name="Status">刪除狀態 0:正常 1:刪除</param>
        /// <param name="UpUser">異動人員</param>
        /// <returns></returns>
        public bool DelBull(string DB,string BullNo,string Status,string UpUser)
        {
            bool blDel = false;
            DB_IO IO = new DB_IO();
            Hashtable Hs = new Hashtable();
            int intCount = 0;
            try 
            {
                string SqlCom = @"Update dbo.Bulletin 
                            Set Del_status=@Del_status,
                            UpdUser=@UpdUser,
                            UpdDate=Getdate()  
                            where Bull_No=@Bull_No ";

                Hs.Add("@Del_status", Status);
                Hs.Add("@UpdUser", UpUser);
                Hs.Add("@Bull_No", BullNo);
                blDel = IO.SqlUpdate(DB, SqlCom, Hs, ref intCount);
                if (intCount > 0)
                {
                    blDel = true;
                }
                else
                {
                    blDel = false;
                }
            }
            catch
            {
                blDel = false;
            }
            return blDel;
        }
        
    }
}
