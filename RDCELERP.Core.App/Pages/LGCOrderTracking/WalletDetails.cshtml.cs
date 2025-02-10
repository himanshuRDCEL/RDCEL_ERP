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
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using NPOI.SS.Formula.Functions;
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
using RDCELERP.Model.Base;
using RDCELERP.Model.DriverDetails;
using RDCELERP.Model.ImagLabel;
using RDCELERP.Model.LGC;
using RDCELERP.Model.MobileApplicationModel;
using RDCELERP.Model.QC;
using RDCELERP.Model.ServicePartner;
using RDCELERP.Model.VehicleJourneyViewModel;
using static RDCELERP.Model.Whatsapp.WhatsappLgcDropViewModel;

namespace RDCELERP.Core.App.Pages.LGCOrderTracking
{
    public class WalletDetailsModel : BasePageModel
    {
        #region Variable declartion
        private readonly ILogging _logging;
        private readonly ILogisticManager _logisticManager;
        private readonly IServicePartnerManager _servicePartnerManager;
        #endregion

        #region Constructor
        public WalletDetailsModel(IOptions<ApplicationSettings> config, ILogisticManager logisticManager, ILogging logging, IServicePartnerManager servicePartnerManager) : base(config)
        {
            _logisticManager = logisticManager;
            _logging = logging;
            _servicePartnerManager = servicePartnerManager;
        }
        #endregion

        #region Bind Property
        [BindProperty(SupportsGet = true)]
        public LGCOrderViewModel lGCOrderViewModel { get; set; }
        [BindProperty(SupportsGet = true)]
        public ServicePartnerViewModel servicePartnerVM { get; set; }
        [BindProperty(SupportsGet = true)]
        public VehicleJourneyTrackDetailsModel VJTD_VM { get; set; }
        //public int SPId { get; set; }
        #endregion

        public IActionResult OnGet(int? TrackingDetailsId = null)
        {
            int userId = 0; int servicePartnerId = 0; int driverDetailsId = 0; int trackingId = 0;
            lGCOrderViewModel = new LGCOrderViewModel();
            ResponseResult responseMessage = new ResponseResult();
            responseMessage.message = string.Empty;
            try
            {
                if (TrackingDetailsId > 0)
                {
                    #region Get Service Partner and Order details and tracking details
                    if (_loginSession != null)
                    {
                        userId = _loginSession.UserViewModel.UserId;
                        servicePartnerVM = _logisticManager.GetServicePartnerByUserId(userId);
                        if (servicePartnerVM != null)
                        {
                            servicePartnerId = servicePartnerVM.ServicePartnerId;
                        }

                        #region Get Order Detail
                        responseMessage = _servicePartnerManager.GetDriverEarningDetailsById(TrackingDetailsId,servicePartnerId);
                        if (responseMessage != null && responseMessage.Status && responseMessage.Data != null)
                        {
                            var jsonString = JsonConvert.SerializeObject(responseMessage.Data);
                            VJTD_VM = JsonConvert.DeserializeObject<VehicleJourneyTrackDetailsModel>(jsonString);
                            if (VJTD_VM != null)
                            {
                                trackingId = VJTD_VM.TrackingId;
                            }
                        }
                        #endregion

                        #region Get Service Partner Details by tracking Id or ServicePartnerId
                        if (servicePartnerVM == null)
                        {
                            if (trackingId > 0)
                            {
                                servicePartnerVM = _logisticManager.GetSPDetailsByTrackingId(trackingId);
                                if (servicePartnerVM != null)
                                    servicePartnerId = servicePartnerVM.ServicePartnerId;
                            }
                            else if (servicePartnerId > 0)
                            {
                                servicePartnerVM = _logisticManager.GetSPDetailsById(servicePartnerId);
                            }
                        }
                        #endregion
                    }
                    #endregion

                    #region Get Driver Details
                    lGCOrderViewModel.driverDetailsVM = _logisticManager.GetDriverDetailsByTrackingId(trackingId);
                    if (lGCOrderViewModel.driverDetailsVM == null)
                    {
                        lGCOrderViewModel.driverDetailsVM = new DriverDetailsViewModel();
                    }
                    if (servicePartnerId > 0)
                    {
                        lGCOrderViewModel.driverDetailsVM.ServicePartnerId = servicePartnerId;
                    }
                    else if (servicePartnerVM != null && servicePartnerVM.ServicePartnerId > 0)
                    {
                        lGCOrderViewModel.driverDetailsVM.ServicePartnerId = servicePartnerVM.ServicePartnerId;
                    }
                    driverDetailsId = lGCOrderViewModel.driverDetailsVM.DriverDetailsId;
                    #endregion

                    if (servicePartnerVM == null)
                    {
                        servicePartnerVM = new ServicePartnerViewModel();
                    }
                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("WalletDetailsModel", "OnGet", ex);
            }
            return Page();
        }

        // Get multiple orders wallet details
        //public IActionResult OnGet(string? TrackingId = null, string? ServicePartnerId = null)
        //{
        //    int userId = 0; int servicePartnerId = 0; int driverDetailsId = 0; int trackingId = 0;
        //    lGCOrderViewModel = new LGCOrderViewModel();
        //    ResponseResult responseMessage = new ResponseResult();
        //    responseMessage.message = string.Empty;
        //    try
        //    {
        //        if (!string.IsNullOrEmpty(ServicePartnerId)) { servicePartnerId = Convert.ToInt32(ServicePartnerId); }
        //        if (!string.IsNullOrEmpty(TrackingId)) { trackingId = Convert.ToInt32(TrackingId); }
        //        //if (!string.IsNullOrEmpty(DriverDetailsId)) { driverDetailsId = Convert.ToInt32(DriverDetailsId); }

        //        #region Get Service Partner details
        //        if (_loginSession != null)
        //        {
        //            userId = _loginSession.UserViewModel.UserId;
        //            servicePartnerVM = _logisticManager.GetServicePartnerByUserId(userId);
        //            if (servicePartnerVM == null)
        //            {
        //                if (trackingId > 0)
        //                {
        //                    servicePartnerVM = _logisticManager.GetSPDetailsByTrackingId(trackingId);
        //                    if (servicePartnerVM != null)
        //                        servicePartnerId = servicePartnerVM.ServicePartnerId;
        //                }
        //                else if (servicePartnerId > 0)
        //                {
        //                    servicePartnerVM = _logisticManager.GetSPDetailsById(servicePartnerId);
        //                }
        //            }
        //        }
        //        if (servicePartnerVM == null)
        //        {
        //            servicePartnerVM = new ServicePartnerViewModel();
        //        }
        //        #endregion

        //        #region Get Driver Details
        //        lGCOrderViewModel.driverDetailsVM = _logisticManager.GetDriverDetailsByTrackingId(trackingId);
        //        if (lGCOrderViewModel.driverDetailsVM == null)
        //        {
        //            lGCOrderViewModel.driverDetailsVM = new DriverDetailsViewModel();
        //        }
        //        if (servicePartnerId > 0)
        //        {
        //            lGCOrderViewModel.driverDetailsVM.ServicePartnerId = servicePartnerId;
        //        }
        //        else if (servicePartnerVM != null && servicePartnerVM.ServicePartnerId > 0)
        //        {
        //            lGCOrderViewModel.driverDetailsVM.ServicePartnerId = servicePartnerVM.ServicePartnerId;
        //        }
        //        driverDetailsId = lGCOrderViewModel.driverDetailsVM.DriverDetailsId;
        //        #endregion

        //        #region Get Orders Detail
        //        if (trackingId > 0)
        //        {
        //            //VJTD_VM = 
        //            DateTime? today = DateTime.Now.Date;
        //            responseMessage = _servicePartnerManager.SPWalletSummeryList(today, servicePartnerVM.ServicePartnerId, driverDetailsId);
        //            lGCOrderViewModel.TrackingId = trackingId;
        //            if (responseMessage != null && responseMessage.Status && responseMessage.Data != null)
        //            {
        //                var jsonString = JsonConvert.SerializeObject(responseMessage.Data);
        //                //driverList = JsonConvert.DeserializeObject<List<DriverDetailsListByCityResponse>>(jsonString);
        //                //if (driverList != null)
        //                //{
        //                //    myArray = driverList.Where(x => (term == "#" || (x.DriverName ?? "").Contains(term)))
        //                //          .Select(s => new SelectListItem
        //                //          {
        //                //              Value = s.DriverName,
        //                //              Text = s.DriverDetailsId.ToString()
        //                //          })
        //                //      .ToArray();

        //                //    // return new JsonResult(data);
        //                //}
        //            }

        //        }
        //        #endregion

        //    }
        //    catch (Exception ex)
        //    {
        //        _logging.WriteErrorToDB("JourneyTrackingDetailsModel", "OnGet", ex);
        //    }
        //    return Page();
        //}

        // Method for Store LGC Product Drop Images With POD Image and Invoice
        public IActionResult OnPostAsync()
        {
            return Page();    
        }
    }
}