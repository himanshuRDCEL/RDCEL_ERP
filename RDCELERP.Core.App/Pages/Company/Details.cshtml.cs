using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RDCELERP.DAL.Entities;
using RDCELERP.Core.App.Pages.Base;
using Microsoft.Extensions.Options;
using RDCELERP.Model.Base;

namespace RDCELERP.Core.App.Pages.Company
{
    public class DetailsModel : BasePageModel
    {
        private readonly RDCELERP.DAL.Entities.Digi2l_DevContext _context;

        public DetailsModel(RDCELERP.DAL.Entities.Digi2l_DevContext context, IOptions<ApplicationSettings> config)
: base(config)
        {
            _context = context;
        }

        public TblCompany TblCompany { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            
            if (_loginSession == null)
            {
                return RedirectToPage("/index");
            }
            else
            {
                if (id == null)
                {
                    return NotFound();
                }

                TblCompany = await _context.TblCompanies
                    .Include(t => t.CreatedByNavigation)
                    .Include(t => t.ModifiedByNavigation).FirstOrDefaultAsync(m => m.CompanyId == id && m.IsActive == true);

                if (TblCompany == null)
                {
                    return NotFound();
                }
                return Page();
            }
        }
    }
}
