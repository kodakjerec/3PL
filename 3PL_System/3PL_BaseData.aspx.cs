using System;
using System.Web.UI.WebControls;
using _3PL_LIB;
using _3PL_DAO;
using System.Data;

namespace _3PL_System
{
    public partial class _3PL_BaseData : System.Web.UI.Page
    {
        _3PL_BaseData_DAO GetBD = new _3PL_BaseData_DAO();
        ControlBind CB = new ControlBind();
        private UserInf UI = new UserInf();
        private string Login_Server = "3PL";

        protected void Page_Load(object sender, EventArgs e)
        {
            ((_3PLMasterPage)Master).SessionCheck(ref UI);

            if (!IsPostBack)
            {
                //產生作業大類下拉式選單
                CB.DropDownListBind(ref ddl_CateId, GetBD.GetCateId(Login_Server), "S_bsda_CateId", "S_bsda_CateName", "請選擇", "");
            }
        }

        //大類切換
        protected void ddl_CateId_SelectedIndexChanged(object sender, EventArgs e)
        {
            GVRefresh();
        }

        #region GV_BaseData_Control
        //重新整理
        private void GVRefresh()
        {
            string CateId = ddl_CateId.SelectedValue.ToString();
            Session["BDList"] = GetBD.GetBDList(Login_Server, CateId);
            GVBind();
        }

        //綁定
        private void GVBind()
        {
            if (GV_BaseData.EditIndex != -1)  //修改中
            {
                GV_BaseData.ShowFooter = false;
            }
            else
            {
                GV_BaseData.ShowFooter = true;
            }
            GV_BaseData.DataSource = Session["BDList"];
            GV_BaseData.DataBind();
        }

        //換頁
        protected void GV_BaseData_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {

            GridView GV = sender as GridView;

            DataTable dt = new DataTable();
            dt = (DataTable)Session["BDList"];
            GV.PageIndex = e.NewPageIndex;
            GVBind();
        }

        //修改
        protected void GV_BaseData_RowEditing(object sender, GridViewEditEventArgs e)
        {
            GV_BaseData.EditIndex = e.NewEditIndex;
            GVBind();
        }

        //更新
        protected void GV_BaseData_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            //取得最新的DataTable
            DataTable dt = (DataTable)Session["BDList"];

            GridViewRow row = (GridViewRow)GV_BaseData.Rows[e.RowIndex];
            DataRow dr = dt.Rows[row.DataItemIndex];
            #region 更新修改欄位
            dr["S_bsda_FieldId"] = ((TextBox)row.FindControl("TBX_S_bsda_FieldId")).Text.Trim();
            dr["S_bsda_FieldName"] = ((TextBox)row.FindControl("TBX_S_bsda_Fieldname")).Text.Trim();
            #endregion
            GetBD.BDList_Update(Login_Server, UI.UserID, dr);
            GV_BaseData.EditIndex = -1;
            GVRefresh();
        }

        //取消
        protected void GV_BaseData_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            GV_BaseData.EditIndex = -1;
            GVBind();
        }

        //新增
        protected void BTN_Insert_BaseData_Click(object sender, EventArgs e)
        {
            #region 錯誤處理
            string InsertString0 = ((TextBox)GV_BaseData.FooterRow.FindControl("TBX_INS_S_bsda_FieldId")).Text.Trim();
            if (InsertString0 == "")
            {
                ((_3PLMasterPage)Master).ShowMessage("無法建立空白代號");
                return;
            }
            string InsertString1 = ((TextBox)GV_BaseData.FooterRow.FindControl("TBX_INS_S_bsda_Fieldname")).Text.Trim();
            if (InsertString1 == "")
            {
                ((_3PLMasterPage)Master).ShowMessage("無法建立空白大類");
                return;
            }
            #endregion

            //取得最新的DataTable
            DataTable dt = (DataTable)Session["BDList"];

            DataRow dr = dt.NewRow();
            dr[0] = 65535;
            dr[1] = dt.Rows[0][1];
            dr[2] = dt.Rows[0][2];
            dr["S_bsda_FieldId"] = InsertString0;
            dr["S_bsda_FieldName"] = InsertString1;
            GetBD.BDList_Insert(Login_Server, UI.UserID, dr);

            string CateId = dr[1].ToString();
            GV_BaseData.EditIndex = -1;
            GVRefresh();
        }

        //刪除
        protected void GV_BaseData_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            //取得最新的DataTable
            DataTable dt = (DataTable)Session["BDList"];

            GridViewRow row = (GridViewRow)GV_BaseData.Rows[e.RowIndex];
            DataRow dr = dt.Rows[row.DataItemIndex];
            #region 更新修改欄位
            string DelSeq = ((Label)row.FindControl("Lbl_I_bsda_seq")).Text.Trim();
            #endregion
            GetBD.BDList_Delete(Login_Server, DelSeq, UI.UserID);
            GVRefresh();
        }
        #endregion
    }
}
