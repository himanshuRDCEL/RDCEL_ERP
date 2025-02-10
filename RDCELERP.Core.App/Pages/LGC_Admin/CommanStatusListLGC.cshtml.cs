using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Options;
using RDCELERP.BAL.Interface;
using RDCELERP.BAL.MasterManager;
using RDCELERP.Core.App.Pages.Base;
using RDCELERP.DAL.Entities;
using RDCELERP.Model;
using RDCELERP.Model.Base;
using RDCELERP.Model.LGC;

namespace RDCELERP.Core.App.Pages.LGC_Admin
{
    public class CommanStatusListLGCModel : BasePageModel
    {
        private readonly RDCELERP.DAL.Entities.Digi2l_DevContext _context;
        private readonly ILogisticManager _logisticManager;
        public CommanStatusListLGCModel(RDCELERP.DAL.Entities.Digi2l_DevContext context, IOptions<ApplicationSettings> config, ILogisticManager logisticManager)
      : base(config)
        {

            _context = context;
            _logisticManager = logisticManager;
        }
        [BindProperty(SupportsGet = true)]
        public int? ServiceId { get; set; }
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
            var statuscode = _logisticManager.GetExchangeOrderStatusByLGCDepartment();
            if (statuscode != null)
            {
                //ViewData["SelectStatusCode"] = new SelectList(statuscode, "Id", "StatusCode");

                ViewData["SelectStatusCode"] = new SelectList((from s in statuscode.ToList()
                                                           select new
                                                           {
                                                               Id = s.Id,
                                                               StatusCode = s.StatusCode + "-" + s.StatusDescription
                                                           }), "Id", "StatusCode", null);



            }



            //if (exchangeOrderStatuses != null && exchangeOrderStatuses.Count > 0)
            //{
            //    QCCommentViewModel.StatusList = exchangeOrderStatuses.Select(o => new SelectListItem
            //    {
            //        Text = o.StatusCode,
            //        Value = o.Id.ToString()
            //    }).ToList();
            //}


            if (ServicePartnerId > 0)
            {
                ServiceId = ServicePartnerId;
            }
            else
            {
                ServiceId = null;
            }
        }
    }
}
