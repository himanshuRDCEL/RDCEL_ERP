using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RestSharp;
using RDCELERP.BAL.Helper;
using RDCELERP.BAL.Interface;
using RDCELERP.BAL.MasterManager;
using RDCELERP.Common.Constant;
using RDCELERP.Common.Enums;
using RDCELERP.Common.Helper;
using RDCELERP.Core.App.Pages.Base;
using RDCELERP.DAL.Entities;
using RDCELERP.DAL.IRepository;
using RDCELERP.Model.Base;
using RDCELERP.Model.DriverDetails;
using RDCELERP.Model.ImagLabel;
using RDCELERP.Model.LGC;
using RDCELERP.Model.QC;
using RDCELERP.Model.ServicePartner;
using static RDCELERP.Model.Whatsapp.WhatsappLgcDropViewModel;

namespace RDCELERP.Core.App.Pages.LGC_Mobile_Portal
{
    public class LGCDriverDetailsModel : BasePageModel
    {
        #region Variable declartion
        private readonly ILogisticManager _logisticManager;
        private readonly ILogisticsRepository _logisticsRepository;
        private readonly INotificationManager _notificationManager;
        private readonly IEVCRepository _evcRepository;
        private readonly IOrderLGCRepository _orderLgcRepository;
        private readonly IWhatsappNotificationManager _whatsappNotificationManager;
        IWhatsAppMessageRepository _WhatsAppMessageRepository;
        #endregion

        #region Constructor
        public LGCDriverDetailsModel(IOptions<ApplicationSettings> config, ILogisticManager logisticManager, ILogisticsRepository logisticsRepository, INotificationManager notificationManager, IEVCRepository evcRepository, IOrderLGCRepository orderLgcRepository, IWhatsappNotificationManager whatsappNotificationManager, IWhatsAppMessageRepository whatsAppMessageRepository) : base(config)
        {
            _logisticManager = logisticManager;
            _logisticsRepository = logisticsRepository;
            _notificationManager = notificationManager;
            _evcRepository = evcRepository;
            _orderLgcRepository = orderLgcRepository;
            _whatsappNotificationManager = whatsappNotificationManager;
            _WhatsAppMessageRepository = whatsAppMessageRepository;
        }
        #endregion

        [BindProperty(SupportsGet = true)]
        public LGCOrderViewModel lGCOrderViewModel { get; set; }
        [BindProperty(SupportsGet = true)]
        public PODViewModel PODVM { get; set; }

        [BindProperty(SupportsGet = true)]
        public List<PODViewModel> PODVMList { get; set; }
        [BindProperty(SupportsGet = true)]
        public IList<OrderImageUploadViewModel> OrderImageUploadVMList { get; set; }

        [BindProperty(SupportsGet = true)]
        public IList<ImageLabelViewModel> imageLabelVMList { get; set; }
        [BindProperty(SupportsGet = true)]
        public TblEvcregistration tblEvcregistration { get; set; }
        public List<TblOrderLgc> tblOrderLgcList { get; set; }
        [BindProperty(SupportsGet = true)]
        public ServicePartnerViewModel servicePartnerVM { get; set; }
        public IActionResult OnGet(string? DriverDetailsId = null, string? ServicePartnerId = null)
        {
            int userId = 0;
            if (_loginSession != null)
            {
                userId = _loginSession.UserViewModel.UserId;
                servicePartnerVM = _logisticManager.GetServicePartnerByUserId(userId);
                if (servicePartnerVM == null)
                {
                    servicePartnerVM = new ServicePartnerViewModel();
                    servicePartnerVM.UserId = 0;
                }
            }
            int servicePartnerId = 0; int driverDetailsId = 0;

            if(!string.IsNullOrEmpty(ServicePartnerId)){ servicePartnerId = Convert.ToInt32(ServicePartnerId); }
            if (!string.IsNullOrEmpty(DriverDetailsId)) { driverDetailsId = Convert.ToInt32(DriverDetailsId); }

            lGCOrderViewModel = new LGCOrderViewModel();
            lGCOrderViewModel.driverDetailsVM = new DriverDetailsViewModel();
            if (driverDetailsId > 0)
            {
                lGCOrderViewModel.driverDetailsVM = _logisticManager.GetDriverDetailsById(driverDetailsId);
                if (servicePartnerId > 0)
                {
                    lGCOrderViewModel.driverDetailsVM.ServicePartnerId = servicePartnerId;
                }
                else if (servicePartnerVM != null)
                {
                    lGCOrderViewModel.driverDetailsVM.ServicePartnerId = servicePartnerVM.ServicePartnerId;
                }
            }
            
            //int EVCRegistrationId = 0;
            //TblEvcregistration tblEvcregistration = null;
            //PODVM.podVMList = new List<PODViewModel>();
            //PODVM.evcDetailsVM = new EVCDetailsViewModel();
            ////tblEvcregistration = _evcRepository.GetEVCDetailsById(EVCRegistrationId);
            ////tblOrderLgcList = _orderLgcRepository.GetOrderLGCListByDriverIdEVCId(driverDetailsId, EVCRegistrationId);
            //if (tblEvcregistration != null)
            //{
            //    lGCOrderViewModel = new LGCOrderViewModel();
            //    lGCOrderViewModel.DriverId = driverDetailsId;
            //    lGCOrderViewModel.EVCRegistrationId = EVCRegistrationId;
            //    //Evc
            //    lGCOrderViewModel.Tblevcregistration.BussinessName = tblEvcregistration.BussinessName;
            //    lGCOrderViewModel.Tblevcregistration.ContactPerson = tblEvcregistration.ContactPerson;
            //    lGCOrderViewModel.Tblevcregistration.EvcmobileNumber = tblEvcregistration.EvcmobileNumber;
            //    lGCOrderViewModel.Tblevcregistration.AlternateMobileNumber = tblEvcregistration.AlternateMobileNumber != null ? tblEvcregistration.AlternateMobileNumber : "";
            //    lGCOrderViewModel.Tblevcregistration.EmailId = tblEvcregistration.EmailId;
            //    lGCOrderViewModel.Tblevcregistration.RegdAddressLine1 = tblEvcregistration.RegdAddressLine1;
            //    lGCOrderViewModel.Tblevcregistration.RegdAddressLine2 = tblEvcregistration.RegdAddressLine2;
            //    if (tblEvcregistration.City == null)
            //    {
            //        tblEvcregistration.City = new TblCity();
            //    }
            //    lGCOrderViewModel.Tblevcregistration.City = tblEvcregistration.City;
            //    if (tblEvcregistration.State == null)
            //    {
            //        tblEvcregistration.State = new TblState();
            //    }
            //    lGCOrderViewModel.Tblevcregistration.State = tblEvcregistration.State;
            //    lGCOrderViewModel.Tblevcregistration.PinCode = tblEvcregistration.PinCode;
            //}
            //else
            //{
            //    tblEvcregistration = new TblEvcregistration();
            //    lGCOrderViewModel.Tblevcregistration.City = new TblCity();
            //    lGCOrderViewModel.Tblevcregistration.State = new TblState();
            //}
            
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
            return RedirectToPage("./LGCDropDetails", new { DriverDetailsId = PODVM.DriverId, EVCRegistrationId = PODVM.EVCRegistrationId});
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