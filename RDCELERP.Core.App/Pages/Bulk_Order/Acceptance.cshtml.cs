using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;
using RDCELERP.BAL.Interface;
using RDCELERP.Common.Enums;
using RDCELERP.Common.Helper;
using RDCELERP.Core.App.Pages.Base;
using RDCELERP.DAL.Entities;
using RDCELERP.DAL.IRepository;
using RDCELERP.Model.AcceptancePageBulkLiquidation;
using RDCELERP.Model.Base;
using static RDCELERP.Model.Whatsapp.WhatsappQCPriceViewModel;

using RDCELERP.Model.Master;
using RDCELERP.Model.Whatsapp;
using static RDCELERP.Model.CashVoucher.VoucherSucessViewModel;
using RDCELERP.BAL.MasterManager;
using RDCELERP.DAL.Repository;
using RDCELERP.Model.QCComment;
using ZXing;
using RDCELERP.Model.CashfreeModel;
using RDCELERP.Model.UPIVerificationModel;
using static Org.BouncyCastle.Math.EC.ECCurve;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace RDCELERP.Core.App.Pages.Bulk_Order
{
    public class AcceptanceModel : CrudBasePageModel
    {

        #region Variable Declaration

        private readonly RDCELERP.DAL.Entities.Digi2l_DevContext _context;
        private readonly IExchangeOrderRepository _exchangeOrderRepository;
        private readonly IBusinessPartnerRepository _basinessPartnerRepository;
        private readonly IQCCommentManager _QcCommentManager;
        ICustomerDetailsRepository _customerDetailsRepository;
        IWhatsappNotificationManager _whatsappNotificationManager;
        IWhatsAppMessageRepository _whatsAppMessageRepository;
        IBusinessUnitRepository _businessUnitRepository;
        IOrderTransRepository _orderTransRepository;
        ILogging _logging;
        IUPIIdVerification _UpiIdVerification;
        ICashfreePayoutCall _cashfreePayoutCall;
        IOptions<ApplicationSettings> _config;
        IDropdownManager _dropdownManager;
        #endregion

        public AcceptanceModel(Digi2l_DevContext context, ICustomerDetailsRepository customerDetailsRepository, ILogging logging, IWhatsAppMessageRepository whatsAppMessageRepository, IWhatsappNotificationManager whatsappNotificationManager, IExchangeOrderRepository exchangeOrderRepository, IBusinessUnitRepository businessUnitRepository, IOrderTransRepository orderTransRepository, ICashfreePayoutCall cashfreePayoutCall, IUPIIdVerification UpiIdVerification, IQCCommentManager QcCommentManager, IBusinessPartnerRepository basinessPartnerRepository, IDropdownManager dropdownManager, IOptions<ApplicationSettings> config, CustomDataProtection protector) : base(config, protector)
        {

            _context = context;
            _exchangeOrderRepository = exchangeOrderRepository;
            _basinessPartnerRepository = basinessPartnerRepository;
            _customerDetailsRepository = customerDetailsRepository;
            _whatsappNotificationManager = whatsappNotificationManager;
            _whatsAppMessageRepository = whatsAppMessageRepository;
            _businessUnitRepository = businessUnitRepository;
            _orderTransRepository = orderTransRepository;
            _UpiIdVerification = UpiIdVerification;
            _cashfreePayoutCall = cashfreePayoutCall;
            _QcCommentManager = QcCommentManager;
            _logging = logging;
            _config = config;
            _dropdownManager = dropdownManager; 

        }

        [BindProperty(SupportsGet = true)]
        public AcceptanceViewModel AcceptanceViewModel { get; set; }

        [BindProperty(SupportsGet = true)]
        public string InvalidName { get; set; }

        [BindProperty(SupportsGet = true)]
        public bool IsUPIQrRequired { get; set; }

        TblExchangeOrder tblExchangeOrder = new TblExchangeOrder();
        TblBusinessPartner tblBusinessPartner = new TblBusinessPartner();
        public IActionResult OnGet(string regdNo)
        {

            if (regdNo != null)
            {
                tblExchangeOrder = _exchangeOrderRepository.GetSingle(x => x.RegdNo == regdNo && x.IsActive == true);
                AcceptanceViewModel.RegdNo = regdNo;
                if (tblExchangeOrder != null)
                {
                    TblOrderTran tblOrderTran = _context.TblOrderTrans.Where(x => x.IsActive == true && x.RegdNo == tblExchangeOrder.RegdNo).FirstOrDefault();
                    TblOrderQc tblOrderQc = _context.TblOrderQcs.Where(x => x.IsActive == true && x.OrderTransId == tblOrderTran.OrderTransId).FirstOrDefault();

                    if (tblOrderQc?.QualityAfterQc != null)
                    {
                        //if (tblOrderQc.QualityAfterQc == "Excellent")
                        //{
                        //    item.ProductGrade = "Working - A";
                        //}
                        if (tblOrderQc.QualityAfterQc == "Good" || tblOrderQc.QualityAfterQc == "Working")
                        {
                            AcceptanceViewModel.PriceGrade = "Working - A";
                        }
                        if (tblOrderQc.QualityAfterQc == "Average" || tblOrderQc.QualityAfterQc == "Heavily Used")
                        {
                            AcceptanceViewModel.PriceGrade = "Heavily Used - B";
                        }
                        if (tblOrderQc.QualityAfterQc == "Not_Working" || tblOrderQc.QualityAfterQc == "Not Working" || tblOrderQc.QualityAfterQc == "Non Working")
                        {
                            AcceptanceViewModel.PriceGrade = "Not Working - C";
                        }
                        if (tblBusinessPartner != null)
                        {
                            tblBusinessPartner = _basinessPartnerRepository.GetSingle(x => x.BusinessUnitId == tblExchangeOrder.BusinessUnitId && x.BusinessPartnerId == tblExchangeOrder.BusinessPartnerId && x.IsActive == true);

                            if (tblBusinessPartner != null)
                            {
                                AcceptanceViewModel.UPIId = tblBusinessPartner.Upiid;
                            }
                        }

                    }
                }

                AcceptanceViewModel.PreferredPickupDate = tblExchangeOrder?.CreatedDate?.AddHours(72);
                AcceptanceViewModel.PreferredPickupTime = "10AM-12PM";
            }


            else
            {
                AcceptanceViewModel = new AcceptanceViewModel();
            }

            if (_loginSession == null)
            {
                return RedirectToPage("/Bulk_Order/Bulk_Upload");
            }
            else
            {
                return Page();
            }

        }

        #region AddBeneficiary for FlipKart added by Kranti       
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


        //To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {

            int result = 0;
            TblExchangeOrder? tblExchangeOrder = null;
            TblBusinessPartner? tblBusinessPartner = null;
            TblOrderTran? tblOrderTran = null;
            TblCustomerDetail? tblCustomerDetail = null;
            TblWhatsAppMessage? tblwhatsappmessage = null;
            UPIVerification root = new UPIVerification();
            CashfreeAuth cashfreeAuthCall = new CashfreeAuth();
            AddBeneficiary addBeneficiarry = new AddBeneficiary();
            string vpaValid = EnumHelper.DescriptionAttr(UPIResponseEnum.vpaValid);
            AddBeneficiaryResponse beneficiaryResponse = new AddBeneficiaryResponse();
            string subcode = Convert.ToInt32(CashfreeEnum.Succcess).ToString();
            var enableEvcAutoAllocation = _config.Value.EvcAutoAllocation;


            #region Manage UPI for Flipkart Bulk order added by Kranti
            if(AcceptanceViewModel.RegdNo != null)
            {
                tblExchangeOrder = _context.TblExchangeOrders.Where(x => x.RegdNo == AcceptanceViewModel.RegdNo && x.IsActive == true).FirstOrDefault();
                tblOrderTran = _orderTransRepository.GetOrderTransByRegdno(tblExchangeOrder.RegdNo);
                TblBusinessUnit businessUnit = _context.TblBusinessUnits.Where(x => x.BusinessUnitId == tblExchangeOrder.BusinessUnitId && x.IsActive == true).FirstOrDefault();
                if (businessUnit != null)
                {
                    if (businessUnit.IsBulkOrder == true)
                    {
                        TblBusinessPartner businessPartner = _basinessPartnerRepository.GetSingle(x => x.BusinessPartnerId == tblExchangeOrder.BusinessPartnerId && x.BusinessUnitId == tblExchangeOrder.BusinessUnitId && x.IsActive == true);
                        if (businessPartner != null && businessPartner.Upiid != null)
                        {
                            cashfreeAuthCall = _cashfreePayoutCall.CashFreeAuthCall();
                            if (cashfreeAuthCall.subCode == subcode)
                            {
                                UPINoViewModel UpiNoViewModel = new UPINoViewModel();
                                UpiNoViewModel.Regdno = tblExchangeOrder?.RegdNo;
                                UpiNoViewModel.UPIId = businessPartner?.Upiid;
                                UpiNoViewModel.PreferredPickupTime = 2.ToString();
                                DateTime? date1 = tblExchangeOrder?.CreatedDate?.AddHours(72);
                                UpiNoViewModel.PreferredPickupDate = date1.ToString();
                                root = _UpiIdVerification.CheckUpiId(tblExchangeOrder?.RegdNo, businessPartner.Upiid, cashfreeAuthCall.data.token);

                                if (root.subCode == subcode && root.data.accountExists == "YES")
                                {
                                    int? statusid = tblExchangeOrder.StatusId;
                                    if ((statusid == (int?)OrderStatusEnum.Waitingforcustapproval || statusid == (int?)OrderStatusEnum.QCByPass))
                                    {
                                        UpiNoViewModel.StatusId = statusid;
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
                                                _logging.WriteErrorToDB("AcceptanceModel", "OnPostAsync", e);

                                                if (enableEvcAutoAllocation == true)
                                                {
                                                    #region EVC Auto-Allocation Phase II
                                                    return RedirectToPage("/EVC_Allocation/AutoAllocation", new { orderTransId = tblOrderTran.OrderTransId, BuId = businessUnit.BusinessUnitId });
                                                    #endregion
                                                }
                                                else
                                                {
                                                    return RedirectToPage("/Bulk_Order/Bulk_Upload");
                                                }
                                            }
                                            else
                                            {
                                                string beneficiaryResponseMsg = beneficiaryResponse.message;
                                                ViewData["Message"] = beneficiaryResponseMsg;

                                                Exception e = new Exception(beneficiaryResponseMsg);
                                                _logging.WriteErrorToDB("AcceptanceModel", "OnPostAsync", e);
                                                return RedirectToPage("/Bulk_Order/Bulk_Upload");
                                            }
                                        }
                                        else
                                            return Page();

                                    }
                                }
                                else if (subcode == root.subCode && root.data.accountExists == "NO")
                                {
                                    var timeslot = _dropdownManager.GetTimeSlot();
                                    if (timeslot != null)
                                    {
                                        ViewData["timeslot"] = new SelectList(timeslot, "Value", "Text");
                                    }
                                    ViewData["Message"] = "Upi id is not valid";
                                    IsUPIQrRequired = _config.Value.IsUPIQrRequired;
                                    return Page();
                                }
                                else
                                {
                                    if (root.message != null && root.message.Contains("Please provide a valid Name"))
                                    {
                                        InvalidName = root.message;
                                        InvalidName = "Please provide a valid Name";
                                        ViewData["Message"] = "Please provide a valid Name";
                                        return Page();
                                    }

                                    var timeslot = _dropdownManager.GetTimeSlot();
                                    if (timeslot != null)
                                    {
                                        ViewData["timeslot"] = new SelectList(timeslot, "Value", "Text");
                                    }
                                    string invalidupiid = root.message;
                                    ViewData["Message"] = invalidupiid;
                                    IsUPIQrRequired = _config.Value.IsUPIQrRequired;
                                    return Page();
                                }
                            }
                        }

                    }
                    #endregion
                }
            }
            
            if(AcceptanceViewModel.RegdNo != null)
            {
                return RedirectToPage("/Bulk_Order/Bulk_Upload");

            }
            else
            {
                return Page();
            }
            
        }
       
    }
   
}


    