using DocumentFormat.OpenXml.Vml.Spreadsheet;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.Web.CodeGeneration;
using NPOI.OpenXmlFormats.Dml.Diagram;
using System.Drawing;
using System.Drawing.Text;
using RDCELERP.BAL.Interface;
using RDCELERP.Common.Enums;
using RDCELERP.Common.Helper;
using RDCELERP.Core.App.Pages.Base;
using RDCELERP.DAL.Entities;
using RDCELERP.DAL.IRepository;
using RDCELERP.Model.Base;
using RDCELERP.Model.CashfreeModel;
using RDCELERP.Model.Paymant;
using RDCELERP.Model.TechGuard;

namespace RDCELERP.Core.App.Pages.PayOut
{
    public class PaymentStatusModel : BasePageModel
    {
        #region Constants

        private const string PaymentMode = "upi";
        private const string Success = "SUCCESS";
        private const string Error = "ERROR";
        private const string Failed = "FAILED";
        private const string Pending = "PENDING";
        private const string Reversed = "REVERSED";

        #endregion endregion Constants

        private readonly IOptions<ApplicationSettings> _config;
        private readonly Digi2l_DevContext _context;
        private ILogging _logging;
        private readonly ICashfreePayoutCall _cashfreePayoutCall;
        IPaymentLeaser _paymentLeaserRepository;
        IOrderTransRepository _transRepository;

        public PaymentStatusModel(IOptions<ApplicationSettings> config, ILogging logging, Digi2l_DevContext _dbcontext, ICashfreePayoutCall cashfreePayoutCall, IPaymentLeaser paymentLeaserRepository, IOrderTransRepository transRepository) : base(config)
        {
            _config = config;
            _context = _dbcontext;
            _cashfreePayoutCall = cashfreePayoutCall;
            _paymentLeaserRepository = paymentLeaserRepository;
            _transRepository = transRepository;
        }

        [BindProperty(SupportsGet = true)]
        public CashFreeStatusViewModel PaymentDetails { get; set; }

        string subcode = Convert.ToInt32(CashfreeEnum.Succcess).ToString();
        TransactionCashFree? cashFreeStatusResponseModel = new TransactionCashFree();
        CashfreeAuth cashfreeAuthCall = new CashfreeAuth();
        string returnUrl = "PayOut/RePaymentRecords";

        public IActionResult OnGet(string RegdNo, string UtcReferenceId)
        {
            //Call the Cashfree API and bind the response

            cashfreeAuthCall = _cashfreePayoutCall.CashFreeAuthCall();
            if (cashfreeAuthCall != null)
            {
                cashFreeStatusResponseModel = _cashfreePayoutCall.GetPaymentTransferStatus(UtcReferenceId, cashfreeAuthCall?.data?.token);
                // Handle api call failed case
            }
            if (cashFreeStatusResponseModel?.subCode == subcode)
            {
                // Save status in to the database...

                if (cashFreeStatusResponseModel?.data?.transfer?.status == Success)
                {
                    PaymentDetails.StatusDescription = "Transfer completed successfully";
                }
                if (cashFreeStatusResponseModel?.data?.transfer?.status == Error)
                {
                    PaymentDetails.StatusDescription = "There was an error while requesting the transfer";
                }
                if (cashFreeStatusResponseModel?.data?.transfer?.status == Failed)
                {
                    PaymentDetails.StatusDescription = "The transfer has failed.";
                }
                if (cashFreeStatusResponseModel?.data?.transfer?.status == Pending)
                {
                    PaymentDetails.StatusDescription = "The transfer is pending.";
                }
                if (cashFreeStatusResponseModel?.data?.transfer?.status == Reversed)
                {
                    PaymentDetails.StatusDescription = "Transfer rejected by the beneficiary bank. The payout balance gets credited back";
                }

                PaymentDetails.Amount = cashFreeStatusResponseModel?.data?.transfer?.amount;
                PaymentDetails.BeneficiaryId = cashFreeStatusResponseModel?.data?.transfer?.beneId;
                PaymentDetails.Status = cashFreeStatusResponseModel?.data?.transfer?.status;
                PaymentDetails.ReferenceId = cashFreeStatusResponseModel?.data?.transfer?.referenceId;
                PaymentDetails.TransferMethod = cashFreeStatusResponseModel?.data?.transfer?.transferMode;
                PaymentDetails.UTRNumber = cashFreeStatusResponseModel?.data?.transfer?.utr;
                PaymentDetails.Vpa = cashFreeStatusResponseModel?.data?.transfer?.vpa;
                PaymentDetails.InitiatedAt = cashFreeStatusResponseModel?.data?.transfer?.addedOn;
                PaymentDetails.phone = cashFreeStatusResponseModel?.data?.transfer?.phone;
                PaymentDetails.TransferAcknowledge = cashFreeStatusResponseModel?.data?.transfer?.acknowledged;
                PaymentDetails.ProcessedOn = cashFreeStatusResponseModel?.data?.transfer?.processedOn;

                // Call the method to Update the transaction status in the database.
                var payment = _context.TblPaymentLeasers.Where(x => x.RegdNo == RegdNo && x.UtcreferenceId == UtcReferenceId).FirstOrDefault();

                bool PaymentStatus = false;
                if (cashFreeStatusResponseModel?.data?.transfer?.status == Success)
                {
                    PaymentStatus = true;
                }
                payment.PaymentStatus = PaymentStatus;
                payment.PaymentResponse = PaymentDetails?.StatusDescription;
                _paymentLeaserRepository.Update(payment);
                _paymentLeaserRepository.SaveChanges();

                //update ordertrans as well
                var OrderTransactionData = _transRepository.GetOrdertransDetails(RegdNo);

                if (OrderTransactionData != null)
                {
                    if (PaymentStatus == true)
                    {
                        OrderTransactionData.AmountPaidToCustomer = true;
                        _transRepository.Update(OrderTransactionData);
                        _transRepository.SaveChanges();
                    }
                }
            }
            else
            {
                return Redirect(_config.Value.BaseURL + "PayOut/DetailsForFailedTransaction/?message= " + cashFreeStatusResponseModel.message + "&ReturnURL=" + returnUrl);
            }
            return Page();
        }

        public IActionResult OnPostPayment()
        {
            try
            {
                TransactionResponseCashfree transactionResponseCashfree = new TransactionResponseCashfree();
                string RegdNo = PaymentDetails?.BeneficiaryId;
                int userId = _loginSession.UserViewModel.UserId;
                string moduleType = string.Empty;
                int? moduleReferenceId = null;
                string message = string.Empty;
                // to check if already paid...

                var Payments = _context.TblPaymentLeasers.Where(x => x.RegdNo == RegdNo).ToList();

                if (Payments != null && Payments.Count > 0)
                {
                    List<string> AllPaymentStatusForRegdNo = new List<string>();

                    cashfreeAuthCall = _cashfreePayoutCall.CashFreeAuthCall();

                    foreach (var eachRecord in Payments)
                    {
                        if (cashfreeAuthCall != null)
                        {
                            cashFreeStatusResponseModel = _cashfreePayoutCall.GetPaymentTransferStatus(eachRecord?.UtcreferenceId, cashfreeAuthCall?.data?.token);
                        }
                        else
                        {
                            message = cashfreeAuthCall.message;
                            return Redirect(_config.Value.BaseURL + "PayOut/DetailsForFailedTransaction/?message= " + message + "&ReturnURL=" + returnUrl);
                        }
                        if (cashFreeStatusResponseModel?.subCode == subcode)
                        {
                            bool PaymentStatus = false;
                            if (cashFreeStatusResponseModel?.data?.transfer?.status == Success)
                            {
                                PaymentStatus = true;
                            }
                            eachRecord.PaymentStatus = PaymentStatus;
                            eachRecord.PaymentResponse = PaymentDetails?.StatusDescription;
                            _paymentLeaserRepository.Update(eachRecord);
                            _paymentLeaserRepository.SaveChanges();

                            //update ordertrans as well
                            var OrderTransactionData = _transRepository.GetOrdertransDetails(eachRecord?.RegdNo);

                            if (OrderTransactionData != null)
                            {
                                if (PaymentStatus == true)
                                {
                                    OrderTransactionData.AmountPaidToCustomer = true;
                                    _transRepository.Update(OrderTransactionData);
                                    _transRepository.SaveChanges();
                                }
                            }
                        }
                        else
                        {
                            message = cashFreeStatusResponseModel?.message;
                            return Redirect(_config.Value.BaseURL + "PayOut/DetailsForFailedTransaction/?message= " + message + "&ReturnURL=" + returnUrl);
                        }
                        if (cashFreeStatusResponseModel?.data?.transfer?.status != null)
                        {
                            AllPaymentStatusForRegdNo.Add(cashFreeStatusResponseModel?.data?.transfer?.status);
                        }
                    }
                    if (AllPaymentStatusForRegdNo != null && AllPaymentStatusForRegdNo.Count > 0)
                    {
                        var result = AllPaymentStatusForRegdNo.All(x => x.Contains(Failed));

                        string number = string.Empty;
                        if (result)
                        {
                            ProcessTransactionCashfree processTransactionCashfree = new ProcessTransactionCashfree();
                            string newTransferId = string.Empty;

                            // Initiate the payment with new Order id corresponding the latest Order Id incremented by one...

                            var latestTransaction = Payments.OrderByDescending(x=>x.Id).FirstOrDefault();
                            if (latestTransaction != null)
                            {
                                if (!string.IsNullOrEmpty(latestTransaction?.UtcreferenceId))
                                {
                                    var arr = latestTransaction.UtcreferenceId?.Split("_");
                                    if (arr?.Length > 1)
                                    {
                                        number = arr[1];
                                        if (!string.IsNullOrEmpty(number))
                                        {
                                            var num = Convert.ToInt32(number) + 1;
                                            number = num.ToString();
                                        }

                                        newTransferId = arr[0] + "_" + number;
                                    }
                                    else
                                    {
                                        newTransferId = latestTransaction.UtcreferenceId + "_1";
                                    }
                                    processTransactionCashfree.amount = latestTransaction.Amount.ToString();
                                    processTransactionCashfree.beneId = RegdNo;
                                    processTransactionCashfree.transferId = newTransferId;
                                    processTransactionCashfree.transferMode = PaymentMode;

                                    transactionResponseCashfree = _cashfreePayoutCall.Transaction(processTransactionCashfree, cashfreeAuthCall?.data?.token);


                                    TblPaymentLeaser paymentLedgeradd = new TblPaymentLeaser();

                                    paymentLedgeradd.RegdNo = processTransactionCashfree.beneId;
                                    paymentLedgeradd.Amount = Convert.ToDecimal(processTransactionCashfree.amount);
                                    paymentLedgeradd.PaymentMode = PaymentMode;
                                    paymentLedgeradd.OrderId = transactionResponseCashfree?.data?.referenceId;
                                    paymentLedgeradd.UtcreferenceId = newTransferId;
                                    paymentLedgeradd.PaymentDate = DateTime.Now;
                                    paymentLedgeradd.ResponseDescription = transactionResponseCashfree?.message;
                                    paymentLedgeradd.TransactionId = transactionResponseCashfree?.data?.utr;
                                    paymentLedgeradd.IsActive = true;
                                    paymentLedgeradd.PaymentStatus = true;

                                    if (transactionResponseCashfree?.subCode != subcode && transactionResponseCashfree?.data?.acknowledged != 1)
                                    {
                                        paymentLedgeradd.PaymentStatus = false;
                                        paymentLedgeradd.PaymentResponse = transactionResponseCashfree?.message;//status
                                    }
                                    else
                                    {
                                        paymentLedgeradd.PaymentResponse = "Transfer completed successfully";//status
                                    }

                                    paymentLedgeradd.ResponseCode = transactionResponseCashfree?.subCode;
                                    paymentLedgeradd.CreatedDate = DateTime.Now;

                                    var OrderTransactionData = _transRepository.GetOrdertransDetails(processTransactionCashfree?.beneId);

                                    if (OrderTransactionData.OrderType == 16)
                                    {
                                        moduleType = "ABB";
                                        moduleReferenceId = OrderTransactionData?.AbbredemptionId;
                                    }
                                    else
                                    {
                                        moduleType = "Exchange";
                                        moduleReferenceId = OrderTransactionData?.ExchangeId;
                                    }

                                    paymentLedgeradd.ModuleType = moduleType;
                                    paymentLedgeradd.CreatedBy = userId;
                                    paymentLedgeradd.TransactionType = "Dr";
                                    paymentLedgeradd.ModuleReferenceId = moduleReferenceId;
                                    paymentLedgeradd.GatewayTransactioId = transactionResponseCashfree?.data?.utr;

                                    // Update the IsDeleted is true for all the failed transaction in case of when any transaction is success
                                    if (transactionResponseCashfree?.subCode == subcode && transactionResponseCashfree?.data?.acknowledged == 1)
                                    {
                                        var onlyFailedRecordsForRegdNumber = _context.TblPaymentLeasers.Where(x => x.RegdNo == RegdNo).ToList();
                                        onlyFailedRecordsForRegdNumber.ForEach(x => { x.IsDeleted = true; x.IsActive = false; });
                                        _context.TblPaymentLeasers.UpdateRange(onlyFailedRecordsForRegdNumber);
                                        _context.SaveChanges();
                                    }

                                    _paymentLeaserRepository.Create(paymentLedgeradd);
                                    var payledger = _paymentLeaserRepository.SaveChanges();
                                    if (paymentLedgeradd.Id > 0 && OrderTransactionData != null && transactionResponseCashfree?.subCode == subcode)
                                    {
                                        OrderTransactionData.AmountPaidToCustomer = true;
                                        OrderTransactionData.ModifiedDate = DateTime.Now;
                                        _transRepository.Update(OrderTransactionData);
                                        _transRepository.SaveChanges();
                                        message = "Transaction for amount Rs." + processTransactionCashfree.amount + "/- has been completed TransactionId=" + transactionResponseCashfree?.data?.utr;
                                        return Redirect(_config.Value.BaseURL + "PayOut/Details/?message=" + message + "&ReturnURL=" + returnUrl);
                                    }

                                    else
                                    {
                                        message = transactionResponseCashfree.message;
                                        return Redirect(_config.Value.BaseURL + "PayOut/DetailsForFailedTransaction/?message= " + message + "&ReturnURL=" + returnUrl);
                                    }
                                }
                                else
                                {
                                    message = "UtcTransferId is not as null in database for the corresponding RegdNumber, while repay the amount";
                                    return Redirect(_config.Value.BaseURL + "PayOut/DetailsForFailedTransaction/?message= " + message + "&ReturnURL=" + returnUrl);
                                }                
                            }
                        }
                        else
                        {
                            message = "All Payments is not failed to initiate another transaction";
                            return Redirect(_config.Value.BaseURL + "PayOut/DetailsForFailedTransaction/?message= " + message + "&ReturnURL=" + returnUrl);
                        }
                    }
                    else
                    {
                        message = "PaymentStatus not getting from the database for the RegdNo." + RegdNo;
                        return Redirect(_config.Value.BaseURL + "PayOut/DetailsForFailedTransaction/?message = " + message + "&ReturnURL=" + returnUrl);
                    }
                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("Repayment and save repaymentdata", "RepaymentTransaction", ex);
            }
            return Page();
        }
    }
}