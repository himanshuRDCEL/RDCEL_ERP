using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Options;
using RDCELERP.BAL.Helper;
using RDCELERP.BAL.Interface;
using RDCELERP.Core.App.Pages.Base;
using RDCELERP.DAL.Entities;
using RDCELERP.Model.Base;
using RDCELERP.Model.Company;
using RDCELERP.Model.Master;

namespace RDCELERP.Core.App.Pages.ProductCategory
{
    public class ManageModel : CrudBasePageModel
    {

        #region Variable Declaration
        private readonly IProductCategoryManager _ProductCategoryManager;
        private readonly RDCELERP.DAL.Entities.Digi2l_DevContext _context;
        private IWebHostEnvironment _webHostEnvironment;
        private readonly IImageHelper _imageHelper;
        #endregion

        public ManageModel(IProductCategoryManager ProductCategoryManager, IWebHostEnvironment webHostEnvironment, Digi2l_DevContext context, IOptions<ApplicationSettings> config, CustomDataProtection protector, IImageHelper imageHelper) : base(config, protector)
        {
            _ProductCategoryManager = ProductCategoryManager;
            _context = context;
            _webHostEnvironment = webHostEnvironment;
            _imageHelper = imageHelper;
        }

        [BindProperty(SupportsGet = true)]
        public ProductCategoryViewModel ProductCategoryViewModel { get; set; }

        public IActionResult OnGet(string id)
        {
            if (id != null)
                id = _protector.Decode(id);
            ProductCategoryViewModel = _ProductCategoryManager.GetProductCategoryById(Convert.ToInt32(id));

            if (ProductCategoryViewModel == null)
                ProductCategoryViewModel = new ProductCategoryViewModel();

            string URL = _baseConfig.Value.URLPrefixforProd;

            //ViewData["CountryList"] = new SelectList(_countryManager.GetAllCountries(), "CountryId", "Name");
            if (!string.IsNullOrEmpty(ProductCategoryViewModel.ProductCategoryImage))
            {
                ProductCategoryViewModel.ProductCategoryImageLink = _baseConfig.Value.BaseURL + "DBFiles/ProductCategoryImage/" + ProductCategoryViewModel.ProductCategoryImage;
                ProductCategoryViewModel.ProductCategoryImage = ProductCategoryViewModel?.ProductCategoryImageLink;
                ProductCategoryViewModel.ProductCategoryImageUrl = ProductCategoryViewModel?.ProductCategoryImageLink;
            }

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
        public async Task<IActionResult> OnPostAsync(IFormFile ProductCategoryImage)
        {
            int result = 0;

            if (ProductCategoryImage != null)
            {
                string fileName = ProductCategoryViewModel?.Name;
                var filePath = @"\DBFiles\ProductCategoryImage";
                string FileNameWithExtension = fileName?.Trim() + System.IO.Path.GetExtension(ProductCategoryImage.FileName);

                _imageHelper.SaveFile(ProductCategoryImage, filePath, FileNameWithExtension);
                ProductCategoryViewModel.ProductCategoryImageUrl = FileNameWithExtension;
                ProductCategoryViewModel.ProductCategoryImage = FileNameWithExtension;
            }
            else
            {
                var image = Path.GetFileName(ProductCategoryViewModel.ProductCategoryImageUrl);
                ProductCategoryViewModel.ProductCategoryImageUrl = image;
                ProductCategoryViewModel.ProductCategoryImage = image;
            }

            result = _ProductCategoryManager.ManageProductCategory(ProductCategoryViewModel, _loginSession.UserViewModel.UserId);
            if (result == 0)
            {
                ViewData["Message"] = "This Category is already exist";
                return Page();

            }

            if (result > 0)
                return RedirectToPage("./Index", new { id = _protector.Encode(result) });
            else
                return Page();
        }

        public IActionResult OnPostCheckName(string? Name, int? CategoryId)
        {
            string? nameTrimmed = string.Empty;
            if (CategoryId > 0)
            {
                TblProductCategory TblProductCategory = new TblProductCategory();
                nameTrimmed = Name?.Trim()?.ToLower();// Trim the Name parameter
                bool isValid = !_context.TblProductCategories.Where(x=> !string.IsNullOrEmpty(x.Name)).ToList().Exists(p => p!.Name!.Trim().Equals(nameTrimmed, StringComparison.CurrentCultureIgnoreCase) && p.Id != CategoryId);
                return new JsonResult(isValid);
            }
            else
            {
                TblProductCategory TblProductCategory = new TblProductCategory();
                nameTrimmed = Name?.Trim()?.ToLower(); // Trim the Name parameter
                bool isValid = !_context.TblProductCategories.Where(x => !string.IsNullOrEmpty(x.Name)).ToList().Exists(p => p!.Name!.Trim().Equals(nameTrimmed, StringComparison.CurrentCultureIgnoreCase));
                return new JsonResult(isValid);
            }
        }
        public IActionResult OnPostCheckCode(string? code, int? Id)
        {
            if (Id > 0)
            {
                TblProductCategory TblProductCategory = new TblProductCategory();
                string? codeTrimmed = code?.Trim()?.ToLower();
                bool isValid = !_context.TblProductCategories.Where(x => !string.IsNullOrEmpty(x.Code)).ToList().Exists(p => p.Code!.Equals(codeTrimmed, StringComparison.CurrentCultureIgnoreCase) && p.Id != Id);
                return new JsonResult(isValid);
            }
            else
            {
                TblProductCategory TblProductCategory = new TblProductCategory();
                string? codeTrimmed = code?.Trim()?.ToLower();
                bool isValid = !_context.TblProductCategories.Where(x => !string.IsNullOrEmpty(x.Code)).ToList().Exists(p => p.Code!.Equals(codeTrimmed, StringComparison.CurrentCultureIgnoreCase));
                return new JsonResult(isValid);
            }

        }

        public IActionResult OnPostCheckDesc(string? Desc, int? CategoryId)
        {
            string? descTrimmed = Desc?.Trim()?.ToLower();
            if (CategoryId > 0)
            {
                TblProductCategory TblProductCategory = new TblProductCategory();
                bool isValid = !_context.TblProductCategories.Where(x => !string.IsNullOrEmpty(x.Description)).ToList().Exists(p => p.Description!.Trim() == descTrimmed && p.Id != CategoryId);
                return new JsonResult(isValid);
            }
            else
            {
                TblProductCategory TblProductCategory = new TblProductCategory();
                bool isValid = !_context.TblProductCategories.Where(x => !string.IsNullOrEmpty(x.Description)).ToList().Exists(p => p.Description!.Trim() == descTrimmed);
                return new JsonResult(isValid);
            }
        }
    }
}

