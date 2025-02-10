using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RestSharp;
using RDCELERP.BAL.Helper;
using RDCELERP.BAL.Interface;
using RDCELERP.BAL.MasterManager;
using RDCELERP.Common.Constant;
using RDCELERP.Core.App.Pages.Base;
using RDCELERP.DAL.Entities;
using RDCELERP.DAL.IRepository;
using RDCELERP.Model.Base;
using RDCELERP.Model.EVC_Portal;
using RDCELERP.Model.ImagLabel;
using RDCELERP.Model.LGC;
using static RDCELERP.Model.Whatsapp.WhatsappLgcDropViewModel;

namespace RDCELERP.Core.App.Pages.LGC
{
    public class LGCDropDetailsModel : BasePageModel
    {
        #region Variable declartion
        private readonly ILogisticManager _logisticManager;
        private readonly ILogisticsRepository _logisticsRepository;
        private readonly INotificationManager _notificationManager;
        private readonly IEVCRepository _evcRepository;
        private readonly IOrderLGCRepository _orderLgcRepository;
        private readonly IWhatsappNotificationManager _whatsappNotificationManager;
        IWhatsAppMessageRepository _WhatsAppMessageRepository;
        private readonly IProductCategoryManager _productCategoryManager;
        #endregion

        #region Constructor
        public LGCDropDetailsModel(IOptions<ApplicationSettings> config, ILogisticManager logisticManager, ILogisticsRepository logisticsRepository, INotificationManager notificationManager, IEVCRepository evcRepository, IOrderLGCRepository orderLgcRepository, IWhatsappNotificationManager whatsappNotificationManager, IWhatsAppMessageRepository whatsAppMessageRepository, IProductCategoryManager productCategoryManager) : base(config)
        {
            _logisticManager = logisticManager;
            _logisticsRepository = logisticsRepository;
            _notificationManager = notificationManager;
            _evcRepository = evcRepository;
            _orderLgcRepository = orderLgcRepository;
            _whatsappNotificationManager = whatsappNotificationManager;
            _WhatsAppMessageRepository = whatsAppMessageRepository;
            _productCategoryManager = productCategoryManager;
        }
        #endregion

        #region Bind Property
        [BindProperty(SupportsGet = true)]
        public LGCOrderViewModel lGCOrderViewModel { get; set; }
        [BindProperty(SupportsGet = true)]
        public PODViewModel PODVM { get; set; }

        [BindProperty(SupportsGet = true)]
        public IList<ImageLabelViewModel> imageLabelVMList { get; set; }

        [BindProperty(SupportsGet = true)]
        public TblServicePartner tblServicePartner { get; set; }
        #endregion
        public IActionResult OnGet(int DriverDetailsId, int EVCPartnerId)
        {
            List<TblOrderLgc>? tblOrderLgcList = null;
            int userId = 0;
            if (_loginSession != null)
            {
                userId = _loginSession.UserViewModel.UserId;
                tblServicePartner = _logisticManager.GetServicePartnerDetails(userId);
                if (tblServicePartner == null)
                {
                    tblServicePartner = new TblServicePartner();
                    tblServicePartner.UserId = 0;
                }
            }
            PODVM.podVMList = new List<PODViewModel>();
            PODVM.evcDetailsVM = new EVCDetailsViewModel();
            //tblOrderLgcList = _orderLgcRepository.GetOrderLGCListByDriverIdEVCId(DriverDetailsId, EVCRegistrationId);
            lGCOrderViewModel = _logisticManager.GetLGCDropDetails(EVCPartnerId);
            if (lGCOrderViewModel == null)
            {
                lGCOrderViewModel = new LGCOrderViewModel();
                lGCOrderViewModel.evcPartnerDetailsVM = new EVC_PartnerViewModel();
            }
            else if (lGCOrderViewModel != null && lGCOrderViewModel.evcPartnerDetailsVM == null)
            {
                lGCOrderViewModel.evcPartnerDetailsVM = new EVC_PartnerViewModel();
            }
            else if(lGCOrderViewModel != null)
            {
                lGCOrderViewModel.DriverId = DriverDetailsId;
            }
            // Get Product Category List
            var ProductGroup = _productCategoryManager.GetAllProductCategory();
            if (ProductGroup != null)
            {
                ViewData["ProductGroup"] = new SelectList(ProductGroup, "Id", "Description");
            }
            return Page();
        }

        // Method for Store LGC Product Drop Images With POD Image and Invoice
        public IActionResult OnPostAsync()
        {
            int userId = _loginSession.UserViewModel.UserId;
            var url = ViewData["URLPrefixforProd"];
            bool flag = false;
            bool isSaved = false;
            if (ModelState.IsValid)
            {
                if (imageLabelVMList != null && PODVM != null)
                {
                    //flag = _logisticManager.SaveLGCDropImages(imageLabelVMList, PODVM, userId);
                    isSaved = _logisticManager.SaveLGCDropStatus(PODVM, userId);
                    if (isSaved)
                    {
                        return RedirectToPage("./LogiPickDrop");
                    }
                }
            }
            return RedirectToPage("./LGCDropDetails", new { DriverDetailsId = PODVM.DriverId, EVCPartnerId = PODVM.EvcPartnerId});
        }

        public IActionResult OnPostSendOTP(string mobnumber, string tempaltename)
        {
            WhatasappResponse whatasappResponse = new WhatasappResponse();
            TblWhatsAppMessage tblwhatsappmessage = null;
            bool flag = false;
            string message = string.Empty;
            string OTPValue = UniqueString.RandomNumber();
            if (tempaltename.Equals("SMS_Drop_OTP"))
                message = NotificationConstants.SMS_Drop_OTP.Replace("[OTP]", OTPValue);
            flag = _notificationManager.SendNotificationSMS(mobnumber, message, OTPValue);
            #region code to send OTP on whatsappNotification for lgc Drop
            WhatsappTemplate whatsappObj = new WhatsappTemplate();
            whatsappObj.userDetails = new UserDetails();
            whatsappObj.notification = new LogiDrop();
            whatsappObj.notification.@params = new SendOTP();
            whatsappObj.userDetails.number = mobnumber;
            whatsappObj.notification.sender = _baseConfig.Value.YelloaiSenderNumber;
            whatsappObj.notification.type = _baseConfig.Value.YellowaiMesssaheType;
            whatsappObj.notification.templateId = NotificationConstants.Logi_Drop;
            whatsappObj.notification.@params.OTP = OTPValue;
            string url = _baseConfig.Value.YellowAiUrl;
            RestResponse response = _whatsappNotificationManager.Rest_InvokeWhatsappserviceCall(url, Method.Post, whatsappObj);
            if (response.Content != null)
            {
                whatasappResponse = JsonConvert.DeserializeObject<WhatasappResponse>(response.Content);
                tblwhatsappmessage = new TblWhatsAppMessage();
                tblwhatsappmessage.TemplateName = NotificationConstants.Logi_Drop;
                tblwhatsappmessage.IsActive = true;
                tblwhatsappmessage.PhoneNumber = mobnumber;
                tblwhatsappmessage.SendDate = DateTime.Now;
                tblwhatsappmessage.MsgId = whatasappResponse.msgId;
                tblwhatsappmessage.Code = OTPValue;
                _WhatsAppMessageRepository.Create(tblwhatsappmessage);
                _WhatsAppMessageRepository.SaveChanges();
            }
            #endregion
            return new JsonResult(flag);
        }

        public IActionResult OnPostVerifyOTP(string mobnumber, string OTP)
        {
            bool flag = false;
            string message = string.Empty;
            flag = _notificationManager.ValidateOTP(mobnumber, OTP);
            return new JsonResult(flag);
        }
    }
}