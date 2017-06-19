using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using _3PL_LIB;
using _3PL_DAO;
using System.Data;

namespace _3PL_System
{
    public partial class _3PL_CostList_Modify : System.Web.UI.Page
    {
        _3PL_CommonQuery _3PLCQ = new _3PL_CommonQuery();
        _3PL_CostList_DAO _3PLCostList = new _3PL_CostList_DAO();
        EmpInf _3PLEmpInf = new EmpInf();

        ControlBind CB = new ControlBind();
        Check _3PL_Check = new Check();
        private UserInf UI = new UserInf();
        private string Login_Server = "3PL";

        protected void Page_Load(object sender, EventArgs e)
        {
            ((_3PLMasterPage)Master).SessionCheck(ref UI);

            if (!IsPostBack)
            {
                //產生作業大類下拉式選單
                CB.DropDownListBind(ref DDL_DC, _3PLCQ.GetFieldList(Login_Server, "SiteNo"), "S_bsda_FieldId", "S_bsda_FieldName", "請選擇", "");

                hidTotal_I_qthe_seq.Value = Request.QueryString["Wk_Id"] == null ? "" : Request.QueryString["Wk_Id"].ToString();


                //判別新增/修改
                if (hidTotal_I_qthe_seq.Value == "")
                {
                    //新增
                    lbl_Quotation_Query.Text = "報價單新增";
                }
                else
                {
                    //修改
                    lbl_Quotation_Query.Text = "報價單異動";
                    Query_Head(hidTotal_I_qthe_seq.Value);
                }
            }
        }

        #region Head

        #region Head-Controller-動作
        //預先載入報價單
        private void Query_Head(string I_qthe_seq)
        {
            string Wk_Id = hidTotal_I_qthe_seq.Value;
            DataTable dt_head = _3PLCostList.GetCostList(Wk_Id, UI.UserID);
            if (dt_head.Rows.Count <= 0)
            {
                ScriptManager.RegisterClientScriptBlock(this, typeof(string), "alert", "alert('找不到指定單號')", true);
                return;
            }
            DataRow dr = dt_head.Rows[0];
            //派工日期
            if (dr["Wk_Date"].ToString() != "")
                Txb_Wk_Date.Text = DateTime.Parse(dr["Wk_Date"].ToString()).ToString("yyyy/MM/dd");
            else
                Txb_Wk_Date.Text = "";
            //報價主類別
            Txb_Wk_ClassName.Text = dr["Wk_ClassName"].ToString();
            //倉別
            DDL_DC.SelectedValue = dr["DC"].ToString();
            //預定完工日
            if (dr["EtaDate"].ToString() != "")
                Txb_EtaDate.Text = DateTime.Parse(dr["EtaDate"].ToString()).ToString("yyyy/MM/dd");
            else
                Txb_EtaDate.Text = "";
            //建單人
            DataSet ds1 = _3PLEmpInf.dsEmpInf(Login_Server, UI.UserID, "");
            string UserClassAndName = ds1.Tables[0].Rows[0]["ClassName"].ToString() + "/" + ds1.Tables[0].Rows[0]["WorkName"].ToString();

            Txb_CreateUser.Text = UserClassAndName;
            //處理單位
            Txb_Wk_Unit.Text = dr["Wk_Unit"].ToString() + "," + dr["Wk_UnitName"].ToString();
            //廠商編號
            Txb_SupID.Text = dr["SupID"].ToString();
            //實際完工日
            if (dr["ActDate"].ToString() != "")
                Txb_ActDate.Text = DateTime.Parse(dr["ActDate"].ToString()).ToString("yyyy/MM/dd");
            else
                Txb_ActDate.Text = "";

            Session["Quotation_head"] = dt_head;
            Session["Quotation_Detail"] = _3PLCostList.GetCostDetail(Wk_Id);

            GVBind_Quotation_Detail();

            DIV_Quotation_Detail_New.Visible = true;
            Btn_Detail_New_Update.Visible = true;
        }
        #endregion

        #endregion

        #region Detail
        protected void GV_Quotation_Detail_New_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GV_Quotation_Detail_New.PageIndex = e.NewPageIndex;
            GVBind_Quotation_Detail();
        }
        //重新綁定
        private void GVBind_Quotation_Detail()
        {
            DataTable dt = (DataTable)Session["Quotation_Detail"];
            GV_Quotation_Detail_New.DataSource = dt;
            GV_Quotation_Detail_New.DataBind();
        }

        #endregion

        //選定更新行
        protected void GV_Quotation_Detail_New_RowCommand(object sender, GridViewCommandEventArgs e)
        {

        }

        #region 單據修改完成,按下確定
        //回寫資料
        protected void Btn_Detail_New_Update_Click(object sender, EventArgs e)
        {
            string ErrMsg = "";
            bool IsSuccess = false;
            DataTable dt_head = (DataTable)Session["Quotation_head"];
            DataTable dt_Detail = (DataTable)Session["Quotation_Detail"];

            #region 將資料寫入至Session的Table
            string Wk_Id = hidTotal_I_qthe_seq.Value;
            string Sn = "", WorkHr = "", CostFree = "";
            DataRow dr;
            foreach (GridViewRow gvr in GV_Quotation_Detail_New.Rows)
            {
                Sn = ((HiddenField)gvr.Cells[1].FindControl("hid_Sn")).Value;
                object[] objParam = { Wk_Id, Sn };
                dr = dt_Detail.Rows.Find(objParam);

                //人數, 20150224改為數量
                WorkHr = ((TextBox)gvr.Cells[3].FindControl("Txb_WorkHr")).Text;
                if (WorkHr == "")
                    dr["WorkHr"] = DBNull.Value;
                else
                {
                    if (!_3PL_Check.CkDecimal(WorkHr))
                    {
                        ScriptManager.RegisterClientScriptBlock(this, typeof(string), "alert", "alert('人數  輸入格式有問題')", true);
                        return;
                    }
                    else
                    {
                        dr["WorkerNum"] = 1;
                        dr["WorkHr"] = WorkHr;
                    }
                }

                //成本
                CostFree = ((Label)gvr.Cells[6].FindControl("Lbl_CostFree")).Text;
                if (WorkHr != "")
                {
                    dr["Total"] = Convert.ToDouble(WorkHr) * Convert.ToDouble(CostFree);
                }
                dr["UpdUser"] = UI.UserID;
            }
            #endregion

            if (GV_Quotation_Detail_New.Rows.Count <= 0)
            {
                ErrMsg += "明細無資料！\\n";
            }
            else
            {
                //新增
                if (hidTotal_I_qthe_seq.Value == "")
                {
                    //取得最大單號
                    string dt_No = _3PLCQ.GetMaxPageNo(Login_Server, 1);

                }
                else
                {
                    //修改
                    IsSuccess = _3PLCostList.InsertQuotation(Login_Server, dt_head, dt_Detail, UI.UserID);
                    if (IsSuccess)
                        ErrMsg += "修改成功！\\n";
                    else
                    {
                        ErrMsg += "修改失敗！\\n";
                    }
                }
            }

            //跳出錯誤訊息
            if (ErrMsg != "")
            {
                if (IsSuccess)
                {
                    ((_3PLMasterPage)Master).ShowMessage(ErrMsg, CreatePath());
                }
                else
                {
                    ((_3PLMasterPage)Master).ShowMessage(ErrMsg);
                }
            }
            
        }

        //取消更新
        protected void Btn_Detail_New_Update_Leave_Click(object sender, EventArgs e)
        {
            Response.Redirect(CreatePath());
        }

        /// <summary>
        /// 建立回傳路徑
        /// </summary>
        /// <returns></returns>
        private string CreatePath()
        {
            string Path = "";
            if (Request.QueryString["VarString"] != "")
            {
                Path = string.Format("3PL_Assign_Query.aspx?VarString={0}", Request.QueryString["VarString"]);
            }
            else
            {
                Path = string.Format("3PL_Assign_Query.aspx");
            }
            return Path;
        }
        #endregion
    }
}
