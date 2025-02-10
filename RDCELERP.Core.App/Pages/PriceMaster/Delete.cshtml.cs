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
    public class DeleteModel : PageModel
    {
        private readonly RDCELERP.DAL.Entities.Digi2l_DevContext _context;

        public DeleteModel(RDCELERP.DAL.Entities.Digi2l_DevContext context)
        {
            _context = context;
        }

        [BindProperty]
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

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            TblPriceMaster = await _context.TblPriceMasters.FindAsync(id);

            if (TblPriceMaster != null)
            {
                _context.TblPriceMasters.Remove(TblPriceMaster);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
