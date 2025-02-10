using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RDCELERP.DAL.Entities;

namespace RDCELERP.Core.App.Pages.ServicePartner
{
    public class DetailsModel : PageModel
    {
        private readonly RDCELERP.DAL.Entities.Digi2l_DevContext _context;

        public DetailsModel(RDCELERP.DAL.Entities.Digi2l_DevContext context)
        {
            _context = context;
        }

        public TblServicePartner TblServicePartner { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            TblServicePartner = await _context.TblServicePartners
                .Include(t => t.User).FirstOrDefaultAsync(m => m.ServicePartnerId == id);

            if (TblServicePartner == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
