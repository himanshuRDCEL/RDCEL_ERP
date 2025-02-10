using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using RDCELERP.BAL.Interface;
using RDCELERP.Common.Helper;
using RDCELERP.Core.App.Pages.Base;
using RDCELERP.DAL.Entities;
using RDCELERP.DAL.IRepository;
using RDCELERP.Model.Base;
using RDCELERP.Model.City;

namespace RDCELERP.Core.App.Pages.City
{
    public class IndexModel : CrudBasePageModel
    {

        private readonly RDCELERP.DAL.Entities.Digi2l_DevContext _context;
        private readonly ICityManager _CityManager;
        private readonly ICityRepository _CityRepository;
        public IndexModel(RDCELERP.DAL.Entities.Digi2l_DevContext context, ICityManager CityManager, ICityRepository CityRepository, IOptions<ApplicationSettings> config, CustomDataProtection protector) : base(config, protector)
        {
            _context = context;
            _CityManager = CityManager;
            _CityRepository = CityRepository;
        }

        [BindProperty(SupportsGet = true)]
        public CityViewModel CityVM { get; set; }
        [BindProperty(SupportsGet = true)]
        public CityVMExcel CityVMExcel { get; set; }
        [BindProperty(SupportsGet = true)]
        public IList<TblCity> TblCity { get; set; }
        [BindProperty(SupportsGet = true)]
        public TblCity TblCityObj { get; set; }
        public IActionResult OnGet()
        {
            TblCityObj = new TblCity();

            if (_loginSession == null)
            {
                return RedirectToPage("/index");
            }
            else
            {
                CityVM = _CityManager.GetCityById(_loginSession.UserViewModel.UserId);

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
                if (TblCityObj != null && TblCityObj.CityId > 0)
                {
                    TblCityObj = _context.TblCities.Find(TblCityObj.CityId);
                }

                if (TblCityObj != null)
                {
                    if (TblCityObj.IsActive == true)
                    {
                        TblCityObj.IsActive = false;
                    }
                    else
                    {
                        TblCityObj.IsActive = true;
                    }
                    TblCityObj.ModifiedBy = _loginSession.UserViewModel.UserId;
                    _context.TblCities.Update(TblCityObj);
                    //  _context.TblRoles.Remove(TblRole);
                    _context.SaveChanges();
                }

                return RedirectToPage("./index");
            }
        }

        
    }
}

        
    