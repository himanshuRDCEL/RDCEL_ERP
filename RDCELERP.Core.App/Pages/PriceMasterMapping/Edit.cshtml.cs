using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RDCELERP.DAL.Entities;

namespace RDCELERP.Core.App.Pages.PriceMasterMapping
{
    public class EditModel : PageModel
    {
        private readonly RDCELERP.DAL.Entities.Digi2l_DevContext _context;

        public EditModel(RDCELERP.DAL.Entities.Digi2l_DevContext context)
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
           ViewData["ProductCategoryId"] = new SelectList(_context.TblProductCategories, "Id", "Id");
           ViewData["ProductTypeId"] = new SelectList(_context.TblProductTypes, "Id", "Id");
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

            _context.Attach(TblPriceMaster).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TblPriceMasterExists(TblPriceMaster.Id))
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

        private bool TblPriceMasterExists(int id)
        {
            return _context.TblPriceMasters.Any(e => e.Id == id);
        }
    }
}
