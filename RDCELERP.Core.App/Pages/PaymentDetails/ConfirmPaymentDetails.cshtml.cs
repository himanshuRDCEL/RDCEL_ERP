using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Options;
using RDCELERP.BAL.Interface;
using RDCELERP.BAL.MasterManager;
using RDCELERP.Common.Enums;
using RDCELERP.Common.Helper;
using RDCELERP.DAL.Entities;
using RDCELERP.DAL.IRepository;
using RDCELERP.Model;
using RDCELERP.Model.ABBRedemption;
using RDCELERP.Model.Base;
using RDCELERP.Model.BusinessUnit;
using RDCELERP.Model.CashfreeModel;
using RDCELERP.Model.CommonModel;
using RDCELERP.Model.EVC_Allocated;
using RDCELERP.Model.ExchangeOrder;
using RDCELERP.Model.OrderTrans;
using RDCELERP.Model.QCComment;
using RDCELERP.Model.UPIVerificationModel;
using Microsoft.AspNetCore.Hosting;
using System.Drawing;
using System.Net.NetworkInformation;
using ZXing;
using ZXing.QrCode;

namespace RDCELERP.Core.App.Pages.PaymentDetails
{
    public class ConfirmPaymentDetailsModel : PageModel
    {
        #region Variable Declaration
        ILogging _logging;
        IQCCommentManager _QcCommentManager;
        ICustomerDetailsRepository _customerDetailsRepository;
        IUPIIdVerification _UpiIdVerification;
        ICashfreePayoutCall _cashfreePayoutCall;
        IExchangeABBStatusHistoryRepository _exchangeABBStatusHistoryRepository;
        IDropdownManager _dropdownManager;
        IOrderTransRepository _orderTransRepository;
        IOrderQCRepository _orderQCRepository;
        IBusinessPartnerRepository _businessPartnerRepository;
        ICommonManager _commonManager;
        IExchangeOrderRepository _exchangeOrderRepository;
        DateTime _currentDatetime = DateTime.Now.TrimMilliseconds();
        IABBRedemptionRepository _abbRedemptionRepository;
        IAbbRegistrationRepository _abbRegistrationRepository;
        IEVCManager _EVCManager;
        IOptions<ApplicationSettings> _config;
        IWebHostEnvironment _webHostEnvironment;
        #endregion

        #region Constructor
        public ConfirmPaymentDetailsModel(IQCCommentManager qcCommentManager, ICustomerDetailsRepository customerDetailsRepository, IUPIIdVerification upiverification, ICashfreePayoutCall cashfreePayoutCall, ILogging logging, IExchangeABBStatusHistoryRepository exchangeABBStatusHistoryRepository, IDropdownManager dropdownManager, IOrderTransRepository orderTransRepository, IOrderQCRepository orderQCRepository, IBusinessPartnerRepository businessPartnerRepository, ICommonManager commonManager, IExchangeOrderRepository exchangeOrderRepository, IABBRedemptionRepository aBBRedemptionRepository, IAbbRegistrationRepository abbRegistrationRepository, IEVCManager eVCManager, IOptions<ApplicationSettings> config, IWebHostEnvironment webHostEnvironment)
        {
            _QcCommentManager = qcCommentManager;
            _customerDetailsRepository = customerDetailsRepository;
            _UpiIdVerification = upiverification;
            _cashfreePayoutCall = cashfreePayoutCall;
            _logging = logging;
            _exchangeABBStatusHistoryRepository = exchangeABBStatusHistoryRepository;
            _dropdownManager = dropdownManager;
            _orderTransRepository = orderTransRepository;
            _orderQCRepository = orderQCRepository;
            _businessPartnerRepository = businessPartnerRepository;
            _commonManager = commonManager;
            _exchangeOrderRepository = exchangeOrderRepository;
            _abbRedemptionRepository = aBBRedemptionRepository;
            _abbRegistrationRepository = abbRegistrationRepository;
            _EVCManager = eVCManager;
            _config = config;
            _webHostEnvironment = webHostEnvironment;
        }
        #endregion

        #region Binding Properties
        [BindProperty(SupportsGet = true)]
        public UPINoViewModel UpiNoViewModel { get; set; }
        public int login_id { get; set; }

        [BindProperty(SupportsGet = true)]
        public string InvalidName { get; set; }

        [BindProperty(SupportsGet = true)]
        public int OrderType { get; set; }

        [BindProperty(SupportsGet = true)]
        public List<Allocate_EVCFromViewModel>? Allocate_EVCFromViewModels { get; set; }

        [BindProperty(SupportsGet = true)]
        public string URLPrefixforProd { get; set; }

        [BindProperty(SupportsGet = true)]
        public bool IsUPIQrRequired { get; set; }

        [BindProperty(SupportsGet = true)]
        public int? PicupExpectedInHrs { get; set; }
        #endregion
        public IActionResult OnGet(string regdNo, int userid, int? exchangeid, int status, int? AbbRedemptionid)
        {
            TblExchangeAbbstatusHistory tblExchangeAbbstatusHistory = null;
            TblOrderTran tblOrderTran = null;
            TblBusinessPartner tblBusinessPartner = null;
            TblCustomerDetail tblCustomerDetail = null;
            URLPrefixforProd = _config.Value.BaseURL;
            ViewData["URLPrefixforProd"] = URLPrefixforProd.TrimEnd('/');
            IsUPIQrRequired = _config.Value.IsUPIQrRequired;

            #region Pickup Expected Hrs ()
            PicupExpectedInHrs = _config.Value.PicupExpectedInHrs;
            if (PicupExpectedInHrs == null || PicupExpectedInHrs == 0)
            {
                // Default set 
                PicupExpectedInHrs = 36;
            }
            #endregion

            #region check for Discount Voucher For Exchange & ABB

            tblOrderTran = _orderTransRepository.GetRegdno(regdNo);
            if (tblOrderTran != null)
            {
                tblOrderTran = _orderTransRepository.GetExchangeABBbyordertrans(tblOrderTran.OrderTransId);
                {
                    if (tblOrderTran.Exchange != null)
                    {
                        OrderType = Convert.ToInt32(tblOrderTran.OrderType);
                        if (tblOrderTran.Exchange.StatusId == Convert.ToInt32(OrderStatusEnum.Waitingforcustapproval) || tblOrderTran.Exchange.StatusId == Convert.ToInt32(OrderStatusEnum.QCByPass))
                        {
                            tblBusinessPartner = _businessPartnerRepository.GetBPId(tblOrderTran.Exchange.BusinessPartnerId);
                            if (tblBusinessPartner != null && tblBusinessPartner.IsVoucher == true && tblBusinessPartner.VoucherType == 1)
                            {
                                UpiNoViewModel.VoucherType = "DiscountVoucher";
                            }
                            if (tblOrderTran.Exchange.CustomerDetailsId != null)
                            {
                                tblCustomerDetail = _customerDetailsRepository.GetCustDetails(tblOrderTran.Exchange.CustomerDetailsId);
                                if (tblCustomerDetail != null)
                                {
                                    UpiNoViewModel.CustomerName = tblCustomerDetail.FirstName != null ? tblCustomerDetail.FirstName + " " + tblCustomerDetail.LastName : string.Empty;
                                }
                            }
                        }
                        else
                        {
                            return RedirectToPage("/ThankYouPage/ThankYou");
                        }
                    }
                    else
                    {
                        OrderType = Convert.ToInt32(tblOrderTran.OrderType);
                        if (tblOrderTran.Abbredemption.Abbregistration.BusinessPartnerId != null)
                        {
                            if (tblOrderTran.Abbredemption.StatusId == Convert.ToInt32(OrderStatusEnum.Waitingforcustapproval) || tblOrderTran.Abbredemption.StatusId == Convert.ToInt32(OrderStatusEnum.QCByPass))
                            {
                                tblBusinessPartner = _businessPartnerRepository.GetBPId(tblOrderTran.Abbredemption.Abbregistration.BusinessPartnerId);
                                if (tblBusinessPartner != null && tblBusinessPartner.IsVoucher == true && tblBusinessPartner.VoucherType == 1)
                                {
                                    UpiNoViewModel.VoucherType = "DiscountVoucher";
                                }
                                if (tblOrderTran.Abbredemption.CustomerDetailsId != null)
                                {
                                    tblCustomerDetail = _customerDetailsRepository.GetCustDetails(tblOrderTran.Abbredemption.CustomerDetailsId);
                                    if (tblCustomerDetail != null)
                                    {
                                        UpiNoViewModel.CustomerName = tblCustomerDetail.FirstName != null ? tblCustomerDetail.FirstName + " " + tblCustomerDetail.LastName : string.Empty;
                                    }
                                }
                            }
                            else
                            {
                                return RedirectToPage("/ThankYouPage/ThankYou");
                            }
                        }
                    }
                }
            }
            #endregion

            #region dropdown for Time Slot
                var timeslot = _dropdownManager.GetTimeSlot();
            if (timeslot != null)
            {
                ViewData["timeslot"] = new SelectList(timeslot, "Value", "Text");
            }
            #endregion

            #region Check UPI Id in TblorderQc
            if (status == 0) //Check UPI For Add Benefinary button of Exchange Page
            {
                //to check UPI Id in TblorderQc
                if (tblOrderTran != null)
                {
                    TblOrderQc tblOrderQc = _orderQCRepository.GetQcorderBytransId(tblOrderTran.OrderTransId);
                    if (tblOrderQc != null && tblOrderQc.Upiid != null)
                    {
                        return RedirectToPage("/ThankYouPage/ThankYou");
                    }
                    else
                    {
                        return Page();
                    }
                }
                return Page();
            }
            #endregion

            #region Check either link is Expire or Not
            // tblOrderTran = _orderTransRepository.GetRegdno(regdNo);
            if (tblOrderTran != null && tblOrderTran.OrderType == (int?)LoVEnum.Exchange)
            {
                bool checkupiisrequired = _commonManager.CheckUpiisRequired(tblOrderTran.OrderTransId);
                if (checkupiisrequired == true)
                {
                    if (tblOrderTran.Exchange != null && exchangeid == null && userid <= 0)
                    {
                        tblExchangeAbbstatusHistory = _exchangeABBStatusHistoryRepository.GetByRegdstatusno(regdNo, tblOrderTran.Exchange.StatusId);
                        if (tblExchangeAbbstatusHistory != null && (tblExchangeAbbstatusHistory.StatusId == Convert.ToInt32(OrderStatusEnum.Waitingforcustapproval) || tblExchangeAbbstatusHistory.StatusId == Convert.ToInt32(OrderStatusEnum.QCByPass)))
                        {
                            DateTime complaintDate = Convert.ToDateTime(tblExchangeAbbstatusHistory.CreatedDate);
                            DateTime todaysdate = DateTime.Now;
                            TimeSpan variable = todaysdate - complaintDate;
                            int date = Convert.ToInt32(variable.Days * 24);
                            date = date + variable.Hours;
                            if (date > 48)
                            {
                                return RedirectToPage("/ThankYouPage/ExpireLink");
                            }
                        }
                        else if (tblExchangeAbbstatusHistory != null && (status <= Convert.ToInt32(OrderStatusEnum.AmountApprovedbyCustomerAfterQC) && status == 0) && userid > 0)
                        {
                            return Page();
                        }
                        else if (tblOrderTran.Exchange.StatusId == Convert.ToInt32(OrderStatusEnum.Waitingforcustapproval) || tblOrderTran.Exchange.StatusId == Convert.ToInt32(OrderStatusEnum.QCByPass) || tblOrderTran.Exchange.StatusId == Convert.ToInt32(OrderStatusEnum.AmountApprovedbyCustomerAfterQC))
                        {
                            return RedirectToPage("/ThankYouPage/ThankYou");
                        }
                        else
                        {
                            return RedirectToPage("/ThankYouPage/ThankYou");
                        }
                    }
                    else
                    {
                        if (tblOrderTran.Exchange.StatusId == Convert.ToInt32(OrderStatusEnum.AmountApprovedbyCustomerAfterQC))
                        {
                            return RedirectToPage("/ThankYouPage/ThankYou");
                        }
                        else
                        {
                            return Page();
                        }
                    }

                }
                else
                {
                    UpiNoViewModel.VoucherType = "DiscountVoucher";
                }

            }
            else
            {
                if (tblOrderTran != null)
                {
                    bool checkupiisrequired = _commonManager.CheckUpiisRequired(tblOrderTran.OrderTransId);
                    if (checkupiisrequired == false)
                    {
                        UpiNoViewModel.VoucherType = "DiscountVoucher";
                    }
                    if (tblOrderTran.Abbredemption != null && AbbRedemptionid == null && userid <= 0)
                    {
                        tblExchangeAbbstatusHistory = _exchangeABBStatusHistoryRepository.GetByRegdstatusno(regdNo, tblOrderTran.Abbredemption.StatusId);
                        if (tblExchangeAbbstatusHistory != null && (tblExchangeAbbstatusHistory.StatusId == Convert.ToInt32(OrderStatusEnum.Waitingforcustapproval) || tblExchangeAbbstatusHistory.StatusId == Convert.ToInt32(OrderStatusEnum.QCByPass)))
                        {
                            DateTime StatussetDate = Convert.ToDateTime(tblExchangeAbbstatusHistory.CreatedDate);
                            DateTime todaysdate = DateTime.Now;
                            TimeSpan variable = todaysdate - StatussetDate;
                            int date = Convert.ToInt32(variable.Days * 24);
                            date = date + variable.Hours;
                            if (date > 48)
                            {
                                return RedirectToPage("/ThankYouPage/ExpireLink");
                            }
                        }
                        else if (tblExchangeAbbstatusHistory != null && (status <= Convert.ToInt32(OrderStatusEnum.AmountApprovedbyCustomerAfterQC) && status == 0))
                        {
                            return Page();
                        }
                        else if (new[] { Convert.ToInt32(OrderStatusEnum.Waitingforcustapproval), Convert.ToInt32(OrderStatusEnum.QCByPass), Convert.ToInt32(OrderStatusEnum.AmountApprovedbyCustomerAfterQC) }.Contains(tblOrderTran.Abbredemption.StatusId.Value))
                        {
                            return RedirectToPage("/ThankYouPage/ThankYou");
                        }
                        else
                        {
                            return RedirectToPage("/ThankYouPage/ThankYou");
                        }
                    }
                    else
                    {
                        if (tblOrderTran.Abbredemption.StatusId == Convert.ToInt32(OrderStatusEnum.AmountApprovedbyCustomerAfterQC))
                        {
                            return RedirectToPage("/ThankYouPage/ThankYou");
                        }
                        else
                        {
                            return Page();
                        }
                    }


                }
                else
                {
                    if (tblOrderTran != null)
                    {
                        if (tblOrderTran.Abbredemption.StatusId == Convert.ToInt32(OrderStatusEnum.AmountApprovedbyCustomerAfterQC))
                        {
                            return RedirectToPage("/ThankYouPage/ThankYou");
                        }
                        else
                        {
                            return Page();
                        }
                    }
                }
            }
            #endregion
            return Page();
        }
        public IActionResult OnPostAsync()
        {
            try
            {
                #region Declaration
                UPIVerification root = new UPIVerification();
                CashfreeAuth cashfreeAuthCall = new CashfreeAuth();
                AddBeneficiary addBeneficiarry = new AddBeneficiary();
                string vpaValid = EnumHelper.DescriptionAttr(UPIResponseEnum.vpaValid);
                AddBeneficiaryResponse beneficiaryResponse = new AddBeneficiaryResponse();
                string subcode = Convert.ToInt32(CashfreeEnum.Succcess).ToString();
                TblOrderTran tblOrderTran = null;
                TblCustomerDetail tblCustomerDetail = null;
                TblExchangeOrder tblExchangeOrder = null;
                TblAbbredemption tblAbbredemption = null;
                int result = 0;
                var enableEvcAutoAllocation = _config.Value.EvcAutoAllocation;
                #endregion
                if (UpiNoViewModel.Regdno != null)
                {
                    #region Pickup Expected Hrs ()
                    PicupExpectedInHrs = _config.Value.PicupExpectedInHrs;
                    if (PicupExpectedInHrs == null || PicupExpectedInHrs == 0)
                    {
                        // Default set 
                        PicupExpectedInHrs = 36;
                    }
                    #endregion
                    tblOrderTran = _orderTransRepository.GetOrderTransByRegdno(UpiNoViewModel.Regdno);
                    if (tblOrderTran != null)
                    {
                        bool checkupiisrequired = _commonManager.CheckUpiisRequired(tblOrderTran.OrderTransId);
                        if (checkupiisrequired == true)
                        {
                            if (UpiNoViewModel.CustomerFirstName != null)
                            {
                                if (OrderType > 0 && OrderType == (int?)LoVEnum.Exchange)
                                {
                                    #region Update Name in TblcustomerDetails for Exchange Order
                                    tblExchangeOrder = _exchangeOrderRepository.GetExchOrderByRegdNo(UpiNoViewModel.Regdno);
                                    if (tblExchangeOrder != null)
                                    {
                                        tblCustomerDetail = _customerDetailsRepository.GetCustDetails(tblExchangeOrder.CustomerDetailsId);
                                        if (tblCustomerDetail != null)
                                        {
                                            tblCustomerDetail.FirstName = UpiNoViewModel.CustomerFirstName;
                                            tblCustomerDetail.LastName = UpiNoViewModel.CustomerLastName;
                                            tblCustomerDetail.ModifiedBy = 3;
                                            tblCustomerDetail.ModifiedDate = _currentDatetime;
                                            _customerDetailsRepository.Update(tblCustomerDetail);
                                            _customerDetailsRepository.SaveChanges();
                                        }
                                    }
                                    #endregion
                                }
                                else
                                {
                                    #region Update Name in TblcustomerDetails & tblabbregistration for ABB Order 
                                    TblAbbregistration tblAbbregistration = _abbRegistrationRepository.GetRegdNo(UpiNoViewModel.Regdno);
                                    if (tblAbbregistration != null)
                                    {
                                        tblAbbregistration.CustFirstName = UpiNoViewModel.CustomerFirstName;
                                        tblAbbregistration.CustLastName = UpiNoViewModel.CustomerLastName;
                                        tblAbbregistration.ModifiedBy = 3;
                                        tblAbbregistration.ModifiedDate = _currentDatetime;
                                        _abbRegistrationRepository.Update(tblAbbregistration);
                                        _abbRegistrationRepository.SaveChanges();
                                    }

                                    tblAbbredemption = _abbRedemptionRepository.GetRegdNo(UpiNoViewModel.Regdno);
                                    if (tblAbbredemption != null)
                                    {
                                        tblCustomerDetail = _customerDetailsRepository.GetCustDetails(tblAbbredemption.CustomerDetailsId);
                                        if (tblCustomerDetail != null)
                                        {
                                            tblCustomerDetail.FirstName = UpiNoViewModel.CustomerFirstName;
                                            tblCustomerDetail.LastName = UpiNoViewModel.CustomerLastName;
                                            tblCustomerDetail.ModifiedBy = 3;
                                            tblCustomerDetail.ModifiedDate = _currentDatetime;
                                            _customerDetailsRepository.Update(tblCustomerDetail);
                                            _customerDetailsRepository.SaveChanges();
                                        }
                                    }

                                    #endregion
                                }
                            }
                            if (UpiNoViewModel.UPIId != null)
                            {
                                cashfreeAuthCall = _cashfreePayoutCall.CashFreeAuthCall();
                                if (cashfreeAuthCall.subCode == subcode)
                                {
                                    root = _UpiIdVerification.CheckUpiId(UpiNoViewModel.Regdno, UpiNoViewModel.UPIId, cashfreeAuthCall.data.token);
                                    //if (root.subCode == subcode && !string.IsNullOrEmpty(root.data.nameAtBank)&& root.data.accountExists=="YES")
                                    if (root.subCode == subcode && root.data.accountExists == "YES")
                                        {
                                        int statusid = !String.IsNullOrEmpty(HttpContext.Request.Query["status"]) ? Convert.ToInt32(HttpContext.Request.Query["status"]) : 0;
                                        if ((statusid == (int?)OrderStatusEnum.Waitingforcustapproval || statusid == (int?)OrderStatusEnum.QCByPass))
                                        {
                                            UpiNoViewModel.StatusId = Convert.ToInt32(HttpContext.Request.Query["status"]);
                                            UpiNoViewModel.Userid = UpiNoViewModel.Userid == 0 || UpiNoViewModel.Userid == null ? 3 : UpiNoViewModel.Userid;
                                            result = _QcCommentManager.SaveUpino(UpiNoViewModel);
                                            if (result > 0)
                                            {
                                                beneficiaryResponse = AddBeneficiary(UpiNoViewModel, tblOrderTran, cashfreeAuthCall);
                                                if (beneficiaryResponse.subCode == subcode)
                                                {
                                                    string beneficiaryResponseMsg = beneficiaryResponse.message;
                                                    ViewData["Message"] = beneficiaryResponseMsg;

                                                    Exception e = new Exception(beneficiaryResponseMsg);
                                                    _logging.WriteErrorToDB("ConfirmPaymentDetailsModel", "OnPostAsync", e);

                                                    if (enableEvcAutoAllocation == true)
                                                    {
                                                        #region EVC Auto-Allocation Phase II
                                                        return RedirectToPage("/EVC_Allocation/AutoAllocation", new { orderTransId = tblOrderTran.OrderTransId });
                                                        #endregion
                                                    }
                                                    else
                                                    {
                                                        return RedirectToPage("/ThankYouPage/ThankYou");
                                                    }
                                                }
                                                else
                                                {
                                                    string beneficiaryResponseMsg = beneficiaryResponse.message;
                                                    ViewData["Message"] = beneficiaryResponseMsg;

                                                    Exception e = new Exception(beneficiaryResponseMsg);
                                                    _logging.WriteErrorToDB("ConfirmPaymentDetailsModel", "OnPostAsync", e);
                                                    return RedirectToPage("/ThankYouPage/ThankYou");
                                                }
                                            }
                                            else
                                                return Page();
                                        }
                                        else
                                        {
                                            UpiNoViewModel.Userid = Convert.ToInt32(HttpContext.Request.Query["userid"]);
                                            result = _QcCommentManager.SaveUPIIdByExchangemanage(UpiNoViewModel);
                                            if (result > 0)
                                                return RedirectToPage("/ThankYouPage/ThankYou");
                                            else
                                                return Page();
                                        }
                                    }

                                    //else if (subcode==root.subCode && string.IsNullOrEmpty(root.data.nameAtBank)&& root.data.accountExists=="NO")
                                    else if (subcode == root.subCode && root.data.accountExists == "NO")
                                    {
                                        var timeslot = _dropdownManager.GetTimeSlot();
                                        if (timeslot != null)
                                        {
                                            ViewData["timeslot"] = new SelectList(timeslot, "Value", "Text");
                                        }
                                        ViewData["Message"] = "Upi id is not valid";
                                        IsUPIQrRequired = _config.Value.IsUPIQrRequired;
                                    }
                                    else
                                    {
                                        if (root.message != null && root.message.Contains("Please provide a valid Name"))
                                        {
                                            InvalidName = root.message;
                                            InvalidName = "Please provide a valid Name";
                                            ViewData["Message"] = "Please provide a valid Name";
                                        }

                                        var timeslot = _dropdownManager.GetTimeSlot();
                                        if (timeslot != null)
                                        {
                                            ViewData["timeslot"] = new SelectList(timeslot, "Value", "Text");
                                        }
                                        string invalidupiid = root.message;
                                        ViewData["Message"] = invalidupiid;
                                        IsUPIQrRequired = _config.Value.IsUPIQrRequired;
                                    }
                                }
                                else
                                {
                                    string cashfreeAuthCallMsg = cashfreeAuthCall.message;
                                    ViewData["Message"] = cashfreeAuthCallMsg;
                                }
                            }
                        }
                        else
                        {
                            UpiNoViewModel.StatusId = Convert.ToInt32(HttpContext.Request.Query["status"]);
                            result = _QcCommentManager.SaveUpino(UpiNoViewModel);
                            if (result > 0)
                            {
                                if (enableEvcAutoAllocation == true)
                                {
                                    #region EVC Auto-Allocation Phase II
                                    return RedirectToPage("/EVC_Allocation/AutoAllocation", new { orderTransId = tblOrderTran.OrderTransId });
                                    #endregion
                                }
                                else
                                {
                                    return RedirectToPage("/ThankYouPage/ThankYou");
                                }
                            }
                            else
                                return Page();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("ConfirmPaymentDetailsModel", "OnPostAsync", ex);
            }
            return Page();
        }

        #region AddBeneficiary        
        public AddBeneficiaryResponse AddBeneficiary(UPINoViewModel UpiNoViewModel, TblOrderTran tblOrderTran, CashfreeAuth cashfreeAuthCall)
        {
            AddBeneficiary addBeneficiarry = new AddBeneficiary();
            AddBeneficiaryResponse beneficiaryResponse = new AddBeneficiaryResponse();
            TblCustomerDetail tblCustomerDetail = null;
            if (tblOrderTran != null && tblOrderTran.OrderType == (int?)LoVEnum.Exchange)
            {
                if (tblOrderTran.Exchange.CustomerDetailsId != null)
                {
                    tblCustomerDetail = _customerDetailsRepository.GetCustDetails(tblOrderTran.Exchange.CustomerDetailsId);
                    if (tblCustomerDetail != null)
                    {
                        addBeneficiarry.beneId = UpiNoViewModel.Regdno;
                        addBeneficiarry.name = tblCustomerDetail.FirstName + " " + tblCustomerDetail.LastName;
                        addBeneficiarry.email = tblCustomerDetail.Email != null ? tblCustomerDetail.Email : string.Empty;
                        addBeneficiarry.phone = tblCustomerDetail.PhoneNumber != null ? tblCustomerDetail.PhoneNumber : string.Empty;
                        addBeneficiarry.address1 = tblCustomerDetail.Address1 != null ? tblCustomerDetail.Address1 : string.Empty;
                        addBeneficiarry.city = tblCustomerDetail.City != null ? tblCustomerDetail.City : string.Empty;
                        addBeneficiarry.state = tblCustomerDetail.State != null ? tblCustomerDetail.State : string.Empty;
                        addBeneficiarry.pincode = tblCustomerDetail.ZipCode != null ? tblCustomerDetail.ZipCode : string.Empty;
                        addBeneficiarry.vpa = UpiNoViewModel.UPIId;
                    }
                }
            }
            else
            {
                addBeneficiarry.beneId = UpiNoViewModel.Regdno;
                addBeneficiarry.name = tblOrderTran.Abbredemption.Abbregistration.CustFirstName + " " + tblOrderTran.Abbredemption.Abbregistration.CustLastName;
                addBeneficiarry.email = tblOrderTran.Abbredemption.Abbregistration.CustEmail != null ? tblOrderTran.Abbredemption.Abbregistration.CustEmail : string.Empty;
                addBeneficiarry.phone = tblOrderTran.Abbredemption.Abbregistration.CustMobile != null ? tblOrderTran.Abbredemption.Abbregistration.CustMobile : string.Empty;
                addBeneficiarry.address1 = tblOrderTran.Abbredemption.Abbregistration.CustAddress1 != null ? tblOrderTran.Abbredemption.Abbregistration.CustAddress1 : string.Empty;
                addBeneficiarry.city = tblOrderTran.Abbredemption.Abbregistration.CustCity != null ? tblOrderTran.Abbredemption.Abbregistration.CustCity : string.Empty;
                addBeneficiarry.state = tblOrderTran.Abbredemption.Abbregistration.CustState != null ? tblOrderTran.Abbredemption.Abbregistration.CustState : string.Empty;
                addBeneficiarry.pincode = tblOrderTran.Abbredemption.Abbregistration.CustPinCode != null ? tblOrderTran.Abbredemption.Abbregistration.CustPinCode : string.Empty;
                addBeneficiarry.vpa = UpiNoViewModel.UPIId;
            }
            beneficiaryResponse = _cashfreePayoutCall.AddBenefiaciary(addBeneficiarry, cashfreeAuthCall.data.token);

            return beneficiaryResponse;
        }
        #endregion

        #region Check Valid UPIId Added By Pooja Jatav
        public JsonResult OnGetCheckValidUPIIdAsync(string ValidUPIId)
        {
            //string upiPattern = @"^[a-zA-Z0-9.-]{2, 256}@[a-zA-Z][a-zA-Z]{2, 64}$";
            string upiPattern = @"^(.{2,256}@.{2,64}|[0-9]{7,15})$";
            bool isvalid = false;

            if (ValidUPIId == null)
            {
                return new JsonResult(isvalid);
            }
            else
            {
                Regex regex = new Regex(upiPattern);
                Match match = regex.Match(ValidUPIId);
                isvalid = match.Success;
                return new JsonResult(isvalid);
            }
        }
        #endregion

        public class DataContainer
        {
            public string UpiId { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
        }

        public IActionResult OnPostImage(string baseData)
        {
            string Result = baseData;

            if (Result != null)
            {
                Uri uri = new Uri(Result);
                string pa = HttpUtility.ParseQueryString(uri.Query).Get("pa");
                string pn = HttpUtility.ParseQueryString(uri.Query).Get("pn");
                if(pa == null || pn == null)
                {
                    return new JsonResult(null);
                }
                string[] strings = pn.Split(' ');
                int len = strings.Length;
                
                // upi://pay?pa=9151532070@paytm&pn=Avinash%20srivastav&mc=0000&mode=02&purpose=00&orgid=159761
                var data = new DataContainer
                {
                    UpiId = pa,
                    FirstName = strings[0],
                    LastName = strings[len-1]
                };
                return new JsonResult(data);
            }
            return new JsonResult(null);
        }
    }
}
