using _3PL_LIB;
using _3PL_DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace _3PL_System
{
    public partial class _PL_Addition : System.Web.UI.Page
    {
        _3PL_Addition_DAO _3PLAdd = new _3PL_Addition_DAO();
        Check _3PLcheck = new Check();
        private UserInf UI = new UserInf();

        protected void Page_Load(object sender, EventArgs e)
        {
            ((_3PLMasterPage)Master).SessionCheck(ref UI);
            if (!IsPostBack)
            {
                txb_CloseDate.Text = _3PLAdd.Addon_GetCloseData();
                lbl_CloseDate.Text = txb_CloseDate.Text+"之前的單據禁止修改(不包含"+txb_CloseDate.Text+")";
            }
        }

        //確定修改關帳日期
        protected void btn_CloseDateConfirm_Click(object sender, EventArgs e)
        {
            string CloseData = txb_CloseDate.Text;
            if (!_3PLcheck.CkDate(CloseData)) {
                ((_3PLMasterPage)Master).ShowMessage("關帳日期格式不正確");
                return;
            }

            bool IsOK = _3PLAdd.Addon_UpdCloseData(CloseData);
            if (!IsOK)
            {
                ((_3PLMasterPage)Master).ShowMessage("關帳日期異動失敗");
            }
            else
            {
                ((_3PLMasterPage)Master).ShowMessage("關帳日期異動完成");
            }

            txb_CloseDate.Text = _3PLAdd.Addon_GetCloseData();
            lbl_CloseDate.Text = txb_CloseDate.Text + "之前的單據禁止修改(不包含" + txb_CloseDate.Text + ")";
            
        }
    }
}
