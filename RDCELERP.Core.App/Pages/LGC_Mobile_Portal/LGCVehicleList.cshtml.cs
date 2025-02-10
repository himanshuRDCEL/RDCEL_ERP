using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RestSharp;
using RDCELERP.BAL.Interface;
using RDCELERP.BAL.MasterManager;
using RDCELERP.Common.Constant;
using RDCELERP.Common.Enums;
using RDCELERP.Common.Helper;
using RDCELERP.Core.App.Pages.Base;
using RDCELERP.DAL.Entities;
using RDCELERP.DAL.IRepository;
using RDCELERP.Model;
using RDCELERP.Model.Base;
using RDCELERP.Model.SearchFilters;
using RDCELERP.Model.ServicePartner;
using static RDCELERP.Model.Whatsapp.WhatsappQCPriceViewModel;

namespace RDCELERP.Core.App.Pages.LGC_Mobile_Portal
{
    public class LGCVehicleListModel : BasePageModel
    {
        #region Variable Declaration
        ILogisticManager _logisticManager;
        ILogging _logging;

        #endregion

        #region Constructor
        public LGCVehicleListModel(IOptions<ApplicationSettings> config, ILogging logging, ILogisticManager logisticManager)
        : base(config)
        {
            _logging = logging;
            _logisticManager = logisticManager;
        }
        #endregion

        #region Bind Properties
        [BindProperty(SupportsGet = true)]
        public SearchFilterViewModel searchFilterVM { get; set; }

        [BindProperty(SupportsGet = true)]
        public ServicePartnerViewModel servicePartnerVM { get; set; }
        public int SPId { get; set; }
        #endregion
        public IActionResult OnGet(string? ServicePartnerId = null)
        {
            int userId = 0;
            SPId = 0;
            if (!string.IsNullOrEmpty(ServicePartnerId)) { SPId = Convert.ToInt32(ServicePartnerId); }
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
            //if (_loginSession != null)
            //{
            //    UserId = _loginSession.UserViewModel.UserId;
            //    servicePartnerVM = _logisticManager.GetServicePartnerByUserId(userId);
            //    if (servicePartnerVM == null)
            //    {
            //        servicePartnerVM = new ServicePartnerViewModel();
            //        servicePartnerVM.UserId = 0;
            //    }
            //}
            return Page();
        }

       /* #region Autopopulate Search Filter for search by RegdNo
        public IActionResult OnGetSearchRegdNo(string term)
        {
            if (term == null)
            {
                return BadRequest();
            }
            //string searchTerm = term.ToString();

            var data = _context.TblExchangeOrders
            .Where(x => x.RegdNo.Contains(term)
            && (x.StatusId == Convert.ToInt32(OrderStatusEnum.Waitingforcustapproval) || x.StatusId == Convert.ToInt32(OrderStatusEnum.QCByPass)))
            .Select(x => x.RegdNo)
            .ToArray();
            return new JsonResult(data);
        }
        #endregion*/
       
    }
}
