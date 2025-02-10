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

namespace RDCELERP.Core.App.Pages.RoleAccess
{
    public class EditModel : BasePageModel
    {
        private readonly RDCELERP.DAL.Entities.Digi2l_DevContext _context;

        public EditModel(RDCELERP.DAL.Entities.Digi2l_DevContext context, IOptions<ApplicationSettings> config)
        : base(config)
        {
            _context = context;
        }

        [BindProperty]
        public TblRoleAccess TblRoleAccess { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            TblRoleAccess = await _context.TblRoleAccesses
                .Include(t => t.AccessList)
                .Include(t => t.Company)
                .Include(t => t.CreatedByNavigation)
                .Include(t => t.ModifiedByNavigation)
                .Include(t => t.Role).FirstOrDefaultAsync(m => m.RoleAccessId == id);

            if (TblRoleAccess == null)
            {
                return NotFound();
            }
           ViewData["AccessListId"] = new SelectList(_context.TblAccessLists, "AccessListId", "AccessListId");
           ViewData["CompanyId"] = new SelectList(_context.TblCompanies, "CompanyId", "CompanyId");
           ViewData["CreatedBy"] = new SelectList(_context.TblUsers, "UserId", "UserId");
           ViewData["ModifiedBy"] = new SelectList(_context.TblUsers, "UserId", "UserId");
           ViewData["RoleId"] = new SelectList(_context.TblRoles, "RoleId", "RoleId");
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

            _context.Attach(TblRoleAccess).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TblRoleAccessExists(TblRoleAccess.RoleAccessId))
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

        private bool TblRoleAccessExists(int id)
        {
            return _context.TblRoleAccesses.Any(e => e.RoleAccessId == id);
        }
    }
}
