using Mailjet.Client.Resources;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Net;
using RDCELERP.BAL.Interface;
using RDCELERP.Common.Enums;
using RDCELERP.Common.Helper;
using RDCELERP.DAL.Entities;
using RDCELERP.DAL.IRepository;
using RDCELERP.Model.Base;
using RDCELERP.Model.CashfreeModel;
using RDCELERP.Model.MobileApplicationModel;
using static Org.BouncyCastle.Math.EC.ECCurve;

namespace RDCELERP.CoreWebApi.Controllers.Common
{
    [Route("api/Common/[controller]")]
    [ApiController]
    public class PayOutController : ControllerBase
    {
        ILogging _logging;
        ICashfreePayoutCall _cashfreePayoutCall;
        public readonly IOptions<ApplicationSettings> _config;
        IOrderTransRepository _orderTransRepository;
        private readonly Digi2l_DevContext _context;
        IABBRedemptionRepository _redemptionRepository;
        IPaymentLeaser _paymentLeaserRepository;

        public PayOutController(ILogging logging, ICashfreePayoutCall cashfreePayoutCall, IOptions<ApplicationSettings> config, IOrderTransRepository orderTransRepository, Digi2l_DevContext context, IABBRedemptionRepository aBBRedemptionRepository, IPaymentLeaser paymentLeaserRepository)
        {
            _logging = logging;
            _cashfreePayoutCall = cashfreePayoutCall;
            _config = config;
            _orderTransRepository = orderTransRepository;
            _context = context;
            _redemptionRepository = aBBRedemptionRepository;
            _paymentLeaserRepository = paymentLeaserRepository;
        }

        #region Test PayOutController API
        [HttpGet]
        [Route("Test")]
        public ResponseResult GetTest()
        {
            ResponseResult responseResult = new ResponseResult();
            try
            {
                responseResult.message = "API Working..";
                responseResult.Status = true;
                responseResult.Status_Code = HttpStatusCode.Accepted;
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("PayOutController", "GetTest", ex);
            }
            return responseResult;
        }
        #endregion

        #region To get the beneficiary details
        [Authorize]
        [HttpGet]
        [Route("GetBeneficiaryDetail")]

        public ResponseResult GetBeneficiaryDetail(string RegdNo)
        {
            GetBeneficiary getbeneficiary = new GetBeneficiary();
            CashfreeAuth cashfreeAuthCall = new CashfreeAuth();
            string subcode = null;
            ResponseResult responseResult = new ResponseResult();

            try
            {
                //RegdNo = SecurityHelper.DecryptString(RegdNo, _config.Value.SecurityKey);

                subcode = Convert.ToInt32(CashfreeEnum.Succcess).ToString();
                if(RegdNo != null)
                {
                    TblOrderTran OrderTransObj;
                    OrderTransObj = _orderTransRepository.GetOrdertransDetails(RegdNo);
                    if(OrderTransObj != null)
                    {
                        if (OrderTransObj.OrderType == Convert.ToInt32(OrderTypeEnum.Exchange))
                        {
                            TblExchangeOrder ExchangeObj;
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
                                        responseResult.message = getbeneficiary.message;
                                        responseResult.Status = true;
                                        responseResult.Status_Code = HttpStatusCode.OK;
                                        responseResult.Data = getbeneficiary.data;
                                    }
                                    else
                                    {
                                        responseResult.message = getbeneficiary.message;
                                        responseResult.Status = false;
                                        responseResult.Status_Code = HttpStatusCode.BadRequest;
                                        return responseResult;
                                    }
                                }
                                else
                                {
                                    responseResult.message = cashfreeAuthCall.message;
                                    responseResult.Status = false;
                                    responseResult.Status_Code = HttpStatusCode.BadRequest;
                                    return responseResult;
                                }

                            }
                            else
                            {
                                responseResult.message = "Order data not found";
                                responseResult.Status = false;
                                responseResult.Status_Code = HttpStatusCode.BadRequest;
                                return responseResult;
                            }
                        }
                        else if (OrderTransObj.OrderType == Convert.ToInt32(OrderTypeEnum.ABB))
                        {
                            TblAbbredemption redemptionObj;
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
                                        responseResult.message = getbeneficiary.message;
                                        responseResult.Status = true;
                                        responseResult.Status_Code = HttpStatusCode.OK;
                                    }
                                    else
                                    {
                                        responseResult.message = getbeneficiary.message;
                                        responseResult.Status = false;
                                        responseResult.Status_Code = HttpStatusCode.BadRequest;
                                        return responseResult;
                                    }
                                }
                                else
                                {
                                    responseResult.message = cashfreeAuthCall.message;
                                    responseResult.Status = false;
                                    responseResult.Status_Code = HttpStatusCode.BadRequest;
                                    return responseResult;
                                }
                            }
                            else
                            {
                                responseResult.message = "Order data not found";
                                responseResult.Status = false;
                                responseResult.Status_Code = HttpStatusCode.BadRequest;
                                return responseResult;
                            }
                        }
                        else
                        {
                            responseResult.message = "Order type not defined";
                            responseResult.Status = false;
                            responseResult.Status_Code = HttpStatusCode.BadRequest;
                            return responseResult;
                        }
                    }
                    else
                    {
                        responseResult.message = "Order data not found";
                        responseResult.Status = false;
                        responseResult.Status_Code = HttpStatusCode.BadRequest;
                        return responseResult;
                    }
                }
                else
                {
                    responseResult.message = "Please provide RegdNo";
                    responseResult.Status = false;
                    responseResult.Status_Code = HttpStatusCode.BadRequest;
                    return responseResult;
                }
            }
            catch(Exception ex)
            {
                _logging.WriteErrorToDB("PayOutController", "GetBeneficiaryDetail", ex);

                responseResult.message = ex.Message;
                responseResult.Status_Code = HttpStatusCode.InternalServerError;
                responseResult.Status = false;
            }
            return responseResult;
        }
        #endregion


        #region The API to send the transfer request
        [Authorize]
        [HttpPost]
        [Route("SendTransferRequest")]
        public ResponseResult SendTransferRequest(TransferAmount transferAmount)
        {
            TblPaymentLeaser paymentLedger = null;
            TransactionResponseCashfree transactionResponse = new TransactionResponseCashfree();
            ProcessTransactionCashfree processtransaction = new ProcessTransactionCashfree();
            CashfreeAuth cashfreeAuthCall = new CashfreeAuth();
            ResponseResult responseResult = new ResponseResult();
            string subcode = null;
            string ModuleType = null;
            int? CreatedBy = null;
            int? ModuleReferenceId = null;
            string TransactionType = null;
            try
            {
                subcode = Convert.ToInt32(CashfreeEnum.Succcess).ToString();
                TblOrderTran OrderTransObj;
                OrderTransObj = _orderTransRepository.GetOrdertransDetails(transferAmount.beneId);
                if(OrderTransObj != null)
                {
                    if (OrderTransObj.OrderType == Convert.ToInt32(OrderTypeEnum.Exchange))
                    {
                        ModuleType = EnumHelper.DescriptionAttr(CashfreeEnum.Exchange);
                        CreatedBy = OrderTransObj.ExchangeId;
                        ModuleReferenceId = OrderTransObj.ExchangeId;
                    }
                    else
                    {
                        ModuleType = EnumHelper.DescriptionAttr(CashfreeEnum.ABB);
                        CreatedBy = OrderTransObj.AbbredemptionId;
                        ModuleReferenceId = OrderTransObj.AbbredemptionId;
                    }
                
                    TransactionType = EnumHelper.DescriptionAttr(CashfreeEnum.TransactionType);
                
                    paymentLedger = _context.TblPaymentLeasers.FirstOrDefault(x => x.RegdNo == transferAmount.beneId && x.ModuleType == ModuleType && x.IsActive == true && x.PaymentStatus == true);
                    if (paymentLedger == null)
                    {
                        cashfreeAuthCall = _cashfreePayoutCall.CashFreeAuthCall();
                        if (cashfreeAuthCall.subCode == subcode)
                        {
                            processtransaction.beneId = transferAmount.beneId;
                            processtransaction.amount = transferAmount.amount;
                            processtransaction.transferMode = transferAmount.transferMode;
                            processtransaction.transferId = transferAmount.transferId;
                            transactionResponse = _cashfreePayoutCall.Transaction(processtransaction, cashfreeAuthCall.data.token);
                            if (transactionResponse.subCode == subcode && transactionResponse != null)
                            {
                                TblPaymentLeaser paymentLedgeradd = new TblPaymentLeaser();
                                paymentLedgeradd.RegdNo = transferAmount.beneId;
                                paymentLedgeradd.Amount = Convert.ToDecimal(transferAmount.amount);
                                paymentLedgeradd.PaymentMode = transferAmount.transferMode;
                                paymentLedgeradd.OrderId = transferAmount.beneId;
                                paymentLedgeradd.PaymentDate = DateTime.Now;
                                paymentLedgeradd.ResponseDescription = transactionResponse.message;
                                paymentLedgeradd.PaymentResponse = transactionResponse.message;
                                paymentLedgeradd.TransactionId = transactionResponse.data.utr;
                                paymentLedgeradd.ModuleType = ModuleType;
                                paymentLedgeradd.IsActive = true;
                                paymentLedgeradd.PaymentStatus = true;
                                paymentLedgeradd.ResponseCode = subcode;
                                paymentLedgeradd.TransactionType = TransactionType;
                                paymentLedgeradd.CreatedBy = CreatedBy;
                                paymentLedgeradd.CreatedDate = DateTime.Now;
                                paymentLedgeradd.ModuleReferenceId = ModuleReferenceId;
                                paymentLedgeradd.UtcreferenceId = transferAmount.beneId;
                                _paymentLeaserRepository.Create(paymentLedgeradd);
                                _paymentLeaserRepository.SaveChanges();
                                if (paymentLedgeradd.Id >= 0)
                                {
                                    #region Update order trans for payment recieved for this order
                                    OrderTransObj.AmountPaidToCustomer = true;
                                    _orderTransRepository.Update(OrderTransObj);
                                    _orderTransRepository.SaveChanges();
                                    #endregion
                                    responseResult.message = transactionResponse.message;
                                    responseResult.Status = true;
                                    responseResult.Status_Code = HttpStatusCode.OK;
                                    return responseResult;
                                }
                                else
                                {
                                    responseResult.message = "Something Went wrong";
                                    responseResult.Status = false;
                                    responseResult.Status_Code = HttpStatusCode.OK;
                                    return responseResult;
                                }
                            }
                            else if(transactionResponse.data != null)
                            {
                                TblPaymentLeaser paymentLedgeradd = new TblPaymentLeaser();
                                paymentLedgeradd.RegdNo = transferAmount.beneId;
                                paymentLedgeradd.Amount = Convert.ToDecimal(transferAmount.amount);
                                paymentLedgeradd.PaymentMode = transferAmount.transferMode;
                                paymentLedgeradd.OrderId = transferAmount.beneId;
                                paymentLedgeradd.PaymentDate = DateTime.Now;
                                paymentLedgeradd.ResponseDescription = transactionResponse.message;
                                paymentLedgeradd.PaymentResponse = transactionResponse.message;
                                paymentLedgeradd.TransactionId = transactionResponse.data.utr;
                                paymentLedgeradd.ModuleType = ModuleType;
                                paymentLedgeradd.IsActive = true;
                                paymentLedgeradd.PaymentStatus = false;
                                paymentLedgeradd.ResponseCode = transactionResponse.subCode;
                                paymentLedgeradd.TransactionType = TransactionType;
                                paymentLedgeradd.CreatedBy = CreatedBy;
                                paymentLedgeradd.CreatedDate = DateTime.Now;
                                paymentLedgeradd.ModuleReferenceId = ModuleReferenceId;
                                paymentLedgeradd.UtcreferenceId = transferAmount.beneId;
                                _paymentLeaserRepository.Create(paymentLedgeradd);
                                _paymentLeaserRepository.SaveChanges();
                                responseResult.message = transactionResponse.message;
                                responseResult.Status = false;
                                responseResult.Status_Code = HttpStatusCode.OK;
                                return responseResult;
                            }
                            else
                            {
                                TblPaymentLeaser paymentLedgeradd = new TblPaymentLeaser();
                                paymentLedgeradd.RegdNo = transferAmount.beneId;
                                paymentLedgeradd.Amount = Convert.ToDecimal(transferAmount.amount);
                                paymentLedgeradd.PaymentMode = transferAmount.transferMode;
                                paymentLedgeradd.OrderId = transferAmount.beneId;
                                paymentLedgeradd.PaymentDate = DateTime.Now;
                                paymentLedgeradd.ResponseDescription = transactionResponse.message;
                                paymentLedgeradd.PaymentResponse = transactionResponse.message;
                                //paymentLedgeradd.TransactionId = transactionResponse.data.utr;
                                paymentLedgeradd.ModuleType = ModuleType;
                                paymentLedgeradd.IsActive = true;
                                paymentLedgeradd.PaymentStatus = false;
                                paymentLedgeradd.ResponseCode = transactionResponse.subCode;
                                paymentLedgeradd.TransactionType = TransactionType;
                                paymentLedgeradd.CreatedBy = CreatedBy;
                                paymentLedgeradd.CreatedDate = DateTime.Now;
                                paymentLedgeradd.ModuleReferenceId = ModuleReferenceId;
                                paymentLedgeradd.UtcreferenceId = transferAmount.beneId;
                                _paymentLeaserRepository.Create(paymentLedgeradd);
                                _paymentLeaserRepository.SaveChanges();
                                responseResult.message = transactionResponse.message;
                                responseResult.Status = false;
                                responseResult.Status_Code = HttpStatusCode.OK;
                                return responseResult;
                            }
                        }
                        else
                        {
                            responseResult.message = cashfreeAuthCall.message;
                            responseResult.Status = false;
                            responseResult.Status_Code = HttpStatusCode.OK;
                            return responseResult;
                        }
                    }
                    else
                    {
                        responseResult.message = "Amount= " + paymentLedger.Amount + "/- already paid transaction id=" + paymentLedger.TransactionId;
                        responseResult.Status = false;
                        responseResult.Status_Code = HttpStatusCode.OK;
                        return responseResult;
                    }
                }
                else
                {
                    responseResult.message = "Order data not found";
                    responseResult.Status = false;
                    responseResult.Status_Code = HttpStatusCode.OK;
                    return responseResult;
                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("PayOutController", "SendTransferRequest", ex);
            }
            return responseResult;
        }

        #endregion

    }
}
