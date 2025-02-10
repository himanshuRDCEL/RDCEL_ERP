using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;
using SendGrid;
using RDCELERP.BAL.Interface;
using RDCELERP.BAL.MasterManager;
using RDCELERP.Common.Enums;
using RDCELERP.Common.Helper;
using RDCELERP.Core.App.Pages.Base;
using RDCELERP.DAL.Entities;
using RDCELERP.DAL.IRepository;
using RDCELERP.DAL.Repository;
using RDCELERP.Model.Base;
using RDCELERP.Model.Paymant;
using UTC_Model.PluralGateway;

namespace RDCELERP.Core.App.Pages.EVC_Portal
{
    public class PaymentPageModel : BasePageModel
    {
        private readonly RDCELERP.DAL.Entities.Digi2l_DevContext _context;
        private readonly IEVCManager _EVCManager;
        private readonly IPaymentLeaser _paymentRepository;
        private readonly IEVCRepository _eVCRepository;

        IPluralGatewayManager _pluralGatewayManager;
        ILogging _logging;
        DateTime _currentDatetime = DateTime.Now.TrimMilliseconds();


        public PaymentPageModel(RDCELERP.DAL.Entities.Digi2l_DevContext context, IOptions<ApplicationSettings> config, IEVCManager EVCManager, ILogging logging, IPaymentLeaser paymentLeaser, IPluralGatewayManager pluralGatewayManager, IEVCRepository eVCRepository)
      : base(config)
        {
            _EVCManager = EVCManager;
            _context = context;
            _logging = logging;
            _paymentRepository = paymentLeaser;
            _pluralGatewayManager = pluralGatewayManager;
            _eVCRepository = eVCRepository;
        }


        [BindProperty(SupportsGet = true)]
        public OrderModel OrderModel1 { get; set; }
        public void OnGet(OrderModel OrderModel2)
        {
            OrderModel1.channelId = OrderModel2.channelId;
            OrderModel1.countryCode = OrderModel2.countryCode;
            OrderModel1.emailId = OrderModel2.emailId;
            OrderModel1.orderToken = OrderModel2.orderToken;
            OrderModel1.paymentMode = OrderModel2.paymentMode;
            OrderModel1.mobileNumber = OrderModel2.mobileNumber;
        }             
        public IActionResult OnPost()
        {

            GetpaymentStatusResponse paymentResponse = new GetpaymentStatusResponse();

            string Message = null;
            string Orderstatus = null;
            string dbresponse = string.Empty;
            int amount = 0;
            string returnurl = "EVC_Portal/EVC_Dashboard";
            string msg = string.Empty;
            try
            {
                //Code to get error message if any
                string errorCode = OrderModel1.errorcode;
                //Code to get error message if any
                string errorMessage = OrderModel1.errrorResponse;
                //Code to get Payment Id 
                string PaymentId = OrderModel1.PaymentId;
                //Code to get Order Id 
                string OrderId = OrderModel1.OrderId;
                if (PaymentId != null && OrderId != null && PaymentId != "undefined" && OrderId != "undefined")
                {
                    paymentResponse = _pluralGatewayManager.GetpaymentStatus(PaymentId, OrderId);
                    PaymentResponseModel response = new PaymentResponseModel();
                    if (paymentResponse.payment_info_data.captured_amount_in_paisa != null)
                    {
                        amount = Convert.ToInt32(paymentResponse.payment_info_data.captured_amount_in_paisa);
                        amount = amount / 100;
                        response.amount = Convert.ToDecimal(amount);
                    }
                    else
                    {
                        response.amount = 0;
                    }
                    response.paymentMode = paymentResponse.payment_info_data.payment_mode;
                    response.paymentmethod = paymentResponse.payment_info_data.payment_mode;
                    //response.gatewayTransactionId = paymentResponse.payment_info_data.gateway_payment_id;
                    response.OrderId = paymentResponse.merchant_data.order_id;
                    response.transactionId = paymentResponse.payment_info_data.payment_id;
                    response.responseDescription = paymentResponse.payment_info_data.payment_response_message;
                    response.gatewayTransactionId = paymentResponse.payment_info_data.gateway_payment_id;
                    response.responseCode = paymentResponse.payment_info_data.payment_response_code;
                    response.pgTransTime = paymentResponse.payment_info_data.payment_completion_date_time.ToString();
                    response.status = paymentResponse.payment_info_data.payment_status;
                    response.orderStatus = paymentResponse.order_data.order_status;                    

                    string[] orderIdParts = response.OrderId.Split('_');
                    string EVCregdNo = orderIdParts[1];
                    response.RegdNo = EVCregdNo;
                    dbresponse = _EVCManager.EVCPaymentstatusUpdate(response);

                    //// Check payment made successfully
                    string ResponseForSuccessfullPayment = null;
                    ResponseForSuccessfullPayment = EnumHelper.DescriptionAttr(PluralEnum.PaymentStatus);
                    Orderstatus = EnumHelper.DescriptionAttr(PluralEnum.OrderStatus);
                    
                    if (paymentResponse.payment_info_data.payment_status == ResponseForSuccessfullPayment && paymentResponse.order_data.order_status == Orderstatus)
                    {
                        TblEvcregistration registrationObj = _eVCRepository.GetSingle(x => x.EvcregdNo == response.RegdNo);
                        if (registrationObj != null)
                        {

                            if (dbresponse == "success" && registrationObj != null)
                            {

                                msg = "Thank You " + registrationObj.EvcregdNo + " " + registrationObj.BussinessName
                                + " Wallet has been recharged Successfully Transaction Id "
                                + response.transactionId + " for Wallet Amount " + response.amount +
                                ". Your Current Wallet Balance is " + registrationObj.EvcwalletAmount;
                                if (!string.IsNullOrEmpty(msg))
                                    TempData["Msg"] = msg;
                                // Create these action method
                                return Redirect("../PayOut/Details/?message=" + msg + "&ReturnURL=" + returnurl);
                            }
                        }
                    }
                    else
                    {
                        msg = "Payment Is Failed  transactionId  " + response.transactionId + " EVC Reg Id  -" + response.RegdNo;
                        if (!string.IsNullOrEmpty(msg))
                            TempData["Msg"] = msg;
                        return Redirect("../PayOut/DetailsForFailedTransaction/?message=" + msg + "&ReturnURL="+ returnurl);
                    }                   
                }
                else
                {
                    msg = "Order received could not initiate paymnet at the moment please ccontact to administrator";
                    TempData["Msg"] = Message;
                    return Redirect("../PayOut/DetailsForFailedTransaction/?message=" + msg + "&ReturnURL=" + returnurl);
                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("ABBController", "Complete", ex);
                msg = "Unable to process payment at the moment please contact with administrator";
                TempData["Msg"] = Message;
                return Redirect("../PayOut/DetailsForFailedTransaction/?message=" + msg + "&ReturnURL=" + returnurl);
            }
            return Redirect("../PayOut/DetailsForFailedTransaction/?message=" + msg + "&ReturnURL=" + returnurl);
        }    
    }

}
