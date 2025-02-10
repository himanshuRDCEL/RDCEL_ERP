using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using RDCELERP.BAL.Interface;
using RDCELERP.Core.App.Pages.Base;
using RDCELERP.DAL.Entities;
using RDCELERP.Model.Base;
using RDCELERP.Model.PriceMaster;

namespace RDCELERP.Core.App.Pages.PriceMaster
{
    public class IndexModel : CrudBasePageModel
    {
        private readonly RDCELERP.DAL.Entities.Digi2l_DevContext _context;
        private readonly IPriceMasterManager _priceMasterManager;

        public IndexModel(RDCELERP.DAL.Entities.Digi2l_DevContext context, IPriceMasterManager priceMasterManager, IOptions<ApplicationSettings> config, CustomDataProtection protector) : base(config, protector)
        {
            _context = context;
            _priceMasterManager = priceMasterManager;
        }

        [BindProperty(SupportsGet = true)]
        public PriceMasterViewModel PriceMasterVM { get; set; }

        [BindProperty(SupportsGet = true)]
        public IList<TblPriceMaster> TblPriceMaster { get; set; }
        [BindProperty(SupportsGet = true)]
        public TblPriceMaster TblPriceMasterObj { get; set; }
        public IActionResult OnGet()
        {
            TblPriceMasterObj = new TblPriceMaster();
            var BusinessUnit = _context.TblBusinessUnits.Where(x => x.IsActive == true);

            if (BusinessUnit != null)
            {
                ViewData["BusinessUnit"] = new SelectList(BusinessUnit, "BusinessUnitId", "Name");

            }

            if (_loginSession == null)
            {
                return RedirectToPage("/index");
            }
            else
            {
                PriceMasterVM = _priceMasterManager.GetPriceMasterById(_loginSession.UserViewModel.UserId);

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
                if (TblPriceMasterObj != null && TblPriceMasterObj.Id > 0)
                {
                    TblPriceMasterObj = _context.TblPriceMasters.Find(TblPriceMasterObj.Id);
                }

                if (TblPriceMasterObj != null)
                {
                    TblPriceMasterObj.IsActive = false;
                    TblPriceMasterObj.ModifiedBy = _loginSession.UserViewModel.UserId;
                    _context.TblPriceMasters.Update(TblPriceMasterObj);
                    //  _context.TblRoles.Remove(TblRole);
                    _context.SaveChanges();
                }

                return RedirectToPage("./index");
            }
        }
    }
}
