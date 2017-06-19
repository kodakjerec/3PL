using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using _3PL_LIB;
using _3PL_DAO;
using System.Data;

namespace _3PL_System
{
    public partial class _3PL_SignOffPermission : System.Web.UI.Page
    {
        _3PL_SignOff_DAO GetSignOff = new _3PL_SignOff_DAO();
        EmpInf empInf = new EmpInf();
        ControlBind CB = new ControlBind();
        private UserInf UI = new UserInf();
        private string Login_Server = "3PL";

        protected void Page_Load(object sender, EventArgs e)
        {
            ((_3PLMasterPage)Master).SessionCheck(ref UI);

            if (!IsPostBack)
            {
                //產生作業大類下拉式選單
                CB.DropDownListBind(ref DDL_PageType, GetSignOff.GetSOPageType(Login_Server), "PageType", "PageName", "請選擇", "");
                DDL_PageType.SelectedIndex = 1;
                CB.DropDownListBind(ref DDL_StatusName, GetSignOff.GetSOStatusName(Login_Server, DDL_PageType.SelectedValue), "Sn", "StatusName", "請選擇", "");
                DDL_StatusName.SelectedIndex = 1;
                CB.DropDownListBind(ref DDL_Class, empInf.dsClassInf(Login_Server, "", "").Tables[0], "ClassId", "ClassName", "請選擇", "");
                CB.DropDownListBind(ref DDL_Worker, empInf.dsEmpList(Login_Server, "", "", "").Tables[0], "WorkId", "WorkName", "請選擇", "");

                Session["AccList"] = GetSignOff.GetSOPermission(Login_Server);
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

        protected void GV_BaseAccounting_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName != "Page")
            {
                GridViewRow row = ((GridViewRow)((WebControl)(e.CommandSource)).NamingContainer);
                string Sn = ((HiddenField)row.Cells[0].FindControl("hid_Sn")).Value;
                if (e.CommandName == "Btn_Del")
                    Btn_Del_Click(Sn);
            }
        }

        //新增
        protected void Btn_New_Click(object sender, EventArgs e)
        {
            if (DDL_Class.SelectedIndex == 0 && DDL_Worker.SelectedIndex == 0)
                return;

            string SN = DDL_StatusName.SelectedValue;

            //群組
            string ClassId = DDL_Class.SelectedItem.Value, ClassName = DDL_Class.SelectedItem.Text;
            if (ClassId == "")
                ClassName = "";

            //使用者
            string WorkId = DDL_Worker.SelectedItem.Value, WorkName = DDL_Worker.SelectedItem.Text;
            if (WorkId == "")
                WorkName = "";

            int SuccessCount = GetSignOff.AddPermission(Login_Server, SN, ClassId, ClassName, WorkId, WorkName);
            if (SuccessCount > 0)
            {
                ((_3PLMasterPage)Master).ShowMessage("權限新增完成");
            }
            else
            {
                ((_3PLMasterPage)Master).ShowMessage("權限新增失敗");
            }

            Session["AccList"] = GetSignOff.GetSOPermission(Login_Server);
            GVBind();
        }

        //刪除
        private void Btn_Del_Click(string Sn)
        {
            int SuccessCount = GetSignOff.DelPermission(Login_Server, Sn);
            if (SuccessCount > 0)
            {
                ((_3PLMasterPage)Master).ShowMessage("權限刪除完成");
            }
            else
            {
                ((_3PLMasterPage)Master).ShowMessage("權限刪除失敗");
            }

            Session["AccList"] = GetSignOff.GetSOPermission(Login_Server);
            GVBind();
        }
        #endregion

        //切換單據種類
        protected void DDL_PageType_SelectedIndexChanged(object sender, EventArgs e)
        {
            CB.DropDownListBind(ref DDL_StatusName, GetSignOff.GetSOStatusName(Login_Server, DDL_PageType.SelectedValue), "Sn", "StatusName", "請選擇", "");
            DDL_StatusName.SelectedIndex = 1;
        }


    }
}
