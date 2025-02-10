using RDCELERP.BAL.Interface;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using RDCELERP.Model.Users;
using RDCELERP.Core.App.Helper;

namespace RDCELERP.Core.App.Components
{
    public class SidebarViewComponent : ViewComponent
    {
        private readonly ICommonManager _commonManager;
        public LoginViewModel _loginSession;

        public SidebarViewComponent(ICommonManager commonManager)
        {
            _commonManager = commonManager;            
        }

        public IViewComponentResult Invoke()
        {

            _loginSession = SessionHelper.GetObjectFromJson<LoginViewModel>(HttpContext.Session, "LoginUser");

            var SidebarMenu = _commonManager.GetAllAccessList(_loginSession.RoleViewModel.RoleId);
            ViewData["Catid"] = Request.Query["Catid"];

            ViewData["PortalName"] = _loginSession.RoleViewModel.PortalName;
            ViewData["PortalLink"] = _loginSession.RoleViewModel.PortalLink;
            return View(SidebarMenu);
        }
      
    }
}
