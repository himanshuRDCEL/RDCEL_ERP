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

namespace RDCELERP.Core.App.Pages.ProductType
{
    public class DeletProductTypeById : BasePageModel
    {
        
        private readonly RDCELERP.DAL.Entities.Digi2l_DevContext _context;

        public DeletProductTypeById(RDCELERP.DAL.Entities.Digi2l_DevContext context, IOptions<ApplicationSettings> config)
        : base(config)
        {
            _context = context;
        }

        [BindProperty]
        public TblProductType TblProductType { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            TblProductType = await _context.TblProductTypes
                .Include(t => t.ProductCat).FirstOrDefaultAsync(m => m.Id == id);

            if (TblProductType == null)
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

            TblProductType = await _context.TblProductTypes.FindAsync(id);

            if (TblProductType != null)
            {
                _context.TblProductTypes.Remove(TblProductType);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
