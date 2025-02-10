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
using RDCELERP.Model.Company;

namespace RDCELERP.Core.App.Pages.Brands
{
    public class IndexModel : CrudBasePageModel
    {
        private readonly RDCELERP.DAL.Entities.Digi2l_DevContext _context;
        private readonly IBrandManager _brandManager;

        public IndexModel(RDCELERP.DAL.Entities.Digi2l_DevContext context, IBrandManager brandManager, IOptions<ApplicationSettings> config, CustomDataProtection protector) : base(config, protector)
        {
            _context = context;
            _brandManager = brandManager;
        }

        [BindProperty(SupportsGet = true)]
        public BrandViewModel BrandVM { get; set; }

        [BindProperty(SupportsGet = true)]
        public IList<TblBrand> TblBrand { get; set; }
        [BindProperty(SupportsGet = true)]
        public TblBrand TblBrandObj { get; set; }
        public IActionResult OnGet()
        {
            TblBrandObj = new TblBrand();

            if (_loginSession == null)
            {
                return RedirectToPage("/index");
            }
            else
            {
                BrandVM = _brandManager.GetBrandById(_loginSession.UserViewModel.UserId);

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
                if (TblBrandObj != null && TblBrandObj.Id > 0)
                {
                    TblBrandObj = _context.TblBrands.Find(TblBrandObj.Id);
                }

                if (TblBrandObj != null)
                {
                    if(TblBrandObj.IsActive == true)
                    {
                        TblBrandObj.IsActive = false;
                    }
                    else
                    {
                        TblBrandObj.IsActive = true;
                    }
                    
                    TblBrandObj.ModifiedBy = _loginSession.UserViewModel.UserId;
                    _context.TblBrands.Update(TblBrandObj);
                    //  _context.TblRoles.Remove(TblRole);
                    _context.SaveChanges();
                }

                return RedirectToPage("./index");
            }
        }

    }
}