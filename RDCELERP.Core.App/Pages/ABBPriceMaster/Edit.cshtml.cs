using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RDCELERP.DAL.Entities;

namespace RDCELERP.Core.App.Pages.ABBPriceMaster
{
    public class EditModel : PageModel
    {
        private readonly RDCELERP.DAL.Entities.Digi2l_DevContext _context;

        public EditModel(RDCELERP.DAL.Entities.Digi2l_DevContext context)
        {
            _context = context;
        }

        [BindProperty]
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
           ViewData["BusinessUnitId"] = new SelectList(_context.TblBusinessUnits, "BusinessUnitId", "BusinessUnitId");
           ViewData["ProductCatId"] = new SelectList(_context.TblProductCategories, "Id", "Id");
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

            _context.Attach(TblAbbpriceMaster).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TblAbbpriceMasterExists(TblAbbpriceMaster.PriceMasterId))
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

        private bool TblAbbpriceMasterExists(int id)
        {
            return _context.TblAbbpriceMasters.Any(e => e.PriceMasterId == id);
        }
    }
}
