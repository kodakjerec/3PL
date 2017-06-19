using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using _3PL_DAO;
using _3PL_LIB;
using _3PL_System;

namespace SC_Offer
{
    public partial class _3PL_BaseCostSetSelect : System.Web.UI.Page
    {
        _3PL_CommonQuery _3PLCQ = new _3PL_CommonQuery();
        _3PL_BaseCostSet_DAO _3PLBCS = new _3PL_BaseCostSet_DAO();
        ControlBind CB = new ControlBind();
        string Login_Server = "3PL";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //產生作業大類下拉式選單
                CB.DropDownListBind(ref ddl_SiteNo, _3PLCQ.GetFieldList(Login_Server, "SiteNo"), "S_bsda_FieldId", "S_bsda_FieldName", "請選擇", "");
                CB.DropDownListBind(ref ddl_TypeId, _3PLCQ.GetFieldList(Login_Server, "TypeId"), "S_bsda_FieldId", "S_bsda_FieldName", "請選擇", "");

                if (Request.QueryString["SBCString"] != null)
                {
                    string[] SBCString = Request.QueryString["SBCString"].Split(',');
                    ddl_SiteNo.SelectedValue = SBCString[0];
                    ddl_TypeId.SelectedValue = SBCString[1];
                }
            }
        }

        //按下查詢
        protected void btn_Query_Click(object sender, EventArgs e)
        {
            DataTable dtSup = new DataTable();
            string SiteNo = string.Empty;
            string TypeId = "";
            try
            {
                SiteNo = ddl_SiteNo.SelectedValue;
                TypeId = ddl_TypeId.SelectedValue;
                dtSup = _3PLBCS.PriceList_Query("3PL", SiteNo, TypeId);
                Session["Sup"] = dtSup;
                BindGV();
            }
            catch
            {
                ((_3PLMasterPage)Master).ShowMessage("系統異常請洽資訊部");
            }
        }

        #region GridView
        public void BindGV()
        {
            DataTable Dt = new DataTable();
            Dt = (DataTable)Session["Sup"];
            gv_List.DataSource = Dt;
            gv_List.DataBind();
        }

        protected void gv_List_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gv_List.PageIndex = e.NewPageIndex;
                BindGV();
            }
            catch
            {
                ((_3PLMasterPage)Master).ShowMessage("系統異常請洽資訊人員");
            }
        }

        //選定費用
        protected void gv_List_RowEditing(object sender, GridViewEditEventArgs e)
        {
            string _txbSupNo = string.Empty;
            try
            {
                int index = e.NewEditIndex;
                string hid_Bcse_seq = gv_List.Rows[index].Cells[4].Text;
                string ddl_SiteNo = gv_List.Rows[index].Cells[5].Text;
                string ddl_TypeId = gv_List.Rows[index].Cells[6].Text;
                string Txb_CostName = gv_List.Rows[index].Cells[1].Text;

                //hid_Bcse_seq
                _txbSupNo = "opener.document." + Request.QueryString["FormLocation"].ToString() + "." + Request.QueryString["RL4"].ToString() + ".value='" + hid_Bcse_seq + "';";
                ScriptManager.RegisterClientScriptBlock(Page, typeof(string), "a", _txbSupNo, true);

                //ddl_SiteNo
                _txbSupNo = "opener.document." + Request.QueryString["FormLocation"].ToString() + "." + Request.QueryString["RL"].ToString() + ".value='" + ddl_SiteNo + "';";
                ScriptManager.RegisterClientScriptBlock(Page, typeof(string), "b", _txbSupNo, true);

                //ddl_TypeId
                _txbSupNo = "opener.document." + Request.QueryString["FormLocation"].ToString() + "." + Request.QueryString["RL2"].ToString() + ".value='" + ddl_TypeId + "';";
                ScriptManager.RegisterClientScriptBlock(Page, typeof(string), "c", _txbSupNo, true);

                //DDL_CostName
                _txbSupNo = "opener.document." + Request.QueryString["FormLocation"].ToString() + "." + Request.QueryString["RL3"].ToString() + ".value='" + Txb_CostName + "';";
                ScriptManager.RegisterClientScriptBlock(Page, typeof(string), "d", _txbSupNo, true);

                ScriptManager.RegisterClientScriptBlock(Page, typeof(string), "e", "window.close();", true);
            }
            catch
            { }
        }
        #endregion


    }
}
