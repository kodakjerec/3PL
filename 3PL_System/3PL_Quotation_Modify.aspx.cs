using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using _3PL_LIB;
using _3PL_DAO;
using System.Data;
using System.Linq;

namespace _3PL_System
{
    public partial class _3PL_Quotation_Modify : System.Web.UI.Page
    {
        _3PL_CommonQuery _3PLCQ = new _3PL_CommonQuery();
        _3PL_Quotation_DAO _3PLQu = new _3PL_Quotation_DAO();
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
                CB.DropDownListBind(ref ddl_TypeId, _3PLCQ.GetFieldList(Login_Server, "TypeId"), "S_bsda_FieldId", "S_bsda_FieldName", "請選擇", "");
                CB.CheckBoxListBind(ref CheckBoxList_Quotation, _3PLCQ.GetFieldList(Login_Server, "SiteNo"), "S_bsda_FieldId", "S_bsda_FieldName");
                CB.CheckBoxListBind(ref CheckBoxList_Detail, _3PLCQ.GetFieldList(Login_Server, "SiteNo"), "S_bsda_FieldId", "S_bsda_FieldName");
                CB.DropDownListBind(ref ddl_HaveMinimum, _3PLCQ.GetFieldList(Login_Server,"Minimum"), "S_bsda_FieldId", "S_bsda_FieldName");


                hidTotal_I_qthe_seq.Value = Request.QueryString["I_qthe_Seq"] == null ? "" : Request.QueryString["I_qthe_Seq"].ToString();


                //判別新增/修改
                if (hidTotal_I_qthe_seq.Value == "")
                {
                    //新增
                    lbl_Quotation_Query.Text = "報價單新增";
                }
                else
                {
                    //修改
                    lbl_Quotation_Query.Text = "報價單異動";
                    Query_Head(hidTotal_I_qthe_seq.Value);
                }

            }
        }

        #region Page functions

        //帶出供應商代號
        protected void Btn_S_qthe_SupdId_New_Click(object sender, EventArgs e)
        {
            string Path = "3PL_SupdSelect.aspx?ReturnLocation=" + Txb_S_qthe_SupdId_New.ClientID +
                "&ReturnLocation2=" + Txb_S_qthe_SupdName_New.ClientID +
                "&btnCloseID=" + ((_3PLMasterPage)Master).FindControl("btn_Close_div_URL").ClientID; ;
            ((_3PLMasterPage)Master).ShowURL(Path);
        }

        //其他議價單的欄位要Disable
        protected void Chk_IsSpecial_CheckedChanged(object sender, EventArgs e)
        {
            if (Chk_IsSpecial.Checked == true)
            {
                Txb_Price.Enabled = true;
                //Txb_PriceMemo.Enabled = true;
                //Chk_IsBaseCost.Enabled = true;
            }
            else
            {
                Txb_Price.Enabled = false;
                //Txb_PriceMemo.Enabled = false;
                //Chk_IsBaseCost.Enabled = false;
            }
        }

        #endregion

        #region Head

        #region Head-View
        //報價單鎖定禁止更改
        private void Enable_Quotation_Head(bool State)
        {
            Btn_S_qthe_SupdId_New.Enabled = State;
            Txb_S_qthe_SupdId_New.Enabled = State;
        }

        #endregion

        #region Head-Controller-動作
        //新增/修改 報價單單頭
        protected void Btn_QuotationHead_New_Click(object sender, EventArgs e)
        {
            #region 錯誤控制
            string CloseDate = _3PLCQ.Addon_GetCloseData();
            string ErrMsg = "";
            if (Txb_S_qthe_SupdId_New.Text == "")
                ErrMsg += "供應商未設定！\\n";
            if (Txb_D_qthe_ContractS.Text == "")
                ErrMsg += "報價日期起未設定！\\n";
            if (Txb_D_qthe_ContractE.Text == "")
                ErrMsg += "報價日期迄未設定！\\n";
            if (CheckBoxList_Quotation.Items[0].Selected == false &&
                CheckBoxList_Quotation.Items[1].Selected == false &&
                CheckBoxList_Quotation.Items[2].Selected == false)
                ErrMsg += "倉別未指定！\\n";
            if (_3PL_Check.strSEDate(CloseDate, Txb_D_qthe_ContractS.Text) == false)
                ErrMsg += "報價日期起小於關帳日期" + CloseDate + "！\\n";
            if (_3PL_Check.strSEDate(CloseDate, Txb_D_qthe_ContractE.Text) == false)
                ErrMsg += "報價日期迄小於關帳日期" + CloseDate + "！\\n";
            if (ErrMsg != "")
            {
                ((_3PLMasterPage)Master).ShowMessage(ErrMsg);
                //ScriptManager.RegisterClientScriptBlock(this, typeof(string), "alert", "alert('" + ErrMsg + "')", true);
                return;
            }
            #endregion

            if (Btn_QuotationHead_New.Text == "確定單頭")
            {
                DataTable dt_head = _3PLQu.GetQuotationList("None", "0", 0, "", "", "", UI, true);
                DataRow dr = dt_head.NewRow();
                dr["S_qthe_PLNO"] = "";
                dr["S_qthe_SupdId"] = Txb_S_qthe_SupdId_New.Text;
                dr["I_qthe_IsSpecial"] = Chk_IsSpecial.Checked == true ? 1 : 0;
                dr["D_qthe_ContractS"] = Txb_D_qthe_ContractS.Text;
                dr["D_qthe_ContractE"] = Txb_D_qthe_ContractE.Text;
                dr["S_qthe_Memo"] = Txb_S_qthe_Memo.Text;
                dr["S_qthe_TEL"] = txb_S_qthe_TEL.Text;
                dr["S_qthe_BOSS"] = txb_S_qthe_BOSS.Text;
                dr["S_qthe_FAX"] = txb_S_qthe_FAX.Text;
                dr["S_qthe_CreateId"] = UI.UserID;
                dr["S_qthe_UpdId"] = UI.UserID;
                dr["Flag_DC1"] = CheckBoxList_Quotation.Items[0].Selected == true ? "1" : "0";
                dr["Flag_DC2"] = CheckBoxList_Quotation.Items[1].Selected == true ? "1" : "0";
                dr["Flag_DC3"] = CheckBoxList_Quotation.Items[2].Selected == true ? "1" : "0";

                dr["UIStatus"] = "Added";
                dt_head.Rows.Add(dr);

                Session["Quotation_head"] = dt_head;
                Session["Quotation_Detail"] = _3PLQu.GetQuotationDetail("");

                GVBind_Quotation_Detail();
                Enable_Quotation_Head(false);

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
                dr["I_qthe_IsSpecial"] = Chk_IsSpecial.Checked == true ? 1 : 0;
                dr["D_qthe_ContractS"] = Txb_D_qthe_ContractS.Text;
                dr["D_qthe_ContractE"] = Txb_D_qthe_ContractE.Text;
                dr["S_qthe_Memo"] = Txb_S_qthe_Memo.Text;
                dr["S_qthe_TEL"] = txb_S_qthe_TEL.Text;
                dr["S_qthe_BOSS"] = txb_S_qthe_BOSS.Text;
                dr["S_qthe_FAX"] = txb_S_qthe_FAX.Text;
                dr["S_qthe_UpdId"] = UI.UserID;
                dr["Flag_DC1"] = CheckBoxList_Quotation.Items[0].Selected == true ? "1" : "0";
                dr["Flag_DC2"] = CheckBoxList_Quotation.Items[1].Selected == true ? "1" : "0";
                dr["Flag_DC3"] = CheckBoxList_Quotation.Items[2].Selected == true ? "1" : "0";

                if (dr["UIStatus"].ToString() != "Added")
                    dr["UIStatus"] = "Modified";

                //ErrMsg = "修改單頭資料完成\\n請記得按下提交更新才是完整更新單頭";
                //((_3PLMasterPage)Master).ShowMessage(ErrMsg);
                //ScriptManager.RegisterClientScriptBlock(this, typeof(string), "alert", "alert('" + ErrMsg + "')", true);
            }
        }

        //預先載入報價單
        private void Query_Head(string I_qthe_seq)
        {
            DataTable dt_head = _3PLQu.GetQuotationList(Convert.ToInt32(I_qthe_seq), UI);
            if (dt_head.Rows.Count <= 0)
            {
                ((_3PLMasterPage)Master).ShowMessage("找不到指定單號");
                //ScriptManager.RegisterClientScriptBlock(this, typeof(string), "alert", "alert('找不到指定單號')", true);
                return;
            }
            DataRow dr = dt_head.Rows[0];
            Txb_S_qthe_PLNO.Text = dr["S_qthe_PLNO"].ToString();
            string[] SupdId = dr["供應商"].ToString().Split(',');
            Txb_S_qthe_SupdId_New.Text = SupdId[0];
            Txb_S_qthe_SupdName_New.Text = SupdId[1];
            Chk_IsSpecial.Checked = (bool)dr["I_qthe_IsSpecial"];
            object sender = new object();
            EventArgs e = new EventArgs();
            Chk_IsSpecial_CheckedChanged(sender, e);

            Txb_D_qthe_ContractS.Text = DateTime.Parse(dr["D_qthe_ContractS"].ToString()).ToString("yyyy/MM/dd");
            Txb_D_qthe_ContractE.Text = DateTime.Parse(dr["D_qthe_ContractE"].ToString()).ToString("yyyy/MM/dd");
            Txb_S_qthe_Memo.Text = dr["S_qthe_Memo"].ToString();
            txb_S_qthe_TEL.Text = dr["S_qthe_TEL"].ToString();
            txb_S_qthe_BOSS.Text = dr["S_qthe_BOSS"].ToString();
            txb_S_qthe_FAX.Text = dr["S_qthe_FAX"].ToString();
            CheckBoxList_Quotation.Items[0].Selected = (dr["Flag_DC1"].ToString() == "1" ? true : false);
            CheckBoxList_Quotation.Items[1].Selected = (dr["Flag_DC2"].ToString() == "1" ? true : false);
            CheckBoxList_Quotation.Items[2].Selected = (dr["Flag_DC3"].ToString() == "1" ? true : false);


            Session["Quotation_head"] = dt_head;
            Session["Quotation_Detail"] = _3PLQu.GetQuotationDetail(Txb_S_qthe_PLNO.Text);

            GVBind_Quotation_Detail();
            Enable_Quotation_Head(false);

            DIV_Quotation_Detail_New.Visible = true;
            Btn_Detail_New_Update.Visible = true;

            Btn_QuotationHead_New.Text = "修改單頭";
            Btn_QuotationHead_Delete.Visible = true;
            //修改已成立的報價單,新增報價單,欄位要禁止更動
            Head_Disable();
        }

        //修改已成立的報價單,新增報價單,欄位要禁止更動
        private void Head_Disable()
        {
            //單頭
            Chk_IsSpecial.Enabled = false;
            CheckBoxList_Quotation.Enabled = false;

            //明細
            int i = 0;
            foreach (ListItem item in CheckBoxList_Quotation.Items)
            {
                if (item.Selected)
                    CheckBoxList_Detail.Items[i].Selected = true;
                else
                    CheckBoxList_Detail.Items[i].Enabled = false;
                i += 1;
            }
            Btn_Detail_New_Update_Leave.Visible = true;
        }
        #endregion

        #endregion

        #region Detail
        /// <summary>
        /// 換頁
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        #region Detail-Controler
        /// <summary>
        /// 選定報價主類別後，帶出計價費用
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddl_TypeId_SelectedIndexChanged(object sender, EventArgs e)
        {
            string SiteNo = "";
            foreach (ListItem item in CheckBoxList_Detail.Items)
            {
                if (item.Selected)
                {
                    SiteNo = item.Value;
                    break;
                }
            }

            string TypeId = ddl_TypeId.SelectedValue;

            CB.DropDownListBind(ref DDL_bcseseq, _3PLBCS.PriceList_Query(Login_Server, SiteNo, TypeId), "I_bcse_seq", "S_bcse_CostName", "請選擇", "");
            if (DDL_bcseseq.Items.Count > 0)
            {
                DDL_bcseseq.Enabled = true;
            }
        }

        /// <summary>
        /// 選定計價費用，帶出明細
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void DDL_bcseseq_SelectedIndexChanged(object sender, EventArgs e)
        {
            string I_bcseSeq = DDL_bcseseq.SelectedValue;
            if (I_bcseSeq == "")
                return;
            DataTable dt = _3PLBCS.PriceList_Query(Login_Server, I_bcseSeq);
            DataRow dr = dt.Rows[0];
            Txb_Price.Text = string.Format("{0:N2}", dr["I_bcse_Price"]);
            Lbl_DollarUnit.Text = dr["S_bcse_DollarUnit"].ToString();
            Lbl_Unit2.Text = dr["UnitName"].ToString();
        }

        /// <summary>
        /// 最低收費
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddl_HaveMinimum_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (((DropDownList)sender).SelectedIndex)
            {
                case 0:
                    Txb_Memo.Text.Replace("每倉每次作業不足新台幣壹仟元以壹仟元計。", "");
                    Txb_Memo.Text.Replace("每次作業不足新台幣壹仟元以壹仟元計。", "");
                    break;
                case 1:
                    Txb_Memo.Text += "每倉每次作業不足新台幣壹仟元以壹仟元計。";
                    break;
                case 2:
                    Txb_Memo.Text += "每次作業不足新台幣壹仟元以壹仟元計。";
                    break;
            }
        }
        #endregion

        #region Detail-Mode
        //錯誤控制
        private bool Detail_ErrControl()
        {
            #region 錯誤控制
            string ErrMsg = "";
            if (ddl_TypeId.SelectedIndex == 0)
                ErrMsg += "報價主類別未設定！\\n";
            if (DDL_bcseseq.SelectedIndex == 0)
                ErrMsg += "計價費用未設定！\\n";
            if (!_3PL_Check.CkDecimal(Txb_Price.Text))
                ErrMsg += "單價輸入錯誤！\\n";

            int ChkSiteNo = 0;
            foreach (ListItem item in CheckBoxList_Detail.Items)
            {
                if (item.Selected)
                    ChkSiteNo += 1;
            }
            if (ChkSiteNo == 0)
                ErrMsg += "倉別未設定！\\n";
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

            //針對選擇的倉別，去帶入價格
            foreach (ListItem item in CheckBoxList_Detail.Items)
            {
                if (item.Selected)
                    Btn_Detail_New_Click_SiteNo(item.Value);
            }

            GVBind_Quotation_Detail();
        }
        private void Btn_Detail_New_Click_SiteNo(string SiteNo)
        {
            DataTable dt = (DataTable)Session["Quotation_Detail"];
            object MaxSeq_Pre = dt.Compute("max(I_qtde_Detailseq)", "");
            int Detail_MaxSeq = Convert.ToInt32(MaxSeq_Pre == DBNull.Value ? 0 : MaxSeq_Pre) + 1;
            DataRow dr = dt.NewRow();
            dr["I_qtde_seq"] = 0;
            dr["S_qtde_qthePLNO"] = Txb_S_qthe_PLNO.Text;
            dr["I_qtde_Detailseq"] = Detail_MaxSeq;
            dr["I_qtde_TypeId"] = ddl_TypeId.SelectedValue;
            dr["S_bsda_FieldName"] = ddl_TypeId.SelectedItem;

            #region 根據倉別不同取得個別的計價費用設定值
            string[] Costnamelist = DDL_bcseseq.SelectedItem.Text.ToString().Split(',');
            DataTable dt_Detail = _3PLBCS.PriceList_Query(Login_Server, SiteNo, ddl_TypeId.SelectedValue, Costnamelist[1]);
            if (dt_Detail.Rows.Count == 0)
                return;
            dr["I_qtde_bcseseq"] = dt_Detail.Rows[0]["I_bcse_seq"].ToString();
            dr["S_bcse_CostName"] = dt_Detail.Rows[0]["S_bcse_CostName"].ToString();
            dr["S_bcse_DollarUnit"] = dt_Detail.Rows[0]["S_bcse_DollarUnit"].ToString();

            decimal intTxb_Price_Check = 0; //檢查帶入直

            if (Chk_IsSpecial.Checked == true)  //其他議價單
            {
                try
                {
                    if (Txb_Price.Text != "")
                        intTxb_Price_Check = decimal.Parse(Txb_Price.Text);
                    else
                        intTxb_Price_Check = Convert.ToDecimal(dt_Detail.Rows[0]["I_bcse_Price"]);
                }
                catch
                {
                    intTxb_Price_Check = 0;
                }
            }
            else
            {
                try
                {
                    intTxb_Price_Check = Convert.ToDecimal(dt_Detail.Rows[0]["I_bcse_Price"]);
                }
                catch
                {
                    intTxb_Price_Check = 0;
                }
            }
            dr["I_qtde_Price"] = intTxb_Price_Check;
            #endregion

            dr["S_qtde_Memo"] = Txb_Memo.Text;
            //dr["I_qtde_IsBaseCost"] = Chk_IsBaseCost.Checked == true ? 1 : 0;
            //dr["S_qtde_PriceMemo"] = Txb_PriceMemo.Text;
            dr["I_qtde_DelFlag"] = 0;
            dr["S_qtde_CreateId"] = UI.UserID;
            dr["S_qtde_UpdId"] = UI.UserID;
            dr["S_qtde_SiteNo"] = SiteNo;
            dr["UIStatus"] = "Added";
            dt.Rows.Add(dr);
        }

        //更新明細
        protected void Btn_Detail_Upd_Click(object sender, EventArgs e)
        {
            if (Detail_ErrControl())
            {
                return;
            }

            DataRow dr = GV_Quotation_Detail_getSelectedRow();

            dr["I_qtde_TypeId"] = ddl_TypeId.SelectedValue;     //報價主類別
            dr["S_bsda_FieldName"] = ddl_TypeId.SelectedItem;

            dr["I_qtde_bcseseq"] = DDL_bcseseq.SelectedValue;   //計價費用
            string[] Costnamelist = DDL_bcseseq.SelectedItem.Text.ToString().Split(',');
            dr["S_bcse_CostName"] = Costnamelist[1];
            dr["S_bcse_DollarUnit"] = Lbl_DollarUnit.Text;
            dr["I_qtde_Price"] = Txb_Price.Text;                //單價
            dr["S_qtde_Memo"] = Txb_Memo.Text;                  //備註
            dr["S_qtde_UpdId"] = UI.UserID;

            dr["I_qtde_HaveMinimum"] = ddl_HaveMinimum.SelectedValue;   //最低收費

            //dr["I_qtde_IsBaseCost"] = Chk_IsBaseCost.Checked == true ? 1 : 0;
            //dr["S_qtde_PriceMemo"] = Txb_PriceMemo.Text;

            #region Save 明細倉別
            string SiteNo = "";
            foreach (ListItem item in CheckBoxList_Detail.Items)
            {
                if (item.Selected)
                    SiteNo = item.Value;
            }
            dr["S_qtde_SiteNo"] = SiteNo;
            #endregion

            if (dr["UIStatus"].ToString() != "Added")
                dr["UIStatus"] = "Modified";

            ((_3PLMasterPage)Master).ShowMessage("明細修改完成，最後記得提交更新");

            change_div_Detail_Upd(0);

            GVBind_Quotation_Detail();
        }
        //取消更新明細
        protected void Btn_Detail_Upd_Cancel_Click(object sender, EventArgs e)
        {
            change_div_Detail_Upd(0);
        }
        private void change_div_Detail_Upd(int Mode) {
            if (Mode == 0)
            {
                CheckBoxList_Detail.Enabled = true;
                Btn_Detail_New.Visible = true;
                Btn_Detail_Upd.Visible = false;
                Btn_Detail_Upd_Cancel.Visible = false;
            }
            else {
                CheckBoxList_Detail.Enabled = false;
                Btn_Detail_New.Visible = false;
                Btn_Detail_Upd.Visible = true;
                Btn_Detail_Upd_Cancel.Visible = true;
            }
        }

        //刪除明細
        protected void Btn_Detail_Del_Click(object sender, EventArgs e)
        {
            DataTable dt = (DataTable)Session["Quotation_Detail"];
            object[] objParam = { hidTotal_I_qtde_seq.Value, hidTotal_I_qtde_Detailseq.Value };
            DataRow dr = dt.Rows.Find(objParam);
            dr["S_qtde_UpdId"] = UI.UserID;
            dr["UIStatus"] = "Deleted";
            dr["I_qtde_DelFlag"] = true;

            GVBind_Quotation_Detail();
        }

        //選定要更新/刪除的Row
        protected void GV_Quotation_Detail_New_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            //刪除
            if (e.CommandName == "DeleteButton")
            {
                GridViewRow Row = ((GridViewRow)((WebControl)(e.CommandSource)).NamingContainer);
                DataTable dt = (DataTable)Session["Quotation_Detail"];
                string qtde_seq = ((HiddenField)Row.Cells[0].FindControl("hid_I_qtde_seq")).Value;
                String Detail_Seq = ((LinkButton)Row.Cells[0].FindControl("Txb_I_qtde_Detailseq")).Text;
                hidTotal_I_qtde_seq.Value = qtde_seq;
                hidTotal_I_qtde_Detailseq.Value = Detail_Seq;
                Btn_Detail_Del_Click(sender, e);
            }
            else if (e.CommandName == "UpdateButton")
            {
                //更新
                if (e.CommandArgument.ToString() == string.Empty)
                {
                    //定位明細
                    GridViewRow Row = ((GridViewRow)((WebControl)(e.CommandSource)).NamingContainer);
                    DataTable dt = (DataTable)Session["Quotation_Detail"];
                    string qtde_seq = ((HiddenField)Row.Cells[0].FindControl("hid_I_qtde_seq")).Value;
                    String Detail_Seq = ((LinkButton)Row.Cells[0].FindControl("Txb_I_qtde_Detailseq")).Text;
                    hidTotal_I_qtde_seq.Value = qtde_seq;
                    hidTotal_I_qtde_Detailseq.Value = Detail_Seq;

                    //決定明細Row
                    DataRow dr = GV_Quotation_Detail_getSelectedRow();

                    #region 帶出明細倉別
                    string SiteNo = dr["S_qtde_SiteNo"].ToString();
                    foreach (ListItem item in CheckBoxList_Detail.Items)
                    {
                        if (item.Value == SiteNo)
                            item.Selected = true;
                        else
                            item.Selected = false;
                    }
                    #endregion

                    //報價主類別
                    ddl_TypeId.SelectedValue = dr["I_qtde_TypeId"].ToString();
                    ddl_TypeId_SelectedIndexChanged(sender, e);

                    //計價費用
                    if (DDL_bcseseq.Items.FindByValue(dr["I_qtde_bcseseq"].ToString()) != null)
                        DDL_bcseseq.SelectedValue = dr["I_qtde_bcseseq"].ToString();
                    
                    Txb_Memo.Text = dr["S_qtde_Memo"].ToString();   //備註
                    Txb_Price.Text = dr["I_qtde_Price"].ToString(); //單價
                    string str_HaveMinimum = dr["I_qtde_HaveMinimum"].ToString();
                    if (str_HaveMinimum.Length <= 0)
                        str_HaveMinimum = "0";
                    ddl_HaveMinimum.SelectedValue = str_HaveMinimum;    //最低費用

                    //Chk_IsBaseCost.Checked = dr["I_qtde_IsBaseCost"].ToString() == "1" ? true : false;
                    //Txb_PriceMemo.Text = dr["S_qtde_PriceMemo"].ToString();

                    change_div_Detail_Upd(1);
                }
            }
        }
        private DataRow GV_Quotation_Detail_getSelectedRow()
        {
            string qtde_seq = hidTotal_I_qtde_seq.Value;
            string Detail_Seq = hidTotal_I_qtde_Detailseq.Value;

            DataTable dt = (DataTable)Session["Quotation_Detail"];
            object[] objParam = { qtde_seq, Detail_Seq };
            DataRow dr = dt.Rows.Find(objParam);

            return dr;
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

            #region 檢查明細內容有沒有重複
            //一個倉別+一個派工類別，只有一個計價費用
            var Result = from r1 in dt_Detail.AsEnumerable()
                         where r1.Field<string>("UIStatus") == "Added" || r1.Field<string>("UIStatus") == "Modified"
                         group r1 by new
                         {
                             TypeId = r1.Field<Byte>("I_qtde_TypeId"),
                             bcseSeq = r1.Field<int>("I_qtde_bcseSeq"),
                             SiteNo = r1.Field<string>("S_qtde_SiteNo"),
                             TypeName = r1.Field<string>("S_bsda_FieldName"),
                             CostName = r1.Field<string>("S_bcse_CostName")
                         } into g
                         where g.Count() > 1
                         select new
                         {
                             TypeId = g.Key.TypeId,
                             bcseSeq = g.Key.bcseSeq,
                             SiteNo = g.Key.SiteNo,
                             TypeName = g.Key.TypeName,
                             CostName = g.Key.CostName,
                             TCount = g.Count()
                         };

            foreach (var dr in Result)
            {
                ErrMsg += dr.TypeName + " " + dr.CostName + " " + "計價費用重複出現\\n";
            }
            #endregion

            string QuotationNo = "";

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
                        QuotationNo = _3PLCQ.GetMaxPageNo(Login_Server, 1);

                        //新增
                        IsSuccess = _3PLQu.InsertQuotation(Login_Server, dt_head, dt_Detail, QuotationNo, UI.UserID);
                        if (IsSuccess)
                            ErrMsg += "新增成功，單號：" + QuotationNo + "！\\n";
                        else
                        {
                            ErrMsg += "新增失敗！\\n";
                        }
                    }
                    else
                    {
                        //取得原有單號
                        QuotationNo = dt_head.Rows[0]["S_qthe_PLNO"].ToString();

                        //修改
                        IsSuccess = _3PLQu.InsertQuotation(Login_Server, dt_head, dt_Detail, QuotationNo, UI.UserID);
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
                    ((_3PLMasterPage)Master).ShowMessage(ErrMsg, CreatePath(QuotationNo));
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
            string VarString = _3PLCQ.Page_QuotationQuery(
                                                Txb_S_qthe_SupdId_New.Text
                                                , ""
                                                , Txb_D_qthe_ContractS.Text
                                                , Txb_D_qthe_ContractE.Text
                                                , QuotationNo);

            string Path = "";
            if (Request.QueryString["VarString"] != null && Request.QueryString["VarString"] != "")
            {
                Path = string.Format("3PL_Quotation_Query.aspx?VarString={0}", Request.QueryString["VarString"]);
            }
            else
            {
                Path = string.Format("3PL_Quotation_Query.aspx?VarString={0}", VarString);
            }
            return Path;
        }
        #endregion

        #region 派工單作廢

        //開啟派工單作廢
        protected void Btn_QuotationHead_Delete_Click(object sender, EventArgs e)
        {
            DataTable dt_head = (DataTable)Session["Quotation_head"];
            string PLNO = dt_head.Rows[0]["S_qthe_PLNO"].ToString(),
                Status = dt_head.Rows[0]["I_qthe_Status"].ToString(),
                PageType = "1";
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
                //ScriptManager.RegisterClientScriptBlock(this, typeof(string), "alert", "alert('未輸入作廢原因')", true);
                ScriptManager.RegisterClientScriptBlock(Page, typeof(string), "c", "show_table_SignOff_Back();", true);
                return;
            }

            //簽核取消
            int IsSuccess = _3PLSignOff.SignOff_Quotation(Login_Server, UI.UserID, UI.UserName, false, txb_Status.Text, txb_ObjNo.Text, txb_PageType.Text);

            //寫入原因
            _3PLSignOff.SignOffBackReason(Login_Server, UI.UserID, txb_Status.Text, txb_ObjNo.Text, txb_ObjName.Text, txb_PageType.Text);
            if (IsSuccess > 0)
            {
                ((_3PLMasterPage)Master).ShowMessage("報價單作廢成功", CreatePath(txb_ObjNo.Text));
                //ScriptManager.RegisterClientScriptBlock(this, typeof(string), "alert", "location.href='" + CreatePath(txb_ObjNo.Text) + "';", true);
            }
            else
            {
                ((_3PLMasterPage)Master).ShowMessage("報價單作廢失敗");
                //ScriptManager.RegisterClientScriptBlock(this, typeof(string), "alert", "alert('報價單作廢失敗')", true);
            }
        }
        #endregion

    }
}
