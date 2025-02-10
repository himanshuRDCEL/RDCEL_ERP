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
using RDCELERP.Model.PriceMaster;

namespace RDCELERP.Core.App.Pages.PriceMaster
{
    public class ManageModel : CrudBasePageModel
    {
        #region Variable Declaration
        private readonly IProductTypeManager _ProductTypeManager;
        private readonly IProductCategoryManager _productCategoryManager;
        private readonly IPriceMasterManager _priceMasterManager;
        private readonly RDCELERP.DAL.Entities.Digi2l_DevContext _context;
        #endregion

        public ManageModel(IProductTypeManager ProductTypeManager, Digi2l_DevContext context, IProductCategoryManager productCategoryManager, IPriceMasterManager priceMasterManager, IOptions<ApplicationSettings> config, CustomDataProtection protector) : base(config, protector)
        {
            _ProductTypeManager = ProductTypeManager;
            _productCategoryManager = productCategoryManager;
            _priceMasterManager = priceMasterManager;
            _context = context;
        }

        [BindProperty(SupportsGet = true)]
        public PriceMasterViewModel PriceMasterViewModel { get; set; }


        public IActionResult OnGet(string id)
        {


            if (id != null)
            {
                id = _protector.Decode(id);
                PriceMasterViewModel = _priceMasterManager.GetPriceMasterById(Convert.ToInt32(id));
                var productcat = _context.TblProductCategories.Where(x => x.Name == PriceMasterViewModel.ProductCat).FirstOrDefault();
                if(productcat != null)
                {
                    PriceMasterViewModel.ProductCat = productcat.Description;
                }
            }

           

            if (PriceMasterViewModel == null)
                PriceMasterViewModel = new PriceMasterViewModel();

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


            if (PriceMasterViewModel.BrandName3 == PriceMasterViewModel.BrandName4 || PriceMasterViewModel.BrandName1 == PriceMasterViewModel.BrandName4 || PriceMasterViewModel.BrandName2 == PriceMasterViewModel.BrandName4)
            {
                ViewData["Msg2"] = "Brand 4 should be different from the brand 1,brand 2 and brand 3";
                return Page();
            }
            if (PriceMasterViewModel.BrandName2 == PriceMasterViewModel.BrandName3 || PriceMasterViewModel.BrandName1 == PriceMasterViewModel.BrandName3)
            {
                ViewData["Msg1"] = "Brand 3 should be different from the brand 1 & brand 2";
                return Page();
            }
            if (PriceMasterViewModel.BrandName1 == PriceMasterViewModel.BrandName2)
            {
                ViewData["Msg"] = "Brand 2 should be different from the brand 1";
                return Page();
            }


            if (ModelState.IsValid)
            {
                result = _priceMasterManager.ManagePriceMaster(PriceMasterViewModel, _loginSession.UserViewModel.UserId);
            }
           
            result = _priceMasterManager.ManagePriceMaster(PriceMasterViewModel, _loginSession.UserViewModel.UserId);


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
                       .Where(s => s.Description.Contains(term) && s.IsActive == true)
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


        public IActionResult OnPostCheckDuplicate(string? type, int? Id, string? Category, string? PriceCode)
            {
            TblProductCategory category = _context.TblProductCategories.Where(x=> x.Description == Category && x.IsActive == true).FirstOrDefault();
            TblProductType tblProductType = _context.TblProductTypes.Where(x=>x.Description + " " + x.Size == type && x.IsActive == true).FirstOrDefault();
            if (Id > 0)
            {

                bool isValid = !_context.TblPriceMasters.ToList().Exists(p => p.ExchPriceCode == PriceCode && p.ProductTypeId == tblProductType?.Id && p.ProductCategoryId == category?.Id && p.Id == Id && p.IsActive == true);

                return new JsonResult(isValid);
            }
            else
            {
                bool isValid = !_context.TblPriceMasters.ToList().Exists
                      (p => p.ExchPriceCode == PriceCode && p.ProductTypeId == tblProductType?.Id && p.ProductCategoryId == category?.Id && p.IsActive == true);

                return new JsonResult(isValid);
            }

        }
    }
}
