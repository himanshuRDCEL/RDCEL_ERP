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
using RDCELERP.Model.ModelNumber;

namespace RDCELERP.Core.App.Pages.ModelNumber
{
    public class SweetnerModel : CrudBasePageModel
    {

        #region Variable Declaration
        private readonly IModelNumberManager _ModelNumberManager;
        private readonly IProductCategoryManager _productCategoryManager;
        private readonly IProductTypeManager _productTypeManager;
        private readonly IBrandManager _brandManager;
        private readonly IBusinessUnitManager _BusinessUnitManager;
        private readonly RDCELERP.DAL.Entities.Digi2l_DevContext _context;
        #endregion

        public SweetnerModel(IModelNumberManager ModelNumberManager, Digi2l_DevContext context, IProductTypeManager ProductTypeManager, IProductCategoryManager productCategoryManager, IBrandManager brandManager, IBusinessUnitManager BusinessUnitManager, IOptions<ApplicationSettings> config, CustomDataProtection protector) : base(config, protector)
        {
            _ModelNumberManager = ModelNumberManager;
            _productTypeManager = ProductTypeManager;
            _productCategoryManager = productCategoryManager;
            _brandManager = brandManager;
            _BusinessUnitManager = BusinessUnitManager;
            _context = context;
        }

        [BindProperty(SupportsGet = true)]
        public ModelNumberViewModel ModelNumberViewModel { get; set; }

        public IActionResult OnGet(string id)
        {

            if (id != null)
            {
                id = _protector.Decode(id);
                // <<<<<<< HEAD
                // ModelNumberViewModel = _ModelNumberManager.GetModelNumberById(Convert.ToInt32(id));
                // TblBusinessUnit businessUnit = _context.TblBusinessUnits.Where(x => x.BusinessUnitId == ModelNumberViewModel.BusinessUnitId).FirstOrDefault();
                // ModelNumberViewModel.BusinessUnitName = businessUnit.Name;

                // TblProductCategory productCategory = _context.TblProductCategories.Where(x => x.Id == ModelNumberViewModel.ProductCategoryId).FirstOrDefault();
                // ModelNumberViewModel.ProductCategoryName = productCategory.Description;

                // TblProductType productType = _context.TblProductTypes.Where(x => x.Id == ModelNumberViewModel.ProductTypeId).FirstOrDefault();
                // ModelNumberViewModel.ProductTypeName = productType.Description;
                // }

                // =======
                ModelNumberViewModel = _ModelNumberManager.GetModelNumberById(Convert.ToInt32(id));
                TblBusinessUnit businessUnit = _context.TblBusinessUnits.Where(x => x.BusinessUnitId == ModelNumberViewModel.BusinessUnitId).FirstOrDefault();
                ModelNumberViewModel.BusinessUnitName = businessUnit.Name;

                TblProductCategory productCategory = _context.TblProductCategories.Where(x => x.Id == ModelNumberViewModel.ProductCategoryId).FirstOrDefault();
                ModelNumberViewModel.ProductCategoryName = productCategory.Description;

                TblProductType productType = _context.TblProductTypes.Where(x => x.Id == ModelNumberViewModel.ProductTypeId).FirstOrDefault();
                ModelNumberViewModel.ProductTypeName = productType.Description;
                // >>>>>>> 3a1eef61 (Code Push for Autocomplete dropdown edit issue)

                if (ModelNumberViewModel == null)
                    ModelNumberViewModel = new ModelNumberViewModel();

                if (_loginSession == null)
                {
                    return RedirectToPage("/index");
                }
                else
                {
                    return Page();
                }

            }
            return Page();

        }
        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
            {

                int result = 0;
                if (ModelNumberViewModel.ProductTypeId != null)
                {
                    var ModelList = _context.TblModelNumbers.Where(x => x.ProductTypeId == ModelNumberViewModel.ProductTypeId).ToList();
                    if (ModelList != null)
                    {
                        ModelNumberViewModel.ModelList = ModelList;
                    }
                }
                else
                {
                    var ModelList1 = _context.TblModelNumbers.Where(x => x.ProductCategoryId == ModelNumberViewModel.ProductCategoryId).ToList();
                    if (ModelList1 != null)
                    {
                        ModelNumberViewModel.ModelList = ModelList1;
                    }
                }


                result = _ModelNumberManager.ManageSweetner(ModelNumberViewModel, _loginSession.UserViewModel.UserId);


                return RedirectToPage("./Index", new { id = _protector.Encode(result) });

            }



            public JsonResult OnGetProductTypeByCategoryAsync()
            {

                var ProductType = _productTypeManager.GetProductTypeBYCategory((int)ModelNumberViewModel.ProductCategoryId);
                if (ProductType != null)
                {
                    ViewData["ProductType"] = new SelectList(ProductType, "Id", "Description");
                }
                return new JsonResult(ProductType);
            }

            public IActionResult OnGetSearchBUName(string term)
            {
                if (term == null)
                {
                    return BadRequest();
                }
                var data = _context.TblBusinessUnits
                    .Where(p => p.Name.Contains(term) && p.IsActive == true)
                     .Select(s => new SelectListItem
                     {
                         Value = s.Name,
                         Text = s.BusinessUnitId.ToString()
                     })
                    .ToArray();
                return new JsonResult(data);
            }
            public IActionResult OnGetSearchBrandName(string term)
            {
                if (term == null)
                {
                    return BadRequest();
                }
                var data = _context.TblBrands
                    .Where(p => p.Name.Contains(term))
                     .Select(s => new SelectListItem
                     {
                         Value = s.Name,
                         Text = s.Id.ToString()
                     })
                    .ToArray();
                return new JsonResult(data);
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
                                Value = s.Description + " " + s.Size,
                                Text = s.Id.ToString()
                            })
                           .ToArray();
                return new JsonResult(list);
            }

    }
}
