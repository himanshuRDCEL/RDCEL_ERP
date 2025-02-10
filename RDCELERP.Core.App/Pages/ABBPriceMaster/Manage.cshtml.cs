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
using RDCELERP.Model.ABBPriceMaster;
using RDCELERP.Model.Base;

namespace RDCELERP.Core.App.Pages.ABBPriceMaster
{
    public class ManageModel : CrudBasePageModel
    {
        #region Variable Declaration
        private readonly IABBPriceMasterManager _ABBPriceMasterManager;
        private readonly IBusinessUnitManager _BusinessUnitManager;
        private readonly IProductCategoryManager _ProductCategoryManager;
        private readonly IProductTypeManager _ProductTypeManager;
        private readonly RDCELERP.DAL.Entities.Digi2l_DevContext _context;
        #endregion

        public ManageModel(IABBPriceMasterManager ABBPriceMasterManager, IBusinessUnitManager BusinessUnitManager, IProductCategoryManager ProductCategoryManager, Digi2l_DevContext context, IProductTypeManager ProductTypeManager, IOptions<ApplicationSettings> config, CustomDataProtection protector) : base(config, protector)
        {
            _ProductTypeManager = ProductTypeManager;
            _ProductCategoryManager = ProductCategoryManager;
            _BusinessUnitManager = BusinessUnitManager; ;
            _ABBPriceMasterManager = ABBPriceMasterManager;
            _context = context;
        }

        [BindProperty(SupportsGet = true)]
        public ABBPriceMasterViewModel ABBPriceMasterViewModel { get; set; }

        public IActionResult OnGet(string id)
        {
            //var ProductCategorylist = _ProductCategoryManager.GetAllProductCategory();

            //if (ProductCategorylist != null)
            //{
                
            //    ViewData["ProductCategorylists"] = new SelectList(ProductCategorylist, "Description", "Description");
            //}

            //var ProductTypelist = _ProductTypeManager.GetAllProductType();

            //if (ProductTypelist != null)
            //{
               
            //    ViewData["ProductTypelists"] = new SelectList((from s in ProductTypelist.ToList()
            //                                                   select new
            //                                                   {
            //                                                       Size = s.Size,

            //                                                       Description = s.Description + " " + s.Size
            //                                                   }), "Description", "Description", null);

            //}
            //var BusinessUnitlist = _BusinessUnitManager.GetAllBusinessUnit();

            //if (BusinessUnitlist != null)
            //{
            //    ViewData["BusinessUnitlist"] = new SelectList(BusinessUnitlist, "BusinessUnitId", "Name");
            //}
            if (id != null)
            {
                id = _protector.Decode(id);
                ABBPriceMasterViewModel = _ABBPriceMasterManager.GetABBPriceMasterById(Convert.ToInt32(id));

                TblBusinessUnit businessUnit = _context.TblBusinessUnits.Where(x => x.BusinessUnitId == ABBPriceMasterViewModel.BusinessUnitId).FirstOrDefault();
                if (businessUnit != null)
                {
                    ABBPriceMasterViewModel.BusinessUnitName = businessUnit.Name;
                }

                TblProductCategory productCategory = _context.TblProductCategories.Where(x => x.Description == ABBPriceMasterViewModel.ProductCategory).FirstOrDefault();
                if (productCategory != null)
                {
                    ABBPriceMasterViewModel.ProductCategoryId = productCategory.Id;
                }

            }

                

            if (ABBPriceMasterViewModel == null)
                ABBPriceMasterViewModel = new ABBPriceMasterViewModel();

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
            if (ABBPriceMasterViewModel.PriceStartRange > ABBPriceMasterViewModel.PriceEndRange)
            {
                ViewData["Msg"] = "Price start range should be less than the price end range.";
                return Page();
            }
            if (ModelState.IsValid)
            {
                result = _ABBPriceMasterManager.ManageABBPriceMaster(ABBPriceMasterViewModel, _loginSession.UserViewModel.UserId);
            }

            if (result > 0)
                return RedirectToPage("./Index", new { id = _protector.Encode(result) });
            else

                return RedirectToPage("./Manage");
        }
       

        public IActionResult OnGetSearchBUName(string term)
        {
            if (term == null)
            {
                return BadRequest();
            }
            var data = _context.TblBusinessUnits
                .Where(p => p.Name.Contains(term) && p.IsActive == true)
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
                       .Where(s => s.Description.Contains(term) && s.IsActive == true)
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
                       .Where(e => e.Description.Contains(term) && e.ProductCatId == Convert.ToInt32(term2) && e.IsActive == true)
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
            TblBusinessUnit tblBusinessUnit = _context.TblBusinessUnits.Where(x=>x.Name == BUId && x.IsActive == true).FirstOrDefault();
            if (Id > 0)
            {

                bool isValid = !_context.TblAbbpriceMasters.ToList().Exists(p => p.BusinessUnitId == tblBusinessUnit?.BusinessUnitId && p.ProductSabcategory == type && p.ProductCategory == Category && p.PriceMasterId == Id && p.IsActive == true);

                return new JsonResult(isValid);
            }
            else
            {
                bool isValid = !_context.TblAbbpriceMasters.ToList().Exists
                      (p => p.BusinessUnitId == tblBusinessUnit?.BusinessUnitId && p.ProductSabcategory == type && p.ProductCategory == Category && p.IsActive == true);

                return new JsonResult(isValid);
            }

        }
    }
    }

