using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using NPOI.Util;
using RDCELERP.BAL.Interface;
using RDCELERP.Core.App.Pages.Base;
using RDCELERP.DAL.Entities;
using RDCELERP.DAL.IRepository;
using RDCELERP.Model.Base;
using RDCELERP.Model.EVC;
using RDCELERP.Model.EVC_Portal;
using RDCELERP.Model.SearchFilters;

namespace RDCELERP.Core.App.Pages.EVC
{
    public class EVC_PartnerAddListofPincdeModel : BasePageModel
    {
        #region Variable declartion
        private readonly IEVCPartnerRepository _eVCPartnerRepository;
        private readonly IProductCategoryManager _productCategoryManager;
        private readonly ITemplateConfigurationRepository _configurationRepository;
        private readonly IEVCManager _EVCManager;
        private readonly RDCELERP.DAL.Entities.Digi2l_DevContext _context;
        #endregion

        #region Constructor
        public EVC_PartnerAddListofPincdeModel(IOptions<ApplicationSettings> config, IEVCPartnerRepository eVCPartnerRepository, IProductCategoryManager productCategoryManager, ITemplateConfigurationRepository configurationRepository, IEVCManager eVCManager, Digi2l_DevContext context) : base(config)
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
        public TblEvcPartner tblEvcPartner { get; set; }
        [BindProperty(SupportsGet = true)]
        public TblEvcPartnerPreference tblEvcPartnerPreference { get; set; }
        [BindProperty(SupportsGet = true)]
        public SearchFilterViewModel searchFilterVM { get; set; }

        [BindProperty(SupportsGet = true)]
        public IList<TblPinCode> tblPinCodeList { get; set; }
        [BindProperty(SupportsGet = true)]
        public EVC_StoreRegistrastionViewModel eVC_StoreRegistrastionViewModels { get; set; }
        [BindProperty(SupportsGet = true)]
        //public List<int?> PinCodeList { get; set; }
        public SelectListItem[]? PinCodeList { get; set; }

        public IActionResult OnGet(int id)
        {
            if (_loginSession != null && id > 0)
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
                    eVC_PartnerListModels.Address = tblEvcPartner.Address1 + " " + tblEvcPartner.Address2;
                    eVC_PartnerListModels.BussinessName = tblEvcPartner.Evcregistration.BussinessName;

                    eVC_StoreRegistrastionViewModels.ListOfPincode = tblEvcPartner.ListOfPincode;
                    eVC_StoreRegistrastionViewModels.EvcregistrationId = tblEvcPartner.EvcregistrationId;
                    eVC_StoreRegistrastionViewModels.EvcPartnerId = tblEvcPartner.EvcPartnerId;
                    eVC_StoreRegistrastionViewModels.StateId = tblEvcPartner.StateId;
                    eVC_StoreRegistrastionViewModels.CityId = tblEvcPartner.CityId;
                    eVC_StoreRegistrastionViewModels.PinCode = tblEvcPartner.PinCode;
                    eVC_StoreRegistrastionViewModels.EmailId = tblEvcPartner.EmailId;
                    eVC_StoreRegistrastionViewModels.ContactNumber = tblEvcPartner.ContactNumber;
                    eVC_StoreRegistrastionViewModels.Address1 = tblEvcPartner.Address1;
                    eVC_StoreRegistrastionViewModels.Address2 = tblEvcPartner.Address2;
                    eVC_StoreRegistrastionViewModels.IsActive = tblEvcPartner.IsActive;
                    eVC_StoreRegistrastionViewModels.CreatedBy = tblEvcPartner.Createdby;
                    eVC_StoreRegistrastionViewModels.CreatedDate = tblEvcPartner.CreatedDate;                   
                    eVC_StoreRegistrastionViewModels.IsApprove = tblEvcPartner.IsApprove;
                    // eVC_StoreRegistrastionViewModels.userId = userId;
                }
            }
            var list = _context.TblPinCodes
                      .Where(x => x.IsActive == true && x.CityId == tblEvcPartner.CityId)
                       .Select(s => new SelectListItem
                       {
                           Value = s.ZipCode.ToString(),
                           Text = s.Id.ToString()
                       })
                      .ToArray();
            // Fetch pin codes from your data source
            //var pinCodeData = _context.TblPinCodes
            //    .Where(x => x.IsActive == true && x.CityId == tblEvcPartner.CityId)
            //    .Select(x => x.ZipCode)
            //    .ToList();
            PinCodeList = list;
            //string PinCodeList1 = JsonConvert.SerializeObject(PinCodeList);
            return Page();
        }
        public IActionResult OnPostAsync()
        {
            int result = 0;
            string Message;
            
            eVC_StoreRegistrastionViewModels.IsApprove = true;
                result = _EVCManager.SaveEVCPartnerDetails(eVC_StoreRegistrastionViewModels, _loginSession.UserViewModel.UserId);
                if (result == 0)
                {
                    Message = "Somthing wont wrong";
                  //  return RedirectToPage("EVC_PartnerList", new { userId = userId });
                 return RedirectToPage("EVC_PartnerListforAdmin");
            }
            else if (result == 1)
                {
                    Message = "Add Store Sucssfully";
                  //  return RedirectToPage("EVC_PartnerList", new { userId = userId });

                return RedirectToPage("EVC_PartnerListforAdmin");
            }
            else if (result == 2)
                {
                    Message = "EVC Data not fount";
                // return RedirectToPage("EVC_PartnerList", new { userId = userId });

                return RedirectToPage("EVC_PartnerListforAdmin");
            }
            else
                {
                  //  return RedirectToPage("EVC_PartnerList", new { userId = userId });

                return RedirectToPage("EVC_PartnerListforAdmin");
            }

            //else
            //{
            //    return RedirectToPage("EVC_PartnerList");
            //}
        }

        public JsonResult OnGetGetFilteredPinCodes(string term)
        {
            var filteredPinCodes = PinCodeList
        .Where(x => x.ToString().Contains(term.ToString()))
        .ToList();

            return new JsonResult(filteredPinCodes);
        }

    }
}
