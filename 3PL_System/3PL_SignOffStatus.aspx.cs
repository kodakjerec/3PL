using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using _3PL_LIB;
using _3PL_DAO;
using System.Data;

namespace _3PL_System
{
    public partial class _3PL_SignOffStatus : System.Web.UI.Page
    {
        _3PL_SignOff_DAO GetSignOff = new _3PL_SignOff_DAO();
        ControlBind CB = new ControlBind();
        private UserInf UI = new UserInf();
        private string Login_Server = "3PL";

        protected void Page_Load(object sender, EventArgs e)
        {
            ((_3PLMasterPage)Master).SessionCheck(ref UI);

            if (!IsPostBack)
            {
                Session["AccList"] = GetSignOff.GetSOStatus(Login_Server);
                GVBind();
            }
        }

        #region GV_BaseAccounting_Control
        //綁定
        private void GVBind()
        {
            if (GV_BaseAccounting.EditIndex != -1)  //修改中
            {
                GV_BaseAccounting.ShowFooter = false;
            }
            else
            {
                GV_BaseAccounting.ShowFooter = true;
            }
            GV_BaseAccounting.DataSource = Session["AccList"];
            GV_BaseAccounting.DataBind();
        }

        //換頁
        protected void GV_BaseAccounting_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridView GV = sender as GridView;

            DataTable dt = new DataTable();
            dt = (DataTable)Session["AccList"];
            GV.PageIndex = e.NewPageIndex;
            GVBind();
        }

        //修改
        protected void GV_BaseAccounting_RowEditing(object sender, GridViewEditEventArgs e)
        {
            GV_BaseAccounting.EditIndex = e.NewEditIndex;
            GVBind();
        }

        //更新
        protected void GV_BaseAccounting_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            //取得最新的DataTable
            DataTable dt = (DataTable)Session["AccList"];

            GridViewRow row = (GridViewRow)GV_BaseAccounting.Rows[e.RowIndex];
            DataRow dr = dt.Rows[row.DataItemIndex];
            #region 更新修改欄位
            dr["StatusName"] = ((TextBox)row.FindControl("TBX_StatusName")).Text.Trim();
            dr["OkbuttonName"] = ((TextBox)row.FindControl("TBX_OkbuttonName")).Text.Trim();
            dr["NobuttonName"] = ((TextBox)row.FindControl("TBX_NobuttonName")).Text.Trim();
            #endregion
            GetSignOff.SignOff_Update(Login_Server, UI.UserID, dr);
            GV_BaseAccounting.EditIndex = -1;
            GVBind();
        }

        //取消
        protected void GV_BaseAccounting_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            GV_BaseAccounting.EditIndex = -1;
            GVBind();
        }

        //新增
        protected void BTN_Insert_BaseAccounting_Click(object sender, EventArgs e)
        {
            //string InsertId = ((TextBox)GV_BaseAccounting.FooterRow.FindControl("TBX_INS_S_Acci_Id")).Text.Trim();
            //string InsertName = ((TextBox)GV_BaseAccounting.FooterRow.FindControl("TBX_INS_S_Acci_Name")).Text.Trim();
            //if (InsertId == "" || InsertName=="")
            //{
            //    ScriptManager.RegisterClientScriptBlock(this, typeof(string), "alert", "alert('無法建立空白科目')", true);
            //    return;
            //}

            ////取得最新的DataTable
            //DataTable dt = (DataTable)Session["AccList"];

            //DataRow dr = dt.NewRow();
            //dr[0] = 65535;
            //dr[1] = InsertId;
            //dr[2] = InsertName;
            //GetAcc.AccList_Insert(Login_Server, UI.UserID, dr);

            //Session["AccList"] = GetAcc.GetAccList(Login_Server);
            //GV_BaseAccounting.EditIndex = -1;
            //GVBind();
        }
        #endregion
    }
}
