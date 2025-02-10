using AutoMapper;
using DocumentFormat.OpenXml.ExtendedProperties;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using NPOI.SS.Formula.Functions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
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
using RDCELERP.Model.EVC;
using RDCELERP.Model.ExchangeOrder;
using RDCELERP.Model.ImagLabel;
using RDCELERP.Model.LGC;
using RDCELERP.Model.Master;
using RDCELERP.Model.ModelNumber;
using RDCELERP.Model.PinCode;
using RDCELERP.Model.PriceMaster;
using RDCELERP.Model.Product;
using RDCELERP.Model.ProductQuality;
using RDCELERP.Model.Program;
using RDCELERP.Model.Role;
using RDCELERP.Model.SearchFilters;
using RDCELERP.Model.ServicePartner;
using RDCELERP.Model.State;
using RDCELERP.Model.StoreCode;
using RDCELERP.Model.TimeLine;
using RDCELERP.Model.UniversalPriceMaster;
using RDCELERP.Model.Users;

namespace RDCELERP.Core.App.Controller
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class MasterListController : ControllerBase
    {
        private Digi2l_DevContext _context;
        private IMapper _mapper;
        private CustomDataProtection _protector;
        private IOptions<ApplicationSettings> _config;
        private LoginViewModel _loginSession;
        private ILogging _logging;
        private IBusinessUnitRepository _businessUnitRepository;
        private IBusinessPartnerRepository _businessPartnerRepository;
        private IBrandRepository _brandRepository;
        private IPriceMasterNameRepository _priceMasterNameRepository;


        public MasterListController(IMapper mapper, Digi2l_DevContext context, CustomDataProtection protector, IOptions<ApplicationSettings> config, ILogging logging, IBusinessUnitRepository businessUnitRepository, IBusinessPartnerRepository businessPartnerRepository, IBrandRepository brandRepository, IPriceMasterNameRepository priceMasterNameRepository)
        {
            _context = context;
            _mapper = mapper;
            _protector = protector;
            _config = config;
            _logging = logging;
            _businessUnitRepository = businessUnitRepository;
            _businessPartnerRepository = businessPartnerRepository;
            _brandRepository = brandRepository;
            _priceMasterNameRepository = priceMasterNameRepository;
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
                    tblBrands = await _context.TblBrands.Where(x => x.IsActive == true
                       && (string.IsNullOrEmpty(searchValue) || x.Name.ToLower().Contains(searchValue.ToLower().Trim()))).ToListAsync();
                }
                else
                {
                    startDate = Convert.ToDateTime(startDate).AddMinutes(-1);
                    endDate = Convert.ToDateTime(endDate).AddDays(1).AddSeconds(-1);
                    tblBrands = await _context.TblBrands.Where(x => x.IsActive == true && x.CreatedDate >= startDate && x.CreatedDate <= endDate
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

                    actionURL = " <div class='actionbtns'>";
                    actionURL = actionURL + "<a href=' " + URL + "/Brand/Manage?id=" + _protector.Encode(item.Id) + "' data-bs-toggle='tooltip' data-bs-placement='top' title='Edit' class='btn btn-sm btn-primary'><i class='fa-solid fa-pen'></i></a> " +
                        "<a href='" + URL + " javascript: void(0)' onclick='deleteConfirm(" + item.Id + ")' class='btn btn-sm btn-danger' data-bs-toggle='tooltip' data-bs-placement='top' title='Delete'><i class='fa-solid fa-trash'></i></a>";
                    actionURL = actionURL + " </div>";
                    item.Action = actionURL;

                    ImageURL = "<img src='" + URL + "/DBFiles/Brands/" + item.BrandLogoUrl + "' class='brandlistimg' />";
                    item.BrandLogoUrl = ImageURL;

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
        public async Task<ActionResult> GetUsersList(int roleId, DateTime? startDate, DateTime? endDate)
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
                       .Include(t => t.ModifiedByNavigation).Where(x => x.IsActive == true && x.RoleId != 1 && x.CompanyId == _loginSession.RoleViewModel.CompanyId).OrderByDescending(x => x.UserId).ToList();

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
                        .Include(t => t.User).Where(x => x.IsActive == true && x.CreatedDate >= startDate && x.CreatedDate <= endDate && x.RoleId != 1 && (string.IsNullOrEmpty(searchValue) || x.User.FirstName.ToLower().Contains(searchValue.ToLower().Trim()) || x.User.LastName.ToLower().Contains(searchValue.ToLower().Trim()) || x.Role.RoleName.ToLower().Contains(searchValue.ToLower().Trim()) || x.Company.CompanyName.ToLower().Contains(searchValue.ToLower().Trim()) || x.User.Email.ToLower().Contains(searchValue.ToLower().Trim()) || x.User.Phone.ToLower().Contains(searchValue.ToLower().Trim()))).OrderByDescending(x => x.UserId).ToList();

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
                       .Include(t => t.ModifiedByNavigation).Where(x => x.IsActive == true && x.CreatedDate >= startDate && x.CreatedDate <= endDate && x.RoleId != 1 && x.CompanyId == _loginSession.RoleViewModel.CompanyId).OrderByDescending(x => x.UserId).ToList();

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
                    actionURL = actionURL + "<a href=' " + URL + "/Users/Manage?id=" + _protector.Encode(item.UserId) + "' data-bs-toggle='tooltip' data-bs-placement='top' title='Edit' class='btn btn-sm btn-primary'><i class='fa-solid fa-pen'></i></a> " + "<a href=' " + URL + "/Users/Details?id=" + _protector.Encode(item.UserId) + "' data-bs-toggle='tooltip' data-bs-placement='top' title='View' class='btn btn-sm btn-primary'><i class='fa-solid fa-eye'></i></a> " +
                        "<a href=' " + URL + "/Users/ManageUserRole?id=" + (item.UserRoleId) + "&" + (item.UserId) + "' data-bs-toggle='tooltip' data-bs-placement='top' title=' Role Edit' class='btn btn-sm btn-primary'><i class='fa-solid fa-user'></i></a> " +
                        "<a href='javascript: void(0)' onclick='deleteConfirm(" + item.UserRoleId + ")' class='btn btn-sm btn-danger' data-bs-toggle='tooltip' data-bs-placement='top' title='Delete'><i class='fa-solid fa-trash'></i></a>";
                    actionURL = actionURL + " </div>";
                    ImageURL = "<img src='" + URL + "/DBFiles/Users/" + item.User.ImageName + "' class='brandlistimg' />";
                    if (item != null)
                    {
                        UserListViewModel = new UserRolesView();
                        UserListViewModel.Action = actionURL;
                        UserListViewModel.ImageName = ImageURL;
                        UserListViewModel.Name = item.User.FirstName + " " + item.User.LastName;
                        UserListViewModel.Email = SecurityHelper.DecryptString(item.User.Email, _config.Value.SecurityKey);
                        UserListViewModel.Phone = SecurityHelper.DecryptString(item.User.Phone, _config.Value.SecurityKey);

                        UserListViewModel.Role = item.Role.RoleName;
                        UserListViewModel.CompanyName = item.Company.CompanyName;
                        UserListViewModel.CreatedDate = item.CreatedDate;
                        //if(item.IsActive==true)
                        //{
                        //    UserListViewModel.UserStatus = 'Active';
                        //}
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
                    tblProductCategory = await _context.TblProductCategories.Where(x => x.IsActive == true
                       && (string.IsNullOrEmpty(searchValue) || x.Name.ToLower().Contains(searchValue.ToLower().Trim()) || x.Description.ToLower().Contains(searchValue.ToLower().Trim()) || x.Code.ToLower().Contains(searchValue.ToLower().Trim()))).ToListAsync();
                }
                else
                {
                    startDate = Convert.ToDateTime(startDate).AddMinutes(-1);
                    endDate = Convert.ToDateTime(endDate).AddDays(1).AddSeconds(-1);
                    tblProductCategory = await _context.TblProductCategories.Where(x => x.IsActive == true && x.CreatedDate >= startDate && x.CreatedDate <= endDate
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
                    actionURL = "<div class='actionbtns'>";
                    actionURL = actionURL + "<a href=' " + URL + "/ProductCategory/Manage?id=" + _protector.Encode(item.Id) + "' data-bs-toggle='tooltip' data-bs-placement='top' title='Edit' class='btn btn-sm btn-primary'><i class='fa-solid fa-pen'></i></a> " +
                        "<a href='javascript: void(0)' onclick='deleteConfirm(" + item.Id + ")' class='btn btn-sm btn-danger' data-bs-toggle='tooltip' data-bs-placement='top' title='Delete'><i class='fa-solid fa-trash'></i></a>";
                    actionURL = actionURL + " </div>";
                    item.Action = actionURL;

                    TblProductCategory ProductCategoryDetails = tblProductCategory.FirstOrDefault(x => x.Id == item.Id);

                    if (ProductCategoryDetails.CreatedDate != null && ProductCategoryDetails.CreatedDate != null)
                    {
                        //var Date = (DateTime)item.CreatedDate;
                        //item.Date = Date.ToShortDateString();


                        DateTime dateTime = (DateTime)item.CreatedDate;
                        item.Date = dateTime.ToString("yyyy-MM-dd");
                        item.CreatedDate = ProductCategoryDetails.CreatedDate;

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
                    tblproductTypes = await _context.TblProductTypes.Include(x => x.ProductCat).Where(x => x.IsActive == true
                          && (string.IsNullOrEmpty(searchValue) || x.Name.ToLower().Contains(searchValue.ToLower().Trim()) || x.Description.ToLower().Contains(searchValue.ToLower().Trim()) || x.Code.ToLower().Contains(searchValue.ToLower().Trim()) || x.Size.ToLower().Contains(searchValue.ToLower().Trim()) || x.ProductCat.Description.ToLower().Contains(searchValue.ToLower().Trim()))).ToListAsync();
                }
                else
                {
                    startDate = Convert.ToDateTime(startDate).AddMinutes(-1);
                    endDate = Convert.ToDateTime(endDate).AddDays(1).AddSeconds(-1);
                    tblproductTypes = await _context.TblProductTypes.Include(x => x.ProductCat).Where(x => x.IsActive == true && x.CreatedDate >= startDate && x.CreatedDate <= endDate
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

                    actionURL = "<div class='actionbtns'>";
                    actionURL = actionURL + "<a href=' " + URL + "/ProductType/Manage?id=" + _protector.Encode(item.Id) + "' data-bs-toggle='tooltip' data-bs-placement='top' title='Edit' class='btn btn-sm btn-primary'><i class='fa-solid fa-pen'></i></a> " +
                        "<a href='javascript: void(0)' onclick='deleteConfirm(" + item.Id + ")' class='btn btn-sm btn-danger' data-bs-toggle='tooltip' data-bs-placement='top' title='Delete'><i class='fa-solid fa-trash'></i></a>";
                    actionURL = actionURL + " </div>";
                    item.Action = actionURL;
                    TblProductType productType = tblproductTypes.FirstOrDefault(x => x.Id == item.Id);
                    if (productType.CreatedDate != null && productType.CreatedDate != null)
                    {
                        //var Date = (DateTime)item.CreatedDate;
                        //item.Date = Date.ToShortDateString();


                        DateTime dateTime = (DateTime)item.CreatedDate;
                        item.Date = dateTime.ToString("yyyy-MM-dd");
                        item.CreatedDate = productType.CreatedDate;

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
                        item.CreatedDate = Role.CreatedDate;

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

                    tblProductQualityIndex = await _context.TblProductQualityIndices.Include(x => x.ProductCategory).Where(x => x.IsActive == true
                          && (string.IsNullOrEmpty(searchValue) || x.Name.ToLower().Contains(searchValue.ToLower().Trim()) || x.ProductCategory.Description.ToLower().Contains(searchValue.ToLower().Trim()))).ToListAsync();
                }
                else
                {
                    startDate = Convert.ToDateTime(startDate).AddMinutes(-1);
                    endDate = Convert.ToDateTime(endDate).AddDays(1).AddSeconds(-1);
                    tblProductQualityIndex = await _context.TblProductQualityIndices.Include(x => x.ProductCategory).Where(x => x.IsActive == true && x.CreatedDate >= startDate && x.CreatedDate <= endDate
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
                    actionURL = "<div class='actionbtns'>";
                    actionURL = actionURL + "<a href=' " + URL + "/ProductQualityIndex/Manage?id=" + _protector.Encode(item.ProductQualityIndexId) + "' data-bs-toggle='tooltip' data-bs-placement='top' title='Edit' class='btn btn-sm btn-primary'><i class='fa-solid fa-pen'></i></a> " +
                        "<a href='javascript: void(0)' onclick='deleteConfirm(" + item.ProductQualityIndexId + ")' class='btn btn-sm btn-danger' data-bs-toggle='tooltip' data-bs-placement='top' title='Delete'><i class='fa-solid fa-trash'></i></a>";
                    actionURL = actionURL + " </div>";
                    item.Action = actionURL;


                    TblProductQualityIndex ProductQualityIndex = tblProductQualityIndex.FirstOrDefault(x => x.ProductQualityIndexId == item.ProductQualityIndexId);
                    if (ProductQualityIndex.CreatedDate != null && ProductQualityIndex.CreatedDate != null)
                    {
                        //var Date = (DateTime)item.CreatedDate;
                        //item.Date = Date.ToShortDateString();


                        DateTime dateTime = (DateTime)item.CreatedDate;
                        item.Date = dateTime.ToString("yyyy-MM-dd");
                        item.CreatedDate = ProductQualityIndex.CreatedDate;

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

                    TblPinCodes = await _context.TblPinCodes.Where(x => x.IsActive == true
                        && (string.IsNullOrEmpty(searchValue) || x.Location.ToLower().Contains(searchValue.ToLower().Trim()) || x.ZipCode.ToString().Contains(searchValue.ToLower().Trim()) || x.State.ToLower().Contains(searchValue.ToLower().Trim()))).ToListAsync();
                }
                else
                {
                    startDate = Convert.ToDateTime(startDate).AddMinutes(-1);
                    endDate = Convert.ToDateTime(endDate).AddDays(1).AddSeconds(-1);
                    TblPinCodes = await _context.TblPinCodes.Where(x => x.IsActive == true && x.CreatedDate >= startDate && x.CreatedDate <= endDate
                        && (string.IsNullOrEmpty(searchValue) || x.Location.ToLower().Contains(searchValue.ToLower().Trim()) || x.ZipCode.ToString().Contains(searchValue.ToLower().Trim()))).ToListAsync();

                }


                recordsTotal = TblPinCodes != null ? TblPinCodes.Count : 0;
                if (TblPinCodes != null)
                {
                    TblPinCodes = sortColumnDirection.Equals(SortingOrder.ASCENDING) ? TblPinCodes.OrderBy(o => o.GetType().GetProperty(sortColumn).GetValue(o, null)).ToList() : TblPinCodes.OrderByDescending(o => o.GetType().GetProperty(sortColumn).GetValue(o, null)).ToList();
                    TblPinCodes = TblPinCodes.Skip(skip).Take(pageSize).ToList();
                }
                else
                    TblPinCodes = new List<TblPinCode>();

                List<PinCodeViewModel> pinCodeList = _mapper.Map<List<TblPinCode>, List<PinCodeViewModel>>(TblPinCodes);
                string actionURL = string.Empty;

                foreach (PinCodeViewModel item in pinCodeList)
                {
                    actionURL = "<div class='actionbtns'>";
                    actionURL = actionURL + "<a href=' " + URL + "/PinCode/Manage?id=" + _protector.Encode(item.Id) + "' data-bs-toggle='tooltip' data-bs-placement='top' title='Edit' class='btn btn-sm btn-primary'><i class='fa-solid fa-pen'></i></a> " +
                        "<a href='javascript: void(0)' onclick='deleteConfirm(" + item.Id + ")' class='btn btn-sm btn-danger' data-bs-toggle='tooltip' data-bs-placement='top' title='Delete'><i class='fa-solid fa-trash'></i></a>";
                    actionURL = actionURL + " </div>";
                    item.Action = actionURL;

                    TblPinCode PinCodeDetails = TblPinCodes.FirstOrDefault(x => x.Id == item.Id);
                    if (PinCodeDetails.CreatedDate != null && PinCodeDetails.CreatedDate != null)
                    {
                        //var Date = (DateTime)item.CreatedDate;
                        //item.Date = Date.ToShortDateString();


                        DateTime dateTime = (DateTime)item.CreatedDate;
                        item.Date = dateTime.ToString("yyyy-MM-dd");
                        item.CreatedDate = PinCodeDetails.CreatedDate;

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

                    TblStates = await _context.TblStates.Where(x => x.IsActive == true
                       && (string.IsNullOrEmpty(searchValue) || x.Name.ToLower().Contains(searchValue.ToLower().Trim()))).ToListAsync();
                }
                else
                {
                    startDate = Convert.ToDateTime(startDate).AddMinutes(-1);
                    endDate = Convert.ToDateTime(endDate).AddDays(1).AddSeconds(-1);
                    TblStates = await _context.TblStates.Where(x => x.IsActive == true && x.CreatedDate >= startDate && x.CreatedDate <= endDate
                         && (string.IsNullOrEmpty(searchValue) || x.Name.ToLower().Contains(searchValue.ToLower().Trim()))).ToListAsync();

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
                    actionURL = "<div class='actionbtns'>";
                    actionURL = actionURL + "<a href=' " + URL + "/State/Manage?id=" + _protector.Encode(item.StateId) + "' data-bs-toggle='tooltip' data-bs-placement='top' title='Edit' class='btn btn-sm btn-primary'><i class='fa-solid fa-pen'></i></a> " +
                        "<a href='javascript: void(0)' onclick='deleteConfirm(" + item.StateId + ")' class='btn btn-sm btn-danger' data-bs-toggle='tooltip' data-bs-placement='top' title='Delete'><i class='fa-solid fa-trash'></i></a>";
                    actionURL = actionURL + " </div>";
                    item.Action = actionURL;

                    TblState StateDetails = TblStates.FirstOrDefault(x => x.StateId == item.StateId);
                    if (StateDetails.CreatedDate != null && StateDetails.CreatedDate != null)
                    {
                        //var Date = (DateTime)item.CreatedDate;
                        //item.Date = Date.ToShortDateString();


                        DateTime dateTime = (DateTime)item.CreatedDate;
                        item.Date = dateTime.ToString("yyyy-MM-dd");
                        item.CreatedDate = StateDetails.CreatedDate;

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

                    TblCities = await _context.TblCities.Include(x => x.State).Where(x => x.IsActive == true
                          && (string.IsNullOrEmpty(searchValue) || x.Name.ToLower().Contains(searchValue.ToLower().Trim()) || x.CityCode.ToLower().Contains(searchValue.ToLower().Trim()) || x.State.Name.ToLower().Contains(searchValue.ToLower().Trim()))).ToListAsync();
                }
                else
                {
                    startDate = Convert.ToDateTime(startDate).AddMinutes(-1);
                    endDate = Convert.ToDateTime(endDate).AddDays(1).AddSeconds(-1);
                    TblCities = await _context.TblCities.Include(x => x.State).Where(x => x.IsActive == true && x.CreatedDate >= startDate && x.CreatedDate <= endDate
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
                    actionURL = "<div class='actionbtns'>";
                    actionURL = actionURL + "<a href=' " + URL + "/City/Manage?id=" + _protector.Encode(item.CityId) + "' data-bs-toggle='tooltip' data-bs-placement='top' title='Edit' class='btn btn-sm btn-primary'><i class='fa-solid fa-pen'></i></a> " +
                        "<a href='javascript: void(0)' onclick='deleteConfirm(" + item.CityId + ")' class='btn btn-sm btn-danger' data-bs-toggle='tooltip' data-bs-placement='top' title='Delete'><i class='fa-solid fa-trash'></i></a>";
                    actionURL = actionURL + " </div>";
                    item.Action = actionURL;

                    TblCity city = TblCities.FirstOrDefault(x => x.CityId == item.CityId);
                    if (city.CreatedDate != null && city.CreatedDate != null)
                    {
                        //var Date = (DateTime)item.CreatedDate;
                        //item.Date = Date.ToShortDateString();


                        DateTime dateTime = (DateTime)item.CreatedDate;
                        item.Date = dateTime.ToString("yyyy-MM-dd");
                        item.CreatedDate = city.CreatedDate;

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
                        "<a href='javascript: void(0)' onclick='deleteConfirm(" + item.BusinessPartnerId + ")' class='btn btn-sm btn-danger' data-bs-toggle='tooltip' data-bs-placement='top' title='Delete'><i class='fa-solid fa-trash'></i></a>";
                    actionURL = actionURL + " </div>";
                    item.Action = actionURL;

                    TblBusinessPartner BusinessPartner = TblBusinessPartners.FirstOrDefault(x => x.BusinessPartnerId == item.BusinessPartnerId);
                    if (BusinessPartner.CreatedDate != null && BusinessPartner.CreatedDate != null)
                    {
                        //var Date = (DateTime)item.CreatedDate;
                        //item.Date = Date.ToShortDateString();


                        DateTime dateTime = (DateTime)item.CreatedDate;
                        item.Date = dateTime.ToString("yyyy-MM-dd");
                        item.CreatedDate = BusinessPartner.CreatedDate;

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

                List<ImageLabelViewModel> ImageLabelList = _mapper.Map<List<TblImageLabelMaster>, List<ImageLabelViewModel>>(tblImageLabelMaster);
                //List<ProductCategoryViewModel> ProductCategoryList = _mapper.Map<List<TblProductCategory>, List<ProductCategoryViewModel>>(tblProductCategory);
                string actionURL = string.Empty;

                foreach (ImageLabelViewModel item in ImageLabelList)
                {
                    actionURL = "<div class='actionbtns'>";
                    actionURL = actionURL + "<a href=' " + URL + "/ImageLabelMaster/Manage?id=" + _protector.Encode(item.ImageLabelid) + "' data-bs-toggle='tooltip' data-bs-placement='top' title='Edit' class='btn btn-sm btn-primary'><i class='fa-solid fa-pen'></i></a> " +
                        "<a href='javascript: void(0)' onclick='deleteConfirm(" + item.ImageLabelid + ")' class='btn btn-sm btn-danger' data-bs-toggle='tooltip' data-bs-placement='top' title='Delete'><i class='fa-solid fa-trash'></i></a>";
                    actionURL = actionURL + " </div>";
                    item.Action = actionURL;

                    TblImageLabelMaster ImageLabel = tblImageLabelMaster.FirstOrDefault(x => x.ImageLabelid == item.ImageLabelid);

                    if (ImageLabel.CreatedDate != null && ImageLabel.CreatedDate != null)
                    {
                        //var Date = (DateTime)item.CreatedDate;
                        //item.Date = Date.ToShortDateString();


                        DateTime dateTime = (DateTime)item.CreatedDate;
                        item.Date = dateTime.ToString("yyyy-MM-dd");
                        item.CreatedDate = ImageLabel.CreatedDate;

                    }

                    if (ImageLabel != null && ImageLabel.ProductCatId > 0)
                    {
                        TblProductCategory tblProductCategory = _context.TblProductCategories.FirstOrDefault(x => x.Id == ImageLabel.ProductCatId);
                        item.ProductCategoryName = tblProductCategory != null ? tblProductCategory.Description : string.Empty;
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
                          && (string.IsNullOrEmpty(searchValue) || x.ServicePartnerName.ToLower().Contains(searchValue.ToLower().Trim()) || x.ServicePartnerDescription.ToLower().Contains(searchValue.ToLower().Trim()) || x.User.FirstName.ToLower().Contains(searchValue.ToLower().Trim()))).ToListAsync();
                }
                else
                {
                    startDate = Convert.ToDateTime(startDate).AddMinutes(-1);
                    endDate = Convert.ToDateTime(endDate).AddDays(1).AddSeconds(-1);
                    tblServicePartner = await _context.TblServicePartners.Include(x => x.User).Where(x => x.IsActive == true && x.CreatedDate >= startDate && x.CreatedDate <= endDate
                          && (string.IsNullOrEmpty(searchValue) || x.ServicePartnerName.ToLower().Contains(searchValue.ToLower().Trim()) || x.ServicePartnerDescription.ToLower().Contains(searchValue.ToLower().Trim()) || x.User.FirstName.ToLower().Contains(searchValue.ToLower().Trim()))).ToListAsync();

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
                        "<a href='javascript: void(0)' onclick='deleteConfirm(" + item.ServicePartnerId + ")' class='btn btn-sm btn-danger' data-bs-toggle='tooltip' data-bs-placement='top' title='Delete'><i class='fa-solid fa-trash'></i></a>";
                    actionURL = actionURL + " </div>";
                    item.Action = actionURL;

                    TblServicePartner ServicePartner = tblServicePartner.FirstOrDefault(x => x.ServicePartnerId == item.ServicePartnerId);

                    if (ServicePartner.CreatedDate != null && ServicePartner.CreatedDate != null)
                    {
                        //var Date = (DateTime)item.CreatedDate;
                        //item.Date = Date.ToShortDateString();


                        DateTime dateTime = (DateTime)item.CreatedDate;
                        item.Date = dateTime.ToString("yyyy-MM-dd");
                        item.CreatedDate = ServicePartner.CreatedDate;

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
                    actionURL = actionURL + "<a href=' " + URL + "/BusinessUnit/Edit?id=" + _protector.Encode(item.BusinessUnitId) + "' data-bs-toggle='tooltip' data-bs-placement='top' title='Edit' class='btn btn-sm btn-primary'><i class='fa-solid fa-pen'></i></a> " +
                        "<a href='javascript: void(0)' onclick='deleteConfirm(" + item.BusinessUnitId + ")' class='btn btn-sm btn-danger' data-bs-toggle='tooltip' data-bs-placement='top' title='Delete'><i class='fa-solid fa-trash'></i></a>";
                    actionURL = actionURL + " </div>";
                    item.Action = actionURL;

                    TblBusinessUnit BusinessUnit = tblBusinessUnit.FirstOrDefault(x => x.BusinessUnitId == item.BusinessUnitId);

                    if (BusinessUnit.CreatedDate != null && BusinessUnit.CreatedDate != null)
                    {
                        //var Date = (DateTime)item.CreatedDate;
                        //item.Date = Date.ToShortDateString();


                        DateTime dateTime = (DateTime)item.CreatedDate;
                        item.Date = dateTime.ToString("yyyy-MM-dd");
                        item.CreatedDate = BusinessUnit.CreatedDate;

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
        public async Task<ActionResult> GetModelNumberList(DateTime? startDate, DateTime? endDate)
        {
            string URL = _config.Value.URLPrefixforProd;
            List<TblModelNumber> tblModelNumber = null;
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
                    tblModelNumber = await _context.TblModelNumbers.Where(x => x.IsActive == true
                        && (string.IsNullOrEmpty(searchValue) || x.ModelName.ToLower().Contains(searchValue.ToLower().Trim()) || x.Description.ToLower().Contains(searchValue.ToLower().Trim()) || x.Code.ToLower().Contains(searchValue.ToLower().Trim()) || x.Brand.ToString().Contains(searchValue.ToLower().Trim()))).ToListAsync();
                }
                else
                {
                    startDate = Convert.ToDateTime(startDate).AddMinutes(-1);
                    endDate = Convert.ToDateTime(endDate).AddDays(1).AddSeconds(-1);
                    tblModelNumber = await _context.TblModelNumbers.Where(x => x.IsActive == true && x.CreatedDate >= startDate && x.CreatedDate <= endDate
                       && (string.IsNullOrEmpty(searchValue) || x.ModelName.ToLower().Contains(searchValue.ToLower().Trim()) || x.Description.ToLower().Contains(searchValue.ToLower().Trim()) || x.Code.ToLower().Contains(searchValue.ToLower().Trim()) || x.Brand.ToString().Contains(searchValue.ToLower().Trim()))).ToListAsync();
                }





                recordsTotal = tblModelNumber != null ? tblModelNumber.Count : 0;
                if (tblModelNumber != null)
                {
                    tblModelNumber = sortColumnDirection.Equals(SortingOrder.ASCENDING) ? tblModelNumber.OrderBy(o => o.GetType().GetProperty(sortColumn).GetValue(o, null)).ToList() : tblModelNumber.OrderByDescending(o => o.GetType().GetProperty(sortColumn).GetValue(o, null)).ToList();
                    tblModelNumber = tblModelNumber.Skip(skip).Take(pageSize).ToList();
                }
                else
                    tblModelNumber = new List<TblModelNumber>();

                List<ModelNumberViewModel> ModelNumberList = _mapper.Map<List<TblModelNumber>, List<ModelNumberViewModel>>(tblModelNumber);
                //List<ProductCategoryViewModel> ProductCategoryList = _mapper.Map<List<TblProductCategory>, List<ProductCategoryViewModel>>(tblProductCategory);
                string actionURL = string.Empty;

                foreach (ModelNumberViewModel item in ModelNumberList)
                {
                    actionURL = "<div class='actionbtns'>";
                    actionURL = actionURL + "<a href=' " + URL + "/ModelNumber/Manage?id=" + _protector.Encode(item.ModelNumberId) + "' data-bs-toggle='tooltip' data-bs-placement='top' title='Edit' class='btn btn-sm btn-primary'><i class='fa-solid fa-pen'></i></a> " +
                        "<a href='javascript: void(0)' onclick='deleteConfirm(" + item.BusinessUnitId + ")' class='btn btn-sm btn-danger' data-bs-toggle='tooltip' data-bs-placement='top' title='Delete'><i class='fa-solid fa-trash'></i></a>";
                    actionURL = actionURL + " </div>";
                    item.Action = actionURL;

                    TblModelNumber modelNumber = tblModelNumber.FirstOrDefault(x => x.ModelNumberId == item.ModelNumberId);
                    if (modelNumber.CreatedDate != null && modelNumber.CreatedDate != null)
                    {
                        //var Date = (DateTime)item.CreatedDate;
                        //item.Date = Date.ToShortDateString();


                        DateTime dateTime = (DateTime)item.CreatedDate;
                        item.Date = dateTime.ToString("yyyy-MM-dd");
                        item.CreatedDate = modelNumber.CreatedDate;

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
                                && (string.IsNullOrEmpty(searchValue) || x.ProductCat.ToLower().Contains(searchValue.ToLower().Trim()) || x.ProductType.ToLower().Contains(searchValue.ToLower().Trim()) || x.BrandName2.ToLower().Contains(searchValue.ToLower().Trim()) || x.BrandName1.ToLower().Contains(searchValue.ToLower().Trim()) || x.BrandName3.ToLower().Contains(searchValue.ToLower().Trim()) || x.BrandName4.ToLower().Contains(searchValue.ToLower().Trim()) || x.ProductTypeCode.ToLower().Contains(searchValue.ToLower().Trim()))).ToListAsync();

                    }
                    else
                    {
                        var login = _context.Logins.Where(x => x.SponsorId == buid);
                        foreach (var item in login)
                        {

                            tblPriceMaster = await _context.TblPriceMasters

                                .Where(x => x.IsActive == true && x.ExchPriceCode == item.PriceCode

                                     && (string.IsNullOrEmpty(searchValue) || x.ProductCat.ToLower().Contains(searchValue.ToLower().Trim()) || x.ProductType.ToLower().Contains(searchValue.ToLower().Trim()) || x.BrandName2.ToLower().Contains(searchValue.ToLower().Trim()) || x.BrandName1.ToLower().Contains(searchValue.ToLower().Trim()) || x.BrandName3.ToLower().Contains(searchValue.ToLower().Trim()) || x.BrandName4.ToLower().Contains(searchValue.ToLower().Trim()) || x.ProductTypeCode.ToLower().Contains(searchValue.ToLower().Trim()) || x.BrandName3.ToLower().Contains(searchValue.ToLower().Trim()) || x.BrandName4.ToLower().Contains(searchValue.ToLower().Trim()) || x.ProductTypeCode.ToLower().Contains(searchValue.ToLower().Trim()))).ToListAsync();
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
                    actionURL = actionURL + "<a target='_blanck' href=' " + URL + "/PriceMaster/Manage?id=" + _protector.Encode(item.Id) + "' data-bs-toggle='tooltip' data-bs-placement='top' title='Edit' class='btn btn-sm btn-primary'><i class='fa-solid fa-pen'></i></a> " +
                        "<a href='javascript: void(0)' onclick='deleteConfirm(" + item.Id + ")' class='btn btn-sm btn-danger' data-bs-toggle='tooltip' data-bs-placement='top' title='Delete'><i class='fa-solid fa-trash'></i></a>";
                    actionURL = actionURL + " </div>";
                    item.Action = actionURL;
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
                //_context = new Digi2L_DevContext();

                if (startDate == null && endDate == null)
                {
                    tblAbbplanMaster = await _context.TblAbbplanMasters.Include(x => x.ProductType).Include(x => x.ProductCat).Where(x => x.IsActive == true
                                && (string.IsNullOrEmpty(searchValue) || x.AbbplanName.ToLower().Contains(searchValue.ToLower().Trim()) || x.Sponsor.ToLower().Contains(searchValue.ToLower().Trim()) || x.ToMonth.ToString().Contains(searchValue.ToLower().Trim()) || x.FromMonth.ToString().Contains(searchValue.ToLower().Trim()) || x.BusinessUnit.Name.ToLower().Contains(searchValue.ToLower().Trim()) || x.ProductCat.Description.ToLower().Contains(searchValue.ToLower().Trim()) || x.ProductType.Description.ToLower().Contains(searchValue.ToLower().Trim()))).ToListAsync();
                }
                else
                {
                    startDate = Convert.ToDateTime(startDate).AddMinutes(-1);
                    endDate = Convert.ToDateTime(endDate).AddDays(1).AddSeconds(-1);
                    tblAbbplanMaster = await _context.TblAbbplanMasters.Include(x => x.BusinessUnit).Include(x => x.ProductType).Include(x => x.ProductCat).Where(x => x.IsActive == true && x.CreatedDate >= startDate && x.CreatedDate <= endDate
                              && (string.IsNullOrEmpty(searchValue) || x.AbbplanName.ToLower().Contains(searchValue.ToLower().Trim()) || x.Sponsor.ToLower().Contains(searchValue.ToLower().Trim()) || x.BusinessUnit.Name.ToLower().Contains(searchValue.ToLower().Trim()) || x.ProductCat.Description.ToLower().Contains(searchValue.ToLower().Trim()) || x.ProductType.Description.ToLower().Contains(searchValue.ToLower().Trim()))).ToListAsync();
                }






                recordsTotal = tblAbbplanMaster != null ? tblAbbplanMaster.Count : 0;
                if (tblAbbplanMaster != null)
                {
                    tblAbbplanMaster = sortColumnDirection.Equals(SortingOrder.ASCENDING) ? tblAbbplanMaster.OrderBy(o => o.GetType().GetProperty(sortColumn).GetValue(o, null)).ToList() : tblAbbplanMaster.OrderByDescending(o => o.GetType().GetProperty(sortColumn).GetValue(o, null)).ToList();
                    tblAbbplanMaster = tblAbbplanMaster.Skip(skip).Take(pageSize).ToList();
                }
                else
                    tblAbbplanMaster = new List<TblAbbplanMaster>();

                List<ABBPlanMasterViewModel> ABBPlanMasterList = _mapper.Map<List<TblAbbplanMaster>, List<ABBPlanMasterViewModel>>(tblAbbplanMaster);
                //List<ProductCategoryViewModel> ProductCategoryList = _mapper.Map<List<TblProductCategory>, List<ProductCategoryViewModel>>(tblProductCategory);
                string actionURL = string.Empty;

                foreach (ABBPlanMasterViewModel item in ABBPlanMasterList)
                {
                    actionURL = "<div class='actionbtns'>";
                    actionURL = actionURL + "<a target='_blanck' href=' " + URL + "/ABBPlanMaster/Manage?id=" + _protector.Encode(item.PlanMasterId) + "' data-bs-toggle='tooltip' data-bs-placement='top' title='Edit' class='btn btn-sm btn-primary'><i class='fa-solid fa-pen'></i></a> " +
                        "<a href='javascript: void(0)' onclick='deleteConfirm(" + item.PlanMasterId + ")' class='btn btn-sm btn-danger' data-bs-toggle='tooltip' data-bs-placement='top' title='Delete'><i class='fa-solid fa-trash'></i></a>";
                    actionURL = actionURL + " </div>";
                    item.Action = actionURL;

                    TblAbbplanMaster AbbplanMaster = tblAbbplanMaster.FirstOrDefault(x => x.PlanMasterId == item.PlanMasterId);
                    if (AbbplanMaster.CreatedDate != null && AbbplanMaster.CreatedDate != null)
                    {
                        //var Date = (DateTime)item.CreatedDate;
                        //item.Date = Date.ToShortDateString();


                        DateTime dateTime = (DateTime)item.CreatedDate;
                        item.Date = dateTime.ToString("yyyy-MM-dd");
                        item.CreatedDate = AbbplanMaster.CreatedDate;

                    }

                    if (AbbplanMaster != null && AbbplanMaster.BusinessUnitId > 0)
                    {
                        TblBusinessUnit tblBusinessUnit = _context.TblBusinessUnits.FirstOrDefault(x => x.BusinessUnitId == AbbplanMaster.BusinessUnitId);
                        item.BusinessUnitName = tblBusinessUnit != null ? tblBusinessUnit.Name : string.Empty;
                    }
                    if (AbbplanMaster != null && AbbplanMaster.ProductTypeId > 0)
                    {
                        TblProductType tblProductType = _context.TblProductTypes.FirstOrDefault(x => x.Id == AbbplanMaster.ProductTypeId);
                        item.ProductTypeName = tblProductType != null ? tblProductType.Description : string.Empty;
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
                    actionURL = actionURL + "<a target='_blanck' href=' " + URL + "/ABBPriceMaster/Manage?id=" + _protector.Encode(item.PriceMasterId) + "' data-bs-toggle='tooltip' data-bs-placement='top' title='Edit' class='btn btn-sm btn-primary'><i class='fa-solid fa-pen'></i></a> " +
                        "<a href='javascript: void(0)' onclick='deleteConfirm(" + item.PriceMasterId + ")' class='btn btn-sm btn-danger' data-bs-toggle='tooltip' data-bs-placement='top' title='Delete'><i class='fa-solid fa-trash'></i></a>";
                    actionURL = actionURL + " </div>";
                    item.Action = actionURL;

                    TblAbbpriceMaster AbbpriceMaster = tblAbbpriceMaster.FirstOrDefault(x => x.PriceMasterId == item.PriceMasterId);
                    if (AbbpriceMaster.CreatedDate != null && AbbpriceMaster.CreatedDate != null)
                    {
                        //var Date = (DateTime)item.CreatedDate;
                        //item.Date = Date.ToShortDateString();


                        DateTime dateTime = (DateTime)item.CreatedDate;
                        item.Date = dateTime.ToString("yyyy-MM-dd");
                        item.CreatedDate = AbbpriceMaster.CreatedDate;

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

        [HttpPost]
        public async Task<ActionResult> GetABBPlanMasterListByBUId(int BUId, DateTime? startDate, DateTime? endDate)
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
                if (string.IsNullOrEmpty(searchValue))
                { searchValue = null; }
                else
                { searchValue = searchValue.Trim().ToLower(); }
                int pageSize = length != null ? Convert.ToInt32(length) : 0;
                int skip = start != null ? Convert.ToInt32(start) : 0;
                int recordsTotal = 0;
                //_context = new Digi2L_DevContext();

                if (startDate == null && endDate == null)
                {
                    tblAbbplanMaster = await _context.TblAbbplanMasters.Include(x => x.ProductType).Include(x => x.ProductCat).Where(x => x.IsActive == true && x.BusinessUnitId == BUId
                                && (string.IsNullOrEmpty(searchValue)
                                || x.AbbplanName.ToLower().Contains(searchValue)
                                || x.Sponsor.ToLower().Contains(searchValue)
                                || x.ToMonth.ToString().Contains(searchValue)
                                || x.FromMonth.ToString().Contains(searchValue)
                                || x.BusinessUnit.Name.ToLower().Contains(searchValue)
                                || x.ProductCat.Description.ToLower().Contains(searchValue)
                                || x.ProductType.Description.ToLower().Contains(searchValue)
                                )).OrderByDescending(x => x.PlanMasterId).ToListAsync();
                }
                else
                {
                    startDate = Convert.ToDateTime(startDate).AddMinutes(-1);
                    endDate = Convert.ToDateTime(endDate).AddDays(1).AddSeconds(-1);
                    tblAbbplanMaster = await _context.TblAbbplanMasters.Include(x => x.BusinessUnit).Include(x => x.ProductType).Include(x => x.ProductCat).Where(x => x.IsActive == true && x.BusinessUnitId == BUId
                      && x.CreatedDate >= startDate && x.CreatedDate <= endDate
                              && (string.IsNullOrEmpty(searchValue)
                              || x.AbbplanName.ToLower().Contains(searchValue)
                              || x.Sponsor.ToLower().Contains(searchValue)
                              || x.ToMonth.ToString().Contains(searchValue)
                              || x.FromMonth.ToString().Contains(searchValue)
                              || x.BusinessUnit.Name.ToLower().Contains(searchValue)
                              || x.ProductCat.Description.ToLower().Contains(searchValue)
                              || x.ProductType.Description.ToLower().Contains(searchValue)
                              )).OrderByDescending(x => x.PlanMasterId).ToListAsync();
                }
                recordsTotal = tblAbbplanMaster != null ? tblAbbplanMaster.Count : 0;
                if (tblAbbplanMaster != null)
                {
                    tblAbbplanMaster = sortColumnDirection.Equals(SortingOrder.ASCENDING) ? tblAbbplanMaster.OrderBy(o => o.GetType().GetProperty(sortColumn).GetValue(o, null)).ToList() : tblAbbplanMaster.OrderByDescending(o => o.GetType().GetProperty(sortColumn).GetValue(o, null)).ToList();
                    tblAbbplanMaster = tblAbbplanMaster.Skip(skip).Take(pageSize).ToList();
                }
                else
                    tblAbbplanMaster = new List<TblAbbplanMaster>();

                List<ABBPlanMasterViewModel> ABBPlanMasterList = _mapper.Map<List<TblAbbplanMaster>, List<ABBPlanMasterViewModel>>(tblAbbplanMaster);
                //List<ProductCategoryViewModel> ProductCategoryList = _mapper.Map<List<TblProductCategory>, List<ProductCategoryViewModel>>(tblProductCategory);
                string actionURL = string.Empty;

                foreach (ABBPlanMasterViewModel item in ABBPlanMasterList)
                {
                    actionURL = "<div class='actionbtns'>";
                    actionURL = actionURL + "<a href=' " + URL + "/ABBPlanMaster/Manage?id=" + _protector.Encode(item.PlanMasterId) + "' data-bs-toggle='tooltip' data-bs-placement='top' title='Edit' class='btn btn-sm btn-primary'><i class='fa-solid fa-pen'></i></a> " +
                        "<a href='javascript: void(0)' onclick='deleteConfirm(" + item.PlanMasterId + ")' class='btn btn-sm btn-danger' data-bs-toggle='tooltip' data-bs-placement='top' title='Delete'><i class='fa-solid fa-trash'></i></a>";
                    actionURL = actionURL + " </div>";
                    item.Action = actionURL;

                    TblAbbplanMaster AbbplanMaster = tblAbbplanMaster.FirstOrDefault(x => x.PlanMasterId == item.PlanMasterId);
                    if (AbbplanMaster.CreatedDate != null && AbbplanMaster.CreatedDate != null)
                    {
                        DateTime dateTime = (DateTime)item.CreatedDate;
                        item.Date = dateTime.ToString("yyyy-MM-dd");
                        item.CreatedDate = AbbplanMaster.CreatedDate;
                    }

                    if (AbbplanMaster != null && AbbplanMaster.BusinessUnitId > 0)
                    {
                        TblBusinessUnit tblBusinessUnit = _context.TblBusinessUnits.FirstOrDefault(x => x.BusinessUnitId == AbbplanMaster.BusinessUnitId);
                        item.BusinessUnitName = tblBusinessUnit != null ? tblBusinessUnit.Name : string.Empty;
                    }
                    if (AbbplanMaster != null && AbbplanMaster.ProductTypeId > 0)
                    {
                        TblProductType tblProductType = _context.TblProductTypes.FirstOrDefault(x => x.Id == AbbplanMaster.ProductTypeId);
                        item.ProductTypeName = tblProductType != null ? tblProductType.Description : string.Empty;
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
        public async Task<ActionResult> GetABBPriceMasterListByBUId(int BUId, DateTime? startDate, DateTime? endDate)
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
                if (string.IsNullOrEmpty(searchValue))
                { searchValue = null; }
                else
                { searchValue = searchValue.Trim().ToLower(); }
                int pageSize = length != null ? Convert.ToInt32(length) : 0;
                int skip = start != null ? Convert.ToInt32(start) : 0;
                int recordsTotal = 0;
                //_context = new Digi2L_DevContext();

                if (startDate == null && endDate == null)
                {
                    tblAbbpriceMaster = await _context.TblAbbpriceMasters.Include(x => x.BusinessUnit)
                        .Include(x => x.ProductCat).Include(x => x.ProductType).Where(x => x.IsActive == true && x.BusinessUnitId == BUId
                          && (string.IsNullOrEmpty(searchValue)
                          || x.ProductCategory.ToLower().Contains(searchValue)
                          || x.Sponsor.ToLower().Contains(searchValue)
                          || x.ProductSabcategory.ToLower().Contains(searchValue)
                          || x.BusinessUnit.Name.ToLower().Contains(searchValue)
                          || x.ProductCat.Description.ToLower().Contains(searchValue)
                          || x.ProductType.Description.ToLower().Contains(searchValue)
                          || x.FeeType.ToLower().Contains(searchValue)
                          || x.FeeTypeId.ToString().Contains(searchValue)
                          || x.PriceStartRange.ToString().Contains(searchValue)
                          || x.PriceEndRange.ToString().Contains(searchValue)
                          )).OrderByDescending(x => x.PriceMasterId).ToListAsync();
                }
                else
                {
                    startDate = Convert.ToDateTime(startDate).AddMinutes(-1);
                    endDate = Convert.ToDateTime(endDate).AddDays(1).AddSeconds(-1);
                    tblAbbpriceMaster = await _context.TblAbbpriceMasters.Include(x => x.BusinessUnit)
                        .Include(x => x.ProductCat).Include(x => x.ProductType)
                        .Where(x => x.IsActive == true && x.BusinessUnitId == BUId
                        && x.CreatedDate >= startDate && x.CreatedDate <= endDate
                          && (string.IsNullOrEmpty(searchValue)
                          || x.ProductCategory.ToLower().Contains(searchValue)
                          || x.Sponsor.ToLower().Contains(searchValue)
                          || x.ProductSabcategory.ToLower().Contains(searchValue)
                          || x.BusinessUnit.Name.ToLower().Contains(searchValue)
                          || x.ProductCat.Description.ToLower().Contains(searchValue)
                          || x.ProductType.Description.ToLower().Contains(searchValue)
                          || x.FeeType.ToLower().Contains(searchValue)
                          || x.FeeTypeId.ToString().Contains(searchValue)
                          || x.PriceStartRange.ToString().Contains(searchValue)
                          || x.PriceEndRange.ToString().Contains(searchValue)
                          )).OrderByDescending(x => x.PriceMasterId).ToListAsync();
                }



                recordsTotal = tblAbbpriceMaster != null ? tblAbbpriceMaster.Count : 0;
                if (tblAbbpriceMaster != null)
                {
                    //tblAbbpriceMaster = sortColumnDirection.Equals(SortingOrder.ASCENDING) ? tblAbbpriceMaster.OrderBy(o => o.GetType().GetProperty(sortColumn).GetValue(o, null)).ToList() : tblAbbpriceMaster.OrderByDescending(o => o.GetType().GetProperty(sortColumn).GetValue(o, null)).ToList();
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
                        "<a href='javascript: void(0)' onclick='deleteConfirm(" + item.PriceMasterId + ")' class='btn btn-sm btn-danger' data-bs-toggle='tooltip' data-bs-placement='top' title='Delete'><i class='fa-solid fa-trash'></i></a>";
                    actionURL = actionURL + " </div>";
                    item.Action = actionURL;

                    TblAbbpriceMaster AbbpriceMaster = tblAbbpriceMaster.FirstOrDefault(x => x.PriceMasterId == item.PriceMasterId);
                    if (AbbpriceMaster.CreatedDate != null && AbbpriceMaster.CreatedDate != null)
                    {
                        //var Date = (DateTime)item.CreatedDate;
                        //item.Date = Date.ToShortDateString();


                        DateTime dateTime = (DateTime)item.CreatedDate;
                        item.Date = dateTime.ToString("yyyy-MM-dd");
                        item.CreatedDate = AbbpriceMaster.CreatedDate;

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

        [HttpPost]
        public async Task<ActionResult> GetUniversalPriceMasterListByBUId(int BUId, DateTime? startDate, DateTime? endDate)
        {
            List<TblUniversalPriceMaster> tblUniversalPriceMaster = null;

            string URL = _config.Value.URLPrefixforProd;

            try
            {
                var draw = Request.Form["draw"].FirstOrDefault();
                var start = Request.Form["start"].FirstOrDefault();
                var length = Request.Form["length"].FirstOrDefault();
                var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
                var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();
                var searchValue = Request.Form["search[value]"].FirstOrDefault();
                if (string.IsNullOrEmpty(searchValue))
                { searchValue = null; }
                else
                { searchValue = searchValue.Trim().ToLower(); }
                int pageSize = length != null ? Convert.ToInt32(length) : 0;
                int skip = start != null ? Convert.ToInt32(start) : 0;
                int recordsTotal = 0;
                //_context = new Digi2L_DevContext();

                if (BUId != null)
                {
                    TblBusinessUnit tblBusinessUnit = _context.TblBusinessUnits.Where(x => x.IsActive == true && x.BusinessUnitId == BUId).FirstOrDefault();
                    if (tblBusinessUnit != null)
                    {
                        var tblPriceMasterMapping = _context.TblPriceMasterMappings.Where(x => x.BusinessUnitId == tblBusinessUnit.BusinessUnitId && x.IsActive == true).FirstOrDefault();


                        if (tblPriceMasterMapping != null)
                        {
                            if (startDate == null && endDate == null)
                            {
                                tblUniversalPriceMaster = await _context.TblUniversalPriceMasters.Where(x => x.IsActive == true && x.PriceMasterNameId == tblPriceMasterMapping.PriceMasterNameId
                                && (string.IsNullOrEmpty(searchValue)
                                || x.ProductCategoryName.ToLower().Contains(searchValue)
                                || x.ProductTypeName.ToLower().Contains(searchValue)
                                || x.BrandName2.ToLower().Contains(searchValue)
                                || x.BrandName1.ToLower().Contains(searchValue)
                                || x.BrandName3.ToLower().Contains(searchValue)
                                || x.BrandName4.ToLower().Contains(searchValue)
                                || x.ProductTypeCode.ToLower().Contains(searchValue)
                                )).OrderByDescending(x => x.PriceMasterUniversalId).ToListAsync();
                            }
                            else
                            {
                                startDate = Convert.ToDateTime(startDate).AddMinutes(-1);
                                endDate = Convert.ToDateTime(endDate).AddDays(1).AddSeconds(-1);
                                tblUniversalPriceMaster = await _context.TblUniversalPriceMasters.Where(x => x.IsActive == true && x.PriceMasterNameId == tblPriceMasterMapping.PriceMasterNameId
                               && x.CreatedDate >= startDate && x.CreatedDate <= endDate
                                        && (string.IsNullOrEmpty(searchValue)
                                       | x.ProductCategoryName.ToLower().Contains(searchValue)
                                || x.ProductTypeName.ToLower().Contains(searchValue)
                                || x.BrandName2.ToLower().Contains(searchValue)
                                || x.BrandName1.ToLower().Contains(searchValue)
                                || x.BrandName3.ToLower().Contains(searchValue)
                                || x.BrandName4.ToLower().Contains(searchValue)
                                || x.ProductTypeCode.ToLower().Contains(searchValue)
                                )).OrderByDescending(x => x.PriceMasterUniversalId).ToListAsync();
                            }
                        }
                    }
                }
                recordsTotal = tblUniversalPriceMaster != null ? tblUniversalPriceMaster.Count : 0;
                if (tblUniversalPriceMaster != null)
                {
                    tblUniversalPriceMaster = sortColumnDirection.Equals(SortingOrder.ASCENDING) ? tblUniversalPriceMaster.OrderBy(o => o.GetType().GetProperty(sortColumn).GetValue(o, null)).ToList() : tblUniversalPriceMaster.OrderByDescending(o => o.GetType().GetProperty(sortColumn).GetValue(o, null)).ToList();
                    tblUniversalPriceMaster = tblUniversalPriceMaster.Skip(skip).Take(pageSize).ToList();
                }
                else
                    tblUniversalPriceMaster = new List<TblUniversalPriceMaster>();

                List<UniversalPriceMasterViewModel> UniversalPriceMasterList = _mapper.Map<List<TblUniversalPriceMaster>, List<UniversalPriceMasterViewModel>>(tblUniversalPriceMaster);
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
            catch (Exception ex)
            {
                throw;
            }
        }

        #region Display List of Business Partners by BusinessUnit Id
        public async Task<IActionResult> GetBusinessPartnerListByBUId(int BUId, DateTime? startDate, DateTime? endDate)
        {
            string URL = _config.Value.URLPrefixforProd;
            List<TblBusinessPartner> TblBusinessPartners = null;
            TblBusinessUnit tblBusinessUnit = null;
            try
            {
                var draw = Request.Form["draw"].FirstOrDefault();
                var start = Request.Form["start"].FirstOrDefault();
                var length = Request.Form["length"].FirstOrDefault();
                var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
                var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();
                var searchValue = Request.Form["search[value]"].FirstOrDefault();
                if (string.IsNullOrEmpty(searchValue))
                { searchValue = null; }
                else
                { searchValue = searchValue.Trim().ToLower(); }
                int pageSize = length != null ? Convert.ToInt32(length) : 0;
                int skip = start != null ? Convert.ToInt32(start) : 0;
                int recordsTotal = 0; int count = 0;

                tblBusinessUnit = _context.TblBusinessUnits.Where(x => x.IsActive == true && x.BusinessUnitId == BUId).FirstOrDefault();
                if (tblBusinessUnit != null)
                {
                    if (startDate == null && endDate == null)
                    {
                        count = _context.TblBusinessPartners.Count(x => x.IsActive == true && x.BusinessUnitId == tblBusinessUnit.BusinessUnitId
                        && (string.IsNullOrEmpty(searchValue)
                        || x.StoreCode.ToLower().Contains(searchValue) || x.Email.ToLower().Contains(searchValue)
                        || x.Name.ToLower().Contains(searchValue) || x.State.ToLower().Contains(searchValue)
                        || x.City.ToLower().Contains(searchValue) || x.Name.ToLower().Contains(searchValue)
                        || x.State.ToLower().Contains(searchValue) || x.AddressLine1.ToLower().Contains(searchValue)
                        || x.AddressLine2.ToLower().Contains(searchValue) || x.Bppassword.ToLower().Contains(searchValue)
                        || x.AssociateCode.ToLower().Contains(searchValue)));

                        if (count > 0)
                        {
                            TblBusinessPartners = await _context.TblBusinessPartners.Where(x => x.IsActive == true && x.BusinessUnitId == tblBusinessUnit.BusinessUnitId
                        && (string.IsNullOrEmpty(searchValue)
                        || x.StoreCode.ToLower().Contains(searchValue) || x.Email.ToLower().Contains(searchValue)
                        || x.Name.ToLower().Contains(searchValue) || x.State.ToLower().Contains(searchValue)
                        || x.City.ToLower().Contains(searchValue) || x.Name.ToLower().Contains(searchValue)
                        || x.State.ToLower().Contains(searchValue) || x.AddressLine1.ToLower().Contains(searchValue)
                        || x.AddressLine2.ToLower().Contains(searchValue) || x.Bppassword.ToLower().Contains(searchValue)
                        || x.AssociateCode.ToLower().Contains(searchValue))).OrderByDescending(x => x.BusinessPartnerId).Skip(skip).Take(pageSize).ToListAsync();
                        }
                    }
                    else
                    {
                        startDate = Convert.ToDateTime(startDate).AddMinutes(-1);
                        endDate = Convert.ToDateTime(endDate).AddDays(1).AddSeconds(-1);
                        count = _context.TblBusinessPartners.Count(x => x.IsActive == true && x.BusinessUnitId == tblBusinessUnit.BusinessUnitId
                        && x.CreatedDate >= startDate && x.CreatedDate <= endDate
                        && (string.IsNullOrEmpty(searchValue)
                        || x.StoreCode.ToLower().Contains(searchValue) || x.Email.ToLower().Contains(searchValue)
                        || x.Name.ToLower().Contains(searchValue) || x.State.ToLower().Contains(searchValue)
                        || x.City.ToLower().Contains(searchValue) || x.Name.ToLower().Contains(searchValue)
                        || x.State.ToLower().Contains(searchValue) || x.AddressLine1.ToLower().Contains(searchValue)
                        || x.AddressLine2.ToLower().Contains(searchValue) || x.Bppassword.ToLower().Contains(searchValue)
                        || x.AssociateCode.ToLower().Contains(searchValue)));

                        if (count > 0)
                        {
                            TblBusinessPartners = await _context.TblBusinessPartners.Where(x => x.IsActive == true && x.BusinessUnitId == tblBusinessUnit.BusinessUnitId
                            && x.CreatedDate >= startDate && x.CreatedDate <= endDate
                            && (string.IsNullOrEmpty(searchValue)
                            || x.StoreCode.ToLower().Contains(searchValue) || x.Email.ToLower().Contains(searchValue)
                            || x.Name.ToLower().Contains(searchValue) || x.State.ToLower().Contains(searchValue)
                            || x.City.ToLower().Contains(searchValue) || x.Name.ToLower().Contains(searchValue)
                            || x.State.ToLower().Contains(searchValue) || x.AddressLine1.ToLower().Contains(searchValue)
                            || x.AddressLine2.ToLower().Contains(searchValue) || x.Bppassword.ToLower().Contains(searchValue)
                            || x.AssociateCode.ToLower().Contains(searchValue))).OrderByDescending(x => x.BusinessPartnerId).Skip(skip).Take(pageSize).ToListAsync();
                        }
                    }
                }

                recordsTotal = count;
                List<BusinessPartnerViewModel> BusinessPartnerList = _mapper.Map<List<TblBusinessPartner>, List<BusinessPartnerViewModel>>(TblBusinessPartners);
                string actionURL = string.Empty;

                foreach (BusinessPartnerViewModel item in BusinessPartnerList)
                {
                    actionURL = "<div class='actionbtns'>";
                    if (item.QrcodeUrl != null)
                    {
                        actionURL += "<a href=' " + URL + "/DBFiles/GeneratedQRImages/" + item.QrcodeUrl + "' data-bs-toggle='tooltip' data-bs-placement='top' title='Download QR-Code' class='btn btn-sm btn-primary' target='_blanck' download><i class='fa-sharp fa-solid fa-qrcode fa-xl'></i></a> ";
                    }
                    actionURL +=
                        "<a href=' " + URL + "/BusinessPartner/Manage?id=" + _protector.Encode(item.BusinessPartnerId) + "' data-bs-toggle='tooltip' data-bs-placement='top' title='Edit' class='btn btn-sm btn-primary' target='_blanck'><i class='fa-solid fa-pen'></i></a> " +
                        "<a href='javascript: void(0)' onclick='deleteConfirm(" + item.BusinessPartnerId + ")' class='btn btn-sm btn-danger' data-bs-toggle='tooltip' data-bs-placement='top' title='Delete'><i class='fa-solid fa-trash'></i></a>";
                    actionURL = actionURL + " </div>";
                    item.Action = actionURL;

                    TblBusinessPartner BusinessPartner = TblBusinessPartners.FirstOrDefault(x => x.BusinessPartnerId == item.BusinessPartnerId);
                    if (BusinessPartner.CreatedDate != null && BusinessPartner.CreatedDate != null)
                    {
                        DateTime dateTime = (DateTime)item.CreatedDate;
                        item.Date = dateTime.ToString("MM/dd/yyyy");
                        item.CreatedDate = BusinessPartner.CreatedDate;
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

        #endregion

        #region List of PriceMasterName
        [HttpPost]
        public async Task<ActionResult> PriceMasterNameList(string? name, string? description, DateTime? startDate, DateTime? endDate)
        {
            #region Variable declaration
            if (!string.IsNullOrWhiteSpace(name) && name != "null")
            { name = name.Trim().ToLower(); }
            else { name = null; }
            if (!string.IsNullOrWhiteSpace(description) && description != "null")
            { description = description.Trim().ToLower(); }
            else { description = null; }


            string URL = _config.Value.URLPrefixforProd;
            string MVCURL = _config.Value.MVCBaseURLForExchangeInvoice;
            List<TblPriceMasterName> priceMasterNames = null;
           
            int count = 0;
            string actionURL = string.Empty;

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
                int searchByBusinessUnitId = 0;




                if (startDate == null && endDate == null)
                {

                    count = _context.TblPriceMasterNames.Count(x => x.IsActive == true
                         && ((string.IsNullOrEmpty(name)) || x.Name.ToLower() == name)
                         && (string.IsNullOrEmpty(description)) || x.Description == description);

                    if (count > 0)
                    {
                        priceMasterNames = await _context.TblPriceMasterNames.Where(x => x.IsActive == true
                       && ((string.IsNullOrEmpty(name)) || x.Name.ToLower() == name)
                       && (string.IsNullOrEmpty(description)) || x.Description == description)
                       .OrderByDescending(x => x.CreatedDate).Skip(skip).Take(pageSize).ToListAsync();


                    }

                }
                else
                {
                    startDate = Convert.ToDateTime(startDate).AddMinutes(-1);
                    endDate = Convert.ToDateTime(endDate).AddDays(1).AddSeconds(-1);

                    count = _context.TblPriceMasterNames.Count(x => x.IsActive == true && x.CreatedDate >= startDate && x.CreatedDate <= endDate
                    && ((string.IsNullOrEmpty(name)) || x.Name.ToLower() == name)
                         && (string.IsNullOrEmpty(description)) || x.Description == description);

                    if (count > 0)
                    {
                        priceMasterNames = await _context.TblPriceMasterNames.Where(x => x.IsActive == true && x.CreatedDate >= startDate && x.CreatedDate <= endDate
                       && ((string.IsNullOrEmpty(name)) || x.Name.ToLower() == name)
                       && (string.IsNullOrEmpty(description)) || x.Description == description)
                       .OrderByDescending(x => x.CreatedDate).Skip(skip).Take(pageSize).ToListAsync();

                    }
                }


                recordsTotal = count;
                

                List<PriceMasterNameViewModel> priceMasterNameViewModelList = _mapper.Map<List<TblPriceMasterName>, List<PriceMasterNameViewModel>>(priceMasterNames);
                foreach (PriceMasterNameViewModel item in priceMasterNameViewModelList)
                {
                    actionURL = " <div class='actionbtns'>";
                    actionURL = "<a href ='" + URL + "/NewPriceMasterName/Manage?id=" + _protector.Encode(item.PriceMasterNameId) + "' ><button onclick='RecordEdit(" + _protector.Encode(item.PriceMasterNameId) + ")' class='btn btn-sm btn-primary' data-bs-toggle='tooltip' data-bs-placement='top' title='Edit'><i class='fa-solid fa-pen'></i></button></a>&nbsp;" +
                        "<a href='javascript: void(0)' onclick='deleteConfirm(" + item.PriceMasterNameId + ")' class='btn btn-sm btn-danger' data-bs-toggle='tooltip' data-bs-placement='top' title='Delete'><i class='fa-solid fa-trash'></i></a>";
                    actionURL = actionURL + " </div>";
                    item.Action = actionURL;
                    TblPriceMasterName PriceMasterobj = _context.TblPriceMasterNames.FirstOrDefault(x => x.PriceMasterNameId == item.PriceMasterNameId);
                    if (PriceMasterobj.CreatedDate != null)
                    {
                        //var Date = (DateTime)item.CreatedDate;
                        //item.Date = Date.ToShortDateString();


                        DateTime dateTime = (DateTime)item.CreatedDate;
                        item.Date = dateTime.ToString("yyyy-MM-dd");
                        //item.CreatedDate = tblUniversalPriceMasterobj.CreatedDate;

                    }
                }

                var data = priceMasterNameViewModelList;
                var jsonData = new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data };
                return Ok(jsonData);
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("MasterListController", "PriceMasterNameList", ex);
            }
            return Ok();
        }
        #endregion

        #region List of PriceMasterMapping 
        /// <summary>
        /// Added by Ashwin for getting list of PriceMasterMapping 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="description"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> PriceMasterMappingList(string? Company, string? BusinessPartner, string? Brand, string? PriceMasterName, DateTime? startDate, DateTime? endDate)
        {
            #region Variable declaration
            if (!string.IsNullOrWhiteSpace(Company) && Company != "null")
            { Company = Company.Trim().ToLower(); }
            else { Company = null; }
            if (!string.IsNullOrWhiteSpace(BusinessPartner) && BusinessPartner != "null")
            { BusinessPartner = BusinessPartner.Trim().ToLower(); }
            else { BusinessPartner = null; }
            if (!string.IsNullOrWhiteSpace(Brand) && Brand != "null")
            { Brand = Brand.Trim().ToLower(); }
            else { Brand = null; }
            if (!string.IsNullOrWhiteSpace(PriceMasterName) && PriceMasterName != "null")
            { PriceMasterName = PriceMasterName.Trim().ToLower(); }
            else { PriceMasterName = null; }

            string URL = _config.Value.URLPrefixforProd;
            string MVCURL = _config.Value.MVCBaseURLForExchangeInvoice;
            List<TblPriceMasterMapping> tblPriceMasterMapping = null;
           
            int count = 0;
            string actionURL = string.Empty;

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
                int searchByBusinessUnitId = 0;

                #region Advanced Filters Mapping
                if (startDate != null && endDate != null)
                {
                    startDate = Convert.ToDateTime(startDate).AddMinutes(-1);
                    endDate = Convert.ToDateTime(endDate).AddDays(1).AddSeconds(-1);
                }
                #endregion

                #region table object Initialization

                if (startDate == null && endDate == null)
                {

                    count = _context.TblPriceMasterMappings.Count(x => x.IsActive == true);

                    if (count > 0)
                    {
                        tblPriceMasterMapping = await _context.TblPriceMasterMappings
                        .Include(x => x.BusinessUnit)
                        .Include(x => x.BusinessPartner)
                        .Include(x => x.PriceMasterName)
                        .Include(x => x.Brand)
                        .Where(x => x.IsActive == true
                        && ((string.IsNullOrEmpty(Company)) || x.BusinessUnit.Name == Company)
                        && ((string.IsNullOrEmpty(BusinessPartner)) || x.BusinessPartner.Name == BusinessPartner)
                        && ((string.IsNullOrEmpty(Brand)) || x.Brand.Name == Brand)
                        && ((string.IsNullOrEmpty(PriceMasterName)) || x.PriceMasterName.Name == PriceMasterName))                                                                                                              //&& (string.IsNullOrEmpty(description)) || x.Description == description)
                        .OrderByDescending(x => x.CreatedDate).Skip(skip).Take(pageSize).ToListAsync();


                    }

                }
                else
                {
                    startDate = Convert.ToDateTime(startDate).AddMinutes(-1);
                    endDate = Convert.ToDateTime(endDate).AddDays(1).AddSeconds(-1);

                    count = _context.TblPriceMasterMappings.Count(x => x.IsActive == true && x.CreatedDate >= startDate && x.CreatedDate <= endDate);

                    if (count > 0)
                    {
                        tblPriceMasterMapping = await _context.TblPriceMasterMappings
                        .Include(x => x.BusinessUnit)
                        .Include(x => x.BusinessPartner)
                        .Include(x => x.PriceMasterName)
                        .Include(x => x.Brand)
                        .Where(x => x.IsActive == true && x.CreatedDate >= startDate && x.CreatedDate <= endDate

                        && ((string.IsNullOrEmpty(Company)) || x.BusinessUnit.Name == Company)
                        && ((string.IsNullOrEmpty(BusinessPartner)) || x.BusinessPartner.Name == BusinessPartner)
                        && ((string.IsNullOrEmpty(Brand)) || x.Brand.Name == Brand)
                        && ((string.IsNullOrEmpty(PriceMasterName)) || x.PriceMasterName.Name == PriceMasterName))                                                                                                              //&& (string.IsNullOrEmpty(description)) || x.Description == description)
                        .OrderByDescending(x => x.CreatedDate).Skip(skip).Take(pageSize).ToListAsync();


                    }
                }


                recordsTotal = count;


                #endregion

                List<PriceMasterMappingViewModel> priceMasterMappingViewModelList = _mapper.Map<List<TblPriceMasterMapping>, List<PriceMasterMappingViewModel>>(tblPriceMasterMapping);

                foreach (PriceMasterMappingViewModel item in priceMasterMappingViewModelList)
                {
                    actionURL = " <div class='actionbtns'>";
                    actionURL = "<a href ='" + URL + "/NewPriceMasterMapping/Manage?id=" + _protector.Encode(item.PriceMasterMappingId) + "' ><button onclick='RecordEdit(" + item.PriceMasterMappingId + ")' class='btn btn-sm btn-primary' data-bs-toggle='tooltip' data-bs-placement='top' title='Edit'><i class='fa-solid fa-pen'></i></button></a>&nbsp;" +
                        "<a href='javascript: void(0)' onclick='deleteConfirm(" + item.PriceMasterMappingId + ")' class='btn btn-sm btn-danger' data-bs-toggle='tooltip' data-bs-placement='top' title='Delete'><i class='fa-solid fa-trash'></i></a>";
                    actionURL = actionURL + " </div>";
                    item.Action = actionURL;

                    TblPriceMasterMapping PriceMasterMappingobj = _context.TblPriceMasterMappings.FirstOrDefault(x => x.PriceMasterMappingId == item.PriceMasterMappingId);
                    if (PriceMasterMappingobj.CreatedDate != null)
                    {
                        //var Date = (DateTime)item.CreatedDate;
                        //item.Date = Date.ToShortDateString();


                        DateTime dateTime = (DateTime)item.CreatedDate;
                        item.Date = dateTime.ToString("yyyy-MM-dd");
                        //item.CreatedDate = tblUniversalPriceMasterobj.CreatedDate;

                    }
                    TblPriceMasterName PriceMasterobj = _context.TblPriceMasterNames.FirstOrDefault(x => x.PriceMasterNameId == item.PriceMasterNameId);
                    if (PriceMasterobj.CreatedDate != null)
                    {
                        //var Date = (DateTime)item.CreatedDate;
                        //item.Date = Date.ToShortDateString();


                        DateTime dateTime = (DateTime)item.CreatedDate;
                        item.Date = dateTime.ToString("yyyy-MM-dd");
                        //item.CreatedDate = tblUniversalPriceMasterobj.CreatedDate;

                    }

                    if (item.BusinessUnitId > 0)
                    {
                        TblBusinessUnit tblBusinessUnit = _businessUnitRepository.GetSingle(x => x.BusinessUnitId == Convert.ToInt32(item.BusinessUnitId));
                        item.BusinessUnitName = tblBusinessUnit?.Name != string.Empty ? tblBusinessUnit.Name : string.Empty;
                    }

                    if (item.BusinessPartnerId > 0)
                    {
                        TblBusinessPartner tblBusinessPartner = _businessPartnerRepository.GetSingle(x=> x.BusinessPartnerId == Convert.ToInt32(item.BusinessPartnerId));
                        if(tblBusinessPartner != null)
                        {
                            item.BusinessPartnerName = tblBusinessPartner.Name != string.Empty ? tblBusinessPartner?.Name : string.Empty;
                        }
                        
                    }
                    if (item.BrandId > 0)
                    {
                        TblBrand tblBrand = _brandRepository.GetSingle(x => x.IsActive == true && x.Id == Convert.ToInt32(item.BrandId));
                        if(tblBrand != null)
                        {
                            item.BrandName = tblBrand.Name != string.Empty ? tblBrand?.Name : string.Empty;
                        }
                        
                    }
                    if (item.PriceMasterNameId > 0)
                    {
                        TblPriceMasterName priceMasterName = _priceMasterNameRepository.GetSingle(x=> x.PriceMasterNameId == Convert.ToInt32(item.PriceMasterNameId));
                        if(priceMasterName != null)
                        {
                            item.PriceMasterName = priceMasterName.Name != string.Empty ? priceMasterName?.Name : string.Empty;
                        }
                        
                    }
                }
                var data = priceMasterMappingViewModelList;
                var jsonData = new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data };
                return Ok(jsonData);

            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("MasterListController", "PriceMasterMappingList", ex);
            }
            return Ok();
        }
        #endregion


        #region Display List of Business Partners by BusinessUnit Id
        public async Task<IActionResult> GetModelNumberListByBUId(int BUId, DateTime? startDate, DateTime? endDate)
        {
            string URL = _config.Value.URLPrefixforProd;
            List<TblModelNumber> TblModelNumbers = null;
            TblBusinessUnit tblBusinessUnit = null;
            try
            {
                var draw = Request.Form["draw"].FirstOrDefault();
                var start = Request.Form["start"].FirstOrDefault();
                var length = Request.Form["length"].FirstOrDefault();
                var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
                var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();
                var searchValue = Request.Form["search[value]"].FirstOrDefault();
                if (string.IsNullOrEmpty(searchValue))
                { searchValue = null; }
                else
                { searchValue = searchValue.Trim().ToLower(); }
                int pageSize = length != null ? Convert.ToInt32(length) : 0;
                int skip = start != null ? Convert.ToInt32(start) : 0;
                int recordsTotal = 0; int count = 0;

                tblBusinessUnit = _context.TblBusinessUnits.Where(x => x.IsActive == true && x.BusinessUnitId == BUId).FirstOrDefault();
                if (tblBusinessUnit != null)
                {
                    if (startDate == null && endDate == null)
                    {
                        count = _context.TblModelNumbers.Count(x => x.IsActive == true && x.BusinessUnitId == tblBusinessUnit.BusinessUnitId
                        && (string.IsNullOrEmpty(searchValue)
                        || x.ProductCategory.Description.ToLower().Contains(searchValue)
                        || x.ProductType.Description.ToLower().Contains(searchValue)
                        || x.ModelName.ToLower().Contains(searchValue)
                        || x.Code.ToLower().Contains(searchValue)
                        || x.Description.ToLower().Contains(searchValue)
                        || x.BusinessUnit.Name.ToLower().Contains(searchValue)
                        || x.Brand.Name.ToLower().Contains(searchValue))); 
                        

                        if (count > 0)
                        {
                            TblModelNumbers = await _context.TblModelNumbers.Where(x => x.IsActive == true && x.BusinessUnitId == tblBusinessUnit.BusinessUnitId
                        && (string.IsNullOrEmpty(searchValue)
                        || x.ProductCategory.Description.ToLower().Contains(searchValue)
                        || x.ProductType.Description.ToLower().Contains(searchValue)
                        || x.ModelName.ToLower().Contains(searchValue)
                        || x.Code.ToLower().Contains(searchValue)
                        || x.Description.ToLower().Contains(searchValue)
                        || x.BusinessUnit.Name.ToLower().Contains(searchValue)
                        || x.Brand.Name.ToLower().Contains(searchValue))).OrderByDescending(x => x.BusinessPartnerId).Skip(skip).Take(pageSize).ToListAsync();
                        }
                    }
                    else
                    {
                        startDate = Convert.ToDateTime(startDate).AddMinutes(-1);
                        endDate = Convert.ToDateTime(endDate).AddDays(1).AddSeconds(-1);
                        count = _context.TblModelNumbers.Count(x => x.IsActive == true && x.BusinessUnitId == tblBusinessUnit.BusinessUnitId
                        && x.CreatedDate >= startDate && x.CreatedDate <= endDate
                        && (string.IsNullOrEmpty(searchValue)
                        || x.ProductCategory.Description.ToLower().Contains(searchValue)
                        || x.ProductType.Description.ToLower().Contains(searchValue)
                        || x.ModelName.ToLower().Contains(searchValue)
                        || x.Code.ToLower().Contains(searchValue)
                        || x.Description.ToLower().Contains(searchValue)
                        || x.BusinessUnit.Name.ToLower().Contains(searchValue)
                        || x.Brand.Name.ToLower().Contains(searchValue)));

                        if (count > 0)
                        {
                            TblModelNumbers = await _context.TblModelNumbers.Where(x => x.IsActive == true && x.BusinessUnitId == tblBusinessUnit.BusinessUnitId
                            && x.CreatedDate >= startDate && x.CreatedDate <= endDate
                             && (string.IsNullOrEmpty(searchValue)
                        || x.ProductCategory.Description.ToLower().Contains(searchValue)
                        || x.ProductType.Description.ToLower().Contains(searchValue)
                        || x.ModelName.ToLower().Contains(searchValue)
                        || x.Code.ToLower().Contains(searchValue)
                        || x.Description.ToLower().Contains(searchValue)
                        || x.BusinessUnit.Name.ToLower().Contains(searchValue)
                        || x.Brand.Name.ToLower().Contains(searchValue))).OrderByDescending(x => x.BusinessPartnerId).Skip(skip).Take(pageSize).ToListAsync();
                        }
                    }
                }

                recordsTotal = count;
                List<ModelNumberViewModel> ModelNumberList = _mapper.Map<List<TblModelNumber>, List<ModelNumberViewModel>>(TblModelNumbers);
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


                    TblModelNumber ModelNumber = TblModelNumbers.FirstOrDefault(x => x.BusinessPartnerId == item.BusinessPartnerId);
                    if (ModelNumber.CreatedDate != null && ModelNumber.CreatedDate != null)
                    {
                        DateTime dateTime = (DateTime)item.CreatedDate;
                        item.Date = dateTime.ToString("MM/dd/yyyy");
                        item.CreatedDate = ModelNumber.CreatedDate;
                    }

                    if (ModelNumber != null && ModelNumber.BrandId > 0)
                    {
                        TblBrand tblBrand = _context.TblBrands.FirstOrDefault(x => x.Id == ModelNumber.BrandId);
                        item.BrandName = tblBrand != null ? tblBrand.Name : string.Empty;
                    }

                    if (ModelNumber != null && ModelNumber.ProductTypeId > 0)
                    {
                        TblProductType tblProductType = _context.TblProductTypes.FirstOrDefault(x => x.Id == ModelNumber.ProductTypeId);
                        item.ProductTypeName = tblProductType != null ? tblProductType.Description : string.Empty;
                    }

                    if (ModelNumber != null && ModelNumber.ProductCategoryId > 0)
                    {
                        TblProductCategory tblProductCategory = _context.TblProductCategories.FirstOrDefault(x => x.Id == ModelNumber.ProductCategoryId);
                        item.ProductCategoryName = tblProductCategory != null ? tblProductCategory.Description : string.Empty;
                    }

                    if (ModelNumber != null && ModelNumber.BusinessUnitId > 0)
                    {
                        
                        item.BusinessUnitName = tblBusinessUnit != null ? tblBusinessUnit.Name : string.Empty;
                    }

                    if (ModelNumber != null && ModelNumber.BusinessPartnerId > 0)
                    {
                        TblBusinessPartner TblBusinessPartner = _context.TblBusinessPartners.FirstOrDefault(x => x.BusinessPartnerId == ModelNumber.BusinessPartnerId);
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

        #endregion

    }
}







