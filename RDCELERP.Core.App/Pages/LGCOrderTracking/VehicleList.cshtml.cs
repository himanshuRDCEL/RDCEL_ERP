using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;
using RDCELERP.BAL.Interface;
using RDCELERP.Common.Helper;
using RDCELERP.Core.App.Pages.Base;
using RDCELERP.Model.Base;
using RDCELERP.Model.ServicePartner;

namespace RDCELERP.Core.App.Pages.LGCOrderTracking
{
    public class VehicleListModel : BasePageModel
    {
        ILogisticManager _logisticManager;
        ILogging _logging;
        public VehicleListModel(IOptions<ApplicationSettings> baseConfig, ILogging logging, ILogisticManager logisticManager) : base(baseConfig)
        {
            _logging = logging;
            _logisticManager = logisticManager;
        }

        #region Bind Properties

        [BindProperty(SupportsGet = true)]
        public ServicePartnerViewModel servicePartnerVM { get; set; }
        public int SPId { get; set; }
        #endregion
        public IActionResult OnGet(string? ServicePartnerId = null)
        {
            int userId = 0;
            SPId = 0;
            if (!string.IsNullOrEmpty(ServicePartnerId))
            { SPId = Convert.ToInt32(ServicePartnerId); }
            else if (_loginSession != null)
            {
                userId = _loginSession.UserViewModel.UserId;
                servicePartnerVM = _logisticManager.GetServicePartnerByUserId(userId);
                if (servicePartnerVM != null)
                {
                    SPId = servicePartnerVM.ServicePartnerId;
                }
                else
                {
                    servicePartnerVM = new ServicePartnerViewModel();
                    servicePartnerVM.UserId = 0;
                }
            }
            return Page();
        }
    }
}
