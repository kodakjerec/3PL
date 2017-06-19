using _3PL_DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace _3PL_System.handler
{
    /// <summary>
    /// Handler_Log 的摘要描述
    /// </summary>
    public class Handler_Log : IHttpHandler
    {
        _3PL_CommonQuery _3PLCQ = new _3PL_CommonQuery();

        public void ProcessRequest(HttpContext context)
        {
            //網址
            string Mode = context.Request.QueryString["mode"] ?? string.Empty; //傳入的參數

            //data
            string[] paramlist = context.Request.Form.AllKeys;
            string UserID = context.Request.Form["UserID"];
            string MenuID = context.Request.Form["MenuID"].Replace("menu_","");
            string MenuName = context.Request.Form["MenuName"];
            string Active = context.Request.Form["Active"];

            _3PLCQ.LOG_Insert("3PL", UserID, MenuID, MenuName, Active);

            //context.Response.ContentType = "text/plain";
            //context.Response.Write("Hello World");
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