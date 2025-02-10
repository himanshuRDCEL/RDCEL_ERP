using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RDCELERP.DAL.Entities;
using RDCELERP.Core.App.Pages.Base;
using RDCELERP.Model.Base;
using Microsoft.Extensions.Options;
using RDCELERP.BAL.Interface;

namespace RDCELERP.Core.App.Pages.AccessList
{
    public class DetailsModel : BasePageModel
    {
        private readonly RDCELERP.DAL.Entities.Digi2l_DevContext _context;

        public DetailsModel(RDCELERP.DAL.Entities.Digi2l_DevContext context, IOptions<ApplicationSettings> config)
        : base(config)
        {
            _context = context;
        }

        public TblAccessList TblAccessList { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            TblAccessList = await _context.TblAccessLists
                .Include(t => t.Company)
                .Include(t => t.CreatedByNavigation)
                .Include(t => t.ModifiedByNavigation)
                .Include(t => t.ParentAccessList).FirstOrDefaultAsync(m => m.AccessListId == id);

            if (TblAccessList == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
