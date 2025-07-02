using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;
using RDCELERP.BAL.Interface;
using RDCELERP.Model.Base;
using RDCELERP.Core.App.Pages.Base;

namespace RDCELERP.Core.App.Pages.BusinessCustomer
{
    public class OrderHistoryModel : BasePageModel
    {
        #region Variable Declaration

        #endregion
        public int? BusinessCustomerId { get; set; }

        public OrderHistoryModel( IOptions<ApplicationSettings> config)
        : base(config)
        {

        }
        public void OnGet()
        {
            BusinessCustomerId=_loginSession.BusinessCustomerViewModel.BusinessCustomerId;

        }
    }
}
