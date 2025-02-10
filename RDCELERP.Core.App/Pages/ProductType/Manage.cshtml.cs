using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Options;
using RDCELERP.BAL.Helper;
using RDCELERP.BAL.Interface;
using RDCELERP.BAL.MasterManager;
using RDCELERP.Core.App.Pages.Base;
using RDCELERP.DAL.Entities;
using RDCELERP.Model.Base;
using RDCELERP.Model.Master;
using RDCELERP.Model.Product;

namespace RDCELERP.Core.App.Pages.ProductType
{
    public class ManageModel : CrudBasePageModel
    {

        #region Variable Declaration
        private readonly IProductTypeManager _ProductTypeManager;
        private readonly IProductCategoryManager _productCategoryManager;
        private readonly RDCELERP.DAL.Entities.Digi2l_DevContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IImageHelper _imageHelper;
        #endregion

        public ManageModel(IProductTypeManager ProductTypeManager, Digi2l_DevContext context, IImageHelper imageHelper, IWebHostEnvironment webHostEnvironment, IProductCategoryManager productCategoryManager, IOptions<ApplicationSettings> config, CustomDataProtection protector) : base(config, protector)
        {
            _ProductTypeManager = ProductTypeManager;
            _productCategoryManager = productCategoryManager;
            _context = context;
            _imageHelper = imageHelper;
            _webHostEnvironment = webHostEnvironment;
        }

        [BindProperty(SupportsGet = true)]
        public ProductTypeViewModel ProductTypeViewModel { get; set; }

        public IActionResult OnGet(string id)
        {
            var ProductCategorylist = _productCategoryManager.GetAllProductCategory();

            if (id != null)
            {
                id = _protector.Decode(id);
                ProductTypeViewModel = _ProductTypeManager.GetProductTypeById(Convert.ToInt32(id));
                if (ProductTypeViewModel.ProductCatId != null)
                {
                    TblProductCategory productCategory = _context.TblProductCategories.Where(x => x.Id == ProductTypeViewModel.ProductCatId).FirstOrDefault();
                    ProductTypeViewModel.ProductCategoryName = productCategory?.Description;
                }
            }
            if (!string.IsNullOrEmpty(ProductTypeViewModel.ProductTypeImage))
            {
                ProductTypeViewModel.ProductTypeImageLink = _baseConfig.Value.BaseURL + "DBFiles/ProductTypeImage/" + ProductTypeViewModel.ProductTypeImage;
                ProductTypeViewModel.ProductTypeImage = ProductTypeViewModel?.ProductTypeImageLink;
                ProductTypeViewModel.ProductTypeImageUrl = ProductTypeViewModel?.ProductTypeImageLink;
            }


            if (ProductTypeViewModel == null)
                ProductTypeViewModel = new ProductTypeViewModel();

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
        public async Task<IActionResult> OnPostAsync(IFormFile ProductTypeImage)
        {
            int result = 0;

            if (ProductTypeImage != null)
            {
                //extract the extension from filename -
                //var extension = System.IO.Path.GetExtension(ProductTypeImage.FileName);

                string fileName = ProductTypeViewModel?.Name?.Trim() +" "+ ProductTypeImage.FileName;
                var filePath = string.Concat(_webHostEnvironment.WebRootPath, "\\", @"\DBFiles\ProductTypeImage");
                var fileNameWithPath = string.Concat(filePath, "\\", fileName);

                _imageHelper.SaveFile(ProductTypeImage, @"DBFiles\ProductTypeImage", fileName);
                ProductTypeViewModel.ProductTypeImageUrl = fileName;
                ProductTypeViewModel.ProductTypeImage = fileName;
            }
            else
            {
                var image = Path.GetFileName(ProductTypeViewModel.ProductTypeImageUrl);
                ProductTypeViewModel.ProductTypeImageUrl = image;
                ProductTypeViewModel.ProductTypeImage = image;
            }

            result = _ProductTypeManager.ManageProductType(ProductTypeViewModel, _loginSession.UserViewModel.UserId);

            if (result > 0)
                return RedirectToPage("./Index", new { id = _protector.Encode(result) });
            else
                return Page();
        }

        public IActionResult OnGetSearchCategoryName(string term)
        {
            if (term == null)
            {
                return BadRequest();
            }
            var data = _context.TblProductCategories.Where(p => !string.IsNullOrEmpty(p.Description) && p.Description.Contains(term) && p.IsActive == true)
                 .Select(s => new SelectListItem
                 {
                     Value = s.Description,
                     Text = s.Id.ToString()
                 })
                .ToArray();
            return new JsonResult(data);
        }


        public IActionResult OnPostCheckName(string? Name, int? TypeId)
        {
            string? nameTrimmed = Name?.Trim()?.ToLower(); // Trim the Name parameter

            if (TypeId > 0)
            {
                bool isValid = !_context.TblProductTypes.Where(x => !string.IsNullOrEmpty(x.Name)).ToList().Exists(p => p!.Name!.Trim().Equals(nameTrimmed, StringComparison.CurrentCultureIgnoreCase) && p.Id != TypeId);
                return new JsonResult(isValid);
            }
            else
            {
                bool isValid = !_context.TblProductTypes.Where(x => !string.IsNullOrEmpty(x.Name)).ToList().Exists(p => p.Name!.Trim().Equals(nameTrimmed, StringComparison.CurrentCultureIgnoreCase));
                return new JsonResult(isValid);
            }
        }

        public IActionResult OnPostCheckDesc(string? Desc, int? TypeId)
       {
            string? DescTrimmed = Desc?.Trim()?.ToLower(); // Trim the Name parameter

            if (TypeId > 0)
            {
                bool isValid = !_context.TblProductTypes.Where(x => !string.IsNullOrEmpty(x.Description)).ToList().Exists(p => p!.Description!.Trim().Equals(DescTrimmed, StringComparison.CurrentCultureIgnoreCase) && p.Id != TypeId);
                return new JsonResult(isValid);
            }
            else
            {
                bool isValid = !_context.TblProductTypes.Where(x => !string.IsNullOrEmpty(x.Description)).ToList().Exists(p => p.Description!.Trim().Equals(DescTrimmed, StringComparison.CurrentCultureIgnoreCase));
                return new JsonResult(isValid);
            }
        }




        public IActionResult OnPostCheckCode(string? code, int? Id)
        {
            string? codeTrimmed = code?.Trim()?.ToLower();
            if (Id > 0)
            {
                bool isValid = !_context.TblProductTypes.Where(x => !string.IsNullOrEmpty(x.Code)).ToList().Exists(p => p.Code!.Equals(codeTrimmed, StringComparison.CurrentCultureIgnoreCase) && p.Id != Id);
                return new JsonResult(isValid);
            }
            else
            {
                bool isValid = !_context.TblProductTypes.Where(x => !string.IsNullOrEmpty(x.Code)).ToList().Exists(p => p.Code!.Equals(codeTrimmed, StringComparison.CurrentCultureIgnoreCase));
                return new JsonResult(isValid);
            }
        }
    }
}

