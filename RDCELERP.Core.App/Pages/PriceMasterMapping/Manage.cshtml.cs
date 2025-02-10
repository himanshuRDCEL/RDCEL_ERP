using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Options;
using RDCELERP.BAL.Interface;
using RDCELERP.BAL.MasterManager;
using RDCELERP.Common.Helper;
using RDCELERP.Core.App.Pages.Base;
using RDCELERP.DAL.Entities;
using RDCELERP.DAL.IRepository;
using RDCELERP.DAL.Repository;
using RDCELERP.Model.Base;
using RDCELERP.Model.BusinessPartner;
using RDCELERP.Model.BusinessUnit;
using RDCELERP.Model.Company;
using RDCELERP.Model.Master;
using RDCELERP.Model.PriceMaster;

namespace RDCELERP.Core.App.Pages.PriceMasterMapping
{
    public class ManageModel : CrudBasePageModel
    {
        #region Variable Declaration
        private readonly IProductTypeManager _ProductTypeManager;
        private readonly IProductCategoryManager _productCategoryManager;
        private readonly IPriceMasterMappingManager _priceMasterMappingManager;
        private readonly RDCELERP.DAL.Entities.Digi2l_DevContext _context;
        private readonly IBusinessUnitManager _BusinessUnitManager;
        private readonly ICommonManager _commonManager;
        private readonly IPriceMasterMappingRepository _priceMasterMappingRepository;
        private readonly IBrandManager _brandManager;
        IBusinessPartnerManager _businessPartnerManager;
        private readonly IMapper _mapper;
       
        private readonly ILogging _logging;

        #endregion

        public ManageModel(IProductTypeManager productTypeManager, IProductCategoryManager productCategoryManager, IPriceMasterMappingManager priceMasterMappingManager, Digi2l_DevContext context, IBusinessUnitManager BusinessUnitManager, ICommonManager commonManager, IPriceMasterMappingRepository priceMasterMappingRepository, IBrandManager brandManager, IBusinessPartnerManager businessPartnerManager, IMapper mapper,ILogging logging, IPriceMasterNameRepository priceMasterNameRepository,IOptions<ApplicationSettings> config, CustomDataProtection protector) : base(config, protector)
        {
            _ProductTypeManager = productTypeManager;
            _productCategoryManager = productCategoryManager;
            _priceMasterMappingManager = priceMasterMappingManager;
            _context = context;
            _BusinessUnitManager = BusinessUnitManager;
            _commonManager = commonManager;
            _priceMasterMappingRepository = priceMasterMappingRepository;
            _brandManager = brandManager;
            _businessPartnerManager = businessPartnerManager;
            _mapper = mapper;
           
            _logging = logging;
        }

        [BindProperty(SupportsGet = true)]
        public PriceMasterMappingViewModel priceMasterMappingViewModel { get; set; }

        TblPriceMasterMapping tblPriceMasterMapping = null;

        public IActionResult OnGet(string id)
        {
            if (id != null)
            {
                id = _protector.Decode(id);
                priceMasterMappingViewModel = _priceMasterMappingManager.GetPriceMasterMappingById(Convert.ToInt32(id));
                var company = _context.TblBusinessUnits.Where(x => x.BusinessUnitId == priceMasterMappingViewModel.BusinessUnitId).FirstOrDefault();
                if(company != null)
                {
                    priceMasterMappingViewModel.BusinessUnitName = company.Name;
                }
                var businesspartner = _context.TblBusinessPartners.Where(x => x.BusinessPartnerId == priceMasterMappingViewModel.BusinessPartnerId).FirstOrDefault();
                if (businesspartner != null)
                {
                    priceMasterMappingViewModel.BusinessPartnerName = businesspartner.Name;
                }
                var Brands = _context.TblBrands.Where(x => x.Id == priceMasterMappingViewModel.BrandId).FirstOrDefault();
                if(Brands != null)
                {
                    priceMasterMappingViewModel.BrandName = Brands.Name;
                }
                var PriceMastername = _context.TblPriceMasterNames.Where(x => x.PriceMasterNameId == priceMasterMappingViewModel.PriceMasterNameId).FirstOrDefault();
                if (PriceMastername != null)
                {
                    priceMasterMappingViewModel.PriceMasterName = PriceMastername.Name;
                }
            }

            if (_loginSession == null)
            {
                return RedirectToPage("/index");
            }
            else
            {
                return Page();
            }

        }

        public async Task<IActionResult> OnPostAsync()
        {
            int result = 0;
            RDCELERP.DAL.Entities.TblPriceMasterMapping priceMasterMapping = null;
            try
            {
                if (ModelState.IsValid)
                {

                    if (ModelState.IsValid)
                    {
                        result = _priceMasterMappingManager.ManagePriceMasterMapping(priceMasterMappingViewModel, _loginSession.UserViewModel.UserId);
                    }  
                }
                else
                {
                    result = 0;
                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("PriceMasterMapping_ManageModel", "Post", ex);
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
                .Where(p => p.Name.Contains(term))
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
                .Where(p => p.Name.Contains(term))
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



        public IActionResult OnGetSearchPriceMasterName(string term)
        {
            if (term == null)
            {
                return BadRequest();
            }
            var data = _context.TblPriceMasterNames
                .Where(p => p.Name.Contains(term))
                 .Select(s => new SelectListItem
                 {
                     Value = s.Name,
                     Text = s.PriceMasterNameId.ToString()
                 })
                .ToArray();
            return new JsonResult(data);
        }

    }


    }

