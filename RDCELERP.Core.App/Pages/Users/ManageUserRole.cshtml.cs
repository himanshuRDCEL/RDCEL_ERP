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
using RDCELERP.Model.Role;
using Microsoft.Extensions.Options;
using RDCELERP.Model.Base;

namespace RDCELERP.Core.App.Pages.Company
{
    public class ManageUserRoleModel : CrudBasePageModel
    {
        #region Variable Declaration
        private readonly IRoleManager _RoleManager;
        private readonly IUserManager _UserManager;
        private readonly ICompanyManager _companyManager;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private CustomDataProtection _protector;
        public readonly IOptions<ApplicationSettings> _config;
        private readonly RDCELERP.DAL.Entities.Digi2l_DevContext _context;
        #endregion


        public ManageUserRoleModel(IRoleManager RoleManager, IUserManager UserManager, ICompanyManager CompanyManager, RDCELERP.DAL.Entities.Digi2l_DevContext context, IWebHostEnvironment webHostEnvironment, IOptions<ApplicationSettings> config, CustomDataProtection protector) : base(config, protector)
        {
            _RoleManager = RoleManager;
            _UserManager = UserManager;
            _companyManager = CompanyManager;
            _webHostEnvironment = webHostEnvironment;
            _config = config;
            _protector = protector;
            _context = context; 

        }

        [BindProperty(SupportsGet = true)]
        public UserRoleViewModel UserRoleViewModel { get; set; }


        public IActionResult OnGet(string id, string  userId)
        {
            if (_loginSession == null)
            {
                return RedirectToPage("/index");
            }
            else
            {
                //get company list

                
                    var companyList = _companyManager.GetCompanyToAssignRole(_loginSession.RoleViewModel.CompanyId, _loginSession.RoleViewModel.RoleId, _loginSession.UserViewModel.UserId, (Convert.ToInt32(userId)));
                    // var companyList = _companyManager.GetAllCompany(_loginSession.RoleViewModel.CompanyId, _loginSession.RoleViewModel.RoleId, _loginSession.UserViewModel.UserId);              
                    if (companyList != null)
                    {
                        ViewData["CompanyList"] = new SelectList(companyList, "CompanyId", "CompanyName");
                    }
                    //get role list
                    //var roleList = _RoleManager.GetAllRole(_loginSession.RoleViewModel.CompanyId);
                    //if (roleList != null)
                    //{
                    //    ViewData["RoleList"] = new SelectList(roleList, "RoleId", "RoleName");
                    //}

                


                if (id != null)
                {
                    
                    //id = _protector.Decode(id);
                    UserRoleViewModel = _UserManager.GetUserRoleById(Convert.ToInt32(id));
                    
                    //var companyList1 = _companyManager.GetCompanyToAssignRole(_loginSession.RoleViewModel.CompanyId, _loginSession.RoleViewModel.RoleId, _loginSession.UserViewModel.UserId, userId);
                    //ViewData["CompanyList"] = new SelectList(companyList1, "CompanyId", "CompanyName");
                    var roleList = _RoleManager.GetAllRole(UserRoleViewModel.CompanyId);
                    ViewData["RoleList"] = new SelectList(roleList, "RoleId", "RoleName");

                    
                }

                if (UserRoleViewModel == null)
                {
                    UserRoleViewModel = new UserRoleViewModel();
                    UserRoleViewModel.UserId = (Convert.ToInt32(userId));
                }

                return Page();          
            }
        }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public IActionResult OnPostAsync()
        {
            int result = 0;
            if (ModelState.IsValid)
            {
                result = _UserManager.ManageUserRole(UserRoleViewModel, _loginSession.UserViewModel.UserId, _loginSession.RoleViewModel.CompanyId);
            }
            
            if (result > 0)
            {
                return RedirectToPage("Index", new { id = _protector.Encode(UserRoleViewModel.UserId) });
            }

            else
                return RedirectToPage("ManageUserRole");
        }

        public JsonResult OnGetRoleBYCompanyId()
        {
            //return new JsonResult(_LeadManager.GetAllCity());
            //return new JsonResult(_RoleManager.GetRoleByCompanyId(UserRoleViewModel.CompanyId, _loginSession.UserViewModel.UserId));
            return new JsonResult(_RoleManager.GetRoleByCompanyId(UserRoleViewModel.CompanyId));
        }
    }
}
