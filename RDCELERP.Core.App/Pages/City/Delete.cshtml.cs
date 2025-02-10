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

namespace RDCELERP.Core.App.Pages.City
{
    public class DeletCityById : BasePageModel
    {
        private readonly RDCELERP.DAL.Entities.Digi2l_DevContext _context;

        public DeletCityById(RDCELERP.DAL.Entities.Digi2l_DevContext context, IOptions<ApplicationSettings> config)
        : base(config)
        {
            _context = context;
        }

        [BindProperty]
        public TblCity TblCity { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            TblCity = await _context.TblCities
                .Include(t => t.State).FirstOrDefaultAsync(m => m.CityId == id);

            if (TblCity == null)
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

            TblCity = await _context.TblCities.FindAsync(id);

            if (TblCity != null)
            {
                _context.TblCities.Remove(TblCity);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
