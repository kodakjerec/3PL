using _3PL_DAO;
using _3PL_LIB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace _3PL_System
{
    public partial class _3PL_Other_edi_retn_price_Settings : System.Web.UI.Page
    {
        _3PL_Other_edi_retn_price_DAO _edi_retn = new _3PL_Other_edi_retn_price_DAO();
        private UserInf UI = new UserInf();

        protected void Page_Load(object sender, EventArgs e)
        {
            ((_3PLMasterPage)Master).SessionCheck(ref UI);

            if (!IsPostBack) {
                btn_Supno_Click(sender, e);
            }
        }

        protected void btn_Supno_Click(object sender, EventArgs e)
        {
            btn_Supno.CssClass = "btn-primary";
            btn_itemno.CssClass = "btn-default";
            div_Supno.Visible = true;
            div_itemno.Visible = false;
            ListFor_Supno();
        }

        protected void btn_Itemno_Click(object sender, EventArgs e)
        {
            btn_Supno.CssClass = "btn-default";
            btn_itemno.CssClass = "btn-primary";
            div_Supno.Visible = false;
            div_itemno.Visible = true;
            ListFor_ItemNo();
        }

        #region div_Supno
        /// <summary>
        /// 讀取 廠商編號清單
        /// </summary>
        private void ListFor_Supno() {
            GV_ClassList.DataSource=_edi_retn.Get_DeleteList_Supno();
            GV_ClassList.DataBind();
        }

        protected void GV_ClassList_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName != "Page")
            {
                GridViewRow row = ((GridViewRow)((WebControl)(e.CommandSource)).NamingContainer);
                string lbl_SupNo = ((Label)row.Cells[1].FindControl("lbl_sup_no")).Text;
                //if (e.CommandName == "Select_I_qthe_PLNO")
                //    I_qthe_PLNO_Select_Action(I_qthe_PLNO);
                if (e.CommandName == "DeleteButton")
                {
                    Sup_No_Delete(lbl_SupNo);
                }
            }
        }

        /// <summary>
        /// 刪除廠商邊號
        /// </summary>
        /// <param name="lbl_SupNo"></param>
        private void Sup_No_Delete(string lbl_SupNo)
        {
            bool IsOK=_edi_retn.del_Supno(lbl_SupNo, UI.UserID);
            if (IsOK)
            {
                lbl_Message.Text = "刪除廠商編號 " + lbl_SupNo + " 成功。";
                ListFor_Supno();
            }
            else {
                lbl_Message.Text = "刪除廠商編號 " + lbl_SupNo + " 失敗。";
            }
            return;
        }

        /// <summary>
        /// 新增廠商編號
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btn_SupNo_New_Click(object sender, EventArgs e)
        {
            string lbl_SupNo = txb_SupNo_New.Text;
            bool IsOK = _edi_retn.Insert_Supno(lbl_SupNo);
            if (IsOK)
            {
                lbl_Message.Text = "新增廠商編號 " + lbl_SupNo + " 成功。";
                ListFor_Supno();
            }
            else
            {
                lbl_Message.Text = "新增廠商編號 " + lbl_SupNo + " 失敗。";
            }
        }
        #endregion

        #region div_ItemNo
        /// <summary>
        /// 讀取 貨號清單
        /// </summary>
        private void ListFor_ItemNo()
        {
            GV_ItemNoList.DataSource = _edi_retn.Get_DeleteList_ItemNo();
            GV_ItemNoList.DataBind();
        }

        protected void GV_ItemNoList_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName != "Page")
            {
                GridViewRow row = ((GridViewRow)((WebControl)(e.CommandSource)).NamingContainer);
                string lbl_ItemNo = ((Label)row.Cells[1].FindControl("lbl_Item_id")).Text;
                //if (e.CommandName == "Select_I_qthe_PLNO")
                //    I_qthe_PLNO_Select_Action(I_qthe_PLNO);
                if (e.CommandName == "DeleteButton")
                {
                    ItemNo_Delete(lbl_ItemNo);
                }
            }
        }

        /// <summary>
        /// 刪除貨號
        /// </summary>
        /// <param name="lbl_ItemNo"></param>
        private void ItemNo_Delete(string lbl_ItemNo)
        {
            bool IsOK = _edi_retn.del_ItemNo(lbl_ItemNo, UI.UserID);
            if (IsOK)
            {
                lbl_Message.Text = "刪除廠商編號 " + lbl_ItemNo + " 成功。";
                ListFor_ItemNo();
            }
            else
            {
                lbl_Message.Text = "刪除廠商編號 " + lbl_ItemNo + " 失敗。";
            }
            return;
        }

        /// <summary>
        /// 新增貨號
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btn_ItemNo_New_Click(object sender, EventArgs e)
        {
            string lbl_ItemNo = txb_ItemNo.Text;
            bool IsOK = _edi_retn.Insert_ItemNo(lbl_ItemNo);
            if (IsOK)
            {
                lbl_Message.Text = "新增貨號 " + lbl_ItemNo + " 成功。";
                ListFor_ItemNo();
            }
            else
            {
                lbl_Message.Text = "新增貨號 " + lbl_ItemNo + " 失敗。";
            }
        }
        #endregion
    }
}