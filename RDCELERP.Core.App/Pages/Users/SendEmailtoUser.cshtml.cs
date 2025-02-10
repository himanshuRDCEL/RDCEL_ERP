using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using RDCELERP.BAL.Interface;
using RDCELERP.Common.Constant;
using RDCELERP.Common.Enums;
using RDCELERP.Common.Helper;
using RDCELERP.Core.App.Pages.Base;
using RDCELERP.DAL.Entities;
using RDCELERP.DAL.IRepository;
using RDCELERP.DAL.Repository;
using RDCELERP.Model.Base;
using RDCELERP.Model.Users;

namespace RDCELERP.Core.App.Pages.Users
{
    public class SendEmailtoUserModel : CrudBasePageModel
    {
        private readonly RDCELERP.DAL.Entities.Digi2l_DevContext _context;
        private CustomDataProtection _protector;
        IOptions<ApplicationSettings> _config;
        IUserManager _userManager;
        ICompanyRepository _companyRepository;

        public SendEmailtoUserModel(RDCELERP.DAL.Entities.Digi2l_DevContext context, IOptions<ApplicationSettings> config, IUserManager userManager, CustomDataProtection protector) : base(config, protector)

        {
            _context = context;
            _protector = protector;
            _config = config;
            _userManager = userManager;
        }

        //        [BindProperty(SupportsGet = true)]
        //        public IList<TblUserRole> TblUserRole { get; set; }
        //        [BindProperty(SupportsGet = true)]
        //        public IList<TblUserRole> TblUserRoleList { get; set; }
        //        [BindProperty(SupportsGet = true)]
        //        public TblUserRole TblUserRoleObj { get; set; }
        //        [BindProperty(SupportsGet = true)]
        //        public TblCompany TblCompany { get; set; }
        //        [BindProperty(SupportsGet = true)]
        //        public TblRole TblRole { get; set; }
        //        [BindProperty(SupportsGet = true)]
        //        public UserViewModel userVM { get; set; }

        //       /* public UserViewModel UserVM { get; set; }*/
        //        public IActionResult OnGet()
        //        {
        //            TblUserRoleObj = new TblUserRole();
        //            /*userVM.UserViewModelList = new List<UserViewModel>();
        //*/
        //            IList<UserViewModel> userVMList = new List<UserViewModel>();
        //            LoginViewModel loginSession = _loginSession;
        //            if (_loginSession == null)
        //            {
        //                return RedirectToPage("/index1");
        //            }
        //            else
        //            {

        //                if (_loginSession.RoleViewModel.RoleId == Convert.ToInt32(RoleEnum.SuperAdmin))
        //                {

        //                    // for super admin

        //                    TblUserRole = _context.TblUserRoles
        //                    .Include(t => t.Role)
        //                    .Include(t => t.User)
        //                    .Include(t => t.Company)
        //                    .Include(t => t.ModifiedByNavigation).Where(x => x.IsActive == true && x.RoleId != 1).OrderByDescending(x => x.UserId).ToList();

        //                }
        //                else
        //                {
        //                    // for other role
        //                    if (_loginSession.RoleViewModel.CompanyId > 0)
        //                        TblUserRole = _context.TblUserRoles
        //                       .Include(t => t.Role)
        //                       .Include(t => t.User)
        //                       .Include(t => t.Company)
        //                       .Include(t => t.ModifiedByNavigation).Where(x => x.IsActive == true && x.RoleId != 1 && x.CompanyId == _loginSession.RoleViewModel.CompanyId).OrderByDescending(x => x.UserId).ToList();

        //                }

        //                if (TblUserRole != null && TblUserRole.Count > 0)
        //                {
        //                    TblUserRoleList = new List<TblUserRole>();
        //                    foreach (var item in TblUserRole)
        //                    {

        //                        TblUserRoleObj = new TblUserRole();
        //                        if (item.CompanyId > 0 && item.RoleId > 0)
        //                        {
        //                            TblCompany = _context.TblCompanies.FirstOrDefault(x => x.CompanyId == item.CompanyId && x.IsActive == true);
        //                            TblRole = _context.TblRoles.FirstOrDefault(x => x.RoleId == item.RoleId && x.IsActive == true);

        //                            if (TblCompany != null && TblRole != null)
        //                            {

        //                                TblUserRoleObj = item;
        //                                TblUserRoleList.Add(TblUserRoleObj);
        //                            }
        //                        }

        //                    }

        //                    //User filtered list
        //                    TblUserRole = TblUserRoleList;
        //                    if (TblUserRole != null)
        //                    {
        //                        foreach (var items in TblUserRole)
        //                        {
        //                            items.User.Email = SecurityHelper.DecryptString(items.User.Email, _config.Value.SecurityKey);
        //                            items.User.Phone = SecurityHelper.DecryptString(items.User.Phone, _config.Value.SecurityKey);
        //                        }
        //                    }
        //                }

        //                return Page();
        //            }
        //        }
        [BindProperty(SupportsGet = true)]
        public List<UserViewModel> UserVMList { get; set; }
        [BindProperty(SupportsGet = true)]
        public UserViewModel UserVM { get; set; }

        [BindProperty(SupportsGet = true)]
        public TblUserRole TblUserRoleObj { get; set; }
        [BindProperty(SupportsGet = true)]
        public string text { get; set; }

        [BindProperty(SupportsGet = true)]
        public string title { get; set; }
        [BindProperty(SupportsGet = true)]
        public int? ListNA { get; set; }
        public IActionResult OnGet(string? ReturnList)
        {
            TblUserRoleObj = new TblUserRole();
            if (_loginSession == null)
            {
                return RedirectToPage("/index");
            }
            else
            {
                UserVMList = _userManager.GetAllUsersByRole( _loginSession.RoleViewModel.RoleId).ToList();
                if (ReturnList != null)
                {
                   
                    if (ReturnList == "Success")
                    {
                        title = " Email Successfully Sent";
                        text = "Email has successfully sent to on registered email address";
                        ListNA = 1;
                        ReturnList = null;
                    }
                    else
                    {
                        title = "Failed";
                        text = "Email not found for this user";
                        ListNA = 0;
                        ReturnList = null;
                    }
                }

                return Page();
            }
        }

        public IActionResult OnPostAsync()
        {
           
            int result;

            if (UserVM != null && UserVM.UserIdList != null)
            {
                var messege =string.Empty;
                result = _userManager.NotificationAllUsers(UserVM, "Your Details", EmailTemplateConstant.NewUserAdded_User);
                if (result > 0)
                {
                     messege = "Success";
                }
                else
                {
                    messege = "Failed";
                }
                if (messege != null && messege != " ")
                {
                    return RedirectToPage("SendEmailtoUser", new { ReturnList = messege });
                    //return RedirectToPage("Not_Allocated");
                }
                else
                {
                    return RedirectToPage("SendEmailtoUser", new { ReturnList = messege });
                }
            }

            return RedirectToPage("./SendEmailtoUser");

        }

        
        public IActionResult OnPostDeleteAsync()
        {
            if (_loginSession == null)
            {
                return RedirectToPage("/SendEmailtoUser");
            }
            else
            {
                if (TblUserRoleObj != null && TblUserRoleObj.UserRoleId > 0)
                {
                    TblUserRoleObj = _context.TblUserRoles.Find(TblUserRoleObj.UserRoleId);
                }

                if (TblUserRoleObj != null)
                {

                    TblUserRoleObj.IsActive = false;
                    TblUserRoleObj.ModifiedBy = _loginSession.UserViewModel.UserId;

                    _context.TblUserRoles.Update(TblUserRoleObj);
                    //  _context.TblUsers.Remove(TblUser);
                    _context.SaveChanges();
                }

                return RedirectToPage("./SendEmailtoUser");
            }
        }
    }
}

