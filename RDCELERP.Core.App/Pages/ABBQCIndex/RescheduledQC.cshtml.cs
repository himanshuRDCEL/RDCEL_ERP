using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
//using System.Web.Mvc;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using RDCELERP.BAL.Interface;
using RDCELERP.Common.Enums;
using RDCELERP.Core.App.Pages.Base;
using RDCELERP.DAL.Entities;
using RDCELERP.Model.Base;
using RDCELERP.Model.SearchFilters;

namespace RDCELERP.Core.App.Pages.ABBQCIndex
{
    public class RescheduledQCModel : BasePageModel
    {
        #region Variable declartion
        private readonly IProductCategoryManager _productCategoryManager;
        private readonly IProductTypeManager _productTypeManager;
        private readonly Digi2l_DevContext _context;
        #endregion
        public RescheduledQCModel(IOptions<ApplicationSettings> config, IProductCategoryManager productCategoryManager, IProductTypeManager productTypeManager, Digi2l_DevContext context)
       : base(config)
        {
            _productCategoryManager = productCategoryManager;
            _productTypeManager = productTypeManager;
            _context = context;
        }
        [BindProperty(SupportsGet = true)]
        public SearchFilterViewModel searchFilterVM { get; set; }
        public int? ActiveTabId { get; set; }
        public IActionResult OnGet(int? TabId = null)
        {
             // Get Product Category List
                var ProductGroup = _productCategoryManager.GetAllProductCategory();
                if (ProductGroup != null)
                {
                    ViewData["ProductGroup"] = new SelectList(ProductGroup, "Id", "Description");
                }
            if (TabId != null)
            {
                ActiveTabId = TabId;
            }
            else
            {
                ActiveTabId = 1;
            }
            
            return Page();
        }
        #region Product category type
        public JsonResult OnGetProductCategoryTypeAsync()
        {
            var productTypeList = _productTypeManager.GetProductTypeByCategoryId(Convert.ToInt32(searchFilterVM.ProductCatId));
            productTypeList = productTypeList.GroupBy(x => x.Description).Select(x => x.First()).ToList();
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

            var data = _context.TblOrderQcs.Include(x=>x.OrderTrans).ThenInclude(x=>x.Abbredemption)
            .Where(x => x.OrderTrans != null && x.OrderTrans.Abbredemption != null && x.OrderTrans.RegdNo.Contains(term)
            && (x.StatusId == Convert.ToInt32(OrderStatusEnum.QCAppointmentrescheduled)
                               || x.StatusId == Convert.ToInt32(OrderStatusEnum.QCAppointmentRescheduled_3RA)))
            .Select(x => x.OrderTrans.RegdNo)
            .ToArray();
            return new JsonResult(data);
        }
        #endregion
    }
}
