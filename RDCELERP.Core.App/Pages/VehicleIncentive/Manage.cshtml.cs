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
using RDCELERP.Model.VehicleIncentive;

namespace RDCELERP.Core.App.Pages.VehicleIncentive
{
    public class ManageModel : CrudBasePageModel
    {

        #region Variable Declaration
        private readonly IVehicleIncentiveManager _vehicleIncentiveManager;
        private readonly IProductCategoryManager _productCategoryManager;
        private readonly IProductTypeManager _productTypeManager;
        private readonly IBrandManager _brandManager;
        private readonly IBusinessUnitManager _BusinessUnitManager;
        private readonly RDCELERP.DAL.Entities.Digi2l_DevContext _context;
        #endregion

        public ManageModel(IVehicleIncentiveManager vehicleIncentiveManager, Digi2l_DevContext context, IProductTypeManager ProductTypeManager, IProductCategoryManager productCategoryManager, IBrandManager brandManager, IBusinessUnitManager BusinessUnitManager, IOptions<ApplicationSettings> config, CustomDataProtection protector) : base(config, protector)
        {
            _vehicleIncentiveManager = vehicleIncentiveManager;
            _productTypeManager = ProductTypeManager;
            _productCategoryManager = productCategoryManager;
            _brandManager = brandManager;
            _BusinessUnitManager = BusinessUnitManager;
            _context = context;
        }

        [BindProperty(SupportsGet = true)]
        public VehicleIncentiveViewModel VehicleIncentiveViewModel { get; set; }

        public IActionResult OnGet(string id)
        {


            if (id != null)
            {
                id = _protector.Decode(id);
                VehicleIncentiveViewModel = _vehicleIncentiveManager.GetVehicleIncentiveById(Convert.ToInt32(id));
                TblBusinessUnit businessUnit = _context.TblBusinessUnits.Where(x => x.BusinessUnitId == VehicleIncentiveViewModel.BusinessUnitId).FirstOrDefault();
                if(businessUnit != null)
                {
                    VehicleIncentiveViewModel.BusinessUnitName = businessUnit.Name;
                }

                TblProductCategory productCategory = _context.TblProductCategories.Where(x => x.Id == VehicleIncentiveViewModel.ProductCategoryId).FirstOrDefault();
                if (productCategory != null)
                {
                    VehicleIncentiveViewModel.ProductCategoryName = productCategory.Description;
                }
          
                TblProductType productType = _context.TblProductTypes.Where(x => x.Id == VehicleIncentiveViewModel.ProductTypeId).FirstOrDefault();
                if (productType != null)
                {
                    VehicleIncentiveViewModel.ProductTypeName = productType.Description + " " + productType.Size;
                }
                
            }

            if (VehicleIncentiveViewModel == null)
                VehicleIncentiveViewModel = new VehicleIncentiveViewModel();

            if (_loginSession == null)
            {
                return RedirectToPage("/index");
            }
            else
            {
                return Page();
            }

        }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            int result = 0;
            if (ModelState.IsValid)
            {
                result = _vehicleIncentiveManager.ManageVehicleIncentive(VehicleIncentiveViewModel, _loginSession.UserViewModel.UserId);
            }

            if (result > 0)
                return RedirectToPage("./Index", new { id = _protector.Encode(result) });
            else

                return RedirectToPage("./Manage");
        }

       

        public JsonResult OnGetDefaultProductByBusinessUnitAsync()
        {

            var result = _BusinessUnitManager.GetDefaultProductByModelBaised((int)VehicleIncentiveViewModel.BusinessUnitId);
            bool isvalid;
            if (result != null)
            {
                isvalid = false;
            }
            else
            {
                isvalid = true;
            }

            return new JsonResult(isvalid);
        }

        public IActionResult OnGetSearchBUName(string term)
        {
            if (term == null)
            {
                return BadRequest();
            }
            var data = _context.TblBusinessUnits
                .Where(p => p.Name.Contains(term))
                 .Select(s => new SelectListItem
                 {
                     Value = s.Name,
                     Text = s.BusinessUnitId.ToString()
                 })
                .ToArray();
            return new JsonResult(data);
        }

        

        public IActionResult OnGetAutoProductCatName(string term)
        {
            if (term == null)
            {
                return BadRequest();
            }
            var data = _context.TblProductCategories
                       .Where(s => s.Description.Contains(term))
                       .Select(s => new SelectListItem
                       {
                           Value = s.Description,
                           Text = s.Id.ToString()
                       })
                       .ToArray();
            return new JsonResult(data);
        }

        public IActionResult OnGetAutoProductTypeName(string term, string term2)
        {
            if (term == null)
            {
                return BadRequest();
            }
            var list = _context.TblProductTypes
                       .Where(e => e.Description.Contains(term) && e.ProductCatId == Convert.ToInt32(term2))
                        .Select(s => new SelectListItem
                        {
                            Value = s.Description + s.Size,
                            Text = s.Id.ToString()
                        })
                       .ToArray();
            return new JsonResult(list);
        }

        public IActionResult OnPostCheckDuplicate(string? type, int? Id, string? Category, string? BUId)
        {
            TblBusinessUnit tblBusinessUnit = _context.TblBusinessUnits.Where(x => x.Name == BUId && x.IsActive == true).FirstOrDefault();
            TblProductCategory tblProductCategory  = _context.TblProductCategories.Where(x => x.Description == Category && x.IsActive == true).FirstOrDefault();
            TblProductType tblProductType = _context.TblProductTypes.Where(x => x.Description == type && x.IsActive == true).FirstOrDefault();
            if (Id > 0)
            {

                bool isValid = !_context.TblVehicleIncentives.ToList().Exists(p => p.BusinessUnitId == tblBusinessUnit?.BusinessUnitId && p.ProductType?.Description == type && p.ProductCategory?.Description == Category && p.IncentiveId == Id && p.IsActive == true);

                return new JsonResult(isValid);
            }
            else
            {
                bool isValid = !_context.TblVehicleIncentives.ToList().Exists
                      (p => p.BusinessUnitId == tblBusinessUnit?.BusinessUnitId && p.ProductTypeId == tblProductType?.Id && p.ProductCategoryId == tblProductCategory?.Id && p.IsActive == true);

                return new JsonResult(isValid);
            }

        }
    }
}
