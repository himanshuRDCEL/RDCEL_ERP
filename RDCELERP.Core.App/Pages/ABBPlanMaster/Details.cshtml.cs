using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RDCELERP.DAL.Entities;

namespace RDCELERP.Core.App.Pages.ABBPlanMaster
{
    public class DetailsModel : PageModel
    {
        private readonly RDCELERP.DAL.Entities.Digi2l_DevContext _context;

        public DetailsModel(RDCELERP.DAL.Entities.Digi2l_DevContext context)
        {
            _context = context;
        }

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
            return Page();
        }
    }
}
