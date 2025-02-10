using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RDCELERP.DAL.Entities;
using RDCELERP.Core.App.Pages.Base;
using Microsoft.Extensions.Options;
using RDCELERP.Model.Base;
using RDCELERP.BAL.Interface;
using RDCELERP.DAL.IRepository;

namespace RDCELERP.Core.App.Pages.AccessList
{
    public class EditModel : BasePageModel
    {
        private readonly RDCELERP.DAL.Entities.Digi2l_DevContext _context;
        private readonly IAccessListRepository _AccessListRepository;
        IUserManager _userManager;
        ICompanyManager _companyManager;



        public EditModel(RDCELERP.DAL.Entities.Digi2l_DevContext context, IUserManager userManager, ICompanyManager companyManager, IAccessListRepository AccessListRepository, IOptions<ApplicationSettings> config)
        : base(config)
        {
            _context = context;
            _AccessListRepository = AccessListRepository;
            _userManager = userManager;
            _companyManager = companyManager;
        }

        [BindProperty]
        public TblAccessList TblAccessList { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            TblAccessList = await _context.TblAccessLists
                .Include(t => t.Company)
                .Include(t => t.CreatedByNavigation)
                .Include(t => t.ModifiedByNavigation)
                .Include(t => t.ParentAccessList).FirstOrDefaultAsync(m => m.AccessListId == id);

            if (TblAccessList == null)
            {
                return NotFound();
            }
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

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            //_context.Attach(TblAccessList).State = EntityState.Modified;
            _AccessListRepository.Update(TblAccessList);

            try
            {
                 _AccessListRepository.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TblAccessListExists(TblAccessList.AccessListId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool TblAccessListExists(int id)
        {
            return _context.TblAccessLists.Any(e => e.AccessListId == id);
        }
    }
}
