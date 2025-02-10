using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Options;
using RDCELERP.BAL.Interface;
using RDCELERP.Common.Enums;
using RDCELERP.Common.Helper;
using RDCELERP.Core.App.Pages.Base;
using RDCELERP.DAL.Entities;
using RDCELERP.Model.Base;
using RDCELERP.Model.ImagLabel;
using RDCELERP.Model.Master;

namespace RDCELERP.Core.App.Pages.ImageLabelMaster
{
    public class ManageModel : CrudBasePageModel
    {
        
        #region Variable Declaration
        private readonly IImageLabelMasterManager _ImageLabelMasterManager;
        private readonly IProductCategoryManager _productCategoryManager;
        private readonly RDCELERP.DAL.Entities.Digi2l_DevContext _context;

        #endregion

        public ManageModel(IImageLabelMasterManager ImageLabelMasterManager, Digi2l_DevContext context, IProductCategoryManager productCategoryManager, IOptions<ApplicationSettings> config, CustomDataProtection protector) : base(config, protector)
        {
            _ImageLabelMasterManager = ImageLabelMasterManager;
            _productCategoryManager = productCategoryManager;
            _context = context;
        }

        [BindProperty(SupportsGet = true)]
        public ImageLabelNewViewModel ImageLabelNewViewModel { get; set; }

        public IActionResult OnGet(string id)
        {

            if (id != null)
            {
                id = _protector.Decode(id);
                ImageLabelNewViewModel = _ImageLabelMasterManager.GetImageLabelById(Convert.ToInt32(id));
                TblProductCategory productCategory = _context.TblProductCategories.Where(x => x.Id == ImageLabelNewViewModel.ProductCatId).FirstOrDefault();
                if(productCategory != null)
                {
                    ImageLabelNewViewModel.ProductCategoryName = productCategory.Description;
                }
                TblProductType productType = _context.TblProductTypes.Where(x => x.Id == ImageLabelNewViewModel.ProductTypeId).FirstOrDefault();
                if(productType != null)
                {
                    ImageLabelNewViewModel.ProductTypeName = productType.Description + " " + productType.Size;
                }

            }
            
            if (ImageLabelNewViewModel != null)
            {
                if (ImageLabelNewViewModel.ImagePlaceHolder != null) {
                    string url = _baseConfig.Value.BaseURL;
                    ImageLabelNewViewModel.FullPlaceHolderImageUrl = url + EnumHelper.DescriptionAttr(FileAddressEnum.ImageLabelMaster) + ImageLabelNewViewModel.ImagePlaceHolder;
                }
            }
            else
            {
                ImageLabelNewViewModel = new ImageLabelNewViewModel();
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
        public async Task<IActionResult> OnPostAsync()
        {
            int result = 0;
            if (ModelState.IsValid)
            {
                result = _ImageLabelMasterManager.ManageImageLabel(ImageLabelNewViewModel, _loginSession.UserViewModel.UserId);
               
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
                .Where(p => p.Description.Contains(term) && p.IsActive == true)
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
                       .Where(e => e.Description.Contains(term) && e.ProductCatId == Convert.ToInt32(term2))
                        .Select(s => new SelectListItem
                        {
                            Value = s.Description + s.Size,
                            Text = s.Id.ToString()
                        })
                       .ToArray();
            return new JsonResult(list);
        }

        public IActionResult OnPostCheckName(int? pattern, int? Id, int? categoryId)
        {
            if (Id > 0)
            {

                bool isValid = !_context.TblImageLabelMasters.ToList().Exists(p => p.Pattern == pattern && p.ProductCatId == categoryId && p.ImageLabelid == Id && p.IsActive == true);

                return new JsonResult(isValid);
            }
            else
            {
                bool isValid = !_context.TblImageLabelMasters.ToList().Exists
                      (p => p.Pattern == pattern && p.ProductCatId == categoryId && p.IsActive == true);

                return new JsonResult(isValid);
            }

        }


    }
}
