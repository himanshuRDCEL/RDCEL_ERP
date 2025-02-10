using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using RDCELERP.DAL.Entities;
using RDCELERP.Core.App.Pages.Base;
using RDCELERP.BAL.Interface;
using RDCELERP.Model.Users;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using RDCELERP.Core.App.Helper;
using Microsoft.Extensions.Options;
using RDCELERP.Model.Base;
using RDCELERP.Common.Helper;

namespace RDCELERP.Core.App.Pages.Profile
{
    public class ManageModel : BasePageModel
    {
        #region Variable Declaration
        private readonly IUserManager _UserManager;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public readonly IOptions<ApplicationSettings> _config;
        #endregion


        public ManageModel(IUserManager UserManager, IWebHostEnvironment webHostEnvironment, IOptions<ApplicationSettings> config)
        : base(config)
        {
            _UserManager = UserManager;
            _webHostEnvironment = webHostEnvironment;
            _config = config;

        }

        [BindProperty(SupportsGet = true)]
        public UserViewModel UserViewModel { get; set; }
        
        public IActionResult OnGet()
        {

            if (_loginSession != null)
            {
                UserViewModel = _UserManager.GetUserById(Convert.ToInt32(_loginSession.UserViewModel.UserId));
                UserViewModel.Email = SecurityHelper.DecryptString(UserViewModel.Email, _config.Value.SecurityKey);
                UserViewModel.Phone = SecurityHelper.DecryptString(UserViewModel.Phone, _config.Value.SecurityKey);
            }
            else
            {
                return RedirectToPage("/index");
            }

            return Page();
        }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public IActionResult OnPostAsync(IFormFile UserProfile)
        {
            int result = 0;
            if (UserProfile != null)
            {
                string fileName = Guid.NewGuid().ToString("N") + UserProfile.FileName;
                var filePath = string.Concat(_webHostEnvironment.WebRootPath, "\\", @"\DBFiles\Users");
                var fileNameWithPath = string.Concat(filePath, "\\", fileName);
                using (var stream = new FileStream(fileNameWithPath, FileMode.Create))
                {
                    UserProfile.CopyTo(stream);
                    UserViewModel.ImageName = fileName;
                }
            }
            result = _UserManager.ManageUser(UserViewModel, _loginSession.UserViewModel.UserId, _loginSession.RoleViewModel.CompanyId);




            if (result > 0)
            {
                return RedirectToPage("/Profile/Details");
            }
            else
            {
                return RedirectToPage("/Profile/Manage");
            }
        }

    }
}
