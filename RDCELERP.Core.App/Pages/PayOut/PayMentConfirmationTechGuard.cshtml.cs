using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RDCELERP.BAL.Interface;
using RDCELERP.Common.Enums;
using RDCELERP.Common.Helper;
using RDCELERP.Core.App.Pages.Base;
using RDCELERP.DAL.Entities;
using RDCELERP.DAL.IRepository;
using RDCELERP.Model.Base;
using RDCELERP.Model.CashfreeModel;
using RDCELERP.Model.TechGuard;

namespace RDCELERP.Core.App.Pages.PayOut
{
    public class PayMentConfirmationTechGuardModel : BasePageModel
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
        IOrderTransRepository _transRepository;
        IABBRedemptionRepository _redemptionRepository;
        IPaymentLeaser _paymentLegder;
        public ILogging _logging;
        ITechGuard _techGuard;
        #endregion
        public PayMentConfirmationTechGuardModel(Digi2l_DevContext context, IOptions<ApplicationSettings> config, CustomDataProtection protector, IExchangeOrderManager exchangeOrderManager, IDealerManager dashBoardManager, IExchangeOrderRepository exchangeOrderRepository, IBusinessPartnerRepository businessPartnerRepository, ICashfreePayoutCall cashfreePayoutCall, IPaymentLeaser paymentLeaserRepository, ILogging logging, ITechGuard techGuard, IOrderTransRepository transRepository, IABBRedemptionRepository redemptionRepository, IPaymentLeaser paymentLegder) : base(config)
        {
            _context = context;
            _config = config;
            _protector = protector;
            _exchangeOrderManager = exchangeOrderManager;
            _dashBoardManager = dashBoardManager;
            _exchangeOrderRepository = exchangeOrderRepository;
            _businessPartnerRepository = businessPartnerRepository;
            _cashfreePayoutCall = cashfreePayoutCall;
            _paymentLeaserRepository = paymentLeaserRepository;
            _logging = logging;
            _techGuard = techGuard;
            _transRepository = transRepository;
            _redemptionRepository = redemptionRepository;
            _paymentLegder = paymentLegder;
        }

        #region Model declaration
        [BindProperty(SupportsGet = true)]
        public TechGuardRequestModel requestPaymentToTechGuard { get; set; }
        [BindProperty(SupportsGet = true)]
        public ProcessPaymentTechGuard processPayment { get; set; }
        [BindProperty(SupportsGet = true)]
        public TblOrderTran orderTransObj { get; set; }
        [BindProperty(SupportsGet = true)]
        public TblAbbredemption redemptionObj { get; set; }
        [BindProperty(SupportsGet = true)]
        public TechGuardResponse techGuardResponse { get; set; }

        [BindProperty(SupportsGet = true)]
        public TblPaymentLeaser paymentledgerObj { get; set; }
        #endregion
        #region get page data
        public IActionResult OnGet(string RegdNo)
        {
           
            string message = null;
            string returnurl = "LGC/LogiPickDrop";
            try
            {
                if (!string.IsNullOrEmpty(RegdNo))
                {
                    try
                    {
                        RegdNo = SecurityHelper.DecryptString(RegdNo, _config.Value.SecurityKey);
                    }
                    catch(Exception ex)
                    {
                        _logging.WriteErrorToDB("PayMentConfirmationTechGuardModel", "OnGet", ex);
                        message = "Registration number is not proper";
                        return Redirect("DetailsForFailedTransaction/?message=" + message + "&ReturnURL=" + returnurl);
                    }
                    //check if payment is already done or not in tblpaymentladger
                    paymentledgerObj = _paymentLegder.GetPaymentdetails(RegdNo);
                    if (paymentledgerObj == null)
                    {
                        orderTransObj = _transRepository.GetTransactionDetailsForABBRedemption(RegdNo);
                        if (orderTransObj != null)
                        {
                            if (orderTransObj.OrderType == Convert.ToInt32(OrderTypeEnum.ABB))
                            {
                                if (orderTransObj.AbbredemptionId > 0)
                                {
                                    if (string.IsNullOrEmpty(orderTransObj?.Abbredemption?.ReferenceId))
                                    {
                                        message = "ReferenceId Id not available";
                                        return Redirect("DetailsForFailedTransaction/?message=" + message + "&ReturnURL=" + returnurl);
                                    }
                                    processPayment.Address = orderTransObj.Abbredemption.Abbregistration.CustAddress1 + " " + orderTransObj.Abbredemption.Abbregistration.CustAddress2;
                                    processPayment.CustomerName = orderTransObj.Abbredemption.Abbregistration.CustFirstName + " " + orderTransObj.Abbredemption.Abbregistration.CustLastName;
                                    processPayment.PhoneNumber = orderTransObj.Abbredemption.Abbregistration.CustMobile;
                                    processPayment.Email = orderTransObj.Abbredemption.Abbregistration.CustEmail;
                                    processPayment.FinalPrice = orderTransObj.FinalPriceAfterQc.ToString();
                                    processPayment.RepairId = orderTransObj.Abbredemption.ReferenceId;
                                    processPayment.RegdNo = RegdNo;
                                    processPayment.RedemptoinId = orderTransObj.Abbredemption.RedemptionId;
                                }
                                else
                                {
                                    message = "Redemption Id not available";
                                    return Redirect("DetailsForFailedTransaction/?message=" + message + "&ReturnURL=" + returnurl);
                                }

                            }
                            else
                            {
                                message = "Order type is exchange";
                                return Redirect("DetailsForFailedTransaction/?message=" + message + "&ReturnURL=" + returnurl);
                            }
                        }
                        else
                        {
                            message = "No data found in order trans";
                            return Redirect("DetailsForFailedTransaction/?message=" + message + "&ReturnURL=" + returnurl);
                        }
                    }
                    else
                    {
                        message = "Unable to process without registration number";
                        return Redirect("DetailsForFailedTransaction/?message=" + message + "&ReturnURL=" + returnurl);
                    }
                }
                else
                {
                    message = "Payment is already done for this order";
                    return Redirect("DetailsForFailedTransaction/?message=" + message + "&ReturnURL=" + returnurl);
                }
               
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("PayMentConfirmationTechGuardModel", "OnGet", ex);
            }
            return Page();
        }
        #endregion

        public IActionResult OnPostPayOutTC(ProcessPaymentTechGuard processPayment)
        {
            TransactionResponseCashfree transactionResponse = new TransactionResponseCashfree();
            ProcessTransactionCashfree processtransaction = new ProcessTransactionCashfree();
            CashfreeAuth cashfreeAuthCall = new CashfreeAuth();
            TblOrderTran updateOrderTransDC = new TblOrderTran();
            TransactionCashFree transactioncashfree = new TransactionCashFree();
            
            string message = null;
            string subcode = null;
            string PaymentMode = null;
            string ModuleType = null;
            string TransactionType = null;
            int payledger = 0;
            string url = _config.Value.URLPrefixforProd;
            string returnurl = "LGC/LogiPickDrop";
            string paymentstatus = EnumHelper.DescriptionAttr(TechGuardEnum.Succcess);
            subcode = Convert.ToInt32(CashfreeEnum.Succcess).ToString();
            
            ModuleType = EnumHelper.DescriptionAttr(CashfreeEnum.ABB);
            TransactionType = EnumHelper.DescriptionAttr(CashfreeEnum.TransactionType);
            string cstomersupportnumber = null;
            string customersupportemail = null;

            int UserId = _loginSession.UserViewModel.UserId;
            try
            {
                //check if payment is already done or not in tblpaymentladger
                paymentledgerObj = _paymentLegder.GetPaymentdetails(processPayment.RegdNo);
                if (paymentledgerObj != null)
                {
                    message = "Payment is already done for this order";
                    return Redirect("DetailsForFailedTransaction/?message=" + message + "&ReturnURL=" + returnurl);
                }

                PaymentMode = _config.Value.TechGuardsettelemntType;
                if (!string.IsNullOrEmpty(processPayment.FinalPrice))
                {
                    decimal amount = Convert.ToDecimal(processPayment.FinalPrice);
                    requestPaymentToTechGuard.amount = Convert.ToInt32(amount) * 100;
                }

                requestPaymentToTechGuard.company_id = _config.Value.CompanyIdTechGuard;
                requestPaymentToTechGuard.secret_key = _config.Value.TechGuardAPIKey;
                requestPaymentToTechGuard.repair_id = processPayment.RepairId;
                customersupportemail = _config.Value.TechGuardSupportEmail;
                cstomersupportnumber = _config.Value.TechGuardsupportPhone;
                techGuardResponse = _techGuard.PaymentResponse(requestPaymentToTechGuard, UserId);

                //Log the response from the API...

                var jsonString = JsonConvert.SerializeObject(techGuardResponse);
                _logging.WriteAPIRequestToDB("PayMentConfirmationTechGuardModel", "OnPostPayOutTC", processPayment.RegdNo, jsonString);

                if (techGuardResponse != null)
                {
                    if (techGuardResponse.status == true && !string.IsNullOrEmpty(techGuardResponse.referance_id))
                    {
                        cashfreeAuthCall = _cashfreePayoutCall.CashFreeAuthCall();
                        if (cashfreeAuthCall.subCode == subcode)
                        {
                            processtransaction.transferId = processPayment.RegdNo;
                            processtransaction.transferMode = PaymentMode;
                            processtransaction.amount = processPayment.FinalPrice;
                            processtransaction.beneId = _config.Value.TechGuardBeneficiary;
                            transactionResponse = _cashfreePayoutCall.Transaction(processtransaction, cashfreeAuthCall.data.token);
                            if (transactionResponse?.subCode == subcode)
                            {
                                TblPaymentLeaser paymentLedgeradd = new TblPaymentLeaser();

                                paymentLedgeradd.RegdNo = processPayment.RegdNo;
                                paymentLedgeradd.UtcreferenceId = processPayment.RegdNo;
                                paymentLedgeradd.Amount = Convert.ToDecimal(processPayment.FinalPrice);
                                paymentLedgeradd.PaymentMode = PaymentMode;
                                paymentLedgeradd.OrderId = transactionResponse?.data?.referenceId;
                                paymentLedgeradd.PaymentDate = DateTime.Now;
                                paymentLedgeradd.ResponseDescription = transactionResponse?.message;
                                paymentLedgeradd.PaymentResponse = transactionResponse?.message;//status
                                paymentLedgeradd.TransactionId = transactionResponse?.data?.utr;
                                paymentLedgeradd.ModuleType = ModuleType;
                                paymentLedgeradd.IsActive = true;
                                paymentLedgeradd.PaymentStatus = true;
                                paymentLedgeradd.ResponseCode = subcode;
                                paymentLedgeradd.TransactionType = TransactionType;
                                paymentLedgeradd.CreatedBy = UserId;
                                paymentLedgeradd.CreatedDate = DateTime.Now;
                                paymentLedgeradd.ModuleReferenceId = processPayment.RedemptoinId;
                                paymentLedgeradd.GatewayTransactioId = techGuardResponse.referance_id;
                                _paymentLeaserRepository.Create(paymentLedgeradd);
                                payledger = _paymentLeaserRepository.SaveChanges();
                                if (paymentLedgeradd.Id > 0)
                                {
                                    #region update order trans for payment recieved for this order 
                                    updateOrderTransDC = _transRepository.GetOrdertransDetails(processPayment.RegdNo);
                                    if (updateOrderTransDC != null)
                                    {
                                        updateOrderTransDC.AmountPaidToCustomer = true;
                                        _transRepository.Update(updateOrderTransDC);
                                        _transRepository.SaveChanges();
                                    }
                                    #endregion
                                    message = "Transaction for amount Rs." + processPayment.FinalPrice + "/- has been completed TransactionId=" + transactionResponse?.data?.utr;
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
                                DateTime startTime = DateTime.Now;
                                DateTime endTime = startTime.AddSeconds(60); // Run loop for 60 seconds
                                TblPaymentLeaser paymentLedgeradd = new TblPaymentLeaser();

                                transactioncashfree = _cashfreePayoutCall.GetPaymentTransferStatus(processPayment.RegdNo, cashfreeAuthCall.data.token);

                                if (transactioncashfree.data.transfer.status != "SUCCESS")
                                {
                                    while (startTime < endTime)
                                    {
                                        transactioncashfree = _cashfreePayoutCall.GetPaymentTransferStatus(processPayment.RegdNo, cashfreeAuthCall.data.token);
                                        if (transactioncashfree?.data?.transfer?.status == "SUCCESS")
                                        {
                                            break;
                                        }
                                    }
                                }
                                paymentLedgeradd.RegdNo = processPayment.RegdNo;
                                paymentLedgeradd.UtcreferenceId = processPayment.RegdNo;
                                paymentLedgeradd.Amount = Convert.ToDecimal(processPayment.FinalPrice);
                                paymentLedgeradd.PaymentMode = PaymentMode;
                                paymentLedgeradd.OrderId = transactionResponse?.data?.referenceId;
                                paymentLedgeradd.PaymentDate = DateTime.Now;
                                paymentLedgeradd.ResponseDescription = transactioncashfree?.message;
                                paymentLedgeradd.PaymentResponse = transactioncashfree?.data?.transfer?.status;//status
                                paymentLedgeradd.TransactionId = transactioncashfree?.data?.transfer?.utr;
                                paymentLedgeradd.ModuleType = ModuleType;
                                paymentLedgeradd.IsActive = true;
                                paymentLedgeradd.PaymentStatus = true;
                                paymentLedgeradd.ResponseCode = subcode;

                                if (transactioncashfree?.data?.transfer?.status != "SUCCESS")
                                {
                                    paymentLedgeradd.PaymentStatus = false;
                                    paymentLedgeradd.ResponseCode = transactioncashfree?.subCode;
                                }

                                paymentLedgeradd.TransactionType = TransactionType;
                                paymentLedgeradd.CreatedBy = UserId;
                                paymentLedgeradd.CreatedDate = DateTime.Now;
                                paymentLedgeradd.ModuleReferenceId = processPayment.RedemptoinId;
                                paymentLedgeradd.GatewayTransactioId = techGuardResponse.referance_id;

                                _paymentLeaserRepository.Create(paymentLedgeradd);
                                payledger = _paymentLeaserRepository.SaveChanges();

                                if (paymentLedgeradd.Id > 0)
                                {
                                    #region update order trans for payment recieved for this order 
                                    updateOrderTransDC = _transRepository.GetOrdertransDetails(processPayment.RegdNo);
                                    if (updateOrderTransDC != null && transactioncashfree?.data?.transfer?.status == "SUCCESS")
                                    {
                                        updateOrderTransDC.AmountPaidToCustomer = true;
                                        _transRepository.Update(updateOrderTransDC);
                                        _transRepository.SaveChanges();
                                    }
                                    #endregion
                                    message = "Transaction for amount Rs." + processPayment.FinalPrice + "/- has been completed TransactionId=" + transactionResponse?.data?.utr;
                                    return Redirect("Details/?message=" + message + "&ReturnURL=LGC/LogiPickDrop");
                                }
                                else
                                {
                                    message = "Something Went wrong";
                                    return Redirect("DetailsForFailedTransaction/?message=" + message + "&ReturnURL=" + returnurl);
                                }
                            }
                        }
                        else
                        {
                            message = cashfreeAuthCall?.message;
                            return Redirect("DetailsForFailedTransaction/?message=" + message + "&ReturnURL=" + returnurl);
                        }
                    }
                    else
                    {
                        message = "Amount not paid to customer please try contacting to number "+ cstomersupportnumber + " or email "+ customersupportemail;
                        return Redirect("DetailsForFailedTransaction/?message=" + message + "&ReturnURL=" + returnurl);
                    }
                }
                else
                {
                    message = "No data found in order trans";
                    return Redirect("DetailsForFailedTransaction/?message=" + message + "&ReturnURL=" + returnurl);
                }

            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("PayMentConfirmationTechGuardModel", "OnPostPayOutTC", ex);
            }
            return Page();
        }
    }
}
