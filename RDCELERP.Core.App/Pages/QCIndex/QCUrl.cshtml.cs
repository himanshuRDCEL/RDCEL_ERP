using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using NPOI.OpenXmlFormats.Dml.Diagram;
using RestSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using RDCELERP.BAL.Helper;
using RDCELERP.BAL.Interface;
using RDCELERP.BAL.MasterManager;
using RDCELERP.Common.Constant;
using RDCELERP.Common.Enums;
using RDCELERP.Common.Helper;
using RDCELERP.Core.App.Pages.Base;
using RDCELERP.DAL.Entities;
using RDCELERP.DAL.IRepository;
using RDCELERP.Model;
using RDCELERP.Model.Base;
using RDCELERP.Model.CashfreeModel;
using RDCELERP.Model.Company;
using RDCELERP.Model.EVC;
using RDCELERP.Model.ExchangeOrder;
using RDCELERP.Model.ImageLabel;
using RDCELERP.Model.ImagLabel;
using RDCELERP.Model.LGC;
using RDCELERP.Model.ModelNumber;
using RDCELERP.Model.QC;
using RDCELERP.Model.QCComment;
using RDCELERP.Model.UniversalPriceMaster;
using RDCELERP.Model.UPIVerificationModel;
using static Org.BouncyCastle.Math.EC.ECCurve;
using static RDCELERP.Model.CashVoucher.VoucherSucessViewModel;
using static RDCELERP.Model.QCCommentViewModel;
using static RDCELERP.Model.Whatsapp.WhatsappPickupDateViewModel;
using static RDCELERP.Model.Whatsapp.WhatsappQCPriceViewModel;
using MediaToolkit;
using MediaToolkit.Model;
using MediaToolkit.Options;
using System.Collections;
using NPOI.SS.Formula.Functions;

namespace RDCELERP.Core.App.Pages.QCIndex
{
    public class QCUrlModel : BasePageModel
    {
        #region Variable Declaration
        private readonly IQCCommentManager _QcCommentManager;
        private readonly IUserManager _UserManager;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IStateManager _stateManager;
        private readonly ICityManager _cityManager;
        private readonly IBrandManager _brandManager;
        private readonly ILogisticManager _LogisticManager;
        private readonly IBusinessPartnerManager _businessPartnerManager;
        private readonly RDCELERP.DAL.Entities.Digi2l_DevContext _context;
        IABBRedemptionManager _aBBRedemptionManager;
        IOrderQCRepository _orderQCRepository;
        IExchangeOrderRepository _ExchangeOrderRepository;
        private CustomDataProtection _protector;
        IBusinessPartnerRepository _businessPartnerRepository;
        ICustomerDetailsRepository _customerDetailsRepository;
        IWhatsappNotificationManager _whatsappNotificationManager;
        IWhatsAppMessageRepository _whatsAppMessageRepository;
        ICommonManager _commonManager;
        INotificationManager _notificationManager;
        IMailManager _mailManager;
        IEmailTemplateManager _emailTemplateManager;
        IExchangeABBStatusHistoryRepository _exchangeABBStatusHistoryRepository;
        IImageHelper _imageHelper;
        IProductConditionLabelRepository _productConditionLabelRepository;
        IBusinessUnitRepository _businessUnitRepository;
        IOrderTransRepository _orderTransRepository;
        IOrderBasedConfigRepository _orderBasedConfigRepository;

        IUPIIdVerification _UpiIdVerification;
        ICashfreePayoutCall _cashfreePayoutCall;
        ILogging _logging;
        IOptions<ApplicationSettings> _config;
        IQCManager _qcManager;
        ITemplateConfigurationRepository _templateConfigurationRepository;
        ITempDataRepository _tempDataRepository;
        #endregion

        #region Constructor
        public QCUrlModel(CustomDataProtection protector, IExchangeOrderRepository ExchangeOrderRepository, IOrderQCRepository orderQCRepository, RDCELERP.DAL.Entities.Digi2l_DevContext context, IABBRedemptionManager aBBRedemptionManager, IBusinessPartnerManager businessPartnerManager, IBrandManager brandManager, IStateManager StateManager, ICityManager CityManager, IWebHostEnvironment webHostEnvironment, IOptions<ApplicationSettings> config, IUserManager userManager, IQCCommentManager QcCommentManager, ILogisticManager logisticManager, IBusinessPartnerRepository businessPartnerRepository, ICustomerDetailsRepository customerDetailsRepository, IWhatsappNotificationManager whatsappNotificationManager, IWhatsAppMessageRepository whatsAppMessageRepository, ICommonManager commonManager, INotificationManager notificationManager, IMailManager mailManager, IEmailTemplateManager emailTemplateManager, IExchangeABBStatusHistoryRepository exchangeABBStatusHistoryRepository, IImageHelper imageHelper, IProductConditionLabelRepository productConditionLabelRepository, IBusinessUnitRepository businessUnitRepository, IOrderTransRepository orderTransRepository, IOrderBasedConfigRepository orderBasedConfigRepository, IUPIIdVerification UpiIdVerification, ICashfreePayoutCall cashfreePayoutCall, ILogging logging, IQCManager qcManager, ITemplateConfigurationRepository templateConfigurationRepository, ITempDataRepository tempDataRepository)
            : base(config)
        {
            _ExchangeOrderRepository = ExchangeOrderRepository;
            _orderQCRepository = orderQCRepository;
            _webHostEnvironment = webHostEnvironment;
            _stateManager = StateManager;
            _cityManager = CityManager;
            _brandManager = brandManager;
            _businessPartnerManager = businessPartnerManager;
            _context = context;
            _UserManager = userManager;
            _QcCommentManager = QcCommentManager;
            _aBBRedemptionManager = aBBRedemptionManager;
            _LogisticManager = logisticManager;
            _protector = protector;
            _businessPartnerRepository = businessPartnerRepository;
            _customerDetailsRepository = customerDetailsRepository;
            _whatsappNotificationManager = whatsappNotificationManager;
            _whatsAppMessageRepository = whatsAppMessageRepository;
            _commonManager = commonManager;
            _notificationManager = notificationManager;
            _mailManager = mailManager;
            _emailTemplateManager = emailTemplateManager;
            _exchangeABBStatusHistoryRepository = exchangeABBStatusHistoryRepository;
            _imageHelper = imageHelper;
            _productConditionLabelRepository = productConditionLabelRepository;
            _businessUnitRepository = businessUnitRepository;
            _orderTransRepository = orderTransRepository;
            _orderBasedConfigRepository = orderBasedConfigRepository;
            _cashfreePayoutCall = cashfreePayoutCall;
            _UpiIdVerification = UpiIdVerification;
            _logging = logging;
            _config = config;
            _qcManager = qcManager;
            _templateConfigurationRepository = templateConfigurationRepository;
            _tempDataRepository = tempDataRepository;
        }
        #endregion

        #region Properties Declaration
        [BindProperty(SupportsGet = true)]
        public QCCommentViewModel? QCCommentViewModel { get; set; }
        [BindProperty(SupportsGet = true)]
        public List<BUBasedSweetnerValidation> bUBasedSweetnerValidations { get; set; }
        public TblExchangeOrder tblExchangeorder { get; set; }
        public TblOrderQc TblOrderQc { get; set; }
        public ExchangeOrderViewModel exchangeOrderViewModel { get; set; }
        public OrderImageUploadViewModel OrderImageUploadVM { get; set; }
        public IList<SelfQCViewModel> SelfQCVMList { get; set; }
        [BindProperty(SupportsGet = true)]
        public IList<ImageLabelViewModel> imageLabelViewModels { get; set; }
        public int login_id { get; set; }
        public VoucherDetailsViewModel voucherDetailsViewModel { get; set; }
        public List<SelectListItem> ProductQuality { get; set; }

        [BindProperty(SupportsGet = true)]
        public string? VideotimerSec { get; set; }

        [BindProperty(SupportsGet = true)]
        public string? MaxVideoFileSizeinMB { get; set; }
        public bool? IsSelfQCDone { get; set; }
        #endregion
        public IActionResult OnGet(int id)
        {
            string MVCExchangeInvoceImageURL = _baseConfig.Value.MVCBaseURLForExchangeInvoice;
            TblBusinessPartner? tblBusinessPartner = null;
            TblBusinessUnit? tblBusinessUnit = null;
            login_id = _loginSession.UserViewModel.UserId;
            List<TblConfiguration?> tblConfiguration = null;
            if (id > 0)
            {
                TblExchangeOrder exchangeOrder = _ExchangeOrderRepository.GetSingleOrder(Convert.ToInt32(id));
                
                if (exchangeOrder != null)
                {
                    List<TblProductConditionLabel>? tblProductConditionLabel = null;

                    exchangeOrderViewModel = new ExchangeOrderViewModel();
                    exchangeOrderViewModel.exchangeId = _protector.Encode(id);
                    QCCommentViewModel = _QcCommentManager.GetQCByExchangeId(Convert.ToInt32(id));
                    List<TblExchangeOrderStatus> exchangeOrderStatuses = _aBBRedemptionManager.GetExchangeOrderStatusByDepartment("QC");
                    if (exchangeOrderStatuses != null && exchangeOrderStatuses.Count > 0)
                    {
                        QCCommentViewModel.StatusList = exchangeOrderStatuses.Select(o => new SelectListItem
                        {
                            Text = o.StatusCode,
                            Value = o.Id.ToString()
                        }).ToList();
                    }
                    tblBusinessPartner = _businessPartnerRepository.GetBPId(exchangeOrder.BusinessPartnerId);
                    if (QCCommentViewModel != null && QCCommentViewModel.ExchangeOrderViewModel != null && QCCommentViewModel.ExchangeOrderViewModel.IsDefferedSettlement == null)
                    {
                        if (tblBusinessPartner != null && tblBusinessPartner.IsDefferedSettlement != null)
                            QCCommentViewModel.ExchangeOrderViewModel.IsDefferedSettlement = tblBusinessPartner.IsDefferedSettlement == true ? "1" : "0";
                    }
                    if(QCCommentViewModel != null && tblBusinessPartner != null && tblBusinessPartner.BusinessUnitId != null)
                    {
                        tblBusinessUnit = _businessUnitRepository.Getbyid(tblBusinessPartner.BusinessUnitId);
                        if(tblBusinessUnit != null)
                        {
                            if(tblBusinessUnit.IsInvoiceDetailsRequired == true)
                            {
                                QCCommentViewModel.NewProductInvoiceImage = MVCExchangeInvoceImageURL + (QCCommentViewModel.ExchangeOrderViewModel != null ? QCCommentViewModel.ExchangeOrderViewModel.InvoiceImageName:"");
                                QCCommentViewModel.IsInvoiceDetailsRequired = true;
                            }
                            if(tblBusinessUnit.IsModelDetailRequired == true)
                            {
                                QCCommentViewModel.IsModelRequired = true;
                            }
                            else
                            {
                                QCCommentViewModel.IsModelRequired = false;
                            }
                            if (tblBusinessUnit.IsBumultiBrand == true)
                            {
                                QCCommentViewModel.IsBumultiBrand = true;
                            }
                            else
                            {
                                QCCommentViewModel.IsBumultiBrand = false;
                            }
                        }
                    }

                    voucherDetailsViewModel = _QcCommentManager.GetVoucherDetails(exchangeOrder.Id);

                    #region Get Product condition on BU & BP base
                    if(exchangeOrder.BusinessPartnerId != null && exchangeOrder.BusinessUnitId != null)
                    {
                        tblProductConditionLabel = _productConditionLabelRepository.GetProductConditionByBUBP(exchangeOrder.BusinessPartnerId, exchangeOrder.BusinessUnitId, QCCommentViewModel?.ProductCategoryId);
                        ProductQuality = tblProductConditionLabel.Select(o => new SelectListItem
                        {
                            Text = o.PclabelName,
                            Value = o.Id.ToString()
                        }).ToList();
                    }
                    #endregion

                    #region Code for Get Self QC Images 
                    if (exchangeOrder != null && !string.IsNullOrEmpty(exchangeOrder.RegdNo))
                    {
                        string? url = _baseConfig.Value.BaseURL;
                        SelfQCVMList = _QcCommentManager.GetImagesUploadedBySelfQC(exchangeOrder.RegdNo);

                        //Temporary
                        imageLabelViewModels = _LogisticManager.GetImageLabelUploadByProductCat(exchangeOrder.RegdNo);
                        if (imageLabelViewModels == null)
                        {
                            imageLabelViewModels = new List<ImageLabelViewModel>();
                        }
                        else
                        {
                            foreach (var item in imageLabelViewModels)
                            {
                                if (item.IsMediaTypeVideo == true)
                                {
                                    //isVideoTrue = item.ProductImageLabel != null ? item.ProductImageLabel : "";
                                    //tblConfiguration = _context.TblConfigurations.Where(x => x.IsActive == true && x.Name == "MaxVideoFileSizeMB").FirstOrDefault();
                                    //MaxVideoFileSizeinMB = tblConfiguration.Value != null ? tblConfiguration.Value : 20.ToString();                                        
                                    tblConfiguration = _templateConfigurationRepository.GetConfigurationList();
                                    if (tblConfiguration != null)
                                    {
                                        foreach(var item2 in tblConfiguration)
                                        {
                                            if(item2.Name == ConfigurationEnum.VideoRecordingTimerSec.ToString())
                                            {
                                                VideotimerSec = item2.Value;
                                            }   
                                            if(item2.Name == ConfigurationEnum.MaxVideoFileSizeMB.ToString())
                                            {
                                                MaxVideoFileSizeinMB = item2.Value;
                                            }
                                        }
                                        
                                    }
                                }

                                //string productLabel = item.ProductImageLabel.ToLower();
                                //if (productLabel.Contains("video"))
                                //{
                                //    isVideoTrue = item.ProductImageLabel;
                                //}                             
                            }
                        }

                        #region Display Self QC Images
                        //bool IsSelfQCDone = false;
                        IsSelfQCDone = _qcManager.verifyDuplicateSelfQC(exchangeOrder.RegdNo);
                        if (IsSelfQCDone == true)
                        {
                            if (SelfQCVMList != null && SelfQCVMList.Count > 0)
                            {
                                foreach (var item in SelfQCVMList)
                                {
                                    if (item != null)
                                    {
                                        item.FilePath = EnumHelper.DescriptionAttr(FileAddressEnum.SelfQC);
                                        item.ImageWithPath = url + item.FilePath + item.ImageName;
                                        var extn = item?.ImageName?.Split(".").Last();
                                        if (extn == "webm")
                                        {
                                            item.IsMediaTypeVideo = true;
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            SelfQCVMList = new List<SelfQCViewModel>();
                            SelfQCViewModel? selfQCViewModel = null;
                            ArrayList list = new ArrayList();
                            list = GetMediaFiles(exchangeOrder.RegdNo);
                            if (list != null && list.Count > 0)
                            {
                                foreach (var item in list)
                                {
                                    selfQCViewModel = new SelfQCViewModel();
                                    selfQCViewModel.ImageWithPath = item?.ToString();
                                    var extn = selfQCViewModel.ImageWithPath?.Split(".").Last();
                                    if (extn == "webm")
                                    {
                                        selfQCViewModel.IsMediaTypeVideo = true;
                                    }
                                    SelfQCVMList.Add(selfQCViewModel);
                                }
                            }
                        }
                        #endregion
                    }
                    TblOrderBasedConfig tblOrderBased = _orderBasedConfigRepository.GetSingle(x=>x.IsActive==true && x.BusinessUnitId == exchangeOrder?.BusinessUnitId&& x.BusinessPartnerId== exchangeOrder?.BusinessPartnerId);
                    if (tblOrderBased !=null && tblOrderBased.IsValidationBasedSweetener == true)
                    {                                                                   
                        bUBasedSweetnerValidations = _QcCommentManager.GetBUSweetnerValidationQuestions(exchangeOrder.BusinessPartner.BusinessUnit.BusinessUnitId);
                        if (bUBasedSweetnerValidations.Count > 0)
                        {
                            QCCommentViewModel.IsValidationBasedSweetner = true;
                            foreach (var item in bUBasedSweetnerValidations)
                            {
                                if (item.QuestionKey == "InvoiceV")
                                {
                                    QCCommentViewModel.IsInvoiceEnabale = true;
                                    QCCommentViewModel.InvoiceImageName = _baseConfig.Value.MVCBaseURLForExchangeInvoice + exchangeOrder.InvoiceImageName;
                                }
                                if (item.QuestionKey == "InstallationV")
                                {
                                    QCCommentViewModel.IsInstallationEnabale = true;
                                    QCCommentViewModel.IsInstallationValidated = false;
                                    List<TblExchangeAbbstatusHistory> tblExchangeAbbstatusHistories = _exchangeABBStatusHistoryRepository.GetList(x => x.IsActive == true && x.RegdNo == exchangeOrder.RegdNo).ToList();
                                    if (tblExchangeAbbstatusHistories != null)
                                    {
                                        foreach (TblExchangeAbbstatusHistory x in tblExchangeAbbstatusHistories)
                                        {
                                            if (x.StatusId == Convert.ToInt32(OrderStatusEnum.InstalledbySponsor))
                                            {
                                                QCCommentViewModel.IsInstallationValidated = true;
                                            }
                                        }
                                    }
                                }
                            }

                        }
                        else
                        {
                            QCCommentViewModel.IsValidationBasedSweetner = true;
                        }
                    }
                    #endregion
                }
            }

            var statusflag = _QcCommentManager.GetQcFlag();
            if (statusflag != null)
            {
                ViewData["statusflag"] = new SelectList(statusflag, "Id", "CombinedDisplay");
            }          

            if (_loginSession == null)
            {
                return RedirectToPage("/index");
            }
            else
            {
                return Page();
            }
        }
        public IActionResult OnPostAsync(IFormFile? InvoiceImage)
        {
            int result = 0;
            bool imgSave = false;
            string EmailbaseUrl = _baseConfig.Value.BaseURL + "EmailTemplate/ERPEmailTemplate";
            if (ModelState.IsValid)
            {
                if (QCCommentViewModel != null)
                {
                    if (InvoiceImage != null)
                    {
                        string fileName = string.Empty;
                        if (QCCommentViewModel.ExchangeOrderViewModel.InvoiceImageName != null)
                        {
                            fileName = QCCommentViewModel.ExchangeOrderViewModel.InvoiceImageName;
                        }
                        else
                        {
                            fileName = QCCommentViewModel.ExchangeOrderViewModel.RegdNo + DateTime.Now.ToString("yyyyMMddHHmmssFFF") + Path.GetExtension("image.jpeg");                            
                        }

                        string filePath = _baseConfig.Value.MVCPhysicalURL;

                        bool cencelCheck = _imageHelper.SaveFileDefRoot(InvoiceImage, filePath, fileName);
                        if (cencelCheck)
                        {
                            QCCommentViewModel.InvoiceImageName = fileName;
                            cencelCheck = false;
                        }
                    }
                    if (QCCommentViewModel.IsValidationBasedSweetner == true)
                    {
                        QCCommentViewModel.IsInvoiceValidated = QCCommentViewModel.InvoiceValidated == "Yes" ? true : false;
                    }

                    if (QCCommentViewModel.QualityAfterQC != null)
                    {
                        int qualityid = Convert.ToInt32(QCCommentViewModel.QualityAfterQC);
                        TblProductConditionLabel tblProductConditionLabel = _productConditionLabelRepository.GetOrderSequenceNo(qualityid);
                        if (tblProductConditionLabel != null)
                        {
                            switch (tblProductConditionLabel.OrderSequence)
                            {
                                case 1:
                                    QCCommentViewModel.QualityAfterQC = "Excellent";
                                    break;
                                case 2:
                                    QCCommentViewModel.QualityAfterQC = "Good";
                                    break;
                                case 3:
                                    QCCommentViewModel.QualityAfterQC = "Average";
                                    break;
                                case 4:
                                    QCCommentViewModel.QualityAfterQC = "Not Working";
                                    break;
                                default:
                                    break;
                            }
                        }
                    }
                    #region Replace Invoice image of order by Pooja Jatav
                    //if (QCCommentViewModel.Base64StringValue != null)
                    //{
                    //    string fileName = string.Empty;
                    //    if (QCCommentViewModel.ExchangeOrderViewModel.InvoiceImageName != null)
                    //    {
                    //        fileName = QCCommentViewModel.ExchangeOrderViewModel.InvoiceImageName;
                    //    }
                    //    else
                    //    {
                    //        fileName = QCCommentViewModel.ExchangeOrderViewModel.RegdNo + DateTime.Now.ToString("yyyyMMddHHmmssFFF") + Path.GetExtension("image.jpeg");                            
                    //    }

                    //    string filePath = EnumHelper.DescriptionAttr(FilePathEnum.ExchangeInvoiceImage);
                    //    imgSave = _imageHelper.SaveFileFromBase64(QCCommentViewModel.Base64StringValue, filePath, fileName);
                    //}
                    #endregion

                    result = _QcCommentManager.ManageQCComment(QCCommentViewModel, imageLabelViewModels, Convert.ToInt32(_loginSession.UserViewModel.UserId));
                    if (result > 0)
                    {
                        UPIVerification root = new UPIVerification();
                        CashfreeAuth cashfreeAuthCall = new CashfreeAuth();
                        AddBeneficiary addBeneficiarry = new AddBeneficiary();
                        string vpaValid = EnumHelper.DescriptionAttr(UPIResponseEnum.vpaValid);
                        AddBeneficiaryResponse beneficiaryResponse = new AddBeneficiaryResponse();
                        string subcode = Convert.ToInt32(CashfreeEnum.Succcess).ToString();
                        TblExchangeOrder? tblExchangeOrder = null;
                        TblBusinessPartner? tblBusinessPartner = null;
                        TblOrderTran? tblOrderTran = null;
                        TblCustomerDetail? tblCustomerDetail = null;
                        TblWhatsAppMessage? tblwhatsappmessage = null;
                        string message = string.Empty;
                        string smsmessage = string.Empty;
                        bool flag = false;
                        bool EmailResponse = false;
                        bool isupirequired = false;
                        string baseUrl = string.Empty;
                        string url = string.Empty;
                        var enableEvcAutoAllocation = _config.Value.EvcAutoAllocation;
                        
                        if (QCCommentViewModel.StatusId == (int)OrderStatusEnum.Waitingforcustapproval || QCCommentViewModel.StatusId == (int)OrderStatusEnum.QCByPass)
                        {
                            tblExchangeOrder = _ExchangeOrderRepository.GetRegdNo(QCCommentViewModel.ExchangeOrderViewModel.RegdNo);
                            if (tblExchangeOrder != null)
                            {
                                tblOrderTran = _orderTransRepository.GetOrderTransByRegdno(tblExchangeOrder.RegdNo);
                                if (tblOrderTran != null)
                                {
                                    isupirequired = _commonManager.CheckUpiisRequired(tblOrderTran.OrderTransId);
                                }

                                tblCustomerDetail = _customerDetailsRepository.GetCustDetails(tblExchangeOrder.CustomerDetailsId);

                                #region whatsappNotification for UPI No
                                TblBusinessUnit businessUnit = _businessUnitRepository.GetSingle(x => x.BusinessUnitId == tblExchangeOrder.BusinessUnitId && x.IsActive == true);
                                if (businessUnit != null && businessUnit.IsBulkOrder == true)
                                {
                                    return RedirectToPage("/QCIndex/OrdersForQC");
                                }
                                else

                                {
                                    if (isupirequired == true)
                                    {
                                        WhatasappResponse whatasappResponse = new WhatasappResponse();


                                        WhatsappTemplate whatsappObj = new WhatsappTemplate();
                                        whatsappObj.userDetails = new UserDetails();
                                        whatsappObj.notification = new QCFinalPrice();
                                        whatsappObj.notification.@params = new SendDate();
                                        whatsappObj.userDetails.number = tblCustomerDetail.PhoneNumber;
                                        whatsappObj.notification.sender = _baseConfig.Value.YelloaiSenderNumber;
                                        whatsappObj.notification.type = _baseConfig.Value.YellowaiMesssaheType;
                                        whatsappObj.notification.@params.Customername = tblCustomerDetail.FirstName + " " + tblCustomerDetail.LastName;
                                        whatsappObj.notification.templateId = NotificationConstants.WaitingForPrice_Approval_instant_settlement;// deferred_settlement Template
                                        if (QCCommentViewModel.StatusId == (int)OrderStatusEnum.QCByPass)
                                        {
                                            whatsappObj.notification.@params.FinalQcPrice = tblExchangeOrder.ExchangePrice;
                                        }
                                        else
                                        {

                                            whatsappObj.notification.@params.FinalQcPrice = QCCommentViewModel.PriceAfterQC;
                                        }

                                        baseUrl = _baseConfig.Value.BaseURL + "PaymentDetails/ConfirmPaymentDetails?regdNo=" + QCCommentViewModel.ExchangeOrderViewModel.RegdNo + "&status=" + QCCommentViewModel.StatusId;
                                        whatsappObj.notification.@params.PageLink = baseUrl;
                                        url = _baseConfig.Value.YellowAiUrl;


                                        RestResponse response = _whatsappNotificationManager.Rest_InvokeWhatsappserviceCall(url, Method.Post, whatsappObj);
                                        if (response.Content != null)
                                        {
                                            tblwhatsappmessage = new TblWhatsAppMessage();
                                            whatasappResponse = JsonConvert.DeserializeObject<WhatasappResponse>(response.Content);
                                            //tblwhatsappmessage.TemplateName = NotificationConstants.WaitingForPrice_Approval_deferred_settlement;
                                            tblwhatsappmessage.TemplateName = whatsappObj.notification.templateId;
                                            tblwhatsappmessage.IsActive = true;
                                            tblwhatsappmessage.PhoneNumber = tblCustomerDetail.PhoneNumber;
                                            tblwhatsappmessage.SendDate = DateTime.Now;
                                            tblwhatsappmessage.MsgId = whatasappResponse.msgId;
                                            _whatsAppMessageRepository.Create(tblwhatsappmessage);
                                            _whatsAppMessageRepository.SaveChanges();
                                        }
                                    }
                                    else
                                    {
                                        WhatsAppResponse whatsAppResponse = new WhatsAppResponse();

                                        WhatsappPickUpDateTemplate whatsappPickUpDateObj = new WhatsappPickUpDateTemplate();
                                        whatsappPickUpDateObj.userDetails = new PickUpDateUserDetails();
                                        whatsappPickUpDateObj.notification = new WhatsappPickupDateVM();
                                        whatsappPickUpDateObj.notification.@params = new SendPickUpDate();
                                        whatsappPickUpDateObj.userDetails.number = tblCustomerDetail.PhoneNumber;
                                        whatsappPickUpDateObj.notification.sender = _baseConfig.Value.YelloaiSenderNumber;
                                        whatsappPickUpDateObj.notification.type = _baseConfig.Value.YellowaiMesssaheType;
                                        whatsappPickUpDateObj.notification.@params.Customername = tblCustomerDetail.FirstName + " " + tblCustomerDetail.LastName;
                                        whatsappPickUpDateObj.notification.templateId = NotificationConstants.WaitingForPrice_Approval_deferred_settlement; // instant_settlement Template

                                        baseUrl = _baseConfig.Value.BaseURL + "PaymentDetails/ConfirmPaymentDetails?regdNo=" + QCCommentViewModel.ExchangeOrderViewModel.RegdNo + "&status=" + QCCommentViewModel.StatusId;
                                        whatsappPickUpDateObj.notification.@params.PageLink = baseUrl;
                                        url = _baseConfig.Value.YellowAiUrl;

                                        RestResponse response = _whatsappNotificationManager.Rest_InvokeWhatsappserviceCall(url, Method.Post, whatsappPickUpDateObj);

                                        if (response.Content != null)
                                        {
                                            tblwhatsappmessage = new TblWhatsAppMessage();
                                            whatsAppResponse = JsonConvert.DeserializeObject<WhatsAppResponse>(response.Content);
                                            //tblwhatsappmessage.TemplateName = NotificationConstants.WaitingForPrice_Approval_deferred_settlement;
                                            tblwhatsappmessage.TemplateName = whatsappPickUpDateObj.notification.templateId;
                                            tblwhatsappmessage.IsActive = true;
                                            tblwhatsappmessage.PhoneNumber = tblCustomerDetail.PhoneNumber;
                                            tblwhatsappmessage.SendDate = DateTime.Now;
                                            tblwhatsappmessage.MsgId = whatsAppResponse.msgId;
                                            _whatsAppMessageRepository.Create(tblwhatsappmessage);
                                            _whatsAppMessageRepository.SaveChanges();
                                        }
                                    }
                                }
                               
                                #endregion

                                #region Send SMS and Email
                                //smsmessage = NotificationConstants.UpiId_Verfication_SMS.Replace("(customer name)", tblCustomerDetail.FirstName + " " + tblCustomerDetail.LastName).Replace("[Price]", (QCCommentViewModel.PriceAfterQC).ToString()).Replace("[link]", baseUrl);
                                //flag = _notificationManager.SendQCSMS(tblCustomerDetail.PhoneNumber, smsmessage);

                                //string subject = "UPI and Pick-up details";
                                //string htmlbody = NotificationConstants.UpiId_Verfication_Email.Replace("[Customername]", tblCustomerDetail.FirstName + " " + tblCustomerDetail.LastName).Replace("[Priceafterqc]", (QCCommentViewModel.PriceAfterQC).ToString()).Replace("[link]", baseUrl);
                                //string too = tblCustomerDetail.Email;

                                //EmailResponse = _emailTemplateManager.CommonEmailBody(too, htmlbody, subject);
                                #endregion

                                #region Send SMS and Email for Discount Voucher
                                //tblBusinessPartner = _businessPartnerRepository.GetBPId(tblExchangeOrder.BusinessPartnerId);
                                //if (tblBusinessPartner != null && tblBusinessPartner.IsVoucher == true && tblBusinessPartner.VoucherType == 1)
                                //{                               
                                //    smsmessage = NotificationConstants.PickUp_Details_SMS.Replace("(customer name)", tblCustomerDetail.FirstName + " " + tblCustomerDetail.LastName).Replace("[link]", baseUrl);
                                //    flag = _notificationManager.SendQCSMS(tblCustomerDetail.PhoneNumber, smsmessage);
                                //    string Pickupsubject = "Pick-up details";
                                //    string Content = NotificationConstants.PickUp_Details_Email.Replace("[Customername]", tblCustomerDetail.FirstName + " " + tblCustomerDetail.LastName).Replace("[link]", baseUrl);
                                //    string Sendtoo = tblCustomerDetail.Email;
                                //    EmailResponse = _emailTemplateManager.CommonEmailBody(Sendtoo, Content, Pickupsubject);                               
                                //}
                                #endregion
                            }
                        }                       
                    }
                }
            }
            
            if (result > 0)
                return RedirectToPage("/QCIndex/OrdersForQC");
            else
                return RedirectToPage("/Exchange/Index");
        }
        public IActionResult OnPostBackonExchange()
        {
            return RedirectToPage("/Exchange/Index", new { id = "exchangeOrderViewModel.exchangeId" });
        }

        #region Get Qc Details by regdno Added By Pooja Jatav
        public JsonResult OnGetEditByRegdNoAsync()
        {
            return new JsonResult(_QcCommentManager.GetQCByExchangeId(Convert.ToInt32(QCCommentViewModel.ExchangeOrderViewModel.RegdNo)));
        }
        #endregion     

        #region SaveSelfQCImageAsFinalQCImage 
        public JsonResult OnPostSaveSelfQCImageAsFinalQCImage(string RegdNo)
        {
            return new JsonResult(_QcCommentManager.saveSelfQCImageAsFinalImage(RegdNo, Convert.ToInt32(_loginSession.UserViewModel.UserId)));
        }
        #endregion
        public JsonResult OnGetValidationBasedSweetnerQCQualityAsync(string ExchangeRegdno, int ProdConditionId, string custQuality, int newbrandid, int Modelnoid, string invoiceV, string installationV)
        {
            bool InvoiceV = invoiceV == "Yes" ? true : false;
            bool InstallationV = installationV == "Yes" ? true : false;
            UniversalPMViewModel? universalPMView = null;

            universalPMView = _QcCommentManager.ValidationbasedGetPriceAfterQc(ExchangeRegdno, ProdConditionId, custQuality, Convert.ToInt32(_loginSession.UserViewModel.UserId), InvoiceV, InstallationV,newbrandid,Modelnoid);
            return new JsonResult(universalPMView);
        }

        #region Get Price AfterQc by product condition Added by Pooja Jatav
        public JsonResult OnGetQCQualityAsync(string ExchangeRegdno, int ProdConditionId, string custQuality, int newbrandid, int Modelnoid)
        {
            List<decimal> Qcprice = new List<decimal>();
            UniversalPMViewModel? universalPMView = null;
            QCProductPriceDetails qCProductPriceDetails = new QCProductPriceDetails();
            qCProductPriceDetails.RegdNo = ExchangeRegdno;
            qCProductPriceDetails.conditionId = ProdConditionId;
            qCProductPriceDetails.CustQuality = custQuality;
            qCProductPriceDetails.NewBrandId = newbrandid;
            qCProductPriceDetails.NewModelId = Modelnoid;
            universalPMView = _QcCommentManager.GetProductPrice(qCProductPriceDetails);
            if(universalPMView != null)
            {
                if (universalPMView.BaseValue == null)
                { universalPMView.BaseValue = 0;
                    universalPMView.FinalQCPrice = 0;}
                else
                {
                    universalPMView.BaseValue = Convert.ToInt32(universalPMView.BaseValue);
                }

                if (universalPMView.FinalQCPrice == null)
                { universalPMView.FinalQCPrice = Convert.ToInt32(universalPMView.BaseValue); }
                else
                {
                    universalPMView.FinalQCPrice = Convert.ToInt32(universalPMView.FinalQCPrice);
                }

                if (universalPMView.TotalSweetener == null)
                { universalPMView.TotalSweetener = 0; }

                if (universalPMView.CollectedAmount == null)
                { universalPMView.CollectedAmount = 0; }

                if (universalPMView.SweetenerBU == null)
                { universalPMView.SweetenerBU = 0; }

                if (universalPMView.SweetenerBP == null)
                { universalPMView.SweetenerBP = 0; }

                if (universalPMView.SweetenerDigi2l == null)
                { universalPMView.SweetenerDigi2l = 0; }
               
            }
            else
            {
                universalPMView = new UniversalPMViewModel();
            }
            

            return new JsonResult(universalPMView);
        }
        #endregion

        #region Autocomplete for Brand Name and Model Name Added by Pooja Jatav
        public IActionResult OnGetSearchBrandName(string term)
        {
            if (term == null)
            {
                return BadRequest();
            }
            var data = _context.TblBrands
                       .Where(s => s.Name.Contains(term) || term == "#")
                       .Select(s => new SelectListItem
                       {
                           Value = s.Name,
                           Text = s.Id.ToString()
                       })
                       .ToArray();
            return new JsonResult(data);
        }

        public IActionResult OnGetSearchModelNo(string term, string term2, string term3, string term4)
        {
            if (term == null)
            {
                return BadRequest();
            }
            var data = _context.TblModelMappings
                       .Include(s => s.Model)
                       .Where(s => (s.Model != null && s.Model.ProductTypeId == Convert.ToInt32(term2) && s.BusinessPartnerId == Convert.ToInt32(term3) && s.BusinessUnitId == Convert.ToInt32(term4)) && ( term == "#") || (s.Model.ModelName ?? "").Contains(term))
                       //.Where(s => (s.Model != null && (s.Model.ModelName??"").Contains(term) && s.Model.ProductTypeId == Convert.ToInt32(term2) && s.BusinessPartnerId == Convert.ToInt32(term3) && s.BusinessUnitId == Convert.ToInt32(term4)) || term == "#")
                       .Select(s => new SelectListItem
                       {
                           Value = s.Model.ModelName,
                           Text = s.Model.ModelNumberId.ToString()
                       })
                       .ToArray();
            //var data = _context.TblModelNumbers
            //           .Where(s => s.ModelName.Contains(term) || s.ProductTypeId == Convert.ToInt32(term2) || term == "#")
            //           .Select(s => new SelectListItem
            //           {
            //               Value = s.ModelName,
            //               Text = s.ModelNumberId.ToString()
            //           })
            //           .ToArray();
            return new JsonResult(data);
        }

        #endregion

        #region Video Compressor Added by Pooja Jatav
        [ValidateAntiForgeryToken]
        public JsonResult OnPostCompressVideo(string fileName, string regdNo,bool isMediaTypeVideo, int srNum)
        {
            try
            {
                string bytedata = null;
                bool videoSave = false;
                if (fileName != null)
                {
                    byte[] videobyte = Convert.FromBase64String(fileName);

                    string documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                    string tempInputFilePath = System.IO.Path.Combine(documentsPath, "input_video.mp4");
                    System.IO.File.WriteAllBytes(tempInputFilePath, videobyte);
                    string outputFilePath = System.IO.Path.Combine(documentsPath, "compressed_video.mp4");
                    var inputFile = new MediaFile { Filename = tempInputFilePath };
                    var outputFile = new MediaFile { Filename = outputFilePath };
                    //using (var engine = new Engine())
                    using (var engine = new Engine())
                    {
                        engine.GetMetadata(inputFile);
                        var options = new ConversionOptions
                        {
                            VideoAspectRatio = VideoAspectRatio.R3_2,
                            VideoSize = VideoSize.Hd480,
                            //AudioSampleRate = AudioSampleRate.Hz22050,
                            VideoBitRate = 620,
                            VideoFps = 54,
                        };
                        engine.Convert(inputFile, outputFile, options);
                    }

                    Task<byte[]> compressedfile = System.IO.File.ReadAllBytesAsync(outputFilePath);

                    System.IO.File.Delete(tempInputFilePath);
                    System.IO.File.Delete(outputFilePath);

                    if (compressedfile != null)
                    {
                        if (compressedfile.Result.Length > 0)
                        {
                            string videoBase64 = Convert.ToBase64String(compressedfile.Result);
                            string FileName = regdNo + "_" + "FinalQCVideo_" + srNum + ".webm";
                            string filePath = EnumHelper.DescriptionAttr(FilePathEnum.VideoQC);
                            videoSave = _imageHelper.SaveVideoFileFromBase64(videoBase64, filePath, FileName);
                        }
                    }

                    //bytedata = Convert.ToBase64String(compressedfile.Result);
                    bytedata = "videofilesaved";
                    return new JsonResult(bytedata);
                }
                else
                {
                    return new JsonResult(fileName);
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        #endregion

        #region Get Media Files
        //public JsonResult OnPostGetMediaFiles(string? regdNo)
        //{
        //    bool flag = false;
        //    string[]? files = null;
        //    var fileName = "";
        //    var count = 1;
        //    List<ImageLabelViewModel>? imageLabelViewModels = null;
        //    JsonResult jsonResult = null;
        //    try
        //    {
        //        var placeholderPath = "";
        //        string? baseUrl = _baseConfig.Value.BaseURL;
        //        string documentsPath = string.Concat(baseUrl, "DBFiles/QC/SelfQC/");
        //        //string tempInputFilePath = System.IO.Path.Combine(documentsPath, fileName);
        //        string fullFilePath = "";
        //        string[]? fullFilePathArray = null;
        //        ArrayList list = new ArrayList();
        //        files = _QcCommentManager.OnPostGetMediaFiles(regdNo);
        //        imageLabelViewModels = _qcManager.GetQCImageLabels(regdNo);
        //        if (imageLabelViewModels != null && imageLabelViewModels.Count > 0)
        //        {
        //            foreach (var imageLabel in imageLabelViewModels)
        //            {
        //                if (imageLabel != null)
        //                {
        //                    if (imageLabel.IsMediaTypeVideo == true)
        //                    {
        //                        fileName = regdNo + "_" + "Video_" + count + ".webm";
        //                        placeholderPath = "Images/VIDEO-placeholder.png";
        //                    }
        //                    else
        //                    {
        //                        fileName = regdNo + "_" + "Image_" + count + ".jpg";
        //                        placeholderPath = "Images/placeholder-01.png";
        //                    }
        //                    var file = files.FirstOrDefault(x => x.Equals(fileName));
        //                    if (file != null)
        //                    {
        //                        fullFilePath = string.Concat(documentsPath, fileName);
        //                    }
        //                    else
        //                    {
        //                        fullFilePath = string.Concat(baseUrl, placeholderPath);
        //                    }
        //                    list.Add(fullFilePath);
        //                }
        //                count++;
        //            }
        //        }
        //        jsonResult = new JsonResult(list);
        //    }
        //    catch (Exception ex)
        //    {
        //    }
        //    return jsonResult;
        //}

        public JsonResult OnPostGetMediaFiles(string? regdNo)
        {
            ArrayList list = new ArrayList();
            JsonResult jsonResult = null;
            try
            {
                if (!string.IsNullOrEmpty(regdNo))
                {
                    list = GetMediaFiles(regdNo);
                }
                jsonResult = new JsonResult(list);
            }
            catch (Exception ex)
            {
            }
            return jsonResult;
        }

        public ArrayList GetMediaFiles(string? regdNo)
        {
            var fileName = "";
            var count = 1;
            List<ImageLabelViewModel>? imageLabelViewModels = null;
            List<TblTempDatum>? tempData = null;
            var placeholderPath = "";
            string fullFilePath = "";
            ArrayList list = new ArrayList();
            try
            {

                string? baseUrl = _baseConfig.Value.BaseURL;
                string documentsPath = string.Concat(baseUrl, "DBFiles/QC/SelfQC/");
                tempData = _tempDataRepository.GetMediaFilesTempDataList(regdNo);
                imageLabelViewModels = _qcManager.GetQCImageLabels(regdNo);
                if (imageLabelViewModels != null && imageLabelViewModels.Count > 0)
                {
                    foreach (var imageLabel in imageLabelViewModels)
                    {
                        if (imageLabel != null)
                        {
                            if (imageLabel.IsMediaTypeVideo == true)
                            {
                                fileName = regdNo + "_Video_" + count + ".webm";
                            }
                            else
                            {
                                fileName = regdNo + "_Image_" + count + ".jpg";
                            }
                            var tempDataObj = tempData?.FirstOrDefault(x => x.FileName == fileName);
                            if (tempDataObj != null && !string.IsNullOrEmpty(tempDataObj.FileName))
                            {
                                fullFilePath = string.Concat(documentsPath, tempDataObj.FileName);
                            }
                            else
                            {
                                if (imageLabel.IsMediaTypeVideo == true)
                                {
                                    placeholderPath = "Images/VIDEO-placeholder.png";
                                }
                                else
                                {
                                    placeholderPath = "Images/placeholder-01.png";
                                }
                                fullFilePath = string.Concat(baseUrl, placeholderPath);
                            }
                            list.Add(fullFilePath);
                        }
                        count++;
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return list;
        }
        #endregion
    }
}
