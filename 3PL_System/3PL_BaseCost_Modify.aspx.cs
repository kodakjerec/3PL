using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using _3PL_LIB;
using _3PL_DAO;
using System.Data;

namespace _3PL_System
{
    public partial class _3PL_BaseCost_Modify : System.Web.UI.Page
    {
        _3PL_BaseCost_DAO GetBC = new _3PL_BaseCost_DAO();
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
                CB.DropDownListBind(ref ddl_SiteNo, GetCQ.GetFieldList(Login_Server, "SiteNo"), "S_bsda_FieldId", "S_bsda_FieldName", "請選擇", "");
                CB.DropDownListBind(ref ddl_TypeId, GetCQ.GetFieldList(Login_Server, "TypeId"), "S_bsda_FieldId", "S_bsda_FieldName", "請選擇", "");
                CB.DropDownListBind(ref ddl_UnitId, GetCQ.GetFieldList(Login_Server, "UnitId"), "S_bsda_FieldId", "S_bsda_FieldName", "請選擇", "");
                CB.DropDownListBind(ref ddl_CostType, GetCQ.GetFieldList(Login_Server, "CostType"), "S_bsda_FieldId", "S_bsda_FieldName", "請選擇", "");
                
                hid_bascSeq.Value = Request.QueryString["bascSeq"] == null ? "" : Request.QueryString["bascSeq"].ToString();

                //判別新增/修改
                if (hid_bascSeq.Value == "")
                {
                    //新增
                    lbl_PriceSet_Query.Text = "成本費用新增";
                    Txb_DollarUnit.Text = "元";
                    Btn_Del.Visible = false;
                    //代入變數
                    if (Request.QueryString["VarString"] != null)
                    {
                        string[] VarList = Request.QueryString["VarString"].Split(',');
                        ddl_SiteNo.SelectedValue = VarList[0];
                        ddl_TypeId.SelectedValue = VarList[1];
                        ddl_TypeId_SelectedIndexChanged(sender, e);
                        DDL_CostName.SelectedValue = VarList[2];
                    }
                }
                else
                {
                    //修改
                    lbl_PriceSet_Query.Text = "成本費用異動";
                    Btn_SelectBaseCostSet.Visible = false;
                    GetData();
                }
            }
        }

        #region Form_Control
        /// <summary>
        /// 帶入預設資料
        /// </summary>
        private void GetData()
        {
            string I_bcse_seq = hid_bascSeq.Value;
            DataTable dt = GetBC.BaseCost_Query(Login_Server, I_bcse_seq);
            if (dt.Rows.Count > 0)
            {
                //計價費用資料
                ddl_SiteNo.SelectedValue = dt.Rows[0]["S_bcse_SiteNo"].ToString();
                ddl_TypeId.SelectedValue = dt.Rows[0]["I_bcse_TypeId"].ToString();
                EventArgs e = new EventArgs();
                object sender = new object();
                ddl_TypeId_SelectedIndexChanged(sender, e);
                DDL_CostName.SelectedValue = dt.Rows[0]["I_basc_bcseseq"].ToString();
                ddl_SiteNo.Enabled = false;
                ddl_TypeId.Enabled = false;
                DDL_CostName.Enabled = false;

                //明細資料
                ddl_CostType.SelectedValue = dt.Rows[0]["S_basc_CostType"].ToString();
                ddl_UnitId.SelectedValue = dt.Rows[0]["I_basc_UnitId"].ToString();

                Txb_bascCostName.Text = dt.Rows[0]["S_basc_CostName"].ToString();
                Txb_Price.Text = dt.Rows[0]["I_basc_Free"].ToString();
                Txb_DollarUnit.Text = dt.Rows[0]["S_basc_DollarUnit"].ToString();
            }
        }

        #region 新刪修
        //取消
        protected void Btn_Cancel_Click(object sender, EventArgs e)
        {
            ReturnTo3PL_BaseCost_Modify();
        }

        //刪除
        protected void Btn_Del_Click(object sender, EventArgs e)
        {
            string I_bcse_seq = hid_bascSeq.Value;
            int DeleteCount = GetBC.BaseCost_Delete(Login_Server, UI.UserID, I_bcse_seq);
            if (DeleteCount <= 0)
            {
                ((_3PLMasterPage)Master).ShowMessage("刪除失敗");
            }
            else
            {
                ((_3PLMasterPage)Master).ShowMessage("刪除成功");
                ReturnTo3PL_BaseCost_Modify();
            }
        }

        //新增/更新
        protected void Btn_Upd_Click(object sender, EventArgs e)
        {
            #region 錯誤控制
            string ErrMsg = "";
            if (ddl_SiteNo.SelectedIndex == 0)
                ErrMsg += "寄倉倉別未設定! ";
            if (ddl_TypeId.SelectedIndex == 0)
                ErrMsg += "報價主類別未設定! ";
            if (ddl_UnitId.SelectedIndex == 0)
                ErrMsg += "計價單位未設定! ";
            if (DDL_CostName.SelectedIndex == 0)
                ErrMsg += "費用名稱未設定! ";
            if (Txb_Price.Text.Trim() == "")
                ErrMsg += "單價未設定! ";
            if (Txb_DollarUnit.Text.Trim() == "")
                ErrMsg += "貨幣單位未設定! ";
            if (ErrMsg != "")
            {
                ((_3PLMasterPage)Master).ShowMessage(ErrMsg);
                return;
            }
            #endregion

            string I_bcse_seq = hid_bascSeq.Value;
            DataTable dt = GetBC.BaseCost_Default(Login_Server);
            if (I_bcse_seq == "")
            {
                //新增
                DataRow dr = dt.NewRow();
                dr["I_basc_bcseseq"] = DDL_CostName.SelectedValue;
                dr["I_basc_Detailseq"] = 1;
                dr["S_basc_CostType"] = ddl_CostType.SelectedValue;
                dr["S_basc_CostName"] = Txb_bascCostName.Text;
                dr["I_basc_Free"] = Txb_Price.Text;
                dr["S_basc_DollarUnit"] = Txb_DollarUnit.Text;
                dr["I_basc_UnitId"] = ddl_UnitId.SelectedValue;
             
                int DeleteCount = GetBC.BaseCost_New(Login_Server, UI.UserID, dr);
                if (DeleteCount <= 0)
                {
                    ((_3PLMasterPage)Master).ShowMessage("新增失敗");
                }
                else
                {
                    ((_3PLMasterPage)Master).ShowMessage("新增成功");
                    ReturnTo3PL_BaseCost_Modify();
                }
            }
            else
            {
                DataRow dr = dt.Rows[0];
                //修改
                dr["I_basc_seq"] = hid_bascSeq.Value;
                dr["I_basc_Detailseq"] = 1;
                dr["S_basc_CostType"] = ddl_CostType.SelectedValue;
                dr["S_basc_CostName"] = Txb_bascCostName.Text;
                dr["I_basc_Free"] = Txb_Price.Text;
                dr["S_basc_DollarUnit"] = Txb_DollarUnit.Text;
                dr["I_basc_UnitId"] = ddl_UnitId.SelectedValue;
                int DeleteCount = GetBC.BaseCost_Update(Login_Server, UI.UserID, dr);
                if (DeleteCount <= 0)
                {
                    ((_3PLMasterPage)Master).ShowMessage("更新失敗");
                 }
                else
                {
                    ((_3PLMasterPage)Master).ShowMessage("更新成功");
                    ReturnTo3PL_BaseCost_Modify();
                }
            }
        }


        private void ReturnTo3PL_BaseCost_Modify()
        {
            string Path = "";
            if (Request.QueryString["VarString"] != "")
            {
                Path = string.Format("3PL_BaseCost.aspx?VarString={0}", Request.QueryString["VarString"]);
            }
            else
            {
                Path = string.Format("3PL_BaseCost.aspx");
            }
            Response.Redirect(Path);
        }
        #endregion

        #region 選擇計價費用小視窗
        //選擇計價費用小視窗
        protected void Btn_SelectBaseCostSet_Click(object sender, EventArgs e)
        {
            string FormLocation= "FormLocation="+this.Form.ID
                   + "&RL=" + ddl_SiteNo.ID
                   +"&RL2="+ddl_TypeId.ID
                   +"&RL3=" + DDL_CostName.ID
                   +"&RL4="+hid_Bcse_seq.ID
                   + "&SBCString=" + GetSelectBaseCost();
            string Path = "3PL_BaseCostSetSelect.aspx?" + FormLocation;
            Path = "window.open('" + Path + "','作業對象','menubar=no,location=no,height=600,width=800')";
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", Path, true);
        }
        //取得選擇的變數
        private string GetSelectBaseCost()
        {
            string SBCString = "";
            SBCString += ddl_SiteNo.SelectedValue + ",";
            SBCString += ddl_TypeId.SelectedValue;
            return SBCString;
        }
        #endregion

        #endregion

        //選擇倉別 
        protected void ddl_SiteNo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddl_TypeId.SelectedIndex > 0)
            {
                ddl_TypeId_SelectedIndexChanged(sender, e);
            }
        }

        //選擇好報價主類別後，帶出費用名稱
        protected void ddl_TypeId_SelectedIndexChanged(object sender, EventArgs e)
        {
            ddl_TypeId_Change();
        }

        public void ddl_TypeId_Change()
        {
            if (ddl_TypeId.SelectedIndex == 0)
                return;
            string SiteNo = ddl_SiteNo.SelectedValue;
            string TypeId = ddl_TypeId.SelectedValue;
            CB.DropDownListBind(ref DDL_CostName, GetBCS.PriceList_Query(Login_Server, SiteNo, TypeId), "I_bcse_Seq", "S_bcse_CostName", "請選擇", "");
            DDL_CostName.Enabled = true;
        }



    }
}
