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
    public class EncryptedEmailPassModel : CrudBasePageModel
    {
        #region Variable Declaration
        private readonly IUserManager _UserManager;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private CustomDataProtection _protector;
        public readonly IOptions<ApplicationSettings> _config;
        #endregion


        public EncryptedEmailPassModel(IUserManager UserManager, IWebHostEnvironment webHostEnvironment, IOptions<ApplicationSettings> config, CustomDataProtection protector) : base(config, protector)
        {
            _UserManager = UserManager;
            _webHostEnvironment = webHostEnvironment;
            _protector = protector;
            _config = config;
        }

        [BindProperty(SupportsGet = true)]
        public UserViewModel UserViewModel { get; set; }

        public IActionResult OnGetAsync(string id)

        {

            if (_loginSession == null)
            {
                return RedirectToPage("/index");
            }
            else
            {
                if (id != null)
                {
                    id = _protector.Decode(id);
                    UserViewModel = _UserManager.GetUserById(Convert.ToInt32(id));
                    UserViewModel.Email = SecurityHelper.EncryptString(UserViewModel.Email, _config.Value.SecurityKey);
                    UserViewModel.Password = SecurityHelper.EncryptString(UserViewModel.Password, _config.Value.SecurityKey);
                    UserViewModel.Phone = SecurityHelper.EncryptString(UserViewModel.Phone, _config.Value.SecurityKey);

                }


                return Page();
            }
        }
    }
}



