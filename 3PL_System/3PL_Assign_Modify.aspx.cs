using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using _3PL_LIB;
using _3PL_DAO;
using System.Data;
using System.Threading;
using System.Collections.Generic;

namespace _3PL_System
{
    public partial class _3PL_Assign_Modify : System.Web.UI.Page
    {
        _3PL_CommonQuery _3PLCQ = new _3PL_CommonQuery();
        _3PL_Assign_DAO _3PLAssign = new _3PL_Assign_DAO();
        _3PL_DAO._3PL_Assign_New _3pa = new _3PL_DAO._3PL_Assign_New();
        _3PL_BaseCostSet_DAO _3PLBCS = new _3PL_BaseCostSet_DAO();
        _3PL_SignOff_DAO _3PLSignOff = new _3PL_SignOff_DAO();
        EmpInf _3PLEmpInf = new EmpInf();

        List<Thread> myAnswerThreads = new List<Thread>();

        ControlBind CB = new ControlBind();
        Check _3PL_Check = new Check();
        private UserInf UI = new UserInf();
        private string Login_Server = "3PL";

        protected void Page_Load(object sender, EventArgs e)
        {
            ((_3PLMasterPage)Master).SessionCheck(ref UI);
            if (!IsPostBack)
            {
                //產生作業大類下拉式選單
                CB.DropDownListBind(ref DDL_DC, _3PLCQ.GetFieldList(Login_Server, "SiteNo"), "S_bsda_FieldId", "S_bsda_FieldName", "請選擇", "");

                hidTotal_Wk_Id.Value = Request.QueryString["Wk_Id"] == null ? "" : Request.QueryString["Wk_Id"].ToString();

                //修改
                lbl_Quotation_Query.Text = "派工單異動";
                Query_Head(hidTotal_Wk_Id.Value);
            }
        }

        #region Head

        //預先載入派工單
        private void Query_Head(string I_qthe_seq)
        {
            DataTable dt_head = _3PLAssign.GetAssignList(hidTotal_Wk_Id.Value, UI);
            if (dt_head.Rows.Count <= 0)
            {
                ((_3PLMasterPage)Master).ShowMessage("找不到指定單號");
                return;
            }
            DataRow dr = dt_head.Rows[0];
            if (dr["Status"].ToString() == "0")
            {
                Btn_Assign_Delete.Visible = false;
                Btn_Detail_New_Update.Visible = false;
            }

            //派工日期
            if (dr["Wk_Date"].ToString() != "")
                Txb_Wk_Date.Text = DateTime.Parse(dr["Wk_Date"].ToString()).ToString("yyyy/MM/dd");
            else
                Txb_Wk_Date.Text = "";
            //報價主類別
            Txb_Wk_ClassName.Text = dr["Wk_ClassName"].ToString();
            //倉別
            DDL_DC.SelectedValue = dr["DC"].ToString();
            //預定完工日
            if (dr["EtaDate"].ToString() != "")
                Txb_EtaDate.Text = DateTime.Parse(dr["EtaDate"].ToString()).ToString("yyyy/MM/dd");
            else
                Txb_EtaDate.Text = "";
            //建單人
            Txb_CreateUser.Text = dr["CrtUser"].ToString() + "," + dr["CrtUserName"].ToString();
            //處理單位
            Txb_Wk_Unit.Text = dr["Wk_Unit"].ToString() + "," + dr["Wk_UnitName"].ToString();
            //廠商編號
            Txb_SupID.Text = dr["SupID"].ToString();
            //實際完工日
            if (dr["ActDate"].ToString() != "")
                Txb_ActDate.Text = DateTime.Parse(dr["ActDate"].ToString()).ToString("yyyy/MM/dd");
            else
                Txb_ActDate.Text = "";
            //派工單號
            txb_AssignHead_Wk_Id.Text = hidTotal_Wk_Id.Value;
            //報價單號
            Txb_S_qthe_PLNO.Text = dr["FreeId"].ToString();
            //Memo
            txa_Memo.Value = dr["Memo"].ToString();

            Session["Quotation_head"] = dt_head;

            //帶出明細資訊
            BringDetail();

            int PageStep = Convert.ToInt32(dr["Step"].ToString());
            if (PageStep <= 1)  //建單階段
            {
                GV_Quotation_HideCells(true);
            }
            else
            {
                GV_Quotation_HideCells(false);
            }
        }

        /// <summary>
        /// 單據狀態不同,顯示欄位有差別
        /// </summary>
        /// <param name="CellVisible"></param>
        private void GV_Quotation_HideCells(bool CellVisible)
        {
            //預計完工日
            Txb_EtaDate.Enabled = CellVisible;
            //派工數量,完工數量顯示
            foreach (GridViewRow gvr in GV_Quotation_Detail_New.Rows)
            {
                ((Label)gvr.Cells[3].FindControl("Lbl_Qty")).Visible = !CellVisible;
                ((TextBox)gvr.Cells[2].FindControl("Txb_Qty")).Visible = CellVisible;
                ((Label)gvr.Cells[3].FindControl("Lbl_RealQty")).Visible = CellVisible;
                ((TextBox)gvr.Cells[3].FindControl("Txb_RealQty")).Visible = !CellVisible;
            }
            //派工單Memo顯示
            if (!CellVisible)
            {
                //不顯示的時候
                txa_Memo.Style.Add("background-image", "linear-gradient(to bottom,#CCCCB2,#E6E6E6)");
                txa_Memo.Style.Add("background-color", "#CCCCB2");
                txa_Memo.Disabled = true;
            }

            //派工單作廢
            Btn_Assign_Delete.Visible = CellVisible;
            //派工單明細_PO貨號
            DIV_Quotation_Detail_New_POItem.Visible = CellVisible;
            //派工單明細_PO貨號:刪除
            foreach (GridViewRow gvr in GV_Quotation_Detail_New.Rows)
            {
                ((Button)gvr.Cells[0].FindControl("btn_GV_Quotation_Detail_New_Del")).Visible = CellVisible;
            }
        }

        #endregion

        #region Detail
        //帶出明細資訊
        private void BringDetail()
        {
            //原始的派工單明細
            DataTable AssignDetail_Origin = _3PLAssign.GetAssignDetail(hidTotal_Wk_Id.Value);
            //帶出顯示用的派工單明細,最終結果
            DataTable Quotation_Detail = AssignDetail_Origin.Copy();

            Session["Quotation_Detail"] = Quotation_Detail;
            GVBind_Quotation_Detail();

            DIV_Quotation_Detail_New_POItem.Visible = true;
            DIV_Quotation_Detail_New.Visible = true;

            //帶出派工單原始價格設定, 已有派工單的情況
            DataTable dt_detail = AssignDetail_Origin.Clone();
            if (AssignDetail_Origin.Rows.Count > 0)
            {
                string PO_NO = AssignDetail_Origin.Rows[0]["PONO"].ToString();
                string item_no = AssignDetail_Origin.Rows[0]["itemno"].ToString();

                Session["MaxSeq"] = 0;
                foreach (DataRow dr in AssignDetail_Origin.Rows)
                {
                    dr["RealQty"] = DBNull.Value;
                    if (dr["PONO"].ToString() == PO_NO && dr["itemno"].ToString() == item_no)
                    {
                        dt_detail.ImportRow(dr);
                    }
                    if (Convert.ToInt32(dr["Seq"]) > (int)Session["MaxSeq"])
                        Session["MaxSeq"] = Convert.ToInt32(dr["Seq"]);
                }
            }
            Session["dt_detail"] = dt_detail;
        }
        //重新綁定
        private void GVBind_Quotation_Detail()
        {
            DataTable Quotation_Detail = (DataTable)Session["Quotation_Detail"];
            DataView dv = new DataView(Quotation_Detail);
            dv.RowFilter = "[UIStatus]<>'Deleted'";
            GV_Quotation_Detail_New.DataSource = dv;
            GV_Quotation_Detail_New.DataBind();
        }

        #endregion

        #region 單據修改完成,按下確定
        /// <summary>
        /// 檢查有無設定完工日期
        /// </summary>
        /// <returns></returns>
        private bool Head_ErrControl_Upd()
        {
            string CloseDate = _3PLCQ.Addon_GetCloseData();
            string ErrMsg = "";
            if (!_3PL_Check.CkDate(Txb_EtaDate.Text))
                ErrMsg += "預定完工日期未設定！\\n";
            if (_3PL_Check.strSEDate(CloseDate, Txb_EtaDate.Text) == false)
                ErrMsg += "預定完工日期小於關帳日期" + CloseDate + "！\\n";
            if (ErrMsg != "")
            {
                ((_3PLMasterPage)Master).ShowMessage(ErrMsg);
                return true;
            }
            return false;
        }
        /// <summary>
        /// 檢查派工單明細
        /// </summary>
        /// <returns></returns>
        private bool Detail_ErrControl_Upd()
        {
            string ErrMsg = "";

            string OrdQty = "", RealQty = "";
            foreach (GridViewRow gvr in GV_Quotation_Detail_New.Rows)
            {
                OrdQty = ((TextBox)gvr.Cells[2].FindControl("Txb_Qty")).Text;
                if (OrdQty == "")
                    OrdQty = "0";
                if (!_3PL_Check.CkDecimal(OrdQty))
                {
                    ErrMsg = "派工數量輸入錯誤";
                    break;
                }
                RealQty = ((TextBox)gvr.Cells[3].FindControl("Txb_RealQty")).Text;
                if (RealQty == "")
                    RealQty = "0";
                if (!_3PL_Check.CkDecimal(RealQty))
                {
                    ErrMsg = "完工數量輸入錯誤";
                    break;
                }

                if (Convert.ToInt32(RealQty) > Convert.ToInt32(OrdQty))
                {
                    ErrMsg = "完工數量:" + RealQty + " 大於 派工數量:" + OrdQty + "<p>"
                            + "請確定是否存檔??";
                    div_Message_Confirm.Style.Add("display", "inline");
                    lbl_Message_YesNo.Text = ErrMsg.Replace(@"\n", "<br/>");
                    hid_Confirm_Value.Value = "";

                    return true;
                }
            }
            if (ErrMsg != "")
            {
                ((_3PLMasterPage)Master).ShowMessage(ErrMsg);
                return true;
            }
            return false;
        }

        #region 確認小視窗
        protected void btn_Close_div_Message_Close_Click(object sender, EventArgs e)
        {
            if (div_Message_Confirm.Style["display"] != "none")
            {
                div_Message_Confirm.Style.Add("display", "none");
            }
        }
        protected void btn_close_div_Message_Confirm_Click(object sender, EventArgs e)
        {
            hid_Confirm_Value.Value = "YES";
            if (div_Message_Confirm.Style["display"] != "none")
            {
                div_Message_Confirm.Style.Add("display", "none");
            }
            Btn_Detail_New_Update_Click(Btn_Detail_New_Update, e);
        }
        #endregion

        //回寫資料
        protected void Btn_Detail_New_Update_Click(object sender, EventArgs e)
        {
            string ErrMsg = "";
            bool IsSuccess = false;

            #region 錯誤控制
            if (hid_Confirm_Value.Value == "")
            {
                if (Head_ErrControl_Upd())
                {
                    return;
                }
                if (Detail_ErrControl_Upd())
                {
                    return;
                }
            }
            #endregion

            DataTable Quotation_head = (DataTable)Session["Quotation_head"];
            DataTable Quotation_Detail = (DataTable)Session["Quotation_Detail"];

            #region Head,將資料寫入至Session的Table,Step=1才需要
            DataRow drHead = Quotation_head.Rows[0];
            int PageStep = Convert.ToInt32(drHead["Step"].ToString());
            if (PageStep <= 1)
            {
                drHead["EtaDate"] = Txb_EtaDate.Text;
                drHead["UpdUser"] = UI.UserID;
                drHead["Memo"] = txa_Memo.Value;
            }
            #endregion

            #region Detail,將資料寫入至Session的Table
            string Wk_Id = hidTotal_Wk_Id.Value;
            string Seq = "", OrdQty = "", RealQty = "";
            DataRow dr;
            foreach (GridViewRow gvr in GV_Quotation_Detail_New.Rows)
            {
                Seq = ((Label)gvr.Cells[0].FindControl("Lbl_Seq")).Text;
                object[] objParam = { Wk_Id, Seq };
                dr = Quotation_Detail.Rows.Find(objParam);

                if (Convert.ToInt32(dr["Sn"]) < 0 && dr["UIStatus"].ToString() == "Deleted")
                    dr.Delete();

                OrdQty = ((TextBox)gvr.Cells[2].FindControl("Txb_Qty")).Text;
                if (OrdQty == "")
                    dr["Qty"] = DBNull.Value;
                else
                    dr["Qty"] = OrdQty;

                RealQty = ((TextBox)gvr.Cells[3].FindControl("Txb_RealQty")).Text;
                if (RealQty == "")
                    dr["RealQty"] = DBNull.Value;
                else
                    dr["RealQty"] = RealQty;

                dr["UpdUser"] = UI.UserID;

                if (dr["UIStatus"].ToString() != "Added")
                    dr["UIStatus"] = "Modified";
            }
            Quotation_Detail.AcceptChanges();
            #endregion

            #region 更新DB
            if (GV_Quotation_Detail_New.Rows.Count <= 0)
            {
                ErrMsg += "明細無資料！\\n";
            }
            else
            {
                //修改
                IsSuccess = _3PLAssign.InsertQuotation(Login_Server, Quotation_head, Quotation_Detail, UI.UserID);
                if (IsSuccess)
                    ErrMsg += "修改成功！\\n";
                else
                {
                    ErrMsg += "修改失敗！\\n";
                }
            }
            #endregion

            //跳出錯誤訊息
            if (ErrMsg != "")
            {
                if (IsSuccess)
                {
                    ((_3PLMasterPage)Master).ShowMessage(ErrMsg, CreatePath());
                }
                else
                {
                    ((_3PLMasterPage)Master).ShowMessage(ErrMsg);
                }
            }
        }

        //取消修改單據
        protected void Btn_Detail_New_Update_Leave_Click(object sender, EventArgs e)
        {
            Response.Redirect(CreatePath());
        }

        /// <summary>
        /// 建立回傳路徑
        /// </summary>
        /// <returns></returns>
        private string CreatePath()
        {
            string Path = "";
            if (Request.QueryString["VarString"] != "")
            {
                Path = string.Format("3PL_Assign_Query.aspx?VarString={0}", Request.QueryString["VarString"]);
            }
            else
            {
                Path = string.Format("3PL_Assign_Query.aspx");
            }
            return Path;
        }
        #endregion

        #region 派工單作廢

        //開啟派工單作廢
        protected void Btn_Assign_Delete_Click(object sender, EventArgs e)
        {
            DataTable dt_head = (DataTable)Session["Quotation_head"];
            string PLNO = dt_head.Rows[0]["Wk_Id"].ToString(),
                Status = dt_head.Rows[0]["Status"].ToString(),
                PageType = "2";
            txb_ObjNo.Text = PLNO;
            txb_Status.Text = Status;
            txb_PageType.Text = PageType;
            switch (PageType)
            {
                case "1":
                    lbl_TypeFee.Text = "報價單退回";
                    lbl_ObjNo.Text = "報價單號"; break;
                case "2":
                    lbl_TypeFee.Text = "派工單退回";
                    lbl_ObjNo.Text = "派工單號"; break;
                case "3":
                    lbl_TypeFee.Text = "成本單退回";
                    lbl_ObjNo.Text = "成本單號"; break;
            }
            ScriptManager.RegisterClientScriptBlock(Page, typeof(string), "c", "show_table_SignOff_Back();", true);
        }

        //派工單作廢按下OK
        protected void btn_SignOff_OK_Click(object sender, EventArgs e)
        {
            if (txb_ObjName.Text == "")
            {
                ((_3PLMasterPage)Master).ShowMessage("未輸入作廢原因");
                ScriptManager.RegisterClientScriptBlock(Page, typeof(string), "c", "show_table_SignOff_Back();", true);
                return;
            }

            //簽核取消
            int IsSuccess = _3PLSignOff.SignOff_Quotation(Login_Server, UI.UserID, UI.UserName, false, txb_Status.Text, txb_ObjNo.Text, txb_PageType.Text);

            //寫入原因
            _3PLSignOff.SignOffBackReason(Login_Server, UI.UserID, txb_Status.Text, txb_ObjNo.Text, txb_ObjName.Text, txb_PageType.Text);
            if (IsSuccess > 0)
            {
                ((_3PLMasterPage)Master).ShowMessage("派工單作廢成功", CreatePath());
            }
            else
            {
                ((_3PLMasterPage)Master).ShowMessage("派工單作廢失敗");
            }
        }
        #endregion

        #region 產生派工單明細
        //產生明細
        protected void Btn_Query_Click(object sender, EventArgs e)
        {
            DataTable Quotation_Detail = (DataTable)Session["Quotation_Detail"];
            DataTable dt_detail = (DataTable)Session["dt_detail"];

            string PO_NO = txb_D_qthe_ContractS_Qry.Text;
            string item_no = txb_D_qthe_ContractE_Qry.Text;
            string site_no = DDL_DC.SelectedValue;

            #region 檢查目的地有沒有相同key值
            foreach (DataRow dr in Quotation_Detail.Rows)
            {
                if (dr["UIStatus"].ToString() == "Deleted")
                    continue;
                if (dr["PONO"].ToString() == PO_NO && dr["itemno"].ToString() == item_no)
                {
                    ((_3PLMasterPage)Master).ShowMessage("已有相同關鍵字明細");
                    return;
                }
            }
            #endregion

            #region 帶入PO單數量
            DataTable POlist = _3pa.getPOList(PO_NO, item_no, site_no, Txb_SupID.Text.Substring(0, 4));
            string TQty = POlist.Rows[0][0].ToString(),
                    TBox = POlist.Rows[0][1].ToString(),
                    TPallet = POlist.Rows[0][2].ToString();
            if (TQty == null || TQty == "")
                TQty = "0";
            if (TBox == null || TBox == "")
                TBox = "0";
            if (TPallet == null || TPallet == "")
                TPallet = "0";
            if (PO_NO != "" && TQty == "0")
            {
                ((_3PLMasterPage)Master).ShowMessage("查無指定PO單及貨號");
                return;
            }

            foreach (DataRow dr in dt_detail.Rows)
            {
                dr["PONO"] = PO_NO;
                dr["itemno"] = item_no;
                //帶入數量
                dr["Qty"] = _3PLAssign.GetDetailNeedQty(POlist.Rows[0], dr["Unit"].ToString());

                //帶入序號
                Session["MaxSeq"] = (int)Session["MaxSeq"] + 1;
                dr["Seq"] = Session["MaxSeq"];
                //新增資料不會有Sn
                dr["Sn"] = -1;
                dr["UIStatus"] = "Added";
            }
            dt_detail.AcceptChanges();
            #endregion

            Quotation_Detail.Merge(dt_detail);

            GVBind_Quotation_Detail();
        }
        //選擇要刪除的row
        protected void GV_Quotation_Detail_New_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName != "Page")
            {
                GridViewRow row = ((GridViewRow)((WebControl)(e.CommandSource)).NamingContainer);
                if (e.CommandName == "EdtSCdata")
                {
                    string PO_NO = ((Label)row.Cells[0].FindControl("Lbl_PONO")).Text;
                    string itemno = ((Label)row.Cells[0].FindControl("Lbl_itemno")).Text;
                    GridView1_DeleteRows(PO_NO, itemno);
                }
            }
        }
        //刪除view的row
        private void GridView1_DeleteRows(string PO_NO, string item_no)
        {
            DataTable Quotation_Detail = (DataTable)Session["Quotation_Detail"];
            foreach (DataRow dr in Quotation_Detail.Rows)
            {
                if (dr.RowState != DataRowState.Deleted)
                    if (dr["PONO"].ToString() == PO_NO && dr["itemno"].ToString() == item_no)
                    {
                        dr["UIStatus"] = "Deleted";
                    }
            }

            GVBind_Quotation_Detail();
        }
        #endregion
    }
}
