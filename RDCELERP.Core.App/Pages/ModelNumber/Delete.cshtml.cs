using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RDCELERP.DAL.Entities;

namespace RDCELERP.Core.App.Pages.ModelNumber
{
    public class DeleteModel : PageModel
    {
        private readonly RDCELERP.DAL.Entities.Digi2l_DevContext _context;

        public DeleteModel(RDCELERP.DAL.Entities.Digi2l_DevContext context)
        {
            _context = context;
        }

        [BindProperty]
        public TblModelNumber TblModelNumber { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            TblModelNumber = await _context.TblModelNumbers
                .Include(t => t.Brand)
                .Include(t => t.BusinessUnit)
                .Include(t => t.ProductCategory)
                .Include(t => t.ProductType).FirstOrDefaultAsync(m => m.ModelNumberId == id);

            if (TblModelNumber == null)
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

            TblModelNumber = await _context.TblModelNumbers.FindAsync(id);

            if (TblModelNumber != null)
            {
                _context.TblModelNumbers.Remove(TblModelNumber);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
