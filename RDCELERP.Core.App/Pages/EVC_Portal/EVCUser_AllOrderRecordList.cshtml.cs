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
using RDCELERP.Model.Base;
using RDCELERP.Model.SearchFilters;

namespace RDCELERP.Core.App.Pages
{
    public class EVCUser_AllOrderRecordListModel : BasePageModel
    {
        private readonly RDCELERP.DAL.Entities.Digi2l_DevContext _context;
        private readonly IEVCManager _EVCManager;
        private readonly IProductTypeManager _productTypeManager;
        private readonly IProductCategoryManager _productCategoryManager;
        public EVCUser_AllOrderRecordListModel(RDCELERP.DAL.Entities.Digi2l_DevContext context, IOptions<ApplicationSettings> config, IEVCManager EVCManager, CustomDataProtection protector,IProductTypeManager productTypeManager, IProductCategoryManager ProductCategoryManager) : base(config)

        {
            _EVCManager = EVCManager;
            _context = context;
            _productTypeManager = productTypeManager;
            _productCategoryManager = ProductCategoryManager;
        }
        [BindProperty(SupportsGet = true)]
        public int? userId { get; set; }

        [BindProperty(SupportsGet = true)]
        public SearchFilterViewModel searchFilterVM { get; set; }
        public void OnGet(int? UserId)
        {

            if (UserId != null)
            {
                userId = UserId;

            }
            else
            {
                userId = _loginSession.UserViewModel.UserId;
            }
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
            .Where(x => x.RegdNo.Contains(term)&&x.IsActive==true)
            .Select(x => x.RegdNo)
            .ToArray();
            return new JsonResult(data);
        }
        #endregion
    }
}
