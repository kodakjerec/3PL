using System.Data;
using _3PL_LIB;
using System.Collections;

namespace _3PL_DAO
{
    public partial class _3PL_BaseCostSet_DAO
    {
        DB_IO IO = new DB_IO();

        #region 查詢
        /// <summary>
        /// 查詢已建立的計價費用
        /// </summary>
        /// <param name="SiteNo">倉別</param>
        /// <param name="TypeId">報價主類別</param>
        /// <param name="SupdId">供應商代碼</param>
        /// <returns></returns>
        public DataTable PriceList_Query(string Login_Server, string SiteNo, string TypeId)
        {
            DataTable PriceList = new DataTable();

            string Sql_cmd = @"Select I_bcse_seq,S_bcse_SiteNo,S_bcse_CostName=convert(varchar,I_bcse_seq)+','+S_bcse_CostName,
                            I_bcse_TypeId, I_bcse_Price,S_bcse_DollarUnit,
                            I_bcse_UnitId,I_bcse_AccId,I_bcse_IsDbLink,I_bcse_IsDiscount,I_bcse_IsFormula,
                            I_bcse_IsPeriod,UnitName=b.S_bsda_FieldName,AccName=e.S_Acci_Id+','+e.S_Acci_Name,
                            TypeName=c.S_bsda_FieldName
                            From [3PL_BaseCostSet] a
							inner join [3PL_BaseData] b on a.I_bcse_UnitId=b.S_bsda_FieldId and b.S_bsda_CateId='UnitId'
                            inner join [3PL_BaseData] c on a.I_bcse_TypeId=c.S_bsda_FieldId and c.S_bsda_CateId='TypeId'
                            inner join [3PL_BaseAccounting] e on a.I_bcse_AccId=e.I_Acci_seq
                            Where S_bcse_SiteNo=@SiteNo and I_bcse_DelFlag=0";
            Hashtable ht1 = new Hashtable();
            ht1.Add("@SiteNo", SiteNo);
            //有選擇TypeId
            if (TypeId!="")
            {
                Sql_cmd += " and I_bcse_TypeId=@TypeId";
                ht1.Add("@TypeId", TypeId);
            }
            DataSet ds = IO.SqlQuery(Login_Server, Sql_cmd, ht1);
            PriceList = ds.Tables[0];

            return PriceList;
        }
        /// <summary>
        /// 查詢已建立的計價費用
        /// </summary>
        /// <param name="Login_Server">資料庫</param>
        /// <param name="SiteNo">倉別</param>
        /// <param name="TypeId">報價主類別</param>
        /// <param name="CostName">費用名稱</param>
        /// <returns></returns>
        public DataTable PriceList_Query(string Login_Server, string SiteNo, string TypeId, string CostName)
        {
            DataTable PriceList = new DataTable();

            string Sql_cmd = @"Select I_bcse_seq,S_bcse_SiteNo, S_bcse_CostName=convert(varchar,I_bcse_seq)+','+S_bcse_CostName,
                            I_bcse_TypeId, I_bcse_Price,S_bcse_DollarUnit,
                            I_bcse_UnitId,I_bcse_AccId,I_bcse_IsDbLink,I_bcse_IsDiscount,I_bcse_IsFormula,
                            I_bcse_IsPeriod,UnitName=b.S_bsda_FieldName,AccName=e.S_Acci_Id+','+e.S_Acci_Name,
                            TypeName=c.S_bsda_FieldName
                            From [3PL_BaseCostSet] a
							inner join [3PL_BaseData] b on a.I_bcse_UnitId=b.S_bsda_FieldId and b.S_bsda_CateId='UnitId'
                            inner join [3PL_BaseData] c on a.I_bcse_TypeId=c.S_bsda_FieldId and c.S_bsda_CateId='TypeId'
                            inner join [3PL_BaseAccounting] e on a.I_bcse_AccId=e.I_Acci_seq
                            Where S_bcse_SiteNo=@SiteNo and I_bcse_TypeId=@TypeId and S_bcse_CostName=@CostName and I_bcse_DelFlag=0";
            Hashtable ht1 = new Hashtable();
            ht1.Add("@SiteNo", SiteNo);
            ht1.Add("@CostName", CostName);
            ht1.Add("@TypeId", TypeId);
            DataSet ds = IO.SqlQuery(Login_Server, Sql_cmd, ht1);
            PriceList = ds.Tables[0];

            return PriceList;
        }

        /// <summary>
        /// 查詢已建立的計價費用
        /// </summary>
        /// <param name="I_bcse_seq">系統序號</param>
        /// <returns></returns>
        public DataTable PriceList_Query(string Login_Server, string I_bcse_seq)
        {
            DataTable PriceList = new DataTable();

            string Sql_cmd = @"Select I_bcse_seq,S_bcse_SiteNo, S_bcse_CostName, I_bcse_TypeId, I_bcse_Price,S_bcse_DollarUnit,
                            I_bcse_UnitId,I_bcse_AccId,I_bcse_IsDbLink,I_bcse_IsDiscount,I_bcse_IsFormula,I_bcse_IsPeriod,
                            UnitName=b.S_bsda_FieldName,AccName=e.S_Acci_Id+','+e.S_Acci_Name,
                            TypeName=c.S_bsda_FieldName
                            From [3PL_BaseCostSet] a
							inner join [3PL_BaseData] b on a.I_bcse_UnitId=b.S_bsda_FieldId and b.S_bsda_CateId='UnitId'
                            inner join [3PL_BaseData] c on a.I_bcse_TypeId=c.S_bsda_FieldId and c.S_bsda_CateId='TypeId'
                            inner join [3PL_BaseAccounting] e on a.I_bcse_AccId=e.I_Acci_seq
                            Where I_bcse_seq=@seq and I_bcse_DelFlag=0";
            Hashtable ht1 = new Hashtable();
            ht1.Add("@seq", I_bcse_seq);

            DataSet ds = IO.SqlQuery(Login_Server, Sql_cmd, ht1);
            PriceList = ds.Tables[0];

            return PriceList;
        }

        /// <summary>
        /// 查詢已建立計價費用的處理單位一覽表
        /// </summary>
        /// <param name="Login_Server"></param>
        /// <param name="I_bcse_seq"></param>
        /// <returns></returns>
        public DataTable GetMyClassList(string Login_Server, string I_bcse_seq)
        {
            DataTable PriceList = new DataTable();

            string Sql_cmd = @"SELECT [Sn],[I_bcse_Seq],[ClassId],[ClassName],[UIStatus]='Unchanged'
            FROM [3PL_BaseCostSet_ClassList]
            where I_bcse_Seq=@seq order by ClassId";
            Hashtable ht1 = new Hashtable();
            ht1.Add("@seq", I_bcse_seq);

            DataSet ds = IO.SqlQuery(Login_Server, Sql_cmd, ht1);
            PriceList = ds.Tables[0];

            //加入primaryKey
            DataColumn[] keys = new DataColumn[2];
            keys[0] = PriceList.Columns["I_bcse_Seq"];
            keys[1] = PriceList.Columns["ClassId"];
            PriceList.PrimaryKey = keys;

            return PriceList;
        }
        #endregion

        #region 新刪修_基本價格
        //刪除
        public int PriceList_Delete(string Login_Server, string UserId, string I_bcse_seq)
        {
            int DeleteCount = 0;

            string Sql_cmd = @"Update [3PL_BaseCostSet] set I_bcse_DelFlag=1,S_bcse_UpdId=@id
                            Where I_bcse_seq=@seq";
            Hashtable ht1 = new Hashtable();
            ht1.Add("@seq", I_bcse_seq);
            ht1.Add("@id", UserId);

            bool IsDeleteSuccess = IO.SqlUpdate(Login_Server, Sql_cmd, ht1, ref DeleteCount);
            if (!IsDeleteSuccess)
                DeleteCount = 0;
            return DeleteCount;
        }
        //新增
        public int PriceList_New(string Login_Server,string UserId, DataRow dr)
        {
            int UpdateCount = 0;
            DataTable PriceList = new DataTable();

            string Sql_cmd = @"Insert Into [3PL_BaseCostSet] (S_bcse_SiteNo,S_bcse_CostName,I_bcse_TypeId,I_bcse_Price,S_bcse_DollarUnit,I_bcse_UnitId
                ,I_bcse_AccId,I_bcse_IsDbLink,I_bcse_IsDiscount,I_bcse_IsFormula,I_bcse_IsPeriod,S_bcse_CreateId,S_bcse_UpdId)
                values(@S_bcse_SiteNo,@S_bcse_CostName,@I_bcse_TypeId,@I_bcse_Price,@S_bcse_DollarUnit,@I_bcse_UnitId
                ,@I_bcse_AccId,@I_bcse_IsDbLink,@I_bcse_IsDiscount,@I_bcse_IsFormula,@I_bcse_IsPeriod,@S_bcse_CreateId,@S_bcse_UpdId)";
            Hashtable ht1 = new Hashtable();
            ht1.Add("@S_bcse_SiteNo", dr["S_bcse_SiteNo"]);
            ht1.Add("@S_bcse_CostName", dr["S_bcse_CostName"]);
            ht1.Add("@I_bcse_TypeId", dr["I_bcse_TypeId"]);
            ht1.Add("@I_bcse_Price", dr["I_bcse_Price"]);
            ht1.Add("@S_bcse_DollarUnit", dr["S_bcse_DollarUnit"]);
            ht1.Add("@I_bcse_UnitId", dr["I_bcse_UnitId"]);
            ht1.Add("@I_bcse_AccId", dr["I_bcse_AccId"]);
            ht1.Add("@I_bcse_IsDbLink", dr["I_bcse_IsDbLink"]);
            ht1.Add("@I_bcse_IsDiscount", dr["I_bcse_IsDiscount"]);
            ht1.Add("@I_bcse_IsFormula", dr["I_bcse_IsFormula"]);
            ht1.Add("@I_bcse_IsPeriod", dr["I_bcse_IsPeriod"]);
            ht1.Add("@S_bcse_CreateId", UserId);
            ht1.Add("@S_bcse_UpdId", UserId);

            bool IsDeleteSuccess = IO.SqlUpdate(Login_Server, Sql_cmd, ht1, ref UpdateCount);
            if (!IsDeleteSuccess)
                UpdateCount = 0;
            return UpdateCount;
        }
        //更新
        public int PriceList_Update(string Login_Server, string UserId, DataRow dr)
        {
            int UpdateCount = 0;
            DataTable PriceList = new DataTable();

            string Sql_cmd = @"Update [3PL_BaseCostSet] 
                set S_bcse_CostName=@S_bcse_CostName,I_bcse_Price=@I_bcse_Price,S_bcse_DollarUnit=@S_bcse_DollarUnit,I_bcse_UnitId=@I_bcse_UnitId
                ,I_bcse_AccId=@I_bcse_AccId,I_bcse_IsDbLink=@I_bcse_IsDbLink,I_bcse_IsDiscount=@I_bcse_IsDiscount,I_bcse_IsFormula=@I_bcse_IsFormula,I_bcse_IsPeriod=@I_bcse_IsPeriod,S_bcse_UpdId=@S_bcse_UpdId
                where I_bcse_seq=@I_bcse_seq";
            Hashtable ht1 = new Hashtable();
            ht1.Add("@S_bcse_CostName", dr["S_bcse_CostName"]);
            ht1.Add("@I_bcse_Price", dr["I_bcse_Price"]);
            ht1.Add("@S_bcse_DollarUnit", dr["S_bcse_DollarUnit"]);
            ht1.Add("@I_bcse_UnitId", dr["I_bcse_UnitId"]);
            ht1.Add("@I_bcse_AccId", dr["I_bcse_AccId"]);
            ht1.Add("@I_bcse_IsDbLink", dr["I_bcse_IsDbLink"]);
            ht1.Add("@I_bcse_IsDiscount", dr["I_bcse_IsDiscount"]);
            ht1.Add("@I_bcse_IsFormula", dr["I_bcse_IsFormula"]);
            ht1.Add("@I_bcse_IsPeriod", dr["I_bcse_IsPeriod"]);
            ht1.Add("@S_bcse_UpdId", UserId);
            ht1.Add("@I_bcse_seq", dr["I_bcse_seq"]);

            bool IsDeleteSuccess = IO.SqlUpdate(Login_Server, Sql_cmd, ht1, ref UpdateCount);
            if (!IsDeleteSuccess)
                UpdateCount = 0;
            return UpdateCount;
        }

        #endregion

        #region 新增/修改_處理單位
        /// <summary>
        /// 處理單位的更新
        /// </summary>
        /// <param name="Login_Server">資料庫</param>
        /// <param name="dt">DataTable</param>
        /// <returns></returns>
        public int MyClassList_Update(string Login_Server, DataTable dt)
        {
            string RowStatus = "";
            foreach (DataRow dr in dt.Rows)
            {
                RowStatus = dr["UIStatus"].ToString();
                if (RowStatus == "Added")
                {
                    MyClassList_Add(Login_Server,dr);
                }
                else if (RowStatus == "Deleted")
                { 
                    MyClassList_Delete(Login_Server,dr);
                }
            }
            return 1;
        }
        //新增處理單位
        private void MyClassList_Add(string Login_Server, DataRow dr)
        {
            int SuccessCount_head = 0;
            string Delcmd_head =
            @"Insert Into [3PL_BaseCostSet_ClassList](I_bcse_seq,ClassId,ClassName)
            values(@seq,@Id,@Name)";
            Hashtable ht1 = new Hashtable();
            ht1.Add("@seq", dr["I_bcse_seq"]);
            ht1.Add("@Id", dr["ClassId"]);
            ht1.Add("@Name", dr["ClassName"]);

            IO.SqlUpdate(Login_Server, Delcmd_head, ht1, ref SuccessCount_head);
        }
        //刪除處理單位
        private void MyClassList_Delete(string Login_Server, DataRow dr)
        {
            int SuccessCount_head = 0;
            string Delcmd_head =
            @"Delete from [3PL_BaseCostSet_ClassList] 
            where Sn=@Sn";
            Hashtable ht1 = new Hashtable();
            ht1.Add("@Sn", dr["Sn"]);

            IO.SqlUpdate(Login_Server, Delcmd_head, ht1, ref SuccessCount_head);
        }
        #endregion
    }
}
