using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RestSharp;
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
using RDCELERP.Model.SearchFilters;
using static RDCELERP.Model.Whatsapp.WhatsappPickupDateViewModel;
using static RDCELERP.Model.Whatsapp.WhatsappQCPriceViewModel;

namespace RDCELERP.Core.App.Pages.QCIndex
{
    public class PriceQuotedQCModel : BasePageModel
    {
        #region Variable Declaration
        IExchangeOrderRepository _ExchangeOrderRepository;
        IWhatsappNotificationManager _whatsappNotificationManager;
        IWhatsAppMessageRepository _whatsAppMessageRepository;
        ICustomerDetailsRepository _customerDetailsRepository;
        IOrderTransRepository _orderTransRepository;
        IExchangeABBStatusHistoryRepository _exchangeABBStatusHistoryRepository;
        DateTime _currentDatetime = DateTime.Now.TrimMilliseconds();
        IOrderQCRepository _orderQCRepository;
        private readonly Digi2l_DevContext _context;
        private readonly IProductCategoryManager _productCategoryManager;
        private readonly IProductTypeManager _productTypeManager;
        ILogging _logging;
        ICommonManager _commonManager;
        #endregion

        #region Constructor
        public PriceQuotedQCModel(IOptions<ApplicationSettings> config, IExchangeOrderRepository exchangeOrderRepository, IWhatsappNotificationManager whatsappNotificationManager, IWhatsAppMessageRepository whatsAppMessageRepository, ICustomerDetailsRepository customerDetailsRepository, IOrderTransRepository orderTransRepository, IExchangeABBStatusHistoryRepository exchangeABBStatusHistoryRepository, IOrderQCRepository orderQCRepository, Digi2l_DevContext context, IProductCategoryManager productCategoryManager, IProductTypeManager productTypeManager, ILogging logging, ICommonManager commonManager)
        : base(config)
        {
            _ExchangeOrderRepository = exchangeOrderRepository;
            _whatsappNotificationManager = whatsappNotificationManager;
            _whatsAppMessageRepository = whatsAppMessageRepository;
            _customerDetailsRepository = customerDetailsRepository;
            _orderTransRepository = orderTransRepository;
            _exchangeABBStatusHistoryRepository = exchangeABBStatusHistoryRepository;
            _orderQCRepository = orderQCRepository;
            _context = context;
            _productCategoryManager = productCategoryManager;
            _productTypeManager = productTypeManager;
            _logging = logging;
            _commonManager = commonManager;
        }
        #endregion

        #region Bind Properties
        [BindProperty(SupportsGet = true)]
        public SearchFilterViewModel searchFilterVM { get; set; }
        [BindProperty(SupportsGet = true)]
        public QCCommentViewModel QCCommentViewModel { get; set; }
        [BindProperty(SupportsGet = true)]
        public int UserId { get; set; }
        #endregion
        public void OnGet()
        {
            UserId = _loginSession.UserViewModel.UserId;
            // Get Product Category List
            var ProductGroup = _productCategoryManager.GetAllProductCategory();
            if (ProductGroup != null)
            {
                ViewData["ProductGroup"] = new SelectList(ProductGroup, "Id", "Description");
            }
        }

        #region Resend UPI Verification Link
        public IActionResult OnGetSendUPIVerificationLinkAsync(int? ExchangeId)
        {
            int result = 0;
            if (ExchangeId != null && ExchangeId > 0)
            {
                WhatasappResponse whatasappResponse = new WhatasappResponse();
                TblWhatsAppMessage? tblwhatsappmessage = null;
                //TblWhatsAppMessage tblWhatsAppMessage = null;
                TblOrderTran? tblOrderTran = null;
                string message = string.Empty;
                bool isupirequired = false;
                string baseUrl = string.Empty;
                string url = string.Empty;

                #region whatsappNotification for UPI No
                TblExchangeOrder tblExchangeOrder = _ExchangeOrderRepository.GetExchangeorder(ExchangeId);
                if (tblExchangeOrder != null && tblExchangeOrder.FinalExchangePrice != null)
                {
                    if (tblExchangeOrder.StatusId == (int)OrderStatusEnum.Waitingforcustapproval || tblExchangeOrder.StatusId == (int)OrderStatusEnum.QCByPass)
                    {
                        int ElapsedHrs = 0;

                        tblOrderTran = _orderTransRepository.GetOrderTransByRegdno(tblExchangeOrder.RegdNo);
                        if (tblOrderTran != null)
                        {
                            isupirequired = _commonManager.CheckUpiisRequired(tblOrderTran.OrderTransId);
                        }
                        TblCustomerDetail tblCustomerDetail = _customerDetailsRepository.GetCustDetails(tblExchangeOrder.CustomerDetailsId);
                        TblExchangeAbbstatusHistory tblExchangeAbbstatusHistory = _exchangeABBStatusHistoryRepository.GetByRegdstatusno(tblExchangeOrder.RegdNo, tblExchangeOrder.StatusId);
                        
                        if (tblCustomerDetail != null && tblExchangeAbbstatusHistory != null && tblCustomerDetail.PhoneNumber != null && tblExchangeAbbstatusHistory.CreatedDate != null)
                        {
                            DateTime complaintDate = Convert.ToDateTime(tblExchangeAbbstatusHistory.CreatedDate);
                            DateTime todaysdate = DateTime.Now;
                            TimeSpan variable = todaysdate - complaintDate;
                            ElapsedHrs = Convert.ToInt32(variable.Days * 24);
                            ElapsedHrs = ElapsedHrs + variable.Hours;
                            if (ElapsedHrs <= 48)
                            {
                                #region whatsappNotification for UPI No and Pickup Date time

                                if (isupirequired == true)
                                {
                                    WhatasappResponse whatasappresponse = new WhatasappResponse();

                                    WhatsappTemplate whatsappObj = new WhatsappTemplate();
                                    whatsappObj.userDetails = new UserDetails();
                                    whatsappObj.notification = new QCFinalPrice();
                                    whatsappObj.notification.@params = new SendDate();
                                    whatsappObj.userDetails.number = tblCustomerDetail.PhoneNumber;
                                    whatsappObj.notification.sender = _baseConfig.Value.YelloaiSenderNumber;
                                    whatsappObj.notification.type = _baseConfig.Value.YellowaiMesssaheType;
                                    whatsappObj.notification.@params.Customername = tblCustomerDetail.FirstName + " " + tblCustomerDetail.LastName;
                                    whatsappObj.notification.templateId = NotificationConstants.WaitingForPrice_Approval_instant_settlement;// deferred_settlement Template
                                    whatsappObj.notification.@params.FinalQcPrice = tblExchangeOrder.FinalExchangePrice;

                                    baseUrl = _baseConfig.Value.BaseURL + "PaymentDetails/ConfirmPaymentDetails?regdNo=" + tblExchangeOrder.RegdNo + "&userid=" + Convert.ToInt32(_loginSession.UserViewModel.UserId) + "&status=" + tblExchangeOrder.StatusId;
                                    whatsappObj.notification.@params.PageLink = baseUrl;
                                    url = _baseConfig.Value.YellowAiUrl;

                                    RestResponse response = _whatsappNotificationManager.Rest_InvokeWhatsappserviceCall(url, Method.Post, whatsappObj);
                                    try
                                    {
                                        if (response.Content != null)
                                        {
                                            tblwhatsappmessage = new TblWhatsAppMessage();
                                            whatasappresponse = JsonConvert.DeserializeObject<WhatasappResponse>(response.Content);
                                            //tblwhatsappmessage.TemplateName = NotificationConstants.WaitingForPrice_Approval_deferred_settlement;
                                            tblwhatsappmessage.TemplateName = whatsappObj.notification.templateId;
                                            tblwhatsappmessage.IsActive = true;
                                            tblwhatsappmessage.PhoneNumber = tblCustomerDetail.PhoneNumber;
                                            tblwhatsappmessage.SendDate = DateTime.Now;
                                            tblwhatsappmessage.MsgId = whatasappresponse.msgId;
                                            _whatsAppMessageRepository.Create(tblwhatsappmessage);
                                            result = _whatsAppMessageRepository.SaveChanges();
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        _logging.WriteErrorToDB("PriceQuotedQCModel", "PriceQuotedQCModel", ex);
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

                                    baseUrl = _baseConfig.Value.BaseURL + "PaymentDetails/ConfirmPaymentDetails?regdNo=" + tblExchangeOrder.RegdNo + "&userid=" + Convert.ToInt32(_loginSession.UserViewModel.UserId) + "&status=" + tblExchangeOrder.StatusId;
                                    whatsappPickUpDateObj.notification.@params.PageLink = baseUrl;
                                    url = _baseConfig.Value.YellowAiUrl;

                                    RestResponse response = _whatsappNotificationManager.Rest_InvokeWhatsappserviceCall(url, Method.Post, whatsappPickUpDateObj);
                                    try
                                    {
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
                                            result = _whatsAppMessageRepository.SaveChanges();
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        _logging.WriteErrorToDB("PriceQuotedQCModel", "PriceQuotedQCModel", ex);
                                    }

                                }

                                #endregion

                                #region Send Msg by Whatsapp
                                //WhatsappTemplate whatsappObj = new WhatsappTemplate();
                                //whatsappObj.userDetails = new UserDetails();
                                //whatsappObj.notification = new QCFinalPrice();
                                //whatsappObj.notification.@params = new SendDate();
                                //whatsappObj.userDetails.number = tblCustomerDetail.PhoneNumber;
                                //whatsappObj.notification.sender = _baseConfig.Value.YelloaiSenderNumber;
                                //whatsappObj.notification.type = _baseConfig.Value.YellowaiMesssaheType;
                                //if (isupirequired == true)
                                //{
                                //    whatsappObj.notification.templateId = NotificationConstants.WaitingForPrice_Approval_instant_settlement;// deferred_settlement Template
                                //}
                                //else
                                //{
                                //    whatsappObj.notification.templateId = NotificationConstants.WaitingForPrice_Approval_deferred_settlement; // instant_settlement Template                                
                                //}
                                ////whatsappObj.notification.templateId = NotificationConstants.WaitingForPrice_Approval;
                                //whatsappObj.notification.@params.Customername = tblCustomerDetail.FirstName + " " + tblCustomerDetail.LastName;
                                //whatsappObj.notification.@params.FinalQcPrice = tblExchangeOrder.FinalExchangePrice;
                                //string baseUrl = _baseConfig.Value.BaseURL + "PaymentDetails/ConfirmPaymentDetails?regdNo=" + tblExchangeOrder.RegdNo + "&userid=" + Convert.ToInt32(_loginSession.UserViewModel.UserId) + "&status=" + tblExchangeOrder.StatusId;
                                //whatsappObj.notification.@params.PageLink = baseUrl;
                                //string url = _baseConfig.Value.YellowAiUrl;
                                //RestResponse response = _whatsappNotificationManager.Rest_InvokeWhatsappserviceCall(url, Method.Post, whatsappObj);
                                #endregion


                                #region Store WhatsappMsg Detail in DB
                                //try
                                //{

                                //    if (response.Content != null)
                                //    {
                                //        whatasappResponse = JsonConvert.DeserializeObject<WhatasappResponse>(response.Content);
                                //        tblWhatsAppMessage = new TblWhatsAppMessage();
                                //        tblWhatsAppMessage.TemplateName = whatsappObj.notification.templateId;
                                //        //tblWhatsAppMessage.TemplateName = NotificationConstants.WaitingForPrice_Approval;
                                //        tblWhatsAppMessage.IsActive = false;
                                //        tblWhatsAppMessage.PhoneNumber = tblCustomerDetail.PhoneNumber;
                                //        tblWhatsAppMessage.IsActive = true;
                                //        tblWhatsAppMessage.SendDate = DateTime.Now;
                                //        tblWhatsAppMessage.MsgId = whatasappResponse.msgId;
                                //        _whatsAppMessageRepository.Create(tblWhatsAppMessage);
                                //        result = _whatsAppMessageRepository.SaveChanges();
                                //    }
                                //}
                                //catch (Exception ex)
                                //{
                                //    _logging.WriteErrorToDB("PriceQuotedQCModel", "PriceQuotedQCModel", ex);
                                //}
                                #endregion
                            }
                        }
                    }
                }
                #endregion
            }
            if (result > 0)
            {
                ViewData["SuccessMessage"] = "UPI Verification Link Send Sucessfully";
                //Write code to show message
                //ShowMessage("UPI Verification Link Send Sucessfully", MessageTypeEnum.error);
            }
            else
            {
                ViewData["ErrorMessage"] = "UPI Verification Link Not Send";
                //Write code to show message
                //ShowMessage("UPI Verification Link Not Send", MessageTypeEnum.error);
            }
            return RedirectToPage("/QCIndex/PriceQuotedQC");
        }
        #endregion

        #region Cancel 5W status Order
        public IActionResult OnPostUpdateAsync()
        {
            string mystring = QCCommentViewModel.Exchangeidlist;
            string comment = QCCommentViewModel.Qccomments;

            if (_loginSession == null)
            {
                return RedirectToPage("ReopenedQC");
            }
            else
            {
                if (mystring != null)
                {
                    var query = from val in mystring.Split(',')
                                select int.Parse(val);
                    foreach (int num in query)
                    {
                        TblExchangeOrder tblExchangeOrders = _ExchangeOrderRepository.GetSingle(x => x.Id == num);
                        if (tblExchangeOrders != null)
                        {
                            TblOrderTran tblOrderTrans = _orderTransRepository.GetSingle(x => x.RegdNo == tblExchangeOrders.RegdNo);
                            if (tblOrderTrans != null)
                            {
                                #region Exchange Order
                                tblExchangeOrders.IsActive = true;
                                tblExchangeOrders.OrderStatus = "Cancel Order for QC";
                                tblExchangeOrders.StatusId = Convert.ToInt32(OrderStatusEnum.QCOrderCancel);
                                tblExchangeOrders.Comment1 = comment;
                                tblExchangeOrders.ModifiedBy = _loginSession.UserViewModel.UserId;
                                tblExchangeOrders.ModifiedDate = _currentDatetime;
                                _ExchangeOrderRepository.Update(tblExchangeOrders);
                                _ExchangeOrderRepository.SaveChanges();
                                TblExchangeAbbstatusHistory tblExchangeAbbstatusHistory = new TblExchangeAbbstatusHistory();

                                #endregion

                                tblOrderTrans.StatusId = Convert.ToInt32(OrderStatusEnum.QCOrderCancel);
                                tblOrderTrans.ModifiedBy = _loginSession.UserViewModel.UserId;
                                tblOrderTrans.ModifiedDate = _currentDatetime;
                                _orderTransRepository.Update(tblOrderTrans);
                                _orderTransRepository.SaveChanges();
                                
                                TblOrderQc tblOrderQc = _orderQCRepository.GetQcorderBytransId(tblOrderTrans.OrderTransId);
                                if (tblOrderQc != null)
                                {
                                    tblOrderQc.StatusId = Convert.ToInt32(OrderStatusEnum.QCOrderCancel);
                                    tblOrderQc.Qccomments = comment;
                                    tblOrderQc.ModifiedBy = _loginSession.UserViewModel.UserId;
                                    tblOrderQc.ModifiedDate = _currentDatetime;
                                    _orderQCRepository.Update(tblOrderQc);
                                    _orderQCRepository.SaveChanges();
                                }

                                #region Update tblExchangeAbbstatusHistory
                                tblExchangeAbbstatusHistory.OrderTransId = tblOrderTrans.OrderTransId;
                                tblExchangeAbbstatusHistory.OrderType = 17;
                                tblExchangeAbbstatusHistory.SponsorOrderNumber = tblExchangeOrders.SponsorOrderNumber;
                                tblExchangeAbbstatusHistory.CustId = tblExchangeOrders.CustomerDetailsId;
                                tblExchangeAbbstatusHistory.RegdNo = tblExchangeOrders.RegdNo;
                                tblExchangeAbbstatusHistory.StatusId = Convert.ToInt32(OrderStatusEnum.QCOrderCancel);
                                tblExchangeAbbstatusHistory.Comment = comment;
                                tblExchangeAbbstatusHistory.IsActive = true;
                                tblExchangeAbbstatusHistory.CreatedBy = _loginSession.UserViewModel.UserId;
                                tblExchangeAbbstatusHistory.CreatedDate = _currentDatetime;
                                _exchangeABBStatusHistoryRepository.Create(tblExchangeAbbstatusHistory);
                                _exchangeABBStatusHistoryRepository.SaveChanges();
                                #endregion
                            }
                        }
                    }

                }

            }
            return RedirectToPage("ReopenedQC");
        }
        #endregion

        #region Autopopulate Search Filter for search by RegdNo
        public IActionResult OnGetSearchRegdNo(string term)
        {
            if (term == null)
            {
                return BadRequest();
            }
            //string searchTerm = term.ToString();

            var data = _context.TblExchangeOrders
            .Where(x => x.RegdNo.Contains(term)
            && (x.StatusId == Convert.ToInt32(OrderStatusEnum.Waitingforcustapproval) || x.StatusId == Convert.ToInt32(OrderStatusEnum.QCByPass)))
            .Select(x => x.RegdNo)
            .ToArray();
            return new JsonResult(data);
        }
        #endregion
    }
}
