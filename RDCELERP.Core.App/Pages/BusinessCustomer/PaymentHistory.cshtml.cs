using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;
using RDCELERP.BAL.Interface;
using RDCELERP.Model.Base;
using RDCELERP.Core.App.Pages.Base;
using RDCELERP.Model.BusinessCustomer;

namespace RDCELERP.Core.App.Pages.BusinessCustomer
{
    public class PaymentHistoryModel : BasePageModel
    {
        private readonly IPaymentManager _paymentManager;
        public List<PaymentHistoryViewModel> PaymentList { get; set; }

        public PaymentHistoryModel(IPaymentManager paymentManager, IOptions<ApplicationSettings> config)
        : base(config)
        {

            _paymentManager = paymentManager;
        }
        public async Task<IActionResult> OnGetAsync()
        {
            int BusinessCustomerId = _loginSession.BusinessCustomerViewModel.BusinessCustomerId;
            PaymentList = await _paymentManager.GetCustomerPaymentsAsync(BusinessCustomerId);
            return Page();

        }
    }
}
