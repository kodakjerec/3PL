using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace _3PL_LIB
{
    public class Check
    {
        /// <summary>
        /// 檢查起訖日
        /// </summary>
        /// <returns></returns>
        public bool strSEDate(string sDate, string eDate)
        {
            if (sDate == "" || eDate == "")
                return false;
            DateTime S = Convert.ToDateTime(sDate);
            DateTime E = Convert.ToDateTime(eDate);
            if (S > E)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// 檢核日期格式
        /// </summary>
        /// <param name="strDate"></param>
        /// <returns></returns>
        public bool CkDate(string strDate)
        {
            DateTime t = DateTime.Now;
            return DateTime.TryParse(strDate, out t);
        }

        /// <summary>
        /// 檢核數字格式
        /// </summary>
        /// <param name="Num"></param>
        /// <returns></returns>
        public bool CkNum(string Num)
        {
            int i = 0;
            return int.TryParse(Num, out i);
        }

        /// <summary>
        /// 檢核數字格式(Double)
        /// </summary>
        /// <param name="Num"></param>
        /// <returns>True=合法</returns>
        public bool CkDecimal(string Num)
        {
            decimal i = 0;
            return decimal.TryParse(Num, out i);
        }

    }
}
