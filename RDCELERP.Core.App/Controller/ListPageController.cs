using AutoMapper;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using NPOI.HSSF.Record.Chart;
using NPOI.SS.Formula.Functions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using RDCELERP.BAL.Enum;
using RDCELERP.BAL.Interface;
using RDCELERP.Common.Constant;
using RDCELERP.Common.Enums;
using RDCELERP.Common.Helper;
using RDCELERP.DAL.Entities;
using RDCELERP.DAL.IRepository;
using RDCELERP.Model.ABBPlanMaster;
using RDCELERP.Model.ABBPriceMaster;
using RDCELERP.Model.AbbRegistration;
using RDCELERP.Model.Base;
using RDCELERP.Model.BusinessPartner;
using RDCELERP.Model.BusinessUnit;
using RDCELERP.Model.City;
using RDCELERP.Model.Company;
using RDCELERP.Model.DealerDashBoard;
using RDCELERP.Model.EVC;
using RDCELERP.Model.ExchangeOrder;
using RDCELERP.Model.ImagLabel;
using RDCELERP.Model.LGC;
using RDCELERP.Model.Master;
using RDCELERP.Model.ModelNumber;
using RDCELERP.Model.PinCode;
using RDCELERP.Model.PriceMaster;
using RDCELERP.Model.Product;
using RDCELERP.Model.ProductConditionLabel;
using RDCELERP.Model.ProductQuality;
using RDCELERP.Model.ProductTechnology;
using RDCELERP.Model.Program;
using RDCELERP.Model.QCComment;
using RDCELERP.Model.QuestionerLOV;
using RDCELERP.Model.Role;
using RDCELERP.Model.SearchFilters;
using RDCELERP.Model.ServicePartner;
using RDCELERP.Model.State;
using RDCELERP.Model.StoreCode;
using RDCELERP.Model.TimeLine;
using RDCELERP.Model.UniversalPriceMaster;
using RDCELERP.Model.Users;
using RDCELERP.Model.VehicleIncentive;

namespace RDCELERP.Core.App.Controller
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ListPageController : ControllerBase
    {
        private Digi2l_DevContext _context;
        private IMapper _mapper;
        private CustomDataProtection _protector;
        private IOptions<ApplicationSettings> _config;
        private LoginViewModel _loginSession;
        public IDealerManager _dealerManager;
        public ILogging _logging;
        IBusinessPartnerRepository _bussinesPartnerRepository;
        IExchangeOrderRepository _exchangeOrderRepository;
        IBusinessUnitRepository _businessUnitRepository;
        IVoucherRepository _voucherRepository;

        public ListPageController(IMapper mapper, Digi2l_DevContext context, CustomDataProtection protector, IOptions<ApplicationSettings> config, IDealerManager dealerManager, ILogging logging, IBusinessPartnerRepository bussinesPartnerRepository, IExchangeOrderRepository exchangeOrderRepository, IBusinessUnitRepository businessUnitRepository, IVoucherRepository voucherRepository)
        {
            _context = context;
            _mapper = mapper;
            _protector = protector;
            _config = config;
            _dealerManager = dealerManager;
            _logging = logging;
            _bussinesPartnerRepository = bussinesPartnerRepository;
            _exchangeOrderRepository = exchangeOrderRepository;
            _businessUnitRepository = businessUnitRepository;
            _voucherRepository = voucherRepository;
        }

        [HttpPost]
        public async Task<ActionResult> GetBrandList(DateTime? startDate, DateTime? endDate)
        {
            string URL = _config.Value.URLPrefixforProd;

            List<TblBrand> tblBrands = null;
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
                if (startDate == null && endDate == null)
                {
                    tblBrands = await _context.TblBrands.Where(x =>
                        (string.IsNullOrEmpty(searchValue) || x.Name.ToLower().Contains(searchValue.ToLower().Trim()))).ToListAsync();
                }
                else
                {
                    startDate = Convert.ToDateTime(startDate).AddMinutes(-1);
                    endDate = Convert.ToDateTime(endDate).AddDays(1).AddSeconds(-1);
                    tblBrands = await _context.TblBrands.Where(x=> x.CreatedDate >= startDate && x.CreatedDate <= endDate
                       && (string.IsNullOrEmpty(searchValue) || x.Name.ToLower().Contains(searchValue.ToLower().Trim()))).ToListAsync();
                }

                recordsTotal = tblBrands != null ? tblBrands.Count : 0;
                if (tblBrands != null)
                {
                    tblBrands = sortColumnDirection.Equals(SortingOrder.ASCENDING) ? tblBrands.OrderBy(o => o.GetType().GetProperty(sortColumn).GetValue(o, null)).ToList() : tblBrands.OrderByDescending(o => o.GetType().GetProperty(sortColumn).GetValue(o, null)).ToList();
                    tblBrands = tblBrands.Skip(skip).Take(pageSize).ToList();
                }
                else
                    tblBrands = new List<TblBrand>();

                List<BrandViewModel> brandList = _mapper.Map<List<TblBrand>, List<BrandViewModel>>(tblBrands);
                string actionURL = string.Empty;
                string ImageURL = string.Empty;

                foreach (BrandViewModel item in brandList)
                {

                    if (item.IsActive == true)
                    {
                        actionURL = " <div class='actionbtns'>";
                        actionURL = actionURL + "<a href=' " + URL + "/Brand/Manage?id=" + _protector.Encode(item.Id) + "' data-bs-toggle='tooltip' data-bs-placement='top' title='Edit' class='btn btn-sm btn-primary'><i class='fa-solid fa-pen'></i></a> " +
                            "<a href='javascript: void(0)' onclick='deleteConfirmBrand(" + item.Id + ")' class='btn btn-sm btn-danger' data-bs-toggle='tooltip' data-bs-placement='top' title='Inactive'><i class='fa-solid fa-trash'></i></a>";
                        actionURL = actionURL + " </div>";
                        item.Action = actionURL;

                        ImageURL = "<img src='" + URL + "/DBFiles/Brands/" + item.BrandLogoUrl + "' class='brandlistimg' />";
                        item.BrandLogoUrl = ImageURL;
                    }
                    else
                    {
                        actionURL = " <div class='actionbtns'>";
                        actionURL = actionURL + "<a href=' " + URL + "/Brand/Manage?id=" + _protector.Encode(item.Id) + "' data-bs-toggle='tooltip' data-bs-placement='top' title='Edit' class='btn btn-sm btn-primary'><i class='fa-solid fa-pen'></i></a> " +
                            "<a href='" + "javascript:void(0)' onclick='activeConfirmBrand(" + item.Id + ")' class='btn btn-sm btn-success' data-bs-toggle='tooltip' data-bs-placement='top' title='Active'><i class='fa-solid fa-circle-check text-white'></i></a>";
                        actionURL = actionURL + " </div>";
                        item.Action = actionURL;

                        ImageURL = "<img src='" + URL + "/DBFiles/Brands/" + item.BrandLogoUrl + "' class='brandlistimg' />";
                        item.BrandLogoUrl = ImageURL;

                    }

                    TblBrand BrandDetails = tblBrands.FirstOrDefault(x => x.Id == item.Id);

                    if (BrandDetails.CreatedDate != null && BrandDetails.CreatedDate != null)
                    {
                        //var Date = (DateTime)item.CreatedDate;
                        //item.Date = Date.ToShortDateString();


                        DateTime dateTime = (DateTime)item.CreatedDate;
                        item.Date = dateTime.ToString("yyyy-MM-dd");
                        //item.CreatedDate = string.Format("{0}-{1}-{2}",);

                    }

                }

                var data = brandList;
                var jsonData = new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data };
                return Ok(jsonData);
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        [HttpPost]
        public async Task<ActionResult> GetBrandDeletedList(DateTime? startDate, DateTime? endDate)
        {
            string URL = _config.Value.URLPrefixforProd;

            List<TblBrand> tblBrands = null;
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
                if (startDate == null && endDate == null)
                {
                    tblBrands = await _context.TblBrands.Where(x => x.IsActive == false
                       && (string.IsNullOrEmpty(searchValue) || x.Name.ToLower().Contains(searchValue.ToLower().Trim()))).ToListAsync();
                }
                else
                {
                    startDate = Convert.ToDateTime(startDate).AddMinutes(-1);
                    endDate = Convert.ToDateTime(endDate).AddDays(1).AddSeconds(-1);
                    tblBrands = await _context.TblBrands.Where(x => x.IsActive == false && x.CreatedDate >= startDate && x.CreatedDate <= endDate
                       && (string.IsNullOrEmpty(searchValue) || x.Name.ToLower().Contains(searchValue.ToLower().Trim()))).ToListAsync();
                }

                recordsTotal = tblBrands != null ? tblBrands.Count : 0;
                if (tblBrands != null)
                {
                    tblBrands = sortColumnDirection.Equals(SortingOrder.ASCENDING) ? tblBrands.OrderBy(o => o.GetType().GetProperty(sortColumn).GetValue(o, null)).ToList() : tblBrands.OrderByDescending(o => o.GetType().GetProperty(sortColumn).GetValue(o, null)).ToList();
                    tblBrands = tblBrands.Skip(skip).Take(pageSize).ToList();
                }
                else
                    tblBrands = new List<TblBrand>();

                List<BrandViewModel> brandList = _mapper.Map<List<TblBrand>, List<BrandViewModel>>(tblBrands);
                string actionURL = string.Empty;
                string ImageURL = string.Empty;

                foreach (BrandViewModel item in brandList)
                {
                    if (item.IsActive == true)
                    {
                        actionURL = " <div class='actionbtns'>";
                        actionURL = actionURL + "<a href=' " + URL + "/Brand/Manage?id=" + _protector.Encode(item.Id) + "' data-bs-toggle='tooltip' data-bs-placement='top' title='Edit' class='btn btn-sm btn-primary'><i class='fa-solid fa-pen'></i></a> " +
                            "<a href='" + "javascript:void(0)' onclick='deleteConfirmBrand(" + item.Id + ")' class='btn btn-sm btn-success' data-bs-toggle='tooltip' data-bs-placement='top' title='Active'><i class='fa-solid fa-circle-check text-white'></i></a>";
                        actionURL = actionURL + " </div>";
                        item.Action = actionURL;

                        ImageURL = "<img src='" + URL + "/DBFiles/Brands/" + item.BrandLogoUrl + "' class='brandlistimg' />";
                        item.BrandLogoUrl = ImageURL;
                    }
                    else
                    {
                        actionURL = " <div class='actionbtns'>";
                        actionURL = actionURL + "<a href=' " + URL + "/Brand/Manage?id=" + _protector.Encode(item.Id) + "' data-bs-toggle='tooltip' data-bs-placement='top' title='Edit' class='btn btn-sm btn-primary'><i class='fa-solid fa-pen'></i></a> " +
                            "<a href='" + "javascript:void(0)' onclick='activeConfirmBrand(" + item.Id + ")' class='btn btn-sm  btn-success' data-bs-toggle='tooltip' data-bs-placement='top' title='Active'><i class='fa-solid fa-recycle'></i></a>";
                        actionURL = actionURL + " </div>";
                        item.Action = actionURL;

                        ImageURL = "<img src='" + URL + "/DBFiles/Brands/" + item.BrandLogoUrl + "' class='brandlistimg' />";
                        item.BrandLogoUrl = ImageURL;

                    }

                    

                    TblBrand BrandDetails = tblBrands.FirstOrDefault(x => x.Id == item.Id);

                    if (BrandDetails.CreatedDate != null && BrandDetails.CreatedDate != null)
                    {
                        //var Date = (DateTime)item.CreatedDate;
                        //item.Date = Date.ToShortDateString();


                        DateTime dateTime = (DateTime)item.CreatedDate;
                        item.Date = dateTime.ToString("yyyy-MM-dd");
                        item.CreatedDate = BrandDetails.CreatedDate;

                    }

                }

                var data = brandList;
                var jsonData = new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data };
                return Ok(jsonData);
            }
            catch (Exception ex)
            {
                throw;
            }

        }
        [HttpPost]
        public async Task<ActionResult> GetUsersList(int roleId, DateTime? startDate, DateTime? endDate, string? FName, string? LName, string? Email, string? PhoneNo, string? role, string? Company)
        {
            string URL = _config.Value.URLPrefixforProd;
            if (string.IsNullOrEmpty(FName) || FName == "null")
            { FName = "".Trim(); }
            else
            { FName = FName.Trim(); }
            if (string.IsNullOrEmpty(LName) || LName == "null")
            { LName = "".Trim().ToLower(); }
            else
            { LName = LName.Trim().ToLower(); }
            if (string.IsNullOrEmpty(Email) || Email == "null")
            { Email = "".Trim().ToLower(); }
            else
            { Email = Email.Trim().ToLower(); }
            if (string.IsNullOrEmpty(PhoneNo) || PhoneNo == "null")
            { PhoneNo = "".Trim().ToLower(); }
            else
            { PhoneNo = PhoneNo.Trim().ToLower(); }
            if (string.IsNullOrEmpty(role) || role == "null")
            { role = "".Trim().ToLower(); }
            else
            { role = role.Trim().ToLower(); }
            if (string.IsNullOrEmpty(Company) || Company == "null")
            { Company = "".Trim().ToLower(); }
            else
            { Company = Company.Trim().ToLower(); }
            

            List<TblUser> TblUsers = null;
            List<TblUserRole> tblUserRoles = null;
            List<UserRolesView> UserList = null;
            UserRolesView UserListViewModel = null;

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
                int count = 0;
                

                if (startDate == null && endDate == null)
                {
                    if (roleId == Convert.ToInt32(RoleEnum.SuperAdmin))
                    {

                        // for super admin

                        count = _context.TblUserRoles
                        .Include(t => t.Role)
                        .Include(t => t.Company)
                        .Include(t => t.ModifiedByNavigation)
                        .Include(t => t.User).Count(x => x.RoleId != 1
                        && ((string.IsNullOrEmpty(FName)) || x.User.FirstName.ToLower() == FName)
                              && ((string.IsNullOrEmpty(LName)) || x.User.LastName == LName)
                               && ((string.IsNullOrEmpty(Email)) || x.User.Email == SecurityHelper.EncryptString(Email, _config.Value.SecurityKey))
                          && ((string.IsNullOrEmpty(PhoneNo)) || x.User.Phone == SecurityHelper.EncryptString(PhoneNo, _config.Value.SecurityKey))
                              && ((string.IsNullOrEmpty(role)) || x.Role.RoleName == role)
                              && ((string.IsNullOrEmpty(Company)) || x.Company.CompanyName == Company));
                        

                        if (count > 0)
                        {
                            tblUserRoles = await _context.TblUserRoles
                           .Include(t => t.Role)
                           .Include(t => t.Company)
                           .Include(t => t.ModifiedByNavigation)
                           .Include(t => t.User).Where(x => x.RoleId != 1
                          && ((string.IsNullOrEmpty(FName)) || x.User.FirstName.ToLower() == FName)
                          && ((string.IsNullOrEmpty(LName)) || x.User.LastName == LName)
                          && ((string.IsNullOrEmpty(Email)) || x.User.Email == SecurityHelper.EncryptString(Email, _config.Value.SecurityKey))
                          && ((string.IsNullOrEmpty(PhoneNo)) || x.User.Phone == SecurityHelper.EncryptString(PhoneNo, _config.Value.SecurityKey))
                          && ((string.IsNullOrEmpty(role)) || x.Role.RoleName == role)
                          && ((string.IsNullOrEmpty(Company)) || x.Company.CompanyName == Company)).OrderByDescending(x => x.UserId).Skip(skip).Take(pageSize).ToListAsync();
                        }
                        else
                        {
                            // for other role

                            count = _context.TblUserRoles
                        .Include(t => t.Role)
                        .Include(t => t.Company)
                        .Include(t => t.ModifiedByNavigation)
                        .Include(t => t.User).Where(x => x.IsActive == true || x.IsActive == false
                               && ((string.IsNullOrEmpty(FName)) || x.User.FirstName.ToLower() == FName)
                              && ((string.IsNullOrEmpty(LName)) || x.User.LastName == LName)
                               && ((string.IsNullOrEmpty(Email)) || x.User.Email == SecurityHelper.EncryptString(Email, _config.Value.SecurityKey))
                          && ((string.IsNullOrEmpty(PhoneNo)) || x.User.Phone == SecurityHelper.EncryptString(PhoneNo, _config.Value.SecurityKey))
                              && ((string.IsNullOrEmpty(role)) || x.Role.RoleName == role)
                              && ((string.IsNullOrEmpty(Company)) || x.Company.CompanyName == Company)).Count();

                            if (count > 0)
                            {
                                tblUserRoles = await _context.TblUserRoles
                               .Include(t => t.Role)
                               .Include(t => t.Company)
                               .Include(t => t.ModifiedByNavigation)
                               .Include(t => t.User).Where(x=>x.IsActive == true || x.IsActive == false 
                               && ((string.IsNullOrEmpty(FName)) || x.User.FirstName.ToLower() == FName)
                              && ((string.IsNullOrEmpty(LName)) || x.User.LastName == LName)
                               && ((string.IsNullOrEmpty(Email)) || x.User.Email == SecurityHelper.EncryptString(Email, _config.Value.SecurityKey))
                          && ((string.IsNullOrEmpty(PhoneNo)) || x.User.Phone == SecurityHelper.EncryptString(PhoneNo, _config.Value.SecurityKey))
                              && ((string.IsNullOrEmpty(role)) ||   x.Role.RoleName == role)
                              && ((string.IsNullOrEmpty(Company)) ||   x.Company.CompanyName == Company)).OrderByDescending(x => x.UserId).Skip(skip).Take(pageSize).ToListAsync();
                            }


                        }
                    }
                }
                else
                {
                    startDate = Convert.ToDateTime(startDate).AddMinutes(-1);
                    endDate = Convert.ToDateTime(endDate).AddDays(1).AddSeconds(-1);

                    if (roleId == Convert.ToInt32(RoleEnum.SuperAdmin))
                    {

                        // for super admin

                        count = _context.TblUserRoles
                        .Include(t => t.Role)
                        .Include(t => t.Company)
                        .Include(t => t.ModifiedByNavigation)
                        .Include(t => t.User).Count(x => x.RoleId != 1 && x.CreatedDate >= startDate && x.CreatedDate <= endDate
                         && ((string.IsNullOrEmpty(FName)) || x.User.FirstName.ToLower() == FName)
                              && ((string.IsNullOrEmpty(LName)) || x.User.LastName == LName)
                               && ((string.IsNullOrEmpty(Email)) || x.User.Email == SecurityHelper.EncryptString(Email, _config.Value.SecurityKey))
                          && ((string.IsNullOrEmpty(PhoneNo)) || x.User.Phone == SecurityHelper.EncryptString(PhoneNo, _config.Value.SecurityKey))
                              && ((string.IsNullOrEmpty(role)) || x.Role.RoleName == role)
                              && ((string.IsNullOrEmpty(Company)) || x.Company.CompanyName == Company));


                        if (count > 0)
                        {
                            tblUserRoles = await _context.TblUserRoles
                           .Include(t => t.Role)
                           .Include(t => t.Company)
                           .Include(t => t.ModifiedByNavigation)
                           .Include(t => t.User).Where(x => x.RoleId != 1 && x.CreatedDate >= startDate && x.CreatedDate <= endDate
                          && ((string.IsNullOrEmpty(FName)) || x.User.FirstName.ToLower() == FName)
                          && ((string.IsNullOrEmpty(LName)) || x.User.LastName == LName)
                          && ((string.IsNullOrEmpty(Email)) || x.User.Email == SecurityHelper.EncryptString(Email, _config.Value.SecurityKey))
                          && ((string.IsNullOrEmpty(PhoneNo)) || x.User.Phone == SecurityHelper.EncryptString(PhoneNo, _config.Value.SecurityKey))
                          && ((string.IsNullOrEmpty(role)) || x.Role.RoleName == role)
                          && ((string.IsNullOrEmpty(Company)) || x.Company.CompanyName == Company)).OrderByDescending(x => x.UserId).Skip(skip).Take(pageSize).ToListAsync();

                        }

                    }
                    else
                    {
                        // for other role
                        count = _context.TblUserRoles
                        .Include(t => t.Role)
                        .Include(t => t.Company)
                        .Include(t => t.ModifiedByNavigation)
                        .Include(t => t.User).Count(x => x.CreatedDate >= startDate && x.CreatedDate <= endDate
                             && ((string.IsNullOrEmpty(FName)) || x.User.FirstName.ToLower() == FName)
    && ((string.IsNullOrEmpty(LName)) || x.User.LastName == LName)
     && ((string.IsNullOrEmpty(Email)) || x.User.Email == SecurityHelper.EncryptString(Email, _config.Value.SecurityKey))
&& ((string.IsNullOrEmpty(PhoneNo)) || x.User.Phone == SecurityHelper.EncryptString(PhoneNo, _config.Value.SecurityKey))
    && ((string.IsNullOrEmpty(role)) || x.Role.RoleName == role)
    && ((string.IsNullOrEmpty(Company)) || x.Company.CompanyName == Company));
                       

                        if (count > 0)
                        {
                            tblUserRoles = await _context.TblUserRoles
                           .Include(t => t.Role)
                           .Include(t => t.Company)
                           .Include(t => t.ModifiedByNavigation)
                           .Include(t => t.User).Where(x => x.CreatedDate >= startDate && x.CreatedDate <= endDate
                          && ((string.IsNullOrEmpty(FName)) || x.User.FirstName.ToLower() == FName)
                          && ((string.IsNullOrEmpty(LName)) || x.User.LastName == LName)
                           && ((string.IsNullOrEmpty(Email)) || x.User.Email == SecurityHelper.EncryptString(Email, _config.Value.SecurityKey))
                          && ((string.IsNullOrEmpty(PhoneNo)) || x.User.Phone == SecurityHelper.EncryptString(PhoneNo, _config.Value.SecurityKey))
                          && ((string.IsNullOrEmpty(role)) || x.Role.RoleName == role)
                          && ((string.IsNullOrEmpty(Company)) || x.Company.CompanyName == Company)).OrderByDescending(x => x.UserId).Skip(skip).Take(pageSize).ToListAsync();

                        }
                    }
                }

                recordsTotal = count;
                UserList = new List<UserRolesView>();

                foreach (var item in tblUserRoles)
                {
                    string actionURL = string.Empty;
                    string ImageURL = string.Empty;
                    actionURL = "<div class='actionbtns'>";
                    if(item.IsActive == true)
                    {
                        actionURL = actionURL + "<a href=' " + URL + "/Users/Manage?id=" + _protector.Encode(item.UserId) + "' data-bs-toggle='tooltip' data-bs-placement='top' title='Edit' class='btn btn-sm btn-primary'><i class='fa-solid fa-pen'></i></a> " + "<a href=' " + URL + "/Users/Details?id=" + _protector.Encode(item.UserId) + "' data-bs-toggle='tooltip' data-bs-placement='top' title='View' class='btn btn-sm btn-primary'><i class='fa-solid fa-eye'></i></a> " +
                        "<a href=' " + URL + "/Users/ManageUserRole?id=" + (item.UserRoleId) + "&" + (item.UserId) + "' data-bs-toggle='tooltip' data-bs-placement='top' title=' Role Edit' class='btn btn-sm btn-primary'><i class='fa-solid fa-user'></i></a> " +
                         "<a href=' " + URL + "/Users/EncryptedEmailPass?id=" + _protector.Encode(item.UserId) + "' data-bs-toggle='tooltip' data-bs-placement='top' title='View' class='btn btn-sm btn-primary'><i class='fa-solid fa-lock'></i></a> " +
                        "<a href='javascript: void(0)' onclick='deleteConfirmUser(" + item.UserRoleId + ")' class='btn btn-sm btn-danger' data-bs-toggle='tooltip' data-bs-placement='top' title='Inactive'><i class='fa-solid fa-trash'></i></a>";
                        actionURL = actionURL + " </div>";
                        ImageURL = "<img src='" + URL + "/DBFiles/Users/" + item?.User?.ImageName + "' class='brandlistimg' />";
                    }
                    else
                    {
                        actionURL = actionURL + "<a href=' " + URL + "/Users/Manage?id=" + _protector.Encode(item.UserId) + "' data-bs-toggle='tooltip' data-bs-placement='top' title='Edit' class='btn btn-sm btn-primary'><i class='fa-solid fa-pen'></i></a> " + "<a href=' " + URL + "/Users/Details?id=" + _protector.Encode(item.UserId) + "' data-bs-toggle='tooltip' data-bs-placement='top' title='View' class='btn btn-sm btn-primary'><i class='fa-solid fa-eye'></i></a> " +
                        "<a href=' " + URL + "/Users/ManageUserRole?id=" + (item.UserRoleId) + "&" + (item.UserId) + "' data-bs-toggle='tooltip' data-bs-placement='top' title=' Role Edit' class='btn btn-sm btn-primary'><i class='fa-solid fa-user'></i></a> " +
                         "<a href=' " + URL + "/Users/EncryptedEmailPass?id=" + _protector.Encode(item.UserId) + "' data-bs-toggle='tooltip' data-bs-placement='top' title='View' class='btn btn-sm btn-primary'><i class='fa-solid fa-lock'></i></a> " +
                        "<a href='javascript: void(0)' onclick='activeConfirmUser(" + item.UserRoleId + ")' class='btn btn-sm btn-success' data-bs-toggle='tooltip' data-bs-placement='top' title='Active'><i class='fa-solid fa-circle-check text-white'></i></a>";
                        actionURL = actionURL + " </div>";
                        ImageURL = "<img src='" + URL + "/DBFiles/Users/" + item?.User?.ImageName + "' class='brandlistimg' />";
                    }
                    
                    if (item != null)
                    {
                        UserListViewModel = new UserRolesView();
                        UserListViewModel.Action = actionURL;
                        UserListViewModel.ImageName = ImageURL;
                        UserListViewModel.FirstName = item.User.FirstName;
                        UserListViewModel.LastName = item.User.LastName;
                        if (item.User?.Email != null)
                        {
                            UserListViewModel.Email = SecurityHelper.DecryptString(item.User?.Email, _config.Value.SecurityKey);
                        }
                        if (item.User?.Password != null)
                        {
                            UserListViewModel.Phone = SecurityHelper.DecryptString(item.User?.Phone, _config.Value.SecurityKey);
                        }
                        if (item.Role?.RoleName != null)
                        {
                            UserListViewModel.Role = item.Role?.RoleName;
                        }
                        if (item.User?.CompanyId != null)
                        {

                            UserListViewModel.CompanyName = item.Company?.CompanyName;

                        }


                        UserListViewModel.CreatedDate = item.CreatedDate;
                        if (item.IsActive == true)
                        {
                            UserListViewModel.UserStatus = "Active";
                        }
                        else
                        {
                            UserListViewModel.UserStatus = "Inactive";
                        }
                        //UserListViewModel.AmountPayableThroughLGC = 0;
                        UserList.Add(UserListViewModel);

                        TblUserRole UserDetails = tblUserRoles.FirstOrDefault(x => x.UserRoleId == item.UserRoleId);

                        if (UserDetails.CreatedDate != null && UserDetails.CreatedDate != null)
                        {
                            //var Date = (DateTime)item.CreatedDate;
                            //item.Date = Date.ToShortDateString();


                            DateTime dateTime = (DateTime)item.CreatedDate;
                            //item.Date = dateTime.ToString("yyyy-MM-dd");
                            item.CreatedDate = UserDetails.CreatedDate;

                        }


                    }

                }

                var data = UserList;
                var jsonData = new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data };
                return Ok(jsonData);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [HttpPost]
        public async Task<ActionResult> GetSendEmailtoUserList(int roleId, DateTime? startDate, DateTime? endDate)
        {
            string URL = _config.Value.URLPrefixforProd;
            List<TblUser> TblUsers = null;
            List<TblUserRole> tblUserRoles = null;
            List<UserRolesView> UserList = null;
            UserRolesView UserListViewModel = null;

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
                // roleId = new Digi2l_DevContext();

                if (startDate == null && endDate == null)
                {
                    if (roleId == Convert.ToInt32(RoleEnum.SuperAdmin))
                    {

                        // for super admin

                        tblUserRoles = _context.TblUserRoles
                        .Include(t => t.Role)
                        .Include(t => t.Company)
                        .Include(t => t.ModifiedByNavigation)
                        .Include(t => t.User).Where(x => x.IsActive == true && x.RoleId != 1 && (string.IsNullOrEmpty(searchValue) || x.User.FirstName.ToLower().Contains(searchValue.ToLower().Trim()) || x.User.LastName.ToLower().Contains(searchValue.ToLower().Trim()) || x.Role.RoleName.ToLower().Contains(searchValue.ToLower().Trim()) || x.Company.CompanyName.ToLower().Contains(searchValue.ToLower().Trim()))).OrderByDescending(x => x.UserId).ToList();

                    }
                    else
                    {
                        // for other role

                        tblUserRoles = _context.TblUserRoles
                       .Include(t => t.Role)
                       .Include(t => t.User)
                       .Include(t => t.Company)
                       .Include(t => t.User).Where(x => x.IsActive == true && (string.IsNullOrEmpty(searchValue) || x.User.FirstName.ToLower().Contains(searchValue.ToLower().Trim()) || x.User.LastName.ToLower().Contains(searchValue.ToLower().Trim()) || x.Role.RoleName.ToLower().Contains(searchValue.ToLower().Trim()) || x.Company.CompanyName.ToLower().Contains(searchValue.ToLower().Trim()))).OrderByDescending(x => x.UserId).ToList();

                    }
                }
                else
                {
                    if (roleId == Convert.ToInt32(RoleEnum.SuperAdmin))
                    {

                        // for super admin

                        tblUserRoles = _context.TblUserRoles
                        .Include(t => t.Role)
                        .Include(t => t.Company)
                        .Include(t => t.ModifiedByNavigation)
                        .Include(t => t.User).Where(x => x.IsActive == true && x.RoleId != 1 && (string.IsNullOrEmpty(searchValue) || x.User.FirstName.ToLower().Contains(searchValue.ToLower().Trim()) || x.User.LastName.ToLower().Contains(searchValue.ToLower().Trim()) || x.Role.RoleName.ToLower().Contains(searchValue.ToLower().Trim()) || x.Company.CompanyName.ToLower().Contains(searchValue.ToLower().Trim()) || x.User.Email.ToLower().Contains(searchValue.ToLower().Trim()) || x.User.Phone.ToLower().Contains(searchValue.ToLower().Trim()))).OrderByDescending(x => x.UserId).ToList();

                    }
                    else
                    {
                        // for other role
                        startDate = Convert.ToDateTime(startDate).AddMinutes(-1);
                        endDate = Convert.ToDateTime(endDate).AddDays(1).AddSeconds(-1);
                        tblUserRoles = _context.TblUserRoles
                       .Include(t => t.Role)
                       .Include(t => t.User)
                       .Include(t => t.Company)
                       .Include(t => t.User).Where(x => x.IsActive == true && (string.IsNullOrEmpty(searchValue) || x.User.FirstName.ToLower().Contains(searchValue.ToLower().Trim()) || x.User.LastName.ToLower().Contains(searchValue.ToLower().Trim()) || x.Role.RoleName.ToLower().Contains(searchValue.ToLower().Trim()) || x.Company.CompanyName.ToLower().Contains(searchValue.ToLower().Trim()) || x.User.Email.ToLower().Contains(searchValue.ToLower().Trim()) || x.User.Phone.ToLower().Contains(searchValue.ToLower().Trim()))).OrderByDescending(x => x.UserId).ToList();

                    }

                }


                recordsTotal = tblUserRoles != null ? tblUserRoles.Count : 0;
                if (tblUserRoles != null)
                {
                    tblUserRoles = sortColumnDirection.Equals(SortingOrder.ASCENDING) ? tblUserRoles.OrderBy(o => o.GetType().GetProperty(sortColumn).GetValue(o, null)).ToList() : tblUserRoles.OrderByDescending(o => o.GetType().GetProperty(sortColumn).GetValue(o, null)).ToList();
                    tblUserRoles = tblUserRoles.Skip(skip).Take(pageSize).ToList();
                }
                UserList = new List<UserRolesView>();

                foreach (var item in tblUserRoles)
                {
                    string actionURL = string.Empty;
                    string ImageURL = string.Empty;
                    actionURL = "<div class='actionbtns'>";

                    actionURL = actionURL + " <span><input type='checkbox' value=" + item.UserId + " class='checkboxinput' /></span>";
                    actionURL = actionURL + " </td>";
                    ImageURL = "<img src='" + URL + "/DBFiles/Users/" + item.User.ImageName + "' class='brandlistimg' />";
                    if (item != null)
                    {
                        UserListViewModel = new UserRolesView();
                        UserListViewModel.Action = actionURL;
                        UserListViewModel.ImageName = ImageURL;
                        UserListViewModel.FirstName = item.User.FirstName;
                        UserListViewModel.LastName = item.User.LastName;
                        if (item.User.Email != null)
                        {
                            UserListViewModel.Email = SecurityHelper.DecryptString(item.User.Email, _config.Value.SecurityKey);

                        }
                        if (item.User.Phone != null)
                        {
                            UserListViewModel.Phone = SecurityHelper.DecryptString(item.User.Phone, _config.Value.SecurityKey);

                        }

                        UserListViewModel.Role = item.Role.RoleName;
                        UserListViewModel.CompanyName = item.Company.CompanyName;
                        UserListViewModel.CreatedDate = item.CreatedDate;
                        if (item.IsActive == true)
                        {
                            UserListViewModel.UserStatus = "Active";
                        }
                        else
                        {
                            UserListViewModel.UserStatus = "Inactive";
                        }
                        UserList.Add(UserListViewModel);

                        TblUserRole UserDetails = tblUserRoles.FirstOrDefault(x => x.UserRoleId == item.UserRoleId);

                        if (UserDetails.CreatedDate != null && UserDetails.CreatedDate != null)
                        {
                            //var Date = (DateTime)item.CreatedDate;
                            //item.Date = Date.ToShortDateString();


                            DateTime dateTime = (DateTime)item.CreatedDate;
                            //item.Date = dateTime.ToString("yyyy-MM-dd");
                            item.CreatedDate = UserDetails.CreatedDate;

                        }


                    }

                }

                var data = UserList;
                var jsonData = new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data };
                return Ok(jsonData);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [HttpPost]
        public async Task<ActionResult> GetProductCategoryList(DateTime? startDate, DateTime? endDate)
        {
            string URL = _config.Value.URLPrefixforProd;
            List<TblProductCategory> tblProductCategory = null;
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
                //_context = new Digi2l_DevContext();
                if (startDate == null && endDate == null)
                {
                    tblProductCategory = await _context.TblProductCategories.Where(x =>
                        (string.IsNullOrEmpty(searchValue) || x.Name.ToLower().Contains(searchValue.ToLower().Trim()) || x.Description.ToLower().Contains(searchValue.ToLower().Trim()) || x.Code.ToLower().Contains(searchValue.ToLower().Trim()))).ToListAsync();
                }
                else
                {
                    startDate = Convert.ToDateTime(startDate).AddMinutes(-1);
                    endDate = Convert.ToDateTime(endDate).AddDays(1).AddSeconds(-1);
                    tblProductCategory = await _context.TblProductCategories.Where(x => x.CreatedDate >= startDate && x.CreatedDate <= endDate
                        && (string.IsNullOrEmpty(searchValue) || x.Name.ToLower().Contains(searchValue.ToLower().Trim()) || x.Description.ToLower().Contains(searchValue.ToLower().Trim()) || x.Code.ToLower().Contains(searchValue.ToLower().Trim()))).ToListAsync();

                }



                recordsTotal = tblProductCategory != null ? tblProductCategory.Count : 0;
                if (tblProductCategory != null)
                {
                    tblProductCategory = sortColumnDirection.Equals(SortingOrder.ASCENDING) ? tblProductCategory.OrderBy(o => o.GetType().GetProperty(sortColumn).GetValue(o, null)).ToList() : tblProductCategory.OrderByDescending(o => o.GetType().GetProperty(sortColumn).GetValue(o, null)).ToList();
                    tblProductCategory = tblProductCategory.Skip(skip).Take(pageSize).ToList();
                }
                else
                    tblProductCategory = new List<TblProductCategory>();

                List<ProductCategoryViewModel> ProductCategoryList = _mapper.Map<List<TblProductCategory>, List<ProductCategoryViewModel>>(tblProductCategory);
                //List<ProductCategoryViewModel> ProductCategoryList = _mapper.Map<List<TblProductCategory>, List<ProductCategoryViewModel>>(tblProductCategory);
                string actionURL = string.Empty;

                foreach (ProductCategoryViewModel item in ProductCategoryList)
                {
                    if (item.IsActive == true)

                    {
                       
                        actionURL = "<div class='actionbtns'>";
                        actionURL = actionURL + "<a href=' " + URL + "/ProductCategory/Manage?id=" + _protector.Encode(item.Id) + "' data-bs-toggle='tooltip' data-bs-placement='top' title='Edit' class='btn btn-sm btn-primary'><i class='fa-solid fa-pen'></i></a> " +
                            "<a href='javascript: void(0)' onclick='deleteConfirmProductCategory(" + item.Id + ")' class='btn btn-sm btn-danger' data-bs-toggle='tooltip' data-bs-placement='top' title='Inactive'><i class='fa-solid fa-trash'></i></a>";
                        actionURL = actionURL + " </div>";
                       
                    }
                    else
                    {

                        actionURL = "<div class='actionbtns'>";
                        actionURL = actionURL + "<a href=' " + URL + "/ProductCategory/Manage?id=" + _protector.Encode(item.Id) + "' data-bs-toggle='tooltip' data-bs-placement='top' title='Edit' class='btn btn-sm btn-primary'><i class='fa-solid fa-pen'></i></a> " +
                            "<a href='javascript: void(0)' onclick='activeConfirmProductCategory(" + item.Id + ")'  class='btn btn-sm btn-success' data-bs-toggle='tooltip' data-bs-placement='top' title='Active'><i class='fa-solid fa-circle-check text-white'></i></a>";
                        actionURL = actionURL + " </div>";
                    }

                    item.Action = actionURL;
                    TblProductCategory ProductCategoryDetails = tblProductCategory.FirstOrDefault(x => x.Id == item.Id);

                    if (ProductCategoryDetails.CreatedDate != null && ProductCategoryDetails.CreatedDate != null)
                    {
                        //var Date = (DateTime)item.CreatedDate;
                        //item.Date = Date.ToShortDateString();


                        DateTime dateTime = (DateTime)item.CreatedDate;
                        item.Date = dateTime.ToString("yyyy-MM-dd");
                        //item.CreatedDate = ProductCategoryDetails.CreatedDate;

                    }
                }

                var data = ProductCategoryList;
                var jsonData = new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data };
                return Ok(jsonData);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [HttpPost]
        public async Task<ActionResult> GetProductTypeList(DateTime? startDate, DateTime? endDate)
        {
            string URL = _config.Value.URLPrefixforProd;
            List<TblProductType> tblproductTypes = null;
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
                //_context = new Digi2l_DevContext();

                if (startDate == null && endDate == null)
                {
                    tblproductTypes = await _context.TblProductTypes.Include(x => x.ProductCat).Where(x => (string.IsNullOrEmpty(searchValue) || x.Name.ToLower().Contains(searchValue.ToLower().Trim()) || x.Description.ToLower().Contains(searchValue.ToLower().Trim()) || x.Code.ToLower().Contains(searchValue.ToLower().Trim()) || x.Size.ToLower().Contains(searchValue.ToLower().Trim()) || x.ProductCat.Description.ToLower().Contains(searchValue.ToLower().Trim()))).ToListAsync();
                }
                else
                {
                    startDate = Convert.ToDateTime(startDate).AddMinutes(-1);
                    endDate = Convert.ToDateTime(endDate).AddDays(1).AddSeconds(-1);
                    tblproductTypes = await _context.TblProductTypes.Include(x => x.ProductCat).Where(x =>  x.CreatedDate >= startDate && x.CreatedDate <= endDate
                        && (string.IsNullOrEmpty(searchValue) || x.Name.ToLower().Contains(searchValue.ToLower().Trim()) || x.Description.ToLower().Contains(searchValue.ToLower().Trim()) || x.Code.ToLower().Contains(searchValue.ToLower().Trim()) || x.Size.ToLower().Contains(searchValue.ToLower().Trim()) || x.ProductCat.Description.ToLower().Contains(searchValue.ToLower().Trim()))).ToListAsync();

                }



                recordsTotal = tblproductTypes != null ? tblproductTypes.Count : 0;
                if (tblproductTypes != null)
                {
                    tblproductTypes = sortColumnDirection.Equals(SortingOrder.ASCENDING) ? tblproductTypes.OrderBy(o => o.GetType().GetProperty(sortColumn).GetValue(o, null)).ToList() : tblproductTypes.OrderByDescending(o => o.GetType().GetProperty(sortColumn).GetValue(o, null)).ToList();
                    tblproductTypes = tblproductTypes.Skip(skip).Take(pageSize).ToList();
                }
                else
                    tblproductTypes = new List<TblProductType>();

                List<ProductTypeViewModel> ProductTypeList = _mapper.Map<List<TblProductType>, List<ProductTypeViewModel>>(tblproductTypes);

                string actionURL = string.Empty;

                foreach (ProductTypeViewModel item in ProductTypeList)
                {
                    if (item.IsActive== true)

                    { 
                    actionURL = "<div class='actionbtns'>";
                    actionURL = actionURL + "<a href=' " + URL + "/ProductType/Manage?id=" + _protector.Encode(item.Id) + "' data-bs-toggle='tooltip' data-bs-placement='top' title='Edit' class='btn btn-sm btn-primary'><i class='fa-solid fa-pen'></i></a> " +
                        "<a href='javascript: void(0)' onclick='deleteConfirmProductType(" + item.Id + ")' class='btn btn-sm btn-danger' data-bs-toggle='tooltip' data-bs-placement='top' title='InActive'><i class='fa-solid fa-trash'></i></a>";
                    actionURL = actionURL + " </div>";
                    }
                    else
                    {
                        actionURL = "<div class='actionbtns'>";
                        actionURL = actionURL + "<a href=' " + URL + "/ProductType/Manage?id=" + _protector.Encode(item.Id) + "' data-bs-toggle='tooltip' data-bs-placement='top' title='Edit' class='btn btn-sm btn-primary'><i class='fa-solid fa-pen'></i></a> " +
                            "<a href='javascript: void(0)' onclick='activeConfirmProductType(" + item.Id + ")' class='btn btn-sm btn-success' data-bs-toggle='tooltip' data-bs-placement='top' title='Active'><i class='fa-solid fa-circle-check text-white'></i></a>";
                        actionURL = actionURL + " </div>";
                        
                    }

                    item.Action = actionURL;
                    TblProductType productType = tblproductTypes.FirstOrDefault(x => x.Id == item.Id);
                    if (productType.CreatedDate != null && productType.CreatedDate != null)
                    {
                        //var Date = (DateTime)item.CreatedDate;
                        //item.Date = Date.ToShortDateString();


                        DateTime dateTime = (DateTime)item.CreatedDate;
                        item.Date = dateTime.ToString("yyyy-MM-dd");
                        //item.CreatedDate = productType.CreatedDate;

                    }

                    if (productType != null && productType.ProductCatId > 0)
                    {
                        TblProductCategory tblProductCategory = _context.TblProductCategories.FirstOrDefault(x => x.Id == productType.ProductCatId);
                        item.ProductCategoryName = tblProductCategory != null ? tblProductCategory.Description : string.Empty;
                    }
                }

                var data = ProductTypeList;
                var jsonData = new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data };
                return Ok(jsonData);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [HttpPost]
        public async Task<ActionResult> GetRoleList(DateTime? startDate, DateTime? endDate)
        {
            string URL = _config.Value.URLPrefixforProd;
            List<TblRole> tblRoles = null;
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
                //_context = new Digi2l_DevContext();
                if (startDate == null && endDate == null)
                {
                    tblRoles = await _context.TblRoles.Where(x => x.IsActive == true
                        && (string.IsNullOrEmpty(searchValue) || x.RoleName.ToLower().Contains(searchValue.ToLower().Trim()) || x.Company.CompanyName.ToLower().Contains(searchValue.ToLower().Trim()))).ToListAsync();
                }
                else
                {
                    startDate = Convert.ToDateTime(startDate).AddMinutes(-1);
                    endDate = Convert.ToDateTime(endDate).AddDays(1).AddSeconds(-1);
                    tblRoles = await _context.TblRoles.Where(x => x.IsActive == true && x.CreatedDate >= startDate && x.CreatedDate <= endDate
                        && (string.IsNullOrEmpty(searchValue) || x.RoleName.ToLower().Contains(searchValue.ToLower().Trim()) || x.Company.CompanyName.ToLower().Contains(searchValue.ToLower().Trim()))).ToListAsync();

                }



                recordsTotal = tblRoles != null ? tblRoles.Count : 0;
                if (tblRoles != null)
                {
                    tblRoles = sortColumnDirection.Equals(SortingOrder.ASCENDING) ? tblRoles.OrderBy(o => o.GetType().GetProperty(sortColumn).GetValue(o, null)).ToList() : tblRoles.OrderByDescending(o => o.GetType().GetProperty(sortColumn).GetValue(o, null)).ToList();
                    tblRoles = tblRoles.Skip(skip).Take(pageSize).ToList();
                }
                else
                    tblRoles = new List<TblRole>();

                List<RoleViewModel> RoleList = _mapper.Map<List<TblRole>, List<RoleViewModel>>(tblRoles);

                string actionURL = string.Empty;

                foreach (RoleViewModel item in RoleList)
                {

                    actionURL = "<div class='actionbtns'>";
                    actionURL = actionURL + "<a href=' " + URL + "/Company/RoleManagedumy?id=" + item.RoleId + "' data-bs-toggle='tooltip' data-bs-placement='top' title='Edit' class='btn btn-sm btn-primary'><i class='fa-solid fa-pen'></i></a> " +
                        "<a href='javascript: void(0)' onclick='deleteConfirm(" + item.RoleId + ")' class='btn btn-sm btn-danger' data-bs-toggle='tooltip' data-bs-placement='top' title='Delete'><i class='fa-solid fa-trash'></i></a>";
                    actionURL = actionURL + " </div>";
                    item.Action = actionURL;
                    TblRole Role = tblRoles.FirstOrDefault(x => x.RoleId == item.RoleId);

                    if (Role.CreatedDate != null && Role.CreatedDate != null)
                    {
                        //var Date = (DateTime)item.CreatedDate;
                        //item.Date = Date.ToShortDateString();


                        DateTime dateTime = (DateTime)item.CreatedDate;
                        item.Date = dateTime.ToString("yyyy-MM-dd");
                        //item.CreatedDate = Role.CreatedDate;

                    }


                    if (Role != null && Role.CompanyId > 0)
                    {
                        TblCompany tblCompany = _context.TblCompanies.FirstOrDefault(x => x.CompanyId == Role.CompanyId);
                        item.CompanyName = tblCompany != null ? tblCompany.CompanyName : string.Empty;
                    }
                }

                var data = RoleList;
                var jsonData = new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data };
                return Ok(jsonData);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [HttpPost]
        public async Task<ActionResult> GetProductQualityIndexList(DateTime? startDate, DateTime? endDate)
        {
            string URL = _config.Value.URLPrefixforProd;
            List<TblProductQualityIndex> tblProductQualityIndex = null;
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
                //_context = new Digi2l_DevContext();

                if (startDate == null && endDate == null)
                {

                    tblProductQualityIndex = await _context.TblProductQualityIndices.Include(x => x.ProductCategory).Where(x => 
                         (string.IsNullOrEmpty(searchValue) || x.Name.ToLower().Contains(searchValue.ToLower().Trim()) || x.ProductCategory.Description.ToLower().Contains(searchValue.ToLower().Trim()))).ToListAsync();
                }
                else
                {
                    startDate = Convert.ToDateTime(startDate).AddMinutes(-1);
                    endDate = Convert.ToDateTime(endDate).AddDays(1).AddSeconds(-1);
                    tblProductQualityIndex = await _context.TblProductQualityIndices.Include(x => x.ProductCategory).Where(x =>  x.CreatedDate >= startDate && x.CreatedDate <= endDate
                        && (string.IsNullOrEmpty(searchValue) || x.Name.ToLower().Contains(searchValue.ToLower().Trim()) || x.ProductCategory.Description.ToLower().Contains(searchValue.ToLower().Trim()))).ToListAsync();

                }

                recordsTotal = tblProductQualityIndex != null ? tblProductQualityIndex.Count : 0;
                if (tblProductQualityIndex != null)
                {
                    tblProductQualityIndex = sortColumnDirection.Equals(SortingOrder.ASCENDING) ? tblProductQualityIndex.OrderBy(o => o.GetType().GetProperty(sortColumn).GetValue(o, null)).ToList() : tblProductQualityIndex.OrderByDescending(o => o.GetType().GetProperty(sortColumn).GetValue(o, null)).ToList();
                    tblProductQualityIndex = tblProductQualityIndex.Skip(skip).Take(pageSize).ToList();
                }
                else
                    tblProductQualityIndex = new List<TblProductQualityIndex>();

                List<ProductQualityIndexViewModel> ProductQualityIndexList = _mapper.Map<List<TblProductQualityIndex>, List<ProductQualityIndexViewModel>>(tblProductQualityIndex);
                //List<ProductCategoryViewModel> ProductCategoryList = _mapper.Map<List<TblProductCategory>, List<ProductCategoryViewModel>>(tblProductCategory);
                string actionURL = string.Empty;

                foreach (ProductQualityIndexViewModel item in ProductQualityIndexList)
                {
                    if (item.IsActive == true)
                    {
                        actionURL = "<div class='actionbtns'>";
                        actionURL = actionURL + "<a href=' " + URL + "/ProductQualityIndex/Manage?id=" + _protector.Encode(item.ProductQualityIndexId) + "' data-bs-toggle='tooltip' data-bs-placement='top' title='Edit' class='btn btn-sm btn-primary'><i class='fa-solid fa-pen'></i></a> " +
                            "<a href='javascript: void(0)' onclick='deleteConfirmQuality(" + item.ProductQualityIndexId + ")' class='btn btn-sm btn-danger' data-bs-toggle='tooltip' data-bs-placement='top' title='Inactive'><i class='fa-solid fa-trash'></i></a>";
                        actionURL = actionURL + " </div>";
                    }
                    else
                    {

                        actionURL = "<div class='actionbtns'>";
                        actionURL = actionURL + "<a href=' " + URL + "/ProductQualityIndex/Manage?id=" + _protector.Encode(item.ProductQualityIndexId) + "' data-bs-toggle='tooltip' data-bs-placement='top' title='Edit' class='btn btn-sm btn-primary'><i class='fa-solid fa-pen'></i></a> " +
                            "<a href='javascript: void(0)' onclick='activeConfirmQuality(" + item.ProductQualityIndexId + ")' class='btn btn-sm btn-success' data-bs-toggle='tooltip' data-bs-placement='top' title='active'><i class='fa-solid fa-circle-check text-white'></i></a>";
                        actionURL = actionURL + " </div>";


                       
                    }

                    item.Action = actionURL;


                    TblProductQualityIndex ProductQualityIndex = tblProductQualityIndex.FirstOrDefault(x => x.ProductQualityIndexId == item.ProductQualityIndexId);
                    if (ProductQualityIndex.CreatedDate != null && ProductQualityIndex.CreatedDate != null)
                    {
                        //var Date = (DateTime)item.CreatedDate;
                        //item.Date = Date.ToShortDateString();


                        DateTime dateTime = (DateTime)item.CreatedDate;
                        item.Date = dateTime.ToString("yyyy-MM-dd");
                        //item.CreatedDate = ProductQualityIndex.CreatedDate;

                    }


                    if (ProductQualityIndex != null && ProductQualityIndex.ProductCategoryId > 0)
                    {
                        TblProductCategory tblProductCategory = _context.TblProductCategories.FirstOrDefault(x => x.Id == ProductQualityIndex.ProductCategoryId);
                        item.ProductCategoryName = tblProductCategory != null ? tblProductCategory.Description : string.Empty;
                    }
                }

                var data = ProductQualityIndexList;
                var jsonData = new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data };
                return Ok(jsonData);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [HttpPost]
        public async Task<ActionResult> GetPinCodeList(DateTime? startDate, DateTime? endDate)
        {
            string URL = _config.Value.URLPrefixforProd;
            List<TblPinCode> TblPinCodes = null;
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
                // _context = new Digi2l_DevContext();

                if (startDate == null && endDate == null)
                {

                    TblPinCodes = await _context.TblPinCodes.Where(x => (string.IsNullOrEmpty(searchValue) || x.Location.ToLower().Contains(searchValue.ToLower().Trim()) || x.ZipCode.ToString().Contains(searchValue.ToLower().Trim()) || x.State.ToLower().Contains(searchValue.ToLower().Trim()))).OrderByDescending(k => k.CreatedDate).ToListAsync();
                }
                else
                {
                    startDate = Convert.ToDateTime(startDate).AddMinutes(-1);
                    endDate = Convert.ToDateTime(endDate).AddDays(1).AddSeconds(-1);
                    TblPinCodes = await _context.TblPinCodes.Where(x => x.CreatedDate >= startDate && x.CreatedDate <= endDate
&& (string.IsNullOrEmpty(searchValue) || x.Location.ToLower().Contains(searchValue.ToLower().Trim()) || x.ZipCode.ToString().Contains(searchValue.ToLower().Trim()))).OrderByDescending(k => k.CreatedDate).ToListAsync();

                }

                recordsTotal = TblPinCodes != null ? TblPinCodes.Count : 0;
                if (TblPinCodes != null)
                {
                    //TblPinCodes = sortColumnDirection.Equals(SortingOrder.ASCENDING) ? TblPinCodes.OrderBy(o => o.GetType().GetProperty(sortColumn).GetValue(o, null)).ToList() : TblPinCodes.OrderByDescending(o => o.GetType().GetProperty(sortColumn).GetValue(o, null)).ToList();
                    TblPinCodes = TblPinCodes.Skip(skip).Take(pageSize).ToList();
                }
                else
                    TblPinCodes = new List<TblPinCode>();

                List<PinCodeViewModel> pinCodeList = _mapper.Map<List<TblPinCode>, List<PinCodeViewModel>>(TblPinCodes);
                string actionURL = string.Empty;

                foreach (PinCodeViewModel item in pinCodeList)
                {
                    if (item.IsActive == true)

                    {
                        actionURL = "<div class='actionbtns'>";
                        actionURL = actionURL + "<a href=' " + URL + "/PinCode/Manage?id=" + _protector.Encode(item.Id) + "' data-bs-toggle='tooltip' data-bs-placement='top' title='Edit' class='btn btn-sm btn-primary'><i class='fa-solid fa-pen'></i></a> " +
                            "<a href='javascript: void(0)' onclick='deleteConfirmPincode(" + item.Id + ")' class='btn btn-sm btn-danger' data-bs-toggle='tooltip' data-bs-placement='top' title='Inactive'><i class='fa-solid fa-trash'></i></a>";
                        actionURL = actionURL + " </div>";

                    }
                    else
                    {

                        actionURL = "<div class='actionbtns'>";
                        actionURL = actionURL + "<a href=' " + URL + "/PinCode/Manage?id=" + _protector.Encode(item.Id) + "' data-bs-toggle='tooltip' data-bs-placement='top' title='Edit' class='btn btn-sm btn-primary'><i class='fa-solid fa-pen'></i></a> " +
                            "<a href='javascript: void(0)' onclick='activeConfirmPincode(" + item.Id + ")' class='btn btn-sm  btn-success' data-bs-toggle='tooltip' data-bs-placement='top' title='Active'><i class='fa-solid fa-circle-check text-white'></i></a>";
                        actionURL = actionURL + " </div>";
                    }


                  
                    item.Action = actionURL;

                    TblPinCode PinCodeDetails = TblPinCodes.FirstOrDefault(x => x.Id == item.Id);
                    if (PinCodeDetails.CreatedDate != null && PinCodeDetails.CreatedDate != null)
                    {
                        //var Date = (DateTime)item.CreatedDate;
                        //item.Date = Date.ToShortDateString();


                        DateTime dateTime = (DateTime)item.CreatedDate;
                        item.Date = dateTime.ToString("yyyy-MM-dd");
                        //item.CreatedDate = PinCodeDetails.CreatedDate;

                    }

                }

                var data = pinCodeList;
                var jsonData = new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data };
                return Ok(jsonData);
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        [HttpPost]
        public async Task<ActionResult> GetStateList(DateTime? startDate, DateTime? endDate)
        {
            string URL = _config.Value.URLPrefixforProd;
            List<TblState> TblStates = null;
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
                // _context = new Digi2l_DevContext();
                if (startDate == null && endDate == null)
                {

                    TblStates = await _context.TblStates.Where(x => (string.IsNullOrEmpty(searchValue)
                       || x.Name.ToLower().Contains(searchValue.ToLower().Trim())
                       || x.Code.ToLower().Contains(searchValue.ToLower().Trim())
                       )).ToListAsync();
                }
                else
                {
                    startDate = Convert.ToDateTime(startDate).AddMinutes(-1);
                    endDate = Convert.ToDateTime(endDate).AddDays(1).AddSeconds(-1);
                    TblStates = await _context.TblStates.Where(x => x.CreatedDate >= startDate && x.CreatedDate <= endDate
                         && (string.IsNullOrEmpty(searchValue)
                         || x.Name.ToLower().Contains(searchValue.ToLower().Trim())
                          || x.Code.ToLower().Contains(searchValue.ToLower().Trim())
                         )).ToListAsync();

                }

                recordsTotal = TblStates != null ? TblStates.Count : 0;
                if (TblStates != null)
                {
                    TblStates = sortColumnDirection.Equals(SortingOrder.ASCENDING) ? TblStates.OrderBy(o => o.GetType().GetProperty(sortColumn).GetValue(o, null)).ToList() : TblStates.OrderByDescending(o => o.GetType().GetProperty(sortColumn).GetValue(o, null)).ToList();
                    TblStates = TblStates.Skip(skip).Take(pageSize).ToList();
                }
                else
                    TblStates = new List<TblState>();

                List<StateViewModel> StateList = _mapper.Map<List<TblState>, List<StateViewModel>>(TblStates);
                string actionURL = string.Empty;

                foreach (StateViewModel item in StateList)
                {
                    if (item.IsActive == true)

                    {
                        actionURL = "<div class='actionbtns'>";
                        actionURL = actionURL + "<a href=' " + URL + "/State/Manage?id=" + _protector.Encode(item.StateId) + "' data-bs-toggle='tooltip' data-bs-placement='top' title='Edit' class='btn btn-sm btn-primary'><i class='fa-solid fa-pen'></i></a> " +
                            "<a href='javascript: void(0)' onclick='deleteConfirmState(" + item.StateId + ")' class='btn btn-sm btn-danger' data-bs-toggle='tooltip' data-bs-placement='top' title='Inactive'><i class='fa-solid fa-trash'></i></a>";
                        actionURL = actionURL + " </div>";
                     
                    }
                    else
                    {

                        actionURL = "<div class='actionbtns'>";
                        actionURL = actionURL + "<a href=' " + URL + "/State/Manage?id=" + _protector.Encode(item.StateId) + "' data-bs-toggle='tooltip' data-bs-placement='top' title='Edit' class='btn btn-sm btn-primary'><i class='fa-solid fa-pen'></i></a> " +
                            "<a href='javascript: void(0)' onclick='activeConfirmState(" + item.StateId + ")' class='btn btn-sm btn-success' data-bs-toggle='tooltip' data-bs-placement='top' title='active'><i class='fa-solid fa-circle-check text-white'></i></a>";
                        actionURL = actionURL + " </div>";
                    }

                    item.Action = actionURL;

                    TblState StateDetails = TblStates.FirstOrDefault(x => x.StateId == item.StateId);
                    if (StateDetails.CreatedDate != null && StateDetails.CreatedDate != null)
                    {
                        //var Date = (DateTime)item.CreatedDate;
                        //item.Date = Date.ToShortDateString();


                        DateTime dateTime = (DateTime)item.CreatedDate;
                        item.Date = dateTime.ToString("yyyy-MM-dd");
                      

                    }


                }

                var data = StateList;
                var jsonData = new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data };
                return Ok(jsonData);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [HttpPost]
        public async Task<ActionResult> GetCityList(DateTime? startDate, DateTime? endDate)
        {
            string URL = _config.Value.URLPrefixforProd;
            List<TblCity> TblCities = null;
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
                // _context = new Digi2l_DevContext();

                if (startDate == null && endDate == null)
                {

                    TblCities = await _context.TblCities.Include(x => x.State).Where(x => (string.IsNullOrEmpty(searchValue) || x.Name.ToLower().Contains(searchValue.ToLower().Trim()) || x.CityCode.ToLower().Contains(searchValue.ToLower().Trim()) || x.State.Name.ToLower().Contains(searchValue.ToLower().Trim()))).ToListAsync();
                }
                else
                {
                    startDate = Convert.ToDateTime(startDate).AddMinutes(-1);
                    endDate = Convert.ToDateTime(endDate).AddDays(1).AddSeconds(-1);
                    TblCities = await _context.TblCities.Include(x => x.State).Where(x => x.CreatedDate >= startDate && x.CreatedDate <= endDate
                          && (string.IsNullOrEmpty(searchValue) || x.Name.ToLower().Contains(searchValue.ToLower().Trim()) || x.CityCode.ToLower().Contains(searchValue.ToLower().Trim()) || x.State.Name.ToLower().Contains(searchValue.ToLower().Trim()))).ToListAsync();

                }


                recordsTotal = TblCities != null ? TblCities.Count : 0;
                if (TblCities != null)
                {
                    TblCities = sortColumnDirection.Equals(SortingOrder.ASCENDING) ? TblCities.OrderBy(o => o.GetType().GetProperty(sortColumn).GetValue(o, null)).ToList() : TblCities.OrderByDescending(o => o.GetType().GetProperty(sortColumn).GetValue(o, null)).ToList();
                    TblCities = TblCities.Skip(skip).Take(pageSize).ToList();
                }
                else
                    TblCities = new List<TblCity>();

                List<CityViewModel> CityList = _mapper.Map<List<TblCity>, List<CityViewModel>>(TblCities);
                string actionURL = string.Empty;

                foreach (CityViewModel item in CityList)
                {
                    if (item.IsActive == true)

                    {
                        actionURL = "<div class='actionbtns'>";
                        actionURL = actionURL + "<a href=' " + URL + "/City/Manage?id=" + _protector.Encode(item.CityId) + "' data-bs-toggle='tooltip' data-bs-placement='top' title='Edit' class='btn btn-sm btn-primary'><i class='fa-solid fa-pen'></i></a> " +
                            "<a href='javascript: void(0)' onclick='deleteConfirmCity(" + item.CityId + ")' class='btn btn-sm btn-danger' data-bs-toggle='tooltip' data-bs-placement='top' title='Inactive'><i class='fa-solid fa-trash'></i></a>";
                        actionURL = actionURL + " </div>";

                    }
                    else
                    {

                        actionURL = "<div class='actionbtns'>";
                        actionURL = actionURL + "<a href=' " + URL + "/City/Manage?id=" + _protector.Encode(item.CityId) + "' data-bs-toggle='tooltip' data-bs-placement='top' title='Edit' class='btn btn-sm btn-primary'><i class='fa-solid fa-pen'></i></a> " +
                            "<a href='javascript: void(0)' onclick='activeConfirmCity(" + item.CityId + ")' class='btn btn-sm btn-success' data-bs-toggle='tooltip' data-bs-placement='top' title='Active'><i class='fa-solid fa-circle-check text-white'></i></a>";
                        actionURL = actionURL + " </div>";
                    }

                   
                    item.Action = actionURL;

                    TblCity city = TblCities.FirstOrDefault(x => x.CityId == item.CityId);
                    if (city.CreatedDate != null && city.CreatedDate != null)
                    {
                        //var Date = (DateTime)item.CreatedDate;
                        //item.Date = Date.ToShortDateString();


                        DateTime dateTime = (DateTime)item.CreatedDate;
                        item.Date = dateTime.ToString("yyyy-MM-dd");
                        //item.CreatedDate = city.CreatedDate;

                    }

                    if (city != null && city.StateId > 0)
                    {
                        TblState TblState = _context.TblStates.FirstOrDefault(x => x.StateId == city.StateId);
                        item.StateName = TblState != null ? TblState.Name : string.Empty;
                    }
                }

                var data = CityList;
                var jsonData = new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data };
                return Ok(jsonData);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [HttpPost]
        public async Task<ActionResult> GetBusineesPartnerList(DateTime? startDate, DateTime? endDate)
        {
            string URL = _config.Value.URLPrefixforProd;
            List<TblBusinessPartner> TblBusinessPartners = null;
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
                // _context = new Digi2l_DevContext();

                if (startDate == null && endDate == null)
                {

                    TblBusinessPartners = await _context.TblBusinessPartners.Where(x => x.IsActive == true
                       && (string.IsNullOrEmpty(searchValue) || x.StoreCode.ToLower().Contains(searchValue.ToLower().Trim()) || x.Email.ToLower().Contains(searchValue.ToLower().Trim()) || x.Name.ToLower().Contains(searchValue.ToLower().Trim()) || x.State.ToLower().Contains(searchValue.ToLower().Trim()) || x.City.ToLower().Contains(searchValue.ToLower().Trim()) || x.Name.ToLower().Contains(searchValue.ToLower().Trim()) || x.State.ToLower().Contains(searchValue.ToLower().Trim()) || x.AddressLine1.ToLower().Contains(searchValue.ToLower().Trim()) || x.AddressLine2.ToLower().Contains(searchValue.ToLower().Trim()) || x.Bppassword.ToLower().Contains(searchValue.ToLower().Trim()) || x.AssociateCode.ToLower().Contains(searchValue.ToLower().Trim()))).ToListAsync();
                }
                else
                {
                    startDate = Convert.ToDateTime(startDate).AddMinutes(-1);
                    endDate = Convert.ToDateTime(endDate).AddDays(1).AddSeconds(-1);
                    TblBusinessPartners = await _context.TblBusinessPartners.Where(x => x.IsActive == true && x.CreatedDate >= startDate && x.CreatedDate <= endDate
                      && (string.IsNullOrEmpty(searchValue) || x.StoreCode.ToLower().Contains(searchValue.ToLower().Trim()) || x.Email.ToLower().Contains(searchValue.ToLower().Trim()) || x.Name.ToLower().Contains(searchValue.ToLower().Trim()) || x.State.ToLower().Contains(searchValue.ToLower().Trim()) || x.City.ToLower().Contains(searchValue.ToLower().Trim()) || x.Name.ToLower().Contains(searchValue.ToLower().Trim()) || x.State.ToLower().Contains(searchValue.ToLower().Trim()) || x.AddressLine1.ToLower().Contains(searchValue.ToLower().Trim()) || x.AddressLine2.ToLower().Contains(searchValue.ToLower().Trim()) || x.Bppassword.ToLower().Contains(searchValue.ToLower().Trim()) || x.AssociateCode.ToLower().Contains(searchValue.ToLower().Trim()))).ToListAsync();
                }




                recordsTotal = TblBusinessPartners != null ? TblBusinessPartners.Count : 0;
                if (TblBusinessPartners != null)
                {
                    TblBusinessPartners = sortColumnDirection.Equals(SortingOrder.ASCENDING) ? TblBusinessPartners.OrderBy(o => o.GetType().GetProperty(sortColumn).GetValue(o, null)).ToList() : TblBusinessPartners.OrderByDescending(o => o.GetType().GetProperty(sortColumn).GetValue(o, null)).ToList();
                    TblBusinessPartners = TblBusinessPartners.Skip(skip).Take(pageSize).ToList();
                }
                else
                    TblBusinessPartners = new List<TblBusinessPartner>();

                List<BusinessPartnerViewModel> BusinessPartnerList = _mapper.Map<List<TblBusinessPartner>, List<BusinessPartnerViewModel>>(TblBusinessPartners);
                string actionURL = string.Empty;

                foreach (BusinessPartnerViewModel item in BusinessPartnerList)
                {
                    actionURL = "<div class='actionbtns'>";
                    actionURL = actionURL + "<a href=' " + URL + "/BusinessPartner/Manage?id=" + _protector.Encode(item.BusinessPartnerId) + "' data-bs-toggle='tooltip' data-bs-placement='top' title='Edit' class='btn btn-sm btn-primary'><i class='fa-solid fa-pen'></i></a> " +
                        "<a href='javascript: void(0)' onclick='deleteConfirmBusinessPartner(" + item.BusinessPartnerId + ")' class='btn btn-sm btn-danger' data-bs-toggle='tooltip' data-bs-placement='top' title='Delete'><i class='fa-solid fa-trash'></i></a>";
                    actionURL = actionURL + " </div>";
                    item.Action = actionURL;

                    TblBusinessPartner BusinessPartner = TblBusinessPartners.FirstOrDefault(x => x.BusinessPartnerId == item.BusinessPartnerId);
                    if (BusinessPartner.CreatedDate != null && BusinessPartner.CreatedDate != null)
                    {
                        //var Date = (DateTime)item.CreatedDate;
                        //item.Date = Date.ToShortDateString();


                        DateTime dateTime = (DateTime)item.CreatedDate;
                        item.Date = dateTime.ToString("yyyy-MM-dd");
                        //item.CreatedDate = BusinessPartner.CreatedDate;
                        //if (BusinessPartner != null && BusinessPartner.BusinessUnitId > 0)
                        //{
                        //    TblBusinessUnit tblBusinessUnit = _context.TblBusinessUnits.FirstOrDefault(x => x.BusinessUnitId == BusinessPartner.BusinessUnitId);
                        //    item.ProductCategoryName = tblProductCategory != null ? tblProductCategory.Name : string.Empty;
                        //}

                    }



                }

                var data = BusinessPartnerList;
                var jsonData = new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data };
                return Ok(jsonData);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        
        public async Task<ActionResult> GetAllWalletSummary()
        {
            List<TblEvcregistration> TblEvcregistrations = null;
            List<TblEvcwalletAddition> TblEvcwalletAddition = null;
            List<TblWalletTransaction> TblWalletTransactions = null;
            // long? IsProgress, IsDeleverd, IsComplete;
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
                // _context = new Digi2l_DevContext();
                TblEvcregistrations = await _context.TblEvcregistrations.Where(x => x.IsActive == true &&x.Isevcapprovrd==true
                        && (string.IsNullOrEmpty(searchValue) || x.BussinessName.ToLower().Contains(searchValue.ToLower()))).ToListAsync();

                recordsTotal = TblEvcregistrations != null ? TblEvcregistrations.Count : 0;
                if (TblEvcregistrations != null)
                {
                    TblEvcregistrations = sortColumnDirection.Equals(SortingOrder.ASCENDING) ? TblEvcregistrations.OrderBy(o => o.GetType().GetProperty(sortColumn).GetValue(o, null)).ToList() : TblEvcregistrations.OrderByDescending(o => o.GetType().GetProperty(sortColumn).GetValue(o, null)).ToList();
                    TblEvcregistrations = TblEvcregistrations.Skip(skip).Take(pageSize).ToList();
                }
                else
                    TblEvcregistrations = new List<TblEvcregistration>();

                List<AllWalletSummaryViewModel> allWalletSummaryViewModels = _mapper.Map<List<TblEvcregistration>, List<AllWalletSummaryViewModel>>(TblEvcregistrations);
                string actionURL = string.Empty;
                string actionURL1 = string.Empty;

                foreach (AllWalletSummaryViewModel item in allWalletSummaryViewModels)
                {
                    actionURL = " <td class='actions'>";
                    actionURL = actionURL + "<li><a href='/EVC/EVC_Registration?id=" + _protector.Encode(item.EvcregistrationId) + "&AFlag=1' class='fas fa-edit' data-toggle='tooltip' data-placement ='top' title='Edit'></a><span class='actiondevider'> || </span>"
                        + "<a href='javascript: void(0)' onclick='deleteConfirm( " + item.EvcregistrationId + " )' class='fas fa-trash' data-toggle='tooltip' data-placement='top' title='Delete'></a></a><span class='actiondevider'> || </span>"
                          + "<button onclick='AddWalletConfirm(" + item.EvcregistrationId + ")' class='badge badge-pill badge-warning'>Add Wallet</button></li>";
                    actionURL = actionURL + " </td>";
                    item.Action = actionURL;

                    actionURL1 = "<button onclick='WalletRe_Calculation()' class='btn btn-primary btn-lg'>Re-calculation</button>";
                    item.Edit = actionURL1;
                    //Take Data is More Tbl 
                    TblEvcregistration EVC_Reg = TblEvcregistrations.FirstOrDefault(x => x.EvcregistrationId == item.EvcregistrationId);
                    if (EVC_Reg != null && EVC_Reg.EvcwalletAmount > 0)
                    {
                        TblWalletTransactions = _context.TblWalletTransactions.Where(x => x.EvcregistrationId == EVC_Reg.EvcregistrationId && x.IsActive == true && x.StatusId != 26.ToString()).ToList();

                        if (TblWalletTransactions != null)
                        {
                            TblWalletTransaction tblWalletTransaction = new TblWalletTransaction();
                            long? IsProgress, IsDeleverd, IsComplete;
                            AllWalletSummaryViewModel allWalletSummaryViewModel = new AllWalletSummaryViewModel();
                            foreach (var items in TblWalletTransactions)
                            {
                                if (items.OrderOfInprogressDate != null && items.OrderOfDeliverdDate == null && items.OrderOfCompleteDate == null)
                                {
                                    allWalletSummaryViewModel.TotalofInprogress += items.OrderAmount;

                                    // tblWalletTransaction.OrderAmount += items.OrderAmount;
                                }
                                if (items.OrderOfInprogressDate != null && items.OrderOfDeliverdDate != null && items.OrderOfCompleteDate == null)
                                {
                                    allWalletSummaryViewModel.TotalofDeliverd += items.OrderAmount;
                                }

                            }

                            item.TotalofInprogress = allWalletSummaryViewModel.TotalofInprogress == null ? null : allWalletSummaryViewModel.TotalofInprogress;
                            item.TotalofDeliverd = allWalletSummaryViewModel.TotalofDeliverd == null ? null : allWalletSummaryViewModel.TotalofDeliverd;
                            item.RuningBalance = item.EvcwalletAmount - (item.TotalofInprogress + item.TotalofDeliverd);
                            item.EvcName = item.EvcregdNo + '-' + item.BussinessName;
                        }

                    }
                    if (EVC_Reg != null && EVC_Reg.EmployeeId > 0)
                    {
                        TblUser tblUser = _context.TblUsers.FirstOrDefault(x => x.UserId == EVC_Reg.EmployeeId);
                        item.EmployeeName = tblUser != null ? tblUser.FirstName : string.Empty;
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

        #region LGC pickup & drop list
        #region Get List of Orders who is ready for pickup
        public async Task<IActionResult> TicketGeneratedOrderListbyServicePartner(int userId, DateTime? orderStartDate, DateTime? orderEndDate,
           int? productCatId, int? productTypeId, string? regdNo, string? ticketNo, string? custCity)
        {
            #region Variable Declaration
            if (string.IsNullOrEmpty(regdNo) || regdNo == "null")
            { regdNo = null; }
            else
            { regdNo = regdNo.Trim().ToLower(); }
            if (string.IsNullOrEmpty(ticketNo) || ticketNo == "null")
            { ticketNo = null; }
            else
            { ticketNo = ticketNo.Trim().ToLower(); }
            if (string.IsNullOrEmpty(custCity) || custCity == "null")
            { custCity = null; }
            else
            { custCity = custCity.Trim().ToLower(); }
            if (productCatId == 0)
            { productCatId = null; }
            if (productTypeId == 0)
            { productTypeId = null; }
            List<TblLogistic> tblLogistic = null;
            TblServicePartner tblServicePartner = null;
            List<LGCOrderViewModel> lGCOrderList = null;
            LGCOrderViewModel lGCOrderViewModel = null;
            string URL = _config.Value.URLPrefixforProd;
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
                if (orderStartDate != null && orderEndDate != null)
                {
                    orderStartDate = Convert.ToDateTime(orderStartDate).AddMinutes(-1);
                    orderEndDate = Convert.ToDateTime(orderEndDate).AddDays(1).AddSeconds(-1);
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
                                    && ((orderStartDate == null && orderEndDate == null) || (x.CreatedDate >= orderStartDate && x.CreatedDate <= orderEndDate))
                                    && (string.IsNullOrEmpty(ticketNo) || (x.TicketNumber ?? "").ToLower() == ticketNo)
                                    && (string.IsNullOrEmpty(regdNo) || (x.RegdNo ?? "").ToLower() == regdNo)
                                    && (x.OrderTrans != null &&
                                    ((x.OrderTrans.Exchange != null
                                    && (x.OrderTrans.Exchange.ProductType != null && x.OrderTrans.Exchange.ProductType.ProductCat != null)
                                    && (productCatId == null || x.OrderTrans.Exchange.ProductType.ProductCatId == productCatId)
                                    && (productTypeId == null || x.OrderTrans.Exchange.ProductTypeId == productTypeId)
                                    && (string.IsNullOrEmpty(custCity) || (x.OrderTrans.Exchange.CustomerDetails != null && (x.OrderTrans.Exchange.CustomerDetails.City ?? "").ToLower() == custCity)))
                                    || (x.OrderTrans.Abbredemption != null && x.OrderTrans.Abbredemption.Abbregistration != null
                                    && (x.OrderTrans.Abbredemption.Abbregistration.NewProductCategoryTypeNavigation != null && x.OrderTrans.Abbredemption.Abbregistration.NewProductCategory != null)
                                    && (productCatId == null || x.OrderTrans.Abbredemption.Abbregistration.NewProductCategoryId == productCatId)
                                    && (productTypeId == null || x.OrderTrans.Abbredemption.Abbregistration.NewProductCategoryTypeId == productTypeId)
                                    && (string.IsNullOrEmpty(custCity) || (x.OrderTrans.Abbredemption.CustomerDetails != null && (x.OrderTrans.Abbredemption.CustomerDetails.City ?? "").ToLower() == custCity))))
                                    ));
                    if (count > 0)
                    {
                        tblLogistic = await _context.TblLogistics
                                .Include(x => x.OrderTrans).ThenInclude(x => x.Exchange).ThenInclude(x => x.ProductType).ThenInclude(x => x.ProductCat)
                                .Include(x => x.OrderTrans).ThenInclude(x => x.Exchange).ThenInclude(x => x.CustomerDetails)
                                .Include(x => x.OrderTrans).ThenInclude(x => x.Abbredemption).ThenInclude(x => x.Abbregistration).ThenInclude(x => x.NewProductCategory)
                                .Include(x => x.OrderTrans).ThenInclude(x => x.Abbredemption).ThenInclude(x => x.Abbregistration).ThenInclude(x => x.NewProductCategoryTypeNavigation)
                                .Include(x => x.OrderTrans).ThenInclude(x => x.Abbredemption).ThenInclude(x => x.Abbregistration)
                                .Include(x => x.OrderTrans).ThenInclude(x => x.Abbredemption).ThenInclude(x => x.CustomerDetails)
                                .Where(x => x.IsActive == true && x.StatusId == 18 && x.ServicePartnerId == tblServicePartner.ServicePartnerId
                                && ((orderStartDate == null && orderEndDate == null) || (x.CreatedDate >= orderStartDate && x.CreatedDate <= orderEndDate))
                                && (string.IsNullOrEmpty(ticketNo) || (x.TicketNumber ?? "").ToLower() == ticketNo)
                                && (string.IsNullOrEmpty(regdNo) || (x.RegdNo ?? "").ToLower() == regdNo)
                                && (x.OrderTrans != null &&
                                ((x.OrderTrans.Exchange != null
                                && (x.OrderTrans.Exchange.ProductType != null && x.OrderTrans.Exchange.ProductType.ProductCat != null)
                                && (productCatId == null || x.OrderTrans.Exchange.ProductType.ProductCatId == productCatId)
                                && (productTypeId == null || x.OrderTrans.Exchange.ProductTypeId == productTypeId)
                                && (string.IsNullOrEmpty(custCity) || (x.OrderTrans.Exchange.CustomerDetails != null && (x.OrderTrans.Exchange.CustomerDetails.City ?? "").ToLower() == custCity)))
                                || (x.OrderTrans.Abbredemption != null && x.OrderTrans.Abbredemption.Abbregistration != null
                                && (x.OrderTrans.Abbredemption.Abbregistration.NewProductCategoryTypeNavigation != null && x.OrderTrans.Abbredemption.Abbregistration.NewProductCategory != null)
                                && (productCatId == null || x.OrderTrans.Abbredemption.Abbregistration.NewProductCategoryId == productCatId)
                                && (productTypeId == null || x.OrderTrans.Abbredemption.Abbregistration.NewProductCategoryTypeId == productTypeId)
                                && (string.IsNullOrEmpty(custCity) || (x.OrderTrans.Abbredemption.CustomerDetails != null && (x.OrderTrans.Abbredemption.CustomerDetails.City ?? "").ToLower() == custCity))))
                                )).OrderByDescending(x => x.ModifiedDate ?? x.CreatedDate).Skip(skip).Take(pageSize).ToListAsync();
                    }
                }
                recordsTotal = count;
                #endregion

                #region Datatable Mapping
                lGCOrderList = new List<LGCOrderViewModel>();
                if (tblLogistic != null && tblLogistic.Count > 0)
                {
                    foreach (var item in tblLogistic)
                    {
                        if (item != null)
                        {
                            string? productCatDesc = null; string? productCatTypeDesc = null; string? custCity1 = null;
                            if (item?.OrderTrans?.Exchange != null && item.OrderTrans.Exchange.ProductType != null)
                            {
                                productCatDesc = item.OrderTrans.Exchange.ProductType.ProductCat != null ? item.OrderTrans.Exchange.ProductType.ProductCat.Description : null;
                                productCatTypeDesc = item.OrderTrans.Exchange.ProductType.Description;
                                custCity1 = item.OrderTrans.Exchange.CustomerDetails != null ? item.OrderTrans.Exchange.CustomerDetails.City : null;
                            }
                            else if (item?.OrderTrans?.Abbredemption != null && item.OrderTrans.Abbredemption.Abbregistration != null)
                            {
                                productCatDesc = item.OrderTrans.Abbredemption.Abbregistration.NewProductCategory != null ? item.OrderTrans.Abbredemption.Abbregistration.NewProductCategory.Description : null;
                                productCatTypeDesc = item.OrderTrans.Abbredemption.Abbregistration.NewProductCategoryTypeNavigation != null ? item.OrderTrans.Abbredemption.Abbregistration.NewProductCategoryTypeNavigation.Description : null;
                                custCity1 = item.OrderTrans.Abbredemption.CustomerDetails != null ? item.OrderTrans.Abbredemption.CustomerDetails.City : null;
                            }
                            string actionURL = string.Empty;
                            actionURL = " <ul class='actions'>";

                            actionURL = "<a href ='" + URL + "/LGC/LGCPickup?regdNo=" + item.RegdNo + "' ><button onclick='View(" + item.RegdNo + ")' class='btn btn-primary btn'>View</button></a>";
                            actionURL = actionURL + "</ul>";

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
                            lGCOrderViewModel.City = custCity1;
                            lGCOrderViewModel.CreatedDate = Convert.ToDateTime(item.CreatedDate).ToString("MM/dd/yyyy");
                            TblOrderQc? tblOrderQc1 = _context.TblOrderQcs.Where(x => x.OrderTransId == item.OrderTransId && x.IsActive == true && x.PreferredPickupDate != null && x.PickupStartTime != null && x.PickupEndTime != null).FirstOrDefault();
                            if (tblOrderQc1 != null)
                            {
                                // var pickupScheduleDate1 = (DateTime)tblOrderQc1.PreferredPickupDate;
                                lGCOrderViewModel.PickupScheduleTime = tblOrderQc1.PickupStartTime + " - " + tblOrderQc1.PickupEndTime;
                            }
                            lGCOrderList.Add(lGCOrderViewModel);
                        }
                    }
                }
                #endregion

                var data = lGCOrderList;
                var jsonData = new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data };
                return Ok(jsonData);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        #endregion

        #region Get List of LGC Drop Orders
        /*public IActionResult GetListForLGCDropOrders(int userId)
        {
            List<TblOrderLgc> tblOrderLgc = null;
            List<LGCOrderViewModel> lGCOrderList = null;
            LGCOrderViewModel lGCOrderViewModel = null;
            string URL = _config.Value.URLPrefixforProd;

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

                tblOrderLgc = _context.TblOrderLgcs
                    .Include(x => x.Logistic).ThenInclude(x => x.OrderTrans)
                    .ThenInclude(x => x.Exchange).ThenInclude(x => x.ProductType).ThenInclude(x => x.ProductCat)
                    .Include(x => x.Logistic).ThenInclude(x => x.ServicePartner).Where(x => x.Logistic.ServicePartner.UserId == userId)
                    .Where(x => x.IsActive == true && (string.IsNullOrEmpty(searchValue) || x.Logistic.RegdNo.ToLower().Contains(searchValue.ToLower())) && x.StatusId == 23)
                    .ToList();

                recordsTotal = tblOrderLgc != null ? tblOrderLgc.Count : 0;
                if (tblOrderLgc != null)
                {
                    tblOrderLgc = tblOrderLgc.Skip(skip).Take(pageSize).ToList();
                }
                lGCOrderList = new List<LGCOrderViewModel>();
                if (tblOrderLgc != null)
                {
                    foreach (var item in tblOrderLgc)
                    {
                        string actionURL = string.Empty;
                        var regdno = item.Logistic.RegdNo;
                        actionURL = " <ul class='actions'>";
                        actionURL = "<a href ='" + URL + "/LGC/LGCDrop?regdNo=" + item.Logistic.RegdNo + "' ><button onclick='View(" + item.Logistic.RegdNo + ")' class='btn btn-primary btn'>View</button></a>";
                        actionURL = actionURL + "</ul>";

                        if (item != null)
                        {
                            lGCOrderViewModel = new LGCOrderViewModel();
                            lGCOrderViewModel.Id = item.OrderLgcid;
                            lGCOrderViewModel.Action = actionURL;
                            lGCOrderViewModel.RegdNo = item.Logistic.RegdNo;
                            lGCOrderViewModel.TicketNumber = item.Logistic.TicketNumber;
                            lGCOrderViewModel.ProductCategory = item.Logistic.OrderTrans.Exchange.ProductType.ProductCat.Description;
                            lGCOrderViewModel.ProductType = item.Logistic.OrderTrans.Exchange.ProductType.Description;
                            lGCOrderViewModel.AmountPayableThroughLGC = 0;
                            lGCOrderList.Add(lGCOrderViewModel);
                        }
                    }
                }

                var data = lGCOrderList.OrderByDescending(x => x.Id);
                var jsonData = new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data };
                return Ok(jsonData);
            }
            catch (Exception ex)
            {
                throw;
            }
        }*/
        #endregion

        #region Get List of Orders for Drop to EVC (LGC Drop) Created by VK
        public IActionResult GetListForLGCDropLoad(int userId, int DriverDetailsId, int EVCPartnerId, DateTime? orderStartDate, DateTime? orderEndDate,
            int? productCatId, int? productTypeId, string? regdNo, string? ticketNo, string? custCity)
        {
            #region Variable Declaration
            List<TblOrderLgc> tblOrderLgc = null;
            List<LGCOrderViewModel> lGCOrderList = null;
            LGCOrderViewModel lGCOrderViewModel = null;
            TblServicePartner tblServicePartner = null;
            string URL = _config.Value.URLPrefixforProd;
            #endregion

            #region For Advanced filters
            if (string.IsNullOrEmpty(regdNo) || regdNo == "null")
            { regdNo = null; }
            else
            { regdNo = regdNo.Trim().ToLower(); }
            if (string.IsNullOrEmpty(ticketNo) || ticketNo == "null")
            { ticketNo = null; }
            else
            { ticketNo = ticketNo.Trim().ToLower(); }
            #endregion
            try
            {
                #region Datatable variables
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
                int count = 0;
                #endregion

                #region Advanced Filters
                if (orderStartDate != null && orderEndDate != null)
                {
                    orderStartDate = Convert.ToDateTime(orderStartDate).AddMinutes(-1);
                    orderEndDate = Convert.ToDateTime(orderEndDate).AddDays(1).AddSeconds(-1);
                }
                #endregion

                #region tblOrderLgc initialization
                tblServicePartner = _context.TblServicePartners.Where(x => x.UserId == userId && x.IsActive == true).FirstOrDefault();
                if (tblServicePartner != null && DriverDetailsId > 0 && EVCPartnerId > 0)
                {
                    count = _context.TblOrderLgcs
                        .Include(x => x.Logistic)
                        .Include(x => x.OrderTrans).ThenInclude(x => x.Exchange).ThenInclude(x => x.Brand)
                        .Include(x => x.OrderTrans).ThenInclude(x => x.Exchange).ThenInclude(x => x.ProductType).ThenInclude(x => x.ProductCat)
                        .Include(x => x.OrderTrans).ThenInclude(x => x.Exchange).ThenInclude(x => x.CustomerDetails)
                        .Include(x => x.OrderTrans).ThenInclude(x => x.Abbredemption).ThenInclude(x => x.Abbregistration).ThenInclude(x => x.NewProductCategory)
                        .Include(x => x.OrderTrans).ThenInclude(x => x.Abbredemption).ThenInclude(x => x.Abbregistration).ThenInclude(x => x.NewProductCategoryTypeNavigation)
                        .Include(x => x.OrderTrans).ThenInclude(x => x.Abbredemption).ThenInclude(x => x.CustomerDetails)
                        .Count(x => x.IsActive == true && x.StatusId == Convert.ToInt32(OrderStatusEnum.LGCPickup)
                        && (x.Logistic != null && x.Logistic.ServicePartnerId == tblServicePartner.ServicePartnerId)
                        && x.DriverDetailsId == DriverDetailsId && x.EvcpartnerId == EVCPartnerId && x.OrderTrans != null
                        && ((orderStartDate == null && orderEndDate == null) || (x.Logistic.CreatedDate >= orderStartDate && x.Logistic.CreatedDate <= orderEndDate))
                        && (string.IsNullOrEmpty(regdNo) || (x.Logistic.RegdNo ?? "").ToLower() == regdNo)
                        && (string.IsNullOrEmpty(ticketNo) || (x.Logistic.TicketNumber ?? "").ToLower() == ticketNo)
                        && ((x.OrderTrans.Exchange != null
                        && (x.OrderTrans.Exchange.ProductType != null && x.OrderTrans.Exchange.ProductType.ProductCat != null && x.OrderTrans.Exchange.CustomerDetails != null)
                        && (x.OrderTrans.Exchange.ProductType.ProductCatId != null && x.OrderTrans.Exchange.CustomerDetails != null)
                        && (productCatId == null || x.OrderTrans.Exchange.ProductType.ProductCatId == productCatId)
                        && (productTypeId == null || x.OrderTrans.Exchange.ProductTypeId == productTypeId)
                        && (string.IsNullOrEmpty(custCity) || (x.OrderTrans.Exchange.CustomerDetails.City ?? "").ToLower() == custCity)
                        ) // Code for ABB Redumption 
                        || (x.OrderTrans.Abbredemption != null && x.OrderTrans.Abbredemption.Abbregistration != null
                        && (x.OrderTrans.Abbredemption.Abbregistration.NewProductCategoryTypeNavigation != null
                        && x.OrderTrans.Abbredemption.Abbregistration.NewProductCategory != null
                        && x.OrderTrans.Abbredemption.CustomerDetails != null)
                        && (productCatId == null || x.OrderTrans.Abbredemption.Abbregistration.NewProductCategoryId == productCatId)
                        && (productTypeId == null || x.OrderTrans.Abbredemption.Abbregistration.NewProductCategoryTypeId == productTypeId)
                        && (string.IsNullOrEmpty(custCity) || (x.OrderTrans.Abbredemption.CustomerDetails.City ?? "").ToLower() == custCity)
                        )));
                    if (count > 0)
                    {
                        tblOrderLgc = _context.TblOrderLgcs
                        .Include(x => x.Logistic)
                        .Include(x => x.OrderTrans).ThenInclude(x => x.Exchange).ThenInclude(x => x.Brand)
                        .Include(x => x.OrderTrans).ThenInclude(x => x.Exchange).ThenInclude(x => x.ProductType).ThenInclude(x => x.ProductCat)
                        .Include(x => x.OrderTrans).ThenInclude(x => x.Exchange).ThenInclude(x => x.CustomerDetails)
                        .Include(x => x.OrderTrans).ThenInclude(x => x.Abbredemption).ThenInclude(x => x.Abbregistration).ThenInclude(x => x.NewProductCategory)
                        .Include(x => x.OrderTrans).ThenInclude(x => x.Abbredemption).ThenInclude(x => x.Abbregistration).ThenInclude(x => x.NewProductCategoryTypeNavigation)
                        .Include(x => x.OrderTrans).ThenInclude(x => x.Abbredemption).ThenInclude(x => x.CustomerDetails)
                        .Where(x => x.IsActive == true && x.StatusId == Convert.ToInt32(OrderStatusEnum.LGCPickup)
                        && (x.Logistic != null && x.Logistic.ServicePartnerId == tblServicePartner.ServicePartnerId)
                        && x.DriverDetailsId == DriverDetailsId && x.EvcpartnerId == EVCPartnerId && x.OrderTrans != null
                        && ((orderStartDate == null && orderEndDate == null) || (x.Logistic.CreatedDate >= orderStartDate && x.Logistic.CreatedDate <= orderEndDate))
                        && (string.IsNullOrEmpty(regdNo) || (x.Logistic.RegdNo ?? "").ToLower() == regdNo)
                        && (string.IsNullOrEmpty(ticketNo) || (x.Logistic.TicketNumber ?? "").ToLower() == ticketNo)
                        && ((x.OrderTrans.Exchange != null
                        && (x.OrderTrans.Exchange.ProductType != null && x.OrderTrans.Exchange.ProductType.ProductCat != null && x.OrderTrans.Exchange.CustomerDetails != null)
                        && (x.OrderTrans.Exchange.ProductType.ProductCatId != null && x.OrderTrans.Exchange.CustomerDetails != null)
                        && (productCatId == null || x.OrderTrans.Exchange.ProductType.ProductCatId == productCatId)
                        && (productTypeId == null || x.OrderTrans.Exchange.ProductTypeId == productTypeId)
                        && (string.IsNullOrEmpty(custCity) || (x.OrderTrans.Exchange.CustomerDetails.City ?? "").ToLower() == custCity)
                        ) // Code for ABB Redumption 
                        || (x.OrderTrans.Abbredemption != null && x.OrderTrans.Abbredemption.Abbregistration != null
                        && (x.OrderTrans.Abbredemption.Abbregistration.NewProductCategoryTypeNavigation != null
                        && x.OrderTrans.Abbredemption.Abbregistration.NewProductCategory != null
                        && x.OrderTrans.Abbredemption.CustomerDetails != null)
                        && (productCatId == null || x.OrderTrans.Abbredemption.Abbregistration.NewProductCategoryId == productCatId)
                        && (productTypeId == null || x.OrderTrans.Abbredemption.Abbregistration.NewProductCategoryTypeId == productTypeId)
                        && (string.IsNullOrEmpty(custCity) || (x.OrderTrans.Abbredemption.CustomerDetails.City ?? "").ToLower() == custCity)
                        ))).OrderByDescending(x => x.Logistic.CreatedDate).Skip(skip).Take(pageSize).ToList();
                    }
                }
                recordsTotal = count;
                #endregion

                #region table to model mapping
                lGCOrderList = new List<LGCOrderViewModel>();
                if (tblOrderLgc != null)
                {
                    /*int i = 0;*/
                    foreach (var item in tblOrderLgc)
                    {
                        string? actionURL = string.Empty;
                        string? regdno = item?.Logistic?.RegdNo;
                        if (item != null)
                        {
                            string? productCatDesc = null; string? productCatTypeDesc = null; string? custCityObj = null;
                            if (item.OrderTrans?.Exchange != null && item.OrderTrans?.Exchange?.ProductType != null)
                            {
                                productCatDesc = item.OrderTrans?.Exchange?.ProductType?.ProductCat != null ? item.OrderTrans?.Exchange?.ProductType?.ProductCat?.Description : null;
                                productCatTypeDesc = item.OrderTrans?.Exchange?.ProductType?.Description;
                                custCityObj = item.OrderTrans?.Exchange?.CustomerDetails != null ? item.OrderTrans?.Exchange?.CustomerDetails?.City : null;
                            }
                            else if (item.OrderTrans?.Abbredemption != null && item.OrderTrans?.Abbredemption?.Abbregistration != null)
                            {
                                productCatDesc = item.OrderTrans?.Abbredemption?.Abbregistration?.NewProductCategory != null ? item.OrderTrans?.Abbredemption?.Abbregistration?.NewProductCategory?.Description : null;
                                productCatTypeDesc = item.OrderTrans?.Abbredemption?.Abbregistration?.NewProductCategoryTypeNavigation != null ? item.OrderTrans?.Abbredemption?.Abbregistration?.NewProductCategoryTypeNavigation?.Description : null;
                                custCityObj = item.OrderTrans?.Abbredemption?.CustomerDetails != null ? item.OrderTrans?.Abbredemption?.CustomerDetails?.City : null;
                            }
                            lGCOrderViewModel = new LGCOrderViewModel();
                            lGCOrderViewModel.Id = item.OrderLgcid;
                            lGCOrderViewModel.Action = actionURL;
                            lGCOrderViewModel.RegdNo = item.Logistic?.RegdNo;
                            lGCOrderViewModel.TicketNumber = item.Logistic?.TicketNumber;
                            lGCOrderViewModel.ProductCategory = productCatDesc;
                            lGCOrderViewModel.ProductType = productCatTypeDesc;
                            lGCOrderViewModel.AmountPayableThroughLGC = Convert.ToDecimal(item.Logistic.AmtPaybleThroughLgc);
                            lGCOrderViewModel.CreatedDate = Convert.ToDateTime(item.Logistic.CreatedDate).ToString("MM/dd/yyyy");
                            lGCOrderList.Add(lGCOrderViewModel);
                        }
                    }
                }
                #endregion

                var data = lGCOrderList;
                var jsonData = new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data };
                return Ok(jsonData);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        #endregion

        #region Get List of Drivers which is Load the orders (Driver List)
        public IActionResult GetListofDriversLoad(int userId, DateTime? orderStartDate, DateTime? orderEndDate,
   int? productCatId, int? productTypeId, string? regdNo, string? ticketNo, string? custCity)
        {
            #region Variable Declaration
            List<TblOrderLgc> tblOrderLgc = null;
            List<LGCOrderViewModel> lGCOrderList = null;
            LGCOrderViewModel lGCOrderViewModel = null;
            TblServicePartner tblServicePartner = null;
            string URL = _config.Value.URLPrefixforProd;
            #endregion

            #region For Advanced filters
            if (string.IsNullOrEmpty(regdNo) || regdNo == "null")
            { regdNo = null; }
            else
            { regdNo = regdNo.Trim().ToLower(); }
            if (string.IsNullOrEmpty(ticketNo) || ticketNo == "null")
            { ticketNo = null; }
            else
            { ticketNo = ticketNo.Trim().ToLower(); }
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
                searchValue = searchValue.Trim();
                int pageSize = length != null ? Convert.ToInt32(length) : 0;
                int skip = start != null ? Convert.ToInt32(start) : 0;
                int recordsTotal = 0;
                int count = 0;
                #endregion

                #region Advanced Filters
                if (orderStartDate != null && orderEndDate != null)
                {
                    orderStartDate = Convert.ToDateTime(orderStartDate).AddMinutes(-1);
                    orderEndDate = Convert.ToDateTime(orderEndDate).AddDays(1).AddSeconds(-1);
                }
                #endregion

                tblServicePartner = _context.TblServicePartners.Where(x => x.UserId == userId && x.IsActive == true).FirstOrDefault();
                if (tblServicePartner != null)
                {
                    count = _context.TblOrderLgcs
                        .Include(x => x.Logistic).ThenInclude(x => x.OrderTrans)
                        .Include(x => x.DriverDetails).Include(x => x.Evcregistration).ThenInclude(x => x.City)
                        .Include(x => x.Evcpartner)
                        .Include(x => x.Logistic).ThenInclude(x => x.ServicePartner).Where(x => x.Logistic.ServicePartner.UserId == userId).AsEnumerable()
                        .Count(x => x.IsActive == true && x.DriverDetailsId != null && x.EvcregistrationId != null && x.StatusId == Convert.ToInt32(OrderStatusEnum.LGCPickup)
                        && (x.Logistic != null && x.Logistic.ServicePartnerId == tblServicePartner.ServicePartnerId)
                        && ((orderStartDate == null && orderEndDate == null) || (x.DriverDetails.CreatedDate >= orderStartDate && x.DriverDetails.CreatedDate <= orderEndDate))
                        && (string.IsNullOrEmpty(searchValue)
                        || x.Logistic.RegdNo.ToLower().Contains(searchValue.ToLower())
                        || (x.DriverDetails == null ? false :
                        ((x.DriverDetails.DriverName ?? "").ToLower().Contains(searchValue.ToLower())
                        || (x.DriverDetails.DriverPhoneNumber ?? "").ToLower().Contains(searchValue.ToLower())
                        || (x.DriverDetails.VehicleNumber ?? "").ToLower().Contains(searchValue.ToLower())
                        || (x.DriverDetails.City ?? "").ToLower().Contains(searchValue.ToLower())
                        ))
                        || (x.Evcregistration == null ? false :
                        ((x.Evcregistration.BussinessName ?? "").ToLower().Contains(searchValue.ToLower())))
                        || (searchValue.Contains("/") && Convert.ToDateTime(x.DriverDetails.CreatedDate).ToString("MM/dd/yyyy").Contains(searchValue))
                        ));
                    if (count > 0)
                    {
                        tblOrderLgc = _context.TblOrderLgcs
                        .Include(x => x.Logistic).ThenInclude(x => x.OrderTrans)
                        .Include(x => x.DriverDetails).Include(x => x.Evcregistration).ThenInclude(x => x.City)
                        .Include(x => x.Evcpartner)
                        .Include(x => x.Logistic).ThenInclude(x => x.ServicePartner).Where(x => x.Logistic.ServicePartner.UserId == userId).AsEnumerable()
                        .Where(x => x.IsActive == true && x.DriverDetailsId != null && x.EvcregistrationId != null && x.StatusId == Convert.ToInt32(OrderStatusEnum.LGCPickup)
                        && (x.Logistic != null && x.Logistic.ServicePartnerId == tblServicePartner.ServicePartnerId)
                        && ((orderStartDate == null && orderEndDate == null) || (x.DriverDetails.CreatedDate >= orderStartDate && x.DriverDetails.CreatedDate <= orderEndDate))
                        && (string.IsNullOrEmpty(searchValue)
                        || x.Logistic.RegdNo.ToLower().Contains(searchValue.ToLower())
                        || (x.DriverDetails == null ? false :
                        ((x.DriverDetails.DriverName ?? "").ToLower().Contains(searchValue.ToLower())
                        || (x.DriverDetails.DriverPhoneNumber ?? "").ToLower().Contains(searchValue.ToLower())
                        || (x.DriverDetails.VehicleNumber ?? "").ToLower().Contains(searchValue.ToLower())
                        || (x.DriverDetails.City ?? "").ToLower().Contains(searchValue.ToLower())
                        ))
                        || (x.Evcregistration == null ? false :
                        ((x.Evcregistration.BussinessName ?? "").ToLower().Contains(searchValue.ToLower())))
                        || (searchValue.Contains("/") && Convert.ToDateTime(x.DriverDetails.CreatedDate).ToString("MM/dd/yyyy").Contains(searchValue))
                        )).ToList();
                    }
                }

                tblOrderLgc = tblOrderLgc.GroupBy(x => x.DriverDetailsId).Select(x => x.FirstOrDefault()).ToList();
                tblOrderLgc = tblOrderLgc.OrderByDescending(x => x.ModifiedDate).ToList();
                recordsTotal = tblOrderLgc != null ? tblOrderLgc.Count : 0;
                if (tblOrderLgc != null)
                {
                    tblOrderLgc = tblOrderLgc.Skip(skip).Take(pageSize).ToList();
                }
                lGCOrderList = new List<LGCOrderViewModel>();
                if (tblOrderLgc != null && tblOrderLgc.Count > 0)
                {
                    foreach (var item in tblOrderLgc)
                    {
                        string actionURL = string.Empty;
                        var regdno = item.Logistic.RegdNo;
                        actionURL = " <ul class='actions'>";
                        actionURL = "<a href ='" + URL + "/LGC/LGCDropDetails?DriverDetailsId=" + item.DriverDetailsId + "&EVCPartnerId=" + item.EvcpartnerId + "' ><button onclick='View(" + item.Logistic.RegdNo + ")' class='btn btn-primary btn'>View</button></a>";
                        actionURL = actionURL + "</ul>";

                        if (item != null)
                        {
                            lGCOrderViewModel = new LGCOrderViewModel();
                            lGCOrderViewModel.Id = item.EvcregistrationId;
                            lGCOrderViewModel.Action = actionURL;
                            lGCOrderViewModel.DriverName = item.DriverDetails?.DriverName;
                            lGCOrderViewModel.DriverPhoneNumber = item.DriverDetails?.DriverPhoneNumber;
                            lGCOrderViewModel.VehicleNumber = item.DriverDetails?.VehicleNumber;
                            lGCOrderViewModel.City = item.Evcpartner?.City?.Name;
                            lGCOrderViewModel.EVCBusinessName = item.Evcregistration?.BussinessName;
                            lGCOrderViewModel.EvcStoreCode = item.Evcpartner?.EvcStoreCode;
                            lGCOrderViewModel.ModifiedDate = item.ModifiedDate;
                            lGCOrderViewModel.CreatedDate = Convert.ToDateTime(item.DriverDetails?.CreatedDate).ToString("MM/dd/yyyy");
                            lGCOrderList.Add(lGCOrderViewModel);
                        }
                    }
                }
                lGCOrderList = lGCOrderList.Distinct().ToList();
                //lGCOrderList = lGCOrderList.GroupBy(x => x.Id).Select(x => x.FirstOrDefault()).ToList();
                var data = lGCOrderList.OrderByDescending(x => x.ModifiedDate);
                var jsonData = new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data };
                return Ok(jsonData);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        //public IActionResult GetListofDriversLoad(int userId, DateTime? orderStartDate, DateTime? orderEndDate,
        //   int? productCatId, int? productTypeId, string? regdNo, string? ticketNo, string? custCity)
        //{
        //    #region Variable Declaration
        //    List<TblOrderLgc> tblOrderLgc = null;
        //    List<LGCOrderViewModel> lGCOrderList = null;
        //    LGCOrderViewModel lGCOrderViewModel = null;
        //    TblServicePartner tblServicePartner = null;
        //    string URL = _config.Value.URLPrefixforProd;
        //    #endregion

        //    try
        //    {
        //        #region Datatable Variables
        //        var draw = Request.Form["draw"].FirstOrDefault();
        //        var start = Request.Form["start"].FirstOrDefault();
        //        var length = Request.Form["length"].FirstOrDefault();
        //        var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
        //        var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();
        //        var searchValue = Request.Form["search[value]"].FirstOrDefault();
        //        searchValue = searchValue.Trim();
        //        int pageSize = length != null ? Convert.ToInt32(length) : 0;
        //        int skip = start != null ? Convert.ToInt32(start) : 0;
        //        int recordsTotal = 0;
        //        #endregion

        //        tblServicePartner = _context.TblServicePartners.Where(x => x.UserId == userId && x.IsActive == true).FirstOrDefault();
        //        if (tblServicePartner != null)
        //        {
        //            if (orderStartDate == null && orderEndDate == null)
        //            {
        //                tblOrderLgc = _context.TblOrderLgcs
        //                .Include(x => x.Logistic).ThenInclude(x => x.OrderTrans)
        //                .Include(x => x.DriverDetails).Include(x => x.Evcregistration).ThenInclude(x => x.City)
        //                .Include(x => x.Evcpartner)
        //                .Include(x => x.Logistic).ThenInclude(x => x.ServicePartner).AsEnumerable()
        //                .Where(x => x.IsActive == true && x.DriverDetailsId != null && x.EvcregistrationId != null && x.StatusId == Convert.ToInt32(OrderStatusEnum.LGCPickup)
        //                && (x.Logistic != null && x.Logistic.ServicePartnerId == tblServicePartner.ServicePartnerId)
        //                && (string.IsNullOrEmpty(searchValue)
        //                || x.Logistic.RegdNo.ToLower().Contains(searchValue.ToLower())
        //                || (x.DriverDetails == null ? false :
        //                ((x.DriverDetails.DriverName ?? "").ToLower().Contains(searchValue.ToLower())
        //                || (x.DriverDetails.DriverPhoneNumber ?? "").ToLower().Contains(searchValue.ToLower())
        //                || (x.DriverDetails.VehicleNumber ?? "").ToLower().Contains(searchValue.ToLower())
        //                ))
        //                || (x.Evcregistration == null ? false :
        //                ((x.Evcregistration.BussinessName ?? "").ToLower().Contains(searchValue.ToLower()))
        //                || ((x.Evcregistration.City.Name ?? "").ToLower().Contains(searchValue.ToLower()))
        //                )
        //                || (searchValue.Contains("/") && Convert.ToDateTime(x.DriverDetails.CreatedDate).ToString("MM/dd/yyyy").Contains(searchValue))
        //                )).ToList();
        //            }
        //            else
        //            {
        //                orderStartDate = Convert.ToDateTime(orderStartDate).AddMinutes(-1);
        //                orderEndDate = Convert.ToDateTime(orderEndDate).AddDays(1).AddSeconds(-1);

        //                tblOrderLgc = _context.TblOrderLgcs
        //                .Include(x => x.Logistic).ThenInclude(x => x.OrderTrans)
        //                .Include(x => x.DriverDetails).Include(x => x.Evcregistration).ThenInclude(x => x.City)
        //                .Include(x => x.Evcpartner)
        //                .Include(x => x.Logistic).ThenInclude(x => x.ServicePartner).Where(x => x.Logistic.ServicePartner.UserId == userId).AsEnumerable()
        //                .Where(x => x.IsActive == true && x.DriverDetailsId != null && x.EvcregistrationId != null && x.StatusId == Convert.ToInt32(OrderStatusEnum.LGCPickup)
        //                && (x.Logistic != null && x.Logistic.ServicePartnerId == tblServicePartner.ServicePartnerId)
        //                && x.DriverDetails.CreatedDate >= orderStartDate && x.DriverDetails.CreatedDate <= orderEndDate
        //                && (string.IsNullOrEmpty(searchValue)
        //                || x.Logistic.RegdNo.ToLower().Contains(searchValue.ToLower())
        //                || (x.DriverDetails == null ? false :
        //                ((x.DriverDetails.DriverName ?? "").ToLower().Contains(searchValue.ToLower())
        //                || (x.DriverDetails.DriverPhoneNumber ?? "").ToLower().Contains(searchValue.ToLower())
        //                || (x.DriverDetails.VehicleNumber ?? "").ToLower().Contains(searchValue.ToLower())
        //                || (x.DriverDetails.City ?? "").ToLower().Contains(searchValue.ToLower())
        //                ))
        //                || (x.Evcregistration == null ? false :
        //                ((x.Evcregistration.BussinessName ?? "").ToLower().Contains(searchValue.ToLower())))
        //                || (searchValue.Contains("/") && Convert.ToDateTime(x.DriverDetails.CreatedDate).ToString("MM/dd/yyyy").Contains(searchValue))
        //                )).ToList();
        //            }
        //        }

        //        tblOrderLgc = tblOrderLgc.GroupBy(x => x.DriverDetailsId).Select(x => x.FirstOrDefault()).ToList();
        //        tblOrderLgc = tblOrderLgc.OrderByDescending(x => x.ModifiedDate).ToList();
        //        recordsTotal = tblOrderLgc != null ? tblOrderLgc.Count : 0;
        //        if (tblOrderLgc != null)
        //        {
        //            tblOrderLgc = tblOrderLgc.Skip(skip).Take(pageSize).ToList();
        //        }
        //        lGCOrderList = new List<LGCOrderViewModel>();
        //        if (tblOrderLgc != null && tblOrderLgc.Count > 0)
        //        {
        //            foreach (var item in tblOrderLgc)
        //            {
        //                string actionURL = string.Empty;
        //                var regdno = item.Logistic.RegdNo;
        //                actionURL = " <ul class='actions'>";
        //                actionURL = "<a href ='" + URL + "/LGC/LGCDropDetails?DriverDetailsId=" + item.DriverDetailsId + "&EVCPartnerId=" + item.EvcpartnerId + "' ><button onclick='View(" + item.Logistic.RegdNo + ")' class='btn btn-primary btn'>View</button></a>";
        //                actionURL = actionURL + "</ul>";

        //                if (item != null)
        //                {
        //                    lGCOrderViewModel = new LGCOrderViewModel();
        //                    lGCOrderViewModel.Id = item.EvcregistrationId;
        //                    lGCOrderViewModel.Action = actionURL;
        //                    lGCOrderViewModel.DriverName = item.DriverDetails?.DriverName;
        //                    lGCOrderViewModel.DriverPhoneNumber = item.DriverDetails?.DriverPhoneNumber;
        //                    lGCOrderViewModel.VehicleNumber = item.DriverDetails?.VehicleNumber;
        //                    lGCOrderViewModel.City = item.Evcpartner?.City?.Name;
        //                    lGCOrderViewModel.EVCBusinessName = item.Evcregistration?.BussinessName;
        //                    lGCOrderViewModel.EvcStoreCode = item.Evcpartner?.EvcStoreCode;
        //                    lGCOrderViewModel.ModifiedDate = item.ModifiedDate;
        //                    lGCOrderViewModel.CreatedDate = Convert.ToDateTime(item.DriverDetails?.CreatedDate).ToString("MM/dd/yyyy");
        //                    lGCOrderList.Add(lGCOrderViewModel);
        //                }
        //            }
        //        }
        //        lGCOrderList = lGCOrderList.Distinct().ToList();
        //        //lGCOrderList = lGCOrderList.GroupBy(x => x.Id).Select(x => x.FirstOrDefault()).ToList();
        //        var data = lGCOrderList.OrderByDescending(x => x.ModifiedDate);
        //        var jsonData = new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data };
        //        return Ok(jsonData);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw;
        //    }
        //}
        #endregion


        #region Get List of Orders by EVC (Create Load)
        //public IActionResult GetRegdnoListByEvc(int userId, int evcRegId, DateTime? startDate, DateTime? endDate)
        //{
        //    List<TblOrderLgc> tblOrderLgc = null;
        //    List<LGCOrderViewModel> lGCOrderList = null;
        //    LGCOrderViewModel lGCOrderViewModel = null;
        //    TblServicePartner tblServicePartner = null;
        //    string URL = _config.Value.URLPrefixforProd;

        //    try
        //    {
        //        #region datatable variable
        //        var draw = Request.Form["draw"].FirstOrDefault();
        //        var start = this.Request.Form["start"].FirstOrDefault();
        //        var length = this.Request.Form["length"].FirstOrDefault();
        //        var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
        //        var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();
        //        var searchValue = Request.Form["search[value]"].FirstOrDefault();
        //        searchValue = searchValue.Trim();
        //        int pageSize = length != null ? Convert.ToInt32(length) : 0;
        //        int skip = start != null ? Convert.ToInt32(start) : 0;
        //        int recordsTotal = 0; int count = 0;
        //        #endregion

        //        #region Advanced Filters
        //        if (startDate != null && endDate != null)
        //        {
        //            startDate = Convert.ToDateTime(startDate).AddMinutes(-1);
        //            endDate = Convert.ToDateTime(endDate).AddDays(1).AddSeconds(-1);
        //        }
        //        #endregion

        //        #region TblOrderLgcs Initialization
        //        tblServicePartner = _context.TblServicePartners.Where(x => x.IsActive == true && x.UserId == userId).FirstOrDefault();
        //        if (tblServicePartner != null && evcRegId > 0)
        //        {
        //            count = _context.TblOrderLgcs.Include(x => x.OrderTrans).Include(x => x.Logistic)
        //                .Include(x => x.OrderTrans).ThenInclude(x => x.Exchange).ThenInclude(x => x.ProductType).ThenInclude(x => x.ProductCat)
        //                .Include(x => x.OrderTrans).ThenInclude(x => x.Exchange).ThenInclude(x => x.CustomerDetails)
        //                .Include(x => x.OrderTrans).ThenInclude(x => x.Abbredemption).ThenInclude(x => x.Abbregistration).ThenInclude(x => x.NewProductCategory)
        //                .Include(x => x.OrderTrans).ThenInclude(x => x.Abbredemption).ThenInclude(x => x.Abbregistration).ThenInclude(x => x.NewProductCategoryTypeNavigation)
        //                .Include(x => x.OrderTrans).ThenInclude(x => x.Abbredemption).ThenInclude(x => x.CustomerDetails)
        //                .Count(x => x.IsActive == true && x.StatusId == 23 && x.OrderTrans != null
        //                && (x.Logistic != null && x.Logistic.ServicePartnerId == tblServicePartner.ServicePartnerId)
        //                && x.EvcpartnerId == evcRegId && x.DriverDetailsId == null
        //                && ((startDate == null && endDate == null) || (x.Logistic.CreatedDate >= startDate && x.Logistic.CreatedDate <= endDate))
        //                && (((x.Logistic.TicketNumber ?? "").ToLower().Contains(searchValue.ToLower()))
        //                    || ((x.OrderTrans.RegdNo ?? "").ToLower().Contains(searchValue.ToLower()))
        //                    || ((x.OrderTrans.Exchange != null && x.OrderTrans.Exchange.ProductType != null && x.OrderTrans.Exchange.ProductType.ProductCat != null && x.OrderTrans.Exchange.CustomerDetails != null &&
        //                        ((x.OrderTrans.Exchange.ProductType.Description ?? "").ToLower().Contains(searchValue.ToLower())
        //                        || (x.OrderTrans.Exchange.ProductType.ProductCat.Description ?? "").ToLower().Contains(searchValue.ToLower())
        //                        || (x.OrderTrans.Exchange.CustomerDetails.City ?? "").ToLower().Contains(searchValue.ToLower())
        //                        ))
        //                        || (x.OrderTrans.Abbredemption != null && x.OrderTrans.Abbredemption.Abbregistration != null && x.OrderTrans.Abbredemption.Abbregistration.NewProductCategory != null && x.OrderTrans.Abbredemption.Abbregistration.NewProductCategoryTypeNavigation != null && x.OrderTrans.Abbredemption.CustomerDetails != null &&
        //                        ((x.OrderTrans.Abbredemption.Abbregistration.NewProductCategoryTypeNavigation.Description ?? "").ToLower().Contains(searchValue.ToLower())
        //                        || (x.OrderTrans.Abbredemption.Abbregistration.NewProductCategory.Description ?? "").ToLower().Contains(searchValue.ToLower())
        //                        || (x.OrderTrans.Abbredemption.CustomerDetails.City ?? "").ToLower().Contains(searchValue.ToLower())
        //                        ))
        //                       )
        //                    ));

        //            if (count > 0)
        //            {
        //                tblOrderLgc = _context.TblOrderLgcs.Include(x => x.OrderTrans).Include(x => x.Logistic)
        //                .Include(x => x.OrderTrans).ThenInclude(x => x.Exchange).ThenInclude(x => x.ProductType).ThenInclude(x => x.ProductCat)
        //                .Include(x => x.OrderTrans).ThenInclude(x => x.Exchange).ThenInclude(x => x.CustomerDetails)
        //                .Include(x => x.OrderTrans).ThenInclude(x => x.Abbredemption).ThenInclude(x => x.Abbregistration).ThenInclude(x => x.NewProductCategory)
        //                .Include(x => x.OrderTrans).ThenInclude(x => x.Abbredemption).ThenInclude(x => x.Abbregistration).ThenInclude(x => x.NewProductCategoryTypeNavigation)
        //                .Include(x => x.OrderTrans).ThenInclude(x => x.Abbredemption).ThenInclude(x => x.CustomerDetails)
        //                .Where(x => x.IsActive == true && x.StatusId == 23 && x.OrderTrans != null
        //                && (x.Logistic != null && x.Logistic.ServicePartnerId == tblServicePartner.ServicePartnerId)
        //                && x.EvcpartnerId == evcRegId && x.DriverDetailsId == null
        //                && ((startDate == null && endDate == null) || (x.Logistic.CreatedDate >= startDate && x.Logistic.CreatedDate <= endDate))
        //                && (((x.Logistic.TicketNumber ?? "").ToLower().Contains(searchValue.ToLower()))
        //                    || ((x.OrderTrans.RegdNo ?? "").ToLower().Contains(searchValue.ToLower()))
        //                    || ((x.OrderTrans.Exchange != null && x.OrderTrans.Exchange.ProductType != null && x.OrderTrans.Exchange.ProductType.ProductCat != null && x.OrderTrans.Exchange.CustomerDetails != null &&
        //                        ((x.OrderTrans.Exchange.ProductType.Description ?? "").ToLower().Contains(searchValue.ToLower())
        //                        || (x.OrderTrans.Exchange.ProductType.ProductCat.Description ?? "").ToLower().Contains(searchValue.ToLower())
        //                        || (x.OrderTrans.Exchange.CustomerDetails.City ?? "").ToLower().Contains(searchValue.ToLower())
        //                        ))
        //                        || (x.OrderTrans.Abbredemption != null && x.OrderTrans.Abbredemption.Abbregistration != null && x.OrderTrans.Abbredemption.Abbregistration.NewProductCategory != null && x.OrderTrans.Abbredemption.Abbregistration.NewProductCategoryTypeNavigation != null && x.OrderTrans.Abbredemption.CustomerDetails != null &&
        //                        ((x.OrderTrans.Abbredemption.Abbregistration.NewProductCategoryTypeNavigation.Description ?? "").ToLower().Contains(searchValue.ToLower())
        //                        || (x.OrderTrans.Abbredemption.Abbregistration.NewProductCategory.Description ?? "").ToLower().Contains(searchValue.ToLower())
        //                        || (x.OrderTrans.Abbredemption.CustomerDetails.City ?? "").ToLower().Contains(searchValue.ToLower())
        //                        ))
        //                       )
        //                    )).OrderByDescending(x => x.ModifiedDate).Skip(skip).Take(pageSize).ToList();
        //            }
        //            recordsTotal = count;
        //        }
        //        #endregion

        //        #region Model Mapping
        //        lGCOrderList = new List<LGCOrderViewModel>();
        //        if (tblOrderLgc != null)
        //        {
        //            foreach (var item in tblOrderLgc)
        //            {
        //                string actionURL = string.Empty;
        //                var regdno = item.OrderTrans.RegdNo;
        //                actionURL = " <td class='actions'>";
        //                actionURL = actionURL + " <span><input type='checkbox' value=" + item.OrderTransId + " class='checkboxinput' /></span>";
        //                actionURL = actionURL + " </td>";

        //                if (item != null)
        //                {
        //                    string productCatDesc = null; string productCatTypeDesc = null; string custCity = null;
        //                    if (item.OrderTrans.Exchange != null && item.OrderTrans.Exchange.ProductType != null)
        //                    {
        //                        productCatDesc = item.OrderTrans.Exchange.ProductType.ProductCat != null ? item.OrderTrans.Exchange.ProductType.ProductCat.Description : null;
        //                        productCatTypeDesc = item.OrderTrans.Exchange.ProductType.Description;
        //                        custCity = item.OrderTrans.Exchange.CustomerDetails != null ? item.OrderTrans.Exchange.CustomerDetails.City : null;
        //                    }
        //                    else if (item.OrderTrans.Abbredemption != null && item.OrderTrans.Abbredemption.Abbregistration != null)
        //                    {
        //                        productCatDesc = item.OrderTrans.Abbredemption.Abbregistration.NewProductCategory != null ? item.OrderTrans.Abbredemption.Abbregistration.NewProductCategory.Description : null;
        //                        productCatTypeDesc = item.OrderTrans.Abbredemption.Abbregistration.NewProductCategoryTypeNavigation != null ? item.OrderTrans.Abbredemption.Abbregistration.NewProductCategoryTypeNavigation.Description : null;
        //                        //custCity = item.OrderTrans.Abbredemption.CustomerDetails != null ? item.OrderTrans.Abbredemption.CustomerDetails.City : null;
        //                    }

        //                    lGCOrderViewModel = new LGCOrderViewModel();
        //                    lGCOrderViewModel.Id = item.OrderLgcid;
        //                    lGCOrderViewModel.Action = actionURL;
        //                    lGCOrderViewModel.RegdNo = item.OrderTrans.RegdNo;
        //                    lGCOrderViewModel.TicketNumber = item.Logistic.TicketNumber;
        //                    lGCOrderViewModel.ProductCategory = productCatDesc;
        //                    lGCOrderViewModel.ProductType = productCatTypeDesc;
        //                    lGCOrderViewModel.CreatedDate = Convert.ToDateTime(item.Logistic.CreatedDate).ToString("MM/dd/yyyy");
        //                    lGCOrderList.Add(lGCOrderViewModel);
        //                }
        //            }
        //        }
        //        #endregion

        //        var data = lGCOrderList;
        //        var jsonData = new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data };
        //        return Ok(jsonData);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw;
        //    }
        //}

        public IActionResult GetRegdnoListByEvc(int userId, int evcRegId, DateTime? orderStartDate, DateTime? orderEndDate,
           int? productCatId, int? productTypeId, string? regdNo, string? ticketNo, string? custCity)
        {
            List<TblOrderLgc>? tblOrderLgc = null;
            List<LGCOrderViewModel>? lGCOrderList = null;
            LGCOrderViewModel? lGCOrderViewModel = null;
            TblServicePartner? tblServicePartner = null;
            string? URL = _config.Value.URLPrefixforProd;

            try
            {
                #region datatable variable
                var draw = Request.Form["draw"].FirstOrDefault();
                var start = this.Request.Form["start"].FirstOrDefault();
                var length = this.Request.Form["length"].FirstOrDefault();
                var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
                var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();
                var searchValue = Request.Form["search[value]"].FirstOrDefault();
                searchValue = searchValue.Trim();
                int pageSize = length != null ? Convert.ToInt32(length) : 0;
                int skip = start != null ? Convert.ToInt32(start) : 0;
                int recordsTotal = 0; int count = 0;
                #endregion

                #region Advanced Filters
                if (orderStartDate != null && orderEndDate != null)
                {
                    orderStartDate = Convert.ToDateTime(orderStartDate).AddMinutes(-1);
                    orderEndDate = Convert.ToDateTime(orderEndDate).AddDays(1).AddSeconds(-1);
                }
                #endregion

                #region TblOrderLgcs Initialization
                tblServicePartner = _context.TblServicePartners.Where(x => x.IsActive == true && x.UserId == userId).FirstOrDefault();
                if (tblServicePartner != null && evcRegId > 0)
                {
                    count = _context.TblOrderLgcs.Include(x => x.OrderTrans).Include(x => x.Logistic)
                        .Include(x => x.OrderTrans).ThenInclude(x => x.Exchange).ThenInclude(x => x.ProductType).ThenInclude(x => x.ProductCat)
                        .Include(x => x.OrderTrans).ThenInclude(x => x.Exchange).ThenInclude(x => x.CustomerDetails)
                        .Include(x => x.OrderTrans).ThenInclude(x => x.Abbredemption).ThenInclude(x => x.Abbregistration).ThenInclude(x => x.NewProductCategory)
                        .Include(x => x.OrderTrans).ThenInclude(x => x.Abbredemption).ThenInclude(x => x.Abbregistration).ThenInclude(x => x.NewProductCategoryTypeNavigation)
                        .Include(x => x.OrderTrans).ThenInclude(x => x.Abbredemption).ThenInclude(x => x.CustomerDetails)
                        .Count(x => x.IsActive == true && x.StatusId == 23 && x.OrderTrans != null
                        && (x.Logistic != null && x.Logistic.ServicePartnerId == tblServicePartner.ServicePartnerId)
                        && x.EvcpartnerId == evcRegId && x.DriverDetailsId == null
                        && ((orderStartDate == null && orderEndDate == null) || (x.Logistic.CreatedDate >= orderStartDate && x.Logistic.CreatedDate <= orderEndDate))
                        && (string.IsNullOrEmpty(ticketNo) || (x.Logistic.TicketNumber ?? "").ToLower() == ticketNo)
                                    && (string.IsNullOrEmpty(regdNo) || (x.OrderTrans.RegdNo ?? "").ToLower() == regdNo)
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
                        tblOrderLgc = _context.TblOrderLgcs.Include(x => x.OrderTrans).Include(x => x.Logistic)
                        .Include(x => x.OrderTrans).ThenInclude(x => x.Exchange).ThenInclude(x => x.ProductType).ThenInclude(x => x.ProductCat)
                        .Include(x => x.OrderTrans).ThenInclude(x => x.Exchange).ThenInclude(x => x.CustomerDetails)
                        .Include(x => x.OrderTrans).ThenInclude(x => x.Abbredemption).ThenInclude(x => x.Abbregistration).ThenInclude(x => x.NewProductCategory)
                        .Include(x => x.OrderTrans).ThenInclude(x => x.Abbredemption).ThenInclude(x => x.Abbregistration).ThenInclude(x => x.NewProductCategoryTypeNavigation)
                        .Include(x => x.OrderTrans).ThenInclude(x => x.Abbredemption).ThenInclude(x => x.CustomerDetails)
                        .Where(x => x.IsActive == true && x.StatusId == 23 && x.OrderTrans != null
                        && (x.Logistic != null && x.Logistic.ServicePartnerId == tblServicePartner.ServicePartnerId)
                        && x.EvcpartnerId == evcRegId && x.DriverDetailsId == null
                        && ((orderStartDate == null && orderEndDate == null) || (x.Logistic.CreatedDate >= orderStartDate && x.Logistic.CreatedDate <= orderEndDate))
                        && (string.IsNullOrEmpty(ticketNo) || (x.Logistic.TicketNumber ?? "").ToLower() == ticketNo)
                                    && (string.IsNullOrEmpty(regdNo) || (x.OrderTrans.RegdNo ?? "").ToLower() == regdNo)
                                    && (x.OrderTrans != null &&
                                    ((x.OrderTrans.Exchange != null
                                    && (x.OrderTrans.Exchange.ProductType != null && x.OrderTrans.Exchange.ProductType.ProductCat != null)
                                    && (productCatId == null || x.OrderTrans.Exchange.ProductType.ProductCatId == productCatId)
                                    && (productTypeId == null || x.OrderTrans.Exchange.ProductTypeId == productTypeId))
                                    || (x.OrderTrans.Abbredemption != null && x.OrderTrans.Abbredemption.Abbregistration != null
                                    && (x.OrderTrans.Abbredemption.Abbregistration.NewProductCategoryTypeNavigation != null && x.OrderTrans.Abbredemption.Abbregistration.NewProductCategory != null)
                                    && (productCatId == null || x.OrderTrans.Abbredemption.Abbregistration.NewProductCategoryId == productCatId)
                                    && (productTypeId == null || x.OrderTrans.Abbredemption.Abbregistration.NewProductCategoryTypeId == productTypeId))
                                    ))).OrderByDescending(x => x.ModifiedDate).Skip(skip).Take(pageSize).ToList();
                    }
                    recordsTotal = count;
                }
                #endregion

                #region Model Mapping
                lGCOrderList = new List<LGCOrderViewModel>();
                if (tblOrderLgc != null)
                {
                    foreach (var item in tblOrderLgc)
                    {
                        string actionURL = string.Empty;
                        var regdno = item.OrderTrans.RegdNo;
                        actionURL = " <td class='actions'>";
                        actionURL = actionURL + " <span><input type='checkbox' value=" + item.OrderTransId + " class='checkboxinput' /></span>";
                        actionURL = actionURL + " </td>";

                        if (item != null)
                        {
                            string? productCatDesc = null; string? productCatTypeDesc = null; string? custCityObj = null;
                            if (item.OrderTrans.Exchange != null && item.OrderTrans.Exchange.ProductType != null)
                            {
                                productCatDesc = item.OrderTrans.Exchange.ProductType.ProductCat != null ? item.OrderTrans.Exchange.ProductType.ProductCat.Description : null;
                                productCatTypeDesc = item.OrderTrans.Exchange.ProductType.Description;
                                custCityObj = item.OrderTrans.Exchange.CustomerDetails != null ? item.OrderTrans.Exchange.CustomerDetails.City : null;
                            }
                            else if (item.OrderTrans.Abbredemption != null && item.OrderTrans.Abbredemption.Abbregistration != null)
                            {
                                productCatDesc = item.OrderTrans.Abbredemption.Abbregistration.NewProductCategory != null ? item.OrderTrans.Abbredemption.Abbregistration.NewProductCategory.Description : null;
                                productCatTypeDesc = item.OrderTrans.Abbredemption.Abbregistration.NewProductCategoryTypeNavigation != null ? item.OrderTrans.Abbredemption.Abbregistration.NewProductCategoryTypeNavigation.Description : null;
                                //custCityObj = item.OrderTrans.Abbredemption.CustomerDetails != null ? item.OrderTrans.Abbredemption.CustomerDetails.City : null;
                            }

                            lGCOrderViewModel = new LGCOrderViewModel();
                            lGCOrderViewModel.Id = item.OrderLgcid;
                            lGCOrderViewModel.Action = actionURL;
                            lGCOrderViewModel.RegdNo = item.OrderTrans.RegdNo;
                            lGCOrderViewModel.TicketNumber = item.Logistic.TicketNumber;
                            lGCOrderViewModel.ProductCategory = productCatDesc;
                            lGCOrderViewModel.ProductType = productCatTypeDesc;
                            lGCOrderViewModel.CreatedDate = Convert.ToDateTime(item.Logistic.CreatedDate).ToString("MM/dd/yyyy");
                            lGCOrderList.Add(lGCOrderViewModel);
                        }
                    }
                }
                #endregion

                var data = lGCOrderList;
                var jsonData = new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data };
                return Ok(jsonData);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        #endregion
        #endregion

        //[HttpPost]
        //public async Task<ActionResult> GetTimeLineList(int orderTransId)
        //{
        //    string URL = _config.Value.URLPrefixforProd;
        //    List<TblExchangeAbbstatusHistory> tblExchangeABBStatusHistory = null;
        //    List<TblCustomerDetail> TblCustomerDetails = null;
        //    List<TblExchangeOrder> TblExchangeOrders = null;
        //    List<TblUser> TblUser = null;
        //    List<TblExchangeOrderStatus> TblExchangeOrderStatus = null;
        //    List<TblTimelineStatusMapping> TblTimelineStatusMappings = null;
        //    List<TblTimeLine> TblTimeLines = null;
        //    List<TimeListViewModel> TimeList = null;
        //    TimeListViewModel timeListViewModel = null;

        //public IActionResult GetListofDriversLoad(int userId)
        //{
        //    List<TblOrderLgc> tblOrderLgc = null;
        //    List<LGCOrderViewModel> lGCOrderList = null;
        //    LGCOrderViewModel lGCOrderViewModel = null;
        //    string URL = _config.Value.URLPrefixforProd;


        //    try
        //    {
        //        var draw = Request.Form["draw"].FirstOrDefault();
        //        var start = Request.Form["start"].FirstOrDefault();
        //        var length = Request.Form["length"].FirstOrDefault();
        //        var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
        //        var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();
        //        var searchValue = Request.Form["search[value]"].FirstOrDefault();
        //        int pageSize = length != null ? Convert.ToInt32(length) : 0;
        //        int skip = start != null ? Convert.ToInt32(start) : 0;
        //        int recordsTotal = 0;
        //        _context = new Digi2l_DevContext();
        //        tblExchangeABBStatusHistory = await _context.TblExchangeAbbstatusHistories.Where(x => x.IsActive == true && x.OrderTransId == orderTransId
        //               && (string.IsNullOrEmpty(searchValue) || x.RegdNo.ToLower().Contains(searchValue.ToLower())))
        //                .Include(x => x.OrderTrans).ThenInclude(x => x.Exchange).ThenInclude(x => x.CustomerDetails)
        //                .Include(x => x.OrderTrans).ThenInclude(x => x.Exchange).ToListAsync();


        //        recordsTotal = tblExchangeABBStatusHistory != null ? tblExchangeABBStatusHistory.Count : 0;
        //        if (tblExchangeABBStatusHistory != null)
        //        {
        //            tblExchangeABBStatusHistory = sortColumnDirection.Equals(SortingOrder.ASCENDING) ? tblExchangeABBStatusHistory.OrderBy(o => o.GetType().GetProperty(sortColumn).GetValue(o, null)).ToList() : tblExchangeABBStatusHistory.OrderByDescending(o => o.GetType().GetProperty(sortColumn).GetValue(o, null)).ToList();
        //            tblExchangeABBStatusHistory = tblExchangeABBStatusHistory.Skip(skip).Take(pageSize).ToList();
        //        }

        //        TimeList = new List<TimeListViewModel>();

        //        List<TimeListViewModel> ExchangeAbbstatusHistoryList = _mapper.Map<List<TblExchangeAbbstatusHistory>, List<TimeListViewModel>>(tblExchangeABBStatusHistory);
        //        List<ProductCategoryViewModel> ProductCategoryList = _mapper.Map<List<TblProductCategory>, List<ProductCategoryViewModel>>(tblProductCategory);
        //        string actionURL = string.Empty;
        //        foreach (var item in tblExchangeABBStatusHistory)
        //        {
        //            TblUser = _context.TblUsers.Where(x => x.UserId == item.CreatedBy).ToList();
        //            TblExchangeOrderStatus = _context.TblExchangeOrderStatuses.Where(x => x.Id == item.StatusId).ToList();
        //            foreach(var item1 in TblExchangeOrderStatus)
        //            {


        //                TblTimelineStatusMappings = _context.TblTimelineStatusMappings.Where(x => x.StatusId == item1.Id).ToList();
        //                foreach (var item2 in TblTimelineStatusMappings)
        //                {
        //                    TblTimeLines = _context.TblTimeLines.Where(x => x.TimeLineId == item2.OrderTimeLineId).ToList();

        //                    timeListViewModel = new TimeListViewModel();

        //                    timeListViewModel.RegdNo = item.RegdNo;
        //                    timeListViewModel.FirstName = item.OrderTrans.Exchange.CustomerDetails.FirstName;
        //                    timeListViewModel.OrderStatus = item.OrderTrans.Exchange.OrderStatus;
        //                    timeListViewModel.OrderTimeline = TblTimeLines[0].OrderTimeline;
        //                    timeListViewModel.StatusName = TblExchangeOrderStatus[0].StatusName;
        //                    timeListViewModel.CreatedBy = item.CreatedBy;

        //                    TimeList.Add(timeListViewModel);

        //                }
        //            }


        //        }
        //        for(int i= 0; i<= ExchangeAbbstatusHistoryList.Count; i++)
        //        {
        //            TimeListViewModel = new TimeListViewModel();
        //            TimeListViewModel.RegdNo = ExchangeAbbstatusHistoryList[i].RegdNo;
        //            TimeListViewModel.FirstName = ExchangeAbbstatusHistoryList[i].FirstName;
        //            TimeListViewModel.OrderStatus = ExchangeAbbstatusHistoryList[i].OrderStatus;
        //            TimeListViewModel.OrderTimeline = ExchangeAbbstatusHistoryList[i].OrderTimeline;
        //            TimeListViewModel.StatusId = ExchangeAbbstatusHistoryList[i].StatusId;
        //            ExchangeAbbstatusHistoryList.Add(TimeListViewModel);
        //        }


        //        var data = TimeList;


        //        tblOrderLgc = _context.TblOrderLgcs
        //            .Include(x => x.Logistic).ThenInclude(x => x.OrderTrans)
        //            .ThenInclude(x => x.Exchange).ThenInclude(x => x.ProductType).ThenInclude(x => x.ProductCat)
        //            .Include(x => x.Logistic).ThenInclude(x => x.ServicePartner).Where(x => x.Logistic.ServicePartner.UserId == userId)
        //            .Where(x => x.IsActive == true && (string.IsNullOrEmpty(searchValue) || x.Logistic.RegdNo.ToLower().Contains(searchValue.ToLower())) && x.StatusId == 23)
        //            .ToList();

        //        recordsTotal = tblOrderLgc != null ? tblOrderLgc.Count : 0;
        //        if (tblOrderLgc != null)
        //        {
        //            tblOrderLgc = tblOrderLgc.Skip(skip).Take(pageSize).ToList();
        //        }
        //        lGCOrderList = new List<LGCOrderViewModel>();
        //        if (tblOrderLgc != null)
        //        {
        //            foreach (var item in tblOrderLgc)
        //            {
        //                string actionURL = string.Empty;
        //                var regdno = item.Logistic.RegdNo;
        //                actionURL = " <ul class='actions'>";
        //                actionURL = "<a href ='" + URL + "/LGC/LGCDrop?regdNo=" + item.Logistic.RegdNo + "' ><button onclick='View(" + item.Logistic.RegdNo + ")' class='btn btn-primary btn'>View</button></a>";
        //                actionURL = actionURL + "</ul>";

        //                if (item != null)
        //                {
        //                    lGCOrderViewModel = new LGCOrderViewModel();
        //                    lGCOrderViewModel.Id = item.OrderLgcid;
        //                    lGCOrderViewModel.Action = actionURL;
        //                    lGCOrderViewModel.RegdNo = item.Logistic.RegdNo;
        //                    lGCOrderViewModel.TicketNumber = item.Logistic.TicketNumber;
        //                    lGCOrderViewModel.ProductCategory = item.Logistic.OrderTrans.Exchange.ProductType.ProductCat.Description;
        //                    lGCOrderViewModel.ProductType = item.Logistic.OrderTrans.Exchange.ProductType.Description;
        //                    lGCOrderViewModel.AmountPayableThroughLGC = 0;
        //                    lGCOrderList.Add(lGCOrderViewModel);
        //                }
        //            }
        //        }

        //        var data = lGCOrderList.OrderByDescending(x => x.Id);
        //        var jsonData = new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data };
        //        return Ok(jsonData);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw;
        //    }
        //}


        [HttpPost]
        public async Task<ActionResult> GetAccessList(DateTime? startDate, DateTime? endDate)
        {
            string URL = _config.Value.URLPrefixforProd;
            List<TblAccessList> TblAccesLists = null;
            List<TblCustomerDetail> TblCustomerDetails = null;
            List<TblExchangeOrder> TblExchangeOrders = null;
            List<TblUser> TblUser = null;
            List<TblExchangeOrderStatus> TblExchangeOrderStatus = null;
            List<TblTimelineStatusMapping> TblTimelineStatusMappings = null;
            List<TblTimeLine> TblTimeLines = null;
            List<AccessListViewModel> Accesslist = null;
            AccessListViewModel accessListViewModel = null;
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
                if (startDate == null && endDate == null)
                {

                    TblAccesLists = await _context.TblAccessLists
                .Include(t => t.Company)
                .Include(t => t.CreatedByNavigation)
                .Include(t => t.ModifiedByNavigation)
                .Include(t => t.ParentAccessList).Where(x => x.IsActive == true
                       && (string.IsNullOrEmpty(searchValue) || x.Name.ToLower().Contains(searchValue.ToLower().Trim()))).ToListAsync();
                }
                else
                {
                    startDate = Convert.ToDateTime(startDate).AddMinutes(-1);
                    endDate = Convert.ToDateTime(endDate).AddDays(1).AddSeconds(-1);
                    TblAccesLists = await _context.TblAccessLists
                .Include(t => t.Company)
                .Include(t => t.CreatedByNavigation)
                .Include(t => t.ModifiedByNavigation)
                .Include(t => t.ParentAccessList).Where(x => x.IsActive == true && x.CreatedDate >= startDate && x.CreatedDate <= endDate
                       && (string.IsNullOrEmpty(searchValue) || x.Name.ToLower().Contains(searchValue.ToLower().Trim()))).ToListAsync();
                }





                recordsTotal = TblAccesLists != null ? TblAccesLists.Count : 0;
                if (TblAccesLists != null)
                {
                    TblAccesLists = sortColumnDirection.Equals(SortingOrder.ASCENDING) ? TblAccesLists.OrderBy(o => o.GetType().GetProperty(sortColumn).GetValue(o, null)).ToList() : TblAccesLists.OrderByDescending(o => o.GetType().GetProperty(sortColumn).GetValue(o, null)).ToList();
                    TblAccesLists = TblAccesLists.Skip(skip).Take(pageSize).ToList();
                }

                Accesslist = new List<AccessListViewModel>();

                List<AccessListViewModel> AccessList = _mapper.Map<List<TblAccessList>, List<AccessListViewModel>>(TblAccesLists);

                string actionURL = string.Empty;
                foreach (var item in TblAccesLists)
                {
                    actionURL = " <td class='actions'>";
                    actionURL = actionURL + "<a href=' " + URL + "/AccessList/Edit?id=" + (item.AccessListId) + "' data-bs-toggle='tooltip' data-bs-placement='top' title='Edit' class='btn btn-sm btn-primary'><i class='fa-solid fa-pen'></i></a> " +
                         "<a href=' " + URL + "/AccessList/Details?id=" + (item.AccessListId) + "' data-bs-toggle='tooltip' data-bs-placement='top' title='Details' class='btn btn-sm btn-primary'><i class='fa-solid fa-eye'></i></a> " +
                        "<a href='javascript: void(0)' onclick='deleteConfirm(" + item.AccessListId + ")' class='btn btn-sm btn-danger' data-bs-toggle='tooltip' data-bs-placement='top' title='Delete'><i class='fa-solid fa-trash'></i></a>";
                    actionURL = actionURL + " </div>";
                    accessListViewModel = new AccessListViewModel();
                    accessListViewModel.Action = actionURL;
                    accessListViewModel.AccessListId = item.AccessListId;
                    accessListViewModel.Name = item.Name;
                    accessListViewModel.Description = item.Description;
                    accessListViewModel.ActionName = item.ActionName;
                    accessListViewModel.ActionUrl = item.ActionUrl;

                    if (item.ParentAccessListId != null)
                    {
                        accessListViewModel.ParentAccessListId = item.ParentAccessList.AccessListId;
                    }
                    accessListViewModel.CreatedBy = item.CreatedBy;
                    accessListViewModel.CreatedDate = item.CreatedDate;
                    accessListViewModel.ModifiedBy = item.ModifiedBy;
                    accessListViewModel.ModifiedDate = item.ModifiedDate;
                    accessListViewModel.SetIcon = item.SetIcon;
                    accessListViewModel.CompanyId = item.CompanyId;
                    accessListViewModel.IsMenu = item.IsMenu;

                    Accesslist.Add(accessListViewModel);
                    TblAccessList AccessListDetails = TblAccesLists.FirstOrDefault(x => x.AccessListId == item.AccessListId);

                    if (AccessListDetails.CreatedDate != null && AccessListDetails.CreatedDate != null)
                    {
                        //var Date = (DateTime)item.CreatedDate;
                        //item.Date = Date.ToShortDateString();


                        DateTime dateTime = (DateTime)item.CreatedDate;
                        //item.Date = dateTime.ToString("yyyy-MM-dd");
                        item.CreatedDate = AccessListDetails.CreatedDate;

                    }
                }
                var data = Accesslist;
                var jsonData = new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data };
                return Ok(jsonData);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [HttpPost]
        public async Task<ActionResult> GetImageLabelList(DateTime? startDate, DateTime? endDate)
        {
            string URL = _config.Value.URLPrefixforProd;
            List<TblImageLabelMaster> tblImageLabelMaster = null;
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
                //_context = new Digi2l_DevContext();
                if (startDate == null && endDate == null)
                {
                    tblImageLabelMaster = await _context.TblImageLabelMasters.Include(x => x.ProductCat).Where(x => x.IsActive == true
                        && (string.IsNullOrEmpty(searchValue) || x.ProductName.ToLower().Contains(searchValue.ToLower().Trim()) || x.ProductImageLabel.ToLower().Contains(searchValue.ToLower().Trim()) || x.ProductImageDescription.ToLower().Contains(searchValue.ToLower().Trim()) || x.Pattern.ToString().Contains(searchValue.ToLower().Trim()) || x.ProductCat.Description.ToLower().Contains(searchValue.ToLower().Trim()))).ToListAsync();
                }
                else
                {
                    startDate = Convert.ToDateTime(startDate).AddMinutes(-1);
                    endDate = Convert.ToDateTime(endDate).AddDays(1).AddSeconds(-1);
                    tblImageLabelMaster = await _context.TblImageLabelMasters.Include(x => x.ProductCat).Where(x => x.IsActive == true && x.CreatedDate >= startDate && x.CreatedDate <= endDate
                       && (string.IsNullOrEmpty(searchValue) || x.ProductName.ToLower().Contains(searchValue.ToLower().Trim()) || x.ProductImageLabel.ToLower().Contains(searchValue.ToLower().Trim()) || x.ProductImageDescription.ToLower().Contains(searchValue.ToLower().Trim()) || x.Pattern.ToString().Contains(searchValue.ToLower().Trim()) || x.ProductCat.Description.ToLower().Contains(searchValue.ToLower().Trim()))).ToListAsync();

                }

                recordsTotal = tblImageLabelMaster != null ? tblImageLabelMaster.Count : 0;
                if (tblImageLabelMaster != null)
                {
                    tblImageLabelMaster = sortColumnDirection.Equals(SortingOrder.ASCENDING) ? tblImageLabelMaster.OrderBy(o => o.GetType().GetProperty(sortColumn).GetValue(o, null)).ToList() : tblImageLabelMaster.OrderByDescending(o => o.GetType().GetProperty(sortColumn).GetValue(o, null)).ToList();
                    tblImageLabelMaster = tblImageLabelMaster.Skip(skip).Take(pageSize).ToList();
                }
                else
                    tblImageLabelMaster = new List<TblImageLabelMaster>();

                List<ImageLabelNewViewModel> ImageLabelList = _mapper.Map<List<TblImageLabelMaster>, List<ImageLabelNewViewModel>>(tblImageLabelMaster);
                //List<ProductCategoryViewModel> ProductCategoryList = _mapper.Map<List<TblProductCategory>, List<ProductCategoryViewModel>>(tblProductCategory);
                string actionURL = string.Empty;

                foreach (ImageLabelNewViewModel item in ImageLabelList)
                {
                    actionURL = "<div class='actionbtns'>";
                    actionURL = actionURL + "<a href=' " + URL + "/ImageLabelMaster/Manage?id=" + _protector.Encode(item.ImageLabelid) + "' data-bs-toggle='tooltip' data-bs-placement='top' title='Edit' class='btn btn-sm btn-primary'><i class='fa-solid fa-pen'></i></a> " +
                        "<a href='javascript: void(0)' onclick='deleteConfirmImageLabel(" + item.ImageLabelid + ")' class='btn btn-sm btn-danger' data-bs-toggle='tooltip' data-bs-placement='top' title='Delete'><i class='fa-solid fa-trash'></i></a>";
                    actionURL = actionURL + " </div>";
                    item.Action = actionURL;

                    TblImageLabelMaster ImageLabel = tblImageLabelMaster.FirstOrDefault(x => x.ImageLabelid == item.ImageLabelid);

                    if (ImageLabel.CreatedDate != null && ImageLabel.CreatedDate != null)
                    {
                        //var Date = (DateTime)item.CreatedDate;
                        //item.Date = Date.ToShortDateString();


                        DateTime dateTime = (DateTime)item.CreatedDate;
                        item.Date = dateTime.ToString("yyyy-MM-dd");
                        //item.CreatedDate = ImageLabel.CreatedDate;

                    }

                    if (ImageLabel != null && ImageLabel.ProductCatId > 0)
                    {
                        TblProductCategory tblProductCategory = _context.TblProductCategories.FirstOrDefault(x => x.Id == ImageLabel.ProductCatId);
                        item.ProductCategoryName = tblProductCategory != null ? tblProductCategory.Description : string.Empty;
                    }
                    if (ImageLabel != null && ImageLabel.ProductTypeId > 0)
                    {
                        TblProductType tblProductType = _context.TblProductTypes.FirstOrDefault(x => x.Id == ImageLabel.ProductTypeId);
                        item.ProductTypeName = tblProductType != null ? tblProductType.Description + " " + tblProductType .Size : string.Empty;
                    }
                }

                var data = ImageLabelList;
                var jsonData = new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data };
                return Ok(jsonData);
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        [HttpPost]
        public async Task<ActionResult> GetServicePartnerList(DateTime? startDate, DateTime? endDate)
        {
            string URL = _config.Value.URLPrefixforProd;
            List<TblServicePartner> tblServicePartner = null;
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

                if (startDate == null && endDate == null)
                {
                    tblServicePartner = await _context.TblServicePartners.Include(x => x.User).Where(x => x.IsActive == true
                        && (string.IsNullOrEmpty(searchValue) || x.ServicePartnerName.ToLower().Contains(searchValue.ToLower().Trim())

                        || x.ServicePartnerDescription.ToLower().Contains(searchValue.ToLower().Trim())
                        || x.ServicePartnerRegdNo.ToLower().Contains(searchValue.ToLower().Trim())
                         || x.ServicePartnerMobileNumber.ToLower().Contains(searchValue.ToLower().Trim())
                          || x.ServicePartnerEmailId.ToLower().Contains(searchValue.ToLower().Trim())
                        || x.User.FirstName.ToLower().Contains(searchValue.ToLower().Trim()))).ToListAsync();
                }
                else
                {
                    startDate = Convert.ToDateTime(startDate).AddMinutes(-1);
                    endDate = Convert.ToDateTime(endDate).AddDays(1).AddSeconds(-1);
                    tblServicePartner = await _context.TblServicePartners.Include(x => x.User).Where(x => x.IsActive == true && x.CreatedDate >= startDate && x.CreatedDate <= endDate
                        && (string.IsNullOrEmpty(searchValue) || x.ServicePartnerName.ToLower().Contains(searchValue.ToLower().Trim())
                        || x.ServicePartnerDescription.ToLower().Contains(searchValue.ToLower().Trim())
                        || x.ServicePartnerRegdNo.ToLower().Contains(searchValue.ToLower().Trim())
                         || x.ServicePartnerMobileNumber.ToLower().Contains(searchValue.ToLower().Trim())
                          || x.ServicePartnerEmailId.ToLower().Contains(searchValue.ToLower().Trim())
                        || x.User.FirstName.ToLower().Contains(searchValue.ToLower().Trim()))).ToListAsync();

                }

                recordsTotal = tblServicePartner != null ? tblServicePartner.Count : 0;
                if (tblServicePartner != null)
                {
                    tblServicePartner = sortColumnDirection.Equals(SortingOrder.ASCENDING) ? tblServicePartner.OrderBy(o => o.GetType().GetProperty(sortColumn).GetValue(o, null)).ToList() : tblServicePartner.OrderByDescending(o => o.GetType().GetProperty(sortColumn).GetValue(o, null)).ToList();
                    tblServicePartner = tblServicePartner.Skip(skip).Take(pageSize).ToList();
                }
                else
                    tblServicePartner = new List<TblServicePartner>();

                List<ServicePartnerViewModel> ServicePartnerList = _mapper.Map<List<TblServicePartner>, List<ServicePartnerViewModel>>(tblServicePartner);
                //List<ProductCategoryViewModel> ProductCategoryList = _mapper.Map<List<TblProductCategory>, List<ProductCategoryViewModel>>(tblProductCategory);
                string actionURL = string.Empty;

                foreach (ServicePartnerViewModel item in ServicePartnerList)
                {
                    actionURL = "<div class='actionbtns'>";
                    actionURL = actionURL + "<a href=' " + URL + "/ServicePartner/Manage?id=" + _protector.Encode(item.ServicePartnerId) + "' data-bs-toggle='tooltip' data-bs-placement='top' title='Edit' class='btn btn-sm btn-primary'><i class='fa-solid fa-pen'></i></a> " +
                        "<a href='javascript: void(0)' onclick='deleteConfirmServicePartner(" + item.ServicePartnerId + ")' class='btn btn-sm btn-danger' data-bs-toggle='tooltip' data-bs-placement='top' title='Delete'><i class='fa-solid fa-trash'></i></a>";
                    actionURL = actionURL + " </div>";
                    item.Action = actionURL;

                    TblServicePartner ServicePartner = tblServicePartner.FirstOrDefault(x => x.ServicePartnerId == item.ServicePartnerId);

                    if (ServicePartner.CreatedDate != null && ServicePartner.CreatedDate != null)
                    {
                        //var Date = (DateTime)item.CreatedDate;
                        //item.Date = Date.ToShortDateString();

                        DateTime dateTime = (DateTime)item.CreatedDate;
                        item.Date = dateTime.ToString("yyyy-MM-dd");
                        //item.CreatedDate = ServicePartner.CreatedDate;

                    }

                    if (ServicePartner != null && ServicePartner.UserId > 0)
                    {
                        TblUser tblUser = _context.TblUsers.FirstOrDefault(x => x.UserId == ServicePartner.UserId);
                        item.UserName = tblUser != null ? tblUser.FirstName + " " + tblUser.LastName : string.Empty;
                    }
                }

                var data = ServicePartnerList;
                var jsonData = new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data };
                return Ok(jsonData);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [HttpPost]
        public async Task<ActionResult> GetBusinessUnitList(DateTime? startDate, DateTime? endDate)
        {
            string URL = _config.Value.URLPrefixforProd;
            List<TblBusinessUnit> tblBusinessUnit = null;
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

                if (startDate == null && endDate == null)
                {
                    tblBusinessUnit = await _context.TblBusinessUnits.Where(x => x.IsActive == true
                       && (string.IsNullOrEmpty(searchValue) || x.Name.ToLower().Contains(searchValue.ToLower().Trim()) || x.Description.ToLower().Contains(searchValue.ToLower().Trim()) || x.Email.ToLower().Contains(searchValue.ToLower().Trim()))).ToListAsync();
                }
                else
                {
                    startDate = Convert.ToDateTime(startDate).AddMinutes(-1);
                    endDate = Convert.ToDateTime(endDate).AddDays(1).AddSeconds(-1);
                    tblBusinessUnit = await _context.TblBusinessUnits.Where(x => x.IsActive == true && x.CreatedDate >= startDate && x.CreatedDate <= endDate
                        && (string.IsNullOrEmpty(searchValue) || x.Name.ToLower().Contains(searchValue.ToLower().Trim()) || x.Description.ToLower().Contains(searchValue.ToLower().Trim()) || x.Email.ToLower().Contains(searchValue.ToLower().Trim()))).ToListAsync();
                }

                recordsTotal = tblBusinessUnit != null ? tblBusinessUnit.Count : 0;
                if (tblBusinessUnit != null)
                {
                    tblBusinessUnit = sortColumnDirection.Equals(SortingOrder.ASCENDING) ? tblBusinessUnit.OrderBy(o => o.GetType().GetProperty(sortColumn).GetValue(o, null)).ToList() : tblBusinessUnit.OrderByDescending(o => o.GetType().GetProperty(sortColumn).GetValue(o, null)).ToList();
                    tblBusinessUnit = tblBusinessUnit.Skip(skip).Take(pageSize).ToList();
                }
                else
                    tblBusinessUnit = new List<TblBusinessUnit>();

                List<BusinessUnitViewModel> BusinessUnitList = _mapper.Map<List<TblBusinessUnit>, List<BusinessUnitViewModel>>(tblBusinessUnit);
                //List<ProductCategoryViewModel> ProductCategoryList = _mapper.Map<List<TblProductCategory>, List<ProductCategoryViewModel>>(tblProductCategory);
                string actionURL = string.Empty;

                foreach (BusinessUnitViewModel item in BusinessUnitList)
                {
                    actionURL = "<div class='actionbtns'>";
                    actionURL = actionURL + "<a href=' " + URL + "/BusinessUnit/ManageBusinessUnit?id=" + _protector.Encode(item.BusinessUnitId) + "' data-bs-toggle='tooltip' data-bs-placement='top' title='Edit' class='btn btn-sm btn-primary'><i class='fa-solid fa-pen'></i></a> " +
                        /*"<a href=' " + URL + "/BusinessUnit/Edit?id=" + _protector.Encode(item.BusinessUnitId) + "' data-bs-toggle='tooltip' data-bs-placement='top' title='Edit' class='btn btn-sm btn-primary'><i class='fa-solid fa-pen'></i></a> " +
                        */"<a href='javascript: void(0)' onclick='deleteConfirm(" + item.BusinessUnitId + ")' class='btn btn-sm btn-danger' data-bs-toggle='tooltip' data-bs-placement='top' title='Delete'><i class='fa-solid fa-trash'></i></a>";
                    actionURL = actionURL + " </div>";
                    item.Action = actionURL;

                    TblBusinessUnit BusinessUnit = tblBusinessUnit.FirstOrDefault(x => x.BusinessUnitId == item.BusinessUnitId);

                    if (BusinessUnit.CreatedDate != null && BusinessUnit.CreatedDate != null)
                    {
                        //var Date = (DateTime)item.CreatedDate;
                        //item.Date = Date.ToShortDateString();


                        DateTime dateTime = (DateTime)item.CreatedDate;
                        item.Date = dateTime.ToString("yyyy-MM-dd");
                        //item.CreatedDate = BusinessUnit.CreatedDate;

                    }

                }

                var data = BusinessUnitList;
                var jsonData = new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data };
                return Ok(jsonData);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [HttpPost]
        public async Task<ActionResult> GetModelNumberList(DateTime? startDate, DateTime? endDate, int? productCatId,
            int? productTypeId, string? BuId, string? Brand, string? ModelName, string? description, string? code)
        {
            string URL = _config.Value.URLPrefixforProd;
            if (string.IsNullOrEmpty(BuId) || BuId == "null")
            { BuId = "".Trim(); }
            else
            { BuId = BuId.Trim(); }
            if (string.IsNullOrEmpty(Brand) || Brand == "null")
            { Brand = "".Trim().ToLower(); }
            else
            { Brand = Brand.Trim().ToLower(); }
            if (string.IsNullOrEmpty(ModelName) || ModelName == "null")
            { ModelName = "".Trim().ToLower(); }
            else
            { ModelName = ModelName.Trim().ToLower(); }
            if (string.IsNullOrEmpty(description) || description == "null")
            { description = "".Trim().ToLower(); }
            else
            { description = description.Trim().ToLower(); }
            if (string.IsNullOrEmpty(code) || code == "null")
            { code = "".Trim().ToLower(); }
            else
            { code = code.Trim().ToLower(); }
            List<TblModelNumber> tblModelNumber = null;
            try
            {
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

                if (startDate == null && endDate == null)
                {

                    count = _context.TblModelNumbers
                        .Include(x => x.BusinessUnit)
                        .Include(x => x.Brand)
                        .Include(x => x.ProductType).ThenInclude(x => x.ProductCat)
                        .Where(x =>  (productCatId > 0 ? (x.ProductCategoryId == 0 ? false : (x.ProductCategoryId == productCatId)) : true)
                                     && (productTypeId > 0 ? (x.ProductTypeId == 0 ? false : (x.ProductTypeId == productTypeId)) : true)
                                    && x.BusinessUnit.Name.Contains(BuId)
                                     && x.Brand.Name.Contains(Brand)
                                     && x.ModelName.Contains(ModelName)
                                     && x.Description.Contains(description)
                                      && x.Code.Contains(code)).Count();


                    if (count > 0)
                    {
                        tblModelNumber = await _context.TblModelNumbers

                           .Include(x => x.BusinessUnit)
                        .Include(x => x.Brand)
                        .Include(x => x.ProductType).ThenInclude(x => x.ProductCat)
                          .Where(x => 

                           (productCatId > 0 ? (x.ProductType.ProductCat.Id == 0 ? false : (x.ProductType.ProductCat.Id == productCatId)) : true)
                                      && (productTypeId > 0 ? (x.ProductTypeId == 0 ? false : (x.ProductTypeId == productTypeId)) : true)
                                      && x.BusinessUnit.Name.Contains(BuId)
                                      && x.Brand.Name.Contains(Brand)
                                      && x.ModelName.Contains(ModelName)
                                      && x.Description.Contains(description)
                                      && x.Code.Contains(code)).OrderByDescending(x => x.ModelNumberId).Skip(skip).Take(pageSize).ToListAsync();


                    }

                }
                else
                {
                    startDate = Convert.ToDateTime(startDate).AddMinutes(-1);
                    endDate = Convert.ToDateTime(endDate).AddDays(1).AddSeconds(-1);
                    count = _context.TblModelNumbers.Include(x => x.BusinessUnit)
                        .Include(x => x.Brand)
                        .Include(x => x.ProductType).ThenInclude(x => x.ProductCat).Where(x => x.CreatedDate >= startDate && x.CreatedDate <= endDate

                       && (productCatId > 0 ? (x.ProductType.ProductCat.Id == 0 ? false : (x.ProductType.ProductCat.Id == productCatId)) : true)
                                     && (productTypeId > 0 ? (x.ProductTypeId == 0 ? false : (x.ProductTypeId == productTypeId)) : true)
                                     && x.BusinessUnit.Name.Contains(BuId)
                                     && x.Brand.Name.Contains(Brand)
                                     && x.ModelName.Contains(ModelName)
                                     && x.Description.Contains(description)
                                      && x.Code.Contains(code)).Count();


                    if (count > 0)
                    {
                        tblModelNumber = await _context.TblModelNumbers.Include(x => x.BusinessUnit)
                        .Include(x => x.Brand)
                        .Include(x => x.ProductType).ThenInclude(x => x.ProductCat).Where(x => x.CreatedDate >= startDate && x.CreatedDate <= endDate

                       && (productCatId > 0 ? (x.ProductType.ProductCat.Id == 0 ? false : (x.ProductType.ProductCat.Id == productCatId)) : true)
                                     && (productTypeId > 0 ? (x.ProductTypeId == 0 ? false : (x.ProductTypeId == productTypeId)) : true)
                                     && x.BusinessUnit.Name.Contains(BuId)
                                     && x.Brand.Name.Contains(Brand)
                                     && x.ModelName.Contains(ModelName)
                                     && x.Description.Contains(description)
                                      && x.Code.Contains(code)).OrderByDescending(x => x.ModelNumberId).Skip(skip).Take(pageSize).ToListAsync();

                    }
                }


                recordsTotal = count;

                List<ModelNumberViewModel> ModelNumberList = _mapper.Map<List<TblModelNumber>, List<ModelNumberViewModel>>(tblModelNumber);
                //List<ProductCategoryViewModel> ProductCategoryList = _mapper.Map<List<TblProductCategory>, List<ProductCategoryViewModel>>(tblProductCategory);
                string actionURL = string.Empty;

                foreach (ModelNumberViewModel item in ModelNumberList)
                {
                    if (item.IsActive == true)
                    {
                        actionURL = "<div class='actionbtns'>";
                        actionURL = actionURL + "<a href=' " + URL + "/ModelNumber/Manage?id=" + _protector.Encode(item.ModelNumberId) + "' data-bs-toggle='tooltip' data-bs-placement='top' title='Edit' class='btn btn-sm btn-primary'><i class='fa-solid fa-pen'></i></a> " +
                            "<a href='javascript: void(0)' onclick='deleteConfirmModel(" + item.ModelNumberId + ")' class='btn btn-sm btn-danger' data-bs-toggle='tooltip' data-bs-placement='top' title='Delete'><i class='fa-solid fa-trash'></i></a>";
                        actionURL = actionURL + " </div>";
                        item.Action = actionURL;
                    }
                    else
                    {
                        actionURL = "<div class='actionbtns'>";
                        actionURL = actionURL + "<a href=' " + URL + "/ModelNumber/Manage?id=" + _protector.Encode(item.ModelNumberId) + "' data-bs-toggle='tooltip' data-bs-placement='top' title='Edit' class='btn btn-sm btn-primary'><i class='fa-solid fa-pen'></i></a> " +
                           "<a href='" + "javascript:void(0)' onclick='activeConfirmModel(" + item.ModelNumberId + ")' class='btn btn-sm btn-success' data-bs-toggle='tooltip' data-bs-placement='top' title='Active'><i class='fa-solid fa-circle-check text-white'></i></a>";
                        actionURL = actionURL + " </div>";
                        item.Action = actionURL;

                    }


                    TblModelNumber modelNumber = tblModelNumber.FirstOrDefault(x => x.ModelNumberId == item.ModelNumberId);
                    if (modelNumber.CreatedDate != null && modelNumber.CreatedDate != null)
                    {
                        //var Date = (DateTime)item.CreatedDate;
                        //item.Date = Date.ToShortDateString();


                        DateTime dateTime = (DateTime)item.CreatedDate;
                        item.Date = dateTime.ToString("yyyy-MM-dd");
                        //item.CreatedDate = modelNumber.CreatedDate;

                    }


                    if (modelNumber != null && modelNumber.BrandId > 0)
                    {
                        TblBrand tblBrand = _context.TblBrands.FirstOrDefault(x => x.Id == modelNumber.BrandId);
                        item.BrandName = tblBrand != null ? tblBrand.Name : string.Empty;
                    }

                    if (modelNumber != null && modelNumber.ProductTypeId > 0)
                    {
                        TblProductType tblProductType = _context.TblProductTypes.FirstOrDefault(x => x.Id == modelNumber.ProductTypeId);
                        item.ProductTypeName = tblProductType != null ? tblProductType.Description : string.Empty;
                    }

                    if (modelNumber != null && modelNumber.ProductCategoryId > 0)
                    {
                        TblProductCategory tblProductCategory = _context.TblProductCategories.FirstOrDefault(x => x.Id == modelNumber.ProductCategoryId);
                        item.ProductCategoryName = tblProductCategory != null ? tblProductCategory.Description : string.Empty;
                    }

                    if (modelNumber != null && modelNumber.BusinessUnitId > 0)
                    {
                        TblBusinessUnit tblBusinessUnit = _context.TblBusinessUnits.FirstOrDefault(x => x.BusinessUnitId == modelNumber.BusinessUnitId);
                        item.BusinessUnitName = tblBusinessUnit != null ? tblBusinessUnit.Name : string.Empty;
                    }

                    if (modelNumber != null && modelNumber.BusinessPartnerId > 0)
                    {
                        TblBusinessPartner TblBusinessPartner = _context.TblBusinessPartners.FirstOrDefault(x => x.BusinessPartnerId == modelNumber.BusinessPartnerId);
                        item.BusinessPartnerName = TblBusinessPartner != null ? TblBusinessPartner.Name : string.Empty;
                    }
                }



                var data = ModelNumberList;
                var jsonData = new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data };
                return Ok(jsonData);
            }

            catch (Exception ex)
            {
                throw;
            }
        }

        [HttpPost]
        public async Task<ActionResult> GetPriceMasterList(int? buid, DateTime? startDate, DateTime? endDate)
        {
            List<TblPriceMaster> tblPriceMaster = null;
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
                //_context = new Digi2L_DevContext();

                if (startDate == null && endDate == null)
                {
                    if (buid == null)
                    {
                        tblPriceMaster = await _context.TblPriceMasters.Where(x => x.IsActive == true
                                && (string.IsNullOrEmpty(searchValue)
                                || x.ExchPriceCode.ToLower().Contains(searchValue.ToLower().Trim())
                                || x.ProductCategory.Description.ToLower().
                                Contains(searchValue.ToLower().Trim())
                                || x.ProductType.ToLower().Contains(searchValue.ToLower().Trim())
                                || x.BrandName2.ToLower().Contains(searchValue.ToLower().Trim())
                                || x.BrandName1.ToLower().Contains(searchValue.ToLower().Trim())
                                || x.BrandName3.ToLower().Contains(searchValue.ToLower().Trim())
                                || x.BrandName4.ToLower().Contains(searchValue.ToLower().Trim())
                                || x.ProductTypeCode.ToLower().Contains(searchValue.ToLower().Trim()))).ToListAsync();

                    }
                    else
                    {
                        var login = _context.Logins.Where(x => x.SponsorId == buid);
                        foreach (var item in login)
                        {

                            tblPriceMaster = await _context.TblPriceMasters

                                .Where(x => x.IsActive == true && x.ExchPriceCode == item.PriceCode

                                     && (string.IsNullOrEmpty(searchValue)
                                     || x.ExchPriceCode.ToLower().Contains(searchValue.ToLower().Trim())
                                     || x.ProductCategory.Description.ToLower().Contains(searchValue.ToLower().Trim())
                                     || x.ProductType.ToLower().Contains(searchValue.ToLower().Trim())
                                     || x.BrandName2.ToLower().Contains(searchValue.ToLower().Trim())
                                     || x.BrandName1.ToLower().Contains(searchValue.ToLower().Trim())
                                     || x.BrandName3.ToLower().Contains(searchValue.ToLower().Trim())
                                     || x.BrandName4.ToLower().Contains(searchValue.ToLower().Trim())
                                     || x.ProductTypeCode.ToLower().Contains(searchValue.ToLower().Trim())
                                     || x.BrandName3.ToLower().Contains(searchValue.ToLower().Trim())
                                     || x.BrandName4.ToLower().Contains(searchValue.ToLower().Trim())
                                     || x.ProductTypeCode.ToLower().Contains(searchValue.ToLower().Trim()))).ToListAsync();
                        }
                    }
                }
                else
                {
                    if (buid == null)
                    {
                        startDate = Convert.ToDateTime(startDate).AddMinutes(-1);
                        endDate = Convert.ToDateTime(endDate).AddDays(1).AddSeconds(-1);
                        tblPriceMaster = await _context.TblPriceMasters.Where(x => x.IsActive == true && x.CreatedDate >= startDate && x.CreatedDate <= endDate
                                && (string.IsNullOrEmpty(searchValue) || x.ProductCat.ToLower().Contains(searchValue.ToLower().Trim()) || x.ProductType.ToLower().Contains(searchValue.ToLower().Trim()) || x.BrandName2.ToLower().Contains(searchValue.ToLower().Trim()) || x.BrandName1.ToLower().Contains(searchValue.ToLower().Trim()) || x.BrandName3.ToLower().Contains(searchValue.ToLower().Trim()) || x.BrandName4.ToLower().Contains(searchValue.ToLower().Trim()))).ToListAsync();

                    }
                    else
                    {
                        startDate = Convert.ToDateTime(startDate).AddMinutes(-1);
                        endDate = Convert.ToDateTime(endDate).AddDays(1).AddSeconds(-1);
                        var login = _context.Logins.Where(x => x.SponsorId == buid);
                        foreach (var item in login)
                        {

                            tblPriceMaster = await _context.TblPriceMasters

                                .Where(x => x.IsActive == true && x.ExchPriceCode == item.PriceCode && x.CreatedDate >= startDate && x.CreatedDate <= endDate

                                     && (string.IsNullOrEmpty(searchValue) || x.ProductCat.ToLower().Contains(searchValue.ToLower().Trim()) || x.ProductType.ToLower().Contains(searchValue.ToLower().Trim()) || x.BrandName2.ToLower().Contains(searchValue.ToLower().Trim()) || x.BrandName1.ToLower().Contains(searchValue.ToLower().Trim()) || x.BrandName3.ToLower().Contains(searchValue.ToLower().Trim()) || x.BrandName4.ToLower().Contains(searchValue.ToLower().Trim()))).ToListAsync();
                        }


                    }
                }

                recordsTotal = tblPriceMaster != null ? tblPriceMaster.Count : 0;
                if (tblPriceMaster != null)
                {
                    tblPriceMaster = sortColumnDirection.Equals(SortingOrder.ASCENDING) ? tblPriceMaster.OrderBy(o => o.GetType().GetProperty(sortColumn).GetValue(o, null)).ToList() : tblPriceMaster.OrderByDescending(o => o.GetType().GetProperty(sortColumn).GetValue(o, null)).ToList();
                    tblPriceMaster = tblPriceMaster.Skip(skip).Take(pageSize).ToList();
                }
                else
                    tblPriceMaster = new List<TblPriceMaster>();

                List<PriceMasterViewModel> PriceMasterList = _mapper.Map<List<TblPriceMaster>, List<PriceMasterViewModel>>(tblPriceMaster);
                //List<ProductCategoryViewModel> ProductCategoryList = _mapper.Map<List<TblProductCategory>, List<ProductCategoryViewModel>>(tblProductCategory);
                string actionURL = string.Empty;

                foreach (PriceMasterViewModel item in PriceMasterList)
                {
                    actionURL = "<div class='actionbtns'>";
                    actionURL = actionURL + "<a href=' " + URL + "/PriceMaster/Manage?id=" + _protector.Encode(item.Id) + "' data-bs-toggle='tooltip' data-bs-placement='top' title='Edit' class='btn btn-sm btn-primary'><i class='fa-solid fa-pen'></i></a> " +
                        "<a href='javascript: void(0)' onclick='deleteConfirmPriceMaster(" + item.Id + ")' class='btn btn-sm btn-danger' data-bs-toggle='tooltip' data-bs-placement='top' title='Delete'><i class='fa-solid fa-trash'></i></a>";
                    actionURL = actionURL + " </div>";
                    item.Action = actionURL;
                    TblPriceMaster PriceMaster = tblPriceMaster.FirstOrDefault(x => x.Id == item.Id);
                    if (PriceMaster.CreatedDate != null && PriceMaster.CreatedDate != null)
                    {
                        //var Date = (DateTime)item.CreatedDate;
                        //item.Date = Date.ToShortDateString();


                        DateTime dateTime = (DateTime)item.CreatedDate;
                        item.Date = dateTime.ToString("yyyy-MM-dd");
                        //item.CreatedDate = PriceMaster.CreatedDate;

                    }
                    //if (PriceMaster != null && PriceMaster.ProductTypeId > 0)
                    //{
                    //    TblProductType tblProductType = _context.TblProductTypes.FirstOrDefault(x => x.Id == PriceMaster.ProductTypeId);
                    //    item.ProductTypeName = tblProductType != null ? tblProductType.Name : string.Empty;
                    //}

                    if (PriceMaster != null && PriceMaster.ProductCategoryId > 0)
                    {
                        TblProductCategory tblProductCategory = _context.TblProductCategories.FirstOrDefault(x => x.Id == PriceMaster.ProductCategoryId);
                        item.ProductCategoryDiscription = tblProductCategory != null ? tblProductCategory.Description : string.Empty;
                    }

                    if (PriceMaster != null && PriceMaster.ProductTypeCode != null)
                    {
                        TblProductType tblProductType = _context.TblProductTypes.FirstOrDefault(x => x.Id == PriceMaster.ProductTypeId);
                        item.ProductTypeCode = tblProductType != null ? tblProductType.Code : string.Empty;
                    }

                }

                var data = PriceMasterList;
                var jsonData = new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data };
                return Ok(jsonData);
            }
            catch (Exception ex)
            {
                throw;
            }


        }

        [HttpPost]
        public async Task<ActionResult> GetABBPlanMasterList(DateTime? startDate, DateTime? endDate)
        {
            List<TblAbbplanMaster> tblAbbplanMaster = null;

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
                int count = 0;
                //_context = new Digi2L_DevContext();



                if (startDate == null && endDate == null)
                {
                    tblAbbplanMaster = await _context.TblAbbplanMasters
                        .Include(x => x.ProductType)
                        .Include(x => x.ProductCat)
                        .Where(x => x.IsActive == true
                            && (string.IsNullOrEmpty(searchValue)
                                || x.AbbplanName.ToLower().Contains(searchValue.ToLower().Trim())
                                || x.Sponsor.ToLower().Contains(searchValue.ToLower().Trim())
                                || x.ToMonth.ToString().Contains(searchValue.ToLower().Trim())
                                || x.FromMonth.ToString().Contains(searchValue.ToLower().Trim())
                                || x.BusinessUnit.Name.ToLower().Contains(searchValue.ToLower().Trim())
                                || x.ProductCat.Description.ToLower().Contains(searchValue.ToLower().Trim())
                                || x.ProductType.Description.ToLower().Contains(searchValue.ToLower().Trim())))
                        .OrderByDescending(x => x.CreatedDate)
                      
                        .GroupBy(x => x.BusinessUnitId)
                        .Select(group => group.FirstOrDefault())
                        .Skip(skip)
                        .Take(pageSize)
                        .ToListAsync();
                }
                else
                {
                    startDate = Convert.ToDateTime(startDate).AddMinutes(-1);
                    endDate = Convert.ToDateTime(endDate).AddDays(1).AddSeconds(-1);
                    tblAbbplanMaster = await _context.TblAbbplanMasters
                        .Include(x => x.BusinessUnit)
                        .Include(x => x.ProductType)
                        .Include(x => x.ProductCat)
                        .Where(x => x.IsActive == true
                            && x.CreatedDate >= startDate
                            && x.CreatedDate <= endDate
                            && (string.IsNullOrEmpty(searchValue)
                                || x.AbbplanName.ToLower().Contains(searchValue.ToLower().Trim())
                                || x.Sponsor.ToLower().Contains(searchValue.ToLower().Trim())
                                || x.BusinessUnit.Name.ToLower().Contains(searchValue.ToLower().Trim())
                                || x.ProductCat.Description.ToLower().Contains(searchValue.ToLower().Trim())
                                || x.ProductType.Description.ToLower().Contains(searchValue.ToLower().Trim())))
                        .OrderByDescending(x => x.CreatedDate)
                       
                        .GroupBy(x => x.BusinessUnitId)
                        .Select(group => group.FirstOrDefault())
                        .Skip(skip)
                        .Take(pageSize)
                        .ToListAsync();


                }


                if (tblAbbplanMaster != null)
                {
                    count = await _context.TblAbbplanMasters
                        .Where(x => x.IsActive == true
                            && (string.IsNullOrEmpty(searchValue)
                                || x.AbbplanName.ToLower().Contains(searchValue.ToLower().Trim())
                                || x.Sponsor.ToLower().Contains(searchValue.ToLower().Trim())
                                || x.ToMonth.ToString().Contains(searchValue.ToLower().Trim())
                                || x.FromMonth.ToString().Contains(searchValue.ToLower().Trim())
                                || x.BusinessUnit.Name.ToLower().Contains(searchValue.ToLower().Trim())
                                || x.ProductCat.Description.ToLower().Contains(searchValue.ToLower().Trim())
                                || x.ProductType.Description.ToLower().Contains(searchValue.ToLower().Trim())))
                        .CountAsync();
                }

                recordsTotal = count;

                List<ABBPlanMasterViewModel> ABBPlanMasterList = _mapper.Map<List<TblAbbplanMaster>, List<ABBPlanMasterViewModel>>(tblAbbplanMaster);
                //List<ProductCategoryViewModel> ProductCategoryList = _mapper.Map<List<TblProductCategory>, List<ProductCategoryViewModel>>(tblProductCategory);
                string actionURL = string.Empty;

                foreach (ABBPlanMasterViewModel item in ABBPlanMasterList)
                {
                    actionURL = "<div class='actionbtns'>";
                    actionURL = actionURL + "<a href=' " + URL + "/ABBPlanMaster/Manage?id=" + _protector.Encode(item.PlanMasterId) + "' data-bs-toggle='tooltip' data-bs-placement='top' title='Edit' class='btn btn-sm btn-primary'><i class='fa-solid fa-pen'></i></a> " +
                        "<a href='javascript: void(0)' onclick='deleteConfirmPlanMaster(" + item.PlanMasterId + ")' class='btn btn-sm btn-danger' data-bs-toggle='tooltip' data-bs-placement='top' title='Delete'><i class='fa-solid fa-trash'></i></a>";
                    actionURL = actionURL + " </div>";
                    item.Action = actionURL;



                    TblAbbplanMaster AbbplanMaster = tblAbbplanMaster.FirstOrDefault(x => x.PlanMasterId == item.PlanMasterId);
                    if (AbbplanMaster.CreatedDate != null && AbbplanMaster.CreatedDate != null)
                    {
                        //var Date = (DateTime)item.CreatedDate;
                        //item.Date = Date.ToShortDateString();

                        DateTime dateTime = (DateTime)item.CreatedDate;
                        item.Date = dateTime.ToString("yyyy-MM-dd");
                        //item.CreatedDate = AbbplanMaster.CreatedDate;

                    }



                    if (AbbplanMaster != null && AbbplanMaster.BusinessUnitId > 0)
                    {
                        TblBusinessUnit tblBusinessUnit = _context.TblBusinessUnits.FirstOrDefault(x => x.BusinessUnitId == AbbplanMaster.BusinessUnitId);
                        item.BusinessUnitName = tblBusinessUnit != null ? tblBusinessUnit.Name : string.Empty;
                    }
                    if (AbbplanMaster != null && AbbplanMaster.ProductTypeId > 0)
                    {
                        TblProductType tblProductType = _context.TblProductTypes.FirstOrDefault(x => x.Id == AbbplanMaster.ProductTypeId);
                        item.ProductTypeName = tblProductType != null ? tblProductType.Description + " " + tblProductType.Size : string.Empty;
                    }


                    if (AbbplanMaster != null && AbbplanMaster.ProductCatId > 0)
                    {
                        TblProductCategory tblProductCategory = _context.TblProductCategories.FirstOrDefault(x => x.Id == AbbplanMaster.ProductCatId);
                        item.ProductCategoryName = tblProductCategory != null ? tblProductCategory.Description : string.Empty;
                    }
                }



                var data = ABBPlanMasterList;
                var jsonData = new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data };
                return Ok(jsonData);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [HttpPost]
        public async Task<ActionResult> GetABBPriceMasterList(DateTime? startDate, DateTime? endDate)
        {
            List<TblAbbpriceMaster> tblAbbpriceMaster = null;


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
                //_context = new Digi2L_DevContext();

                if (startDate == null && endDate == null)
                {
                    tblAbbpriceMaster = await _context.TblAbbpriceMasters.Include(x => x.BusinessUnit).Include(x => x.ProductCat).Include(x => x.ProductType).Where(x => x.IsActive == true
                          && (string.IsNullOrEmpty(searchValue) || x.ProductCategory.ToLower().Contains(searchValue.ToLower().Trim()) || x.Sponsor.ToLower().Contains(searchValue.ToLower().Trim()) || x.ProductSabcategory.ToLower().Contains(searchValue.ToLower().Trim()) || x.BusinessUnit.Name.ToLower().Contains(searchValue.ToLower().Trim()) || x.ProductCat.Description.ToLower().Contains(searchValue.ToLower().Trim()) || x.ProductType.Description.ToLower().Contains(searchValue.ToLower().Trim()) || x.FeeType.ToLower().Contains(searchValue.ToLower().Trim()) || x.FeeTypeId.ToString().Contains(searchValue.ToLower().Trim()) || x.PriceStartRange.ToString().Contains(searchValue.ToLower().Trim()) || x.PriceEndRange.ToString().Contains(searchValue.ToLower().Trim()))).ToListAsync();
                }
                else
                {
                    startDate = Convert.ToDateTime(startDate).AddMinutes(-1);
                    endDate = Convert.ToDateTime(endDate).AddDays(1).AddSeconds(-1);
                    tblAbbpriceMaster = await _context.TblAbbpriceMasters.Include(x => x.BusinessUnit).Include(x => x.ProductCat).Include(x => x.ProductType).Where(x => x.IsActive == true && x.CreatedDate >= startDate && x.CreatedDate <= endDate
                          && (string.IsNullOrEmpty(searchValue) || x.ProductCategory.ToLower().Contains(searchValue.ToLower().Trim()) || x.Sponsor.ToLower().Contains(searchValue.ToLower().Trim()) || x.ProductSabcategory.ToLower().Contains(searchValue.ToLower().Trim()) || x.BusinessUnit.Name.ToLower().Contains(searchValue.ToLower().Trim()) || x.ProductCat.Description.ToLower().Contains(searchValue.ToLower().Trim()) || x.ProductType.Description.ToLower().Contains(searchValue.ToLower().Trim()) || x.FeeType.ToLower().Contains(searchValue.ToLower().Trim()) || x.FeeTypeId.ToString().Contains(searchValue.ToLower().Trim()) || x.PriceStartRange.ToString().Contains(searchValue.ToLower().Trim()) || x.PriceEndRange.ToString().Contains(searchValue.ToLower().Trim()))).ToListAsync();
                }



                recordsTotal = tblAbbpriceMaster != null ? tblAbbpriceMaster.Count : 0;
                if (tblAbbpriceMaster != null)
                {
                    tblAbbpriceMaster = sortColumnDirection.Equals(SortingOrder.ASCENDING) ? tblAbbpriceMaster.OrderBy(o => o.GetType().GetProperty(sortColumn).GetValue(o, null)).ToList() : tblAbbpriceMaster.OrderByDescending(o => o.GetType().GetProperty(sortColumn).GetValue(o, null)).ToList();
                    tblAbbpriceMaster = tblAbbpriceMaster.Skip(skip).Take(pageSize).ToList();
                }
                else
                    tblAbbpriceMaster = new List<TblAbbpriceMaster>();

                List<ABBPriceMasterViewModel> ABBPriceMasterList = _mapper.Map<List<TblAbbpriceMaster>, List<ABBPriceMasterViewModel>>(tblAbbpriceMaster);
                //List<ProductCategoryViewModel> ProductCategoryList = _mapper.Map<List<TblProductCategory>, List<ProductCategoryViewModel>>(tblProductCategory);
                string actionURL = string.Empty;

                foreach (ABBPriceMasterViewModel item in ABBPriceMasterList)
                {
                    actionURL = "<div class='actionbtns'>";
                    actionURL = actionURL + "<a href=' " + URL + "/ABBPriceMaster/Manage?id=" + _protector.Encode(item.PriceMasterId) + "' data-bs-toggle='tooltip' data-bs-placement='top' title='Edit' class='btn btn-sm btn-primary'><i class='fa-solid fa-pen'></i></a> " +
                        "<a href='javascript: void(0)' onclick='deleteConfirmABBPriceMaster(" + item.PriceMasterId + ")' class='btn btn-sm btn-danger' data-bs-toggle='tooltip' data-bs-placement='top' title='Delete'><i class='fa-solid fa-trash'></i></a>";
                    actionURL = actionURL + " </div>";
                    item.Action = actionURL;

                    TblAbbpriceMaster AbbpriceMaster = tblAbbpriceMaster.FirstOrDefault(x => x.PriceMasterId == item.PriceMasterId);
                    if (AbbpriceMaster.CreatedDate != null && AbbpriceMaster.CreatedDate != null)
                    {
                        //var Date = (DateTime)item.CreatedDate;
                        //item.Date = Date.ToShortDateString();


                        DateTime dateTime = (DateTime)item.CreatedDate;
                        item.Date = dateTime.ToString("yyyy-MM-dd");
                        //item.CreatedDate = AbbpriceMaster.CreatedDate;

                    }
                    if (AbbpriceMaster != null && AbbpriceMaster.BusinessUnitId > 0)
                    {
                        TblBusinessUnit tblBusinessUnit = _context.TblBusinessUnits.FirstOrDefault(x => x.BusinessUnitId == AbbpriceMaster.BusinessUnitId);
                        item.BusinessUnitName = tblBusinessUnit != null ? tblBusinessUnit.Name : string.Empty;
                    }


                }

                var data = ABBPriceMasterList;
                var jsonData = new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data };
                return Ok(jsonData);
            }
            catch (Exception ex)
            {
                throw;
            }


        }

        public IActionResult OrderDataTableForDealer(int? BusinessPartnerId, DateTime? startdate, DateTime? enddate, string? AssociateCode, string? CompanyName, string? userCompany, string? userRole, int? BusinessUnitId, string? state, string? city)
        {
            var draw = Request.Form["draw"].FirstOrDefault();
            var start = Request.Form["start"].FirstOrDefault();
            var length = Request.Form["length"].FirstOrDefault();
            var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
            var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();
            //var searchValue = Request.Form["search[value]"].FirstOrDefault();
            //searchValue = searchValue.Trim();
            int pageSize = length != null ? Convert.ToInt32(length) : 0;
            int skip = start != null ? Convert.ToInt32(start) : 0;
            int recordsTotal = 0;
            List<ExchangeOrderDataContract> exchangeObj = new List<ExchangeOrderDataContract>();
            List<DealerDashboardViewModel> dealerdatalist = new List<DealerDashboardViewModel>();
            List<TblExchangeOrder> ExchangeList = null;
            TblVoucherVerfication voucherVerification = null;
            int bpid = 0;
            string URL = _config.Value.URLPrefixforProd;
            try
            {
                ExchangeOrderDataContract exchageOrderObj = new ExchangeOrderDataContract();
                //if (startdate == "notprovided")
                //{
                //    startdate = null;
                //}
                //else
                //{
                //    exchageOrderObj.startdate = startdate;
                //}
                //if (enddate == "notprovided")
                //{
                //    enddate = null;
                //}
                //else
                //{
                //    exchageOrderObj.enddate = enddate;
                //}

                //exchageOrderObj.enddate = enddate;
                exchageOrderObj.BusinessPartnerId = BusinessPartnerId;
                exchageOrderObj.AssociateCode = AssociateCode;
                exchageOrderObj.userCompany = userCompany;
                exchageOrderObj.userRole = userRole;
                exchageOrderObj.BusinessUnitId = BusinessUnitId;
                exchageOrderObj.CompanyName = CompanyName;

                if (startdate != null && enddate != null)
                {
                    startdate = Convert.ToDateTime(startdate).AddMinutes(-1);
                    enddate = Convert.ToDateTime(enddate).AddDays(1).AddSeconds(-1);
                }


                // dealerdatalist = _dealerManager.GetDashboardList(exchageOrderObj, skip, pageSize);

                // recordsTotal = dealerdatalist.Count;
                string RoleDescription = EnumHelper.DescriptionAttr(RoleEnum.SuperAdmin);
                string comapnyDescription = EnumHelper.DescriptionAttr(CompanyNameenum.UTC);

                if (exchageOrderObj.BusinessUnitId >= 0)
                {
                    ExchangeList = _exchangeOrderRepository.GetOrderDetailsForDealerDashBoard(startdate, enddate, exchageOrderObj.BusinessPartnerId, exchageOrderObj.CompanyName, exchageOrderObj.AssociateCode, exchageOrderObj.BusinessUnitId, state, city, userRole);
                }

                //if (exchageOrderObj.BusinessPartnerId > 0 && exchageOrderObj.startdate != null && exchageOrderObj.enddate != null && exchageOrderObj.CompanyName != null)
                //{

                //    bpid = Convert.ToInt32(exchageOrderObj.BusinessPartnerId);
                //    ExchangeList = _exchangeOrderRepository.GetOrderDetailsForDashBoard(exchageOrderObj.StartRangedate, exchageOrderObj.EndRangeDate, bpid, exchageOrderObj.CompanyName);


                //}

                //else if (exchageOrderObj.enddate == null && exchageOrderObj.startdate == null && exchageOrderObj.AssociateCode != null && exchageOrderObj.userCompany != comapnyDescription && exchageOrderObj.userRole != RoleDescription)
                //{
                //    ExchangeList = _exchangeOrderRepository.GetOrderDataforsingeldealer(exchageOrderObj.AssociateCode, exchageOrderObj.CompanyName);


                //}

                //else if (exchageOrderObj.BusinessPartnerId > 0 && exchageOrderObj.userCompany == comapnyDescription && exchageOrderObj.startdate != null && exchageOrderObj.enddate != null)
                //{
                //    ExchangeList = _context.TblExchangeOrders.Include(x => x.ProductType)
                //        .ThenInclude(x => x.ProductCat)
                //        .Include(x => x.BusinessPartner)
                //        .Include(x => x.CustomerDetails)
                //        .Include(x => x.TblVoucherVerfications).Where(x => x.BusinessPartnerId == exchageOrderObj.BusinessPartnerId && x.CreatedDate >= exchageOrderObj.StartRangedate && x.CreatedDate <= exchageOrderObj.EndRangeDate && x.IsActive == true).OrderByDescending(x => x.Id).ToList();

                //}

                //else if (exchageOrderObj.BusinessPartnerId == 0 && exchageOrderObj.userCompany == comapnyDescription && exchageOrderObj.CompanyName != null)
                //{
                //    ExchangeList = _context.TblExchangeOrders.Include(x => x.ProductType)
                //       .ThenInclude(x => x.ProductCat)
                //       .Include(x => x.BusinessPartner)
                //       .Include(x => x.CustomerDetails)
                //       .Include(x => x.TblVoucherVerfications).Where(x => x.CompanyName != null && x.CompanyName == exchageOrderObj.CompanyName && x.IsActive == true).OrderByDescending(x => x.Id).ToList();
                //}

                //else if (exchageOrderObj.BusinessPartnerId == 0 && exchageOrderObj.userCompany == comapnyDescription && exchageOrderObj.CompanyName == null)
                //{
                //    ExchangeList = _context.TblExchangeOrders.Include(x => x.ProductType)
                //       .ThenInclude(x => x.ProductCat)
                //       .Include(x => x.BusinessPartner)
                //       .Include(x => x.CustomerDetails)
                //       .Include(x => x.TblVoucherVerfications).Where(x => x.IsActive == true).OrderByDescending(x => x.Id).ToList();

                //}

                //else if (exchageOrderObj.BusinessPartnerId > 0 && exchageOrderObj.userRole == RoleDescription && exchageOrderObj.startdate != null && exchageOrderObj.enddate != null)
                //{
                //    ExchangeList = _context.TblExchangeOrders.Include(x => x.ProductType)
                //       .ThenInclude(x => x.ProductCat)
                //       .Include(x => x.BusinessPartner)
                //       .Include(x => x.CustomerDetails)
                //       .Include(x => x.TblVoucherVerfications).Where(x => x.BusinessPartnerId == exchageOrderObj.BusinessPartnerId && x.CreatedDate >= exchageOrderObj.StartRangedate && x.CreatedDate <= exchageOrderObj.EndRangeDate && x.IsActive == true).OrderByDescending(x => x.Id).ToList();

                //}

                //else if (exchageOrderObj.BusinessPartnerId == 0 && exchageOrderObj.userRole == RoleDescription && exchageOrderObj.BusinessUnitId > 0)
                //{
                //    ExchangeList = _context.TblExchangeOrders.Include(x => x.ProductType)
                //      .ThenInclude(x => x.ProductCat)
                //      .Include(x => x.BusinessPartner)
                //      .Include(x => x.CustomerDetails)
                //      .Include(x => x.TblVoucherVerfications).Where(x => x.IsActive == true && x.CompanyName != null && x.BusinessPartner.BusinessUnitId == exchageOrderObj.BusinessUnitId).OrderByDescending(x => x.Id).ToList();

                //}

                //else if (exchageOrderObj.BusinessPartnerId == 0 && exchageOrderObj.userRole == RoleDescription && exchageOrderObj.CompanyName == null)
                //{
                //    ExchangeList = _context.TblExchangeOrders.Include(x => x.ProductType)
                //      .ThenInclude(x => x.ProductCat)
                //      .Include(x => x.BusinessPartner)
                //      .Include(x => x.CustomerDetails)
                //      .Include(x => x.TblVoucherVerfications).Where(x => x.IsActive == true).OrderByDescending(x => x.Id).ToList();

                //}
                //else if (exchageOrderObj.BusinessUnitId > 0 && (exchageOrderObj.userRole == RoleDescription || exchageOrderObj.userCompany == comapnyDescription))
                //{
                //    ExchangeList = _context.TblExchangeOrders.Include(x => x.ProductType)
                //      .ThenInclude(x => x.ProductCat)
                //      .Include(x => x.BusinessPartner)
                //      .Include(x => x.CustomerDetails)
                //      .Include(x => x.TblVoucherVerfications).Where(x => x.IsActive == true && x.BusinessPartner.BusinessUnitId == exchageOrderObj.BusinessUnitId).OrderByDescending(x => x.Id).ToList();

                //}
                if (ExchangeList != null)
                {
                    recordsTotal = ExchangeList.Count;
                    if (ExchangeList.Count > 0)
                    {
                        ExchangeList = ExchangeList.Skip(skip).Take(pageSize).ToList();
                        DealerDashboardViewModel dealerdata;
                        foreach (var item in ExchangeList)
                        {

                            string actionURL = string.Empty;
                            var regdno = item.RegdNo;

                            actionURL = "<a href ='" + URL + "/Exchange/Manage?Exchangeorderid=" + _protector.Encode(item.Id) + "' ><button onclick='RecordView(" + item.Id + ")' class='btn btn-sm btn-primary' data-bs-toggle='tooltip' data-bs-placement='top' title='View'><i class='fa-solid fa-eye'></i></button></a>";

                            dealerdata = new DealerDashboardViewModel();
                            dealerdata.Id = item.Id;
                            dealerdata.actionUrl = actionURL;
                            dealerdata.CompanyName = item.CompanyName;
                            dealerdata.RegdNo = item.RegdNo;
                            dealerdata.VoucherCode = item.VoucherCode;
                            dealerdata.ExchangePrice = item.ExchangePrice.ToString();
                            dealerdata.OrderReferenceId = item.SponsorServiceRefId;
                            dealerdata.CustomerName = item.CustomerDetails.FirstName + " " + item.CustomerDetails.LastName;
                            if (item.ProductType != null)
                            {
                                if (item.ProductType.ProductCat != null)
                                {
                                    dealerdata.OldProductCategory = item.ProductType.ProductCat.Description;
                                }

                                dealerdata.OldProductType = item.ProductType.Description;
                            }

                            dealerdata.Sweetner = item.Sweetener.ToString();
                            DateTime OrderDate = Convert.ToDateTime(item.CreatedDate);
                            dealerdata.OrderDate = OrderDate.ToString("dd/MMM/yyyy");
                            if (item.IsDefferedSettlement == true)
                            {
                                dealerdata.TypeOfSettelment = EnumHelper.DescriptionAttr(SettlementEnumForDashboard.deffred);
                            }
                            else
                            {
                                dealerdata.TypeOfSettelment = EnumHelper.DescriptionAttr(SettlementEnumForDashboard.instant);
                            }
                            voucherVerification = _voucherRepository.GetVoucherDataByExchangeId(item.Id);
                            if (voucherVerification != null)
                            {
                                DateTime voucherdate = Convert.ToDateTime(voucherVerification.CreatedDate);
                                dealerdata.VoucherRedeemDate = voucherdate.ToString("dd/MMM/yyyy");

                                string status = EnumHelper.DescriptionAttr(VoucherStatusEnum.Redeemed);
                                if (voucherVerification.VoucherStatusId == Convert.ToInt32(VoucherStatusEnum.Redeemed))
                                {
                                    dealerdata.VoucherStatus = status;
                                    dealerdata.Paymentstatus = EnumHelper.DescriptionAttr(SettlementEnumForDashboard.Paid);
                                }
                                if (voucherVerification.VoucherStatusId == Convert.ToInt32(VoucherStatusEnum.Captured))
                                {
                                    string capturedStatus = EnumHelper.DescriptionAttr(VoucherStatusEnum.Captured);
                                    dealerdata.VoucherStatus = capturedStatus;
                                    dealerdata.Paymentstatus = EnumHelper.DescriptionAttr(SettlementEnumForDashboard.NotPaid);
                                }
                            }
                            else
                            {

                            }
                            dealerdatalist.Add(dealerdata);
                        }
                    }

                }
                if (dealerdatalist.Count > 0)
                {
                    dealerdatalist.OrderByDescending(x => x.Id);
                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("DealerDashboardModel", "OnGetStoreList", ex);
            }

            var data = dealerdatalist;
            var jsonData = new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data };
            return new JsonResult(jsonData);
        }

        public IActionResult SeacrhByRegdNoOrPhoneNumber(string? RegdNo, string? PhoneNumber, string AssociateCode, string userRole, string? UserCompany, string? selectedcompany)
        {
            var draw = Request.Form["draw"].FirstOrDefault();
            var start = Request.Form["start"].FirstOrDefault();
            var length = Request.Form["length"].FirstOrDefault();
            var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
            var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();
            //var searchValue = Request.Form["search[value]"].FirstOrDefault();
            //searchValue = searchValue.Trim();
            int pageSize = length != null ? Convert.ToInt32(length) : 0;
            int skip = start != null ? Convert.ToInt32(start) : 0;
            int recordsTotal = 0;
            List<ExchangeOrderDataContract> exchangeObj = new List<ExchangeOrderDataContract>();
            List<DealerDashboardViewModel> dealerdatalist = new List<DealerDashboardViewModel>();
            List<TblExchangeOrder> ExchangeList = null;
            TblVoucherVerfication voucherVerification = null;
            int bpid = 0;
            string URL = _config.Value.URLPrefixforProd;
            try
            {
                ExchangeOrderDataContract exchageOrderObj = new ExchangeOrderDataContract();
                exchageOrderObj.AssociateCode = AssociateCode;
                exchageOrderObj.userRole = userRole;
                // dealerdatalist = _dealerManager.GetDashboardList(exchageOrderObj, skip, pageSize);

                // recordsTotal = dealerdatalist.Count;
                string RoleDescription = EnumHelper.DescriptionAttr(RoleEnum.SuperAdmin);
                string comapnyDescription = EnumHelper.DescriptionAttr(CompanyNameenum.UTC);

                if (exchageOrderObj.userRole == RoleDescription && selectedcompany != null && selectedcompany != comapnyDescription)
                {
                    ExchangeList = _context.TblExchangeOrders.Include(x => x.ProductType)
                      .ThenInclude(x => x.ProductCat)
                      .Include(x => x.BusinessPartner)
                      .Include(x => x.CustomerDetails)
                      .Include(x => x.TblVoucherVerfications).Where(x => x.IsActive == true && x.CustomerDetails != null && x.CompanyName != null && x.CompanyName == selectedcompany && x.RegdNo != null && x.CustomerDetails.PhoneNumber != null && (x.RegdNo.ToLower() == RegdNo.ToLower() || x.CustomerDetails.PhoneNumber == PhoneNumber)).OrderByDescending(x => x.Id).ToList();

                }

                else if (exchageOrderObj.userRole != RoleDescription && exchageOrderObj.AssociateCode != null)
                {
                    ExchangeList = _context.TblExchangeOrders.Include(x => x.ProductType)
                     .ThenInclude(x => x.ProductCat)
                     .Include(x => x.BusinessPartner)
                     .Include(x => x.CustomerDetails)
                     .Include(x => x.TblVoucherVerfications).Where(x => x.IsActive == true && x.BusinessPartner.AssociateCode != null && x.CustomerDetails != null && x.BusinessPartner.AssociateCode == exchageOrderObj.AssociateCode && x.RegdNo != null && x.CustomerDetails.PhoneNumber != null && (x.RegdNo.ToLower() == RegdNo.ToLower() || x.CustomerDetails.PhoneNumber == PhoneNumber)).OrderByDescending(x => x.Id).ToList();

                }

                else if (exchageOrderObj.userRole == RoleDescription && selectedcompany != null && selectedcompany == comapnyDescription)
                {
                    ExchangeList = _context.TblExchangeOrders.Include(x => x.ProductType)
                      .ThenInclude(x => x.ProductCat)
                      .Include(x => x.BusinessPartner)
                      .Include(x => x.CustomerDetails)
                      .Include(x => x.TblVoucherVerfications).Where(x => x.IsActive == true && x.RegdNo != null && x.CustomerDetails != null && x.CustomerDetails.PhoneNumber != null && (x.RegdNo.ToLower() == RegdNo.ToLower() || x.CustomerDetails.PhoneNumber == PhoneNumber)).OrderByDescending(x => x.Id).ToList();

                }
                else if (exchageOrderObj.userRole == RoleDescription && selectedcompany == null)
                {
                    ExchangeList = _context.TblExchangeOrders.Include(x => x.ProductType)
                      .ThenInclude(x => x.ProductCat)
                      .Include(x => x.BusinessPartner)
                      .Include(x => x.CustomerDetails)
                      .Include(x => x.TblVoucherVerfications).Where(x => x.IsActive == true && x.RegdNo != null && x.CustomerDetails != null && x.CustomerDetails.PhoneNumber != null && (x.RegdNo.ToLower() == RegdNo.ToLower() || x.CustomerDetails.PhoneNumber == PhoneNumber)).OrderByDescending(x => x.Id).ToList();

                }
                if (ExchangeList != null)
                {
                    recordsTotal = ExchangeList.Count;
                    if (ExchangeList.Count > 0)
                    {
                        ExchangeList = ExchangeList.Skip(skip).Take(pageSize).ToList();
                        DealerDashboardViewModel dealerdata;
                        foreach (var item in ExchangeList)
                        {
                            string actionURL = string.Empty;
                            var regdno = item.RegdNo;



                            actionURL = "<a href ='" + URL + "/Exchange/Manage?Exchangeorderid=" + _protector.Encode(item.Id) + "' ><button onclick='RecordView(" + item.Id + ")' class='btn btn-sm btn-primary' data-bs-toggle='tooltip' data-bs-placement='top' title='View'><i class='fa-solid fa-eye'></i></button></a>";

                            dealerdata = new DealerDashboardViewModel();
                            dealerdata.Id = item.Id;
                            dealerdata.CompanyName = item.CompanyName;
                            dealerdata.actionUrl = actionURL;
                            dealerdata.RegdNo = item.RegdNo;
                            dealerdata.VoucherCode = item.VoucherCode;
                            dealerdata.ExchangePrice = item.ExchangePrice.ToString();
                            dealerdata.OrderReferenceId = item.SponsorServiceRefId;
                            dealerdata.CustomerName = item.CustomerDetails.FirstName + " " + item.CustomerDetails.LastName;
                            if (item.ProductType != null)
                            {
                                if (item.ProductType.ProductCat != null)
                                {
                                    dealerdata.OldProductCategory = item.ProductType.ProductCat.Description;
                                }

                                dealerdata.OldProductType = item.ProductType.Description;
                            }

                            dealerdata.Sweetner = item.Sweetener.ToString();
                            DateTime OrderDate = Convert.ToDateTime(item.CreatedDate);
                            dealerdata.OrderDate = OrderDate.ToString("dd/MMM/yyyy");
                            if (item.IsDefferedSettlement == true)
                            {
                                dealerdata.TypeOfSettelment = EnumHelper.DescriptionAttr(SettlementEnumForDashboard.deffred);
                            }
                            else
                            {
                                dealerdata.TypeOfSettelment = EnumHelper.DescriptionAttr(SettlementEnumForDashboard.instant);
                            }
                            voucherVerification = _voucherRepository.GetVoucherDataByExchangeId(item.Id);
                            if (voucherVerification != null)
                            {
                                DateTime voucherdate = Convert.ToDateTime(voucherVerification.CreatedDate);
                                dealerdata.VoucherRedeemDate = voucherdate.ToString("dd/MMM/yyyy");

                                string status = EnumHelper.DescriptionAttr(VoucherStatusEnum.Redeemed);
                                if (voucherVerification.VoucherStatusId == Convert.ToInt32(VoucherStatusEnum.Redeemed))
                                {
                                    dealerdata.VoucherStatus = status;
                                    dealerdata.Paymentstatus = EnumHelper.DescriptionAttr(SettlementEnumForDashboard.Paid);
                                }
                                if (voucherVerification.VoucherStatusId == Convert.ToInt32(VoucherStatusEnum.Captured))
                                {
                                    string capturedStatus = EnumHelper.DescriptionAttr(VoucherStatusEnum.Captured);
                                    dealerdata.VoucherStatus = capturedStatus;
                                    dealerdata.Paymentstatus = EnumHelper.DescriptionAttr(SettlementEnumForDashboard.NotPaid);
                                }
                            }
                            dealerdatalist.Add(dealerdata);


                        }
                    }

                }
                if (dealerdatalist.Count > 0)
                {
                    dealerdatalist.OrderByDescending(x => x.Id);
                }



            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("DealerDashboardModel", "OnGetStoreList", ex);
            }

            var data = dealerdatalist;
            var jsonData = new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data };
            return new JsonResult(jsonData);
        }

        public IActionResult OrderDataTableForBU(int? BusinessUnitId, string? RegdNo, string? SponsorOrderNumber, string? PhoneNumber, string? ReferenceId, string? Storecode, string? VoucherStatus, string? LatestStatusDescription, string? OrderId)
        {
            var draw = Request.Form["draw"].FirstOrDefault();
            var start = Request.Form["start"].FirstOrDefault();
            var length = Request.Form["length"].FirstOrDefault();
            var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
            var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();
            //var searchValue = Request.Form["search[value]"].FirstOrDefault();
            //searchValue = searchValue.Trim();
            int pageSize = length != null ? Convert.ToInt32(length) : 0;
            int skip = start != null ? Convert.ToInt32(start) : 0;
            int recordsTotal = 0;
            List<ExchangeOrderDataContract> exchangeObj = new List<ExchangeOrderDataContract>();
            List<DealerDashboardViewModel> dealerdatalist = new List<DealerDashboardViewModel>();
            List<TblExchangeOrder> ExchangeList = null;
            TblVoucherVerfication voucherVerification = null;
            int bpid = 0;
            string URL = _config.Value.URLPrefixforProd;
            #region handeling of search filters  for null

            if (RegdNo != null)
            {
                RegdNo = RegdNo.Trim();
            }
            if (!string.IsNullOrEmpty(RegdNo) && RegdNo != "null" && !string.IsNullOrWhiteSpace(RegdNo))
            {
                RegdNo = RegdNo.Trim();
            }
            else
            {
                RegdNo = null;
            }
            if (!string.IsNullOrEmpty(SponsorOrderNumber) && SponsorOrderNumber != "null" && !string.IsNullOrWhiteSpace(SponsorOrderNumber))
            {
                SponsorOrderNumber = SponsorOrderNumber.Trim();
            }
            else
            {
                SponsorOrderNumber = null;
            }

            if (!string.IsNullOrEmpty(PhoneNumber) && PhoneNumber != "null" && !string.IsNullOrWhiteSpace(PhoneNumber))
            {
                PhoneNumber = PhoneNumber.Trim();
            }
            else
            {
                PhoneNumber = null;
            }
            if (!string.IsNullOrEmpty(ReferenceId) && ReferenceId != "null" && !string.IsNullOrWhiteSpace(ReferenceId))
            {
                ReferenceId = ReferenceId.Trim();
            }
            else
            {
                ReferenceId = null;
            }
            if (!string.IsNullOrEmpty(Storecode) && Storecode != "null" && !string.IsNullOrWhiteSpace(Storecode))
            {
                Storecode = Storecode.Trim();
            }
            else
            {
                Storecode = null;
            }
            if (!string.IsNullOrEmpty(VoucherStatus) && VoucherStatus != "null" && VoucherStatus != "undefined" && !string.IsNullOrWhiteSpace(VoucherStatus))
            {
                VoucherStatus = VoucherStatus.Trim();
            }
            else
            {
                VoucherStatus = null;
            }
            if (!string.IsNullOrEmpty(LatestStatusDescription) && LatestStatusDescription != "null" && !string.IsNullOrWhiteSpace(LatestStatusDescription))
            {
                LatestStatusDescription = LatestStatusDescription.Trim();
            }
            else
            {
                LatestStatusDescription = null;
            }
            if (!string.IsNullOrEmpty(OrderId) && OrderId != "null" && !string.IsNullOrWhiteSpace(OrderId))
            {
                OrderId = OrderId.Trim();
            }
            else
            {
                OrderId = null;
            }
            #endregion
            try
            {
                ExchangeOrderDataContract exchageOrderObj = new ExchangeOrderDataContract();

                List<TblExchangeOrderStatus> StatusList = new List<TblExchangeOrderStatus>();

                StatusList = _context.TblExchangeOrderStatuses.ToList();

                string RoleDescription = EnumHelper.DescriptionAttr(RoleEnum.SuperAdmin);
                string comapnyDescription = EnumHelper.DescriptionAttr(CompanyNameenum.UTC);

                ExchangeList = _exchangeOrderRepository.GetOrderDetailsForBUDashboard(BusinessUnitId, RegdNo, SponsorOrderNumber, PhoneNumber, ReferenceId, Storecode, VoucherStatus, LatestStatusDescription, OrderId);
                if (ExchangeList != null)
                {
                    recordsTotal = ExchangeList.Count;
                    if (ExchangeList.Count > 0)
                    {
                        ExchangeList = ExchangeList.Skip(skip).Take(pageSize).ToList();
                        DealerDashboardViewModel dealerdata;
                        foreach (var item in ExchangeList)
                        {

                            string actionURL = string.Empty;
                            var regdno = item.RegdNo;
                            actionURL = "<a href ='" + URL + "/Exchange/Manage?Exchangeorderid=" + _protector.Encode(item.Id) + "' ><button onclick='RecordView(" + item.Id + ")' class='btn btn-sm btn-primary' data-bs-toggle='tooltip' data-bs-placement='top' title='View'><i class='fa-solid fa-eye'></i></button></a>";
                            dealerdata = new DealerDashboardViewModel();
                            dealerdata.Id = item.Id;
                            dealerdata.actionUrl = actionURL;
                            dealerdata.CompanyName = item.CompanyName;
                            dealerdata.RegdNo = item.RegdNo;
                            dealerdata.VoucherCode = item.VoucherCode;
                            dealerdata.ExchangePrice = item.ExchangePrice.ToString();
                            dealerdata.OrderReferenceId = item.SponsorServiceRefId;
                            dealerdata.SponsorOrderNumber = item.SponsorOrderNumber;
                            dealerdata.StoreCode = item.StoreCode;

                            dealerdata.ActualAmountAsPerQC = item?.TblOrderTrans?.FirstOrDefault()?.FinalPriceAfterQc?.ToString();
                            dealerdata.QCComment = item?.TblOrderTrans?.FirstOrDefault()?.TblOrderQcs?.FirstOrDefault()?.Qccomments;
                            dealerdata.ActualProdQlty = item?.TblOrderTrans?.FirstOrDefault()?.TblOrderQcs?.FirstOrDefault()?.QualityAfterQc;
                            dealerdata.LatestStatusDescription = StatusList.Where(x => x.Id == item?.StatusId)?.FirstOrDefault()?.StatusDescription;
                            dealerdata.LatestDateAndTime = item?.ModifiedDate.ToString();
                            dealerdata.City = item?.CustomerDetails?.City;
                            dealerdata.State = item?.CustomerDetails?.State;
                            dealerdata.PinCode = item?.CustomerDetails?.ZipCode;
                            dealerdata.CustomerDeclareQtly = item?.ProductCondition;
                            dealerdata.ActualPickupDate = item?.TblOrderTrans?.FirstOrDefault()?.TblOrderLgcs?.FirstOrDefault()?.ActualPickupDate?.ToString();

                            if (item?.IsDefferedSettlement != null && item?.IsDefferedSettlement == true)
                            {
                                dealerdata.AmountPayableThroughLGC = item?.TblOrderTrans?.FirstOrDefault()?.FinalPriceAfterQc?.ToString();
                            }

                            if (item.Status != null)
                            {
                                dealerdata.Status = item.Status.StatusCode;
                            }

                            dealerdata.CustomerName = item.CustomerDetails.FirstName + " " + item.CustomerDetails.LastName;
                            if (item.ProductType != null)
                            {
                                if (item.ProductType.ProductCat != null)
                                {
                                    dealerdata.OldProductCategory = item.ProductType.ProductCat.Description;
                                }

                                dealerdata.OldProductType = item.ProductType.Description;
                            }

                            dealerdata.Sweetner = item.Sweetener.ToString();
                            DateTime OrderDate = Convert.ToDateTime(item.CreatedDate);
                            dealerdata.OrderDate = OrderDate.ToString("dd/MMM/yyyy");
                            if (item.IsDefferedSettlement == true)
                            {
                                dealerdata.TypeOfSettelment = EnumHelper.DescriptionAttr(SettlementEnumForDashboard.deffred);
                            }
                            else
                            {
                                dealerdata.TypeOfSettelment = EnumHelper.DescriptionAttr(SettlementEnumForDashboard.instant);
                            }
                            voucherVerification = _voucherRepository.GetVoucherDataByExchangeId(item.Id);
                            if (voucherVerification != null)
                            {
                                DateTime voucherdate = Convert.ToDateTime(voucherVerification.CreatedDate);
                                dealerdata.VoucherRedeemDate = voucherdate.ToString("dd/MMM/yyyy");

                                string status = EnumHelper.DescriptionAttr(VoucherStatusEnum.Redeemed);
                                if (voucherVerification.VoucherStatusId == Convert.ToInt32(VoucherStatusEnum.Redeemed))
                                {
                                    dealerdata.VoucherStatus = status;
                                    dealerdata.Paymentstatus = EnumHelper.DescriptionAttr(SettlementEnumForDashboard.Paid);
                                }
                                if (voucherVerification.VoucherStatusId == Convert.ToInt32(VoucherStatusEnum.Captured))
                                {
                                    string capturedStatus = EnumHelper.DescriptionAttr(VoucherStatusEnum.Captured);
                                    dealerdata.VoucherStatus = capturedStatus;
                                    dealerdata.Paymentstatus = EnumHelper.DescriptionAttr(SettlementEnumForDashboard.NotPaid);
                                }
                            }
                            dealerdatalist.Add(dealerdata);


                        }
                    }

                }
                if (dealerdatalist.Count > 0)
                {
                    dealerdatalist.OrderByDescending(x => x.Id);
                }



            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("DealerDashboardModel", "OnGetStoreList", ex);
            }

            var data = dealerdatalist;
            var jsonData = new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data };
            return new JsonResult(jsonData);
        }

        public IActionResult SearchByRegdNOForBUDashBoard(string RegdNo, string CompanyName, string UserCompany, string SponsorOrderNumber, string PhoneNumber)
        {
            var draw = Request.Form["draw"].FirstOrDefault();
            var start = Request.Form["start"].FirstOrDefault();
            var length = Request.Form["length"].FirstOrDefault();
            var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
            var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();
            //var searchValue = Request.Form["search[value]"].FirstOrDefault();
            //searchValue = searchValue.Trim();
            int pageSize = length != null ? Convert.ToInt32(length) : 0;
            int skip = start != null ? Convert.ToInt32(start) : 0;
            int recordsTotal = 0;
            List<ExchangeOrderDataContract> exchangeObj = new List<ExchangeOrderDataContract>();
            List<DealerDashboardViewModel> dealerdatalist = new List<DealerDashboardViewModel>();
            List<TblExchangeOrder> ExchangeList = null;
            TblVoucherVerfication voucherVerification = null;
            int bpid = 0;
            string URL = _config.Value.URLPrefixforProd;
            try
            {
                ExchangeOrderDataContract exchageOrderObj = new ExchangeOrderDataContract();
                exchageOrderObj.userCompany = UserCompany;
                exchageOrderObj.CompanyName = CompanyName;
                exchageOrderObj.RegdNo = RegdNo;
                exchageOrderObj.SponsorOrderNumber = SponsorOrderNumber;
                exchageOrderObj.PhoneNumber = PhoneNumber;

                if (exchageOrderObj.userCompany == "NotDefined")
                {
                    UserCompany = null;
                }
                if (exchageOrderObj.CompanyName == "NotDefined")
                {
                    CompanyName = null;
                }
                if (exchageOrderObj.RegdNo == "NotDefined")
                {
                    RegdNo = null;
                }
                if (exchageOrderObj.SponsorOrderNumber == "NotDefined")
                {
                    SponsorOrderNumber = null;
                }
                if (exchageOrderObj.PhoneNumber == "NotDefined")
                {
                    PhoneNumber = null;
                }
                if (exchageOrderObj.startdate != null && exchageOrderObj.enddate != null)
                {
                    exchageOrderObj.StartRangedate = Convert.ToDateTime(exchageOrderObj.startdate).AddMinutes(-1);
                    exchageOrderObj.EndRangeDate = Convert.ToDateTime(exchageOrderObj.enddate).AddDays(1).AddSeconds(-1);
                }


                string RoleDescription = EnumHelper.DescriptionAttr(RoleEnum.SuperAdmin);
                string comapnyDescription = EnumHelper.DescriptionAttr(CompanyNameenum.UTC);

                if (!string.IsNullOrEmpty(CompanyName))
                {
                    ExchangeList = _context.TblExchangeOrders.Include(x => x.ProductType)
                      .ThenInclude(x => x.ProductCat)
                      .Include(x => x.BusinessPartner)
                      .Include(x => x.CustomerDetails)
                      .Include(x => x.TblVoucherVerfications).Where(x => x.IsActive == true && x.CompanyName == CompanyName && (string.IsNullOrEmpty(RegdNo) || x.RegdNo == RegdNo) && (string.IsNullOrEmpty(SponsorOrderNumber) || x.SponsorOrderNumber == SponsorOrderNumber) && (string.IsNullOrEmpty(PhoneNumber) || x.CustomerDetails.PhoneNumber == PhoneNumber)).OrderByDescending(x => x.Id).ToList();

                }
                else if (string.IsNullOrEmpty(UserCompany) && !string.IsNullOrEmpty(CompanyName))
                {
                    ExchangeList = _context.TblExchangeOrders.Include(x => x.ProductType)
                    .ThenInclude(x => x.ProductCat)
                    .Include(x => x.BusinessPartner)
                    .Include(x => x.CustomerDetails)
                    .Include(x => x.TblVoucherVerfications).Where(x => x.IsActive == true && x.CompanyName == CompanyName && (string.IsNullOrEmpty(RegdNo) || x.RegdNo == RegdNo) && (string.IsNullOrEmpty(SponsorOrderNumber) || x.SponsorOrderNumber == SponsorOrderNumber) && (string.IsNullOrEmpty(PhoneNumber) || x.CustomerDetails.PhoneNumber == PhoneNumber)).OrderByDescending(x => x.Id).ToList();
                }
                else if (string.IsNullOrEmpty(UserCompany) && string.IsNullOrEmpty(CompanyName))
                {
                    ExchangeList = _context.TblExchangeOrders.Include(x => x.ProductType)
                   .ThenInclude(x => x.ProductCat)
                   .Include(x => x.BusinessPartner)
                   .Include(x => x.CustomerDetails)
                   .Include(x => x.TblVoucherVerfications).Where(x => x.IsActive == true && (string.IsNullOrEmpty(RegdNo) || x.RegdNo == RegdNo) && (string.IsNullOrEmpty(SponsorOrderNumber) || x.SponsorOrderNumber == SponsorOrderNumber) && (string.IsNullOrEmpty(PhoneNumber) || x.CustomerDetails.PhoneNumber == PhoneNumber)).OrderByDescending(x => x.Id).ToList();
                }
                if (ExchangeList != null)
                {
                    recordsTotal = ExchangeList.Count;
                    if (ExchangeList.Count > 0)
                    {
                        ExchangeList = ExchangeList.Skip(skip).Take(pageSize).ToList();
                        DealerDashboardViewModel dealerdata;
                        foreach (var item in ExchangeList)
                        {

                            string actionURL = string.Empty;
                            var regdno = item.RegdNo;



                            actionURL = "<a href ='" + URL + "/Exchange/Manage?Exchangeorderid=" + _protector.Encode(item.Id) + "' ><button onclick='RecordView(" + item.Id + ")' class='btn btn-sm btn-primary' data-bs-toggle='tooltip' data-bs-placement='top' title='View'><i class='fa-solid fa-eye'></i></button></a>";


                            dealerdata = new DealerDashboardViewModel();
                            dealerdata.Id = item.Id;
                            dealerdata.actionUrl = actionURL;
                            dealerdata.CompanyName = item.CompanyName;
                            dealerdata.RegdNo = item.RegdNo;
                            dealerdata.VoucherCode = item.VoucherCode;
                            dealerdata.ExchangePrice = item.ExchangePrice.ToString();
                            dealerdata.OrderReferenceId = item.SponsorServiceRefId;
                            dealerdata.CustomerName = item.CustomerDetails.FirstName + " " + item.CustomerDetails.LastName;
                            if (item.ProductType != null)
                            {
                                if (item.ProductType.ProductCat != null)
                                {
                                    dealerdata.OldProductCategory = item.ProductType.ProductCat.Description;
                                }

                                dealerdata.OldProductType = item.ProductType.Description;
                            }

                            dealerdata.Sweetner = item.Sweetener.ToString();
                            DateTime OrderDate = Convert.ToDateTime(item.CreatedDate);
                            dealerdata.OrderDate = OrderDate.ToString("dd/MMM/yyyy");
                            if (item.IsDefferedSettlement == true)
                            {
                                dealerdata.TypeOfSettelment = EnumHelper.DescriptionAttr(SettlementEnumForDashboard.deffred);
                            }
                            else
                            {
                                dealerdata.TypeOfSettelment = EnumHelper.DescriptionAttr(SettlementEnumForDashboard.instant);
                            }
                            voucherVerification = _voucherRepository.GetVoucherDataByExchangeId(item.Id);
                            if (voucherVerification != null)
                            {
                                DateTime voucherdate = Convert.ToDateTime(voucherVerification.CreatedDate);
                                dealerdata.VoucherRedeemDate = voucherdate.ToString("dd/MMM/yyyy");

                                string status = EnumHelper.DescriptionAttr(VoucherStatusEnum.Redeemed);
                                if (voucherVerification.VoucherStatusId == Convert.ToInt32(VoucherStatusEnum.Redeemed))
                                {
                                    dealerdata.VoucherStatus = status;
                                    dealerdata.Paymentstatus = EnumHelper.DescriptionAttr(SettlementEnumForDashboard.Paid);
                                }
                                if (voucherVerification.VoucherStatusId == Convert.ToInt32(VoucherStatusEnum.Captured))
                                {
                                    string capturedStatus = EnumHelper.DescriptionAttr(VoucherStatusEnum.Captured);
                                    dealerdata.VoucherStatus = capturedStatus;
                                    dealerdata.Paymentstatus = EnumHelper.DescriptionAttr(SettlementEnumForDashboard.NotPaid);
                                }
                            }
                            dealerdatalist.Add(dealerdata);


                        }
                    }

                }
                if (dealerdatalist.Count > 0)
                {
                    dealerdatalist.OrderByDescending(x => x.Id);
                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("DealerDashboardModel", "OnGetStoreList", ex);
            }

            var data = dealerdatalist;
            var jsonData = new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data };
            return new JsonResult(jsonData);
        }

        #region List of status code 5P for QC Admin
        [HttpPost]
        public IActionResult UpperLevelCapListForQCBonus(int companyId, DateTime? startDate, DateTime? endDate)
        {
            List<TblOrderQc> tblOrderQcList = null;
            List<OrderQcViewModel> orderQcList = null;
            OrderQcViewModel orderQcViewModel = null;
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

                if (startDate == null && endDate == null)
                {
                    tblOrderQcList = _context.TblOrderQcs
                       .Include(x => x.OrderTrans).ThenInclude(x => x.Exchange).ThenInclude(x => x.ProductType).ThenInclude(x => x.ProductCat)
                       .Include(x => x.OrderTrans).ThenInclude(x => x.Exchange).ThenInclude(x => x.CustomerDetails)
                       .Where(x => x.IsActive == true && x.StatusId == 53 && (string.IsNullOrEmpty(searchValue)
                       || (x.OrderTrans.Exchange.CustomerDetails.FirstName != null ? x.OrderTrans.Exchange.CustomerDetails.FirstName.ToLower().Contains(searchValue.ToLower().Trim()) : false)
                       || (x.OrderTrans.Exchange.CustomerDetails.PhoneNumber != null ? x.OrderTrans.Exchange.CustomerDetails.PhoneNumber.ToLower().Contains(searchValue.ToLower().Trim()) : false)
                       || (x.OrderTrans.Exchange.CustomerDetails.ZipCode != null ? x.OrderTrans.Exchange.CustomerDetails.ZipCode.ToLower().Contains(searchValue.ToLower().Trim()) : false)
                       || (x.OrderTrans.Exchange.RegdNo != null ? x.OrderTrans.Exchange.RegdNo.ToLower().Contains(searchValue.ToLower().Trim()) : false)
                       || (x.OrderTrans.Exchange.CompanyName != null ? x.OrderTrans.Exchange.CompanyName.ToLower().Contains(searchValue.ToLower().Trim()) : false)
                       )).ToList();
                }
                else
                {
                    startDate = Convert.ToDateTime(startDate).AddMinutes(-1);
                    endDate = Convert.ToDateTime(endDate).AddDays(1).AddSeconds(-1);

                    tblOrderQcList = _context.TblOrderQcs
                       .Include(x => x.OrderTrans).ThenInclude(x => x.Exchange).ThenInclude(x => x.ProductType).ThenInclude(x => x.ProductCat)
                       .Include(x => x.OrderTrans).ThenInclude(x => x.Exchange).ThenInclude(x => x.CustomerDetails)
                       .Where(x => x.IsActive == true && x.StatusId == 53 && x.CreatedDate >= startDate && x.CreatedDate <= endDate && (string.IsNullOrEmpty(searchValue)
                       || (x.OrderTrans.Exchange.CustomerDetails.FirstName != null ? x.OrderTrans.Exchange.CustomerDetails.FirstName.ToLower().Contains(searchValue.ToLower().Trim()) : false)
                       || (x.OrderTrans.Exchange.CustomerDetails.PhoneNumber != null ? x.OrderTrans.Exchange.CustomerDetails.PhoneNumber.ToLower().Contains(searchValue.ToLower().Trim()) : false)
                       || (x.OrderTrans.Exchange.CustomerDetails.ZipCode != null ? x.OrderTrans.Exchange.CustomerDetails.ZipCode.ToLower().Contains(searchValue.ToLower().Trim()) : false)
                       || (x.OrderTrans.Exchange.RegdNo != null ? x.OrderTrans.Exchange.RegdNo.ToLower().Contains(searchValue.ToLower().Trim()) : false)
                       || (x.OrderTrans.Exchange.CompanyName != null ? x.OrderTrans.Exchange.CompanyName.ToLower().Contains(searchValue.ToLower().Trim()) : false)
                       )).ToList();
                }

                recordsTotal = tblOrderQcList != null ? tblOrderQcList.Count : 0;

                if (tblOrderQcList != null)
                {
                    tblOrderQcList = tblOrderQcList.Skip(skip).Take(pageSize).ToList();
                }
                else
                {
                    tblOrderQcList = new List<TblOrderQc>();
                }
                orderQcList = new List<OrderQcViewModel>();

                if (tblOrderQcList != null)
                {
                    foreach (var item in tblOrderQcList)
                    {
                        orderQcViewModel = new OrderQcViewModel();
                        string actionURL = string.Empty;
                        actionURL = " <ul class='actions'>";
                        actionURL = "<a href ='" + URL + "/QCIndex/UpperBonusCap?OrderTransId=" + item.OrderTrans.OrderTransId + "' ><button onclick='View(" + item.OrderTrans.OrderTransId + ")' class='btn btn-primary btn'>View</button></a>";
                        actionURL = actionURL + "</ul>";
                        orderQcViewModel.Id = item.OrderTrans.OrderTransId;
                        orderQcViewModel.Action = actionURL;
                        orderQcViewModel.CompanyName = item.OrderTrans.Exchange.CompanyName != null ? item.OrderTrans.Exchange.CompanyName : string.Empty;
                        orderQcViewModel.RegdNo = item.OrderTrans.Exchange.RegdNo != null ? item.OrderTrans.Exchange.RegdNo : string.Empty;
                        orderQcViewModel.CustomerName = item.OrderTrans.Exchange.CustomerDetails.FirstName + " " + item.OrderTrans.Exchange.CustomerDetails.LastName;
                        orderQcViewModel.ProductType = item.OrderTrans.Exchange.ProductType.Description + "( " + item.OrderTrans.Exchange.ProductType.Size + ")";
                        string dateTime = Convert.ToDateTime(item.Qcdate).ToString("dd/MM/yyyy");
                        orderQcViewModel.QCDate = dateTime;
                        orderQcViewModel.CustomerPhoneNumber = item.OrderTrans.Exchange.CustomerDetails.PhoneNumber != null ? item.OrderTrans.Exchange.CustomerDetails.PhoneNumber : string.Empty;
                        orderQcViewModel.CustomerEmail = item.OrderTrans.Exchange.CustomerDetails.Email != null ? item.OrderTrans.Exchange.CustomerDetails.Email : string.Empty;
                        orderQcViewModel.CustomerAddress = item.OrderTrans.Exchange.CustomerDetails.Address1 + " " + item.OrderTrans.Exchange.CustomerDetails.Address2;
                        orderQcViewModel.CustomerCity = item.OrderTrans.Exchange.CustomerDetails.City != null ? item.OrderTrans.Exchange.CustomerDetails.City : string.Empty;
                        orderQcViewModel.CustomerState = item.OrderTrans.Exchange.CustomerDetails.State != null ? item.OrderTrans.Exchange.CustomerDetails.State : string.Empty;
                        orderQcViewModel.FinalPrice = item.OrderTrans.Exchange.FinalExchangePrice != null ? (decimal)item.OrderTrans.Exchange.FinalExchangePrice : 0;
                        orderQcList.Add(orderQcViewModel);
                    }
                }

                var data = orderQcList.OrderByDescending(x => x.ModifiedDate);
                var jsonData = new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data };
                return Ok(jsonData);
            }
            catch (Exception ex)
            {
                throw;
            }

        }
        #endregion

        //Vehicle Incentive Chart Master Added by Kranti
        #region
        [HttpPost]
        public async Task<ActionResult> GetVehicleIncentiveList(DateTime? startDate, DateTime? endDate, int? ddlprodcatid, int? ddlprodcattypeid, string? BuId)
        {
           
            string URL = _config.Value.URLPrefixforProd;
            
            if (string.IsNullOrEmpty(BuId) || BuId == "null")
            { BuId = "".Trim().ToLower(); }
            else
            { BuId = BuId.Trim().ToLower(); }
            

            List<TblVehicleIncentive> tblVehicleIncentive = null;
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
                int count = 0;
                if (startDate == null && endDate == null)
                {
                   

                    count =  _context.TblVehicleIncentives
                        .Include(x => x.BusinessUnit)
                        .Include(x => x.ProductCategory)
                        .Include(x => x.ProductType)
                        .Where(x => x.IsActive == true).Count();

                    if(count > 0)
                    {
                        tblVehicleIncentive = await _context.TblVehicleIncentives.Where(x => x.IsActive == true
                      && (ddlprodcatid > 0 ? (x.ProductCategory.Id == 0 ? false : (x.ProductCategory.Id == ddlprodcatid)) : true)
                                      && (ddlprodcattypeid > 0 ? (x.ProductTypeId == 0 ? false : (x.ProductTypeId == ddlprodcattypeid)) : true)
                                      && x.BusinessUnit.Name.Contains(BuId)
                          && ((string.IsNullOrEmpty(BuId)) || x.BusinessUnit.Name == BuId)).OrderByDescending(x=> x.IncentiveId).Skip(skip).Take(pageSize).ToListAsync();



                    }


                }
                else
                {
                    startDate = Convert.ToDateTime(startDate).AddMinutes(-1);
                    endDate = Convert.ToDateTime(endDate).AddDays(1).AddSeconds(-1);
                   
                    count = _context.TblVehicleIncentives
                        .Include(x => x.BusinessUnit)
                        .Include(x => x.ProductCategory)
                        .Include(x => x.ProductType)
                        .Where(x => x.IsActive == true && x.CreatedDate >= startDate && x.CreatedDate <= endDate).Count();

                    if (count > 0)
                    {
                        tblVehicleIncentive = await _context.TblVehicleIncentives.Where(x => x.IsActive == true && x.CreatedDate >= startDate && x.CreatedDate <= endDate
                     && (ddlprodcatid > 0 ? (x.ProductCategory.Id == 0 ? false : (x.ProductCategory.Id == ddlprodcatid)) : true)
                                      && (ddlprodcattypeid > 0 ? (x.ProductTypeId == 0 ? false : (x.ProductTypeId == ddlprodcattypeid)) : true)
                                      && x.BusinessUnit.Name.Contains(BuId)
                          && ((string.IsNullOrEmpty(BuId)) || x.BusinessUnit.Name == BuId)).OrderByDescending(x => x.IncentiveId).Skip(skip).Take(pageSize).ToListAsync();
                    }
                }

                recordsTotal = count;
               
                List<VehicleIncentiveViewModel> VehicleIncentiveList = _mapper.Map<List<TblVehicleIncentive>, List<VehicleIncentiveViewModel>>(tblVehicleIncentive);
                string actionURL = string.Empty;
                string ImageURL = string.Empty;

                foreach (VehicleIncentiveViewModel item in VehicleIncentiveList)
                {

                    actionURL = " <div class='actionbtns'>";
                    actionURL = actionURL + "<a href=' " + URL + "/VehicleIncentive/Manage?id=" + _protector.Encode(item.IncentiveId) + "' data-bs-toggle='tooltip' data-bs-placement='top' title='Edit' class='btn btn-sm btn-primary'><i class='fa-solid fa-pen'></i></a> " +
                        "<a href='javascript:void(0)' onclick='deleteConfirmVehicleIncentive(" + item.IncentiveId + ")' class='btn btn-sm btn-danger' data-bs-toggle='tooltip' data-bs-placement='top' title='Delete'><i class='fa-solid fa-trash'></i></a>";
                    actionURL = actionURL + " </div>";
                    item.Action = actionURL;



                    TblVehicleIncentive VehicleDetails = tblVehicleIncentive.FirstOrDefault(x => x.IncentiveId == item.IncentiveId);

                    if (VehicleDetails.CreatedDate != null)
                    {
                        //var Date = (DateTime)item.CreatedDate;
                        //item.Date = Date.ToShortDateString();


                        DateTime dateTime = (DateTime)item.CreatedDate;
                        item.Date = dateTime.ToString("yyyy-MM-dd");
                        //item.CreatedDate = VehicleDetails.CreatedDate;

                    }
                    if (VehicleDetails.PickupTatinHr != null)
                    {
                        TimeSpan timeSpan = (TimeSpan)item.PickupTatinHr;
                        item.PickupTat = timeSpan.ToString("hh\\:mm");

                    }

                    if (VehicleDetails.DropTatinHr != null)
                    {
                        TimeSpan Timespandrop = (TimeSpan)item.DropTatinHr;
                        item.DropTat = Timespandrop.ToString("hh\\:mm");

                    }
                    if (VehicleDetails != null && VehicleDetails.ProductCategoryId > 0)
                    {
                        TblProductCategory tblProductCategory = _context.TblProductCategories.FirstOrDefault(x => x.Id == VehicleDetails.ProductCategoryId);
                        item.ProductCategoryName = tblProductCategory != null ? tblProductCategory.Description : string.Empty;
                    }
                    if (VehicleDetails != null && VehicleDetails.ProductTypeId > 0)
                    {
                        TblProductType tblProductType = _context.TblProductTypes.FirstOrDefault(x => x.Id == VehicleDetails.ProductTypeId);
                        item.ProductTypeName = tblProductType != null ? tblProductType.Description + " " + tblProductType.Size : string.Empty;
                    }

                    if (VehicleDetails != null && VehicleDetails.BusinessUnitId > 0)
                    {
                        TblBusinessUnit TblBusinessUnit = _context.TblBusinessUnits.FirstOrDefault(x => x.BusinessUnitId == VehicleDetails.BusinessUnitId);
                        item.BusinessUnitName = TblBusinessUnit != null ? TblBusinessUnit.Name : string.Empty;
                    }

                }

                var data = VehicleIncentiveList;
                var jsonData = new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data };
                return Ok(jsonData);
            }
            catch (Exception ex)
            {
                throw;
            }

        }

        #endregion

        #region Product Technology 



        [HttpPost]
        public async Task<ActionResult> GetProductTechnologyList(int? buid, DateTime? startDate, DateTime? endDate)
        {
            List<TblProductTechnology> tblProductTechnology = null;
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
                //_context = new Digi2L_DevContext();

                if (startDate == null && endDate == null)
                {

                    tblProductTechnology = await _context.TblProductTechnologies.Where(x => x.IsActive == true
                            && (string.IsNullOrEmpty(searchValue)
                            || x.ProductTechnologyName.ToLower().Contains(searchValue.ToLower().Trim()))).ToListAsync();

                }
                else
                {

                    startDate = Convert.ToDateTime(startDate).AddMinutes(-1);
                    endDate = Convert.ToDateTime(endDate).AddDays(1).AddSeconds(-1);
                    tblProductTechnology = await _context.TblProductTechnologies.Where(x => x.IsActive == true && x.CreatedDate >= startDate && x.CreatedDate <= endDate
                            && (string.IsNullOrEmpty(searchValue) || x.ProductTechnologyName.ToLower().Contains(searchValue.ToLower().Trim()))).ToListAsync();

                }

                recordsTotal = tblProductTechnology != null ? tblProductTechnology.Count : 0;
                if (tblProductTechnology != null)
                {
                    tblProductTechnology = sortColumnDirection.Equals(SortingOrder.ASCENDING) ? tblProductTechnology.OrderBy(o => o.GetType().GetProperty(sortColumn).GetValue(o, null)).ToList() : tblProductTechnology.OrderByDescending(o => o.GetType().GetProperty(sortColumn).GetValue(o, null)).ToList();
                    tblProductTechnology = tblProductTechnology.Skip(skip).Take(pageSize).ToList();
                }
                else
                    tblProductTechnology = new List<TblProductTechnology>();

                List<ProductTechnologyViewModel> ProductTechnologyList = _mapper.Map<List<TblProductTechnology>, List<ProductTechnologyViewModel>>(tblProductTechnology);
                //List<ProductCategoryViewModel> ProductCategoryList = _mapper.Map<List<TblProductCategory>, List<ProductCategoryViewModel>>(tblProductCategory);
                string actionURL = string.Empty;

                foreach (ProductTechnologyViewModel item in ProductTechnologyList)
                {
                    actionURL = "<div class='actionbtns'>";
                    actionURL = actionURL + "<a href=' " + URL + "/ProductTechnology/Manage?id=" + _protector.Encode(item.ProductTechnologyId) + "' data-bs-toggle='tooltip' data-bs-placement='top' title='Edit' class='btn btn-sm btn-primary'><i class='fa-solid fa-pen'></i></a> " +
                        "<a href='javascript: void(0)' onclick='deleteConfirm(" + item.ProductTechnologyId + ")' class='btn btn-sm btn-danger' data-bs-toggle='tooltip' data-bs-placement='top' title='Delete'><i class='fa-solid fa-trash'></i></a>";
                    actionURL = actionURL + " </div>";
                    item.Action = actionURL;
                    TblProductTechnology ProductTechnology = tblProductTechnology.FirstOrDefault(x => x.ProductTechnologyId == item.ProductTechnologyId);
                    if (ProductTechnology.CreatedDate != null && ProductTechnology.CreatedDate != null)
                    {
                        //var Date = (DateTime)item.CreatedDate;
                        //item.Date = Date.ToShortDateString();


                        DateTime dateTime = (DateTime)item.CreatedDate;
                        item.Date = dateTime.ToString("yyyy-MM-dd");
                        item.CreatedDate = ProductTechnology.CreatedDate;

                    }
                    //if (ProductTechnology != null && ProductTechnology.ProductTypeId > 0)

                    if (ProductTechnology != null && ProductTechnology.ProductCatId > 0)
                    {
                        TblProductCategory tblProductCategory = _context.TblProductCategories.FirstOrDefault(x => x.Id == ProductTechnology.ProductCatId);
                        item.ProductCategoryDiscription = tblProductCategory != null ? tblProductCategory.Description : string.Empty;
                    }


                }

                var data = ProductTechnologyList;
                var jsonData = new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data };
                return Ok(jsonData);
            }
            catch (Exception ex)
            {
                throw;
            }


        }


        #endregion


        #region QuestionerLOVViewModel


        //[HttpPost]
        //public async Task<ActionResult> GetQuestionerLOVList(int? buid, DateTime? startDate, DateTime? endDate)
        //{
        //    List<TblQuestionerLov> tblQuestionerLOV = null;
        //    string URL = _config.Value.URLPrefixforProd;

        //    try
        //    {
        //        var draw = Request.Form["draw"].FirstOrDefault();
        //        var start = Request.Form["start"].FirstOrDefault();
        //        var length = Request.Form["length"].FirstOrDefault();
        //        var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
        //        var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();
        //        var searchValue = Request.Form["search[value]"].FirstOrDefault();
        //        int pageSize = length != null ? Convert.ToInt32(length) : 0;
        //        int skip = start != null ? Convert.ToInt32(start) : 0;
        //        int recordsTotal = 0;
        //        //_context = new Digi2L_DevContext();

        //        if (startDate == null && endDate == null)
        //        {


        //            tblQuestionerLOV = await _context.TblQuestionerLovs.Where(x => x.IsActive == true
        //                    && (string.IsNullOrEmpty(searchValue)
        //                    || x.ProductTechnologyName.ToLower().Contains(searchValue.ToLower().Trim()))).ToListAsync();


        //        }
        //        else
        //        {


        //            startDate = Convert.ToDateTime(startDate).AddMinutes(-1);
        //            endDate = Convert.ToDateTime(endDate).AddDays(1).AddSeconds(-1);
        //            tblQuestionerLOV = await _context.TblQuestionerLovs.Where(x => x.IsActive == true && x.CreatedDate >= startDate && x.CreatedDate <= endDate
        //                    && (string.IsNullOrEmpty(searchValue) || x.ProductTechnologyName.ToLower().Contains(searchValue.ToLower().Trim()))).ToListAsync();


        //        }

        //        recordsTotal = tblQuestionerLOV != null ? tblQuestionerLOV.Count : 0;
        //        if (tblQuestionerLOV != null)
        //        {
        //            tblQuestionerLOV = sortColumnDirection.Equals(SortingOrder.ASCENDING) ? tblQuestionerLOV.OrderBy(o => o.GetType().GetProperty(sortColumn).GetValue(o, null)).ToList() : tblQuestionerLOV.OrderByDescending(o => o.GetType().GetProperty(sortColumn).GetValue(o, null)).ToList();
        //            tblQuestionerLOV = tblQuestionerLOV.Skip(skip).Take(pageSize).ToList();
        //        }
        //        else
        //            tblQuestionerLOV = new List<TblQuestionerLov>();


        //        List<ProductTechnologyViewModel> ProductTechnologyList = _mapper.Map<List<TblProductTechnology>, List<ProductTechnologyViewModel>>(tblQuestionerLOV);
        //        //List<ProductCategoryViewModel> ProductCategoryList = _mapper.Map<List<TblProductCategory>, List<ProductCategoryViewModel>>(tblProductCategory);
        //        string actionURL = string.Empty;

        //        foreach (ProductTechnologyViewModel item in ProductTechnologyList)
        //        {
        //            actionURL = "<div class='actionbtns'>";
        //            actionURL = actionURL + "<a href=' " + URL + "/ProductTechnology/Manage?id=" + _protector.Encode(item.ProductTechnologyId) + "' data-bs-toggle='tooltip' data-bs-placement='top' title='Edit' class='btn btn-sm btn-primary'><i class='fa-solid fa-pen'></i></a> " +
        //                "<a href='javascript: void(0)' onclick='deleteConfirm(" + item.ProductTechnologyId + ")' class='btn btn-sm btn-danger' data-bs-toggle='tooltip' data-bs-placement='top' title='Delete'><i class='fa-solid fa-trash'></i></a>";
        //            actionURL = actionURL + " </div>";
        //            item.Action = actionURL;
        //            TblProductTechnology ProductTechnology = tblQuestionerLOV.FirstOrDefault(x => x.ProductTechnologyId == item.ProductTechnologyId);
        //            if (ProductTechnology.CreatedDate != null && ProductTechnology.CreatedDate != null)
        //            {
        //                //var Date = (DateTime)item.CreatedDate;
        //                //item.Date = Date.ToShortDateString();


        //                DateTime dateTime = (DateTime)item.CreatedDate;
        //                item.Date = dateTime.ToString("yyyy-MM-dd");
        //                item.CreatedDate = ProductTechnology.CreatedDate;

        //            }
        //            //if (ProductTechnology != null && ProductTechnology.ProductTypeId > 0)

        //            if (ProductTechnology != null && ProductTechnology.ProductCatId > 0)
        //            {
        //                TblProductCategory tblProductCategory = _context.TblProductCategories.FirstOrDefault(x => x.Id == ProductTechnology.ProductCatId);
        //                item.ProductCategoryDiscription = tblProductCategory != null ? tblProductCategory.Description : string.Empty;
        //            }



        //        }


        //        var data = ProductTechnologyList;
        //        var jsonData = new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data };
        //        return Ok(jsonData);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw;
        //    }



        //}


        #endregion


        #region dsalPricemaster

        [HttpPost]
        public async Task<ActionResult> GetUniversalPriceMasterList(string? PriceMasterName, DateTime? startDate, DateTime? endDate)
        {
            List<TblUniversalPriceMaster> tblUniversalPriceMasterlist = null;
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
                //_context = new Digi2L_DevContext();

                if (startDate == null && endDate == null)
                {
                    

                    tblUniversalPriceMasterlist = await _context.TblUniversalPriceMasters.Where(x => 

                                  (string.IsNullOrEmpty(searchValue)
                                 || x.PriceMasterName.ToLower().Contains(searchValue.ToLower().Trim())
                                 || x.ProductCategory.Description.ToLower().Contains(searchValue.ToLower().Trim())
                                 || x.ProductTypeName.ToLower().Contains(searchValue.ToLower().Trim())
                                 || x.ProductTypeCode.ToLower().Contains(searchValue.ToLower().Trim())

                                 || x.QuotePHigh.ToLower().Contains(searchValue.ToLower().Trim())
                                 || x.QuoteQHigh.ToLower().Contains(searchValue.ToLower().Trim())
                                 || x.QuoteRHigh.ToLower().Contains(searchValue.ToLower().Trim())
                                 || x.QuoteSHigh.ToLower().Contains(searchValue.ToLower().Trim())

                                 || x.QuoteP.ToLower().Contains(searchValue.ToLower().Trim())
                                 || x.QuoteQ.ToLower().Contains(searchValue.ToLower().Trim())
                                 || x.QuoteR.ToLower().Contains(searchValue.ToLower().Trim())
                                 || x.QuoteS.ToLower().Contains(searchValue.ToLower().Trim())


                                 || x.BrandName2.ToLower().Contains(searchValue.ToLower().Trim()) 
                                 || x.BrandName1.ToLower().Contains(searchValue.ToLower().Trim()) 
                                 || x.BrandName3.ToLower().Contains(searchValue.ToLower().Trim()) 
                                 || x.BrandName4.ToLower().Contains(searchValue.ToLower().Trim())

                                 || x.PriceStartDate.ToLower().Contains(searchValue.ToLower().Trim())
                                 || x.PriceEndDate.ToLower().Contains(searchValue.ToLower().Trim())

                                 )).ToListAsync();
                         
                    
                    
                }
                else
                {
                    
                    startDate = Convert.ToDateTime(startDate).AddMinutes(-1);
                    endDate = Convert.ToDateTime(endDate).AddDays(1).AddSeconds(-1);
                    tblUniversalPriceMasterlist = await _context.TblUniversalPriceMasters.Where(x => x.CreatedDate >= startDate && x.CreatedDate <= endDate
                                && (string.IsNullOrEmpty(searchValue)
                                || x.PriceMasterName.ToLower().Contains(searchValue.ToLower().Trim())
                                 || x.ProductCategory.Description.ToLower().Contains(searchValue.ToLower().Trim())
                                 || x.ProductTypeName.ToLower().Contains(searchValue.ToLower().Trim())
                                 || x.ProductTypeCode.ToLower().Contains(searchValue.ToLower().Trim())

                                 || x.QuotePHigh.ToLower().Contains(searchValue.ToLower().Trim())
                                 || x.QuoteQHigh.ToLower().Contains(searchValue.ToLower().Trim())
                                 || x.QuoteRHigh.ToLower().Contains(searchValue.ToLower().Trim())
                                 || x.QuoteSHigh.ToLower().Contains(searchValue.ToLower().Trim())

                                 || x.QuoteP.ToLower().Contains(searchValue.ToLower().Trim())
                                 || x.QuoteQ.ToLower().Contains(searchValue.ToLower().Trim())
                                 || x.QuoteR.ToLower().Contains(searchValue.ToLower().Trim())
                                 || x.QuoteS.ToLower().Contains(searchValue.ToLower().Trim())


                                 || x.BrandName2.ToLower().Contains(searchValue.ToLower().Trim())
                                 || x.BrandName1.ToLower().Contains(searchValue.ToLower().Trim())
                                 || x.BrandName3.ToLower().Contains(searchValue.ToLower().Trim())
                                 || x.BrandName4.ToLower().Contains(searchValue.ToLower().Trim())

                                 || x.PriceStartDate.ToLower().Contains(searchValue.ToLower().Trim())
                                 || x.PriceEndDate.ToLower().Contains(searchValue.ToLower().Trim())
                                )).ToListAsync();

                 
                    
                }

                recordsTotal = tblUniversalPriceMasterlist != null ? tblUniversalPriceMasterlist.Count : 0;
                if (tblUniversalPriceMasterlist != null)
                {
                tblUniversalPriceMasterlist = sortColumnDirection.Equals(SortingOrder.ASCENDING) ? tblUniversalPriceMasterlist.OrderBy(o => o.GetType().GetProperty(sortColumn).GetValue(o, null)).ToList() : tblUniversalPriceMasterlist.OrderByDescending(o => o.GetType().GetProperty(sortColumn).GetValue(o, null)).ToList();
                tblUniversalPriceMasterlist = tblUniversalPriceMasterlist.Skip(skip).Take(pageSize).ToList();
                }
                else
                tblUniversalPriceMasterlist = new List<TblUniversalPriceMaster>();

                List<UniversalPriceMasterViewModel> UniversalPriceMasterList = _mapper.Map<List<TblUniversalPriceMaster>, List<UniversalPriceMasterViewModel>>(tblUniversalPriceMasterlist);
                //List<ProductCategoryViewModel> ProductCategoryList = _mapper.Map<List<TblProductCategory>, List<ProductCategoryViewModel>>(tblProductCategory);
                string actionURL = string.Empty;

                foreach (UniversalPriceMasterViewModel item in UniversalPriceMasterList)
                {
                if (item.IsActive == true)
                {


                    actionURL = "<div class='actionbtns'>";
                    actionURL = actionURL + "<a href=' " + URL + "/UniversalPriceMaster/Manage?id=" + _protector.Encode(item.PriceMasterUniversalId) + "' data-bs-toggle='tooltip' data-bs-placement='top' title='Edit' class='btn btn-sm btn-primary'><i class='fa-solid fa-pen'></i></a> " +
                        "<a href='javascript: void(0)' onclick='deleteConfirmUniversal(" + item.PriceMasterUniversalId + ")' class='btn btn-sm btn-danger' data-bs-toggle='tooltip' data-bs-placement='top' title='Delete'><i class='fa-solid fa-trash'></i></a>";
                    actionURL = actionURL + " </div>";
                    item.Action = actionURL;
                }
                else
                {
                    actionURL = "<div class='actionbtns'>";
                    actionURL = actionURL + "<a href=' " + URL + "/UniversalPriceMaster/Manage?id=" + _protector.Encode(item.PriceMasterUniversalId) + "' data-bs-toggle='tooltip' data-bs-placement='top' title='Edit' class='btn btn-sm btn-primary'><i class='fa-solid fa-pen'></i></a> " +
                       "<a href='" + "javascript:void(0)' onclick='activeConfirmUniversal(" + item.PriceMasterUniversalId + ")' class='btn btn-sm btn-success' data-bs-toggle='tooltip' data-bs-placement='top' title='Active'><i class='fa-solid fa-circle-check text-white'></i></a>";
                    actionURL = actionURL + " </div>";
                    item.Action = actionURL;

                }


                TblUniversalPriceMaster tblUniversalPriceMasterobj = _context.TblUniversalPriceMasters.FirstOrDefault(x => x.PriceMasterUniversalId == item.PriceMasterUniversalId);
                    if (tblUniversalPriceMasterobj.CreatedDate != null)
                    {
                        //var Date = (DateTime)item.CreatedDate;
                        //item.Date = Date.ToShortDateString();


                        DateTime dateTime = (DateTime)item.CreatedDate;
                        item.Date = dateTime.ToString("yyyy-MM-dd");
                        //item.CreatedDate = tblUniversalPriceMasterobj.CreatedDate;

                    }
                    //if (UniversalPriceMaster != null && UniversalPriceMaster.ProductTypeId > 0)
                    //{
                    //    TblProductType tblProductType = _context.TblProductTypes.FirstOrDefault(x => x.Id == UniversalPriceMaster.ProductTypeId);
                    //    item.ProductTypeName = tblProductType != null ? tblProductType.Name : string.Empty;
                    //}

                    if (tblUniversalPriceMasterobj != null && tblUniversalPriceMasterobj.ProductCategoryId > 0)
                    {
                        TblProductCategory tblProductCategory = _context.TblProductCategories.FirstOrDefault(x => x.Id == tblUniversalPriceMasterobj.ProductCategoryId);
                        item.ProductCategoryDiscription = tblProductCategory != null ? tblProductCategory.Description : string.Empty;
                    }

                    //if (UniversalPriceMaster != null && UniversalPriceMaster.ProductType != null)
                    //{
                    //    TblProductType tblProductType = _context.TblProductTypes.FirstOrDefault(x => x.Id == UniversalPriceMaster.ProductTypeId);
                    //    item.ProductTypeCode = tblProductType != null ? tblProductType.Code : string.Empty;
                    //}

                }

                var data = UniversalPriceMasterList;
                var jsonData = new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data };
                return Ok(jsonData);
           


        }


        #endregion

        #region
        [HttpPost]
        public async Task<ActionResult> ProductConditionLabelList(DateTime? startDate, DateTime? endDate)
        {
            List<TblProductConditionLabel> TblProductConditionLabellist = null;
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
            //_context = new Digi2L_DevContext();

            if (startDate == null && endDate == null)
            {


                TblProductConditionLabellist = await _context.TblProductConditionLabels.Include(x=>x.BusinessUnit).Include(x => x.BusinessPartner).Where(x => x.IsActive == true

                             && (string.IsNullOrEmpty(searchValue)
                             || (!string.IsNullOrEmpty(x.PclabelName)) && x!.PclabelName!.ToLower().Contains(searchValue.ToLower().Trim())
                             || x.BusinessUnit != null && !string.IsNullOrEmpty(x.BusinessUnit.Name) && x.BusinessUnit.Name.ToLower().Contains(searchValue.ToLower().Trim())
                             || x.BusinessPartner != null && !string.IsNullOrEmpty(x.BusinessPartner.Name) && x.BusinessPartner.Name.ToLower().Contains(searchValue.ToLower().Trim())
                             || (x.OrderSequence != null) && x.OrderSequence!.ToString()!.Contains(searchValue.ToLower().Trim())
                             )).ToListAsync();

            }
            else
            {

                startDate = Convert.ToDateTime(startDate).AddMinutes(-1);
                endDate = Convert.ToDateTime(endDate).AddDays(1).AddSeconds(-1);
                TblProductConditionLabellist = await _context.TblProductConditionLabels.Where(x => x.IsActive == true && x.CreatedDate >= startDate && x.CreatedDate <= endDate
                            && (string.IsNullOrEmpty(searchValue)
                             || (!string.IsNullOrEmpty(x.PclabelName)) && x!.PclabelName!.ToLower().Contains(searchValue.ToLower().Trim())
                             || x.BusinessUnit != null && !string.IsNullOrEmpty(x.BusinessUnit.Name) && x.BusinessUnit.Name.ToLower().Contains(searchValue.ToLower().Trim())
                             || x.BusinessPartner != null && !string.IsNullOrEmpty(x.BusinessPartner.Name) && x.BusinessPartner.Name.ToLower().Contains(searchValue.ToLower().Trim())
                             || (x.OrderSequence != null) && x.OrderSequence!.ToString()!.Contains(searchValue.ToLower().Trim())
                             )).ToListAsync();

            }

            recordsTotal = TblProductConditionLabellist != null ? TblProductConditionLabellist.Count : 0;
            if (TblProductConditionLabellist != null)
            {
                TblProductConditionLabellist = sortColumnDirection.Equals(SortingOrder.ASCENDING) ? TblProductConditionLabellist.OrderBy(o => o.GetType().GetProperty(sortColumn).GetValue(o, null)).ToList() : TblProductConditionLabellist.OrderByDescending(o => o.GetType().GetProperty(sortColumn).GetValue(o, null)).ToList();
                TblProductConditionLabellist = TblProductConditionLabellist.Skip(skip).Take(pageSize).ToList();
            }
            else
                TblProductConditionLabellist = new List<TblProductConditionLabel>();

            List<ProductConditionLabelViewModel> ProductConditionLabelList = _mapper.Map<List<TblProductConditionLabel>, List<ProductConditionLabelViewModel>>(TblProductConditionLabellist);
            //List<ProductCategoryViewModel> ProductCategoryList = _mapper.Map<List<TblProductCategory>, List<ProductCategoryViewModel>>(tblProductCategory);
            string actionURL = string.Empty;

            foreach (ProductConditionLabelViewModel item in ProductConditionLabelList)
            {
                actionURL = "<div class='actionbtns'>";
                actionURL = actionURL + "<a href=' " + URL + "/NewProductConditionLabel/Manage?id=" + _protector.Encode(item.Id) + "' data-bs-toggle='tooltip' data-bs-placement='top' title='Edit' class='btn btn-sm btn-primary'><i class='fa-solid fa-pen'></i></a> " +
                    "<a href='javascript: void(0)' onclick='deleteConfirm(" + item.Id + ")' class='btn btn-sm btn-danger' data-bs-toggle='tooltip' data-bs-placement='top' title='Delete'><i class='fa-solid fa-trash'></i></a>";
                actionURL = actionURL + " </div>";
                item.Action = actionURL;
                TblProductConditionLabel? tblProductConditionobj = _context.TblProductConditionLabels.FirstOrDefault(x => x.Id == item.Id);
                if (tblProductConditionobj != null && tblProductConditionobj.CreatedDate != null)
                {
                    //var Date = (DateTime)item.CreatedDate;
                    //item.Date = Date.ToShortDateString();


                    DateTime? dateTime = Convert.ToDateTime(item?.CreatedDate);
                    item.Date = dateTime?.ToString("yyyy-MM-dd");
                    //item.CreatedDate = tblProductConditionobj.CreatedDate;

                }
                if (tblProductConditionobj != null && tblProductConditionobj.BusinessPartnerId > 0)
                {
                    TblBusinessPartner? TblBusinessPartner = _context.TblBusinessPartners.FirstOrDefault(x => x.BusinessPartnerId == tblProductConditionobj.BusinessPartnerId);
                    item.BusinessPartnerName = TblBusinessPartner != null && !string.IsNullOrEmpty(TblBusinessPartner.Name) ? TblBusinessPartner.Name : string.Empty;
                }

                if (tblProductConditionobj != null && tblProductConditionobj.BusinessUnitId > 0)
                {
                    TblBusinessUnit? TblBusinessUnit = _context.TblBusinessUnits.FirstOrDefault(x => x.BusinessUnitId == tblProductConditionobj.BusinessUnitId);
                    item.BusinessUnitName = TblBusinessUnit != null && !string.IsNullOrEmpty(TblBusinessUnit.Name) ? TblBusinessUnit.Name : string.Empty;
                }
            }

            var data = ProductConditionLabelList;
            var jsonData = new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data };
            return Ok(jsonData);



        }
        #endregion

        #region
        [HttpPost]
        public async Task<ActionResult> GetModelMappingList(DateTime? startDate, DateTime? endDate, string? BuId, string? Brand, string? modelName, string? businesspartner)
        {
            string URL = _config.Value.URLPrefixforProd;
            if (string.IsNullOrEmpty(BuId) || BuId == "null")
            { BuId = "".Trim(); }
            else
            { BuId = BuId.Trim(); }
            if (string.IsNullOrEmpty(Brand) || Brand == "null")
            { Brand = "".Trim().ToLower(); }
            else
            { Brand = Brand.Trim().ToLower(); }
            if (string.IsNullOrEmpty(modelName) || modelName == "null")
            { modelName = "".Trim().ToLower(); }
            else
            { modelName = modelName.Trim().ToLower(); }
            if (string.IsNullOrEmpty(businesspartner) || businesspartner == "null")
            { businesspartner = "".Trim().ToLower(); }
            else
            { businesspartner = businesspartner.Trim().ToLower(); }

            List<TblModelMapping> tblModelMapping = null;
            try
            {
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

                if (startDate == null && endDate == null)
                {

                    count = _context.TblModelMappings
                        .Include(x => x.BusinessUnit)
                        .Include(x => x.Brand)
                        .Include(x => x.BusinessPartner)
                        .Include(x => x.Model)
                        .Where(x=>x.BusinessUnit.Name.Contains(BuId)
                                     && x.Brand.Name.Contains(Brand)
                                     && x.BusinessPartner.Name.Contains(businesspartner)
                                     && x.Model.ModelName.Contains(modelName)).Count();

                    if (count > 0)
                    {
                        tblModelMapping = await _context.TblModelMappings
                        .Include(x => x.BusinessUnit)
                        .Include(x => x.Brand)
                        .Include(x => x.BusinessPartner)
                        .Include(x => x.Model)
                          .Where(x => x.BusinessUnit.Name.Contains(BuId)
                                      && x.Brand.Name.Contains(Brand)
                                      && x.BusinessPartner.Name.Contains(businesspartner)
                                      && x.Model.ModelName.Contains(modelName)).OrderByDescending(x => x.Id).Skip(skip).Take(pageSize).ToListAsync();

                    }

                }
                else
                {
                    startDate = Convert.ToDateTime(startDate).AddMinutes(-1);
                    endDate = Convert.ToDateTime(endDate).AddDays(1).AddSeconds(-1);
                    count = _context.TblModelMappings
                        .Include(x => x.BusinessUnit)
                        .Include(x => x.Brand)
                        .Include(x => x.BusinessPartner)
                        .Include(x => x.Model)
                         .Where(x => x.CreatedDate >= startDate && x.CreatedDate <= endDate

                                     && x.BusinessUnit.Name.Contains(BuId)
                                     && x.Brand.Name.Contains(Brand)
                                     && x.BusinessPartner.Name.Contains(businesspartner)
                                     && x.Model.ModelName.Contains(modelName)
                                     ).Count();


                    if (count > 0)
                    {
                        tblModelMapping = await _context.TblModelMappings.Include(x => x.BusinessUnit)
                       .Include(x => x.BusinessUnit)
                        .Include(x => x.Brand)
                        .Include(x => x.BusinessPartner)
                        .Include(x => x.Model)
                        .Where(x => x.CreatedDate >= startDate && x.CreatedDate <= endDate                     
                                     && x.BusinessUnit.Name.Contains(BuId)
                                     && x.Brand.Name.Contains(Brand)
                                     && x.BusinessPartner.Name.Contains(businesspartner)
                                     && x.Model.ModelName.Contains(modelName)).OrderByDescending(x => x.Id).Skip(skip).Take(pageSize).ToListAsync();

                    }
                }
                recordsTotal = count;

                List<ModelMappingViewModel> ModelList = _mapper.Map<List<TblModelMapping>, List<ModelMappingViewModel>>(tblModelMapping);
                //List<ProductCategoryViewModel> ProductCategoryList = _mapper.Map<List<TblProductCategory>, List<ProductCategoryViewModel>>(tblProductCategory);
                string actionURL = string.Empty;

                foreach (ModelMappingViewModel item in ModelList)
                {
                    if (item.IsActive == true)
                    {
                        actionURL = "<div class='actionbtns'>";
                        actionURL = actionURL + "<a href=' " + URL + "/ModelMapping/Manage?id=" + _protector.Encode(item.Id) + "' data-bs-toggle='tooltip' data-bs-placement='top' title='Edit' class='btn btn-sm btn-primary'><i class='fa-solid fa-pen'></i></a> ";
                            
                        actionURL = actionURL + " </div>";
                        item.Action = actionURL;
                    }
                    else
                    {
                        actionURL = "<div class='actionbtns'>";
                        actionURL = actionURL + "<a href=' " + URL + "/ModelMapping/Manage?id=" + _protector.Encode(item.Id) + "' data-bs-toggle='tooltip' data-bs-placement='top' title='Edit' class='btn btn-sm btn-primary'><i class='fa-solid fa-pen'></i></a> " +
                           "<a href='" + "javascript:void(0)' onclick='activeConfirmModel(" + item.Id + ")' class='btn btn-sm btn-success' data-bs-toggle='tooltip' data-bs-placement='top' title='Active'><i class='fa-solid fa-circle-check text-white'></i></a>";
                        actionURL = actionURL + " </div>";
                        item.Action = actionURL;

                    }


                    TblModelMapping modelMapping = tblModelMapping.FirstOrDefault(x => x.Id == item.Id);
                    if (modelMapping.CreatedDate != null && modelMapping.CreatedDate != null)
                    {
                        //var Date = (DateTime)item.CreatedDate;
                        //item.Date = Date.ToShortDateString();


                        DateTime dateTime = (DateTime)item.CreatedDate;
                        item.Date = dateTime.ToString("yyyy-MM-dd");
                        //item.CreatedDate = modelNumber.CreatedDate;

                    }


                    if (modelMapping != null && modelMapping.BrandId > 0)
                    {
                        TblBrand tblBrand = _context.TblBrands.FirstOrDefault(x => x.Id == modelMapping.BrandId);
                        item.BrandName = tblBrand != null ? tblBrand.Name : string.Empty;
                    }

                    

                    if (modelMapping != null && modelMapping.BusinessUnitId > 0)
                    {
                        TblBusinessUnit tblBusinessUnit = _context.TblBusinessUnits.FirstOrDefault(x => x.BusinessUnitId == modelMapping.BusinessUnitId);
                        item.BusinessUnitName = tblBusinessUnit != null ? tblBusinessUnit.Name : string.Empty;
                    }

                    if (modelMapping != null && modelMapping.BusinessPartnerId > 0)
                    {
                        TblBusinessPartner TblBusinessPartner = _context.TblBusinessPartners.FirstOrDefault(x => x.BusinessPartnerId == modelMapping.BusinessPartnerId);
                        item.BusinessPartnerName = TblBusinessPartner != null ? TblBusinessPartner.Name : string.Empty;
                    }

                    if (modelMapping != null && modelMapping.ModelId > 0)
                    {
                        TblModelNumber ModelNumber = _context.TblModelNumbers.FirstOrDefault(x => x.ModelNumberId == modelMapping.ModelId);
                        item.ModelName = ModelNumber != null ? ModelNumber.ModelName : string.Empty;
                    }
                }

                var data = ModelList;
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








