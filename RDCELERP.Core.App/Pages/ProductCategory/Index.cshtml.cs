using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using RDCELERP.BAL.Interface;
using RDCELERP.BAL.MasterManager;
using RDCELERP.Core.App.Pages.Base;
using RDCELERP.DAL.Entities;
using RDCELERP.Model.Base;
using RDCELERP.Model.Master;
using RDCELERP.Model.Users;

namespace RDCELERP.Core.App.Pages.ProductCategory
{
    public class IndexModel : BasePageModel
    {

        private readonly RDCELERP.DAL.Entities.Digi2l_DevContext _context;
        private readonly IProductCategoryManager _ProductCategoryManager;

        public IndexModel(RDCELERP.DAL.Entities.Digi2l_DevContext context, IProductCategoryManager ProductCategoryManager, IOptions<ApplicationSettings> config)
        : base(config)
        {
            _context = context;
            _ProductCategoryManager = ProductCategoryManager;
        }
        [BindProperty(SupportsGet = true)]
        public IList<TblProductCategory> TblProductCategory { get; set; }
        [BindProperty(SupportsGet = true)]
        public TblProductCategory TblProductCategoryObj { get; set; }
        [BindProperty(SupportsGet = true)]
        public ProductCategoryViewModel ProductCategoryVM { get; set; }


        public IActionResult OnGet()
        {
            TblProductCategoryObj = new TblProductCategory();

            if (_loginSession == null)
            {
                return RedirectToPage("/index");
            }
            else
            {
                ProductCategoryVM = _ProductCategoryManager.GetProductCategoryById(_loginSession.UserViewModel.UserId);

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
                if (TblProductCategoryObj != null && TblProductCategoryObj.Id > 0)
                {
                    TblProductCategoryObj = _context.TblProductCategories.Find(TblProductCategoryObj.Id);
                }

                if (TblProductCategoryObj != null)
                {

                    if (TblProductCategoryObj.IsActive == true)
                    {
                        TblProductCategoryObj.IsActive = false;
                    }
                    else
                    {
                        TblProductCategoryObj.IsActive = true;
                    }
                    TblProductCategoryObj.ModifiedBy = _loginSession.UserViewModel.UserId;
                    _context.TblProductCategories.Update(TblProductCategoryObj);
                    //  _context.TblRoles.Remove(TblRole);
                    _context.SaveChanges();
                }

                return RedirectToPage("./index");
            }
        }

    }
}



