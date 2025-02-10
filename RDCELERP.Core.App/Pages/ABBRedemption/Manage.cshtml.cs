using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using RDCELERP.BAL.Interface;
using RDCELERP.BAL.MasterManager;
using RDCELERP.Common.Constant;
using RDCELERP.Common.Enums;
using RDCELERP.Common.Helper;
using RDCELERP.Core.App.Pages.Base;
using RDCELERP.DAL.Entities;
using RDCELERP.DAL.IRepository;
using RDCELERP.Model;
using RDCELERP.Model.ABBRedemption;
using RDCELERP.Model.Base;
using RDCELERP.Model.ExchangeOrder;
using RDCELERP.Model.LGC;
using RDCELERP.Model.QC;
using RDCELERP.Model.QCComment;
using static RDCELERP.Model.Whatsapp.WhatsappRescheduleViewModel;


namespace RDCELERP.Core.App.Pages.ABBRedemption
{
    public class ManageModel : BasePageModel
    {
        #region Variable Declaration
        private readonly Digi2l_DevContext _context;
        IABBRedemptionManager _aBBRedemptionManager;
        IQCCommentManager _qCCommentManager;
        IOrderQCManager _orderQCManager;
        IOrderTransactionManager _orderTransactionManager;
        IWhatsappNotificationManager _whatsappNotificationManager;
        IWhatsAppMessageRepository _whatsAppMessageRepository;
        IExchangeOrderManager _exchangeOrderManager;
        #endregion

        #region Constructor
        public ManageModel(IOptions<ApplicationSettings> config, IABBRedemptionManager aBBRedemptionManager, IQCCommentManager qCCommentManager, Digi2l_DevContext context, IOrderQCManager orderQCManager, IOrderTransactionManager orderTransactionManager, IWhatsappNotificationManager whatsappNotificationManager, IWhatsAppMessageRepository whatsAppMessageRepository, IExchangeOrderManager exchangeOrderManager)
       : base(config)
        {
            _aBBRedemptionManager = aBBRedemptionManager;
            _qCCommentManager = qCCommentManager;
            _context = context;
            _orderQCManager = orderQCManager;
            _orderTransactionManager = orderTransactionManager;
            _whatsappNotificationManager = whatsappNotificationManager;
            _whatsAppMessageRepository = whatsAppMessageRepository;
            _exchangeOrderManager = exchangeOrderManager;
        }
        #endregion

        #region Model
        [BindProperty(SupportsGet = true)]
        public ABBRedemptionViewModel aBBRedemptionViewModel { get; set; }
        [BindProperty(SupportsGet = true)]
        public QCCommentViewModel QCCommentViewModel { get; set; }
        [BindProperty(SupportsGet = true)]
        public IList<SelfQCViewModel> SelfQCVMList { get; set; }
        [BindProperty(SupportsGet = true)]
        public IList<OrderImageUploadViewModel> OrderImageUploadList { get; set; }
        [BindProperty(SupportsGet = true)]
        public IList<OrderImageUploadViewModel> LGCOrderImageUploadList { get; set; }
        [BindProperty(SupportsGet = true)]
        public QCCommentViewModel qcCommentViewModel { get; set; }
        [BindProperty(SupportsGet = true)]
        public TblCompany TblCompany { get; set; }
        [BindProperty(SupportsGet = true)]
        public int UserId { get; set; }
        public ABBVoucherDetailsViewModel voucherDetailsViewModel { get; set; }
        #endregion

        public IActionResult OnGet(string regdNo) 
        {
            string url = _baseConfig.Value.BaseURL;
            string UserComapnyName = null;
            string CompanyName = null;
            string MvcABBInvoceImageURL = _baseConfig.Value.MVCBaseURLForABBInvoice;            
            string ERPABBInvoiceURL = _baseConfig.Value.BaseURL + EnumHelper.DescriptionAttr(FileAddressEnum.ABBInvoice);

            if (!string.IsNullOrEmpty(regdNo))
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
                            UserComapnyName = TblCompany.CompanyName;
                        }
                    }
                    CompanyName = EnumHelper.DescriptionAttr(CompanyNameenum.UTC);
                }

                UserId = _loginSession.UserViewModel.UserId;

                #region get the abb order details
                aBBRedemptionViewModel = _aBBRedemptionManager.GetAbbOrderDetails(regdNo);
                if(aBBRedemptionViewModel != null && !string.IsNullOrEmpty(aBBRedemptionViewModel.InvoiceImageName))
                {
                    aBBRedemptionViewModel.InvoiceImageUrlMVC = MvcABBInvoceImageURL + aBBRedemptionViewModel.InvoiceImageName;
                    aBBRedemptionViewModel.InvoiceImageUrlERP = ERPABBInvoiceURL + aBBRedemptionViewModel.InvoiceImage;
                }
                #endregion

                #region SelfQC Images
                SelfQCVMList = _qCCommentManager.GetImagesUploadedBySelfQC(regdNo);
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
                #endregion

                #region VideoQC Images
                OrderImageUploadList = _qCCommentManager.GetImagesUploadedByVideoQC(regdNo);
                if (OrderImageUploadList != null && OrderImageUploadList.Count > 0)
                {
                    foreach (var item in OrderImageUploadList)
                    {
                        item.FilePath = EnumHelper.DescriptionAttr(FilePathEnum.VideoQC);
                        item.ImageWithPath = url + item.FilePath.Replace("\\", "/") + "/" + item.ImageName;
                    }
                }
                else
                {
                    OrderImageUploadList = new List<OrderImageUploadViewModel>();
                }
                #endregion

                #region LGC Images
                LGCOrderImageUploadList = _qCCommentManager.GetImagesUploadedByLGCQC(regdNo);
                if (LGCOrderImageUploadList != null && LGCOrderImageUploadList.Count > 0)
                {
                    foreach (var item in LGCOrderImageUploadList)
                    {
                        item.FilePath = EnumHelper.DescriptionAttr(FilePathEnum.LGCPickup);
                        item.ImageWithPath = url + item.FilePath.Replace("\\", "/") + "/" + item.ImageName;
                    }
                }
                else
                {
                    LGCOrderImageUploadList = new List<OrderImageUploadViewModel>();
                }
                #endregion
                         
                #region get QC Information
                if (aBBRedemptionViewModel != null && aBBRedemptionViewModel.OrderTransId > 0)
                {
                    qcCommentViewModel = _qCCommentManager.GetQCDetailsByOrderTransId(aBBRedemptionViewModel.OrderTransId);
                    qcCommentViewModel.CompanyUTC = CompanyName;
                    qcCommentViewModel.UserCompany = UserComapnyName;
                }
                #endregion

                #region get the logistics details
                if (aBBRedemptionViewModel != null && aBBRedemptionViewModel.OrderTransId > 0)
                {
                    QCCommentViewModel = _qCCommentManager.GetLGCDetails(aBBRedemptionViewModel.OrderTransId);
                }

                #region EVC Pod Details, invoice, DebitNotePdf, CustomerDeclarationPdf, PoDPdf
                if (QCCommentViewModel != null && QCCommentViewModel.LogisticViewModel != null)
                {
                    if (QCCommentViewModel.LogisticViewModel.PoDPdf != null)
                    {
                        var FullPoDUrl = url + EnumHelper.DescriptionAttr(FileAddressEnum.EVCPoD) + QCCommentViewModel.LogisticViewModel.PoDPdf;
                        QCCommentViewModel.LogisticViewModel.PoDPdf = FullPoDUrl;
                    }
                    if (QCCommentViewModel.LogisticViewModel.DebitNotePdf != null)
                    {
                        var FullDebitNoteUrl = url + EnumHelper.DescriptionAttr(FileAddressEnum.EVCDebitNote) + QCCommentViewModel.LogisticViewModel.DebitNotePdf;
                        QCCommentViewModel.LogisticViewModel.DebitNotePdf = FullDebitNoteUrl;
                    }
                    if (QCCommentViewModel.LogisticViewModel.InvoieImagePdf != null)
                    {
                        var FullInvoiceUrl = url + EnumHelper.DescriptionAttr(FileAddressEnum.EVCInvoice) + QCCommentViewModel.LogisticViewModel.InvoieImagePdf;
                        QCCommentViewModel.LogisticViewModel.InvoieImagePdf = FullInvoiceUrl;
                    }
                    if (QCCommentViewModel.LogisticViewModel.CustomerDeclarationPdf != null)
                    {
                        var FullCustomerDeclarationUrl = url + EnumHelper.DescriptionAttr(FileAddressEnum.CustomerDeclaration) + QCCommentViewModel.LogisticViewModel.CustomerDeclarationPdf;
                        QCCommentViewModel.LogisticViewModel.CustomerDeclarationPdf = FullCustomerDeclarationUrl;
                    }
                }
                #endregion

                #endregion
                #region Get Voucher Details
                voucherDetailsViewModel = _qCCommentManager.GetABBVoucherDetails(aBBRedemptionViewModel.RedemptionId);
               
                #endregion 
            }
            return Page();
        }

        #region Set Rescheduled data & send whatsapp msg
        public JsonResult OnGetRescheduleddataAsync(string RegdNo, string RecheduledTime, string RecheduledDate, string QCComment, string MobileNumber)
        {
            if (!string.IsNullOrEmpty(RegdNo) && !string.IsNullOrEmpty(RecheduledTime) && !string.IsNullOrEmpty(RecheduledDate) && !string.IsNullOrEmpty(MobileNumber))
            {
                if (RecheduledTime == "NaN:undefined AM")
                {
                    string RescheduleDatetime = "Please Provide valid Time format (hh:mm AM/PM)";
                    return new JsonResult(RescheduleDatetime);
                }
                else
                {
                    if (!string.IsNullOrEmpty(QCComment))
                    {
                        WhatasappResponse whatasappResponse = new WhatasappResponse();
                        TblWhatsAppMessage tblwhatsappmessage = null;
                        string message = string.Empty;
                        bool rescheduleresponse = false;
                        string originalFormat = "dd-MM-yyyy h:mm tt";
                        string desiredFormat = "yyyy-MM-dd HH:mm:ss.fff";
                        TblOrderTran tblOrderTran = _orderTransactionManager.GetOrderDetailsByRegdNo(RegdNo);
                        TblOrderQc tblOrderQc = _orderQCManager.GetOrderDetails(tblOrderTran.OrderTransId);


                        QCCommentViewModel qcCommentViewModel = new QCCommentViewModel();
                        qcCommentViewModel.RegdNo = RegdNo;
                        DateTime parsedDateTime = DateTime.ParseExact((RecheduledDate + " " + RecheduledTime), originalFormat, CultureInfo.InvariantCulture);
                        string formattedDateTime = parsedDateTime.ToString(desiredFormat);
                        //qcCommentViewModel.ProposedQcdate = Convert.ToDateTime(RecheduledDate + " " + RecheduledTime);
                        qcCommentViewModel.ProposedQcdate = Convert.ToDateTime(formattedDateTime);
                        qcCommentViewModel.Qccomments = QCComment;
                        qcCommentViewModel.Isrescheduled = false;
                        if (tblOrderQc != null && tblOrderQc.Reschedulecount > 0)
                        {
                            qcCommentViewModel.Reschedulecount = (int)tblOrderQc.Reschedulecount + 1;
                        }
                        else
                        {
                            qcCommentViewModel.Reschedulecount += 1;
                        }
                        if (qcCommentViewModel.Reschedulecount == 4)
                        {
                            qcCommentViewModel.StatusId = Convert.ToInt32(OrderStatusEnum.QCAppointmentRescheduled_3RA);
                        }
                        else
                        {
                            qcCommentViewModel.StatusId = (int?)OrderStatusEnum.QCAppointmentrescheduled;
                        }

                        rescheduleresponse = _qCCommentManager.RescheduledQCOrder(qcCommentViewModel, Convert.ToInt32(_loginSession.UserViewModel.UserId));
                        if (rescheduleresponse == true)
                        {
                            #region whatsappNotification for RescheduleQCDate
                            string ScheduledDate = Convert.ToDateTime(qcCommentViewModel.ProposedQcdate).ToString("MM/dd/yyyy hh:mm:ss tt");

                            WhatsappTemplate whatsappObj = new WhatsappTemplate();
                            whatsappObj.userDetails = new UserDetails();
                            whatsappObj.notification = new RescheduleQCDate();
                            whatsappObj.notification.@params = new SendDate();
                            whatsappObj.userDetails.number = MobileNumber;
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
                                tblwhatsappmessage.PhoneNumber = MobileNumber;
                                tblwhatsappmessage.SendDate = DateTime.Now;
                                tblwhatsappmessage.MsgId = whatasappResponse.msgId;
                                _whatsAppMessageRepository.Create(tblwhatsappmessage);
                                _whatsAppMessageRepository.SaveChanges();
                            }
                            //if (response.IsSuccessful == true)
                            //{
                            //    rescheduleresponse = _qCCommentManager.RescheduledQCOrder(qcCommentViewModel, Convert.ToInt32(_loginSession.UserViewModel.UserId));
                            //    //qcCommentViewModel = _qCCommentManager.RescheduledQCOrder(qcCommentViewModel, Convert.ToInt32(_loginSession.UserViewModel.UserId));
                            //}
                            #endregion
                        }

                        return new JsonResult(qcCommentViewModel);

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

        #region Set status for Qc Cancel
        public JsonResult OnGetCancelQcAsync(string RegdNo, string CancelComment)
        {
            bool flag = false;
            flag = _qCCommentManager.CancelQCOrder(RegdNo, CancelComment, Convert.ToInt32(_loginSession.UserViewModel.UserId));
            return new JsonResult(flag);
        }
        #endregion

        #region Send self qc link to customer
        public IActionResult OnPostSendSelfQCUrlToCustomer(string regdno, string mobnumber, int loginid)
        {
            bool result = _exchangeOrderManager.sendSelfQCUrl(regdno, mobnumber, loginid);
            return new JsonResult(result);
        }
        #endregion

        #region Check UPI and Beneficiary
        public JsonResult OnGetCheckBeneficiaryAsync(string RegdNo)
        {
            string result = null;
            result = _qCCommentManager.CheckUPIandBeneficiaryForAbb(RegdNo);
            return new JsonResult(result);
        }
        #endregion
    }
}
