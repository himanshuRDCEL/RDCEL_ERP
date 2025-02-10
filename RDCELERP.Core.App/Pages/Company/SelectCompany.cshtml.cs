using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;
using RDCELERP.BAL.Interface;
using RDCELERP.Common.Constant;
using RDCELERP.Common.Enums;
using RDCELERP.Common.Helper;
using RDCELERP.Core.App.Helper;
using RDCELERP.Core.App.Pages.Base;
using RDCELERP.DAL.Entities;
using RDCELERP.Model.Base;
using RDCELERP.Model.Company;
using RDCELERP.Model.Users;

namespace RDCELERP.Core.App.Pages.Company
{
    public class SelectCompanyModel : BasePageModel
    {
        private readonly RDCELERP.DAL.Entities.Digi2l_DevContext _context;
        private readonly ICompanyManager _companyManager;
        private readonly IRoleManager _roleManager;

        public SelectCompanyModel(RDCELERP.DAL.Entities.Digi2l_DevContext context, ICompanyManager companyManager, IRoleManager roleManager, IOptions<ApplicationSettings> config)
        : base(config)
        {
            _context = context;
            _companyManager = companyManager;
            _roleManager = roleManager;
        }

        [BindProperty(SupportsGet = true)]
        public IList<CompanyViewModel> CompanyVM { get; set; }
        [BindProperty(SupportsGet = true)]
        public IList<TblCompany> TblCompany { get; set; }
        //new//
        [BindProperty(SupportsGet = true)]
        public TblCompany TblCompanyObj { get; set; }


        public IActionResult OnGet()
        {
            if (_loginSession == null)
            {
                return RedirectToPage("/index");
            }
            else
            {
                CompanyVM = _companyManager.GetAllCompany(_loginSession.RoleViewModel.CompanyId, _loginSession.RoleViewModel.RoleId, _loginSession.UserViewModel.UserId);
                if (CompanyVM != null && CompanyVM.Count == 1)
                {
                    TblCompanyObj.CompanyName = CompanyVM[0].CompanyName;
                    TblCompanyObj.CompanyId = CompanyVM[0].CompanyId;
                    TblCompanyObj.BusinessUnitId = CompanyVM[0].BusinessUnitId;
                    return OnPostAsync();
                }
                // TblCompany = _context.TblCompanies.Where(m => m.IsActive == true).OrderByDescending(x => x.CreatedDate).ToList();
                return Page();
            }
        }

        public IActionResult OnPostAsync()
       {
            LoginViewModel loginSession = _loginSession;
            if (ModelState.IsValid)
            {
                    
                if (loginSession.UserViewModel.UserId > 0 && TblCompanyObj.CompanyId > 0)
                {
                    loginSession.RoleViewModel.CompanyId = TblCompanyObj.CompanyId; //Pass the selected company values
                }
                 if(TblCompanyObj.BusinessUnitId>0 && loginSession.RoleViewModel.BusinessUnitViewModel == null)
                {
                    int BusinessUnitId = Convert.ToInt32(TblCompanyObj.BusinessUnitId);
                    loginSession.RoleViewModel.BusinessUnitViewModel=_companyManager.GetBusinessUnitByCompanyId(BusinessUnitId);
                }
                if(loginSession.RoleViewModel.BusinessUnitViewModel != null)
                {
                    //get role by selected company Id and BusinessUnitId
                    if (loginSession.RoleViewModel.CompanyId > 0 && loginSession.UserViewModel.UserId > 0 && loginSession.RoleViewModel.BusinessUnitViewModel != null)
                    {
                        loginSession.RoleViewModel = _roleManager.GetRoleByUserIdAndCompanyIdBUId(loginSession.UserViewModel.UserId, loginSession.RoleViewModel.CompanyId, loginSession.RoleViewModel.BusinessUnitViewModel.BusinessUnitId);
                    }
                }
                else
                {
                    //get role by selected company Id
                    if (loginSession.RoleViewModel.CompanyId > 0 && loginSession.UserViewModel.UserId > 0)
                    {
                        loginSession.RoleViewModel = _roleManager.GetRoleByUserIdAndCompanyId(loginSession.UserViewModel.UserId, loginSession.RoleViewModel.CompanyId);
                    }
                }

                if (loginSession.RoleViewModel.RoleName == RoleConstant.RoleServicePartner)
                {
                    loginSession.RoleViewModel.PortalName = PortalNameConstant.LGCPortal;
                    loginSession.RoleViewModel.PortalLink = PortalLinkConstant.LGCPortalLink;
                }
                else if (loginSession.RoleViewModel.RoleName == RoleConstant.RoleEVPPortal)
                {
                    loginSession.RoleViewModel.PortalName = PortalNameConstant.EVCPortal;
                    loginSession.RoleViewModel.PortalLink = PortalLinkConstant.EVCPortalLink;
                }
                else if (loginSession.RoleViewModel.RoleName == RoleConstant.RoleQC)
                {
                    loginSession.RoleViewModel.PortalName = PortalNameConstant.QCPortal;
                    loginSession.RoleViewModel.PortalLink = PortalLinkConstant.QCPortalLink;
                }
                else if (loginSession.RoleViewModel.RoleName == RoleConstant.RoleEVCAdmin)
                {
                    loginSession.RoleViewModel.PortalName = PortalNameConstant.EVCAdminPortal;
                    loginSession.RoleViewModel.PortalLink = PortalLinkConstant.EVCAdminPortalLink;
                }
                else if (loginSession.RoleViewModel.RoleName == RoleConstant.RoleSponsorAdmin)
                {
                    loginSession.RoleViewModel.PortalName = PortalNameConstant.SponsorAdminPortal;
                    loginSession.RoleViewModel.PortalLink = PortalLinkConstant.SponsorAdminPortalLink;
                }
                else if (loginSession.RoleViewModel.RoleName == RoleConstant.RoleDealerAdmin)
                {
                    loginSession.RoleViewModel.PortalName = PortalNameConstant.DealerAdminPortal;
                    loginSession.RoleViewModel.PortalLink = PortalLinkConstant.DealerAdminPortalLink;
                }
                else if ((TblCompanyObj.CompanyName != null && TblCompanyObj.CompanyName == EnumHelper.DescriptionAttr(CompanyNameenum.UTC))
                    || loginSession.RoleViewModel.RoleName == RoleConstant.RoleSuperAdmin)
                {
                    loginSession.RoleViewModel.PortalName = PortalNameConstant.UTCAdminPortal;
                    loginSession.RoleViewModel.PortalLink = PortalLinkConstant.UTCAdminPortalLink;
                }
                SessionHelper.SetObjectAsJson(HttpContext.Session, "LoginUser", loginSession);
                if (loginSession.RoleViewModel.PortalLink != null)
                {
                    return RedirectToPage(loginSession.RoleViewModel.PortalLink);
                }
                else
                {
                    return new RedirectToPageResult("/Dashboard");
                }
            }
            if (loginSession.RoleViewModel.CompanyId > 0 && loginSession.UserViewModel.UserId > 0 && loginSession.RoleViewModel.CompanyId > 0)
                return RedirectToPage("/Dashboard");

            else
                return RedirectToPage("SelectCompany");
        }
    }
}
