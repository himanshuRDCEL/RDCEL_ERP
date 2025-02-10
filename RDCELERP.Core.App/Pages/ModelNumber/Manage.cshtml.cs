using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Options;
using RDCELERP.BAL.Interface;
using RDCELERP.Core.App.Pages.Base;
using RDCELERP.DAL.Entities;
using RDCELERP.Model.Base;
using RDCELERP.Model.ModelNumber;

namespace RDCELERP.Core.App.Pages.ModelNumber
{
    public class ManageModel : CrudBasePageModel
    {

        #region Variable Declaration
        private readonly IModelNumberManager _ModelNumberManager;
        private readonly IProductCategoryManager _productCategoryManager;
        private readonly IProductTypeManager _productTypeManager;
        private readonly IBrandManager _brandManager;
        private readonly IBusinessUnitManager _BusinessUnitManager;
        private readonly RDCELERP.DAL.Entities.Digi2l_DevContext _context;
        #endregion

        public ManageModel(IModelNumberManager ModelNumberManager, Digi2l_DevContext context, IProductTypeManager ProductTypeManager, IProductCategoryManager productCategoryManager, IBrandManager brandManager, IBusinessUnitManager BusinessUnitManager, IOptions<ApplicationSettings> config, CustomDataProtection protector) : base(config, protector)
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
                ModelNumberViewModel = _ModelNumberManager.GetModelNumberById(Convert.ToInt32(id));
                TblBusinessUnit businessUnit = _context.TblBusinessUnits.Where(x => x.BusinessUnitId == ModelNumberViewModel.BusinessUnitId).FirstOrDefault();
                ModelNumberViewModel.BusinessUnitName = businessUnit.Name;
                TblBrand Brand = _context.TblBrands.Where(x => x.Id == ModelNumberViewModel.BrandId).FirstOrDefault();
                if (Brand != null)
                {
                    ModelNumberViewModel.BrandName = Brand.Name;
                }

                TblProductCategory productCategory = _context.TblProductCategories.Where(x => x.Id == ModelNumberViewModel.ProductCategoryId).FirstOrDefault();
                ModelNumberViewModel.ProductCategoryName = productCategory.Description;
                TblProductType productType = _context.TblProductTypes.Where(x => x.Id == ModelNumberViewModel.ProductTypeId).FirstOrDefault();
                ModelNumberViewModel.ProductTypeName = productType.Description;
                
            }

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

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            int result = 0;
            
            result = _ModelNumberManager.ManageModelNumber(ModelNumberViewModel, _loginSession.UserViewModel.UserId);
            

            if (result > 0)
                return RedirectToPage("./Index", new { id = _protector.Encode(result) });
            else

                return RedirectToPage("./Manage");
        }


        public IActionResult OnPostCheckName(string? modelname, int? Id, int? BUId)
        {
            string nameTrimmed = modelname?.Trim(); // Trim the Name parameter
            if (Id > 0)
            {
              
                bool isValid = !_context.TblModelNumbers.ToList().Exists(p => p.ModelName == nameTrimmed && p.ModelNumberId != Id && p.BusinessUnitId == BUId);
               
                return new JsonResult(isValid);
            }
            else
            {
             
                bool isValid = !_context.TblModelNumbers.ToList().Exists
                          (p => p.ModelName == nameTrimmed && p.BusinessUnitId == BUId);

                return new JsonResult(isValid);
            }

        }


        public IActionResult OnPostCheckDefaultModelDuplicate(int? Id, string? BUId)
        {
            TblBusinessUnit businessUnit = _context.TblBusinessUnits.Where(x=>x.Name == BUId).FirstOrDefault();
           
            if (Id > 0)
            {

                bool isValid = !_context.TblModelNumbers.ToList().Exists(p => p.ModelNumberId != Id && p.BusinessUnitId == businessUnit?.BusinessUnitId && p.IsDefaultProduct == true && p.IsActive == true);

                return new JsonResult(isValid);
            }
            else
            {

                bool isValid = !_context.TblModelNumbers.ToList().Exists
                          (p => p.BusinessUnitId == businessUnit?.BusinessUnitId  && p.IsDefaultProduct == true && p.IsActive == true);

                return new JsonResult(isValid);
            }

        }



        //public JsonResult OnGetDefaultProductByBusinessUnitAsync()
        //{

        //    var result = _BusinessUnitManager.GetDefaultProductByModelBaised((int)ModelNumberViewModel.BusinessUnitId);
        //    bool isvalid;
        //    if (result != null)
        //    {
        //         isvalid = false;
        //    }
        //    else
        //    {
        //        isvalid = true;
        //    }

        //    return new JsonResult(isvalid);
        //}

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

        //public IActionResult OnGetSearchBUNameABB(string term)
        //{
        //    if (term == null)
        //    {
        //        return BadRequest();
        //    }
        //    var data = _context.TblBusinessUnits
        //        .Where(p => p.Name.Contains(term) && p.IsActive == true && p.IsAbb == true)
        //         .Select(s => new SelectListItem
        //         {
        //             Value = s.Name,
        //             Text = s.BusinessUnitId.ToString()
        //         })
        //        .ToArray();
        //    return new JsonResult(data);
        //}


        public IActionResult OnGetSearchBrandName(string term)
        {
            if (term == null)
            {
                return BadRequest();
            }
            var data = _context.TblBrands
                .Where(p => p.Name.Contains(term) && p.IsActive == true)
                 .Select(s => new SelectListItem
                 {
                     Value = s.Name,
                     Text = s.Id.ToString()
                 })
                .ToArray();
            return new JsonResult(data);
        }


        //public IActionResult OnGetSearchBrandNameAbb(string term)
        //{
        //    if (term == null)
        //    {
        //        return BadRequest();
        //    }
        //    var data = _context.TblBrands
        //        .Where(p => p.Name.Contains(term) && p.IsActive == true && p.BusinessUnit.IsAbb == true)
        //         .Select(s => new SelectListItem
        //         {
        //             Value = s.Name + " - " + s.BusinessUnit.Name,
        //             Text = s.Id.ToString()
        //         })
        //        .ToArray();
        //    return new JsonResult(data);
        //}

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
                       .Where(e => e.Description.Contains(term) && e.ProductCatId == Convert.ToInt32(term2) && e.IsActive == true && e.Size == null)
                        .Select(s => new SelectListItem
                        {
                            Value = s.Description,
                            Text = s.Id.ToString()
                        })
                       .ToArray();
            return new JsonResult(list);
        }

        public IActionResult OnGetSearchBPName(string term)
        {
            if (term == null)
            {
                return BadRequest();
            }
            var data = _context.TblBusinessPartners
                .Where(p => p.Name.Contains(term))
                 .Select(s => new SelectListItem
                 {
                     Value = s.Name,
                     Text = s.BusinessPartnerId.ToString()
                 })
                .ToArray();
            return new JsonResult(data);
        }

    }
}
