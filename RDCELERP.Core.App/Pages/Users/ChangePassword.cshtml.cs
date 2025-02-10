using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;
using RDCELERP.BAL.Interface;
using RDCELERP.Common.Helper;
using RDCELERP.Core.App.Pages.Base;
using RDCELERP.Model.Base;
using RDCELERP.Model.Users;

namespace RDCELERP.Core.App.Pages.Users
{
    public class ChangePasswordModel : CrudBasePageModel
    {
        #region Variable Declaration
        private readonly IUserManager _userManager;
        private readonly IRoleManager _roleManager;
        private readonly ICompanyManager _companyManager;

        private readonly IWebHostEnvironment _webHostEnvironment;

        #endregion


        public ChangePasswordModel(IUserManager userManager, IWebHostEnvironment webHostEnvironment, IOptions<ApplicationSettings> config, CustomDataProtection protector) : base(config, protector)
        {
            _userManager = userManager;
            _webHostEnvironment = webHostEnvironment;
        }


        [BindProperty(SupportsGet = true)]
        public UserPasswordViewModel UserPasswordViewModel { get; set; }



        public IActionResult OnGet(string id)
        {
            if (id != null)
            {
               
                UserViewModel UserViewModel = _userManager.GetUserById(Convert.ToInt32(id));

                UserPasswordViewModel = new UserPasswordViewModel();
                UserPasswordViewModel.UserId = UserViewModel.UserId;
                UserPasswordViewModel.Email = UserViewModel.Email;
                UserPasswordViewModel.OldGetPassword = SecurityHelper.DecryptString(UserViewModel.Password, _baseConfig.Value.SecurityKey);

                if (TempData["ChangePasswordUpdated"] != null && Convert.ToBoolean(TempData["ChangePasswordUpdated"]) == true)
                {
                    UserPasswordViewModel.ResponseMessage = "Your password has been updated successfully.";
                }
            }

            return Page();
        }



        public IActionResult OnPostAsync()
        {
            int result = 0;
            if (ModelState.IsValid)
            {
                string pwd = SecurityHelper.EncryptString(UserPasswordViewModel.Password, _baseConfig.Value.SecurityKey);
                result = _userManager.UpdateUserChangePassword(UserPasswordViewModel.UserId, _loginSession.RoleViewModel.CompanyId, UserPasswordViewModel.OldPassword, pwd);

            }
            if (result > 0)
                TempData["ChangePasswordUpdated"] = true;
            else
                TempData["ChangePasswordUpdated"] = false;
            return RedirectToPage("ChangePassword");

        }

        public JsonResult OnGetValidateoldPassword()
        {
            bool result = false;
            if (!string.IsNullOrEmpty(UserPasswordViewModel.Email) && !string.IsNullOrEmpty(UserPasswordViewModel.OldPassword))
            {

                UserViewModel userVM = _userManager.GetUserByEmailandPasswordLogin(UserPasswordViewModel.Email, UserPasswordViewModel.OldPassword);
                if (userVM != null)
                    result = true;
                else
                    result = false;

            }
            else
                result = false;
            return new JsonResult(result);
        }
    }
}
