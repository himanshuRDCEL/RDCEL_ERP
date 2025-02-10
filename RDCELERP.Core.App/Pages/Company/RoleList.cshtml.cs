using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RDCELERP.DAL.Entities;
using RDCELERP.Core.App.Pages.Base;
using Microsoft.Extensions.Options;
using RDCELERP.Model.Base;
using RDCELERP.Model.Role;
using RDCELERP.BAL.Interface;

namespace RDCELERP.Core.App.Pages.Company
{
    public class RoleListModel : BasePageModel
    {
        private readonly RDCELERP.DAL.Entities.Digi2l_DevContext _context;
        private readonly IRoleManager _roleManager;

        public RoleListModel(RDCELERP.DAL.Entities.Digi2l_DevContext context, IRoleManager roleManager, IOptions<ApplicationSettings> config)
        : base(config)
        {
            _context = context;
            _roleManager = roleManager;
        }

        [BindProperty(SupportsGet = true)]
        public IList<RoleViewModel> RoleVM { get; set; }
        [BindProperty(SupportsGet = true)]
        public IList<TblCompany> TblCompany { get; set; }
        [BindProperty(SupportsGet = true)]
        public TblRole TblRoleObj { get; set; }

        // public IList<TblRole> TblRoleList { get; set; }
        // public IList<TblUserRole> TblUserRole { get; set; }

        public IActionResult OnGet()
        {
            TblRoleObj = new TblRole();

            if (_loginSession == null)
            {
                return RedirectToPage("/index");
            }
            else
            {
                RoleVM = _roleManager.GetRoleListByUserId(_loginSession.RoleViewModel.CompanyId, _loginSession.RoleViewModel.RoleId, _loginSession.UserViewModel.UserId);
                if (RoleVM != null && RoleVM.Count > 0)
                    TblCompany = _context.TblCompanies.Where(x => x.IsActive == true).ToList();

                return Page();
            }
        }

        public IActionResult OnPostDeleteAsync()
        {
            if (_loginSession == null)
            {
                return RedirectToPage("/index");
            }
            else
            {
                if (TblRoleObj != null && TblRoleObj.RoleId > 0)
                {
                    TblRoleObj = _context.TblRoles.Find(TblRoleObj.RoleId);
                }

                if (TblRoleObj != null)
                {
                    TblRoleObj.IsActive = false;
                    TblRoleObj.ModifiedBy = _loginSession.UserViewModel.UserId;
                    _context.TblRoles.Update(TblRoleObj);
                    //  _context.TblRoles.Remove(TblRole);
                    _context.SaveChanges();
                }

                return RedirectToPage("./RoleList");
            }
        }

    }
}
