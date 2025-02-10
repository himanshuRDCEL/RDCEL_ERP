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

namespace RDCELERP.Core.App.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly IUserManager _userManager;
        private readonly IRoleManager _roleManager;
        public readonly IOptions<ApplicationSettings> _config;
        public IndexModel(ILogger<IndexModel> logger, IUserManager userManager, IRoleManager roleManager, IOptions<ApplicationSettings> config)
        {
            _logger = logger;
            _userManager = userManager;
            _roleManager = roleManager;
            _config = config;
        }

        [BindProperty(SupportsGet = true)]
        public UserLoginModel UserViewModel { get; set; }
        public IActionResult OnGet()
        {
            LoginViewModel loginVM = SessionHelper.GetObjectFromJson<LoginViewModel>(HttpContext.Session, "LoginUser");
            if (loginVM != null)
            {
                loginVM.RoleViewModel = _roleManager.GetRoleByUserId(loginVM.UserViewModel.UserId);
                SessionHelper.SetObjectAsJson(HttpContext.Session, "LoginUser", loginVM);

                return RedirectToPage("/Company/SelectCompany");
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

                LoginViewModel loginVM = _userManager.GetUserByLogin(UserViewModel.Email.Trim(), UserViewModel.Password);

                if (loginVM != null && loginVM.UserViewModel != null && loginVM.UserViewModel.UserId != 0)
                {
                    loginVM.RoleViewModel = _roleManager.GetRoleByUserId(loginVM.UserViewModel.UserId);
                    if(loginVM.RoleViewModel != null && loginVM.RoleViewModel.RoleId > 0)
                    {
                        SessionHelper.SetObjectAsJson(HttpContext.Session, "LoginUser", loginVM);

                        //if (loginVM.RoleViewModel.RoleName == Common.Constant.RoleConstant.RoleEVPPortal)
                        //    return new RedirectToPageResult("/EVC_Portal/EVC_Dashboard");
                        //else if (loginVM.RoleViewModel.RoleName == Common.Constant.RoleConstant.RoleLGCAdmin)
                        //    return new RedirectToPageResult("/LGC/LogiPickDrop");
                        //else
                        //   return new RedirectToPageResult("Company/SelectCompany");

                        return new RedirectToPageResult("Company/SelectCompany");
                    }
                    else
                    {
                        TempData["Auth"] = false;
                        return new RedirectToPageResult("Index");
                    }
                }
                else
                {
                    TempData["Auth"] = false;
                    return new RedirectToPageResult("Index");
                }
            }
            else
            {
                TempData["Auth"] = false;
                return new RedirectToPageResult("Index");
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
