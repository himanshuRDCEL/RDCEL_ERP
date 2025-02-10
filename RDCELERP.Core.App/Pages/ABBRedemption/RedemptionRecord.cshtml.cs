using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Options;
using System;
using RDCELERP.BAL.Interface;
using RDCELERP.Core.App.Pages.Base;
using RDCELERP.Model.Base;
using RDCELERP.Model.SearchFilters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace RDCELERP.Core.App.Pages.ABBRedemp
{
    public class RedemptionRecordModel : BasePageModel
    {
        private readonly RDCELERP.DAL.Entities.Digi2l_DevContext _context;
        private readonly IAbbRegistrationManager _AbbRegistrationManager;
        private readonly IABBRedemptionManager _AbbRedemptionManager;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IProductCategoryManager _productCategoryManager;
        private readonly IProductTypeManager _productTypeManager;


        public RedemptionRecordModel(IWebHostEnvironment webHostEnvironment, IABBRedemptionManager abbRedemptionManager, RDCELERP.DAL.Entities.Digi2l_DevContext context, IOptions<ApplicationSettings> config, IProductCategoryManager productCategoryManager, IProductTypeManager productTypeManager)
       : base(config)
        {
            _webHostEnvironment = webHostEnvironment;
            _AbbRedemptionManager = abbRedemptionManager;
            _context = context;
            _productCategoryManager = productCategoryManager;
            _productTypeManager = productTypeManager;
        }

        [BindProperty(SupportsGet = true)]
        public SearchFilterViewModel searchFilterVM { get; set; }
        public IActionResult OnGet()
        {
            var ProductGroup = _productCategoryManager.GetAllProductCategory();
            if (ProductGroup != null)
            {
                ViewData["ProductGroup"] = new SelectList(ProductGroup, "Id", "Description");
            }
            return Page();
        }

        #region Product category type
        public JsonResult OnGetProductCategoryTypeAsync()
        {
            var productTypeList = _productTypeManager.GetProductTypeByCategoryId(Convert.ToInt32(searchFilterVM.ProductCatId));
            if (productTypeList != null)
            {
                ViewData["productTypeList"] = new SelectList(productTypeList, "Id", "Description");
            }
            return new JsonResult(productTypeList);
        }

        #endregion
    }
}
