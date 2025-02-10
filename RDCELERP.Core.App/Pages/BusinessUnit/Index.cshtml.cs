using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using RDCELERP.BAL.Interface;
using RDCELERP.Core.App.Pages.Base;
using RDCELERP.DAL.Entities;
using RDCELERP.Model.Base;
using RDCELERP.Model.BusinessUnit;

namespace RDCELERP.Core.App.Pages.BusinessUnit
{
    public class IndexModel : CrudBasePageModel
    {
        private readonly RDCELERP.DAL.Entities.Digi2l_DevContext _context;
        private readonly IBusinessUnitManager _BusinessUnitManager;

        public IndexModel(RDCELERP.DAL.Entities.Digi2l_DevContext context, IBusinessUnitManager BusinessUnitManager, IOptions<ApplicationSettings> config, CustomDataProtection protector) : base(config, protector)
        {
            _context = context;
            _BusinessUnitManager = BusinessUnitManager;
        }

        [BindProperty(SupportsGet = true)]
        public BusinessUnitViewModel BusinessUnitVM { get; set; }

        [BindProperty(SupportsGet = true)]
        public IList<TblBusinessUnit> TblBusinessUnit { get; set; }
        [BindProperty(SupportsGet = true)]
        public TblBusinessUnit TblBusinessUnitObj { get; set; }
        public IActionResult OnGet()
        {
            TblBusinessUnitObj = new TblBusinessUnit();

            if (_loginSession == null)
            {
                return RedirectToPage("/index");
            }
            else
            {
                BusinessUnitVM = _BusinessUnitManager.GetBusinessUnitById(_loginSession.UserViewModel.UserId);

                return Page();
            }
        }

        public IActionResult OnPostDeleteAsync()
        {
            if (_loginSession == null)
            {
                return RedirectToPage("/index");
            }
            else
            {
                if (TblBusinessUnitObj != null && TblBusinessUnitObj.BusinessUnitId > 0)
                {
                    TblBusinessUnitObj = _context.TblBusinessUnits.Find(TblBusinessUnitObj.BusinessUnitId);
                }

                if (TblBusinessUnitObj != null)
                {
                    TblBusinessUnitObj.IsActive = false;
                    TblBusinessUnitObj.ModifiedBy = _loginSession.UserViewModel.UserId;
                    _context.TblBusinessUnits.Update(TblBusinessUnitObj);
                    //  _context.TblRoles.Remove(TblRole);
                    _context.SaveChanges();
                }

                return RedirectToPage("./index");
            }
        }
    }
}
