using ClosedXML.Excel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using RDCELERP.BAL.Interface;
using RDCELERP.BAL.MasterManager;
using RDCELERP.Common.Helper;
using RDCELERP.Core.App.Helper;
using RDCELERP.Core.App.Pages.Base;
using RDCELERP.DAL.Entities;
using RDCELERP.DAL.IRepository;
using RDCELERP.DAL.Repository;
using RDCELERP.Model.Base;
using RDCELERP.Model.BusinessPartner;
using RDCELERP.Model.DealerDashBoard;
using RDCELERP.Model.EVC;
using RDCELERP.Model.ImagLabel;
using RDCELERP.Model.LGC;
using static Org.BouncyCastle.Math.EC.ECCurve;

namespace RDCELERP.Core.App.Pages.DealerDashboard
{
    public class ExchangeDashboardModel : BasePageModel
    {
        #region variable declaration
        private readonly RDCELERP.DAL.Entities.Digi2l_DevContext _context;
        public readonly IOptions<ApplicationSettings> _config;
        private CustomDataProtection _protector;
        private readonly IExchangeOrderManager _exchangeOrderManager;
        private readonly IDealerManager _dashBoardManager;
        IExchangeOrderRepository _exchangeOrderRepository;
        IBusinessPartnerRepository _businessPartnerRepository;
        public ILogging _logging;
        #endregion
        #region Constructor
        public ExchangeDashboardModel(IOptions<ApplicationSettings> config, Digi2l_DevContext _dbcontext, CustomDataProtection _dataprotector, IExchangeOrderManager exchangeOrderManager, IDealerManager dealerDashBoardManager, ILogging logging, IBusinessPartnerRepository businessPartnerRepository, IExchangeOrderRepository exchangeOrderRepository) : base(config)
        {
            _config = config;
            _context = _dbcontext;
            _protector = _dataprotector;
            _exchangeOrderManager = exchangeOrderManager;
            _dashBoardManager = dealerDashBoardManager;
            _logging = logging;
            _businessPartnerRepository = businessPartnerRepository;
            _exchangeOrderRepository = exchangeOrderRepository;
        }
        #endregion
        #region Model Binding
        [BindProperty(SupportsGet = true)]
        public OrderCountViewModel OrderCountViewModel { get; set; }
        [BindProperty(SupportsGet = true)]
        public TblUser TblUser { get; set; }
        [BindProperty(SupportsGet = true)]
        public TblUserRole TblUserRole { get; set; }
        [BindProperty(SupportsGet = true)]
        public StoreListModel storeListModel { get; set; }

        [BindProperty(SupportsGet = true)]
        public DealerDashboardViewModel dealerDashboardViewModel { get; set; }
        [BindProperty (SupportsGet = true)]
        public string AssociateCodeforbp { get; set; } = string.Empty;
        [BindProperty(SupportsGet = true)]
        public  TblCompany TblCompany { get; set; }

        [BindProperty(SupportsGet = true)]
        public TblBusinessUnit TblBusinessUnit { get; set; }


        [BindProperty(SupportsGet =true)]
        public BusinessUnitObject businessunitObj { get; set; }
        #endregion
        public IActionResult OnGet()
        {
            int companyId = 0;
            int BusinessUnitId = 0;
            string BusinessUnitName =null;
            string RoleName=null;
            int UserComapny = 0;
            string UserComapnyName = null;
            CompanyList companyModel = new CompanyList();
            IEnumerable<SelectListItem> sponsorlist = null;
            companyModel.BusinessUnitList = new List<SelectListItem>();
            try
            {
                if (_loginSession == null)
                {
                    return RedirectToPage("/index");
                }
                else
                {
                    int userId = _loginSession.UserViewModel.UserId;
                    if(_loginSession.RoleViewModel.CompanyId > 0)
                    {
                        companyId=Convert.ToInt32(_loginSession.RoleViewModel.CompanyId);
                        RoleName = _loginSession.RoleViewModel.RoleName;
                        
                        TblCompany = _context.TblCompanies.Include(x => x.BusinessUnit).FirstOrDefault(x => x.CompanyId == companyId);
                        if (TblCompany != null)
                        {
                            if (TblCompany.BusinessUnit != null)
                            {
                                BusinessUnitName = TblCompany.BusinessUnit.Name;
                                BusinessUnitId = TblCompany.BusinessUnit.BusinessUnitId;
                                OrderCountViewModel.BusinessUnitId=BusinessUnitId;
                            }
                        }
                    }
                    if (_loginSession.UserViewModel.CompanyId > 0)
                    {
                        TblCompany = _context.TblCompanies.FirstOrDefault(x=>x.CompanyId== _loginSession.UserViewModel.CompanyId);
                        if (TblCompany != null)
                        {
                            UserComapnyName = TblCompany.CompanyName;
                            if (TblCompany.BusinessUnitId > 0)
                            {
                                OrderCountViewModel.BusinessUnitId = Convert.ToInt32(TblCompany.BusinessUnitId);
                            }
                        }
                    }

                    TblUser = _context.TblUsers
                       .Include(t => t.CreatedByNavigation)
                       .Include(t => t.ModifiedByNavigation).FirstOrDefault(m => m.IsActive == true && m.UserId == Convert.ToInt32(userId));
                    if (TblUser != null)
                    {
                        TblUser.Email = SecurityHelper.DecryptString(TblUser.Email, _config.Value.SecurityKey);
                        TblUser.Phone = SecurityHelper.DecryptString(TblUser.Phone, _config.Value.SecurityKey);
                        TblUser.Password = SecurityHelper.DecryptString(TblUser.Password, _config.Value.SecurityKey);
                    }

                    if (TblUser == null)
                    {
                        return NotFound();
                    }
                    else
                    {
                        OrderCountViewModel = _dashBoardManager.GetOrderCount(TblUser.Password, BusinessUnitName, RoleName, UserComapnyName);
                        OrderCountViewModel.UserEmail = TblUser.Email;
                        OrderCountViewModel.AssociateCode = TblUser.Password;
                        AssociateCodeforbp= TblUser.Password;
                        OrderCountViewModel.BusinessUnitId = BusinessUnitId;
                        OrderCountViewModel.UserCompanyName = UserComapnyName;
                        OrderCountViewModel.CompanyName = BusinessUnitName;
                        OrderCountViewModel.userRole = RoleName;
                        OrderCountViewModel.CityList = new List<Microsoft.AspNetCore.Mvc.Rendering.SelectListItem>();
                        OrderCountViewModel.BusinessPartnerList = new List<Microsoft.AspNetCore.Mvc.Rendering.SelectListItem>();
                        OrderCountViewModel.StateList = new List<SelectListItem>();
                        companyModel = _dashBoardManager.GetCompanyList();
                        sponsorlist = new SelectList(companyModel.BusinessUnitList, "Value", "Text");
                        ViewData["sponsorList"] = new SelectList(sponsorlist, "Value", "Text");
                        // ViewData["StateList"] = new SelectList(OrderCountViewModel.StateList, "Text", "Text");
                    }
                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("DealerDashboardModel", "OnGet", ex);
            }
            return Page();

        }

        public JsonResult OnGetCityList(string state, string AssociateCode,int buid,string userCompany,string userRole)
        {
            CityListModel citymodel = new CityListModel();
            IEnumerable<SelectListItem> cityList = null;
            citymodel.CityList = new List<SelectListItem>();
            try
            {
                citymodel = _dashBoardManager.GetCityList(state, buid, AssociateCode, userCompany, userRole);
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("DealerDashboardModel", "OngetCityList", ex);
            }
            //var result = new SelectList(citymodel.CityList, "Value", "Text");
            cityList = new SelectList(citymodel.CityList, "Value", "Text");
            ViewData["cityList"] = new SelectList(cityList, "Value", "Text");
            return new JsonResult(cityList);
        }

        public JsonResult OnGetStoreList(int buid, string AssociateCode, string City,string userCompany,string userRole)
        {
            StoreListModel storeModel = new StoreListModel();
            IEnumerable<SelectListItem> storeList = null;
            storeModel.StoreList = new List<SelectListItem>();
            try
            {
                storeModel = _dashBoardManager.GetStoreList(buid, AssociateCode, City, userCompany,userRole);
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("DealerDashboardModel", "OnGetStoreList", ex);
            }
            //var result = new SelectList(citymodel.CityList, "Value", "Text");
            storeList = new SelectList(storeModel.StoreList, "Value", "Text");
            ViewData["storeList"] = new SelectList(storeModel.StoreList, "Value", "Text");
            return new JsonResult(storeList);
        }

        public JsonResult OnGetOrderDataTable(ExchangeOrderDataContract exchageOrderObj)
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
                

                 dealerdatalist = _dashBoardManager.GetDashboardList(exchageOrderObj,skip, pageSize);
               
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("DealerDashboardModel", "OnGetStoreList", ex);
            }
            return new JsonResult(new { data = dealerdatalist });
        }

        #region Export Data To Excell
        public FileResult OnPostExportExcel_StoreData(OrderCountViewModel orderCountView)
        {
            List<DealerDashboardViewModel> dealerdatalist = new List<DealerDashboardViewModel>();
            dealerdatalist = _dashBoardManager.ExportDealerdata(OrderCountViewModel.AssociateCode);
            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(ListToDatatable.ToDataTable(dealerdatalist));
                using (MemoryStream stream = new MemoryStream())
                {


                    wb.SaveAs(stream);
                    return (File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "OrderDeatils.xlsx"));
                }
            }



        }
        #endregion

        #region Company List(Business Unit list)
        public JsonResult OnGetCompanyList()
        {
            CompanyList companyModel= new CompanyList();
            IEnumerable<SelectListItem> sponsorlist = null;
            companyModel.BusinessUnitList= new List<SelectListItem>(); 
            try
            {
                companyModel = _dashBoardManager.GetCompanyList();

            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("DealerDashboardModel", "OnGetCompanyList", ex);
            }
            sponsorlist = new SelectList(companyModel.BusinessUnitList, "Value", "Text");
            ViewData["sponsorList"] = new SelectList(sponsorlist, "Value", "Text");
            return new JsonResult(new { data = sponsorlist });
        }
        #endregion


        public JsonResult OnGetStateList(int buid,string UserCompanyName,string AssociateCode)
        {
            StateListModel statemodel = new StateListModel();
            IEnumerable<SelectListItem> stateList = null;
            statemodel.StateList = new List<SelectListItem>();
            try
            {
                if (buid >= 0)
                {
                    statemodel = _dashBoardManager.GetStateList(buid, AssociateCode, UserCompanyName);
                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("DealerDashboardModel", "OnGetStateList", ex);
            }
            //var result = new SelectList(citymodel.CityList, "Value", "Text");
            stateList = new SelectList(statemodel.StateList, "Value", "Text");
            ViewData["stateList"] = new SelectList(stateList, "Value", "Text");
            return new JsonResult(stateList);
        }

        public JsonResult OnGetBusinessUnitName(int buid)
        {
            StateListModel statemodel = new StateListModel();
            IEnumerable<SelectListItem> stateList = null;
            statemodel.StateList = new List<SelectListItem>();
            TblBusinessUnit businessUnit = null;
            try
            {
                if (buid > 0)
                {
                    businessUnit = _context.TblBusinessUnits.FirstOrDefault(x => x.BusinessUnitId == buid);
                    if (businessUnit != null)
                    {
                        businessunitObj.Name = businessUnit.Name;
                        businessunitObj.BusinessUnitId = businessUnit.BusinessUnitId;
                    }
                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("DealerDashboardModel", "OnGetStateList", ex);
            }
            //var result = new SelectList(citymodel.CityList, "Value", "Text");
            stateList = new SelectList(statemodel.StateList, "Value", "Text");
            ViewData["stateList"] = new SelectList(stateList, "Value", "Text");
            return new JsonResult(businessunitObj);
        }
    }
}
