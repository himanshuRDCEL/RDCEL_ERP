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
using RDCELERP.Model.Base;
using RDCELERP.Model.ExchangeOrder;
using RDCELERP.Model.Refurbisher;
using static Org.BouncyCastle.Math.EC.ECCurve;

namespace RDCELERP.Core.App.Refurbisher
{
    public class RefurbisherRegistrationModel : BasePageModel
    {
        #region Variable
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ILogging _logger;
        private readonly ICityManager _cityManager;
        private readonly IStateManager _stateManager;
        private readonly IEVCManager _EVCManager;
        private readonly IOptions<ApplicationSettings> _config;
        private readonly Digi2l_DevContext _context;
        ICommonManager _commonManager;
        IRefurbisherManager _refurbisherManager;
        IRefurbisherRepository _refurbisherRepository;
        private readonly IMapper _mapper;
        #endregion

        #region Constructor
        public RefurbisherRegistrationModel(IOptions<ApplicationSettings> baseConfig, Digi2l_DevContext context, IWebHostEnvironment webHostEnvironment, ILogging logger, ICityManager cityManager, IStateManager stateManager, IEVCManager EVCManager, ICommonManager commonManager, IRefurbisherManager refurbisherManager, IRefurbisherRepository refurbisherRepository, IMapper mapper)
            : base(baseConfig)
        {
            _webHostEnvironment = webHostEnvironment;
            _logger = logger;
            _config = baseConfig;
            _cityManager = cityManager;
            _stateManager = stateManager;
            _context = context;
            _EVCManager = EVCManager;
            _commonManager = commonManager;
            _refurbisherManager = refurbisherManager;
            _refurbisherRepository = refurbisherRepository;
            _mapper = mapper;
        }
        #endregion

        #region Model
        [BindProperty(SupportsGet = true)]
        public RefurbisherRegViewModel? refurbisherRegViewModel { get; set; }
        public TblRefurbisherRegistration TblRefurbisherRegistration { get; set; }
        #endregion
        public IActionResult OnGet(int? id)
        {
            if (id > 0)
            {
                refurbisherRegViewModel.RefurbisherId = id;
                TblRefurbisherRegistration = _refurbisherRepository.GetSingleOrder(id);
                refurbisherRegViewModel = _mapper.Map<TblRefurbisherRegistration, RefurbisherRegViewModel>(TblRefurbisherRegistration);

            }
            return Page();
        }

        public IActionResult OnPost()
        {
            string URL = _config.Value.URLPrefixforProd;
            if (refurbisherRegViewModel != null && ModelState.IsValid)
            {
                int userid = _loginSession.UserViewModel != null ? _loginSession.UserViewModel.UserId : 3;
                var result = _refurbisherManager.ManageRefurbisher(refurbisherRegViewModel, _loginSession.UserViewModel.UserId);
                if (result > 0)
                {
                    return RedirectToPage("RefurbisherDetails");
                }                                
                else
                {
                    return RedirectToPage("RefurbisherRegistration");
                }
            }
            else
            {
                return RedirectToPage("RefurbisherRegistration");
            }
        }

        //public JsonResult OnGetCityByStateIdAsync()
        //{
        //    var citylistS = _cityManager.GetCityBYStateID(refurbisherRegViewModel.StateId);
        //    if (citylistS != null)
        //    {
        //        ViewData["citylistS"] = new SelectList(citylistS, "CityId", "Name");
        //    }
        //    return new JsonResult(citylistS);
        //}

        //public JsonResult OnGetPincodebycityidAsync()
        //{
        //    var pincodeList = _context.TblPinCodes.Where(x => x.IsActive == true && x.CityId == refurbisherRegViewModel.cityId).ToList();
        //    if (pincodeList != null)
        //    {
        //        ViewData["pincodeList"] = new SelectList(pincodeList, "ZipCode", "ZipCode");
        //    }
        //    return new JsonResult(pincodeList);
        //}

        public IActionResult OnPostCheckEmail(string email)
        {
            string emailTrimmed = string.Empty;
            TblUser TblUser = new TblUser();

            if (!string.IsNullOrEmpty(email))
            {
                emailTrimmed = email.Trim().ToLower();
            }

            var email1 = SecurityHelper.EncryptString(emailTrimmed, _config.Value.SecurityKey);
            bool isValidUser = !_context.TblUsers.ToList().Exists(p => (p.Email ?? "") == email1);
            if (isValidUser)
            {
                bool isValidEVC = !_context.TblEvcregistrations.ToList().Exists(p => (p.EmailId ?? "").Equals(email, StringComparison.CurrentCultureIgnoreCase));
                return new JsonResult(isValidEVC);
            }
            else
            {
                return new JsonResult(isValidUser);
            }
        }

        #region AutoFill To Get State List Added By PJ
        public IActionResult OnGetSearchState(string term)
        {
            if (term == null)
            {
                return BadRequest();
            }
            var data = _context.TblStates
                       .Where(s => s.Name.Contains(term) || term == "#")
                       .Select(s => new SelectListItem
                       {
                           Value = s.Name,
                           Text = s.StateId.ToString()
                       })
                       .ToArray();
            return new JsonResult(data);
        }

        public IActionResult OnGetCheckState(string Statename)
        {
            IList<string>? list = null;
            list = _commonManager.CheckStateisValid(Statename);
            return new JsonResult(list);
        }
        #endregion

        #region AutoFill To Get City List Added By PJ

        public IActionResult OnGetCheckstatename(string statename)
        {
            TblState tblState = null;
            if (statename != null)
            {
                tblState = _context.TblStates.FirstOrDefault(x => x.IsActive == true && x.Name == statename);
            }
            return new JsonResult(tblState);
        }
        public IActionResult OnGetSearchCity(string term, string term2, string term3 = null)
        {
            if (term == null)
            {
                return BadRequest();
            }
            term = term.TrimStart(); ///use to trim blank space before character

            if (term == "#")
            {
                var list = _context.TblCities
                     .Where(s => s.StateId == Convert.ToInt32(term2))
                     .Select(s => new SelectListItem
                     {
                         Value = s.Name,
                         Text = s.CityId.ToString()
                     })
                     .ToArray();
                return new JsonResult(list);
            }
            else
            {
                var list = _context.TblCities
                        .Where(e => e.Name.Contains(term) && e.StateId == Convert.ToInt32(term2))
                        .Select(s => new SelectListItem
                        {
                            Value = s.Name,
                            Text = s.CityId.ToString()
                        })
                        .ToArray();
                return new JsonResult(list);
            }
        }

        public IActionResult OnGetCheckCity(string cityname, int StateId)
        {
            IList<string>? list = null;
            list = _commonManager.CheckCityisValid(cityname, StateId);
            return new JsonResult(list);
        }

        #endregion

        #region AutoFill To Get Pincode List Added By PJ
        public IActionResult OnGetSearchPincode(string term, string term2, string term3)
        {
            if (term == null)
            {
                return BadRequest();
            }
            term = term.TrimStart(); ///use to trim blank space before character

            if (term == "#")
            {
                var list = _context.TblPinCodes
                     .Where(s => s.IsActive == true && s.State == term2 && s.Location == term3)
                     .Select(s => new SelectListItem
                     {
                         Value = s.ZipCode.ToString(),
                         Text = s.Id.ToString()
                     })
                     .ToArray();
                return new JsonResult(list);
            }
            else
            {
                IEnumerable<SelectListItem>? pincodeList = null;
                var list = _context.TblPinCodes
                        .Where(e => e.IsActive == true && e.State == term2 && e.Location == term3)
                        //.Where(e => e.ZipCode == Convert.ToInt32(term) && e.IsActive == true && e.State == term2 && e.Location == term3)
                        .Select(s => new SelectListItem
                        {
                            Value = s.ZipCode.ToString(),
                            Text = s.Id.ToString()
                        })
                        .ToArray();
                pincodeList = list.OrderBy(o => o.Text).ToList();
                pincodeList = pincodeList.Where(x => x.Value.Contains(term)).ToList();
                return new JsonResult(pincodeList);
            }
        }

        public IActionResult OnGetCheckPincode(int Pincode, string Editcityname, string Editstatename)
        {
            TblPinCode tblPinCode = null;
            tblPinCode = _commonManager.CheckPincodeValid(Pincode, Editcityname, Editstatename);
            return new JsonResult(tblPinCode);
        }

        #endregion

        #region Edit Refurbisher order details 
        public JsonResult OnGetEditRefurbisherDetailsAsync(int id)
        {
            if (id > 0)
            {
                TblRefurbisherRegistration = _refurbisherRepository.GetSingleOrder(id);
                if (TblRefurbisherRegistration != null)
                {
                    refurbisherRegViewModel = _mapper.Map<TblRefurbisherRegistration, RefurbisherRegViewModel>(TblRefurbisherRegistration);                    
                }
            }
            return new JsonResult(refurbisherRegViewModel);
        }
        #endregion
    }
}
