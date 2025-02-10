using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RDCELERP.DAL.Entities;
using RDCELERP.Core.App.Pages.Base;
using Microsoft.Extensions.Options;
using RDCELERP.Model.Base;
using RDCELERP.Common.Helper;
using RDCELERP.BAL.Interface;
using RDCELERP.Model.EVC_Portal;
using RDCELERP.Model.EVC;
using RDCELERP.Model.Users;

namespace RDCELERP.Core.App.Pages.EVC_Portal
{
    public class EVC_UserProfileModel : BasePageModel
    {
        private readonly RDCELERP.DAL.Entities.Digi2l_DevContext _context;
        public readonly IOptions<ApplicationSettings> _config;
        private CustomDataProtection _protector;
        private readonly IEVCManager _eVCManager;


        public EVC_UserProfileModel(RDCELERP.DAL.Entities.Digi2l_DevContext context, IOptions<ApplicationSettings> config, CustomDataProtection protector, IEVCManager eVCManager)
        : base(config)
        {
            _context = context;
            _config = config;
            _protector = protector;
            _eVCManager = eVCManager;
        }
    
        [BindProperty(SupportsGet = true)]
        public EVC_RegistrationModel eVC_RegistrationModel { get; set; }
        [BindProperty(SupportsGet = true)]
        public UserViewModel UserViewModel { get; set; }


        public IList<TblUserRole> TblUserRole { get; set; }

        public IActionResult OnGetAsync(string id)
        {

            if (_loginSession == null)
            {
                return RedirectToPage("/index");
            }
            else
            {
                UserViewModel = _loginSession.UserViewModel;

                if (UserViewModel != null)
                {
                    UserViewModel.Email = SecurityHelper.DecryptString(UserViewModel.Email, _config.Value.SecurityKey);
                    UserViewModel.Phone = SecurityHelper.DecryptString(UserViewModel.Phone, _config.Value.SecurityKey);
                    UserViewModel.Password = SecurityHelper.DecryptString(UserViewModel.Password, _config.Value.SecurityKey);

                    eVC_RegistrationModel = _eVCManager.GetEvcByUserId(UserViewModel.UserId);
                }

                return Page();
            }
        }
    }
}
