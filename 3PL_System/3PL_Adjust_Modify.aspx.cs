using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using _3PL_LIB;
using _3PL_DAO;
using System.Data;
using System.Linq;

namespace _3PL_System
{
    public partial class _3PL_Adjust_Modify : System.Web.UI.Page
    {
        _3PL_CommonQuery _3PLCQ = new _3PL_CommonQuery();
        _3PL_Adjust_DAO _3PLAdjust = new _3PL_Adjust_DAO();
        _3PL_BaseCostSet_DAO _3PLBCS = new _3PL_BaseCostSet_DAO();
        _3PL_SignOff_DAO _3PLSignOff = new _3PL_SignOff_DAO();

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
                CB.DropDownListBind(ref DDL_Adj_Type, _3PLCQ.GetFieldList(Login_Server, "AdjType"), "S_bsda_FieldId", "S_bsda_FieldName");

                hidTotal_I_qthe_seq.Value = Request.QueryString["Wk_Id"] == null ? "" : Request.QueryString["Wk_Id"].ToString();


                //判別新增/修改
                if (hidTotal_I_qthe_seq.Value == "")
                {
                    //新增
                    lbl_Quotation_Query.Text = "調整單新增";
                }
                else
                {
                    //修改
                    lbl_Quotation_Query.Text = "調整單異動";
                    Query_Head(hidTotal_I_qthe_seq.Value);
                }

            }
        }

        #region DropDownList changed

        protected void DDL_Adj_Type_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (DDL_Adj_Type.SelectedIndex)
            {
                case 0:
                    CB.DropDownListBind(ref ddl_OriginID, _3PLCQ.GetFieldList(Login_Server, "3PL_QuotationHead"), "S_bsda_FieldId", "S_bsda_FieldName");
                    break;
                case 1:
                    CB.DropDownListBind(ref ddl_OriginID, _3PLCQ.GetFieldList(Login_Server, "AssignHead"), "S_bsda_FieldId", "S_bsda_FieldName");
                    break;
            }

        }

        protected void ddl_OriginId_SelectedIndexChanged(object sender, EventArgs e)
        {
            lbl_ColName.Text = ddl_OriginID.SelectedValue;
        }
        #endregion

        #region Head

        #region 預先載入
        //預先載入調整單
        private void Query_Head(string I_qthe_seq)
        {
            DataTable dt_head = _3PLAdjust.GetHead(I_qthe_seq, UI, "ALL", true, "0");
            if (dt_head.Rows.Count <= 0)
            {
                ((_3PLMasterPage)Master).ShowMessage("找不到指定單號");
                //ScriptManager.RegisterClientScriptBlock(this, typeof(string), "alert", "alert('找不到指定單號')", true);
                return;
            }
            DataRow dr = dt_head.Rows[0];
            lbl_Adj_Id.Text = dr["Adj_Id"].ToString();

            DDL_Adj_Type.SelectedValue = dr["Adj_Type"].ToString();
            DDL_Adj_Type_SelectedIndexChanged(DDL_Adj_Type, new EventArgs());
            lbl_Adj_PageId.Text = dr["Adj_PageId"].ToString();
            txa_Memo.Value = dr["Memo"].ToString();

            Session["Quotation_head"] = dt_head;
            DataTable dt_Detail = _3PLAdjust.GetDetail(lbl_Adj_Id.Text);
            Session["Quotation_Detail"] = dt_Detail;

            GVBind_Quotation_Detail();
            if (dt_Detail.Rows.Count > 0)
                BringDetail(dt_Detail.Rows[0]);
            else
                BringDetail();

            DIV_Quotation_Detail_New.Visible = true;
            Btn_Detail_New_Update.Visible = true;

            Btn_QuotationHead_New.Text = "修改單頭";
            Btn_QuotationHead_Delete.Visible = true;
            //修改已成立的調整單,新增調整單,欄位要禁止更動
            Head_Disable();
        }

        //修改已成立的調整單,新增調整單,欄位要禁止更動
        private void Head_Disable()
        {
            //單頭

            //明細
            Btn_Detail_New_Update_Leave.Visible = true;
        }
        #endregion

        /// <summary>
        /// 新增/修改 調整單單頭
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Btn_QuotationHead_New_Click(object sender, EventArgs e)
        {
            #region 錯誤控制
            string CloseDate = _3PLCQ.Addon_GetCloseData();
            string ErrMsg = "";
            if (DDL_Adj_Type.Text == "")
                ErrMsg += "調整單類別未設定！\\n";
            if (lbl_Adj_PageId.Text == "")
                ErrMsg += "異動單號未設定！\\n";
            //檢查單號正確性
            ErrMsg += _3PLAdjust.CheckHeadAdjustPageId(DDL_Adj_Type.Text, lbl_Adj_PageId.Text);


            if (ErrMsg != "")
            {
                ((_3PLMasterPage)Master).ShowMessage(ErrMsg);
                //ScriptManager.RegisterClientScriptBlock(this, typeof(string), "alert", "alert('" + ErrMsg + "')", true);
                return;
            }
            #endregion

            if (Btn_QuotationHead_New.Text == "確定單頭")
            {
                DataTable dt_head = _3PLAdjust.GetHead("0", UI, "ALL", true, "0");
                DataRow dr = dt_head.NewRow();
                dr["Adj_Id"] = "";
                dr["Adj_Type"] = DDL_Adj_Type.SelectedValue;
                DDL_Adj_Type_SelectedIndexChanged(DDL_Adj_Type, new EventArgs());

                dr["Adj_PageId"] = lbl_Adj_PageId.Text;
                dr["Memo"] = txa_Memo.Value;
                dr["Status"] = "10";
                dr["CrtUser"] = UI.UserID;
                dr["UpdUser"] = UI.UserID;

                dr["UIStatus"] = "Added";
                dt_head.Rows.Add(dr);

                Session["Quotation_head"] = dt_head;
                DataTable dt_Detail = _3PLAdjust.GetDetail("");
                Session["Quotation_Detail"] = dt_Detail;

                GVBind_Quotation_Detail();
                BringDetail();

                DIV_Quotation_Detail_New.Visible = true;
                Btn_Detail_New_Update.Visible = true;

                Btn_QuotationHead_New.Text = "存檔自動更新單頭";
                Btn_QuotationHead_New.Enabled = false;
                Head_Disable();
            }
            else
            {
                DataTable dt_head = (DataTable)Session["Quotation_head"];
                if (dt_head == null)
                    return;
                DataRow dr = dt_head.Rows[0];
                dr["Adj_Type"] = DDL_Adj_Type.SelectedValue;
                dr["Adj_PageId"] = lbl_Adj_PageId.Text;
                dr["Memo"] = txa_Memo.Value;
                dr["UpdUser"] = UI.UserID;

                if (dr["UIStatus"].ToString() != "Added")
                    dr["UIStatus"] = "Modified";

                //ErrMsg = "修改單頭資料完成\\n請記得按下提交更新才是完整更新單頭";
                //((_3PLMasterPage)Master).ShowMessage(ErrMsg);
                //ScriptManager.RegisterClientScriptBlock(this, typeof(string), "alert", "alert('" + ErrMsg + "')", true);
            }
        }

        #endregion

        #region Detail

        #region View Control
        protected void GV_Quotation_Detail_New_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GV_Quotation_Detail_New.PageIndex = e.NewPageIndex;
            GVBind_Quotation_Detail();
        }
        //重新綁定
        private void GVBind_Quotation_Detail()
        {
            DataTable dt = (DataTable)Session["Quotation_Detail"];
            DataView dv = new DataView(dt);
            dv.RowFilter = "[UIStatus]<>'Deleted'";
            GV_Quotation_Detail_New.DataSource = dv;
            GV_Quotation_Detail_New.DataBind();


        }
        private void BringDetail()
        {
            ddl_OriginID.SelectedIndex = 0;
            ddl_OriginId_SelectedIndexChanged(ddl_OriginID, new EventArgs());

            txb_OriginValue.Text = _3PLAdjust.BringHeadAdjustPageId_Value(DDL_Adj_Type.Text, lbl_Adj_PageId.Text, ddl_OriginID.SelectedValue);
            txb_NewValue.Text = "";

            lbl_SN.Text = "";
            hidTotal_I_qtde_seq.Value = "";
        }
        private void BringDetail(DataRow dr)
        {
            if (dr["OriginID"].ToString() == "")
            {
                dr["OriginID"] = ddl_OriginID.Items[0].Value;
            }
            ddl_OriginID.SelectedValue = dr["OriginID"].ToString();
            lbl_ColName.Text = dr["OriginID"].ToString();
            ddl_OriginId_SelectedIndexChanged(ddl_OriginID, new EventArgs());

            txb_OriginValue.Text = dr["OriginValue"].ToString();
            txb_NewValue.Text = dr["newValue"].ToString();

            lbl_SN.Text = dr["SEQ"].ToString();
            hidTotal_I_qtde_seq.Value = dr["SN"].ToString();
        }

        //選定要更新/刪除的Row
        protected void GV_Quotation_Detail_New_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            //刪除
            if (e.CommandName == "DeleteButton")
            {
                GridViewRow Row = ((GridViewRow)((WebControl)(e.CommandSource)).NamingContainer);
                DataTable dt = (DataTable)Session["Quotation_Detail"];
                hidTotal_I_qtde_seq.Value = Row.Cells[1].Text;
                Btn_Detail_Del_Click(sender, e);
            }
            else
            {
                //更新
                if (e.CommandArgument.ToString() == string.Empty)
                {
                    GridViewRow Row = ((GridViewRow)((WebControl)(e.CommandSource)).NamingContainer);
                    DataTable dt = (DataTable)Session["Quotation_Detail"];
                    hidTotal_I_qtde_seq.Value = Row.Cells[1].Text;
                    object[] objParam = { hidTotal_I_qthe_seq.Value, hidTotal_I_qtde_seq.Value };
                    DataRow dr = dt.Rows.Find(objParam);
                    BringDetail(dr);

                    Btn_Detail_Upd.Visible = true;
                }
            }
        }

        #endregion

        #region 新增明細
        //錯誤控制
        private bool Detail_ErrControl()
        {
            #region 錯誤控制
            string ErrMsg = "";
            if (ddl_OriginID.SelectedIndex < 0)
                ErrMsg += "異動欄位未設定！\\n";
            if (txb_OriginValue.Text == "")
                ErrMsg += "元內容未設定！\\n";
            if (txb_NewValue.Text == "")
                ErrMsg += "異動內容未設定！\\n";

            if (ErrMsg != "")
            {
                ((_3PLMasterPage)Master).ShowMessage(ErrMsg);
                //ScriptManager.RegisterClientScriptBlock(this, typeof(string), "alert", "alert('" + ErrMsg + "')", true);
                return true;
            }
            return false;
            #endregion
        }

        //新增明細
        protected void Btn_Detail_New_Click(object sender, EventArgs e)
        {
            if (Detail_ErrControl())
            {
                return;
            }

            Btn_Detail_New_Click_SiteNo();

            GVBind_Quotation_Detail();
        }
        private void Btn_Detail_New_Click_SiteNo()
        {
            DataTable dt = (DataTable)Session["Quotation_Detail"];
            object MaxSeq_Pre = dt.Compute("max(SEQ)", "");
            int Detail_MaxSeq = Convert.ToInt32(MaxSeq_Pre == DBNull.Value ? 0 : MaxSeq_Pre) + 1;
            DataRow dr = dt.NewRow();
            dr["SEQ"] = Detail_MaxSeq;
            dr["SN"] = Detail_MaxSeq;
            dr["Adj_Id"] = hidTotal_I_qthe_seq.Value;
            dr["ColName"] = ddl_OriginID.SelectedItem.Text;
            dr["OriginID"] = ddl_OriginID.SelectedValue;
            dr["OriginValue"] = txb_OriginValue.Text;
            dr["NewValue"] = txb_NewValue.Text;
            dr["CrtUser"] = UI.UserID;
            dr["UpdUser"] = UI.UserID;

            dr["UIStatus"] = "Added";
            dt.Rows.Add(dr);
            BringDetail(dr);

            Btn_Detail_Upd.Visible = true;
        }
        #endregion

        #region 更新明細
        //更新明細
        protected void Btn_Detail_Upd_Click(object sender, EventArgs e)
        {
            if (Detail_ErrControl())
            {
                return;
            }

            DataTable dt = (DataTable)Session["Quotation_Detail"];
            object[] objParam = { hidTotal_I_qthe_seq.Value, hidTotal_I_qtde_seq.Value };
            DataRow dr = dt.Rows.Find(objParam);

            dr["OriginID"] = ddl_OriginID.SelectedValue;
            dr["OriginValue"] = txb_OriginValue.Text;
            dr["NewValue"] = txb_NewValue.Text;
            dr["UpdUser"] = UI.UserID;

            if (dr["UIStatus"].ToString() != "Added")
                dr["UIStatus"] = "Modified";

            ((_3PLMasterPage)Master).ShowMessage("明細修改完成");
            //ScriptManager.RegisterClientScriptBlock(this, typeof(string), "alert", "alert('明細修改完成')", true);
            GVBind_Quotation_Detail();
        }
        #endregion

        #region 刪除明細
        //刪除明細
        protected void Btn_Detail_Del_Click(object sender, EventArgs e)
        {
            DataTable dt = (DataTable)Session["Quotation_Detail"];
            object[] objParam = { hidTotal_I_qthe_seq.Value, hidTotal_I_qtde_seq.Value };
            DataRow dr = dt.Rows.Find(objParam);
            dr["UIStatus"] = "Deleted";

            Btn_Detail_Upd.Visible = false;

            GVBind_Quotation_Detail();
        }
        #endregion

        #endregion

        #region 單據修改完成,按下確定
        //回寫資料
        protected void Btn_Detail_New_Update_Click(object sender, EventArgs e)
        {
            string ErrMsg = "";
            bool IsSuccess = false;
            DataTable dt_Detail = (DataTable)Session["Quotation_Detail"];

            string AdjustNo = "";

            if (ErrMsg == "")
            {
                if (GV_Quotation_Detail_New.Rows.Count <= 0)
                {
                    ErrMsg += "明細無資料！\\n";
                }
                else
                {
                    #region 自動更新單頭
                    Btn_QuotationHead_New_Click(sender, e);
                    #endregion

                    DataTable dt_head = (DataTable)Session["Quotation_head"];

                    //新增
                    if (hidTotal_I_qthe_seq.Value == "")
                    {
                        //取得最大單號
                        AdjustNo = _3PLCQ.GetMaxPageNo(Login_Server, 4);

                        //新增
                        IsSuccess = _3PLAdjust.InsertAdjust(Login_Server, dt_head, dt_Detail, AdjustNo, UI.UserID);
                        if (IsSuccess)
                            ErrMsg += "新增成功，單號：" + AdjustNo + "！\\n";
                        else
                        {
                            ErrMsg += "新增失敗！\\n";
                        }
                    }
                    else
                    {
                        //取得原有單號
                        AdjustNo = dt_head.Rows[0]["Adj_Id"].ToString();

                        //修改
                        IsSuccess = _3PLAdjust.InsertAdjust(Login_Server, dt_head, dt_Detail, AdjustNo, UI.UserID);
                        if (IsSuccess)
                            ErrMsg += "修改成功！\\n";
                        else
                        {
                            ErrMsg += "修改失敗！\\n";
                        }
                    }
                }
            }

            //跳出錯誤訊息
            if (ErrMsg != "")
            {
                if (IsSuccess)
                {
                    ((_3PLMasterPage)Master).ShowMessage(ErrMsg, CreatePath(AdjustNo));
                }
                else
                {
                    ((_3PLMasterPage)Master).ShowMessage(ErrMsg);
                }
            }
        }

        //取消更新
        protected void Btn_Detail_New_Update_Leave_Click(object sender, EventArgs e)
        {
            Response.Redirect(CreatePath(""));
        }

        /// <summary>
        /// 建立回傳路徑
        /// </summary>
        /// <returns></returns>
        private string CreatePath(string QuotationNo)
        {

            string Path = "";
            if (QuotationNo != "")
            {
                Path = string.Format("3PL_Adjust_Query.aspx?VarString={0}", "0," + QuotationNo);
            }
            else if (Request.QueryString["VarString"] != null && Request.QueryString["VarString"] != "")
            {
                Path = string.Format("3PL_Adjust_Query.aspx?VarString={0}", Request.QueryString["VarString"]);
            }
            return Path;
        }
        #endregion

        #region 調整單作廢

        //開啟調整單作廢
        protected void Btn_QuotationHead_Delete_Click(object sender, EventArgs e)
        {
            DataTable dt_head = (DataTable)Session["Quotation_head"];
            string PLNO = dt_head.Rows[0]["Adj_Id"].ToString(),
                Status = dt_head.Rows[0]["Status"].ToString(),
                PageType = "4";
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
                    lbl_TypeFee.Text = "派工單退回";
                    lbl_ObjNo.Text = "派工單號"; break;
                case "4":
                    lbl_TypeFee.Text = "調整單退回";
                    lbl_ObjNo.Text = "調整單號"; break;
            }
            ScriptManager.RegisterClientScriptBlock(Page, typeof(string), "c", "show_table_SignOff_Back();", true);
        }

        //調整單作廢按下OK
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
                ((_3PLMasterPage)Master).ShowMessage("調整單作廢成功", CreatePath(txb_ObjNo.Text));
            }
            else
            {
                ((_3PLMasterPage)Master).ShowMessage("調整單作廢失敗");
            }
        }
        #endregion
    }
}
