using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using RDCELERP.BAL.Interface;
using RDCELERP.Common.Enums;
using RDCELERP.Common.Helper;
using RDCELERP.Core.App.Pages.Base;
using RDCELERP.Core.App.Pages.EVC;
using RDCELERP.DAL.Entities;
using RDCELERP.Model.Base;
using RDCELERP.Model.Company;
using RDCELERP.Model.EVC;
using RDCELERP.Model.Users;
using JsonResult = Microsoft.AspNetCore.Mvc.JsonResult;
using System.IO.Compression;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using static Org.BouncyCastle.Math.EC.ECCurve;
using NPOI.HPSF;
using System.Net;

namespace RDCELERP.Core.App.Pages.EVC_Portal
{
    public class BulkDownloadPDFModel : BasePageModel
    {
        #region Variable declartion
        private readonly RDCELERP.DAL.Entities.Digi2l_DevContext _context;
        private readonly IEVCManager _EVCManager;
        private readonly ILogisticManager _LogisticManager;
        private readonly IDropdownManager _dropdownManager;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IOptions<ApplicationSettings> _config;
        #endregion

        # region Constructor
        public BulkDownloadPDFModel(RDCELERP.DAL.Entities.Digi2l_DevContext context, IOptions<ApplicationSettings> config, IEVCManager EVCManager, CustomDataProtection protector, ILogisticManager logisticManager, IDropdownManager dropdownManager, IWebHostEnvironment webHostEnvironment) : base(config)
        {
            _EVCManager = EVCManager;
            _context = context;
            _LogisticManager = logisticManager;
            _dropdownManager = dropdownManager;
            _webHostEnvironment = webHostEnvironment;
            _config = config;
        }
        #endregion 

        #region Model Binding
        
        [BindProperty(SupportsGet = true)]
        public int? userId { get; set; }

        [BindProperty(SupportsGet = true)]
        public int? EVCBulkzipdownloddiff { get; set; }

        [BindProperty(SupportsGet = true)]
        public int? EVCRegistrationId { get; set; }
        #endregion

        public async Task OnGetAsync()
        {
            userId = _loginSession.UserViewModel.UserId;
            IEnumerable<SelectListItem>? EVCPartnerList = null;
            TblEvcregistration? tblEvcregistration = null;
           
            if (userId!=null)
            {               
                EVCBulkzipdownloddiff = _config.Value.EVCBulkzipdownloddiff;
                tblEvcregistration =_context.TblEvcregistrations.Where(x => x.UserId == userId && x.IsActive == true && x.Isevcapprovrd == true).FirstOrDefault();
                if (tblEvcregistration != null)
                {
                    EVCRegistrationId = tblEvcregistration.EvcregistrationId;
                    EVCPartnerList = _dropdownManager.GetEVCPartnerlistByEVCID(tblEvcregistration.EvcregistrationId);
                    ViewData["EVCPartnerList"] = EVCPartnerList;
                }
            }
          
        }
       
        // Handler to generate and download the ZIP file
        public IActionResult OnGetGenerateZip(DateTime startDate, DateTime endDate, int evcPartnerId,string Downloadziptype,int EvcRegistrationId)
        {            
            string? returnZippath = null;
            string? zipFilePath = null;
            string? tempDir = null;
            bool flag = false;

            try
            {
                // Your logic to get the list of PDF names from the database based on filter parameters
                List<string> pdfNames = GetPdfNames(startDate, endDate, evcPartnerId, Downloadziptype, EvcRegistrationId);
                if (pdfNames.Count>0)
                {                    
                    string? baseUrl = null;
                    string? URL = _config.Value.BaseURL;
                    // Base URL for the PDF files
                    if (Downloadziptype == "CD")
                    {
                        baseUrl = Path.Combine(_webHostEnvironment.WebRootPath, EnumHelper.DescriptionAttr(FilePathEnum.CustomerDeclaration));
                    }
                    else if (Downloadziptype == "Invoice")
                    {
                        baseUrl = Path.Combine(_webHostEnvironment.WebRootPath, EnumHelper.DescriptionAttr(FilePathEnum.EVCInvoice));
                    }
                    else if (Downloadziptype == "DebitNote")
                    {
                        baseUrl = Path.Combine(_webHostEnvironment.WebRootPath, EnumHelper.DescriptionAttr(FilePathEnum.EVCDebitNote));
                    }
                    else if (Downloadziptype == "POD")
                    {
                        baseUrl = Path.Combine(_webHostEnvironment.WebRootPath, EnumHelper.DescriptionAttr(FilePathEnum.EVCPoD));
                    }

                    // Create a temporary directory to store the PDF files
                    var filePath = string.Concat(_webHostEnvironment.WebRootPath, "\\", EnumHelper.DescriptionAttr(FilePathEnum.EVCBulkZip));
                    tempDir = string.Concat(filePath, "\\", Guid.NewGuid().ToString());
                    if (!Directory.Exists(tempDir))
                    {
                        Directory.CreateDirectory(tempDir); //Create directory if it doesn't exist
                    }
                    flag = true;
                    // Download PDF files to the temporary directory
                    foreach (string pdfName in pdfNames)
                    {
                        string pdfUrl = $"{baseUrl}/{pdfName}";
                        string destPath = Path.Combine(tempDir, pdfName);
                        DownloadFile(pdfUrl, destPath);
                    }

                    // Create a ZIP file
                    string zipFileName = $"Download_{Downloadziptype}{DateTime.Now:yyyyMMddHHmmss}" + ".zip";
                    zipFilePath = Path.Combine(filePath, zipFileName);
                    ZipFile.CreateFromDirectory(tempDir, zipFilePath);
                    returnZippath = URL + EnumHelper.DescriptionAttr(FilePathEnum.EVCBulkZip) + "/" + zipFileName;

                    return Content(returnZippath, "text/plain");
                }
                else 
                {
                    string? message = "Data does not exist.";
                    flag= false;
                    return Content(message, "text/plain");
                    //return Content(message, "Data not found");
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions
                return BadRequest($"Error: {ex.Message}");
            }
            finally
            {
                if (flag == true)
                {
                    Directory.Delete(tempDir, true);
                }
                
            }
        }

        // Your existing GetPdfNames method
        private List<string> GetPdfNames(DateTime startDate, DateTime endDate, int evcPartnerId, string Downloadziptype, int EvcRegistrationId)
        {
            List<string?> pdfNames = null;

            startDate = startDate.AddMinutes(-1);
            endDate = endDate.AddDays(1).AddMinutes(-1).AddSeconds(-1);

            //DateTime startDateTime = DateTime.Parse(startDate);
            //DateTime endDateTime = DateTime.Parse(endDate);
            if (Downloadziptype == "CD")
            {
                pdfNames = _context.TblOrderLgcs.Where(x=>x.IsActive==true&& x.ActualPickupDate !=null && x.CustDeclartionpdfname !=null && x.ActualPickupDate >= startDate && x.ActualPickupDate <= endDate && x.EvcregistrationId==EvcRegistrationId && ((evcPartnerId == 0) ? true: (x.EvcpartnerId == evcPartnerId)))
                    .Select(x=>x.CustDeclartionpdfname).ToList();
            }
            else if (Downloadziptype == "Invoice")
            {
                pdfNames = _context.TblEvcpoddetails.Where(x => x.IsActive == true && x.InvoicePdfName !=null&&x.InvoiceDate!=null && x.InvoiceDate >= startDate && x.InvoiceDate <= endDate && x.Evcid == EvcRegistrationId && ((evcPartnerId == 0) ? true : (x.EvcpartnerId == evcPartnerId)))
                    .Select(x => x.InvoicePdfName).ToList();
            }
            else if (Downloadziptype == "DebitNote")
            {
                pdfNames = _context.TblEvcpoddetails.Where(x => x.IsActive == true && x.DebitNotePdfName != null && x.DebitNoteDate != null && x.DebitNoteDate >= startDate && x.DebitNoteDate <= endDate && x.Evcid == EvcRegistrationId && ((evcPartnerId == 0) ? true : (x.EvcpartnerId == evcPartnerId)))
                    .Select(x => x.DebitNotePdfName).ToList();
            }
            else if (Downloadziptype == "POD")
            {
                pdfNames = _context.TblEvcpoddetails.Where(x => x.IsActive == true && x.Podurl != null && x.CreatedDate != null && x.CreatedDate >= startDate && x.CreatedDate <= endDate && x.Evcid == EvcRegistrationId && ((evcPartnerId == 0) ? true : (x.EvcpartnerId == evcPartnerId)))
                        .Select(x => x.Podurl).ToList();
            }
            return pdfNames;
        }
       
        // Helper method to download a file from a URL
        private void DownloadFile(string url, string destinationPath)
        {
            try
            {
                url = url.Replace('/', '\\');
                using (var client = new System.Net.WebClient())
                {
                    client.DownloadFile(url, destinationPath);
                }
            }
            catch (Exception ex)
            {
                // Log the exception or print the error message for debugging
                Console.WriteLine($"Error downloading file from {url}. Error: {ex.Message}");
            }
        }
    }
}

