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
using RDCELERP.BAL.MasterManager;
using RDCELERP.Common.Enums;
using RDCELERP.Core.App.Pages.Base;
using RDCELERP.Model.Base;
using RDCELERP.Model.SearchFilters;

namespace RDCELERP.Core.App.Pages.EVC_Allocation
{
    public class Assign_OrderModel : BasePageModel
    {
        private readonly RDCELERP.DAL.Entities.Digi2l_DevContext _context;
        private readonly IEVCManager _EVCManager;
        private readonly IProductCategoryManager _productCategoryManager;
        private readonly IProductTypeManager _productTypeManager;
        public Assign_OrderModel(RDCELERP.DAL.Entities.Digi2l_DevContext context, IOptions<ApplicationSettings> config, IEVCManager EVCManager, IProductCategoryManager productCategoryManager, IProductTypeManager productTypeManager)
       : base(config)
        {
            _EVCManager = EVCManager;
            _context = context;
            _productCategoryManager = productCategoryManager;
            _productTypeManager = productTypeManager;
        }
        [BindProperty(SupportsGet = true)]
        public SearchFilterViewModel searchFilterVM { get; set; }
        [BindProperty(SupportsGet = true)]
        public int UserId1 { get; set; }
        public void OnGet()
        {
            UserId1 = _loginSession.UserViewModel.UserId;
            var ProductGroup = _productCategoryManager.GetAllProductCategory();
            if (ProductGroup != null)
            {
                ViewData["ProductGroup"] = new SelectList(ProductGroup, "Id", "Description");
            }

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

        #region Autopopulate Search Filter for search by RegdNo
        public IActionResult OnGetSearchRegdNo(string term)
        {
            if (term == null)
            {
                return BadRequest();
            }
            //string searchTerm = term.ToString();

            var data = _context.TblWalletTransactions
            .Where(x => x.RegdNo.Contains(term)
            && (x.StatusId == Convert.ToInt32(OrderStatusEnum.EVCAllocationcompleted).ToString()))
            .Select(x => x.RegdNo)
            .ToArray();
            return new JsonResult(data);
        }
        #endregion

        #region Autopopulate Search Filter for search by EVCRegdNo
        public IActionResult OnGetSearchEVCRegdNo(string term)
        {
            if (term == null)
            {
                return BadRequest();
            }
           

            var data = _context.TblWalletTransactions.Include(x => x.Evcregistration).Where(x => x.Evcregistration != null && x.Evcregistration.EvcregdNo.Contains(term)           
              && (x.StatusId == Convert.ToInt32(OrderStatusEnum.EVCAllocationcompleted).ToString()))           
            .Select(x => x.Evcregistration.EvcregdNo)
            .ToArray().Distinct();
            return new JsonResult(data);
        }
        #endregion
    }
}
