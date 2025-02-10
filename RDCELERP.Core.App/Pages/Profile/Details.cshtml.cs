using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RDCELERP.DAL.Entities;
using RDCELERP.Core.App.Pages.Base;
using RDCELERP.Model.Users;
using RDCELERP.BAL.Interface;
using Microsoft.Extensions.Options;
using RDCELERP.Model.Base;
using RDCELERP.Common.Helper;

namespace RDCELERP.Core.App.Pages.Profile
{
    public class DetailsModel : BasePageModel
    {
        private readonly RDCELERP.DAL.Entities.Digi2l_DevContext _context;
        private readonly IUserManager _userManager;
        private IOptions<ApplicationSettings> _config;
        public DetailsModel(IUserManager userManager, RDCELERP.DAL.Entities.Digi2l_DevContext context, IOptions<ApplicationSettings> config)
        : base(config)
        {
            _context = context;
            _userManager = userManager;
            _config = config;
        }

        [BindProperty(SupportsGet = true)]
        public UserViewModel UserViewModel { get; set; }
        public IActionResult OnGet()
        {

            if (_loginSession != null)
            {
                UserViewModel = _userManager.GetUserById(Convert.ToInt32(_loginSession.UserViewModel.UserId));
                UserViewModel.Email = SecurityHelper.DecryptString(UserViewModel.Email, _config.Value.SecurityKey);
                UserViewModel.Phone = SecurityHelper.DecryptString(UserViewModel.Phone, _config.Value.SecurityKey);

            }
            else
            {
                return RedirectToPage("/index");
            }

            return Page();
        }
    }
}
