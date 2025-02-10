using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RDCELERP.DAL.Entities;

namespace RDCELERP.Core.App.Pages.PriceMaster
{
    public class DetailsModel : PageModel
    {
        private readonly RDCELERP.DAL.Entities.Digi2l_DevContext _context;

        public DetailsModel(RDCELERP.DAL.Entities.Digi2l_DevContext context)
        {
            _context = context;
        }

        public TblPriceMaster TblPriceMaster { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            TblPriceMaster = await _context.TblPriceMasters
                .Include(t => t.ProductCategory)
                .Include(t => t.ProductTypeNavigation).FirstOrDefaultAsync(m => m.Id == id);

            if (TblPriceMaster == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
