using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;
using RDCELERP.BAL.Interface;
using RDCELERP.Model.Base;
using RDCELERP.Core.App.Pages.Base;

namespace RDCELERP.Core.App.Pages.BusinessCustomer
{
    public class CheckoutModel : BasePageModel
    {
        public CheckoutModel(IOptions<ApplicationSettings> config)
      : base(config)
        {

        }
        public void OnGet()
        {
        }
    }
}
