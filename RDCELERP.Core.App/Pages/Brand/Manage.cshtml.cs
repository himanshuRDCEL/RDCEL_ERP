using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Cuemon;
using DocumentFormat.OpenXml.Drawing.Charts;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using RDCELERP.BAL.Interface;
using RDCELERP.Core.App.Pages.Base;
using RDCELERP.DAL.Entities;
using RDCELERP.Model.Base;
using RDCELERP.Model.Company;
using RDCELERP.Model.State;

namespace RDCELERP.Core.App.Pages.Brands
{
    public class ManageModel : CrudBasePageModel
    {
        #region Variable Declaration
        private readonly IBrandManager _BrandManager;
        private IWebHostEnvironment _webHostEnvironment;
        public string LblMessage { get; set; } = string.Empty;
        private readonly RDCELERP.DAL.Entities.Digi2l_DevContext _context;


        #endregion

        public ManageModel(IBrandManager BrandManager, Digi2l_DevContext context, IWebHostEnvironment webHostEnvironment, IOptions<ApplicationSettings> config, CustomDataProtection protector) : base(config, protector)
        {
            _BrandManager = BrandManager;
            _webHostEnvironment = webHostEnvironment;
            _context = context;
        }

        [BindProperty(SupportsGet = true)]
        public BrandViewModel BrandViewModel { get; set; }
        public TblBrand tblBrand { get; set; }
        public dynamic ViewBag { get; set; }

        public IActionResult OnGet(string id)
        {
            if (id != null)
            {
                id = _protector.Decode(id);
                BrandViewModel = _BrandManager.GetBrandById(Convert.ToInt32(id));
                var CurrentBU = _context.TblBusinessUnits.Where(x => x.BusinessUnitId == BrandViewModel.BusinessUnitId).FirstOrDefault();
                if (CurrentBU != null)
                {
                    BrandViewModel.BusinessUnitName = CurrentBU.Name;
                }
                if (!string.IsNullOrEmpty(BrandViewModel.BrandLogoUrl))
                {
                    BrandViewModel.BrandLogoUrlLink = _baseConfig.Value.BaseURL + "/DBFiles/Brands/" + BrandViewModel.BrandLogoUrl;
                }
            }

            if (BrandViewModel == null)
                BrandViewModel = new BrandViewModel();
            string URL = _baseConfig.Value.URLPrefixforProd;


            //ViewData["CountryList"] = new SelectList(_countryManager.GetAllCountries(), "CountryId", "Name");
            if (!string.IsNullOrEmpty(BrandViewModel.BrandLogoUrl))
            {
                BrandViewModel.BrandLogoUrlLink = _baseConfig.Value.BaseURL + "/DBFiles/Brands/" + BrandViewModel.BrandLogoUrl;
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

        [ViewData]
        public string Message { get; set; }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public IActionResult OnPostAsync(IFormFile BrandLogo)
        {
            int result = 0;

            if (BrandLogo != null)
            {
                string fileName = Guid.NewGuid().ToString("N") + BrandLogo.FileName;
                var filePath = string.Concat(_webHostEnvironment.WebRootPath, "\\", @"\DBFiles\Brands");
                var fileNameWithPath = string.Concat(filePath, "\\", fileName);
                using (var stream = new FileStream(fileNameWithPath, FileMode.Create))
                {
                    BrandLogo.CopyTo(stream);
                    BrandViewModel.BrandLogoUrl = fileName;
                }
            }
            else
            {
                var image = Path.GetFileName(BrandViewModel.BrandLogoUrl);
                BrandViewModel.BrandLogoUrl = image;
                BrandViewModel.BrandLogoUrlLink = image;
            }

            result = _BrandManager.ManageBrand(BrandViewModel, _loginSession.UserViewModel!.UserId);

            if (result > 0)
                return RedirectToPage("Index", new { id = _protector.Encode(result) });
            else
                return RedirectToPage("Manage");
        }
        public IActionResult OnPostCheckName(string Name, int? Id)
        {
            string? nameTrimmed = string.Empty;
            if (Id > 0)
            {
                TblBrand TblBrand = new TblBrand();
                nameTrimmed = Name?.Trim(); // Trim the Name parameter
                bool isValid = !_context.TblBrands.Where(x => !string.IsNullOrEmpty(x.Name)).ToList().Exists(p => p.Name == Name && p.Id != Id);
                return new JsonResult(isValid);
            }
            else
            {
                TblBrand TblBrand = new TblBrand();
                nameTrimmed = Name?.Trim(); // Trim the Name parameter
                bool isValid = !_context.TblBrands.Where(x => !string.IsNullOrEmpty(x.Name)).ToList().Exists(p => p!.Name!.Trim().Equals(nameTrimmed, StringComparison.CurrentCultureIgnoreCase));
                return new JsonResult(isValid);
            }
        }

        public IActionResult OnGetSearchBuidName(string term)
        {
            List<TblBusinessUnit> tblBusinessUnit = new List<TblBusinessUnit>();
            if (term == null)
            {
                return BadRequest();
            }
            var data = _context.TblBusinessUnits.Where(p => !string.IsNullOrEmpty(p.Name) && p.Name.Contains(term) && p.IsActive == true)
                    .Select(s => new SelectListItem
                    {
                        Value = s.Name,
                        Text = s.BusinessUnitId.ToString()
                    }).ToArray();

            return new JsonResult(data);
        }
    }
}

