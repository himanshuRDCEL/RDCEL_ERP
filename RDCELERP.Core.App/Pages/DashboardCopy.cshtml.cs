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

namespace RDCELERP.Core.App.Pages
{
    public class DashboardCopy : BasePageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly IUserManager _userManager;
        private readonly RDCELERP.DAL.Entities.Digi2l_DevContext _context;
        public DashboardCopy(ILogger<IndexModel> logger, IUserManager userManager, RDCELERP.DAL.Entities.Digi2l_DevContext context, IOptions<ApplicationSettings> config)
        : base(config)

        {
            _logger = logger;
            _userManager = userManager;
            _context = context;
        }


        [BindProperty(SupportsGet = true)]
        public UserViewModel UserViewModel { get; set; }
       
        [BindProperty(SupportsGet = true)]
        public IList<TblUserRole> TblUserRole { get; set; }
        [BindProperty(SupportsGet = true)]
        public List<UserViewModel> UserVMList { get; set; }
        [BindProperty(SupportsGet = true)]
        public UserViewModel UserVM { get; set; }
        public int RoleId { get; set; }

        public IActionResult OnGet()
        {

            if (_loginSession == null)
            {
                return RedirectToPage("Index");
            }
            else
            {

                //get user list

                if (_loginSession.RoleViewModel.CompanyId > 0)
                {
                    TblUserRole = _context.TblUserRoles
                  .Include(t => t.Role)
                  .Include(t => t.User)
                  .Include(t => t.Company)
                  .Include(t => t.ModifiedByNavigation).Where(x => x.IsActive == true && x.CompanyId == _loginSession.RoleViewModel.CompanyId).OrderByDescending(x => x.UserId).ToList();
                }

                if (TblUserRole != null && TblUserRole.Count > 0)
                {
                    UserVMList = new List<UserViewModel>();

                    foreach (var item in TblUserRole)
                    {
                        UserVM = new UserViewModel();
                        UserVM.UserId = (int)item.UserId;
                        UserVM.FirstName = item.User.FirstName;
                        UserVM.LastName = item.User.LastName;
                        UserVM.RoleName = item.Role.RoleName;
                        
                        // UserVM. = item.CompanyId

                        UserVMList.Add(UserVM);
                    }
                }
                if (UserVMList != null && UserVMList.Count > 0)
                {
                   ViewData["AssignToList"] = new SelectList(UserVMList, "UserId", "FullNameAndRole");
                                       
                }

                //get lead list
              
                return Page();
            }

        }

        public IActionResult OnPost()
        {
            if (!string.IsNullOrEmpty(UserViewModel.Email) && !string.IsNullOrEmpty(UserViewModel.Password))
            {
                LoginViewModel loginVM = _userManager.GetUserByLogin(UserViewModel.Email, UserViewModel.Password);
                if (loginVM != null && loginVM.UserViewModel != null && loginVM.UserViewModel.UserId != 0)
                {
                    SessionHelper.SetObjectAsJson(HttpContext.Session, "LoginUser", loginVM);
                    return new RedirectToPageResult("/Dashboard");
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
    }
}
