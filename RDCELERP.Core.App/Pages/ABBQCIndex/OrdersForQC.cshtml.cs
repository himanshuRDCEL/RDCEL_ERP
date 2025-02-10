using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RDCELERP.DAL.Entities;
using RDCELERP.Core.App.Pages.Base;
using Microsoft.Extensions.Options;
using RDCELERP.Model.Base;
using RDCELERP.BAL.Interface;
using RDCELERP.Model.Company;
using RDCELERP.Model.SearchFilters;
using Microsoft.AspNetCore.Mvc.Rendering;
using RDCELERP.Common.Enums;
using RDCELERP.Common.Helper;

namespace RDCELERP.Core.App.Pages.ABBQCIndex
{
    public class OrdersForQCModel : BasePageModel
    {
        #region Variable declartion
        private readonly IProductCategoryManager _productCategoryManager;
        private readonly IProductTypeManager _productTypeManager;
        private readonly Digi2l_DevContext _context;
        #endregion

        #region Constructor
        public OrdersForQCModel(IOptions<ApplicationSettings> config, IProductCategoryManager productCategoryManager, IProductTypeManager productTypeManager, Digi2l_DevContext context)
       : base(config)
        {
            _productCategoryManager = productCategoryManager;
            _productTypeManager = productTypeManager;
            _context = context;
        }
        #endregion

        [BindProperty(SupportsGet = true)]
        public SearchFilterViewModel searchFilterVM { get; set; }
        [BindProperty(SupportsGet = true)]
        public int? TblReloadTimeMs { get; set; }
        public void OnGet()
        {
            // Get Product Category List
            var ProductGroup = _productCategoryManager.GetAllProductCategory();
            if (ProductGroup != null)
            {
                ViewData["ProductGroup"] = new SelectList(ProductGroup, "Id", "Description");
            }
            TblConfiguration tblConfiguration = _context.TblConfigurations.FirstOrDefault(x=>x.Name == EnumHelper.DescriptionAttr(ConfigurationEnum.TblReloadTimeMs));

            if (tblConfiguration != null)
            {
                TblReloadTimeMs = Convert.ToInt32(tblConfiguration.Value);
            }
            else
            {
                TblReloadTimeMs = 30000;
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

            var data = _context.TblAbbredemptions
            .Where(x => x.RegdNo.Contains(term)
            && (x.StatusId == Convert.ToInt32(OrderStatusEnum.OrderCreatedbySponsor)
                                || x.StatusId == Convert.ToInt32(OrderStatusEnum.QCInProgress_3Q)
                                || x.StatusId == Convert.ToInt32(OrderStatusEnum.CallAndGoScheduledAppointmentTaken_3P)
                                || x.StatusId == Convert.ToInt32(OrderStatusEnum.QCAppointmentrescheduled)
                                || x.StatusId == Convert.ToInt32(OrderStatusEnum.ReopenOrder)
                                || x.StatusId == Convert.ToInt32(OrderStatusEnum.InstalledbySponsor)))
            .Select(x => x.RegdNo)
            .ToArray();
            return new JsonResult(data);
        }
        #endregion
    }
}
