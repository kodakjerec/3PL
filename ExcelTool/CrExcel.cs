using System;
using System.Data;
using System.IO;
using System.Windows.Forms;
using NPOI.HSSF.UserModel;
using System.Text;
using NPOI.SS.UserModel;

namespace ExcelTool
{
    public class CrExcel
    {
        /// <summary>
        /// WinForm 有抬頭
        /// </summary>
        /// <param name="dt">DataTable</param>
        /// <param name="SheetName">頁籤名稱</param>
        /// <param name="tName">抬頭名稱</param>
        public bool CrWinEx(DataTable dt, string SheetName, string tName, ref string path)
        {
            bool BooSec = false;
            SaveFileDialog SFD = new SaveFileDialog();
            HSSFWorkbook workbook = new HSSFWorkbook();
            //MemoryStream ms = new MemoryStream();//參考 System.IO
            //SFD.DefaultExt = Name+".xls";
            //SFD.Filter = "Excel檔(*.xls) |" + Name + ".xls "; //存檔類型
            SFD.Filter = "Excel檔(*.xls) |*.xls "; //存檔類型
            SFD.AddExtension = true;
            try
            {
                if (SFD.ShowDialog() == DialogResult.OK)
                {
                    string filename = SFD.FileName;
                    path = filename;
                    HSSFSheet sheet = (HSSFSheet)workbook.CreateSheet(SheetName);
                    HSSFCellStyle cs = (HSSFCellStyle)workbook.CreateCellStyle();
                    //啟動多行文字
                    cs.WrapText = true;

                    //合併儲存格
                    sheet.AddMergedRegion(new NPOI.SS.Util.Region(0, 0, 0, dt.Columns.Count - 1));

                    //文字置中 標題
                    cs.VerticalAlignment = NPOI.SS.UserModel.VerticalAlignment.CENTER;
                    cs.Alignment = NPOI.SS.UserModel.HorizontalAlignment.CENTER;
                    cs.DataFormat = HSSFDataFormat.GetBuiltinFormat("@");//儲存格格式:文字
                    //IFont Ft = workbook.CreateFont();
                    //Ft.FontHeight = 20 * 20;
                    HSSFFont Ft = (HSSFFont)workbook.CreateFont();
                    Ft.FontHeightInPoints = 18;
                    cs.SetFont(Ft);
                    sheet.CreateRow(0).CreateCell(0).CellStyle = cs;
                    sheet.GetRow(0).GetCell(0).SetCellValue(tName);
                    //HSSFRow Row = (HSSFRow)sheet.CreateRow(0);
                    //Row.GetCell(0).SetCellValue("測試文字");

                    //字型字體 表頭
                    HSSFFont font = (HSSFFont)workbook.CreateFont();
                    HSSFCellStyle Style = (HSSFCellStyle)workbook.CreateCellStyle();
                    Style.VerticalAlignment = NPOI.SS.UserModel.VerticalAlignment.CENTER;
                    Style.Alignment = NPOI.SS.UserModel.HorizontalAlignment.CENTER;
                    font.FontHeightInPoints = 12;//字型大小
                    //font.FontName = "標楷體";//字體
                    //font.Color = NPOI.HSSF.Util.HSSFColor.DARK_RED.index;//字體顏色
                    Style.DataFormat = HSSFDataFormat.GetBuiltinFormat("@");//儲存格格式:文字
                    Style.SetFont(font);

                    //取欄位名稱
                    sheet.CreateRow(1);//1.2.5版 CreateRow(1).CreateCell(i)會把前面資料洗掉
                    for (int i = 0; i < dt.Columns.Count; i++)
                    {
                        string strValue = dt.Columns[i].ColumnName == null ? "" : dt.Columns[i].ColumnName.ToString();
                        sheet.GetRow(1).CreateCell(i).CellStyle = Style;
                        sheet.GetRow(1).GetCell(i).SetCellValue(strValue);
                    }

                    //取值
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        sheet.CreateRow(i + 2);
                        for (int j = 0; j < dt.Columns.Count; j++)
                        {
                            string strValue = dt.Rows[i][j] == null ? "" : dt.Rows[i][j].ToString();
                            sheet.GetRow(i + 2).CreateCell(j).CellStyle = Style;
                            sheet.GetRow(i + 2).GetCell(j).SetCellValue(strValue);
                        }
                    }

                    //Response.AddHeader("Content-Disposition", string.Format("attachment; filename=TestExcel.xls"));
                    using (FileStream fs = new FileStream(filename, FileMode.OpenOrCreate))
                    {
                        workbook.Write(fs);
                    }
                }
                //workbook.Write(ms);

                BooSec = true;
            }
            catch
            {
                BooSec = false;
            }
            workbook = null;
            return BooSec;
        }

        /// <summary>
        /// WinFrom 無抬頭
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="SheetName"></param>
        public bool CrWinEx(DataTable dt, string SheetName)
        {
            SaveFileDialog SFD = new SaveFileDialog();
            HSSFWorkbook workbook = new HSSFWorkbook();
            //MemoryStream ms = new MemoryStream();
            //SFD.DefaultExt = Name + ".xls";
            //SFD.Filter = "Excel檔(*.xls) |" + Name + ".xls "; //存檔類型
            SFD.Filter = "Excel檔(*.xls) |*.xls "; //存檔類型
            SFD.AddExtension = true;
            bool BooSec = false;
            try
            {
                if (SFD.ShowDialog() == DialogResult.OK)
                {
                    string filename = SFD.FileName;
                    HSSFSheet sheet = (HSSFSheet)workbook.CreateSheet(SheetName);

                    //字型字體 表頭
                    HSSFFont font = (HSSFFont)workbook.CreateFont();
                    HSSFCellStyle Style = (HSSFCellStyle)workbook.CreateCellStyle();
                    Style.VerticalAlignment = NPOI.SS.UserModel.VerticalAlignment.CENTER;
                    Style.Alignment = NPOI.SS.UserModel.HorizontalAlignment.CENTER;
                    font.FontHeightInPoints = 12;//字型大小
                    //font.FontName = "標楷體";//字體
                    //font.Color = NPOI.HSSF.Util.HSSFColor.DARK_RED.index;//字體顏色
                    Style.SetFont(font);

                    //取欄位名稱
                    sheet.CreateRow(0);//1.2.5版 CreateRow(1).CreateCell(i)會把前面資料洗掉
                    for (int i = 0; i < dt.Columns.Count; i++)
                    {
                        string strValue = dt.Columns[i].ColumnName == null ? "" : dt.Columns[i].ColumnName.ToString();
                        sheet.GetRow(0).CreateCell(i).CellStyle = Style;
                        sheet.GetRow(0).GetCell(i).SetCellValue(strValue);
                    }

                    //取值
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        sheet.CreateRow(i + 1);
                        for (int j = 0; j < dt.Columns.Count; j++)
                        {
                            string strValue = dt.Rows[i][j] == null ? "" : dt.Rows[i][j].ToString();
                            sheet.GetRow(i + 1).CreateCell(j).CellStyle = Style;
                            sheet.GetRow(i + 1).GetCell(j).SetCellValue(strValue);
                        }
                    }

                    using (FileStream fs = new FileStream(filename, FileMode.OpenOrCreate))
                    {
                        workbook.Write(fs);
                    }
                }
                BooSec = true;
            }
            catch
            {
                BooSec = false;
            }
            workbook = null;
            return BooSec;
        }

        /// <summary>
        /// Web 有抬頭
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="SheetName"></param>
        /// <param name="Name"></param>
        /// <param name="tName"></param>
        public bool CrWebEx(DataTable dt, string SheetName, string Name, string tName)
        {
            HSSFWorkbook workbook = new HSSFWorkbook();
            MemoryStream ms = new MemoryStream();//參考 System.IO
            bool BooSec = false;
            try
            {
                HSSFSheet sheet = (HSSFSheet)workbook.CreateSheet(SheetName);

                HSSFCellStyle cs = (HSSFCellStyle)workbook.CreateCellStyle();
                //啟動多行文字
                cs.WrapText = true;

                //合併儲存格
                sheet.AddMergedRegion(new NPOI.SS.Util.Region(0, 0, 0, dt.Columns.Count - 1));

                //文字置中
                cs.VerticalAlignment = NPOI.SS.UserModel.VerticalAlignment.CENTER;
                cs.Alignment = NPOI.SS.UserModel.HorizontalAlignment.CENTER;
                HSSFFont Ft = (HSSFFont)workbook.CreateFont();
                Ft.FontHeightInPoints = 18;
                cs.SetFont(Ft);
                sheet.CreateRow(0).CreateCell(0).CellStyle = cs;
                sheet.GetRow(0).GetCell(0).SetCellValue(tName);
                //HSSFRow Row = (HSSFRow)sheet.CreateRow(0);
                //Row.GetCell(0).SetCellValue("測試文字");

                //字型字體
                HSSFFont font = (HSSFFont)workbook.CreateFont();
                HSSFCellStyle Style = (HSSFCellStyle)workbook.CreateCellStyle();
                font.FontHeightInPoints = 12;//字型大小
                //font.FontName = "標楷體";//字體
                //font.Color = NPOI.HSSF.Util.HSSFColor.DARK_RED.index;//字體顏色
                Style.SetFont(font);

                //取欄位名稱
                sheet.CreateRow(1);//1.2.5版 CreateRow(1).CreateCell(i)會把前面資料洗掉
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    string strValue = dt.Columns[i].ColumnName == null ? "" : dt.Columns[i].ColumnName.ToString();
                    sheet.GetRow(1).CreateCell(i).CellStyle = Style;
                    sheet.GetRow(1).GetCell(i).SetCellValue(strValue);
                }

                //取值
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    sheet.CreateRow(i + 2);
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        string strValue = dt.Rows[i][j] == null ? "" : dt.Rows[i][j].ToString();
                        sheet.GetRow(i + 2).CreateCell(j).CellStyle = Style;
                        sheet.GetRow(i + 2).GetCell(j).SetCellValue(strValue);
                    }
                }
                workbook.Write(ms);
                //Response.AddHeader("Content-Disposition", string.Format("attachment; filename=TestExcel.xls"));
                string strDate = string.Format("{0:yyyyMMdd}", DateTime.Now);
                string excelFileName = strDate + "_" + Name + ".xls";
                System.Web.HttpContext.Current.Response.AddHeader("content-disposition", "attachment;filename=" + System.Web.HttpContext.Current.Server.UrlEncode(excelFileName));
                System.Web.HttpContext.Current.Response.BinaryWrite(ms.ToArray());
                //ms.Length 檔案大小
                BooSec = true;
            }
            catch
            {
                BooSec = false;
            }

            workbook = null;
            ms.Close();
            ms.Dispose();
            return BooSec;
        }

        /// <summary>
        /// Web 無抬頭
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="SheetName"></param>
        /// <param name="Name"></param>
        public bool CrWebEx(DataTable dt, string SheetName, string Name)
        {
            HSSFWorkbook workbook = new HSSFWorkbook();
            MemoryStream ms = new MemoryStream();//參考 System.IO
            bool BooSec = false;
            try
            {
                HSSFSheet sheet = (HSSFSheet)workbook.CreateSheet(SheetName);

                //放入Excel資料
                PutCellsValue(workbook, dt, sheet);

                //調整欄位寬度
                ChangeColumnWidth(dt, sheet);

                workbook.Write(ms);
                //Response.AddHeader("Content-Disposition", string.Format("attachment; filename=TestExcel.xls"));
                string strDate = string.Format("{0:yyyyMMdd}", DateTime.Now);
                string excelFileName = strDate + "_" + Name + ".xls";
                System.Web.HttpContext.Current.Response.ContentType = "application/octet-stream";
                System.Web.HttpContext.Current.Response.AddHeader("content-disposition", "attachment;filename=" + System.Web.HttpContext.Current.Server.UrlEncode(excelFileName));
                System.Web.HttpContext.Current.Response.BinaryWrite(ms.ToArray());
                System.Web.HttpContext.Current.Response.End();
                //ms.Length 檔案大小
                BooSec = true;
            }
            catch
            {
                BooSec = false;
            }
            workbook = null;
            ms.Close();
            ms.Dispose();
            return BooSec;
        }

        /// <summary>
        /// Web 無抬頭 Ajax下載用
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="SheetName"></param>
        /// <param name="Name"></param>
        public bool CrWebExForAjax(DataTable dt, string SheetName, string Name, ref string ErrMsg, ref MemoryStream ms)
        {
            HSSFWorkbook workbook = new HSSFWorkbook();
            bool BooSec = false;
            try
            {
                HSSFSheet sheet = (HSSFSheet)workbook.CreateSheet(SheetName);

                //放入Excel資料
                PutCellsValue(workbook, dt, sheet);

                //調整欄位寬度
                ChangeColumnWidth(dt, sheet);

                workbook.Write(ms);

                //ms.Length 檔案大小
                BooSec = true;
            }
            catch (Exception ex)
            {
                ErrMsg = ex.Message;
                BooSec = false;
            }
            workbook = null;
            ms.Close();
            ms.Dispose();
            return BooSec;
        }

        /// <summary>
        /// 放入excel資料
        /// </summary>
        /// <param name="workbook"></param>
        /// <param name="dt"></param>
        /// <param name="sheet"></param>
        private void PutCellsValue(HSSFWorkbook workbook, DataTable dt, HSSFSheet sheet)
        {
            string strValue = "";

            //字型字體
            IDataFormat dataFormatCustom = workbook.CreateDataFormat();
            HSSFFont font = (HSSFFont)workbook.CreateFont();
            HSSFCellStyle Style = (HSSFCellStyle)workbook.CreateCellStyle();
            font.FontHeightInPoints = 12;//字型大小
                                         //font.FontName = "標楷體";//字體
                                         //font.Color = NPOI.HSSF.Util.HSSFColor.DARK_RED.index;//字體顏色
            Style.SetFont(font);

            //取欄位名稱
            sheet.CreateRow(0);//1.2.5版 CreateRow(1).CreateCell(i)會把前面資料洗掉
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                strValue = "";
                strValue = dt.Columns[i].ColumnName == null ? "" : dt.Columns[i].ColumnName.ToString();
                sheet.GetRow(0).CreateCell(i).CellStyle = Style;
                sheet.GetRow(0).GetCell(i).SetCellValue(strValue);
                sheet.AutoSizeColumn(i);
            }

            //取值
            string ColumnType = "";
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                sheet.CreateRow(i + 1);
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    strValue = "";
                    strValue = dt.Rows[i][j] == null ? "" : dt.Rows[i][j].ToString();
                    sheet.GetRow(i + 1).CreateCell(j).CellStyle = Style;
                    ColumnType = dt.Rows[i][j].GetType().Name.ToLower();
                    switch (ColumnType)
                    {
                        case "byte":
                            sheet.GetRow(i + 1).GetCell(j).SetCellValue(Convert.ToByte(strValue));
                            break;
                        case "double":
                        case "decimal":
                            sheet.GetRow(i + 1).GetCell(j).SetCellValue(Convert.ToDouble(strValue));
                            break;
                        case "datetime":
                            sheet.GetRow(i + 1).CreateCell(j).CellStyle.DataFormat = dataFormatCustom.GetFormat("yyyy/MM/dd HH:mm:ss");
                            sheet.GetRow(i + 1).CreateCell(j).CellStyle.SetFont(font);
                            sheet.GetRow(i + 1).GetCell(j).SetCellValue(DateTime.Parse(strValue).ToString("yyyy/MM/dd HH:mm:ss"));
                            break;
                        default:
                            sheet.GetRow(i + 1).GetCell(j).SetCellValue(strValue);
                            break;
                    }
                }
            }
        }

        /// <summary>
        /// 調整欄位寬度
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="sheet"></param>
        private void ChangeColumnWidth(DataTable dt, HSSFSheet sheet)
        {
            int columnWidth, TcharLength, Fullcharlength, ChinesecharLength;

            for (int columnNum = 0; columnNum <= dt.Columns.Count; columnNum++)
            {
                columnWidth = sheet.GetColumnWidth(columnNum) / 256;
                for (int rowNum = 1; rowNum <= sheet.LastRowNum; rowNum++)
                {
                    IRow currentRow;
                    //当前行未被使用过  
                    if (sheet.GetRow(rowNum) == null)
                    {
                        currentRow = sheet.CreateRow(rowNum);
                    }
                    else
                    {
                        currentRow = sheet.GetRow(rowNum);
                    }

                    if (currentRow.GetCell(columnNum) != null)
                    {
                        ICell currentCell = currentRow.GetCell(columnNum);
                        //總字元數
                        TcharLength = currentCell.ToString().Length;

                        //字串總字元數
                        Fullcharlength = Encoding.Default.GetBytes(currentCell.ToString()).Length;
                        //純數字太窄, 依照字元數再加寬
                        if (Fullcharlength < TcharLength * 1.6)
                        {
                            Fullcharlength = Convert.ToInt32(TcharLength * 1.6);
                        }

                        //中文雙字元數
                        ChinesecharLength = Fullcharlength - TcharLength;

                        //中文字元要再把欄寬加寬
                        Fullcharlength = Fullcharlength + ChinesecharLength;

                        if (columnWidth < Fullcharlength)
                        {
                            columnWidth = Fullcharlength;
                        }
                    }
                }
                sheet.SetColumnWidth(columnNum, columnWidth * 256);
            }
        }
    }
}
