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
    public class ManageRoleModel : CrudBasePageModel
    {
        #region Variable Declaration
        private readonly IRoleManager _RoleManager;
        private readonly ICompanyManager _companyManager;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly RDCELERP.DAL.Entities.Digi2l_DevContext _context;
        #endregion


        public ManageRoleModel(IRoleManager RoleManager, ICompanyManager CompanyManager, IWebHostEnvironment webHostEnvironment, RDCELERP.DAL.Entities.Digi2l_DevContext context, IOptions<ApplicationSettings> config, CustomDataProtection protector) : base(config, protector)

        {
            _RoleManager = RoleManager;
            _companyManager = CompanyManager;
            _webHostEnvironment = webHostEnvironment;
            _context = context;
            

        }

        [BindProperty(SupportsGet = true)]
        public RoleViewModel RoleViewModel { get; set; }
        [BindProperty(SupportsGet = true)]
        public IList<RoleViewModel> RoleVMList { get; set; }

        [BindProperty(SupportsGet = true)]
        public RoleAccessViewModel RoleAccessViewModel { get; set; }
        [BindProperty(SupportsGet = true)]
        public TblCompany TblCompany { get; set; }

        public IActionResult OnGet(int? id, int? tabid)
        {
            if (_loginSession == null)
            {
                return RedirectToPage("/index");
            }
            else
            {
                //get company list
                var companyList = _companyManager.GetAllCompany(_loginSession.RoleViewModel.CompanyId, _loginSession.RoleViewModel.RoleId, _loginSession.UserViewModel.UserId);

                if (companyList != null)
                {
                    ViewData["CompanyList"] = new SelectList(companyList, "CompanyId", "CompanyName");
                }

                RoleViewModel = _RoleManager.GetRoleById(Convert.ToInt32(id), tabid);

                if (RoleViewModel != null && RoleViewModel.CompanyId > 0)
                {
                    TblCompany = _context.TblCompanies.FirstOrDefault(x => x.CompanyId == RoleViewModel.CompanyId && x.IsActive == true);
                    if (TblCompany != null && TblCompany.CompanyId != 0)
                    {
                        RoleViewModel.CompanyName = TblCompany.CompanyName;
                    }
                }

                if (RoleViewModel == null)
                    RoleViewModel = new RoleViewModel();

                return Page();
            }
        }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public IActionResult OnPostAsync(int id)
        {
            if (_loginSession == null)
            {
                return RedirectToPage("/index");
            }
            else
            {
                int result = 0;

                //get Role list
                RoleVMList = _RoleManager.GetAllRole(RoleViewModel.CompanyId);
                List<string> RoleNames = new List<string>();
                if (RoleVMList != null && RoleVMList.Count > 0)
                {
                    foreach (RoleViewModel RoleViewModel in RoleVMList)
                    {

                        RoleNames.Add((RoleViewModel.RoleName.ToLower()));
                    }
                }

                if (ModelState.IsValid)
                {
                    var Role = RoleNames.Contains(RoleViewModel.RoleName.ToLower());
                    if (Role != true && RoleViewModel.RoleId == 0)

                        result = _RoleManager.ManageRole(RoleViewModel, _loginSession.UserViewModel.UserId, _loginSession.RoleViewModel.CompanyId);
                    else if ((Role == true || Role == false) && RoleViewModel.RoleId != 0)

                        result = _RoleManager.ManageRole(RoleViewModel, _loginSession.UserViewModel.UserId, _loginSession.RoleViewModel.CompanyId);

                    else
                       if (RoleViewModel.RoleId != 0)
                        return RedirectToPage("ManageRole", new { id = RoleViewModel.RoleId });
                    else
                        return RedirectToPage("ManageRole");
                }
                if (result > 0)
                    //return RedirectToPage("RoleList", new { id = RoleViewModel.CompanyId });
                    return RedirectToPage("RoleList");
                else
                    return RedirectToPage("ManageRole");
            }
        }
    }
}
