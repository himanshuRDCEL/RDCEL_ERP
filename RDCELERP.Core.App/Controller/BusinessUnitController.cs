using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RDCELERP.BAL.Interface;
using RDCELERP.Common.Constant;
using RDCELERP.Common.Enums;
using RDCELERP.Common.Helper;
using RDCELERP.DAL.Entities;
using RDCELERP.DAL.IRepository;
using RDCELERP.DAL.Repository;
using RDCELERP.Model.Base;
using RDCELERP.Model.DealerDashBoard;
using RDCELERP.Model.ExchangeOrder;
using RDCELERP.Model.OrderDetails;
using RDCELERP.Model.OrderTrans;
using RDCELERP.Model.Users;

namespace RDCELERP.Core.App.Controller
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class BusinessUnitController : ControllerBase
    {
        private Digi2l_DevContext _context;
        private IMapper _mapper;
        private CustomDataProtection _protector;
        private IOptions<ApplicationSettings> _config;
        private LoginViewModel _loginSession;
        public IDealerManager _dealerManager;
        public ILogging _logging;
        private IOrderTransRepository _orderTransRepository;
        private IOrderQCRepository _orderQCRepository;
        private ICompanyRepository _companyRepository;
        private IBusinessUnitRepository _businessUnitRepository;
        private ITemplateConfigurationRepository _templateConfigurationRepository;
        public BusinessUnitController(IMapper mapper, Digi2l_DevContext context, CustomDataProtection protector, IOptions<ApplicationSettings> config, IDealerManager dealerManager, ILogging logging, IOrderTransRepository orderTransRepository, IOrderQCRepository orderQCRepository, ICompanyRepository companyRepository, IBusinessUnitRepository businessUnitRepository, ITemplateConfigurationRepository templateConfigurationRepository)
        {
            _context = context;
            _mapper = mapper;
            _protector = protector;
            _config = config;
            _dealerManager = dealerManager;
            _logging = logging;
            _orderTransRepository = orderTransRepository;
            _orderQCRepository = orderQCRepository;
            _companyRepository = companyRepository;
            _businessUnitRepository = businessUnitRepository;
            _templateConfigurationRepository = templateConfigurationRepository;
        }
        [HttpPost]
        public JsonResult OrderDataTableForDealer(ExchangeOrderDataContract exchageOrderObj)
        //public async Task<ActionResult> OrderDataTableForDealer(ExchangeOrderDataContract exchageOrderObj)
        { 
            var draw = Request.Form["draw"].FirstOrDefault();
            var start = Request.Form["start"].FirstOrDefault();
            var length = Request.Form["length"].FirstOrDefault();
            var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
            var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();
            var searchValue = Request.Form["search[value]"].FirstOrDefault();
            searchValue = searchValue.Trim();
            int pageSize = length != null ? Convert.ToInt32(length) : 0;
            int skip = start != null ? Convert.ToInt32(start) : 0;
            int recordsTotal = 0;
            List<ExchangeOrderDataContract> exchangeObj = new List<ExchangeOrderDataContract>();
            List<DealerDashboardViewModel> dealerdatalist = null;
            try
            {
                if (exchageOrderObj.startdate != null && exchageOrderObj.enddate != null)
                {
                    exchageOrderObj.StartRangedate = Convert.ToDateTime(exchageOrderObj.startdate);
                    exchageOrderObj.EndRangeDate = Convert.ToDateTime(exchageOrderObj.enddate);
                }


                dealerdatalist = _dealerManager.GetDashboardList(exchageOrderObj, skip, pageSize);

                

            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("DealerDashboardModel", "OnGetStoreList", ex);
            }

            var data = dealerdatalist;
            var jsonData = new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data };
            return new JsonResult(jsonData) ;
        }

        #region Pending for QC List Created by VK
        [HttpPost]
        public async Task<ActionResult> PendingForQC(int companyId, DateTime? orderStartDate,
            DateTime? orderEndDate, string? companyName, string? regdNo, int? productCatId,
            int? productTypeId, int? orderTypeId, string? custCity)
        {
            #region Variable Declaration
            if (!string.IsNullOrWhiteSpace(companyName) && companyName != "null")
            { companyName = companyName.Trim().ToLower(); }
            else { companyName = null; }
            if (!string.IsNullOrWhiteSpace(regdNo) && regdNo != "null")
            { regdNo = regdNo.Trim().ToLower(); }
            else { regdNo = null; }
            //if (!string.IsNullOrWhiteSpace(phoneNo) && phoneNo != "null")
            //{ phoneNo = phoneNo.Trim().ToLower(); }
            //else { phoneNo = null; }
            if (!string.IsNullOrWhiteSpace(custCity) && custCity != "null")
            { custCity = custCity.Trim().ToLower(); }
            else { custCity = null; }

            List<TblExchangeOrder> TblExchangeOrders = null;
            string URL = _config.Value.URLPrefixforProd;
            string MVCURL = _config.Value.MVCBaseURLForExchangeInvoice;
            string InvoiceimagURL = string.Empty;
            TblCompany tblCompany = null;
            TblBusinessUnit tblBusinessUnit = null;
            List<OrderDetailsViewModel> OrderDetailsList = new List<OrderDetailsViewModel>(); 
            int count = 0;
            TblOrderTran? tblOrderTran = null;
            List<TblOrderTran>? tblOrderTranList = null;
            int? ElapsedHrs = 0;
            int? orderPendingTimeH = null;
            TblConfiguration? tblConfiguration = null;
            //int count = 0;
            #endregion

            try
            {
                #region Datatable Variables
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
                if (companyId > 0 && companyId != 1007)
                {
                    tblCompany = _companyRepository.GetCompanyId(companyId);
                    if (tblCompany != null)
                    {
                        tblBusinessUnit = _businessUnitRepository.Getbyid(tblCompany.BusinessUnitId);
                    }
                }
                if (orderStartDate != null && orderEndDate != null)
                {
                    orderStartDate = Convert.ToDateTime(orderStartDate).AddMinutes(-1);
                    orderEndDate = Convert.ToDateTime(orderEndDate).AddDays(1).AddSeconds(-1);
                }
                #endregion

                #region Get Orders Pending Time from Configuration
                if(tblBusinessUnit == null)
                {
                    tblConfiguration = _templateConfigurationRepository.GetConfigByKeyName(ConfigurationEnum.OrderPendingTimeH.ToString());
                    if (tblConfiguration != null)
                    {
                        orderPendingTimeH = tblConfiguration.Value != null ? Convert.ToInt32(tblConfiguration.Value) : null;
                    }
                }
                #endregion

                #region Elapsed Hours from the Current Dates
                DateTime? DateTimeByElapsedHours = null;
                if (tblBusinessUnit != null || orderPendingTimeH > 0)
                {
                    DateTime DateTimeByElapsedHours1 = DateTime.Now;
                    ElapsedHrs = tblBusinessUnit != null ? tblBusinessUnit?.OrderPendingTimeH : orderPendingTimeH;
                    DateTimeByElapsedHours = DateTimeByElapsedHours1.AddHours(-(ElapsedHrs ?? 0));
                }
                #endregion

                #region table object Initialization
                count =  _context.TblOrderTrans.Include(x => x.Status)
                    .Include(x => x.Exchange).ThenInclude(x => x.Brand)
                    .Include(x => x.Exchange).ThenInclude(x => x.CustomerDetails)
                    .Include(x => x.Exchange).ThenInclude(x => x.ProductType).ThenInclude(x => x.ProductCat)
                    .Include(x => x.Abbredemption).ThenInclude(x => x.Abbregistration).ThenInclude(x => x.NewProductCategory)
                    .Include(x => x.Abbredemption).ThenInclude(x => x.Abbregistration).ThenInclude(x => x.NewProductCategoryTypeNavigation)
                    .Include(x => x.Abbredemption).ThenInclude(x => x.Abbregistration).ThenInclude(x => x.BusinessUnit)
                    .Include(x => x.Abbredemption).ThenInclude(x => x.CustomerDetails)
                    .Count(x => (x.StatusId == Convert.ToInt32(OrderStatusEnum.OrderCreatedbySponsor)
                                || x.StatusId == Convert.ToInt32(OrderStatusEnum.QCInProgress_3Q)
                                || x.StatusId == Convert.ToInt32(OrderStatusEnum.CallAndGoScheduledAppointmentTaken_3P)
                                || x.StatusId == Convert.ToInt32(OrderStatusEnum.QCAppointmentrescheduled)
                                || x.StatusId == Convert.ToInt32(OrderStatusEnum.ReopenOrder)
                                || x.StatusId == Convert.ToInt32(OrderStatusEnum.InstalledbySponsor)
                                || x.StatusId == Convert.ToInt32(OrderStatusEnum.OrderWithDiagnostic))
                               && ((orderStartDate == null && orderEndDate == null) || (x.CreatedDate >= orderStartDate && x.CreatedDate <= orderEndDate))
                               && ((DateTimeByElapsedHours == null) || (x.CreatedDate <= DateTimeByElapsedHours))
                               && (string.IsNullOrEmpty(regdNo) || (x.RegdNo ?? "").ToLower() == regdNo)
                               && ((orderTypeId == null || orderTypeId == 0) || x.OrderType == orderTypeId)
                               && ((x.Exchange != null && x.Exchange.IsActive == true
                               && (tblBusinessUnit == null || (x.Exchange.BusinessUnitId == tblBusinessUnit.BusinessUnitId))
                               && (x.Exchange.ProductType != null && x.Exchange.ProductType.ProductCat != null)
                               && (productCatId == null || x.Exchange.ProductType.ProductCatId == productCatId)
                               && (productTypeId == null || x.Exchange.ProductTypeId == productTypeId)
                               // && (string.IsNullOrEmpty(phoneNo) || (x.Exchange.CustomerDetails != null && x.Exchange.CustomerDetails.PhoneNumber == phoneNo))
                               && (string.IsNullOrEmpty(custCity) || (x.Exchange.CustomerDetails != null && (x.Exchange.CustomerDetails.City ?? "").ToLower() == custCity))
                               && (string.IsNullOrEmpty(companyName) || (x.Exchange.CompanyName ?? "").ToLower() == companyName))
                               || (x.Abbredemption != null && x.Abbredemption.Abbregistration != null && x.Abbredemption.IsActive == true && x.Abbredemption.Abbregistration.IsActive == true
                               && (tblBusinessUnit == null || (x.Abbredemption.Abbregistration.BusinessUnitId == tblBusinessUnit.BusinessUnitId))
                               && (x.Abbredemption.Abbregistration.NewProductCategoryTypeNavigation != null && x.Abbredemption.Abbregistration.NewProductCategory != null)
                               && (productCatId == null || x.Abbredemption.Abbregistration.NewProductCategoryId == productCatId)
                               && (productTypeId == null || x.Abbredemption.Abbregistration.NewProductCategoryTypeId == productTypeId)
                               // && (string.IsNullOrEmpty(phoneNo) || (x.Abbredemption.CustomerDetails != null && x.Abbredemption.CustomerDetails.PhoneNumber == phoneNo))
                               && (string.IsNullOrEmpty(custCity) || (x.Abbredemption.CustomerDetails != null && (x.Abbredemption.CustomerDetails.City ?? "").ToLower() == custCity))
                               && (string.IsNullOrEmpty(companyName) || (x.Abbredemption.Abbregistration.BusinessUnit.Name ?? "").ToLower() == companyName)
                               )));
                if (count > 0)
                {
                    tblOrderTranList = await _context.TblOrderTrans.Include(x => x.Status)
                    .Include(x => x.Exchange).ThenInclude(x => x.Brand)
                    .Include(x => x.Exchange).ThenInclude(x => x.CustomerDetails)
                    .Include(x => x.Exchange).ThenInclude(x => x.ProductType).ThenInclude(x => x.ProductCat)
                    .Include(x => x.Abbredemption).ThenInclude(x => x.Abbregistration).ThenInclude(x => x.NewProductCategory)
                    .Include(x => x.Abbredemption).ThenInclude(x => x.Abbregistration).ThenInclude(x => x.NewProductCategoryTypeNavigation)
                    .Include(x => x.Abbredemption).ThenInclude(x => x.Abbregistration).ThenInclude(x => x.BusinessUnit)
                    .Include(x => x.Abbredemption).ThenInclude(x => x.CustomerDetails)
                    .Where(x => (x.StatusId == Convert.ToInt32(OrderStatusEnum.OrderCreatedbySponsor)
                                || x.StatusId == Convert.ToInt32(OrderStatusEnum.QCInProgress_3Q)
                                || x.StatusId == Convert.ToInt32(OrderStatusEnum.CallAndGoScheduledAppointmentTaken_3P)
                                || x.StatusId == Convert.ToInt32(OrderStatusEnum.QCAppointmentrescheduled)
                                || x.StatusId == Convert.ToInt32(OrderStatusEnum.ReopenOrder)
                                || x.StatusId == Convert.ToInt32(OrderStatusEnum.InstalledbySponsor)
                                || x.StatusId == Convert.ToInt32(OrderStatusEnum.OrderWithDiagnostic))
                               && ((orderStartDate == null && orderEndDate == null) || (x.CreatedDate >= orderStartDate && x.CreatedDate <= orderEndDate))
                               && ((DateTimeByElapsedHours == null) || (x.CreatedDate <= DateTimeByElapsedHours))
                               && (string.IsNullOrEmpty(regdNo) || (x.RegdNo ?? "").ToLower() == regdNo)
                               && ((orderTypeId == null || orderTypeId == 0) || x.OrderType == orderTypeId)
                               && ((x.Exchange != null && x.Exchange.IsActive == true
                               && (tblBusinessUnit == null || (x.Exchange.BusinessUnitId == tblBusinessUnit.BusinessUnitId))
                               && (x.Exchange.ProductType != null && x.Exchange.ProductType.ProductCat != null)
                               && (productCatId == null || x.Exchange.ProductType.ProductCatId == productCatId)
                               && (productTypeId == null || x.Exchange.ProductTypeId == productTypeId)
                               // && (string.IsNullOrEmpty(phoneNo) || (x.Exchange.CustomerDetails != null && x.Exchange.CustomerDetails.PhoneNumber == phoneNo))
                               && (string.IsNullOrEmpty(custCity) || (x.Exchange.CustomerDetails != null && (x.Exchange.CustomerDetails.City ?? "").ToLower() == custCity))
                               && (string.IsNullOrEmpty(companyName) || (x.Exchange.CompanyName ?? "").ToLower() == companyName))
                               || (x.Abbredemption != null && x.Abbredemption.Abbregistration != null && x.Abbredemption.IsActive == true && x.Abbredemption.Abbregistration.IsActive == true
                               && (tblBusinessUnit == null || (x.Abbredemption.Abbregistration.BusinessUnitId == tblBusinessUnit.BusinessUnitId))
                               && (x.Abbredemption.Abbregistration.NewProductCategoryTypeNavigation != null && x.Abbredemption.Abbregistration.NewProductCategory != null)
                               && (productCatId == null || x.Abbredemption.Abbregistration.NewProductCategoryId == productCatId)
                               && (productTypeId == null || x.Abbredemption.Abbregistration.NewProductCategoryTypeId == productTypeId)
                               // && (string.IsNullOrEmpty(phoneNo) || (x.Abbredemption.CustomerDetails != null && x.Abbredemption.CustomerDetails.PhoneNumber == phoneNo))
                               && (string.IsNullOrEmpty(custCity) || (x.Abbredemption.CustomerDetails != null && (x.Abbredemption.CustomerDetails.City ?? "").ToLower() == custCity))
                               && (string.IsNullOrEmpty(companyName) || (x.Abbredemption.Abbregistration.BusinessUnit.Name ?? "").ToLower() == companyName)
                               ))).OrderByDescending(x => x.ModifiedDate ?? x.CreatedDate).Skip(skip).Take(pageSize).ToListAsync();
                }
                recordsTotal = count;
                #endregion

                #region Data Initialization for Datatable from table to Model
                if (tblOrderTranList != null && tblOrderTranList.Count > 0)
                {
                    string actionURL = string.Empty;
                    string actionURL1 = string.Empty;
                    OrderDetailsViewModel? item = null;
                    TblCustomerDetail? tblCustomerDetail = null;
                    
                    foreach (TblOrderTran tblOrderTransObj in tblOrderTranList)
                    {
                        if (tblOrderTransObj != null)
                        {
                            item = new OrderDetailsViewModel();
                            item.RegdNo = tblOrderTransObj.RegdNo;
                            //item.ProductCondition = item.ProductCondition;
                            if (tblOrderTransObj.CreatedDate != null)
                            {
                                item.OrderCreatedDateString = Convert.ToDateTime(tblOrderTransObj.CreatedDate).ToString("dd/MM/yyyy H:mm:ss");
                            }
                            if (tblOrderTransObj.Exchange != null)
                            {
                                item.CompanyName = tblOrderTransObj.Exchange.CompanyName;
                                item.ProductCategory = tblOrderTransObj.Exchange.ProductType?.ProductCat?.Description;
                                item.ProductCondition = tblOrderTransObj.Exchange.ProductCondition;
                                tblCustomerDetail = tblOrderTransObj.Exchange.CustomerDetails;
                                //if (tblOrderTransObj.Exchange.CreatedDate != null)
                                //{
                                //    item.OrderCreatedDateString = Convert.ToDateTime(tblOrderTransObj.Exchange.CreatedDate).ToString("dd/MM/yyyy H:mm:ss");
                                //}
                            }
                            else if (tblOrderTransObj.Abbredemption != null)
                            {
                                item.CompanyName = tblOrderTransObj.Abbredemption.Abbregistration?.BusinessUnit?.Name;
                                item.ProductCategory = tblOrderTransObj.Abbredemption.Abbregistration?.NewProductCategory?.DescriptionForAbb;
                                tblCustomerDetail = tblOrderTransObj.Abbredemption.CustomerDetails;
                                //if (tblOrderTransObj.Abbredemption.CreatedDate != null)
                                //{
                                //    item.OrderCreatedDateString = Convert.ToDateTime(tblOrderTransObj.Abbredemption.CreatedDate).ToString("dd/MM/yyyy H:mm:ss");
                                //}
                            }
                            if (tblCustomerDetail != null)
                            {
                                item.CustCity = tblCustomerDetail.City;
                            }

                            TblOrderQc tblOrderQc = _orderQCRepository.GetQcorderBytransId(tblOrderTransObj.OrderTransId);
                            if (tblOrderQc == null)
                            {
                                tblOrderQc = new TblOrderQc();
                            }
                            else
                            {
                                if (tblOrderQc.ProposedQcdate != null)
                                {
                                    item.Reschedulecount = tblOrderQc.Reschedulecount != null ? tblOrderQc.Reschedulecount : 0;
                                    item.RescheduleDate = Convert.ToDateTime(tblOrderQc.ProposedQcdate).ToString("dd/MM/yyyy");
                                    item.PreferredQCDate = tblOrderQc.ProposedQcdate != null ? tblOrderQc.ProposedQcdate : null;
                                    item.PreferredQCDateString = Convert.ToDateTime(item.PreferredQCDate).ToString("dd/MM/yyyy");
                                }
                            }
                            if (tblOrderTransObj.OrderType == Convert.ToInt32(OrderTypeEnum.Exchange))
                            {
                                actionURL1 = " <div class='actionbtns'>";
                                actionURL1 += "<a href ='" + URL + "/Exchange/Manage?Exchangeorderid=" + _protector.Encode(tblOrderTransObj.ExchangeId) + "' ><button onclick='RecordView(" + tblOrderTransObj.ExchangeId + ")' class='btn btn-sm btn-primary' data-bs-toggle='tooltip' data-bs-placement='top' title='View'><i class='fa-solid fa-eye'></i></button></a>";
                                //actionURL1 += " <a class='btn btn-sm btn-primary' href='" + URL + "/Index1?orderTransId=" + tblOrderTransObj.OrderTransId + "' title='TimeLine'><i class='fa solid fa-clock-rotate-left'></i></a>";
                                actionURL1 += "</div>";
                            }
                            else if (tblOrderTransObj.OrderType == Convert.ToInt32(OrderTypeEnum.ABB))
                            {
                                actionURL1 = " <div class='actionbtns'>";
                                actionURL1 += "<a href ='" + URL + "/ABBRedemption/Manage?regdNo=" + item.RegdNo + "' ><button onclick='RecordView(" + item.RegdNo + ")'  class='btn btn-sm btn-primary' data-bs-toggle='tooltip' data-bs-placement='top' title='View'><i class='fa-solid fa-eye'></i></button></a>";
                                actionURL1 += "</div>";
                            }
                            item.Action = actionURL1;

                            item.StatusCode = tblOrderTransObj.Status?.StatusCode;
                            OrderDetailsList.Add(item);
                        }
                    }
                }
                #endregion

                #region pagination
                //recordsTotal = OrderDetailsList != null ? OrderDetailsList.Count : 0;
                //if (OrderDetailsList != null && OrderDetailsList.Count > 0)
                //{
                //    OrderDetailsList = OrderDetailsList.Skip(skip).Take(pageSize).ToList();
                //}
                #endregion

                var data = OrderDetailsList;
                var jsonData = new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data };
                return Ok(jsonData);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        #endregion

        #region Pending for Price Acceptance List Created by VK
        [HttpPost]
        public async Task<ActionResult> PendingForPriceAcceptance(int companyId, DateTime? orderStartDate,
            DateTime? orderEndDate, string? companyName, string? regdNo, int? productCatId,
            int? productTypeId, int? orderTypeId, string? custCity)
        {
            #region Variable Declaration
            if (!string.IsNullOrWhiteSpace(companyName) && companyName != "null")
            { companyName = companyName.Trim().ToLower(); }
            else { companyName = null; }
            if (!string.IsNullOrWhiteSpace(regdNo) && regdNo != "null")
            { regdNo = regdNo.Trim().ToLower(); }
            else { regdNo = null; }
            //if (!string.IsNullOrWhiteSpace(phoneNo) && phoneNo != "null")
            //{ phoneNo = phoneNo.Trim().ToLower(); }
            //else { phoneNo = null; }
            if (!string.IsNullOrWhiteSpace(custCity) && custCity != "null")
            { custCity = custCity.Trim().ToLower(); }
            else { custCity = null; }

            List<TblExchangeOrder> TblExchangeOrders = null;
            string URL = _config.Value.URLPrefixforProd;
            string MVCURL = _config.Value.MVCBaseURLForExchangeInvoice;
            string InvoiceimagURL = string.Empty;
            TblCompany tblCompany = null;
            TblBusinessUnit tblBusinessUnit = null;
            List<OrderDetailsViewModel> OrderDetailsList = new List<OrderDetailsViewModel>();
            int count = 0;
            TblOrderTran? tblOrderTran = null;
            List<TblOrderTran>? tblOrderTranList = null;
            int? ElapsedHrs = 0;
            int? orderPendingTimeH = null;
            TblConfiguration? tblConfiguration = null;
            #endregion

            try
            {
                #region Datatable Variables
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
                if (companyId > 0 && companyId != 1007)
                {
                    tblCompany = _companyRepository.GetCompanyId(companyId);
                    if (tblCompany != null)
                    {
                        tblBusinessUnit = _businessUnitRepository.Getbyid(tblCompany.BusinessUnitId);
                    }
                }
                if (orderStartDate != null && orderEndDate != null)
                {
                    orderStartDate = Convert.ToDateTime(orderStartDate).AddMinutes(-1);
                    orderEndDate = Convert.ToDateTime(orderEndDate).AddDays(1).AddSeconds(-1);
                }
                #endregion

                #region Get Orders Pending Time from Configuration
                if (tblBusinessUnit == null)
                {
                    tblConfiguration = _templateConfigurationRepository.GetConfigByKeyName(ConfigurationEnum.OrderPendingTimeH.ToString());
                    if (tblConfiguration != null)
                    {
                        orderPendingTimeH = tblConfiguration.Value != null ? Convert.ToInt32(tblConfiguration.Value) : null;
                    }
                }
                #endregion

                #region Elapsed Hours from the Current Dates
                DateTime? DateTimeByElapsedHours = null;
                if (tblBusinessUnit != null || orderPendingTimeH > 0)
                {
                    DateTime DateTimeByElapsedHours1 = DateTime.Now;
                    ElapsedHrs = tblBusinessUnit != null ? tblBusinessUnit?.OrderPendingTimeH : orderPendingTimeH;
                    DateTimeByElapsedHours = DateTimeByElapsedHours1.AddHours(-(ElapsedHrs ?? 0));
                }
                #endregion

                #region table object Initialization
                count = _context.TblOrderTrans.Include(x => x.Status)
                    .Include(x => x.Exchange).ThenInclude(x => x.Brand)
                    .Include(x => x.Exchange).ThenInclude(x => x.CustomerDetails)
                    .Include(x => x.Exchange).ThenInclude(x => x.ProductType).ThenInclude(x => x.ProductCat)
                    .Include(x => x.Abbredemption).ThenInclude(x => x.Abbregistration).ThenInclude(x => x.NewProductCategory)
                    .Include(x => x.Abbredemption).ThenInclude(x => x.Abbregistration).ThenInclude(x => x.NewProductCategoryTypeNavigation)
                    .Include(x => x.Abbredemption).ThenInclude(x => x.Abbregistration).ThenInclude(x => x.BusinessUnit)
                    .Include(x => x.Abbredemption).ThenInclude(x => x.CustomerDetails)
                    .Count(x => (x.StatusId == Convert.ToInt32(OrderStatusEnum.Waitingforcustapproval) || x.StatusId == Convert.ToInt32(OrderStatusEnum.QCByPass))
                               && ((orderStartDate == null && orderEndDate == null) || (x.ModifiedDate >= orderStartDate && x.ModifiedDate <= orderEndDate))
                               && ((DateTimeByElapsedHours == null) || (x.ModifiedDate <= DateTimeByElapsedHours))
                               && (string.IsNullOrEmpty(regdNo) || (x.RegdNo ?? "").ToLower() == regdNo)
                               && ((orderTypeId == null || orderTypeId == 0) || x.OrderType == orderTypeId)
                               && ((x.Exchange != null && x.Exchange.IsActive == true
                               && (tblBusinessUnit == null || (x.Exchange.BusinessUnitId == tblBusinessUnit.BusinessUnitId))
                               && (x.Exchange.ProductType != null && x.Exchange.ProductType.ProductCat != null)
                               && (productCatId == null || x.Exchange.ProductType.ProductCatId == productCatId)
                               && (productTypeId == null || x.Exchange.ProductTypeId == productTypeId)
                               //  && (string.IsNullOrEmpty(phoneNo) || (x.Exchange.CustomerDetails != null && x.Exchange.CustomerDetails.PhoneNumber == phoneNo))
                               && (string.IsNullOrEmpty(custCity) || (x.Exchange.CustomerDetails != null && (x.Exchange.CustomerDetails.City ?? "").ToLower() == custCity))
                               && (string.IsNullOrEmpty(companyName) || (x.Exchange.CompanyName ?? "").ToLower() == companyName))
                               || (x.Abbredemption != null && x.Abbredemption.Abbregistration != null && x.Abbredemption.IsActive == true && x.Abbredemption.Abbregistration.IsActive == true
                               && (tblBusinessUnit == null || (x.Abbredemption.Abbregistration.BusinessUnitId == tblBusinessUnit.BusinessUnitId))
                               && (x.Abbredemption.Abbregistration.NewProductCategoryTypeNavigation != null && x.Abbredemption.Abbregistration.NewProductCategory != null)
                               && (productCatId == null || x.Abbredemption.Abbregistration.NewProductCategoryId == productCatId)
                               && (productTypeId == null || x.Abbredemption.Abbregistration.NewProductCategoryTypeId == productTypeId)
                               //   && (string.IsNullOrEmpty(phoneNo) || (x.Abbredemption.CustomerDetails != null && x.Abbredemption.CustomerDetails.PhoneNumber == phoneNo))
                               && (string.IsNullOrEmpty(custCity) || (x.Abbredemption.CustomerDetails != null && (x.Abbredemption.CustomerDetails.City ?? "").ToLower() == custCity))
                               && (string.IsNullOrEmpty(companyName) || (x.Abbredemption.Abbregistration.BusinessUnit.Name ?? "").ToLower() == companyName)
                               )));
                if (count > 0)
                {
                    tblOrderTranList = await _context.TblOrderTrans.Include(x => x.Status)
                    .Include(x => x.Exchange).ThenInclude(x => x.Brand)
                    .Include(x => x.Exchange).ThenInclude(x => x.CustomerDetails)
                    .Include(x => x.Exchange).ThenInclude(x => x.ProductType).ThenInclude(x => x.ProductCat)
                    .Include(x => x.Abbredemption).ThenInclude(x => x.Abbregistration)
                    .Include(x => x.Abbredemption).ThenInclude(x => x.Abbregistration).ThenInclude(x => x.NewProductCategory)
                    .Include(x => x.Abbredemption).ThenInclude(x => x.Abbregistration).ThenInclude(x => x.NewProductCategoryTypeNavigation)
                    .Include(x => x.Abbredemption).ThenInclude(x => x.Abbregistration).ThenInclude(x => x.BusinessUnit)
                    .Include(x => x.Abbredemption).ThenInclude(x => x.CustomerDetails)
                    .Where(x => (x.StatusId == Convert.ToInt32(OrderStatusEnum.Waitingforcustapproval) || x.StatusId == Convert.ToInt32(OrderStatusEnum.QCByPass))
                               && ((orderStartDate == null && orderEndDate == null) || (x.ModifiedDate >= orderStartDate && x.ModifiedDate <= orderEndDate))
                               && ((DateTimeByElapsedHours == null) || (x.ModifiedDate <= DateTimeByElapsedHours))
                               && (string.IsNullOrEmpty(regdNo) || (x.RegdNo ?? "").ToLower() == regdNo)
                               && ((orderTypeId == null || orderTypeId == 0) || x.OrderType == orderTypeId)
                               && ((x.Exchange != null && x.Exchange.IsActive == true
                               && (tblBusinessUnit == null || (x.Exchange.BusinessUnitId == tblBusinessUnit.BusinessUnitId))
                               && (x.Exchange.ProductType != null && x.Exchange.ProductType.ProductCat != null)
                               && (productCatId == null || x.Exchange.ProductType.ProductCatId == productCatId)
                               && (productTypeId == null || x.Exchange.ProductTypeId == productTypeId)
                               //  && (string.IsNullOrEmpty(phoneNo) || (x.Exchange.CustomerDetails != null && x.Exchange.CustomerDetails.PhoneNumber == phoneNo))
                               && (string.IsNullOrEmpty(custCity) || (x.Exchange.CustomerDetails != null && (x.Exchange.CustomerDetails.City ?? "").ToLower() == custCity))
                               && (string.IsNullOrEmpty(companyName) || (x.Exchange.CompanyName ?? "").ToLower() == companyName))
                               || (x.Abbredemption != null && x.Abbredemption.Abbregistration != null && x.Abbredemption.IsActive == true && x.Abbredemption.Abbregistration.IsActive == true
                               && (tblBusinessUnit == null || (x.Abbredemption.Abbregistration.BusinessUnitId == tblBusinessUnit.BusinessUnitId))
                               && (x.Abbredemption.Abbregistration.NewProductCategoryTypeNavigation != null && x.Abbredemption.Abbregistration.NewProductCategory != null)
                               && (productCatId == null || x.Abbredemption.Abbregistration.NewProductCategoryId == productCatId)
                               && (productTypeId == null || x.Abbredemption.Abbregistration.NewProductCategoryTypeId == productTypeId)
                               //   && (string.IsNullOrEmpty(phoneNo) || (x.Abbredemption.CustomerDetails != null && x.Abbredemption.CustomerDetails.PhoneNumber == phoneNo))
                               && (string.IsNullOrEmpty(custCity) || (x.Abbredemption.CustomerDetails != null && (x.Abbredemption.CustomerDetails.City ?? "").ToLower() == custCity))
                               && (string.IsNullOrEmpty(companyName) || (x.Abbredemption.Abbregistration.BusinessUnit.Name ?? "").ToLower() == companyName)
                               ))).OrderByDescending(x => x.ModifiedDate ?? x.CreatedDate).Skip(skip).Take(pageSize).ToListAsync();
                }
                recordsTotal = count;
                #endregion

                #region Data Initialization for Datatable from table to Model
                if (tblOrderTranList != null && tblOrderTranList.Count > 0)
                {
                    List<OrderDetailsViewModel>? OrderDetailsListObj = null;
                    //OrderDetailsListObj = _mapper.Map<List<TblExchangeOrder>, List<OrderDetailsViewModel>>(TblExchangeOrders);

                    string actionURL = string.Empty;
                    string actionURL1 = string.Empty;
                    OrderDetailsViewModel? item = null;
                    TblCustomerDetail? tblCustomerDetail = null;

                    foreach (TblOrderTran tblOrderTransObj in tblOrderTranList)
                    {
                        if (tblOrderTransObj != null)
                        {
                            item = new OrderDetailsViewModel();
                            item.RegdNo = tblOrderTransObj.RegdNo;

                            #region Exchange and ABB orders mapping
                            if (tblOrderTransObj.Exchange != null)
                            {
                                item.CompanyName = tblOrderTransObj.Exchange.CompanyName;
                                item.ProductCategory = tblOrderTransObj.Exchange.ProductType?.ProductCat?.Description;
                                tblCustomerDetail = tblOrderTransObj.Exchange.CustomerDetails;
                                if (tblOrderTransObj.Exchange.CreatedDate != null)
                                {
                                    item.OrderCreatedDateString = Convert.ToDateTime(tblOrderTransObj.Exchange.CreatedDate).ToString("dd/MM/yyyy H:mm:ss");
                                }
                                if (tblOrderTransObj.Exchange.ModifiedDate != null)
                                {
                                    item.OrderDueDateTime = Convert.ToDateTime(tblOrderTransObj.Exchange.ModifiedDate).ToString("dd/MM/yyyy H:mm:ss");
                                }
                            }
                            else if (tblOrderTransObj.Abbredemption != null)
                            {
                                item.CompanyName = tblOrderTransObj.Abbredemption.Abbregistration?.BusinessUnit?.Name;
                                item.ProductCategory = tblOrderTransObj.Abbredemption.Abbregistration?.NewProductCategory?.DescriptionForAbb;
                                tblCustomerDetail = tblOrderTransObj.Abbredemption.CustomerDetails;
                                if (tblOrderTransObj.Abbredemption.CreatedDate != null)
                                {
                                    item.OrderCreatedDateString = Convert.ToDateTime(tblOrderTransObj.Abbredemption.CreatedDate).ToString("dd/MM/yyyy H:mm:ss");
                                }
                                if (tblOrderTransObj.Abbredemption.ModifiedDate != null)
                                {
                                    item.OrderDueDateTime = Convert.ToDateTime(tblOrderTransObj.Abbredemption.ModifiedDate).ToString("dd/MM/yyyy H:mm:ss");
                                }
                            }
                            #endregion

                            #region Customer Details
                            if (tblCustomerDetail != null)
                            {
                                item.CustCity = tblCustomerDetail.City;
                            }
                            #endregion

                            #region Get data from tblOrderQC 
                            TblOrderQc tblOrderQc = _orderQCRepository.GetQcorderBytransId(tblOrderTransObj.OrderTransId);
                            if (tblOrderQc == null)
                            {
                                tblOrderQc = new TblOrderQc();
                            }
                            else
                            {
                                if (tblOrderQc.ProposedQcdate != null)
                                {
                                    item.Reschedulecount = tblOrderQc.Reschedulecount != null ? tblOrderQc.Reschedulecount : 0;
                                    item.RescheduleDate = Convert.ToDateTime(tblOrderQc.ProposedQcdate).ToString("dd/MM/yyyy");
                                    item.PreferredQCDate = tblOrderQc.ProposedQcdate != null ? tblOrderQc.ProposedQcdate : null;
                                    item.PreferredQCDateString = Convert.ToDateTime(item.PreferredQCDate).ToString("dd/MM/yyyy");
                                }
                            }
                            item.ProductCondition = tblOrderQc?.QualityAfterQc;
                            #endregion

                            if (tblOrderTransObj.OrderType == Convert.ToInt32(OrderTypeEnum.Exchange))
                            {
                                actionURL1 = " <div class='actionbtns'>";
                                actionURL1 += "<a href ='" + URL + "/Exchange/Manage?Exchangeorderid=" + _protector.Encode(tblOrderTransObj?.ExchangeId) + "' ><button onclick='RecordView(" + tblOrderTransObj?.ExchangeId + ")' class='btn btn-sm btn-primary' data-bs-toggle='tooltip' data-bs-placement='top' title='View'><i class='fa-solid fa-eye'></i></button></a>";
                                //actionURL1 += " <a class='btn btn-sm btn-primary' href='" + URL + "/Index1?orderTransId=" + tblOrderTransObj.OrderTransId + "' title='TimeLine'><i class='fa solid fa-clock-rotate-left'></i></a>";
                                actionURL1 += "</div>";
                            }
                            else if (tblOrderTransObj.OrderType == Convert.ToInt32(OrderTypeEnum.ABB))
                            {
                                actionURL1 = " <div class='actionbtns'>";
                                actionURL1 += "<a href ='" + URL + "/ABBRedemption/Manage?regdNo=" + item.RegdNo + "' ><button onclick='RecordView(" + item.RegdNo + ")'  class='btn btn-sm btn-primary' data-bs-toggle='tooltip' data-bs-placement='top' title='View'><i class='fa-solid fa-eye'></i></button></a>";
                                actionURL1 += "</div>";
                            }
                            item.Action = actionURL1;
                            item.StatusCode = tblOrderTransObj.Status?.StatusCode;

                            OrderDetailsList.Add(item);
                        }
                    }
                }
                #endregion

                //#region pagination
                //recordsTotal = OrderDetailsList != null ? OrderDetailsList.Count : 0;
                //if (OrderDetailsList != null && OrderDetailsList.Count > 0)
                //{
                //    OrderDetailsList = OrderDetailsList.Skip(skip).Take(pageSize).ToList();
                //}
                //#endregion

                var data = OrderDetailsList;
                var jsonData = new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data };
                return Ok(jsonData);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        #endregion

        #region Pending for Pickup Created by VK
        public async Task<ActionResult> PendingforPickup(int companyId, DateTime? orderStartDate, DateTime? orderEndDate,
             string? companyName, string? regdNo, int? productCatId, int? productTypeId, int? orderTypeId,
             int? ServicePartnerId, string? SPName, string? ticketNo, string? custCity)
        {
            #region Variable Declaration
            List<TblLogistic>? tblLogistic = null;
            TblServicePartner? tblServicePartner = null;
            List<OrderDetailsViewModel>? OrderDetailsList = null;
            OrderDetailsViewModel? OrderDetailsVM = null;
            string? URL = _config.Value.URLPrefixforProd;
            TblBusinessUnit? tblBusinessUnit = null;
            TblCompany? tblCompany = null;
            int? ElapsedHrs = 0;
            int? orderPendingTimeH = null;
            TblConfiguration? tblConfiguration = null;
            #endregion

            try
            {
                #region Datatable Variables
                var draw = Request.Form["draw"].FirstOrDefault();
                var start = Request.Form["start"].FirstOrDefault();
                var length = Request.Form["length"].FirstOrDefault();
                var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
                var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();
                var searchValue = Request.Form["search[value]"].FirstOrDefault();
                searchValue = searchValue.ToLower().Trim();
                int pageSize = length != null ? Convert.ToInt32(length) : 0;
                int skip = start != null ? Convert.ToInt32(start) : 0;
                int recordsTotal = 0;
                int count = 0;
                #endregion

                #region Advanced Filters Mapping
                if (companyId > 0 && companyId != 1007)
                {
                    tblCompany = _companyRepository.GetCompanyId(companyId);
                    if (tblCompany != null)
                    {
                        tblBusinessUnit = _businessUnitRepository.Getbyid(tblCompany.BusinessUnitId);
                    }
                }
                if (orderStartDate != null && orderEndDate != null)
                {
                    orderStartDate = Convert.ToDateTime(orderStartDate).AddMinutes(-1);
                    orderEndDate = Convert.ToDateTime(orderEndDate).AddDays(1).AddSeconds(-1);
                }
                if (ServicePartnerId == 0)
                {
                    ServicePartnerId = null;
                }
                #endregion

                #region Get Orders Pending Time from Configuration
                if (tblBusinessUnit == null)
                {
                    tblConfiguration = _templateConfigurationRepository.GetConfigByKeyName(ConfigurationEnum.OrderPendingTimeH.ToString());
                    if (tblConfiguration != null)
                    {
                        orderPendingTimeH = tblConfiguration.Value != null ? Convert.ToInt32(tblConfiguration.Value) : null;
                    }
                }
                #endregion

                #region Elapsed Hours from the Current Dates
                DateTime? DateTimeByElapsedHours = null;
                if (tblBusinessUnit != null || orderPendingTimeH > 0)
                {
                    DateTime DateTimeByElapsedHours1 = DateTime.Now;
                    ElapsedHrs = tblBusinessUnit != null ? tblBusinessUnit?.OrderPendingTimeH : orderPendingTimeH;
                    DateTimeByElapsedHours = DateTimeByElapsedHours1.AddHours(-(ElapsedHrs ?? 0));
                }
                #endregion

                //tblServicePartner = _context.TblServicePartners.Where(x => x.UserId == userId && x.IsActive == true).FirstOrDefault();
                #region tblLogistic Implementation
                count = _context.TblLogistics
                             .Include(x => x.TblOrderLgcs)
                             .Include(x => x.ServicePartner)
                             .Include(x => x.Status)
                                        .Include(x => x.OrderTrans).ThenInclude(x => x.Exchange).ThenInclude(x => x.ProductType).ThenInclude(x => x.ProductCat)
                                        .Include(x => x.OrderTrans).ThenInclude(x => x.Exchange).ThenInclude(x => x.CustomerDetails)
                                        //Changes for ABB Redemption by Vk 
                                        .Include(x => x.OrderTrans).ThenInclude(x => x.Abbredemption).ThenInclude(x => x.Abbregistration).ThenInclude(x => x.NewProductCategory)
                                        .Include(x => x.OrderTrans).ThenInclude(x => x.Abbredemption).ThenInclude(x => x.Abbregistration).ThenInclude(x => x.NewProductCategoryTypeNavigation)
                                        .Include(x => x.OrderTrans).ThenInclude(x => x.Abbredemption).ThenInclude(x => x.CustomerDetails)
                                        .Include(x => x.OrderTrans).ThenInclude(x => x.Abbredemption).ThenInclude(x => x.Abbregistration).ThenInclude(x => x.BusinessUnit)
                                        //Changes for ABB Redemption by Vk
                                        .Count(x => x.IsActive == true && x.OrderTrans != null && x.OrderTrans.IsActive == true
                                        && x.StatusId == Convert.ToInt32(OrderStatusEnum.LogisticsTicketUpdated) && x.OrderTrans.StatusId == Convert.ToInt32(OrderStatusEnum.LogisticsTicketUpdated)
                                        && (ServicePartnerId == null || (x.ServicePartner != null && x.ServicePartnerId == ServicePartnerId))
                                        && ((DateTimeByElapsedHours == null) || (x.CreatedDate <= DateTimeByElapsedHours))
                                        && ((orderStartDate == null && orderEndDate == null) || (x.CreatedDate >= orderStartDate && x.CreatedDate <= orderEndDate))
                                        && (string.IsNullOrEmpty(SPName) || (x.ServicePartner != null && (x.ServicePartner.ServicePartnerBusinessName??"").ToLower().Contains(SPName)))
                                        && (string.IsNullOrEmpty(regdNo) || (x.RegdNo ?? "").ToLower() == regdNo)
                                        && (string.IsNullOrEmpty(ticketNo) || (x.TicketNumber ?? "").ToLower() == ticketNo)
                                        && ((orderTypeId == null || orderTypeId == 0) || x.OrderTrans.OrderType == orderTypeId)
                                        && ((x.OrderTrans.Exchange != null && x.OrderTrans.Exchange.IsActive == true
                                        && (tblBusinessUnit == null || (x.OrderTrans.Exchange.BusinessUnitId == tblBusinessUnit.BusinessUnitId))
                                        && (x.OrderTrans.Exchange.ProductType != null && x.OrderTrans.Exchange.ProductType.ProductCat != null)
                                        && (productCatId == null || x.OrderTrans.Exchange.ProductType.ProductCatId == productCatId)
                                        && (productTypeId == null || x.OrderTrans.Exchange.ProductTypeId == productTypeId)
                                      //&& (string.IsNullOrEmpty(phoneNo) || (x.OrderTrans.Exchange.CustomerDetails != null && x.OrderTrans.Exchange.CustomerDetails.PhoneNumber == phoneNo))
                                        && (string.IsNullOrEmpty(custCity) || (x.OrderTrans.Exchange.CustomerDetails != null && (x.OrderTrans.Exchange.CustomerDetails.City ?? "").ToLower() == custCity))
                                        && (string.IsNullOrEmpty(companyName) || (x.OrderTrans.Exchange.CompanyName ?? "").ToLower() == companyName))
                                        || (x.OrderTrans.Abbredemption != null && x.OrderTrans.Abbredemption.Abbregistration != null && x.OrderTrans.Abbredemption.IsActive == true && x.OrderTrans.Abbredemption.Abbregistration.IsActive == true
                                        && (tblBusinessUnit == null || (x.OrderTrans.Abbredemption.Abbregistration.BusinessUnitId == tblBusinessUnit.BusinessUnitId))
                                        && (x.OrderTrans.Abbredemption.Abbregistration.NewProductCategoryTypeNavigation != null && x.OrderTrans.Abbredemption.Abbregistration.NewProductCategory != null)
                                        && (productCatId == null || x.OrderTrans.Abbredemption.Abbregistration.NewProductCategoryId == productCatId)
                                        && (productTypeId == null || x.OrderTrans.Abbredemption.Abbregistration.NewProductCategoryTypeId == productTypeId)
                                      //&& (string.IsNullOrEmpty(phoneNo) || (x.OrderTrans.Abbredemption.CustomerDetails != null && x.OrderTrans.Abbredemption.CustomerDetails.PhoneNumber == phoneNo))
                                        && (string.IsNullOrEmpty(custCity) || (x.OrderTrans.Abbredemption.CustomerDetails != null && (x.OrderTrans.Abbredemption.CustomerDetails.City ?? "").ToLower() == custCity))
                                        && (string.IsNullOrEmpty(companyName) || (x.OrderTrans.Abbredemption.Abbregistration.BusinessUnit.Name ?? "").ToLower() == companyName)
                               )));
                if (count > 0)
                {
                    tblLogistic = _context.TblLogistics
                             .Include(x => x.TblOrderLgcs)
                             .Include(x => x.ServicePartner)
                             .Include(x => x.Status)
                                        .Include(x => x.OrderTrans).ThenInclude(x => x.Exchange).ThenInclude(x => x.ProductType).ThenInclude(x => x.ProductCat)
                                        .Include(x => x.OrderTrans).ThenInclude(x => x.Exchange).ThenInclude(x => x.CustomerDetails)
                                        //Changes for ABB Redemption by Vk 
                                        .Include(x => x.OrderTrans).ThenInclude(x => x.Abbredemption).ThenInclude(x => x.Abbregistration).ThenInclude(x => x.NewProductCategory)
                                        .Include(x => x.OrderTrans).ThenInclude(x => x.Abbredemption).ThenInclude(x => x.Abbregistration).ThenInclude(x => x.NewProductCategoryTypeNavigation)
                                        .Include(x => x.OrderTrans).ThenInclude(x => x.Abbredemption).ThenInclude(x => x.CustomerDetails)
                                        .Include(x => x.OrderTrans).ThenInclude(x => x.Abbredemption).ThenInclude(x => x.Abbregistration).ThenInclude(x => x.BusinessUnit)
                                        //Changes for ABB Redemption by Vk
                                        .Where(x => x.IsActive == true && x.OrderTrans != null && x.OrderTrans.IsActive == true
                                        && x.StatusId == Convert.ToInt32(OrderStatusEnum.LogisticsTicketUpdated) && x.OrderTrans.StatusId == Convert.ToInt32(OrderStatusEnum.LogisticsTicketUpdated)
                                        && (ServicePartnerId == null || (x.ServicePartner != null && x.ServicePartnerId == ServicePartnerId))
                                        && ((DateTimeByElapsedHours == null) || (x.CreatedDate <= DateTimeByElapsedHours))
                                        && ((orderStartDate == null && orderEndDate == null) || (x.CreatedDate >= orderStartDate && x.CreatedDate <= orderEndDate))
                                        && (string.IsNullOrEmpty(SPName) || (x.ServicePartner != null && (x.ServicePartner.ServicePartnerBusinessName ?? "").ToLower().Contains(SPName)))
                                        && (string.IsNullOrEmpty(regdNo) || (x.RegdNo ?? "").ToLower() == regdNo)
                                        && (string.IsNullOrEmpty(ticketNo) || (x.TicketNumber ?? "").ToLower() == ticketNo)
                                        && ((orderTypeId == null || orderTypeId == 0) || x.OrderTrans.OrderType == orderTypeId)
                                        && ((x.OrderTrans.Exchange != null && x.OrderTrans.Exchange.IsActive == true
                                        && (tblBusinessUnit == null || (x.OrderTrans.Exchange.BusinessUnitId == tblBusinessUnit.BusinessUnitId))
                                        && (x.OrderTrans.Exchange.ProductType != null && x.OrderTrans.Exchange.ProductType.ProductCat != null)
                                        && (productCatId == null || x.OrderTrans.Exchange.ProductType.ProductCatId == productCatId)
                                        && (productTypeId == null || x.OrderTrans.Exchange.ProductTypeId == productTypeId)
                                     // && (string.IsNullOrEmpty(phoneNo) || (x.OrderTrans.Exchange.CustomerDetails != null && x.OrderTrans.Exchange.CustomerDetails.PhoneNumber == phoneNo))
                                        && (string.IsNullOrEmpty(custCity) || (x.OrderTrans.Exchange.CustomerDetails != null && (x.OrderTrans.Exchange.CustomerDetails.City ?? "").ToLower() == custCity))
                                        && (string.IsNullOrEmpty(companyName) || (x.OrderTrans.Exchange.CompanyName ?? "").ToLower() == companyName))
                                        || (x.OrderTrans.Abbredemption != null && x.OrderTrans.Abbredemption.Abbregistration != null && x.OrderTrans.Abbredemption.IsActive == true && x.OrderTrans.Abbredemption.Abbregistration.IsActive == true
                                        && (tblBusinessUnit == null || (x.OrderTrans.Abbredemption.Abbregistration.BusinessUnitId == tblBusinessUnit.BusinessUnitId))
                                        && (x.OrderTrans.Abbredemption.Abbregistration.NewProductCategoryTypeNavigation != null && x.OrderTrans.Abbredemption.Abbregistration.NewProductCategory != null)
                                        && (productCatId == null || x.OrderTrans.Abbredemption.Abbregistration.NewProductCategoryId == productCatId)
                                        && (productTypeId == null || x.OrderTrans.Abbredemption.Abbregistration.NewProductCategoryTypeId == productTypeId)
                                     // && (string.IsNullOrEmpty(phoneNo) || (x.OrderTrans.Abbredemption.CustomerDetails != null && x.OrderTrans.Abbredemption.CustomerDetails.PhoneNumber == phoneNo))
                                        && (string.IsNullOrEmpty(custCity) || (x.OrderTrans.Abbredemption.CustomerDetails != null && (x.OrderTrans.Abbredemption.CustomerDetails.City ?? "").ToLower() == custCity))
                                        && (string.IsNullOrEmpty(companyName) || (x.OrderTrans.Abbredemption.Abbregistration.BusinessUnit.Name ?? "").ToLower() == companyName)
                               ))).OrderByDescending(x => x.ModifiedDate ?? x.CreatedDate).Skip(skip).Take(pageSize).ToList();
                }
                recordsTotal = count;
                #endregion

                #region Sorting
                if (tblLogistic != null)
                {
                    tblLogistic = sortColumnDirection.Equals(SortingOrder.ASCENDING)
 ? tblLogistic.OrderBy(o => o != null && o.GetType().GetProperty(sortColumn) != null ? o.GetType().GetProperty(sortColumn).GetValue(o, null) : null)
                       .ToList()
 : tblLogistic.OrderByDescending(o => o != null && o.GetType().GetProperty(sortColumn) != null ? o.GetType().GetProperty(sortColumn).GetValue(o, null) : null)
                       .ToList();
                }
                #endregion

                #region table to model mapping
                OrderDetailsList = new List<OrderDetailsViewModel>();
                if (tblLogistic != null && tblLogistic.Count > 0)
                {
                    foreach (var item in tblLogistic)
                    {
                        OrderDetailsVM = new OrderDetailsViewModel();
                        string actionURL = string.Empty;
                        actionURL = " <ul class='actions'>";
                        actionURL = "<a href ='" + URL + "/LGC_Admin/OrderViewPage?OrderTransId=" + (item.OrderTransId) + "' ><button onclick='View(" + item.OrderTransId + ")' class='btn btn-sm btn-primary'><i class='fa-solid fa-eye'></i></button></a>";
                        actionURL = actionURL + "</ul>";

                        string? productTypeDesc = null; string? productCatDesc = null; string? statusCode = null;
                        string? servicePartnerName = null; string? custCityObj = null; string? companyNameObj = null; string? OrderDueDateTime = null;
                        if (item != null)
                        {
                            #region Exchange and ABB common Configuraion Add by VK
                            if (item.OrderTrans?.Exchange != null)
                            {
                                if (item.OrderTrans.Exchange.ProductType != null)
                                {
                                    productTypeDesc = item.OrderTrans.Exchange.ProductType.Description;
                                    if (item.OrderTrans.Exchange.ProductType.ProductCat != null)
                                    {
                                        productCatDesc = item.OrderTrans.Exchange.ProductType.ProductCat.Description;
                                    }
                                }
                                if (item.OrderTrans.Exchange.CustomerDetails != null)
                                {
                                    custCityObj = item.OrderTrans.Exchange.CustomerDetails.City;
                                }
                                companyNameObj = item.OrderTrans.Exchange.CompanyName;
                                if (item.CreatedDate != null)
                                {
                                    OrderDetailsVM.OrderDueDateTime = Convert.ToDateTime(item.CreatedDate).ToString("dd/MM/yyyy H:mm:ss");
                                }
                                if (item.OrderTrans.Exchange.CreatedDate != null)
                                {
                                    OrderDetailsVM.OrderCreatedDateString = Convert.ToDateTime(item.OrderTrans.Exchange.CreatedDate).ToString("dd/MM/yyyy H:mm:ss");
                                }
                            }
                            else if (item.OrderTrans?.Abbredemption != null && item.OrderTrans.Abbredemption.Abbregistration != null)
                            {
                                if (item.OrderTrans.Abbredemption.Abbregistration.NewProductCategoryTypeNavigation != null)
                                {
                                    productTypeDesc = item.OrderTrans.Abbredemption.Abbregistration.NewProductCategoryTypeNavigation.Description;
                                }
                                if (item.OrderTrans.Abbredemption.Abbregistration.NewProductCategory != null)
                                {
                                    productCatDesc = item.OrderTrans.Abbredemption.Abbregistration.NewProductCategory.Description;
                                }
                                if (item.OrderTrans.Abbredemption.CustomerDetails != null)
                                {
                                    custCityObj = item.OrderTrans.Abbredemption.CustomerDetails.City;
                                }
                                companyNameObj = item.OrderTrans.Abbredemption?.Abbregistration?.BusinessUnit?.Name;
                                if (item.CreatedDate != null)
                                {
                                    OrderDetailsVM.OrderDueDateTime = Convert.ToDateTime(item.CreatedDate).ToString("dd/MM/yyyy H:mm:ss");
                                }
                                if (item.OrderTrans.Abbredemption.CreatedDate != null)
                                {
                                    OrderDetailsVM.OrderCreatedDateString = Convert.ToDateTime(item.OrderTrans.Abbredemption.CreatedDate).ToString("dd/MM/yyyy H:mm:ss");
                                }
                            }
                            #endregion

                            #region Other table init
                            if (item.Status != null)
                            {
                                OrderDetailsVM.StatusCode = item.Status.StatusCode;
                            }
                            if (item.ServicePartner != null)
                            {
                                OrderDetailsVM.ServicePartnerName = item.ServicePartner.ServicePartnerBusinessName;
                            }
                            #endregion 

                            #region tblOrderQC
                            TblOrderQc? tblOrderQc1 = _context.TblOrderQcs.Where(x => x.OrderTransId == item.OrderTransId && x.IsActive == true && x.PreferredPickupDate != null && x.PickupStartTime != null && x.PickupEndTime != null).FirstOrDefault();
                            if (tblOrderQc1 != null)
                            {
                                // var pickupScheduleDate1 = (DateTime)tblOrderQc1.PreferredPickupDate;
                                OrderDetailsVM.PickupScheduleTime = tblOrderQc1.PickupStartTime + " - " + tblOrderQc1.PickupEndTime;
                                OrderDetailsVM.ProductCondition = tblOrderQc1.QualityAfterQc;
                            }
                            #endregion

                            OrderDetailsVM.Action = actionURL;
                            OrderDetailsVM.Id = item.LogisticId > 0 ? item.LogisticId : 0;
                            OrderDetailsVM.CompanyName = companyNameObj;
                            OrderDetailsVM.RegdNo = item.RegdNo != null ? item.RegdNo : String.Empty;
                            OrderDetailsVM.TicketNumber = item.TicketNumber != null ? item.TicketNumber : String.Empty;
                            OrderDetailsVM.ProductCategory = productCatDesc;
                            OrderDetailsVM.ProductType = productTypeDesc;
                            //OrderDetailsVM.AmountPayableThroughLGC = Convert.ToDecimal(item.AmtPaybleThroughLgc);
                            OrderDetailsVM.CustCity = custCityObj;

                            OrderDetailsVM.PickupScheduleDate = item.PickupScheduleDate != null
                            ? Convert.ToDateTime(item.PickupScheduleDate).ToString("dd/MM/yyyy")
                            : Convert.ToDateTime(item.CreatedDate).ToString("dd/MM/yyyy");

                            OrderDetailsList.Add(OrderDetailsVM);
                        }
                    }
                }
                #endregion

                var data = OrderDetailsList;
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
