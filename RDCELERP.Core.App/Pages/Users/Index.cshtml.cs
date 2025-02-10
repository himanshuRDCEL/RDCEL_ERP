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
using RDCELERP.Common.Enums;
using RDCELERP.Model.Users;
using RDCELERP.Common.Helper;

namespace RDCELERP.Core.App.Pages.Users
{
    public class IndexModel : CrudBasePageModel
    {
        private readonly RDCELERP.DAL.Entities.Digi2l_DevContext _context;
        private CustomDataProtection _protector;
        IOptions<ApplicationSettings> _config;

        public IndexModel(RDCELERP.DAL.Entities.Digi2l_DevContext context, IOptions<ApplicationSettings> config, CustomDataProtection protector) : base(config, protector)
       
        {
            _context = context;
            _protector = protector;
            _config = config;
        }
               
        [BindProperty(SupportsGet = true)]
        public IList<TblUserRole> TblUserRole { get; set; }

        [BindProperty(SupportsGet = true)]
        public IList<TblUserRole> TblUserRoleList { get; set; }
        [BindProperty(SupportsGet = true)]
        public TblUserRole TblUserRoleObj { get; set; }
        [BindProperty(SupportsGet = true)]
        public TblCompany TblCompany { get; set; }
        [BindProperty(SupportsGet = true)]
        public TblRole TblRole { get; set; }

        public IActionResult OnGet()
        {


            return Page();
        

    }

        public IActionResult OnPostDeleteAsync()
        {
            if (_loginSession == null)
            {
                return RedirectToPage("/index");
            }
            else
            {
                if (TblUserRoleObj != null && TblUserRoleObj.UserRoleId > 0)
                {
                    TblUserRoleObj = _context.TblUserRoles.Find(TblUserRoleObj.UserRoleId);
                }

                if (TblUserRoleObj != null)
                {

                    if (TblUserRoleObj.IsActive == true)
                    {
                        TblUserRoleObj.IsActive = false;
                    }
                    else
                    {
                        TblUserRoleObj.IsActive = true;
                    }
                    
                    TblUserRoleObj.ModifiedBy = _loginSession.UserViewModel.UserId;
                  
                    _context.TblUserRoles.Update(TblUserRoleObj);
                    //  _context.TblUsers.Remove(TblUser);
                    _context.SaveChanges();
                }

                return RedirectToPage("./Index");
            }
        }

    }
}
