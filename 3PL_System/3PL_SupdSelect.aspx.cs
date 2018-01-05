using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using _3PL_DAO;
using _3PL_System;

namespace SC_Offer
{
    public partial class SCObject : System.Web.UI.Page
    {
        _3PL_CommonQuery _3PLCQ = new _3PL_CommonQuery();

        protected void Page_Load(object sender, EventArgs e)
        {
            DataTable dtSup = new DataTable();
            string strSupNo = string.Empty;
            string strSupName = string.Empty;
            try
            {
                dtSup = (DataTable)Session["Sup"];
                if (dtSup == null)
                {
                    strSupNo = txb_ObjNo.Text.Trim();
                    strSupName = txb_ObjName.Text.Trim();
                    dtSup = _3PLCQ.GetSupdId("3PL", 0, strSupNo, strSupName);
                    Session["Sup"] = dtSup;
                }
                BindGV();
            }
            catch
            {
                ((_3PLMasterPage)Master).ShowMessage("系統異常請洽資訊部");
            }
        }

        public void BindGV()
        {
            DataTable Dt = (DataTable)Session["Sup"];
            gv_List.DataSource = Dt;
            gv_List.DataBind();
        }

        protected void gv_List_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gv_List.PageIndex = e.NewPageIndex;
                BindGV();
            }
            catch
            {
                ((_3PLMasterPage)Master).ShowMessage("系統異常請洽資訊人員");
            }
        }

        protected void gv_List_RowEditing(object sender, GridViewEditEventArgs e)
        {
            try
            {
                int index = e.NewEditIndex;
                string SupNo = gv_List.Rows[index].Cells[1].Text;
                string SupSn = gv_List.Rows[index].Cells[2].Text;
                //_txbSupNo = "opener.document." + Request.QueryString["ReturnLocation"].ToString() + ".value='" + SupNo + "';";
                string _txbSupNo = "parent.document.getElementById('" + Request.QueryString["ReturnLocation"].ToString() + "').value='" + SupNo + "';";

                ScriptManager.RegisterClientScriptBlock(Page, typeof(string), "a", _txbSupNo, true);
                if (Request.QueryString["ReturnLocation2"] != null)
                {
                    //string _txbSupSn = "opener.document." + Request.QueryString["ReturnLocation2"].ToString() + ".value='" + SupSn + "';";
                    string _txbSupSn = "parent.document.getElementById('" + Request.QueryString["ReturnLocation2"].ToString() + "').value='" + SupSn + "';";

                    ScriptManager.RegisterClientScriptBlock(Page, typeof(string), "b", _txbSupSn, true);
                }
                //ScriptManager.RegisterClientScriptBlock(Page, typeof(string), "c", "window.close();", true);
                string _Closebtn = "parent.$('#" + Request.QueryString["btnCloseID"] + "').trigger('click'); ";
                ScriptManager.RegisterClientScriptBlock(Page, typeof(string), "c", _Closebtn, true);
            }
            catch
            { }
        }

        protected void txb_ObjNo_TextChanged(object sender, EventArgs e)
        {
            DataTable dt = (DataTable)Session["Sup"];
            try
            {
                DataTable tblFiltered = dt.AsEnumerable()
              .Where(row => row.Field<String>("ID").IndexOf(txb_ObjNo.Text) >= 0
                       || row.Field<String>("ALIAS").IndexOf(txb_ObjName.Text) >= 0)
              .OrderBy(row => row.Field<String>("ID"))
              .CopyToDataTable();
                Session["Sup"] = tblFiltered;
            }
            catch
            {
                DataTable tblFiltered = dt.Clone();
                Session["Sup"] = tblFiltered;
            }

            BindGV();

        }
    }
}
