using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DocumentFormat.OpenXml.Drawing;
using DocumentFormat.OpenXml.Math;
using DocumentFormat.OpenXml.Presentation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RestSharp.Portable;
using SendGrid;
using RDCELERP.BAL.Helper;
using RDCELERP.BAL.Interface;
using RDCELERP.BAL.MasterManager;
using RDCELERP.Common.Enums;
using RDCELERP.Common.Helper;

using RDCELERP.Core.App.Pages.Base;
using RDCELERP.DAL.Entities;
using RDCELERP.DAL.IRepository;
using RDCELERP.DAL.Repository;
using RDCELERP.Model.Base;
using RDCELERP.Model.EVC_Portal;
using RDCELERP.Model.Paymant;
using UTC_Bridge.DocUpload.BAL.Common;
using UTC_Model.PluralGateway;
using TblPaymentLeaser = RDCELERP.DAL.Entities.TblPaymentLeaser;

using RestResponse = RestSharp.RestResponse;
using static Org.BouncyCastle.Math.EC.ECCurve;

namespace RDCELERP.Core.App.Pages.EVC_Portal
{
    public class WalletRechargeModel : BasePageModel
    {
        private readonly RDCELERP.DAL.Entities.Digi2l_DevContext _context;
        private readonly IEVCManager _EVCManager;
        private readonly IPaymentLeaser _paymentRepository;
        IPluralGatewayManager _pluralGatewayManager;
        ILogging _logging;
        DateTime _currentDatetime = DateTime.Now.TrimMilliseconds();

        public WalletRechargeModel(RDCELERP.DAL.Entities.Digi2l_DevContext context, IOptions<ApplicationSettings> config, IEVCManager EVCManager, ILogging logging, IPaymentLeaser paymentLeaser, IPluralGatewayManager pluralGatewayManager)
      : base(config)
        {
            _EVCManager = EVCManager;
            _context = context;
            _logging = logging;
            _paymentRepository = paymentLeaser;
            _pluralGatewayManager = pluralGatewayManager;
        }

        [BindProperty(SupportsGet = true)]
        public PaymentInitiateModel PaymentInitiateModels { get; set; }

        #region 

        public IActionResult OnGet(int EVCRegistration)
        {
            if (_loginSession == null)
            {
                return RedirectToPage("/index");
            }
            else
            {
                #region Zaakpay paymant Gateway
                if (_baseConfig.Value.ZaakPayActive)
                {
                    if (EVCRegistration > 0)
                    {
                        TblEvcregistration tblEvcregistration = _context.TblEvcregistrations.Where(x => x.EvcregistrationId == EVCRegistration && x.IsActive == true && x.Isevcapprovrd == true).FirstOrDefault();
                        if (tblEvcregistration != null)
                        {
                            PaymentInitiateModels.EvcregistrationId = tblEvcregistration.EvcregistrationId > 0 ? tblEvcregistration.EvcregistrationId : 0;
                            PaymentInitiateModels.BussinessName = tblEvcregistration.BussinessName != null ? tblEvcregistration.BussinessName : String.Empty;
                            PaymentInitiateModels.EVCemail = tblEvcregistration.EmailId != null ? tblEvcregistration.EmailId : String.Empty;
                            PaymentInitiateModels.EVCcontactNumber = tblEvcregistration.EvcmobileNumber != null ? tblEvcregistration.EvcmobileNumber : String.Empty;
                            PaymentInitiateModels.EVCaddress = tblEvcregistration.ContactPersonAddress != null ? tblEvcregistration.ContactPersonAddress : String.Empty;
                            PaymentInitiateModels.EVCRegdNo = tblEvcregistration.EvcregdNo != null ? tblEvcregistration.EvcregdNo : String.Empty;
                        }

                        ViewData["BaseUrl"] = _baseConfig.Value.BaseURL;
                        return Page();
                    }
                    else
                    {
                        return RedirectToPage("/EVC_Portal/EVC_Dashboard");
                    }
                }
                #endregion

                #region Plural paymant Gateway
                if (_baseConfig.Value.PluralActive)
                {
                    if (EVCRegistration > 0)
                    {
                        DAL.Entities.TblEvcregistration tblEvcregistration = _context.TblEvcregistrations.Where(x => x.EvcregistrationId == EVCRegistration && x.IsActive == true && x.Isevcapprovrd == true).FirstOrDefault();
                        if (tblEvcregistration != null)
                        {
                            PaymentInitiateModel payment = new PaymentInitiateModel();
                            if (PaymentInitiateModels != null)
                            {
                                string KeyString = _baseConfig.Value.EncryptionKey.ToString();
                                string IV = _baseConfig.Value.EncryptionIV.ToString();
                                string returnurl = _baseConfig.Value.BaseURL.ToString() + "EVC_Portal/PaymentPage";

                                PaymentInitiateModels.EvcregistrationId = tblEvcregistration.EvcregistrationId > 0 ? tblEvcregistration.EvcregistrationId : 0;
                                PaymentInitiateModels.BussinessName = tblEvcregistration.BussinessName != null ? tblEvcregistration.BussinessName : String.Empty;
                                PaymentInitiateModels.EVCemail = tblEvcregistration.EmailId != null ? tblEvcregistration.EmailId : String.Empty;
                                PaymentInitiateModels.EVCcontactNumber = tblEvcregistration.EvcmobileNumber != null ? tblEvcregistration.EvcmobileNumber : String.Empty;
                                PaymentInitiateModels.EVCaddress = tblEvcregistration.ContactPersonAddress != null ? tblEvcregistration.ContactPersonAddress : String.Empty;
                                PaymentInitiateModels.EVCRegdNo = tblEvcregistration.EvcregdNo != null ? tblEvcregistration.EvcregdNo : String.Empty;

                            }

                            ViewData["BaseUrl"] = _baseConfig.Value.BaseURL;
                            return Page();
                        }

                    }
                    else
                    {
                        return RedirectToPage("/EVC_Portal/EVC_Dashboard");
                    }
                  
                }
               
                    return RedirectToPage("/EVC_Portal/EVC_Dashboard");
                
                #endregion
                
            }
        }
        public IActionResult OnPostAsync()
        {
            
            #region Plural Payment Gatway
            // Generate random receipt number for order
            OrderModel orderModel = new OrderModel();
            PluralCreateOrder createorder = new PluralCreateOrder();
            CreateOrderResponse orderResponse = new CreateOrderResponse();
            CreateOrderErrorResponse errorResponse = new CreateOrderErrorResponse();
            string ReturnUrl = null;
            string Message = null;
            RestResponse response = null;
            string returnurl = "EVC_Portal/EVC_Dashboard";
            List<TblPaymentLeaser> paymenttTableObj = new List<TblPaymentLeaser>();
            try
            {
                if (_baseConfig.Value.PluralActive)
                {
                    PaymentInitiateModels.EVCRegdNo = UniqueString.RandomNumberByLength(5) + "_" + PaymentInitiateModels.EVCRegdNo;
                    TempData["RegNo"] = PaymentInitiateModels.EVCRegdNo;
                    pluralgatewayKey apikey = new pluralgatewayKey();
                    apikey = getPaymentKey();
                    ReturnUrl = _baseConfig.Value.BaseURL.ToString() + "EVC/EVC_Dashboard";
                    //merchant data
                    createorder.merchant_data = new MerchantData();
                    createorder.merchant_data.merchant_id = apikey.merchantId;

                    createorder.merchant_data.merchant_order_id = PaymentInitiateModels.EVCRegdNo;
                    createorder.merchant_data.merchant_return_url = ReturnUrl;
                    createorder.merchant_data.merchant_access_code = apikey.accessCode;

                    // Payment information
                    createorder.payment_info_data = new PaymentInfoData();
                    createorder.payment_info_data.amount = Convert.ToInt32(PaymentInitiateModels.amount * 100);
                    createorder.payment_info_data.currency_code = "INR";
                    createorder.payment_info_data.order_desc = "EVC Wallet Recharge";



                    // Customer information
                    createorder.customer_data = new UTC_Model.PluralGateway.CustomerData();
                    createorder.customer_data.country_code = Convert.ToInt32(PluralGatewayEnum.CountryCode).ToString();
                    createorder.customer_data.mobile_number = PaymentInitiateModels.EVCcontactNumber;
                    createorder.customer_data.email_id = PaymentInitiateModels.EVCemail;

                    //ShippingAddress  information
                    createorder.shipping_address_data = new ShippingAddressData();
                    createorder.shipping_address_data.first_name = PaymentInitiateModels.BussinessName;
                    createorder.shipping_address_data.address1 = PaymentInitiateModels.EVCaddress;
                    createorder.shipping_address_data.pin_code = PaymentInitiateModels.EVCPin;
                    createorder.shipping_address_data.city = PaymentInitiateModels.EVCCity;
                    createorder.shipping_address_data.state = PaymentInitiateModels.EVCstate;
                    createorder.shipping_address_data.country = "India";

                    //BillingAddress information
                    createorder.billing_address_data = new BillingAddressData();
                    createorder.billing_address_data.first_name = PaymentInitiateModels.BussinessName;
                    createorder.billing_address_data.address1 = PaymentInitiateModels.EVCaddress;
                    createorder.billing_address_data.pin_code = PaymentInitiateModels.EVCPin;
                    createorder.billing_address_data.city = PaymentInitiateModels.EVCCity;
                    createorder.billing_address_data.state = PaymentInitiateModels.EVCstate;
                    createorder.billing_address_data.country = "India";

                    //Additional information
                    createorder.additional_info_data = new AdditionalInfoData();
                    createorder.additional_info_data.rfu1 = "123456";

                    response = _pluralGatewayManager.CreateOrderManager(createorder, apikey.secretKey);

                    string SuccessResponse = response.StatusCode.ToString();
                    if (SuccessResponse == "OK")
                    {
                        orderResponse = JsonConvert.DeserializeObject<CreateOrderResponse>(response.Content);
                    }
                    else
                    {
                        errorResponse = JsonConvert.DeserializeObject<CreateOrderErrorResponse>(response.Content);
                    }

                    if (orderResponse != null && orderResponse.token != null)
                    {
                        orderModel.channelId = "WEB";
                        orderModel.theme = "default";
                        orderModel.orderToken = orderResponse.token;
                        orderModel.paymentMode = "CREDIT_DEBIT,NETBANKING,UPI,WALLET,EMI,DEBIT_EMI";
                        orderModel.countryCode = Convert.ToInt32(PluralGatewayEnum.CountryCode).ToString();
                        orderModel.mobileNumber = PaymentInitiateModels.EVCcontactNumber;
                        orderModel.emailId = PaymentInitiateModels.EVCemail;
                        orderModel.showSavedCardsFeature = false;
                    }
                    else
                    {
                        Message = "Unable to process payment at the moment please contact with administrator";
                        TempData["Msg"] = Message;
                        return Redirect("../PayOut/DetailsForFailedTransaction/?message=" + Message + "&ReturnURL=" + returnurl);
                    }
                }
                if (_baseConfig.Value.ZaakPayActive)
                {
                    #region
                    PaymentInitiateModel _requestData = new PaymentInitiateModel();
                    _requestData = PaymentInitiateModels;

                    string transactionId = _requestData.EVCRegdNo;
                    ZaakpayKey apikey = new ZaakpayKey();
                    apikey = getPaymentZaakpayKey();
                    //  string returnurl = "http://localhost:44318/ERP/EVCCompletePayment?UserId="+_loginSession.UserViewModel.UserId;
                    string MVCreturnurl = _baseConfig.Value.MVCBaseURL + "/ERP/EVCCompletePayment?UserId=" + _loginSession.UserViewModel.UserId;      /* "http://103.127.146.29/QA/ABB/Complete"; */                    /* "http://localhost:44332/api/EVCList/Complete"; */           /*"CompletePaymant";*/
                    double amount = Convert.ToDouble(_requestData.amount);
                    Dictionary<string, string> options = new Dictionary<string, string>();
                    options.Add("amount", (amount * 100).ToString());  // Amount will in paise
                    options.Add("buyerEmail", _requestData.EVCemail);  // Amount will in paise
                    options.Add("currency", "INR");
                    options.Add("merchantIdentifier", apikey.merchantId);
                    options.Add("orderId", UniqueString.RandomNumberByLength(5) + "_" + _requestData.EVCRegdNo);
                    options.Add("returnUrl", MVCreturnurl);
                    string allparameters = ChecksumCalculator.generateSignature(options);

                    var a = UniqueString.RandomNumberByLength(5) + "_" + _requestData.EVCRegdNo;
                    string[] orderIdParts = a.Split('_');
                    string EVCregdNo = orderIdParts[1];

                    allparameters = allparameters + "&";
                    string checksum = ChecksumCalculator.calculateChecksum(apikey.secretKey, allparameters);
                    bool flag = ChecksumCalculator.verifyChecksum(apikey.secretKey, allparameters, checksum);
                    string ZaakPayUrl = _baseConfig.Value.ZaakPayUrl.ToString().Trim() + "?" + allparameters + "checksum=" + checksum;
                    return Redirect(ZaakPayUrl);
                }
                #endregion

            }
            catch (Exception ex)
            {
                //LibLogging.WriteErrorToDB("ABBController", "CreateOrder", ex);
                Message = "Unable to process payment at the moment please contact with administrator";
                TempData["Msg"] = Message;
                return Redirect("../PayOut/DetailsForFailedTransaction/?message=" + Message + "&ReturnURL=" + returnurl);
            }
            return RedirectToPage("PaymentPage", new { orderToken = orderModel.orderToken, channelId = orderModel.channelId, theme = orderModel.theme, paymentMode = orderModel.paymentMode, countryCode = orderModel.countryCode, mobileNumber = orderModel.mobileNumber, emailId = orderModel.emailId, showSavedCardsFeature = orderModel.showSavedCardsFeature });
            // return RedirectToAction("/EVC_Portal/PaymentPage", new { OrderModel = "orderModel" });
            #endregion
         
            
        }
        #endregion
        public pluralgatewayKey getPaymentKey()
        {
            pluralgatewayKey pluralkey = new pluralgatewayKey();
            pluralkey.merchantId = _baseConfig.Value.MerchantIdPlural.ToString();
            pluralkey.secretKey = _baseConfig.Value.SecretKeyPlural.ToString();
            pluralkey.accessCode = _baseConfig.Value.AccessCodePlural.ToString();
            return pluralkey;
        }

        public ZaakpayKey getPaymentZaakpayKey()
        {
            ZaakpayKey zaakpay = new ZaakpayKey();
            zaakpay.merchantId = _baseConfig.Value.merchantId.ToString();
            zaakpay.secretKey = _baseConfig.Value.secretKey.ToString();
            return zaakpay;
        }
    }
}