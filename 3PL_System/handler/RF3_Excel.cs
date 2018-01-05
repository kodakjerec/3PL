using System;
using System.Drawing;
using System.IO;
using System.Text;
using System.Web;
using ExcelTool;
using System.Data;
using _3PL_LIB;
using System.Collections;

namespace handler
{
    public class RF3_Excel
    {
        HttpServerUtility server = HttpContext.Current.Server;
        CrExcel crExcel = new CrExcel();
        DB_IO dbIO = new DB_IO();

        public void Controller(HttpContext context)
        {
            string FileName = "";

            Hashtable ht = new Hashtable();
            Hashtable ht1 = new Hashtable();

            foreach (string key in context.Request.QueryString)
            {
                if (key.IndexOf("@") >= 0)
                    ht.Add(key, context.Request.QueryString[key]);
                if (key == "FileName")
                    FileName = context.Request.QueryString[key];
            }

            if (FileName == string.Empty)
            {
                context.Response.ContentType = "application/json";
                context.Response.Charset = "utf-8";
                context.Response.Write("[{}]");
                return;
            }

            DataSet ds = dbIO.SqlSp(context.Request.QueryString["server"], context.Request.QueryString["sqlcmd"], ht, ref ht1);
            if (ds.Tables.Count > 0)
            {
                string strDate = string.Format("{0:yyyyMMdd}", DateTime.Now);
                string excelFileName = strDate + "_" + FileName + ".xls";
                string ErrMsg = "";
                MemoryStream ms = new MemoryStream();
                bool isSuccess= crExcel.CrWebExForAjax(ds.Tables[0], excelFileName, excelFileName, ref ErrMsg, ref ms);
                if (isSuccess)
                {
                    System.Web.HttpResponse response = System.Web.HttpContext.Current.Response;

                    System.Web.HttpContext.Current.Response.ContentType = "application/vnd.ms-excel";
                    System.Web.HttpContext.Current.Response.AddHeader("Content-Disposition", string.Format("attachment;filename={0}", excelFileName));
                    System.Web.HttpContext.Current.Response.BinaryWrite(ms.GetBuffer());
                    System.Web.HttpContext.Current.Response.End();
                }
                else {
                    System.Web.HttpContext.Current.Response.ContentType = "text/html";
                    //System.Web.HttpContext.Current.Response.AddHeader("content-disposition", "attachment;filename=" + System.Web.HttpContext.Current.Server.UrlEncode(excelFileName));
                    System.Web.HttpContext.Current.Response.Write(ErrMsg);
                    System.Web.HttpContext.Current.Response.End();
                }
            }
        }
    }
}