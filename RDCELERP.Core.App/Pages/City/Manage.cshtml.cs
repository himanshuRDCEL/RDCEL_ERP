using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Options;
using RDCELERP.BAL.Interface;
using RDCELERP.BAL.MasterManager;
using RDCELERP.Core.App.Pages.Base;
using RDCELERP.DAL.Entities;
using RDCELERP.Model.Base;
using RDCELERP.Model.City;
using System.Data;
using static System.Runtime.CompilerServices.RuntimeHelpers;
using Microsoft.AspNetCore.Hosting;
using RDCELERP.BAL.Helper;
using static ICSharpCode.SharpZipLib.Zip.ExtendedUnixData;
using RDCELERP.Model.Master;

namespace RDCELERP.Core.App.Pages.City
{
    public class ManageModel : CrudBasePageModel
    {
        #region Variable Declaration
        private readonly ICityManager _CityManager;
        private readonly IStateManager _stateManager;
        private readonly IImageHelper _imageHelper;
        private readonly RDCELERP.DAL.Entities.Digi2l_DevContext _context;
        #endregion

        public ManageModel(ICityManager CityManager, IStateManager StateManager, Digi2l_DevContext context, IImageHelper imageHelper, IWebHostEnvironment webHostEnvironment, IOptions<ApplicationSettings> config, CustomDataProtection protector) : base(config, protector)
        { 
            _CityManager = CityManager;
            _stateManager = StateManager;
            _context = context;
            _imageHelper = imageHelper;
        }

        [BindProperty(SupportsGet = true)]
        public CityViewModel? CityViewModel { get; set; }
        [BindProperty(SupportsGet = true)]
        public TblState TblState { get; set; }

        public IActionResult OnGet(string id)
        {

            if (id != null)
            {
                id = _protector.Decode(id);
                CityViewModel = _CityManager.GetCityById(Convert.ToInt32(id));
                TblState? tblState = null;
                tblState = _context.TblStates.Where(x => x.StateId == CityViewModel.StateId).FirstOrDefault();
                CityViewModel.StateName = tblState?.Name;
              
            }

            if (!string.IsNullOrEmpty(CityViewModel?.CityLogo))
            {
                var extension = System.IO.Path.GetExtension(CityViewModel.CityLogo);
                
                CityViewModel.CityLogoLink = _baseConfig.Value.MVCBaseURL + "/Content/assets/img/cities/" + $"cities_{CityViewModel.Name}" + extension;
                CityViewModel.CityLogo = CityViewModel.CityLogoLink;
                CityViewModel.CityLogoUrl = CityViewModel.CityLogoLink;
            }

            if (CityViewModel == null)
                CityViewModel = new CityViewModel();

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
        public async Task<IActionResult> OnPostAsync(IFormFile CityLogo)
        {
            int result = 0;

            if (CityLogo != null)
            {
                //extract the extension from filename -
                var extension = System.IO.Path.GetExtension(CityLogo.FileName);

                string fileName = "cities_" + CityViewModel?.Name + extension;
                var filePath = _baseConfig.Value.MVCCityPhysicalURL;

                _imageHelper.SaveFileDefRoot(CityLogo, filePath, fileName);

                CityViewModel.CityLogoUrl = fileName;
                CityViewModel.CityLogo = fileName;
            }
            else
            {
                var image = Path.GetFileName(CityViewModel.CityLogo);
                CityViewModel.CityLogoUrl = image;
                CityViewModel.CityLogo = image;
            }
            result = _CityManager.ManageCity(CityViewModel, _loginSession.UserViewModel.UserId);

            if (result > 0)
                return RedirectToPage("./Index");
            else
                return RedirectToPage("./Manage");
        }
       
        public IActionResult OnPostCheckName(string? name, int? cityId)
        {
            TblCity TblCity = new TblCity();
            string? nameTrimmed = string.Empty;
            if (cityId > 0)
            {
                nameTrimmed = name?.Trim(); // Trim the Name parameter
                bool isValid = !_context.TblCities.Where(x=>!string.IsNullOrEmpty(x.Name)).ToList().Exists(p => p.Name!.Trim() == nameTrimmed && p.CityId != cityId);     
                return new JsonResult(isValid);
            }
            else
            {
                nameTrimmed = name?.Trim(); // Trim the Name parameter
                bool isValid = !_context.TblCities.Where(x => !string.IsNullOrEmpty(x.Name)).ToList().Exists(p => p.Name!.Trim().Equals(nameTrimmed, StringComparison.CurrentCultureIgnoreCase));
                return new JsonResult(isValid);
            }
        }
        public IActionResult OnPostCheckCode(string? CityCode, int? cityId)
        {
            TblCity TblCity = new TblCity();
            string? codeTrimmed = CityCode?.Trim(); // Trim the Name parameter
            if (cityId > 0)
            {
                bool isValid = !_context.TblCities.Where(x => !string.IsNullOrEmpty(x.CityCode)).ToList().Exists(p => p.CityCode!.Trim() == codeTrimmed && p.CityId != cityId);       
                return new JsonResult(isValid);
            }
            else
            {
                bool isValid = !_context.TblCities.Where(x=>!string.IsNullOrEmpty(x.CityCode)).ToList().Exists(p => p.CityCode!.Trim() == codeTrimmed);
                return new JsonResult(isValid);
            }
        }

        public IActionResult OnGetSearchStateName(string term)
        {
            if (term == null)
            {
                return BadRequest();
            }
            var data = _context.TblStates
                .Where(p => p.Name.Contains(term) && p.IsActive == true)
                 .Select(s => new SelectListItem
                 {
                     Value = s.Name,
                     Text = s.StateId.ToString()
                 })
                .ToArray();
            return new JsonResult(data);
        }
    }
}