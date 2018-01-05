using System;
using System.Drawing;
using System.IO;
using System.Text;
using System.Web;

namespace handler
{
    public class RF3_PhotoControl
    {
        HttpServerUtility server = HttpContext.Current.Server;
        string savePath = "";

        public void Controller(HttpContext context)
        {
            savePath = server.MapPath("~/RFUploadPictures/");
            string cmdType = context.Request.Form["sqlcmd"] ?? string.Empty; //傳入的參數

            switch (cmdType)
            {
                case "upload":
                    upload(context);
                    break;
                case "search":
                    search(context);
                    break;
                case "open":
                    open(context);
                    break;
                case "delete":
                    delete(context);
                    break;
                case "download":
                    download(context);
                    break;
                default:
                    context.Response.ContentType = "application/json";
                    context.Response.Charset = "utf-8";
                    context.Response.Write("[{}]");
                    break;
            }
        }

        #region upload
        private void upload(HttpContext context)
        {
            int RT_CODE = 0;
            string RT_MSG = "";

            // Specify the path to save the uploaded file to.

            string ImageFileName = savePath + context.Request.Form["@FileName"] + ".png";
            string ImageFileName_small = savePath + context.Request.Form["@FileName"] + "s.png";
            string TxtFileName = savePath + context.Request.Form["@FileName"] + ".txt";
            string FileSource = context.Request.Form["@FileSource"];
            string FileDescription = context.Request.Form["@FileDescription"];
            try
            {
                //影像存檔
                Image obj = Base64ToImage(FileSource);
                obj.Save(ImageFileName);

                //影像縮圖存檔
                Image obj_small = ToThumbnailImage(obj);
                obj_small.Save(ImageFileName_small);

                //註解存檔
                using (StreamWriter sw_OutPutTXT = new StreamWriter(TxtFileName, false, System.Text.Encoding.UTF8))
                {
                    sw_OutPutTXT.Write(FileDescription);
                }
            }
            catch (Exception e)
            {
                RT_CODE = 1;
                RT_MSG = e.Message;
            }

            string result_sp = "[{\"RT_CODE\":" + RT_CODE.ToString() + ",\"RT_MSG\":\"" + RT_MSG + "\"}]";

            context.Response.ContentType = "application/json";
            context.Response.Charset = "utf-8";
            context.Response.Write(result_sp);
        }

        /// <summary>
        /// Base64 To Image
        /// </summary>
        /// <param name="base64String"></param>
        /// <returns></returns>
        private Image Base64ToImage(string base64String)
        {
            //由於QueryString中Base64的"+"會被解讀成空白符號，故用函數還原
            base64String = base64String.Replace(" ", "+");

            // Convert Base64 String to byte[]
            byte[] imageBytes = Convert.FromBase64String(base64String);
            MemoryStream ms = new MemoryStream(imageBytes, 0,
              imageBytes.Length);

            // Convert byte[] to Image
            ms.Write(imageBytes, 0, imageBytes.Length);
            Image image = Image.FromStream(ms, true);
            return image;
        }

        /// <summary>
        /// 產生縮圖
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        private Image ToThumbnailImage(Image source)
        {
            RectangleF destinationBounds = new RectangleF(0, 0, 240, 180);
            RectangleF sourceBounds = new RectangleF(0.0f, 0.0f, (float)source.Width, (float)source.Height);

            Image destinationImage = new Bitmap((int)destinationBounds.Width, (int)destinationBounds.Height);
            Graphics graph = Graphics.FromImage(destinationImage);
            graph.InterpolationMode =
                System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;

            // Fill with background color
            graph.FillRectangle(new SolidBrush(System.Drawing.Color.Transparent), destinationBounds);

            float resizeRatio, sourceRatio;
            float scaleWidth, scaleHeight;

            sourceRatio = (float)source.Width / (float)source.Height;

            if (sourceRatio >= 1.0f)
            {
                //landscape
                resizeRatio = destinationBounds.Width / sourceBounds.Width;
                scaleWidth = destinationBounds.Width;
                scaleHeight = sourceBounds.Height * resizeRatio;
                float trimValue = destinationBounds.Height - scaleHeight;
                graph.DrawImage(source, 0, (trimValue / 2), destinationBounds.Width, scaleHeight);
            }
            else
            {
                //portrait
                resizeRatio = destinationBounds.Height / sourceBounds.Height;
                scaleWidth = sourceBounds.Width * resizeRatio;
                scaleHeight = destinationBounds.Height;
                float trimValue = destinationBounds.Width - scaleWidth;
                graph.DrawImage(source, (trimValue / 2), 0, scaleWidth, destinationBounds.Height);
            }

            return destinationImage;
        }
        #endregion

        #region search
        private void search(HttpContext context)
        {
            string returnJSON = "";
            string[] fileList = Directory.GetFiles(savePath, "*.txt");
            string filename = "",
                    filename_Description = "",
                    filename_ThumbnailImage = "",
                    filename_Image = "";
            Boolean IamFirst = true;
            returnJSON += "[";

            foreach (string file in fileList)
            {

                if (IamFirst)
                {
                    IamFirst = false;
                }
                else
                {
                    returnJSON += ",";
                }
                //檔名
                filename = Path.GetFileName(savePath + file).Replace(".txt", "");
                returnJSON += "{ \"FileName\": \"" + filename + "\"";
                filename_Description = savePath + filename + ".txt";
                filename_ThumbnailImage = savePath + filename + "s.png";
                filename_Image = savePath + filename + ".png";

                //描述內容
                string sDescription = File.ReadAllText(filename_Description);
                sDescription = sDescription.Replace(Environment.NewLine, "<br>")
                    .Replace("\n", "<br>")
                    .Replace("\r", "<br>");
                returnJSON += ",\"Description\":\"" + sDescription + "\"";

                //縮圖
                byte[] bytes = File.ReadAllBytes(filename_ThumbnailImage);
                string sBase64ThumbnailImage = Convert.ToBase64String(bytes);

                returnJSON += ",\"ThumbnailImage\":\"" + sBase64ThumbnailImage + "\"";

                //檔案時間
                returnJSON += ",\"CreateTime\":\"" + File.GetCreationTime(filename_Image).ToString("yyyyMMddHHmmss") + "\"";

                //結尾
                returnJSON += "}";
            }

            returnJSON += "]";

            context.Response.ContentType = "application/json";
            context.Response.Charset = "utf-8";
            context.Response.Write(returnJSON);
        }
        #endregion

        #region open
        private void open(HttpContext context)
        {
            string filename = context.Request.Form["@FileName"],
                   filename_Image = "";

            filename_Image = savePath + filename + ".png";

            //image轉成base64
            byte[] responsebytes = File.ReadAllBytes(filename_Image);
            string responseBase64 = Convert.ToBase64String(responsebytes);

            context.Response.Write(responseBase64);
            context.Response.ContentType = "image/png";
            context.Response.End();
        }
        #endregion

        #region delete
        private void delete(HttpContext context)
        {
            string filenames = context.Request.Form["@FileName"];
            string[] filenameList = filenames.Split(',');
            string filename_Description = "",
                    filename_ThumbnailImage = "",
                    filename_Image = "";
            foreach (string filename in filenameList)
            {
                filename_Description = savePath + filename + ".txt";
                filename_ThumbnailImage = savePath + filename + "s.png";
                filename_Image = savePath + filename + ".png";

                try
                {
                    File.Delete(filename_Description);
                    File.Delete(filename_ThumbnailImage);
                    File.Delete(filename_Image);
                }
                catch { }
            }

            context.Response.Write("[{}]");
            context.Response.ContentType = "image/png";
            context.Response.End();
        }
        #endregion

        #region download
        private void download(HttpContext context)
        {
            string filename = context.Request.Form["@FileName"],
                   filename_Image = "";

            filename_Image = savePath + filename + ".png";

            //image轉成base64
            byte[] responsebytes = File.ReadAllBytes(filename_Image);
            string responseBase64 = Convert.ToBase64String(responsebytes);

            context.Response.Write(responseBase64);
            context.Response.ContentType = "image/png";
            context.Response.End();
        }
        #endregion
    }
}