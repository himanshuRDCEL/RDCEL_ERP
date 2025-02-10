using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Options;
using NPOI.SS.Formula.Functions;
using RDCELERP.BAL.Helper;
using RDCELERP.BAL.Interface;
using RDCELERP.BAL.MasterManager;
using RDCELERP.Common.Constant;
using RDCELERP.Common.Helper;
using RDCELERP.Core.App.Pages.Base;
using RDCELERP.Core.App.Pages.PinCode;
using RDCELERP.DAL.Entities;
using RDCELERP.Model.Base;
using RDCELERP.Model.City;
using RDCELERP.Model.EVC;
using RDCELERP.Model.Users;

namespace RDCELERP.Core.App.Pages
{


    public class EVCOnboardingModel : PageModel
    {
        #region Variable Declaration
        private readonly IEVCManager _EVCManager;
        private readonly IUserManager _UserManager;
        private readonly IStateManager _stateManager;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ICityManager _cityManager;
        private readonly IEntityManager _entityManager;
        private readonly RDCELERP.DAL.Entities.Digi2l_DevContext _context;
        public readonly IOptions<ApplicationSettings> _baseConfig;
        INotificationManager _notificationManager;

        #endregion
        public EVCOnboardingModel(IOptions<ApplicationSettings> baseConfig, IEVCManager EVCManager, Digi2l_DevContext context, IStateManager StateManager, ICityManager CityManager, IEntityManager EntityManager, IWebHostEnvironment webHostEnvironment, IOptions<ApplicationSettings> config, IUserManager userManager, CustomDataProtection protector, INotificationManager notificationManager)
        {
            _EVCManager = EVCManager;
            _webHostEnvironment = webHostEnvironment;
            _UserManager = userManager;
            _stateManager = StateManager;
            _cityManager = CityManager;
            _entityManager = EntityManager;
            _context = context;
            _baseConfig = baseConfig;
            _notificationManager = notificationManager;
        }

        [BindProperty(SupportsGet = true)]
        public EVC_RegistrationModel evc_RegistrationModel { get; set; }
        public int stateId;
        public List<CityViewModel> citylist = new List<CityViewModel>();
        public UserViewModel UserViewModel { get; set; }
        public IActionResult OnGet()
        {

            string URLPrefixforProd = _baseConfig.Value.URLPrefixforProd;
            ViewData["URLPrefixforProd"] = URLPrefixforProd;

            string InvoiceImageURL = _baseConfig.Value.InvoiceImageURL;
            ViewData["InvoiceImageURL"] = InvoiceImageURL;
            ViewData["BaseUrl"] = _baseConfig.Value.BaseURL;

            var Statelist = _stateManager.GetAllState();
            if (Statelist != null)
            {
                ViewData["Statelist"] = new SelectList(Statelist, "StateId", "Name");
            }
            var EntityList = _entityManager.GetAllEntity();
            if (EntityList != null)
            {
                ViewData["EntityList"] = new SelectList(EntityList, "EntityTypeId", "Name");
            }
            return Page();
        }
        public IActionResult OnPostAsync()
        {
            int result = 0;
            int UserId = 3;

            if (evc_RegistrationModel != null)
            {
                result = _EVCManager.SaveEVCDetails(evc_RegistrationModel, UserId);
                if (result == 1)
                {
                    ViewData["Message"] = "Created Successfully";
                    return RedirectToPage("Details");
                }
                if (result == 0)
                {
                    ViewData["Message"] = "Registration Failed";
                    return RedirectToPage("EVCOnboarding");
                }

                else
                    return RedirectToPage("EVCOnboarding");
            }
            return RedirectToPage("EVCOnboarding");

        }

        public JsonResult OnGetCityByStateIdAsync()
        {
            var citylistS = _cityManager.GetCityBYStateID(evc_RegistrationModel.StateId);
            if (citylistS != null)
            {
                ViewData["citylistS"] = new SelectList(citylistS, "CityId", "Name");
            }
            return new JsonResult(citylistS);
        }

        public JsonResult OnGetPincodebycityidAsync()
        {
            var pincodeList = _context.TblPinCodes.Where(x => x.IsActive == true && x.CityId == evc_RegistrationModel.cityId).ToList();
            if (pincodeList != null)
            {
                ViewData["pincodeList"] = new SelectList(pincodeList, "ZipCode", "ZipCode");
            }
            return new JsonResult(pincodeList);
        }

        public JsonResult OnGetEmpDetailsByEmpIdAsync()
        {
            return new JsonResult(_UserManager.UserById(evc_RegistrationModel.EmployeeId));
        }
        //Email Validation[FromBody]
        public IActionResult OnPostCheckEmail(string email)
        {

            //bool isValid = !_context.TblEvcregistrations.ToList().Exists(p => p.EmailId.Equals(email, StringComparison.CurrentCultureIgnoreCase));
            //return new JsonResult(isValid);


            TblUser TblUser = new TblUser();
            string emailTrimmed = email?.Trim().ToLower();
            var email1 = SecurityHelper.EncryptString(emailTrimmed, _baseConfig.Value.SecurityKey);
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
        public IActionResult OnPostSendOTP(string mobnumber, string tempaltename)
        {

            bool flag = false;
            string message = string.Empty;
            string contactNumber = "+919321276341";
            string OTPValue = UniqueString.RandomNumber();
            if (tempaltename.Equals("SMS_EVCRegistration_OTP"))
                message = NotificationConstants.SMS_EVCRegistration_OTP.Replace("[OTP]", OTPValue).Replace("[Contact]", contactNumber);
            flag = _notificationManager.SendNotificationSMS(mobnumber, message, OTPValue);
            return new JsonResult(flag);
        }
        public IActionResult OnPostVerifyOTP(string mobnumber, string OTP)
        {
            bool flag = false;
            string message = string.Empty;
            flag = _notificationManager.ValidateOTP(mobnumber, OTP);
            return new JsonResult(flag);
        }

    }
}
