using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using _3PL_DAO;
using _3PL_LIB;

namespace _3PL_System
{
    public partial class EmpList : System.Web.UI.Page
    {
        private UserInf UI = new UserInf();
        protected void Page_Load(object sender, EventArgs e)
        {
            ((_3PLMasterPage)Master).SessionCheck(ref UI);

            if (!Page.IsPostBack)
            {
                CreateCtrl();
                GetUser(string.Empty, string.Empty, string.Empty);
            }
        }

        /// <summary>
        /// 查詢
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btn_Submit_Click(object sender, EventArgs e)
        {
            string strWorkId = string.Empty;
            string strClassId = string.Empty;
            string strName = string.Empty;
            try
            {
                strWorkId = txb_WorkId.Text.Trim();
                strClassId = ddl_Class.SelectedValue;
                strName = txb_Name.Text.Trim();
                GetUser(strWorkId, strClassId, strName);
            }
            catch
            {
                ScriptManager.RegisterClientScriptBlock(this, typeof(string), "alert", "alert(' 系統異常，請洽資訊人員!!! ');", true);
                return;
            }
        }

        /// <summary>
        /// 產生控制項
        /// </summary>
        private void CreateCtrl()
        {
            ControlBind CB = new ControlBind();
            EmpInf EI = new EmpInf();
            try
            {
                CB.DropDownListBind(ref ddl_Class, EI.dsClassInf("3PL", string.Empty,string.Empty).Tables[0], "ClassId", "ClassName", "請選擇", string.Empty);
            }
            catch
            {
                ScriptManager.RegisterClientScriptBlock(this, typeof(string), "alert", "alert(' 系統異常，請洽資訊人員!!! ');", true);
                return;
            }
        }

        /// <summary>
        /// 取得使用者清單
        /// </summary>
        /// <param name="WorkID"></param>
        /// <param name="ClassID"></param>
        private void GetUser(string WorkID, string ClassID,string WorkName)
        {
            DataTable dtUser = new DataTable();
            DataSet dsUser = new DataSet();
            EmpInf EI = new EmpInf();

            try
            {
                dsUser = EI.dsEmpList("3PL", WorkID, ClassID, WorkName);
                dtUser = dsUser.Tables[0];
                if (dtUser.Rows.Count > 0)
                {
                    Session["UserList"] = dtUser;
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this, typeof(string), "alert", "alert(' 查無資料 ');", true);
                }
                BindGV(dtUser);
            }
            catch
            {
                ScriptManager.RegisterClientScriptBlock(this, typeof(string), "alert", "alert(' 系統異常，請洽資訊人員!!! ');", true);
                return;
            }
        }

        /// <summary>
        /// 建置Gridview
        /// </summary>
        /// <param name="dt"></param>
        private void BindGV(DataTable dt)
        {
            gv_List.DataSource = dt;
            gv_List.DataBind();
        }

        /// <summary>
        /// 點選Gridview
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gv_List_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName != "Page")
            {
                GridViewRow Row = ((GridViewRow)((WebControl)(e.CommandSource)).NamingContainer);
                string WorkID = Row.Cells[0].Text.ToString();
                try
                {
                    if (e.CommandName == "Select")
                    {
                        string Path = string.Format("EmpItem.aspx?WorkId={0}", WorkID);
                        ScriptManager.RegisterClientScriptBlock(this, typeof(string), "", "location.href='" + Path + "';", true);
                    }
                    else if (e.CommandName == "Del")
                    {
                        bool booDel = false;
                        EmpInf EI = new EmpInf();
                        booDel = EI.DelEmp("3PL", WorkID, "1", UI.UserID);
                        if (booDel)
                        {
                            GetUser(string.Empty, string.Empty, string.Empty);
                        }
                        else
                        {
                            ScriptManager.RegisterClientScriptBlock(this, typeof(string), "alert", "alert(' 刪除失敗!!! ');", true);
                        }
                    }
                }
                catch
                { }
            }
        }

        /// <summary>
        /// Gridview換頁
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gv_List_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                DataTable dt = new DataTable();
                dt = (DataTable)Session["UserList"];
                gv_List.PageIndex = e.NewPageIndex;
                BindGV(dt);
            }
            catch
            {
                ScriptManager.RegisterClientScriptBlock(this, typeof(string), "alert", "alert(' 系統異常，請洽資訊人員!!! ');", true);
                return;
            }
        }
    }
}
