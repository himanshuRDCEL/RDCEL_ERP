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
using RDCELERP.BAL.Interface;

namespace RDCELERP.Core.App.Pages.AccessList
{
    public class IndexModel : BasePageModel
    {
        private readonly RDCELERP.DAL.Entities.Digi2l_DevContext _context;

        public IndexModel(RDCELERP.DAL.Entities.Digi2l_DevContext context, IOptions<ApplicationSettings> config)
        : base(config)
        {
            _context = context;
        }

        [BindProperty(SupportsGet = true)]
        public IList<TblAccessList> TblAccessList { get; set; }
        [BindProperty(SupportsGet = true)]
        public TblAccessList TblAccessListObj { get; set; }

        public async Task OnGetAsync()
        {
            TblAccessList = await _context.TblAccessLists
                .Include(t => t.Company)
                .Include(t => t.CreatedByNavigation)
                .Include(t => t.ModifiedByNavigation)
                .Include(t => t.ParentAccessList).OrderByDescending(t=>t.AccessListId).ToListAsync();
        }

        public IActionResult OnPostDeleteAsync()
        {
            if (_loginSession == null)
            {
                return RedirectToPage("/index");
            }
            else
            {
                if (TblAccessListObj != null && TblAccessListObj.AccessListId > 0)
                {
                    TblAccessListObj = _context.TblAccessLists.Find(TblAccessListObj.AccessListId);
                }

                if (TblAccessListObj != null)
                {
                    TblAccessListObj.IsActive = false;
                    TblAccessListObj.ModifiedBy = _loginSession.UserViewModel.UserId;
                    _context.TblAccessLists.Update(TblAccessListObj);
                    //  _context.TblRoles.Remove(TblRole);
                    _context.SaveChanges();
                }

                return RedirectToPage("./index");
            }
        }
    }
}

