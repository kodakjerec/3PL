using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.OleDb;

namespace ExcelTool
{
    public class RdExcel
    {
        /// <summary>
        /// 讀取Excel
        /// </summary>
        /// <param name="strPath">Excel路徑</param>
        /// <param name="strSheet">Sheet名稱</param>
        ///  <param name="strHDR">有無抬頭 有:Yes 無:No </param>
        ///  <param name="SheetOrd">Sheet順序</param>
        /// <param name="ErrMsg">錯誤信息</param>
        /// <returns></returns>
        public DataTable dtExcel(string strPath, string strSheet,string strHDR,int intSheetOrd,ref string ErrMsg)
        {
            DataTable dt = new DataTable();
            string SheetName = string.Empty;
            intSheetOrd--;
            List<string> sheetNameList = new List<string>();
            DataSet ds = new DataSet();
            string strConn = "Provider=Microsoft.Ace.OleDb.12.0;" + "data source=" + strPath
              + ";Extended Properties='Excel 12.0;HDR=" + strHDR + "; IMEX=1'; "; //HDR=NO; HDR:判別第一列是否為抬頭，預設為Yes

            OleDbConnection dataConn = new OleDbConnection(strConn);
            try
            {
                dataConn.Open();
                //*********get sheet name*********
                DataRow[] sheetList = dataConn.GetSchema("Tables").Select();
                foreach (DataRow sheet in sheetList)
                {
                    /// query each sheet name from excel file
                    sheetNameList.Add(sheet["TABLE_NAME"] as string);
                }
                //*******************************
                if (strSheet.Length > 0) //提共sheet name
                {
                    strSheet += "$";
                    if (sheetNameList.Contains(strSheet))//sheet name存在與否
                    {
                        SheetName = strSheet;
                    }
                    else 
                    {
                        if (intSheetOrd > sheetNameList.Count - 1 || intSheetOrd<0)//Order有無大於頁籤 Index或小於0
                        {
                            intSheetOrd = 0;
                        }
                        SheetName = sheetNameList[intSheetOrd].ToString();
                    }
                }
                else 
                {
                    if (intSheetOrd > sheetNameList.Count - 1 || intSheetOrd < 0)//Order有無大於頁籤Index或小於0
                    {
                        intSheetOrd = 0;
                    }
                    SheetName = sheetNameList[intSheetOrd].ToString();
                }
                string strComm = "SELECT * FROM ["+SheetName+"]";
                OleDbDataAdapter ta = new OleDbDataAdapter(strComm, dataConn);
                ta.Fill(ds);
                dt = ds.Tables[0];
            }
            catch (Exception ex)
            {
                ErrMsg = ex.Message.ToString();
            }
            finally
            {
                dataConn.Close();
                dataConn.Dispose();
            }
            return dt;
        }
    }
}
