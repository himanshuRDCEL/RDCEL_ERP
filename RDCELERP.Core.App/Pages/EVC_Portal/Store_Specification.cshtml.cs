using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Options;
using RDCELERP.BAL.Interface;
using RDCELERP.BAL.MasterManager;
using RDCELERP.Core.App.Pages.Base;
using RDCELERP.DAL.Entities;
using RDCELERP.DAL.IRepository;
using RDCELERP.Model.Base;
using RDCELERP.Model.DriverDetails;
using RDCELERP.Model.EVC;
using RDCELERP.Model.EVC_Portal;
using RDCELERP.Model.SearchFilters;

namespace RDCELERP.Core.App.Pages.EVC_Portal
{
    public class Store_SpecificationModel : BasePageModel
    {
        #region Variable declartion
        private readonly IEVCPartnerRepository _eVCPartnerRepository;
        private readonly IProductCategoryManager _productCategoryManager;
        private readonly ITemplateConfigurationRepository _configurationRepository;
        private readonly IEVCManager _EVCManager;
        private readonly RDCELERP.DAL.Entities.Digi2l_DevContext _context;
        #endregion

        #region Constructor
        public Store_SpecificationModel(IOptions<ApplicationSettings> config, IEVCPartnerRepository eVCPartnerRepository, IProductCategoryManager productCategoryManager,ITemplateConfigurationRepository configurationRepository, IEVCManager eVCManager, Digi2l_DevContext context) : base(config)
        {
            _eVCPartnerRepository = eVCPartnerRepository;
            _productCategoryManager = productCategoryManager;
            _configurationRepository = configurationRepository;
            _EVCManager = eVCManager;
            _context = context;
        }
        #endregion

        [BindProperty(SupportsGet = true)]
        public EVC_PartnerViewModel eVC_PartnerListModels { get; set; }

        [BindProperty(SupportsGet = true)]
        public EVCStore_SpecificationViewModel eVCStore_SpecificationViewModel { get; set; }        
        [BindProperty(SupportsGet = true)]
        public TblEvcPartner  tblEvcPartner { get; set; }
        [BindProperty(SupportsGet = true)]
        public TblEvcPartnerPreference tblEvcPartnerPreference { get; set; }
        [BindProperty(SupportsGet = true)]
        public SearchFilterViewModel searchFilterVM { get; set; }
        public IActionResult OnGet(int id)
        {                     
            if (_loginSession != null && id>0)
            {
                tblEvcPartner = _eVCPartnerRepository.GetEVCPartnerDetails(id);
                if (tblEvcPartner != null)
                {      
                    eVC_PartnerListModels.EvcStoreCode = tblEvcPartner.EvcStoreCode;
                    eVC_PartnerListModels.EvcPartnerId = tblEvcPartner.EvcPartnerId;
                    eVC_PartnerListModels.EvcregdNo = tblEvcPartner.Evcregistration.EvcregdNo;
                    eVC_PartnerListModels.StateName = tblEvcPartner.State.Name;
                    eVC_PartnerListModels.CityName = tblEvcPartner.City.Name;
                    eVC_PartnerListModels.PinCode = tblEvcPartner.PinCode;
                    eVC_PartnerListModels.EmailId = tblEvcPartner.EmailId;
                    eVC_PartnerListModels.ContactNumber = tblEvcPartner.ContactNumber;
                    eVC_PartnerListModels.Address = tblEvcPartner.Address1+" "+ tblEvcPartner.Address2;
                    eVC_PartnerListModels.BussinessName= tblEvcPartner.Evcregistration.BussinessName;
                    eVCStore_SpecificationViewModel.EvcPartnerId = tblEvcPartner.EvcPartnerId;
                }
            }
            #region get ProductCategorylist
            var ProductCategorylist = _productCategoryManager.GetAllProductCategory();
            if (ProductCategorylist != null)
            {
                ViewData["ProductCategorylist"] = new SelectList(ProductCategorylist, "Id", "Description");
            }            
            #endregion

            #region get Quality
            List<TblConfiguration> tblConfigurationList = _configurationRepository.GetList(x => x.IsActive == true && (x.Name == "Not Working" || x.Name == "Average" || x.Name == "Good" || x.Name == "Excellent")).ToList();
            if(tblConfigurationList != null)
            {
                ViewData["Quality"] = new SelectList(tblConfigurationList, "Value", "Name");
            }
            #endregion
            return Page();
        }

        public IActionResult OnPost()
        {
            int result = 0;
            int id = eVCStore_SpecificationViewModel.EvcPartnerId;
            string Message;           
                result = _EVCManager.SaveStoreSpecificationDetails(eVCStore_SpecificationViewModel, _loginSession.UserViewModel.UserId);
                if (result == 0)
                {
                return RedirectToPage("Store_Specification", new { id = id });
                Message = "Somthing wont wrong";
                   // return RedirectToPage("Store_Specification");
                }
                else if (result == 1)
                {
                    Message = "Add Store Sucssfully";
                return RedirectToPage("Store_Specification", new { id = id });

               // return RedirectToPage("Store_Specification");
                }
                else if (result == 2)
                {
                    Message = "EVC Data not fount";
                return RedirectToPage("Store_Specification", new { id = id });

               // return RedirectToPage("Store_Specification");
                }
                else
                {
                return RedirectToPage("Store_Specification", new { id = id });

               // return RedirectToPage("Store_Specification");
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
                if (eVCStore_SpecificationViewModel != null && eVCStore_SpecificationViewModel.EvcPartnerpreferenceId > 0)
                {
                    tblEvcPartnerPreference = _context.TblEvcPartnerPreferences.Find(eVCStore_SpecificationViewModel.EvcPartnerpreferenceId);
                }

                if (tblEvcPartnerPreference != null)
                {

                    if (tblEvcPartnerPreference.IsActive == true)
                    {
                        tblEvcPartnerPreference.IsActive = false;
                    }
                    else
                    {
                        tblEvcPartnerPreference.IsActive = true;
                    }
                    tblEvcPartnerPreference.ModifiedBy = _loginSession.UserViewModel.UserId;
                    _context.TblEvcPartnerPreferences.Update(tblEvcPartnerPreference);
                    //  _context.TblRoles.Remove(TblRole);
                    _context.SaveChanges();
                    int id = tblEvcPartnerPreference.EvcpartnerId;
                    return RedirectToPage("Store_Specification", new { id = id });
                }

                return RedirectToPage("EVC_PartnerList");
            }
        }

    
        public IActionResult OnGetQualityOptions(int productCategoryId,int evcPartnerId)
        {
            // Assuming you have entities named Quality and ProductCategory
            var qualityOptions = _context.TblEvcPartnerPreferences
                .Where(x=>x.IsActive==true&&x.EvcpartnerId== evcPartnerId&&x.ProductCatId== productCategoryId)
                .Select(x=>x.ProductQualityId)
                .ToArray();
           
                var quality = qualityOptions.ToArray();
            
            return new JsonResult(quality);
           
        }
    }
}
