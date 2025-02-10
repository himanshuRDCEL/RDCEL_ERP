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
using RDCELERP.Model.ProductQuality;

namespace RDCELERP.Core.App.Pages.ProductQualityIndex
{
    public class ManageModel : CrudBasePageModel
    {
        #region Variable Declaration
        private readonly IProductQualityIndexManager _ProductQualityIndexManager;
        private readonly IProductCategoryManager _productCategoryManager;
        private readonly RDCELERP.DAL.Entities.Digi2l_DevContext _context;
        #endregion

        public ManageModel(IProductQualityIndexManager ProductQualityIndexManager, Digi2l_DevContext context, IProductCategoryManager productCategoryManager, IOptions<ApplicationSettings> config, CustomDataProtection protector) : base(config, protector)
        {
            _ProductQualityIndexManager = ProductQualityIndexManager;
            _productCategoryManager = productCategoryManager;
            _context = context;
        }

        [BindProperty(SupportsGet = true)]
        public ProductQualityIndexViewModel ProductQualityIndexViewModel { get; set; }

        public IActionResult OnGet(string id)
        {          
            if (id != null)
            {
                id = _protector.Decode(id);
                ProductQualityIndexViewModel = _ProductQualityIndexManager.GetProductQualityIndexById(Convert.ToInt32(id));
                if(ProductQualityIndexViewModel != null && ProductQualityIndexViewModel?.ProductCategoryId != null)
                {
                    TblProductCategory? productCategory = _context.TblProductCategories.Where(x => x.Id == ProductQualityIndexViewModel.ProductCategoryId)?.FirstOrDefault();
                    ProductQualityIndexViewModel.ProductCategoryName = productCategory?.Description;
                }
            }             
            if (ProductQualityIndexViewModel == null)
                ProductQualityIndexViewModel = new ProductQualityIndexViewModel();

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
            if (ModelState.IsValid)
            {
                result = _ProductQualityIndexManager.ManageProductQualityIndex(ProductQualityIndexViewModel, _loginSession.UserViewModel.UserId);                
            }

            if (result > 0)
                return RedirectToPage("./Index", new { id = _protector.Encode(result) });
            else
                return Page();
        }
        public IActionResult OnPostCheckName(string Name, int? Id)
        {
            if(Id > 0)
            {
                TblProductQualityIndex TblProductQualityIndex = new TblProductQualityIndex();
                string? nameTrimmed = Name?.Trim(); // Trim the Name parameter
                bool isValid = !_context.TblProductQualityIndices.Where(x=>!string.IsNullOrEmpty(x.Name)).ToList().Exists(p => p.Name!.Trim() == nameTrimmed && p.ProductQualityIndexId !=Id);            
                return new JsonResult(isValid);
            }
            else
            {
                TblProductQualityIndex TblProductQualityIndex = new TblProductQualityIndex();
                string? nameTrimmed = Name?.Trim(); // Trim the Name parameter
                bool isValid = !_context.TblProductQualityIndices.Where(x=>!string.IsNullOrEmpty(x.Name)).ToList().Exists(p => p.Name!.Trim().Equals(nameTrimmed, StringComparison.CurrentCultureIgnoreCase));
                return new JsonResult(isValid);
            }    
        }

        public IActionResult OnGetSearchCategoryName(string term)
        {
            if (term == null)
            {
                return BadRequest();
            }
            //IEnumerable<TblProductCategory> AllCategories = new List<TblProductCategory>();
            //IEnumerable<TblProductQualityIndex> ExistingCategories = new List<TblProductQualityIndex>();
            List<TblProductCategory> categories = new List<TblProductCategory>();

            var AllCategories = _context.TblProductCategories.Distinct().ToList();
            var ExistingCategories = _context.TblProductQualityIndices.Select(x=>x.ProductCategoryId).Distinct().ToList();

            if (ExistingCategories != null && AllCategories != null)
            {
                categories = _context.TblProductCategories.Where(x => !ExistingCategories.Contains(x.Id)).ToList();
            }

           var data = categories
            .Where(p => !string.IsNullOrEmpty(p.Description) && p.Description.Contains(term, StringComparison.OrdinalIgnoreCase) && p.IsActive == true)
             .Select(s => new SelectListItem
             {
                 Value = s.Description,
                 Text = s.Id.ToString()
             })
            .ToArray();
            return new JsonResult(data);
        }
    }
}
