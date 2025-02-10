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
using RDCELERP.Core.App.Pages.Base;
using RDCELERP.DAL.Entities;
using RDCELERP.Model.Base;
using RDCELERP.Model.VehicleIncentive;

namespace RDCELERP.Core.App.Pages.VehicleIncentive
{
    public class IndexModel : CrudBasePageModel
    {

        private readonly RDCELERP.DAL.Entities.Digi2l_DevContext _context;
        private readonly IVehicleIncentiveManager _vehicleIncentiveManager;
        private readonly IProductTypeManager _productTypeManager;
        private readonly IProductCategoryManager _productCategoryManager;
        public IndexModel(RDCELERP.DAL.Entities.Digi2l_DevContext context, IVehicleIncentiveManager vehicleIncentiveManager, IProductTypeManager productTypeManager, IProductCategoryManager productCategoryManager, IOptions<ApplicationSettings> config, CustomDataProtection protector) : base(config, protector)

        {
            _context = context;
            _vehicleIncentiveManager = vehicleIncentiveManager;
            _productTypeManager = productTypeManager;
            _productCategoryManager = productCategoryManager;
        }



        [BindProperty(SupportsGet = true)]
        public VehicleIncentiveViewModel VehicleIncentiveVM { get; set; }

        [BindProperty(SupportsGet = true)]
        public IList<TblVehicleIncentive> TblVehicleIncentive { get; set; }
        [BindProperty(SupportsGet = true)]
        public TblVehicleIncentive TblVehicleIncentiveObj { get; set; }

        public IActionResult OnGet()
        {
            TblVehicleIncentiveObj = new TblVehicleIncentive();
            var ProductGroup = _productCategoryManager.GetAllProductCategory();
            if (ProductGroup != null)
            {
                ViewData["ProductGroup"] = new SelectList(ProductGroup, "Id", "Description");
            }

            if (_loginSession == null)
            {
                return RedirectToPage("/index");
            }
            else
            {
                VehicleIncentiveVM = _vehicleIncentiveManager.GetVehicleIncentiveById(_loginSession.UserViewModel.UserId);

                return Page();
            }
        }

        public IActionResult OnPostDeleteAsync()
        {
            if (_loginSession == null)
            {
                return RedirectToPage("/index");
            }
            else
            {
                if (TblVehicleIncentiveObj != null && TblVehicleIncentiveObj.IncentiveId > 0)
                {
                    TblVehicleIncentiveObj = _context.TblVehicleIncentives.Find(TblVehicleIncentiveObj.IncentiveId);
                }

                if (TblVehicleIncentiveObj != null)
                {
                    TblVehicleIncentiveObj.IsActive = false;
                    TblVehicleIncentiveObj.ModifiedBy = _loginSession.UserViewModel.UserId;
                    _context.TblVehicleIncentives.Update(TblVehicleIncentiveObj);
                    //  _context.TblRoles.Remove(TblRole);
                    _context.SaveChanges();
                }

                return RedirectToPage("./index");
            }
        }
        #region Product category type
        public JsonResult OnGetProductCategoryTypeAsync()
        {
            var productTypeList = _productTypeManager.GetProductTypeByCategoryId(Convert.ToInt32(VehicleIncentiveVM.ProductCategoryId));
            if (productTypeList != null)
            {
                ViewData["productTypeList"] = new SelectList(productTypeList, "Id", "Description");
            }
            return new JsonResult(productTypeList);
        }

        #endregion 
    }
}
