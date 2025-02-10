using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using RDCELERP.BAL.Interface;
using RDCELERP.Common.Helper;
using RDCELERP.Core.App.Pages.Base;
using RDCELERP.DAL.Entities;
using RDCELERP.DAL.IRepository;
using RDCELERP.Model.ABBDashBoardModel;
using RDCELERP.Model.Base;
using RDCELERP.Model.DealerDashBoard;

namespace RDCELERP.Core.App.Pages.CompanyDashBoard
{
    public class ABBDashBoardModel : BasePageModel
    {

        #region variable declaration
        private readonly Digi2l_DevContext _context;
        public readonly IOptions<ApplicationSettings> _config;
        private CustomDataProtection _protector;
        private readonly IExchangeOrderManager _exchangeOrderManager;
        private readonly IDealerManager _dashBoardManager;
        IExchangeOrderRepository _exchangeOrderRepository;
        IBusinessPartnerRepository _businessPartnerRepository;
        ICompanyRepository companyRepository;
        private readonly IABBSponsorManager _abbSponsorManager;
        public ILogging _logging;
        #endregion
        #region Constructor
        public ABBDashBoardModel(IOptions<ApplicationSettings> config, Digi2l_DevContext _dbcontext, CustomDataProtection _dataprotector, IExchangeOrderManager exchangeOrderManager, IDealerManager dealerDashBoardManager, ILogging logging, IBusinessPartnerRepository businessPartnerRepository, IExchangeOrderRepository exchangeOrderRepository, ICompanyRepository _companyRepository, IABBSponsorManager abbsponsorManager) : base(config)
        {
            _config = config;
            _context = _dbcontext;
            _protector = _dataprotector;
            _exchangeOrderManager = exchangeOrderManager;
            _dashBoardManager = dealerDashBoardManager;
            _logging = logging;
            _businessPartnerRepository = businessPartnerRepository;
            _exchangeOrderRepository = exchangeOrderRepository;
            companyRepository = _companyRepository;
            _abbSponsorManager = abbsponsorManager;
        }
        #endregion
        #region Add Models to bind
        [BindProperty(SupportsGet = true)]
        public TblCompany TblCompany { get; set; }
        [BindProperty(SupportsGet = true)]
        public ABBDashBoardCountModel ABBDetailsDC { get; set; }
        [BindProperty(SupportsGet = true)]
        public UserDetailsForABBDashBoard userDetails { get; set; }
        #endregion
        public IActionResult OnGet()
        {
            int companyId = 0;
            string RoleName = null;
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
                        
                        TblCompany = companyRepository.GetCompanyIdPairedtoBU(companyId);
                        if (TblCompany != null)
                        {
                            if (TblCompany.BusinessUnit != null)
                            {
                                userDetails.BusinessUnitId=Convert.ToInt32(TblCompany.BusinessUnitId);
                                userDetails.BusinessUnitName =TblCompany.BusinessUnit.Name;
                            }
                        }
                    }
                    if (_loginSession.UserViewModel.CompanyId > 0)
                    {
                        TblCompany = companyRepository.GetCompanyId(_loginSession.UserViewModel.CompanyId);
                        if (TblCompany != null)
                        {
                            userDetails.UserComapnyId =Convert.ToInt32( _loginSession.UserViewModel.CompanyId);
                            userDetails.UserComapny = TblCompany.CompanyName;
                        }
                        
                    }
                    ABBDetailsDC = _abbSponsorManager.GetOrderCounts(userDetails);
                    ABBDetailsDC.BusinessUnitId = userDetails.BusinessUnitId;
                    ABBDetailsDC.BusinessUnitName = userDetails.BusinessUnitName;
                    if (userId == 0)
                    {
                        return NotFound();
                    }
                }
            }
            catch(Exception ex)
            {
                _logging.WriteErrorToDB("ABBDashBoardModel", "OnGet", ex);
            }
            return Page();
        }
    }
}
