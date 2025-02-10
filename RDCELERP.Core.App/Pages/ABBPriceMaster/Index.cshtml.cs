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
using RDCELERP.Model.ABBPriceMaster;
using RDCELERP.Model.Base;

namespace RDCELERP.Core.App.Pages.ABBPriceMaster
{
    public class IndexModel : CrudBasePageModel
    {
        private readonly RDCELERP.DAL.Entities.Digi2l_DevContext _context;
        private readonly IABBPriceMasterManager _ABBPriceMasterManager;

        public IndexModel(RDCELERP.DAL.Entities.Digi2l_DevContext context, IABBPriceMasterManager ABBPriceMasterManager, IOptions<ApplicationSettings> config, CustomDataProtection protector) : base(config, protector)
        {
            _context = context;
            _ABBPriceMasterManager = ABBPriceMasterManager;
        }

        [BindProperty(SupportsGet = true)]
        public ABBPriceMasterViewModel ABBPriceMasterVM { get; set; }

        [BindProperty(SupportsGet = true)]
        public IList<TblAbbpriceMaster> TblAbbpriceMaster { get; set; }
        [BindProperty(SupportsGet = true)]
        public TblAbbpriceMaster TblAbbpriceMasterObj { get; set; }
        public IActionResult OnGet()
        {
            TblAbbpriceMasterObj = new TblAbbpriceMaster();

            if (_loginSession == null)
            {
                return RedirectToPage("/index");
            }
            else
            {

                ABBPriceMasterVM = _ABBPriceMasterManager.GetABBPriceMasterById(_loginSession.UserViewModel.UserId);

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
                if (TblAbbpriceMasterObj != null && TblAbbpriceMasterObj.PriceMasterId > 0)
                {
                    TblAbbpriceMasterObj = _context.TblAbbpriceMasters.Find(TblAbbpriceMasterObj.PriceMasterId);
                }

                if (TblAbbpriceMasterObj != null)
                {


                    TblAbbpriceMasterObj.IsActive = false;
                    TblAbbpriceMasterObj.ModifiedBy = _loginSession.UserViewModel.UserId;
                    _context.TblAbbpriceMasters.Update(TblAbbpriceMasterObj);
                    //  _context.TblRoles.Remove(TblRole);
                    _context.SaveChanges();
                }

                return RedirectToPage("./index");
            }
        }
    }
}

