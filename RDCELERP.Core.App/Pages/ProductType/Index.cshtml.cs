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
using RDCELERP.Model.Product;
using RDCELERP.Model.Role;

namespace RDCELERP.Core.App.Pages.ProductType
{
    public class IndexModel : CrudBasePageModel
    {

        private readonly RDCELERP.DAL.Entities.Digi2l_DevContext _context;
        private readonly IProductTypeManager _productTypeManager;

        public IndexModel(RDCELERP.DAL.Entities.Digi2l_DevContext context, IProductTypeManager productTypeManager, IOptions<ApplicationSettings> config, CustomDataProtection protector) : base(config, protector)

        {
            _context = context;
            _productTypeManager = productTypeManager;
        }



        [BindProperty(SupportsGet = true)]
        public ProductTypeViewModel ProductTypeVM { get; set; }

        [BindProperty(SupportsGet = true)]
        public IList<TblProductCategory> TblProductCategory { get; set; }
        [BindProperty(SupportsGet = true)]
        public TblProductType TblProductTypeObj { get; set; }

        public IActionResult OnGet()
        {
            TblProductTypeObj = new TblProductType();

            if (_loginSession == null)
            {
                return RedirectToPage("/index");
            }
            else
            {
                ProductTypeVM = _productTypeManager.GetProductTypeById(_loginSession.UserViewModel.UserId);

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
                if (TblProductTypeObj != null && TblProductTypeObj.Id > 0)
                {
                    TblProductTypeObj = _context.TblProductTypes.Find(TblProductTypeObj.Id);
                }

                if (TblProductTypeObj != null)
                {
                    
                    if (TblProductTypeObj.IsActive == true)
                    {
                        TblProductTypeObj.IsActive = false;
                    }
                    else
                    { TblProductTypeObj.IsActive = true; 
                    }
                   
                    TblProductTypeObj.ModifiedBy = _loginSession.UserViewModel.UserId;
                    _context.TblProductTypes.Update(TblProductTypeObj);
                    //  _context.TblRoles.Remove(TblRole);
                    _context.SaveChanges();
                }

                return RedirectToPage("./index");
            }
        }

    }
}




