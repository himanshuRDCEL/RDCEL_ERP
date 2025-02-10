using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RDCELERP.DAL.Entities;
using Microsoft.Extensions.Options;
using RDCELERP.Model.Base;
using RDCELERP.Core.App.Pages.Base;

namespace RDCELERP.Core.App.Pages.RoleAccess
{
    public class DetailsModel : BasePageModel
    {
        private readonly RDCELERP.DAL.Entities.Digi2l_DevContext _context;

        public DetailsModel(RDCELERP.DAL.Entities.Digi2l_DevContext context, IOptions<ApplicationSettings> config)
        : base(config)
        {
            _context = context;
        }

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
            return Page();
        }
    }
}
