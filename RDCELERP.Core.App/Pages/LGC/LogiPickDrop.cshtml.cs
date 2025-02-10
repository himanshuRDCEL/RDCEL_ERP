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
using RDCELERP.Model.LGC;
using RDCELERP.Model.SearchFilters;

namespace RDCELERP.Core.App.Pages.LGC
{
    public class LogiPickDropModel : BasePageModel
    {
        #region Variable declartion
        private readonly ILogisticManager _logisticManager;
        private readonly IProductCategoryManager _productCategoryManager;
        private readonly IProductTypeManager _productTypeManager;
        #endregion

        #region Constructor
        public LogiPickDropModel(IOptions<ApplicationSettings> config, ILogisticManager logisticManager, IProductCategoryManager productCategoryManager, IProductTypeManager productTypeManager) : base(config)
        {
            _logisticManager = logisticManager;
            _productCategoryManager = productCategoryManager;
            _productTypeManager = productTypeManager;
        }
        #endregion

        [BindProperty(SupportsGet =true)]
        public List<LGCOrderViewModel> lGCOrderViewModels { get; set; }
        [BindProperty(SupportsGet = true)]
        public TblServicePartner tblServicePartner { get; set; }
        [BindProperty(SupportsGet = true)]
        public SearchFilterViewModel searchFilterVM { get; set; }
        public IActionResult OnGet()
        {
            int userId = 0;
            if (_loginSession != null)
            {
                userId = _loginSession.UserViewModel.UserId;
                tblServicePartner = _logisticManager.GetServicePartnerDetails(userId);
                if (tblServicePartner == null)
                {
                    tblServicePartner = new TblServicePartner();
                    tblServicePartner.UserId = 0;
                }
                // Get Product Category List
                var ProductGroup = _productCategoryManager.GetAllProductCategory();
                if (ProductGroup != null)
                {
                    ViewData["ProductGroup"] = new SelectList(ProductGroup, "Id", "Description");
                }
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
