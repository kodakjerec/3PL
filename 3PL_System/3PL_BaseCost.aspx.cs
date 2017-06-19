using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using _3PL_LIB;
using _3PL_DAO;
using System.Data;

namespace _3PL_System
{
    public partial class _3PL_BaseCost : System.Web.UI.Page
    {
        _3PL_BaseCost_DAO GetBC = new _3PL_BaseCost_DAO();
        _3PL_BaseCostSet_DAO GetBCS = new _3PL_BaseCostSet_DAO();
        _3PL_CommonQuery GetCQ = new _3PL_CommonQuery();
        ControlBind CB = new ControlBind();
        private UserInf UI = new UserInf();
        private string Login_Server = "3PL";

        protected void Page_Load(object sender, EventArgs e)
        {
            ((_3PLMasterPage)Master).SessionCheck(ref UI);

            if (!IsPostBack)
            {
                //產生作業大類下拉式選單
                CB.DropDownListBind(ref ddl_SiteNo, GetCQ.GetFieldList(Login_Server, "SiteNo"), "S_bsda_FieldId", "S_bsda_FieldName", "請選擇", "");
                CB.DropDownListBind(ref ddl_TypeId, GetCQ.GetFieldList(Login_Server, "TypeId"), "S_bsda_FieldId", "S_bsda_FieldName", "請選擇", "");
                ddl_SiteNo.SelectedIndex = 1;
                ddl_TypeId.SelectedIndex = 1;
                ddl_TypeId_SelectedIndexChanged(sender, e);

                if (Request.QueryString["VarString"] != null)
                    PushPageVar(Request.QueryString["VarString"]);
            }
        }

        #region GV_PriceList_Query_相關功能
        //綁定
        private void GVBind()
        {
            GV_PriceList_Query.DataSource = Session["PriceList"];
            GV_PriceList_Query.DataBind();
        }

        protected void GV_PriceList_Query_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridView GV = sender as GridView;

            DataTable dt = new DataTable();
            dt = (DataTable)Session["PriceList"];
            GV.PageIndex = e.NewPageIndex;
            GVBind();
        }
        //跳至更新頁面
        protected void GV_PriceList_Query_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandArgument.ToString() == string.Empty)
                {
                    GridViewRow Row = ((GridViewRow)((WebControl)(e.CommandSource)).NamingContainer);
                    string Path = string.Empty;
                    string I_basc_seq = ((HiddenField)Row.Cells[0].FindControl("hid_bascSeq")).Value;
                    Path = string.Format("3PL_BaseCost_Modify.aspx?bascSeq={0}&VarString={1}", I_basc_seq, GetPageVar());
                    Response.Redirect(Path);
                }
            }
            catch (Exception ex)
            {
                ((_3PLMasterPage)Master).ShowMessage(ex.Message);
            }
        }
        #endregion

        #region 進入頁面相關函數
        //收集本頁變數
        private string GetPageVar()
        {
            string VarString = "";
            VarString += ddl_SiteNo.SelectedValue + ",";
            VarString += ddl_TypeId.SelectedValue + ",";
            VarString += DDL_CostName.SelectedValue;

            return VarString;
        }

        //獲得本頁變數
        private void PushPageVar(string VarString)
        {
            object sender=new object();
            EventArgs e = new EventArgs();
            string[] VarList = VarString.Split(',');
            ddl_SiteNo.SelectedValue = VarList[0];
            ddl_SiteNo_SelectedIndexChanged(sender, e);
            ddl_TypeId.SelectedValue = VarList[1];
            ddl_TypeId_SelectedIndexChanged(sender, e);
            DDL_CostName.SelectedValue = VarList[2];
            Btn_Query_Act();
        }
        #endregion

        #region 按鈕動作
        //查詢
        protected void Btn_Query_Click(object sender, EventArgs e)
        {
            #region 錯誤處理
            //倉別一定要輸入
            if (ddl_SiteNo.SelectedIndex == 0)
            {
                ((_3PLMasterPage)Master).ShowMessage("倉別一定要選擇");
                return;
            }
            if (ddl_TypeId.SelectedIndex == 0)
            {
                ((_3PLMasterPage)Master).ShowMessage("報價主類別一定要選擇");
                return;
            }
            #endregion

            Btn_Query_Act();
        }
        private void Btn_Query_Act()
        {
            string TypeId = ddl_TypeId.SelectedValue;
            string SiteNo = ddl_SiteNo.SelectedValue;
            string CostName = DDL_CostName.SelectedValue;
            if (CostName != "")
            {
                Session["PriceList"] = GetBC.BaseCost_Query(Login_Server, SiteNo, TypeId, CostName);
            }
            else
            {
                Session["PriceList"] = GetBC.BaseCost_Query(Login_Server, SiteNo, TypeId);
            }
            GVBind();
        }

        //按下新增
        protected void Btn_New_Click(object sender, EventArgs e)
        {
            string Path = string.Format("3PL_BaseCost_Modify.aspx?VarString={0}", GetPageVar());
            Response.Redirect(Path);
        }

        //選擇好報價主類別後，帶出費用名稱
        protected void ddl_TypeId_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddl_TypeId.SelectedIndex == 0)
                return;
            string SiteNo = ddl_SiteNo.SelectedValue;
            string TypeId = ddl_TypeId.SelectedValue;
            CB.DropDownListBind(ref DDL_CostName, GetBCS.PriceList_Query(Login_Server, SiteNo, TypeId), "I_bcse_Seq", "S_bcse_CostName", "請選擇", "");
            DDL_CostName.Enabled = true;
        }

        //選擇倉別
        protected void ddl_SiteNo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddl_TypeId.SelectedIndex > 0)
            {
                ddl_TypeId_SelectedIndexChanged(sender, e);
            }
        }
        #endregion


    }
}
