using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;
using RDCELERP.BAL.Interface;
using RDCELERP.Core.App.Pages.Base;
using RDCELERP.DAL.Entities;
using RDCELERP.Model.Base;
using RDCELERP.Model.CashfreeModel;

namespace RDCELERP.Core.App.Pages.PayOut
{
    public class RePaymentRecords : CrudBasePageModel
    {
        #region Variable Declaration
             
        private readonly RDCELERP.DAL.Entities.Digi2l_DevContext _context;
        private readonly ICashfreePayoutCall _cashfreePayoutCall;
        #endregion

        public RePaymentRecords( Digi2l_DevContext context,ICashfreePayoutCall cashfreePayoutCall,  IOptions<ApplicationSettings> config, CustomDataProtection protector) : base(config, protector)
        {
            _context = context;
            _cashfreePayoutCall = cashfreePayoutCall;
        }

        [BindProperty(SupportsGet = true)]
        public ResponseData ResponseData { get; set; }

        public void OnGet()
        {
            CashfreeAuth cashfreeAuthCall = new CashfreeAuth();
            GetWalletBalanceResponse getWalletBalanceResponse = new GetWalletBalanceResponse();
            

            cashfreeAuthCall = _cashfreePayoutCall.CashFreeAuthCall();

            if (cashfreeAuthCall != null)
            {
                getWalletBalanceResponse = _cashfreePayoutCall.GetWalletBalance(cashfreeAuthCall?.data?.token);
            }
            if(getWalletBalanceResponse.subCode == "200" && getWalletBalanceResponse?.data != null) 
            {
                ResponseData.balance = getWalletBalanceResponse.data.balance;
                ResponseData.availableBalance = getWalletBalanceResponse.data.availableBalance;
            }
            else
            {
                var model = new ResponseData { availableBalance = "0.00", balance = "0.00" };
                ResponseData = model;
            }
        }
    }
}
