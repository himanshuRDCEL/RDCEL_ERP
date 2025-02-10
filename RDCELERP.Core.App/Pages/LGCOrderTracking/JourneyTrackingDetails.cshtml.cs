using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RestSharp;
using RDCELERP.BAL.Helper;
using RDCELERP.BAL.Interface;
using RDCELERP.BAL.MasterManager;
using RDCELERP.Common.Constant;
using RDCELERP.Common.Enums;
using RDCELERP.Common.Helper;
using RDCELERP.Core.App.Pages.Base;
using RDCELERP.DAL.Entities;
using RDCELERP.DAL.IRepository;
using RDCELERP.DAL.Repository;
using RDCELERP.Model.Base;
using RDCELERP.Model.DriverDetails;
using RDCELERP.Model.ImagLabel;
using RDCELERP.Model.LGC;
using RDCELERP.Model.QC;
using RDCELERP.Model.ServicePartner;
using static RDCELERP.Model.Whatsapp.WhatsappLgcDropViewModel;

namespace RDCELERP.Core.App.Pages.LGCOrderTracking
{
    public class JourneyTrackingDetailsModel : BasePageModel
    {
        #region Variable declartion
        private readonly ILogging _logging;
        private readonly ILogisticManager _logisticManager;
        IDriverListRepository _driverListRepository;
        #endregion

        #region Constructor
        public JourneyTrackingDetailsModel(IOptions<ApplicationSettings> config, ILogisticManager logisticManager, ILogging logging, IDriverListRepository driverListRepository) : base(config)
        {
            _logisticManager = logisticManager;
            _logging = logging;
            _driverListRepository = driverListRepository;
        }
        #endregion

        [BindProperty(SupportsGet = true)]
        public LGCOrderViewModel lGCOrderViewModel { get; set; }
        [BindProperty(SupportsGet = true)]
        public ServicePartnerViewModel servicePartnerVM { get; set; }
        //public int SPId { get; set; }
        public IActionResult OnGet(string? TrackingId = null, string? ServicePartnerId = null)
        {
            int userId = 0; int servicePartnerId = 0; int driverDetailsId = 0; int trackingId = 0;
            lGCOrderViewModel = new LGCOrderViewModel();
            TblDriverList tblDriverList= new TblDriverList();
            try
            {
                if (!string.IsNullOrEmpty(ServicePartnerId)) { servicePartnerId = Convert.ToInt32(ServicePartnerId); }
                if (!string.IsNullOrEmpty(TrackingId)) { trackingId = Convert.ToInt32(TrackingId); }
               
                if (_loginSession != null)
                {
                    userId = _loginSession.UserViewModel.UserId;
                    servicePartnerVM = _logisticManager.GetServicePartnerByUserId(userId);
                    if (servicePartnerVM == null)
                    {
                        if (trackingId > 0)
                        {
                            servicePartnerVM = _logisticManager.GetSPDetailsByTrackingId(trackingId);
                        }
                        else if(servicePartnerId > 0)
                        {
                            servicePartnerVM = _logisticManager.GetSPDetailsById(servicePartnerId);
                        }
                        servicePartnerVM.UserId = 0;
                    }
                }
                if (servicePartnerVM == null)
                {
                    servicePartnerVM = new ServicePartnerViewModel();
                }
                lGCOrderViewModel.driverDetailsVM = _logisticManager.GetDriverDetailsByTrackingId(trackingId);
                if (lGCOrderViewModel.driverDetailsVM == null)
                {
                    lGCOrderViewModel.driverDetailsVM = new DriverDetailsViewModel();
                }
                else
                {
                    tblDriverList = _driverListRepository.GetDriverlistById(lGCOrderViewModel.driverDetailsVM.DriverId);
                    if (tblDriverList != null)
                    {
                        lGCOrderViewModel.driverDetailsVM.DriverName = tblDriverList.DriverName;
                        lGCOrderViewModel.driverDetailsVM.City = tblDriverList.City?.Name;
                        lGCOrderViewModel.driverDetailsVM.DriverPhoneNumber = tblDriverList.DriverPhoneNumber;
                        lGCOrderViewModel.driverDetailsVM.DriverlicenseNumber = tblDriverList.DriverLicenseNumber;

                    }
                }
                if (servicePartnerId > 0)
                {
                    lGCOrderViewModel.driverDetailsVM.ServicePartnerId = servicePartnerId;
                }
                else if (servicePartnerVM != null && servicePartnerVM.ServicePartnerId > 0)
                {
                    lGCOrderViewModel.driverDetailsVM.ServicePartnerId = servicePartnerVM.ServicePartnerId;
                }

                if (trackingId > 0)
                {
                    lGCOrderViewModel.TrackingId = trackingId;
                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("JourneyTrackingDetailsModel", "OnGet", ex);
            }
            
            return Page();
        }

        // Method for Store LGC Product Drop Images With POD Image and Invoice
        public IActionResult OnPostAsync()
        {
            return Page();    
        }
    }
}