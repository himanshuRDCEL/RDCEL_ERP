using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;
using Microsoft.VisualBasic;
using RDCELERP.BAL.Interface;
using RDCELERP.Core.App.Pages.Base;
using RDCELERP.DAL.Entities;
using RDCELERP.Model.Base;
using RDCELERP.Model.CashfreeModel;

namespace RDCELERP.Core.App.Pages.PayOut
{
    public class GetBeneficiaryDetailsModel : BasePageModel
    {
        private readonly Digi2l_DevContext _context;
        private readonly IOptions<ApplicationSettings> _config;
        private readonly ICashfreePayoutCall _cashfreePayoutCall;


        public GetBeneficiaryDetailsModel(IOptions<ApplicationSettings> config, Digi2l_DevContext context , ICashfreePayoutCall cashfreePayoutCall) : base(config)
        {
            _context = context;
            _config = config;
            _cashfreePayoutCall = cashfreePayoutCall;
        }

        [BindProperty(SupportsGet = true)]
        public BaneficiaryDetails BeneficiaryDetails { get; set; }
        string returnUrl = string.Empty;

        public IActionResult OnGet([FromQuery] string RegdNumber)
        {
            returnUrl = "PayOut/RePaymentRecords";
            // Check if the Beneficiary Already Exist Or not...
            CashfreeAuth cashfreeAuthCall = new CashfreeAuth();
            GetBeneficiary getBeneficiary = new GetBeneficiary();
            TblAbbregistration? AbbList = new TblAbbregistration();
            TblExchangeOrder? ExchangeList = new TblExchangeOrder();

            cashfreeAuthCall = _cashfreePayoutCall.CashFreeAuthCall();

            if (cashfreeAuthCall != null)
            {
                getBeneficiary = _cashfreePayoutCall.GetBeneficiary(cashfreeAuthCall?.data?.token, RegdNumber);
            }

            if (getBeneficiary.subCode == "200" && getBeneficiary?.data != null)
            {
                BeneficiaryDetails.beneId = getBeneficiary?.data?.beneId;
                BeneficiaryDetails.name = getBeneficiary?.data?.name;
                BeneficiaryDetails.address1 = getBeneficiary?.data?.address1;
                BeneficiaryDetails.address2 = getBeneficiary?.data?.address2;
                BeneficiaryDetails.pincode = getBeneficiary?.data?.pincode;
                BeneficiaryDetails.state = getBeneficiary?.data?.state;
                BeneficiaryDetails.city = getBeneficiary?.data?.city;
                BeneficiaryDetails.phone = getBeneficiary?.data?.phone;
                BeneficiaryDetails.email = getBeneficiary?.data?.email;
                BeneficiaryDetails.vpa = getBeneficiary?.data?.vpa;
            }

            else
            {

                //verify the Regd Number...
                if (RegdNumber.StartsWith('E'))
                {
                    ExchangeList = _context.TblExchangeOrders.Where(x => x.RegdNo == RegdNumber && x.IsActive == true).FirstOrDefault();
                }
                else
                {
                    AbbList = _context.TblAbbregistrations.Where(x => x.RegdNo == RegdNumber && x.IsActive == true).FirstOrDefault();
                }

                if (ExchangeList == null || AbbList == null) 
                {
                    var message = "No Record Found for this RegdNo : " + RegdNumber;
                    return Redirect(_config.Value.BaseURL + "PayOut/DetailsForFailedTransaction/?message= " + message + "&ReturnURL=" + returnUrl);
                }

                return Redirect(_config.Value.BaseURL + "PayOut/CreateBeneficiary?RegdNumber=" + RegdNumber);
            }

            return Page();
        }

        public IActionResult OnPost()
        {
            string? RegdNumber = BeneficiaryDetails?.beneId;
            string message = string.Empty;
            returnUrl = "PayOut/RePaymentRecords";
            CashfreeAuth cashfreeAuthCall = new CashfreeAuth();
            RemoveBeneficiary removeBeneficiary = new RemoveBeneficiary { beneId = RegdNumber };
            RemoveBeneficiaryResponse removeBeneficiaryResponse = new RemoveBeneficiaryResponse();  

            cashfreeAuthCall = _cashfreePayoutCall.CashFreeAuthCall();

            var orderTrans = _context.TblOrderTrans.Where(x => x.RegdNo == RegdNumber).FirstOrDefault();

            if (orderTrans != null && orderTrans.AmountPaidToCustomer != true)
            {
                if (cashfreeAuthCall != null)
                {
                    removeBeneficiaryResponse = _cashfreePayoutCall.RemoveBeneficiary(removeBeneficiary, cashfreeAuthCall?.data?.token);
                }
                if (removeBeneficiaryResponse != null && removeBeneficiaryResponse.subCode == "200")
                {
                    message = "Beneficiary Deleted Successfully for the ID " + RegdNumber;
                    return Redirect(_config.Value.BaseURL + "PayOut/Details/?message= " + message + "&ReturnURL=" + returnUrl);
                }
                message = "Beneficiary not deleted for the ID " + RegdNumber;
                return Redirect(_config.Value.BaseURL + "PayOut/DetailsForFailedTransaction/?message= " + message + "&ReturnURL=" + returnUrl);
            }
            else
            { 
                message = "Can not update Beneficiary UPI Id due to, the Amount Already paid to Customer";
                return Redirect(_config.Value.BaseURL + "PayOut/DetailsForFailedTransaction/?message= " + message + "&ReturnURL=" + returnUrl);
            }
        }
    }
}
