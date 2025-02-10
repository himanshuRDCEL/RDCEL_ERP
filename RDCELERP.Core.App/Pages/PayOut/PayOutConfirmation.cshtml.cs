using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;
using RDCELERP.BAL.Interface;
using RDCELERP.Common.Enums;
using RDCELERP.Common.Helper;
using RDCELERP.Core.App.Pages.Base;
using RDCELERP.DAL.Entities;
using RDCELERP.DAL.IRepository;
using RDCELERP.Model.Base;
using RDCELERP.Model.CashfreeModel;

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
        #endregion

        #region Constructor
        public PayOutConfirmationModel(IOptions<ApplicationSettings> config, Digi2l_DevContext _dbcontext, CustomDataProtection _dataprotector, IExchangeOrderManager exchangeOrderManager, IDealerManager dealerDashBoardManager, ILogging logging, IBusinessPartnerRepository businessPartnerRepository, IExchangeOrderRepository exchangeOrderRepository, ICashfreePayoutCall cashfreecall, IPaymentLeaser paymentLeaser, IOrderTransRepository orderTransRepository, IABBRedemptionRepository redemptionRepository) : base(config)
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
        public TblExchangeOrder ExchangeObj { get; set; }

        [BindProperty(SupportsGet = true)]
        public TblOrderTran OrderTransObj { get; set; }
        [BindProperty(SupportsGet = true)]
        public TblAbbredemption redemptionObj { get; set; }
        #endregion

        public IActionResult OnGet(string RegdNo)
        {
            GetBeneficiary getBeneficiarry = new GetBeneficiary();
            CashfreeAuth cashfreeAuthCall = new CashfreeAuth();
            string subcode = null;
            string message = null;
            string returnurl = "LGC/LogiPickDrop";
            try
            {
                RegdNo = SecurityHelper.DecryptString(RegdNo, _config.Value.SecurityKey);


                subcode = Convert.ToInt32(CashfreeEnum.Succcess).ToString();
                if (RegdNo != null)
                {
                    OrderTransObj = _orderTransRepository.GetOrdertransDetails(RegdNo);
                    if (OrderTransObj != null)
                    {
                        if (OrderTransObj.OrderType == Convert.ToInt32(OrderTypeEnum.Exchange))
                        {

                            ExchangeObj = _context.TblExchangeOrders.FirstOrDefault(x => x.RegdNo == RegdNo);
                            if (ExchangeObj != null)
                            {
                                cashfreeAuthCall = _cashfreePayoutCall.CashFreeAuthCall();
                                if (cashfreeAuthCall.subCode == subcode)
                                {
                                    getbeneficiary = _cashfreePayoutCall.GetBeneficiary(cashfreeAuthCall.data.token, RegdNo);
                                    if (getbeneficiary.subCode == subcode)
                                    {

                                        getbeneficiary.data.FinalExchangePrice = ExchangeObj.FinalExchangePrice.ToString();
                                        getbeneficiary.data.ExchangeId = ExchangeObj.Id;
                                        getbeneficiary.data.ordertype = Convert.ToInt32(OrderTransObj.OrderType);
                                    }
                                    else
                                    {
                                        message = getbeneficiary.message;
                                        return Redirect("DetailsForFailedTransaction/?message=" + message + "&ReturnURL=" + returnurl);
                                    }
                                }
                                else
                                {
                                    message = cashfreeAuthCall.message;
                                    return Redirect("DetailsForFailedTransaction/?message=" + message + "&ReturnURL=" + returnurl);
                                }
                            }
                            else
                            {
                                message = "Order data not found";
                                return Redirect("DetailsForFailedTransaction/?message=" + message + "&ReturnURL=" + returnurl);
                            }
                        }
                        else if (OrderTransObj.OrderType == Convert.ToInt32(OrderTypeEnum.ABB))
                        {

                            redemptionObj = _redemptionRepository.GetOrderDetails(RegdNo, OrderTransObj.AbbredemptionId);
                            if (redemptionObj != null)
                            {
                                cashfreeAuthCall = _cashfreePayoutCall.CashFreeAuthCall();
                                if (cashfreeAuthCall.subCode == subcode)
                                {
                                    getbeneficiary = _cashfreePayoutCall.GetBeneficiary(cashfreeAuthCall.data.token, RegdNo);
                                    if (getbeneficiary.subCode == subcode)
                                    {
                                        getbeneficiary.data.FinalExchangePrice = OrderTransObj.FinalPriceAfterQc.ToString();
                                        getbeneficiary.data.ExchangeId = redemptionObj.RedemptionId;
                                        getbeneficiary.data.ordertype = Convert.ToInt32(OrderTransObj.OrderType);
                                    }
                                    else
                                    {
                                        message = getbeneficiary.message;
                                        return Redirect("DetailsForFailedTransaction/?message=" + message + "&ReturnURL=" + returnurl);
                                    }
                                }
                                else
                                {
                                    message = cashfreeAuthCall.message;
                                    return Redirect("DetailsForFailedTransaction/?message=" + message + "&ReturnURL=" + returnurl);
                                }
                            }
                            else
                            {
                                message = "Order data not found";
                                return Redirect("DetailsForFailedTransaction/?message=" + message + "&ReturnURL=" + returnurl);
                            }
                        }
                        else
                        {
                            message = "Order type not defined";
                            return Redirect("DetailsForFailedTransaction/?message=" + message + "&ReturnURL=" + returnurl);
                        }
                    }
                    else
                    {
                        message = "Order data not found";
                        return Redirect("DetailsForFailedTransaction/?message=" + message + "&ReturnURL=" + returnurl);
                    }


                }
                else
                {
                    message = "Please provide order number";
                    return Redirect("Details/?message=" + message);
                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("PayOutConfirmationModel", "OnGet", ex);
            }
            return Page();
        }

        public IActionResult OnPostPayOut(GetBeneficiary getBeneFiciary)
        {
            TblPaymentLeaser paymentLedger = null;
            TransactionResponseCashfree transactionResponse = new TransactionResponseCashfree();
            ProcessTransactionCashfree processtransaction = new ProcessTransactionCashfree();
            CashfreeAuth cashfreeAuthCall = new CashfreeAuth();
            string subcode = null;
            string PaymentMode = null;
            string ModuleType = null;
            string TransactionType = null;
            int payledger = 0;
            string message = null;
            string url = _config.Value.URLPrefixforProd;
            string returnurl = "LGC/LogiPickDrop";
            try
            {
                subcode = Convert.ToInt32(CashfreeEnum.Succcess).ToString();
                PaymentMode = EnumHelper.DescriptionAttr(CashfreeEnum.upi);
                if (getBeneFiciary.data.ordertype == Convert.ToInt32(OrderTypeEnum.ABB))
                {
                    ModuleType = EnumHelper.DescriptionAttr(CashfreeEnum.ABB);
                }
                else
                {
                    ModuleType = EnumHelper.DescriptionAttr(CashfreeEnum.Exchange);
                }

                TransactionType = EnumHelper.DescriptionAttr(CashfreeEnum.TransactionType);
                if (getBeneFiciary.data != null)
                {
                    paymentLedger = _context.TblPaymentLeasers.FirstOrDefault(x => x.RegdNo == getBeneFiciary.data.beneId && x.ModuleType == ModuleType && x.IsActive == true && x.PaymentStatus == true);
                    if (paymentLedger == null)
                    {
                        cashfreeAuthCall = _cashfreePayoutCall.CashFreeAuthCall();
                        if (cashfreeAuthCall.subCode == subcode)
                        {
                            TblPaymentLeaser paymentLedgeradd = new TblPaymentLeaser();

                            processtransaction.beneId = getBeneFiciary.data.beneId;
                            processtransaction.amount = getBeneFiciary.data.FinalExchangePrice;
                            processtransaction.transferMode = PaymentMode;
                            processtransaction.transferId = getBeneFiciary.data.beneId;
                            transactionResponse = _cashfreePayoutCall.Transaction(processtransaction, cashfreeAuthCall.data.token);
                            if (transactionResponse?.subCode == subcode)
                            {
                                paymentLedgeradd.RegdNo = getBeneFiciary.data.beneId;
                                paymentLedgeradd.Amount = Convert.ToDecimal(getBeneFiciary.data.FinalExchangePrice);
                                paymentLedgeradd.PaymentMode = PaymentMode;
                                paymentLedgeradd.OrderId = transactionResponse?.data?.referenceId; //getBeneFiciary.data.beneId;
                                paymentLedgeradd.UtcreferenceId = getbeneficiary?.data?.beneId;
                                paymentLedgeradd.PaymentDate = DateTime.Now;
                                paymentLedgeradd.ResponseDescription = transactionResponse?.message;
                                paymentLedgeradd.PaymentResponse = transactionResponse?.message;
                                paymentLedgeradd.TransactionId = transactionResponse?.data?.utr;
                                paymentLedgeradd.ModuleType = ModuleType;
                                paymentLedgeradd.IsActive = true;
                                paymentLedgeradd.PaymentStatus = true;
                                paymentLedgeradd.ResponseCode = subcode;
                                paymentLedgeradd.TransactionType = TransactionType;
                                paymentLedgeradd.CreatedBy = getBeneFiciary?.data?.ExchangeId;
                                paymentLedgeradd.CreatedDate = DateTime.Now;
                                paymentLedgeradd.ModuleReferenceId = getBeneFiciary?.data?.ExchangeId;
                                _paymentLeaserRepository.Create(paymentLedgeradd);
                                payledger = _paymentLeaserRepository.SaveChanges();

                                if (paymentLedgeradd.Id > 0)
                                {
                                    #region update order trans for payment recieved for this order 
                                    var updateOrderTransDC = _orderTransRepository.GetOrdertransDetails(getBeneFiciary?.data?.beneId);
                                    if (updateOrderTransDC != null && transactionResponse?.data?.acknowledged == 1)
                                    {
                                        updateOrderTransDC.AmountPaidToCustomer = true;
                                        _orderTransRepository.Update(updateOrderTransDC);
                                        _orderTransRepository.SaveChanges();
                                    }
                                    #endregion
                                    message = "Transaction for amount Rs." + getBeneFiciary?.data?.FinalExchangePrice + "/- has been completed TransactionId=" + transactionResponse?.data?.utr;
                                    return Redirect("Details/?message=" + message + "&ReturnURL=LGC/LogiPickDrop");
                                }
                                else
                                {
                                    message = "Something Went wrong";
                                    return Redirect("DetailsForFailedTransaction/?message=" + message + "&ReturnURL=" + returnurl);
                                }
                            }
                            else
                            {
                                paymentLedgeradd.RegdNo = getBeneFiciary.data.beneId;
                                paymentLedgeradd.UtcreferenceId = getbeneficiary?.data?.beneId;
                                paymentLedgeradd.Amount = Convert.ToDecimal(getBeneFiciary.data.FinalExchangePrice);
                                paymentLedgeradd.PaymentMode = PaymentMode;
                                paymentLedgeradd.OrderId = transactionResponse?.data?.referenceId;//getBeneFiciary.data.beneId;
                                paymentLedgeradd.PaymentDate = DateTime.Now;
                                paymentLedgeradd.ResponseDescription = transactionResponse?.message;
                                paymentLedgeradd.PaymentResponse = transactionResponse?.message;
                                paymentLedgeradd.TransactionId = transactionResponse?.data?.utr;
                                paymentLedgeradd.ModuleType = ModuleType;
                                paymentLedgeradd.IsActive = true;
                                paymentLedgeradd.PaymentStatus = false;
                                paymentLedgeradd.ResponseCode = transactionResponse?.subCode;
                                paymentLedgeradd.TransactionType = TransactionType;
                                paymentLedgeradd.CreatedBy = getBeneFiciary?.data?.ExchangeId;
                                paymentLedgeradd.CreatedDate = DateTime.Now;
                                paymentLedgeradd.ModuleReferenceId = getBeneFiciary?.data?.ExchangeId;

                                message = transactionResponse?.message;
                                _paymentLeaserRepository.Create(paymentLedgeradd);
                                payledger = _paymentLeaserRepository.SaveChanges();

                                return Redirect("DetailsForFailedTransaction/?message=" + message + "&ReturnURL=" + returnurl);
                            }
                        }
                        else
                        {
                            message = transactionResponse?.message;
                            return Redirect("DetailsForFailedTransaction/?message=" + message + "&ReturnURL=" + returnurl);
                        }
                    }
                    else
                    {
                        message = "Amount= " + paymentLedger.Amount + "/- already paid transaction id=" + paymentLedger.TransactionId;
                        return Redirect("Details/?message=" + message + "&ReturnURL=" + returnurl);
                    }
                }
                else
                {
                    return Page();
                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("PayOutConfirmationModel", "OnPostPayOut", ex);
            }
            return Page();
        }
    }
}
