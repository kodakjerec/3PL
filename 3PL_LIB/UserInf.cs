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

        //身分類別
        private string _ClassId;
        public string ClassId
        {
            get { return _ClassId; }
            set { _ClassId = value; }
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
        private DataTable _DCList;
        public DataTable DCList
        {
            get { return _DCList; }
            set { _DCList = value; }
        }
    }
}
