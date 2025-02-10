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
using RDCELERP.Model.Master;


namespace RDCELERP.Core.App.Pages.PriceMasterMapping
{
    public class IndexModel : CrudBasePageModel
    {
        private readonly RDCELERP.DAL.Entities.Digi2l_DevContext _context;
        private readonly IPriceMasterMappingManager _priceMasterMappingManager;

        public IndexModel(RDCELERP.DAL.Entities.Digi2l_DevContext context, IPriceMasterMappingManager priceMasterMappingManager, IOptions<ApplicationSettings> config, CustomDataProtection protector) : base(config, protector)
        {
            _context = context;
            _priceMasterMappingManager = priceMasterMappingManager;
        }

        [BindProperty(SupportsGet = true)]
        public PriceMasterMappingViewModel PriceMasterMappingVM { get; set; }
        [BindProperty(SupportsGet = true)]
        public IList<TblPriceMasterMapping> TblPriceMasterMapping { get; set; }
        [BindProperty(SupportsGet = true)]
        public TblPriceMasterMapping TblPriceMasterMappingObj { get; set; }
        public IActionResult OnGet()
        {
            TblPriceMasterMappingObj = new TblPriceMasterMapping();
           

            if (_loginSession == null)
            {
                return RedirectToPage("/index");
            }
            else
            {
                PriceMasterMappingVM = _priceMasterMappingManager.GetPriceMasterMappingById(_loginSession.UserViewModel.UserId);

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
                if (TblPriceMasterMappingObj != null && TblPriceMasterMappingObj.PriceMasterMappingId > 0)
                {
                    TblPriceMasterMappingObj = _context.TblPriceMasterMappings.Find(TblPriceMasterMappingObj.PriceMasterMappingId);
                }

                if (TblPriceMasterMappingObj != null)
                {
                    TblPriceMasterMappingObj.IsActive = false;
                    TblPriceMasterMappingObj.ModifiedBy = _loginSession.UserViewModel.UserId;
                    _context.TblPriceMasterMappings.Update(TblPriceMasterMappingObj);
                    //  _context.TblRoles.Remove(TblRole);
                    _context.SaveChanges();
                }

                return RedirectToPage("./index");
            }
        }
    }
}
