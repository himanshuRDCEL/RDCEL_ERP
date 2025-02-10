using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Options;
using RDCELERP.BAL.Interface;
using RDCELERP.BAL.MasterManager;
using RDCELERP.Core.App.Pages.Base;
using RDCELERP.DAL.Entities;
using RDCELERP.Model.Base;
using RDCELERP.Model.EVC;
using RDCELERP.Model.EVCdispute;
using RDCELERP.Model.LGC;
using RDCELERP.Model.TicketGenrateModel;

namespace RDCELERP.Core.App.Pages.LGC_Admin
{
    public class SelectLGCPatnerModel : BasePageModel
    {
        private readonly RDCELERP.DAL.Entities.Digi2l_DevContext _context;
        private readonly ILogisticManager _logisticManager;
        public SelectLGCPatnerModel(RDCELERP.DAL.Entities.Digi2l_DevContext context, IOptions<ApplicationSettings> config, ILogisticManager logisticManager)
       : base(config)
        {

            _context = context;
            _logisticManager = logisticManager;
        }
        [BindProperty(SupportsGet = true)]
        public ServicePartnerLogin ServicePartnerLogin { get; set; }

        [BindProperty(SupportsGet = true)]
        public ServicePartnerDashboardViewModel servicePartnerDashboardViewModel { get; set; }
        public void OnGet()
        {
           var ServicePartnerId = 0;
           servicePartnerDashboardViewModel = _logisticManager.servicePartnerDashboard(ServicePartnerId);
                      
            var SelectServiceP = _logisticManager.SelectServicePartner();
            if (SelectServiceP != null)
            {
                ViewData["SelectService"] = new SelectList(SelectServiceP, "ServicePartnerId", "ServicePartnerName");
            }
        }



       // this method use for Approved EVC and Genrete Role and Send ID/Password for EVC Mail
        public async Task<IActionResult> OnGetSelectedServicePerterAsync(int ServicePartnerId)
        {
            if (ServicePartnerId == null)
            {
                return NotFound();
            }
            else
            {
                return RedirectToPage("ServicePartnerDashboard", new { ServicePartnerId });
            }
        }

        public JsonResult OnGetSelectLGCPartnerAsync(int ServicePartnerId)
        {
            if (ServicePartnerId == null)
            {
                return new JsonResult(servicePartnerDashboardViewModel);
            }
            else
            {
                return new JsonResult(_logisticManager.servicePartnerDashboard(ServicePartnerId));
            }
        }

        //public IActionResult OnGetSearchLGCServicePartner(string term)
        //{
        //    if (term == null)
        //    {
        //        return BadRequest();
        //    }
        //    var data = _context.TblServicePartners.Where(x => x.IsActive == true && x.ServicePartnerIsApprovrd == true && x.IsServicePartnerLocal != null && x.ServicePartnerName.Contains(term))
        //                   .Select(s => new SelectListItem
        //                   {
        //                       Value = s.ServicePartnerName,
        //                       Text = s.ServicePartnerId.ToString()
        //                   })
        //               .ToArray();
        //    return new JsonResult(data);
        //}

        public IActionResult OnGetSearchLGCServicePartner(string term)
        {
            if (term == null)
            {
                return BadRequest();
            }
            var data = _context.TblServicePartners.Where(x => x.IsActive == true && x.ServicePartnerIsApprovrd == true && x.IsServicePartnerLocal != null && (x.ServicePartnerName.Contains(term) || term == "#"))
                           .Select(s => new SelectListItem
                           {
                               Value = s.ServicePartnerName,
                               Text = s.ServicePartnerId.ToString()
                           })
                       .ToArray();
            return new JsonResult(data);
        }

    }
}
