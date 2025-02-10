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

namespace RDCELERP.Core.App.Pages.Users
{
    public class DeleteUserRoleModel : BasePageModel
    {
        private readonly RDCELERP.DAL.Entities.Digi2l_DevContext _context;

        public DeleteUserRoleModel(RDCELERP.DAL.Entities.Digi2l_DevContext context, IOptions<ApplicationSettings> config)
        : base(config)
        {
            _context = context;
        }

        [BindProperty]
        public TblUserRole TblUserRole { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            TblUserRole = await _context.TblUserRoles
                .Include(t => t.CreatedByNavigation)
                .Include(t => t.User)
                .Include(t => t.Company)
                 .Include(t => t.Role)
                .Include(t => t.ModifiedByNavigation).FirstOrDefaultAsync(m => m.IsActive == true && m.UserRoleId == id);

            if (TblUserRole == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            TblUserRole = await _context.TblUserRoles.FindAsync(id);

            if (TblUserRole != null)
            {
                TblUserRole.IsActive = false;
                _context.TblUserRoles.Update(TblUserRole);
                //_context.TblUserRoles.Remove(TblUserRole);
                await _context.SaveChangesAsync();
            }
            return RedirectToPage("Details", new { id = TblUserRole.UserId });
            //return RedirectToPage("./Index");
        }
    }
}
