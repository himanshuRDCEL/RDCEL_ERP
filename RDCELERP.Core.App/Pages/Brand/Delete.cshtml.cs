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

namespace RDCELERP.Core.App.Pages.Brands
{
    public class DeletBrandById : BasePageModel
    {
        private readonly RDCELERP.DAL.Entities.Digi2l_DevContext _context;

        public DeletBrandById(RDCELERP.DAL.Entities.Digi2l_DevContext context, IOptions<ApplicationSettings> config)
        : base(config)
        {
            _context = context;
        }

        
        [BindProperty]
        public TblBrand TblBrand { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            TblBrand = await _context.TblBrands.FirstOrDefaultAsync(m => m.Id == id);

            if (TblBrand == null)
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

            TblBrand = await _context.TblBrands.FindAsync(id);

            if (TblBrand != null)
            {
                _context.TblBrands.Remove(TblBrand);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }


    }
}
