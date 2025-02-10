using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Options;
using RDCELERP.BAL.Interface;
using RDCELERP.Common.Enums;
using RDCELERP.Core.App.Pages.Base;
using RDCELERP.DAL.Entities;
using RDCELERP.Model.Base;
using RDCELERP.Model.SearchFilters;

namespace RDCELERP.Core.App.Pages.QCIndex
{
    public class RatioReportSelfQcModel : BasePageModel
    {
        #region variable declaration
        private readonly IProductCategoryManager _productCategoryManager;
        private readonly Digi2l_DevContext _context;

        #endregion

        #region Constructor
        public RatioReportSelfQcModel(IOptions<ApplicationSettings> baseConfig, IProductCategoryManager productCategoryManager, Digi2l_DevContext context) : base(baseConfig)
        {
            _productCategoryManager = productCategoryManager;
            _context = context;
        }
        #endregion

        #region Model Binding
        [BindProperty(SupportsGet = true)]
        public SearchFilterViewModel searchFilterVM { get; set; }

        [BindProperty(SupportsGet = true)]
        public int UserId { get; set; }
        #endregion

        public void OnGet()
        {
            UserId = _loginSession.UserViewModel.UserId;
            // Get Product Category List
            var ProductGroup = _productCategoryManager.GetAllProductCategory();
            if (ProductGroup != null)
            {
                ViewData["ProductGroup"] = new SelectList(ProductGroup, "Id", "Description");
            }
        }

        #region Autopopulate Search Filter for search by RegdNo
        public IActionResult OnGetSearchRegdNo(string term)
        {
            if (term == null)
            {
                return BadRequest();
            }
            //string searchTerm = term.ToString();

            var data = _context.TblExchangeOrders
            .Where(x => x.RegdNo.Contains(term)
            && (x.StatusId == Convert.ToInt32(OrderStatusEnum.Waitingforcustapproval) || x.StatusId == Convert.ToInt32(OrderStatusEnum.QCByPass)))
            .Select(x => x.RegdNo)
            .ToArray();
            return new JsonResult(data);
        }
        #endregion
    }
}
