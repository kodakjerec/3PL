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
    public partial class EmpItem : System.Web.UI.Page
    {
        private ACE_EnCode ACE = new ACE_EnCode();
        private UserInf UI = new UserInf();

        protected void Page_Load(object sender, EventArgs e)
        {
            ((_3PLMasterPage)Master).SessionCheck(ref UI);
            if (!Page.IsPostBack)
            {
                CreateCtrl();
                string WorkId = (Request.QueryString["WorkId"] == null) ? "" : Request.QueryString["WorkId"].ToString();
                if (WorkId.Length > 0)//修改人資
                {
                    lblWorkId.Text = WorkId;
                    lbl_CustInf.Text = "使用者修改";
                    txb_WorkId.Visible = false;
                    GetEmp(WorkId);
                    btn_Deel.Attributes.Add("style", "display:inline");
                    btn_Cancel.Attributes.Add("style", "display:inline");
                }
            }
        }

        /// <summary>
        /// 新增/修改 確認
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btn_Submit_Click(object sender, EventArgs e)
        {
            string ErrMsg = string.Empty;
            string strID = lblWorkId.Text;
            string strName = txb_Name.Text.Trim();
            string strPsw = txb_Psw.Text.Trim();
            string strRePsw = txb_RePsw.Text.Trim();
            string strHdPsw = hid_Psw.Value;
            string strEmail = txb_Mail.Text.Trim();
            string strClassID = ddl_Class.SelectedValue;
            string EnPss = string.Empty;
            bool blSec = false;
            EmpInf EI = new EmpInf();
            try
            {
                if (txb_WorkId.Visible)
                {
                    strID = txb_WorkId.Text.Trim();
                    if (strID.Length == 0)
                    {
                        ErrMsg += "請輸入帳號!!\\n";
                    }
                    else
                    {
                        DataTable dtCKID = new DataTable();
                        dtCKID = dtEmpInf(strID);
                        if (dtCKID.Rows.Count > 0)
                        {
                            ErrMsg += "該帳號已有人使用，請重新確認!!\\n";
                        }
                    }
                }
                if (strName.Length == 0)
                {
                    ErrMsg += "請輸入人員姓名!!\\n";
                }
                if (strClassID == string.Empty)
                {
                    ErrMsg += "請選擇人員類別!!\\n";
                }

                if (strHdPsw.Length == 0 && strPsw.Length == 0)
                {
                    ErrMsg += "請輸入密碼!!\\n";
                }
                if (strHdPsw.Length == 0 && strRePsw.Length == 0)
                {
                    ErrMsg += "請輸入確認密碼!!\\n";
                }
                if (strPsw != strRePsw)
                {
                    ErrMsg += "兩次密碼不同，請重新確認!!\\n";
                }
                if (strEmail.Length > 0 && !strEmail.Contains("@"))
                {
                    ErrMsg += "請輸入正確E-mail !!\\n";
                }
                if (ErrMsg.Length == 0)
                {
                    if (strPsw.Length == 0)
                    {
                        strPsw = strHdPsw;
                    }
                    EnPss = ACE.AESEn(strPsw, "SCSystem");
                    if (strHdPsw.Length == 0)//新增
                    {
                        blSec = EI.InsEmp("3PL", strID, strName, EnPss, strEmail, strClassID, UI.UserID);
                        if (blSec)
                        {
                            ErrMsg = "新增成功!!";
                            txb_WorkId.Text = string.Empty;
                            txb_Name.Text = string.Empty;
                            ddl_Class.SelectedValue = string.Empty;
                            txb_Mail.Text = string.Empty;
                        }
                        else
                        {
                            ErrMsg = "新增失敗!!";
                        }
                        ScriptManager.RegisterClientScriptBlock(this, typeof(string), "alert", "alert('" + ErrMsg + "');", true);
                    }
                    else//修改
                    {
                        blSec = EI.UpdEmp("3PL", strID, strName, EnPss, strEmail, strClassID, UI.UserID);
                        if (blSec)
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('更新成功!!') ; location.href='EmpList.aspx';", true);
                        }
                        else
                        {
                            ScriptManager.RegisterClientScriptBlock(this, typeof(string), "alert", "alert('更新失敗');", true);
                        }
                    }
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this, typeof(string), "alert", "alert('" + ErrMsg + "');", true);
                }
            }
            catch
            {
                ScriptManager.RegisterClientScriptBlock(this, typeof(string), "alert", "alert('系統異常，請洽資訊部!!');", true);
            }
        }

        /// <summary>
        /// 刪除使用者
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btn_Deel_Click(object sender, EventArgs e)
        {
            bool booDel = false;
            EmpInf EI = new EmpInf();
            try
            {
                string WorkId = lblWorkId.Text;
                booDel = EI.DelEmp("3PL", WorkId, "1", UI.UserID);
                if (booDel)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('刪除成功!!') ; location.href='EmpList.aspx';", true);
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this, typeof(string), "alert", "alert('刪除失敗!!');", true);
                }
            }
            catch
            {
                ScriptManager.RegisterClientScriptBlock(this, typeof(string), "alert", "alert('刪除失敗!!');", true);
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
                CB.DropDownListBind(ref ddl_Class, EI.dsClassInf("3PL", string.Empty,string.Empty).Tables[0], "ClassId", "ClassName", "請選擇", string.Empty);
            }
            catch
            {
            }
        }

        /// <summary>
        /// 建置人員資料
        /// </summary>
        private void GetEmp(string WorkID)
        {
            DataTable dtEmp = new DataTable();
            try
            {
                dtEmp = dtEmpInf(WorkID);
                txb_Name.Text = dtEmp.Rows[0]["WorkName"] == null ? "" : dtEmp.Rows[0]["WorkName"].ToString();//姓名
                string strPass = dtEmp.Rows[0]["WorkPsw"] == null ? "" : dtEmp.Rows[0]["WorkPsw"].ToString();//密碼
                hid_Psw.Value = ACE.AESDc(strPass, "SCSystem");
                ddl_Class.SelectedValue = dtEmp.Rows[0]["ClassId"] == null ? "" : dtEmp.Rows[0]["ClassId"].ToString();//身分類別
                txb_Mail.Text = dtEmp.Rows[0]["Email"] == null ? "" : dtEmp.Rows[0]["Email"].ToString();//E-Mail
            }
            catch
            {
                ScriptManager.RegisterClientScriptBlock(this, typeof(string), "alert", "alert('系統異常，請洽資訊部!!');", true);
            }
        }

        /// <summary>
        /// 取得人員資料
        /// </summary>
        /// <param name="WorkId"></param>
        /// <returns></returns>
        private DataTable dtEmpInf(string WorkId)
        {
            DataTable dt = new DataTable();
            DataSet ds = new DataSet();
            EmpInf EI = new EmpInf();
            try
            {
                ds = EI.dsEmpInf("3PL", WorkId, string.Empty);
                dt = ds.Tables[0];
            }
            catch
            {
            }
            return dt;
        }

        protected void btn_Cancel_Click(object sender, EventArgs e)
        {
            ClientScript.RegisterClientScriptBlock(typeof(string), "logout", " location.href='EmpList.aspx';", true);
        }
    }
}
