﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Configuration;
using System.Collections;

namespace _3PL_LIB
{
    public class DB_IO
    {
        /// <summary>
        /// 資料庫連結字串
        /// </summary>
        /// <returns></returns>
        private string strCon(string DB)
        {
            string strConn = string.Empty;
            strConn = ConfigurationManager.AppSettings[DB];
            strConn = MD5.MD5Crypt.Decrypt(strConn, "pxmart", true);
            return strConn;
        }

        /// <summary>
        /// 查詢
        /// </summary>
        /// <param name="strDB">資料庫名稱</param>
        /// <param name="sqlcom">SLQ_Commit</param>
        /// <param name="Prm">參數</param>
        /// <returns></returns>
        public DataSet SqlQuery(string strDB, string sqlcom, Hashtable Prm)
        {
            DataSet ds = new DataSet();
            string str_Conn = strCon(strDB);
            SqlConnection Conn = new SqlConnection(str_Conn);
            try
            {
                SqlCommand com = new SqlCommand(sqlcom, Conn);
                com.CommandTimeout = 0;
                foreach (DictionaryEntry entry in Prm)
                {
                    string strKey = entry.Key.ToString();
                    SqlParameter P = new SqlParameter(strKey, SqlDbType.VarChar);
                    P.Value = entry.Value;
                    com.Parameters.Add(P);
                }
                Conn.Open();
                SqlDataAdapter dapter = new SqlDataAdapter(com);
                dapter.Fill(ds);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Conn.Close();
                Conn.Dispose();
            }
            return ds;
        }

        /// <summary>
        /// 更新/刪除
        /// </summary>
        /// <param name="strDB">資料庫名稱</param>
        /// <param name="sqlcom">SLQ_Commit</param>
        /// <param name="Prm">參數</param>
        /// <param name="intItems">異動筆數</param>
        /// <returns></returns>
        public bool SqlUpdate(string strDB, string sqlcom, Hashtable Prm, ref int intItems)
        {
            string str_Conn = strCon(strDB);
            SqlConnection Conn = new SqlConnection(str_Conn);
            bool booUpdate = false;
            intItems = 0;
            try
            {
                SqlCommand com = new SqlCommand(sqlcom, Conn);
                com.CommandTimeout = 0;
                foreach (DictionaryEntry entry in Prm)
                {
                    string strKey = entry.Key.ToString();
                    SqlParameter P = new SqlParameter(strKey, SqlDbType.VarChar);
                    P.Value = entry.Value;
                    com.Parameters.Add(P);
                }
                Conn.Open();
                intItems = com.ExecuteNonQuery();
                booUpdate = true;
            }
            catch (Exception ex)
            {
                booUpdate = false;
                throw ex;
            }
            finally
            {
                Conn.Close();
                Conn.Dispose();
            }
            return booUpdate;
        }

        /// <summary>
        /// Transaction 更新/刪除
        /// </summary>
        /// <param name="strDB"></param>
        /// <param name="sqlcom"></param>
        /// <param name="Prm"></param>
        /// <returns></returns>
        public bool SqlBeginTran(string strDB, string[] sqlcom, Hashtable[] Prm)
        {
            string str_Conn = strCon(strDB);
            bool booBeginTran = false;
            SqlConnection Conn = new SqlConnection(str_Conn);
            Conn.Open();
            SqlTransaction Trans = Conn.BeginTransaction();
            SqlCommand com = new SqlCommand();
            com.Connection = Conn;
            com.Transaction = Trans;
            com.CommandTimeout = 0;

            try
            {
                for (int i = 0; i < sqlcom.Length; i++)
                {
                    com.CommandText = sqlcom[i];
                    foreach (DictionaryEntry entry in Prm[i])
                    {
                        string strKey = entry.Key.ToString();
                        SqlParameter P = new SqlParameter(strKey, SqlDbType.VarChar);
                        P.Value = entry.Value;
                        com.Parameters.Add(P);
                    }
                    com.ExecuteNonQuery();
                    com.Parameters.Clear();
                }
                Trans.Commit();
                booBeginTran = true;
            }
            catch (Exception ex)
            {
                Trans.Rollback();
                booBeginTran = false;
                throw ex;
            }
            finally 
            {
                Conn.Close();
                Conn.Dispose();
            }
            return booBeginTran;
        }

        /// <summary>
        /// 執行預存程序
        /// </summary>
        /// <param name="strDB">資料庫名稱</param>
        /// <param name="SpName">SP名稱</param>
        /// <param name="Prm">參數</param>
        /// <param name="OutPrm">OutPut參數</param>
        /// <returns></returns>
        public DataSet SqlSp(string strDB, string SpName, Hashtable Prm, ref Hashtable OutPrm)
        {
            DataSet Ds = new DataSet();
            string str_Conn = strCon(strDB);
            SqlConnection Conn = new SqlConnection(str_Conn);
            SqlCommand com = new SqlCommand(SpName, Conn);
            com.CommandTimeout = 0;
            try
            {
                ArrayList arrKey = new ArrayList();
                com.CommandType = CommandType.StoredProcedure;
                foreach (DictionaryEntry entry in Prm)
                {
                    string strKey = entry.Key.ToString();
                    com.Parameters.Add(strKey, SqlDbType.VarChar);
                    com.Parameters[strKey].Value = entry.Value;
                }
                foreach (DictionaryEntry entry in OutPrm)
                {

                    string strKey = entry.Key.ToString();
                    SqlParameter rc = new SqlParameter(strKey, SqlDbType.VarChar, 500);
                    rc.Direction = ParameterDirection.Output;
                    com.Parameters.Add(rc);
                    arrKey.Add(strKey);
                }
                com.Connection.Open();
                SqlDataAdapter dapter = new SqlDataAdapter(com);
                dapter.Fill(Ds);
                if (OutPrm.Count > 0)
                {
                    for (int i = 0; i < arrKey.Count; i++)
                    {
                        int intPrm = Prm.Count + i;
                        string HsKey = arrKey[i] == null ? "" : arrKey[i].ToString();
                        OutPrm[HsKey] = com.Parameters[intPrm].Value == null ? "" : com.Parameters[intPrm].Value;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Conn.Close();
                Conn.Dispose();
            }
            return Ds;
        }
    }
}