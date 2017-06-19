using System;
using System.Web.UI;

namespace _3PL_System
{
    public partial class _3PLMasterPage : System.Web.UI.MasterPage
    {
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            Page.Header.DataBind();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                div_URL.Attributes["onclick"] = this.Page.ClientScript.GetPostBackEventReference(btn_Close_div_URL, "");
                div_Message1.Attributes["onclick"] = this.Page.ClientScript.GetPostBackEventReference(btn_Close_div_Message, "");
                div_Message2_left.Attributes["onclick"] = this.Page.ClientScript.GetPostBackEventReference(btn_Close_div_Message, "");
                div_Message2_right.Attributes["onclick"] = this.Page.ClientScript.GetPostBackEventReference(btn_Close_div_Message, "");
                div_Message3.Attributes["onclick"] = this.Page.ClientScript.GetPostBackEventReference(btn_Close_div_Message, "");
            }
        }

        #region messageURL
        public void btn_Close_div_URL_Click(object sender, EventArgs e)
        {
            CloseURLBox();
        }
        //Open URL
        public void ShowURL(string Path)
        {
            div_URL.Style.Add("display", "inline");
            frame_MessageBox.Attributes["src"] = Path;
            UpdPanel_Message.Update();
        }
        //Close URL
        public void CloseURLBox()
        {
            div_URL.Style.Add("display", "none");
            UpdPanel_Message.Update();
        }
        #endregion

        #region message_Message
        public void btn_Close_div_Message_Click(object sender, EventArgs e)
        {
            if (hid_RedirectURL.Value != "")
            {
                Response.Redirect(hid_RedirectURL.Value);
            }
            else
            {
                CloseMessageBox();
            }
        }
        //Open Message
        public void ShowMessage(string Path)
        {
            div_Message.Style.Add("display","inline");
            lbl_Message.Text = Path.Replace(@"\n","<br/>");
            hid_RedirectURL.Value = "";
            UpdPanel_Message.Update();
        }
        public void ShowMessage(string Path,string URL)
        {
            div_Message.Style.Add("display", "inline");
            lbl_Message.Text = Path.Replace(@"\n", "<br/>");
            hid_RedirectURL.Value = URL;
            UpdPanel_Message.Update();
        }
        //Close Message
        public void CloseMessageBox()
        {
            div_Message.Style.Add("display", "none");
            UpdPanel_Message.Update();
        }
        #endregion
    }
}