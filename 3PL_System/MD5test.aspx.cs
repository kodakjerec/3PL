using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MD5;
using _3PL_LIB;

namespace _3PL_System
{
    public partial class MD5test : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            string beftxt = TextBox1.Text;
            string afttxt = MD5.MD5Crypt.Encrypt(beftxt, "pxmart", true);
            TextBox2.Text = afttxt;
        }

        protected void Button2_Click(object sender, EventArgs e)
        {
            string beftxt = TextBox3.Text;
            string afttxt = MD5.MD5Crypt.Decrypt(beftxt, "pxmart", true);
            TextBox4.Text = afttxt;
        }

        protected void Button3_Click(object sender, EventArgs e)
        {
            string beftxt = TextBox5.Text;
            ACE_EnCode ace = new ACE_EnCode();
            string afttxt = ace.AESDc(beftxt, "SCSystem");
            TextBox6.Text = afttxt;
        }

        protected void Button4_Click(object sender, EventArgs e)
        {
            string beftxt = TextBox5.Text;
            ACE_EnCode ace = new ACE_EnCode();
            string afttxt = ace.AESEn(beftxt, "SCSystem");
            TextBox8.Text = afttxt;
        }
    }
}