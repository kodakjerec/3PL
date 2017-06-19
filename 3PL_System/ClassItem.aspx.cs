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
    public partial class ClassItem : System.Web.UI.Page
    {
        private UserInf UI = new UserInf();
        protected void Page_Load(object sender, EventArgs e)
        {
            ((_3PLMasterPage)Master).SessionCheck(ref UI);
            if (!IsPostBack)
            {
                btn_Ent.Visible = false;
                string ClassID = (Request.QueryString["ClassId"] == null) ? "" : Request.QueryString["ClassId"].ToString();
                string ClassNm = (Request.QueryString["ClassNm"] == null) ? "" : Request.QueryString["ClassNm"].ToString();
                hid_ClassId.Value = ClassID;
                if (ClassID.Length > 0)
                {
                    hid_Status.Value = "1";//修改
                    txb_Name.Visible = false;
                    lblName.Text = ClassNm;
                }
                else
                {
                    hid_Status.Value = "0";//新增
                   
                }
                CrtViewTable();
                CrtCtrl();
                
            }
        }

        protected void btn_Submit_Click(object sender, EventArgs e)
        {
            string ClassId = string.Empty;
            string strRole = ddl_Role.SelectedValue;
            string strRoleName = ddl_Role.SelectedItem.Text;
            string strDC = ddl_DC.SelectedValue;
            string strDCName = ddl_DC.SelectedItem.Text;
            string Msg = string.Empty;
            DataTable dtTemp = (DataTable)ViewState["Role"];
            try
            {
                ClassId = hid_ClassId.Value;
                if (ClassId.Length == 0)
                {
                    if (txb_Name.Text.Trim().Length == 0)
                    {
                        Msg += "請填寫類別名稱!!!" + "\\n";
                    }
                    else
                    {
                        lblName.Text = txb_Name.Text.Trim();
                        txb_Name.Visible = false;
                        ClassId = GetClassId();
                        hid_ClassId.Value = ClassId;
                    }
                }
                if (strRole.Length == 0)
                {
                    Msg += "請選擇權限角色!!" + "\\n";
                }
                if (strDC.Length == 0)
                {
                    Msg += "請選倉別!!" + "\\n";
                }
                if (Msg.Length == 0)
                {
                    int RowsCount = dtTemp.Rows.Count;
                    if (RowsCount == 0)
                    {
                        dtTemp.Rows.Add(new object[] { strRole, strRoleName, strDC, strDCName });
                    }
                    else
                    {
                        DataView view = new DataView();
                        DataTable Temp = new DataTable();
                        view.Table = dtTemp;
                        view.RowFilter = "RoleID = '" + strRole + "' and DC = '" + strDC + "'";
                        Temp = view.ToTable();
                        if (Temp.Rows.Count == 0)
                        {
                            dtTemp.Rows.Add(new object[] { strRole, strRoleName, strDC, strDCName });
                        }
                        else
                        {
                            ScriptManager.RegisterClientScriptBlock(this, typeof(string), "alert", "alert(' 重複新增!!! ');", true);
                            return;
                        }
                    }
                    ViewState["Role"] = dtTemp;
                    BindGv(dtTemp);
                    ddl_Role.SelectedValue = string.Empty;
                    ddl_DC.SelectedValue = string.Empty;
                    btn_Ent.Visible = true;
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this, typeof(string), "alert", "alert('" + Msg + "');", true);
                }
            }
            catch
            {
                ScriptManager.RegisterClientScriptBlock(this, typeof(string), "alert", "alert('系統異常，請洽資訊部!!');", true);
            }
        }

        protected void btn_Ent_Click(object sender, EventArgs e)
        {
            RoleInf RI = new RoleInf();
            string Status=hid_Status.Value;
            string UserId = UI.UserID;
            string ClassId = hid_ClassId.Value;
            string ClassName = lblName.Text;
            DataTable dt = (DataTable)ViewState["Role"];
            bool boClass = false;
            bool boClassRole = false;
            try
            {
                if (Status == "1")
                {
                    RI.DelClass("3PL", ClassId);
                }
                boClass = RI.AddClassInf("3PL", ClassId, ClassName, UserId);
                int Count = dt.Rows.Count;
                for (int i = 0; i < Count; i++)
                {
                    string RoleId = dt.Rows[i]["RoleID"].ToString();
                    string DC = dt.Rows[i]["DC"].ToString();
                    boClassRole = RI.AddClassRole("3PL", ClassId, RoleId, DC, UserId);
                }
                if (boClass && boClassRole)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert(' 執行完成!! ') ; location.href='RoleClassQuery.aspx';", true);
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this, typeof(string), "alert", "alert('執行失敗!!');", true);
                }
            }
            catch
            {
                ScriptManager.RegisterClientScriptBlock(this, typeof(string), "alert", "alert('系統異常，請洽資訊部!!');", true);
            }
        }

        protected void gv_List_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            GridViewRow Row = ((GridViewRow)((WebControl)(e.CommandSource)).NamingContainer);
            int idx = Row.DataItemIndex;
            if (e.CommandName == "Del")
            {
                DataTable dt = (DataTable)ViewState["Role"];
                dt.Rows.RemoveAt(idx);
                ViewState["Role"] = dt;
                gv_List.DataSource = dt;
                gv_List.DataBind();
                if (dt.Rows.Count == 0)
                {
                    btn_Ent.Visible = false;
                }
            }
        }

        protected void gv_List_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                DataTable dt = new DataTable();
                dt = (DataTable)ViewState["Role"];
                gv_List.PageIndex = e.NewPageIndex;
               
                BindGv(dt);
            }
            catch
            {
            }
        }

        /// <summary>
        /// 建置控制項
        /// </summary>
        private void CrtCtrl()
        {
            ControlBind CB = new ControlBind();
            RoleInf RI = new RoleInf();
            _3PL_CommonQuery BaseList = new _3PL_CommonQuery();
            try
            {
                CB.DropDownListBind(ref ddl_DC, BaseList.GetFieldList("3PL", "SiteNo"), "S_bsda_FieldId", "S_bsda_FieldName", "請選擇", string.Empty);//倉別
                CB.DropDownListBind(ref ddl_Role, RI.dsRoleList("3PL", string.Empty).Tables[0], "RoleId", "RoleNm", "請選擇", string.Empty);//權限角色
            }
            catch
            {

            }
        }

        /// <summary>
        /// 建立ViewState
        /// </summary>
        private void CrtViewTable()
        {
            try
            {
                if (ViewState["Role"] == null)
                {
                    DataTable dtRole = new DataTable();
                    dtRole.Columns.Add("RoleID", typeof(string));
                    dtRole.Columns.Add("RoleName", typeof(string));
                    dtRole.Columns.Add("DC", typeof(string));
                    dtRole.Columns.Add("DcName", typeof(string));
                    ViewState["Role"] = dtRole;
                }
                string ClassId = hid_ClassId.Value;
                if (ClassId.Length > 0)
                {
                    InsView(ClassId);
                    btn_Ent.Visible = true;
                }
            }
            catch
            {
            }
        }

        /// <summary>
        ///  填入ViewState
        /// </summary>
        /// <param name="ClassId"></param>
        private void InsView(string ClassId)
        {
            EmpInf EI = new EmpInf();
            DataSet dsTemp = new DataSet();
            DataTable dtTemp = new DataTable();
            DataTable dtClass = new DataTable();
            dsTemp = EI.dsClassRole("3PL", ClassId);
            dtTemp = dsTemp.Tables[0];
            dtClass = (DataTable)ViewState["Role"];
            int Count = dtTemp.Rows.Count;
            for (int i = 0; i < Count; i++)
            {
                string strRole = dtTemp.Rows[i]["RoleId"].ToString();
                string strRoleName = dtTemp.Rows[i]["RoleNm"].ToString();
                string strDC = dtTemp.Rows[i]["DC"].ToString();
                string strDCName = dtTemp.Rows[i]["DCNm"].ToString();
                dtClass.Rows.Add(new object[] { strRole, strRoleName, strDC, strDCName });
            }
            BindGv(dtClass);
            ViewState["Role"] = dtClass;
        }

        /// <summary>
        /// 建置GridView
        /// </summary>
        /// <param name="dt"></param>
        private void BindGv(DataTable dt)
        {
            gv_List.DataSource = dt;
            gv_List.DataBind();
        }

        /// <summary>
        /// 取得最新ClassId
        /// </summary>
        /// <returns></returns>
        private string GetClassId()
        {
            string ClassID = string.Empty;
            string TempClass = string.Empty;
            string NewClass = string.Empty;
            EmpInf EI = new EmpInf();
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            ds = EI.dsGetClassId("3PL");
            dt = ds.Tables[0];
            ClassID = dt.Rows[0]["ClassId"] == null ? "000000" : dt.Rows[0]["ClassId"].ToString();
            TempClass = ClassID.Substring(1, 5);
            int intClass = Convert.ToInt32(TempClass) + 1;
            NewClass = "W" + string.Format("{0:00000}", intClass);
            return NewClass;
        }
    }
}
