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
using Microsoft.Extensions.Options;
using RDCELERP.Model.Base;
using RDCELERP.Common.Helper;
using RDCELERP.BAL.MasterManager;
using RDCELERP.Model.ServicePartner;

namespace RDCELERP.Core.App.Pages.Users
{
    public class ManageModel : CrudBasePageModel
    {
        #region Variable Declaration
        private readonly IUserManager _UserManager;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private CustomDataProtection _protector;
        public readonly IOptions<ApplicationSettings> _config;
        private readonly RDCELERP.DAL.Entities.Digi2l_DevContext _context;
        
        #endregion


        public ManageModel(IUserManager UserManager,IWebHostEnvironment webHostEnvironment, Digi2l_DevContext context, IOptions<ApplicationSettings> config, CustomDataProtection protector) : base(config, protector)
       
        {
            _UserManager = UserManager;
            _webHostEnvironment = webHostEnvironment;
            _protector = protector;
            _config = config;
            _context = context;

        }

        [BindProperty(SupportsGet = true)]
        public UserViewModel UserViewModel { get; set; }
        [BindProperty(SupportsGet = true)]
        public bool Editmode { get; set; } = false;

        public IActionResult OnGet(string id)
       {
            string URL = _config.Value.URLPrefixforProd;

            if (_loginSession == null)
            {
                return RedirectToPage("/index");
            }
            else
            {
                if (id != null)
                {
                    id = _protector.Decode(id);
                    UserViewModel = _UserManager.GetUserById(Convert.ToInt32(id));
                   
                    if (UserViewModel != null)
                    {
                        Editmode = true;
                    }
                    if (UserViewModel.Phone != null)
                    {
                        UserViewModel.Phone = SecurityHelper.DecryptString(UserViewModel.Phone, _config.Value.SecurityKey);
                    }
                    if (UserViewModel.Email != null)
                    {
                        UserViewModel.Email = SecurityHelper.DecryptString(UserViewModel.Email, _config.Value.SecurityKey);
                    }
                    if (UserViewModel.Password != null)
                    {
                        UserViewModel.Password = SecurityHelper.DecryptString(UserViewModel.Password, _config.Value.SecurityKey);
                    }

                    
                }

                if (UserViewModel == null)
                    UserViewModel = new UserViewModel();

                //ViewData["CountryList"] = new SelectList(_countryManager.GetAllCountries(), "CountryId", "Name");
                if (!string.IsNullOrEmpty(UserViewModel.ImageName))
                {

                    UserViewModel.ImageURL = _baseConfig.Value.BaseURL + "/DBFiles/Users/" + UserViewModel.ImageName;
                   
                }

                return Page();
            }

        }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public IActionResult OnPostAsync(IFormFile UserProfile)
        {
            int result = 0;
           
            
                if (UserProfile != null)
                {

                    string fileName = Guid.NewGuid().ToString("N") + UserProfile.FileName;
                    var filePath = string.Concat(_webHostEnvironment.WebRootPath, "\\", @"\DBFiles\Users");
                    var fileNameWithPath = string.Concat(filePath, "\\", fileName);
                    using (var stream = new FileStream(fileNameWithPath, FileMode.Create))
                    {
                        UserProfile.CopyTo(stream);
                        UserViewModel.ImageName = fileName;
                    }
                }
                //if (UserViewModel.UserId == 0)
                //{
                //    //Code to Insert the object 

                //    UserViewModel.UnEncPassword = StringHelper.RandomStrByLength(6);
                //    UserViewModel.Password = SecurityHelper.EncryptString(UserViewModel.UnEncPassword, _baseConfig.Value.SecurityKey);
                //}
                //result = _UserManager.ManageUser(UserViewModel, _loginSession.UserViewModel.UserId, _loginSession.RoleViewModel.CompanyId);
                result = _UserManager.ManageUser(UserViewModel, _loginSession.UserViewModel.UserId, _loginSession.RoleViewModel.CompanyId);
                


            
            if (result > 0)
            {
                return RedirectToPage("Details", new { id = _protector.Encode(result) });
            }
            else
                return RedirectToPage("Manage");
        }

        //Email Validation[FromBody]
        public IActionResult OnPostCheckEmail(string email)
        {
            TblUser TblUser = new TblUser();
            string emailTrimmed = email?.Trim().ToLower();
            var email1 = SecurityHelper.EncryptString(emailTrimmed, _config.Value.SecurityKey);
            bool isValid = !_context.TblUsers.ToList().Exists(p =>p.Email == email1);
            return new JsonResult(isValid);
        }

        public IActionResult OnPostCheckPhone(string phone)
        {
            TblUser TblUser = new TblUser();


            string phoneTrimmed = phone?.Trim().ToLower();
            var phone1 = SecurityHelper.EncryptString(phoneTrimmed, _config.Value.SecurityKey);
            bool isValid = !_context.TblUsers.ToList().Exists(p => p.Phone == phone1);
            return new JsonResult(isValid);
        }

    }
}
