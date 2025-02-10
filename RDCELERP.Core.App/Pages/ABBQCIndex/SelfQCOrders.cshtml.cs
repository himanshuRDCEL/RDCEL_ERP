using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Options;
using RDCELERP.BAL.Interface;
using RDCELERP.BAL.MasterManager;
using RDCELERP.Common.Enums;
using RDCELERP.Core.App.Pages.Base;
using RDCELERP.DAL.Entities;
using RDCELERP.Model.Base;
using RDCELERP.Model.LGC;
using RDCELERP.Model.SearchFilters;

namespace RDCELERP.Core.App.Pages.ABBQCIndex
{
    public class SelfQCOrdersModel : BasePageModel
    {
        #region Variable declartion
        private readonly ILogisticManager _logisticManager;
        private readonly IProductCategoryManager _productCategoryManager;
        private readonly IProductTypeManager _productTypeManager;
        private readonly Digi2l_DevContext _context;
        #endregion

        #region Constructor
        public SelfQCOrdersModel(IOptions<ApplicationSettings> config, ILogisticManager logisticManager, IProductCategoryManager productCategoryManager, IProductTypeManager productTypeManager, Digi2l_DevContext context) : base(config)
        {
            _logisticManager = logisticManager;
            _productCategoryManager = productCategoryManager;
            _productTypeManager = productTypeManager;
            _context = context;
        }
        #endregion
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
                // Get Product Category List
                var ProductGroup = _productCategoryManager.GetAllProductCategory();
                if (ProductGroup != null)
                {
                    ViewData["ProductGroup"] = new SelectList(ProductGroup, "Id", "Description");
                }
            }
            return Page();
        }

        #region Autopopulate Search Filter for search by RegdNo
        public IActionResult OnGetSearchRegdNo(string term)
        {
            if (term == null)
            {
                return BadRequest();
            }

            var data = _context.TblAbbredemptions
            .Where(x => x.RegdNo.Contains(term)
            && (x.StatusId == Convert.ToInt32(OrderStatusEnum.SelfQCbyCustomer)))
            .Select(x => x.RegdNo)
            .ToArray();
            return new JsonResult(data);
        }
        #endregion
    }
}
