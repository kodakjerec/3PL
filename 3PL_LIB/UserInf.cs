using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace _3PL_LIB
{
    public class UserInf
    {
        //工號(帳號)
        private string _UserID;
        public string UserID
        {
            get { return _UserID; }
            set { _UserID = value; }
        }

        //人員姓名
        private string _UserName;
        public string UserName
        {
            get { return _UserName; }
            set { _UserName = value; }
        }

        //權限
        private string _RoleId;
        public string RoleId
        {
            get { return _RoleId; }
            set { _RoleId = value; }
        }

        //登入IP
        private string _IP;
        public string IP
        {
            get { return _IP; }
            set { _IP = value; }
        }

        /// <summary>
        /// 登入時間
        /// </summary>
        private DateTime _LoginTime;
        public DateTime LoginTime
        {
            get { return _LoginTime; }
            set { _LoginTime = value; }
        }

        /// <summary>
        /// 倉別
        /// </summary>
        private DataTable _Class;
        public DataTable Class
        {
            get { return _Class; }
            set { _Class = value; }
        }
    }
}
