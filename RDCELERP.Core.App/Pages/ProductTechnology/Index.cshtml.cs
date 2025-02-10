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
using RDCELERP.Model.ProductTechnology;


namespace RDCELERP.Core.App.Pages.ProductTechnology
{
    public class IndexModel : CrudBasePageModel
    {
        private readonly RDCELERP.DAL.Entities.Digi2l_DevContext _context;
        private readonly IProductTechnologyManager _productTechnologyManager;

        public IndexModel(RDCELERP.DAL.Entities.Digi2l_DevContext context , IProductTechnologyManager productTechnologyManager, IOptions<ApplicationSettings> config, CustomDataProtection protector) : base(config, protector)
        {
            _context = context;
            _productTechnologyManager = productTechnologyManager;
        }

        [BindProperty(SupportsGet = true)]
        public ProductTechnologyViewModel ProductTechnologyVM { get; set; }

        [BindProperty(SupportsGet = true)]
        public IList<TblProductTechnology> TblProductTechnology { get; set; }
        [BindProperty(SupportsGet = true)]
        public TblProductTechnology TblProductTechnologyObj { get; set; }
        public IActionResult OnGet()
        { 
            TblProductTechnologyObj = new TblProductTechnology();
       
        if (_loginSession == null)
        {
            return RedirectToPage("/index");
        }
        else
        {
           // ProductTechnologyVM = _ProductTechnologyManager.GetProductTechnologyById(_loginSession.UserViewModel.UserId);

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
            if (TblProductTechnologyObj != null && TblProductTechnologyObj.ProductTechnologyId > 0)
            {
                TblProductTechnologyObj = _context.TblProductTechnologies.Find(TblProductTechnologyObj.ProductTechnologyId);
            }

            if (TblProductTechnologyObj != null)
            {
                TblProductTechnologyObj.IsActive = false;
                TblProductTechnologyObj.ModifiedBy = _loginSession.UserViewModel.UserId;
                _context.TblProductTechnologies.Update(TblProductTechnologyObj);
                //  _context.TblRoles.Remove(TblRole);
                _context.SaveChanges();
            }

            return RedirectToPage("./index");
        }
    }
}
}

