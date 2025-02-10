using AutoMapper;
using DocumentFormat.OpenXml.Drawing.Charts;
using DocumentFormat.OpenXml.InkML;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using NPOI.SS.Formula.Functions;
using System.Globalization;
using RDCELERP.Common.Enums;
using RDCELERP.Common.Helper;
using RDCELERP.DAL.Entities;
using RDCELERP.DAL.IRepository;
using RDCELERP.Model.Base;
using RDCELERP.Model.DriverDetails;
using RDCELERP.Model.ExchangeOrder;
using RDCELERP.Model.LGCMobileApp;
using RDCELERP.Model.VehicleJourneyViewModel;

namespace RDCELERP.Core.App.Controller
{

    [Route("api/[controller]/[action]")]
    [ApiController]
    public class LGCMobileAppController : ControllerBase
    {
        #region veriable decleration 
        private Digi2l_DevContext _context;
        private IMapper _mapper;
        private readonly IOptions<ApplicationSettings> _config;
        ICompanyRepository _companyRepository;
        IBusinessUnitRepository _businessUnitRepository;
        CustomDataProtection _protector;
        IUserRepository _userRepository;
        DateTime _currentDatetime = DateTime.Now.TrimMilliseconds();
        #endregion

        #region Controller 
        public LGCMobileAppController(IMapper mapper, Digi2l_DevContext context, IOptions<ApplicationSettings> config, ICompanyRepository companyRepository, CustomDataProtection protector, IUserRepository userRepository)
        {
            _context = context;
            _mapper = mapper;
            _config = config;
            _companyRepository = companyRepository;
            _protector = protector;
            _userRepository = userRepository;
        }
        #endregion

        #region Assign Order to Driver by Service partner list Added by Pooja Jatav
        [HttpPost]        
        public async Task<ActionResult> OrderAssignbySPlist(int companyId, int SPId, DateTime? orderStartDate, DateTime? orderEndDate, string? regdNo, string? driverphoneNo, string? servicepartnerName, string? driverName, string? vehicleno, string? driverCity, string? ticketnumber)
        {
            #region Variable declaration
            if (!string.IsNullOrWhiteSpace(regdNo) && regdNo != "null")
            { regdNo = regdNo.Trim().ToLower(); }
            else { regdNo = null; }

            if (!string.IsNullOrWhiteSpace(driverphoneNo) && driverphoneNo != "null")
            { driverphoneNo = driverphoneNo.Trim().ToLower(); }
            else { driverphoneNo = null; }

            if (!string.IsNullOrWhiteSpace(servicepartnerName) && servicepartnerName != "null")
            { servicepartnerName = servicepartnerName.Trim().ToLower(); }
            else { servicepartnerName = null; }

            if (!string.IsNullOrWhiteSpace(driverName) && driverName != "null")
            { driverName = driverName.Trim().ToLower(); }
            else { driverName = null; }

            if (!string.IsNullOrWhiteSpace(vehicleno) && vehicleno != "null")
            { vehicleno = vehicleno.Trim().ToLower(); }
            else { vehicleno = null; }

            if (!string.IsNullOrWhiteSpace(driverCity) && driverCity != "null")
            { driverCity = driverCity.Trim().ToLower(); }
            else { driverCity = null; }

            if (!string.IsNullOrWhiteSpace(ticketnumber) && ticketnumber != "null")
            { ticketnumber = ticketnumber.Trim().ToLower(); }
            else { ticketnumber = null; }

            int servicePartnerId = 0;


            List<TblLogistic>? TblLogistics = null;
            string URL = _config.Value.URLPrefixforProd;
            //string MVCURL = _config.Value.MVCBaseURLForExchangeInvoice;
            string InvoiceimagURL = string.Empty;
            TblServicePartner? tblServicePartner = null;
            TblDriverDetail? tblDriverDetail = null;
            TblCity? tblCity = null;
            List<MobileAppLogisticViewModel> MobileAppLogisticList = null;
            int count = 0;
            TblCompany? tblCompany = null;
            TblBusinessUnit? tblBusinessUnit = null;
            List<TblUserRole>? tblUserRole = null;
            #endregion
            try
            {
                #region Datatable form variables
                var draw = Request.Form["draw"].FirstOrDefault();
                var start = Request.Form["start"].FirstOrDefault();
                var length = Request.Form["length"].FirstOrDefault();
                var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
                var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();
                var searchValue = Request.Form["search[value]"].FirstOrDefault();
                int pageSize = length != null ? Convert.ToInt32(length) : 0;
                int skip = start != null ? Convert.ToInt32(start) : 0;
                int recordsTotal = 0;
                #endregion

                #region Advanced Filters Mapping
               
                if (orderStartDate != null && orderEndDate != null)
                {
                    orderStartDate = Convert.ToDateTime(orderStartDate).AddMinutes(-1);
                    orderEndDate = Convert.ToDateTime(orderEndDate).AddDays(1).AddSeconds(-1);
                }
                #endregion

                #region tblServicePartner Check
                tblServicePartner = _context.TblServicePartners.Where(x => x.IsActive == true && x.ServicePartnerId == SPId).FirstOrDefault();
                if (tblServicePartner != null)
                {
                    servicePartnerId = tblServicePartner.ServicePartnerId;
                }
                
                #endregion

                #region table object Initialization
                count = _context.TblLogistics
                               .Include(x => x.ServicePartner)
                               .Include(x => x.DriverDetails)
                               .Count(x => x.IsActive == true && x.DriverDetails != null
                               && ((companyId > 0 && companyId == 1007)
                               && (x.StatusId == Convert.ToInt32(OrderStatusEnum.VehicleAssignbyServicePartner))
                               && ((orderStartDate == null && orderEndDate == null) || (x.ModifiedDate >= orderStartDate && x.ModifiedDate <= orderEndDate))
                               && ((servicePartnerId == 0 || servicePartnerId == null) || (servicePartnerId > 0 && x.ServicePartnerId == servicePartnerId))
                               && (string.IsNullOrEmpty(servicepartnerName) || (x.ServicePartner != null && (x.ServicePartner.ServicePartnerBusinessName ?? "").Contains(servicepartnerName)))
                               && (string.IsNullOrEmpty(driverName) || (x.DriverDetails != null && (x.DriverDetails.DriverName ?? "").Contains(driverName)))
                               && (string.IsNullOrEmpty(vehicleno) || (x.DriverDetails != null && (x.DriverDetails.VehicleNumber ?? "").Contains(vehicleno)))
                               && (string.IsNullOrEmpty(ticketnumber) || (x.TicketNumber != null && (x.TicketNumber ?? "").Contains(ticketnumber)))
                               && (string.IsNullOrEmpty(driverCity) || (x.DriverDetails != null && (x.DriverDetails.City ?? "").ToLower() == driverCity))
                               && (string.IsNullOrEmpty(driverphoneNo) || (x.DriverDetails != null && (x.DriverDetails.DriverPhoneNumber ?? "").ToLower() == driverphoneNo))
                               && (string.IsNullOrEmpty(regdNo) || (x.RegdNo ?? "").ToLower() == regdNo)
                               ));
                if (count > 0)
                {
                    TblLogistics = await _context.TblLogistics
                               .Include(x => x.ServicePartner)
                               .Include(x => x.DriverDetails)
                               .Where(x => x.IsActive == true
                               && ((companyId > 0 && companyId == 1007)
                               && (x.StatusId == Convert.ToInt32(OrderStatusEnum.VehicleAssignbyServicePartner))
                               && ((orderStartDate == null && orderEndDate == null) || (x.ModifiedDate >= orderStartDate && x.ModifiedDate <= orderEndDate))
                               && ((servicePartnerId == 0 || servicePartnerId == null) || (servicePartnerId > 0 && x.ServicePartnerId == servicePartnerId))
                               && (string.IsNullOrEmpty(servicepartnerName) || (x.ServicePartner != null && (x.ServicePartner.ServicePartnerBusinessName ?? "").Contains(servicepartnerName)))
                               && (string.IsNullOrEmpty(driverName) || (x.DriverDetails != null && (x.DriverDetails.DriverName ?? "").Contains(driverName)))
                               && (string.IsNullOrEmpty(vehicleno) || (x.DriverDetails != null && (x.DriverDetails.VehicleNumber ?? "").Contains(vehicleno)))
                               && (string.IsNullOrEmpty(ticketnumber) || (x.TicketNumber != null && (x.TicketNumber ?? "").Contains(ticketnumber)))
                               && (string.IsNullOrEmpty(driverCity) || (x.DriverDetails != null && (x.DriverDetails.City ?? "").ToLower() == driverCity))
                               && (string.IsNullOrEmpty(driverphoneNo) || (x.DriverDetails != null && (x.DriverDetails.DriverPhoneNumber ?? "").ToLower() == driverphoneNo))
                               && (string.IsNullOrEmpty(regdNo) || (x.RegdNo ?? "").ToLower() == regdNo)
                               )).GroupBy(x => x.DriverDetailsId).Select(group => group.First()).ToListAsync();
                               //)).OrderByDescending(x => x.ModifiedDate).ThenByDescending(x => x.LogisticId).GroupBy(x => x.DriverDetailsId).Select(group => group.First()).ToListAsync();
                               //)).OrderByDescending(x => x.ModifiedDate).ThenByDescending(x => x.LogisticId).GroupBy(x => x.DriverDetailsId).Select(group => group.First()).Skip(skip).Take(pageSize).ToListAsync();
                    //)).OrderByDescending(x => x.ModifiedDate).ThenByDescending(x => x.LogisticId).Skip(skip).Take(pageSize).ToListAsync();                    
                    count = TblLogistics.Count();

                    recordsTotal = count;

                    TblLogistics = TblLogistics.OrderByDescending(x => x.ModifiedDate).ThenByDescending(x => x.LogisticId).Skip(skip).Take(pageSize).ToList();
                    //TblLogistics = TblLogistics.Skip(skip).Take(pageSize).ToList();
                }
                #endregion

                #region Data Initialization for Datatable from table to Model
                if (TblLogistics != null && TblLogistics.Count > 0)
                {
                    MobileAppLogisticList = _mapper.Map<List<TblLogistic>, List<MobileAppLogisticViewModel>>(TblLogistics);
                    string actionURL = string.Empty;

                    foreach (MobileAppLogisticViewModel item in MobileAppLogisticList)
                    {
                        item.LogisticId = item.LogisticId > 0 ? item.LogisticId : 0;
                        item.TicketNumber = item.TicketNumber != null ? item.TicketNumber.ToString() : string.Empty;
                        item.RegdNo = item.RegdNo != null ? item.RegdNo.ToString() : string.Empty;
                        item.ModifiedDate = item.ModifiedDate != null ? item.ModifiedDate : null;
                 
                        if (item.ServicePartnerId > 0)
                        {
                            tblServicePartner = _context.TblServicePartners.Where(x => x.IsActive == true && x.ServicePartnerId == item.ServicePartnerId).FirstOrDefault();
                            item.ServicePartnerBusinessName = tblServicePartner != null ? tblServicePartner.ServicePartnerBusinessName : null;
                        }
                        if (item.driverDetailsId > 0)
                        {
                            tblDriverDetail = _context.TblDriverDetails.Where(x => x.IsActive == true && x.DriverDetailsId == item.driverDetailsId).FirstOrDefault();
                            item.DriverName = tblDriverDetail != null ? tblDriverDetail.DriverName : null;
                            item.VehicleNo = tblDriverDetail != null ? tblDriverDetail.VehicleNumber : null;
                            item.DriverPhoneNo = tblDriverDetail != null ? tblDriverDetail.DriverPhoneNumber : null;
                            if (tblDriverDetail?.CityId > 0)
                            {
                                tblCity = _context.TblCities.Where(x => x.IsActive == true && x.CityId == tblDriverDetail.CityId).FirstOrDefault();
                                item.DriverCity = tblCity != null ? tblCity.Name : null;
                            }
                        }
                        actionURL = " <div class='actionbtns'>";
                        actionURL = actionURL + " <a class='btn btn-sm btn-primary' href='" + URL + "/Index1?orderTransId=" + item.OrderTransId + "' title='TimeLine'><i class='fa solid fa-clock-rotate-left'></i></a>&nbsp;";
                        actionURL += "<a href ='" + URL + "/LGCOrderTracking/LGCDriverDetails?DriverDetailsId=" + item.driverDetailsId + "' class='btn btn-primary btn-sm viewableWithAddPermission' data-bs-toggle='tooltip' data-bs-placement='top' title='View'><i class='fa-solid fa-eye'></i></a>&nbsp;";
                        actionURL += "</div>";
                        item.Action = actionURL;
                    }
                   
                }
                #endregion
                var data = MobileAppLogisticList;
                var jsonData = new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data };
                return Ok(jsonData);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        #endregion

        #region Order Accept By Driver list Added by Pooja Jatav 
        [HttpPost]
        public async Task<ActionResult> OrderAcceptbyDriverlist(int companyId, int SPId, DateTime? orderStartDate, DateTime? orderEndDate, string? regdNo, string? driverphoneNo, string? servicepartnerName, string? driverName, string? vehicleno, string? driverCity)
        {
            #region Variable declaration
            if (!string.IsNullOrWhiteSpace(regdNo) && regdNo != "null")
            { regdNo = regdNo.Trim().ToLower(); }
            else { regdNo = null; }

            if (!string.IsNullOrWhiteSpace(driverphoneNo) && driverphoneNo != "null")
            { driverphoneNo = driverphoneNo.Trim().ToLower(); }
            else { driverphoneNo = null; }

            if (!string.IsNullOrWhiteSpace(servicepartnerName) && servicepartnerName != "null")
            { servicepartnerName = servicepartnerName.Trim().ToLower(); }
            else { servicepartnerName = null; }

            if (!string.IsNullOrWhiteSpace(driverName) && driverName != "null")
            { driverName = driverName.Trim().ToLower(); }
            else { driverName = null; }

            if (!string.IsNullOrWhiteSpace(vehicleno) && vehicleno != "null")
            { vehicleno = vehicleno.Trim().ToLower(); }
            else { vehicleno = null; }

            if (!string.IsNullOrWhiteSpace(driverCity) && driverCity != "null")
            { driverCity = driverCity.Trim().ToLower(); }
            else { driverCity = null; }

            int servicePartnerId = 0;


            List<TblVehicleJourneyTrackingDetail>? tblVehicleJourneyTrackingDetail = null;
            string URL = _config.Value.URLPrefixforProd;
            string MVCURL = _config.Value.MVCBaseURLForExchangeInvoice;
            string InvoiceimagURL = string.Empty;
            TblServicePartner? tblServicePartner = null;
            TblDriverDetail? tblDriverDetail = null;
            TblCity? tblCity = null;
            TblOrderTran? tblOrderTran = null;
            List<VehiclesTrackingDetails> VehiclesTrackingDetailList = null;
            int count = 0;
            TblCompany? tblCompany = null;
            TblBusinessUnit? tblBusinessUnit = null;
            #endregion
            try
            {
                #region Datatable form variables
                var draw = Request.Form["draw"].FirstOrDefault();
                var start = Request.Form["start"].FirstOrDefault();
                var length = Request.Form["length"].FirstOrDefault();
                var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
                var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();
                var searchValue = Request.Form["search[value]"].FirstOrDefault();
                int pageSize = length != null ? Convert.ToInt32(length) : 0;
                int skip = start != null ? Convert.ToInt32(start) : 0;
                int recordsTotal = 0;
                #endregion

                #region Advanced Filters Mapping
                if (orderStartDate != null && orderEndDate != null)
                {
                    orderStartDate = Convert.ToDateTime(orderStartDate).AddMinutes(-1);
                    orderEndDate = Convert.ToDateTime(orderEndDate).AddDays(1).AddSeconds(-1);
                }
                #endregion

                #region tblServicePartner Check
                tblServicePartner = _context.TblServicePartners.Where(x => x.IsActive == true && x.ServicePartnerId == SPId).FirstOrDefault();
                if (tblServicePartner != null)
                {
                    servicePartnerId = tblServicePartner.ServicePartnerId;
                }
                #endregion

                #region table object Initialization
                count = _context.TblVehicleJourneyTrackingDetails
                               .Include(x => x.ServicePartner)
                               .Include(x => x.Driver)
                               .Include(x => x.OrderTrans)
                               .Count(x => x.IsActive == true
                               && ((companyId > 0 && companyId == 1007)
                               && (x.StatusId == Convert.ToInt32(OrderStatusEnum.OrderAcceptedbyVehicle))
                               && (x.PickupStartDatetime == null)
                               && ((servicePartnerId == 0 || servicePartnerId == null) || (servicePartnerId > 0 && x.ServicePartnerId == servicePartnerId))
                               && ((orderStartDate == null && orderEndDate == null) || (x.CreatedDate >= orderStartDate && x.CreatedDate <= orderEndDate))
                               && (string.IsNullOrEmpty(servicepartnerName) || (x.ServicePartner != null && x.ServicePartner.ServicePartnerBusinessName.Contains(servicepartnerName)))
                               && (string.IsNullOrEmpty(driverName) || (x.Driver != null && x.Driver.DriverName.Contains(driverName)))
                               && (string.IsNullOrEmpty(vehicleno) || (x.Driver != null && x.Driver.VehicleNumber.Contains(vehicleno)))
                               && (string.IsNullOrEmpty(driverCity) || (x.Driver != null && (x.Driver.City ?? "").ToLower() == driverCity))
                               && (string.IsNullOrEmpty(driverphoneNo) || (x.Driver != null && (x.Driver.DriverPhoneNumber ?? "").ToLower() == driverphoneNo))
                               && (string.IsNullOrEmpty(regdNo) || (x.OrderTrans.RegdNo ?? "").ToLower() == regdNo)
                               ));
                if (count > 0)
                {
                    tblVehicleJourneyTrackingDetail = await _context.TblVehicleJourneyTrackingDetails
                               .Include(x => x.ServicePartner)
                               .Include(x => x.Driver)
                               .Include(x => x.OrderTrans)
                               .Where(x => x.IsActive == true
                               && ((companyId > 0 && companyId == 1007)
                               && (x.StatusId == Convert.ToInt32(OrderStatusEnum.OrderAcceptedbyVehicle))
                               && (x.PickupStartDatetime == null)
                               && ((servicePartnerId == 0 || servicePartnerId == null) || (servicePartnerId > 0 && x.ServicePartnerId == servicePartnerId))
                               && ((orderStartDate == null && orderEndDate == null) || (x.CreatedDate >= orderStartDate && x.CreatedDate <= orderEndDate))
                               && (string.IsNullOrEmpty(servicepartnerName) || (x.ServicePartner != null && x.ServicePartner.ServicePartnerBusinessName.Contains(servicepartnerName)))
                               && (string.IsNullOrEmpty(driverName) || (x.Driver != null && x.Driver.DriverName.Contains(driverName)))
                               && (string.IsNullOrEmpty(vehicleno) || (x.Driver != null && x.Driver.VehicleNumber.Contains(vehicleno)))
                               && (string.IsNullOrEmpty(driverCity) || (x.Driver != null && (x.Driver.City ?? "").ToLower() == driverCity))
                               && (string.IsNullOrEmpty(driverphoneNo) || (x.Driver != null && (x.Driver.DriverPhoneNumber ?? "").ToLower() == driverphoneNo))
                               && (string.IsNullOrEmpty(regdNo) || (x.OrderTrans.RegdNo ?? "").ToLower() == regdNo)
                               )).GroupBy(x => x.DriverId).Select(group => group.First()).ToListAsync();
                             
                    count = tblVehicleJourneyTrackingDetail.Count();

                    recordsTotal = count;

                    tblVehicleJourneyTrackingDetail = tblVehicleJourneyTrackingDetail.OrderByDescending(x => x.CreatedDate).ThenByDescending(x => x.TrackingDetailsId).Skip(skip).Take(pageSize).ToList();
                   
                }
                #endregion

                #region Data Initialization for Datatable from table to Model
                if (tblVehicleJourneyTrackingDetail != null && tblVehicleJourneyTrackingDetail.Count > 0)
                {
                    VehiclesTrackingDetailList = _mapper.Map<List<TblVehicleJourneyTrackingDetail>, List<VehiclesTrackingDetails>>(tblVehicleJourneyTrackingDetail);
                    string actionURL = string.Empty;

                    foreach (VehiclesTrackingDetails item in VehiclesTrackingDetailList)
                    {
                        item.TrackingDetailsId = item.TrackingDetailsId > 0 ? item.TrackingDetailsId : 0;
                        item.ModifiedDate = item.ModifiedDate != null ? item.ModifiedDate : null;
                        item.orderAssignedDate = item.CreatedDate != null ? item.CreatedDate : null;

                        if (item.OrderTransId != null)
                        {
                            tblOrderTran = _context.TblOrderTrans.Where(x => x.IsActive == true && x.OrderTransId == item.OrderTransId).FirstOrDefault();
                            item.RegdNo = tblOrderTran != null ? (tblOrderTran.RegdNo != null ? tblOrderTran.RegdNo : null) : null;
                        }
                        if (item.ServicePartnerId != null)
                        {
                            tblServicePartner = _context.TblServicePartners.Where(x => x.IsActive == true && x.ServicePartnerId == item.ServicePartnerId).FirstOrDefault();
                            item.ServicePartnerBusinessName = tblServicePartner != null ? (tblServicePartner.ServicePartnerBusinessName != null ? tblServicePartner.ServicePartnerBusinessName : null) : null;
                        }
                        if (item.DriverId != null)
                        {
                            tblDriverDetail = _context.TblDriverDetails.Where(x => x.IsActive == true && x.DriverDetailsId == item.DriverId).FirstOrDefault();
                            item.DriverName = tblDriverDetail != null ? (tblDriverDetail.DriverName != null ? tblDriverDetail.DriverName : null) : null;
                            item.DriverPhoneNo = tblDriverDetail != null ? (tblDriverDetail.DriverPhoneNumber != null ? tblDriverDetail.DriverPhoneNumber : null) : null;
                            item.VehicleNo = tblDriverDetail != null ? (tblDriverDetail.VehicleNumber != null ? tblDriverDetail.VehicleNumber : null) : null;
                            if (tblDriverDetail != null && tblDriverDetail.CityId > 0)
                            {
                                tblCity = _context.TblCities.Where(x => x.IsActive == true && x.CityId == tblDriverDetail.CityId).FirstOrDefault();
                                item.DriverCity = tblCity != null ? tblCity.Name : null;
                            }
                        }

                        actionURL = " <div class='actionbtns'>";
                        actionURL = actionURL + " <a class='btn btn-sm btn-primary' href='" + URL + "/Index1?orderTransId=" + item.OrderTransId + "' title='TimeLine'><i class='fa solid fa-clock-rotate-left'></i></a>&nbsp;";
                        actionURL += "<a href ='" + URL + "/LGCOrderTracking/LGCDriverDetails?DriverDetailsId=" + item.DriverId + "&StatusId="+ item.StatusId + "' class='btn btn-primary btn-sm viewableWithAddPermission' data-bs-toggle='tooltip' data-bs-placement='top' title='View'><i class='fa-solid fa-eye'></i></a>&nbsp;";
                        actionURL += "</div>";
                        item.Action = actionURL;

                    }
                }
                #endregion
                var data = VehiclesTrackingDetailList;
                var jsonData = new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data };
                return Ok(jsonData);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        #endregion        

        #region Order Reject By Driver list Added by Pooja Jatav
        [HttpPost]
        public async Task<ActionResult> OrderRejectbyDriverlist(int companyId, int SPId, DateTime? orderStartDate, DateTime? orderEndDate, string? regdNo, string? driverphoneNo, string? servicepartnerName, string? driverName, string? vehicleno, string? driverCity, string? ticketnumber)
        {
            #region Variable declaration
            if (!string.IsNullOrWhiteSpace(regdNo) && regdNo != "null")
            { regdNo = regdNo.Trim().ToLower(); }
            else { regdNo = null; }

            if (!string.IsNullOrWhiteSpace(driverphoneNo) && driverphoneNo != "null")
            { driverphoneNo = driverphoneNo.Trim().ToLower(); }
            else { driverphoneNo = null; }

            if (!string.IsNullOrWhiteSpace(servicepartnerName) && servicepartnerName != "null")
            { servicepartnerName = servicepartnerName.Trim().ToLower(); }
            else { servicepartnerName = null; }

            if (!string.IsNullOrWhiteSpace(driverName) && driverName != "null")
            { driverName = driverName.Trim().ToLower(); }
            else { driverName = null; }

            if (!string.IsNullOrWhiteSpace(vehicleno) && vehicleno != "null")
            { vehicleno = vehicleno.Trim().ToLower(); }
            else { vehicleno = null; }

            if (!string.IsNullOrWhiteSpace(driverCity) && driverCity != "null")
            { driverCity = driverCity.Trim().ToLower(); }
            else { driverCity = null; }

            //if (!string.IsNullOrWhiteSpace(ticketnumber) && ticketnumber != "null")
            //{ ticketnumber = ticketnumber.Trim().ToLower(); }
            //else { ticketnumber = null; }

            int servicePartnerId = 0;


            List<TblLogistic>? TblLogistics = null;
            string URL = _config.Value.URLPrefixforProd;
            string MVCURL = _config.Value.MVCBaseURLForExchangeInvoice;
            string InvoiceimagURL = string.Empty;
            TblServicePartner? tblServicePartner = null;
            TblDriverDetail? tblDriverDetail = null;
            TblCity? tblCity = null;
            List<MobileAppLogisticViewModel> MobileAppLogisticList = null;
            int count = 0;
            TblCompany? tblCompany = null;
            TblBusinessUnit? tblBusinessUnit = null;
            string actionURLCheckBox = string.Empty;
            #endregion
            try
            {
                #region Datatable form variables
                var draw = Request.Form["draw"].FirstOrDefault();
                var start = Request.Form["start"].FirstOrDefault();
                var length = Request.Form["length"].FirstOrDefault();
                var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
                var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();
                var searchValue = Request.Form["search[value]"].FirstOrDefault();
                int pageSize = length != null ? Convert.ToInt32(length) : 0;
                int skip = start != null ? Convert.ToInt32(start) : 0;
                int recordsTotal = 0;
                #endregion

                #region Advanced Filters Mapping                
                if (orderStartDate != null && orderEndDate != null)
                {
                    orderStartDate = Convert.ToDateTime(orderStartDate).AddMinutes(-1);
                    orderEndDate = Convert.ToDateTime(orderEndDate).AddDays(1).AddSeconds(-1);
                }
                #endregion

                #region tblServicePartner Check
                tblServicePartner = _context.TblServicePartners.Where(x => x.IsActive == true && x.ServicePartnerId == SPId).FirstOrDefault();
                if (tblServicePartner != null)
                {
                    servicePartnerId = tblServicePartner.ServicePartnerId;
                }
                #endregion

                #region table object Initialization
                count = _context.TblLogistics
                               .Include(x => x.ServicePartner)
                               .Include(x => x.DriverDetails)
                               .Count(x => x.IsActive == true
                               && ((companyId > 0 && companyId == 1007)
                               && (x.IsOrderRejectedByDriver == true) 
                               && (x.StatusId == Convert.ToInt32(OrderStatusEnum.OrderRejectedbyVehicle))
                               && ((servicePartnerId == 0 || servicePartnerId == null) || (servicePartnerId > 0 && x.ServicePartnerId == servicePartnerId))
                               && ((orderStartDate == null && orderEndDate == null) || (x.ModifiedDate >= orderStartDate && x.ModifiedDate <= orderEndDate))

                               && (string.IsNullOrEmpty(servicepartnerName) || (x.ServicePartner != null && (x.ServicePartner.ServicePartnerBusinessName??"").Contains(servicepartnerName)))
                               && (string.IsNullOrEmpty(driverName) || (x.DriverDetails != null && (x.DriverDetails.DriverName ?? "").Contains(driverName)))
                               && (string.IsNullOrEmpty(vehicleno) || (x.DriverDetails != null && (x.DriverDetails.VehicleNumber??"").Contains(vehicleno)))
                               && (string.IsNullOrEmpty(ticketnumber) || (x.TicketNumber != null && (x.TicketNumber?? "").Contains(ticketnumber)))

                               && (string.IsNullOrEmpty(driverCity) || (x.DriverDetails != null && (x.DriverDetails.City ?? "").ToLower() == driverCity))
                               && (string.IsNullOrEmpty(driverphoneNo) || (x.DriverDetails != null && (x.DriverDetails.DriverPhoneNumber ?? "").ToLower() == driverphoneNo))
                               && (string.IsNullOrEmpty(regdNo) || (x.RegdNo ?? "").ToLower() == regdNo)
                               ));
                if (count > 0)
                {
                    TblLogistics = await _context.TblLogistics
                               .Include(x => x.ServicePartner)
                               .Include(x => x.DriverDetails)
                               .Where(x => x.IsActive == true
                               && ((companyId > 0 && companyId == 1007)
                               && (x.IsOrderRejectedByDriver == true)
                               && (x.StatusId == Convert.ToInt32(OrderStatusEnum.OrderRejectedbyVehicle))
                               && ((servicePartnerId == 0 || servicePartnerId == null) || (servicePartnerId > 0 && x.ServicePartnerId == servicePartnerId))
                               && ((orderStartDate == null && orderEndDate == null) || (x.ModifiedDate >= orderStartDate && x.ModifiedDate <= orderEndDate))

                               && (string.IsNullOrEmpty(servicepartnerName) || (x.ServicePartner != null && (x.ServicePartner.ServicePartnerBusinessName ?? "").Contains(servicepartnerName)))
                               && (string.IsNullOrEmpty(driverName) || (x.DriverDetails != null && (x.DriverDetails.DriverName ?? "").Contains(driverName)))
                               && (string.IsNullOrEmpty(vehicleno) || (x.DriverDetails != null && (x.DriverDetails.VehicleNumber ?? "").Contains(vehicleno)))
                               && (string.IsNullOrEmpty(ticketnumber) || (x.TicketNumber != null && (x.TicketNumber?? "").Contains(ticketnumber)))

                               && (string.IsNullOrEmpty(driverCity) || (x.DriverDetails != null && (x.DriverDetails.City ?? "").ToLower() == driverCity))
                               && (string.IsNullOrEmpty(driverphoneNo) || (x.DriverDetails != null && (x.DriverDetails.DriverPhoneNumber ?? "").ToLower() == driverphoneNo))
                               && (string.IsNullOrEmpty(regdNo) || (x.RegdNo ?? "").ToLower() == regdNo)
                               )).GroupBy(x => x.DriverDetailsId).Select(group => group.First()).ToListAsync();
                    count = TblLogistics.Count();

                    recordsTotal = count;

                    TblLogistics = TblLogistics.OrderByDescending(x => x.ModifiedDate).ThenByDescending(x => x.LogisticId).Skip(skip).Take(pageSize).ToList();
                    
                }
                #endregion

                #region Data Initialization for Datatable from table to Model
                if (TblLogistics != null && TblLogistics.Count > 0)
                {
                    MobileAppLogisticList = _mapper.Map<List<TblLogistic>, List<MobileAppLogisticViewModel>>(TblLogistics);
                    string actionURL = string.Empty;

                    foreach (MobileAppLogisticViewModel item in MobileAppLogisticList)
                    {
                        item.LogisticId = item.LogisticId > 0 ? item.LogisticId : 0;
                        //item.TicketNumber = item.TicketNumber != null ? item.TicketNumber.ToString() : string.Empty;
                        item.RegdNo = item.RegdNo != null ? item.RegdNo.ToString() : string.Empty;
                        item.ModifiedDate = item.ModifiedDate != null ? item.ModifiedDate : null;

                        if (item.ServicePartnerId > 0)
                        {
                            tblServicePartner = _context.TblServicePartners.Where(x => x.IsActive == true && x.ServicePartnerId == item.ServicePartnerId).FirstOrDefault();
                            item.ServicePartnerBusinessName = tblServicePartner != null ? tblServicePartner.ServicePartnerBusinessName : null;
                        }
                        if (item.driverDetailsId > 0)
                        {
                            tblDriverDetail = _context.TblDriverDetails.Where(x => x.IsActive == true && x.DriverDetailsId == item.driverDetailsId).FirstOrDefault();
                            item.DriverName = tblDriverDetail != null ? tblDriverDetail.DriverName : null;
                            item.VehicleNo = tblDriverDetail != null ? tblDriverDetail.VehicleNumber : null;
                            item.DriverPhoneNo = tblDriverDetail != null ? tblDriverDetail.DriverPhoneNumber : null;
                            if (tblDriverDetail?.CityId > 0)
                            {
                                tblCity = _context.TblCities.Where(x => x.IsActive == true && x.CityId == tblDriverDetail.CityId).FirstOrDefault();
                                item.DriverCity = tblCity != null ? tblCity.Name : null;
                            }
                        }

                        actionURLCheckBox = " <td class='actions'>";
                        actionURLCheckBox = actionURLCheckBox + " <span><input type='checkbox' id=" + item.OrderTransId + " name ='orders'  value ='" + item.OrderTransId + "'   onclick='OnCheckBoxCheck();' class='checkboxinput' /></span>";
                        actionURLCheckBox = actionURLCheckBox + " </td>";
                        item.Edit = actionURLCheckBox;

                        actionURL = " <div class='actionbtns'>";
                        actionURL = actionURL + " <a class='btn btn-sm btn-primary' href='" + URL + "/Index1?orderTransId=" + item.OrderTransId + "' title='TimeLine'><i class='fa solid fa-clock-rotate-left'></i></a>&nbsp;";
                        actionURL = actionURL + "<a href ='" + URL + "/LGC_Admin/OrderViewPage?OrderTransId=" + item.OrderTransId + "' onclick='RecordView(" + item.OrderTransId + ")' class='btn btn-primary btn-sm viewableWithAddPermission' data-bs-toggle='tooltip' data-bs-placement='top' title='View'><i class='fa-solid fa-eye'></i></a>&nbsp;";
                        //actionURL += "<a href ='" + URL + "/LGCOrderTracking/LGCDriverDetails?DriverDetailsId=" + item.driverDetailsId + "&StatusId=" + item.StatusId + "' class='btn btn-primary btn-sm viewableWithAddPermission' data-bs-toggle='tooltip' data-bs-placement='top' title='View'><i class='fa-solid fa-eye'></i></a>&nbsp;";
                        actionURL += "</div>";
                        item.Action = actionURL;

                    }
                }
                #endregion
                var data = MobileAppLogisticList;
                var jsonData = new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data };
                return Ok(jsonData);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        #endregion

        #region Start Journey list Added by Pooja Jatav
        [HttpPost]
        public async Task<ActionResult> StartJourneyList(int companyId, int SPId, DateTime? orderStartDate, DateTime? orderEndDate, string? driverphoneNo, string? servicepartnerName, string? driverName, string? vehicleno, string? driverCity, DateTime? journeystartdate, DateTime? journeyplandate)
        {
            #region Variable declaration
            //if (!string.IsNullOrWhiteSpace(regdNo) && regdNo != "null")JourneyStartDate
            //{ regdNo = regdNo.Trim().ToLower(); }
            //else { regdNo = null; }
            if (!string.IsNullOrWhiteSpace(driverphoneNo) && driverphoneNo != "null")
            { driverphoneNo = driverphoneNo.Trim().ToLower(); }
            else { driverphoneNo = null; }

            if (!string.IsNullOrWhiteSpace(servicepartnerName) && servicepartnerName != "null")
            { servicepartnerName = servicepartnerName.Trim().ToLower(); }
            else { servicepartnerName = null; }

            if (!string.IsNullOrWhiteSpace(driverName) && driverName != "null")
            { driverName = driverName.Trim().ToLower(); }
            else { driverName = null; }

            if (!string.IsNullOrWhiteSpace(vehicleno) && vehicleno != "null")
            { vehicleno = vehicleno.Trim().ToLower(); }
            else { vehicleno = null; }

            if (!string.IsNullOrWhiteSpace(driverCity) && driverCity != "null")
            { driverCity = driverCity.Trim().ToLower(); }
            else { driverCity = null; }
            DateTime? journeyStartDate = null;
            DateTime? journeyEndDate = null;
            DateTime? journeyPlanStartDate = null;
            DateTime? journeyPlanEndDate = null;

            int servicePartnerId = 0;

            List<TblVehicleJourneyTracking>? TblVehicleJourneyTracking = null;
            string URL = _config.Value.URLPrefixforProd;
            TblServicePartner? tblServicePartner = null;
            TblDriverDetail? tblDriverDetail = null;
            TblCity? tblCity = null;
            List<StartvehicleJourneyViewModel> StartvehicleJourneyList = null;
            int count = 0;
            //TblCompany? tblCompany = null;
            //TblBusinessUnit? tblBusinessUnit = null;
            //TblUser? tblUser = null;
            TblVehicleJourneyTrackingDetail? tblVehicleJourneyTrackingDetail = null;
            TblOrderTran tblOrderTran = null;
            #endregion
            try
            {
                #region Datatable form variables
                var draw = Request.Form["draw"].FirstOrDefault();
                var start = Request.Form["start"].FirstOrDefault();
                var length = Request.Form["length"].FirstOrDefault();
                var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
                var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();
                var searchValue = Request.Form["search[value]"].FirstOrDefault();
                int pageSize = length != null ? Convert.ToInt32(length) : 0;
                int skip = start != null ? Convert.ToInt32(start) : 0;
                int recordsTotal = 0;
                #endregion

                #region Advanced Filters Mapping               
                if (orderStartDate != null && orderEndDate != null)
                {
                    orderStartDate = Convert.ToDateTime(orderStartDate).AddMinutes(-1);
                    orderEndDate = Convert.ToDateTime(orderEndDate).AddDays(1).AddSeconds(-1);
                }
                if (journeystartdate != null)
                {
                    journeyStartDate = Convert.ToDateTime(journeystartdate).AddMinutes(-1);
                    journeyEndDate = Convert.ToDateTime(journeystartdate).AddDays(1).AddSeconds(-1);
                }
                else
                {
                    var CurrentDate = _currentDatetime.ToString("MM/dd/yyyy");
                    journeyStartDate = Convert.ToDateTime(CurrentDate).AddMinutes(-1);
                    journeyEndDate = Convert.ToDateTime(CurrentDate).AddDays(1).AddSeconds(-1);
                }
                if (journeyplandate != null)
                {
                    journeyPlanStartDate = Convert.ToDateTime(journeyplandate).AddMinutes(-1);
                    journeyPlanEndDate = Convert.ToDateTime(journeyplandate).AddDays(1).AddSeconds(-1);
                }
                #endregion

                #region tblServicePartner Check
                tblServicePartner = _context.TblServicePartners.Where(x => x.IsActive == true && x.ServicePartnerId == SPId).FirstOrDefault();
                if (tblServicePartner != null)
                {
                    servicePartnerId = tblServicePartner.ServicePartnerId;
                }
                #endregion

                #region table object Initialization
                count = _context.TblVehicleJourneyTrackings
                               .Include(x => x.ServicePartner)
                               .Include(x => x.Driver)
                               .Include(x => x.TblVehicleJourneyTrackingDetails)
                               .Count(x => x.IsActive == true                                                              
                               && ((companyId > 0 && companyId == 1007)                               
                               && (x.JourneyStartDatetime != null)
                               && ((((journeyPlanStartDate == null && journeyPlanEndDate == null)) && (x.JourneyStartDatetime >= journeyStartDate && x.JourneyStartDatetime <= journeyEndDate))
                               || ((journeyPlanStartDate != null && journeyPlanEndDate != null) && (x.JourneyPlanDate >= journeyPlanStartDate && x.JourneyPlanDate <= journeyPlanEndDate)))
                               && ((servicePartnerId == 0 || servicePartnerId == null) || (servicePartnerId > 0 && x.ServicePartnerId == servicePartnerId))
                               && ((orderStartDate == null && orderEndDate == null) || (x.ModifiedDate >= orderStartDate && x.ModifiedDate <= orderEndDate))
                               //&& ((journeyStartDate == null && journeyEndDate == null) || (x.JourneyStartDatetime >= journeyStartDate && x.JourneyStartDatetime <= journeyEndDate))
                               && (string.IsNullOrEmpty(servicepartnerName) || (x.ServicePartner != null && x.ServicePartner.ServicePartnerBusinessName.Contains(servicepartnerName)))
                               && (string.IsNullOrEmpty(driverName) || (x.Driver != null && x.Driver.DriverName.Contains(driverName)))
                               && (string.IsNullOrEmpty(vehicleno) || (x.Driver != null && x.Driver.VehicleNumber.Contains(vehicleno)))
                               && (string.IsNullOrEmpty(driverCity) || (x.Driver != null && (x.Driver.City ?? "").ToLower() == driverCity))
                               && (string.IsNullOrEmpty(driverphoneNo) || (x.Driver != null && (x.Driver.DriverPhoneNumber ?? "").ToLower() == driverphoneNo))
                               //&& (string.IsNullOrEmpty(regdNo)) || x.TblVehicleJourneyTrackingDetails.Any(d => d.OrderTrans != null && (d.OrderTrans.RegdNo.Contains(regdNo)))                               
                               ));
                 if (count > 0)
                {
                    TblVehicleJourneyTracking = await _context.TblVehicleJourneyTrackings
                               .Include(x => x.ServicePartner)
                               .Include(x => x.Driver)
                               .Include(x => x.TblVehicleJourneyTrackingDetails)
                               .Where(x => x.IsActive == true
                               && ((companyId > 0 && companyId == 1007)
                               && (x.JourneyStartDatetime != null)
                               && ((((journeyPlanStartDate == null && journeyPlanEndDate == null)) && (x.JourneyStartDatetime >= journeyStartDate && x.JourneyStartDatetime <= journeyEndDate))
                               || ((journeyPlanStartDate != null && journeyPlanEndDate != null) && (x.JourneyPlanDate >= journeyPlanStartDate && x.JourneyPlanDate <= journeyPlanEndDate)))
                               && ((servicePartnerId == 0 || servicePartnerId == null) || (servicePartnerId > 0 && x.ServicePartnerId == servicePartnerId))
                               && ((orderStartDate == null && orderEndDate == null) || (x.ModifiedDate >= orderStartDate && x.ModifiedDate <= orderEndDate))
                               //&& ((journeyStartDate == null && journeyEndDate == null) || (x.JourneyStartDatetime >= journeyStartDate && x.JourneyStartDatetime <= journeyEndDate))
                               && (string.IsNullOrEmpty(servicepartnerName) || (x.ServicePartner != null && x.ServicePartner.ServicePartnerBusinessName.Contains(servicepartnerName)))
                               && (string.IsNullOrEmpty(driverName) || (x.Driver != null && x.Driver.DriverName.Contains(driverName)))
                               && (string.IsNullOrEmpty(vehicleno) || (x.Driver != null && x.Driver.VehicleNumber.Contains(vehicleno)))
                               && (string.IsNullOrEmpty(driverCity) || (x.Driver != null && (x.Driver.City ?? "").ToLower() == driverCity))
                               && (string.IsNullOrEmpty(driverphoneNo) || (x.Driver != null && (x.Driver.DriverPhoneNumber ?? "").ToLower() == driverphoneNo))
                               //&& (string.IsNullOrEmpty(regdNo)) || x.TblVehicleJourneyTrackingDetails.Any(d => d.OrderTrans != null && (d.OrderTrans.RegdNo.Contains(regdNo)))
                               )).OrderByDescending(x => x.JourneyStartDatetime).ThenByDescending(x => x.TrackingId).GroupBy(x => x.DriverId).Select(group => group.First()).Skip(skip).Take(pageSize).ToListAsync();

                    recordsTotal = count;
                }
                #endregion

                #region Data Initialization for Datatable from table to Model
                if (TblVehicleJourneyTracking != null && TblVehicleJourneyTracking.Count > 0)
                {

                    StartvehicleJourneyList = _mapper.Map<List<TblVehicleJourneyTracking>, List<StartvehicleJourneyViewModel>>(TblVehicleJourneyTracking);
                    string actionURL = string.Empty;

                    foreach (StartvehicleJourneyViewModel item in StartvehicleJourneyList)
                    {
                        item.TrackingId = item.TrackingId > 0 ? item.TrackingId : 0;
                        item.ServicePartnerID = item.ServicePartnerID > 0 ? item.ServicePartnerID : 0;
                        item.DriverId = item.DriverId !> 0 ? item.DriverId : 0;
                        string timestampString = item.JourneyStartDatetime;
                        DateTime timestamp = DateTime.Parse(timestampString);
                        string formattedTimestamp = timestamp.ToString("yyyy-MM-dd HH:mm:ss");
                        item.JourneyStartDatetime = formattedTimestamp;
                        item.JourneyPlanDatetime = item.JourneyPlanDate != null ? Convert.ToDateTime(item.JourneyPlanDate).ToString("yyyy-MM-dd HH:mm:ss") : "";

                        if (item.TrackingId!= null)
                        {
                            tblVehicleJourneyTrackingDetail = _context.TblVehicleJourneyTrackingDetails.Where(x => x.IsActive == true && x.TrackingId == item.TrackingId).FirstOrDefault();
                            if(tblVehicleJourneyTrackingDetail != null && tblVehicleJourneyTrackingDetail.OrderTransId != null)
                            {
                                tblOrderTran = _context.TblOrderTrans.Where(x => x.IsActive == true && x.OrderTransId == tblVehicleJourneyTrackingDetail.OrderTransId).FirstOrDefault();
                                item.RegdNo = tblOrderTran != null ? tblOrderTran.RegdNo : null;
                                item.OrderTransId = tblVehicleJourneyTrackingDetail.OrderTransId;
                            }
                        }

                        if (item.ServicePartnerID > 0)
                        {
                            tblServicePartner = _context.TblServicePartners.Where(x => x.IsActive == true && x.ServicePartnerId == item.ServicePartnerID).FirstOrDefault();
                            item.ServicePartnerBusinessName = tblServicePartner != null ? tblServicePartner.ServicePartnerBusinessName : null;
                        }
                        if (item.DriverId > 0)
                        {
                            tblDriverDetail = _context.TblDriverDetails.Where(x => x.IsActive == true && x.DriverDetailsId == item.DriverId).FirstOrDefault();
                            item.DriverName = tblDriverDetail != null ? tblDriverDetail.DriverName : null;
                            item.VehicleNo = tblDriverDetail != null ? tblDriverDetail.VehicleNumber : null;
                            item.DriverPhoneNo = tblDriverDetail != null ? tblDriverDetail.DriverPhoneNumber : null;
                            if (tblDriverDetail?.CityId > 0)
                            {
                                tblCity = _context.TblCities.Where(x => x.IsActive == true && x.CityId == tblDriverDetail.CityId).FirstOrDefault();
                                item.DriverCity = tblCity != null ? tblCity.Name : null;
                            }
                        }
                        //LGCOrderTracking/JourneyTrackingDetails?companyId=1007I&trackingId=51
                        actionURL = " <div class='actionbtns'>";
                        //actionURL = actionURL + " <a class='btn btn-sm btn-primary' href='" + URL + "/Index1?orderTransId=" + item.OrderTransId + "' title='TimeLine'><i class='fa solid fa-clock-rotate-left'></i></a>&nbsp;";
                        actionURL += "<a href ='" + URL + "/LGCOrderTracking/JourneyTrackingDetails?trackingId=" + item.TrackingId + "' class='btn btn-primary btn-sm viewableWithAddPermission' data-bs-toggle='tooltip' data-bs-placement='top' title='View'><i class='fa-solid fa-eye'></i></a>&nbsp;";
                        actionURL += "</div>";
                        item.Action = actionURL;

                    }
                }
                #endregion
                var data = StartvehicleJourneyList;
                var jsonData = new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data };
                return Ok(jsonData);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        #endregion
        
        #region Common All Vehicle list for Admin and Service Partner Added by VK Not used now
        [HttpPost]
        public async Task<ActionResult> ActiveVehicleListSP(int servicePartnerId, DateTime? orderStartDate,
            DateTime? orderEndDate, string? regdNo, string? phoneNo, string? DriverName, string? vehicleno, string? driverCity)
        {
            #region Parameters Simplification
            if (!string.IsNullOrWhiteSpace(regdNo) && regdNo != "null")
            { regdNo = regdNo.Trim().ToLower(); }
            else { regdNo = null; }
            if (!string.IsNullOrWhiteSpace(phoneNo) && phoneNo != "null")
            { phoneNo = phoneNo.Trim().ToLower(); }
            else { phoneNo = null; }
            //if (!string.IsNullOrWhiteSpace(ServicePartnerName) && ServicePartnerName != "null")
            //{ ServicePartnerName = ServicePartnerName.Trim().ToLower(); }
            //else { ServicePartnerName = null; }
            if (!string.IsNullOrWhiteSpace(DriverName) && DriverName != "null")
            { DriverName = DriverName.Trim().ToLower(); }
            else { DriverName = null; }
            if (!string.IsNullOrWhiteSpace(vehicleno) && vehicleno != "null")
            { vehicleno = vehicleno.Trim().ToLower(); }
            else { vehicleno = null; }
            if (!string.IsNullOrWhiteSpace(driverCity) && driverCity != "null")
            { driverCity = driverCity.Trim().ToLower(); }
            else { driverCity = null; }
            #endregion

            #region Variable Declaration
            List<TblLogistic>? TblLogistics = null;
            string URL = _config.Value.URLPrefixforProd;
            string MVCURL = _config.Value.MVCBaseURLForExchangeInvoice;
            string InvoiceimagURL = string.Empty;
            TblServicePartner? tblServicePartner = null;
            TblDriverDetail? tblDriverDetail = null;
            TblCity? tblCity = null;
            List<MobileAppLogisticViewModel>? MobileAppLogisticList = null;
            int count = 0;
            TblCompany? tblCompany = null;
            TblBusinessUnit? tblBusinessUnit = null;
            #endregion

            try
            {
                #region Datatable form variables
                var draw = Request.Form["draw"].FirstOrDefault();
                var start = Request.Form["start"].FirstOrDefault();
                var length = Request.Form["length"].FirstOrDefault();
                var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
                var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();
                var searchValue = Request.Form["search[value]"].FirstOrDefault();
                int pageSize = length != null ? Convert.ToInt32(length) : 0;
                int skip = start != null ? Convert.ToInt32(start) : 0;
                int recordsTotal = 0;
                #endregion

                #region Advanced Filters Mapping
                if (orderStartDate != null && orderEndDate != null)
                {
                    orderStartDate = Convert.ToDateTime(orderStartDate).AddMinutes(-1);
                    orderEndDate = Convert.ToDateTime(orderEndDate).AddDays(1).AddSeconds(-1);
                }
                #endregion

                #region table object Initialization
                count = _context.TblLogistics
                               .Include(x => x.ServicePartner)
                               .Include(x => x.DriverDetails)
                               .Count(x => x.IsActive == true && x.DriverDetails != null
                               && (x.StatusId == Convert.ToInt32(OrderStatusEnum.VehicleAssignbyServicePartner)
                               && ((orderStartDate == null && orderEndDate == null) || (x.ModifiedDate >= orderStartDate && x.ModifiedDate <= orderEndDate))
                               && (servicePartnerId > 0 && (x.ServicePartnerId == servicePartnerId))
                               && (string.IsNullOrEmpty(DriverName) || (x.DriverDetails != null && x.DriverDetails.DriverName.Contains(DriverName)))
                               && (string.IsNullOrEmpty(vehicleno) || (x.DriverDetails != null && x.DriverDetails.VehicleNumber.Contains(vehicleno)))
                               && (string.IsNullOrEmpty(driverCity) || (x.DriverDetails != null && (x.DriverDetails.City ?? "").ToLower() == driverCity))
                               && (string.IsNullOrEmpty(phoneNo) || (x.DriverDetails != null && (x.DriverDetails.DriverPhoneNumber ?? "").ToLower() == phoneNo))
                               && (string.IsNullOrEmpty(regdNo) || (x.RegdNo ?? "").ToLower() == regdNo)
                               ));
                if (count > 0)
                {
                    TblLogistics = await _context.TblLogistics
                               .Include(x => x.ServicePartner)
                               .Include(x => x.DriverDetails)
                               .Where(x => x.IsActive == true && x.DriverDetails != null
                               && (x.StatusId == Convert.ToInt32(OrderStatusEnum.VehicleAssignbyServicePartner)
                               && ((orderStartDate == null && orderEndDate == null) || (x.ModifiedDate >= orderStartDate && x.ModifiedDate <= orderEndDate))
                               && (servicePartnerId > 0 && (x.ServicePartnerId == servicePartnerId))
                               && (string.IsNullOrEmpty(DriverName) || (x.DriverDetails != null && x.DriverDetails.DriverName.Contains(DriverName)))
                               && (string.IsNullOrEmpty(vehicleno) || (x.DriverDetails != null && x.DriverDetails.VehicleNumber.Contains(vehicleno)))
                               && (string.IsNullOrEmpty(driverCity) || (x.DriverDetails != null && (x.DriverDetails.City ?? "").ToLower() == driverCity))
                               && (string.IsNullOrEmpty(phoneNo) || (x.DriverDetails != null && (x.DriverDetails.DriverPhoneNumber ?? "").ToLower() == phoneNo))
                               && (string.IsNullOrEmpty(regdNo) || (x.RegdNo ?? "").ToLower() == regdNo)
                               )).OrderByDescending(x => x.ModifiedDate).ThenByDescending(x => x.LogisticId).Skip(skip).Take(pageSize).ToListAsync();

                    recordsTotal = count;
                }
                #endregion

                #region Data Initialization for Datatable from table to Model
                if (TblLogistics != null && TblLogistics.Count > 0)
                {
                    MobileAppLogisticList = _mapper.Map<List<TblLogistic>, List<MobileAppLogisticViewModel>>(TblLogistics);
                    string actionURL = string.Empty;

                    foreach (MobileAppLogisticViewModel item in MobileAppLogisticList)
                    {
                        //item.LogisticId = item.LogisticId > 0? item.LogisticId : 0;
                        item.TicketNumber = item.TicketNumber != null ? item.TicketNumber.ToString() : string.Empty;
                        item.RegdNo = item.RegdNo != null ? item.RegdNo.ToString() : string.Empty;
                        item.ModifiedDate = item.ModifiedDate != null ? item.ModifiedDate : null;
                        if (item.ServicePartnerId > 0)
                        {
                            tblServicePartner = _context.TblServicePartners.Where(x => x.IsActive == true && x.ServicePartnerId == item.ServicePartnerId).FirstOrDefault();
                            item.ServicePartnerName = tblServicePartner != null ? tblServicePartner.ServicePartnerName : null;
                        }
                        if (item.driverDetailsId > 0)
                        {
                            tblDriverDetail = _context.TblDriverDetails.Where(x => x.IsActive == true && x.DriverDetailsId == item.driverDetailsId).FirstOrDefault();
                            item.DriverName = tblDriverDetail != null ? tblDriverDetail.DriverName : null;
                            item.VehicleNo = tblDriverDetail != null ? tblDriverDetail.VehicleNumber : null;
                            item.DriverPhoneNo = tblDriverDetail != null ? tblDriverDetail.DriverPhoneNumber : null;
                            if (tblDriverDetail?.CityId > 0)
                            {
                                tblCity = _context.TblCities.Where(x => x.IsActive == true && x.CityId == tblDriverDetail.CityId).FirstOrDefault();
                                item.DriverCity = tblCity != null ? tblCity.Name : null;
                            }
                        }
                    }
                }
                #endregion
                var data = MobileAppLogisticList;
                var jsonData = new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data };
                return Ok(jsonData);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        #endregion  

        #region Common All Approved Vehicle List SP for Admin and Service Partner Added by VK Active Vehicle List
        [HttpPost]
        public async Task<ActionResult> AllApprovedVehicleListSP(int companyId,DateTime? journeyplandate, int SPId, string? driverphoneNo, string? servicepartnerName, string? driverName, string? vehicleno, string? driverCity)
        {
            #region Parameters Simplification
            //DateTime journeyplandatestr;
            //if (journeyplandate.HasValue && journeyplandate.ToString() != "null")
            //{
            //    journeyplandate
            //    //journeyplandatestr = journeyplandate.Value.ToString("yyyy-MM-dd").Trim().ToLower();}
            //else { journeyplandate = null; }

            if (!string.IsNullOrWhiteSpace(driverphoneNo) && driverphoneNo != "null")
            { driverphoneNo = driverphoneNo.Trim().ToLower(); }
            else { driverphoneNo = null; }

            if (!string.IsNullOrWhiteSpace(servicepartnerName) && servicepartnerName != "null")
            { servicepartnerName = servicepartnerName.Trim().ToLower(); }
            else { servicepartnerName = null; }

            if (!string.IsNullOrWhiteSpace(driverName) && driverName != "null")
            { driverName = driverName.Trim().ToLower(); }
            else { driverName = null; }

            if (!string.IsNullOrWhiteSpace(vehicleno) && vehicleno != "null")
            { vehicleno = vehicleno.Trim().ToLower(); }
            else { vehicleno = null; }

            if (!string.IsNullOrWhiteSpace(driverCity) && driverCity != "null")
            { driverCity = driverCity.Trim().ToLower(); }
            else { driverCity = null; }
            #endregion

            #region Variable Declaration
            List<TblLogistic>? TblLogistics = null;
            string URL = _config.Value.URLPrefixforProd;
            string MVCURL = _config.Value.MVCBaseURLForExchangeInvoice;
            string InvoiceimagURL = string.Empty;
            TblServicePartner? tblServicePartner = null;
            List<TblDriverDetail>? tblDriverDetailList = null;
            TblCity? tblCity = null;
            List<MobileAppLogisticViewModel>? MobileAppLogisticList = null;
            int count = 0;
            TblCompany? tblCompany = null;
            TblBusinessUnit? tblBusinessUnit = null;
            List<DriverDetailsViewModel>? driverDetailsVMList = null;
            List<TblLogistic>? TblLogistic = null;
            int servicePartnerId = 0;
            #endregion

            try
            {
                #region Datatable form variables
                var draw = Request.Form["draw"].FirstOrDefault();
                var start = Request.Form["start"].FirstOrDefault();
                var length = Request.Form["length"].FirstOrDefault();
                var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
                var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();
                var searchValue = Request.Form["search[value]"].FirstOrDefault();
                int pageSize = length != null ? Convert.ToInt32(length) : 0;
                int skip = start != null ? Convert.ToInt32(start) : 0;
                int recordsTotal = 0;
                #endregion

                #region tblServicePartner Check
                if (SPId > 0)
                {
                    tblServicePartner = _context.TblServicePartners.Where(x => x.IsActive == true && x.ServicePartnerId == SPId).FirstOrDefault();
                    if (tblServicePartner != null)
                    {
                        servicePartnerId = tblServicePartner.ServicePartnerId;
                    }
                }
                if (servicepartnerName != null)
                {
                    tblServicePartner = _context.TblServicePartners.Where(x => x.IsActive == true && (x.ServicePartnerBusinessName??"").ToLower().Contains(servicepartnerName)).FirstOrDefault();
                    if (tblServicePartner != null)
                    {
                        servicePartnerId = tblServicePartner.ServicePartnerId;
                    }
                }
                #endregion

                #region table object Initialization
                count = _context.TblDriverDetails
                              .Include(x=>x.CityNavigation)
                               .Count(x => x.IsActive == true 
                               //&& x.IsApproved == true
                              // && ((orderStartDate == null && orderEndDate == null) || (x.ModifiedDate >= orderStartDate && x.ModifiedDate <= orderEndDate))
                               && ((companyId > 0 && companyId == Convert.ToInt32(CompanyNameenum.UTC))
                               /*&& (servicePartnerId == 0 || (x.ServicePartnerId == servicePartnerId))*/)
                               && (servicePartnerId > 0 ? x.ServicePartnerId == servicePartnerId: (string.IsNullOrEmpty(servicepartnerName)))
                               //&& (string.IsNullOrEmpty(journeyplandatestr) ||(x.JourneyPlanDate.HasValue && x.JourneyPlanDate.Value.ToString("yyyy-MM-dd") == journeyplandatestr))
                               && (journeyplandate == null || x.JourneyPlanDate.HasValue && x.JourneyPlanDate == journeyplandate)
                               && (string.IsNullOrEmpty(driverName) || ((x.DriverName??"").ToLower().Contains(driverName)))
                               && (string.IsNullOrEmpty(vehicleno) || ((x.VehicleNumber ?? "").Contains(vehicleno)))
                               && (string.IsNullOrEmpty(driverCity) || ((x.City ?? "").ToLower() == driverCity))
                               && (string.IsNullOrEmpty(driverphoneNo) || ((x.DriverPhoneNumber ?? "").Contains(driverphoneNo)))
                               );
                if (count > 0)
                {
                    tblDriverDetailList = await _context.TblDriverDetails
                               .Include(x => x.CityNavigation)
                               .Where(x => x.IsActive == true 
                               //&& x.IsApproved == true
                               // && ((orderStartDate == null && orderEndDate == null) || (x.ModifiedDate >= orderStartDate && x.ModifiedDate <= orderEndDate))
                               && ((companyId > 0 && companyId == Convert.ToInt32(CompanyNameenum.UTC))
                               /*&& (servicePartnerId == 0 || (x.ServicePartnerId == servicePartnerId))*/)
                               && (servicePartnerId > 0 ? x.ServicePartnerId == servicePartnerId : (string.IsNullOrEmpty(servicepartnerName)))
                               && (journeyplandate == null || x.JourneyPlanDate.HasValue && x.JourneyPlanDate == journeyplandate)
                               && (string.IsNullOrEmpty(driverName) || ((x.DriverName ?? "").ToLower().Contains(driverName)))
                               && (string.IsNullOrEmpty(vehicleno) || ((x.VehicleNumber ?? "").Contains(vehicleno)))
                               && (string.IsNullOrEmpty(driverCity) || ((x.City ?? "").ToLower() == driverCity))
                               && (string.IsNullOrEmpty(driverphoneNo) || ((x.DriverPhoneNumber ?? "").Contains(driverphoneNo)))
                               ).OrderByDescending(x => x.ModifiedDate).ThenByDescending(x => x.DriverDetailsId).Skip(skip).Take(pageSize).ToListAsync();

                    recordsTotal = count;
                }
                #endregion

                #region Data Initialization for Datatable from table to Model
                if (tblDriverDetailList != null && tblDriverDetailList.Count > 0)
                {
                    driverDetailsVMList = new List<DriverDetailsViewModel>();
                    string actionURL = string.Empty;
                    DriverDetailsViewModel? driverDetailsVM = null;
                    foreach (TblDriverDetail item in tblDriverDetailList)
                    {
                        driverDetailsVM = new DriverDetailsViewModel();
                        driverDetailsVM = _mapper.Map<TblDriverDetail, DriverDetailsViewModel>(item);
                        TblLogistic = _context.TblLogistics.Where(x => x.IsActive == true && x.DriverDetailsId == item.DriverDetailsId && x.StatusId == Convert.ToInt32(OrderStatusEnum.VehicleAssignbyServicePartner)).ToList();
                        driverDetailsVM.AssignedOrdersCount = TblLogistic != null ? TblLogistic.Count: 0;

                        if (item.ServicePartnerId > 0)
                        {
                            tblServicePartner = _context.TblServicePartners.Where(x => x.IsActive == true && x.ServicePartnerId == item.ServicePartnerId).FirstOrDefault();
                            driverDetailsVM.ServicePartnerBusinessName = tblServicePartner != null ? tblServicePartner.ServicePartnerBusinessName : null;
                        }
                        if (item?.CityId > 0)
                        {
                            tblCity = item.CityNavigation;
                            driverDetailsVM.DriverCity = tblCity != null ? tblCity.Name : null;
                        }
                        if(item.JourneyPlanDate != null)
                        {
                            driverDetailsVM.JourneyPlanDate = item.JourneyPlanDate.Value.ToString("yyyy-MM-dd");
                        }


                        actionURL = " <div class='actionbtns'>";
                        actionURL += "<a href ='" + URL + "/LGCOrderTracking/LGCDriverDetails?DriverDetailsId=" + item.DriverDetailsId + "' class='btn btn-primary btn-sm viewableWithAddPermission' data-bs-toggle='tooltip' data-bs-placement='top' title='View'><i class='fa-solid fa-eye'></i></a>&nbsp;";
                        driverDetailsVM.Action = actionURL;
                        driverDetailsVMList.Add(driverDetailsVM);
                    }
                }
                #endregion
                var data = driverDetailsVMList;
                var jsonData = new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data };
                return Ok(jsonData);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        #endregion

        #region Common All Assigned Order List Driver for Admin and Service Partner Added by VK
        [HttpPost]
        public async Task<ActionResult> AssignedOrderListDriver(int? driverDetailsId, int? servicePartnerId,int? statusid, DateTime? orderStartDate,
            DateTime? orderEndDate, string? regdNo, string? driverphoneNo, string? DriverName, string? vehicleno, string? driverCity)
        {
            #region Parameters Simplification
            if (!string.IsNullOrWhiteSpace(regdNo) && regdNo != "null")
            { regdNo = regdNo.Trim().ToLower(); }
            else { regdNo = null; }
            if (!string.IsNullOrWhiteSpace(driverphoneNo) && driverphoneNo != "null")
            { driverphoneNo = driverphoneNo.Trim().ToLower(); }
            else { driverphoneNo = null; }
            //if (!string.IsNullOrWhiteSpace(ServicePartnerName) && ServicePartnerName != "null")
            //{ ServicePartnerName = ServicePartnerName.Trim().ToLower(); }
            //else { ServicePartnerName = null; }
            if (!string.IsNullOrWhiteSpace(DriverName) && DriverName != "null")
            { DriverName = DriverName.Trim().ToLower(); }
            else { DriverName = null; }
            if (!string.IsNullOrWhiteSpace(vehicleno) && vehicleno != "null")
            { vehicleno = vehicleno.Trim().ToLower(); }
            else { vehicleno = null; }
            if (!string.IsNullOrWhiteSpace(driverCity) && driverCity != "null")
            { driverCity = driverCity.Trim().ToLower(); }
            else { driverCity = null; }
            #endregion

            #region Variable Declaration
            List<TblLogistic>? TblLogistics = null;
            string URL = _config.Value.URLPrefixforProd;
            string MVCURL = _config.Value.MVCBaseURLForExchangeInvoice;
            string InvoiceimagURL = string.Empty;
            TblServicePartner? tblServicePartner = null;
            TblDriverDetail? tblDriverDetail = null;
            TblCity? tblCity = null;
            List<MobileAppLogisticViewModel>? MobileAppLogisticList = null;
            int count = 0;
            TblCompany? tblCompany = null;
            TblBusinessUnit? tblBusinessUnit = null;
            #endregion

            try
            {
                #region Datatable form variables
                var draw = Request.Form["draw"].FirstOrDefault();
                var start = Request.Form["start"].FirstOrDefault();
                var length = Request.Form["length"].FirstOrDefault();
                var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
                var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();
                var searchValue = Request.Form["search[value]"].FirstOrDefault();
                int pageSize = length != null ? Convert.ToInt32(length) : 0;
                int skip = start != null ? Convert.ToInt32(start) : 0;
                int recordsTotal = 0;
                #endregion

                #region Advanced Filters Mapping
                if (orderStartDate != null && orderEndDate != null)
                {
                    orderStartDate = Convert.ToDateTime(orderStartDate).AddMinutes(-1);
                    orderEndDate = Convert.ToDateTime(orderEndDate).AddDays(1).AddSeconds(-1);
                }
                #endregion

                #region table object Initialization
                count = _context.TblLogistics
                               .Include(x => x.ServicePartner)
                               .Include(x => x.DriverDetails).Include(x => x.Status)
                               .Include(x => x.OrderTrans).ThenInclude(x => x.Exchange).ThenInclude(x => x.ProductType).ThenInclude(x => x.ProductCat)
                               .Include(x => x.OrderTrans).ThenInclude(x => x.Abbredemption).ThenInclude(x => x.Abbregistration).ThenInclude(x => x.NewProductCategoryTypeNavigation)
                               .Include(x => x.OrderTrans).ThenInclude(x => x.Abbredemption).ThenInclude(x => x.Abbregistration).ThenInclude(x => x.NewProductCategory)
                               .Include(x => x.OrderTrans).ThenInclude(x => x.Exchange).ThenInclude(x => x.CustomerDetails)
                               .Include(x => x.OrderTrans).ThenInclude(x => x.Abbredemption).ThenInclude(x => x.CustomerDetails)
                               .Count(x => x.IsActive == true && x.DriverDetails != null
                              // && (((statusid == 0 || statusid == null) && x.StatusId == Convert.ToInt32(OrderStatusEnum.VehicleAssignbyServicePartner)) || (statusid > 0 && x.StatusId == statusid))
                               && ((statusid > 0) ? x.StatusId == statusid : x.StatusId == Convert.ToInt32(OrderStatusEnum.VehicleAssignbyServicePartner))
                               //&& ((x.StatusId == statusid && x.DriverDetailsId == driverDetailsId) || x.StatusId == Convert.ToInt32(OrderStatusEnum.VehicleAssignbyServicePartner)
                               //&& (x.StatusId == Convert.ToInt32(OrderStatusEnum.VehicleAssignbyServicePartner)
                               && ((servicePartnerId == 0 || servicePartnerId == null) || (servicePartnerId > 0 && x.ServicePartnerId == servicePartnerId))
                               && (driverDetailsId > 0 && (x.DriverDetailsId == driverDetailsId))
                               && ((orderStartDate == null && orderEndDate == null) || (x.ModifiedDate >= orderStartDate && x.ModifiedDate <= orderEndDate))
                               && (string.IsNullOrEmpty(DriverName) || (x.DriverDetails != null && x.DriverDetails.DriverName.Contains(DriverName)))
                               && (string.IsNullOrEmpty(vehicleno) || (x.DriverDetails != null && x.DriverDetails.VehicleNumber.Contains(vehicleno)))
                               && (string.IsNullOrEmpty(driverCity) || (x.DriverDetails != null && (x.DriverDetails.City ?? "").ToLower() == driverCity))
                               && (string.IsNullOrEmpty(driverphoneNo) || (x.DriverDetails != null && (x.DriverDetails.DriverPhoneNumber ?? "").ToLower() == driverphoneNo))
                               && (string.IsNullOrEmpty(regdNo) || (x.RegdNo ?? "").ToLower() == regdNo)
                               );
                if (count > 0)
                {
                    TblLogistics = await _context.TblLogistics
                               .Include(x => x.ServicePartner)
                               .Include(x => x.DriverDetails).Include(x => x.Status)
                               .Include(x => x.OrderTrans).ThenInclude(x=>x.Exchange).ThenInclude(x=>x.ProductType).ThenInclude(x=>x.ProductCat)
                               .Include(x => x.OrderTrans).ThenInclude(x => x.Abbredemption).ThenInclude(x => x.Abbregistration).ThenInclude(x => x.NewProductCategoryTypeNavigation)
                               .Include(x => x.OrderTrans).ThenInclude(x => x.Abbredemption).ThenInclude(x => x.Abbregistration).ThenInclude(x => x.NewProductCategory)
                               .Include(x => x.OrderTrans).ThenInclude(x => x.Exchange).ThenInclude(x => x.CustomerDetails)
                               .Include(x => x.OrderTrans).ThenInclude(x => x.Abbredemption).ThenInclude(x => x.CustomerDetails)
                               .Where(x => x.IsActive == true && x.DriverDetails != null
                               // && (((statusid == 0 || statusid == null) && x.StatusId == Convert.ToInt32(OrderStatusEnum.VehicleAssignbyServicePartner)) || (statusid > 0 && x.StatusId == statusid))
                               && ((statusid > 0) ? x.StatusId == statusid : x.StatusId == Convert.ToInt32(OrderStatusEnum.VehicleAssignbyServicePartner))
                               //&& (x.StatusId == Convert.ToInt32(OrderStatusEnum.VehicleAssignbyServicePartner)
                               && ((servicePartnerId == 0 || servicePartnerId == null) || (servicePartnerId > 0 && x.ServicePartnerId == servicePartnerId))
                               && (driverDetailsId > 0 && (x.DriverDetailsId == driverDetailsId))
                               && ((orderStartDate == null && orderEndDate == null) || (x.ModifiedDate >= orderStartDate && x.ModifiedDate <= orderEndDate))
                               && (string.IsNullOrEmpty(DriverName) || (x.DriverDetails != null && x.DriverDetails.DriverName.Contains(DriverName)))
                               && (string.IsNullOrEmpty(vehicleno) || (x.DriverDetails != null && x.DriverDetails.VehicleNumber.Contains(vehicleno)))
                               && (string.IsNullOrEmpty(driverCity) || (x.DriverDetails != null && (x.DriverDetails.City ?? "").ToLower() == driverCity))
                               && (string.IsNullOrEmpty(driverphoneNo) || (x.DriverDetails != null && (x.DriverDetails.DriverPhoneNumber ?? "").ToLower() == driverphoneNo))
                               && (string.IsNullOrEmpty(regdNo) || (x.RegdNo ?? "").ToLower() == regdNo)
                               ).OrderByDescending(x => x.ModifiedDate).ThenByDescending(x => x.LogisticId).Skip(skip).Take(pageSize).ToListAsync();

                    recordsTotal = count;
                }
                #endregion

                #region Data Initialization for Datatable from table to Model
                if (TblLogistics != null && TblLogistics.Count > 0)
                {
                    MobileAppLogisticList = new List<MobileAppLogisticViewModel>();
                    MobileAppLogisticViewModel? mobileAppLogisticVM = null;
                    //MobileAppLogisticList = _mapper.Map<List<TblLogistic>, List<MobileAppLogisticViewModel>>(TblLogistics);
                    string actionURL = string.Empty;

                    foreach (TblLogistic item in TblLogistics)
                    {
                        mobileAppLogisticVM = new MobileAppLogisticViewModel();
                        mobileAppLogisticVM = _mapper.Map<TblLogistic, MobileAppLogisticViewModel>(item);
                        mobileAppLogisticVM.OrderAssignedDate = item.ModifiedDate != null ? Convert.ToDateTime(item.ModifiedDate).ToString("dd/MM/yyyy") : null;
                        mobileAppLogisticVM.StatusCode = item.Status?.StatusCode;
                        mobileAppLogisticVM.StatusDesc = item.Status?.StatusDescription;
                        if (item.ServicePartner != null)
                        {
                            tblServicePartner = item.ServicePartner;
                            mobileAppLogisticVM.ServicePartnerBusinessName = tblServicePartner != null ? tblServicePartner.ServicePartnerBusinessName : null;
                        }
                        if (item.DriverDetails != null)
                        {
                            tblDriverDetail = item.DriverDetails;
                            mobileAppLogisticVM.DriverName = tblDriverDetail != null ? tblDriverDetail.DriverName : null;
                            mobileAppLogisticVM.VehicleNo = tblDriverDetail != null ? tblDriverDetail.VehicleNumber : null;
                            mobileAppLogisticVM.DriverPhoneNo = tblDriverDetail != null ? tblDriverDetail.DriverPhoneNumber : null;
                        }
                        
                        TblOrderTran? tblOrderTrans = item.OrderTrans;
                        if (tblOrderTrans != null)
                        {
                            string? productCatDesc = null; string? productCatTypeDesc = null; string? custCityObj = null;
                            if (tblOrderTrans.Exchange != null && tblOrderTrans.Exchange?.ProductType != null)
                            {
                                productCatDesc = tblOrderTrans.Exchange?.ProductType?.ProductCat != null ? tblOrderTrans.Exchange?.ProductType?.ProductCat?.Description : null;
                                productCatTypeDesc = tblOrderTrans.Exchange?.ProductType?.Description;
                                custCityObj = tblOrderTrans.Exchange?.CustomerDetails != null ? tblOrderTrans.Exchange?.CustomerDetails?.City : null;
                            }
                            else if (tblOrderTrans.Abbredemption != null && tblOrderTrans.Abbredemption?.Abbregistration != null)
                            {
                                productCatDesc = tblOrderTrans.Abbredemption?.Abbregistration?.NewProductCategory != null ? tblOrderTrans.Abbredemption?.Abbregistration?.NewProductCategory?.Description : null;
                                productCatTypeDesc = tblOrderTrans.Abbredemption?.Abbregistration?.NewProductCategoryTypeNavigation != null ? tblOrderTrans.Abbredemption?.Abbregistration?.NewProductCategoryTypeNavigation?.Description : null;
                                custCityObj = tblOrderTrans.Abbredemption?.CustomerDetails != null ? tblOrderTrans.Abbredemption?.CustomerDetails?.City : null;
                            }
                            mobileAppLogisticVM.ProductCategory = productCatDesc;
                            mobileAppLogisticVM.ProductType = productCatTypeDesc;
                            mobileAppLogisticVM.CustCity = custCityObj;
                        }

                        actionURL = " <div class='actionbtns'>";
                        actionURL = actionURL + " <a class='btn btn-sm btn-primary' href='" + URL + "/Index1?orderTransId=" + item.OrderTransId + "' title='TimeLine'><i class='fa solid fa-clock-rotate-left'></i></a>&nbsp;";
                        actionURL = actionURL + "<a href ='" + URL + "/LGC_Admin/OrderViewPage?OrderTransId=" + item.OrderTransId + "' onclick='RecordView(" + item.OrderTransId + ")' class='btn btn-primary btn-sm viewableWithAddPermission' data-bs-toggle='tooltip' data-bs-placement='top' title='View'><i class='fa-solid fa-eye'></i></a>&nbsp;";
                        actionURL = actionURL + "</div>";

                        mobileAppLogisticVM.Action = actionURL;
                        MobileAppLogisticList.Add(mobileAppLogisticVM);

                        //actionURL = " <ul class='actions'>";

                        //actionURL = "<a href ='" + URL + "/LGC/LGCPickup?regdNo=" + item.RegdNo + "' ><button onclick='View(" + item.RegdNo + ")' class='btn btn-primary btn'>View</button></a>";
                        //actionURL = actionURL + "</ul>";
                        //item.Action = actionURL;

                    }
                }
                #endregion
                var data = MobileAppLogisticList;
                var jsonData = new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data };
                return Ok(jsonData);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        #endregion

        #region Common All Get Driver Journey Tracking List for Admin and Service Partner Added by VK
        [HttpPost]
        public async Task<ActionResult> GetDriverJourneyTrackingList(int companyId, int? trackingId, int? servicePartnerId, DateTime? orderStartDate,
            DateTime? orderEndDate, string? regdNo, string? ticketnumber, string? driverphoneNo, string? DriverName, string? vehicleno, string? driverCity)
        {
            #region Parameters Simplification
            if (!string.IsNullOrWhiteSpace(regdNo) && regdNo != "null")
            { regdNo = regdNo.Trim().ToLower(); }
            else { regdNo = null; }
            if (!string.IsNullOrWhiteSpace(driverphoneNo) && driverphoneNo != "null")
            { driverphoneNo = driverphoneNo.Trim().ToLower(); }
            else { driverphoneNo = null; }
            //if (!string.IsNullOrWhiteSpace(ServicePartnerName) && ServicePartnerName != "null")
            //{ ServicePartnerName = ServicePartnerName.Trim().ToLower(); }
            //else { ServicePartnerName = null; }
            if (!string.IsNullOrWhiteSpace(DriverName) && DriverName != "null")
            { DriverName = DriverName.Trim().ToLower(); }
            else { DriverName = null; }
            if (!string.IsNullOrWhiteSpace(vehicleno) && vehicleno != "null")
            { vehicleno = vehicleno.Trim().ToLower(); }
            else { vehicleno = null; }
            if (!string.IsNullOrWhiteSpace(driverCity) && driverCity != "null")
            { driverCity = driverCity.Trim().ToLower(); }
            else { driverCity = null; }
            #endregion

            #region Variable Declaration
            List<TblLogistic>? TblLogistics = null;
            string InvoiceimagURL = string.Empty;
            TblServicePartner? tblServicePartner = null;
            TblDriverDetail? tblDriverDetail = null;
            List<TblVehicleJourneyTrackingDetail>? tblVJTD_List = null;
            List<MobileAppLogisticViewModel>? MobileAppLogisticVMList = null;
            int count = 0;
            #endregion

            try
            {
                #region Variable Initialization
                string? URL = _config.Value.URLPrefixforProd;
                string? MVCURL = _config.Value.MVCBaseURLForExchangeInvoice;
                #endregion

                #region Datatable form variables
                var draw = Request.Form["draw"].FirstOrDefault();
                var start = Request.Form["start"].FirstOrDefault();
                var length = Request.Form["length"].FirstOrDefault();
                var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
                var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();
                var searchValue = Request.Form["search[value]"].FirstOrDefault();
                int pageSize = length != null ? Convert.ToInt32(length) : 0;
                int skip = start != null ? Convert.ToInt32(start) : 0;
                int recordsTotal = 0;
                #endregion

                #region Advanced Filters Mapping
                if (orderStartDate != null && orderEndDate != null)
                {
                    orderStartDate = Convert.ToDateTime(orderStartDate).AddMinutes(-1);
                    orderEndDate = Convert.ToDateTime(orderEndDate).AddDays(1).AddSeconds(-1);
                }
                #endregion

                #region table object Initialization
                count = _context.TblVehicleJourneyTrackingDetails
                               .Include(x => x.ServicePartner)
                               .Include(x => x.Driver).Include(x => x.Status)
                               .Include(x => x.OrderTrans).ThenInclude(x => x.Exchange).ThenInclude(x => x.ProductType).ThenInclude(x => x.ProductCat)
                               .Include(x => x.OrderTrans).ThenInclude(x => x.Abbredemption).ThenInclude(x => x.Abbregistration).ThenInclude(x => x.NewProductCategoryTypeNavigation)
                               .Include(x => x.OrderTrans).ThenInclude(x => x.Abbredemption).ThenInclude(x => x.Abbregistration).ThenInclude(x => x.NewProductCategory)
                               .Include(x => x.OrderTrans).ThenInclude(x => x.Exchange).ThenInclude(x => x.CustomerDetails)
                               .Include(x => x.OrderTrans).ThenInclude(x => x.Abbredemption).ThenInclude(x => x.CustomerDetails)
                               .Count(x => x.IsActive == true && x.Driver != null && x.OrderTrans != null
                            //   && (x.StatusId == Convert.ToInt32(OrderStatusEnum.VehicleAssignbyServicePartner))
                               && (companyId > 0 && companyId == Convert.ToInt32(CompanyNameenum.UTC))
                               && ((servicePartnerId == 0 || servicePartnerId == null) || (servicePartnerId > 0 && x.ServicePartnerId == servicePartnerId))
                               && ((trackingId == 0 || trackingId == null) || (trackingId > 0 && x.TrackingId == trackingId))
                               && (trackingId > 0 || servicePartnerId > 0)
                               && ((orderStartDate == null && orderEndDate == null) || (x.ModifiedDate >= orderStartDate && x.ModifiedDate <= orderEndDate))
                               && (string.IsNullOrEmpty(DriverName) || (x.Driver != null && (x.Driver.DriverName??"").Contains(DriverName)))
                               && (string.IsNullOrEmpty(vehicleno) || (x.Driver != null && (x.Driver.VehicleNumber??"").Contains(vehicleno)))
                               && (string.IsNullOrEmpty(driverCity) || (x.Driver != null && (x.Driver.City ?? "").ToLower() == driverCity))
                               && (string.IsNullOrEmpty(driverphoneNo) || (x.Driver != null && (x.Driver.DriverPhoneNumber ?? "").ToLower() == driverphoneNo))
                               && (string.IsNullOrEmpty(regdNo) || (x.OrderTrans.RegdNo ?? "").ToLower() == regdNo)
                               );
                if (count > 0)
                {
                    tblVJTD_List = await _context.TblVehicleJourneyTrackingDetails
                               .Include(x => x.ServicePartner)
                               .Include(x => x.Driver).Include(x => x.Status)
                               .Include(x => x.OrderTrans).ThenInclude(x => x.Exchange).ThenInclude(x => x.ProductType).ThenInclude(x => x.ProductCat)
                               .Include(x => x.OrderTrans).ThenInclude(x => x.Abbredemption).ThenInclude(x => x.Abbregistration).ThenInclude(x => x.NewProductCategoryTypeNavigation)
                               .Include(x => x.OrderTrans).ThenInclude(x => x.Abbredemption).ThenInclude(x => x.Abbregistration).ThenInclude(x => x.NewProductCategory)
                               .Include(x => x.OrderTrans).ThenInclude(x => x.Exchange).ThenInclude(x => x.CustomerDetails)
                               .Include(x => x.OrderTrans).ThenInclude(x => x.Abbredemption).ThenInclude(x => x.CustomerDetails)
                               .Where(x => x.IsActive == true && x.Driver != null && x.OrderTrans != null
                             //  && (x.StatusId == Convert.ToInt32(OrderStatusEnum.VehicleAssignbyServicePartner))
                               && (companyId > 0 && companyId == Convert.ToInt32(CompanyNameenum.UTC))
                               && ((servicePartnerId == 0 || servicePartnerId == null) || (servicePartnerId > 0 && x.ServicePartnerId == servicePartnerId))
                               && ((trackingId == 0 || trackingId == null) || (trackingId > 0 && x.TrackingId == trackingId))
                               && (trackingId > 0 || servicePartnerId > 0)
                               && ((orderStartDate == null && orderEndDate == null) || (x.ModifiedDate >= orderStartDate && x.ModifiedDate <= orderEndDate))
                               && (string.IsNullOrEmpty(DriverName) || (x.Driver != null && (x.Driver.DriverName ?? "").Contains(DriverName)))
                               && (string.IsNullOrEmpty(vehicleno) || (x.Driver != null && (x.Driver.VehicleNumber ?? "").Contains(vehicleno)))
                               && (string.IsNullOrEmpty(driverCity) || (x.Driver != null && (x.Driver.City ?? "").ToLower() == driverCity))
                               && (string.IsNullOrEmpty(driverphoneNo) || (x.Driver != null && (x.Driver.DriverPhoneNumber ?? "").ToLower() == driverphoneNo))
                               && (string.IsNullOrEmpty(regdNo) || (x.OrderTrans.RegdNo ?? "").ToLower() == regdNo)
                               ).OrderByDescending(x => x.ModifiedDate).ThenByDescending(x => x.TrackingDetailsId).Skip(skip).Take(pageSize).ToListAsync();

                    recordsTotal = count;
                }
                #endregion

                #region Data Initialization for Datatable from table to Model
                if (tblVJTD_List != null && tblVJTD_List.Count > 0)
                {
                    MobileAppLogisticVMList = new List<MobileAppLogisticViewModel>();
                    MobileAppLogisticViewModel? mobileAppLogisticVM = null;
                    string actionURL = string.Empty;
                    TblLogistic? TblLogistic = null;

                    foreach (TblVehicleJourneyTrackingDetail item in tblVJTD_List)
                    {
                        if (item != null)
                        {
                            mobileAppLogisticVM = new MobileAppLogisticViewModel();
                            //mobileAppLogisticVM = _mapper.Map<TblLogistic, TblVehicleJourneyTrackingDetail>(item);
                            mobileAppLogisticVM.OrderDate = item.CreatedDate != null ? Convert.ToDateTime(item.CreatedDate).ToString("dd/MM/yyyy") : null;
                            mobileAppLogisticVM.LastUpdation = item.ModifiedDate != null ? Convert.ToDateTime(item.ModifiedDate).ToString("dd/MM/yyyy hh:mm") : null;
                            //mobileAppLogisticVM.OrderDate = item.Creat != null ? Convert.ToDateTime(item.ModifiedDate).ToString("dd/MM/yyyy") : null;
                            if (item.ServicePartner != null)
                            {
                                tblServicePartner = item.ServicePartner;
                                mobileAppLogisticVM.ServicePartnerBusinessName = tblServicePartner != null ? tblServicePartner.ServicePartnerBusinessName : null;
                            }
                            if (item.Driver != null)
                            {
                                tblDriverDetail = item.Driver;
                                mobileAppLogisticVM.DriverName = tblDriverDetail != null ? tblDriverDetail.DriverName : null;
                                mobileAppLogisticVM.VehicleNo = tblDriverDetail != null ? tblDriverDetail.VehicleNumber : null;
                                mobileAppLogisticVM.DriverPhoneNo = tblDriverDetail != null ? tblDriverDetail.DriverPhoneNumber : null;
                            }
                            if (item.Status != null)
                            {
                                mobileAppLogisticVM.StatusDesc = item.Status?.StatusDescription;
                                mobileAppLogisticVM.StatusCode = item.Status?.StatusCode;
                            }
                            TblOrderTran? tblOrderTrans = item.OrderTrans;
                            if (tblOrderTrans != null)
                            {
                                string? productCatDesc = null; string? productCatTypeDesc = null; string? custCityObj = null;
                                if (tblOrderTrans.Exchange != null && tblOrderTrans.Exchange?.ProductType != null)
                                {
                                    productCatDesc = tblOrderTrans.Exchange?.ProductType?.ProductCat != null ? tblOrderTrans.Exchange?.ProductType?.ProductCat?.Description : null;
                                    productCatTypeDesc = tblOrderTrans.Exchange?.ProductType?.Description;
                                    custCityObj = tblOrderTrans.Exchange?.CustomerDetails != null ? tblOrderTrans.Exchange?.CustomerDetails?.City : null;
                                }
                                else if (tblOrderTrans.Abbredemption != null && tblOrderTrans.Abbredemption?.Abbregistration != null)
                                {
                                    productCatDesc = tblOrderTrans.Abbredemption?.Abbregistration?.NewProductCategory != null ? tblOrderTrans.Abbredemption?.Abbregistration?.NewProductCategory?.Description : null;
                                    productCatTypeDesc = tblOrderTrans.Abbredemption?.Abbregistration?.NewProductCategoryTypeNavigation != null ? tblOrderTrans.Abbredemption?.Abbregistration?.NewProductCategoryTypeNavigation?.Description : null;
                                    custCityObj = tblOrderTrans.Abbredemption?.CustomerDetails != null ? tblOrderTrans.Abbredemption?.CustomerDetails?.City : null;
                                }
                                mobileAppLogisticVM.ProductCategory = productCatDesc;
                                mobileAppLogisticVM.ProductType = productCatTypeDesc;
                                mobileAppLogisticVM.CustCity = custCityObj;

                                TblLogistic = _context.TblLogistics.Where(x => x.IsActive == true && x.OrderTransId == tblOrderTrans.OrderTransId).FirstOrDefault();
                                if (TblLogistic != null)
                                {
                                    mobileAppLogisticVM.RegdNo = TblLogistic.RegdNo;
                                    mobileAppLogisticVM.TicketNumber = TblLogistic.TicketNumber;
                                }
                            }

                            mobileAppLogisticVM.Earning = item.Total;
                            mobileAppLogisticVM.EstimatedEarning = item.EstimateEarning;
                            //actionURL = " <ul class='actions'>";
                            //actionURL = "<a href ='" + URL + "/LGC/LGCPickup?regdNo=" + item.RegdNo + "' ><button onclick='View(" + item.RegdNo + ")' class='btn btn-primary btn'>View</button></a>";
                            //actionURL = actionURL + "</ul>";

                            //actionURL = " <ul class='actions'>";
                            //actionURL = actionURL + "<a href ='" + URL + "/Exchange/Manage?id=" + _protector.Encode(item.Id) + "' onclick='RecordView(" + item.Id + ")' class='btn btn-primary btn-sm viewableWithAddPermission' data-bs-toggle='tooltip' data-bs-placement='top' title='View'><i class='fa-solid fa-eye'></i></a>&nbsp;";
                            //actionURL = "<a href ='" + URL + "/LGC/LGCPickup?regdNo=" + item.RegdNo + "' ><button onclick='View(" + item.RegdNo + ")' class='btn btn-primary btn'>View</button></a>";
                            //actionURL = actionURL + "</ul>";
                            //item.Action = actionURL;

                            actionURL = " <div class='actionbtns'>";
                            actionURL = actionURL + " <a class='btn btn-sm btn-primary' href='" + URL + "/Index1?orderTransId=" + item.OrderTransId + "' title='TimeLine'><i class='fa solid fa-clock-rotate-left'></i></a>&nbsp;";
                            actionURL = actionURL + "<a href ='" + URL + "/LGC_Admin/OrderViewPage?OrderTransId=" + item.OrderTransId + "' onclick='RecordView(" + item.OrderTransId + ")' class='btn btn-primary btn-sm viewableWithAddPermission' data-bs-toggle='tooltip' data-bs-placement='top' title='View'><i class='fa-solid fa-eye'></i></a>&nbsp;";
                            actionURL = actionURL + "</div>";

                            mobileAppLogisticVM.Action = actionURL;
                            MobileAppLogisticVMList.Add(mobileAppLogisticVM);

                        }
                    }
                }
                #endregion
                var data = MobileAppLogisticVMList;
                var jsonData = new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data };
                return Ok(jsonData);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        #endregion

        #region Get List of Orders who is ready for pickup
        /*public async Task<IActionResult> UpcomingOrdersList(int userId, DateTime? startDate, DateTime? endDate,
           int? productCatId, int? productTypeId, string? regdNo, string? ticketNo)
        {
            List<TblLogistic> TblLogistic = null;
            TblServicePartner tblServicePartner = null;
            List<LGCOrderViewModel> lGCOrderList = null;
            LGCOrderViewModel lGCOrderViewModel = null;
            string URL = _config.Value.URLPrefixforProd;
            #region For Advanced filters
            if (string.IsNullOrEmpty(regdNo) || regdNo == "null")
            { regdNo = null; }
            else
            { regdNo = regdNo.Trim().ToLower(); }
            if (string.IsNullOrEmpty(ticketNo) || ticketNo == "null")
            { ticketNo = null; }
            else
            { ticketNo = ticketNo.Trim().ToLower(); }
            if (productCatId == 0)
            { productCatId = null; }
            if (productTypeId == 0)
            { productTypeId = null; }
            #endregion
            try
            {
                #region datatable variables
                var draw = Request.Form["draw"].FirstOrDefault();
                var start = Request.Form["start"].FirstOrDefault();
                var length = Request.Form["length"].FirstOrDefault();
                var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
                var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();
                var searchValue = Request.Form["search[value]"].FirstOrDefault();
                searchValue = searchValue.Trim();
                int pageSize = length != null ? Convert.ToInt32(length) : 0;
                int skip = start != null ? Convert.ToInt32(start) : 0;
                int recordsTotal = 0; int count = 0;
                #endregion

                #region Advanced Filters
                if (startDate != null && endDate != null)
                {
                    startDate = Convert.ToDateTime(startDate).AddMinutes(-1);
                    endDate = Convert.ToDateTime(endDate).AddDays(1).AddSeconds(-1);
                }
                #endregion

                #region TblLogistics data initialization
                tblServicePartner = _context.TblServicePartners.Where(x => x.UserId == userId && x.IsActive == true).FirstOrDefault();
                if (tblServicePartner != null)
                {
                    count = _context.TblLogistics
                                    .Include(x => x.OrderTrans).ThenInclude(x => x.Exchange).ThenInclude(x => x.ProductType).ThenInclude(x => x.ProductCat)
                                    .Include(x => x.OrderTrans).ThenInclude(x => x.Exchange).ThenInclude(x => x.CustomerDetails)
                                    .Include(x => x.OrderTrans).ThenInclude(x => x.Abbredemption).ThenInclude(x => x.Abbregistration).ThenInclude(x => x.NewProductCategory)
                                    .Include(x => x.OrderTrans).ThenInclude(x => x.Abbredemption).ThenInclude(x => x.Abbregistration).ThenInclude(x => x.NewProductCategoryTypeNavigation)
                                    .Include(x => x.OrderTrans).ThenInclude(x => x.Abbredemption).ThenInclude(x => x.CustomerDetails)
                                    .Count(x => x.IsActive == true && x.StatusId == 18 && x.ServicePartnerId == tblServicePartner.ServicePartnerId
                                    && ((startDate == null && endDate == null) || (x.CreatedDate >= startDate && x.CreatedDate <= endDate))
                                    && (string.IsNullOrEmpty(ticketNo) || (x.TicketNumber ?? "").ToLower() == ticketNo)
                                    && (string.IsNullOrEmpty(regdNo) || (x.RegdNo ?? "").ToLower() == regdNo)
                                    && (x.OrderTrans != null &&
                                    ((x.OrderTrans.Exchange != null
                                    && (x.OrderTrans.Exchange.ProductType != null && x.OrderTrans.Exchange.ProductType.ProductCat != null)
                                    && (productCatId == null || x.OrderTrans.Exchange.ProductType.ProductCatId == productCatId)
                                    && (productTypeId == null || x.OrderTrans.Exchange.ProductTypeId == productTypeId))
                                    || (x.OrderTrans.Abbredemption != null && x.OrderTrans.Abbredemption.Abbregistration != null
                                    && (x.OrderTrans.Abbredemption.Abbregistration.NewProductCategoryTypeNavigation != null && x.OrderTrans.Abbredemption.Abbregistration.NewProductCategory != null)
                                    && (productCatId == null || x.OrderTrans.Abbredemption.Abbregistration.NewProductCategoryId == productCatId)
                                    && (productTypeId == null || x.OrderTrans.Abbredemption.Abbregistration.NewProductCategoryTypeId == productTypeId))
                                    )));
                    if (count > 0)
                    {
                        TblLogistic = await _context.TblLogistics
                                .Include(x => x.OrderTrans).ThenInclude(x => x.Exchange).ThenInclude(x => x.ProductType).ThenInclude(x => x.ProductCat)
                                .Include(x => x.OrderTrans).ThenInclude(x => x.Exchange).ThenInclude(x => x.CustomerDetails)
                                .Include(x => x.OrderTrans).ThenInclude(x => x.Abbredemption).ThenInclude(x => x.Abbregistration).ThenInclude(x => x.NewProductCategory)
                                .Include(x => x.OrderTrans).ThenInclude(x => x.Abbredemption).ThenInclude(x => x.Abbregistration).ThenInclude(x => x.NewProductCategoryTypeNavigation)
                                .Include(x => x.OrderTrans).ThenInclude(x => x.Abbredemption).ThenInclude(x => x.Abbregistration)
                                .Include(x => x.OrderTrans).ThenInclude(x => x.Abbredemption).ThenInclude(x => x.CustomerDetails)
                                .Where(x => x.IsActive == true && x.StatusId == 18 && x.ServicePartnerId == tblServicePartner.ServicePartnerId
                                && ((startDate == null && endDate == null) || (x.CreatedDate >= startDate && x.CreatedDate <= endDate))
                                && (string.IsNullOrEmpty(ticketNo) || (x.TicketNumber ?? "").ToLower() == ticketNo)
                                && (string.IsNullOrEmpty(regdNo) || (x.RegdNo ?? "").ToLower() == regdNo)
                                && (x.OrderTrans != null &&
                                ((x.OrderTrans.Exchange != null
                                && (x.OrderTrans.Exchange.ProductType != null && x.OrderTrans.Exchange.ProductType.ProductCat != null)
                                && (productCatId == null || x.OrderTrans.Exchange.ProductType.ProductCatId == productCatId)
                                && (productTypeId == null || x.OrderTrans.Exchange.ProductTypeId == productTypeId))
                                || (x.OrderTrans.Abbredemption != null && x.OrderTrans.Abbredemption.Abbregistration != null
                                && (x.OrderTrans.Abbredemption.Abbregistration.NewProductCategoryTypeNavigation != null && x.OrderTrans.Abbredemption.Abbregistration.NewProductCategory != null)
                                && (productCatId == null || x.OrderTrans.Abbredemption.Abbregistration.NewProductCategoryId == productCatId)
                                && (productTypeId == null || x.OrderTrans.Abbredemption.Abbregistration.NewProductCategoryTypeId == productTypeId))
                                ))).OrderByDescending(x => x.ModifiedDate).ThenByDescending(x => x.LogisticId).Skip(skip).Take(pageSize).ToListAsync();
                    }
                }
                recordsTotal = count;
                #endregion

                #region Datatable Mapping
                lGCOrderList = new List<LGCOrderViewModel>();
                if (TblLogistic != null)
                {
                    foreach (var item in TblLogistic)
                    {
                        string productCatDesc = null; string productCatTypeDesc = null; string custCity = null;
                        if (item.OrderTrans.Exchange != null && item.OrderTrans.Exchange.ProductType != null)
                        {
                            productCatDesc = item.OrderTrans.Exchange.ProductType.ProductCat != null ? item.OrderTrans.Exchange.ProductType.ProductCat.Description : null;
                            productCatTypeDesc = item.OrderTrans.Exchange.ProductType.Description;
                            custCity = item.OrderTrans.Exchange.CustomerDetails != null ? item.OrderTrans.Exchange.CustomerDetails.City : null;
                        }
                        else if (item.OrderTrans.Abbredemption != null && item.OrderTrans.Abbredemption.Abbregistration != null)
                        {
                            productCatDesc = item.OrderTrans.Abbredemption.Abbregistration.NewProductCategory != null ? item.OrderTrans.Abbredemption.Abbregistration.NewProductCategory.Description : null;
                            productCatTypeDesc = item.OrderTrans.Abbredemption.Abbregistration.NewProductCategoryTypeNavigation != null ? item.OrderTrans.Abbredemption.Abbregistration.NewProductCategoryTypeNavigation.Description : null;
                            custCity = item.OrderTrans.Abbredemption.CustomerDetails != null ? item.OrderTrans.Abbredemption.CustomerDetails.City : null;
                        }
                        string actionURL = string.Empty;
                        actionURL = " <ul class='actions'>";

                        actionURL = "<a href ='" + URL + "/LGC/LGCPickup?regdNo=" + item.RegdNo + "' ><button onclick='View(" + item.RegdNo + ")' class='btn btn-primary btn'>View</button></a>";
                        actionURL = actionURL + "</ul>";
                        if (TblLogistic != null)
                        {
                            lGCOrderViewModel = new LGCOrderViewModel();
                            lGCOrderViewModel.Action = actionURL;
                            lGCOrderViewModel.Id = item.LogisticId;
                            lGCOrderViewModel.RegdNo = item.RegdNo;
                            lGCOrderViewModel.TicketNumber = item.TicketNumber;
                            lGCOrderViewModel.PickupScheduleDate = item.PickupScheduleDate != null
     ? Convert.ToDateTime(item.PickupScheduleDate).ToString("MM/dd/yyyy")
     : Convert.ToDateTime(item.CreatedDate).ToString("MM/dd/yyyy");

                            lGCOrderViewModel.ProductCategory = productCatDesc;
                            lGCOrderViewModel.ProductType = productCatTypeDesc;

                            lGCOrderViewModel.AmountPayableThroughLGC = Convert.ToDecimal(item.AmtPaybleThroughLgc);
                            lGCOrderViewModel.City = custCity;
                            lGCOrderViewModel.CreatedDate = Convert.ToDateTime(item.CreatedDate).ToString("MM/dd/yyyy");
                            lGCOrderList.Add(lGCOrderViewModel);
                        }
                    }
                }
                #endregion

                var data = lGCOrderList.OrderByDescending(x => x.CreatedDate);
                var jsonData = new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data };
                return Ok(jsonData);
            }
            catch (Exception ex)
            {
                throw;
            }
        }*/
        #endregion

        #region Get List of Orders who is ready for pickup (UpcomingOrdersList)
        [HttpPost]
        public async Task<ActionResult> UpcomingOrdersList(int companyId, int? SPId, DateTime? orderStartDate,
            DateTime? orderEndDate, string? servicepartnerName, string? regdNo, string? ticketnumber, string? customerCity, string? driverphoneNo, string? driverName, string? vehicleno, string? driverCity)
        {
            #region Parameters Simplification
            if (!string.IsNullOrWhiteSpace(regdNo) && regdNo != "null")
            { regdNo = regdNo.Trim().ToLower(); }
            else { regdNo = null; }

            if (!string.IsNullOrWhiteSpace(driverphoneNo) && driverphoneNo != "null")
            { driverphoneNo = driverphoneNo.Trim().ToLower(); }
            else { driverphoneNo = null; }

            if (!string.IsNullOrWhiteSpace(servicepartnerName) && servicepartnerName != "null")
            { servicepartnerName = servicepartnerName.Trim().ToLower(); }
            else { servicepartnerName = null; }

            if (!string.IsNullOrWhiteSpace(driverName) && driverName != "null")
            { driverName = driverName.Trim().ToLower(); }
            else { driverName = null; }

            if (!string.IsNullOrWhiteSpace(vehicleno) && vehicleno != "null")
            { vehicleno = vehicleno.Trim().ToLower(); }
            else { vehicleno = null; }

            if (!string.IsNullOrWhiteSpace(driverCity) && driverCity != "null")
            { driverCity = driverCity.Trim().ToLower(); }
            else { driverCity = null; }

            if (!string.IsNullOrWhiteSpace(ticketnumber) && ticketnumber != "null")
            { ticketnumber = ticketnumber.Trim().ToLower(); }
            else { ticketnumber = null; }
            #endregion

            #region Variable Declaration
            List<TblLogistic>? TblLogistics = null;
            string InvoiceimagURL = string.Empty;
            TblServicePartner? tblServicePartner = null;
            TblDriverDetail? tblDriverDetail = null;
            string actionURL = string.Empty;
            string actionURLCheckBox = string.Empty;
            List<MobileAppLogisticViewModel>? MobileAppLogisticList = null;
            MobileAppLogisticViewModel? mobileAppLogisticVM = null;
            int servicePartnerId = SPId ?? 0;
            int? productCatId = null;
            int? productTypeId = null;
            #endregion

            try
            {
                #region Datatable form variables
                var draw = Request.Form["draw"].FirstOrDefault();
                var start = Request.Form["start"].FirstOrDefault();
                var length = Request.Form["length"].FirstOrDefault();
                var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
                var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();
                var searchValue = Request.Form["search[value]"].FirstOrDefault();
                int pageSize = length != null ? Convert.ToInt32(length) : 0;
                int skip = start != null ? Convert.ToInt32(start) : 0;
                int recordsTotal = 0; int count = 0;
                #endregion

                #region Advanced Filters Mapping
                if (orderStartDate != null && orderEndDate != null)
                {
                    orderStartDate = Convert.ToDateTime(orderStartDate).AddMinutes(-1);
                    orderEndDate = Convert.ToDateTime(orderEndDate).AddDays(1).AddSeconds(-1);
                }
                #endregion

                #region table object Initialization
                count = _context.TblLogistics
                               .Include(x => x.ServicePartner)
                               .Include(x => x.DriverDetails)
                               .Include(x => x.OrderTrans).ThenInclude(x => x.Exchange).ThenInclude(x => x.ProductType).ThenInclude(x => x.ProductCat)
                               .Include(x => x.OrderTrans).ThenInclude(x => x.Abbredemption).ThenInclude(x => x.Abbregistration).ThenInclude(x => x.NewProductCategoryTypeNavigation)
                               .Include(x => x.OrderTrans).ThenInclude(x => x.Abbredemption).ThenInclude(x => x.Abbregistration).ThenInclude(x => x.NewProductCategory)
                               .Include(x => x.OrderTrans).ThenInclude(x => x.Exchange).ThenInclude(x => x.CustomerDetails)
                               .Include(x => x.OrderTrans).ThenInclude(x => x.Abbredemption).ThenInclude(x => x.CustomerDetails)
                               .Count(x => x.IsActive == true
                               && (x.StatusId == Convert.ToInt32(OrderStatusEnum.LogisticsTicketUpdated)
                               && ((companyId > 0 && companyId == Convert.ToInt32(CompanyNameenum.UTC)) && (servicePartnerId == 0 || (x.ServicePartnerId == servicePartnerId)))
                               && ((orderStartDate == null && orderEndDate == null) || (x.CreatedDate >= orderStartDate && x.CreatedDate <= orderEndDate))
                               && (string.IsNullOrEmpty(servicepartnerName) || (x.ServicePartner != null && (x.ServicePartner.ServicePartnerBusinessName??"").Contains(servicepartnerName)))
                               && (string.IsNullOrEmpty(driverName) || (x.DriverDetails != null && (x.DriverDetails.DriverName ?? "").Contains(driverName)))
                               && (string.IsNullOrEmpty(vehicleno) || (x.DriverDetails != null && (x.DriverDetails.VehicleNumber??"").Contains(vehicleno)))
                               && (string.IsNullOrEmpty(ticketnumber) || (x.TicketNumber != null && (x.TicketNumber??"").Contains(ticketnumber)))
                               && (string.IsNullOrEmpty(driverCity) || (x.DriverDetails != null && (x.DriverDetails.City ?? "").ToLower() == driverCity))
                               && (string.IsNullOrEmpty(driverphoneNo) || (x.DriverDetails != null && (x.DriverDetails.DriverPhoneNumber ?? "").ToLower() == driverphoneNo))
                               && (string.IsNullOrEmpty(regdNo) || (x.RegdNo ?? "").ToLower() == regdNo)
                               && ((x.OrderTrans.Exchange != null
                        && (x.OrderTrans.Exchange.ProductType != null && x.OrderTrans.Exchange.ProductType.ProductCat != null && x.OrderTrans.Exchange.CustomerDetails != null)
                        && (x.OrderTrans.Exchange.ProductType.ProductCatId != null && x.OrderTrans.Exchange.CustomerDetails != null)
                        && (productCatId == null || x.OrderTrans.Exchange.ProductType.ProductCatId == productCatId)
                        && (productTypeId == null || x.OrderTrans.Exchange.ProductTypeId == productTypeId)
                        && (string.IsNullOrEmpty(customerCity) || (x.OrderTrans.Exchange.CustomerDetails.City ?? "").ToLower() == customerCity)
                        ) // Code for ABB Redumption 
                        || (x.OrderTrans.Abbredemption != null && x.OrderTrans.Abbredemption.Abbregistration != null
                        && (x.OrderTrans.Abbredemption.Abbregistration.NewProductCategoryTypeNavigation != null
                        && x.OrderTrans.Abbredemption.Abbregistration.NewProductCategory != null
                        && x.OrderTrans.Abbredemption.CustomerDetails != null)
                        && (productCatId == null || x.OrderTrans.Abbredemption.Abbregistration.NewProductCategoryId == productCatId)
                        && (productTypeId == null || x.OrderTrans.Abbredemption.Abbregistration.NewProductCategoryTypeId == productTypeId)
                        && (string.IsNullOrEmpty(customerCity) || (x.OrderTrans.Abbredemption.CustomerDetails.City ?? "").ToLower() == customerCity)
                        ))));
                if (count > 0)
                {
                    TblLogistics = await _context.TblLogistics
                               .Include(x => x.ServicePartner)
                               .Include(x => x.DriverDetails)
                               .Include(x => x.OrderTrans).ThenInclude(x => x.Exchange).ThenInclude(x => x.ProductType).ThenInclude(x => x.ProductCat)
                               .Include(x => x.OrderTrans).ThenInclude(x => x.Abbredemption).ThenInclude(x => x.Abbregistration).ThenInclude(x => x.NewProductCategoryTypeNavigation)
                               .Include(x => x.OrderTrans).ThenInclude(x => x.Abbredemption).ThenInclude(x => x.Abbregistration).ThenInclude(x => x.NewProductCategory)
                               .Include(x => x.OrderTrans).ThenInclude(x => x.Exchange).ThenInclude(x => x.CustomerDetails)
                               .Include(x => x.OrderTrans).ThenInclude(x => x.Abbredemption).ThenInclude(x => x.CustomerDetails)
                               .Where(x => x.IsActive == true
                               && (x.StatusId == Convert.ToInt32(OrderStatusEnum.LogisticsTicketUpdated)
                               && ((servicePartnerId == 0 || servicePartnerId == null) || (servicePartnerId > 0 && x.ServicePartnerId == servicePartnerId))
                               && ((orderStartDate == null && orderEndDate == null) || (x.CreatedDate >= orderStartDate && x.CreatedDate <= orderEndDate))
                               && (string.IsNullOrEmpty(servicepartnerName) || (x.ServicePartner != null && (x.ServicePartner.ServicePartnerBusinessName ?? "").Contains(servicepartnerName)))
                               && (string.IsNullOrEmpty(driverName) || (x.DriverDetails != null && (x.DriverDetails.DriverName ?? "").Contains(driverName)))
                               && (string.IsNullOrEmpty(vehicleno) || (x.DriverDetails != null && (x.DriverDetails.VehicleNumber ?? "").Contains(vehicleno)))
                               && (string.IsNullOrEmpty(ticketnumber) || (x.TicketNumber != null && (x.TicketNumber ?? "").Contains(ticketnumber)))
                               && (string.IsNullOrEmpty(driverCity) || (x.DriverDetails != null && (x.DriverDetails.City ?? "").ToLower() == driverCity))
                               && (string.IsNullOrEmpty(driverphoneNo) || (x.DriverDetails != null && (x.DriverDetails.DriverPhoneNumber ?? "").ToLower() == driverphoneNo))
                               && (string.IsNullOrEmpty(regdNo) || (x.RegdNo ?? "").ToLower() == regdNo)
                               && ((x.OrderTrans.Exchange != null
                        && (x.OrderTrans.Exchange.ProductType != null && x.OrderTrans.Exchange.ProductType.ProductCat != null && x.OrderTrans.Exchange.CustomerDetails != null)
                        && (x.OrderTrans.Exchange.ProductType.ProductCatId != null && x.OrderTrans.Exchange.CustomerDetails != null)
                        && (productCatId == null || x.OrderTrans.Exchange.ProductType.ProductCatId == productCatId)
                        && (productTypeId == null || x.OrderTrans.Exchange.ProductTypeId == productTypeId)
                        && (string.IsNullOrEmpty(customerCity) || (x.OrderTrans.Exchange.CustomerDetails.City ?? "").ToLower() == customerCity)
                        ) // Code for ABB Redumption 
                        || (x.OrderTrans.Abbredemption != null && x.OrderTrans.Abbredemption.Abbregistration != null
                        && (x.OrderTrans.Abbredemption.Abbregistration.NewProductCategoryTypeNavigation != null
                        && x.OrderTrans.Abbredemption.Abbregistration.NewProductCategory != null
                        && x.OrderTrans.Abbredemption.CustomerDetails != null)
                        && (productCatId == null || x.OrderTrans.Abbredemption.Abbregistration.NewProductCategoryId == productCatId)
                        && (productTypeId == null || x.OrderTrans.Abbredemption.Abbregistration.NewProductCategoryTypeId == productTypeId)
                        && (string.IsNullOrEmpty(customerCity) || (x.OrderTrans.Abbredemption.CustomerDetails.City ?? "").ToLower() == customerCity)
                        )))).OrderByDescending(x => x.ModifiedDate).ThenByDescending(x => x.LogisticId).Skip(skip).Take(pageSize).ToListAsync();

                    recordsTotal = count;
                }
                #endregion

                #region Data Initialization for Datatable from table to Model
                if (TblLogistics != null && TblLogistics.Count > 0)
                {
                    #region Variable Intializations
                    string? URL = _config.Value.URLPrefixforProd;
                    MobileAppLogisticList = new List<MobileAppLogisticViewModel>();
                    #endregion

                    foreach (TblLogistic item in TblLogistics)
                    {
                        if (item != null)
                        {
                            mobileAppLogisticVM = new MobileAppLogisticViewModel();
                            mobileAppLogisticVM = _mapper.Map<TblLogistic, MobileAppLogisticViewModel>(item);

                            if (item.ServicePartner != null)
                            {
                                tblServicePartner = item.ServicePartner;
                                mobileAppLogisticVM.ServicePartnerBusinessName = tblServicePartner != null ? tblServicePartner.ServicePartnerBusinessName : null;
                            }
                            if (item.DriverDetails != null)
                            {
                                tblDriverDetail = item.DriverDetails;
                                mobileAppLogisticVM.DriverName = tblDriverDetail != null ? tblDriverDetail.DriverName : null;
                                mobileAppLogisticVM.VehicleNo = tblDriverDetail != null ? tblDriverDetail.VehicleNumber : null;
                                mobileAppLogisticVM.DriverPhoneNo = tblDriverDetail != null ? tblDriverDetail.DriverPhoneNumber : null;
                            }

                            TblOrderTran? tblOrderTrans = item.OrderTrans;
                            if (tblOrderTrans != null)
                            {
                                #region Common Variables for Exchange or ABB
                                string? productCatDesc = null; string? productCatTypeDesc = null; string? custCityObj = null;
                                if (tblOrderTrans.Exchange != null && tblOrderTrans.Exchange?.ProductType != null)
                                {
                                    productCatDesc = tblOrderTrans.Exchange?.ProductType?.ProductCat != null ? tblOrderTrans.Exchange?.ProductType?.ProductCat?.Description : null;
                                    productCatTypeDesc = tblOrderTrans.Exchange?.ProductType?.Description;
                                    custCityObj = tblOrderTrans.Exchange?.CustomerDetails != null ? tblOrderTrans.Exchange?.CustomerDetails?.City : null;
                                }
                                else if (tblOrderTrans.Abbredemption != null && tblOrderTrans.Abbredemption?.Abbregistration != null)
                                {
                                    productCatDesc = tblOrderTrans.Abbredemption?.Abbregistration?.NewProductCategory != null ? tblOrderTrans.Abbredemption?.Abbregistration?.NewProductCategory?.Description : null;
                                    productCatTypeDesc = tblOrderTrans.Abbredemption?.Abbregistration?.NewProductCategoryTypeNavigation != null ? tblOrderTrans.Abbredemption?.Abbregistration?.NewProductCategoryTypeNavigation?.Description : null;
                                    custCityObj = tblOrderTrans.Abbredemption?.CustomerDetails != null ? tblOrderTrans.Abbredemption?.CustomerDetails?.City : null;
                                }
                                #endregion

                                mobileAppLogisticVM.ProductCategory = productCatDesc;
                                mobileAppLogisticVM.ProductType = productCatTypeDesc;
                                mobileAppLogisticVM.CustCity = custCityObj;
                            }

                            actionURLCheckBox = " <td class='actions'>";
                            actionURLCheckBox = actionURLCheckBox + " <span><input type='checkbox' id=" + item.OrderTransId + " name ='orders'  value ='" + item.OrderTransId + "'   onclick='OnCheckBoxCheck();' class='checkboxinput' /></span>";
                            actionURLCheckBox = actionURLCheckBox + " </td>";
                            mobileAppLogisticVM.Edit = actionURLCheckBox;

                            actionURL = " <ul class='actions'>";
                            actionURL = "<button onclick='RejectOrder(" + item.OrderTransId + ")' class='btn btn-primary btn'>Reject Order</button>";
                            actionURL = actionURL + "</ul>";

                            mobileAppLogisticVM.Action = actionURL;
                            mobileAppLogisticVM.OrderDate = item.CreatedDate != null ? Convert.ToDateTime(item.CreatedDate).ToString("dd/MM/yyyy") : null;
                            //mobileAppLogisticVM.OrderAssignedDate = item.ModifiedDate != null ? Convert.ToDateTime(item.ModifiedDate).ToString("dd/MM/yyyy") : null;
                            MobileAppLogisticList.Add(mobileAppLogisticVM);
                        }
                    }
                }
                #endregion
                
                var data = MobileAppLogisticList;
                var jsonData = new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data };
                return Ok(jsonData);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        #endregion

        #region Common All Get Wallet List for Admin and Service Partner Added by VK
        [HttpPost]
        public async Task<ActionResult> GetWalletList(int companyId, int? SPId, int? trackingId, DateTime? journeyStartDate, DateTime? journeyEndDate, string? regdNo, string? phoneNo, string? DriverName, string? vehicleno, string? driverCity)
        {
            #region Parameters Simplification
            if (!string.IsNullOrWhiteSpace(regdNo) && regdNo != "null")
            { regdNo = regdNo.Trim().ToLower(); }
            else { regdNo = null; }
            if (!string.IsNullOrWhiteSpace(phoneNo) && phoneNo != "null")
            { phoneNo = phoneNo.Trim().ToLower(); }
            else { phoneNo = null; }
            //if (!string.IsNullOrWhiteSpace(ServicePartnerName) && ServicePartnerName != "null")
            //{ ServicePartnerName = ServicePartnerName.Trim().ToLower(); }
            //else { ServicePartnerName = null; }
            if (!string.IsNullOrWhiteSpace(DriverName) && DriverName != "null")
            { DriverName = DriverName.Trim().ToLower(); }
            else { DriverName = null; }
            if (!string.IsNullOrWhiteSpace(vehicleno) && vehicleno != "null")
            { vehicleno = vehicleno.Trim().ToLower(); }
            else { vehicleno = null; }
            if (!string.IsNullOrWhiteSpace(driverCity) && driverCity != "null")
            { driverCity = driverCity.Trim().ToLower(); }
            else { driverCity = null; }
            #endregion

            #region Variable Declaration
            List<TblLogistic>? TblLogistics = null;
            string InvoiceimagURL = string.Empty;
            TblServicePartner? tblServicePartner = null;
            TblDriverDetail? tblDriverDetail = null;
            List<TblVehicleJourneyTrackingDetail>? tblVJTD_List = null;
            List<MobileAppLogisticViewModel>? MobileAppLogisticVMList = null;
            int count = 0;
            int servicePartnerId = 0;
            //DateTime? journeyStartDate = null;
            //DateTime? journeyEndDate = null;
            #endregion

            try
            {
                #region Variable Initialization
                string? URL = _config.Value.URLPrefixforProd;
                string? MVCURL = _config.Value.MVCBaseURLForExchangeInvoice;
                #endregion

                #region Datatable form variables
                var draw = Request.Form["draw"].FirstOrDefault();
                var start = Request.Form["start"].FirstOrDefault();
                var length = Request.Form["length"].FirstOrDefault();
                var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
                var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();
                var searchValue = Request.Form["search[value]"].FirstOrDefault();
                int pageSize = length != null ? Convert.ToInt32(length) : 0;
                int skip = start != null ? Convert.ToInt32(start) : 0;
                int recordsTotal = 0;
                #endregion

                #region Advanced Filters Mapping
                if (journeyStartDate != null && journeyEndDate != null)
                {
                    journeyStartDate = Convert.ToDateTime(journeyStartDate).AddMinutes(-1);
                    journeyEndDate = Convert.ToDateTime(journeyEndDate).AddDays(1).AddSeconds(-1);
                }
                else
                {
                    var CurrentDate = _currentDatetime.ToString("MM/dd/yyyy");
                    journeyStartDate = Convert.ToDateTime(CurrentDate).AddMinutes(-1);
                    journeyEndDate = Convert.ToDateTime(CurrentDate).AddDays(1).AddSeconds(-1);
                }
                #endregion

                #region tblServicePartner Check
                tblServicePartner = _context.TblServicePartners.Where(x => x.IsActive == true && x.ServicePartnerId == SPId).FirstOrDefault();
                if (tblServicePartner != null)
                {
                    servicePartnerId = tblServicePartner.ServicePartnerId;
                }
                #endregion

                #region table object Initialization
                count = _context.TblVehicleJourneyTrackingDetails
                               .Include(x => x.ServicePartner)
                               .Include(x => x.Driver).Include(x => x.Status)
                               .Include(x => x.OrderTrans).ThenInclude(x => x.Exchange).ThenInclude(x => x.ProductType).ThenInclude(x => x.ProductCat)
                               .Include(x => x.OrderTrans).ThenInclude(x => x.Abbredemption).ThenInclude(x => x.Abbregistration).ThenInclude(x => x.NewProductCategoryTypeNavigation)
                               .Include(x => x.OrderTrans).ThenInclude(x => x.Abbredemption).ThenInclude(x => x.Abbregistration).ThenInclude(x => x.NewProductCategory)
                               .Include(x => x.OrderTrans).ThenInclude(x => x.Exchange).ThenInclude(x => x.CustomerDetails)
                               .Include(x => x.OrderTrans).ThenInclude(x => x.Abbredemption).ThenInclude(x => x.CustomerDetails)
                               .Count(x => x.IsActive == true && x.Driver != null && x.OrderTrans != null
                               && (x.StatusId != Convert.ToInt32(OrderStatusEnum.OrderAcceptedbyVehicle))
                               && (companyId > 0 && companyId == Convert.ToInt32(CompanyNameenum.UTC))
                               && ((servicePartnerId == 0 || servicePartnerId == null) || (servicePartnerId > 0 && x.ServicePartnerId == servicePartnerId))
                               && ((trackingId == 0 || trackingId == null) || (trackingId > 0 && x.TrackingId == trackingId))
                               && ((journeyStartDate != null && journeyEndDate != null) && (x.JourneyPlanDate >= journeyStartDate && x.JourneyPlanDate <= journeyEndDate))
                               && (string.IsNullOrEmpty(DriverName) || (x.Driver != null && (x.Driver.DriverName ?? "").Contains(DriverName)))
                               && (string.IsNullOrEmpty(vehicleno) || (x.Driver != null && (x.Driver.VehicleNumber ?? "").Contains(vehicleno)))
                               && (string.IsNullOrEmpty(driverCity) || (x.Driver != null && (x.Driver.City ?? "").ToLower() == driverCity))
                               && (string.IsNullOrEmpty(phoneNo) || (x.Driver != null && (x.Driver.DriverPhoneNumber ?? "").ToLower() == phoneNo))
                               && (string.IsNullOrEmpty(regdNo) || (x.OrderTrans.RegdNo ?? "").ToLower() == regdNo)
                               );
                if (count > 0)
                {
                    tblVJTD_List = await _context.TblVehicleJourneyTrackingDetails
                               .Include(x => x.ServicePartner)
                               .Include(x => x.Driver).Include(x => x.Status)
                               .Include(x => x.OrderTrans).ThenInclude(x => x.Exchange).ThenInclude(x => x.ProductType).ThenInclude(x => x.ProductCat)
                               .Include(x => x.OrderTrans).ThenInclude(x => x.Abbredemption).ThenInclude(x => x.Abbregistration).ThenInclude(x => x.NewProductCategoryTypeNavigation)
                               .Include(x => x.OrderTrans).ThenInclude(x => x.Abbredemption).ThenInclude(x => x.Abbregistration).ThenInclude(x => x.NewProductCategory)
                               .Include(x => x.OrderTrans).ThenInclude(x => x.Exchange).ThenInclude(x => x.CustomerDetails)
                               .Include(x => x.OrderTrans).ThenInclude(x => x.Abbredemption).ThenInclude(x => x.CustomerDetails)
                               .Where(x => x.IsActive == true && x.Driver != null && x.OrderTrans != null
                               && (x.StatusId != Convert.ToInt32(OrderStatusEnum.OrderAcceptedbyVehicle))
                               && (companyId > 0 && companyId == Convert.ToInt32(CompanyNameenum.UTC))
                               && ((servicePartnerId == 0 || servicePartnerId == null) || (servicePartnerId > 0 && x.ServicePartnerId == servicePartnerId))
                               && ((trackingId == 0 || trackingId == null) || (trackingId > 0 && x.TrackingId == trackingId))
                               && ((journeyStartDate != null && journeyEndDate != null) && (x.JourneyPlanDate >= journeyStartDate && x.JourneyPlanDate <= journeyEndDate))
                               && (string.IsNullOrEmpty(DriverName) || (x.Driver != null && (x.Driver.DriverName ?? "").Contains(DriverName)))
                               && (string.IsNullOrEmpty(vehicleno) || (x.Driver != null && (x.Driver.VehicleNumber ?? "").Contains(vehicleno)))
                               && (string.IsNullOrEmpty(driverCity) || (x.Driver != null && (x.Driver.City ?? "").ToLower() == driverCity))
                               && (string.IsNullOrEmpty(phoneNo) || (x.Driver != null && (x.Driver.DriverPhoneNumber ?? "").ToLower() == phoneNo))
                               && (string.IsNullOrEmpty(regdNo) || (x.OrderTrans.RegdNo ?? "").ToLower() == regdNo)
                               ).OrderByDescending(x => x.ModifiedDate).Skip(skip).Take(pageSize).ToListAsync();

                    recordsTotal = count;
                }
                #endregion

                #region Data Initialization for Datatable from table to Model
                if (tblVJTD_List != null && tblVJTD_List.Count > 0)
                {
                    MobileAppLogisticVMList = new List<MobileAppLogisticViewModel>();
                    MobileAppLogisticViewModel? mobileAppLogisticVM = null;
                    string actionURL = string.Empty;
                    TblLogistic? TblLogistic = null;

                    foreach (TblVehicleJourneyTrackingDetail item in tblVJTD_List)
                    {
                        if (item != null)
                        {
                            mobileAppLogisticVM = new MobileAppLogisticViewModel();
                            //mobileAppLogisticVM = _mapper.Map<TblLogistic, TblVehicleJourneyTrackingDetail>(item);
                            mobileAppLogisticVM.OrderDate = item.CreatedDate != null ? Convert.ToDateTime(item.CreatedDate).ToString("dd/MM/yyyy") : null;
                            mobileAppLogisticVM.LastUpdation = item.ModifiedDate != null ? Convert.ToDateTime(item.ModifiedDate).ToString("dd/MM/yyyy hh:mm") : null;
                            //mobileAppLogisticVM.OrderDate = item.Creat != null ? Convert.ToDateTime(item.ModifiedDate).ToString("dd/MM/yyyy") : null;
                            if (item.ServicePartner != null)
                            {
                                tblServicePartner = item.ServicePartner;
                                mobileAppLogisticVM.ServicePartnerBusinessName = tblServicePartner != null ? tblServicePartner.ServicePartnerBusinessName : null;
                            }
                            if (item.Driver != null)
                            {
                                tblDriverDetail = item.Driver;
                                mobileAppLogisticVM.DriverName = tblDriverDetail != null ? tblDriverDetail.DriverName : null;
                                mobileAppLogisticVM.VehicleNo = tblDriverDetail != null ? tblDriverDetail.VehicleNumber : null;
                                mobileAppLogisticVM.DriverPhoneNo = tblDriverDetail != null ? tblDriverDetail.DriverPhoneNumber : null;
                            }
                            if (item.Status != null)
                            {
                                mobileAppLogisticVM.StatusDesc = item.Status?.StatusDescription;
                                mobileAppLogisticVM.StatusCode = item.Status?.StatusCode;
                            }
                            TblOrderTran? tblOrderTrans = item.OrderTrans;
                            if (tblOrderTrans != null)
                            {
                                string? productCatDesc = null; string? productCatTypeDesc = null; string? custCityObj = null;
                                if (tblOrderTrans.Exchange != null && tblOrderTrans.Exchange?.ProductType != null)
                                {
                                    productCatDesc = tblOrderTrans.Exchange?.ProductType?.ProductCat != null ? tblOrderTrans.Exchange?.ProductType?.ProductCat?.Description : null;
                                    productCatTypeDesc = tblOrderTrans.Exchange?.ProductType?.Description;
                                    custCityObj = tblOrderTrans.Exchange?.CustomerDetails != null ? tblOrderTrans.Exchange?.CustomerDetails?.City : null;
                                }
                                else if (tblOrderTrans.Abbredemption != null && tblOrderTrans.Abbredemption?.Abbregistration != null)
                                {
                                    productCatDesc = tblOrderTrans.Abbredemption?.Abbregistration?.NewProductCategory != null ? tblOrderTrans.Abbredemption?.Abbregistration?.NewProductCategory?.Description : null;
                                    productCatTypeDesc = tblOrderTrans.Abbredemption?.Abbregistration?.NewProductCategoryTypeNavigation != null ? tblOrderTrans.Abbredemption?.Abbregistration?.NewProductCategoryTypeNavigation?.Description : null;
                                    custCityObj = tblOrderTrans.Abbredemption?.CustomerDetails != null ? tblOrderTrans.Abbredemption?.CustomerDetails?.City : null;
                                }
                                mobileAppLogisticVM.ProductCategory = productCatDesc;
                                mobileAppLogisticVM.ProductType = productCatTypeDesc;
                                mobileAppLogisticVM.CustCity = custCityObj;

                                TblLogistic = _context.TblLogistics.Where(x => x.IsActive == true && x.OrderTransId == tblOrderTrans.OrderTransId).FirstOrDefault();
                                if (TblLogistic != null)
                                {
                                    mobileAppLogisticVM.RegdNo = TblLogistic.RegdNo;
                                    mobileAppLogisticVM.TicketNumber = TblLogistic.TicketNumber;
                                }
                            }

                            mobileAppLogisticVM.Earning = item.Total;
                            mobileAppLogisticVM.EstimatedEarning = item.EstimateEarning;
                            //actionURL = " <ul class='actions'>";
                            //actionURL = "<a href ='" + URL + "/LGC/LGCPickup?regdNo=" + item.RegdNo + "' ><button onclick='View(" + item.RegdNo + ")' class='btn btn-primary btn'>View</button></a>";
                            //actionURL = actionURL + "</ul>";

                            //actionURL = " <ul class='actions'>";
                            //actionURL = actionURL + "<a href ='" + URL + "/Exchange/Manage?id=" + _protector.Encode(item.Id) + "' onclick='RecordView(" + item.Id + ")' class='btn btn-primary btn-sm viewableWithAddPermission' data-bs-toggle='tooltip' data-bs-placement='top' title='View'><i class='fa-solid fa-eye'></i></a>&nbsp;";
                            //actionURL = "<a href ='" + URL + "/LGC/LGCPickup?regdNo=" + item.RegdNo + "' ><button onclick='View(" + item.RegdNo + ")' class='btn btn-primary btn'>View</button></a>";
                            //actionURL = actionURL + "</ul>";
                            //item.Action = actionURL;

                            actionURL = " <div class='actionbtns'>";
                            actionURL = actionURL + " <a class='btn btn-sm btn-primary' href='" + URL + "/Index1?orderTransId=" + item.OrderTransId + "' title='TimeLine'><i class='fa solid fa-clock-rotate-left'></i></a>&nbsp;";
                            actionURL = actionURL + "<a href ='" + URL + "/LGC_Admin/OrderViewPage?OrderTransId=" + item.OrderTransId + "' onclick='RecordView(" + item.OrderTransId + ")' class='btn btn-primary btn-sm viewableWithAddPermission' data-bs-toggle='tooltip' data-bs-placement='top' title='View'><i class='fa-solid fa-eye'></i></a>&nbsp;";
                            actionURL = actionURL + "<a href ='" + URL + "/LGCOrderTracking/WalletDetails?TrackingDetailsId=" + item.TrackingDetailsId + "' onclick='RecordView(" + item.OrderTransId + ")' class='btn btn-primary btn-sm viewableWithAddPermission' data-bs-toggle='tooltip' data-bs-placement='top' title='View Wallet Details'><i class='fa-solid fa-eye'></i></a>&nbsp;";
                            actionURL = actionURL + "</div>";

                            mobileAppLogisticVM.Action = actionURL;
                            MobileAppLogisticVMList.Add(mobileAppLogisticVM);

                        }
                    }
                }
                #endregion
                var data = MobileAppLogisticVMList;
                var jsonData = new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data };
                return Ok(jsonData);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        #endregion

        #region View list of Accept order by Driver list Added by Pooja Jatav
        [HttpPost]
        public async Task<ActionResult> AcceptedOrderDetails(int companyId, int? driverDetailsId, int? trackingId, int? servicePartnerId, DateTime? orderStartDate,
            DateTime? orderEndDate, string? regdNo, string? driverphoneNo, string? DriverName, string? vehicleno, string? driverCity)
        {
            #region Parameters Simplification
            if (!string.IsNullOrWhiteSpace(regdNo) && regdNo != "null")
            { regdNo = regdNo.Trim().ToLower(); }
            else { regdNo = null; }
            if (!string.IsNullOrWhiteSpace(driverphoneNo) && driverphoneNo != "null")
            { driverphoneNo = driverphoneNo.Trim().ToLower(); }
            else { driverphoneNo = null; }
            if (!string.IsNullOrWhiteSpace(DriverName) && DriverName != "null")
            { DriverName = DriverName.Trim().ToLower(); }
            else { DriverName = null; }
            if (!string.IsNullOrWhiteSpace(vehicleno) && vehicleno != "null")
            { vehicleno = vehicleno.Trim().ToLower(); }
            else { vehicleno = null; }
            if (!string.IsNullOrWhiteSpace(driverCity) && driverCity != "null")
            { driverCity = driverCity.Trim().ToLower(); }
            else { driverCity = null; }
            #endregion

            #region Variable Declaration
            List<TblLogistic>? TblLogistics = null;
            string InvoiceimagURL = string.Empty;
            TblServicePartner? tblServicePartner = null;
            TblDriverDetail? tblDriverDetail = null;
            List<TblVehicleJourneyTrackingDetail>? tblVJTD_List = null;
            List<MobileAppLogisticViewModel>? MobileAppLogisticVMList = null;
            int count = 0;
            int PickupCount = 0;
            int DropCount = 0;
            #endregion

            try
            {
                #region Variable Initialization
                string? URL = _config.Value.URLPrefixforProd;
                string? MVCURL = _config.Value.MVCBaseURLForExchangeInvoice;
                #endregion

                #region Datatable form variables
                var draw = Request.Form["draw"].FirstOrDefault();
                var start = Request.Form["start"].FirstOrDefault();
                var length = Request.Form["length"].FirstOrDefault();
                var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
                var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();
                var searchValue = Request.Form["search[value]"].FirstOrDefault();
                int pageSize = length != null ? Convert.ToInt32(length) : 0;
                int skip = start != null ? Convert.ToInt32(start) : 0;
                int recordsTotal = 0;
                #endregion

                #region Advanced Filters Mapping
                if (orderStartDate != null && orderEndDate != null)
                {
                    orderStartDate = Convert.ToDateTime(orderStartDate).AddMinutes(-1);
                    orderEndDate = Convert.ToDateTime(orderEndDate).AddDays(1).AddSeconds(-1);
                }
                #endregion

                #region table object Initialization
                count = _context.TblVehicleJourneyTrackingDetails
                               .Include(x => x.ServicePartner)
                               .Include(x => x.Driver).Include(x => x.Status)
                               .Include(x => x.OrderTrans).ThenInclude(x => x.Exchange).ThenInclude(x => x.ProductType).ThenInclude(x => x.ProductCat)
                               .Include(x => x.OrderTrans).ThenInclude(x => x.Abbredemption).ThenInclude(x => x.Abbregistration).ThenInclude(x => x.NewProductCategoryTypeNavigation)
                               .Include(x => x.OrderTrans).ThenInclude(x => x.Abbredemption).ThenInclude(x => x.Abbregistration).ThenInclude(x => x.NewProductCategory)
                               .Include(x => x.OrderTrans).ThenInclude(x => x.Exchange).ThenInclude(x => x.CustomerDetails)
                               .Include(x => x.OrderTrans).ThenInclude(x => x.Abbredemption).ThenInclude(x => x.CustomerDetails)
                               .Count(x => x.IsActive == true && x.Driver != null && x.OrderTrans != null
                               && (companyId > 0 && companyId == Convert.ToInt32(CompanyNameenum.UTC))
                               && (x.StatusId == Convert.ToInt32(OrderStatusEnum.OrderAcceptedbyVehicle))
                               && (x.PickupStartDatetime == null)
                               && (x.DriverId != null && x.DriverId > 0 && x.DriverId == driverDetailsId)
                               && ((servicePartnerId == 0 || servicePartnerId == null) || (servicePartnerId > 0 && x.ServicePartnerId == servicePartnerId))
                               && ((orderStartDate == null && orderEndDate == null) || (x.CreatedDate >= orderStartDate && x.CreatedDate<= orderEndDate))                               
                               && (string.IsNullOrEmpty(DriverName) || (x.Driver != null && x.Driver.DriverName.Contains(DriverName)))
                               && (string.IsNullOrEmpty(vehicleno) || (x.Driver != null && x.Driver.VehicleNumber.Contains(vehicleno)))
                               && (string.IsNullOrEmpty(driverCity) || (x.Driver != null && (x.Driver.City ?? "").ToLower() == driverCity))
                               && (string.IsNullOrEmpty(driverphoneNo) || (x.Driver != null && (x.Driver.DriverPhoneNumber ?? "").ToLower() == driverphoneNo))
                               && (string.IsNullOrEmpty(regdNo) || (x.OrderTrans.RegdNo ?? "").ToLower() == regdNo)

                               );
                if (count > 0)
                {
                    tblVJTD_List = await _context.TblVehicleJourneyTrackingDetails
                               .Include(x => x.ServicePartner)
                               .Include(x => x.Driver).Include(x => x.Status)
                               .Include(x => x.OrderTrans).ThenInclude(x => x.Exchange).ThenInclude(x => x.ProductType).ThenInclude(x => x.ProductCat)
                               .Include(x => x.OrderTrans).ThenInclude(x => x.Abbredemption).ThenInclude(x => x.Abbregistration).ThenInclude(x => x.NewProductCategoryTypeNavigation)
                               .Include(x => x.OrderTrans).ThenInclude(x => x.Abbredemption).ThenInclude(x => x.Abbregistration).ThenInclude(x => x.NewProductCategory)
                               .Include(x => x.OrderTrans).ThenInclude(x => x.Exchange).ThenInclude(x => x.CustomerDetails)
                               .Include(x => x.OrderTrans).ThenInclude(x => x.Abbredemption).ThenInclude(x => x.CustomerDetails)
                               .Where(x => x.IsActive == true && x.Driver != null && x.OrderTrans != null
                               && (companyId > 0 && companyId == Convert.ToInt32(CompanyNameenum.UTC))
                               && (x.StatusId == Convert.ToInt32(OrderStatusEnum.OrderAcceptedbyVehicle))
                               && (x.PickupStartDatetime == null)
                               && (x.DriverId != null && x.DriverId > 0 && x.DriverId == driverDetailsId)
                               && ((servicePartnerId == 0 || servicePartnerId == null) || (servicePartnerId > 0 && x.ServicePartnerId == servicePartnerId))
                               && ((orderStartDate == null && orderEndDate == null) || (x.CreatedDate >= orderStartDate && x.CreatedDate <= orderEndDate))
                               && (string.IsNullOrEmpty(DriverName) || (x.Driver != null && x.Driver.DriverName.Contains(DriverName)))
                               && (string.IsNullOrEmpty(vehicleno) || (x.Driver != null && x.Driver.VehicleNumber.Contains(vehicleno)))
                               && (string.IsNullOrEmpty(driverCity) || (x.Driver != null && (x.Driver.City ?? "").ToLower() == driverCity))
                               && (string.IsNullOrEmpty(driverphoneNo) || (x.Driver != null && (x.Driver.DriverPhoneNumber ?? "").ToLower() == driverphoneNo))
                               && (string.IsNullOrEmpty(regdNo) || (x.OrderTrans.RegdNo ?? "").ToLower() == regdNo)
                               ).OrderByDescending(x => x.ModifiedDate).ThenByDescending(x => x.TrackingDetailsId).Skip(skip).Take(pageSize).ToListAsync();

                    recordsTotal = count;
                }
                #endregion

                #region Data Initialization for Datatable from table to Model
                if (tblVJTD_List != null && tblVJTD_List.Count > 0)
                {
                    MobileAppLogisticVMList = new List<MobileAppLogisticViewModel>();
                    MobileAppLogisticViewModel? mobileAppLogisticVM = null;
                    string actionURL = string.Empty;
                    TblLogistic? TblLogistic = null;

                    foreach (TblVehicleJourneyTrackingDetail item in tblVJTD_List)
                    {
                        if (item != null)
                        {
                            mobileAppLogisticVM = new MobileAppLogisticViewModel();
                            mobileAppLogisticVM.OrderDate = item.CreatedDate != null ? Convert.ToDateTime(item.CreatedDate).ToString("dd/MM/yyyy") : null;
                            mobileAppLogisticVM.OrderAssignedDate = item.CreatedDate != null ? Convert.ToDateTime(item.CreatedDate).ToString("dd/MM/yyyy") : null;
                            
                            mobileAppLogisticVM.LastUpdation = item.ModifiedDate != null ? Convert.ToDateTime(item.ModifiedDate).ToString("dd/MM/yyyy hh:mm") : null;
                            if (item.ServicePartner != null)
                            {
                                tblServicePartner = item.ServicePartner;
                                mobileAppLogisticVM.ServicePartnerBusinessName = tblServicePartner != null ? tblServicePartner.ServicePartnerBusinessName : null;
                            }
                            if (item.Driver != null)
                            {
                                tblDriverDetail = item.Driver;
                                mobileAppLogisticVM.DriverName = tblDriverDetail != null ? tblDriverDetail.DriverName : null;
                                mobileAppLogisticVM.VehicleNo = tblDriverDetail != null ? tblDriverDetail.VehicleNumber : null;
                                mobileAppLogisticVM.DriverPhoneNo = tblDriverDetail != null ? tblDriverDetail.DriverPhoneNumber : null;
                            }
                            if (item.Status != null)
                            {
                                mobileAppLogisticVM.StatusDesc = item.Status?.StatusDescription;
                                mobileAppLogisticVM.StatusCode = item.Status?.StatusCode;
                            }
                            if(item.PickupEndDatetime != null)
                            {
                                mobileAppLogisticVM.PickupCount += PickupCount;
                            }
                            if(item.OrderDropTime != null)
                            {
                                mobileAppLogisticVM.DropCount += DropCount;
                            }
                            TblOrderTran? tblOrderTrans = item.OrderTrans;
                            if (tblOrderTrans != null)
                            {
                                string? productCatDesc = null; string? productCatTypeDesc = null; string? custCityObj = null;
                                if (tblOrderTrans.Exchange != null && tblOrderTrans.Exchange?.ProductType != null)
                                {
                                    productCatDesc = tblOrderTrans.Exchange?.ProductType?.ProductCat != null ? tblOrderTrans.Exchange?.ProductType?.ProductCat?.Description : null;
                                    productCatTypeDesc = tblOrderTrans.Exchange?.ProductType?.Description;
                                    custCityObj = tblOrderTrans.Exchange?.CustomerDetails != null ? tblOrderTrans.Exchange?.CustomerDetails?.City : null;
                                }
                                else if (tblOrderTrans.Abbredemption != null && tblOrderTrans.Abbredemption?.Abbregistration != null)
                                {
                                    productCatDesc = tblOrderTrans.Abbredemption?.Abbregistration?.NewProductCategory != null ? tblOrderTrans.Abbredemption?.Abbregistration?.NewProductCategory?.Description : null;
                                    productCatTypeDesc = tblOrderTrans.Abbredemption?.Abbregistration?.NewProductCategoryTypeNavigation != null ? tblOrderTrans.Abbredemption?.Abbregistration?.NewProductCategoryTypeNavigation?.Description : null;
                                    custCityObj = tblOrderTrans.Abbredemption?.CustomerDetails != null ? tblOrderTrans.Abbredemption?.CustomerDetails?.City : null;
                                }
                                mobileAppLogisticVM.ProductCategory = productCatDesc;
                                mobileAppLogisticVM.ProductType = productCatTypeDesc;
                                mobileAppLogisticVM.CustCity = custCityObj;

                                TblLogistic = _context.TblLogistics.Where(x => x.IsActive == true && x.OrderTransId == tblOrderTrans.OrderTransId).FirstOrDefault();
                                if (TblLogistic != null)
                                {
                                    mobileAppLogisticVM.RegdNo = TblLogistic.RegdNo;
                                    mobileAppLogisticVM.TicketNumber = TblLogistic.TicketNumber;
                                }
                            }

                            mobileAppLogisticVM.Earning = item.Total;
                            mobileAppLogisticVM.EstimatedEarning = item.EstimateEarning;

                            actionURL = " <div class='actionbtns'>";
                            actionURL = actionURL + " <a class='btn btn-sm btn-primary' href='" + URL + "/Index1?orderTransId=" + item.OrderTransId + "' title='TimeLine'><i class='fa solid fa-clock-rotate-left'></i></a>&nbsp;";
                            actionURL = actionURL + "<a href ='" + URL + "/LGC_Admin/OrderViewPage?OrderTransId=" + item.OrderTransId + "' onclick='RecordView(" + item.OrderTransId + ")' class='btn btn-primary btn-sm viewableWithAddPermission' data-bs-toggle='tooltip' data-bs-placement='top' title='View'><i class='fa-solid fa-eye'></i></a>&nbsp;";
                            actionURL = actionURL + "</div>";

                            mobileAppLogisticVM.Action = actionURL;
                            MobileAppLogisticVMList.Add(mobileAppLogisticVM);

                        }
                    }
                }
                #endregion
                var data = MobileAppLogisticVMList;
                var jsonData = new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data };
                return Ok(jsonData);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        #endregion

        #region All Driver List Added by Pooja
        [HttpPost]
        public async Task<ActionResult> AllDriverList(int companyId, int SPId, string? servicepartnerName, string? driverName, string? driverphoneNo, string? driverCity)
        {
            #region Parameters Simplification

            if (!string.IsNullOrWhiteSpace(servicepartnerName) && servicepartnerName != "null")
            { servicepartnerName = servicepartnerName.Trim().ToLower(); }
            else { servicepartnerName = null; }

            if (!string.IsNullOrWhiteSpace(driverphoneNo) && driverphoneNo != "null")
            { driverphoneNo = driverphoneNo.Trim().ToLower(); }
            else { driverphoneNo = null; }

            if (!string.IsNullOrWhiteSpace(driverName) && driverName != "null")
            { driverName = driverName.Trim().ToLower(); }
            else { driverName = null; }

            if (!string.IsNullOrWhiteSpace(driverCity) && driverCity != "null")
            { driverCity = driverCity.Trim().ToLower(); }
            else { driverCity = null; }
            #endregion

            #region Variable Declaration
            List<TblDriverList>? TblDriverLists = null;
            string URL = _config.Value.URLPrefixforProd;
            string MVCURL = _config.Value.MVCBaseURLForExchangeInvoice;
            string BaseURL = _config.Value.BaseURL;
            string InvoiceimagURL = string.Empty;
            List<TblDriverList>? tblallDriverLists = null;
            TblCity? tblCity = null;
            TblServicePartner tblServicePartner = null;
            TblUser tblUser = null;
            int count = 0;
            TblCompany? tblCompany = null;
            TblBusinessUnit? tblBusinessUnit = null;
            List<DriverListViewModel>? DriverListVM = null;
            int servicePartnerId = 0;
            #endregion

            try
            {
                #region Datatable form variables
                var draw = Request.Form["draw"].FirstOrDefault();
                var start = Request.Form["start"].FirstOrDefault();
                var length = Request.Form["length"].FirstOrDefault();
                var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
                var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();
                var searchValue = Request.Form["search[value]"].FirstOrDefault();
                int pageSize = length != null ? Convert.ToInt32(length) : 0;
                int skip = start != null ? Convert.ToInt32(start) : 0;
                int recordsTotal = 0;
                #endregion

                #region tblServicePartner Check
                tblServicePartner = _context.TblServicePartners.Where(x => x.IsActive == true && x.ServicePartnerId == SPId).FirstOrDefault();
                if (tblServicePartner != null)
                {
                    servicePartnerId = tblServicePartner.ServicePartnerId;
                }

                #endregion

                #region table object Initialization
                count = _context.TblDriverLists
                               .Include(x => x.City)
                               .Include(x => x.ServicePartner)
                               .Include(x => x.User)
                               .Count(x => x.IsActive == true && x.IsApproved == true                              
                               && ((companyId > 0 && companyId == Convert.ToInt32(CompanyNameenum.UTC))
                                && ((servicePartnerId == 0 || servicePartnerId == null) || (servicePartnerId > 0 && x.ServicePartnerId == servicePartnerId))
                                && (string.IsNullOrEmpty(servicepartnerName) || (x.ServicePartner != null && (x.ServicePartner.ServicePartnerBusinessName ?? "").Contains(servicepartnerName)))
                               && (string.IsNullOrEmpty(driverName) || ((x.DriverName ?? "").ToLower().Contains(driverName)))
                               && (string.IsNullOrEmpty(driverCity) || ((x.City.Name ?? "").ToLower() == driverCity))
                               && (string.IsNullOrEmpty(driverphoneNo) || ((x.DriverPhoneNumber ?? "").Contains(driverphoneNo)))
                               ));
                if (count > 0)
                {
                    tblallDriverLists = await _context.TblDriverLists
                               .Include(x => x.City)
                               .Include(x => x.ServicePartner)
                               .Include(x => x.User)
                               .Where(x => x.IsActive == true && x.IsApproved == true                               
                               && ((companyId > 0 && companyId == Convert.ToInt32(CompanyNameenum.UTC))
                               && ((servicePartnerId == 0 || servicePartnerId == null) || (servicePartnerId > 0 && x.ServicePartnerId == servicePartnerId))
                               && (string.IsNullOrEmpty(servicepartnerName) || (x.ServicePartner != null && (x.ServicePartner.ServicePartnerBusinessName ?? "").Contains(servicepartnerName)))
                               && (string.IsNullOrEmpty(driverName) || ((x.DriverName ?? "").ToLower().Contains(driverName)))
                               && (string.IsNullOrEmpty(driverCity) || ((x.City.Name ?? "").ToLower() == driverCity))
                               && (string.IsNullOrEmpty(driverphoneNo) || ((x.DriverPhoneNumber ?? "").Contains(driverphoneNo)))
                               )).OrderByDescending(x => x.CreatedDate).ThenByDescending(x => x.DriverId).Skip(skip).Take(pageSize).ToListAsync();

                    recordsTotal = count;
                }
                #endregion

                #region Data Initialization for Datatable from table to Model
                if (tblallDriverLists != null && tblallDriverLists.Count > 0)
                {
                    DriverListVM = new List<DriverListViewModel>();
                    string actionURL = string.Empty;
                    string Driverlicense = string.Empty;
                    string DriverProfile = string.Empty;
                    DriverListViewModel? driverVM = null;
                    foreach (TblDriverList item in tblallDriverLists)
                    {
                        driverVM = new DriverListViewModel();
                        driverVM = _mapper.Map<TblDriverList, DriverListViewModel>(item);
                       
                        if (item?.CityId > 0)
                        {
                            tblCity = item.City;
                            driverVM.CityName = tblCity != null ? tblCity.Name : null;
                        }

                        if (item?.ServicePartner != null && item?.ServicePartnerId > 0)
                        {
                            tblServicePartner = item.ServicePartner;
                            driverVM.ServicePartnerName = tblServicePartner.ServicePartnerBusinessName;
                        }

                        if(item?.User != null && item?.ApprovedBy > 0)
                        {
                            tblUser= item.User;
                            driverVM.ApprovedByName = tblUser.FirstName + "" + tblUser.LastName; 
                        }

                        DriverProfile = BaseURL +"/"+ _config.Value.ProfilePicture + item.ProfilePicture;
                        DriverProfile = "<img src='" + DriverProfile + "' class='img-responsive'/>";
                        driverVM.ProfilePicture = DriverProfile;

                        Driverlicense = BaseURL + _config.Value.DriverlicenseImage + item.DriverLicenseImage;
                        Driverlicense = "<img src='" + Driverlicense + "' class='img-responsive'/>";
                        driverVM.DriverLicenseImage = Driverlicense;

                        actionURL = " <div class='actionbtns'>";
                        actionURL += "<a href ='" + URL + "/LGCOrderTracking/DriverList?DriverId=" + item?.DriverId + "' class='btn btn-primary btn-sm viewableWithAddPermission' data-bs-toggle='tooltip' data-bs-placement='top' title='View'><i class='fa-solid fa-eye'></i></a>&nbsp;";
                        driverVM.Action = actionURL;

                       

                        DriverListVM.Add(driverVM);
                    }
                }
                #endregion
                var data = DriverListVM;
                var jsonData = new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data };
                return Ok(jsonData);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        #endregion

        #region All Vehicle List Added by Pooja
        [HttpPost]
        public async Task<ActionResult> AllVehicleList(int companyId, int SPId, string? servicepartnerName, string? vehiclenumber, string? vehicleRcnumber, string? Vehiclecity)
        {
            #region Parameters Simplification
            if (!string.IsNullOrWhiteSpace(servicepartnerName) && servicepartnerName != "null")
            { servicepartnerName = servicepartnerName.Trim().ToLower(); }
            else { servicepartnerName = null; }

            if (!string.IsNullOrWhiteSpace(vehiclenumber) && vehiclenumber != "null")
            { vehiclenumber = vehiclenumber.Trim().ToLower(); }
            else { vehiclenumber = null; }

            if (!string.IsNullOrWhiteSpace(vehicleRcnumber) && vehicleRcnumber != "null")
            { vehicleRcnumber = vehicleRcnumber.Trim().ToLower(); }
            else { vehicleRcnumber = null; }

            if (!string.IsNullOrWhiteSpace(Vehiclecity) && Vehiclecity != "null")
            { Vehiclecity = Vehiclecity.Trim().ToLower(); }
            else { Vehiclecity = null; }
            #endregion

            #region Variable Declaration
            List<TblVehicleList>? TblVehicleLists = null;
            string URL = _config.Value.URLPrefixforProd;
            string MVCURL = _config.Value.MVCBaseURLForExchangeInvoice;
            string BaseURL = _config.Value.BaseURL;
            string InvoiceimagURL = string.Empty;
            List<TblVehicleList>? tblallVehicleLists = null;
            TblServicePartner tblServicePartner = null;
            TblCity? tblCity = null;
            int count = 0;
            TblCompany? tblCompany = null;
            TblBusinessUnit? tblBusinessUnit = null;
            List<VehicleViewModel>? VehicleListVM = null;
            int servicePartnerId = 0;
            TblUser tblUser = null;
            #endregion

            try
            {
                #region Datatable form variables
                var draw = Request.Form["draw"].FirstOrDefault();
                var start = Request.Form["start"].FirstOrDefault();
                var length = Request.Form["length"].FirstOrDefault();
                var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
                var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();
                var searchValue = Request.Form["search[value]"].FirstOrDefault();
                int pageSize = length != null ? Convert.ToInt32(length) : 0;
                int skip = start != null ? Convert.ToInt32(start) : 0;
                int recordsTotal = 0;
                #endregion

                #region tblServicePartner Check
                tblServicePartner = _context.TblServicePartners.Where(x => x.IsActive == true && x.ServicePartnerId == SPId).FirstOrDefault();
                if (tblServicePartner != null)
                {
                    servicePartnerId = tblServicePartner.ServicePartnerId;
                }
                #endregion

                #region table object Initialization
                count = _context.TblVehicleLists
                               .Include(x => x.ServicePartner).ThenInclude(y => y.User)
                               .Include(x => x.City)
                               .Count(x => x.IsActive == true && x.IsApproved == true
                               && ((companyId > 0 && companyId == Convert.ToInt32(CompanyNameenum.UTC))
                               && ((servicePartnerId == 0 || servicePartnerId == null) || (servicePartnerId > 0 && x.ServicePartnerId == servicePartnerId))
                               && (string.IsNullOrEmpty(servicepartnerName) || (x.ServicePartner != null && (x.ServicePartner.ServicePartnerBusinessName ?? "").Contains(servicepartnerName)))
                               && (string.IsNullOrEmpty(vehiclenumber) || ((x.VehicleNumber ?? "").ToLower().Contains(vehiclenumber)))
                               && (string.IsNullOrEmpty(Vehiclecity) || ((x.City.Name ?? "").ToLower() == Vehiclecity))
                               && (string.IsNullOrEmpty(vehicleRcnumber) || ((x.VehicleRcNumber ?? "").Contains(vehicleRcnumber)))
                               ));
                if (count > 0)
                {
                    tblallVehicleLists = await _context.TblVehicleLists
                               .Include(x => x.ServicePartner).ThenInclude(y => y.User)
                               .Include(x => x.City)
                               .Where(x => x.IsActive == true && x.IsApproved == true
                               && ((companyId > 0 && companyId == Convert.ToInt32(CompanyNameenum.UTC))
                               && ((servicePartnerId == 0 || servicePartnerId == null) || (servicePartnerId > 0 && x.ServicePartnerId == servicePartnerId))
                               && (string.IsNullOrEmpty(servicepartnerName) || (x.ServicePartner != null && (x.ServicePartner.ServicePartnerBusinessName ?? "").Contains(servicepartnerName)))
                               && (string.IsNullOrEmpty(vehiclenumber) || ((x.VehicleNumber ?? "").ToLower().Contains(vehiclenumber)))
                               && (string.IsNullOrEmpty(Vehiclecity) || ((x.City.Name ?? "").ToLower() == Vehiclecity))
                               && (string.IsNullOrEmpty(vehicleRcnumber) || ((x.VehicleNumber ?? "").Contains(vehicleRcnumber)))
                               )).OrderByDescending(x => x.CreatedDate).ThenByDescending(x => x.VehicleId).Skip(skip).Take(pageSize).ToListAsync();

                    recordsTotal = count;
                }
                #endregion

                #region Data Initialization for Datatable from table to Model
                if (tblallVehicleLists != null && tblallVehicleLists.Count > 0)
                {
                    VehicleListVM = new List<VehicleViewModel>();
                    string fitnessCertificate = string.Empty;
                    string InsuranceCertificate = string.Empty;
                    string RCCertificate = string.Empty;
                    string actionURL = string.Empty;
                    VehicleViewModel? VehicleVM = null;
                    foreach (TblVehicleList item in tblallVehicleLists)
                    {
                        VehicleVM = new VehicleViewModel();
                        VehicleVM = _mapper.Map<TblVehicleList, VehicleViewModel>(item);

                        if (item?.CityId > 0)
                        {
                            tblCity = item.City;
                            VehicleVM.CityName = tblCity != null ? tblCity.Name : null;
                        }

                        if (item?.ServicePartner != null && item?.ServicePartnerId > 0)
                        {
                            tblServicePartner = item.ServicePartner;
                            VehicleVM.ServicePartnerName = tblServicePartner.ServicePartnerBusinessName;
                        }

                        if (item?.ServicePartner?.User != null && item?.ApprovedBy > 0)
                        {
                            tblUser = item.ServicePartner.User;
                            VehicleVM.ApprovedByName = tblUser.FirstName + "" + tblUser.LastName;
                        }

                        RCCertificate = BaseURL + _config.Value.VehicleRcCertificate + item.VehicleRcCertificate;
                        RCCertificate = "<img src='" + RCCertificate + "' class='img-responsive'/>";
                        VehicleVM.VehicleRCCertificate = RCCertificate;

                        fitnessCertificate = BaseURL + _config.Value.VehiclefitnessCertificate + item.VehiclefitnessCertificate;
                        fitnessCertificate = "<img src='" + fitnessCertificate + "' class='img-responsive'/>";
                        VehicleVM.VehiclefitnessCertificate = fitnessCertificate;

                        InsuranceCertificate = BaseURL + _config.Value.VehicleInsuranceCertificate + item.VehicleInsuranceCertificate;
                        InsuranceCertificate = "<img src='" + InsuranceCertificate + "' class='img-responsive'/>";
                        VehicleVM.VehicleInsuranceCertificate = InsuranceCertificate;


                        actionURL = " <div class='actionbtns'>";
                        actionURL += "<a href ='" + URL + "/LGCOrderTracking/VehicleList?VehicleId=" + item.VehicleId + "' class='btn btn-primary btn-sm viewableWithAddPermission' data-bs-toggle='tooltip' data-bs-placement='top' title='View'><i class='fa-solid fa-eye'></i></a>&nbsp;";
                        VehicleVM.Action = actionURL;
                        VehicleListVM.Add(VehicleVM);
                    }
                }
                #endregion
                var data = VehicleListVM;
                var jsonData = new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data };
                return Ok(jsonData);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        #endregion

    }
}
