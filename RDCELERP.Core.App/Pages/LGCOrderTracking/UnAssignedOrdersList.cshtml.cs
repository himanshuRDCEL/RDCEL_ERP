using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Options;
using Microsoft.Owin.Security.Provider;
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
using RDCELERP.Model.City;
using RDCELERP.Model.DriverDetails;
using RDCELERP.Model.MobileApplicationModel;
using RDCELERP.Model.MobileApplicationModel.LGC;
using RDCELERP.Model.QCComment;
using RDCELERP.Model.SearchFilters;
using RDCELERP.Model.ServicePartner;

namespace RDCELERP.Core.App.Pages.LGCOrderTracking
{
    public class UnAssignedOrdersListModel : BasePageModel
    {
        #region Variable Declaration
        ILogisticManager _logisticManager;
        ILogging _logging;
        IServicePartnerManager _servicePartnerManager;
        IServicePartnerRepository _servicePartnerRepository;
        IDriverDetailsManager _driverDetailsManager;
        #endregion

        #region Constructor
        public UnAssignedOrdersListModel(IOptions<ApplicationSettings> config, ILogging logging, ILogisticManager logisticManager, IServicePartnerManager servicePartnerManager, IDriverDetailsManager driverDetailsManager, IServicePartnerRepository servicePartnerRepository)
        : base(config)
        {
            _logging = logging;
            _logisticManager = logisticManager;
            _servicePartnerManager = servicePartnerManager;
            _driverDetailsManager = driverDetailsManager;
            _servicePartnerRepository = servicePartnerRepository;
        }
        #endregion

        #region Bind Properties
        [BindProperty(SupportsGet = true)]
        public SearchFilterViewModel searchFilterVM { get; set; }

        [BindProperty(SupportsGet = true)]
        public ServicePartnerViewModel servicePartnerVM { get; set; }
        public int SPId { get; set; }
        [BindProperty(SupportsGet = true)]
        public AssignOrderRequest assignRequestVM { get; set; }
        [BindProperty(SupportsGet = true)]
        public RejectLGCOrderRequest rejectLGCOrderVM { get; set; }
        public int? ActiveTabId { get; set; }
        [BindProperty(SupportsGet = true)]
        public string? OrderTransListString { get; set; }
        public bool? IsRejectedOrder { get; set; }
        #endregion
        public IActionResult OnGet(string? ServicePartnerId = null, int? TabId = null)
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
            
            if (TabId != null)
            {
                ActiveTabId = TabId;
            }
            else
            {
                ActiveTabId = 1;
            }
            return Page();
        }

        #region Assign Order to Driver
        public IActionResult OnPostAssignOrderToDriverAsync()
        {
            AssignOrderResponse assignOrderResponse = null;
            if (_loginSession == null)
            {
                return RedirectToPage("UnAssignedOrderList");
            }
            else
            {
                assignRequestVM.OrdertransId = new List<int>();
                if (!string.IsNullOrEmpty(OrderTransListString))
                {
                    var query = from val in OrderTransListString.Split(',')
                                select int.Parse(val);
                    if (query.Count() > 0) { assignRequestVM.OrdertransId.AddRange(query); }
                }
                
                assignOrderResponse = _servicePartnerManager.AssignOrdertoVehiclebyLGC(assignRequestVM);
                if (assignOrderResponse != null)
                {
                    if (assignOrderResponse.Status)
                    {

                    }
                    else
                    {

                    }
                }
            }
            return RedirectToPage("UnAssignedOrdersList");
        }
        #endregion

        #region Reject order by SP or Admin
        public IActionResult OnPostRejectOrderLGCAsync()
        {
            ResponseResult? responseResult = null;
            try
            {
                if (rejectLGCOrderVM != null)
                {
                    responseResult = _servicePartnerManager.RejectOrderbyLGC(rejectLGCOrderVM);
                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("UnAssignedOrdersListModel", "RejectOrderLGC", ex);
            }
            return RedirectToPage("UnAssignedOrdersList");
        }
        #endregion

        #region AutoComplete Dropdowns : Service Partner, Driver, Customer City
        public IActionResult OnGetSearchServicePartner(string term)
        {
            var data = Array.Empty<SelectListItem>();
            if (term == null)
            {
                return BadRequest();
            }
            try
            {
                data = _servicePartnerRepository.GetSPListByBusinessName(term)
                .Select(s => new SelectListItem
                {
                    Value = s.ServicePartnerName,
                    Text = s.ServicePartnerId.ToString()
                })
                .ToArray();
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("UnAssignedOrdersList", "OnGetSearchServicePartner", ex);
            }
            return new JsonResult(data);
        }
        public IActionResult OnGetSearchDriverList(string term, int ServicePartnerId, string? custCity)
        {
            ResponseResult? responseResult = null;
            List<DriverDetailsListByCityResponse>? driverList = null;
            int pageNumber=1;
            int pageSize=10;
            string filterBy=null;
            bool isJourneyPlannedForToday=true;

            //SelectListItem[] data = new SelectListItem();
            var myArray = Array.Empty<SelectListItem>();
            if (term == null)
            {
                return BadRequest();
            }
            try
            {
                responseResult = _driverDetailsManager.VehiclelistbyLGCIdandCityId(ServicePartnerId, custCity,isJourneyPlannedForToday,  pageNumber,  pageSize,  filterBy );
                if (responseResult != null && responseResult.Status && responseResult.Data != null)
                {
                    var jsonString = JsonConvert.SerializeObject(responseResult.Data);
                    driverList = JsonConvert.DeserializeObject<List<DriverDetailsListByCityResponse>>(jsonString);
                    if (driverList != null)
                    {
                        myArray = driverList.Where(x => (term == "#" || (x.DriverName ?? "").Contains(term)))
                              .Select(s => new SelectListItem
                              {
                                  Value = s.DriverName,
                                  Text = s.DriverDetailsId.ToString()
                              })
                          .ToArray();

                        // return new JsonResult(data);
                    }
                }
                else
                {
                    myArray = driverList.Select(s => new SelectListItem
                    {
                        Value = responseResult.message,
                        Text = '0'.ToString()
                    })
                          .ToArray();
                    //  return new JsonResult(data);
                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("UnAssignedOrdersList", "OnGetSearchDriverList", ex);
            }
            //var data = _context.TblServicePartners.Where(x => x.IsActive == true
            //&& x.ServicePartnerIsApprovrd == true && x.IsServicePartnerLocal != null
            //&& (term == "#" || x.ServicePartnerName.Contains(term)))
            //              .Select(s => new SelectListItem
            //              {
            //                  Value = s.ServicePartnerName,
            //                  Text = s.ServicePartnerId.ToString()
            //              })
            //          .ToArray();

            //var data = driverList.Select(s => new SelectListItem
            //              {
            //                  Value = responseResult.message,
            //                  Text = '0'.ToString()
            //              })
            //          .ToArray();
            //  return new JsonResult(data);
            return new JsonResult(myArray);
        }
        public IActionResult OnGetSearchCustCityList(string term, int ServicePartnerId)
        {
            ResponseResult? responseResult = null;
            AllOrderList? allOrderList = null;
            List<CityList>? cityLists = null;
            var myArray = Array.Empty<SelectListItem>();
            if (term == null)
            {
                return BadRequest();
            }
            try
            {
                responseResult = _servicePartnerManager.GetOrdercitylistbyLgcId(ServicePartnerId);
                if (responseResult != null && responseResult.Status && responseResult.Data != null)
                {
                    var jsonString = JsonConvert.SerializeObject(responseResult.Data);
                    cityLists = JsonConvert.DeserializeObject<List<CityList>>(jsonString);
                    if (cityLists != null && cityLists.Count > 0)
                    {
                        myArray = cityLists.Where(x => (term == "#" || (x.Name ?? "").Contains(term)))
                              .Select(s => new SelectListItem
                              {
                                  Value = s.Name,
                                  Text = s.CityId.ToString()
                              })
                          .ToArray();
                    }
                }
                else
                {
                    myArray = allOrderList?.AllOrderlistViewModels?.Select(s => new SelectListItem
                    {
                        Value = responseResult.message,
                        Text = '0'.ToString()
                    })
                          .ToArray();
                }
            }
            catch(Exception ex)
            {
                _logging.WriteErrorToDB("UnAssignedOrdersList", "OnGetSearchCustCityList", ex);
            }
            return new JsonResult(myArray);
        }
        #endregion
    }
}
