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

namespace RDCELERP.Core.App.Pages.ProductQualityIndex
{
    public class DeletProductQualityIndexById : BasePageModel
    {
        private readonly RDCELERP.DAL.Entities.Digi2l_DevContext _context;

        public DeletProductQualityIndexById(RDCELERP.DAL.Entities.Digi2l_DevContext context, IOptions<ApplicationSettings> config)
        : base(config)
        {
            _context = context;
        }

        [BindProperty]
        public TblProductQualityIndex TblProductQualityIndex { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            TblProductQualityIndex = await _context.TblProductQualityIndices
                .Include(t => t.ProductCategory).FirstOrDefaultAsync(m => m.ProductQualityIndexId == id);

            if (TblProductQualityIndex == null)
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

            TblProductQualityIndex = await _context.TblProductQualityIndices.FindAsync(id);

            if (TblProductQualityIndex != null)
            {
                _context.TblProductQualityIndices.Remove(TblProductQualityIndex);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
