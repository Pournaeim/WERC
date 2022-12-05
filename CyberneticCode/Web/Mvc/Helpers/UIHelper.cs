using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Drawing;
using System.IO;
using System.Text.RegularExpressions;
using System.Web;

namespace CyberneticCode.Web.Mvc.Helpers
{

    public static class UIHelper
    {
        #region file handeling
        public static string UploadFile(HttpPostedFileBase file, string path)
        {
          

            var newFilePrefix = "";
            var fileUrl = string.Empty;
            var serverPath = string.Empty;

            var date = DateTime.Now;
            newFilePrefix = date.Year.ToString("D4") + date.Month.ToString("D2") + date.Day.ToString("D2") + date.Hour.ToString("D2") + date.Minute.ToString("D2") + date.Second.ToString("D2");
            var fileName = newFilePrefix + file.FileName.Replace("-", "_").Replace("+", "_").Replace("+", "_");
            if (file != null && file.ContentLength > 0)
            {

                serverPath = Path.Combine(HttpContext.Current.Server.MapPath(path), Path.GetFileName(fileName));

                Directory.CreateDirectory(HttpContext.Current.Server.MapPath(path));
                file.SaveAs(serverPath);

                fileUrl = (Path.Combine(path, fileName + "?CT=" + file.ContentType.Replace("/", "_").Replace("-", "_")) + ".png").Replace("\\", "/");

            }
            return fileUrl;
        }
        public static string UploadPictureFile(Bitmap file, string pictureFileName, int contentLength, string contentType, string path)
        {
             
            var newFilePrefix = "";
            var fileUrl = string.Empty;
            var serverPath = string.Empty;

            var date = DateTime.Now;
            newFilePrefix = date.Year.ToString("D4") + date.Month.ToString("D2") + date.Day.ToString("D2") + date.Hour.ToString("D2") + date.Minute.ToString("D2") + date.Second.ToString("D2");
            var fileName = newFilePrefix + pictureFileName.Replace("-", "_").Replace("+", "_").Replace("+", "_");
            if (file != null && contentLength > 0)
            {

                serverPath = Path.Combine(HttpContext.Current.Server.MapPath(path), Path.GetFileName(fileName));

                Directory.CreateDirectory(HttpContext.Current.Server.MapPath(path));
                file.Save(serverPath);

                fileUrl = (Path.Combine(path, fileName + "?CT=" + contentType.Replace("/", "_").Replace("-", "_")) + ".png").Replace("\\", "/");
            }
            return fileUrl;
        }

        public static void DeleteFile(string path)
        {
            try
            {
                var serverPath = HttpContext.Current.Server.MapPath(path.Split(new char[] { '?' })[0]);

                File.Delete(serverPath);

            }
            catch (Exception ex)
            {

            }
        }
        #endregion file handeling     

        #region PDF file handeling         
        public static void MergePDF(string[] fileArray, string outputPdfPath)
        {

            PdfReader reader = null;
            Document sourceDocument = null;
            PdfCopy pdfCopyProvider = null;
            PdfImportedPage importedPage;

            sourceDocument = new Document();
            pdfCopyProvider = new PdfCopy(sourceDocument, new System.IO.FileStream(outputPdfPath, System.IO.FileMode.Create));

            //output file Open  
            sourceDocument.Open();


            //files list wise Loop  
            for (int f = 0; f < fileArray.Length; f++)
            {
                int pages = TotalPageCount(fileArray[f]);

                reader = new PdfReader(fileArray[f]);
                //Add pages in new file  
                for (int i = 1; i <= pages; i++)
                {
                    importedPage = pdfCopyProvider.GetImportedPage(reader, i);
                    pdfCopyProvider.AddPage(importedPage);
                }

                reader.Close();
            }
            //save the output file  
            sourceDocument.Close();
        }

        private static int TotalPageCount(string file)
        {
            using (StreamReader sr = new StreamReader(System.IO.File.OpenRead(file)))
            {
                Regex regex = new Regex(@"/Type\s*/Page[^s]");
                MatchCollection matches = regex.Matches(sr.ReadToEnd());

                return matches.Count;
            }
        }
        #endregion PDF file handeling         

        #region Calculation
        public static double GetAspectRatio(double x, double y)
        {
            if (x > y)
            {
                return x / y;
            }
            else if (y > x)
            {
                return y / x;
            }
            else
                return 1.0;
        }
        #endregion Calculation 

    }
}