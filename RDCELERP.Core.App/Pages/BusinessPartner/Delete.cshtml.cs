using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using RDCELERP.Core.App.Pages.Base;
using RDCELERP.DAL.Entities;
using RDCELERP.Model.Base;

namespace RDCELERP.Core.App.Pages.BusinessPartner
{
    public class DeletBusinessPartnerById : CrudBasePageModel
    {
        private readonly RDCELERP.DAL.Entities.Digi2l_DevContext _context;

        public DeletBusinessPartnerById(RDCELERP.DAL.Entities.Digi2l_DevContext context, IOptions<ApplicationSettings> config, CustomDataProtection protector) : base(config, protector)
        {
            
            _context = context;
        }
        [BindProperty]
        public TblBusinessPartner TblBusinessPartner { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            TblBusinessPartner = await _context.TblBusinessPartners.FirstOrDefaultAsync(m => m.BusinessPartnerId == id);

            if (TblBusinessPartner == null)
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

            TblBusinessPartner = await _context.TblBusinessPartners.FindAsync(id);

            if (TblBusinessPartner != null)
            {
                _context.TblBusinessPartners.Remove(TblBusinessPartner);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
