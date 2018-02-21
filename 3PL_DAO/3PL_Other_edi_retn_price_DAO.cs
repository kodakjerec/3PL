using System.Data;
using _3PL_LIB;
using System.Collections;
using System;
//逆物流費

#region 修改紀錄
//
#endregion
namespace _3PL_DAO
{
    public partial class _3PL_Other_edi_retn_price_DAO
    {
        public string Login_Server = "EDI";
        DB_IO IO = new DB_IO();

        #region 查詢
        /// <summary>
        /// 查詢逆物流費
        /// </summary>
        public DataTable GetData(string site_no
            , string vendor_no
            , string back_id
            , string boxid
            , string back_date_S
            , string back_date_E
            , string Bill_date
            , string Kind)
        {
            DataTable dt = new DataTable();

            string Sql_cmd =
            @"select TOP 500
                edi_retn_price.*,[UIStatus]='Unchanged'
            from edi_retn_price with(nolock)
            where 1=1 ";
            Hashtable ht1 = new Hashtable();
            if (site_no != "")
            {
                Sql_cmd += " and site_no=@site_no";
                ht1.Add("@site_no", site_no);
            }
            if (vendor_no != "")
            {
                Sql_cmd += " and vendor_no=@vendor_no";
                ht1.Add("@vendor_no", vendor_no);
            }
            if (back_id != "")
            {
                Sql_cmd += " and back_id like @back_id+'%'";
                ht1.Add("@back_id", back_id);
            }
            if (boxid != "")
            {
                Sql_cmd += " and boxid like @boxid+'%'";
                ht1.Add("@boxid", boxid);
            }
            if (back_date_S != "")
            {
                Sql_cmd += " and back_date>=@back_date_S";
                ht1.Add("@back_date_S", back_date_S);
            }
            if (back_date_E != "")
            {
                Sql_cmd += " and back_date<=@back_date_E";
                ht1.Add("@back_date_E", back_date_E);
            }
            if (Bill_date != "")
            {
                Sql_cmd += " and Bill_date=@Bill_date";
                ht1.Add("@Bill_date", Bill_date);
            }
            if (Kind != "")
            {
                Sql_cmd += " and kind=@Kind";
                ht1.Add("@Kind", Kind);
            }

            Sql_cmd += " order by back_date DESC, site_no, vendor_no, back_id DESC, boxid, item_id";

            DataSet ds = IO.SqlQuery(Login_Server, Sql_cmd, ht1);
            dt = ds.Tables[0];

            return dt;
        }

        /// <summary>
        /// 逆物流費的細類清單
        /// </summary>
        /// <returns></returns>
        public DataTable kind_list()
        {
            DataTable dt = new DataTable();

            string Sql_cmd =
            @"select Value=I_bcse_Seq, Name=S_bcse_CostName, Kind=I_bcse_TypeId
            from [3PL_BaseCostSet] with(nolock)
            where S_bcse_SiteNo='DC01'
	        and I_bcse_TypeId in (13,14,15,16)";
            Hashtable ht1 = new Hashtable();
            DataSet ds = IO.SqlQuery("3PL", Sql_cmd, ht1);
            dt = ds.Tables[0];

            return dt;
        }
        /// <summary>
        /// 逆物流費的大類
        /// </summary>
        /// <returns></returns>
        public DataTable charge_kind_List()
        {
            DataTable dt = new DataTable();

            string Sql_cmd =
            @"select Value=S_bsda_FieldId, Name=S_bsda_FieldName
            from [3PL_BaseData] with(nolock)
            where S_bsda_CateId='TypeId'
	        and S_bsda_FieldId in (13,14,15,16)";
            Hashtable ht1 = new Hashtable();
            DataSet ds = IO.SqlQuery("3PL", Sql_cmd, ht1);
            dt = ds.Tables[0];

            return dt;
        }

        //倉別清單
        public DataTable SiteNoList()
        {
            DataTable dt1 = new DataTable();
            dt1.Columns.Add("Name", typeof(string));
            dt1.Columns.Add("Value", typeof(string));
            dt1.Rows.Add(new object[] { "觀音", "觀音" });
            dt1.Rows.Add(new object[] { "岡山", "岡山" });
            dt1.Rows.Add(new object[] { "梧棲", "梧棲" });
            return dt1;
        }

        /// <summary>
        /// 取得 廠商清單編號
        /// </summary>
        /// <returns></returns>
        public DataTable Get_DeleteList_Supno(bool chk_IsShowDeleted)
        {
            DataTable dt = new DataTable();

            string Sql_cmd =
            @"select * from edi_retn_price_disable_supno with(nolock) ";
            if (!chk_IsShowDeleted)
                Sql_cmd += " where del_date is null ";
            Sql_cmd += " order by sup_no, create_date DESC";
            Hashtable ht1 = new Hashtable();
            DataSet ds = IO.SqlQuery("EDI", Sql_cmd, ht1);
            dt = ds.Tables[0];

            return dt;
        }
        /// <summary>
        /// 取得 貨號清單
        /// </summary>
        /// <returns></returns>
        public DataTable Get_DeleteList_ItemNo(bool chk_IsShowDeleted)
        {
            DataTable dt = new DataTable();

            string Sql_cmd =
            @"select * from edi_retn_price_disable_item with(nolock) ";
            if (!chk_IsShowDeleted)
                Sql_cmd += " where del_date is null ";
            Sql_cmd += " order by  item_id, create_date DESC";
            Hashtable ht1 = new Hashtable();
            DataSet ds = IO.SqlQuery("EDI", Sql_cmd, ht1);
            dt = ds.Tables[0];

            return dt;
        }
        #endregion

        #region Update
        /// <summary>
        /// 更新資料
        /// </summary>
        /// <param name="dr"></param>
        /// <param name="UI"></param>
        public void UpdateData(DataRow dr, UserInf UI)
        {
            string Sql_cmd =
            @"UPDATE edi_retn_price
            SET kind=@kind,qty_real=@qty_real,edit_date=GETDATE(),edit_user=@User
            WHERE site_no=@site_no
                AND boxid=@boxid
                AND item_id=@item_id";
            Hashtable ht1 = new Hashtable();
            ht1.Add("@kind", dr["kind"]);
            ht1.Add("@qty_real", dr["qty_real"]);
            ht1.Add("@site_no", dr["site_no"]);
            ht1.Add("@boxid", dr["boxid"]);
            ht1.Add("@item_id", dr["item_id"]);
            ht1.Add("@User", UI.UserID);

            DataSet ds = IO.SqlQuery(Login_Server, Sql_cmd, ht1);
        }
        #endregion

        #region DELETE
        /// <summary>
        /// 刪除廠商編號
        /// </summary>
        /// <param name="sup_no"></param>
        /// <param name="UserID"></param>
        /// <returns></returns>
        public bool del_Supno(string sup_no, string UserID, int Mode)
        {
            bool IsOK = false;

            string Sql_cmd = "";
            if (Mode == 0)
            {
                Sql_cmd = @"Update edi_retn_price_disable_supno 
                set del_user=@UserID, del_date=GETDATE()
                WHERE sup_no=@sup_no and del_date is null;select @@ROWCOUNT";
            }
            else
            {
                Sql_cmd = @"Delete from edi_retn_price_disable_supno 
                WHERE sup_no=@sup_no and del_date is not null;select @@ROWCOUNT";
            }
            Hashtable ht1 = new Hashtable();
            ht1.Add("@UserID", UserID);
            ht1.Add("@sup_no", sup_no);
            DataSet ds = IO.SqlQuery("EDI", Sql_cmd, ht1);
            if (Convert.ToInt32(ds.Tables[0].Rows[0][0]) > 0)
                IsOK = true;

            return IsOK;
        }

        /// <summary>
        /// 刪除貨號
        /// </summary>
        /// <param name="item_no"></param>
        /// <param name="UserID"></param>
        /// <returns></returns>
        public bool del_ItemNo(string item_no, string UserID, int Mode)
        {
            bool IsOK = false;

            string Sql_cmd = "";
            if (Mode == 0)
            {
                Sql_cmd = @"Update edi_retn_price_disable_item 
                set del_user=@UserID, del_date=GETDATE()
                WHERE item_id=@item_no and del_date is null;select @@ROWCOUNT";
            }
            else
            {
                Sql_cmd = @"Delete from edi_retn_price_disable_item 
                    WHERE item_id = @item_no and del_date is not null; select @@ROWCOUNT";
            }
            Hashtable ht1 = new Hashtable();
            ht1.Add("@UserID", UserID);
            ht1.Add("@item_no", item_no);
            DataSet ds = IO.SqlQuery("EDI", Sql_cmd, ht1);
            if (Convert.ToInt32(ds.Tables[0].Rows[0][0]) > 0)
                IsOK = true;

            return IsOK;
        }
        #endregion

        #region Insert
        public bool Insert_Supno(string sup_no)
        {
            bool IsOK = false;

            string Sql_cmd =
            @"Insert Into edi_retn_price_disable_supno (sup_no)
            Select ID from drp.dbo.DRP_SUPPLIER with(nolock) where ID=@sup_no;select @@ROWCOUNT";
            Hashtable ht1 = new Hashtable();
            ht1.Add("@sup_no", sup_no);
            DataSet ds = IO.SqlQuery("EDI", Sql_cmd, ht1);
            if (Convert.ToInt32(ds.Tables[0].Rows[0][0]) > 0)
                IsOK = true;

            return IsOK;
        }

        public bool Insert_ItemNo(string item_no)
        {
            bool IsOK = false;

            string Sql_cmd =
            @"Insert Into edi_retn_price_disable_item (item_id)
           Select ITEM_ID from drp.dbo.DRP_ITEM with(nolock) where ITEM_ID=@item_no;select @@ROWCOUNT";
            Hashtable ht1 = new Hashtable();
            ht1.Add("@item_no", item_no);
            DataSet ds = IO.SqlQuery("EDI", Sql_cmd, ht1);
            if (Convert.ToInt32(ds.Tables[0].Rows[0][0]) > 0)
                IsOK = true;

            return IsOK;
        }
        #endregion
    }
}
