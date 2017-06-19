using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using _3PL_LIB;
using _3PL_DAO;
using System.Data;

namespace _3PL_System
{
    public partial class _3PL_Assign_New : System.Web.UI.Page
    {
        _3PL_CommonQuery _3PLCQ = new _3PL_CommonQuery();
        _3PL_Assign_DAO _3PLAssign = new _3PL_Assign_DAO();
        _3PL_DAO._3PL_Assign_New _3pa = new _3PL_DAO._3PL_Assign_New();
        EmpInf _3PLEmpInf = new EmpInf();
        ControlBind CB = new ControlBind();
        private UserInf UI = new UserInf();
        private const string Login_Server = "3PL";

        protected void Page_Load(object sender, EventArgs e)
        {
            ((_3PLMasterPage)Master).SessionCheck(ref UI);

            if (!IsPostBack)
            {
                DataTable dt1 = _3PLCQ.GetFieldList(Login_Server, "SiteNo");
                CB.DropDownListBind(ref ddl_S_qthe_SiteNo, dt1, "S_bsda_FieldId", "S_bsda_FieldName", "ALL", "");
                CB.DropDownListBind(ref DDL_DC, dt1, "S_bsda_FieldId", "S_bsda_FieldName", "請選擇", "");

            }
        }

        #region 查詢-報價單
        //點下查詢報價單
        protected void Btn_CrePLNOList_Click(object sender, EventArgs e)
        {
            string PLNO = txb_S_qthe_PLNO.Text;
            string site_no = ddl_S_qthe_SiteNo.SelectedValue;
            DataTable dt1 = _3pa.GetQuotation(PLNO, site_no);
            Session["dt_head"] = dt1;
            GV_PriceList_Query_Bind();
        }
        #endregion

        #region 選擇報價單
        //GV_PriceList_Query_Bind
        private void GV_PriceList_Query_Bind()
        {
            DataTable dt1 = (DataTable)Session["dt_head"];
            GV_PriceList_Query.DataSource = dt1;
            GV_PriceList_Query.DataBind();
        }
        //換頁
        protected void GV_PriceList_Query_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GV_PriceList_Query.PageIndex = e.NewPageIndex;
            GV_PriceList_Query_Bind();
        }
        //GV_PriceList選定單頭
        protected void GV_PriceList_Query_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            div_AssignClass_Demo.Visible = true;
            div_AssignClass_Detail_Demo.Visible = true;

            if (e.CommandName != "Page")
            {
                GridViewRow row = ((GridViewRow)((WebControl)(e.CommandSource)).NamingContainer);
                if (e.CommandName == "EdtSCdata")
                    EdtSCData(row);
            }
        }
        //帶出單頭資訊
        private void EdtSCData(GridViewRow row)
        {
            //日期
            Txb_Wk_Date.Text = DateTime.Now.ToString("yyyy/MM/dd");
            //派工類別
            Txb_Wk_ClassName.Text = ((Label)row.Cells[0].FindControl("lbl_SCData_TypeIdname")).Text;
            DDL_DC.SelectedValue = ((HiddenField)row.Cells[0].FindControl("Sel_S_qtde_SiteNo")).Value;
            Txb_EtaDate.Text = Txb_Wk_Date.Text;
            DataSet ds1 = _3PLEmpInf.dsEmpInf(Login_Server, UI.UserID, "");
            string UserClassAndName = ds1.Tables[0].Rows[0]["ClassName"].ToString() + "," + ds1.Tables[0].Rows[0]["WorkName"].ToString();
            Txb_CreateUser.Text = UserClassAndName;
            Txb_Wk_Unit.Text = ((Label)row.Cells[4].FindControl("lbl_SCData_ClassName")).Text;
            Txb_SupID.Text = ((Label)row.Cells[0].FindControl("lbl_SCData_site_no")).Text;
            //選定單頭
            hid_PLNO.Value = ((Label)row.Cells[0].FindControl("lbl_SCData_PLNO")).Text;
            hid_TypeId.Value = ((HiddenField)row.Cells[0].FindControl("Sel_TypeId")).Value;
            hid_ClassId.Value = ((HiddenField)row.Cells[0].FindControl("Sel_ClassId")).Value;

            bringDetail(((Label)row.Cells[0].FindControl("lbl_SCData_PLNO")).Text, DDL_DC.SelectedValue, hid_TypeId.Value, hid_ClassId.Value);
        }
        //帶出明細資訊
        private void bringDetail(string PLNO, string site_no, string TypeId, string ClassId)
        {
            DataTable dt1 = _3pa.getCostlist(PLNO, site_no, TypeId, ClassId);
            Session["dt_detail"] = dt1;
            GV_Quotation_Detail_New.DataSource = dt1;
            GV_Quotation_Detail_New.DataBind();

            DataTable dt_detail_Final = dt1.Clone();
            Session["dt_detail_Final"] = dt_detail_Final;
            GridView1.DataSource = dt_detail_Final;
            GridView1.DataBind();
        }
        #endregion

        #region 產生派工單明細Demo
        //產生明細
        protected void Btn_Query_Click(object sender, EventArgs e)
        {
            DataTable dt_detail_Final = (DataTable)Session["dt_detail_Final"];
            DataTable dt1 = (DataTable)Session["dt_detail"];
            string PO_NO = txb_D_qthe_ContractS_Qry.Text;
            string item_no = txb_D_qthe_ContractE_Qry.Text;
            string site_no = DDL_DC.SelectedValue;

            #region 檢查目的地有沒有相同key值
            foreach (DataRow dr in dt_detail_Final.Rows)
            {
                if (dr["PONO"].ToString() == PO_NO && dr["itemno"].ToString() == item_no)
                {
                    ((_3PLMasterPage)Master).ShowMessage("已有相同關鍵字明細");
                    return;
                }
            }
            #endregion

            #region 帶入PO單數量
            DataTable POlist = _3pa.getPOList(PO_NO, item_no, site_no, Txb_SupID.Text.Substring(0, 4));
            string TQty = POlist.Rows[0][0].ToString(),
                    TBox = POlist.Rows[0][1].ToString(),
                    TPallet = POlist.Rows[0][2].ToString();
            if (TQty == null || TQty == "")
                TQty = "0";
            if (TBox == null || TBox == "")
                TBox = "0";
            if (TPallet == null || TPallet == "")
                TPallet = "0";
            if (PO_NO != "" && TQty == "0")
            {
                ((_3PLMasterPage)Master).ShowMessage("查無指定PO單及貨號");
                return;
            }

            foreach (DataRow dr in dt1.Rows)
            {
                dr["PONO"] = PO_NO;
                dr["itemno"] = item_no;
                dr["Tqty"] = TQty;
                dr["TBoxQty"] = TBox;
                dr["TpalletQty"] = TPallet;
                //帶入數量
                dr["Ordqty"] = _3PLAssign.GetDetailNeedQty(POlist.Rows[0], dr["S_bsda_FieldName"].ToString());

                DataRow dr_detail_Final = dt_detail_Final.NewRow();
                int i = 0;
                foreach (DataColumn dc in dt1.Columns)
                {
                    dr_detail_Final[i] = dr[i];
                    i++;
                }
                dt_detail_Final.Rows.Add(dr_detail_Final);
            }
            #endregion

            dt_detail_Final.AcceptChanges();
            Session["dt_detail_Final"] = dt_detail_Final;
            GridView1.DataSource = dt_detail_Final;
            GridView1.DataBind();
        }
        //選擇要刪除的row
        protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName != "Page")
            {
                GridViewRow row = ((GridViewRow)((WebControl)(e.CommandSource)).NamingContainer);
                if (e.CommandName == "EdtSCdata")
                {
                    string PO_NO = ((Label)row.Cells[0].FindControl("lbl_GridView1_PO_no")).Text;
                    string itemno = ((Label)row.Cells[0].FindControl("lbl_GridView1_itemno")).Text;
                    GridView1_DeleteRows(PO_NO, itemno);
                }
            }
        }
        //刪除view的row
        private void GridView1_DeleteRows(string PO_NO, string item_no)
        {
            DataTable dt_detail_Final = (DataTable)Session["dt_detail_Final"];
            foreach (DataRow dr in dt_detail_Final.Rows)
            {
                if (dr.RowState != DataRowState.Deleted)
                    if (dr["PONO"].ToString() == PO_NO && dr["itemno"].ToString() == item_no)
                    {
                        dr.Delete();
                    }
            }
            dt_detail_Final.AcceptChanges();
            Session["dt_detail_Final"] = dt_detail_Final;
            GridView1.DataSource = dt_detail_Final;
            GridView1.DataBind();
        }
        #endregion

        #region 產生派工單
        //產生派工單
        protected void Btn_CreatePage_Click(object sender, EventArgs e)
        {
            DataTable dt_detail_Final = (DataTable)Session["dt_detail_Final"];
            if (GridView1.Rows.Count == 0)
            {
                ((_3PLMasterPage)Master).ShowMessage("沒有派工單明細");
                return;
            }

            //新增單頭
            string New_WkId = _3pa.Head_Assign_New(Login_Server, hid_PLNO.Value, hid_TypeId.Value, UI.UserID, hid_ClassId.Value);
            if (New_WkId != "")
            {
                //更新單頭備註
                _3pa.Head_Assign_UpdateMemo(Login_Server, New_WkId, txa_Memo.Value);

                //新增明細
                //因為是單純新增, 不用像Assign_Modify需要考慮修改狀況
                string PO_NO = null, item_no = null;
                string TQty = "", I_bcse_seq = "";
                int Tcount = 0, SuccessCount = 0;
                Tcount = GridView1.Rows.Count;

                foreach (GridViewRow gvr in GridView1.Rows)
                {
                    I_bcse_seq = ((HiddenField)gvr.Cells[0].FindControl("lbl_I_bcse_seq")).Value;
                    PO_NO = ((Label)gvr.Cells[0].FindControl("lbl_GridView1_PO_no")).Text;
                    item_no = ((Label)gvr.Cells[0].FindControl("lbl_GridView1_itemno")).Text;
                    TQty = ((TextBox)gvr.Cells[0].FindControl("lbl_GridView1_OrdQty")).Text;

                    string IsSuccess = _3pa.Detail_Assign_New(Login_Server, New_WkId, "115543", PO_NO, item_no, TQty, I_bcse_seq);
                    SuccessCount += Convert.ToInt32(IsSuccess);
                }

                string ErrMsg = "";

                if (Tcount == SuccessCount)
                {
                    ErrMsg = "新增成功，單號：" + New_WkId;
                }
                else
                {
                    ErrMsg = "新增失敗，總筆數：" + Tcount.ToString() + " ，成功筆數：" + SuccessCount.ToString();
                }
                ((_3PLMasterPage)Master).ShowMessage(ErrMsg, CreatePath(New_WkId));
            }
            else
            {
                ((_3PLMasterPage)Master).ShowMessage("新增失敗");
             }
        }

        /// <summary>
        /// 建立回傳路徑
        /// </summary>
        /// <returns></returns>
        private string CreatePath(string New_WkId)
        {
            //建立給3PL_Assign_Query字串
            string VarString = _3PLCQ.Page_AssignQuery(
                                                Txb_SupID.Text
                                                , DDL_DC.SelectedValue
                                                , New_WkId
                                                , New_WkId
                                                ,""
                                                ,"");

            string Path = "";
            if (Request.QueryString["VarString"] != null && Request.QueryString["VarString"] != "")
            {
                Path = string.Format("3PL_Assign_Query.aspx?VarString={0}", Request.QueryString["VarString"]);
            }
            else
            {
                Path = string.Format("3PL_Assign_Query.aspx?VarString={0}", VarString);
            }
            return Path;
        }
        #endregion
    }
}
