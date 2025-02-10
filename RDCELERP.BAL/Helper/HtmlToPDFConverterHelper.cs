using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using SelectPdf;
using SendGrid;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDCELERP.Common.Helper;

namespace RDCELERP.BAL.Helper
{
    public class HtmlToPDFConverterHelper : IHtmlToPDFConverterHelper
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        ILogging _logging;

        public HtmlToPDFConverterHelper(IWebHostEnvironment webHostEnvironment, ILogging logging)
        {
            _webHostEnvironment = webHostEnvironment;
            _logging = logging;
        }

        public bool GeneratePDF(string htmlString, string requestPath, string customFileName)
        {
            bool flag = false;
            string fileName = "";
            var fileNameWithPath = "";
            try
            {
                if (customFileName != null && customFileName != "")
                {
                    fileName = customFileName;
                }
                else
                {
                    fileName = Guid.NewGuid().ToString("N");
                }
                if (requestPath != null)
                {
                    var filePath = string.Concat(_webHostEnvironment.WebRootPath, "\\", requestPath);
                  
                    if (!Directory.Exists(filePath))
                    {
                        Directory.CreateDirectory(filePath); //Create directory if it doesn't exist
                    }
                    fileNameWithPath = string.Concat(filePath, "\\", fileName);
                }
                HtmlToPdf converter = new HtmlToPdf();
                converter.Options.PdfPageSize = PdfPageSize.A4;
                converter.Options.MarginLeft = 5;
                converter.Options.MarginRight = 5;
                converter.Options.MarginTop = 5;
                converter.Options.MarginBottom = 5;
                converter.Options.AutoFitWidth = HtmlToPdfPageFitMode.AutoFit;

                // header settings
                converter.Options.DisplayHeader = true;
                converter.Header.DisplayOnFirstPage = true;
                converter.Header.DisplayOnOddPages = true;
                converter.Header.DisplayOnEvenPages = true;
                converter.Header.Height = 15;

                // footer settings
                converter.Options.DisplayFooter = true;
                converter.Footer.DisplayOnFirstPage = true;
                converter.Footer.DisplayOnOddPages = true;
                converter.Footer.DisplayOnEvenPages = true;
                converter.Footer.Height = 15;

                //  converter.Options.AutoFitHeight = HtmlToPdfPageFitMode.AutoFit;
                converter.Options.PdfPageOrientation = PdfPageOrientation.Portrait;
                /*
                                converter.Options.AutoFitWidth = HtmlToPdfPageFitMode.ShrinkOnly;
                                converter.Options.AutoFitHeight = HtmlToPdfPageFitMode.NoAdjustment;*/

                PdfDocument doc = converter.ConvertHtmlString(htmlString);
                doc.Margins.Left = 0;
                doc.Margins.Right = 0;
                doc.Save(fileNameWithPath);
                doc.Close();
                flag = true;
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("HtmlToPDFConverterHelper", "GeneratePDF", ex);
            }
            return flag;
        }


        //Created by Kranti for API
        public bool GeneratePDFAPI(string htmlString, string requestPath, string customFileName)
        {
            bool flag = false;
            string fileName = "";
            var fileNameWithPath = "";
            try
            {
                if (customFileName != null && customFileName != "")
                {
                    fileName = customFileName;
                }
                else
                {
                    fileName = Guid.NewGuid().ToString("N");
                }
                if (requestPath != null)
                {
                    var filePath = string.Concat(requestPath);

                    if (!Directory.Exists(filePath))
                    {
                        Directory.CreateDirectory(filePath); //Create directory if it doesn't exist
                    }
                    fileNameWithPath = string.Concat(filePath, "\\", fileName);
                }
                HtmlToPdf converter = new HtmlToPdf();
                converter.Options.PdfPageSize = PdfPageSize.A4;
                converter.Options.MarginLeft = 5;
                converter.Options.MarginRight = 5;
                converter.Options.MarginTop = 5;
                converter.Options.MarginBottom = 5;
                converter.Options.AutoFitWidth = HtmlToPdfPageFitMode.AutoFit;

                // header settings
                converter.Options.DisplayHeader = true;
                converter.Header.DisplayOnFirstPage = true;
                converter.Header.DisplayOnOddPages = true;
                converter.Header.DisplayOnEvenPages = true;
                converter.Header.Height = 15;

                // footer settings
                converter.Options.DisplayFooter = true;
                converter.Footer.DisplayOnFirstPage = true;
                converter.Footer.DisplayOnOddPages = true;
                converter.Footer.DisplayOnEvenPages = true;
                converter.Footer.Height = 15;

                //  converter.Options.AutoFitHeight = HtmlToPdfPageFitMode.AutoFit;
                converter.Options.PdfPageOrientation = PdfPageOrientation.Portrait;
                /*
                                converter.Options.AutoFitWidth = HtmlToPdfPageFitMode.ShrinkOnly;
                                converter.Options.AutoFitHeight = HtmlToPdfPageFitMode.NoAdjustment;*/

                PdfDocument doc = converter.ConvertHtmlString(htmlString);
                doc.Margins.Left = 0;
                doc.Margins.Right = 0;
                doc.Save(fileNameWithPath);
                doc.Close();
                flag = true;
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("HtmlToPDFConverterHelper", "GeneratePDFAPI", ex);
            }
            return flag;
        }

        #region Custom layout PDF Generate
        public bool GenerateCustomLayoutPDF(string htmlString, string requestPath, string customFileName)
        {
            bool flag = false;
            string fileName = "";
            var fileNameWithPath = "";
            try
            {
                if (customFileName != null && customFileName != "")
                {
                    fileName = customFileName;
                }
                else
                {
                    fileName = Guid.NewGuid().ToString("N");
                }
                if (requestPath != null)
                {
                    var filePath = string.Concat(_webHostEnvironment.WebRootPath, "\\", requestPath);

                    if (!Directory.Exists(filePath))
                    {
                        Directory.CreateDirectory(filePath); //Create directory if it doesn't exist
                    }
                    fileNameWithPath = string.Concat(filePath, "\\", fileName);
                }
                HtmlToPdf converter = new HtmlToPdf();
                // converter.Options.PdfPageSize = PdfPageSize.A4;
                converter.Options.PdfPageSize = PdfPageSize.Custom;

                //converter.Options.WebPageWidth = 673;
                //converter.Options.WebPageHeight = 962;

                converter.Options.WebPageWidth = 595;
                converter.Options.WebPageHeight = 841;

                converter.Options.MarginLeft = 0;
                converter.Options.MarginRight = 0;
                converter.Options.MarginTop = 0;
                converter.Options.MarginBottom = 0;
                converter.Options.AutoFitWidth = HtmlToPdfPageFitMode.AutoFit;

                // header settings
                converter.Options.DisplayHeader = true;
                converter.Header.DisplayOnFirstPage = true;
                converter.Header.DisplayOnOddPages = true;
                converter.Header.DisplayOnEvenPages = true;
                converter.Header.Height = 0;

                // footer settings
                converter.Options.DisplayFooter = false;
                //converter.Footer.DisplayOnFirstPage = true;
                //converter.Footer.DisplayOnOddPages = true;
                //converter.Footer.DisplayOnEvenPages = true;
                //converter.Footer.Height = 0;

                //  converter.Options.AutoFitHeight = HtmlToPdfPageFitMode.AutoFit;
                converter.Options.PdfPageOrientation = PdfPageOrientation.Portrait;
                /*
                                converter.Options.AutoFitWidth = HtmlToPdfPageFitMode.ShrinkOnly;
                                converter.Options.AutoFitHeight = HtmlToPdfPageFitMode.NoAdjustment;*/

                PdfDocument doc = converter.ConvertHtmlString(htmlString);
                doc.Margins.Left = 0;
                doc.Margins.Right = 0;
                doc.Save(fileNameWithPath);
                doc.Close();
                flag = true;
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("HtmlToPDFConverterHelper", "GenerateCustomLayoutPDF", ex);
            }
            return flag;
        }
        #endregion
    }
}