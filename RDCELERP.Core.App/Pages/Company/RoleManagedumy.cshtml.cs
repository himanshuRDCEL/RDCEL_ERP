using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DocumentFormat.OpenXml.Office2010.Excel;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Options;
using RDCELERP.BAL.Interface;
using RDCELERP.Core.App.Pages.Base;
using RDCELERP.DAL.Entities;
using RDCELERP.Model.Base;
using RDCELERP.Model.Role;

namespace RDCELERP.Core.App.Pages.Company
{
   
        public class RoleManageModel : CrudBasePageModel
        {
            #region Variable Declaration
            private readonly IRoleManager _RoleManager;
            private readonly ICompanyManager _companyManager;
            private readonly IWebHostEnvironment _webHostEnvironment;
            private readonly RDCELERP.DAL.Entities.Digi2l_DevContext _context;
            #endregion


            public RoleManageModel(IRoleManager RoleManager, ICompanyManager CompanyManager, IWebHostEnvironment webHostEnvironment, RDCELERP.DAL.Entities.Digi2l_DevContext context, IOptions<ApplicationSettings> config, CustomDataProtection protector) : base(config, protector)

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
            public IList<TblAccessList> TblAccessList { get; set; }
            [BindProperty(SupportsGet = true)]
            public RoleAccessViewModel RoleAccessViewModel { get; set; }
            [BindProperty(SupportsGet = true)]
            public TblCompany TblCompany { get; set; }
            [BindProperty(SupportsGet = true)]
            public int ModelRoleId { get; set; }

        public IActionResult OnGet(int? id, int? tabId)
            {
           
            if (_loginSession == null)
                {
                    return RedirectToPage("/index");
                }
                else
                {
                    //get company list
                    var companyList = _companyManager.GetAllCompany(_loginSession.RoleViewModel.CompanyId, _loginSession.RoleViewModel.RoleId, _loginSession.UserViewModel.UserId);
                    TblAccessList = _context.TblAccessLists.Where(x=>x.ParentAccessListId == 10 && x.IsActive == true).ToList();
                    if (companyList != null)
                    {
                        ViewData["CompanyList"] = new SelectList(companyList, "CompanyId", "CompanyName");
                    }

                    RoleViewModel = _RoleManager.GetRoleById(Convert.ToInt32(id), tabId);
                    ModelRoleId = RoleViewModel.RoleId;


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

        public IActionResult OnGetCheckAccess(int? Id,int? tabId)
        {
            if (tabId == 0)
            {
                return BadRequest();
            }
            RoleViewModel = _RoleManager.GetRoleById(Convert.ToInt32(Id), tabId);

            return new JsonResult(RoleViewModel.RoleAccessViewModelList);
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
                            return RedirectToPage("RoleManagedumy", new { id = RoleViewModel.RoleId });
                        else
                            return RedirectToPage("RoleManagedumy");
                    }
                    if (result > 0)
                        //return RedirectToPage("RoleList", new { id = RoleViewModel.CompanyId });
                        return RedirectToPage("RoleList");
                    else
                        return RedirectToPage("RoleManagedumy");
                }
            }
        public IActionResult OnPostCheckName(string Name, int? Id)
        {
            if (Id > 0)
            {
                TblRole TblRole = new TblRole();
                string nameTrimmed = Name?.Trim(); // Trim the Name parameter

                bool isValid = !_context.TblRoles.ToList().Exists(p => p.RoleName.Trim() == nameTrimmed && p.RoleId != Id);

                return new JsonResult(isValid);
            }
            else
            {
                TblRole TblRole = new TblRole();
                string nameTrimmed = Name?.Trim(); // Trim the Name parameter
                bool isValid = !_context.TblRoles.ToList().Exists(p => p.RoleName.Trim().Equals(nameTrimmed, StringComparison.CurrentCultureIgnoreCase));

                return new JsonResult(isValid);
            }


        }


        public IActionResult OnPostSaveAccess(int id, [FromBody] List<RoleAccessViewModel>? RoleAccessViewModelList)
        {
            if (RoleAccessViewModelList == null)
            {
                return BadRequest();
            }

            RoleViewModel.RoleAccessViewModelList = new List<RoleAccessViewModel>();

            foreach (var checkboxValue in RoleAccessViewModelList)
            {
                if (checkboxValue != null)
                {
                    RoleAccessViewModel accessViewModel = new RoleAccessViewModel
                    {
                        AccessListId = checkboxValue.AccessListId,
                        CanView = checkboxValue.CanView,
                        CanAdd = checkboxValue.CanAdd,
                        CanDelete = checkboxValue.CanDelete,
                        RoleName = checkboxValue.RoleName,
                        RoleId = id,
                        CompanyName = checkboxValue.CompanyName,
                        CompanyId = checkboxValue.CompanyId,
                        Selected = checkboxValue.Selected,
                        ParentAccessListId = checkboxValue.ParentAccessListId,
                    };
                    RoleViewModel.RoleAccessViewModelList.Add(accessViewModel);
                }
            }
            int result = 0;
            result = _RoleManager.ManageRole(RoleViewModel, _loginSession.UserViewModel.UserId, _loginSession.RoleViewModel.CompanyId);
            if (result > 0)

                return new JsonResult(result);
            else
                return RedirectToPage("RoleManagedumy");

        }

    }
}
