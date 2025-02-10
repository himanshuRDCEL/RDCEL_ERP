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
using RDCELERP.Model.ImagLabel;
using RDCELERP.Model.LGC;
using RDCELERP.Model.QC;
using static RDCELERP.Model.Whatsapp.WhatsappLgcDropViewModel;

namespace RDCELERP.Core.App.Pages.LGC
{
    public class LGCDropModel : BasePageModel
    {
        #region Variable declartion
        private readonly ILogisticManager _logisticManager;
        private readonly ILogisticsRepository _logisticsRepository;
        private readonly INotificationManager _notificationManager;
        private readonly IWhatsappNotificationManager _whatsappNotificationManager;
        IWhatsAppMessageRepository _WhatsAppMessageRepository;

        #endregion

        #region Constructor
        public LGCDropModel(IOptions<ApplicationSettings> config, ILogisticManager logisticManager, ILogisticsRepository logisticsRepository, INotificationManager notificationManager, IWhatsappNotificationManager whatsappNotificationManager, IWhatsAppMessageRepository whatsAppMessageRepository) : base(config)
        {
            _logisticManager = logisticManager;
            _logisticsRepository = logisticsRepository;
            _notificationManager = notificationManager;
            _whatsappNotificationManager = whatsappNotificationManager;
            _WhatsAppMessageRepository = whatsAppMessageRepository;
        }
        #endregion

        [BindProperty(SupportsGet = true)]
        public LGCOrderViewModel lGCOrderViewModel { get; set; }
        [BindProperty(SupportsGet = true)]
        public PODViewModel PODVM { get; set; }
        [BindProperty(SupportsGet = true)]
        public IList<OrderImageUploadViewModel> OrderImageUploadVMList { get; set; }

        [BindProperty(SupportsGet = true)]
        public IList<ImageLabelViewModel> imageLabelVMList { get; set; }

        public IActionResult OnGet(string regdNo)
        {
            TblLogistic tbllogistic = null;
            TblWalletTransaction tblWalletTransaction = null;
            PODVM.evcDetailsVM = new EVCDetailsViewModel();
            tbllogistic = _logisticsRepository.GetExchangeDetailsByRegdno(regdNo);
            tblWalletTransaction = _logisticsRepository.GetEvcDetailsByRegdno(regdNo);
            if (tbllogistic != null && tblWalletTransaction != null)
            {
                lGCOrderViewModel = new LGCOrderViewModel();
                lGCOrderViewModel.RegdNo = tbllogistic.OrderTrans.Exchange.RegdNo;
                lGCOrderViewModel.ProductCategory = tbllogistic.OrderTrans.Exchange.ProductType.ProductCat.Description;
                lGCOrderViewModel.ProductType = tbllogistic.OrderTrans.Exchange.ProductType.DescriptionForAbb;
                lGCOrderViewModel.TicketNumber = tbllogistic.TicketNumber;
                //customer
                lGCOrderViewModel.CustomerName = tbllogistic.OrderTrans.Exchange.CustomerDetails.FirstName + " " + tbllogistic.OrderTrans.Exchange.CustomerDetails.LastName;
                lGCOrderViewModel.TblCustomerDetail.Email = tbllogistic.OrderTrans.Exchange.CustomerDetails.Email;
                lGCOrderViewModel.TblCustomerDetail.PhoneNumber = tbllogistic.OrderTrans.Exchange.CustomerDetails.PhoneNumber;
                lGCOrderViewModel.TblCustomerDetail.Address1 = tbllogistic.OrderTrans.Exchange.CustomerDetails.Address1;
                lGCOrderViewModel.TblCustomerDetail.Address2 = tbllogistic.OrderTrans.Exchange.CustomerDetails.Address2 != null ? tbllogistic.OrderTrans.Exchange.CustomerDetails.Address2 : "";
                lGCOrderViewModel.TblCustomerDetail.City = tbllogistic.OrderTrans.Exchange.CustomerDetails.City;
                lGCOrderViewModel.TblCustomerDetail.State = tbllogistic.OrderTrans.Exchange.CustomerDetails.State != null ? tbllogistic.OrderTrans.Exchange.CustomerDetails.State : "";
                lGCOrderViewModel.TblCustomerDetail.ZipCode = tbllogistic.OrderTrans.Exchange.CustomerDetails.ZipCode;
                //Evc
                lGCOrderViewModel.Tblevcregistration.BussinessName = tblWalletTransaction.Evcregistration.BussinessName;
                lGCOrderViewModel.Tblevcregistration.ContactPerson = tblWalletTransaction.Evcregistration.ContactPerson;
                lGCOrderViewModel.Tblevcregistration.EvcmobileNumber = tblWalletTransaction.Evcregistration.EvcmobileNumber;
                lGCOrderViewModel.Tblevcregistration.AlternateMobileNumber = tblWalletTransaction.Evcregistration.AlternateMobileNumber != null ? tblWalletTransaction.Evcregistration.AlternateMobileNumber : "";
                lGCOrderViewModel.Tblevcregistration.EmailId = tblWalletTransaction.Evcregistration.EmailId;
                lGCOrderViewModel.Tblevcregistration.RegdAddressLine1 = tblWalletTransaction.Evcregistration.RegdAddressLine1;
                lGCOrderViewModel.Tblevcregistration.RegdAddressLine2 = tblWalletTransaction.Evcregistration.RegdAddressLine2;
                lGCOrderViewModel.Tblevcregistration.City = tblWalletTransaction.Evcregistration.City;
                lGCOrderViewModel.Tblevcregistration.State = tblWalletTransaction.Evcregistration.State;
                lGCOrderViewModel.Tblevcregistration.PinCode = tblWalletTransaction.Evcregistration.PinCode;
                lGCOrderViewModel.AmountPayableThroughLGC = 0;
            }
            if (!string.IsNullOrEmpty(regdNo))
            {
                string url = _baseConfig.Value.BaseURL;
                OrderImageUploadVMList = _logisticManager.GetImagesUploadedFromLGCPickup(regdNo);
                if (OrderImageUploadVMList != null && OrderImageUploadVMList.Count > 0)
                {
                    foreach (var item in OrderImageUploadVMList)
                    {
                        item.FilePath = EnumHelper.DescriptionAttr(FileAddressEnum.LGCPickup);
                        item.ImageWithPath = url + item.FilePath + item.ImageName;
                    }
                    imageLabelVMList = _logisticManager.GetImageLabelUploadByProductCat(regdNo);

                    if (imageLabelVMList == null)
                    {
                        imageLabelVMList = new List<ImageLabelViewModel>();
                    }
                }
                else
                {
                    OrderImageUploadVMList = new List<OrderImageUploadViewModel>();
                }
            }
            return Page();
        }

        // Method for Store LGC Product Drop Images With POD Image and Invoice
        public IActionResult OnPostAsync()
        {
            int userId = _loginSession.UserViewModel.UserId;
            var url = ViewData["URLPrefixforProd"];
            bool flag = false;
            if (ModelState.IsValid)
            {
                if (imageLabelVMList != null && PODVM != null)
                {
                    /*flag = _logisticManager.SaveLGCDropImages(imageLabelVMList, PODVM, userId);*/
                    if (flag)
                    {
                        return RedirectToPage("./LogiPickDrop");
                    }
                }
            }
            return RedirectToPage("./LGCDrop", new { regdNo = PODVM.RegdNo });
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