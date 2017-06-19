using _3PL_LIB;
using System;
using System.Collections;
using System.Data;
using System.Web;

namespace _3PL_DAO
{
    public partial class creINVENTORY_DATE_DAO
    {
        private string DBlink = "LGDC", DBlink2 = "EEPDC";
        DB_IO IO = new DB_IO();

        #region LGDC
        /// <summary>
        /// 查詢盤點日期
        /// </summary>
        /// <returns></returns>
        public DataTable searchInventory_Date(string searchString)
        {
            DataTable CateIdList = new DataTable();
            Hashtable ht1 = new Hashtable();
            string cmdstring = "";

            if (searchString == "")
                cmdstring += @"Select TOP 50 ";
            else
                cmdstring += @"Select ";

            cmdstring +=
              @"YYMM,VENDOR_NO,SITE_NO,
                INV_DATE=convert(varchar,INV_DATE,111),
	            lbl_01=convert(varchar,STOP_BACK_KEY_S,111)+'~'+convert(varchar,STOP_BACK_KEY_E,111),
	            lbl_02=convert(varchar,STOP_SITE_IN_S,111)+'~'+convert(varchar,STOP_SITE_IN_E,111),
	            lbl_03=convert(varchar,STOP_SITE_IN_DC_S,111)+'~'+convert(varchar,STOP_SITE_IN_DC_E,111),
	            lbl_04=convert(varchar,STOP_DC_GET_S,111)+'~'+convert(varchar,STOP_DC_GET_E,111),
	            lbl_05=convert(varchar,STOP_SITE_OUT_S,111)+'~'+convert(varchar,STOP_SITE_OUT_E,111),
	            lbl_06=convert(varchar,STOP_SITE_ADJ_S,111)+'~'+convert(varchar,STOP_SITE_ADJ_E,111),
	            lbl_07=convert(varchar,STOP_STORE_ADJ_S,111)+'~'+convert(varchar,STOP_STORE_ADJ_E,111),
                lbl_08=convert(varchar,STOP_PREORDER_S,111)+'~'+convert(varchar,STOP_PREORDER_E,111),
                CheckStr=YYMM+'|'+VENDOR_NO+'|'+SITE_NO
            from VEN_INVENTORY_DATE ";

            if (searchString != "")
            {
                cmdstring += "where YYMM+VENDOR_NO+SITE_NO like '%'+@searchString+'%' ";
                ht1.Add("@searchString", searchString);
            }
            cmdstring += "order by YYMM DESC, VENDOR_NO, SITE_NO ";

            
            DataSet ds = IO.SqlQuery(DBlink, cmdstring, ht1);
            CateIdList = ds.Tables[0];

            return CateIdList;
        }

        /// <summary>
        /// 查詢日期設定
        /// </summary>
        /// <returns></returns>
        public DataTable searchInventory_Date_Set()
        {
            DataTable CateIdList = new DataTable();

            string cmdstring = @"Select * from VEN_INVENTORY_SET";
            Hashtable ht1 = new Hashtable();
            DataSet ds = IO.SqlQuery(DBlink, cmdstring, ht1);
            CateIdList = ds.Tables[0];

            return CateIdList;
        }

        /// <summary>
        /// 檢驗廠商編號
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public DataTable CheckVendorNo(string p)
        {
            DataTable CateIdList = new DataTable();

            string cmdstring = @"Select top 1 ALIAS from drp.dbo.DRP_SUPPLIER where ID=@ID";
            Hashtable ht1 = new Hashtable();
            ht1.Add("@ID", p);
            DataSet ds = IO.SqlQuery(DBlink, cmdstring, ht1);
            CateIdList = ds.Tables[0];

            return CateIdList;
        }

        /// <summary>
        /// 新增至LGDC
        /// </summary>
        /// <returns></returns>
        public bool InsertLGDC(
            string YYMM
            , string vendor_no
            , string SITE_NO
            , string INV_DATE
            , string STOP_BACK_KEY_S
            , string STOP_BACK_KEY_E
            , string STOP_SITE_IN_S
            , string STOP_SITE_IN_E
            , string STOP_SITE_IN_DC_S
            , string STOP_SITE_IN_DC_E
            , string STOP_DC_GET_S
            , string STOP_DC_GET_E
            , string STOP_SITE_OUT_S
            , string STOP_SITE_OUT_E
            , string STOP_SITE_ADJ_S
            , string STOP_SITE_ADJ_E
            , string STOP_STORE_ADJ_S
            , string STOP_STORE_ADJ_E
            , object STOP_PREORDER_S
            , object STOP_PREORDER_E)
        {
            bool IsOK = false;
            int Tcount = 0;

            string cmdstring =
@"Insert into VEN_INVENTORY_DATE(
YYMM,
VENDOR_NO,
SITE_NO,
INV_DATE,
STOP_BACK_KEY_S,
STOP_BACK_KEY_E,
STOP_SITE_IN_S,
STOP_SITE_IN_E,
STOP_SITE_IN_DC_S,
STOP_SITE_IN_DC_E,
STOP_DC_GET_S,
STOP_DC_GET_E,
STOP_SITE_OUT_S,
STOP_SITE_OUT_E,
STOP_SITE_ADJ_S,
STOP_SITE_ADJ_E,
STOP_STORE_ADJ_S,
STOP_STORE_ADJ_E,
STOP_PREORDER_S,
STOP_PREORDER_E)
values(
@YYMM
,@vendor_no
,@SITE_NO
,@INV_DATE
,@STOP_BACK_KEY_S
,@STOP_BACK_KEY_E
,@STOP_SITE_IN_S
,@STOP_SITE_IN_E
,@STOP_SITE_IN_DC_S
,@STOP_SITE_IN_DC_E
,@STOP_DC_GET_S
,@STOP_DC_GET_E
,@STOP_SITE_OUT_S
,@STOP_SITE_OUT_E
,@STOP_SITE_ADJ_S
,@STOP_SITE_ADJ_E
,@STOP_STORE_ADJ_S
,@STOP_STORE_ADJ_E
,@STOP_PREORDER_S
,@STOP_PREORDER_E)";
            Hashtable ht1 = new Hashtable();
            ht1.Add("@YYMM", YYMM);
            ht1.Add("@vendor_no", vendor_no);
            ht1.Add("@SITE_NO", SITE_NO);
            ht1.Add("@INV_DATE", INV_DATE);
            ht1.Add("@STOP_BACK_KEY_S", STOP_BACK_KEY_S);
            ht1.Add("@STOP_BACK_KEY_E", STOP_BACK_KEY_E);
            ht1.Add("@STOP_SITE_IN_S", STOP_SITE_IN_S);
            ht1.Add("@STOP_SITE_IN_E", STOP_SITE_IN_E);
            ht1.Add("@STOP_SITE_IN_DC_S", STOP_SITE_IN_DC_S);
            ht1.Add("@STOP_SITE_IN_DC_E", STOP_SITE_IN_DC_E);
            ht1.Add("@STOP_DC_GET_S", STOP_DC_GET_S);
            ht1.Add("@STOP_DC_GET_E", STOP_DC_GET_E);
            ht1.Add("@STOP_SITE_OUT_S", STOP_SITE_OUT_S);
            ht1.Add("@STOP_SITE_OUT_E", STOP_SITE_OUT_E);
            ht1.Add("@STOP_SITE_ADJ_S", STOP_SITE_ADJ_S);
            ht1.Add("@STOP_SITE_ADJ_E", STOP_SITE_ADJ_E);
            ht1.Add("@STOP_STORE_ADJ_S", STOP_STORE_ADJ_S);
            ht1.Add("@STOP_STORE_ADJ_E", STOP_STORE_ADJ_E);
            if (STOP_PREORDER_S.ToString() == "")
                STOP_PREORDER_S = DBNull.Value;
            if (STOP_PREORDER_E.ToString() == "")
                STOP_PREORDER_E = DBNull.Value;
            ht1.Add("@STOP_PREORDER_S", STOP_PREORDER_S);
            ht1.Add("@STOP_PREORDER_E", STOP_PREORDER_E);

            IsOK = IO.SqlUpdate(DBlink, cmdstring, ht1, ref Tcount);
            if (Tcount <= 0)
                IsOK = false;

            return IsOK;
        }

        /// <summary>
        /// 刪除LGDC資料
        /// </summary>
        /// <param name="p"></param>
        /// <param name="p_2"></param>
        /// <param name="p_3"></param>
        /// <returns></returns>
        public bool DeleteLGDC(string YYMM, string VENDOR_NO, string SITE_NO)
        {
            bool IsOK = false;
            int Tcount = 0;
            string cmdstring = "Delete from VEN_INVENTORY_DATE where YYMM=@YYMM and VENDOR_NO=@VENDOR_NO and SITE_NO=@SITE_NO";
            Hashtable ht1 = new Hashtable();
            ht1.Add("@YYMM", YYMM);
            ht1.Add("@VENDOR_NO", VENDOR_NO);
            ht1.Add("@SITE_NO", SITE_NO);
            IsOK = IO.SqlUpdate(DBlink, cmdstring, ht1, ref Tcount);
            if (Tcount <= 0)
                IsOK = false;

            return IsOK;
        }
        #endregion

        #region EEPDC
        /// <summary>
        /// 查詢XMS_UNDORTNSUP
        /// </summary>
        /// <param name="vendor_no"></param>
        /// <returns></returns>
        public DataTable CheckXMS_UNDORTNSUP(string vendor_no)
        {
            DataTable CateIdList = new DataTable();

            string cmdstring = @"Select top 1 memo1=ISNULL(memo1,'') from XMS_UNDORTNSUP where sup_no=@ID";
            Hashtable ht1 = new Hashtable();
            ht1.Add("@ID", vendor_no);
            DataSet ds = IO.SqlQuery(DBlink2, cmdstring, ht1);
            CateIdList = ds.Tables[0];

            return CateIdList;
        }

        /// <summary>
        /// 新增XMS_UNDORTNSUP
        /// </summary>
        /// <param name="vendor_no"></param>
        /// <param name="stop_dates"></param>
        /// <param name="stop_datee"></param>
        /// <returns></returns>
        public bool InsertXMS_UNDORTNSUP(string vendor_no, string stop_dates, string stop_datee)
        {
            bool IsOK = false;
            int Tcount = 0;

            IsOK = DeleteXMS_UNDORTNSUP(vendor_no);
            if (IsOK)
            {
                IsOK = false;
                string cmdstring = @"Insert into XMS_UNDORTNSUP values(@ID,'',@stop_dates,@stop_datee,getdate(),getdate(),'')";
                Hashtable ht1 = new Hashtable();
                ht1.Add("@ID", vendor_no);
                ht1.Add("@stop_dates", stop_dates);
                ht1.Add("@stop_datee", stop_datee);
                IsOK = IO.SqlUpdate(DBlink2, cmdstring, ht1, ref Tcount);
                if (Tcount <= 0)
                {
                    IsOK = false;
                }
            }

            return IsOK;
        }

        /// <summary>
        /// 刪除IRIS資料
        /// </summary>
        /// <param name="sup_no"></param>
        /// <returns></returns>
        public bool DeleteXMS_UNDORTNSUP(string sup_no)
        {
            bool IsOK = false;
            int Tcount = 0;

            string cmdstring = @"Delete from XMS_UNDORTNSUP where sup_no=@ID and ISNULL(memo1,'')!=@lock";
            Hashtable ht1 = new Hashtable();
            ht1.Add("@ID", sup_no);
            ht1.Add("@lock", "永久鎖退");

            IsOK = IO.SqlUpdate(DBlink2, cmdstring, ht1, ref Tcount);

            return IsOK;
        }
        #endregion
    }
}
