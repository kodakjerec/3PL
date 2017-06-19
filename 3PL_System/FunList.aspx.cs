using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Collections;
using _3PL_DAO;
using _3PL_LIB;

namespace _3PL_System
{
    public partial class FunList : System.Web.UI.Page
    {
        private UserInf UI = new UserInf();
        protected void Page_Load(object sender, EventArgs e)
        {
            ((_3PLMasterPage)Master).SessionCheck(ref UI);
            if (!IsPostBack)
            {
                CreateCtrl();
                btn_Submit.Visible = false;
            }
        }

        /// <summary>
        /// 查詢
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btn_Query_Click(object sender, EventArgs e)
        {
            string ErrMsg = string.Empty;
            string strFun = string.Empty;
            string Role_ID = string.Empty;
            DataSet dsFunList = new DataSet();
            DataTable dtFunList = new DataTable();
            RoleInf RI= new RoleInf();

            try
            {
                Role_ID = ddl_Role.SelectedValue;
                hid_Role_Id.Value = Role_ID;
                strFun = txb_Fun.Text.Trim();
                if (Role_ID == string.Empty)
                {
                    ErrMsg += "請選擇權限角色!!";
                }
                if (ErrMsg.Length != 0)
                {
                    ScriptManager.RegisterClientScriptBlock(this, typeof(string), "alert", "alert('" + ErrMsg + "');", true);
                    return;
                }
                else
                {
                    dsFunList = RI.dsFunRoleList("3PL", Role_ID, strFun);
                    dtFunList = dsFunList.Tables[0];
                    if (dtFunList.Rows.Count > 0)
                    {
                        Session["FunList"] = dtFunList;
                        BindGV(dtFunList);
                        btn_Submit.Visible = true;
                    }
                }
            }
            catch
            {
                ScriptManager.RegisterClientScriptBlock(this, typeof(string), "alert", "alert(' 系統異常，請洽資訊部 !!!');", true);
            }
        }

        /// <summary>
        /// 修改確認
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btn_Submit_Click(object sender, EventArgs e)
        {
            string ErrMsg = string.Empty;
            RoleInf RI = new RoleInf();
            DataSet dsFunList = new DataSet();
            DataTable dtFunList = new DataTable();
            try
            {
                string Role_ID = hid_Role_Id.Value;
                bool boRole = false;
                foreach (GridViewRow row in gv_List.Rows)
                {
                    CheckBox ck = (CheckBox)row.Cells[0].FindControl("cbk_FunList");
                    HiddenField hidRoleID = (HiddenField)row.Cells[0].FindControl("hid_RoleId");
                   
                    string strRole = hidRoleID.Value;
                    if (ck.Checked && strRole.Length == 0)//增加
                    {
                        HiddenField hidFunId = (HiddenField)row.Cells[0].FindControl("hid_FunId");
                        HiddenField hidPgId = (HiddenField)row.Cells[0].FindControl("hid_PgId");
                        string strFunId = hidFunId.Value;
                        string strPgId = hidPgId.Value;
                        boRole=AddFunList(Role_ID, strFunId, strPgId);
                        //((HiddenField)row.Cells[0].FindControl("hid_RoleId")).Value = Role_ID;
                    }
                    else if (!ck.Checked && strRole.Length > 0) //移除
                    {
                        HiddenField hidFunId = (HiddenField)row.Cells[0].FindControl("hid_FunId");
                        HiddenField hidPgId = (HiddenField)row.Cells[0].FindControl("hid_PgId");
                        string strFunId = hidFunId.Value;
                        string strPgId = hidPgId.Value;
                        //((HiddenField)row.Cells[0].FindControl("hid_RoleId")).Value = string.Empty;
                        boRole=RemoveFunList(Role_ID, strFunId, strPgId);
                    }
                }
                dsFunList = RI.dsFunRoleList("3PL", Role_ID, string.Empty);
                dtFunList = dsFunList.Tables[0];
                Session["FunList"] = dtFunList;
                BindGV(dtFunList);
                if (boRole)
                {
                    ScriptManager.RegisterClientScriptBlock(this, typeof(string), "alert", "alert(' 更新成功 !!!');", true);
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this, typeof(string), "alert", "alert(' 更新失敗 !!!');", true);
                }
            }
            catch
            {
                ScriptManager.RegisterClientScriptBlock(this, typeof(string), "alert", "alert(' 系統異常，請洽資訊部 !!!');", true);
            }
        }

        protected void gv_List_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                HiddenField hidRoleID = (HiddenField)e.Row.Cells[0].FindControl("hid_RoleId");
                string RoleId = hidRoleID.Value;
                if (RoleId != string.Empty)
                {
                    ((CheckBox)e.Row.Cells[0].FindControl("cbk_FunList")).Checked = true;
                }
            }
        }

        protected void gv_List_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                DataTable dt = new DataTable();
                dt = (DataTable)Session["FunList"];
                gv_List.PageIndex = e.NewPageIndex;
                BindGV(dt);
            }
            catch
            { }
        }

        /// <summary>
        /// GridView建置
        /// </summary>
        /// <param name="dt"></param>
        public void BindGV(DataTable dt)
        {
            gv_List.DataSource = dt;
            gv_List.DataBind();
        }

        /// <summary>
        /// 建置控制項
        /// </summary>
        private void CreateCtrl()
        {
            ControlBind CB = new ControlBind();
            RoleInf RI = new RoleInf();
            try
            {
                CB.DropDownListBind(ref ddl_Role, RI.dsRoleList("3PL", string.Empty).Tables[0], "RoleId", "RoleNm", "請選擇", string.Empty);
            }
            catch
            {
                ScriptManager.RegisterClientScriptBlock(this, typeof(string), "alert", "alert(' 系統異常，請洽資訊部 !!!');", true);
            }
        }

        /// <summary>
        /// 新增角色選單
        /// </summary>
        private bool AddFunList(string RoleId, string FunId, string PgID)
        {
            RoleInf RI = new RoleInf();
            string UserId = UI.UserID;
            bool blIns = RI.AddRoleFun("3PL", RoleId, FunId, PgID, UserId);
            return blIns; 
        }

        /// <summary>
        /// 移除角色選單
        /// </summary>
        private bool RemoveFunList(string RoleId, string FunId, string PgID)
        {
            
            RoleInf RI = new RoleInf();
            string UserId = UI.UserID;
            bool blIns = RI.RemoveRoleFun("3PL", RoleId, FunId, PgID, UserId);
            return blIns;
        }
    }
}
