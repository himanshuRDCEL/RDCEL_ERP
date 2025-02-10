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
using RDCELERP.BAL.Interface;
using RDCELERP.Model.Company;

namespace RDCELERP.Core.App.Pages.Company
{
    public class IndexModel : BasePageModel
    {
        private readonly RDCELERP.DAL.Entities.Digi2l_DevContext _context;
        private readonly ICompanyManager _companyManager;

        public IndexModel(RDCELERP.DAL.Entities.Digi2l_DevContext context, ICompanyManager companyManager, IOptions<ApplicationSettings> config)
        : base(config)
        {
            _context = context;
            _companyManager = companyManager;
        }

       
        [BindProperty(SupportsGet = true)]
        public IList<CompanyViewModel> CompanyVM { get; set; }
        [BindProperty(SupportsGet = true)]
        public TblCompany TblCompanyObj { get; set; }
        // public IList<TblCompany> TblCompany { get;set; }
        //public TblUserRole TblUserRole { get; set; }
        //public TblRole TblRole { get; set; }

        public IActionResult OnGet()
        {
            TblCompanyObj = new TblCompany();
            if (_loginSession == null)
            {
                return RedirectToPage("/index");
            }
            else
            {
                CompanyVM = _companyManager.GetAllCompany(_loginSession.RoleViewModel.CompanyId, _loginSession.RoleViewModel.RoleId, _loginSession.UserViewModel.UserId);
              
                return Page();
            }
        }

        public IActionResult OnPostDeleteAsync()
        {
            if (_loginSession == null)
            {
                return RedirectToPage("/index");
            }
            else
            {
                if (TblCompanyObj != null && TblCompanyObj.CompanyId > 0)
                {
                    TblCompanyObj = _context.TblCompanies.Find(TblCompanyObj.CompanyId);
                }

                if (TblCompanyObj != null)
                {
                    TblCompanyObj.IsActive = false;
                    TblCompanyObj.ModifiedBy = _loginSession.UserViewModel.UserId;
                    _context.TblCompanies.Update(TblCompanyObj);
                    //  _context.TblCompanys.Remove(TblCompany);
                    _context.SaveChanges();
                }

                return RedirectToPage("./Index");
            }
        }

    }
}
