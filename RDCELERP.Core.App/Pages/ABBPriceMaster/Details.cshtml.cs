using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RDCELERP.DAL.Entities;

namespace RDCELERP.Core.App.Pages.ABBPriceMaster
{
    public class DetailsModel : PageModel
    {
        private readonly RDCELERP.DAL.Entities.Digi2l_DevContext _context;

        public DetailsModel(RDCELERP.DAL.Entities.Digi2l_DevContext context)
        {
            _context = context;
        }

        public TblAbbpriceMaster TblAbbpriceMaster { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            TblAbbpriceMaster = await _context.TblAbbpriceMasters
                .Include(t => t.BusinessUnit)
                .Include(t => t.ProductCat)
                .Include(t => t.ProductType).FirstOrDefaultAsync(m => m.PriceMasterId == id);

            if (TblAbbpriceMaster == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
