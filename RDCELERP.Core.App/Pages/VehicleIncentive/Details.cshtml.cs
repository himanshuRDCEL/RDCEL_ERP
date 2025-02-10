using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RDCELERP.DAL.Entities;

namespace RDCELERP.Core.App.Pages.VehicleIncentive
{
    public class DetailsModel : PageModel
    {
        private readonly RDCELERP.DAL.Entities.Digi2l_DevContext _context;

        public DetailsModel(RDCELERP.DAL.Entities.Digi2l_DevContext context)
        {
            _context = context;
        }

        public TblVehicleIncentive TblVehicleIncentive { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            TblVehicleIncentive = await _context.TblVehicleIncentives
                .Include(t => t.BusinessUnit)
                .Include(t => t.CreatedByNavigation)
                .Include(t => t.ModifiedByNavigation)
                .Include(t => t.ProductCategory)
                .Include(t => t.ProductType).FirstOrDefaultAsync(m => m.IncentiveId == id);

            if (TblVehicleIncentive == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
