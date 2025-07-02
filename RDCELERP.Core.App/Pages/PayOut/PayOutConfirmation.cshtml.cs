using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;
using RDCELERP.BAL.Interface;
using RDCELERP.BAL.MasterManager;
using RDCELERP.Common.Enums;
using RDCELERP.Common.Helper;
using RDCELERP.Core.App.Pages.Base;
using RDCELERP.DAL.Entities;
using RDCELERP.DAL.IRepository;
using RDCELERP.DAL.Repository;
using RDCELERP.Model.Base;
using RDCELERP.Model.CashfreeModel;
using RDCELERP.Model.RazorPayX;

namespace RDCELERP.Core.App.Pages.PayOut
{
    public class PayOutConfirmationModel : BasePageModel
    {

        #region variable declaration
        private readonly RDCELERP.DAL.Entities.Digi2l_DevContext _context;
        public readonly IOptions<ApplicationSettings> _config;
        private CustomDataProtection _protector;
        private readonly IExchangeOrderManager _exchangeOrderManager;
        private readonly IDealerManager _dashBoardManager;
        IExchangeOrderRepository _exchangeOrderRepository;
        IBusinessPartnerRepository _businessPartnerRepository;
        ICashfreePayoutCall _cashfreePayoutCall;
        IPaymentLeaser _paymentLeaserRepository;
        IOrderTransRepository _orderTransRepository;
        IABBRedemptionRepository _redemptionRepository;
        public ILogging _logging;
        IRazorpayXService _razorpayXService;
        ICustomerDetailsRepository _customerDetailsRepository;
        #endregion

        #region Constructor
        public PayOutConfirmationModel(  IRazorpayXService razorpayXService, IOptions<ApplicationSettings> config, Digi2l_DevContext _dbcontext, CustomDataProtection _dataprotector, IExchangeOrderManager exchangeOrderManager, IDealerManager dealerDashBoardManager, ILogging logging, IBusinessPartnerRepository businessPartnerRepository, IExchangeOrderRepository exchangeOrderRepository, ICashfreePayoutCall cashfreecall, IPaymentLeaser paymentLeaser, IOrderTransRepository orderTransRepository, IABBRedemptionRepository redemptionRepository, ICustomerDetailsRepository customerDetailsRepository) : base(config)
        {
            _config = config;
            _context = _dbcontext;
            _protector = _dataprotector;
            _exchangeOrderManager = exchangeOrderManager;
            _dashBoardManager = dealerDashBoardManager;
            _logging = logging;
            _businessPartnerRepository = businessPartnerRepository;
            _exchangeOrderRepository = exchangeOrderRepository;
            _cashfreePayoutCall = cashfreecall;
            _paymentLeaserRepository = paymentLeaser;
            _orderTransRepository = orderTransRepository;
            _redemptionRepository = redemptionRepository;
            _razorpayXService = razorpayXService;
            _customerDetailsRepository = customerDetailsRepository;
        }
        #endregion

        #region Model Binding
        [BindProperty(SupportsGet = true)]
        public CashfreeAuth authObject { get; set; }
        [BindProperty(SupportsGet = true)]
        public ProcessTransactionCashfree transaction { get; set; }
        [BindProperty(SupportsGet = true)]
        public GetBeneficiary getbeneficiary { get; set; }
        
        [BindProperty(SupportsGet = true)]
        public RazorpayFundAccountInfo RazorpayFundAccModel { get; set; }

        [BindProperty(SupportsGet = true)]
        public TblExchangeOrder ExchangeObj { get; set; }

        [BindProperty(SupportsGet = true)]
        public TblOrderTran OrderTransObj { get; set; }
        [BindProperty(SupportsGet = true)]
        public TblAbbredemption redemptionObj { get; set; }
        #endregion

        //public IActionResult OnGet(string RegdNo)
        //{
        //    GetBeneficiary getBeneficiarry = new GetBeneficiary();
        //    CashfreeAuth cashfreeAuthCall = new CashfreeAuth();
        //    string subcode = null;
        //    string message = null;
        //    string returnurl = "LGC/LogiPickDrop";
        //    try
        //    {
        //        RegdNo = SecurityHelper.DecryptString(RegdNo, _config.Value.SecurityKey);


        //        subcode = Convert.ToInt32(CashfreeEnum.Succcess).ToString();
        //        if (RegdNo != null)
        //        {
        //            OrderTransObj = _orderTransRepository.GetOrdertransDetails(RegdNo);
        //            if (OrderTransObj != null)
        //            {
        //                if (OrderTransObj.OrderType == Convert.ToInt32(OrderTypeEnum.Exchange))
        //                {

        //                    ExchangeObj = _context.TblExchangeOrders.FirstOrDefault(x => x.RegdNo == RegdNo);
        //                    if (ExchangeObj != null)
        //                    {
        //                        cashfreeAuthCall = _cashfreePayoutCall.CashFreeAuthCall();
        //                        cashfreeAuthCall.subCode = Convert.ToInt32(CashfreeEnum.Succcess).ToString();
        //                        if (cashfreeAuthCall.subCode == subcode)
        //                        {
        //                             getbeneficiary = _cashfreePayoutCall.GetBeneficiary(cashfreeAuthCall.data.token, RegdNo);
        //                            //getbeneficiary.subCode = Convert.ToInt32(CashfreeEnum.Succcess).ToString();
        //                            if (getbeneficiary.subCode == subcode)
        //                            {

        //                                getbeneficiary.data.FinalExchangePrice = ExchangeObj.FinalExchangePrice.ToString();
        //                                getbeneficiary.data.ExchangeId = ExchangeObj.Id;
        //                                getbeneficiary.data.ordertype = Convert.ToInt32(OrderTransObj.OrderType);
        //                            }
        //                            else
        //                            {
        //                                message = getbeneficiary.message;
        //                                return Redirect("DetailsForFailedTransaction/?message=" + message + "&ReturnURL=" + returnurl);
        //                            }
        //                        }
        //                        else
        //                        {
        //                            message = cashfreeAuthCall.message;
        //                            return Redirect("DetailsForFailedTransaction/?message=" + message + "&ReturnURL=" + returnurl);
        //                        }
        //                    }
        //                    else
        //                    {
        //                        message = "Order data not found";
        //                        return Redirect("DetailsForFailedTransaction/?message=" + message + "&ReturnURL=" + returnurl);
        //                    }
        //                }
        //                else if (OrderTransObj.OrderType == Convert.ToInt32(OrderTypeEnum.ABB))
        //                {

        //                    redemptionObj = _redemptionRepository.GetOrderDetails(RegdNo, OrderTransObj.AbbredemptionId);
        //                    if (redemptionObj != null)
        //                    {
        //                        cashfreeAuthCall = _cashfreePayoutCall.CashFreeAuthCall();
        //                        if (cashfreeAuthCall.subCode == subcode)
        //                        {
        //                            getbeneficiary = _cashfreePayoutCall.GetBeneficiary(cashfreeAuthCall.data.token, RegdNo);
        //                            if (getbeneficiary.subCode == subcode)
        //                            {
        //                                getbeneficiary.data.FinalExchangePrice = OrderTransObj.FinalPriceAfterQc.ToString();
        //                                getbeneficiary.data.ExchangeId = redemptionObj.RedemptionId;
        //                                getbeneficiary.data.ordertype = Convert.ToInt32(OrderTransObj.OrderType);
        //                            }
        //                            else
        //                            {
        //                                message = getbeneficiary.message;
        //                                return Redirect("DetailsForFailedTransaction/?message=" + message + "&ReturnURL=" + returnurl);
        //                            }
        //                        }
        //                        else
        //                        {
        //                            message = cashfreeAuthCall.message;
        //                            return Redirect("DetailsForFailedTransaction/?message=" + message + "&ReturnURL=" + returnurl);
        //                        }
        //                    }
        //                    else
        //                    {
        //                        message = "Order data not found";
        //                        return Redirect("DetailsForFailedTransaction/?message=" + message + "&ReturnURL=" + returnurl);
        //                    }
        //                }
        //                else
        //                {
        //                    message = "Order type not defined";
        //                    return Redirect("DetailsForFailedTransaction/?message=" + message + "&ReturnURL=" + returnurl);
        //                }
        //            }
        //            else
        //            {
        //                message = "Order data not found";
        //                return Redirect("DetailsForFailedTransaction/?message=" + message + "&ReturnURL=" + returnurl);
        //            }


        //        }
        //        else
        //        {
        //            message = "Please provide order number";
        //            return Redirect("Details/?message=" + message);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        _logging.WriteErrorToDB("PayOutConfirmationModel", "OnGet", ex);
        //    }
        //    return Page();
        //}

        public async Task<IActionResult> OnGetAsync(string RegdNo)
        {
            RazorpayContactInfo contactInfo = null;
            //RazorpayRazorpayFundAccModel RazorpayFundAccModel = null;
            string message = null;
            string returnurl = "LGC/LogiPickDrop";

            try
            {
                RegdNo = SecurityHelper.DecryptString(RegdNo, _config.Value.SecurityKey);
                if (string.IsNullOrEmpty(RegdNo))
                {
                    message = "Please provide order number";
                    return Redirect("Details/?message=" + message);
                }

                OrderTransObj = _orderTransRepository.GetOrdertransDetails(RegdNo);
                if (OrderTransObj == null)
                {
                    message = "Order data not found";
                    return Redirect("DetailsForFailedTransaction/?message=" + message + "&ReturnURL=" + returnurl);
                }

                if (OrderTransObj.OrderType == (int)OrderTypeEnum.Exchange)
                {
                    ExchangeObj = _context.TblExchangeOrders.FirstOrDefault(x => x.RegdNo == RegdNo);
                    if (ExchangeObj == null)
                    {
                        message = "Order data not found";
                        return Redirect("DetailsForFailedTransaction/?message=" + message + "&ReturnURL=" + returnurl);
                    }

                    var token = _razorpayXService.GenerateToken(_config.Value.RazorPayKey_Id, _config.Value.RazorPaykey_secret);


                    TblCustomerDetail tblCustomerDetail = _customerDetailsRepository.GetCustDetails(ExchangeObj.CustomerDetailsId);

                    contactInfo = await _razorpayXService.GetContactAsync(tblCustomerDetail.RazorPayxContactId, token);

                    if (contactInfo == null)
                    {
                        message = "Contact not found in RazorpayX";
                        return Redirect("DetailsForFailedTransaction/?message=" + message + "&ReturnURL=" + returnurl);
                    }

                    RazorpayFundAccModel = await _razorpayXService.GetFundAccountAsync(contactInfo.id, token);

                    if (RazorpayFundAccModel == null)
                    {
                        message = "Fund account not found in RazorpayX";
                        return Redirect("DetailsForFailedTransaction/?message=" + message + "&ReturnURL=" + returnurl);
                    }

                    // If all good, populate data
                    RazorpayFundAccModel.FinalExchangePrice = ExchangeObj.FinalExchangePrice.ToString();
                    RazorpayFundAccModel.ExchangeId = ExchangeObj.Id;
                    RazorpayFundAccModel.ordertype = (int)OrderTransObj.OrderType;
                }
                else if (OrderTransObj.OrderType == (int)OrderTypeEnum.ABB)
                {
                    redemptionObj = _redemptionRepository.GetOrderDetails(RegdNo, OrderTransObj.AbbredemptionId);
                    if (redemptionObj == null)
                    {
                        message = "Order data not found";
                        return Redirect("DetailsForFailedTransaction/?message=" + message + "&ReturnURL=" + returnurl);
                    }

                    var token = _razorpayXService.GenerateToken(_config.Value.RazorPayKey_Id, _config.Value.RazorPaykey_secret);
                    contactInfo = await _razorpayXService.GetContactAsync(RegdNo, token);
                    RazorpayFundAccModel = await _razorpayXService.GetFundAccountAsync(contactInfo.id, token);

                    if (contactInfo == null || RazorpayFundAccModel == null)
                    {
                        message = "Contact or fund account not found";
                        return Redirect("DetailsForFailedTransaction/?message=" + message + "&ReturnURL=" + returnurl);
                    }

                    RazorpayFundAccModel.FinalExchangePrice = OrderTransObj.FinalPriceAfterQc.ToString();
                    RazorpayFundAccModel.ExchangeId = redemptionObj.RedemptionId;
                    RazorpayFundAccModel.ordertype = (int)OrderTransObj.OrderType;
                }
                else
                {
                    message = "Order type not defined";
                    return Redirect("DetailsForFailedTransaction/?message=" + message + "&ReturnURL=" + returnurl);
                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("PayOutConfirmationModel", "OnGet", ex);
            }

            return Page();
        }


        public async Task<IActionResult> OnPostPayOutAsync()
        {
            try
            {
                if (RazorpayFundAccModel == null || string.IsNullOrEmpty(RazorpayFundAccModel.id))
                {
                    ViewData["Message"] = "Invalid Razorpay payout data.";
                    return Page();
                }

                string moduleType = RazorpayFundAccModel.ordertype == (int)OrderTypeEnum.ABB
                    ? EnumHelper.DescriptionAttr(CashfreeEnum.ABB)
                    : EnumHelper.DescriptionAttr(CashfreeEnum.Exchange);

                string paymentMode = "upi";
                string transactionType = "payout";
                string returnUrl = "LGC/LogiPickDrop";

                var existingPayment = _context.TblPaymentLeasers.FirstOrDefault(x =>
                    x.RegdNo == RazorpayFundAccModel.id &&
                    x.ModuleType == moduleType &&
                    x.IsActive == true &&
                    x.PaymentStatus == true);

                if (existingPayment != null)
                {
                    string msg = $"Amount= {existingPayment.Amount}/- already paid. Transaction ID: {existingPayment.TransactionId}";
                    return Redirect("Details/?message=" + msg + "&ReturnURL=" + returnUrl);
                }
                string keyId = _config.Value.RazorPayKey_Id;        
                string keySecret = _config.Value.RazorPaykey_secret; 
                string basicAuthToken = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{keyId}:{keySecret}"));

                var payoutPayload = new
                {
                    account_number = "10000000000000", // RazorpayX TEST account number
                    fund_account_id = RazorpayFundAccModel.id,
                    amount = Convert.ToInt32(Convert.ToDecimal(RazorpayFundAccModel.FinalExchangePrice) * 100), // Convert to paise
                    currency = "INR",
                    mode = "upi",
                    purpose = "refund",
                    reference_id = RazorpayFundAccModel.ExchangeId,
                    narration = "Refund to customer"
                };

                var payoutResponse = await _razorpayXService.MakePayoutAsync(payoutPayload, basicAuthToken);

                if (payoutResponse != null && !string.IsNullOrEmpty(payoutResponse.id))
                {
                    // Save to ledger
                    var ledger = new TblPaymentLeaser
                    {
                        RegdNo = RazorpayFundAccModel.id,
                        Amount = Convert.ToDecimal(RazorpayFundAccModel.FinalExchangePrice),
                        PaymentMode = paymentMode,
                        OrderId = payoutResponse.id,
                        UtcreferenceId = RazorpayFundAccModel.contact_id,
                        PaymentDate = DateTime.Now,
                        ResponseDescription = payoutResponse.status,
                        PaymentResponse = payoutResponse.status,
                        TransactionId = payoutResponse.utr,
                        ModuleType = moduleType,
                        IsActive = true,
                        PaymentStatus = payoutResponse.status == "processing" || payoutResponse.status == "processed",
                        ResponseCode = "200",
                        TransactionType = transactionType,
                        CreatedBy = RazorpayFundAccModel.ExchangeId,
                        CreatedDate = DateTime.Now,
                        ModuleReferenceId = RazorpayFundAccModel.ExchangeId
                    };

                    _paymentLeaserRepository.Create(ledger);
                    _paymentLeaserRepository.SaveChanges();

                    // Update order status
                    var order = _orderTransRepository.GetOrdertransDetails(RazorpayFundAccModel.id);
                    if (order != null && payoutResponse.status == "processed")
                    {
                        order.AmountPaidToCustomer = true;
                        _orderTransRepository.Update(order);
                        _orderTransRepository.SaveChanges();
                    }

                    string msg = $"Transaction of ?{RazorpayFundAccModel.FinalExchangePrice} is completed. UTR: {payoutResponse.utr}";
                    return Redirect("Details/?message=" + msg + "&ReturnURL=" + returnUrl);
                }
                else
                {
                    string msg = "Razorpay payout failed.";
                    return Redirect("DetailsForFailedTransaction/?message=" + msg + "&ReturnURL=" + returnUrl);
                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("PayOutConfirmationModel", "OnPostPayOutAsync", ex);
                return Page();
            }
        }
        //public IActionResult OnPostPayOut(GetBeneficiary getBeneFiciary)
        //{
        //    TblPaymentLeaser paymentLedger = null;
        //    TransactionResponseCashfree transactionResponse = new TransactionResponseCashfree();
        //    ProcessTransactionCashfree processtransaction = new ProcessTransactionCashfree();
        //    CashfreeAuth cashfreeAuthCall = new CashfreeAuth();
        //    string subcode = null;
        //    string PaymentMode = null;
        //    string ModuleType = null;
        //    string TransactionType = null;
        //    int payledger = 0;
        //    string message = null;
        //    string url = _config.Value.URLPrefixforProd;
        //    string returnurl = "LGC/LogiPickDrop";
        //    try
        //    {
        //        subcode = Convert.ToInt32(CashfreeEnum.Succcess).ToString();
        //        PaymentMode = EnumHelper.DescriptionAttr(CashfreeEnum.upi);
        //        if (getBeneFiciary.data.ordertype == Convert.ToInt32(OrderTypeEnum.ABB))
        //        {
        //            ModuleType = EnumHelper.DescriptionAttr(CashfreeEnum.ABB);
        //        }
        //        else
        //        {
        //            ModuleType = EnumHelper.DescriptionAttr(CashfreeEnum.Exchange);
        //        }

        //        TransactionType = EnumHelper.DescriptionAttr(CashfreeEnum.TransactionType);
        //        if (getBeneFiciary.data != null)
        //        {
        //            paymentLedger = _context.TblPaymentLeasers.FirstOrDefault(x => x.RegdNo == getBeneFiciary.data.beneId && x.ModuleType == ModuleType && x.IsActive == true && x.PaymentStatus == true);
        //            if (paymentLedger == null)
        //            {
        //                cashfreeAuthCall = _cashfreePayoutCall.CashFreeAuthCall();
        //                if (cashfreeAuthCall.subCode == subcode)
        //                {
        //                    TblPaymentLeaser paymentLedgeradd = new TblPaymentLeaser();

        //                    processtransaction.beneId = getBeneFiciary.data.beneId;
        //                    processtransaction.amount = getBeneFiciary.data.FinalExchangePrice;
        //                    processtransaction.transferMode = PaymentMode;
        //                    processtransaction.transferId = getBeneFiciary.data.beneId;
        //                    transactionResponse = _cashfreePayoutCall.Transaction(processtransaction, cashfreeAuthCall.data.token);
        //                    if (transactionResponse?.subCode == subcode)
        //                    {
        //                        paymentLedgeradd.RegdNo = getBeneFiciary.data.beneId;
        //                        paymentLedgeradd.Amount = Convert.ToDecimal(getBeneFiciary.data.FinalExchangePrice);
        //                        paymentLedgeradd.PaymentMode = PaymentMode;
        //                        paymentLedgeradd.OrderId = transactionResponse?.data?.referenceId; //getBeneFiciary.data.beneId;
        //                        paymentLedgeradd.UtcreferenceId = getbeneficiary?.data?.beneId;
        //                        paymentLedgeradd.PaymentDate = DateTime.Now;
        //                        paymentLedgeradd.ResponseDescription = transactionResponse?.message;
        //                        paymentLedgeradd.PaymentResponse = transactionResponse?.message;
        //                        paymentLedgeradd.TransactionId = transactionResponse?.data?.utr;
        //                        paymentLedgeradd.ModuleType = ModuleType;
        //                        paymentLedgeradd.IsActive = true;
        //                        paymentLedgeradd.PaymentStatus = true;
        //                        paymentLedgeradd.ResponseCode = subcode;
        //                        paymentLedgeradd.TransactionType = TransactionType;
        //                        paymentLedgeradd.CreatedBy = getBeneFiciary?.data?.ExchangeId;
        //                        paymentLedgeradd.CreatedDate = DateTime.Now;
        //                        paymentLedgeradd.ModuleReferenceId = getBeneFiciary?.data?.ExchangeId;
        //                        _paymentLeaserRepository.Create(paymentLedgeradd);
        //                        payledger = _paymentLeaserRepository.SaveChanges();

        //                        if (paymentLedgeradd.Id > 0)
        //                        {
        //                            #region update order trans for payment recieved for this order 
        //                            var updateOrderTransDC = _orderTransRepository.GetOrdertransDetails(getBeneFiciary?.data?.beneId);
        //                            if (updateOrderTransDC != null && transactionResponse?.data?.acknowledged == 1)
        //                            {
        //                                updateOrderTransDC.AmountPaidToCustomer = true;
        //                                _orderTransRepository.Update(updateOrderTransDC);
        //                                _orderTransRepository.SaveChanges();
        //                            }
        //                            #endregion
        //                            message = "Transaction for amount Rs." + getBeneFiciary?.data?.FinalExchangePrice + "/- has been completed TransactionId=" + transactionResponse?.data?.utr;
        //                            return Redirect("Details/?message=" + message + "&ReturnURL=LGC/LogiPickDrop");
        //                        }
        //                        else
        //                        {
        //                            message = "Something Went wrong";
        //                            return Redirect("DetailsForFailedTransaction/?message=" + message + "&ReturnURL=" + returnurl);
        //                        }
        //                    }
        //                    else
        //                    {
        //                        paymentLedgeradd.RegdNo = getBeneFiciary.data.beneId;
        //                        paymentLedgeradd.UtcreferenceId = getbeneficiary?.data?.beneId;
        //                        paymentLedgeradd.Amount = Convert.ToDecimal(getBeneFiciary.data.FinalExchangePrice);
        //                        paymentLedgeradd.PaymentMode = PaymentMode;
        //                        paymentLedgeradd.OrderId = transactionResponse?.data?.referenceId;//getBeneFiciary.data.beneId;
        //                        paymentLedgeradd.PaymentDate = DateTime.Now;
        //                        paymentLedgeradd.ResponseDescription = transactionResponse?.message;
        //                        paymentLedgeradd.PaymentResponse = transactionResponse?.message;
        //                        paymentLedgeradd.TransactionId = transactionResponse?.data?.utr;
        //                        paymentLedgeradd.ModuleType = ModuleType;
        //                        paymentLedgeradd.IsActive = true;
        //                        paymentLedgeradd.PaymentStatus = false;
        //                        paymentLedgeradd.ResponseCode = transactionResponse?.subCode;
        //                        paymentLedgeradd.TransactionType = TransactionType;
        //                        paymentLedgeradd.CreatedBy = getBeneFiciary?.data?.ExchangeId;
        //                        paymentLedgeradd.CreatedDate = DateTime.Now;
        //                        paymentLedgeradd.ModuleReferenceId = getBeneFiciary?.data?.ExchangeId;

        //                        message = transactionResponse?.message;
        //                        _paymentLeaserRepository.Create(paymentLedgeradd);
        //                        payledger = _paymentLeaserRepository.SaveChanges();

        //                        return Redirect("DetailsForFailedTransaction/?message=" + message + "&ReturnURL=" + returnurl);
        //                    }
        //                }
        //                else
        //                {
        //                    message = transactionResponse?.message;
        //                    return Redirect("DetailsForFailedTransaction/?message=" + message + "&ReturnURL=" + returnurl);
        //                }
        //            }
        //            else
        //            {
        //                message = "Amount= " + paymentLedger.Amount + "/- already paid transaction id=" + paymentLedger.TransactionId;
        //                return Redirect("Details/?message=" + message + "&ReturnURL=" + returnurl);
        //            }
        //        }
        //        else
        //        {
        //            return Page();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        _logging.WriteErrorToDB("PayOutConfirmationModel", "OnPostPayOut", ex);
        //    }
        //    return Page();
        //}
    }
}
