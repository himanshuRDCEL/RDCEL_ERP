using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using RDCELERP.BAL.Interface;
using RDCELERP.Common.Helper;
using RDCELERP.Core.App.Pages.Base;
using RDCELERP.DAL.Entities;
using RDCELERP.DAL.IRepository;
using RDCELERP.Model.Base;
using RDCELERP.Model.DealerDashBoard;

namespace RDCELERP.Core.App.Pages.CompanyDashBoard
{
    public class ExchangeDashBoardModel : BasePageModel
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
        public ExchangeDashBoardModel(IOptions<ApplicationSettings> config, Digi2l_DevContext _dbcontext, CustomDataProtection _dataprotector, IExchangeOrderManager exchangeOrderManager, IDealerManager dealerDashBoardManager, ILogging logging, IBusinessPartnerRepository businessPartnerRepository, IExchangeOrderRepository exchangeOrderRepository) : base(config)
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
        [BindProperty(SupportsGet = true)]
        public string AssociateCodeforbp { get; set; } = string.Empty;
        [BindProperty(SupportsGet = true)]
        public TblCompany TblCompany { get; set; }

        [BindProperty(SupportsGet = true)]
        public TblBusinessUnit TblBusinessUnit { get; set; }

        [BindProperty(SupportsGet = true)]
        public List<TblBusinessPartner> TblBusinessPartners { get; set; }
        #endregion
        public IActionResult OnGet()
        {
            int companyId = 0;
            int BusinessUnitId = 0;
            string BusinessUnitName = null;
            string RoleName = null;
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
                    if (_loginSession.RoleViewModel.CompanyId > 0)
                    {
                        companyId = Convert.ToInt32(_loginSession.RoleViewModel.CompanyId);
                        RoleName = _loginSession.RoleViewModel.RoleName;

                        TblCompany = _context.TblCompanies.Include(x => x.BusinessUnit).FirstOrDefault(x => x.CompanyId == companyId);
                        if (TblCompany != null)
                        {
                            if (TblCompany.BusinessUnit != null)
                            {
                                BusinessUnitName = TblCompany.BusinessUnit.Name;
                                BusinessUnitId = TblCompany.BusinessUnit.BusinessUnitId;
                                OrderCountViewModel.BusinessUnitId = BusinessUnitId;
                            }

                        }

                    }
                    if (_loginSession.UserViewModel.CompanyId > 0)
                    {
                        TblCompany = _context.TblCompanies.FirstOrDefault(x => x.CompanyId == UserComapny);
                        if (TblCompany != null)
                        {
                            if (TblCompany.BusinessUnit != null)
                            {
                                UserComapnyName = TblCompany.BusinessUnit.Name;
                                BusinessUnitId = Convert.ToInt32(TblCompany.BusinessUnitId);
                                BusinessUnitName = TblCompany.BusinessUnit.Name;

                            }
                        }
                    }

                    if (userId <0)
                    {
                        return NotFound();
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
                        OrderCountViewModel = _dashBoardManager.GetOrderCountsForBU(BusinessUnitId);
                        OrderCountViewModel.UserEmail = TblUser.Email;
                        OrderCountViewModel.AssociateCode = TblUser.Password;
                        AssociateCodeforbp = TblUser.Password;
                        OrderCountViewModel.BusinessUnitId = BusinessUnitId;
                        OrderCountViewModel.UserCompanyName = UserComapnyName;
                        OrderCountViewModel.CompanyName = BusinessUnitName;
                        OrderCountViewModel.userRole = RoleName;
                       
                        
                        OrderCountViewModel.InProcessOrders = OrderCountViewModel.OderCount - (OrderCountViewModel.CancelledOrders + OrderCountViewModel.CompletedOrders);

                        companyModel = _dashBoardManager.GetCompanyList();
                        sponsorlist = new SelectList(companyModel.BusinessUnitList, "Value", "Text");
                        ViewData["sponsorList"] = new SelectList(sponsorlist, "Value", "Text");
                        // ViewData["StateList"] = new SelectList(OrderCountViewModel.StateList, "Text", "Text");
                    }
                    if (BusinessUnitId > 0)
                    {
                        TblBusinessPartners = _context.TblBusinessPartners.Where(x => x.BusinessUnitId == BusinessUnitId).ToList();
                    }
                }
            }
            catch(Exception ex)
            {
                _logging.WriteErrorToDB("CompanyDashBoardModel", "OnGet", ex);
            }
            return Page();
        }
    }
}
