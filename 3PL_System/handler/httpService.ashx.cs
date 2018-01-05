using _3PL_LIB;
using System;
using System.Collections;
using System.Data;
using System.Web;

namespace handler
{
    /// <summary>
    /// login 
    /// 呼叫134 DCS 登入帳密驗證SP：spDCS_LOGIN
    /// </summary>
    public class RF3_httpService : IHttpHandler
    {
        DB_IO dbIO = new DB_IO();

        public void ProcessRequest(HttpContext context)
        {
            //傳入的參數, 前端用POST
            string Mode = context.Request.Form["mode"] ?? string.Empty;

            //可能是透過網址傳遞, 前端用GET
            if (Mode == string.Empty)
            {
                Mode = context.Request.QueryString["mode"];
            }

            switch (Mode)
            {
                case "sp":
                    DOsp(context);
                    break;
                case "sqlcmd":
                    DOsqlcmd(context);
                    break;
                case "Picture":
                    RF3_PhotoControl photoControl = new RF3_PhotoControl();
                    photoControl.Controller(context);
                    break;
                case "excel":
                    RF3_Excel excelControl = new RF3_Excel();
                    excelControl.Controller(context);
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Call sp
        /// </summary>
        /// <param name="context"></param>
        private void DOsp(HttpContext context)
        {
            Hashtable ht = new Hashtable();
            Hashtable ht1 = new Hashtable();

            foreach (string key in context.Request.Form.Keys)
            {
                if (key.IndexOf("@") >= 0)
                    ht.Add(key, context.Request.Form[key]);
            }

            DataSet ds = dbIO.SqlSp(context.Request.Form["Server"], context.Request.Form["sqlcmd"], ht, ref ht1);
            string result_sp = "[{}]";
            if (ds.Tables.Count > 0)
                result_sp = JSONconvert.DataTableToJSONstr(ds.Tables[0]);

            context.Response.ContentType = "application/json";
            context.Response.Charset = "utf-8";
            context.Response.Write(result_sp);
        }

        /// <summary>
        /// Call command
        /// </summary>
        /// <param name="context"></param>
        private void DOsqlcmd(HttpContext context)
        {
            Hashtable ht = new Hashtable();
            Hashtable ht1 = new Hashtable();

            foreach (string key in context.Request.Form.Keys)
            {
                if (key.IndexOf("@") >= 0)
                    ht.Add(key, context.Request.Form[key]);
            }

            DataSet ds = dbIO.SqlQuery(context.Request.Form["Server"], context.Request.Form["sqlcmd"], ht);
            string result_sp = "[{}]";
            if (ds.Tables.Count > 0)
                result_sp = JSONconvert.DataTableToJSONstr(ds.Tables[0]);

            context.Response.ContentType = "application/json";
            context.Response.Charset = "utf-8";
            context.Response.Write(result_sp);
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}