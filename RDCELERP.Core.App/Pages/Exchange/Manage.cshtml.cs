using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;
using RDCELERP.BAL.Interface;
using RDCELERP.Core.App.Pages.Base;
using RDCELERP.DAL.Entities;
using RDCELERP.Model.Base;
using RDCELERP.Model.AbbRegistration;
using RDCELERP.DAL.IRepository;
using Microsoft.AspNetCore.Mvc.Rendering;
using RDCELERP.Model.City;
using System.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Data;
using ExcelDataReader;
using RDCELERP.Model.PinCode;
using RDCELERP.Model.ExchangeOrder;
using RDCELERP.Model;
using static RDCELERP.Model.QCCommentViewModel;
using RDCELERP.Model.LGC;
using RDCELERP.Model.QCComment;
using RDCELERP.Common.Helper;
using RDCELERP.Model.QC;
using RDCELERP.Common.Enums;
using static RDCELERP.Model.Whatsapp.WhatsappRescheduleViewModel;
using RestSharp;
using RDCELERP.Common.Constant;
using Newtonsoft.Json;
using RDCELERP.Model.Users;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using RDCELERP.Model.UniversalPriceMaster;
using DocumentFormat.OpenXml.Office2010.ExcelAc;

namespace RDCELERP.Core.App.Pages.Exchange
{
    public class ManageModel : CrudBasePageModel
    {
        #region variable declaration
        private readonly IExchangeOrderManager _ExchangeOrderManager;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IStateManager _stateManager;
        private readonly ICityManager _cityManager;
        private readonly IBrandManager _brandManager;
        private readonly IBusinessPartnerManager _businessPartnerManager;
        private readonly IProductCategoryManager _productCategoryManager;
        private readonly IProductTypeManager _productTypeManager;
        private readonly IPinCodeManager _pinCodeManager;
        private readonly IQCCommentManager _QcCommentManager;
        IOrderQCRepository _orderQCRepository;
        IOrderTransRepository _orderTransRepository;
        IWhatsappNotificationManager _whatsappNotificationManager;
        IWhatsAppMessageRepository _WhatsAppMessageRepository;
        IExchangeOrderRepository _exchangeOrderRepository;
        ICustomerDetailsRepository _customerDetailsRepository;
        private readonly RDCELERP.DAL.Entities.Digi2l_DevContext _context;
        INotificationManager _notificationManager;
        IEmailTemplateManager _emailTemplateManager;
        IBusinessPartnerRepository _buinessPartnerRepository;
        ICommonManager _commonManager;
        IDropdownManager _dropdownManager;
        IProductConditionLabelRepository _productConditionLabelRepository;
        IPriceMasterMappingRepository _priceMasterMappingRepository;
        #endregion

        #region constructor
        public ManageModel(IQCCommentManager QcCommentManager, IPinCodeManager pinCodeManager, IStoreCodeManager storeCodeManager, RDCELERP.DAL.Entities.Digi2l_DevContext context, IProductTypeManager productTypeManager, IExchangeOrderManager exchangeOrderManager, IProductCategoryManager productCategoryManager, IBusinessPartnerManager businessPartnerManager, IBrandManager brandManager, IStateManager StateManager, ICityManager CityManager, IWebHostEnvironment webHostEnvironment, IOptions<ApplicationSettings> config, IUserManager userManager, CustomDataProtection protector, IOrderQCRepository orderQCRepository, IOrderTransRepository orderTransRepository, ICustomerDetailsRepository customerDetailsRepository, IExchangeOrderRepository exchangeOrderRepository, IWhatsAppMessageRepository whatsAppMessageRepository, IWhatsappNotificationManager whatsappNotificationManager, INotificationManager notificationManager, IEmailTemplateManager emailTemplateManager, IBusinessPartnerRepository buinessPartnerRepository, ICommonManager commonManager, IDropdownManager dropdownManager, IProductConditionLabelRepository productConditionLabelRepository, IPriceMasterMappingRepository priceMasterMappingRepository) : base(config, protector)

        {
            _webHostEnvironment = webHostEnvironment;
            _ExchangeOrderManager = exchangeOrderManager;
            _stateManager = StateManager;
            _cityManager = CityManager;
            _brandManager = brandManager;
            _businessPartnerManager = businessPartnerManager;
            _productCategoryManager = productCategoryManager;
            _productTypeManager = productTypeManager;
            _pinCodeManager = pinCodeManager;
            _QcCommentManager = QcCommentManager;
            _orderQCRepository = orderQCRepository;
            _orderTransRepository = orderTransRepository;
            _customerDetailsRepository = customerDetailsRepository;
            _exchangeOrderRepository = exchangeOrderRepository;
            _WhatsAppMessageRepository = whatsAppMessageRepository;
            _whatsappNotificationManager = whatsappNotificationManager;
            _context = context;
            _notificationManager = notificationManager;
            _emailTemplateManager = emailTemplateManager;
            _buinessPartnerRepository = buinessPartnerRepository;
            _commonManager = commonManager;
            _dropdownManager = dropdownManager;
            _productConditionLabelRepository = productConditionLabelRepository;
            _priceMasterMappingRepository = priceMasterMappingRepository;
        }
        #endregion

        #region BindProperty
        [BindProperty(SupportsGet = true)]
        public ExchangeOrderViewModel ExchangeOrderViewModel { get; set; }
        public ExchangeOrderViewModel ExchangeOrder { get; set; }
        public QCCommentViewModel QCCommentViewModel { get; set; }
        public QCCommentViewModel commentViewModel { get; set; }
        public LGCOrderViewModel lGCOrderViewModel { get; set; }
        public VoucherDetailsViewModel voucherDetailsViewModel { get; set; }
        public IList<SelfQCViewModel> SelfQCVMList { get; set; }
        public IList<OrderImageUploadViewModel> OrderImageUploadVM{ get; set; }
        public IList<OrderImageUploadViewModel> OrderImageUpload { get; set; }
        public TblEvcpoddetail tblEVCPODDetails { get; set; }

        [BindProperty(SupportsGet = true)]
        public UserViewModel UserViewModel { get; set; }

        [BindProperty(SupportsGet = true)]
        public IList<TblUserRole> TblUserRole { get; set; }
        [BindProperty(SupportsGet = true)]
        public TblCompany TblCompany { get; set; }
        [BindProperty(SupportsGet = true)]
        public int UserId { get; set; }
        public List<SelectListItem> ProductQuality { get; set; }
        #endregion
        public IActionResult OnGet(string Exchangeorderid)
        {            
            string MVCExchangeInvoceImageURL = _baseConfig.Value.MVCBaseURLForExchangeInvoice;
            string UserCompanyName = null;
            string CompanyName = null;
            TblOrderTran? tblOrderTran = null;
            if (Exchangeorderid != null)
            {
                if (_loginSession == null)
                {
                    return RedirectToPage("/index");
                }
                else
                {
                    if (_loginSession.UserViewModel.CompanyId > 0)
                    {
                        TblCompany = _context.TblCompanies.FirstOrDefault(x => x.CompanyId == _loginSession.UserViewModel.CompanyId);
                        if (TblCompany != null)
                        {
                            UserCompanyName = TblCompany.CompanyName;                            
                        }
                    }                   
                    CompanyName = EnumHelper.DescriptionAttr(CompanyNameenum.UTC);                                        
                }

                Exchangeorderid = _protector.Decode(Exchangeorderid);                
                int customerid = 0;
                UserId = _loginSession.UserViewModel.UserId;
                
                #region Get Exchange order Details
                ExchangeOrderViewModel = _ExchangeOrderManager.GetQCOrderByExchangeId(Convert.ToInt32(Exchangeorderid));
                if (ExchangeOrderViewModel != null)
                {                    
                    ExchangeOrderViewModel.IsDiagnostic = _baseConfig.Value.IsDiagnostic;
                    if (!string.IsNullOrEmpty(ExchangeOrderViewModel.InvoiceImageName))
                    {
                        ExchangeOrderViewModel.InvoiceImageName = MVCExchangeInvoceImageURL + ExchangeOrderViewModel.InvoiceImageName;
                    }                    
                }
                if (new[] { Convert.ToInt32(OrderStatusEnum.OrderAcceptedbyVehicle), Convert.ToInt32(OrderStatusEnum.LGCPickup), Convert.ToInt32(OrderStatusEnum.LGCDrop), Convert.ToInt32(OrderStatusEnum.Posted) }.Contains(ExchangeOrderViewModel.StatusId??0))
                {
                    ExchangeOrderViewModel.IsPageEditable = false;
                }
                else
                {
                    if (_loginSession.RoleViewModel != null && _loginSession.RoleViewModel.RoleAccessViewModelList != null)
                    {
                        if (_loginSession.RoleViewModel.RoleName == _baseConfig.Value.ExchangemanagepageEdit)
                        {
                            ExchangeOrderViewModel.IsPageEditable = true;
                        }
                        else
                        {
                            ExchangeOrderViewModel.IsPageEditable = false;
                        }
                    }
                    else
                    {
                        ExchangeOrderViewModel.IsPageEditable = false;
                    }
                }
                

                #endregion

                #region Get QC Details
                QCCommentViewModel = _QcCommentManager.GetQcDetails(Convert.ToInt32(Exchangeorderid));
                if (QCCommentViewModel != null)
                {
                    QCCommentViewModel.UserCompany = UserCompanyName;
                    QCCommentViewModel.CompanyUTC = CompanyName;
                    if (QCCommentViewModel.QuestionerPdfName != null)
                    {
                        var FullDiagnosticPdfUrl = _baseConfig.Value.BaseURL + EnumHelper.DescriptionAttr(FileAddressEnum.DiagnosticPdf) + QCCommentViewModel.QuestionerPdfName;
                        QCCommentViewModel.QuestionerPdfName = FullDiagnosticPdfUrl;
                    }
                }
                #endregion

                #region Get LGC Details
                tblOrderTran = _orderTransRepository.GetQcDetailsByExchangeId(Convert.ToInt32(Exchangeorderid)); //Getting ordertrans details by EXchange order id
                if (tblOrderTran != null)
                {                    
                    commentViewModel = _QcCommentManager.GetLGCDetails(tblOrderTran.OrderTransId);

                    #region EVC Pod Details, invoice, DebitNotePdf, CustomerDeclarationPdf, PoDPdf
                    if (commentViewModel != null && commentViewModel.LogisticViewModel != null)
                    {
                        if (commentViewModel.LogisticViewModel.PoDPdf != null)
                        {
                            var FullPoDUrl = _baseConfig.Value.BaseURL + EnumHelper.DescriptionAttr(FileAddressEnum.EVCPoD) + commentViewModel.LogisticViewModel.PoDPdf;
                            commentViewModel.LogisticViewModel.PoDPdf = FullPoDUrl;
                        }
                        if (commentViewModel.LogisticViewModel.DebitNotePdf != null)
                        {
                            var FullDebitNoteUrl = _baseConfig.Value.BaseURL + EnumHelper.DescriptionAttr(FileAddressEnum.EVCDebitNote) + commentViewModel.LogisticViewModel.DebitNotePdf;
                            commentViewModel.LogisticViewModel.DebitNotePdf = FullDebitNoteUrl;
                        }
                        if (commentViewModel.LogisticViewModel.InvoieImagePdf != null)
                        {
                            var FullInvoiceUrl = _baseConfig.Value.BaseURL + EnumHelper.DescriptionAttr(FileAddressEnum.EVCInvoice) + commentViewModel.LogisticViewModel.InvoieImagePdf;
                            commentViewModel.LogisticViewModel.InvoieImagePdf = FullInvoiceUrl;
                        }
                        if (commentViewModel.LogisticViewModel.CustomerDeclarationPdf != null)
                        {
                            var FullCustomerDeclarationUrl = _baseConfig.Value.BaseURL + EnumHelper.DescriptionAttr(FileAddressEnum.CustomerDeclaration) + commentViewModel.LogisticViewModel.CustomerDeclarationPdf;
                            commentViewModel.LogisticViewModel.CustomerDeclarationPdf = FullCustomerDeclarationUrl;
                        }
                    }
                    #endregion                    
                }
                #endregion

                #region Get Voucher Details
                voucherDetailsViewModel = _QcCommentManager.GetVoucherDetails(Convert.ToInt32(Exchangeorderid));
                if (voucherDetailsViewModel != null)
                {
                    if (!string.IsNullOrEmpty(voucherDetailsViewModel.InvoiceImageName))
                    {
                        voucherDetailsViewModel.InvoiceImageName = MVCExchangeInvoceImageURL + voucherDetailsViewModel.InvoiceImageName;
                    }
                    if (ExchangeOrderViewModel!=null && ExchangeOrderViewModel.CustomerDetailsId != null)
                    {
                        customerid = Convert.ToInt32(ExchangeOrderViewModel.CustomerDetailsId);
                    }                   
                }
                #endregion            

                #region Code for Get Self QC Images
                if (ExchangeOrderViewModel != null && !string.IsNullOrEmpty(ExchangeOrderViewModel.RegdNo))
                {
                    string url = _baseConfig.Value.BaseURL;
                    SelfQCVMList = _QcCommentManager.GetImagesUploadedBySelfQC(ExchangeOrderViewModel.RegdNo);
                    if (SelfQCVMList != null && SelfQCVMList.Count > 0)
                    {
                        foreach (var item in SelfQCVMList)
                        {
                            item.FilePath = EnumHelper.DescriptionAttr(FilePathEnum.SelfQC);
                            item.ImageWithPath = url + item.FilePath.Replace("\\", "/") + "/" + item.ImageName;
                        }
                    }
                    else
                    {
                        SelfQCVMList = new List<SelfQCViewModel>();
                    }
                }
                #endregion 

                #region Code for Get Video QC Images
                if (ExchangeOrderViewModel != null && !string.IsNullOrEmpty(ExchangeOrderViewModel.RegdNo))
                {
                    string url = _baseConfig.Value.BaseURL;
                    OrderImageUpload = _QcCommentManager.GetImagesUploadedByVideoQC(ExchangeOrderViewModel.RegdNo);
                    if (OrderImageUpload != null && OrderImageUpload.Count > 0)
                    {
                        foreach (var item in OrderImageUpload)
                        {
                            item.FilePath = EnumHelper.DescriptionAttr(FilePathEnum.VideoQC);
                            item.ImageWithPath = url + item.FilePath.Replace("\\", "/") + "/" + item.ImageName;
                        }
                    }
                    else
                    {
                        OrderImageUpload = new List<OrderImageUploadViewModel>();
                    }
                }
                #endregion

                #region Code for Get LGC Images
                if (ExchangeOrderViewModel != null && !string.IsNullOrEmpty(ExchangeOrderViewModel.RegdNo))
                {
                    string url = _baseConfig.Value.BaseURL;
                    OrderImageUploadVM = _QcCommentManager.GetImagesUploadedByLGCQC(ExchangeOrderViewModel.RegdNo);
                    if (OrderImageUploadVM != null && OrderImageUploadVM.Count > 0)
                    {
                        foreach (var item in OrderImageUploadVM)
                        {
                            item.FilePath = EnumHelper.DescriptionAttr(FilePathEnum.LGCPickup);
                            item.ImageWithPath = url + item.FilePath.Replace("\\", "/") + "/" + item.ImageName;
                        }
                    }
                    else
                    {
                        OrderImageUploadVM = new List<OrderImageUploadViewModel>();
                    }
                }
                #endregion
            }

            //var NewBrandId = _brandManager.GetAllBrand();
            //if (NewBrandId != null)
            //{
            //    ViewData["NewBrandId"] = new SelectList(NewBrandId, "Id", "Name");
            //}

            //var ProductType = _productTypeManager.GetAllProductType();
            //if (ProductType != null)
            //{
            //    ViewData["ProductType"] = new SelectList(ProductType, "Id", "Description");
            //}

            var ProductQuality = _dropdownManager.GetProductCondition();
            if (ProductQuality != null)
            {                
                ViewData["ProductQuality"] = new SelectList(ProductQuality, "Value", "Text");
            }


            List<ListItem> qualityafterqcstr = EnumHelper.EnumToList<Common.Enums.EvcPartnerPreferenceEnum>();
            if (qualityafterqcstr != null)
            {
                ViewData["qualityafterqc"] = new SelectList(qualityafterqcstr, "Value", "Name");
            }

            if (_loginSession == null)
            {
                return RedirectToPage("/index");
            }
            else
            {
                ViewData["InvoiceImageURL"] = _baseConfig.Value.InvoiceImageURL;
                return Page();
            }
        }

        public IActionResult OnPostAsync()
        {
            int result = 0;
            result = _ExchangeOrderManager.ManageExchangeOrder(ExchangeOrderViewModel, _loginSession.UserViewModel.UserId);

            if (result > 0)
                return RedirectToPage("Index");
            else
                return RedirectToPage("Manage", new { Exchangeorderid = _protector.Encode(ExchangeOrderViewModel.Id) }); 
        }

        public IActionResult OnPostSendSelfQCUrlToCustomer(string regdno, string mobnumber, int loginid)
        {
            bool result = _ExchangeOrderManager.sendSelfQCUrl(regdno, mobnumber, loginid);           
            return new JsonResult(result);
        }

        #region Set Rescheduled data & send whatsapp msg Added by PJ
        public JsonResult OnGetRescheduleddataAsync(string RegdNo, string RecheduledTime,string RecheduledDate, string QCComment)
        {
            
            if (!string.IsNullOrEmpty(RegdNo) && !string.IsNullOrEmpty(RecheduledTime) && !string.IsNullOrEmpty(RecheduledDate))
            {
                if (RecheduledTime == "NaN:undefined AM")
                {
                    string RescheduleDatetime = "Please Provide valid Time format (hh:mm AM/PM)";
                    return new JsonResult(RescheduleDatetime);
                }
                else
                {
                    if (QCComment != null)
                    {
                        WhatasappResponse whatasappResponse = new WhatasappResponse();
                        TblWhatsAppMessage? tblwhatsappmessage = null;
                        TblExchangeOrder? tblExchangeOrder = null;
                        TblCustomerDetail? tblCustomerDetail = null;
                        TblOrderTran? tblOrderTran = null;
                        TblOrderQc? tblOrderQc = null;
                        string message = string.Empty;
                        string smsmessage = string.Empty;
                        //bool flag = false;
                        bool rescheduleresponse = false;
                        string originalFormat = "dd-MM-yyyy h:mm tt";
                        string desiredFormat = "yyyy-MM-dd HH:mm:ss.fff";
                        if (RegdNo != null)
                        {
                            tblOrderTran = _orderTransRepository.GetRegdno(RegdNo);
                            if (tblOrderTran != null)
                            {
                                QCCommentViewModel qcCommentViewModel = new QCCommentViewModel();
                                qcCommentViewModel.RegdNo = RegdNo;
                                DateTime parsedDateTime = DateTime.ParseExact((RecheduledDate + " " + RecheduledTime), originalFormat, CultureInfo.InvariantCulture);
                                string formattedDateTime = parsedDateTime.ToString(desiredFormat);
                                //qcCommentViewModel.ProposedQcdate = Convert.ToDateTime(RecheduledDate + " " + RecheduledTime);
                                qcCommentViewModel.ProposedQcdate = Convert.ToDateTime(formattedDateTime);
                                qcCommentViewModel.Qccomments = QCComment;
                                qcCommentViewModel.Isrescheduled = false;
                                qcCommentViewModel.StatusId = (int?)OrderStatusEnum.QCAppointmentrescheduled;

                                tblOrderQc = _orderQCRepository.GetQcorderBytransId(tblOrderTran.OrderTransId);
                                if (tblOrderQc != null && tblOrderQc.Reschedulecount > 0)
                                {
                                    qcCommentViewModel.Reschedulecount = (int)tblOrderQc.Reschedulecount + 1;
                                }
                                else
                                {
                                    qcCommentViewModel.Reschedulecount += 1;
                                }

                                rescheduleresponse = _QcCommentManager.Rescheduled(qcCommentViewModel, Convert.ToInt32(_loginSession.UserViewModel.UserId));
                                if (rescheduleresponse == true)
                                {
                                    #region whatsappNotification for RescheduleQCDate
                                    string ScheduledDate = Convert.ToDateTime(qcCommentViewModel.ProposedQcdate).ToString("MM/dd/yyyy hh:mm:ss tt");
                                    tblExchangeOrder = _exchangeOrderRepository.GetRegdNo(RegdNo);
                                    if (tblExchangeOrder != null)
                                    {
                                        tblCustomerDetail = _customerDetailsRepository.GetCustDetails(tblExchangeOrder.CustomerDetailsId);

                                        WhatsappTemplate whatsappObj = new WhatsappTemplate();
                                        whatsappObj.userDetails = new UserDetails();
                                        whatsappObj.notification = new RescheduleQCDate();
                                        whatsappObj.notification.@params = new SendDate();
                                        whatsappObj.userDetails.number = tblCustomerDetail.PhoneNumber;
                                        whatsappObj.notification.sender = _baseConfig.Value.YelloaiSenderNumber;
                                        whatsappObj.notification.type = _baseConfig.Value.YellowaiMesssaheType;
                                        whatsappObj.notification.templateId = NotificationConstants.RescheduleDate_Time;
                                        whatsappObj.notification.@params.RescheduleDate = ScheduledDate;
                                        string url = _baseConfig.Value.YellowAiUrl;

                                        RestResponse response = _whatsappNotificationManager.Rest_InvokeWhatsappserviceCall(url, Method.Post, whatsappObj);
                                        if (response.Content != null)
                                        {
                                            whatasappResponse = JsonConvert.DeserializeObject<WhatasappResponse>(response.Content);
                                            tblwhatsappmessage = new TblWhatsAppMessage();
                                            tblwhatsappmessage.TemplateName = NotificationConstants.RescheduleDate_Time;
                                            tblwhatsappmessage.IsActive = true;
                                            tblwhatsappmessage.PhoneNumber = tblCustomerDetail.PhoneNumber;
                                            tblwhatsappmessage.SendDate = DateTime.Now;
                                            tblwhatsappmessage.MsgId = whatasappResponse.msgId;
                                            _WhatsAppMessageRepository.Create(tblwhatsappmessage);
                                            _WhatsAppMessageRepository.SaveChanges();
                                        }
                                    }
                                    #endregion

                                    #region Send SMS and Email
                                    //smsmessage = NotificationConstants.QC_Reschedule_SMS.Replace("[Customer's Name]", tblCustomerDetail.FirstName + " " + tblCustomerDetail.LastName).Replace("[date/time]", ScheduledDate);
                                    //flag = _notificationManager.SendQCSMS(tblCustomerDetail.PhoneNumber, smsmessage);

                                    //string subject = "QC Reschedule Details";
                                    //string htmlbody = NotificationConstants.QC_Reschedule_Email.Replace("[Customer's Name]", tblCustomerDetail.FirstName + " " + tblCustomerDetail.LastName).Replace("[date and time]", ScheduledDate);
                                    //string too = tblCustomerDetail.Email;

                                    //bool EmailResponse = _emailTemplateManager.CommonEmailBody(too, htmlbody, subject);
                                    #endregion
                                }
                                return new JsonResult(qcCommentViewModel);
                            }
                            else
                            {
                                return new JsonResult(Page());
                            }
                        }
                        else
                        {
                            return new JsonResult(Page());
                        }

                    }
                    else
                    {
                        string RescheduleComment = "Please Fill Reschedule Comment";
                        return new JsonResult(RescheduleComment);
                    }
                }
            }
            else
            {
                string RescheduleDatetime = "Please Fill Reschedule Date & Time";
                return new JsonResult(RescheduleDatetime);
            }        
                   
        }
        #endregion

        #region Set status for Qc Cancel Added by PJ
        public JsonResult OnGetCancelQcAsync(string RegdNo, string CancelComment)
        {
            bool respose = false;
            respose = _QcCommentManager.GetQCCancelById(RegdNo, CancelComment, Convert.ToInt32(_loginSession.UserViewModel.UserId));
            return new JsonResult(respose);
        }
        #endregion

        #region Check UPI and Beneficiary Added by PJ
        public JsonResult OnGetCheckBeneficiaryAsync(string RegdNo)
        {
            string result = null;
            result = _QcCommentManager.CheckUPIandBeneficiary(RegdNo.ToString());
            return new JsonResult(result);
        }
        #endregion

        #region AutoFill store details Added By PJ
        public IActionResult OnGetSearchstoredetails(string term, string term2)
        {
            if (term == null)
            {
                return BadRequest();
            }            
            else
            {
                var list = _context.TblBusinessPartners
                     .Where(s => (s.StoreCode.Contains(term) || term == "#") && s.BusinessUnitId == Convert.ToInt32(term2) && s.IsExchangeBp == true && s.IsActive == true)
                     .Select(s => new SelectListItem
                     {
                         Value = s.StoreCode,
                         Text = s.BusinessPartnerId.ToString()
                     })
                     .ToArray();
                return new JsonResult(list);
            }         
        }

        public JsonResult OnGetEditStoreDetailsAsync(string StoreCode)
        {
            TblBusinessPartner? tblBusinessPartner = null;
            if(StoreCode != null)
            {
                tblBusinessPartner=_buinessPartnerRepository.GetStoreDetails(StoreCode);
            }
            return new JsonResult(tblBusinessPartner);
        }
        #endregion

        #region AutoFill To Get State List Added By PJ
        public IActionResult OnGetSearchState(string term)
        {
            if (term == null)
            {
                return BadRequest();
            }
            var data = _context.TblStates
                       .Where(s => s.Name.Contains(term) || term == "#")
                       .Select(s => new SelectListItem
                       {
                           Value = s.Name,
                           Text = s.StateId.ToString()
                       })
                       .ToArray();
            return new JsonResult(data);
        }

        #region Check State is Valid Added By PJ
        public IActionResult OnGetCheckState(string Statename)
        {
            IList<string>? list = null;
            list = _commonManager.CheckStateisValid(Statename);
            return new JsonResult(list);
        }
        #endregion
        #endregion

        #region AutoFill To Get City List Added By PJ

        public IActionResult OnGetCheckstatename(string statename)
        {
            TblState tblState = null;
            if (statename != null)
            {
                tblState = _context.TblStates.FirstOrDefault(x => x.IsActive == true && x.Name == statename);
            }
            return new JsonResult(tblState);
        }
        public IActionResult OnGetSearchCity(string term, string term2, string term3 = null)
        {
            if (term == null)
            {
                return BadRequest();
            }
            term = term.TrimStart(); ///use to trim blank space before character

            if (term == "#")
            {
                var list = _context.TblCities
                     .Where(s => s.StateId == Convert.ToInt32(term2))
                     .Select(s => new SelectListItem
                     {
                         Value = s.Name,
                         Text = s.CityId.ToString()
                     })
                     .ToArray();
                return new JsonResult(list);
            }
            else
            {
                var list = _context.TblCities
                        .Where(e => e.Name.Contains(term) && e.StateId == Convert.ToInt32(term2))
                        .Select(s => new SelectListItem
                        {
                            Value = s.Name,
                            Text = s.CityId.ToString()
                        })
                        .ToArray();
                return new JsonResult(list);
            }          
        }

        #region Check City is Valid Added By PJ
        public IActionResult OnGetCheckCity(string cityname, int StateId)
        {
            IList<string>? list = null;
            list = _commonManager.CheckCityisValid(cityname, StateId);
            return new JsonResult(list);
        }
        #endregion

        #endregion

        #region AutoFill To Get Pincode List Added By PJ
        public IActionResult OnGetSearchPincode(string term, string term2, string term3)
        {
            if (term == null)
            {
                return BadRequest();
            }
            term = term.TrimStart(); ///use to trim blank space before character

            if (term == "#")
            {
                var list = _context.TblPinCodes
                     .Where(s => s.IsActive == true && s.State == term2 && s.Location == term3)
                     .Select(s => new SelectListItem
                     {
                         Value = s.ZipCode.ToString(),
                         Text = s.Id.ToString()
                     })
                     .ToArray();
                return new JsonResult(list);
            }
            else
            {
                IEnumerable<SelectListItem>? pincodeList = null;
                var list = _context.TblPinCodes
                        .Where(e => e.IsActive == true && e.State == term2 && e.Location == term3)
                        //.Where(e => e.ZipCode == Convert.ToInt32(term) && e.IsActive == true && e.State == term2 && e.Location == term3)
                        .Select(s => new SelectListItem
                        {
                            Value = s.ZipCode.ToString(),
                            Text = s.Id.ToString()
                        })
                        .ToArray();
                pincodeList = list.OrderBy(o => o.Text).ToList();
                pincodeList = pincodeList.Where(x => x.Value.Contains(term)).ToList();
                return new JsonResult(pincodeList);
            }
        }

        public IActionResult OnGetCheckPincode(int Pincode,string Editcityname,string Editstatename)
        {
            TblPinCode tblPinCode = null;
            tblPinCode = _commonManager.CheckPincodeValid(Pincode, Editcityname, Editstatename);
            return new JsonResult(tblPinCode);
        }

        #endregion

        #region AutoFill To Get AreaLocality List Added By PJ
        public IActionResult OnGetAreaLocality(string term, int term2)
        {
            if (term == null)
            {
                return BadRequest();
            }
            term = term.TrimStart(); ///use to trim blank space before character

            if (term == "#")
            {
                var list = _context.TblAreaLocalities
                     .Where(s => s.IsActive == true && s.Pincode == term2)
                     .Select(s => new SelectListItem
                     {
                         Value = s.AreaLocality.ToString(),
                         Text = s.AreaId.ToString()
                     })
                     .ToArray();
                return new JsonResult(list);
            }
            else
            {
                var list = _context.TblAreaLocalities
                        .Where(e => e.AreaLocality == term && e.IsActive == true && e.Pincode == term2)
                        .Select(s => new SelectListItem
                        {
                            Value = s.AreaLocality.ToString(),
                            Text = s.AreaId.ToString()
                        })
                        .ToArray();
                return new JsonResult(list);
            }
        }

        public IActionResult OnGetCheckAreaLocality(string Arealocality)
        {
            TblAreaLocality tblAreaLocality = null;
            tblAreaLocality = _commonManager.CheckArealocaityValid(Arealocality);
            return new JsonResult(tblAreaLocality);
        }

        #endregion

        #region AutoFill To Get Product category List Added By PJ 
        public IActionResult OnGetSearchProdcat(string term)
        {
            if (term == null)
            {
                return BadRequest();
            }
            else
            {
                var list = _context.TblProductCategories
                     .Where(s => (s.Name.Contains(term) || term == "#") && s.IsActive == true)
                     .Select(s => new SelectListItem
                     {
                         Value = s.Description,
                         Text = s.Id.ToString()
                     })
                     .ToArray();
                return new JsonResult(list);
            }
        }

        public IActionResult OnGetCheckProdcat(string Prodcat)
        {
            IList<string>? list = null;
            list = _commonManager.CheckProdcatValid(Prodcat);
            return new JsonResult(list);
        }
        #endregion

        #region AutoFill To Get Product Type List Added By PJ 
        public IActionResult OnGetSearchProdType(string term, string term2)
        {
            if (term == null)
            {
                return BadRequest();
            }
            else
            {
                var list = _context.TblProductTypes
                     .Where(s => (s.Name.Contains(term) || term == "#") && s.IsActive == true && s.ProductCatId == Convert.ToInt32(term2))
                     .Select(s => new SelectListItem
                     {
                         Value = s.Description + "" + s.Size,
                         Text = s.Id.ToString()
                     })
                     .ToArray();
                return new JsonResult(list);
            }
        }

        public IActionResult OnGetCheckProdType(string ProdType)
        {
            IList<string>? list = null;
            list = _commonManager.CheckProdTypeValid(ProdType);
            return new JsonResult(list);
        }
        #endregion

        #region AutoFill To Get Brand List Added By PJ 
        public IActionResult OnGetSearchBrand(string term,int term2, int term3, int term4, int term5)
        {
            TblPriceMasterMapping tblPriceMasterMapping = new TblPriceMasterMapping();
            TblUniversalPriceMaster tblUniversalPriceMaster = new TblUniversalPriceMaster();
            List<SelectListItem> SelectListobject = null;

            if (term == null)
            {
                return BadRequest();
            }
            else
            {
                tblPriceMasterMapping = _priceMasterMappingRepository.GetProductPriceByBUIdBPId(term4, term5);
                if (tblPriceMasterMapping != null)
                {
                    tblUniversalPriceMaster = _context.TblUniversalPriceMasters
                     .Where(s => s.ProductCategoryId == term2 && s.ProductTypeId == term3 && s.IsActive == true || (s.PriceMasterNameId == tblPriceMasterMapping.PriceMasterNameId)).FirstOrDefault();

                    if (tblUniversalPriceMaster != null)
                    {
                        SelectListobject = new List<SelectListItem>
                        {
                            new SelectListItem { Value = tblUniversalPriceMaster.BrandName1, Text = "1" },
                            new SelectListItem { Value = tblUniversalPriceMaster.BrandName2, Text = "2" },
                            new SelectListItem { Value = tblUniversalPriceMaster.BrandName3, Text = "3" },
                            new SelectListItem { Value = tblUniversalPriceMaster.BrandName4, Text = "2008" }
                        };
                        return new JsonResult(SelectListobject);
                    }                    
                }
                return new JsonResult(SelectListobject);

            }
        }

        public IActionResult OnGetCheckBrand(string Brand)
        {
            IList<string>? list = null;
            list = _commonManager.CheckBrandValid(Brand);
            return new JsonResult(list);
        }
        #endregion

        #region Product Condition Dropdown
        public IActionResult OnGetProductConditionDropdownAsync(int term2,int term3)
        {
            //List<SelectListItem> SelectListobject = null;

            //SelectListobject = new List<SelectListItem>
            //             {
            //              new SelectListItem { Value = "Excellent", Text = "1" },
            //              new SelectListItem { Value = "Good", Text = "2" },
            //              new SelectListItem { Value = "Average", Text = "3" },
            //              new SelectListItem { Value = "Not Working", Text = "4" },
            //             };

            //return new JsonResult(SelectListobject);

            List<TblProductConditionLabel>? TblProductConditionLabel = null;
            List<SelectListItem> SelectListobject = new List<SelectListItem>(); 

            TblProductConditionLabel = _productConditionLabelRepository.GetProductConditionByBUBP(term2, term3);

            foreach (var item in TblProductConditionLabel)
            {
                SelectListobject.Add(new SelectListItem { Value = item.PclabelName, Text = item.Id.ToString() });
            }
            return new JsonResult(SelectListobject);
        }

        public IActionResult OnGetCheckProductcondition(string Productcondition)
        {
            if(Productcondition == "Excellent" && Productcondition == "Well Maintained" && Productcondition == "Well-maintained")
            {
                return new JsonResult(Productcondition);
            }else if(Productcondition == "Good" && Productcondition == "Working")
            {
                return new JsonResult(Productcondition);
            }else if(Productcondition == "Average" && Productcondition == "Heavily Used".ToLower())
            {
                return new JsonResult(Productcondition);
            }else if (Productcondition == "Not Working" && Productcondition == "Not working")
            {
                return new JsonResult(Productcondition);
            }
            else
            {
                return new JsonResult(null);
            }
                return new JsonResult(null);
        }

        #endregion

        #region Calculate QC price On product details change
        public JsonResult OnGetQCPriceAsync(string RegdNo, int ProdCatId, int ProdTypeId, string BrandId, int productconditionId, int BUId, int BPId, int StatusId,int Sweetener)
        {
            UniversalPMViewModel? universalPMView = null;
            QCProductPriceDetails productPriceDetails = new QCProductPriceDetails();
            PriceCalculate? priceCalculate = new PriceCalculate();
            TblExchangeOrder? tblExchangeOrder = null;
            TblPriceMasterMapping? tblPriceMasterMapping = null;
            TblProductConditionLabel tblProductConditionLabel = null;
            TblBrand? tblBrand = null;

            productPriceDetails.ProductCatId = ProdCatId;
            productPriceDetails.ProductTypeId = ProdTypeId;
            //productPriceDetails.BrandId = BrandId;
            productPriceDetails.BusinessUnitId= BUId;
            productPriceDetails.BusinessPartnerId = BPId;
            productPriceDetails.conditionId = productconditionId;
            productPriceDetails.RegdNo= RegdNo;

            tblPriceMasterMapping = _priceMasterMappingRepository.GetProductPriceByBUIdBPId(BUId, BPId);
            if(tblPriceMasterMapping != null)
            {
                if(BrandId != null)
                {
                    tblBrand = _context.TblBrands.FirstOrDefault(x => x.IsActive == true && x.Name == BrandId);
                }
                tblProductConditionLabel = _productConditionLabelRepository.GetOrderSequenceNo(productconditionId);

                productPriceDetails.PriceNameId = tblPriceMasterMapping.PriceMasterNameId;
                productPriceDetails.FinalProdQualityId = tblProductConditionLabel != null ? tblProductConditionLabel.OrderSequence : 0;
                productPriceDetails.BrandId= tblBrand!=null? tblBrand.Id : 0;

                universalPMView = _QcCommentManager.GetBasePrice(productPriceDetails);
                if (universalPMView != null)
                {
                    if (RegdNo != null)
                    {
                        tblExchangeOrder = _exchangeOrderRepository.GetExchOrderByRegdNo(RegdNo);
                        if (tblExchangeOrder != null)
                        {
                            if (new[] { Convert.ToInt32(OrderStatusEnum.Waitingforcustapproval), Convert.ToInt32(OrderStatusEnum.QCByPass), Convert.ToInt32(OrderStatusEnum.AmountApprovedbyCustomerAfterQC), Convert.ToInt32(OrderStatusEnum.EVCAllocationcompleted) }.Contains(tblExchangeOrder.StatusId.Value))
                            {
                                decimal sweetener = Sweetener != null && Sweetener > 0 ? Sweetener : 0;
                                priceCalculate.ExchangePrice = universalPMView.BaseValue + sweetener;
                            }
                            else
                            {
                                priceCalculate.ExchangePrice = universalPMView.BaseValue;
                            }
                        }
                    }
                }
            }
            return new JsonResult(priceCalculate);
        }
        #endregion
    }
}
