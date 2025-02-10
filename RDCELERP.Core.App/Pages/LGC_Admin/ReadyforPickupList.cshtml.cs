using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;
using RDCELERP.BAL.Interface;
using RDCELERP.BAL.MasterManager;
using RDCELERP.Core.App.Pages.Base;
using RDCELERP.Model.Base;
using RDCELERP.Model.LGC;

namespace RDCELERP.Core.App.Pages.LGC_Admin
{
    public class ReadyforPickupListModel : BasePageModel
    {
        private readonly RDCELERP.DAL.Entities.Digi2l_DevContext _context;
        private readonly ILogisticManager _logisticManager;
        public ReadyforPickupListModel(RDCELERP.DAL.Entities.Digi2l_DevContext context, IOptions<ApplicationSettings> config, ILogisticManager logisticManager)
      : base(config)
        {
            _logisticManager = logisticManager;
            _context = context;
        }
        [BindProperty(SupportsGet = true)]
        public int?ServiceId{ get; set; }
        [BindProperty(SupportsGet = true)]
        public PickupOrderViewModel pickupOrderViewModel { get; set; }
        public void OnGet(int ServicePartnerId)
        {
            if (ServicePartnerId != null)
            {
                ServiceId = ServicePartnerId;
            }
            else
            {
                ServiceId = null;
            }
        }
        public IActionResult OnPostCancelTicketByUTCAsync()
        {

            if (_loginSession == null)
            {
                return NotFound();
            }
           else
           {
                var Result = _logisticManager.CancelTicketByUTC(pickupOrderViewModel.orderTransId, _loginSession.UserViewModel.UserId, pickupOrderViewModel.Comment);
           }
            return RedirectToPage("./ReadyforPickupList/");
        }
    }
}
