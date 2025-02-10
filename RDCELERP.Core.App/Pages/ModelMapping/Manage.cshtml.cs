using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Options;
using RDCELERP.BAL.Interface;
using RDCELERP.Core.App.Pages.Base;
using RDCELERP.DAL.Entities;
using RDCELERP.Model.Base;
using RDCELERP.Model.ModelNumber;

namespace RDCELERP.Core.App.Pages.ModelMapping
{
    public class ManageModel : CrudBasePageModel
    {
        #region Variable Declaration
        private readonly IModelNumberManager _ModelNumberManager;
        private readonly IBrandManager _brandManager;
        private readonly IBusinessUnitManager _businessUnitManager;
        private readonly RDCELERP.DAL.Entities.Digi2l_DevContext _context;
        #endregion

        public ManageModel(IModelNumberManager ModelNumberManager, Digi2l_DevContext context, IBrandManager brandManager, IBusinessUnitManager businessUnitManager, IOptions<ApplicationSettings> config, CustomDataProtection protector) : base(config, protector)
        {
            _ModelNumberManager = ModelNumberManager;
            _brandManager = brandManager;
            _businessUnitManager = businessUnitManager;
            _context = context;
        }

        [BindProperty(SupportsGet = true)]
        public ModelMappingViewModel ModelMappingViewModel { get; set; }

        public IActionResult OnGet(string id)
        {
            if (id != null)
            {
                id = _protector.Decode(id);
                ModelMappingViewModel = _ModelNumberManager.GetModelMappingById(Convert.ToInt32(id));
                TblBusinessUnit businessUnit = _context.TblBusinessUnits.Where(x => x.BusinessUnitId == ModelMappingViewModel.BusinessUnitId).FirstOrDefault();
                if (businessUnit != null)
                {
                    ModelMappingViewModel.BusinessUnitName = businessUnit.Name;
                }
               
                TblBrand Brand = _context.TblBrands.Where(x => x.Id == ModelMappingViewModel.BrandId).FirstOrDefault();
                if (Brand != null)
                {
                    ModelMappingViewModel.BrandName = Brand.Name;
                }

                TblModelNumber modelNumber = _context.TblModelNumbers.Where(x => x.ModelNumberId == ModelMappingViewModel.ModelId).FirstOrDefault();
                if (modelNumber != null)
                {
                    ModelMappingViewModel.ModelName = modelNumber.ModelName;
                }

                TblBusinessPartner tblBusinessPartner = _context.TblBusinessPartners.Where(x => x.BusinessPartnerId == ModelMappingViewModel.BusinessPartnerId).FirstOrDefault();
                if (tblBusinessPartner != null)
                {
                    ModelMappingViewModel.BusinessPartnerName = tblBusinessPartner.Name;
                }

            }

            if (ModelMappingViewModel == null)
                ModelMappingViewModel = new ModelMappingViewModel();

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

            result = _ModelNumberManager.ManageModelMapping(ModelMappingViewModel, _loginSession.UserViewModel.UserId);


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

        public IActionResult OnGetSearchBrandName(string term)
        {
            if (term == null)
            {
                return BadRequest();
            }
            var data = _context.TblBrands
                .Where(p => p.Name.Contains(term) && p.IsActive == true)
                 .Select(s => new SelectListItem
                 {
                     Value = s.Name,
                     Text = s.Id.ToString()
                 })
                .ToArray();
            return new JsonResult(data);
        }


        public IActionResult OnGetSearchBPName(string term)
        {
            if (term == null)
            {
                return BadRequest();
            }
            var data = _context.TblBusinessPartners
                .Where(p => p.Name.Contains(term))
                 .Select(s => new SelectListItem
                 {
                     Value = s.Name,
                     Text = s.BusinessPartnerId.ToString()
                 })
                .ToArray();
            return new JsonResult(data);
        }

    }
}

