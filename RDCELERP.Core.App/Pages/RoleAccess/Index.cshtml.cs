using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RDCELERP.DAL.Entities;

using RDCELERP.Model.Base;
using RDCELERP.Core.App.Pages.Base;
using Microsoft.Extensions.Options;

namespace RDCELERP.Core.App.Pages.RoleAccess
{
    public class IndexModel : BasePageModel
    {
        private readonly RDCELERP.DAL.Entities.Digi2l_DevContext _context;

        public IndexModel(RDCELERP.DAL.Entities.Digi2l_DevContext context, IOptions<ApplicationSettings> config)
        : base(config)
        {
            _context = context;
        }

        public IList<TblRoleAccess> TblRoleAccess { get;set; }

        public async Task OnGetAsync()
        {
            TblRoleAccess = await _context.TblRoleAccesses
                .Include(t => t.AccessList)
                .Include(t => t.Company)
                .Include(t => t.CreatedByNavigation)
                .Include(t => t.ModifiedByNavigation)
                .Include(t => t.Role).ToListAsync();
        }
    }
}
