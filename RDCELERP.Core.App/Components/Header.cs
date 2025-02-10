using RDCELERP.BAL.Interface;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using RDCELERP.Core.App.Helper;
using RDCELERP.Model.Users;
using RDCELERP.Common.Enums;

namespace RDCELERP.Core.App.Components
{
    public class HeaderViewComponent : ViewComponent
    {
        private readonly IUserManager _UserManager;
        public HeaderViewComponent(IUserManager UserManager)
        {
            _UserManager = UserManager;
        }

        [BindProperty(SupportsGet = true)]
        public LoginViewModel _loginSession { get; set; }
        public IViewComponentResult Invoke()
        {
            _loginSession = SessionHelper.GetObjectFromJson<LoginViewModel>(HttpContext.Session, "LoginUser");
            var model = _UserManager.GetUserById(Convert.ToInt32(_loginSession.UserViewModel.UserId));
            if (ViewData["SuccessMessage"] != null && ViewData["SuccessMessage"] != "")
            {
                string SuccessMessage = ViewData["SuccessMessage"].ToString();
                ShowMessage(SuccessMessage, MessageTypeEnum.error);
            }
            if (ViewData["ErrorMessage"] != null && ViewData["ErrorMessage"] != "")
            {
                string ErrorMessage = ViewData["ErrorMessage"].ToString();
                ShowMessage(ErrorMessage, MessageTypeEnum.error);
            }

            return View(model);
        }
        public void ShowMessage(string message, MessageTypeEnum messageType)
        {
            ViewData["MessageType"] = messageType;
            ModelState.AddModelError(string.Empty, message);
        }
    }
}
