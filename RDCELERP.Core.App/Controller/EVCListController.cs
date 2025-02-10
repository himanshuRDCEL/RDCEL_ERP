using AutoMapper;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.CodeAnalysis.Differencing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Threading.Tasks;
using RDCELERP.BAL.Interface;
using RDCELERP.Common.Constant;
using RDCELERP.Common.Enums;
using RDCELERP.Common.Helper;
using RDCELERP.Core.App.Pages.EVC;
using RDCELERP.DAL.Entities;
using RDCELERP.DAL.IRepository;
using RDCELERP.DAL.Repository;
using RDCELERP.Model.Base;
using RDCELERP.Model.CommonModel;
using RDCELERP.Model.EVC;
using RDCELERP.Model.EVC_Allocated;
using RDCELERP.Model.EVC_Allocation;
using RDCELERP.Model.EVC_Portal;
using RDCELERP.Model.EVCdispute;
using RDCELERP.Model.ExchangeOrder;
using RDCELERP.Model.LGC;
using TblUser = RDCELERP.DAL.Entities.TblUser;


namespace RDCELERP.Core.App.Controller
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class EVCListController : ControllerBase
    {
        #region veriable decleration 
        private Digi2l_DevContext _context;
        private IMapper _mapper;
        private CustomDataProtection _protector;
        private IOrderTransRepository _orderTransRepository;
        private IAbbRegistrationRepository _abbRegistrationRepository;
        private readonly IOptions<ApplicationSettings> _config;
        ICompanyRepository _companyRepository;
        IBusinessUnitRepository _businessUnitRepository;
        ICommonManager _commonManager;

        #endregion

        #region Controller 
        public EVCListController(IMapper mapper, Digi2l_DevContext context, CustomDataProtection protector, IOptions<ApplicationSettings> config, IOrderTransRepository orderTransRepository, IAbbRegistrationRepository abbRegistrationRepository, ICompanyRepository companyRepository,
        IBusinessUnitRepository businessUnitRepository, ICommonManager commonManager)
        {
            _context = context;
            _mapper = mapper;
            _protector = protector;
            _config = config;
            _orderTransRepository = orderTransRepository;
            _abbRegistrationRepository = abbRegistrationRepository;
            _companyRepository = companyRepository;
            _businessUnitRepository = businessUnitRepository;
            _commonManager = commonManager;
        }
        #endregion

        #region  ADDED BY Priyanshi Sahu---Get All Not Approverd EVC List //updated Iqueryable
        [HttpPost]
        public async Task<ActionResult> GetEVC_NotApprovedList(DateTime? orderStartDate, DateTime? orderEndDate, string? phoneNo, string? custState, string? custCity, string? custPin, int? createdbyme, string? evcregdNo)
        {
            string URL = _config.Value.URLPrefixforProd;
            List<TblEvcregistration> tblEvcregistrations = null;
            try
            {
                var draw = Request.Form["draw"].FirstOrDefault();
                var start = Request.Form["start"].FirstOrDefault();
                var length = Request.Form["length"].FirstOrDefault();
                var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
                var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();
                var searchValue = Request.Form["search[value]"].FirstOrDefault();
                int pageSize = length != null ? Convert.ToInt32(length) : 0;
                int skip = start != null ? Convert.ToInt32(start) : 0;
                int recordsTotal = 0;

                var query = _context.TblEvcregistrations
                    .Include(x => x.State)
                    .Include(x => x.City)
                    .Include(x => x.EntityType)
                    .Where(x => x.IsActive == true && x.Isevcapprovrd == false);

                if (orderStartDate != null && orderEndDate != null)
                {
                    orderStartDate = orderStartDate.Value.AddMinutes(-1);
                    orderEndDate = orderEndDate.Value.AddDays(1).AddSeconds(-1);
                    query = query.Where(x => x.CreatedDate >= orderStartDate && x.CreatedDate <= orderEndDate);
                }
                if (!string.IsNullOrEmpty(phoneNo))
                {
                    // Placeholder: Add filter for phoneNo
                    query = query.Where(x => x.EvcmobileNumber != null && x.EvcmobileNumber == phoneNo);
                }
                if (!string.IsNullOrEmpty(custState))
                {
                    // Placeholder: Add filter for custState
                    query = query.Where(x => x.State != null && x.State.Name == custState);
                }
                if (!string.IsNullOrEmpty(custCity))
                {
                    // Placeholder: Add filter for custCity
                    query = query.Where(x => x.City != null && x.City.Name == custCity);
                }
                if (!string.IsNullOrEmpty(custPin))
                {
                    // Placeholder: Add filter for custPin
                    query = query.Where(x => x.PinCode != null && x.PinCode == custPin);
                }
                if (!string.IsNullOrEmpty(evcregdNo))
                {
                    // Placeholder: Add filter for regdNo
                    query = query.Where(x => x.EvcregdNo == evcregdNo);
                }
                if (!string.IsNullOrEmpty(searchValue))
                {
                    searchValue = searchValue.ToLower().Trim();
                    query = query.Where(x => x.EvcregdNo.ToLower().Contains(searchValue)
                        || (x.EvcmobileNumber != null && x.EvcmobileNumber.ToLower().Contains(searchValue))
                        || (x.City.Name != null && x.City.Name.ToLower().Contains(searchValue))
                        || (x.State.Name != null && x.State.Name.ToLower().Contains(searchValue))
                        || (x.BussinessName != null && x.BussinessName.ToLower().Contains(searchValue)));
                }

                recordsTotal = await query.CountAsync();

                if (pageSize != -1)
                {
                    if (sortColumnDirection.Equals(SortingOrder.ASCENDING))
                    {
                        query = query.OrderBy(o => EF.Property<object>(o, sortColumn));
                    }
                    else
                    {
                        query = query.OrderByDescending(o => EF.Property<object>(o, sortColumn));
                    }
                    query = query.Skip(skip).Take(pageSize);
                }

                tblEvcregistrations = await query.OrderByDescending(x => x.CreatedDate).ToListAsync();

                List<EVC_NotApprovedViewModel> evcNotApprovedViewList = _mapper.Map<List<TblEvcregistration>, List<EVC_NotApprovedViewModel>>(tblEvcregistrations);
                foreach (EVC_NotApprovedViewModel item in evcNotApprovedViewList)
                {
                    var evcDetails = tblEvcregistrations.FirstOrDefault(x => x.EvcregistrationId == item.EvcregistrationId);
                    if (evcDetails != null)
                    {
                        item.Businessname = evcDetails.EvcregdNo + "-" + evcDetails.BussinessName;
                        item.Address = $"{evcDetails.RegdAddressLine1} {evcDetails.RegdAddressLine2}";

                        if (evcDetails.Isevcapprovrd == null || evcDetails.Isevcapprovrd == false)
                        {
                            item.App_Not = "Not Approved";
                        }
                        else
                        {
                            item.App_Not = "Approved";
                        }

                        item.Type = evcDetails.EmployeeId == 3 ? "Onboarding" : "UTC User Added";
                        item.StateName = evcDetails.State?.Name ?? string.Empty;
                        item.CityName = evcDetails.City?.Name ?? string.Empty;
                        item.CreatedDate = evcDetails.CreatedDate;

                        if (evcDetails.CreatedDate != null)
                        {
                            DateTime dateTime = (DateTime)evcDetails.CreatedDate;
                            item.Date = dateTime.ToString("yyyy-MM-dd");
                        }

                        var tblUser = _context.TblUsers.FirstOrDefault(x => x.UserId == evcDetails.EmployeeId);
                        item.EmployeeName = tblUser?.FirstName ?? string.Empty;

                        var tblEntityType = _context.TblEntityTypes.FirstOrDefault(x => x.EntityTypeId == evcDetails.EntityTypeId);
                        item.EnitityName = tblEntityType?.Name ?? string.Empty;
                    }

                    string actionURL = " <div class='actionbtns'>";
                    actionURL += $"<button onclick='ApprovedEVC({item.EvcregistrationId})' class='btn btn-sm btn-primary'>Approve</button>";
                    actionURL += $"<a class='mx-1 fas fa-edit' href='{URL}/EVC/EVC_Registration?id={_protector.Encode(item.EvcregistrationId)}&AFlag=1' title='Edit'></a>";
                    actionURL += $"<a href='javascript: void(0)' onclick='deleteConfirm({item.EvcregistrationId})' class='fas fa-trash' data-toggle='tooltip' data-placement='top' title='Delete'></a>";
                    actionURL += "</div>";

                    item.Action = actionURL;
                }

                var data = evcNotApprovedViewList;
                var jsonData = new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data };
                return Ok(jsonData);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        #endregion

        #region ADDED BY Priyanshi Sahu---Get All Approverd EVC List //updated Iqueryable
        [HttpPost]
        public async Task<ActionResult> GetEVC_ApprovedList(DateTime? orderStartDate, DateTime? orderEndDate, string? phoneNo, string? custState, string? custCity, string? custPin, int? createdbyme, string? evcregdNo)
        {
            string URL = _config.Value.URLPrefixforProd;
            try
            {
                var draw = Request.Form["draw"].FirstOrDefault();
                var start = Request.Form["start"].FirstOrDefault();
                var length = Request.Form["length"].FirstOrDefault();
                var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
                var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();
                var searchValue = Request.Form["search[value]"].FirstOrDefault();
                int pageSize = length != null ? Convert.ToInt32(length) : 0;
                int skip = start != null ? Convert.ToInt32(start) : 0;
                int recordsTotal = 0;

                IQueryable<TblEvcregistration> query = _context.TblEvcregistrations
                    .Include(x => x.State)
                    .Include(x => x.City)
                    .Include(x => x.EntityType)
                    .Where(x => x.IsActive == true && x.Isevcapprovrd == true);

                //if (!string.IsNullOrEmpty(searchValue))
                //{
                //    query = query.Where(x =>
                //        x.EvcregdNo.ToLower().Contains(searchValue.ToLower().Trim()) ||
                //        (x.EvcmobileNumber != null && x.EvcmobileNumber.ToLower().Contains(searchValue.ToLower().Trim())) ||
                //        (x.City.Name != null && x.City.Name.ToLower().Contains(searchValue.ToLower().Trim())) ||
                //        (x.State.Name != null && x.State.Name.ToLower().Contains(searchValue.ToLower().Trim())) ||
                //        (x.BussinessName != null && x.BussinessName.ToLower().Contains(searchValue.ToLower().Trim()))
                //    );
                //}               
                if (orderStartDate != null && orderEndDate != null)
                {
                    orderStartDate = orderStartDate.Value.AddMinutes(-1);
                    orderEndDate = orderEndDate.Value.AddDays(1).AddSeconds(-1);
                    query = query.Where(x => x.CreatedDate >= orderStartDate && x.CreatedDate <= orderEndDate);
                }
                if (!string.IsNullOrEmpty(phoneNo))
                {
                    // Placeholder: Add filter for phoneNo
                    query = query.Where(x => x.EvcmobileNumber != null && x.EvcmobileNumber == phoneNo);
                }
                if (!string.IsNullOrEmpty(custState))
                {
                    // Placeholder: Add filter for custState
                    query = query.Where(x => x.State != null && x.State.Name == custState);
                }
                if (!string.IsNullOrEmpty(custCity))
                {
                    // Placeholder: Add filter for custCity
                    query = query.Where(x => x.City != null && x.City.Name == custCity);
                }
                if (!string.IsNullOrEmpty(custPin))
                {
                    // Placeholder: Add filter for custPin
                    query = query.Where(x => x.PinCode != null && x.PinCode == custPin);
                }
                if (!string.IsNullOrEmpty(evcregdNo))
                {
                    // Placeholder: Add filter for regdNo
                    query = query.Where(x => x.EvcregdNo == evcregdNo);
                }
                recordsTotal = await query.CountAsync();

                if (!string.IsNullOrEmpty(sortColumn))
                {
                    if (sortColumnDirection.Equals(SortingOrder.ASCENDING))
                    {
                        query = query.OrderBy(x => EF.Property<object>(x, sortColumn));
                    }
                    else
                    {
                        query = query.OrderByDescending(x => EF.Property<object>(x, sortColumn));
                    }
                }

                query = query.OrderByDescending(x => x.ModifiedDate).Skip(skip).Take(pageSize);

                List<TblEvcregistration> tblEvcregistrations = await query.ToListAsync();
                List<EVC_ApprovedViewModel> evcApprovedViewList = _mapper.Map<List<TblEvcregistration>, List<EVC_ApprovedViewModel>>(tblEvcregistrations);

                foreach (EVC_ApprovedViewModel item in evcApprovedViewList)
                {
                    string actionURL = " <div class='actionbtns'>";
                    actionURL += "<a class='btn btn-sm btn-primary' href='" + URL + "/EVC_Portal/EVCUser_AllOrderRecordList?userid=" + item.UserId + "' title='All Order Details'><i class='fa-solid fa-list'></i></a> &nbsp;";
                    actionURL += "<a class='mx-1 fas fa-edit' href='" + URL + "/EVC/EVC_Registration?id=" + _protector.Encode(item.EvcregistrationId) + "&AFlag=2' title ='Edit'></a>";
                    actionURL += "<a href='javascript: void(0)' onclick='deleteConfirm(" + item.EvcregistrationId + ")' class='fas fa-trash' data-toggle='tooltip' data-placement='top' title='Delete'></a>";

                    actionURL += "<a class='btn btn-sm btn-primary' href='" + URL + "/EVC_Portal/EVC_PartnerList?UserId=" + item.UserId + "' title='All Partner Details'><i class='fa-solid fa-handshake'></i></a> &nbsp;";
                    actionURL += "<a class='btn btn-sm btn-primary' href='" + URL + "/EVC_Portal/MyWalletSummary?UserId=" + item.UserId + "' title='All Wallet Details'><i class='fa-solid fa-indian-rupee-sign'></i></a> &nbsp;";

                    actionURL += "</div>";
                    item.Action = actionURL;
                    TblEvcregistration? evcDetails = tblEvcregistrations.FirstOrDefault(x => x.EvcregistrationId == item.EvcregistrationId);

                    if (evcDetails != null)
                    {
                        item.BussinessName = evcDetails.EvcregdNo + "-" + evcDetails.BussinessName;
                        item.Address = $"{evcDetails.RegdAddressLine1} {evcDetails.RegdAddressLine2}";
                        item.App_Not = evcDetails.Isevcapprovrd == null || evcDetails.Isevcapprovrd == false ? "Not Approved" : "Approved";
                        item.Type = evcDetails.EmployeeId == 3 ? "Onboarding" : "UTC User Added";
                        item.StateName = evcDetails?.State?.Name ?? string.Empty;
                        item.CityName = evcDetails?.City?.Name ?? string.Empty;
                        item.Date = evcDetails?.CreatedDate?.ToShortDateString();
                        item.EmployeeName = _context.TblUsers.FirstOrDefault(x => x.UserId == evcDetails.EmployeeId)?.FirstName ?? string.Empty;
                        item.EnitityName = _context.TblEntityTypes.FirstOrDefault(x => x.EntityTypeId == evcDetails.EntityTypeId)?.Name ?? string.Empty;
                    }
                }

                var data = evcApprovedViewList;
                var jsonData = new { draw, recordsFiltered = recordsTotal, recordsTotal, data };
                return Ok(jsonData);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        #endregion

        #region ADDED BY Priyanshi Sahu---Get All Wallet Summary for ALL EVC //updated Iqueryable
        public async Task<ActionResult> GetAllWalletSummary(DateTime? startDate, DateTime? endDate)
        {
            try
            {
                string URL = _config.Value.URLPrefixforProd;

                var draw = Request.Form["draw"].FirstOrDefault();
                var start = Request.Form["start"].FirstOrDefault();
                var length = Request.Form["length"].FirstOrDefault();
                var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
                var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();
                var searchValue = Request.Form["search[value]"].FirstOrDefault();
                int pageSize = length != null ? Convert.ToInt32(length) : 0;
                int skip = start != null ? Convert.ToInt32(start) : 0;
                int recordsTotal = 0;

                IQueryable<TblEvcregistration> query = _context.TblEvcregistrations
                    .Where(x => x.IsActive == true && x.Isevcapprovrd == true &&
                        (string.IsNullOrEmpty(searchValue) ||
                        x.BussinessName.ToLower().Contains(searchValue.ToLower().Trim()) ||
                        x.EvcregdNo.ToLower().Contains(searchValue.ToLower().Trim())));

                if (startDate != null && endDate != null)
                {
                    startDate = startDate.Value.AddMinutes(-1);
                    endDate = endDate.Value.AddDays(1).AddSeconds(-1);
                    query = query.Where(x => x.CreatedDate >= startDate && x.CreatedDate <= endDate);
                }

                recordsTotal = await query.CountAsync();

                string propertyName = typeof(TblEvcregistration).GetProperty(sortColumn) != null ? sortColumn : string.Empty;

                query = sortColumnDirection.Equals(SortingOrder.ASCENDING)
                    ? query.OrderBy(x => EF.Property<object>(x, propertyName))
                    : query.OrderByDescending(x => EF.Property<object>(x, propertyName));

                List<TblEvcregistration> evcregistrations = await query.Skip(skip).Take(pageSize).ToListAsync();

                List<AllWalletSummaryViewModel> allWalletSummaryViewModels = _mapper.Map<List<TblEvcregistration>, List<AllWalletSummaryViewModel>>(evcregistrations);

                foreach (AllWalletSummaryViewModel item in allWalletSummaryViewModels)
                {
                    //string actionURL = " <div class='actionbtns'>";
                    //actionURL += $"<a class='mx-1 fas fa-edit' href='{URL}/EVC/EVC_Registration?id={_protector.Encode(item.EvcregistrationId)}&AFlag=2' title='Edit'></a>";
                    //actionURL += $"<a href='javascript:void(0)' onclick='deleteConfirm({item.EvcregistrationId})' class='fas fa-trash' data-toggle='tooltip' data-placement='top' title='Delete'></a>";
                    //actionURL += "</div>";
                    //item.Action = actionURL;

                    TblEvcregistration? evcRegistration = evcregistrations.FirstOrDefault(x => x.EvcregistrationId == item.EvcregistrationId);
                    if (evcRegistration != null)
                    {
                        EVCClearBalanceViewModel? eVCClearBalanceViewModel = new EVCClearBalanceViewModel();
                        eVCClearBalanceViewModel = _commonManager.CalculateEVCClearBalance(evcRegistration.EvcregistrationId);
                        if (eVCClearBalanceViewModel != null)
                        {
                            item.TotalofInprogress = eVCClearBalanceViewModel.InProgresAmount;
                            item.TotalofDeliverd = eVCClearBalanceViewModel.DeliverdAmount;
                            item.RuningBalance = eVCClearBalanceViewModel.clearBalance;
                            item.EvcName = $"{item.EvcregdNo}-{item.BussinessName}";
                        }
                    }

                    if (evcRegistration != null && evcRegistration.EmployeeId > 0)
                    {
                        TblUser tblUser = await _context.TblUsers.FirstOrDefaultAsync(x => x.UserId == evcRegistration.EmployeeId);
                        item.EmployeeName = tblUser?.FirstName ?? string.Empty;
                    }

                    if (evcRegistration.CreatedDate != null)
                    {
                        DateTime dateTime = (DateTime)evcRegistration.CreatedDate;
                        item.Date = dateTime.ToString("yyyy-MM-dd");
                        item.CreatedDate = evcRegistration.CreatedDate;
                    }
                }

                var data = allWalletSummaryViewModels;
                var jsonData = new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data };
                return Ok(jsonData);
            }
            catch (Exception ex)
            {
                // Handle the exception appropriately
                return StatusCode(500, "An error occurred while processing the request.");
            }
        }


        #endregion

        #region ADDED BY Priyanshi Sahu---Add EVC Wallet Balance //updated Iqueryable
        public async Task<ActionResult> AddEVCWalletBalance(DateTime? orderStartDate, DateTime? orderEndDate, string? phoneNo, string? custState, string? custCity, string? custPin, string? evcregdNo)
        {
            string URL = _config.Value.URLPrefixforProd;
            try
            {
                var draw = Request.Form["draw"].FirstOrDefault();
                var start = Request.Form["start"].FirstOrDefault();
                var length = Request.Form["length"].FirstOrDefault();
                var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
                var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();
                var searchValue = Request.Form["search[value]"].FirstOrDefault();
                int pageSize = length != null ? Convert.ToInt32(length) : 0;
                int skip = start != null ? Convert.ToInt32(start) : 0;
                int recordsTotal = 0;

                IQueryable<TblEvcregistration> evcRegistrationsQuery = _context.TblEvcregistrations
                    .Include(x => x.State)
                    .Include(x => x.City)
                    .Include(x => x.EntityType)
                    .Where(x => x.IsActive == true && x.Isevcapprovrd == true);
                    

                if (orderStartDate != null && orderEndDate != null)
                {
                    orderStartDate = orderStartDate.Value.AddMinutes(-1);
                    orderEndDate = orderEndDate.Value.AddDays(1).AddSeconds(-1);
                    evcRegistrationsQuery = evcRegistrationsQuery.Where(x => x.CreatedDate >= orderStartDate && x.CreatedDate <= orderEndDate);
                }
                if (!string.IsNullOrEmpty(phoneNo))
                {
                    // Placeholder: Add filter for phoneNo
                    evcRegistrationsQuery = evcRegistrationsQuery.Where(x => x.EvcmobileNumber != null && x.EvcmobileNumber == phoneNo);
                }
                if (!string.IsNullOrEmpty(custState))
                {
                    // Placeholder: Add filter for custState
                    evcRegistrationsQuery = evcRegistrationsQuery.Where(x => x.State != null && x.State.Name.ToLower().Equals(custState.ToLower()));
                }
                if (!string.IsNullOrEmpty(custCity))
                {
                    // Placeholder: Add filter for custCity
                    evcRegistrationsQuery = evcRegistrationsQuery.Where(x => x.City != null && x.City.Name.ToLower().Equals(custCity.ToLower()));
                }
                if (!string.IsNullOrEmpty(custPin))
                {
                    // Placeholder: Add filter for custPin
                    evcRegistrationsQuery = evcRegistrationsQuery.Where(x => x.PinCode != null && x.PinCode == custPin);
                }
                if (!string.IsNullOrEmpty(evcregdNo))
                {
                    // Placeholder: Add filter for regdNo
                    evcRegistrationsQuery = evcRegistrationsQuery.Where(x => x.EvcregdNo == evcregdNo);
                }
                recordsTotal = await evcRegistrationsQuery.CountAsync();

                if (!string.IsNullOrEmpty(sortColumn))
                {
                    if (sortColumnDirection.Equals(SortingOrder.ASCENDING))
                    {
                        evcRegistrationsQuery = evcRegistrationsQuery.OrderBy(x => EF.Property<object>(x, sortColumn));
                    }
                    else
                    {
                        evcRegistrationsQuery = evcRegistrationsQuery.OrderByDescending(x => EF.Property<object>(x, sortColumn));
                    }
                }

                

                string actionURL = string.Empty;
                string actionURL1 = string.Empty;


                evcRegistrationsQuery = evcRegistrationsQuery.OrderByDescending(x => x.CreatedDate)
                    .Skip(skip)
                    .Take(pageSize);

                List<TblEvcregistration> evcRegistrations = await evcRegistrationsQuery.ToListAsync();

                List<AllWalletSummaryViewModel> allWalletSummaryViewModels = _mapper.Map<List<TblEvcregistration>, List<AllWalletSummaryViewModel>>(evcRegistrations);

                foreach (AllWalletSummaryViewModel item in allWalletSummaryViewModels)
                {
                    actionURL = " <div class='actionbtns'>";
                    actionURL += "<button onclick=\"AddWalletConfirm('" + item.EvcregistrationId + "')\" class='btn btn-sm btn-primary'>Add Wallet</button> &nbsp;";
                    actionURL += "<a class='btn btn-sm btn-primary' href='" + URL + "/EVC_Portal/MyWalletSummary?UserId=" + item.UserId + "' title='All Wallet Details'><i class='fa-solid fa-indian-rupee-sign'></i></a> &nbsp;";


                    item.Action = actionURL;

                    TblEvcregistration evcRegistration = evcRegistrations.FirstOrDefault(x => x.EvcregistrationId == item.EvcregistrationId);
                    if (evcRegistration != null)
                    {
                        EVCClearBalanceViewModel? eVCClearBalanceViewModel = new EVCClearBalanceViewModel();
                        eVCClearBalanceViewModel = _commonManager.CalculateEVCClearBalance(evcRegistration.EvcregistrationId);
                        if (eVCClearBalanceViewModel != null)
                        {
                            item.TotalofInprogress = eVCClearBalanceViewModel.InProgresAmount;
                            item.TotalofDeliverd = eVCClearBalanceViewModel.DeliverdAmount;
                            item.RuningBalance = eVCClearBalanceViewModel.clearBalance;
                            item.EvcName = $"{item.EvcregdNo}-{item.BussinessName}";
                        }
                    }
                    if (evcRegistration != null && evcRegistration.EmployeeId > 0)
                    {
                        TblUser tblUser = _context.TblUsers.FirstOrDefault(x => x.UserId == evcRegistration.EmployeeId);
                        item.EmployeeName = tblUser?.FirstName ?? string.Empty;
                    }
                    if (evcRegistration?.CreatedDate != null)
                    {
                        DateTime dateTime = (DateTime)evcRegistration.CreatedDate;
                        item.Date = dateTime.ToString("yyyy-MM-dd");
                        item.CreatedDate = evcRegistration.CreatedDate;
                    }
                }

                var data = allWalletSummaryViewModels;
                var jsonData = new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data };
                return Ok(jsonData);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        #endregion

        #region ADDED BY Priyanshi Sahu---- Get All Not Allocate Record //updated Iqueryable and ABB 
        public async Task<ActionResult> GetNotAllocatedOrderList(int companyId, DateTime? orderStartDate,
            DateTime? orderEndDate, string? companyName, int? productCatId,
            int? productTypeId, string? regdNo, string? phoneNo, string? custCity, int? createdbyme)
        {

            #region Variable declaration
            if (!string.IsNullOrWhiteSpace(companyName) && companyName != "null")
            { companyName = companyName.Trim().ToLower(); }
            else { companyName = null; }
            if (!string.IsNullOrWhiteSpace(regdNo) && regdNo != "null")
            { regdNo = regdNo.Trim().ToLower(); }
            else { regdNo = null; }
            if (!string.IsNullOrWhiteSpace(phoneNo) && phoneNo != "null")
            { phoneNo = phoneNo.Trim().ToLower(); }
            else { phoneNo = null; }
            if (!string.IsNullOrWhiteSpace(custCity) && custCity != "null")
            { custCity = custCity.Trim().ToLower(); }
            else { custCity = null; }


            string URL = _config.Value.URLPrefixforProd;
            string MVCURL = _config.Value.MVCBaseURLForExchangeInvoice;
            string InvoiceimagURL = string.Empty;
            TblCompany tblCompany = null;
            TblBusinessUnit? tblBusinessUnit = null;

            #endregion
            try
            {

                var draw = Request.Form["draw"].FirstOrDefault();
                var start = Request.Form["start"].FirstOrDefault();
                var length = Request.Form["length"].FirstOrDefault();
                var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
                var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();
                var searchValue = Request.Form["search[value]"].FirstOrDefault();
                int pageSize = length != null ? Convert.ToInt32(length) : 0;
                int skip = start != null ? Convert.ToInt32(start) : 0;
                int recordsTotal = 0;
                IQueryable<TblOrderTran> query = null;

                #region Advanced Filters Mapping
                if (companyId > 0 && companyId != 1007)
                {
                    tblCompany = _companyRepository.GetCompanyId(companyId);
                    if (tblCompany != null)
                    {
                        tblBusinessUnit = _businessUnitRepository.Getbyid(tblCompany.BusinessUnitId);
                    }
                }


                #endregion



                query = _context.TblOrderTrans
                    .Include(x => x.Exchange)
                        .ThenInclude(x => x.ProductType)
                            .ThenInclude(x => x.ProductCat)
                    .Include(x => x.Exchange)
                        .ThenInclude(x => x.CustomerDetails)
                    .Include(x => x.Status)

                    .Include(x => x.Abbredemption).ThenInclude(x => x.Abbregistration).ThenInclude(x => x.NewProductCategory)
                .Include(x => x.Abbredemption).ThenInclude(x => x.Abbregistration).ThenInclude(x => x.NewProductCategoryTypeNavigation)
                .Include(x => x.Abbredemption).ThenInclude(x => x.CustomerDetails)
                  .Include(x => x.Abbredemption).ThenInclude(x => x.Abbregistration).ThenInclude(x => x.BusinessUnit)
                    .Where(x => x.IsActive == true
                    //&&(tblBusinessUnit == null || (x.Exchange != null ?
                    //(x.Exchange.CompanyName == tblBusinessUnit.Name) : (x.Abbredemption.Abbregistration.BusinessUnitId == tblBusinessUnit.BusinessUnitId)))
                    && x.RegdNo != null && x.ExchangePrice != null &&
                        (x.StatusId == 15 || x.StatusId == 17) && x.FinalPriceAfterQc != null && (x.OrderType == Convert.ToInt32(OrderTypeEnum.Exchange) ? (x.Exchange != null && x.Exchange.FinalExchangePrice != null) : (x.Abbredemption != null)));


                if (orderStartDate != null && orderEndDate != null)
                {
                    orderStartDate = orderStartDate.Value.AddMinutes(-1);
                    orderEndDate = orderEndDate.Value.AddDays(1).AddSeconds(-1);
                    query = query.Where(x => x.ModifiedDate >= orderStartDate && x.ModifiedDate <= orderEndDate);
                }
                // Advanced Filters Mapping
                if (tblBusinessUnit != null)
                {
                    // Placeholder: Add filter for companyId
                    query = query.Where(x => x.Exchange != null ? (x.Exchange.BusinessPartner.BusinessUnit.BusinessUnitId == tblBusinessUnit.BusinessUnitId) : (x.Abbredemption.Abbregistration.BusinessUnitId == tblBusinessUnit.BusinessUnitId));
                }

                if (productCatId.HasValue)
                {
                    // Placeholder: Add filter for productCatId
                    query = query.Where(x => x.Exchange != null ? (x.Exchange.ProductType.ProductCat.Id == productCatId) : (x.Abbredemption.Abbregistration.NewProductCategoryId == productCatId));
                }

                if (productTypeId.HasValue)
                {
                    // Placeholder: Add filter for productTypeId
                    query = query.Where(x => x.Exchange != null ? (x.Exchange.ProductType.Id == productTypeId) : (x.Abbredemption.Abbregistration.NewProductCategoryTypeId == productTypeId));
                }

                if (!string.IsNullOrEmpty(regdNo))
                {
                    // Placeholder: Add filter for regdNo
                    query = query.Where(x => x.RegdNo == regdNo);
                }

                if (!string.IsNullOrEmpty(phoneNo))
                {
                    // Placeholder: Add filter for phoneNo
                    query = query.Where(x => x.Exchange != null ? (x.Exchange.CustomerDetails.PhoneNumber == phoneNo) : (x.Abbredemption.CustomerDetails.PhoneNumber == phoneNo));
                }

                if (!string.IsNullOrEmpty(custCity))
                {
                    // Placeholder: Add filter for custCity
                    query = query.Where(x => x.Exchange != null ? (x.Exchange.CustomerDetails.City == custCity) : (x.Abbredemption.CustomerDetails.City == custCity));
                }
                if (!string.IsNullOrEmpty(companyName))
                {
                    // Placeholder: Add filter for productTypeId
                    query = query.Where(x => x.Exchange != null ? (x.Exchange.BusinessPartner.BusinessUnit.Name == companyName) : (x.Abbredemption.Abbregistration.BusinessUnit.Name == companyName));
                }

                if (createdbyme > 0)
                {
                    // Placeholder: Add filter for productTypeId
                    query = query.Where(x => x.CreatedBy == createdbyme);
                }
                recordsTotal = await query.CountAsync();

                query = query.OrderByDescending(x => x.ModifiedDate)
                    .Skip(skip)
                    .Take(pageSize);

                List<TblOrderTran> TblOrderTrans = query.ToList();

                List<NotAllocatedOrderViewModel> NotAllocationViewList = _mapper.Map<List<TblOrderTran>, List<NotAllocatedOrderViewModel>>(TblOrderTrans);

                foreach (NotAllocatedOrderViewModel item in NotAllocationViewList)
                {
                    string actionURL = "<td class='actions'>";
                    actionURL += "<span><input type='checkbox' id='EVCAllocionCB' value=" + item.OrderTransId + " onclick='OnCheckBoxCheck();' class='checkboxinput' /></span>";
                    actionURL += "</td>";
                    item.Action = actionURL;

                    string actionURL1 = " <a class='btn btn-sm btn-primary' href='" + URL + "/Index1?orderTransId=" + item.OrderTransId + "' title='TimeLine'><i class='fa solid fa-clock-rotate-left'></i></a> &nbsp;" + "<span><button onclick='PrimeProduct(" + item.OrderTransId + ")' class='btn btn-sm btn-primary'>Assign Prime Product</button></span>";
                    item.Edit = actionURL1;

                    TblOrderTran OrdeDetails = TblOrderTrans.FirstOrDefault(x => x.OrderTransId == item.OrderTransId);
                    if (OrdeDetails != null)
                    {
                        if (OrdeDetails.OrderType == Convert.ToInt32(OrderTypeEnum.Exchange))
                        {
                            item.ordertype = "Exchange";

                            item.ExchangeId = OrdeDetails.ExchangeId;
                            if (OrdeDetails.Exchange.RegdNo != null)
                            {
                                item.RegdNo = OrdeDetails.Exchange.RegdNo;
                                item.Bonus = OrdeDetails.Exchange.Bonus;
                                item.FinalExchangePrice = OrdeDetails.Exchange.FinalExchangePrice;
                                item.SponsorName = OrdeDetails.Exchange.CompanyName;
                            }
                            if (OrdeDetails.Exchange.ProductTypeId != null)
                            {
                                item.OldProdType = OrdeDetails.Exchange.ProductType.Description;
                            }
                            if (OrdeDetails.Exchange.ProductType.ProductCat != null)
                            {
                                item.Prodcat = OrdeDetails.Exchange.ProductType.ProductCat.Description;
                            }
                            if (OrdeDetails.Exchange.CustomerDetails != null)
                            {
                                item.FirstName = OrdeDetails.Exchange.CustomerDetails.FirstName + " " + OrdeDetails.Exchange.CustomerDetails.LastName;
                                item.CustCity = OrdeDetails.Exchange.CustomerDetails.City;
                                item.CustPin = OrdeDetails.Exchange.CustomerDetails.ZipCode;
                                item.StateName = OrdeDetails.Exchange.CustomerDetails.State;
                            }
                            if (OrdeDetails.ModifiedDate != null)
                            {
                                item.Date = OrdeDetails.ModifiedDate.Value.ToShortDateString();
                            }
                            if (OrdeDetails.Status != null)
                            {
                                item.status = OrdeDetails.Status.StatusCode;
                            }
                        }
                        else if (OrdeDetails.OrderType == Convert.ToInt32(OrderTypeEnum.ABB))
                        {
                            item.abbRedemtionId = (int)OrdeDetails.AbbredemptionId;
                            item.ordertype = "ABB";
                            if (OrdeDetails.RegdNo != null)
                            {
                                item.RegdNo = OrdeDetails.RegdNo;
                                //item.Bonus = OrdeDetails.Exchange.Bonus;
                                item.FinalExchangePrice = OrdeDetails.FinalPriceAfterQc != null ? OrdeDetails.FinalPriceAfterQc : 0;
                                item.SponsorName = OrdeDetails.Abbredemption.Abbregistration.BusinessUnit.Name;
                            }

                            if (OrdeDetails.Abbredemption.Abbregistration != null)
                            {
                                item.FirstName = OrdeDetails.Abbredemption.Abbregistration.CustFirstName + " " + OrdeDetails.Abbredemption.Abbregistration.CustLastName;
                                item.CustCity = OrdeDetails.Abbredemption.Abbregistration.CustCity;
                                item.CustPin = OrdeDetails.Abbredemption.Abbregistration.CustPinCode;
                                item.StateName = OrdeDetails.Abbredemption.Abbregistration.CustState;
                                item.OldProdType = OrdeDetails.Abbredemption.Abbregistration.NewProductCategoryTypeNavigation.Description;
                                item.Prodcat = OrdeDetails.Abbredemption.Abbregistration.NewProductCategory.Description;
                            }
                            if (OrdeDetails.ModifiedDate != null)
                            {
                                item.Date = OrdeDetails.ModifiedDate.Value.ToShortDateString();
                            }
                            if (OrdeDetails.Status != null)
                            {
                                item.status = OrdeDetails.Status.StatusCode;
                            }
                        }

                    }
                }

                NotAllocationViewList = NotAllocationViewList.OrderByDescending(x => x.ModifiedDate).ToList();
                var data = NotAllocationViewList;
                var jsonData = new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data };
                return Ok(jsonData);

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        #endregion

        #region ADDED BY Priyanshi Sahu---- Get All Assign Order Record //updated Iqueryable 
        public async Task<ActionResult> GetAssignOrderList(int companyId, DateTime? orderStartDate,
            DateTime? orderEndDate, string? companyName, int? productCatId,
            int? productTypeId, string? regdNo, string? phoneNo, string? custCity, int? createdbyme, string? evcregdNo, string? evcstoreCode)
        {
            #region Variable declaration
            if (!string.IsNullOrWhiteSpace(companyName) && companyName != "null")
            { companyName = companyName.Trim().ToLower(); }
            else { companyName = null; }
            if (!string.IsNullOrWhiteSpace(regdNo) && regdNo != "null")
            { regdNo = regdNo.Trim().ToLower(); }
            else { regdNo = null; }
            if (!string.IsNullOrWhiteSpace(evcstoreCode) && evcstoreCode != "null")
            { evcstoreCode = evcstoreCode.Trim().ToLower(); }
            else { evcstoreCode = null; }
            if (!string.IsNullOrWhiteSpace(phoneNo) && phoneNo != "null")
            { phoneNo = phoneNo.Trim().ToLower(); }
            else { phoneNo = null; }
            if (!string.IsNullOrWhiteSpace(custCity) && custCity != "null")
            { custCity = custCity.Trim().ToLower(); }
            else { custCity = null; }


            string URL = _config.Value.URLPrefixforProd;
            string MVCURL = _config.Value.MVCBaseURLForExchangeInvoice;
            string InvoiceimagURL = string.Empty;
            TblCompany tblCompany = null;
            TblBusinessUnit tblBusinessUnit = null;
            TblEvcPartner? tblEvcPartner = null;
            #endregion
            try
            {


                var draw = Request.Form["draw"].FirstOrDefault();
                var start = Request.Form["start"].FirstOrDefault();
                var length = Request.Form["length"].FirstOrDefault();
                var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
                var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();
                var searchValue = Request.Form["search[value]"].FirstOrDefault();
                int pageSize = length != null ? Convert.ToInt32(length) : 0;
                int skip = start != null ? Convert.ToInt32(start) : 0;

                #region Advanced Filters Mapping
                if (companyId > 0 && companyId != 1007)
                {
                    tblCompany = _companyRepository.GetCompanyId(companyId);
                    if (tblCompany != null)
                    {
                        tblBusinessUnit = _businessUnitRepository.Getbyid(tblCompany.BusinessUnitId);
                    }
                }
                #endregion
                IQueryable<TblWalletTransaction> query = _context.TblWalletTransactions
                    .Include(x => x.Evcregistration)
                    .Include(x => x.Evcpartner)
                    .Include(x => x.OrderTrans).ThenInclude(x => x.Exchange).ThenInclude(x => x.ProductType).ThenInclude(x => x.ProductCat)
                    .Include(x => x.OrderTrans).ThenInclude(x => x.Exchange).ThenInclude(x => x.CustomerDetails)
                    .Include(x => x.OrderTrans).ThenInclude(x => x.Abbredemption).ThenInclude(x => x.Abbregistration).ThenInclude(x => x.NewProductCategory)
                    .Include(x => x.OrderTrans).ThenInclude(x => x.Abbredemption).ThenInclude(x => x.Abbregistration).ThenInclude(x => x.NewProductCategoryTypeNavigation)
                    .Include(x => x.OrderTrans).ThenInclude(x => x.Abbredemption).ThenInclude(x => x.CustomerDetails)
                    .Include(x => x.OrderTrans).ThenInclude(x => x.Abbredemption).ThenInclude(x => x.Abbregistration).ThenInclude(x => x.BusinessUnit)
                    .Where(x => x.IsActive == true
                    && x.OrderTrans.StatusId == Convert.ToInt32(OrderStatusEnum.EVCAllocationcompleted) && x.ModifiedDate != null && x.OrderofAssignDate != null);

                if (orderStartDate != null && orderEndDate != null)
                {
                    orderStartDate = orderStartDate.Value.AddMinutes(-1);
                    orderEndDate = orderEndDate.Value.AddDays(1).AddSeconds(-1);
                    query = query.Where(x => x.CreatedDate >= orderStartDate && x.CreatedDate <= orderEndDate);
                }
                // Advanced Filters Mapping
                if (tblBusinessUnit != null)
                {
                    // Placeholder: Add filter for companyId
                    query = query.Where(x => x.OrderTrans.Exchange != null ? (x.OrderTrans.Exchange.BusinessPartner.BusinessUnit.BusinessUnitId == tblBusinessUnit.BusinessUnitId) : (x.OrderTrans.Abbredemption.Abbregistration.BusinessUnitId == tblBusinessUnit.BusinessUnitId));
                }
                if (productCatId.HasValue)
                {
                    // Placeholder: Add filter for productCatId
                    query = query.Where(x => x.OrderTrans.Exchange != null ? (x.OrderTrans.Exchange.ProductType.ProductCat.Id == productCatId) : (x.OrderTrans.Abbredemption.Abbregistration.NewProductCategoryId == productCatId));
                }

                if (productTypeId.HasValue)
                {
                    // Placeholder: Add filter for productTypeId
                    query = query.Where(x => x.OrderTrans.Exchange != null ? (x.OrderTrans.Exchange.ProductType.Id == productTypeId) : (x.OrderTrans.Abbredemption.Abbregistration.NewProductCategoryTypeId == productTypeId));
                }

                if (!string.IsNullOrEmpty(regdNo))
                {
                    // Placeholder: Add filter for regdNo
                    query = query.Where(x => x.RegdNo == regdNo);
                }

                if (!string.IsNullOrEmpty(phoneNo))
                {
                    // Placeholder: Add filter for phoneNo
                    query = query.Where(x => x.OrderTrans.Exchange != null ? (x.OrderTrans.Exchange.CustomerDetails.PhoneNumber == phoneNo) : (x.OrderTrans.Abbredemption.CustomerDetails.PhoneNumber == phoneNo));
                }

                if (!string.IsNullOrEmpty(custCity))
                {
                    // Placeholder: Add filter for custCity
                    query = query.Where(x => x.OrderTrans.Exchange != null ? (x.OrderTrans.Exchange.CustomerDetails.City == custCity) : (x.OrderTrans.Abbredemption.CustomerDetails.City == custCity));
                }
                if (!string.IsNullOrEmpty(companyName))
                {
                    // Placeholder: Add filter for productTypeId
                    query = query.Where(x => x.OrderTrans.Exchange != null ? (x.OrderTrans.Exchange.BusinessPartner.BusinessUnit.Name == companyName) : (x.OrderTrans.Abbredemption.Abbregistration.BusinessUnit.Name == companyName));
                }

                if (createdbyme > 0)
                {
                    // Placeholder: Add filter for productTypeId
                    query = query.Where(x => x.OrderTrans.CreatedBy == createdbyme);
                }
                if (!string.IsNullOrEmpty(evcregdNo))
                {
                    // Placeholder: Add filter for regdNo 
                    query = query.Where(x => x.Evcregistration.EvcregdNo == evcregdNo);
                }

                if (!string.IsNullOrEmpty(evcstoreCode))
                {
                    // Placeholder: Add filter for regdNo evcstoreCode
                    query = query.Where(x => x.Evcpartner.EvcStoreCode == evcstoreCode);
                }

                if (!string.IsNullOrEmpty(phoneNo))
                {
                    // Placeholder: Add filter for phoneNo
                    query = query.Where(x => x.OrderTrans.Exchange != null ? (x.OrderTrans.Exchange.CustomerDetails.PhoneNumber == phoneNo) : (x.OrderTrans.Abbredemption.CustomerDetails.PhoneNumber == phoneNo));
                }

                int recordsTotal = query.Count();


                query = query.OrderByDescending(x => x.ModifiedDate)
                   .Skip(skip)
                   .Take(pageSize);

                List<TblWalletTransaction> tblWalletTransactions = query.ToList();

                List<AssignOrderViewModel> assignOrderViewModels = _mapper.Map<List<TblWalletTransaction>, List<AssignOrderViewModel>>(tblWalletTransactions);

                foreach (AssignOrderViewModel oAssignOrderViewModel in assignOrderViewModels)
                {
                    string actionURL = $" <td class='actions'>";
                    actionURL += $"<a class='btn btn-sm btn-primary' href='{URL}/Index1?orderTransId={oAssignOrderViewModel.OrderTransId}' title='TimeLine'><i class='fa solid fa-clock-rotate-left'></i></a> &nbsp;";
                    actionURL += " </td>";
                    oAssignOrderViewModel.Action = actionURL;
                    string actionURL1 = "<button onclick='ReassignOrder(" + oAssignOrderViewModel.OrderTransId + ")' class='btn btn-sm btn-primary'><i class='fa-solid fa-repeat'></i>&nbsp; Re-assign</button>";
                    oAssignOrderViewModel.Edit = actionURL1;

                    TblWalletTransaction orderDetails = tblWalletTransactions.FirstOrDefault(x => x.OrderTransId == oAssignOrderViewModel.OrderTransId);

                    if (orderDetails != null)
                    {
                        oAssignOrderViewModel.OrderTransId = orderDetails.OrderTransId;
                        oAssignOrderViewModel.EvcRate = orderDetails.OrderAmount > 0 ? orderDetails.OrderAmount : 0;
                        oAssignOrderViewModel.RegdNo = orderDetails.OrderTrans.RegdNo ?? string.Empty;
                        oAssignOrderViewModel.EvcRegNo = orderDetails.Evcregistration != null ? $"{orderDetails.Evcregistration.EvcregdNo}-{orderDetails.Evcregistration.BussinessName}" : string.Empty;
                        oAssignOrderViewModel.EvcStoreCode = orderDetails.Evcpartner?.EvcStoreCode ?? string.Empty;

                        if (orderDetails.OrderofAssignDate != null)
                        {
                            oAssignOrderViewModel.Date = orderDetails.OrderofAssignDate.Value.ToShortDateString() ?? string.Empty;
                        }
                        oAssignOrderViewModel.FinalExchangePrice = orderDetails.OrderTrans.FinalPriceAfterQc ?? 0;
                        oAssignOrderViewModel.IsPrimeProductId = orderDetails.IsPrimeProductId == true ? "Yes" : "No";
                        oAssignOrderViewModel.ModifyDate = orderDetails.ModifiedDate != null ? orderDetails.ModifiedDate.Value.ToShortDateString() : string.Empty;
                        if (orderDetails.OrderType != null && (orderDetails.OrderType == Convert.ToInt32(OrderTypeEnum.Exchange).ToString()))
                        {
                            oAssignOrderViewModel.ordertype = "Exchange";
                            oAssignOrderViewModel.ExchProdGroup = orderDetails.OrderTrans.Exchange.ProductType.Description ?? string.Empty;
                            oAssignOrderViewModel.OldProdType = orderDetails.OrderTrans.Exchange.ProductType.ProductCat.Description ?? string.Empty;
                            oAssignOrderViewModel.FirstName = orderDetails.OrderTrans.Exchange.CustomerDetails.FirstName + " " + orderDetails.OrderTrans.Exchange.CustomerDetails.LastName ?? string.Empty;
                            oAssignOrderViewModel.CustCity = orderDetails.OrderTrans.Exchange.CustomerDetails.City ?? string.Empty;
                            oAssignOrderViewModel.CustPin = orderDetails.OrderTrans.Exchange.CustomerDetails.ZipCode ?? string.Empty;
                        }
                        if (orderDetails.OrderType != null && (orderDetails.OrderType == Convert.ToInt32(OrderTypeEnum.ABB).ToString()))
                        {
                            oAssignOrderViewModel.ordertype = "ABB";
                            oAssignOrderViewModel.ExchProdGroup = orderDetails.OrderTrans.Abbredemption.Abbregistration.NewProductCategory.Description ?? string.Empty;
                            oAssignOrderViewModel.OldProdType = orderDetails.OrderTrans.Abbredemption.Abbregistration.NewProductCategoryTypeNavigation.Description ?? string.Empty;
                            oAssignOrderViewModel.FirstName = orderDetails.OrderTrans.Abbredemption.CustomerDetails.FirstName + " " + orderDetails.OrderTrans.Abbredemption.CustomerDetails.LastName ?? string.Empty;
                            oAssignOrderViewModel.CustCity = orderDetails.OrderTrans.Abbredemption.CustomerDetails.City ?? string.Empty;
                            oAssignOrderViewModel.CustPin = orderDetails.OrderTrans.Abbredemption.CustomerDetails.ZipCode ?? string.Empty;
                        }
                    }
                }

                assignOrderViewModels = assignOrderViewModels.OrderByDescending(x => x.ModifiedDate).ToList();

                var data = assignOrderViewModels;
                var jsonData = new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data };
                return Ok(jsonData);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        #endregion

        #region ADDED BY Priyanshi Sahu---- Get All Pickup decline Record //updated Iqueryable
        public async Task<ActionResult> GetPickupDeclineList(int companyId, DateTime? orderStartDate,
            DateTime? orderEndDate, string? companyName, int? productCatId,
            int? productTypeId, string? regdNo, string? phoneNo, string? custCity, int? createdbyme, string? evcregdNo, string? evcstoreCode)
        {
            #region Variable declaration
            if (!string.IsNullOrWhiteSpace(companyName) && companyName != "null")
            { companyName = companyName.Trim().ToLower(); }
            else { companyName = null; }
            if (!string.IsNullOrWhiteSpace(regdNo) && regdNo != "null")
            { regdNo = regdNo.Trim().ToLower(); }
            else { regdNo = null; }
            if (!string.IsNullOrWhiteSpace(phoneNo) && phoneNo != "null")
            { phoneNo = phoneNo.Trim().ToLower(); }
            else { phoneNo = null; }
            if (!string.IsNullOrWhiteSpace(custCity) && custCity != "null")
            { custCity = custCity.Trim().ToLower(); }
            else { custCity = null; }
            if (!string.IsNullOrWhiteSpace(evcregdNo) && evcregdNo != "null")
            { evcregdNo = evcregdNo.Trim().ToLower(); }
            else { evcregdNo = null; }


            string URL = _config.Value.URLPrefixforProd;
            string MVCURL = _config.Value.MVCBaseURLForExchangeInvoice;
            string InvoiceimagURL = string.Empty;
            TblCompany tblCompany = null;
            TblBusinessUnit tblBusinessUnit = null;
            #endregion
            try
            {

                var draw = Request.Form["draw"].FirstOrDefault();
                var start = Request.Form["start"].FirstOrDefault();
                var length = Request.Form["length"].FirstOrDefault();
                var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
                var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();
                var searchValue = Request.Form["search[value]"].FirstOrDefault();
                int pageSize = length != null ? Convert.ToInt32(length) : 0;
                int skip = start != null ? Convert.ToInt32(start) : 0;
                int recordsTotal = 0;
                List<PickupDeclineViewModel> pickupDeclineViewModel;
                #region Advanced Filters Mapping
                if (companyId > 0 && companyId != 1007)
                {
                    tblCompany = _companyRepository.GetCompanyId(companyId);
                    if (tblCompany != null)
                    {
                        tblBusinessUnit = _businessUnitRepository.Getbyid(tblCompany.BusinessUnitId);
                    }
                }
                #endregion
                IQueryable<TblWalletTransaction> query = _context.TblWalletTransactions
                    .Include(x => x.Evcregistration)
                    .Include(x => x.Evcpartner)
                    .Include(x => x.OrderTrans).ThenInclude(x => x.Status)
                    .Include(x => x.OrderTrans).ThenInclude(x => x.Exchange).ThenInclude(x => x.ProductType).ThenInclude(x => x.ProductCat)
                    .Include(x => x.OrderTrans).ThenInclude(x => x.Exchange).ThenInclude(x => x.CustomerDetails)
                     .Include(x => x.OrderTrans).ThenInclude(x => x.Abbredemption).ThenInclude(x => x.Abbregistration).ThenInclude(x => x.NewProductCategory)
                .Include(x => x.OrderTrans).ThenInclude(x => x.Abbredemption).ThenInclude(x => x.Abbregistration).ThenInclude(x => x.NewProductCategoryTypeNavigation)
               .Include(x => x.OrderTrans).ThenInclude(x => x.Abbredemption).ThenInclude(x => x.CustomerDetails)
                  .Include(x => x.OrderTrans).ThenInclude(x => x.Abbredemption).ThenInclude(x => x.Abbregistration).ThenInclude(x => x.BusinessUnit)
                    .Where(x => x.IsActive == true
                    //&& (tblBusinessUnit == null || (x.OrderTrans.Exchange != null ?
                    //(x.OrderTrans.Exchange.CompanyName == tblBusinessUnit.Name) : (x.OrderTrans.Abbredemption.Abbregistration.BusinessUnitId == tblBusinessUnit.BusinessUnitId)))
                    && x.OrderTrans.StatusId == Convert.ToInt32(OrderStatusEnum.PickupDecline));



                if (orderStartDate != null && orderEndDate != null)
                {
                    orderStartDate = orderStartDate.Value.AddMinutes(-1);
                    orderEndDate = orderEndDate.Value.AddDays(1).AddSeconds(-1);
                    query = query.Where(x => x.OrderTrans.Exchange.ModifiedDate >= orderStartDate && x.OrderTrans.Exchange.ModifiedDate <= orderEndDate);
                }
                // Advanced Filters Mapping
                if (tblBusinessUnit != null)
                {
                    // Placeholder: Add filter for companyId
                    query = query.Where(x => x.OrderTrans.Exchange != null ? (x.OrderTrans.Exchange.BusinessPartner.BusinessUnit.BusinessUnitId == tblBusinessUnit.BusinessUnitId) : (x.OrderTrans.Abbredemption.Abbregistration.BusinessUnitId == tblBusinessUnit.BusinessUnitId));
                }

                if (productCatId.HasValue)
                {
                    // Placeholder: Add filter for productCatId
                    query = query.Where(x => x.OrderTrans.Exchange != null ? (x.OrderTrans.Exchange.ProductType.ProductCat.Id == productCatId) : (x.OrderTrans.Abbredemption.Abbregistration.NewProductCategoryId == productCatId));
                }

                if (productTypeId.HasValue)
                {
                    // Placeholder: Add filter for productTypeId
                    query = query.Where(x => x.OrderTrans.Exchange != null ? (x.OrderTrans.Exchange.ProductType.Id == productTypeId) : (x.OrderTrans.Abbredemption.Abbregistration.NewProductCategoryTypeId == productTypeId));
                }

                if (!string.IsNullOrEmpty(regdNo))
                {
                    // Placeholder: Add filter for regdNo
                    query = query.Where(x => x.OrderTrans.RegdNo == regdNo);
                }

                if (!string.IsNullOrEmpty(phoneNo))
                {
                    // Placeholder: Add filter for phoneNo
                    query = query.Where(x => x.OrderTrans.Exchange != null ? (x.OrderTrans.Exchange.CustomerDetails.PhoneNumber == phoneNo) : (x.OrderTrans.Abbredemption.CustomerDetails.PhoneNumber == phoneNo));
                }

                if (!string.IsNullOrEmpty(custCity))
                {
                    // Placeholder: Add filter for custCity
                    query = query.Where(x => x.OrderTrans.Exchange != null ? (x.OrderTrans.Exchange.CustomerDetails.City == custCity) : (x.OrderTrans.Abbredemption.CustomerDetails.City == custCity));
                }
                if (!string.IsNullOrEmpty(companyName))
                {
                    // Placeholder: Add filter for productTypeId
                    query = query.Where(x => x.OrderTrans.Exchange != null ? (x.OrderTrans.Exchange.BusinessPartner.BusinessUnit.Name == companyName) : (x.OrderTrans.Abbredemption.Abbregistration.BusinessUnit.Name == companyName));
                }

                if (createdbyme > 0)
                {
                    // Placeholder: Add filter for productTypeId
                    query = query.Where(x => x.OrderTrans.CreatedBy == createdbyme);
                }
                if (!string.IsNullOrEmpty(evcregdNo))
                {
                    // Placeholder: Add filter for regdNo
                    query = query.Where(x => x.Evcregistration.EvcregdNo == evcregdNo);
                }
                if (!string.IsNullOrEmpty(evcstoreCode))
                {
                    // Placeholder: Add filter for regdNo evcstoreCode
                    query = query.Where(x => x.Evcpartner.EvcStoreCode == evcstoreCode);
                }


                recordsTotal = await query.CountAsync();

                if (!string.IsNullOrEmpty(sortColumn))
                {
                    if (sortColumnDirection.Equals(SortingOrder.ASCENDING))
                    {
                        query = query.OrderBy(x => EF.Property<object>(x, sortColumn));
                    }
                    else
                    {
                        query = query.OrderByDescending(x => EF.Property<object>(x, sortColumn));
                    }
                }
                List<TblWalletTransaction> tblWalletTransactions = await query
    .OrderByDescending(x => x.ModifiedDate)
    .Skip(skip)
    .Take(pageSize)
    .ToListAsync();

                pickupDeclineViewModel = _mapper.Map<List<TblWalletTransaction>, List<PickupDeclineViewModel>>(tblWalletTransactions);

                foreach (PickupDeclineViewModel item in pickupDeclineViewModel)
                {
                    TblWalletTransaction orderDetails = tblWalletTransactions.FirstOrDefault(x => x.OrderTransId == item.OrderTransId);

                    if (orderDetails != null)
                    {

                        item.OrderTransId = orderDetails.OrderTransId;
                        item.EvcRate = orderDetails.OrderAmount > 0 ? orderDetails.OrderAmount : 0;
                        item.RegdNo = orderDetails.OrderTrans.RegdNo ?? string.Empty;
                        item.EvcRegNo = orderDetails.Evcregistration != null ? $"{orderDetails.Evcregistration.EvcregdNo}-{orderDetails.Evcregistration.BussinessName}" : string.Empty;
                        item.Date = orderDetails.OrderofAssignDate?.ToShortDateString() ?? string.Empty;
                        item.FinalExchangePrice = orderDetails.OrderTrans.FinalPriceAfterQc ?? null;
                        item.EvcStoreCode = orderDetails.Evcpartner?.EvcStoreCode ?? string.Empty;
                        if (orderDetails.OrderType != null && (orderDetails.OrderType == Convert.ToInt32(OrderTypeEnum.Exchange).ToString()))
                        {
                            item.ordertype = "Exchange";
                            item.ExchProdGroup = orderDetails.OrderTrans.Exchange.ProductType.Description ?? string.Empty;
                            item.OldProdType = orderDetails.OrderTrans.Exchange.ProductType.ProductCat.Description ?? string.Empty;
                            item.FirstName = orderDetails.OrderTrans.Exchange.CustomerDetails.FirstName + " " + orderDetails.OrderTrans.Exchange.CustomerDetails.LastName ?? string.Empty;
                            item.CustCity = orderDetails.OrderTrans.Exchange.CustomerDetails.City ?? string.Empty;
                            item.CustPin = orderDetails.OrderTrans.Exchange.CustomerDetails.ZipCode ?? string.Empty;
                        }
                        if (orderDetails.OrderType != null && (orderDetails.OrderType == Convert.ToInt32(OrderTypeEnum.ABB).ToString()))
                        {
                            item.ordertype = "ABB";
                            item.ExchProdGroup = orderDetails.OrderTrans.Abbredemption.Abbregistration.NewProductCategory.Description ?? string.Empty;
                            item.OldProdType = orderDetails.OrderTrans.Abbredemption.Abbregistration.NewProductCategoryTypeNavigation.Description ?? string.Empty;
                            item.FirstName = orderDetails.OrderTrans.Abbredemption.CustomerDetails.FirstName + " " + orderDetails.OrderTrans.Abbredemption.CustomerDetails.LastName ?? string.Empty;
                            item.CustCity = orderDetails.OrderTrans.Abbredemption.CustomerDetails.City ?? string.Empty;
                            item.CustPin = orderDetails.OrderTrans.Abbredemption.CustomerDetails.ZipCode ?? string.Empty;
                        }


                    }
                }

                pickupDeclineViewModel = pickupDeclineViewModel.OrderByDescending(x => x.ModifiedDate).ToList();

                var jsonData = new
                {
                    draw = draw,
                    recordsFiltered = recordsTotal,
                    recordsTotal = recordsTotal,
                    data = pickupDeclineViewModel
                };

                return Ok(jsonData);
            }
            catch (Exception ex)
            {
                throw;
            }


        }
        #endregion

        #region ADDED BY Priyanshi Sahu---- GET All EVC Dispute Order use for EVC_Portal //updated Iqueryable // Modified by VK For ABB
        public IActionResult GetDisputeOrderList(int LoggingId, int TableNo)
        {
            #region Variable Declaration
            string URL = _config.Value.URLPrefixforProd;
            IQueryable<TblEvcdispute> disputeQuery = null;
            List<TblEvcdispute> TblEvcdisputes = null;
            TblEvcregistration TblEvcregistrations = null;
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


                #region tbl Initialization
                TblEvcregistrations = _context.TblEvcregistrations
                    .FirstOrDefault(x => x.IsActive == true && x.UserId == LoggingId);

                if (TblEvcregistrations != null)
                {
                    disputeQuery = _context.TblEvcdisputes.AsNoTracking()
                        .Include(x => x.Evcregistration)
                        .Include(x => x.OrderTrans).ThenInclude(x => x.Exchange).ThenInclude(x => x.ProductType).ThenInclude(x => x.ProductCat)
                        .Include(x => x.OrderTrans).ThenInclude(x => x.Abbredemption).ThenInclude(x => x.Abbregistration).ThenInclude(x => x.NewProductCategory)
                        .Include(x => x.OrderTrans).ThenInclude(x => x.Abbredemption).ThenInclude(x => x.Abbregistration).ThenInclude(x => x.NewProductCategoryTypeNavigation)
                        .Where(x => x.IsActive == true && x.DisputeRegno != null && x.EvcregistrationId == TblEvcregistrations.EvcregistrationId);

                    if (TableNo == 1)
                    {
                        disputeQuery = disputeQuery.Where(x => x.Status == "1" && x.Digi2Lresponse == null);
                    }
                    else if (TableNo == 2)
                    {
                        disputeQuery = disputeQuery.Where(x => x.Status == "2" && x.Digi2Lresponse != null);
                    }
                    else if (TableNo == 3)
                    {
                        disputeQuery = disputeQuery.Where(x => x.Status == "3" && x.Digi2Lresponse != null);
                    }
                    else if (TableNo == 4)
                    {
                        disputeQuery = disputeQuery.Where(x => x.Status == "4" && x.Digi2Lresponse != null);
                    }

                    if (!string.IsNullOrEmpty(searchValue))
                    {
                        string searchLower = searchValue.ToLower().Trim();
                        disputeQuery = disputeQuery.Where(x =>
                            x.DisputeRegno.ToLower().Contains(searchLower) ||

                            x.OrderTrans.RegdNo.ToLower().Contains(searchLower) ||
                            x.Evcregistration.EvcregdNo.ToLower().Contains(searchLower) ||
                            x.Evcregistration.EvcmobileNumber.ToLower().Contains(searchLower) ||
                            x.Status.ToLower().Contains(searchLower) ||
                           (x.OrderTrans != null && (x.OrderTrans.RegdNo ?? "").ToLower().Contains(searchLower))
                        );
                    }

                    recordsTotal = disputeQuery.Count();

                    /*if (!string.IsNullOrEmpty(sortColumn))
                    {
                        bool isAscending = sortColumnDirection.Equals(SortingOrder.ASCENDING);
                        disputeQuery = ApplySorting(disputeQuery, sortColumn, isAscending);
                    }*/

                    if (disputeQuery != null && recordsTotal > 0)
                    {
                        TblEvcdisputes = disputeQuery.OrderByDescending(x => ((x.ModifiedDate == null || x.ModifiedDate != null) ? x.CreatedDate : x.ModifiedDate)).Skip(skip).Take(pageSize).ToList();
                    }
                }
                #endregion

                #region tbl to model mapping
                List<ShowDisputeListViewModel> ShowDisputeListViewModels = _mapper.Map<List<TblEvcdispute>, List<ShowDisputeListViewModel>>(TblEvcdisputes);

                string actionURL = string.Empty;
                string actionURL1 = string.Empty;

                foreach (ShowDisputeListViewModel item in ShowDisputeListViewModels)
                {
                    actionURL = " <div class='actionbtns'>";
                    actionURL = actionURL + "<a href='" + URL + "/EVC_Portal/OrderdetailsViewPageForEVC_PortAL?OrderTransId=" + (item.orderTransId) + "' ><button onclick='View(" + item.orderTransId + ")' class='btn btn-primary btn'>View</button></a>";
                    actionURL = actionURL + "</div>";
                    item.Action = actionURL;

                    //Take Data is More Tbl                   
                    TblEvcdispute EVC_DisputeReg = TblEvcdisputes.FirstOrDefault(x => x.EvcdisputeId == item.EvcdisputeId);
                    item.DisputeRegno = EVC_DisputeReg.DisputeRegno;
                    if (EVC_DisputeReg != null)
                    {
                        if (EVC_DisputeReg.EvcregistrationId > 0)
                        {
                            item.EvcRegNo = EVC_DisputeReg.EvcregistrationId != null ? EVC_DisputeReg.Evcregistration.EvcregdNo : String.Empty;
                        }
                        if (EVC_DisputeReg.OrderTransId > 0 && EVC_DisputeReg.OrderTransId != null)
                        {
                            item.RegdNo = EVC_DisputeReg.OrderTrans.RegdNo;
                        }
                        if (EVC_DisputeReg.ProductTypeId > 0)
                        {

                            TblProductType tblProductType = _context.TblProductTypes.FirstOrDefault(x => x.Id == item.ProductTypeId || x.Id == item.NewProductCategoryTypeId);
                            item.ProductTypeName = tblProductType != null ? tblProductType.Description : string.Empty;
                            if (tblProductType.ProductCatId != null)
                            {
                                TblProductCategory tblProductCategory = _context.TblProductCategories.FirstOrDefault(x => x.Id == tblProductType.ProductCatId);
                                item.ProductCatName = tblProductCategory != null ? tblProductCategory.Description : string.Empty;
                            }
                        }
                        if (EVC_DisputeReg.Status != null)
                        {
                            item.Status = GetDisputeStatusDescription(EVC_DisputeReg.Status);
                        }
                    }

                    if (TableNo == 1 && EVC_DisputeReg != null && EVC_DisputeReg.CreatedDate != null)
                    {
                        var Date = (DateTime)item.CreatedDate;
                        item.Date = Date.ToShortDateString();
                    }
                    else if (TableNo != 1 && EVC_DisputeReg != null && EVC_DisputeReg.ModifiedDate != null)
                    {
                        var Date = (DateTime)item.ModifiedDate;
                        item.Date = Date.ToShortDateString();
                    }
                }
                #endregion

                var data = ShowDisputeListViewModels;
                var jsonData = new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data };
                return Ok(jsonData);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        private string GetDisputeStatusDescription(string status)
        {
            switch (status)
            {
                case "1":
                    return EnumHelper.DescriptionAttr(EVCDisputeStatusEnum.Open);
                case "2":
                    return EnumHelper.DescriptionAttr(EVCDisputeStatusEnum.WorkInProgress);
                case "3":
                    return EnumHelper.DescriptionAttr(EVCDisputeStatusEnum.Hold);
                case "4":
                    return EnumHelper.DescriptionAttr(EVCDisputeStatusEnum.Close);
                default:
                    return string.Empty;
            }
        }
        private IQueryable<TblEvcdispute> ApplySorting(IQueryable<TblEvcdispute> query, string sortColumn, bool isAscending)
        {
            var property = typeof(TblEvcdispute).GetProperty(sortColumn);
            if (property != null)
            {
                if (isAscending)
                {
                    query = query.OrderBy(x => property.GetValue(x, null));
                }
                else
                {
                    query = query.OrderByDescending(x => property.GetValue(x, null));
                }
            }
            return query;
        }

        #endregion

        #region ADDED BY Priyanshi Sahu ----GET All EVC Dispute Order use for EVC_Admin //updated Iqueryable // Modified by VK For ABB
        public IActionResult GetDisputeOrderListforAdmin(int TableNo, DateTime? startDate, DateTime? endDate)
        {
            #region Variable Declaration
            string URL = _config.Value.URLPrefixforProd;
            List<TblEvcdispute> TblEvcdisputes = new List<TblEvcdispute>();
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

                #region tbl initialization
                IQueryable<TblEvcdispute> disputeQuery = _context.TblEvcdisputes
                    .Include(x => x.Evcregistration)
                    .Include(x => x.OrderTrans).ThenInclude(x => x.Exchange).ThenInclude(x => x.ProductType)
                    .ThenInclude(x => x.ProductCat)
                    .Include(x => x.OrderTrans).ThenInclude(x => x.Abbredemption).ThenInclude(x => x.Abbregistration)
                    .Where(x => x.IsActive == true && x.DisputeRegno != null);

                if (TableNo == 1)
                {
                    disputeQuery = disputeQuery.Where(x => x.Status == "1" && x.Digi2Lresponse == null);
                }
                else if (TableNo == 2)
                {
                    disputeQuery = disputeQuery.Where(x => x.Status == "2" && x.Digi2Lresponse != null);
                }
                else if (TableNo == 3)
                {
                    disputeQuery = disputeQuery.Where(x => x.Status == "3" && x.Digi2Lresponse != null);
                }
                else if (TableNo == 4)
                {
                    disputeQuery = disputeQuery.Where(x => x.Status == "4" && x.Digi2Lresponse != null);
                }
                if (startDate != null && endDate != null)
                {
                    startDate = startDate.Value.AddMinutes(-1);
                    endDate = endDate.Value.AddDays(1).AddSeconds(-1);
                    disputeQuery = disputeQuery.Where(x => x.CreatedDate >= startDate && x.CreatedDate <= endDate);
                }
                if (!string.IsNullOrEmpty(searchValue))
                {
                    string searchLower = searchValue.ToLower().Trim();
                    disputeQuery = disputeQuery.Where(x =>
                        (x.DisputeRegno ?? "").ToLower().Contains(searchLower) ||
                        (x.OrderTrans.RegdNo ?? "").ToLower().Contains(searchLower) ||
                        (x.Evcregistration.EvcregdNo ?? "").ToLower().Contains(searchLower) ||
                        (x.Evcregistration.EvcmobileNumber ?? "").ToLower().Contains(searchLower) ||
                        (x.Status ?? "").ToLower().Contains(searchLower)
                    );
                }
                recordsTotal = disputeQuery.Count();

                /*if (!string.IsNullOrEmpty(sortColumn))
                {
                    bool isAscending = sortColumnDirection.Equals(SortingOrder.ASCENDING);
                    disputeQuery = ApplySorting(disputeQuery, sortColumn, isAscending);
                }*/

                TblEvcdisputes = disputeQuery.OrderByDescending(x => ((x.ModifiedDate == null || x.ModifiedDate != null) ? x.CreatedDate : x.ModifiedDate)).Skip(skip).Take(pageSize).ToList();
                #endregion

                #region tbl to model mapping
                List<ShowDisputeListViewModel> ShowDisputeListViewModels = _mapper.Map<List<TblEvcdispute>, List<ShowDisputeListViewModel>>(TblEvcdisputes);

                string actionURL = string.Empty;
                string actionURL1 = string.Empty;

                foreach (ShowDisputeListViewModel item in ShowDisputeListViewModels)
                {
                    actionURL = " <div class='actionbtns'>";
                    actionURL = actionURL + "<a href='" + URL + "/EVC_Portal/OrderdetailsViewPageForEVC_PortAL?OrderTransId=" + (item.orderTransId) + "' ><button onclick='View(" + item.orderTransId + ")' class='btn btn-primary btn'>View</button></a>";
                    actionURL = actionURL + "<a class='mx-1 fas btn  btn-primary' href='" + URL + "/EVC_Admin_Dispute/EVC_Admin_DisputeForm?id=" + _protector.Encode(item.EvcdisputeId) + "' data-bs-toggle='Tooltip' data-bs-placement='top' title='Reply' ><i class='fa-solid fa-reply'></i></a>";
                    actionURL = actionURL + "<a href='" + URL + "/EVC_Admin_Dispute/DisputeViewPage?OrderTransId=" + (item.orderTransId) + "' ><button onclick='View(" + item.orderTransId + ")' class='btn btn-primary btn' data-bs-toggle='tooltip' data-bs-placement='top' title='View'><i class='fa-solid fa-eye'></i></button></a>";
                    actionURL = actionURL + " <a class='btn-sm btn-primary ' href='" + URL + "/Index1?orderTransId=" + item.orderTransId + "' title='TimeLine'><i class='fa solid fa-clock-rotate-left'></i></a> &nbsp;";
                    actionURL = actionURL + "</div>";
                    item.Action = actionURL;

                    //Take Data is More Tbl                   
                    TblEvcdispute EVC_DisputeReg = TblEvcdisputes.FirstOrDefault(x => x.EvcdisputeId == item.EvcdisputeId);
                    item.DisputeRegno = EVC_DisputeReg.DisputeRegno;
                    if (EVC_DisputeReg != null)
                    {
                        if (EVC_DisputeReg.EvcregistrationId > 0)
                        {
                            item.EvcRegNo = EVC_DisputeReg.EvcregistrationId != null ? EVC_DisputeReg.Evcregistration.EvcregdNo : String.Empty;
                        }
                        if (EVC_DisputeReg.OrderTransId > 0 && EVC_DisputeReg.OrderTransId != null)
                        {
                            item.RegdNo = EVC_DisputeReg.OrderTrans.RegdNo;
                        }
                        if (EVC_DisputeReg.ProductTypeId > 0)
                        {
                            TblProductType tblProductType = _context.TblProductTypes.FirstOrDefault(x => x.Id == item.ProductTypeId || x.Id == item.NewProductCategoryTypeId);
                            item.ProductTypeName = tblProductType != null ? tblProductType.Description : string.Empty;
                            if (tblProductType.ProductCatId != null)
                            {
                                TblProductCategory tblProductCategory = _context.TblProductCategories.FirstOrDefault(x => x.Id == tblProductType.ProductCatId);
                                item.ProductCatName = tblProductCategory != null ? tblProductCategory.Description : string.Empty;
                            }
                        }
                        if (EVC_DisputeReg.Status != null)
                        {
                            item.Status = GetDisputeStatusDescription(EVC_DisputeReg.Status);
                        }
                    }

                    if (TableNo == 1 && EVC_DisputeReg != null && EVC_DisputeReg.CreatedDate != null)
                    {
                        var Date = (DateTime)item.CreatedDate;
                        item.Date = Date.ToShortDateString();
                    }
                    else if (TableNo != 1 && EVC_DisputeReg != null && EVC_DisputeReg.ModifiedDate != null)
                    {
                        var Date = (DateTime)item.ModifiedDate;
                        item.Date = Date.ToShortDateString();
                    }
                }
                #endregion

                var data = ShowDisputeListViewModels;
                var jsonData = new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data };
                return Ok(jsonData);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        #endregion

        #region ADDED BY Priyanshi Sahu ---- Get All Order Record Histroy List For EVC_Portal
        /// <summary>
        /// EVCUser_portalGETAllOcate Records List
        /// </summary>
        /// <returns></returns>
        /// 

        public IActionResult EVCUser_GetAllAllocateRecords(int userid, int TableNo, int companyId, DateTime? orderStartDate, DateTime? orderEndDate, string? companyName, int? productCatId, int? productTypeId, string? regdNo, string? evcstoreCode, string? phoneNo)
        {
            try
            {
                #region Variable Declaration
                List<TblOrderTran> TblOrderTrans = null;
                List<TblWalletTransaction> tblWalletTransactions = null;
                IQueryable<TblWalletTransaction> tblWalletTransQueryable = null;
                List<TblWalletTransaction> tblWalletTransactionforlists = null;
                TblCompany tblCompany = null;
                TblBusinessUnit tblBusinessUnit = null;
                TblEvcregistration TblEvcregistrations = null;
                //int count = 0;

                string URL = _config.Value.URLPrefixforProd;
                #region Trim string values from parameters
                if (!string.IsNullOrWhiteSpace(companyName) && companyName != "null")
                { companyName = companyName.Trim().ToLower(); }
                else { companyName = null; }
                if (!string.IsNullOrWhiteSpace(regdNo) && regdNo != "null")
                { regdNo = regdNo.Trim().ToLower(); }
                else { regdNo = null; }
                //Added EVC store code filter by Pooja Jatav
                if (!string.IsNullOrWhiteSpace(evcstoreCode) && evcstoreCode != "null")
                { evcstoreCode = evcstoreCode.Trim().ToLower(); }
                else { evcstoreCode = null; }
                if (!string.IsNullOrWhiteSpace(phoneNo) && phoneNo != "null")
                { phoneNo = phoneNo.Trim().ToLower(); }
                else { phoneNo = null; }


                #endregion
                #endregion

                #region Datatable variables
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

                TblEvcregistrations = _context.TblEvcregistrations.Where(x => x.IsActive == true && x.Isevcapprovrd == true && x.UserId == userid).FirstOrDefault();
                if (TableNo == 1)
                {
                    tblWalletTransQueryable = _context.TblWalletTransactions
                   .Include(x => x.Evcregistration)
                   .Include(x => x.Evcpartner)
                   .Include(x => x.OrderTrans).ThenInclude(x => x.Exchange).ThenInclude(x => x.Status)
                   .Include(x => x.OrderTrans).ThenInclude(x => x.Exchange).ThenInclude(x => x.ProductType).ThenInclude(x => x.ProductCat)
                   .Include(x => x.OrderTrans).ThenInclude(x => x.Exchange).ThenInclude(x => x.CustomerDetails)
                  //Changes for ABB Redemption by Vk 
                  .Include(x => x.OrderTrans).ThenInclude(x => x.Abbredemption).ThenInclude(x => x.Status)
                  .Include(x => x.OrderTrans).ThenInclude(x => x.Abbredemption).ThenInclude(x => x.Abbregistration).ThenInclude(x => x.NewProductCategory)
                  .Include(x => x.OrderTrans).ThenInclude(x => x.Abbredemption).ThenInclude(x => x.Abbregistration).ThenInclude(x => x.NewProductCategoryTypeNavigation)
                  .Include(x => x.OrderTrans).ThenInclude(x => x.Abbredemption).ThenInclude(x => x.CustomerDetails)
                    //Changes for ABB Redemption by Vk
                    .Where(x => x.IsActive == true && x.EvcregistrationId == TblEvcregistrations.EvcregistrationId && x.OrderofAssignDate != null && x.OrderOfInprogressDate == null && x.OrderOfDeliverdDate == null && x.OrderOfCompleteDate == null && x.StatusId == 34.ToString());


                }
                else if (TableNo == 2)
                {
                    tblWalletTransQueryable = _context.TblWalletTransactions
                   .Include(x => x.Evcregistration)
                   .Include(x => x.Evcpartner)
                   .Include(x => x.OrderTrans).ThenInclude(x => x.Exchange).ThenInclude(x => x.Status)
                   .Include(x => x.OrderTrans).ThenInclude(x => x.Exchange).ThenInclude(x => x.ProductType).ThenInclude(x => x.ProductCat)
                   .Include(x => x.OrderTrans).ThenInclude(x => x.Exchange).ThenInclude(x => x.CustomerDetails)
                  //Changes for ABB Redemption by Vk 
                  .Include(x => x.OrderTrans).ThenInclude(x => x.Abbredemption).ThenInclude(x => x.Status)
                  .Include(x => x.OrderTrans).ThenInclude(x => x.Abbredemption).ThenInclude(x => x.Abbregistration).ThenInclude(x => x.NewProductCategory)
                  .Include(x => x.OrderTrans).ThenInclude(x => x.Abbredemption).ThenInclude(x => x.Abbregistration).ThenInclude(x => x.NewProductCategoryTypeNavigation)
                  .Include(x => x.OrderTrans).ThenInclude(x => x.Abbredemption).ThenInclude(x => x.CustomerDetails)
                    //Changes for ABB Redemption by Vk
                    .Where(x => x.IsActive == true && x.EvcregistrationId == TblEvcregistrations.EvcregistrationId && x.OrderofAssignDate != null && x.OrderOfInprogressDate != null && x.OrderOfDeliverdDate == null && x.OrderOfCompleteDate == null && (x.StatusId == 18.ToString() || x.StatusId == 23.ToString() || x.StatusId == 22.ToString()));

                }
                else if (TableNo == 3)
                {
                    tblWalletTransQueryable = _context.TblWalletTransactions
                   .Include(x => x.Evcregistration)
                   .Include(x => x.Evcpartner)
                   .Include(x => x.OrderTrans).ThenInclude(x => x.Exchange).ThenInclude(x => x.Status)
                   .Include(x => x.OrderTrans).ThenInclude(x => x.Exchange).ThenInclude(x => x.ProductType).ThenInclude(x => x.ProductCat)
                   .Include(x => x.OrderTrans).ThenInclude(x => x.Exchange).ThenInclude(x => x.CustomerDetails)
                  //Changes for ABB Redemption by Vk 
                  .Include(x => x.OrderTrans).ThenInclude(x => x.Abbredemption).ThenInclude(x => x.Status)
                  .Include(x => x.OrderTrans).ThenInclude(x => x.Abbredemption).ThenInclude(x => x.Abbregistration).ThenInclude(x => x.NewProductCategory)
                  .Include(x => x.OrderTrans).ThenInclude(x => x.Abbredemption).ThenInclude(x => x.Abbregistration).ThenInclude(x => x.NewProductCategoryTypeNavigation)
                  .Include(x => x.OrderTrans).ThenInclude(x => x.Abbredemption).ThenInclude(x => x.CustomerDetails)
                    //Changes for ABB Redemption by Vk
                    .Where(x => x.IsActive == true && x.EvcregistrationId == TblEvcregistrations.EvcregistrationId && x.OrderofAssignDate != null && x.OrderOfInprogressDate != null && x.OrderOfDeliverdDate != null && x.OrderOfCompleteDate == null && x.StatusId == 30.ToString());
                }
                else if (TableNo == 4)
                {
                    tblWalletTransQueryable = _context.TblWalletTransactions
                   .Include(x => x.Evcregistration)
                   .Include(x => x.Evcpartner)
                   .Include(x => x.OrderTrans).ThenInclude(x => x.Exchange).ThenInclude(x => x.Status)
                   .Include(x => x.OrderTrans).ThenInclude(x => x.Exchange).ThenInclude(x => x.ProductType).ThenInclude(x => x.ProductCat)
                   .Include(x => x.OrderTrans).ThenInclude(x => x.Exchange).ThenInclude(x => x.CustomerDetails)
                  //Changes for ABB Redemption by Vk 
                  .Include(x => x.OrderTrans).ThenInclude(x => x.Abbredemption).ThenInclude(x => x.Status)
                  .Include(x => x.OrderTrans).ThenInclude(x => x.Abbredemption).ThenInclude(x => x.Abbregistration).ThenInclude(x => x.NewProductCategory)
                  .Include(x => x.OrderTrans).ThenInclude(x => x.Abbredemption).ThenInclude(x => x.Abbregistration).ThenInclude(x => x.NewProductCategoryTypeNavigation)
                  .Include(x => x.OrderTrans).ThenInclude(x => x.Abbredemption).ThenInclude(x => x.CustomerDetails)
                    //Changes for ABB Redemption by Vk
                    .Where(x => x.IsActive == true && x.EvcregistrationId == TblEvcregistrations.EvcregistrationId && x.OrderofAssignDate != null && x.OrderOfInprogressDate != null && x.OrderOfDeliverdDate != null && x.OrderOfCompleteDate != null && x.StatusId == 32.ToString());
                }
                else if (TableNo == 5)
                {
                    tblWalletTransQueryable = _context.TblWalletTransactions
                   .Include(x => x.Evcregistration)
                   .Include(x => x.Evcpartner)
                   .Include(x => x.OrderTrans).ThenInclude(x => x.Exchange).ThenInclude(x => x.Status)
                   .Include(x => x.OrderTrans).ThenInclude(x => x.Exchange).ThenInclude(x => x.ProductType).ThenInclude(x => x.ProductCat)
                   .Include(x => x.OrderTrans).ThenInclude(x => x.Exchange).ThenInclude(x => x.CustomerDetails)
                  //Changes for ABB Redemption by Vk 
                  .Include(x => x.OrderTrans).ThenInclude(x => x.Abbredemption).ThenInclude(x => x.Status)
                  .Include(x => x.OrderTrans).ThenInclude(x => x.Abbredemption).ThenInclude(x => x.Abbregistration).ThenInclude(x => x.NewProductCategory)
                  .Include(x => x.OrderTrans).ThenInclude(x => x.Abbredemption).ThenInclude(x => x.Abbregistration).ThenInclude(x => x.NewProductCategoryTypeNavigation)
                  .Include(x => x.OrderTrans).ThenInclude(x => x.Abbredemption).ThenInclude(x => x.CustomerDetails)
                     //Changes for ABB Redemption by Vk
                     .Where(x => x.IsActive == true && x.EvcregistrationId == TblEvcregistrations.EvcregistrationId && x.StatusId == 26.ToString());
                }
                else if (TableNo == 6)
                {
                    tblWalletTransQueryable = _context.TblWalletTransactions
                   .Include(x => x.Evcregistration)
                   .Include(x => x.Evcpartner)
                   .Include(x => x.OrderTrans).ThenInclude(x => x.Exchange).ThenInclude(x => x.Status)
                   .Include(x => x.OrderTrans).ThenInclude(x => x.Exchange).ThenInclude(x => x.ProductType).ThenInclude(x => x.ProductCat)
                   .Include(x => x.OrderTrans).ThenInclude(x => x.Exchange).ThenInclude(x => x.CustomerDetails)
                  //Changes for ABB Redemption by Vk 
                  .Include(x => x.OrderTrans).ThenInclude(x => x.Abbredemption).ThenInclude(x => x.Status)
                  .Include(x => x.OrderTrans).ThenInclude(x => x.Abbredemption).ThenInclude(x => x.Abbregistration).ThenInclude(x => x.NewProductCategory)
                  .Include(x => x.OrderTrans).ThenInclude(x => x.Abbredemption).ThenInclude(x => x.Abbregistration).ThenInclude(x => x.NewProductCategoryTypeNavigation)
                  .Include(x => x.OrderTrans).ThenInclude(x => x.Abbredemption).ThenInclude(x => x.CustomerDetails)
                    //Changes for ABB Redemption by Vk
                    .Where(x => x.IsActive == true && x.EvcregistrationId == TblEvcregistrations.EvcregistrationId && x.OrderofAssignDate != null && x.OrderOfInprogressDate != null && x.OrderOfDeliverdDate == null && x.OrderOfCompleteDate == null && x.StatusId == 44.ToString());
                }

                #region Advanced Filters
                tblWalletTransQueryable = tblWalletTransQueryable.Where(x =>
                   (string.IsNullOrEmpty(regdNo) || (x.RegdNo ?? "").ToLower() == regdNo)
     && (x.OrderTrans != null
     && (string.IsNullOrEmpty(evcstoreCode) || (x.Evcpartner.EvcStoreCode ?? "").ToLower() == evcstoreCode)
     && ((x.OrderTrans.Exchange != null
    && (x.OrderTrans.Exchange.ProductType != null && x.OrderTrans.Exchange.ProductType.ProductCat != null)
    && (productCatId == null || x.OrderTrans.Exchange.ProductType.ProductCatId == productCatId)
    && (productTypeId == null || x.OrderTrans.Exchange.ProductTypeId == productTypeId)
    && (string.IsNullOrEmpty(phoneNo) || (x.Evcregistration != null && x.Evcregistration.EvcmobileNumber == phoneNo))

    && (string.IsNullOrEmpty(companyName) || (x.OrderTrans.Exchange.CompanyName ?? "").ToLower() == companyName)
                   )
                   || (x.OrderTrans.Abbredemption != null && x.OrderTrans.Abbredemption.Abbregistration != null
    && (x.OrderTrans.Abbredemption.Abbregistration.NewProductCategoryTypeNavigation != null && x.OrderTrans.Abbredemption.Abbregistration.NewProductCategory != null)
    && (productCatId == null || x.OrderTrans.Abbredemption.Abbregistration.NewProductCategoryId == productCatId)
    && (productTypeId == null || x.OrderTrans.Abbredemption.Abbregistration.NewProductCategoryTypeId == productTypeId)
    && (string.IsNullOrEmpty(phoneNo) || (x.OrderTrans.Abbredemption.CustomerDetails != null && x.OrderTrans.Abbredemption.CustomerDetails.PhoneNumber == phoneNo))

    && (string.IsNullOrEmpty(companyName) || (x.OrderTrans.Abbredemption.Abbregistration.BusinessUnitId != null && (x.OrderTrans.Abbredemption.Abbregistration.BusinessUnit.Name ?? "").ToLower() == companyName))
                   ))));
                #endregion

                #region Sorting and Pagination Modified by Vk
                //Code for Pagination
                recordsTotal = tblWalletTransQueryable != null ? tblWalletTransQueryable.Count() : 0;
                tblWalletTransactions = tblWalletTransQueryable
                    .OrderByDescending(x => x.ModifiedDate).ThenByDescending(x => x.OrderTransId)
                    .Skip(skip).Take(pageSize).ToList();
                if (tblWalletTransactions != null)
                {
                    tblWalletTransactions = sortColumnDirection.Equals(SortingOrder.ASCENDING)
    ? tblWalletTransactions.OrderBy(o => o != null && o.GetType().GetProperty(sortColumn) != null ? o.GetType().GetProperty(sortColumn).GetValue(o, null) : null)
                          .ToList()
    : tblWalletTransactions.OrderByDescending(o => o != null && o.GetType().GetProperty(sortColumn) != null ? o.GetType().GetProperty(sortColumn).GetValue(o, null) : null)
                          .ToList();
                }
                else
                {
                    tblWalletTransactions = new List<TblWalletTransaction>();
                }
                #endregion


                #region TblWalletTransaction to model mapping Modified by Vk
                List<EVCUser_AllOrderRecordViewModel> EVCUser_AllOrderRecordViewModels = _mapper.Map<List<TblWalletTransaction>, List<EVCUser_AllOrderRecordViewModel>>(tblWalletTransactions);

                #endregion
                string actionURL = string.Empty;


                foreach (EVCUser_AllOrderRecordViewModel item in EVCUser_AllOrderRecordViewModels)
                {
                    actionURL = " <td class='actions'>";
                    actionURL = actionURL + "<a href='" + URL + "/EVC_Portal/OrderdetailsViewPageForEVC_PortAL?OrderTransId=" + (item.orderTransId) + "' ><button onclick='View(" + item.orderTransId + ")' class='btn btn-primary btn'>View</button></a>";
                    // actionURL = actionURL + " <li><a class='mx-1 fas fa-edit' href='" + URL + "/EVC_Portal/OrderdetailsViewPageForEVC_PortAL?OrderTransId=" + (item.orderTransId) + " class='btn btn-primary btn' title='Edit' ></a></li>";
                    actionURL = actionURL + " </td>";
                    item.Action = actionURL;

                    TblWalletTransaction OrdeDetails = tblWalletTransactions.FirstOrDefault(x => x.OrderTransId == item.orderTransId);
                    item.orderTransId = (int)OrdeDetails.OrderTransId;

                    string regdNoVar = null; string productTypeDesc = null; string productCatDesc = null; string statusCode = null;
                    string custFirstName = null; string custCityVar = null; string custPincode = null; decimal? finalPrice = null;


                    if (OrdeDetails != null)
                    {
                        TblLogistic tblLogistic = _context.TblLogistics.Where(x => x.OrderTransId == OrdeDetails.OrderTransId && x.IsActive == true).FirstOrDefault();
                        if (tblLogistic != null)
                        {
                            if (tblLogistic.TicketNumber != null)
                            {
                                item.TicketNumber = tblLogistic != null ? tblLogistic.TicketNumber : string.Empty;
                            }

                        }

                        TblOrderQc tblOrderQc = _context.TblOrderQcs.Where(x => x.OrderTransId == item.orderTransId).FirstOrDefault();
                        if (tblOrderQc != null)
                        {
                            if (tblOrderQc.QualityAfterQc != null)
                            {
                                item.ActualProdQlty = tblOrderQc != null ? tblOrderQc.QualityAfterQc : string.Empty;
                            }
                        }
                        if (TableNo == 1)
                        {
                            var Date = (DateTime)item.OrderofAssignDate;
                            item.Date = Date.ToShortDateString();
                        }
                        else if (TableNo == 2)
                        {
                            var Date = (DateTime)item.OrderOfInprogressDate;
                            item.Date = Date.ToShortDateString();

                        }
                        else if (TableNo == 3)
                        {
                            var Date = (DateTime)item.OrderOfDeliverdDate;
                            item.Date = Date.ToShortDateString();

                        }
                        else if (TableNo == 4)
                        {
                            var Date = (DateTime)item.OrderOfCompleteDate;
                            item.Date = Date.ToShortDateString();

                        }
                        else if (TableNo == 5)
                        {
                            var Date = (DateTime)item.ModifiedDate;
                            item.Date = Date.ToShortDateString();

                        }
                        else if (TableNo == 6)
                        {
                            var Date = (DateTime)item.ModifiedDate;
                            item.Date = Date.ToShortDateString();

                        }
                        if (OrdeDetails.OrderTrans.Exchange != null)
                        {
                            regdNoVar = OrdeDetails.OrderTrans.Exchange.RegdNo;
                            finalPrice = OrdeDetails.OrderTrans.Exchange.FinalExchangePrice;
                            if (OrdeDetails.OrderTrans.Exchange.ProductType != null)
                            {
                                productTypeDesc = OrdeDetails.OrderTrans.Exchange.ProductType.Description;
                                if (OrdeDetails.OrderTrans.Exchange.ProductType.ProductCat != null)
                                {
                                    productCatDesc = OrdeDetails.OrderTrans.Exchange.ProductType.ProductCat.Description;
                                }
                            }
                            if (OrdeDetails.OrderTrans.Exchange.Status != null)
                            {
                                statusCode = OrdeDetails.OrderTrans.Exchange.Status.StatusCode;
                            }
                            if (OrdeDetails.OrderTrans.Exchange.CustomerDetails != null)
                            {
                                custFirstName = OrdeDetails.OrderTrans.Exchange.CustomerDetails.FirstName;
                                custCityVar = OrdeDetails.OrderTrans.Exchange.CustomerDetails.City;
                                custPincode = OrdeDetails.OrderTrans.Exchange.CustomerDetails.ZipCode;
                            }

                        }
                        else if (OrdeDetails.OrderTrans.Abbredemption != null && OrdeDetails.OrderTrans.Abbredemption.Abbregistration != null)
                        {
                            regdNoVar = OrdeDetails?.OrderTrans?.RegdNo;
                            finalPrice = OrdeDetails.OrderTrans.FinalPriceAfterQc;
                            if (OrdeDetails.OrderTrans.Abbredemption.Abbregistration.NewProductCategoryTypeNavigation != null)
                            {
                                productTypeDesc = OrdeDetails?.OrderTrans?.Abbredemption?.Abbregistration?.NewProductCategoryTypeNavigation?.Description;
                            }
                            if (OrdeDetails.OrderTrans.Abbredemption.Abbregistration.NewProductCategory != null)
                            {
                                productCatDesc = OrdeDetails.OrderTrans.Abbredemption.Abbregistration.NewProductCategory.Description;
                            }
                            if (OrdeDetails.OrderTrans.Status != null)
                            {
                                statusCode = OrdeDetails.OrderTrans.Status.StatusCode;
                            }
                            if (OrdeDetails.OrderTrans.Abbredemption.CustomerDetails != null)
                            {
                                custFirstName = OrdeDetails.OrderTrans.Abbredemption.CustomerDetails.FirstName;
                                custCityVar = OrdeDetails.OrderTrans.Abbredemption.CustomerDetails.City;
                                custPincode = OrdeDetails.OrderTrans.Abbredemption.CustomerDetails.ZipCode;
                            }
                        }

                        item.OrderAmount = OrdeDetails.OrderAmount > 0 ? OrdeDetails.OrderAmount : 0;
                        item.RegdNo = regdNoVar;
                        item.EvcStoreCode = OrdeDetails?.Evcpartner?.EvcStoreCode;
                        item.ProductTypeName = productTypeDesc;
                        item.ProductCatName = productCatDesc;
                        item.Status = statusCode;
                    }

                }
                if (TableNo == 1)
                {
                    EVCUser_AllOrderRecordViewModels = EVCUser_AllOrderRecordViewModels.OrderByDescending(x => x.OrderofAssignDate).ToList();

                }
                else if (TableNo == 2)
                {
                    EVCUser_AllOrderRecordViewModels = EVCUser_AllOrderRecordViewModels.OrderByDescending(x => x.OrderOfInprogressDate).ToList();
                }
                else if (TableNo == 3)
                {
                    EVCUser_AllOrderRecordViewModels = EVCUser_AllOrderRecordViewModels.OrderByDescending(x => x.OrderOfDeliverdDate).ToList();
                }
                else if (TableNo == 4)
                {
                    EVCUser_AllOrderRecordViewModels = EVCUser_AllOrderRecordViewModels.OrderByDescending(x => x.OrderOfCompleteDate).ToList();
                }
                else if (TableNo == 5)
                {
                    EVCUser_AllOrderRecordViewModels = EVCUser_AllOrderRecordViewModels.OrderByDescending(x => x.ModifiedDate).ToList();
                }
                else if (TableNo == 6)
                {
                    EVCUser_AllOrderRecordViewModels = EVCUser_AllOrderRecordViewModels.OrderByDescending(x => x.ModifiedDate).ToList();
                }
                var data = EVCUser_AllOrderRecordViewModels;
                //var data = assignOrderViewModels;
                var jsonData = new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data };
                return Ok(jsonData);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        #endregion

        #region ADDED BY ASHWIN----- FOR SHOWING ALL RECORD OF WALLET RECHARGE TO LOGIN EVC USER      
        public async Task<ActionResult> EVC_All_WalletAdditionHistory(int LoggingId)
        {
            List<MyWalletSummaryAdditionViewModel> myWalletSummaryAddition = new List<MyWalletSummaryAdditionViewModel>();
            TblEvcregistration? TblEvcregistration = null;
            List<TblEvcwalletAddition> tblEvcwalletAddition = null;
            TblUserRole tblUserRole = null;
            TblRole tblRole = null;
            string URL = _config.Value.URLPrefixforProd;

            try
            {
                var draw = Request.Form["draw"].FirstOrDefault();
                var start = Request.Form["start"].FirstOrDefault();
                var length = Request.Form["length"].FirstOrDefault();
                var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
                var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();
                var searchValue = Request.Form["search[value]"].FirstOrDefault();
                int pageSize = length != null ? Convert.ToInt32(length) : 0;
                int skip = start != null ? Convert.ToInt32(start) : 0;
                int recordsTotal = 0;

                TblEvcregistration = _context.TblEvcregistrations.Where(x => x.IsActive == true && x.UserId == LoggingId && x.Isevcapprovrd == true).FirstOrDefault();
                if (TblEvcregistration != null)
                {
                    tblEvcwalletAddition = _context.TblEvcwalletAdditions.Where(x => x.EvcregistrationId == TblEvcregistration.EvcregistrationId).ToList();

                }
                recordsTotal = tblEvcwalletAddition != null ? tblEvcwalletAddition.Count : 0;

                if (tblEvcwalletAddition != null)
                {

                    //int count = 0;
                    foreach (var items in tblEvcwalletAddition)
                    {
                        // count++;
                        MyWalletSummaryAdditionViewModel myWalletSummaryAddition1 = new MyWalletSummaryAdditionViewModel();
                        lattestAllocationViewModel lattestAllocationViewModels = new lattestAllocationViewModel();

                        //tblUserRole = _userRoleRepository.GetSingle(where: x => x.UserId == TblEvcregistration.UserId);
                        tblUserRole = _context.TblUserRoles.Where(x => x.UserId == TblEvcregistration.UserId).FirstOrDefault();
                        //tblRole = _roleRepository.GetSingle(where: x => x.RoleId == tblUserRole.RoleId);
                        tblRole = _context.TblRoles.Where(x => x.RoleId == tblUserRole.RoleId).FirstOrDefault();
                        if (items.CreatedDate != null)
                        {
                            myWalletSummaryAddition1.evcRegistrationId = items.EvcregistrationId;
                            //myWalletSummaryAddition1.EVCRegisterationId = items.EvcregistrationId;
                            myWalletSummaryAddition1.Amount = items.Amount;
                            myWalletSummaryAddition1.AddedBy = tblRole.RoleName;
                            myWalletSummaryAddition1.CreatedDate = items.CreatedDate;
                            if (items.IsCreaditNote == true)
                            {
                                myWalletSummaryAddition1.type = "Credit Recharge";

                            }
                            else
                            {
                                myWalletSummaryAddition1.type = "Wallet Recharge";
                            }

                            var finaldate = (DateTime)myWalletSummaryAddition1.CreatedDate;
                            myWalletSummaryAddition1.FinalDate = finaldate.Date.ToShortDateString();
                        }
                        myWalletSummaryAddition.Add(myWalletSummaryAddition1);
                        //if (count <= 5)
                        //{
                        //    myWalletSummaryAddition.Add(myWalletSummaryAddition1);
                        //}
                        //else
                        //    break;

                    }

                }
                myWalletSummaryAddition = myWalletSummaryAddition.OrderByDescending(O => O.FinalDate).ToList();
                if (myWalletSummaryAddition != null)
                {
                    myWalletSummaryAddition = myWalletSummaryAddition.Skip(skip).Take(pageSize).ToList();
                }
                var data = myWalletSummaryAddition;
                var jsonData = new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data };
                return Ok(jsonData);
            }

            catch (Exception ex)
            {
                throw;
            }
        }
        #endregion

        #region ADDED BY ASHWIN------ FOR SHOWING ALL RECORD OF WALLET UTILIZATION TO LOGIN EVC USE       

        public async Task<ActionResult> EVC_WalletUtilization(int LoggingId, DateTime? orderStartDate,
           DateTime? orderEndDate, int? productCatId,
           int? productTypeId, string? regdNo, string? evcstoreCode)
        {
            #region Variable declaration

            if (!string.IsNullOrWhiteSpace(regdNo) && regdNo != "null")
            { regdNo = regdNo.Trim().ToLower(); }
            else { regdNo = null; }
            if (!string.IsNullOrWhiteSpace(evcstoreCode) && evcstoreCode != "null")
            { evcstoreCode = evcstoreCode.Trim().ToLower(); }
            else { evcstoreCode = null; }



            string URL = _config.Value.URLPrefixforProd;
            string MVCURL = _config.Value.MVCBaseURLForExchangeInvoice;
            string InvoiceimagURL = string.Empty;
            TblCompany tblCompany = null;
            TblBusinessUnit tblBusinessUnit = null;
            TblEvcPartner? tblEvcPartner = null;
            TblEvcregistration TblEvcregistrations = new TblEvcregistration();
            List<lattestAllocationViewModel> lattestAllocationViewModellist = new List<lattestAllocationViewModel>();

            #endregion
            try
            {


                var draw = Request.Form["draw"].FirstOrDefault();
                var start = Request.Form["start"].FirstOrDefault();
                var length = Request.Form["length"].FirstOrDefault();
                var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
                var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();
                var searchValue = Request.Form["search[value]"].FirstOrDefault();
                int pageSize = length != null ? Convert.ToInt32(length) : 0;
                int skip = start != null ? Convert.ToInt32(start) : 0;
                TblEvcregistrations = _context.TblEvcregistrations.Where(x => x.IsActive == true && x.UserId == LoggingId && x.Isevcapprovrd == true).FirstOrDefault();

                IQueryable<TblWalletTransaction> query = _context.TblWalletTransactions
                    .Include(x => x.Evcregistration)
                    .Include(x => x.Evcpartner)
                    .Include(x => x.OrderTrans).ThenInclude(x => x.Exchange).ThenInclude(x => x.ProductType).ThenInclude(x => x.ProductCat)
                    .Include(x => x.OrderTrans).ThenInclude(x => x.Exchange).ThenInclude(x => x.CustomerDetails)
                    .Include(x => x.OrderTrans).ThenInclude(x => x.Abbredemption).ThenInclude(x => x.Abbregistration).ThenInclude(x => x.NewProductCategory)
                    .Include(x => x.OrderTrans).ThenInclude(x => x.Abbredemption).ThenInclude(x => x.Abbregistration).ThenInclude(x => x.NewProductCategoryTypeNavigation)
                    .Include(x => x.OrderTrans).ThenInclude(x => x.Abbredemption).ThenInclude(x => x.CustomerDetails)
                    .Include(x => x.OrderTrans).ThenInclude(x => x.Abbredemption).ThenInclude(x => x.Abbregistration).ThenInclude(x => x.BusinessUnit)
  .Where(x => x.EvcregistrationId == TblEvcregistrations.EvcregistrationId && x.OrderTransId != null && x.StatusId != Convert.ToInt32(OrderStatusEnum.PickupDecline).ToString() && x.OrderofAssignDate != null && x.OrderOfInprogressDate != null /*&& x.OrderOfCompleteDate == null*/ && x.IsActive == true);


                if (orderStartDate != null && orderEndDate != null)
                {
                    orderStartDate = orderStartDate.Value.AddMinutes(-1);
                    orderEndDate = orderEndDate.Value.AddDays(1).AddSeconds(-1);
                    query = query.Where(x => x.CreatedDate >= orderStartDate && x.CreatedDate <= orderEndDate);
                }

                if (productCatId.HasValue)
                {
                    // Placeholder: Add filter for productCatId
                    query = query.Where(x => x.OrderTrans.Exchange != null ? (x.OrderTrans.Exchange.ProductType.ProductCat.Id == productCatId) : (x.OrderTrans.Abbredemption.Abbregistration.NewProductCategoryId == productCatId));
                }

                if (productTypeId.HasValue)
                {
                    // Placeholder: Add filter for productTypeId
                    query = query.Where(x => x.OrderTrans.Exchange != null ? (x.OrderTrans.Exchange.ProductType.Id == productTypeId) : (x.OrderTrans.Abbredemption.Abbregistration.NewProductCategoryTypeId == productTypeId));
                }

                if (!string.IsNullOrEmpty(regdNo))
                {
                    // Placeholder: Add filter for regdNo
                    query = query.Where(x => x.RegdNo == regdNo);
                }


                if (!string.IsNullOrEmpty(evcstoreCode))
                {
                    // Placeholder: Add filter for regdNo evcstoreCode
                    query = query.Where(x => x.Evcpartner.EvcStoreCode == evcstoreCode);
                }


                int recordsTotal = query.Count();


                query = query.OrderByDescending(x => x.ModifiedDate)
                   .Skip(skip)
                   .Take(pageSize);

                List<TblWalletTransaction> tblWalletTransactions = query.ToList();

                // List<AssignOrderViewModel> assignOrderViewModels = _mapper.Map<List<TblWalletTransaction>, List<AssignOrderViewModel>>(tblWalletTransactions);

                foreach (var items in tblWalletTransactions)
                {
                    lattestAllocationViewModel lattestAllocationViewModels = new lattestAllocationViewModel();
                    if (items.OrderOfInprogressDate != null)
                    {
                        lattestAllocationViewModels.evcRegistrationId = items.EvcregistrationId;
                        lattestAllocationViewModels.storecode = items.Evcpartner?.EvcStoreCode;
                        lattestAllocationViewModels.regdNo = items.RegdNo;
                        lattestAllocationViewModels.orderAmount = items.OrderAmount;
                        lattestAllocationViewModels.deliveryDate = items.OrderOfDeliverdDate;
                        lattestAllocationViewModels.completeDate = items.OrderOfCompleteDate;
                        lattestAllocationViewModels.inProgressDate = items.OrderOfInprogressDate;

                        var finaldate = (DateTime)items.OrderofAssignDate;
                        lattestAllocationViewModels.FinalDate = finaldate.Date.ToShortDateString();
                        if (items.OrderOfCompleteDate != null)
                        {
                            var completeDate = (DateTime)items.OrderOfCompleteDate;
                            lattestAllocationViewModels.dateComplete = completeDate.Date.ToShortDateString();
                        }

                        if (items.OrderOfInprogressDate != null)
                        {
                            var inProgressDate = (DateTime)items.OrderOfInprogressDate;
                            lattestAllocationViewModels.dateInProg = inProgressDate.Date.ToShortDateString();
                        }
                        //lattestAllocationViewModels.FinalDate = items.OrderofAssignDate.ToString();
                        //lattestAllocationViewModels.FinalDate = lattestAllocationViewModels.FinalDate.Date.ToShortDateString();
                        if (items.StatusId == Convert.ToInt32(OrderStatusEnum.Posted).ToString())
                        {
                            lattestAllocationViewModels.WalletStatus = Convert.ToString(EVCWalletstatus.Debit) ?? string.Empty;

                        }
                        else
                        {
                            lattestAllocationViewModels.WalletStatus = Convert.ToString(EVCWalletstatus.Hold) ?? string.Empty;


                        }
                        if (items.OrderTrans?.OrderType == Convert.ToInt32(OrderTypeEnum.Exchange))
                        {
                            lattestAllocationViewModels.productTypeName = items.OrderTrans?.Exchange?.ProductType?.Description;
                            lattestAllocationViewModels.productCategoryName = items.OrderTrans?.Exchange?.ProductType?.ProductCat?.Description;
                        }
                        else
                        {
                            lattestAllocationViewModels.productTypeName = items.OrderTrans?.Abbredemption?.Abbregistration?.NewProductCategoryTypeNavigation?.Description;
                            lattestAllocationViewModels.productCategoryName = items.OrderTrans?.Abbredemption?.Abbregistration?.NewProductCategory?.Description;
                        }
                    }
                    lattestAllocationViewModellist.Add(lattestAllocationViewModels);


                }


                //lattestAllocationViewModellist = lattestAllocationViewModellist.OrderByDescending(O => O.ModifiedDate).ToList();
                //if (lattestAllocationViewModellist != null)
                //{
                //    lattestAllocationViewModellist = lattestAllocationViewModellist.Skip(skip).Take(pageSize).ToList();
                //}
                var data = lattestAllocationViewModellist;
                var jsonData = new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data };
                return Ok(jsonData);

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        #endregion

        #region ADDED BY Vishal Khare---Get All Generated Invoice List 
        /// <summary>
        /// Get All Generated Invoice List 
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> GetListOfGeneratedInvoices(int? evcRegId, DateTime? startDate, DateTime? endDate)
        {
            List<TblEvcpoddetail> tblEvcpoddetails = null;
            List<PODViewModel> podVMList = null;
            PODViewModel podVM = null;
            TblWalletTransaction tblWalletTransaction = null;
            TblExchangeOrder tblExchangeOrder = null;
            string URL = _config.Value.BaseURL;

            try
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

                if (startDate == null && endDate == null)
                {
                    tblEvcpoddetails = _context.TblEvcpoddetails
                    .Include(x => x.Evc)
                    .Include(x => x.Evc).ThenInclude(x => x.City).AsEnumerable()
                    .Where(x => x.IsActive == true && x.InvoicePdfName != null
                    && (evcRegId == null ? true : x.Evcid == evcRegId)
                    && (string.IsNullOrEmpty(searchValue)
                    || (x.RegdNo ?? "").ToLower().Contains(searchValue.ToLower())
                    || (x.InvoicePdfName ?? "").ToLower().Contains(searchValue.ToLower())
                    || (x.DebitNotePdfName ?? "").ToLower().Contains(searchValue.ToLower())
                    || (x.Evc == null ? false :
                    (x.Evc.City == null ? false : (x.Evc.City.Name ?? "").ToLower().Contains(searchValue.ToLower()))
                    || (x.Evc.BussinessName ?? "").ToLower().Contains(searchValue.ToLower()))
                    || (x.InvoiceAmount == null ? false : x.InvoiceAmount.ToString().ToLower().Contains(searchValue.ToLower()))
                    || (searchValue.Contains("/") && Convert.ToDateTime(x.InvoiceDate).ToString("dd/MM/yyyy").Contains(searchValue))
                    )).OrderByDescending(x => x.InvoiceDate).ThenByDescending(n => n.InvSrNum).ToList();
                }
                else
                {
                    startDate = Convert.ToDateTime(startDate).AddMinutes(-1);
                    endDate = Convert.ToDateTime(endDate).AddDays(1).AddSeconds(-1);
                    tblEvcpoddetails = _context.TblEvcpoddetails
                    .Include(x => x.Evc)
                    .Include(x => x.Evc).ThenInclude(x => x.City).AsEnumerable()
                    .Where(x => x.IsActive == true && x.InvoicePdfName != null && x.CreatedDate >= startDate && x.CreatedDate <= endDate
                    && (evcRegId == null ? true : x.Evcid == evcRegId)
                    && (string.IsNullOrEmpty(searchValue)
                    || (x.RegdNo ?? "").ToLower().Contains(searchValue.ToLower())
                    || (x.InvoicePdfName ?? "").ToLower().Contains(searchValue.ToLower())
                    || (x.DebitNotePdfName ?? "").ToLower().Contains(searchValue.ToLower())
                    || (x.Evc == null ? false :
                    (x.Evc.City == null ? false : (x.Evc.City.Name ?? "").ToLower().Contains(searchValue.ToLower()))
                    || (x.Evc.BussinessName ?? "").ToLower().Contains(searchValue.ToLower()))
                    || (x.InvoiceAmount == null ? false : x.InvoiceAmount.ToString().ToLower().Contains(searchValue.ToLower()))
                    || (searchValue.Contains("/") && Convert.ToDateTime(x.InvoiceDate).ToString("dd/MM/yyyy").Contains(searchValue))
                    )).OrderByDescending(x => x.InvoiceDate).ThenByDescending(n => n.InvSrNum).ToList();
                }
                if (pageSize != 2147483647)
                {
                    tblEvcpoddetails = tblEvcpoddetails.GroupBy(x => x.InvoicePdfName).Select(x => x.LastOrDefault()).ToList();
                }

                recordsTotal = tblEvcpoddetails != null ? tblEvcpoddetails.Count : 0;
                if (tblEvcpoddetails != null)
                {
                    tblEvcpoddetails = tblEvcpoddetails.Skip(skip).Take(pageSize).ToList();
                }
                podVMList = new List<PODViewModel>();
                if (tblEvcpoddetails != null)
                {
                    foreach (var item in tblEvcpoddetails)
                    {
                        tblWalletTransaction = _context.TblWalletTransactions.Where(x => x.IsActive == true && x.RegdNo == item.RegdNo).FirstOrDefault();
                        tblExchangeOrder = _context.TblExchangeOrders.Where(x => x.IsActive == true && x.RegdNo == item.RegdNo).FirstOrDefault();
                        string actionURL = string.Empty; string actionURLDN = string.Empty;
                        actionURL = "<td class='actions'>";
                        actionURL = actionURL + "<a target='_blanck' href='" + URL + EnumHelper.DescriptionAttr(FileAddressEnum.EVCInvoice) + item.InvoicePdfName + "'>" + item.InvoicePdfName + "</a>";
                        actionURL = actionURL + " </td>";
                        actionURLDN = "<td class='actions'>";
                        actionURLDN = actionURLDN + "<a target='_blanck' href='" + URL + EnumHelper.DescriptionAttr(FileAddressEnum.EVCDebitNote) + item.DebitNotePdfName + "'>" + item.DebitNotePdfName + "</a>";
                        actionURLDN = actionURLDN + " </td>";
                        if (item != null && tblExchangeOrder != null && tblWalletTransaction != null)
                        {
                            var finalExchPirce = Convert.ToDecimal(tblExchangeOrder.FinalExchangePrice ?? 0);
                            var sweetner = Convert.ToDecimal(tblExchangeOrder.Sweetener ?? 0);
                            var FinalOrderAmt = finalExchPirce - sweetner;
                            var OrderAmtEVC = Convert.ToDecimal(tblWalletTransaction.OrderAmount ?? 0);
                            var OrderAmtInv = OrderAmtEVC - FinalOrderAmt;
                            var GSTAmtInv = OrderAmtInv * Convert.ToDecimal(0.18);
                            podVM = new PODViewModel();
                            /*podVM.Action = actionURL;*/
                            podVM.Id = item.Id;
                            podVM.RegdNo = item.RegdNo;
                            podVM.OrderAmtForEVCInv = OrderAmtInv;
                            podVM.GstAmtForEVCInv = GSTAmtInv;
                            podVM.InvoicePdfName = actionURL;
                            podVM.DebitNotePdfName = actionURLDN;
                            podVM.InvoiceAmount = Decimal.Round(item.InvoiceAmount ?? 0, 2);
                            podVM.InvoiceDateString = Convert.ToDateTime(item.InvoiceDate).ToString("dd/MM/yyyy");
                            podVM.FinancialYear = item.FinancialYear;
                            if (item.Evc != null)
                            {
                                podVM.EVCBussinessName = item.Evc.BussinessName;
                                if (item.Evc.City != null)
                                {
                                    podVM.EVCCity = item.Evc.City.Name;
                                }
                            }
                            podVMList.Add(podVM);
                        }
                    }
                }
                var data = podVMList;
                //var data = podVMList.OrderByDescending(x => x.Id);
                var jsonData = new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data };
                return Ok(jsonData);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        #endregion

        #region ADDED BY Priyanshi Sahu---Get All Approverd EVC List //updated Iqueryable
        [HttpPost]
        public async Task<ActionResult> GetEVC_PartnerList(int LoggingId, DateTime? orderStartDate, DateTime? orderEndDate, string? phoneNo, string? custState, string? custCity, string? custPin, int? createdbyme, string? evcregdNo)
        {
            string URL = _config.Value.URLPrefixforProd;
            try
            {
                var draw = Request.Form["draw"].FirstOrDefault();
                var start = Request.Form["start"].FirstOrDefault();
                var length = Request.Form["length"].FirstOrDefault();
                var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
                var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();
                var searchValue = Request.Form["search[value]"].FirstOrDefault();
                int pageSize = length != null ? Convert.ToInt32(length) : 0;
                int skip = start != null ? Convert.ToInt32(start) : 0;
                int recordsTotal = 0;

                TblEvcregistration? tblEvcregistration = _context.TblEvcregistrations.Where(x => x.IsActive == true && x.UserId == LoggingId).FirstOrDefault();
                int evcId = tblEvcregistration.EvcregistrationId;
                IQueryable<TblEvcPartner> query = _context.TblEvcPartners
                    .Include(x => x.Evcregistration)
                    .Include(x => x.State)
                    .Include(x => x.City)
                    .Where(x => x.IsActive == true && x.EvcregistrationId == evcId);

                //if (!string.IsNullOrEmpty(searchValue))
                //{
                //    query = query.Where(x =>
                //        x.EvcregdNo.ToLower().Contains(searchValue.ToLower().Trim()) ||
                //        (x.EvcmobileNumber != null && x.EvcmobileNumber.ToLower().Contains(searchValue.ToLower().Trim())) ||
                //        (x.City.Name != null && x.City.Name.ToLower().Contains(searchValue.ToLower().Trim())) ||
                //        (x.State.Name != null && x.State.Name.ToLower().Contains(searchValue.ToLower().Trim())) ||
                //        (x.BussinessName != null && x.BussinessName.ToLower().Contains(searchValue.ToLower().Trim()))
                //    );
                //}               
                if (orderStartDate != null && orderEndDate != null)
                {
                    orderStartDate = orderStartDate.Value.AddMinutes(-1);
                    orderEndDate = orderEndDate.Value.AddDays(1).AddSeconds(-1);
                    query = query.Where(x => x.CreatedDate >= orderStartDate && x.CreatedDate <= orderEndDate);
                }
                if (!string.IsNullOrEmpty(phoneNo))
                {
                    // Placeholder: Add filter for phoneNo
                    query = query.Where(x => x.ContactNumber != null && x.ContactNumber == phoneNo);
                }
                if (!string.IsNullOrEmpty(custState))
                {
                    // Placeholder: Add filter for custState
                    query = query.Where(x => x.State != null && x.State.Name == custState);
                }
                if (!string.IsNullOrEmpty(custCity))
                {
                    // Placeholder: Add filter for custCity
                    query = query.Where(x => x.City != null && x.City.Name == custCity);
                }
                if (!string.IsNullOrEmpty(custPin))
                {
                    // Placeholder: Add filter for custPin
                    query = query.Where(x => x.PinCode != null && x.PinCode == custPin);
                }
                if (!string.IsNullOrEmpty(evcregdNo))
                {
                    // Placeholder: Add filter for regdNo
                    query = query.Where(x => x.Evcregistration.EvcregdNo == evcregdNo);
                }
                recordsTotal = await query.CountAsync();

                if (!string.IsNullOrEmpty(sortColumn))
                {
                    if (sortColumnDirection.Equals(SortingOrder.ASCENDING))
                    {
                        query = query.OrderBy(x => EF.Property<object>(x, sortColumn));
                    }
                    else
                    {
                        query = query.OrderByDescending(x => EF.Property<object>(x, sortColumn));
                    }
                }

                query = query.OrderByDescending(x => x.ModifiedDate).Skip(skip).Take(pageSize);

                List<TblEvcPartner> tblEvcPartners = await query.ToListAsync();
                List<EVC_PartnerViewModel> eVC_PartnerViewModels = _mapper.Map<List<TblEvcPartner>, List<EVC_PartnerViewModel>>(tblEvcPartners);

                foreach (EVC_PartnerViewModel item in eVC_PartnerViewModels)
                {

                    string actionURL = " <div class='actionbtns'>";
                    if (item.Isapprove == true)
                    {
                        actionURL += "<a class='btn btn-sm btn-primary' href='" + URL + "/EVC_Portal/Store_Specification?id=" + item.EvcPartnerId + "' title='Store Specification'><i class='fa-solid fa-list'></i></a> &nbsp;";
                    }
                    else
                    {
                        actionURL += "<p class='btn btn-sm btn-danger'>Waiting for Approval</p> &nbsp;";
                    }
                    //actionURL += "<a class='mx-1 fas fa-edit' href='" + URL + "/EVC_Portal/EVC_StoreRegistrastion?id=" + _protector.Encode(item.EvcPartnerId) + "&AFlag=2' title ='Edit'></a>";
                    //actionURL += "<a href='javascript: void(0)' onclick='deleteConfirm(" + item.EvcPartnerId + ")' class='fas fa-trash' data-toggle='tooltip' data-placement='top' title='Delete'></a>";
                    //actionURL += "</div>";
                    item.Action = actionURL;
                    TblEvcPartner? storeDetails = tblEvcPartners.FirstOrDefault(x => x.EvcPartnerId == item.EvcPartnerId);

                    if (storeDetails != null)
                    {
                        item.EvcPartnerId = storeDetails.EvcPartnerId;
                        item.BussinessName = storeDetails.Evcregistration.EvcregdNo + "-" + storeDetails.Evcregistration.BussinessName;
                        item.Address = $"{storeDetails.Address1} {storeDetails.Address2}"; item.EvcStoreCode = storeDetails.EvcStoreCode;
                        item.StateName = storeDetails?.State?.Name ?? string.Empty;
                        item.CityName = storeDetails?.City?.Name ?? string.Empty;
                        item.PinCode = storeDetails?.PinCode;
                        item.EvcregdNo = storeDetails?.Evcregistration?.EvcregdNo;
                        item.Date = storeDetails?.CreatedDate?.ToShortDateString();

                    }
                }

                var data = eVC_PartnerViewModels;
                var jsonData = new { draw, recordsFiltered = recordsTotal, recordsTotal, data };
                return Ok(jsonData);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        #endregion

        #region ADDED BY Priyanshi Sahu---Get All GetEVC_PartnerpreferenceList //updated Iqueryable
        [HttpPost]
        public async Task<ActionResult> GetEVC_PartnerpreferenceList(int LoggingId, DateTime? orderStartDate, DateTime? orderEndDate, string? StoreRegdNo, int? productCatId, string? evcregdNo)
        {
            string URL = _config.Value.URLPrefixforProd;
            try
            {
                var draw = Request.Form["draw"].FirstOrDefault();
                var start = Request.Form["start"].FirstOrDefault();
                var length = Request.Form["length"].FirstOrDefault();
                var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
                var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();
                var searchValue = Request.Form["search[value]"].FirstOrDefault();
                int pageSize = length != null ? Convert.ToInt32(length) : 0;
                int skip = start != null ? Convert.ToInt32(start) : 0;
                int recordsTotal = 0;

                //TblEvcregistration? tblEvcregistration = _context.TblEvcregistrations.Where(x => x.IsActive == true && x.UserId == LoggingId).FirstOrDefault();
                //int evcId = tblEvcregistration.EvcregistrationId;
                IQueryable<TblEvcPartnerPreference> query = _context.TblEvcPartnerPreferences
                    .Include(x => x.Evcpartner).ThenInclude(x => x.Evcregistration)
                    .Include(x => x.ProductCat)
                    .Where(x => x.EvcpartnerId == LoggingId);

                //if (!string.IsNullOrEmpty(searchValue))
                //{
                //    query = query.Where(x =>
                //        x.EvcregdNo.ToLower().Contains(searchValue.ToLower().Trim()) ||
                //        (x.EvcmobileNumber != null && x.EvcmobileNumber.ToLower().Contains(searchValue.ToLower().Trim())) ||
                //        (x.City.Name != null && x.City.Name.ToLower().Contains(searchValue.ToLower().Trim())) ||
                //        (x.State.Name != null && x.State.Name.ToLower().Contains(searchValue.ToLower().Trim())) ||
                //        (x.BussinessName != null && x.BussinessName.ToLower().Contains(searchValue.ToLower().Trim()))
                //    );
                //}               
                if (orderStartDate != null && orderEndDate != null)
                {
                    orderStartDate = orderStartDate.Value.AddMinutes(-1);
                    orderEndDate = orderEndDate.Value.AddDays(1).AddSeconds(-1);
                    query = query.Where(x => x.CreatedDate >= orderStartDate && x.CreatedDate <= orderEndDate);
                }
                if (!string.IsNullOrEmpty(StoreRegdNo))
                {
                    // Placeholder: Add filter for phoneNo
                    query = query.Where(x => x.Evcpartner.EvcStoreCode != null && x.Evcpartner.EvcStoreCode == StoreRegdNo);
                }
                if (productCatId.HasValue)
                {
                    // Placeholder: Add filter for productCatId
                    query = query.Where(x => x.ProductCat != null && x.ProductCatId == productCatId);
                }
                if (!string.IsNullOrEmpty(evcregdNo))
                {
                    // Placeholder: Add filter for regdNo
                    query = query.Where(x => x.Evcpartner.Evcregistration.EvcregdNo == evcregdNo);
                }
                recordsTotal = await query.CountAsync();

                if (!string.IsNullOrEmpty(sortColumn))
                {
                    if (sortColumnDirection.Equals(SortingOrder.ASCENDING))
                    {
                        query = query.OrderBy(x => EF.Property<object>(x, sortColumn));
                    }
                    else
                    {
                        query = query.OrderByDescending(x => EF.Property<object>(x, sortColumn));
                    }
                }

                query = query.OrderByDescending(x => x.ModifiedDate).Skip(skip).Take(pageSize);

                List<TblEvcPartnerPreference> tblEvcPartnerPreferences = await query.ToListAsync();
                List<EVC_PartnerpreferenceViewModel> eVC_PartnerPreferenceViewModels = _mapper.Map<List<TblEvcPartnerPreference>, List<EVC_PartnerpreferenceViewModel>>(tblEvcPartnerPreferences);

                foreach (EVC_PartnerpreferenceViewModel item in eVC_PartnerPreferenceViewModels)
                {
                    string actionURL = " <div class='actionbtns'>";
                    if (item.IsActive == true)

                    {

                        actionURL = "<div class='actionbtns'>";
                        actionURL = actionURL +
                            "<a href='javascript: void(0)' onclick='deleteConfirmStoreSpecification(" + item.EvcPartnerpreferenceId + ")' class='btn btn-sm btn-danger' data-bs-toggle='tooltip' data-bs-placement='top' title='Delete'><i class='fa-solid fa-trash'></i></a>";
                        actionURL = actionURL + " </div>";

                    }
                    else
                    {

                        actionURL = "<div class='actionbtns'>";
                        actionURL = actionURL +
                            "<a href='javascript: void(0)' onclick='activeConfirmStoreSpecification(" + item.EvcPartnerpreferenceId + ")' class='btn btn-sm  btn-success' data-bs-toggle='tooltip' data-bs-placement='top' title='Active'><i class='fa-solid fa-recycle'></i></a>";
                        actionURL = actionURL + " </div>";
                    }

                    //actionURL += "<a class='btn btn-sm btn-primary' href='" + URL + "/EVC_Portal/Store_Specification?id=" + item.EvcPartnerpreferenceId + "' title='Store Specification'><i class='fa-solid fa-list'></i></a> &nbsp;";
                    //actionURL += "<a class='mx-1 fas fa-edit' href='" + URL + "/EVC_Portal/EVC_StoreRegistrastion?id=" + _protector.Encode(item.EvcPartnerpreferenceId) + "&AFlag=2' title ='Edit'></a>";
                    //actionURL += "<a href='javascript: void(0)' onclick='deleteConfirm(" + item.EvcPartnerpreferenceId + ")' class='fas fa-trash' data-toggle='tooltip' data-placement='top' title='Delete'></a>";
                    //actionURL += "</div>";
                    item.Action = actionURL;
                    TblEvcPartnerPreference? storeDetails = tblEvcPartnerPreferences.FirstOrDefault(x => x.EvcPartnerpreferenceId == item.EvcPartnerpreferenceId);

                    if (storeDetails != null)
                    {
                        item.EvcPartnerpreferenceId = storeDetails.EvcPartnerpreferenceId;
                        item.EvcStoreCode = storeDetails.Evcpartner.EvcStoreCode;
                        item.productCategory = storeDetails.ProductCat.Description;
                        //item.ProductQuality = storeDetails.ProductQuality.Value;      
                        item.EvcregdNo = storeDetails?.Evcpartner.Evcregistration?.EvcregdNo;
                        item.Date = storeDetails?.ModifiedDate?.ToShortDateString();
                        if (storeDetails.ProductQualityId != null && storeDetails.ProductQualityId > 0)
                        {
                            item.ProductQuality = GetEvcPartnerPreferenceDescription(storeDetails.ProductQualityId);
                        }

                    }
                }

                var data = eVC_PartnerPreferenceViewModels;
                var jsonData = new { draw, recordsFiltered = recordsTotal, recordsTotal, data };
                return Ok(jsonData);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        private string GetEvcPartnerPreferenceDescription(int Quality)
        {
            switch (Quality)
            {
                case 1:
                    return EnumHelper.DescriptionAttr(EvcPartnerPreferenceEnum.Excellent);
                case 2:
                    return EnumHelper.DescriptionAttr(EvcPartnerPreferenceEnum.Good);
                case 3:
                    return EnumHelper.DescriptionAttr(EvcPartnerPreferenceEnum.Average);
                case 4:
                    return EnumHelper.DescriptionAttr(EvcPartnerPreferenceEnum.Not_Working);
                default:
                    return string.Empty;
            }
        }
        #endregion

        #region ADDED BY Priyanshi Sahu---Get All not Approverd EVC partner List //updated Iqueryable
        [HttpPost]
        public async Task<ActionResult> GetEVC_NotApprovePartnerList(DateTime? orderStartDate, DateTime? orderEndDate, string? phoneNo, string? custState, string? custCity, string? custPin, int? createdbyme, string? evcregdNo, string? evcstorecode)
        {
            string URL = _config.Value.URLPrefixforProd;
            try
            {
                var draw = Request.Form["draw"].FirstOrDefault();
                var start = Request.Form["start"].FirstOrDefault();
                var length = Request.Form["length"].FirstOrDefault();
                var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
                var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();
                var searchValue = Request.Form["search[value]"].FirstOrDefault();
                int pageSize = length != null ? Convert.ToInt32(length) : 0;
                int skip = start != null ? Convert.ToInt32(start) : 0;
                int recordsTotal = 0;

                IQueryable<TblEvcPartner> query = _context.TblEvcPartners
                    .Include(x => x.Evcregistration)
                    .Include(x => x.State)
                    .Include(x => x.City)
                    .Where(x => x.IsActive == true && x.IsApprove == false);

                //if (!string.IsNullOrEmpty(searchValue))
                //{
                //    query = query.Where(x =>
                //        x.EvcregdNo.ToLower().Contains(searchValue.ToLower().Trim()) ||
                //        (x.EvcmobileNumber != null && x.EvcmobileNumber.ToLower().Contains(searchValue.ToLower().Trim())) ||
                //        (x.City.Name != null && x.City.Name.ToLower().Contains(searchValue.ToLower().Trim())) ||
                //        (x.State.Name != null && x.State.Name.ToLower().Contains(searchValue.ToLower().Trim())) ||
                //        (x.BussinessName != null && x.BussinessName.ToLower().Contains(searchValue.ToLower().Trim()))
                //    );
                //}               
                if (orderStartDate != null && orderEndDate != null)
                {
                    orderStartDate = orderStartDate.Value.AddMinutes(-1);
                    orderEndDate = orderEndDate.Value.AddDays(1).AddSeconds(-1);
                    query = query.Where(x => x.CreatedDate >= orderStartDate && x.CreatedDate <= orderEndDate);
                }
                if (!string.IsNullOrEmpty(phoneNo))
                {
                    // Placeholder: Add filter for phoneNo
                    query = query.Where(x => x.ContactNumber != null && x.ContactNumber == phoneNo);
                }
                if (!string.IsNullOrEmpty(custState))
                {
                    // Placeholder: Add filter for custState
                    query = query.Where(x => x.State != null && x.State.Name == custState);
                }
                if (!string.IsNullOrEmpty(custCity))
                {
                    // Placeholder: Add filter for custCity
                    query = query.Where(x => x.City != null && x.City.Name == custCity);
                }
                if (!string.IsNullOrEmpty(custPin))
                {
                    // Placeholder: Add filter for custPin
                    query = query.Where(x => x.PinCode != null && x.PinCode == custPin);
                }
                if (!string.IsNullOrEmpty(evcregdNo))
                {
                    // Placeholder: Add filter for regdNo
                    query = query.Where(x => x.Evcregistration.EvcregdNo == evcregdNo);
                }
                if (!string.IsNullOrEmpty(evcstorecode))
                {
                    // Placeholder: Add filter for evcstorecode
                    query = query.Where(x => x.EvcStoreCode != null && x.EvcStoreCode == evcstorecode);
                }
                recordsTotal = await query.CountAsync();

                if (!string.IsNullOrEmpty(sortColumn))
                {
                    if (sortColumnDirection.Equals(SortingOrder.ASCENDING))
                    {
                        query = query.OrderBy(x => EF.Property<object>(x, sortColumn));
                    }
                    else
                    {
                        query = query.OrderByDescending(x => EF.Property<object>(x, sortColumn));
                    }
                }

                query = query.OrderByDescending(x => x.ModifiedDate).Skip(skip).Take(pageSize);

                List<TblEvcPartner> tblEvcPartners = await query.ToListAsync();
                List<EVC_PartnerViewModel> eVC_PartnerViewModels = _mapper.Map<List<TblEvcPartner>, List<EVC_PartnerViewModel>>(tblEvcPartners);

                foreach (EVC_PartnerViewModel item in eVC_PartnerViewModels)
                {
                    string actionURL = " <div class='actionbtns'>";
                    actionURL += "<a class='btn btn-sm btn-primary' href='" + URL + "/EVC/EVC_PartnerAddListofPincde?id=" + item.EvcPartnerId + "' title='Approve Partner'><i class='fa-solid fa-shop'></i></a> &nbsp;";
                    actionURL += "<a class='mx-1 fas fa-edit' href='" + URL + "/EVC_Portal/EVC_StoreRegistrastion?id=" + _protector.Encode(item.EvcPartnerId) + "&AFlag=2' title ='Edit'></a>";
                    actionURL += "<a href='javascript: void(0)' onclick='deleteConfirm(" + item.EvcPartnerId + ")' class='fas fa-trash' data-toggle='tooltip' data-placement='top' title='Delete'></a>";
                    actionURL += "</div>";
                    item.Action = actionURL;
                    TblEvcPartner? storeDetails = tblEvcPartners.FirstOrDefault(x => x.EvcPartnerId == item.EvcPartnerId);

                    if (storeDetails != null)
                    {
                        item.EvcPartnerId = storeDetails.EvcPartnerId;
                        item.BussinessName = storeDetails.Evcregistration.EvcregdNo + "-" + storeDetails.Evcregistration.BussinessName;
                        item.Address = $"{storeDetails.Address1} {storeDetails.Address2}"; item.EvcStoreCode = storeDetails.EvcStoreCode;
                        item.StateName = storeDetails?.State?.Name ?? string.Empty;
                        item.CityName = storeDetails?.City?.Name ?? string.Empty;
                        item.PinCode = storeDetails?.PinCode;
                        item.EvcregdNo = storeDetails?.Evcregistration?.EvcregdNo;
                        item.Date = storeDetails?.CreatedDate?.ToShortDateString();
                        item.EvcStoreCode = storeDetails?.EvcStoreCode ?? string.Empty;

                    }
                }

                var data = eVC_PartnerViewModels;
                var jsonData = new { draw, recordsFiltered = recordsTotal, recordsTotal, data };
                return Ok(jsonData);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        #endregion

        #region ADDED BY Priyanshi Sahu---Get All  Approverd EVC partner List //updated Iqueryable
        [HttpPost]
        public async Task<ActionResult> GetEVC_ApprovePartnerList(DateTime? orderStartDate, DateTime? orderEndDate, string? phoneNo, string? custState, string? custCity, string? custPin, int? createdbyme, string? evcregdNo, string? evcstorecode)
        {
            string URL = _config.Value.URLPrefixforProd;
            try
            {
                var draw = Request.Form["draw"].FirstOrDefault();
                var start = Request.Form["start"].FirstOrDefault();
                var length = Request.Form["length"].FirstOrDefault();
                var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
                var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();
                var searchValue = Request.Form["search[value]"].FirstOrDefault();
                int pageSize = length != null ? Convert.ToInt32(length) : 0;
                int skip = start != null ? Convert.ToInt32(start) : 0;
                int recordsTotal = 0;

                IQueryable<TblEvcPartner> query = _context.TblEvcPartners
                    .Include(x => x.Evcregistration)
                    .Include(x => x.State)
                    .Include(x => x.City)
                    .Where(x => x.IsActive == true && x.IsApprove == true);

                //if (!string.IsNullOrEmpty(searchValue))
                //{
                //    query = query.Where(x =>
                //        x.EvcregdNo.ToLower().Contains(searchValue.ToLower().Trim()) ||
                //        (x.EvcmobileNumber != null && x.EvcmobileNumber.ToLower().Contains(searchValue.ToLower().Trim())) ||
                //        (x.City.Name != null && x.City.Name.ToLower().Contains(searchValue.ToLower().Trim())) ||
                //        (x.State.Name != null && x.State.Name.ToLower().Contains(searchValue.ToLower().Trim())) ||
                //        (x.BussinessName != null && x.BussinessName.ToLower().Contains(searchValue.ToLower().Trim()))
                //    );
                //}               
                if (orderStartDate != null && orderEndDate != null)
                {
                    orderStartDate = orderStartDate.Value.AddMinutes(-1);
                    orderEndDate = orderEndDate.Value.AddDays(1).AddSeconds(-1);
                    query = query.Where(x => x.CreatedDate >= orderStartDate && x.CreatedDate <= orderEndDate);
                }
                if (!string.IsNullOrEmpty(phoneNo))
                {
                    // Placeholder: Add filter for phoneNo
                    query = query.Where(x => x.ContactNumber != null && x.ContactNumber == phoneNo);
                }
                if (!string.IsNullOrEmpty(custState))
                {
                    // Placeholder: Add filter for custState
                    query = query.Where(x => x.State != null && x.State.Name == custState);
                }
                if (!string.IsNullOrEmpty(custCity))
                {
                    // Placeholder: Add filter for custCity
                    query = query.Where(x => x.City != null && x.City.Name == custCity);
                }
                if (!string.IsNullOrEmpty(custPin))
                {
                    // Placeholder: Add filter for custPin
                    query = query.Where(x => x.PinCode != null && x.PinCode == custPin);
                }
                if (!string.IsNullOrEmpty(evcregdNo))
                {
                    // Placeholder: Add filter for regdNo
                    query = query.Where(x => x.Evcregistration.EvcregdNo == evcregdNo);
                }
                if (!string.IsNullOrEmpty(evcstorecode))
                {
                    // Placeholder: Add filter for regdNo
                    query = query.Where(x => x.EvcStoreCode != null && x.EvcStoreCode == evcstorecode);
                }
                recordsTotal = await query.CountAsync();

                if (!string.IsNullOrEmpty(sortColumn))
                {
                    if (sortColumnDirection.Equals(SortingOrder.ASCENDING))
                    {
                        query = query.OrderBy(x => EF.Property<object>(x, sortColumn));
                    }
                    else
                    {
                        query = query.OrderByDescending(x => EF.Property<object>(x, sortColumn));
                    }
                }

                query = query.OrderByDescending(x => x.ModifiedDate).Skip(skip).Take(pageSize);

                List<TblEvcPartner> tblEvcPartners = await query.ToListAsync();
                List<EVC_PartnerViewModel> eVC_PartnerViewModels = _mapper.Map<List<TblEvcPartner>, List<EVC_PartnerViewModel>>(tblEvcPartners);

                foreach (EVC_PartnerViewModel item in eVC_PartnerViewModels)
                {
                    string actionURL = " <div class='actionbtns'>";
                    actionURL += "<a class='btn btn-sm btn-primary' href='" + URL + "/EVC/EVC_PartnerAddListofPincde?id=" + item.EvcPartnerId + "' title='Approve Partner'><i class='fa-solid fa-shop'></i></a> &nbsp;";
                    actionURL += "<a class='btn btn-sm btn-primary' href='" + URL + "/EVC_Portal/Store_Specification?id=" + item.EvcPartnerId + "' title='Store Specification'><i class='fa-solid fa-list'></i></a> &nbsp;";
                    actionURL += "<a class='mx-1 fas fa-edit' href='" + URL + "/EVC_Portal/EVC_StoreRegistrastion?id=" + _protector.Encode(item.EvcPartnerId) + "&AFlag=2' title ='Edit'></a>";
                    actionURL += "<a href='javascript: void(0)' onclick='deleteConfirm(" + item.EvcPartnerId + ")' class='fas fa-trash' data-toggle='tooltip' data-placement='top' title='Delete'></a>";
                    actionURL += "</div>";
                    item.Action = actionURL;
                    TblEvcPartner? storeDetails = tblEvcPartners.FirstOrDefault(x => x.EvcPartnerId == item.EvcPartnerId);

                    if (storeDetails != null)
                    {
                        item.EvcPartnerId = storeDetails.EvcPartnerId;
                        item.BussinessName = storeDetails.Evcregistration.EvcregdNo + "-" + storeDetails.Evcregistration.BussinessName;
                        item.Address = $"{storeDetails.Address1} {storeDetails.Address2}"; item.EvcStoreCode = storeDetails.EvcStoreCode;
                        item.StateName = storeDetails?.State?.Name ?? string.Empty;
                        item.CityName = storeDetails?.City?.Name ?? string.Empty;
                        item.PinCode = storeDetails?.PinCode;
                        item.EvcregdNo = storeDetails?.Evcregistration?.EvcregdNo;
                        item.Date = storeDetails?.CreatedDate?.ToShortDateString();
                        item.EvcStoreCode = storeDetails?.EvcStoreCode ?? string.Empty;

                    }
                }

                var data = eVC_PartnerViewModels;
                var jsonData = new { draw, recordsFiltered = recordsTotal, recordsTotal, data };
                return Ok(jsonData);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        #endregion

        #region ADDED BY Priyanshi sahu---Get All Invoice List 
        /// <summary>
        /// Get All Invoice List 
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> GetInvoiceList(int loggingId, int? evcPartnerId, DateTime? startDate, DateTime? endDate)
        {
            List<BulkDownlodeInvoidePdfviewmodel> BulkDownlodeInvoidePdfviewmodelList = new List<BulkDownlodeInvoidePdfviewmodel>();
            string? URL = _config.Value.BaseURL;
            TblEvcregistration? TblEvcregistrations = new TblEvcregistration();
            try
            {
                var draw = Request.Form["draw"].FirstOrDefault();
                var start = Request.Form["start"].FirstOrDefault();
                var length = Request.Form["length"].FirstOrDefault();
                var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
                var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();
                var searchValue = Request.Form["search[value]"].FirstOrDefault();
                int pageSize = length != null ? Convert.ToInt32(length) : 0;
                int skip = start != null ? Convert.ToInt32(start) : 0;
                TblEvcregistrations = _context.TblEvcregistrations.Where(x => x.IsActive == true && x.UserId == loggingId && x.Isevcapprovrd == true).FirstOrDefault();
                if (TblEvcregistrations != null)
                {
                    IQueryable<TblEvcpoddetail> query = _context.TblEvcpoddetails
                        .Include(x => x.Evc)
                        .Include(x => x.Evc).ThenInclude(x => x.City)
                        .Include(x => x.Evcpartner).ThenInclude(x => x.City)
      .Where(x => x.IsActive == true && x.InvoicePdfName != null && x.Evcid == TblEvcregistrations.EvcregistrationId);


                    if (startDate != null && endDate != null)
                    {
                        startDate = startDate.Value.AddMinutes(-1);
                        endDate = endDate.Value.AddDays(1).AddSeconds(-1);
                        query = query.Where(x => x.InvoiceDate >= startDate && x.InvoiceDate <= endDate);
                    }

                    if (evcPartnerId != null && evcPartnerId > 0)
                    {
                        // Placeholder: Add filter for productCatId
                        query = query.Where(x => x.EvcpartnerId == evcPartnerId);
                    }
                    int recordsTotal = query.Count();

                    query = query.OrderByDescending(x => x.InvoiceDate)
                       .Skip(skip)
                       .Take(pageSize);

                    List<TblEvcpoddetail> tblEvcpoddetails1 = query.ToList();
                    foreach (var item in tblEvcpoddetails1)
                    {
                        BulkDownlodeInvoidePdfviewmodel podVM = new BulkDownlodeInvoidePdfviewmodel();
                        string actionURL = string.Empty;
                        actionURL = "<td class='actions'>";
                        actionURL = actionURL + "<a class='text-link-primary' target='_blank' href='" + URL + EnumHelper.DescriptionAttr(FileAddressEnum.EVCInvoice) + item.InvoicePdfName + "'>" + item.InvoicePdfName + "</a>";
                        actionURL = actionURL + " </td>";
                        if (item != null)
                        {
                            podVM = new BulkDownlodeInvoidePdfviewmodel();
                            podVM.Id = item.Id;
                            podVM.InvoicePdfName = actionURL;
                            podVM.InvoiceAmount = Decimal.Round(item.InvoiceAmount ?? 0, 2);
                            podVM.InvoiceDate = Convert.ToDateTime(item.InvoiceDate).ToString("dd/MM/yyyy");
                            if (item.Evc != null)
                            {
                                podVM.EVCBussinessName = item.Evc.EvcregdNo + " - " + item.Evc.BussinessName;
                                if (item.Evcpartner != null && item.Evcpartner.City != null)
                                {
                                    podVM.EVCPartnerCode = item.Evcpartner?.EvcStoreCode;
                                    podVM.EVCCity = item.Evcpartner?.City.Name;
                                }
                            }
                            BulkDownlodeInvoidePdfviewmodelList.Add(podVM);
                        }
                    }
                    var data = BulkDownlodeInvoidePdfviewmodelList;
                    //data = (List<BulkDownlodeInvoidePdfviewmodel>)BulkDownlodeInvoidePdfviewmodelList.OrderByDescending(x => x.InvoiceDate);
                    var jsonData = new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data };
                    return Ok(jsonData);

                }
                return Ok();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        #endregion

        #region ADDED BY Priyanshi sahu---Get All debit note List 
        /// <summary>
        /// Get All debit note List 
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> GetDebitnoteList(int loggingId, int? evcPartnerId, DateTime? startDate, DateTime? endDate)
        {
            List<BulkDownlodeDebitnotePdfviewmodel> BulkDownlodeDebitnotePdfviewmodelList = new List<BulkDownlodeDebitnotePdfviewmodel>();
            string? URL = _config.Value.BaseURL;
            TblEvcregistration? TblEvcregistrations = new TblEvcregistration();
            try
            {
                var draw = Request.Form["draw"].FirstOrDefault();
                var start = Request.Form["start"].FirstOrDefault();
                var length = Request.Form["length"].FirstOrDefault();
                var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
                var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();
                var searchValue = Request.Form["search[value]"].FirstOrDefault();
                int pageSize = length != null ? Convert.ToInt32(length) : 0;
                int skip = start != null ? Convert.ToInt32(start) : 0;
                TblEvcregistrations = _context.TblEvcregistrations.Where(x => x.IsActive == true && x.UserId == loggingId && x.Isevcapprovrd == true).FirstOrDefault();
                if (TblEvcregistrations != null)
                {
                    IQueryable<TblEvcpoddetail> query = _context.TblEvcpoddetails
                        .Include(x => x.Evc)
                        .Include(x => x.Evc).ThenInclude(x => x.City)
                        .Include(x => x.Evcpartner).ThenInclude(x => x.City)
      .Where(x => x.IsActive == true && x.DebitNotePdfName != null && x.Evcid == TblEvcregistrations.EvcregistrationId);


                    if (startDate != null && endDate != null)
                    {
                        startDate = startDate.Value.AddMinutes(-1);
                        endDate = endDate.Value.AddDays(1).AddSeconds(-1);
                        query = query.Where(x => x.DebitNoteDate >= startDate && x.DebitNoteDate <= endDate);
                    }

                    if (evcPartnerId != null && evcPartnerId > 0)
                    {
                        // Placeholder: Add filter for productCatId
                        query = query.Where(x => x.EvcpartnerId == evcPartnerId);
                    }
                    int recordsTotal = query.Count();

                    query = query.OrderByDescending(x => x.DebitNoteDate)
                       .Skip(skip)
                       .Take(pageSize);

                    List<TblEvcpoddetail> tblEvcpoddetails1 = query.ToList();
                    foreach (var item in tblEvcpoddetails1)
                    {
                        BulkDownlodeDebitnotePdfviewmodel podVM = new BulkDownlodeDebitnotePdfviewmodel();
                        string actionURLDN = string.Empty;
                        actionURLDN = "<td class='actions'>";
                        actionURLDN = actionURLDN + "<a class='text-link-primary' target='_blank' href='" + URL + EnumHelper.DescriptionAttr(FileAddressEnum.EVCDebitNote) + item.DebitNotePdfName + "'>" + item.DebitNotePdfName + "</a>";
                        actionURLDN = actionURLDN + " </td>";
                        if (item != null)
                        {
                            podVM = new BulkDownlodeDebitnotePdfviewmodel();
                            podVM.Id = item.Id;
                            podVM.DebitNotePdfName = actionURLDN;
                            podVM.DebitNoteAmount = Decimal.Round(item.DebitNoteAmount ?? 0, 2);
                            podVM.DebitNoteDate = Convert.ToDateTime(item.DebitNoteDate).ToString("dd/MM/yyyy");
                            if (item.Evc != null)
                            {
                                podVM.EVCBussinessName = item.Evc.EvcregdNo + " - " + item.Evc.BussinessName;
                                if (item.Evcpartner != null && item.Evcpartner.City != null)
                                {
                                    podVM.EVCPartnerCode = item.Evcpartner?.EvcStoreCode;
                                    podVM.EVCCity = item.Evcpartner?.City.Name;
                                }
                            }
                            BulkDownlodeDebitnotePdfviewmodelList.Add(podVM);
                        }
                    }
                    var data = BulkDownlodeDebitnotePdfviewmodelList;
                    //data = (List<BulkDownlodeDebitnotePdfviewmodel>)BulkDownlodeDebitnotePdfviewmodelList.OrderByDescending(x => x.DebitNoteDate);
                    var jsonData = new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data };
                    return Ok(jsonData);

                }
                return Ok();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        #endregion

        #region ADDED BY Priyanshi sahu---Get All POD List 
        /// <summary>
        /// Get All POD List 
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> GetPODList(int loggingId, int? evcPartnerId, DateTime? startDate, DateTime? endDate)
        {
            List<BulkDownlodePODPdfviewmodel> BulkDownlodePODPdfviewmodelList = new List<BulkDownlodePODPdfviewmodel>();
            string? URL = _config.Value.BaseURL;
            TblEvcregistration? TblEvcregistrations = new TblEvcregistration();
            try
            {
                var draw = Request.Form["draw"].FirstOrDefault();
                var start = Request.Form["start"].FirstOrDefault();
                var length = Request.Form["length"].FirstOrDefault();
                var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
                var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();
                var searchValue = Request.Form["search[value]"].FirstOrDefault();
                int pageSize = length != null ? Convert.ToInt32(length) : 0;
                int skip = start != null ? Convert.ToInt32(start) : 0;
                TblEvcregistrations = _context.TblEvcregistrations.Where(x => x.IsActive == true && x.UserId == loggingId && x.Isevcapprovrd == true).FirstOrDefault();
                if (TblEvcregistrations != null)
                {
                    IQueryable<TblEvcpoddetail> query = _context.TblEvcpoddetails
                        .Include(x => x.Evc)
                        .Include(x => x.Evc).ThenInclude(x => x.City)
                        .Include(x => x.Evcpartner).ThenInclude(x => x.City)
      .Where(x => x.IsActive == true && x.Podurl != null && x.Evcid == TblEvcregistrations.EvcregistrationId);


                    if (startDate != null && endDate != null)
                    {
                        startDate = startDate.Value.AddMinutes(-1);
                        endDate = endDate.Value.AddDays(1).AddSeconds(-1);
                        query = query.Where(x => x.CreatedDate >= startDate && x.CreatedDate <= endDate);
                    }

                    if (evcPartnerId != null && evcPartnerId > 0)
                    {
                        // Placeholder: Add filter for productCatId
                        query = query.Where(x => x.EvcpartnerId == evcPartnerId);
                    }
                    int recordsTotal = query.Count();

                    query = query.OrderByDescending(x => x.CreatedDate)
                       .Skip(skip)
                       .Take(pageSize);

                    List<TblEvcpoddetail> tblEvcpoddetails1 = query.ToList();
                    foreach (var item in tblEvcpoddetails1)
                    {
                        BulkDownlodePODPdfviewmodel podVM = new BulkDownlodePODPdfviewmodel();
                        string actionURLPOD = string.Empty;
                        actionURLPOD = "<td class='actions'>";
                        actionURLPOD = actionURLPOD + "<a class='text-link-primary' target='_blank' href='" + URL + EnumHelper.DescriptionAttr(FileAddressEnum.EVCPoD) + item.Podurl + "'>" + item.Podurl + "</a>";
                        actionURLPOD = actionURLPOD + " </td>";
                        if (item != null)
                        {
                            podVM = new BulkDownlodePODPdfviewmodel();
                            podVM.Id = item.Id;
                            podVM.PODPdfName = actionURLPOD;
                            podVM.PODDate = Convert.ToDateTime(item.CreatedDate).ToString("dd/MM/yyyy");
                            if (item.Evc != null)
                            {
                                podVM.EVCBussinessName = item.Evc.EvcregdNo + " - " + item.Evc.BussinessName;
                                if (item.Evcpartner != null && item.Evcpartner.City != null)
                                {
                                    podVM.EVCPartnerCode = item.Evcpartner?.EvcStoreCode;
                                    podVM.EVCCity = item.Evcpartner?.City.Name;
                                }
                            }
                            BulkDownlodePODPdfviewmodelList.Add(podVM);
                        }
                    }
                    var data = BulkDownlodePODPdfviewmodelList;
                    // data = (List<BulkDownlodePODPdfviewmodel>)BulkDownlodePODPdfviewmodelList.OrderByDescending(x => x.PODDate);
                    var jsonData = new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data };
                    return Ok(jsonData);

                }
                return Ok();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        #endregion

        #region ADDED BY Priyanshi sahu---Get All CD List 
        /// <summary>
        /// Get All CD List 
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> GetCDList(int loggingId, int? evcPartnerId, DateTime? startDate, DateTime? endDate)
        {
            List<BulkDownlodeCDPdfviewmodel> BulkDownlodeCDPdfviewmodelList = new List<BulkDownlodeCDPdfviewmodel>();
            string? URL = _config.Value.BaseURL;
            TblEvcregistration? TblEvcregistrations = new TblEvcregistration();
            try
            {
                string? draw = Request.Form["draw"].FirstOrDefault();
                string? start = Request.Form["start"].FirstOrDefault();
                string? length = Request.Form["length"].FirstOrDefault();
                string? sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
                string? sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();
                string? searchValue = Request.Form["search[value]"].FirstOrDefault();
                int pageSize = length != null ? Convert.ToInt32(length) : 0;
                int skip = start != null ? Convert.ToInt32(start) : 0;
                TblEvcregistrations = _context.TblEvcregistrations.Where(x => x.IsActive == true && x.UserId == loggingId && x.Isevcapprovrd == true).FirstOrDefault();
                if (TblEvcregistrations != null)
                {
                    IQueryable<TblOrderLgc> query = _context.TblOrderLgcs
                        .Include(x => x.Logistic).ThenInclude(x => x.ServicePartner)
                             .Include(x => x.Evcregistration).ThenInclude(x => x.City)
                             .Include(x => x.Evcpartner).ThenInclude(x => x.City)
                             .Include(x => x.Logistic).ThenInclude(x => x.Status)
                                        .Include(x => x.OrderTrans).ThenInclude(x => x.Exchange).ThenInclude(x => x.ProductType).ThenInclude(x => x.ProductCat)
                                        .Include(x => x.OrderTrans).ThenInclude(x => x.Exchange).ThenInclude(x => x.CustomerDetails)
                                        //Changes for ABB Redemption by Vk 
                                        .Include(x => x.OrderTrans).ThenInclude(x => x.Abbredemption).ThenInclude(x => x.Abbregistration).ThenInclude(x => x.NewProductCategory)
                                        .Include(x => x.OrderTrans).ThenInclude(x => x.Abbredemption).ThenInclude(x => x.Abbregistration).ThenInclude(x => x.NewProductCategoryTypeNavigation)
                                        .Include(x => x.OrderTrans).ThenInclude(x => x.Abbredemption).ThenInclude(x => x.CustomerDetails)
      .Where(x => x.IsActive == true && x.ActualPickupDate != null && x.CustDeclartionpdfname != null && x.EvcregistrationId == TblEvcregistrations.EvcregistrationId);


                    if (startDate != null && endDate != null)
                    {
                        startDate = startDate.Value.AddMinutes(-1);
                        endDate = endDate.Value.AddDays(1).AddSeconds(-1);
                        query = query.Where(x => x.ActualPickupDate >= startDate && x.ActualPickupDate <= endDate);
                    }

                    if (evcPartnerId != null && evcPartnerId > 0)
                    {
                        // Placeholder: Add filter for productCatId
                        query = query.Where(x => x.EvcpartnerId == evcPartnerId);
                    }
                    int recordsTotal = query.Count();

                    query = query.OrderByDescending(x => x.ActualPickupDate)
                       .Skip(skip)
                       .Take(pageSize);

                    List<TblOrderLgc> tblOrderLgcs = query.ToList();
                    foreach (var item in tblOrderLgcs)
                    {
                        BulkDownlodeCDPdfviewmodel podVM = new BulkDownlodeCDPdfviewmodel();
                        string actionURLCD = string.Empty;
                        string? productTypeDesc = null;
                        string? productCatDesc = null;
                        string? custCity = null;
                        string? custName = null;

                        actionURLCD = "<td class='actions'>";
                        actionURLCD = actionURLCD + "<a class='text-link-primary' target='_blank' href='" + URL + EnumHelper.DescriptionAttr(FileAddressEnum.CustomerDeclaration) + item.CustDeclartionpdfname + "'>" + item.CustDeclartionpdfname + "</a>";
                        actionURLCD = actionURLCD + " </td>";
                        if (item != null)
                        {
                            podVM = new BulkDownlodeCDPdfviewmodel();
                            #region Exchange and ABB common Configuraion Add by VK
                            if (item.OrderTrans.Exchange != null)
                            {
                                if (item.OrderTrans.Exchange.ProductType != null)
                                {
                                    productTypeDesc = item.OrderTrans?.Exchange?.ProductType?.Description;
                                    if (item.OrderTrans?.Exchange?.ProductType?.ProductCat != null)
                                    {
                                        productCatDesc = item.OrderTrans?.Exchange?.ProductType.ProductCat.Description;
                                    }
                                }
                                if (item.OrderTrans.Exchange.CustomerDetails != null)
                                {
                                    custCity = item.OrderTrans.Exchange.CustomerDetails.City;
                                    custName = item.OrderTrans.Exchange.CustomerDetails.FirstName + " " + item.OrderTrans.Exchange.CustomerDetails.LastName;
                                }
                            }
                            else if (item.OrderTrans.Abbredemption != null && item.OrderTrans.Abbredemption.Abbregistration != null)
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
                                    custCity = item.OrderTrans.Abbredemption.CustomerDetails.City;
                                    custName = item.OrderTrans.Abbredemption.CustomerDetails.FirstName + " " + item.OrderTrans.Abbredemption.CustomerDetails.LastName;
                                }
                            }
                            #endregion
                            podVM.Id = item.OrderLgcid;
                            podVM.RegdNo = item.OrderTrans.RegdNo;
                            podVM.CDPdfName = actionURLCD;
                            podVM.ProductCatName = productCatDesc;
                            podVM.CustName = custName;
                            podVM.CDDate = Convert.ToDateTime(item.ActualPickupDate).ToString("dd/MM/yyyy");
                            if (item.Evcregistration != null)
                            {
                                podVM.EVCBussinessName = item.Evcregistration.EvcregdNo + " - " + item.Evcregistration.BussinessName;
                                if (item.Evcpartner != null && item.Evcpartner.City != null)
                                {
                                    podVM.EVCPartnerCode = item.Evcpartner?.EvcStoreCode;
                                    podVM.EVCCity = item.Evcpartner?.City.Name;
                                }
                            }
                            BulkDownlodeCDPdfviewmodelList.Add(podVM);
                        }
                    }
                    var data = BulkDownlodeCDPdfviewmodelList;
                    // data = (List<BulkDownlodePODPdfviewmodel>)BulkDownlodePODPdfviewmodelList.OrderByDescending(x => x.PODDate);
                    var jsonData = new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data };
                    return Ok(jsonData);

                }
                return Ok();
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        #endregion

        #region ADDED BY Priyanshi Sahu---- Get EVC Pending Credit Approval List //updated Iqueryable 
        public async Task<ActionResult> GetEVCPendingCreditApprovalList(int companyId, DateTime? orderStartDate,
            DateTime? orderEndDate, string? companyName, int? productCatId,
            int? productTypeId, string? regdNo, string? phoneNo, string? custCity, string? evcregdNo)
        {
            #region Variable declaration
            if (!string.IsNullOrWhiteSpace(companyName) && companyName != "null")
            { companyName = companyName.Trim().ToLower(); }
            else { companyName = null; }
            if (!string.IsNullOrWhiteSpace(regdNo) && regdNo != "null")
            { regdNo = regdNo.Trim().ToLower(); }
            else { regdNo = null; }        
            if (!string.IsNullOrWhiteSpace(phoneNo) && phoneNo != "null")
            { phoneNo = phoneNo.Trim().ToLower(); }
            else { phoneNo = null; }
            if (!string.IsNullOrWhiteSpace(custCity) && custCity != "null")
            { custCity = custCity.Trim().ToLower(); }
            else { custCity = null; }


            string URL = _config.Value.URLPrefixforProd;
            string MVCURL = _config.Value.MVCBaseURLForExchangeInvoice;
            string InvoiceimagURL = string.Empty;
            TblCompany tblCompany = null;
            TblBusinessUnit tblBusinessUnit = null;
            TblEvcPartner? tblEvcPartner = null;
            #endregion
            try
            {
                var draw = Request.Form["draw"].FirstOrDefault();
                var start = Request.Form["start"].FirstOrDefault();
                var length = Request.Form["length"].FirstOrDefault();
                var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
                var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();
                var searchValue = Request.Form["search[value]"].FirstOrDefault();
                int pageSize = length != null ? Convert.ToInt32(length) : 0;
                int skip = start != null ? Convert.ToInt32(start) : 0;

                #region Advanced Filters Mapping
                if (companyId > 0 && companyId != 1007)
                {
                    tblCompany = _companyRepository.GetCompanyId(companyId);
                    if (tblCompany != null)
                    {
                        tblBusinessUnit = _businessUnitRepository.Getbyid(tblCompany.BusinessUnitId);
                    }
                }
                #endregion
                IQueryable<TblCreditRequest> query = _context.TblCreditRequests
                   .Include(x => x.WalletTransaction)
                   .Include(x => x.CreditRequestUser)
                   .Include(x => x.WalletTransaction).ThenInclude(x => x.Evcregistration)
                   .Include(x => x.WalletTransaction).ThenInclude(x => x.Evcpartner)
                   .Include(x => x.WalletTransaction).ThenInclude(x => x.OrderTrans).ThenInclude(x => x.Exchange).ThenInclude(x => x.ProductType).ThenInclude(x => x.ProductCat)
                   .Include(x => x.WalletTransaction).ThenInclude(x => x.OrderTrans).ThenInclude(x => x.Exchange).ThenInclude(x => x.CustomerDetails)
                   .Include(x => x.WalletTransaction).ThenInclude(x => x.OrderTrans).ThenInclude(x => x.Abbredemption).ThenInclude(x => x.Abbregistration).ThenInclude(x => x.NewProductCategory)
                   .Include(x => x.WalletTransaction).ThenInclude(x => x.OrderTrans).ThenInclude(x => x.Abbredemption).ThenInclude(x => x.Abbregistration).ThenInclude(x => x.NewProductCategoryTypeNavigation)
                   .Include(x => x.WalletTransaction).ThenInclude(x => x.OrderTrans).ThenInclude(x => x.Abbredemption).ThenInclude(x => x.CustomerDetails)
                   .Include(x => x.WalletTransaction).ThenInclude(x => x.OrderTrans).ThenInclude(x => x.Abbredemption).ThenInclude(x => x.Abbregistration).ThenInclude(x => x.BusinessUnit)
                   .Where(x => x.IsActive == true && x.IsCreditRequest == true && x.IsCreditRequestApproved == false && x.WalletTransaction.IsActive == true
                   && x.WalletTransaction.ModifiedDate != null && x.WalletTransaction.OrderofAssignDate != null);

                if (orderStartDate != null && orderEndDate != null)
                {
                    orderStartDate = orderStartDate.Value.AddMinutes(-1);
                    orderEndDate = orderEndDate.Value.AddDays(1).AddSeconds(-1);
                    query = query.Where(x => x.CreditRequestUser.CreatedDate >= orderStartDate && x.CreditRequestUser.CreatedDate <= orderEndDate);
                }
                // Advanced Filters Mapping
                if (tblBusinessUnit != null)
                {
                    // Placeholder: Add filter for companyId
                    query = query.Where(x => x.WalletTransaction.OrderTrans.Exchange != null ? (x.WalletTransaction.OrderTrans.Exchange.BusinessPartner.BusinessUnit.BusinessUnitId == tblBusinessUnit.BusinessUnitId) : (x.WalletTransaction.OrderTrans.Abbredemption.Abbregistration.BusinessUnitId == tblBusinessUnit.BusinessUnitId));
                }
                if (productCatId.HasValue)
                {
                    // Placeholder: Add filter for productCatId
                    query = query.Where(x => x.WalletTransaction.OrderTrans.Exchange != null ? (x.WalletTransaction.OrderTrans.Exchange.ProductType.ProductCat.Id == productCatId) : (x.WalletTransaction.OrderTrans.Abbredemption.Abbregistration.NewProductCategoryId == productCatId));
                }

                if (productTypeId.HasValue)
                {
                    // Placeholder: Add filter for productTypeId
                    query = query.Where(x => x.WalletTransaction.OrderTrans.Exchange != null ? (x.WalletTransaction.OrderTrans.Exchange.ProductType.Id == productTypeId) : (x.WalletTransaction.OrderTrans.Abbredemption.Abbregistration.NewProductCategoryTypeId == productTypeId));
                }

                if (!string.IsNullOrEmpty(regdNo))
                {
                    // Placeholder: Add filter for regdNo
                    query = query.Where(x => x.WalletTransaction.RegdNo == regdNo);
                }

                if (!string.IsNullOrEmpty(phoneNo))
                {
                    // Placeholder: Add filter for phoneNo
                    query = query.Where(x => x.WalletTransaction.OrderTrans.Exchange != null ? (x.WalletTransaction.OrderTrans.Exchange.CustomerDetails.PhoneNumber == phoneNo) : (x.WalletTransaction.OrderTrans.Abbredemption.CustomerDetails.PhoneNumber == phoneNo));
                }

                if (!string.IsNullOrEmpty(custCity))
                {
                    // Placeholder: Add filter for custCity
                    query = query.Where(x => x.WalletTransaction.OrderTrans.Exchange != null ? (x.WalletTransaction.OrderTrans.Exchange.CustomerDetails.City == custCity) : (x.WalletTransaction.OrderTrans.Abbredemption.CustomerDetails.City == custCity));
                }
                if (!string.IsNullOrEmpty(companyName))
                {
                    // Placeholder: Add filter for productTypeId
                    query = query.Where(x => x.WalletTransaction.OrderTrans.Exchange != null ? (x.WalletTransaction.OrderTrans.Exchange.BusinessPartner.BusinessUnit.Name == companyName) : (x.WalletTransaction.OrderTrans.Abbredemption.Abbregistration.BusinessUnit.Name == companyName));
                }
                if (!string.IsNullOrEmpty(evcregdNo))
                {
                    // Placeholder: Add filter for regdNo 
                    query = query.Where(x => x.WalletTransaction.Evcregistration.EvcregdNo == evcregdNo);
                }

                if (!string.IsNullOrEmpty(phoneNo))
                {
                    // Placeholder: Add filter for phoneNo
                    query = query.Where(x => x.WalletTransaction.OrderTrans.Exchange != null ? (x.WalletTransaction.OrderTrans.Exchange.CustomerDetails.PhoneNumber == phoneNo) : (x.WalletTransaction.OrderTrans.Abbredemption.CustomerDetails.PhoneNumber == phoneNo));
                }

                int recordsTotal = query.Count();


                query = query.OrderByDescending(x => x.CreditRequestUser.CreatedBy)
                   .Skip(skip)
                   .Take(pageSize);

                List<TblCreditRequest> tblCreditRequests = query.ToList();

                List<PendingCreditApprovalViewModel> pendingCreditApprovalViewModel = _mapper.Map<List<TblCreditRequest>, List<PendingCreditApprovalViewModel>>(tblCreditRequests);

                foreach (PendingCreditApprovalViewModel oPendingCreditApprovalViewModel in pendingCreditApprovalViewModel)
                {
                    
                    TblCreditRequest orderDetails = tblCreditRequests.FirstOrDefault(x => x.WalletTransactionId == oPendingCreditApprovalViewModel.WalletTransactionId);
                    if (orderDetails != null)
                    {
                        oPendingCreditApprovalViewModel.CreditRequestId = orderDetails.CreditRequestId;

                        oPendingCreditApprovalViewModel.OrderTransId = orderDetails?.WalletTransaction?.OrderTransId;
                        oPendingCreditApprovalViewModel.EvcRate = orderDetails?.WalletTransaction?.OrderAmount > 0 ? orderDetails.WalletTransaction.OrderAmount : 0;
                        oPendingCreditApprovalViewModel.RegdNo = orderDetails.WalletTransaction.OrderTrans.RegdNo ?? string.Empty;
                        oPendingCreditApprovalViewModel.EvcRegNo = orderDetails.WalletTransaction.Evcregistration != null ? $"{orderDetails.WalletTransaction.Evcregistration.EvcregdNo}-{orderDetails.WalletTransaction.Evcregistration.BussinessName}" : string.Empty;
                        oPendingCreditApprovalViewModel.RequestDate = orderDetails.CreatedDate.Value.ToShortDateString() ?? string.Empty;
                        oPendingCreditApprovalViewModel.GenrateRequestBy = orderDetails.CreditRequestUser.FirstName ?? string.Empty;
                        oPendingCreditApprovalViewModel.FinalExchangePrice = orderDetails.WalletTransaction.OrderTrans.FinalPriceAfterQc ?? 0;
                        oPendingCreditApprovalViewModel.StatusCode = orderDetails.WalletTransaction.StatusId ?? string.Empty;
                        oPendingCreditApprovalViewModel.CreatedDate = orderDetails.CreatedDate;
                        if (orderDetails.WalletTransaction.OrderType != null && (orderDetails.WalletTransaction.OrderType == Convert.ToInt32(OrderTypeEnum.Exchange).ToString()))
                        {
                            oPendingCreditApprovalViewModel.ExchProdGroup = orderDetails.WalletTransaction.OrderTrans.Exchange.ProductType.Description ?? string.Empty;
                            oPendingCreditApprovalViewModel.OldProdType = orderDetails.WalletTransaction.OrderTrans.Exchange.ProductType.ProductCat.Description ?? string.Empty;
                            oPendingCreditApprovalViewModel.FirstName = orderDetails.WalletTransaction.OrderTrans.Exchange.CustomerDetails.FirstName + " " + orderDetails.WalletTransaction.OrderTrans.Exchange.CustomerDetails.LastName ?? string.Empty;
                            oPendingCreditApprovalViewModel.CustCity = orderDetails.WalletTransaction.OrderTrans.Exchange.CustomerDetails.City ?? string.Empty;
                        }
                        if (orderDetails.WalletTransaction.OrderType != null && (orderDetails.WalletTransaction.OrderType == Convert.ToInt32(OrderTypeEnum.ABB).ToString()))
                        {
                            oPendingCreditApprovalViewModel.ExchProdGroup = orderDetails.WalletTransaction.OrderTrans.Abbredemption.Abbregistration.NewProductCategory.Description ?? string.Empty;
                            oPendingCreditApprovalViewModel.OldProdType = orderDetails.WalletTransaction.OrderTrans.Abbredemption.Abbregistration.NewProductCategoryTypeNavigation.Description ?? string.Empty;
                            oPendingCreditApprovalViewModel.FirstName = orderDetails.WalletTransaction.OrderTrans.Abbredemption.CustomerDetails.FirstName + " " + orderDetails.WalletTransaction.OrderTrans.Abbredemption.CustomerDetails.LastName ?? string.Empty;
                            oPendingCreditApprovalViewModel.CustCity = orderDetails.WalletTransaction.OrderTrans.Abbredemption.CustomerDetails.City ?? string.Empty;
                        }

                        //clear balance calculation

                        EVCClearBalanceViewModel? eVCClearBalanceViewModel = new EVCClearBalanceViewModel();
                        eVCClearBalanceViewModel = _commonManager.CalculateEVCClearBalance((int)(orderDetails?.WalletTransaction?.EvcregistrationId));
                        if (eVCClearBalanceViewModel != null)
                        {
                            oPendingCreditApprovalViewModel.clearBalance = (decimal)eVCClearBalanceViewModel.clearBalance;
                            oPendingCreditApprovalViewModel.EvcWalletAmount = eVCClearBalanceViewModel.walletAmount;
                        }
                        string actionURL = $" <td class='actions'>";
                        actionURL += $"<a class='btn btn-sm btn-primary' href='{URL}/Index1?orderTransId={oPendingCreditApprovalViewModel.OrderTransId}' title='TimeLine'><i class='fa solid fa-clock-rotate-left'></i></a> &nbsp;";
                        actionURL += " </td>";
                        oPendingCreditApprovalViewModel.Action = actionURL;
                        string actionURL1 = "<button onclick='ApproveCreditRequest(" + oPendingCreditApprovalViewModel.CreditRequestId + ")' class='btn btn-sm btn-primary'><i class='fa-solid fa-repeat'></i>&nbsp; Credit Approve </button>";
                        oPendingCreditApprovalViewModel.Edit = actionURL1;

                    }
                }

                pendingCreditApprovalViewModel = pendingCreditApprovalViewModel.OrderByDescending(x => x.CreatedDate).ToList();

                var data = pendingCreditApprovalViewModel;
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
