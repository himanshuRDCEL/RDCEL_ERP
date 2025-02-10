using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DocumentFormat.OpenXml.Math;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;
using SendGrid;
using RDCELERP.BAL.Helper;
using RDCELERP.BAL.Interface;
using RDCELERP.BAL.MasterManager;
using RDCELERP.Common.Helper;
using RDCELERP.Core.App.Pages.ABBRedemption;
using RDCELERP.Core.App.Pages.Base;
using RDCELERP.Core.App.Pages.EVC;
using RDCELERP.DAL.Entities;
using RDCELERP.DAL.IRepository;
using RDCELERP.DAL.Repository;
using RDCELERP.Model.Base;
using RDCELERP.Model.EVC;
using RDCELERP.Model.EVC_Portal;
using RDCELERP.Model.Paymant;

namespace RDCELERP.Core.App.Pages.EVC_Portal
{
    public class EVCWalletRechargeByAdminModel : BasePageModel
    {
        private readonly RDCELERP.DAL.Entities.Digi2l_DevContext _context;
        private readonly IEVCManager _EVCManager;
        ILogging _logging;
        DateTime _currentDatetime = DateTime.Now.TrimMilliseconds();
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ICreditRequestRepository _creditRequestRepository;

        public EVCWalletRechargeByAdminModel(RDCELERP.DAL.Entities.Digi2l_DevContext context, IOptions<ApplicationSettings> config, IEVCManager EVCManager, ILogging logging, IWebHostEnvironment webHostEnvironment, ICreditRequestRepository creditRequestRepository)
      : base(config)
        {
            _EVCManager = EVCManager;
            _context = context;
            _logging = logging;
            _webHostEnvironment = webHostEnvironment;
            _creditRequestRepository = creditRequestRepository;
        }

        #region Model Binding

        [BindProperty(SupportsGet = true)]
        public TblEvcregistration TblEvcregistrations { get; set; }
        [BindProperty(SupportsGet = true)]
        public IList<EVC_ApprovedModel> EVC_ApprovedModels { get; set; }


        public TblEvcwalletAddition EvcwalletAdditions { get; set; }
        [BindProperty(SupportsGet = true)]
        public EVCWalletAdditionViewModel EVCWalletAdditionViewModels { get; set; }
      
        public class GenrateEVCCreaditTransactionID
        {
            public string? TransactionId { get; set; }
            public string TransactionDate { get; set; }
        } 

        //DateTime _currentDatetime = DateTime.Now.TrimMilliseconds();
        #endregion
        public IActionResult OnGet(int EVCRegistration)
        {
            try
            {
                if (EVCRegistration != null)
                {
                    if (_loginSession == null)
                    {
                        return RedirectToPage("/index");
                    }
                    else
                    {
                        TblEvcregistration tblEvcregistration = _context.TblEvcregistrations.Where(x => x.EvcregistrationId == EVCRegistration && x.IsActive == true && x.Isevcapprovrd == true).FirstOrDefault();
                        if (tblEvcregistration != null)
                        {
                            EVCWalletAdditionViewModels = new EVCWalletAdditionViewModel();
                            EVCWalletAdditionViewModels.EvcregistrationId = tblEvcregistration.EvcregistrationId != null && tblEvcregistration.EvcregistrationId > 0 ? tblEvcregistration.EvcregistrationId : 0;
                            EVCWalletAdditionViewModels.BussinessName = tblEvcregistration.BussinessName != null ? tblEvcregistration.BussinessName : String.Empty;
                            EVCWalletAdditionViewModels.EVCemail = tblEvcregistration.EmailId != null ? tblEvcregistration.EmailId : String.Empty;
                            EVCWalletAdditionViewModels.EVCcontactNumber = tblEvcregistration.EvcmobileNumber != null ? tblEvcregistration.EvcmobileNumber : String.Empty;
                            EVCWalletAdditionViewModels.EVCaddress = tblEvcregistration.ContactPersonAddress != null ? tblEvcregistration.ContactPersonAddress : String.Empty;
                            EVCWalletAdditionViewModels.EVCRegdNo = tblEvcregistration.EvcregdNo != null ? tblEvcregistration.EvcregdNo : String.Empty;
                            EVCWalletAdditionViewModels.EvcWallet = tblEvcregistration.EvcwalletAmount != null && tblEvcregistration.EvcwalletAmount > 0 ? tblEvcregistration.EvcwalletAmount : 0;
                        }

                        ViewData["BaseUrl"] = _baseConfig.Value.BaseURL;
                        return Page();
                    }
                }
                return Page();
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("ZaakpayManager", "ManageImageLabel", ex);
                return Page();
            }
        }
        public IActionResult OnPostAsync(IFormFile? InvoiceImage)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var result = 0;
                    if (_loginSession == null)
                    {
                        return RedirectToPage("/Index");
                    }
                    else
                    {
                        #region Save Data for  TblWalletaddition table and update TblEvcregistrations table 

                        if (InvoiceImage != null)
                        {
                            string fileName = Guid.NewGuid().ToString("N") + InvoiceImage.FileName;
                            var filePath = string.Concat(_webHostEnvironment.WebRootPath, @"\DBFiles\EVCInvoiceRecipetByUTC\InvoiceImage");
                            var fileNameWithPath = string.Concat(filePath, "\\", fileName);
                            using (var stream = new FileStream(fileNameWithPath, FileMode.Create))
                            {
                                InvoiceImage.CopyTo(stream);
                                EVCWalletAdditionViewModels.InvoiceImage = fileName;
                            }
                        }
                       
                            result = _EVCManager.SaveEVCWalletDetails(EVCWalletAdditionViewModels, _loginSession.UserViewModel.UserId);
                        
                        return RedirectToPage("./AddEVCWalletBalance/");
                        #endregion
                    }
                }
                else
                {
                    return RedirectToPage("./AddEVCWalletBalance/");
                }

            }

            catch (Exception ex)
            {
                _logging.WriteErrorToDB("ZaakpayManager", "ManageImageLabel", ex);
                return Page();
            }
        }

        private int sequenceCounter = 0;  // In-memory sequence counter

        public JsonResult OnGetGenrateEVCCreaditTransactionIDAsync()
        {
            string dateStamp = DateTime.Now.ToString("yyyyMMdd");
            int sequenceNumber = GetNextSequenceNumber();
            DateTime dateTime = DateTime.Now;
            string Date = dateTime.ToString("yyyy-MM-dd");


            GenrateEVCCreaditTransactionID result = new GenrateEVCCreaditTransactionID
            {
                TransactionId = $"CT{dateStamp}-{sequenceNumber.ToString("D3")}",
                TransactionDate = Date
            };

            return new JsonResult(result);
        }

        private int GetNextSequenceNumber()
        {
            // Increment the sequence counter
            List<TblEvcwalletAddition> tblEvcwalletAdditions = new List<TblEvcwalletAddition>();
            tblEvcwalletAdditions = _context.TblEvcwalletAdditions.Where(x => x.IsCreaditNote == true).ToList(); 
            int sequenceNumber = tblEvcwalletAdditions.Count;
            sequenceNumber++;

            // Return the incremented counter
            return sequenceNumber;
        }

    }
}