using RDCELERP.BAL.Interface;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using RDCELERP.Core.App.Helper;
using RDCELERP.Model.Users;

namespace RDCELERP.Core.App.Components
{
    public class EVC_WalletViewComponent : ViewComponent
    {
        private readonly IUserManager _UserManager;
        public EVC_WalletViewComponent(IUserManager UserManager)
        {
            _UserManager = UserManager;
        }

        [BindProperty(SupportsGet = true)]
        public LoginViewModel _loginSession { get; set; }
        public IViewComponentResult Invoke()
        {
            _loginSession = SessionHelper.GetObjectFromJson<LoginViewModel>(HttpContext.Session, "LoginUser");
            var model = _UserManager.GetUserById(Convert.ToInt32(_loginSession.UserViewModel.UserId));
            
            return View(model);
        }

    }
}
