using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;
using RDCELERP.BAL.Interface;
using RDCELERP.Core.App.Pages.Base;
using RDCELERP.DAL.Entities;
using RDCELERP.DAL.IRepository;
using RDCELERP.Model.Base;
using RDCELERP.Model.EVC;
using RDCELERP.Model.LGC;

namespace RDCELERP.Core.App.Pages.LGC_Admin
{
    public class PickupDeclineListModel : BasePageModel
    {
        private readonly RDCELERP.DAL.Entities.Digi2l_DevContext _context;
        ILogisticsRepository _logisticsRepository;
        private readonly ILogisticManager _logisticManager;


        public PickupDeclineListModel(RDCELERP.DAL.Entities.Digi2l_DevContext context, IOptions<ApplicationSettings> config,ILoginRepository logisticsRepository,ILogisticManager logisticManager)
      : base(config)
        {

            _context = context;
            // _logisticsRepository = _logisticsRepository;
            _logisticManager = logisticManager;
        }
        [BindProperty(SupportsGet = true)]
        public int? ServiceId { get; set; }

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
        
         public IActionResult OnPostReOpenTicketAsync()
         {
            
            if (_loginSession == null)
            {
                return NotFound();
            }
            else
            {
                var Result = _logisticManager.ReOpenLGCOrder(pickupOrderViewModel.orderTransId, _loginSession.UserViewModel.UserId,pickupOrderViewModel.Comment);                    
            }
            return RedirectToPage("./PickupDeclineList/");
        }

    }
  
}

