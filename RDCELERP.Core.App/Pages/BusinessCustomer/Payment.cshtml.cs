using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;
using RDCELERP.BAL.Interface;
using RDCELERP.Core.App.Pages.Base;
using RDCELERP.Model.RazorPay;
using RDCELERP.Model.Base;


namespace RDCELERP.Core.App.Pages.BusinessCustomer
{
    public class PaymentModel : BasePageModel
    {
        #region Variable Declaration
        private readonly IPaymentManager _paymentManager;
    
        #endregion

        public PaymentModel(IPaymentManager paymentManager, IOptions<ApplicationSettings> config)
        : base(config)
        {
           
            _paymentManager = paymentManager;
        }
        public void OnGet()
        {
        }

        [HttpPost]
        public async Task<IActionResult> OnPostSavePayment([FromBody] RazorpayPaymentViewModel model)
        {
            int result = 0;
            try
            {
               
                  result=   await _paymentManager.ManagePayment(model.PaymentId, model.OrderId,_loginSession.BusinessCustomerViewModel.BusinessCustomerId);

                if (result > 0)
                {
                    return new JsonResult(new { success = true, message = "Payment saved successfully." });
                }

                return new JsonResult(new { success = false, message = "Unable to fetch payment." })
                {
                    StatusCode = 400
                };
            }
            catch (Exception ex)
            {
                return new JsonResult(new { success = false, message = "An error occurred while saving the payment." })
                {
                    StatusCode = 500
                };
            }
        }

    }
}
