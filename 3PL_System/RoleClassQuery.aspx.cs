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
    public partial class RoleClassQuery : System.Web.UI.Page
    {
        private UserInf UI = new UserInf();
        protected void Page_Load(object sender, EventArgs e)
        {
            ((_3PLMasterPage)Master).SessionCheck(ref UI);
            if (!Page.IsPostBack)
            {
                CreateCtrl();
                GetList(string.Empty,string.Empty);
            }
        }

        protected void btn_Query_Click(object sender, EventArgs e)
        {
            string ClassName = string.Empty;
            try 
            {
                ClassName = txb_Class.Text.Trim();
                GetList(string.Empty, ClassName);

            }
            catch
            {
                ScriptManager.RegisterClientScriptBlock(this, typeof(string), "alert", "alert(' 系統異常，請洽資訊人員!!! ');", true);
                return;
            }
        }

        /// <summary>
        /// GridView換頁
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gv_List_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                DataTable dt = new DataTable();
                dt = (DataTable)Session["ClassRole"];
                gv_List.PageIndex = e.NewPageIndex;
                BindGV(dt);
            }
            catch
            {
                ScriptManager.RegisterClientScriptBlock(this, typeof(string), "alert", "alert(' 系統異常，請洽資訊人員!!! ');", true);
                return;
            }
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
                int index = Row.RowIndex;
                string ClassId = ((HiddenField)gv_List.Rows[index].FindControl("hid_ClassId")).Value;
                string ClassName = ((LinkButton)gv_List.Rows[index].FindControl("lbtn_ClassName")).Text;
                try
                {
                    if (e.CommandName == "Select")
                    {
                        string Path = string.Format("ClassItem.aspx?ClassId={0}&ClassNm={1}", ClassId, ClassName);
                        ScriptManager.RegisterClientScriptBlock(this, typeof(string), "", "location.href='" + Path + "';", true);
                    }
                    else if (e.CommandName == "Del")
                    {
                        bool booDel = false;
                        RoleInf RI = new RoleInf();
                        booDel = RI.DelClass("3PL", ClassId);
                        if (booDel)
                        {
                            GetList(string.Empty, string.Empty);
                        }
                        else
                        {
                            ScriptManager.RegisterClientScriptBlock(this, typeof(string), "alert", "alert(' 刪除失敗!!! ');", true);
                        }
                    }
                }
                catch
                {
                    ScriptManager.RegisterClientScriptBlock(this, typeof(string), "alert", "alert(' 系統異常，請洽資訊人員!!! ');", true);
                }
            }
        }

        /// <summary>
        /// 建置控制項
        /// </summary>
        private void CreateCtrl()
        {
            ControlBind CB = new ControlBind();
            EmpInf EI = new EmpInf();
            try
            {
                //CB.DropDownListBind(ref ddl_Class, EI.dsClassInf("3PL", string.Empty).Tables[0], "ClassId", "ClassName", "請選擇", string.Empty);
            }
            catch
            {
                ScriptManager.RegisterClientScriptBlock(this, typeof(string), "alert", "alert(' 系統異常，請洽資訊人員!!! ');", true);
                return;
            }
        }

        /// <summary>
        /// 取得類別清單
        /// </summary>
        /// <param name="ClassId"></param>
        /// <param name="Name"></param>
        private void GetList(string ClassId,string Name) 
        {
            DataTable dtClass = new DataTable();
            DataSet dsClass = new DataSet();
            EmpInf EI = new EmpInf();
            try 
            {
                dsClass = EI.dsClassInf("3PL", ClassId,Name);
                dtClass = dsClass.Tables[0];
                if (dtClass.Rows.Count > 0)
                {
                    Session["ClassRole"] = dtClass;
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this, typeof(string), "alert", "alert(' 查無資料!!! ');", true);
                }
                BindGV(dtClass);
            }
            catch
            { }
        }

        /// <summary>
        /// 建置GridView
        /// </summary>
        /// <param name="dt"></param>
        private void BindGV(DataTable dt)
        {
            gv_List.DataSource = dt;
            gv_List.DataBind();
        }

    }
}
