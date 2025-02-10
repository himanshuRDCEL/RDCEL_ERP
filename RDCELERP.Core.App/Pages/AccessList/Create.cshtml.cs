using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using RDCELERP.DAL.Entities;
using RDCELERP.Core.App.Pages.Base;
using Microsoft.Extensions.Options;
using RDCELERP.Model.Base;
using RDCELERP.BAL.Interface;
using RDCELERP.DAL.IRepository;
using RDCELERP.Common.Helper;

namespace RDCELERP.Core.App.Pages.AccessList
{
    public class CreateModel : BasePageModel
    {
        private readonly RDCELERP.DAL.Entities.Digi2l_DevContext _context;
        DateTime _currentDatetime = DateTime.Now.TrimMilliseconds();
        IRoleAccessRepository _roleAccessRepository;
        IUserManager _userManager;
        ICompanyManager _companyManager;


        public CreateModel(RDCELERP.DAL.Entities.Digi2l_DevContext context, ICompanyManager companyManager, IUserManager userManager, IRoleAccessRepository roleAccessRepository, IOptions<ApplicationSettings> config)
        : base(config)
        {
            _context = context;
            _roleAccessRepository = roleAccessRepository;
            _userManager = userManager;
            _companyManager = companyManager;
        }

        public IActionResult OnGet()
        {
            var Userlist = _userManager.GetAllUser();
            var Companylist = _companyManager.GetAllCompany(_loginSession.RoleViewModel.CompanyId, _loginSession.RoleViewModel.RoleId, _loginSession.UserViewModel.UserId);
            ViewData["Companylist"] = new SelectList((from s in Companylist.ToList()
                                                      select new
                                                      {
                                                          CompanyId = s.CompanyId,
                                                          CompanyName = s.CompanyId + "-" + s.CompanyName
                                                      }), "CompanyId", "CompanyName", null);

            ViewData["Userlist"] = new SelectList((from s in Userlist.ToList()
                                                   select new
                                                   {
                                                       UserId = s.UserId,
                                                       FullName = s.UserId + "-" + s.FirstName + " " + s.LastName
                                                   }), "UserId", "FullName", null);
            ViewData["ModifiedBy"] = new SelectList((from s in Userlist.ToList()
                                                     select new
                                                     {
                                                         UserId = s.UserId,
                                                         FullName = s.UserId + "-" + s.FirstName + " " + s.LastName
                                                     }), "UserId", "FullName", null);
            var ParentAccessList = _context.TblAccessLists;
            
            ViewData["ParentAccessList"] = new SelectList((from s in ParentAccessList.ToList()
                                                           select new
                                                           {
                                                               AccessListId = s.AccessListId,
                                                               Description = s.AccessListId + "-" + s.Description 
                                                           }), "AccessListId", "Description", null);




            return Page();
        }

        [BindProperty]
        public TblAccessList TblAccessList { get; set; }
        TblRoleAccess TblRoleAccess = new TblRoleAccess();

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.TblAccessLists.Add(TblAccessList);
            await _context.SaveChangesAsync();

            //Code to Add Super Admin Access

            TblRoleAccess.CreatedDate = _currentDatetime;
            TblRoleAccess.CreatedBy = TblAccessList.CreatedBy;
            TblRoleAccess.RoleId = 1;
            TblRoleAccess.CanAdd = true;
            TblRoleAccess.CanView = true;
            TblRoleAccess.CanDelete = true;
            TblRoleAccess.AccessListId = TblAccessList.AccessListId;
            TblRoleAccess.IsActive = true;
            TblRoleAccess.CompanyId = TblAccessList.CompanyId;
            _roleAccessRepository.Create(TblRoleAccess);
            _roleAccessRepository.SaveChanges();

            return RedirectToPage("./Index");

            
        }
    }
}
