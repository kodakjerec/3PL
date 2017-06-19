using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using _3PL_DAO;
using _3PL_LIB;
using System.Data;

namespace _3PL_System
{
    public partial class AlartRp : System.Web.UI.Page
    {
        _3PL_CommonQuery _3PLCQ = new _3PL_CommonQuery();
        _3PL_Assign_DAO _3PLAssign = new _3PL_Assign_DAO();
        _3PL_SignOff_DAO _3PLSignOff = new _3PL_SignOff_DAO();
        private string Login_Server = "3PL";
        private UserInf UI = new UserInf();

        protected void Page_Load(object sender, EventArgs e)
        {
            #region 個資
            ((_3PLMasterPage)Master).SessionCheck(ref UI);
            #endregion

            if (!IsPostBack)
            {
                #region 未完成報價單
                DataTable NotOKQuotation = _3PLCQ.GetNotOKQuotation(Login_Server, UI);
                Session["NotOKQuotation"] = NotOKQuotation;
                GV_NotOKQuotation_Bind();
                #endregion

                #region 未完成派工單
                DataTable NotOKAssignList = _3PLCQ.GetNotOKAssign(Login_Server, UI);
                Session["NotOKAssignList"] = NotOKAssignList;
                GV_NotOKAssign_Bind();
                #endregion
            }
        }

        #region Leave_button
        protected void btn_Close_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", "window.close()", true);
        }
        #endregion

        #region 未完成報價單_Command
        protected void GV_NotOKQuotation_RowCreated(object sender, GridViewRowEventArgs e)
        {
            string PLNO = "", Varstring = "", Path = "";
            foreach (GridViewRow gvr in GV_NotOKQuotation.Rows)
            {
                HyperLink linkBtn = ((HyperLink)gvr.Cells[1].FindControl("Lbl_報價單號"));
                PLNO = linkBtn.Text;
                Varstring = _3PLCQ.Page_QuotationQuery("", "", "", "", PLNO);
                Path = "3PL_Quotation_Query.aspx?VarString=" + Varstring;
                linkBtn.NavigateUrl = Path;
            }
        }

        private void GV_NotOKQuotation_Bind()
        {
            DataTable NotOKQuotation = (DataTable)Session["NotOKQuotation"];
            GV_NotOKQuotation.DataSource = NotOKQuotation;
            GV_NotOKQuotation.DataBind();
        }

        protected void GV_NotOKQuotation_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GV_NotOKQuotation.PageIndex = e.NewPageIndex;
            GV_NotOKQuotation_Bind();
        }
        #endregion

        #region 未完成派工單_Command
        protected void GV_NotOKAssign_RowCreated(object sender, GridViewRowEventArgs e)
        {
            string PLNO = "", Varstring = "", Path = "";
            foreach (GridViewRow gvr in GV_NotOKAssign.Rows)
            {
                HyperLink linkBtn = ((HyperLink)gvr.Cells[1].FindControl("Lbl_派工單號"));
                PLNO = linkBtn.Text;
                Varstring = _3PLCQ.Page_AssignQuery("", "", PLNO, PLNO, "", "");
                Path = "3PL_Assign_Query.aspx?VarString=" + Varstring;
                linkBtn.NavigateUrl = Path;
            }
        }

        private void GV_NotOKAssign_Bind()
        {
            DataTable NotOKAssignList = (DataTable)Session["NotOKAssignList"];
            GV_NotOKAssign.DataSource = NotOKAssignList;
            GV_NotOKAssign.DataBind();
        }

        protected void GV_NotOKAssign_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GV_NotOKAssign.PageIndex = e.NewPageIndex;
            GV_NotOKAssign_Bind();
        }
        #endregion

        #region 批次簽核
        /// <summary>
        /// 派工單批次簽核
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btn_SignOff_ThisPageAssign_Click(object sender, EventArgs e)
        {
            string ErrMsg = "";

            foreach (GridViewRow gvr in GV_NotOKAssign.Rows)
            {
                string PLNO = ((HyperLink)gvr.Cells[0].FindControl("Lbl_派工單號")).Text;
                string Assign_Status = ((HiddenField)gvr.Cells[0].FindControl("Hid_Assign_Status")).Value;
                string Assign_EtaDate = ((HiddenField)gvr.Cells[0].FindControl("Hid_Assign_EtaDate")).Value;
                int SuccessCount = 0;

                #region 檢查欄位是否有填
                #region 建單-->業務主管簽核 必填:預定完工日,派工數量
                if (Assign_Status == "10")
                {
                    if (Assign_EtaDate == "")
                    {
                        ErrMsg += PLNO + "派工單:預定完工日尚未填寫\n";
                        continue;
                    }
                    if (!_3PLAssign.CheckAssignClassStep1(PLNO))
                    {
                        ErrMsg += PLNO + "派工單:派工數量尚未填寫\n";
                        continue;
                    }
                }
                #endregion

                #region 派工人員簽核 完工數量要填,工時要填
                if (Assign_Status == "13")
                {
                    if (!_3PLAssign.CheckAssignClassStep4(PLNO))
                    {
                        ErrMsg += PLNO + "派工單:完工數量尚未填寫\n";
                        continue;
                    }
                    if (!_3PLAssign.CheckCostListStep1(PLNO))
                    {
                        ErrMsg += PLNO + "成本單:工時尚未填寫\n";
                        continue;
                    }
                    //檢查通過之後，派工單要押上實際完工日
                    _3PLSignOff.Assign_ActDate(Login_Server, PLNO);
                }
                #endregion
                #endregion

                //需每關做簽核
                SuccessCount = _3PLSignOff.SignOff_Quotation(Login_Server, UI.UserID, UI.UserName, true, Assign_Status, PLNO, "2");

                if (SuccessCount <= 0)
                    ErrMsg += PLNO + " 簽核執行失敗\n";
                else
                    ErrMsg += PLNO + " 簽核執行成功\n";
            }

            if (ErrMsg != "")
                ((_3PLMasterPage)Master).ShowMessage(ErrMsg.Replace("\n", "<br>"));

            #region 未完成派工單
            DataTable NotOKAssignList = _3PLCQ.GetNotOKAssign(Login_Server, UI);
            Session["NotOKAssignList"] = NotOKAssignList;
            GV_NotOKAssign_Bind();
            #endregion
        }

        /// <summary>
        /// 報價單批次簽核
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btn_SignOff_ThisPageQuotation_Click(object sender, EventArgs e)
        {
            string ErrMsg = "";

            foreach (GridViewRow gvr in GV_NotOKQuotation.Rows)
            {
                string PLNO = ((HyperLink)gvr.Cells[0].FindControl("Lbl_報價單號")).Text;
                string I_qthe_IsSpecial = ((HiddenField)gvr.Cells[0].FindControl("Hid_I_qthe_IsSpecial")).Value;
                string I_qthe_Status = ((HiddenField)gvr.Cells[0].FindControl("Hid_I_qthe_Status")).Value;
                int SuccessCount = 0;
                if (Convert.ToBoolean(I_qthe_IsSpecial))
                {
                    //其他議價單,需每關做簽核
                    SuccessCount = _3PLSignOff.SignOff_Quotation(Login_Server, UI.UserID, UI.UserName, true, I_qthe_Status, PLNO, "1");
                }
                else
                {
                    //一般單,直接跳完成
                    SuccessCount = _3PLSignOff.SignOff_Quotation_NotSpecial(Login_Server, UI.UserID, UI.UserName, true, I_qthe_Status, PLNO);
                }
                if (SuccessCount <= 0)
                    ErrMsg += PLNO + " 簽核執行失敗\n";
                else
                    ErrMsg += PLNO + " 簽核執行成功\n";
            }

            if (ErrMsg != "")
                ((_3PLMasterPage)Master).ShowMessage(ErrMsg.Replace("\n", "<br>"));

            #region 未完成報價單
            DataTable NotOKQuotation = _3PLCQ.GetNotOKQuotation(Login_Server, UI);
            Session["NotOKQuotation"] = NotOKQuotation;
            GV_NotOKQuotation_Bind();
            #endregion
        }
        #endregion
    }
}
