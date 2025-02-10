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
using RDCELERP.DAL.Entities;
using RDCELERP.Model.Base;
using RDCELERP.Model.EVC;
using RDCELERP.Model.SearchFilters;

namespace RDCELERP.Core.App.Pages.EVC_Allocation
{
    public class Not_AllocatedModel : BasePageModel
    {
        private readonly RDCELERP.DAL.Entities.Digi2l_DevContext _context;
        private readonly IEVCManager _EVCManager;
        private readonly IProductCategoryManager _productCategoryManager;
        private readonly IProductTypeManager _productTypeManager;

        public Not_AllocatedModel(RDCELERP.DAL.Entities.Digi2l_DevContext context, IOptions<ApplicationSettings> config, IEVCManager EVCManager, IProductCategoryManager productCategoryManager, IProductTypeManager productTypeManager)
       : base(config)
        {
            _EVCManager = EVCManager;
            _context = context;
            _productCategoryManager = productCategoryManager;
            _productTypeManager = productTypeManager;
        }


        [BindProperty(SupportsGet = true)]
        public string text { get; set; }

        [BindProperty(SupportsGet = true)]
        public string title { get; set; }
        [BindProperty(SupportsGet = true)]
        public string ListNA { get; set; }
        [BindProperty(SupportsGet = true)]
        public string MyIds { get; set; }
        [BindProperty(SupportsGet = true)]
        public int UserId1 { get; set; }

        [BindProperty(SupportsGet = true)]
        public SearchFilterViewModel searchFilterVM { get; set; }
        public void OnGet(string? ReturnList)
        {
            UserId1 = _loginSession.UserViewModel.UserId;
            if (ReturnList != null)
            {
                ListNA = ReturnList;
                if (ReturnList == "Success")
                {
                    title = "Successfully Assign";
                    text = "Order Assign Successfully";
                    ReturnList = null;
                }
                else
                {
                    title = "This Orders Not Assign";
                    text = ReturnList;
                    ReturnList = null;
                }
            }
            var ProductGroup = _productCategoryManager.GetAllProductCategory();
            if (ProductGroup != null)
            {
                ViewData["ProductGroup"] = new SelectList(ProductGroup, "Id", "Description");
            }


            //if (ReturnList == null && title != null && text != null)
            //{
            //    ReturnList = null;
            //    title = null;
            //    text = null;
            //}
        }

        public IActionResult OnPostExportAsync()
        {
            try
            {
                string ids = MyIds;

            }
            catch (Exception ex)
            {

                throw;
            }

            return RedirectToPage("Allocate_EVCFrom", new { ids = MyIds });
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

            var data = _context.TblOrderTrans
            .Where(x => x.RegdNo.Contains(term)
            && (x.StatusId == Convert.ToInt32(OrderStatusEnum.AmountApprovedbyCustomerAfterQC)
                                || x.StatusId == Convert.ToInt32(OrderStatusEnum.InstalledbySponsor)
                                ))
            .Select(x => x.RegdNo)
            .ToArray();
            return new JsonResult(data);
        }
        #endregion

    }
}
