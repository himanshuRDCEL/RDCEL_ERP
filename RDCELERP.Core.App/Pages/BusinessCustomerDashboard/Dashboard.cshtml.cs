using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;
using RDCELERP.BAL.Interface;
using RDCELERP.Model.Base;
using RDCELERP.Core.App.Pages.Base;
using RDCELERP.Model.DealerDashBoard;
using RDCELERP.Model.BusinessCustomer;
using Microsoft.AspNetCore.Mvc.Rendering;
using RDCELERP.DAL.Entities;
using static Org.BouncyCastle.Math.EC.ECCurve;
using RDCELERP.Common.Helper;

namespace RDCELERP.Core.App.Pages.BusinessCustomerDashboard
{
    public class IndexModel : BasePageModel
    {
        IBusinessCustomerDashboardManager _businessCustomerDashboardManager;
        public ILogging _logging;

        public IndexModel(ILogging logging,IBusinessCustomerDashboardManager businessCustomerDashboardManager,IOptions<ApplicationSettings> config)
      : base(config)
        {
            _businessCustomerDashboardManager = businessCustomerDashboardManager;
            _logging = logging;
        }
        [BindProperty(SupportsGet = true)]
        public DashboardViewModel DashboardViewModel { get; set; }
        public IActionResult OnGet()
        {
            try
            {
                if (_loginSession == null)
                {
                    return RedirectToPage("/index");
                }
                else
                {
                    DashboardViewModel = _businessCustomerDashboardManager.GetCustomerDashboardById(_loginSession.BusinessCustomerViewModel.BusinessCustomerId);
                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("DashBoardModel", "OnGet", ex);
            }
            return Page();
        }
    }
}
