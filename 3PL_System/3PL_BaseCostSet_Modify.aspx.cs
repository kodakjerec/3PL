using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using _3PL_LIB;
using _3PL_DAO;
using System.Data;

namespace _3PL_System
{
    public partial class _3PL_BaseCostSet_Modify : System.Web.UI.Page
    {
        _3PL_BaseCostSet_DAO GetBCS = new _3PL_BaseCostSet_DAO();
        _3PL_CommonQuery GetCQ = new _3PL_CommonQuery();
        ControlBind CB = new ControlBind();
        private UserInf UI = new UserInf();
        string Login_Server = "3PL";

        protected void Page_Load(object sender, EventArgs e)
        {
            ((_3PLMasterPage)Master).SessionCheck(ref UI);

            if (!IsPostBack)
            {
                //產生作業大類下拉式選單
                CB.DropDownListBind(ref ddl_TypeId, GetCQ.GetFieldList(Login_Server, "TypeId"), "S_bsda_FieldId", "S_bsda_FieldName", "請選擇", "");
                CB.DropDownListBind(ref ddl_UnitId, GetCQ.GetFieldList(Login_Server, "UnitId"), "S_bsda_FieldId", "S_bsda_FieldName", "請選擇", "");
                CB.DropDownListBind(ref ddl_AccId, GetCQ.GetAccId(Login_Server), "I_Acci_seq", "S_Acci_Name", "請選擇", "");
                CB.DropDownListBind(ref DDL_ClassList, GetCQ.GetClassIdList(Login_Server), "ClassId", "ClassName", "請選擇", "");
                CB.CheckBoxListBind(ref CheckBoxList1, GetCQ.GetFieldList(Login_Server, "SiteNo"), "S_bsda_FieldId", "S_bsda_FieldName");

                hid_bcseSeq.Value = Request.QueryString["bcseSeq"] == null ? "" : Request.QueryString["bcseSeq"].ToString();

                //判別新增/修改
                if (hid_bcseSeq.Value == "")
                {
                    //新增
                    lbl_PriceSet_Query.Text = "計價費用新增";
                    Btn_Del.Visible = false;
                    DDL_ClassList.Enabled = false;
                    Lbl_DDL_ClassList.Visible = true;
                }
                else
                {
                    //修改
                    lbl_PriceSet_Query.Text = "計價費用異動";
                    GetData();

                    //代入處理單位一覽表
                    GV_ClassList_Search();
                }
            }
        }

        #region Form_Control
        /// <summary>
        /// 帶入預設資料
        /// </summary>
        private void GetData()
        {
            string I_bcse_seq = hid_bcseSeq.Value;
            DataTable dt = GetBCS.PriceList_Query(Login_Server, I_bcse_seq);
            if (dt.Rows.Count > 0)
            {
                string SiteNo = dt.Rows[0]["S_bcse_SiteNo"].ToString();
                foreach (ListItem item in CheckBoxList1.Items)
                {
                    item.Enabled = false;
                    if (item.Value == SiteNo)
                        item.Selected = true;
                }

                ddl_TypeId.SelectedValue = dt.Rows[0]["I_bcse_TypeId"].ToString();
                ddl_TypeId.Enabled = false;
                ddl_UnitId.SelectedValue = dt.Rows[0]["I_bcse_UnitId"].ToString();
                ddl_AccId.SelectedValue = dt.Rows[0]["I_bcse_AccId"].ToString();

                Txb_CostName.Text = dt.Rows[0]["S_bcse_CostName"].ToString();
                Txb_Price.Text = dt.Rows[0]["I_bcse_Price"].ToString();
                if (dt.Rows[0]["I_bcse_IsDBLink"].ToString() == "1")
                    Chk_IsDBLink.Checked = true;
                if (dt.Rows[0]["I_bcse_IsDiscount"].ToString() == "1")
                    Chk_IsDiscount.Checked = true;
                if (dt.Rows[0]["I_bcse_IsFormula"].ToString() == "1")
                    Chk_IsFormula.Checked = true;
                if (dt.Rows[0]["I_bcse_IsPeriod"].ToString() == "1")
                    Chk_IsPeriod.Checked = true;
            }
        }

        #region 新刪修
        //取消
        protected void Btn_Cancel_Click(object sender, EventArgs e)
        {
            ReturnTo3PL_BaseCostSet_Modify();
        }

        //刪除
        protected void Btn_Del_Click(object sender, EventArgs e)
        {
            string I_bcse_seq = hid_bcseSeq.Value;
            int DeleteCount = GetBCS.PriceList_Delete(Login_Server, UI.UserID, I_bcse_seq);
            if (DeleteCount <= 0)
            {
                ((_3PLMasterPage)Master).ShowMessage("刪除失敗");
            }
            else
            {
                ((_3PLMasterPage)Master).ShowMessage("刪除成功");
                ReturnTo3PL_BaseCostSet_Modify();
            }
        }

        //新增/更新
        protected void Btn_Upd_Click(object sender, EventArgs e)
        {
            #region 錯誤控制
            string ErrMsg = "";
            int ChkSiteNo = 0;
            foreach (ListItem item in CheckBoxList1.Items)
            {
                if (item.Selected)
                    ChkSiteNo += 1;
            }
            if (ChkSiteNo == 0)
                ErrMsg += "寄倉倉別未設定! ";
            if (ddl_TypeId.SelectedIndex == 0)
                ErrMsg += "報價主類別未設定! ";
            if (ddl_UnitId.SelectedIndex == 0)
                ErrMsg += "計價單位未設定! ";
            if (ddl_AccId.SelectedIndex == 0)
                ErrMsg += "會計費用未設定! ";
            if (Txb_CostName.Text.Trim() == "")
                ErrMsg += "費用名稱未設定! ";
            if (Txb_Price.Text.Trim() == "")
                ErrMsg += "單價未設定! ";
            if (ErrMsg != "")
            {
                ((_3PLMasterPage)Master).ShowMessage(ErrMsg);
                return;
            }
            #endregion

            int SuccessCount = 0;
            string I_bcse_seq = hid_bcseSeq.Value;
            DataTable dt = GetBCS.PriceList_Query(Login_Server, I_bcse_seq);
            if (I_bcse_seq == "")
            {
                //新增

                #region 循環倉別新增資料

                string SiteNo = "";

                #region 檢查費用名稱
                foreach (ListItem item in CheckBoxList1.Items)
                {
                    if (item.Selected)
                    {
                        SiteNo = item.Value;

                        //計價費用名稱是否重複
                        DataTable dt_chk = GetBCS.PriceList_Query(Login_Server, SiteNo, ddl_TypeId.SelectedValue, Txb_CostName.Text);
                        if (dt_chk.Rows.Count > 0)
                        {
                            ErrMsg = SiteNo + " 費用名稱重複！";
                            ((_3PLMasterPage)Master).ShowMessage(ErrMsg);
                            return;
                        }
                    }
                }
                #endregion

                #region 新增資料
                foreach (ListItem item in CheckBoxList1.Items)
                {
                    if (item.Selected)
                    {
                        SiteNo = item.Value;
                        DataRow dr = dt.NewRow();
                        SuccessCount += AddNewRow(dr, SiteNo);
                    }
                }
                #endregion

                #endregion

                if (SuccessCount <= 0)
                {
                    ((_3PLMasterPage)Master).ShowMessage("新增失敗");
                }
                else
                {
                    ((_3PLMasterPage)Master).ShowMessage("新增成功");
                     ReturnTo3PL_BaseCostSet_Modify();
                }
            }
            else
            {
                string SiteNo = "";
                foreach (ListItem item in CheckBoxList1.Items)
                {
                    if (item.Selected)
                        SiteNo = item.Value;
                }
                //更新處理單位
                DataTable dt_MyClassList = (DataTable)Session["MyClassList"];
                SuccessCount += GetBCS.MyClassList_Update(Login_Server, dt_MyClassList);

                //計價費用名稱是否重複
                DataTable dt_chk = GetBCS.PriceList_Query(Login_Server, SiteNo, ddl_TypeId.SelectedValue, Txb_CostName.Text);
                if (dt_chk.Rows.Count > 0)
                {
                    if (dt.Rows[0]["I_bcse_seq"].ToString() != dt_chk.Rows[0]["I_bcse_seq"].ToString())
                    {
                        ErrMsg = "費用名稱重複！";
                        ((_3PLMasterPage)Master).ShowMessage(ErrMsg);
                       return;
                    }
                }

                DataRow dr = dt.Rows[0];
                //修改
                dr["S_bcse_CostName"] = Txb_CostName.Text;
                dr["I_bcse_TypeId"] = ddl_TypeId.SelectedValue;
                dr["I_bcse_Price"] = Txb_Price.Text;
                dr["S_bcse_DollarUnit"] = "元";
                dr["I_bcse_UnitId"] = ddl_UnitId.SelectedValue;
                dr["I_bcse_AccId"] = ddl_AccId.SelectedValue;
                dr["I_bcse_IsDbLink"] = Chk_IsDBLink.Checked == true ? 1 : 0;
                dr["I_bcse_IsDiscount"] = Chk_IsDiscount.Checked == true ? 1 : 0;
                dr["I_bcse_IsFormula"] = Chk_IsFormula.Checked == true ? 1 : 0;
                dr["I_bcse_IsPeriod"] = Chk_IsPeriod.Checked == true ? 1 : 0;
                SuccessCount += GetBCS.PriceList_Update(Login_Server, UI.UserID, dr);
                if (SuccessCount <= 0)
                {
                    ((_3PLMasterPage)Master).ShowMessage("更新失敗");
               }
                else
                {
                    ((_3PLMasterPage)Master).ShowMessage("更新成功");
                    ReturnTo3PL_BaseCostSet_Modify();
                }
            }
        }
        //新增計價費用資料
        private int AddNewRow(DataRow dr, string SiteNo)
        {
            int SuccessCount = 0;
            dr["S_bcse_SiteNo"] = SiteNo;
            dr["S_bcse_CostName"] = Txb_CostName.Text;
            dr["I_bcse_TypeId"] = ddl_TypeId.SelectedValue;
            dr["I_bcse_Price"] = Txb_Price.Text;
            dr["S_bcse_DollarUnit"] = "元";
            dr["I_bcse_UnitId"] = ddl_UnitId.SelectedValue;
            dr["I_bcse_AccId"] = ddl_AccId.SelectedValue;
            dr["I_bcse_IsDbLink"] = Chk_IsDBLink.Checked == true ? 1 : 0;
            dr["I_bcse_IsDiscount"] = Chk_IsDiscount.Checked == true ? 1 : 0;
            dr["I_bcse_IsFormula"] = Chk_IsFormula.Checked == true ? 1 : 0;
            dr["I_bcse_IsPeriod"] = Chk_IsPeriod.Checked == true ? 1 : 0;
            SuccessCount += GetBCS.PriceList_New(Login_Server, UI.UserID, dr);
            return SuccessCount;
        }
        //回到查詢葉面
        private void ReturnTo3PL_BaseCostSet_Modify()
        {
            string Path = "";
            if (Request.QueryString["VarString"] != null)
            {
                Path = string.Format("3PL_BaseCostSet.aspx?VarString={0}", Request.QueryString["VarString"]);
            }
            else
            {
                Path = string.Format("3PL_BaseCostSet.aspx");
            }
            Response.Redirect(Path);
        }
        #endregion

        #endregion

        #region 處理單位操作
        //代入處理單位一覽表
        private void GV_ClassList_Search()
        {
            DataTable dt_MyClassList = GetBCS.GetMyClassList(Login_Server, hid_bcseSeq.Value);
            Session["MyClassList"] = dt_MyClassList;
            GV_ClassList_Bind();
        }

        //重新綁定
        private void GV_ClassList_Bind()
        {
            DataTable dt = (DataTable)Session["MyClassList"];
            DataView dv = new DataView(dt);
            dv.RowFilter = "[UIStatus] IN ('Unchanged','Added')";
            GV_ClassList.DataSource = dv;
            GV_ClassList.DataBind();
            UpdatePanel1.Update();
        }

        //新增處理單位
        protected void DDL_ClassList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (DDL_ClassList.SelectedIndex > 0)
            {
                DataTable dt_MyClassList = (DataTable)Session["MyClassList"];
                //查詢是否已經有相同資料
                object[] objParam = { hid_bcseSeq.Value, DDL_ClassList.SelectedValue };
                DataRow dr = dt_MyClassList.Rows.Find(objParam);
                if (dr == null)
                {
                    //新增資料
                    dr = dt_MyClassList.NewRow();
                    dr["Sn"] = 0;
                    dr["I_bcse_seq"] = hid_bcseSeq.Value;
                    dr["ClassId"] = DDL_ClassList.SelectedValue;
                    dr["ClassName"] = DDL_ClassList.SelectedItem;
                    dr["UIStatus"] = "Added";
                    dt_MyClassList.Rows.Add(dr);
                    GV_ClassList_Bind();
                }
            }
        }

        //刪除處理單位
        protected void GV_ClassList_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "DeleteButton")
            {
                GridViewRow Row = ((GridViewRow)((WebControl)(e.CommandSource)).NamingContainer);
                DataTable dt = (DataTable)Session["MyClassList"];
                string I_bcse_seq = hid_bcseSeq.Value;
                String ClassId = ((HiddenField)Row.Cells[0].FindControl("hid_ClassId")).Value;
                object[] objParam = { I_bcse_seq, ClassId };
                DataRow dr = dt.Rows.Find(objParam);
                dr["ClassId"] = "";
                dr["UIStatus"] = "Deleted";
                GV_ClassList_Bind();
            }
        }
        #endregion

    }
}
