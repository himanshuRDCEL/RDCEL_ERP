using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RDCELERP.BAL.Interface;
using RDCELERP.Core.App.Pages.Base;
using RDCELERP.DAL.Entities;
using RDCELERP.Model.ABBPlanMaster;
using RDCELERP.Model.Base;
using RDCELERP.Model.Product;

namespace RDCELERP.Core.App.Pages.ABBPlanMaster
{
    public class ManageModel : CrudBasePageModel
    {

        #region Variable Declaration
        private readonly IABBPlanMasterManager _ABBPlanMasterManager;
        private readonly IBusinessUnitManager _BusinessUnitManager;
        private readonly IProductCategoryManager _ProductCategoryManager;
        private readonly IProductTypeManager _ProductTypeManager;
        private readonly RDCELERP.DAL.Entities.Digi2l_DevContext _context;
        #endregion

        public ManageModel(IABBPlanMasterManager ABBPlanMasterManager, IBusinessUnitManager BusinessUnitManager, Digi2l_DevContext context, IProductCategoryManager ProductCategoryManager, IProductTypeManager ProductTypeManager, IOptions<ApplicationSettings> config, CustomDataProtection protector) : base(config, protector)
        {
            _ProductTypeManager = ProductTypeManager;
            _ProductCategoryManager = ProductCategoryManager;
            _BusinessUnitManager = BusinessUnitManager; ;
            _ABBPlanMasterManager = ABBPlanMasterManager;
            _context = context;
        }

        [BindProperty(SupportsGet = true)]
        public ABBPlanMasterViewModel ABBPlanMasterViewModel { get; set; }

        [BindProperty(SupportsGet = true)]
        public IList<EntryViewModel> EntryViewModel { get; set; }

        [BindProperty(SupportsGet =true)]
        public object TestList { get; set; }
        [BindProperty(SupportsGet = true)]
        public List<PlanDetails> PlanList { get; set; }
        [BindProperty(SupportsGet = true)]
        public List<EntryViewModel> Plans { get; set; }

        public IActionResult OnGet(string id)
        {
           
            if (id != null)
            {
                id = _protector.Decode(id);
                ABBPlanMasterViewModel = _ABBPlanMasterManager.GetABBPlanMasterById(Convert.ToInt32(id));
                
                TblBusinessUnit businessUnit = _context.TblBusinessUnits.Where(x => x.BusinessUnitId == ABBPlanMasterViewModel.BusinessUnitId).FirstOrDefault();
                ABBPlanMasterViewModel.BusinessUnitName = businessUnit.Name;
                TblProductCategory productCategory = _context.TblProductCategories.Where(x => x.Id == ABBPlanMasterViewModel.ProductCatId).FirstOrDefault();
                ABBPlanMasterViewModel.ProductCategoryName = productCategory.Description;
                TblProductType productType = _context.TblProductTypes.Where(x => x.Id == ABBPlanMasterViewModel.ProductTypeId).FirstOrDefault();
                ABBPlanMasterViewModel.ProductTypeName = productType.Description + " " + productType.Size;
                
               var list = _context.TblAbbplanMasters.Where(x => x.BusinessUnitId == ABBPlanMasterViewModel.BusinessUnitId).ToList();
                foreach (var plan in list)
                {
                    EntryViewModel details = new EntryViewModel();
                    details.PlanMasterId = plan.PlanMasterId;
                    details.toMonth = plan.ToMonth;
                    details.fromMonth = plan.FromMonth;
                    details.percentage = plan.AssuredBuyBackPercentage;
                    Plans.Add(details);
                }

            }
           

            if (ABBPlanMasterViewModel == null)
                ABBPlanMasterViewModel = new ABBPlanMasterViewModel();

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
        public IActionResult OnPost()
        {
            List<PlanDetails> RemainingPlans = new List<PlanDetails>();

            int result = 0;
            if (ModelState.IsValid)
            {
                var plans = Request.Form["hiddenPlanList"];

                var AllPlansList = JsonConvert.DeserializeObject<List<PlanDetails>>(plans);
                
                for(int i = 1; i < 250; i++) 
                {
                    var toMonth = "Plans["+ i +"].toMonth";
                    var fromMonth = "Plans["+ i +"].fromMonth";
                    var percentage = "Plans["+ i +"].percentage";
                    var Id = "Plans[" + i + "].PlanMasterId";

                    PlanDetails planDetails = new PlanDetails();
                    planDetails.toMonth = Request.Form[toMonth];
                    planDetails.fromMonth = Request.Form[fromMonth];
                    planDetails.percentage = Request.Form[percentage];
                    planDetails.PlanMasterId = Request.Form[Id];

                    if (string.IsNullOrEmpty(planDetails.fromMonth) && string.IsNullOrEmpty(planDetails.toMonth) && string.IsNullOrEmpty(planDetails.percentage))
                    {
                        break;
                    }
                    RemainingPlans.Add(planDetails);
                }
                result = _ABBPlanMasterManager.ManageABBPlanMaster(ABBPlanMasterViewModel, AllPlansList, RemainingPlans, _loginSession.UserViewModel.UserId);
                
            }

            if(result > 0)
            {
                return RedirectToPage("./Index", new { id = _protector.Encode(result) });
            }
            else
            {
                return Page();
            }
            
            
           
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
    }
}
