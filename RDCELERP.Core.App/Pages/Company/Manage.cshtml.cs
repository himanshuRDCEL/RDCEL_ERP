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
using RDCELERP.Model.Company;
using Microsoft.Extensions.Options;
using RDCELERP.Model.Base;

namespace RDCELERP.Core.App.Pages.Company
{
    public class ManageModel : BasePageModel
    {
        #region Variable Declaration
        private readonly ICompanyManager _CompanyManager;
        private readonly Digi2l_DevContext _context;
        #endregion

        public ManageModel(ICompanyManager CompanyManager, Digi2l_DevContext context, IOptions<ApplicationSettings> config)
        : base(config)
        {
            _CompanyManager = CompanyManager;
            _context = context;
        }

        [BindProperty(SupportsGet = true)]
        public CompanyViewModel CompanyViewModel { get; set; }


        public IActionResult OnGet(int? id)
        {
            if (id != null)
            {
                CompanyViewModel = _CompanyManager.GetCompanyById(Convert.ToInt32(id));
                var BusinessUnit = _context.TblBusinessUnits.Where(x => x.BusinessUnitId == CompanyViewModel.BusinessUnitId && x.IsActive == true).FirstOrDefault();
                if( BusinessUnit != null )
                {
                    CompanyViewModel.BusinessUnitName = BusinessUnit.Name;
                }
            }
               
            

            if (CompanyViewModel == null)
                CompanyViewModel = new CompanyViewModel();

            //ViewData["CountryList"] = new SelectList(_countryManager.GetAllCountries(), "CountryId", "Name");
            if (!string.IsNullOrEmpty(CompanyViewModel.CompanyLogoUrl))
            {

                CompanyViewModel.CompanyLogoUrlLink =  _baseConfig.Value.BaseURL + "/DBFiles/Company/" + CompanyViewModel.CompanyLogoUrl;
            }

            if (_loginSession == null)
            {
                return RedirectToPage("/index");
            }
            else
            {
                return Page();
            }
        }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public IActionResult OnPostAsync(IFormFile CompanyLogo)
        {
            int result = 0;
           
                if (CompanyLogo != null)
                {
                    string fileName = Guid.NewGuid().ToString("N") + CompanyLogo.FileName;
                    //var filePath = string.Concat(_webHostEnvironment.WebRootPath, "\\", @"\DBFiles\Company");
                    //var fileNameWithPath = string.Concat(filePath, "\\", fileName);
                    var filePath = Path.Combine("wwwroot\\DBFiles\\Company");
                    string fileNameWithPath = Path.Combine(filePath, fileName);                   
                    using (var stream = new FileStream(fileNameWithPath, FileMode.Create))
                    {
                        CompanyLogo.CopyTo(stream);
                        CompanyViewModel.CompanyLogoUrl = fileName;
                    }
                }

            result = _CompanyManager.ManageCompany(CompanyViewModel, _loginSession.UserViewModel.UserId);
            if (result > 0)
                return RedirectToPage("Index");
            //return RedirectToPage("Manage", new { id = result });

            else
                return RedirectToPage("Manage");
        }


        public IActionResult OnGetSearchBUName(string term)
        {
            if (term == null)
            {
                return BadRequest();
            }
            var data = _context.TblBusinessUnits
                .Where(p => p.Name.Contains(term) && p.IsActive == true)
                 .Select(s => new SelectListItem
                 {
                     Value = s.Name,
                     Text = s.BusinessUnitId.ToString()
                 })
                .ToArray();
            return new JsonResult(data);
        }

    }
}
