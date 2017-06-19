using System.Data;
using _3PL_LIB;
using System.Collections;

namespace _3PL_DAO
{
    public partial class _3PL_BaseCost_DAO
    {
        DB_IO IO = new DB_IO();

        #region 查詢
        //取得成本費用一覽表
        public DataTable BaseCost_Query(string Login_Server, string SiteNo, string TypeId, string CostNameSeq)
        {
            DataTable PriceList = new DataTable();

            string Sql_cmd = @"select a.I_basc_seq, TypeIdName=c.S_bsda_FieldName,	BaseCostSetName=b.S_bcse_CostName,	c1.S_bsda_FieldName,
	        a.I_basc_seq,a.S_basc_CostName,a.I_basc_Free,a.S_basc_DollarUnit, c2.S_bsda_FieldName,b.I_bcse_TypeId,b.S_bcse_SiteNo,I_basc_UnitId,S_basc_CostType
            from [3PL_BaseCost] a
            inner join [3PL_BaseCostSet] b on a.I_basc_bcseseq=b.I_bcse_Seq
            inner join [3PL_BaseData] c on b.I_bcse_TypeId=c.S_bsda_FieldId and c.S_bsda_CateId='TypeId'
            inner join [3PL_BaseData] c1 on a.S_basc_CostType=c1.S_bsda_FieldId and c1.S_bsda_CateId='CostType'
            inner join [3PL_BaseData] c2 on a.I_basc_UnitId	=c2.S_bsda_FieldId and c2.S_bsda_CateId='UnitId'
            where b.S_bcse_SiteNo=@SiteNo and b.I_bcse_TypeId=@TypeId and a.I_basc_bcseseq=@I_bcse_seq
            and b.I_bcse_Delflag=0 and a.I_basc_DelFlag=0 ";
            Hashtable ht1 = new Hashtable();
            ht1.Add("@SiteNo", SiteNo);
            ht1.Add("@TypeId", TypeId);
            ht1.Add("@I_bcse_seq", CostNameSeq);

            DataSet ds = IO.SqlQuery(Login_Server, Sql_cmd, ht1);
            PriceList = ds.Tables[0];

            return PriceList;
        }
        public DataTable BaseCost_Query(string Login_Server, string SiteNo, string TypeId)
        {
            DataTable PriceList = new DataTable();

            string Sql_cmd = @"select a.I_basc_seq, TypeIdName=c.S_bsda_FieldName,	BaseCostSetName=b.S_bcse_CostName,	c1.S_bsda_FieldName,
	        I_basc_bcseseq,a.S_basc_CostName,a.I_basc_Free,a.S_basc_DollarUnit, c2.S_bsda_FieldName,b.I_bcse_TypeId,b.S_bcse_SiteNo,I_basc_UnitId,S_basc_CostType
            from [3PL_BaseCost] a
            inner join [3PL_BaseCostSet] b on a.I_basc_bcseseq=b.I_bcse_Seq
            inner join [3PL_BaseData] c on b.I_bcse_TypeId=c.S_bsda_FieldId and c.S_bsda_CateId='TypeId'
            inner join [3PL_BaseData] c1 on a.S_basc_CostType=c1.S_bsda_FieldId and c1.S_bsda_CateId='CostType'
            inner join [3PL_BaseData] c2 on a.I_basc_UnitId	=c2.S_bsda_FieldId and c2.S_bsda_CateId='UnitId'
            where b.S_bcse_SiteNo=@SiteNo and b.I_bcse_TypeId=@TypeId
            and b.I_bcse_Delflag=0 and a.I_basc_DelFlag=0
            order by b.I_bcse_TypeId,a.I_basc_bcseseq, a.S_basc_CostType";
            Hashtable ht1 = new Hashtable();
            ht1.Add("@SiteNo", SiteNo);
            ht1.Add("@TypeId", TypeId);

            DataSet ds = IO.SqlQuery(Login_Server, Sql_cmd, ht1);
            PriceList = ds.Tables[0];

            return PriceList;
        }
        public DataTable BaseCost_Query(string Login_Server, string bascseq)
        {
            DataTable PriceList = new DataTable();

            string Sql_cmd = @"select a.I_basc_seq, TypeIdName=c.S_bsda_FieldName,	BaseCostSetName=b.S_bcse_CostName,	c1.S_bsda_FieldName,
	        I_basc_bcseseq,a.S_basc_CostName,a.I_basc_Free,a.S_basc_DollarUnit, c2.S_bsda_FieldName,b.I_bcse_TypeId,b.S_bcse_SiteNo,I_basc_UnitId,S_basc_CostType
            from [3PL_BaseCost] a
            inner join [3PL_BaseCostSet] b on a.I_basc_bcseseq=b.I_bcse_Seq
            inner join [3PL_BaseData] c on b.I_bcse_TypeId=c.S_bsda_FieldId and c.S_bsda_CateId='TypeId'
            inner join [3PL_BaseData] c1 on a.S_basc_CostType=c1.S_bsda_FieldId and c1.S_bsda_CateId='CostType'
            inner join [3PL_BaseData] c2 on a.I_basc_UnitId	=c2.S_bsda_FieldId and c2.S_bsda_CateId='UnitId'
            where a.I_basc_seq=@seq
            and b.I_bcse_Delflag=0 and a.I_basc_DelFlag=0
            order by b.I_bcse_TypeId,a.I_basc_bcseseq, a.S_basc_CostType";
            Hashtable ht1 = new Hashtable();
            ht1.Add("@seq", bascseq);

            DataSet ds = IO.SqlQuery(Login_Server, Sql_cmd, ht1);
            PriceList = ds.Tables[0];

            return PriceList;
        }
        #endregion

        #region 新刪修
        public DataTable BaseCost_Default(string Login_Server)
        {
            DataTable PriceList = new DataTable();

            string Sql_cmd = @"select * from [3PL_BaseCost]";
            Hashtable ht1 = new Hashtable();

            DataSet ds = IO.SqlQuery(Login_Server, Sql_cmd, ht1);
            PriceList = ds.Tables[0];

            return PriceList;
        }
        //刪除
        public int BaseCost_Delete(string Login_Server, string UserId, string I_bcse_seq)
        {
            int DeleteCount = 0;

            string Sql_cmd = @"Update [3PL_BaseCost] set I_basc_DelFlag=1,S_basc_UpdId=@id
                            Where I_basc_seq=@seq";
            Hashtable ht1 = new Hashtable();
            ht1.Add("@seq", I_bcse_seq);
            ht1.Add("@id", UserId);

            bool IsDeleteSuccess = IO.SqlUpdate(Login_Server, Sql_cmd, ht1, ref DeleteCount);
            if (!IsDeleteSuccess)
                DeleteCount = 0;
            return DeleteCount;
        }

        //新增
        public int BaseCost_New(string Login_Server, string UserId, DataRow dr)
        {
            int UpdateCount = 0;
            DataTable PriceList = new DataTable();

            string Sql_cmd = @"Insert Into [3PL_BaseCost] (I_basc_bcseseq,I_basc_Detailseq,S_basc_CostType,S_basc_CostName,I_basc_Free,S_basc_DollarUnit,
            I_basc_UnitId,S_basc_CreateId,S_basc_UpdId)
                values(@I_basc_bcseseq,@I_basc_Detailseq,@S_basc_CostType,@S_basc_CostName,@I_basc_Free,@S_basc_DollarUnit,
            @I_basc_UnitId,@S_basc_CreateId,@S_basc_UpdId)";
            Hashtable ht1 = new Hashtable();
            ht1.Add("@I_basc_bcseseq", dr["I_basc_bcseseq"]);
            ht1.Add("@I_basc_Detailseq",dr["I_basc_Detailseq"]);
            ht1.Add("@S_basc_CostType", dr["S_basc_CostType"]);
            ht1.Add("@S_basc_CostName", dr["S_basc_CostName"]);
            ht1.Add("@I_basc_Free", dr["I_basc_Free"]);
            ht1.Add("@S_basc_DollarUnit", dr["S_basc_DollarUnit"]);
            ht1.Add("@I_basc_UnitId", dr["I_basc_UnitId"]);
            ht1.Add("@S_basc_CreateId", UserId);
            ht1.Add("@S_basc_UpdId", UserId);

            bool IsDeleteSuccess = IO.SqlUpdate(Login_Server, Sql_cmd, ht1, ref UpdateCount);
            if (!IsDeleteSuccess)
                UpdateCount = 0;
            return UpdateCount;
        }

        //更新
        public int BaseCost_Update(string Login_Server, string UserId, DataRow dr)
        {
            int UpdateCount = 0;
            DataTable PriceList = new DataTable();

            string Sql_cmd = @"Update [3PL_BaseCost] 
                set S_basc_CostType=@S_basc_CostType,S_basc_CostName=@S_basc_CostName,I_basc_Free=@I_basc_Free,S_basc_DollarUnit=@S_basc_DollarUnit
                ,I_basc_UnitId=@I_basc_UnitId,S_basc_UpdId=@S_basc_UpdId
                where I_basc_seq=@I_basc_seq";
            Hashtable ht1 = new Hashtable();
            ht1.Add("@S_basc_CostType", dr["S_basc_CostType"]);
            ht1.Add("@S_basc_CostName", dr["S_basc_CostName"]);
            ht1.Add("@I_basc_Free", dr["I_basc_Free"]);
            ht1.Add("@S_basc_DollarUnit", dr["S_basc_DollarUnit"]);
            ht1.Add("@I_basc_UnitId", dr["I_basc_UnitId"]);
            ht1.Add("@S_basc_UpdId", UserId);
            ht1.Add("@I_basc_seq", dr["I_basc_seq"]);

            bool IsDeleteSuccess = IO.SqlUpdate(Login_Server, Sql_cmd, ht1, ref UpdateCount);
            if (!IsDeleteSuccess)
                UpdateCount = 0;
            return UpdateCount;
        }
        #endregion
    }
}
