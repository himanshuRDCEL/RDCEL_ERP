using DocumentFormat.OpenXml.Office2010.Excel;
using Mailjet.Client.Resources;
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
using RDCELERP.Model.BusinessPartner;
using RDCELERP.Model.City;
using RDCELERP.Model.EVC;
using RDCELERP.Model.EVCdispute;

namespace RDCELERP.Core.App.Pages.EVC_Portal
{
    public class EVC_StoreRegistrastionModel : CrudBasePageModel
    {
        #region Variable Declaration       

        private readonly Digi2l_DevContext _context;
        private readonly IEVCManager _EVCManager;
        private readonly IStateRepository _stateRepository;
        private readonly ICityRepository _cityRepository;
        private readonly IEVCPartnerRepository _eVCPartnerRepository;



        #endregion
        public EVC_StoreRegistrastionModel(Digi2l_DevContext context, IOptions<ApplicationSettings> config, CustomDataProtection protector, IEVCManager EVCManager,IStateRepository stateRepository,ICityRepository cityRepository,IEVCPartnerRepository eVCPartnerRepository) : base(config, protector)
        {

            _context = context;
            _EVCManager = EVCManager;
            _stateRepository = stateRepository;
            _cityRepository = cityRepository;
            _eVCPartnerRepository = eVCPartnerRepository;
        }
        [BindProperty(SupportsGet = true)]
        public EVC_StoreRegistrastionViewModel eVC_StoreRegistrastionViewModels { get; set; }

        [BindProperty(SupportsGet = true)]
        public bool Editmode { get; set; }=false;
        public IActionResult OnGet(string id,int userId)
        {
            if (_loginSession == null)
            {
                return RedirectToPage("/index");
            }

            //int loginUserid = _loginSession.UserViewModel.UserId;
            var EVCRegNoList = _EVCManager.GetEvcByUserId(userId);
            if (EVCRegNoList != null)
            {
                eVC_StoreRegistrastionViewModels.EvcregistrationId = EVCRegNoList.EvcregistrationId;

               
                if (id != null)
                {
                    id = _protector.Decode(id);
                    TblEvcPartner? tblEvcPartner = _eVCPartnerRepository.GetSingle(x => x.IsActive == true && x.EvcPartnerId == Convert.ToInt32(id));
                    if (tblEvcPartner != null)
                    {
                        if (tblEvcPartner.ListOfPincode != null && tblEvcPartner.IsApprove == true)
                        {
                            Editmode = true;
                        }
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
                        eVC_StoreRegistrastionViewModels.userId = userId;
                        eVC_StoreRegistrastionViewModels.IsApprove= tblEvcPartner.IsApprove;
                        eVC_StoreRegistrastionViewModels.ListOfPincode = tblEvcPartner.ListOfPincode;

                        TblState? tblState = _stateRepository.GetSingle(x => x.StateId == eVC_StoreRegistrastionViewModels.StateId && x.IsActive == true);
                        eVC_StoreRegistrastionViewModels.State = tblState.Name;

                        TblCity? tblCity = _cityRepository.GetSingle(x => x.CityId == eVC_StoreRegistrastionViewModels.CityId && x.IsActive == true);
                        eVC_StoreRegistrastionViewModels.City = tblCity.Name;


                    }
                }
            }
            
                      
                ViewData["BaseUrl"] = _baseConfig.Value.BaseURL;
                return Page();
            
        }
        public IActionResult OnPostAsync()
        {
            int result = 0;
            string Message;
            int userId = (int)eVC_StoreRegistrastionViewModels.userId;
            if (ModelState.IsValid)
            {
                result = _EVCManager.SaveEVCPartnerDetails(eVC_StoreRegistrastionViewModels, _loginSession.UserViewModel.UserId);
                if (result == 0)
                {
                    Message = "Somthing wont wrong";
                    return RedirectToPage("EVC_PartnerList", new { userId = userId });
                   // return RedirectToPage("EVC_PartnerList");
                }
                else if (result == 1)
                {
                    Message = "Add Store Sucssfully";
                    return RedirectToPage("EVC_PartnerList", new { userId = userId });

                    //return RedirectToPage("EVC_PartnerList");
                }
                else if (result == 2)
                {
                    Message = "EVC Data not fount";
                    return RedirectToPage("EVC_PartnerList", new { userId = userId });

                    //return RedirectToPage("EVC_PartnerList");
                }
                else if (result == 4)
                {
                    Message = "Add Store Sucssfully";
                    return RedirectToPage("/EVC/EVC_PartnerListforAdmin", new { userId = userId });

                    //return RedirectToPage("EVC_PartnerList");
                }
                else
                {
                    return RedirectToPage("EVC_PartnerList", new { userId = userId });

                    //return RedirectToPage("EVC_PartnerList");
                }
            }
            else
            {
                return RedirectToPage("EVC_PartnerList", new { userId = userId });
            }
        }
        public IActionResult OnGetAutoStateName(string term)
        {
            if (term == null)
            {
                return BadRequest();
            }
            var data = _context.TblStates
                       .Where(s => s.Name.Contains(term) && s.IsActive == true)
                       .Select(s => new SelectListItem
                       {
                           Value = s.Name,
                           Text = s.StateId.ToString()
                       })
                       .ToArray();
            return new JsonResult(data);
        }

        public IActionResult OnGetAutoCityName(string term, string term2)
        {
            if (term == null)
            {
                return BadRequest();
            }
            var list = _context.TblCities
                       .Where(e => e.Name.Contains(term) && e.StateId == Convert.ToInt32(term2) && e.IsActive == true)
                        .Select(s => new SelectListItem
                        {
                            Value = s.Name,
                            Text = s.CityId.ToString()
                        })
                       .ToArray();
            return new JsonResult(list);
        }
        public IActionResult OnGetAutoPinCode(int term, int term2)
        {
            if (term == null)
            {
                return BadRequest();
            }
            var list = _context.TblPinCodes
                       .Where(e => e.ZipCode.ToString().Contains(term.ToString()) && e.CityId == term2 && e.IsActive==true)
                        .Select(s => new SelectListItem
                        {
                            Value = s.ZipCode.ToString(),
                            Text = s.Id.ToString()
                        })
                       .ToArray();
            return new JsonResult(list);
        }

    }
}
