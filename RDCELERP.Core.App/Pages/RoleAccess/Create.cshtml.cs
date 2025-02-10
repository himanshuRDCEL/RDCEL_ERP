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

namespace RDCELERP.Core.App.Pages.RoleAccess
{
    public class CreateModel : BasePageModel
    {
        private readonly RDCELERP.DAL.Entities.Digi2l_DevContext _context;

        public CreateModel(RDCELERP.DAL.Entities.Digi2l_DevContext context, IOptions<ApplicationSettings> config)
        : base(config)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
        ViewData["AccessListId"] = new SelectList(_context.TblAccessLists, "AccessListId", "AccessListId");
        ViewData["CompanyId"] = new SelectList(_context.TblCompanies, "CompanyId", "CompanyId");
        ViewData["CreatedBy"] = new SelectList(_context.TblUsers, "UserId", "UserId");
        ViewData["ModifiedBy"] = new SelectList(_context.TblUsers, "UserId", "UserId");
        ViewData["RoleId"] = new SelectList(_context.TblRoles, "RoleId", "RoleId");
            return Page();
        }

        [BindProperty]
        public TblRoleAccess TblRoleAccess { get; set; }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.TblRoleAccesses.Add(TblRoleAccess);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
