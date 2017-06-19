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
    public partial class FunListSet : System.Web.UI.Page
    {
        private DataTable dtFunList = new DataTable();
        private UserInf UI = new UserInf();
        protected void Page_Load(object sender, EventArgs e)
        {
            ((_3PLMasterPage)Master).SessionCheck(ref UI);
            if (!IsPostBack)
            {
                pnl_AddFun.Visible = false;
                CrtCtrl(ddl_Fun, "0");
                ShowBtn(false);
            }
        }

        /// <summary>
        /// 查詢
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btn_Query_Click(object sender, EventArgs e)
        {
            RoleInf RI = new RoleInf();
            DataSet dsFun = new DataSet();
            DataTable dtFun = new DataTable();
            string strFunId = string.Empty;
            string strPgName = string.Empty;
            try
            {
                strFunId = ddl_Fun.SelectedValue;
                strPgName = txb_Pg.Text.Trim();
                dsFun = RI.dsFunList("3PL", strFunId, string.Empty, strPgName, string.Empty);
                dtFun = dsFun.Tables[0];
                Session["Fun"] = dtFun;
                BindGV(dtFun);
                if (dtFun.Rows.Count > 0)
                {
                    ShowBtn(true);
                }
                else
                {
                    ShowBtn(false);
                }
            }
            catch
            {
                ((_3PLMasterPage)Master).ShowMessage("系統異常，請洽資訊部");
            }
        }

        /// <summary>
        /// 開啟新增Function
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btn_Add_Click(object sender, EventArgs e)
        {
            pnl_AddFun.Visible = true;
            txb_FunNm.Visible = false;
            dyn_tr.Visible = false;
            btn_CalFunId.Visible = false;
            CrtCtrl(ddl_FunName, "1");
        }

        /// <summary>
        /// 更新功能清單
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btn_Update_Click(object sender, EventArgs e)
        {
            ArrayList arrPunId = new ArrayList(); //功能Id
            ArrayList arrPunName = new ArrayList();//功能名稱
            ArrayList arrOrd = new ArrayList();   //程式順序
            ArrayList arrPgId = new ArrayList();  //程式Id
            ArrayList arrPgName = new ArrayList();//程式名稱
            ArrayList arrUrl = new ArrayList();   //程式路徑位置
            try
            {
                foreach (GridViewRow row in gv_List.Rows)
                {
                    CheckBox chk = (CheckBox)row.Cells[0].FindControl("cbk_FunList");
                    if (chk.Checked)
                    {
                        string Msg = string.Empty;
                        string FgId = ((HiddenField)row.Cells[0].FindControl("hid_PgId")).Value;
                        string FunId = ((HiddenField)row.Cells[0].FindControl("hid_FunId")).Value;
                        string PgOrd = ((TextBox)row.Cells[2].FindControl("txb_Ord")).Text.Trim();
                        string PgName = ((TextBox)row.Cells[3].FindControl("txb_PgName")).Text.Trim();
                        string PgUrl = ((TextBox)row.Cells[3].FindControl("txb_Url")).Text.Trim();
                        if (PgOrd.Length == 0)
                        {
                            Msg += "順序不得為空值 !!" + " \\n ";
                        }
                        if (PgName.Length == 0)
                        {
                            Msg += "子功能名稱不得為空值 !!" + " \\n ";
                        }
                        if (PgUrl.Length == 0)
                        {
                            Msg += "連結路徑不得為空值 !!" + " \\n ";
                        }
                        if (Msg.Length == 0)
                        {
                            arrPunId.Add(FunId);
                            arrOrd.Add(PgOrd);
                            arrPgId.Add(FgId);
                            arrPgName.Add(PgName);
                            arrUrl.Add(PgUrl);
                        }
                        else
                        {
                            ((_3PLMasterPage)Master).ShowMessage(Msg);
                            return;
                        }
                    }
                }
                int ckCount = arrPunId.Count;
                if (ckCount > 0)
                {
                    bool blOrd = true;//檢查順序
                    bool blUp = false;//確認更新
                    string ErrMsg = string.Empty;
                    for (int i = 0; i < ckCount; i++)
                    {
                        string FgId = arrPgId[i].ToString();
                        string FunId = arrPunId[i].ToString();
                        string PgOrd = arrOrd[i].ToString();
                        string PgName = arrPgName[i].ToString();
                        string PgUrl = arrUrl[i].ToString();
                        blOrd = blCkOrd(FunId, FgId, PgOrd);
                        if (blOrd)
                        {
                            ((_3PLMasterPage)Master).ShowMessage(PgName + " 順序重複，更新失敗");
                            return;
                        }
                    }
                    for (int i = 0; i < ckCount; i++)
                    {
                        string FgId = arrPgId[i].ToString();
                        string FunId = arrPunId[i].ToString();
                        string PgOrd = arrOrd[i].ToString();
                        string PgName = arrPgName[i].ToString();
                        string PgUrl = arrUrl[i].ToString();
                        blUp = blUpMenu(FgId, FunId, PgOrd, PgName, PgUrl);
                        if (!blUp)
                        {
                            ErrMsg += PgName + " 更新失敗!!" + " \\n";
                        }
                    }
                    if (ErrMsg.Length == 0)
                    {
                        BindGV(null);
                        ShowBtn(false);
                        ((_3PLMasterPage)Master).ShowMessage("更新完畢");
                    }
                    else
                    {
                        ((_3PLMasterPage)Master).ShowMessage(ErrMsg);
                        return;
                    }
                }
                else
                {
                    ((_3PLMasterPage)Master).ShowMessage("請勾選欲更新資料");
                    return;
                }
            }

            catch
            {
                ((_3PLMasterPage)Master).ShowMessage("更新失敗");
            }
        }

        /// <summary>
        /// 刪除Function
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btn_Del_Click(object sender, EventArgs e)
        {
            ArrayList arrPunId = new ArrayList();
            ArrayList arrPgId = new ArrayList();
            try
            {
                foreach (GridViewRow row in gv_List.Rows)
                {
                    CheckBox chk = (CheckBox)row.Cells[0].FindControl("cbk_FunList");
                    if (chk.Checked)
                    {
                        string strPunId = ((HiddenField)row.Cells[0].FindControl("hid_FunId")).Value;
                        string strPgId = ((HiddenField)row.Cells[0].FindControl("hid_PgId")).Value;
                        arrPunId.Add(strPunId);
                        arrPgId.Add(strPgId);
                    }
                }
                if (arrPunId.Count > 0)
                {
                    DelFun(arrPunId, arrPgId);
                    BindGV(null);
                }
                ((_3PLMasterPage)Master).ShowMessage("刪除完畢");
                ShowBtn(false);
            }
            catch
            {
                ((_3PLMasterPage)Master).ShowMessage("刪除失敗");
            }
        }

        /// <summary>
        /// 取消新增
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btn_Cancel_Click(object sender, EventArgs e)
        {
            //Page.RegisterStartupScript("Show", "<script language=\"JavaScript\">show_tr();</script>");
            ddl_FunName.Visible = true;
            btn_AddFunId.Visible = true;
            pnl_AddFun.Visible = false;
        }

        /// <summary>
        /// 開啟新增主功能清單
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btn_AddFun_Click(object sender, EventArgs e)
        {
            btn_CalFunId.Visible = true;
            ddl_FunName.Visible = false;
            txb_FunNm.Visible = true;
            dyn_tr.Visible = true;
            btn_CalFunId.Visible = true;
            btn_AddFunId.Visible = false;
            hid_Status.Value = "1";
        }

        /// <summary>
        /// 開啟新增主功能清單
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btn_CalFunId_Click(object sender, EventArgs e)
        {
            ddl_FunName.Visible = true;
            txb_FunNm.Visible = false;
            dyn_tr.Visible = false;
            btn_CalFunId.Visible = false;
            btn_AddFunId.Visible = true;
            hid_Status.Value = string.Empty;
        }

        protected void gv_List_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                DataTable dt = new DataTable();
                dt = (DataTable)Session["Fun"];
                gv_List.PageIndex = e.NewPageIndex;
                BindGV(dt);
            }
            catch
            { }
        }

        protected void btn_Submit_Click(object sender, EventArgs e)
        {
            string ErrMsg = string.Empty;
            RoleInf Fn = new RoleInf();
            bool blInsF = false;
            string UserId = UI.UserID;
            //string UserId = "Chin_Yen";
            string InsStatus = string.Empty;
            InsStatus = hid_Status.Value;
            string FunOrd = string.Empty;   //主功能順序
            string FunId = string.Empty;    //主功能Id
            string FunName = string.Empty;  //主功能名稱
            string PgOrd = string.Empty;    //程式順序
            string PgId = string.Empty;     //程式Id
            string PgNm = string.Empty;     //程式名稱
            string PgUrl = string.Empty;    //程式路徑

            try
            {
                if (InsStatus == "1")
                {
                    FunOrd = txb_FunOrd.Text.Trim();
                    FunId = txb_FunId.Text.Trim();
                    FunName = txb_FunNm.Text.Trim();
                }
                else
                {
                    string strDDL = ddl_FunName.SelectedValue;
                    if (strDDL.Length != 0)
                    {
                        string[] arrFun = new string[2];
                        arrFun = strDDL.Split('-');
                        FunOrd = arrFun[0];
                        FunId = arrFun[1];
                        FunName = ddl_FunName.SelectedItem.ToString();
                    }
                }
                PgOrd = txb_PgOrd.Text.Trim();
                PgId = txb_PgId.Text.Trim();
                PgNm = txb_PgNm.Text.Trim();
                PgUrl = txb_Url.Text.Trim();
                if (FunName == string.Empty)
                {
                    ErrMsg += "請填入功能名稱 !!" + " \\n";
                }
                if (InsStatus == "1" && FunOrd == string.Empty)
                {
                    ErrMsg += "請填入功能順序 !!" + " \\n";
                }
                else if (InsStatus == "1")
                {
                    bool bl_FnOrd = blCkOrd(FunId, string.Empty, string.Empty);
                    if (bl_FnOrd)
                    {
                        ErrMsg = "功能順序重複，請重新確認 !!" + " \\n";
                    }
                }
                if (PgOrd == string.Empty)
                {
                    ErrMsg += "請填入程式順序 !!" + " \\n";
                }
                else
                {
                    bool bl_PgOrd = blCkOrd(FunId, string.Empty, PgOrd);
                    if (bl_PgOrd)
                    {
                        ErrMsg = "程式順序重複，請重新確認 !!" + " \\n";
                    }
                }
                if (PgNm == string.Empty)
                {
                    ErrMsg += "請填入程式名稱 !!" + " \\n";
                }
                if (PgId == string.Empty)
                {
                    ErrMsg += "請填入程式代碼 !!" + " \\n";
                }
                if (PgUrl == string.Empty)
                {
                    ErrMsg += "請填入程式路徑 !!" + " \\n";
                }

                if (ErrMsg == string.Empty)
                {
                    blInsF = Fn.InsFunList("3PL", FunOrd, FunId, FunName, PgOrd, PgId, PgNm, PgUrl, UserId);
                    if (blInsF)
                    {
                        ((_3PLMasterPage)Master).ShowMessage("新增成功");
                        return;
                    }
                    else
                    {
                        ((_3PLMasterPage)Master).ShowMessage("新增失敗");
                    }
                }
                else
                {
                    ((_3PLMasterPage)Master).ShowMessage(ErrMsg);
                    return;
                }
            }
            catch
            {
                ((_3PLMasterPage)Master).ShowMessage("新增失敗");
            }
        }

        /// <summary>
        /// 產生控制項
        /// </summary>
        private void CrtCtrl(DropDownList DL, string Meth)
        {
            DataSet ds = new DataSet();
            ControlBind CB = new ControlBind();
            RoleInf RI = new RoleInf();
            try
            {
                ds = RI.dsFunMain("3PL");
                dtFunList = ds.Tables[0];
                Session["FunList"] = dtFunList;
                if (Meth == "0")
                {
                    CB.DropDownListBind(ref DL, dtFunList, "FunId", "FunNm", "請選擇", string.Empty);
                }
                else if (Meth == "1")
                {
                    CB.DropDownListBind(ref DL, dtFunList, "Ord", "FunNm", "請選擇", string.Empty);
                }
            }
            catch
            {
                ((_3PLMasterPage)Master).ShowMessage("系統異常，請洽資訊部");
            }
        }

        /// <summary>
        /// GridView建置
        /// </summary>
        /// <param name="dt"></param>
        private void BindGV(DataTable dt)
        {
            gv_List.DataSource = dt;
            gv_List.DataBind();
        }

        /// <summary>
        /// 刪除功能程式
        /// </summary>
        /// <param name="arrFun"></param>
        /// <param name="arrPg"></param>
        private void DelFun(ArrayList arrFun, ArrayList arrPg)
        {
            int intFunCount = arrFun.Count;
            RoleInf RI = new RoleInf();
            for (int i = 0; i < intFunCount; i++)
            {
                string FunId = arrFun[i].ToString();
                string PgId = arrPg[i].ToString();
                RI.DelFunList("3PL", FunId, PgId);
            }
        }

        /// <summary>
        /// 檢核功能程式順序
        /// </summary>
        /// <param name="FunId">功能ID</param>
        /// <param name="PgId">程式ID</param>
        /// <param name="PgOrd">程式順序</param>
        /// <returns></returns>
        private bool blCkOrd(string FunId, string PgId, string PgOrd)
        {
            bool blCk = true;
            RoleInf RI = new RoleInf();
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            //ds = RI.dsFunList("3PL", FunId, string.Empty, string.Empty, PgOrd);
            ds = RI.dsCKFunOrd("3PL", FunId, PgId, PgOrd);
            dt = ds.Tables[0];
            int intCount = dt.Rows.Count;
            if (intCount == 0)//順序無重複
            {
                blCk = false;
            }
            else
            {
                blCk = true;
            }
            return blCk;
        }

        /// <summary>
        /// 修改主功能清單
        /// </summary>
        /// <param name="FgId">程式ID</param>
        /// <param name="FunId">功能ID</param>
        /// <param name="PgOrd">程式順序</param>
        /// <param name="PgName">程式名稱</param>
        /// <param name="PgUrl">程式路徑</param>
        /// <returns></returns>
        private bool blUpMenu(string FgId, string FunId, string PgOrd, string PgName, string PgUrl)
        {
            bool blMenu = false;
            RoleInf RI = new RoleInf();
            string User = UI.UserID;
            try
            {
                blMenu = RI.UpFunList("3PL", FgId, FunId, PgOrd, PgName, PgUrl, User);
            }
            catch
            {
                blMenu = false;
            }
            return blMenu;
        }

        private void ShowBtn(bool blShow)
        {
            btn_Update.Visible = blShow;
            btn_Del.Visible = blShow;
        }

    }
}
