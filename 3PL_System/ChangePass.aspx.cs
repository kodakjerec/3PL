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
    public partial class ChangePass : System.Web.UI.Page
    {
        private ACE_EnCode ACE = new ACE_EnCode();
        private UserInf UI = new UserInf();
        protected void Page_Load(object sender, EventArgs e)
        {
            ((_3PLMasterPage)Master).SessionCheck(ref UI);
        }

        protected void btn_Submit_Click(object sender, EventArgs e)
        {
            string Msg = string.Empty;
            bool blUpd = false;
            try
            {
                EmpInf EI = new EmpInf();
                DataSet dsEmpInf = new DataSet();
                string strOldPsw = string.Empty;
                string OldEnPsw = string.Empty;
                strOldPsw = txb_OldPsw.Text.Trim();
                OldEnPsw = ACE.AESEn(strOldPsw, "SCSystem"); //密碼加密
                dsEmpInf = EI.dsEmpInf("3PL", UI.UserID, OldEnPsw); //利用Id/Password 判別密碼有無錯誤
                if (dsEmpInf.Tables[0].Rows.Count == 0)
                {
                    Msg += " 原密碼錯誤!!! \\n 請重新確認!!! \\n";
                }
                else
                {
                    string NewPsw = string.Empty;
                    string RePsw = string.Empty;
                    NewPsw = txb_Psw.Text.Trim();
                    RePsw = txb_RePsw.Text.Trim();
                    if (NewPsw.Length < 5) //判別有無輸入新密碼
                    {
                        Msg += "新密碼長度需介於英文、數字5~20字!!! \\n";
                    }
                    if (Msg.Length == 0)
                    {
                        if (NewPsw==RePsw)//判別前後兩次密碼有無差異
                        {
                            NewPsw = ACE.AESEn(NewPsw, "SCSystem");
                            blUpd = EI.UpdPsw("3PL", UI.UserID, NewPsw);
                            if (blUpd)
                            {
                                Msg += "更新成功!!! \\n";
                            }
                            else
                            {
                                Msg += "更新失敗!!! \\n";
                            }
                        }
                        else
                        {
                            Msg += " 兩次密碼輸入不相等!!! \\n 請重新確認!!! \\n";
                        }
                    }
                }
                ScriptManager.RegisterClientScriptBlock(this, typeof(string), "alert", "alert('" + Msg + "');", true);
            }
            catch
            {
                ScriptManager.RegisterClientScriptBlock(this, typeof(string), "alert", "alert('更新失敗!!!');", true);
            }
        }
    }
}
