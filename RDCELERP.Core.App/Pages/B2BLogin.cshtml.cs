using RDCELERP.BAL.Interface;
using RDCELERP.Common.Enums;
using RDCELERP.Core.App.Helper;
using RDCELERP.Model.Users;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RDCELERP.Common.Helper;
using Microsoft.Extensions.Options;
using RDCELERP.Model.Base;
using RDCELERP.Common.Constant;

namespace RDCELERP.Core.App.Pages
{
    public class B2BLoginModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly IUserManager _userManager;
        private readonly IBusinessCustomerManager _businessCustManager;
        private readonly IRoleManager _roleManager;
        public readonly IOptions<ApplicationSettings> _config;
        public B2BLoginModel(ILogger<IndexModel> logger, IUserManager userManager, IRoleManager roleManager, IBusinessCustomerManager businessCustManager, IOptions<ApplicationSettings> config)
        {
            _logger = logger;
            _userManager = userManager;
            _roleManager = roleManager;
            _config = config;
            _businessCustManager = businessCustManager;
        }

        [BindProperty(SupportsGet = true)]
        public UserLoginModel UserViewModel { get; set; }
        public IActionResult OnGet()
        {
            LoginViewModel loginVM = SessionHelper.GetObjectFromJson<LoginViewModel>(HttpContext.Session, "LoginUser");
            if (loginVM != null)
            {
                if (loginVM.UserViewModel != null)
                {
                    loginVM.RoleViewModel = _roleManager.GetRoleByUserId(loginVM.UserViewModel.UserId);
                }
                SessionHelper.SetObjectAsJson(HttpContext.Session, "LoginUser", loginVM);

                return RedirectToPage("B2BLogin");
                //Redirect("Company/SelectCompany");
                /*  RedirectToPage("Index");*/
            }

            bool auth = true;
            if (TempData["Auth"] != null)
                auth = (Boolean)TempData["Auth"];
            if (!auth)
            {
                //Write code to show message
                ShowMessage("Invalid Credential", MessageTypeEnum.error);
            }
            return Page();
        }

        public IActionResult OnPost()
        {
            if (!string.IsNullOrEmpty(UserViewModel.Email) && !string.IsNullOrEmpty(UserViewModel.Password))
            {

                UserViewModel.Email = SecurityHelper.EncryptString(UserViewModel.Email, _config.Value.SecurityKey);
                UserViewModel.Password = SecurityHelper.EncryptString(UserViewModel.Password, _config.Value.SecurityKey);

                LoginViewModel loginVM = _businessCustManager.GetCustomerByLogin(UserViewModel.Email.Trim(), UserViewModel.Password);

                if (loginVM != null && loginVM.BusinessCustomerViewModel != null && loginVM.BusinessCustomerViewModel.BusinessCustomerId != 0)
                {

                    SessionHelper.SetObjectAsJson(HttpContext.Session, "LoginUser", loginVM);

                    return new RedirectToPageResult("BusinessCustomerDashboard/Dashboard");


                }
                else
                {
                    TempData["Auth"] = false;
                    return new RedirectToPageResult("B2BLogin");
                }
            }
            else
            {
                TempData["Auth"] = false;
                return new RedirectToPageResult("B2BLogin");
            }

        }

        public JsonResult OnGetForgotPassword()
        {

            string replyMessage = string.Empty;
            int result = 0;
            if (!string.IsNullOrEmpty(UserViewModel.Email))
            {
                string pwd = StringHelper.RandomStrByLength(6);
                string encpwd = SecurityHelper.EncryptString(pwd, _config.Value.SecurityKey);
                UserViewModel.Email = SecurityHelper.EncryptString(UserViewModel.Email, _config.Value.SecurityKey);
                result = _userManager.ForgotPassword(UserViewModel.Email, encpwd, pwd);
                if (result > 0)
                {
                    //Code to wrtite generate random new password and send mail
                    replyMessage = "Password has been sent to your registered email address.";
                }
                else if (result == -1)
                {
                    replyMessage = "Invalid Email address.";
                }

            }
            else
            {

                replyMessage = "Invalid Email address.";
            }


            return new JsonResult(replyMessage);

        }

        public void ShowMessage(string message, MessageTypeEnum messageType)
        {
            ViewData["MessageType"] = messageType;
            ModelState.AddModelError(string.Empty, message);
        }
    }
}
