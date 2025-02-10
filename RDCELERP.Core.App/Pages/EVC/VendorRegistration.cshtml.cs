using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Options;
using System.Text.RegularExpressions;
using RDCELERP.BAL.Interface;
using RDCELERP.BAL.MasterManager;
using RDCELERP.Common.Helper;
using RDCELERP.Core.App.Pages.Base;
using RDCELERP.DAL.Entities;
using RDCELERP.Model.Base;
using RDCELERP.Model.EVC;

namespace RDCELERP.Core.App.Pages.EVC
{
    public class VendorRegistration : PageModel
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ILogging _logger;
        private readonly ICityManager _cityManager;
        private readonly IStateManager _stateManager;
        private readonly IEVCManager _EVCManager;
        private readonly IOptions<ApplicationSettings> _config;
        private readonly Digi2l_DevContext _context; 
        public VendorRegistration(IOptions<ApplicationSettings> baseConfig,Digi2l_DevContext context ,IWebHostEnvironment webHostEnvironment, ILogging logger, ICityManager cityManager, IStateManager stateManager,IEVCManager EVCManager)
        {
            _webHostEnvironment = webHostEnvironment;
            _logger = logger;
            _config = baseConfig;
            _cityManager = cityManager;
            _stateManager = stateManager;
            _context = context;
            _EVCManager = EVCManager;
        }

        [BindProperty(SupportsGet=true)]
        public VendorRegistrationModel? vendorRegistrationModel { get; set; }
        public IActionResult OnGet()
        {
            string URLPrefixforProd = _config.Value.URLPrefixforProd;
            ViewData["URLPrefixforProd"] = URLPrefixforProd;

            if (vendorRegistrationModel == null)
            {
                vendorRegistrationModel = new VendorRegistrationModel();
            }

            var Statelist = _stateManager.GetAllState();
            if (Statelist != null)
            { 
                ViewData["Statelist"] = new SelectList(Statelist, "StateId", "Name");
            }

            return Page();
        }


        public IActionResult OnPost()
        {
            string returnUrl = _config.Value.BaseURL + "EVC/VendorRegistration";

            if (vendorRegistrationModel != null && ModelState.IsValid)
            {
                int userid = 3;
                var result = _EVCManager.SaveVendorDetails(vendorRegistrationModel, userid);

                if (result)
                {
                    ViewData["Message"] = "Vendor Registered Successfully";
                    return RedirectToPage("VendorDetails");
                }
                else
                {
                    ViewData["Message"] = "Registration Failed";
                    return RedirectToPage("VendorRegistration");
                }
            }
            else
            {
                ViewData["Message"] = "Registration Failed";
                return RedirectToPage("VendorRegistration");
            }
        }


        public JsonResult OnGetCityByStateIdAsync()
        {
            var citylistS = _cityManager.GetCityBYStateID(vendorRegistrationModel.StateId);
            if (citylistS != null)
            {
                ViewData["citylistS"] = new SelectList(citylistS, "CityId", "Name");
            }
            return new JsonResult(citylistS);
        }

        public JsonResult OnGetPincodebycityidAsync()
        {
            var pincodeList = _context.TblPinCodes.Where(x => x.IsActive == true && x.CityId == vendorRegistrationModel.cityId).ToList();
            if (pincodeList != null)
            {
                ViewData["pincodeList"] = new SelectList(pincodeList, "ZipCode", "ZipCode");
            }
            return new JsonResult(pincodeList);
        }

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
    }
}
