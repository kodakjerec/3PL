﻿using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using _3PL_LIB;
using _3PL_DAO;
using System.Data;

namespace _3PL_System
{
    public partial class _3PL_Other_edi_retn_price : System.Web.UI.Page
    {
        _3PL_CommonQuery _3PLCQ = new _3PL_CommonQuery();
        _3PL_Other_edi_retn_price_DAO _3PLOtherEdiRetnPrice = new _3PL_Other_edi_retn_price_DAO();
        ControlBind CB = new ControlBind();
        private UserInf UI = new UserInf();

        protected void Page_Load(object sender, EventArgs e)
        {
            ((_3PLMasterPage)Master).SessionCheck(ref UI, 0);

            if (!IsPostBack)
            {
                div_Content.Visible = false;
                lbl_CloseDate.Text = _3PLCQ.Addon_GetCloseData();

                txb_back_date_S.Text = DateTime.Parse(lbl_CloseDate.Text).AddDays(-7).ToString("yyyy/MM/dd");
                //倉別
                CB.DropDownListBind(ref ddl_Query_site_no, _3PLOtherEdiRetnPrice.SiteNoList(), "Value", "Name", "ALL", "");
                //計費類別
                CB.DropDownListBind(ref ddl_Query_Kind, _3PLOtherEdiRetnPrice.KindList(), "Value", "Name", "ALL", "");

            }
        }

        #region 查詢
        //查詢報價單單頭
        protected void Btn_Query_Click(object sender, EventArgs e)
        {
            Btn_Query_Click();
        }
        private void Btn_Query_Click()
        {
            Session["SourceTable"] = _3PLOtherEdiRetnPrice.GetData(ddl_Query_site_no.SelectedValue
                , txb_Query_vendor_no.Text
                , txb_Query_back_id.Text
                , txb_Query_boxid.Text
                , txb_back_date_S.Text
                , txb_back_date_E.Text
                , txb_Bill_date.Text
                , ddl_Query_Kind.SelectedValue);
            div_Content.Visible = true;
            GVBind();
        }
        #endregion

        #region GV_BaseAccounting_Control
        //綁定
        private void GVBind()
        {
            GV_BaseAccounting.DataSource = Session["SourceTable"];
            GV_BaseAccounting.DataBind();
        }

        //換頁
        protected void GV_BaseAccounting_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridView GV = sender as GridView;

            DataTable dt = (DataTable)Session["SourceTable"];
            GV.PageIndex = e.NewPageIndex;
            GVBind();
        }

        /// <summary>
        /// 綁訂下拉是選單
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void GV_BaseAccounting_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            DropDownList ddl_kind_obj;
            TextBox txb_qty_real_obj;
            string myCloseDate = _3PLCQ.Addon_GetCloseData();

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                string back_date = ((Label)e.Row.FindControl("lbl_back_date")).Text;
                ddl_kind_obj = (DropDownList)e.Row.FindControl("ddl_kind");
                txb_qty_real_obj = (TextBox)e.Row.FindControl("txb_qty_real");

                //用FindControl(你的DropDownList的ID)，來找我們的DropDownList，記得要轉型喔!
                CB.DropDownListBind(ref ddl_kind_obj, _3PLOtherEdiRetnPrice.KindList(), "Value", "Name");

                DataRowView dr = e.Row.DataItem as DataRowView;
                ddl_kind_obj.SelectedValue = dr["kind"].ToString();

                //小於關帳日
                if (DateTime.Compare(DateTime.Parse(back_date), DateTime.Parse(myCloseDate)) < 0)
                {
                    ddl_kind_obj.Enabled = false;
                    txb_qty_real_obj.Enabled = false;
                }
            }
        }

        /// <summary>
        /// 下拉是選單變更
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddl_kind_SelectedIndexChanged(object sender, EventArgs e)
        {
            int RowIndex = ((GridViewRow)((DropDownList)sender).NamingContainer).RowIndex;
            DataTable dt = (DataTable)Session["SourceTable"];
            dt.Rows[RowIndex]["kind"] = ((DropDownList)sender).SelectedValue;
            dt.Rows[RowIndex]["UIStatus"] = "Modified";
        }

        /// <summary>
        /// 計費輛變更
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void txb_qty_real_TextChanged(object sender, EventArgs e)
        {
            int RowIndex = ((GridViewRow)((TextBox)sender).NamingContainer).RowIndex;
            DataTable dt = (DataTable)Session["SourceTable"];
            dt.Rows[RowIndex]["qty_real"] = ((TextBox)sender).Text;
            dt.Rows[RowIndex]["UIStatus"] = "Modified";
        }
        #endregion

        /// <summary>
        /// 全部更新
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btn_CloseDateConfirm_Click(object sender, EventArgs e)
        {
            DataTable dt = (DataTable)Session["SourceTable"];
            foreach (DataRow dr in dt.Rows)
            {
                if (dr["UIStatus"].ToString() == "Modified")
                {
                    _3PLOtherEdiRetnPrice.UpdateData(dr, UI);
                }
            }

            ((_3PLMasterPage)Master).ShowMessage("修改完畢，請重新查詢");
        }

        //選擇廠商對象
        protected void Btn_Query_vendor_no_Click(object sender, EventArgs e)
        {
            string Path = "3PL_SupdSelect.aspx?ReturnLocation=" + txb_Query_vendor_no.ClientID + "&btnCloseID=" + ((_3PLMasterPage)Master).FindControl("btn_Close_div_URL").ClientID;
            ((_3PLMasterPage)Master).ShowURL(Path);
        }

        //匯出Excel
        protected void btn_OutputExcel_Click(object sender, EventArgs e)
        {
            btn_OutputExcel.Enabled = false;

            ScriptManager.RegisterClientScriptBlock(Page, typeof(string), "alert", "HideProgressBar();", true);
            DataTable dt = (DataTable)Session["SourceTable"];

            #region 建立欄位
            DataTable dt_Output = new DataTable();
            dt_Output.Columns.Add("倉別", typeof(string));
            dt_Output.Columns.Add("供應商", typeof(string));
            dt_Output.Columns.Add("驗收日期", typeof(string));
            dt_Output.Columns.Add("退廠日期", typeof(string));
            dt_Output.Columns.Add("退廠單號", typeof(string));
            dt_Output.Columns.Add("計費類別", typeof(string));
            dt_Output.Columns.Add("退廠箱號", typeof(string));
            dt_Output.Columns.Add("貨號", typeof(string));
            dt_Output.Columns.Add("品名規格", typeof(string));
            dt_Output.Columns.Add("退廠量", typeof(string));
            dt_Output.Columns.Add("計費量", typeof(string));
            #endregion

            #region 填入資料 by Row

            foreach (DataRow dr in dt.Rows)
            {
                dt_Output.Rows.Add(new object[] {
                    dr["site_no"]
                    ,dr["vendor_no"]
                    ,((DateTime)dr["rec_date"]).ToString("yyyy/MM/dd")
                    ,((DateTime)dr["back_date"]).ToString("yyyy/MM/dd")
                    ,dr["back_id"]
                    ,dr["kind"]
                    ,dr["boxid"]
                    ,dr["item_id"]
                    ,dr["name"]
                    ,dr["qty"]
                    ,dr["qty_real"]
                });
            }
            #endregion
            Session["AssignList"] = dt_Output;
            string Path = "3PL_download.aspx?TableName=AssignList&FileName=逆物流費用";
            ((_3PLMasterPage)Master).ShowURL(Path,"download");
            btn_OutputExcel.Enabled = true;
        }

        #region 設定濾除
        protected void Btn_ShowSetting_Click(object sender, EventArgs e)
        {
            dialog_Settings.Style.Add("display", "inline");
        }
        protected void Btn_CloseSetting_Click(object sender, EventArgs e)
        {
            dialog_Settings.Style.Add("display", "none");
        }
        #endregion
    }
}
