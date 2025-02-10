using DocumentFormat.OpenXml.Vml;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using NPOI.SS.Formula.Functions;
using RestSharp;
using System;
using System.Collections.Generic;
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
using RDCELERP.Model.DaikinModel;
using RDCELERP.Model.EVC_Portal;
using RDCELERP.Model.ImagLabel;
using RDCELERP.Model.LGC;
using static RDCELERP.Model.Whatsapp.WhatsappLgcPickupViewModel;

namespace RDCELERP.Core.App.Pages.LGC
{
    public class LGCPickupModel : BasePageModel
    {
        #region Variable declartion
        private readonly ILogisticManager _logisticManager;
        private readonly ILogisticsRepository _logisticsRepository;
        private readonly INotificationManager _notificationManager;
        private readonly IWhatsappNotificationManager _whatsappNotificationManager;
        IWhatsAppMessageRepository _WhatsAppMessageRepository;
        private readonly IOrderTransRepository _orderTransRepository;
        private CustomDataProtection _protector;
        IDaikinManager _daikinManager;
        IExchangeOrderRepository _exchangeOrderRepository;
        #endregion

        #region Constructor
        public LGCPickupModel(IOptions<ApplicationSettings> config, ILogisticManager logisticManager, ILogisticsRepository logisticsRepository, INotificationManager notificationManager, IWhatsappNotificationManager whatsappNotificationManager, IWhatsAppMessageRepository whatsAppMessageRepository, CustomDataProtection protector, IOrderTransRepository orderTransRepository, IDaikinManager daikinManager, IExchangeOrderRepository exchangeOrderRepository) : base(config)
        {
            _logisticManager = logisticManager;
            _logisticsRepository = logisticsRepository;
            _notificationManager = notificationManager;
            _whatsappNotificationManager = whatsappNotificationManager;
            _WhatsAppMessageRepository = whatsAppMessageRepository;
            _protector = protector;
            _orderTransRepository = orderTransRepository;
            _daikinManager = daikinManager;
            _exchangeOrderRepository = exchangeOrderRepository;
        }
        #endregion

        #region Model Binding
        [BindProperty(SupportsGet = true)]
        public LGCOrderViewModel lGCOrderViewModel { get; set; }
        [BindProperty(SupportsGet = true)]
        public IList<OrderImageUploadViewModel> OrderImageUploadViewModel { get; set; }
        [BindProperty(SupportsGet = true)]
        public IList<ImageLabelViewModel> imageLabelViewModels { get; set; }
        #endregion

        public IActionResult OnGet(string regdNo)
        {
            lGCOrderViewModel = _logisticManager.GetLGCPickupOrderDetailsByRegdNo(regdNo);
            if (!string.IsNullOrEmpty(regdNo))
            {
                #region Get Images from DB
                string url = _baseConfig.Value.BaseURL;
                OrderImageUploadViewModel = _logisticManager.GetImageUploadedByQC(regdNo);
                imageLabelViewModels = _logisticManager.GetImageLabelUploadByProductCat(regdNo);
                if (imageLabelViewModels == null)
                {
                    imageLabelViewModels = new List<ImageLabelViewModel>();
                }
                if (OrderImageUploadViewModel != null)
                {
                    foreach (var item in OrderImageUploadViewModel)
                    {
                        item.FilePath = "DBFiles/QC/VideoQC/";
                        item.ImageWithPath = url + item.FilePath + item.ImageName;
                    }
                }
                else
                {
                    OrderImageUploadViewModel = new List<OrderImageUploadViewModel>();
                }
                #endregion

                #region Get Image label for Upload 
                //Get Image Lables for Upload Pickup Images
                imageLabelViewModels = _logisticManager.GetImageLabelUploadByProductCat(regdNo);
                if (imageLabelViewModels == null)
                {
                    imageLabelViewModels = new List<ImageLabelViewModel>();
                }
                #endregion
            }
            if (lGCOrderViewModel == null)
            {
                lGCOrderViewModel = new LGCOrderViewModel();
                lGCOrderViewModel.evcPartnerDetailsVM = new EVC_PartnerViewModel();
            }
            if (lGCOrderViewModel != null && lGCOrderViewModel.evcPartnerDetailsVM == null)
            {
                lGCOrderViewModel.evcPartnerDetailsVM = new EVC_PartnerViewModel();
            }
            return Page();
        }

        public IActionResult OnPostAsync()
        {
            int userId = _loginSession.UserViewModel.UserId;
            bool flag = false;
            if (ModelState.IsValid)
            {
                flag = _logisticManager.AddFinalQCImageToDB(imageLabelViewModels, lGCOrderViewModel, userId);
                if (flag == true)
                {
                    if (_baseConfig.Value.IsDaikinAPIOrderStatus)
                    {
                        TblOrderTran tblOrderTrans = _orderTransRepository.GetOrderTransByRegdno(lGCOrderViewModel.RegdNo);
                        if (tblOrderTrans != null)
                        {
                            if (tblOrderTrans.OrderType == Convert.ToInt32(LoVEnum.Exchange))
                            {
                                TblExchangeOrder tblExchangeOrder = new TblExchangeOrder();
                                tblExchangeOrder = _exchangeOrderRepository.GetRegdNo(lGCOrderViewModel.RegdNo);

                                if (tblExchangeOrder.BusinessUnitId == _baseConfig.Value.DaikinBUId)
                                {
                                    DaikinAuth daikinAuth = _daikinManager.DaikinAuthCall();
                                    DaikinOrderStatus daikinOrderStatus = new DaikinOrderStatus();

                                    if (tblExchangeOrder != null)
                                    {
                                        daikinOrderStatus.GreenExchangeOrderId = tblExchangeOrder.Id.ToString();
                                        daikinOrderStatus.OrderStatus = EnumHelper.DescriptionAttr(OrderStatusEnum.LGCPickup);
                                    }

                                    if (daikinOrderStatus != null && daikinAuth.access_token != null)
                                    {
                                        _daikinManager.PushOrderStatus(daikinOrderStatus, daikinAuth.access_token);
                                    }
                                }
                            }
                        }
                    }
                    
                    if(lGCOrderViewModel.IsDefaultPickupAddress != true)
                    {
                        string PayNowRedirectionLink = _logisticManager.GetLGCPayNowLinkBasedOnBU(lGCOrderViewModel.RegdNo);
                        if (!string.IsNullOrEmpty(PayNowRedirectionLink))
                        {
                            var RegdNumber = SecurityHelper.EncryptString(lGCOrderViewModel.RegdNo, _baseConfig.Value.SecurityKey);
                            return RedirectToPage(PayNowRedirectionLink, new { RegdNo = RegdNumber });
                        }
                        else
                        {
                            return RedirectToPage("./LogiPickDrop");
                        }
                    }
                    else
                    {
                        return RedirectToPage("./LogiPickDrop");
                    }

                }
                else
                {
                    return RedirectToPage("./LGCPickup", new { RegdNo = lGCOrderViewModel.RegdNo });
                }
            }
            return RedirectToPage("./LGCPickup", new { RegdNo = lGCOrderViewModel.RegdNo });
        }
        //Update Pickup decline Status
        public IActionResult OnPostUpdateStatusIdOnRejectAsync()
        {
            if (_loginSession == null)
            {
                return NotFound();
            }
            else
            {
                var result = _logisticManager.AddRejectedOrderStatusToDB(lGCOrderViewModel.RegdNo, lGCOrderViewModel.Commant, _loginSession.UserViewModel.UserId);               
            }
            return RedirectToPage("./LogiPickDrop/");

        }
        //Update Pickup Reschedule Status
        public IActionResult OnPostUpdateRescheduleDateAsync()
        {
            if (_loginSession == null)
            {
                return NotFound();
            }
            else
            {
                var result = _logisticManager.RescheduledLGC(lGCOrderViewModel.RegdNo, lGCOrderViewModel.RescheduleComment,lGCOrderViewModel.RescheduleDate, _loginSession.UserViewModel.UserId);
                if (result == 1)
                {
                    return RedirectToPage("./LogiPickDrop/");
                }              
            }          
            return RedirectToPage("./LogiPickDrop/");
        }

        public IActionResult OnPostSendOTP(string mobnumber, string tempaltename)
        {
            WhatasappResponse whatasappResponse = new WhatasappResponse();
            TblWhatsAppMessage tblwhatsappmessage = null;
            bool flag = false;
            string message = string.Empty;
            string OTPValue = UniqueString.RandomNumber();
            if (tempaltename.Equals("SMS_LGCPickup_OTP"))
                message = NotificationConstants.SMS_LGCPickup_OTP.Replace("[OTP]", OTPValue);
            flag = _notificationManager.SendNotificationSMS(mobnumber, message, OTPValue);

            #region code to send OTP on whatsappNotification for lgc Pickup
            WhatsappTemplate whatsappObj = new WhatsappTemplate();
            whatsappObj.userDetails = new UserDetails();
            whatsappObj.notification = new LogiPickup();
            whatsappObj.notification.@params = new SendOTP();
            whatsappObj.userDetails.number = mobnumber;
            whatsappObj.notification.sender = _baseConfig.Value.YelloaiSenderNumber;
            whatsappObj.notification.type = _baseConfig.Value.YellowaiMesssaheType;
            whatsappObj.notification.templateId = NotificationConstants.Logi_Pickup;
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

        public IActionResult OnPostPayNow(string RegdNo)
        {
            bool flag = false;
            string PayNowRedirectionLink = _logisticManager.GetLGCPayNowLinkBasedOnBU(RegdNo);
            if (!string.IsNullOrEmpty(PayNowRedirectionLink))
            {
                flag = true;
            }
            return new JsonResult(flag);
        }
    }
}
