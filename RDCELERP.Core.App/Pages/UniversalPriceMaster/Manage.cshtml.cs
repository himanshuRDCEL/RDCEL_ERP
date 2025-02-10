
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Options;
using RDCELERP.BAL.Interface;
using RDCELERP.Core.App.Pages.Base;
using RDCELERP.DAL.Entities;
using RDCELERP.Model.Base;
using RDCELERP.Model.UniversalPriceMaster;

namespace RDCELERP.Core.App.Pages.UniversalPriceMaster
{
    public class ManageModel : CrudBasePageModel
    {
        #region Variable Declaration
        private readonly IProductTypeManager _ProductTypeManager;
        private readonly IProductCategoryManager _productCategoryManager;
        private readonly IUniversalPriceMasterManager _universalPriceMasterManager;
        private readonly RDCELERP.DAL.Entities.Digi2l_DevContext _context;
        #endregion

        public ManageModel(IProductTypeManager ProductTypeManager, Digi2l_DevContext context, IProductCategoryManager productCategoryManager, IUniversalPriceMasterManager universalPriceMasterManager, IOptions<ApplicationSettings> config, CustomDataProtection protector) : base(config, protector)
        {
            _ProductTypeManager = ProductTypeManager;
            _productCategoryManager = productCategoryManager;
            _universalPriceMasterManager = universalPriceMasterManager;
            _context = context;
        }

        [BindProperty(SupportsGet = true)]
        public UniversalPriceMasterViewModel UniversalPriceMasterViewModel { get; set; }


        public IActionResult OnGet(string id)
        {


            if (id != null)
            {
                id = _protector.Decode(id);
                UniversalPriceMasterViewModel = _universalPriceMasterManager.GetUniversalPriceMasterById(Convert.ToInt32(id));

                var Category = _context.TblProductCategories.Where(x => x.Name == UniversalPriceMasterViewModel.ProductCategoryName).FirstOrDefault();
                if(Category != null)
                {
                    UniversalPriceMasterViewModel.ProductCategoryDiscription = Category.Description;
                }

            }



            if (UniversalPriceMasterViewModel == null)
                UniversalPriceMasterViewModel = new UniversalPriceMasterViewModel();

            if (_loginSession == null)
            {
                return RedirectToPage("/index");
            }
            else
            {
                return Page();
            }

        }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            int result = 0;


            if (UniversalPriceMasterViewModel.BrandName3 == UniversalPriceMasterViewModel.BrandName4 || UniversalPriceMasterViewModel.BrandName1 == UniversalPriceMasterViewModel.BrandName4 || UniversalPriceMasterViewModel.BrandName2 == UniversalPriceMasterViewModel.BrandName4)
            {
                ViewData["Msg2"] = "Brand 4 should be different from the brand 1,brand 2 and brand 3";
                return Page();
            }
            if (UniversalPriceMasterViewModel.BrandName2 == UniversalPriceMasterViewModel.BrandName3 || UniversalPriceMasterViewModel.BrandName1 == UniversalPriceMasterViewModel.BrandName3)
            {
                ViewData["Msg1"] = "Brand 3 should be different from the brand 1 & brand 2";
                return Page();
            }
            if (UniversalPriceMasterViewModel.BrandName1 == UniversalPriceMasterViewModel.BrandName2)
            {
                ViewData["Msg"] = "Brand 2 should be different from the brand 1";
                return Page();
            }


            if (ModelState.IsValid)
            {
                result = _universalPriceMasterManager.ManageUniversalPriceMaster(UniversalPriceMasterViewModel, _loginSession.UserViewModel.UserId);
            }


            if (result > 0)
                return RedirectToPage("./Index", new { id = _protector.Encode(result) });
            else

                return RedirectToPage("./Manage");
        }

        public IActionResult OnGetAutoProductCatName(string term)
        {
            if (term == null)
            {
                return BadRequest();
            }
            var data = _context.TblProductCategories
                       .Where(s => s.Name.Contains(term) && s.IsActive == true)
                       .Select(s => new SelectListItem
                       {
                           Value = s.Description,
                           Text = s.Id.ToString()
                       })
                       .ToArray();
            return new JsonResult(data);
        }

        public IActionResult OnGetAutoProductTypeName(string term, string term2)
        {
            if (term == null)
            {
                return BadRequest();
            }
            var list = _context.TblProductTypes
                       .Where(e => e.Description.Contains(term) && e.ProductCatId == Convert.ToInt32(term2) && e.IsActive == true)
                        .Select(s => new SelectListItem
                        {
                            Value = s.Description +" " + s.Size,
                            Text = s.Id.ToString()
                        })
                       .ToArray();
            return new JsonResult(list);
        }


        public IActionResult OnGetAutoProductTypeCode(string term, string term2)
        {
            if (term == null)
            {
                return BadRequest();
            }
            var list = _context.TblProductTypes
                       .Where(e => e.Code.Contains(term) && e.ProductCatId == Convert.ToInt32(term2) && e.IsActive == true)
                        .Select(s => new SelectListItem
                        {
                            Value = s.Code,
                            Text = s.Id.ToString()
                        })
                       .ToArray();
            return new JsonResult(list);
        }

        public IActionResult OnGetAutoPriceMasterName(string term)
        {
            if (term == null)
            {
                return BadRequest();
            }
            var list = _context.TblPriceMasterNames
                       .Where(e => e.Name.Contains(term) && e.IsActive == true)
                        .Select(s => new SelectListItem
                        {
                            Value = s.Name,
                            Text = s.PriceMasterNameId.ToString()
                        })
                       .ToArray();
            return new JsonResult(list);
        }

        public IActionResult OnGetAutoBrand(string term)
        {
            if (term == null)
            {
                return BadRequest();
            }
            var list = _context.TblBrands
                       .Where(e => e.Name.Contains(term) && e.IsActive == true)
                        .Select(s => new SelectListItem
                        {
                            Value = s.Name,
                            Text = s.Id.ToString()
                        })
                       .ToArray();
            return new JsonResult(list);
        }

    }
}
