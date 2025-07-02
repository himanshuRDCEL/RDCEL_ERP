using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;
using RDCELERP.BAL.Interface;
using RDCELERP.Model.Base;
using RDCELERP.Core.App.Pages.Base;
using RDCELERP.Model.BusinessCustomer;

namespace RDCELERP.Core.App.Pages.BusinessCustomer
{
    public class OrderHistDetailModel : BasePageModel
    {
        #region Variable Declaration
        IItemBookingManager _bookingManager;
        #endregion
        public int? BusinessCustomerId { get; set; }
        public List<OrderHistoryViewModel> OrderHistoryList { get; set; }


        public OrderHistDetailModel(IOptions<ApplicationSettings> config, IItemBookingManager bookingManager)
        : base(config)
        {
            _bookingManager = bookingManager;
        }
        public async Task<IActionResult> OnGetAsync()
        {
            BusinessCustomerId = _loginSession.BusinessCustomerViewModel.BusinessCustomerId;
            OrderHistoryList = await _bookingManager.GetOrderHistoryAsync(Convert.ToInt32(BusinessCustomerId));
            return Page();

        }
    }
}
