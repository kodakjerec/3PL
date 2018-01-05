using System.Data;
using _3PL_LIB;
using System.Collections;
using System;

namespace _3PL_DAO
{
    public partial class _3PL_Assign_New
    {
        public string Login_Server = "3PL";
        DB_IO IO = new DB_IO();

        /// <summary>
        /// 報價單單頭
        /// </summary>
        /// <param name="PLNO"></param>
        /// <param name="site_no"></param>
        /// <returns></returns>
        public DataTable GetQuotation(string PLNO, string site_no)
        {
            DataTable QuotationDetail = new DataTable();

            string Sql_cmd =
            @"select top 50 a.S_qthe_SupdId, a.S_qthe_PLNO, a.D_qthe_ContractS, a.D_qthe_ContractE, b.I_qtde_TypeId,d.S_bsda_FieldName,c.ClassId, c.ClassName, b.S_qtde_SiteNo
            from [3PL_QuotationHead] a
            inner join [3PL_QuotationDetail] b on a.S_qthe_PLNO=b.S_qtde_qthePLNO and b.I_qtde_DelFlag=0
            inner join [3PL_BaseCostSet_ClassList] c on b.I_qtde_bcseSeq=c.I_bcse_Seq
            inner join [3PL_BaseData] d on b.I_qtde_TypeId=d.S_bsda_FieldId and d.S_bsda_CateId='TypeId'
            where b.S_qtde_SiteNo like '%'+@site_no+'%'
                and a.S_qthe_PLNO like '%'+@PLNO+'%'
                and a.I_qthe_Status=20
            group by a.S_qthe_SupdId, a.S_qthe_PLNO, a.D_qthe_ContractS, a.D_qthe_ContractE, b.I_qtde_TypeId,d.S_bsda_FieldName,c.ClassId, c.ClassName, b.S_qtde_SiteNo
                order by a.S_qthe_PLNO DESC, a.S_qthe_SupdId, b.I_qtde_TypeId, c.ClassId";
            Hashtable ht1 = new Hashtable();
            ht1.Add("@PLNO", PLNO);
            ht1.Add("@site_no", site_no);
            DataSet ds = IO.SqlQuery(Login_Server, Sql_cmd, ht1);
            QuotationDetail = ds.Tables[0];

            return QuotationDetail;
        }

        /// <summary>
        /// 報價單明細
        /// </summary>
        /// <param name="PLNO"></param>
        /// <param name="site_no"></param>
        /// <returns></returns>
        public DataTable getCostlist(string PLNO, string site_no, string TypeId, string ClassId)
        {
            DataTable QuotationDetail = new DataTable();

            string Sql_cmd =
            @"select b.S_bcse_CostName, a.I_qtde_Price, b.S_bcse_DollarUnit,c.S_bsda_FieldName,
            Ordqty=000000,PONO=space(20),itemno=space(8),TQty=0,TBoxQty=0,TpalletQty=0, b.I_bcse_Seq
            from [3PL_QuotationDetail] a
            inner join [3PL_BaseCostSet] b on a.I_qtde_bcseSeq=b.I_bcse_Seq
            inner join [3PL_BaseCostSet_ClassList] b2 on a.I_qtde_bcseSeq=b2.I_bcse_Seq
            inner join [3PL_BaseData] c on b.I_bcse_UnitId=c.S_bsda_FieldId and c.S_bsda_CateId='UnitId'
            where S_qtde_qthePLNO=@PLNO
	            and S_qtde_SiteNo=@site_no
                and I_qtde_Delflag=0
                and b2.ClassId=@ClassId
            and I_qtde_TypeId=@TypeId";
            Hashtable ht1 = new Hashtable();
            ht1.Add("@PLNO", PLNO);
            ht1.Add("@site_no", site_no);
            ht1.Add("@TypeId", TypeId);
            ht1.Add("@ClassId", ClassId);
            DataSet ds = IO.SqlQuery(Login_Server, Sql_cmd, ht1);
            QuotationDetail = ds.Tables[0];

            return QuotationDetail;
        }

        /// <summary>
        /// 取得PO單總量
        /// </summary>
        /// <param name="PLNO"></param>
        /// <param name="itemno"></param>
        /// <param name="site_no"></param>
        /// <returns></returns>
        public DataTable getPOList(string PLNO, string itemno, string site_no, string Sup_Id)
        {
            DataTable QuotationDetail = new DataTable();

            string Sql_cmd =
            @"select TQty=ISNULL(sum(final.Tqty),0), TBox=ISNULL(sum(final.TBox),0), TPallet=ISNULL(Ceiling(sum(final.Tpallet)),0)
            from (
            select Tqty=b.L_reci_reciqty,
	            TBox=b.L_reci_reciqty/ISNULL(c.I_merp_1qty,1),
	            Tpallet=b.L_reci_reciqty/(ISNULL(c.I_merp_1qty,1)*c.I_merp_pacti*c.I_merp_pachi*1.00)
            from reci_head a with(nolock)
            inner join reci_item b with(nolock)
            on a.L_rech_id=b.L_reci_rechid
            left join mer_package c with(nolock) on b.L_reci_merdsysno=c.L_merp_merdsysno and c.I_merp_boxflag=1
            where a.S_rech_erpid=@PLNO
	            and b.S_reci_merdid like '%'+@itemno+'%'
            ) final";
            Hashtable ht1 = new Hashtable();
            ht1.Add("@PLNO", PLNO);
            ht1.Add("@itemno", itemno);
            ht1.Add("@Sup_Id", Sup_Id);
            DataSet ds = IO.SqlQuery(site_no, Sql_cmd, ht1);
            QuotationDetail = ds.Tables[0];

            return QuotationDetail;
        }

        #region 新增資料
        /// <summary>
        /// 報價單完成後，產生派工單,成本單 
        /// </summary>
        /// <param name="DBlink"></param>
        /// <param name="PLNO"></param>
        /// <param name="UserId"></param>
        /// <returns></returns>
        public string Head_Assign_New(string DBlink, string PLNO, string TypeId, string UserId, string ClassId)
        {
            string Create_Assign = "[sp_CrtWC_San_New]";
            Hashtable ht_assign = new Hashtable();
            Hashtable ht_trash = new Hashtable();
            ht_assign.Add("@WCid", PLNO);
            ht_assign.Add("@Usr", UserId);
            ht_assign.Add("@ClassId", ClassId);
            ht_assign.Add("@mKind", TypeId);
            DataSet ds1=IO.SqlSp(DBlink, Create_Assign, ht_assign, ref ht_trash);

            return ds1.Tables[0].Rows[0][0].ToString();
        }

        /// <summary>
        /// 新增派工單明細
        /// </summary>
        /// <param name="DBlink"></param>
        /// <param name="Wk_Id"></param>
        /// <param name="UserId"></param>
        /// <returns></returns>
        public string Detail_Assign_New(string DBlink, string Wk_Id, string UserId, string PONO, string itemno, string TQty, string I_bcse_seq)
        {
            string Create_Assign = "[sp_CrtWC_San_New_Detail]";
            Hashtable ht_assign = new Hashtable();
            Hashtable ht_trash = new Hashtable();
            ht_assign.Add("@mWCno", Wk_Id);
            ht_assign.Add("@Usr", UserId);
            ht_assign.Add("@PONO", PONO);
            ht_assign.Add("@itemno", itemno);
            ht_assign.Add("@TQty", TQty);
            ht_assign.Add("@I_bcse_seq", I_bcse_seq);
            DataSet ds1 = IO.SqlSp(DBlink, Create_Assign, ht_assign, ref ht_trash);

            return ds1.Tables[0].Rows[0][0].ToString();
        }

        public void Head_Assign_UpdateMemo(string DBlink, string Wk_Id, string Memo)
        {
            int SuccessCount = 0, SuccessCount_head = 0;
            string Updcmd_head =
            @"Update [AssignHead] set Memo=@Memo where Wk_Id=@Wk_Id";
            Hashtable ht1 = new Hashtable();
            ht1.Add("@Memo", Memo);
            ht1.Add("@Wk_Id", Wk_Id);

            IO.SqlUpdate(Login_Server, Updcmd_head, ht1, ref SuccessCount_head);
            SuccessCount += SuccessCount_head;

            return;
        }
        #endregion
    }
}
