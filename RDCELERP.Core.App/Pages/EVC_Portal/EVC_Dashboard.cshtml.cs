using RDCELERP.BAL.Interface;
using RDCELERP.Core.App.Helper;
using RDCELERP.Core.App.Pages.Base;
using RDCELERP.DAL.Entities;
using RDCELERP.Model.Base;
using RDCELERP.Model.Users;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RDCELERP.Model.EVC_Portal;

namespace RDCELERP.Core.App.Pages.EVC_Portal
{
    public class EVC_DashboardModel : BasePageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly IUserManager _userManager;
        private readonly IEVCManager _eVCManager;
       
        private readonly RDCELERP.DAL.Entities.Digi2l_DevContext _context;
        public EVC_DashboardModel(ILogger<IndexModel> logger, IUserManager userManager, RDCELERP.DAL.Entities.Digi2l_DevContext context, IOptions<ApplicationSettings> config, IEVCManager eVCManager)
        : base(config)

        {
            _logger = logger;
            _userManager = userManager;
            _context = context;
            _eVCManager = eVCManager;
        }


        
        [BindProperty(SupportsGet = true)]
        public EVC_DashboardViewModel eVC_DashBoardViewModel { get; set; }
        
        [BindProperty(SupportsGet = true)]
        public List<lattestAllocationViewModel> LattestAllocationViewModel { get; set; }
       

        public IActionResult OnGet()
        {

            if (_loginSession == null)
            {
                return RedirectToPage("Index");
            }
            else
            {
                int userId = _loginSession.UserViewModel.UserId;
                //getdashboard data
                if (userId !=null)
                {
                    eVC_DashBoardViewModel = _eVCManager.EvcByUserId(userId);
                    LattestAllocationViewModel = _eVCManager.GetListOFLattestAllocation(userId,true);
                }              
                return Page();
            }

        }

        //public IActionResult OnPost()
        //{
        //    if (!string.IsNullOrEmpty(UserViewModel.Email) && !string.IsNullOrEmpty(UserViewModel.Password))
        //    {
        //        LoginViewModel loginVM = _userManager.GetUserByLogin(UserViewModel.Email, UserViewModel.Password);
        //        if (loginVM != null && loginVM.UserViewModel != null && loginVM.UserViewModel.UserId != 0)
        //        {
        //            SessionHelper.SetObjectAsJson(HttpContext.Session, "LoginUser", loginVM);
        //            return new RedirectToPageResult("/Dashboard");
        //        }
        //        else
        //        {
        //            TempData["Auth"] = false;
        //            return new RedirectToPageResult("Index");
        //        }
        //    }
        //    else
        //    {
        //        TempData["Auth"] = false;
        //        return new RedirectToPageResult("Index");
        //    }
        //}
    }
}
