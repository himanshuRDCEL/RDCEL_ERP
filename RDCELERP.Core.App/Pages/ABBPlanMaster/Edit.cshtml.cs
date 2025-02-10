using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RDCELERP.DAL.Entities;

namespace RDCELERP.Core.App.Pages.ABBPlanMaster
{
    public class EditModel : PageModel
    {
        private readonly RDCELERP.DAL.Entities.Digi2l_DevContext _context;

        public EditModel(RDCELERP.DAL.Entities.Digi2l_DevContext context)
        {
            _context = context;
        }

        [BindProperty]
        public TblAbbplanMaster TblAbbplanMaster { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            TblAbbplanMaster = await _context.TblAbbplanMasters
                .Include(t => t.BusinessUnit)
                .Include(t => t.ProductCat)
                .Include(t => t.ProductType).FirstOrDefaultAsync(m => m.PlanMasterId == id);

            if (TblAbbplanMaster == null)
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

            _context.Attach(TblAbbplanMaster).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TblAbbplanMasterExists(TblAbbplanMaster.PlanMasterId))
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

        private bool TblAbbplanMasterExists(int id)
        {
            return _context.TblAbbplanMasters.Any(e => e.PlanMasterId == id);
        }
    }
}
