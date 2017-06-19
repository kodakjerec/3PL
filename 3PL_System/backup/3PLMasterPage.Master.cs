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
        }

        #region messageURL
        public void btn_Close_div_URL_Click(object sender, EventArgs e)
        {
            div_URL.Style.Add("display", "none");
            UpdPanel_Message.Update();
        }
        //Open URL
        public void ShowURL(string Path)
        {
            div_URL.Style.Add("display", "inline");
            frame_MessageBox.Attributes["src"] = Path;
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
                div_Message.Style.Add("display", "none");
            }
            UpdPanel_Message.Update();
        }
        //Open Message
        public void ShowMessage(string Path)
        {
            div_Message.Style.Add("display", "inline");
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
        #endregion
    }
}