using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Options;
using RDCELERP.BAL.Interface;
using RDCELERP.Common.Enums;
using RDCELERP.Common.Helper;
using RDCELERP.Core.App.Pages.Base;
using RDCELERP.DAL.Entities;
using RDCELERP.Model.Base;
using RDCELERP.Model.BusinessPartner;
using RDCELERP.Model.PinCode;

namespace RDCELERP.Core.App.Pages.BusinessPartner
{
    public class ManageModel : CrudBasePageModel
    {
        //private readonly RDCELERP.DAL.Entities.Digi2l_DevContext _context;
        #region Variable Declaration
        private readonly IBusinessPartnerManager _BusinessPartnerManager;
        private readonly IPinCodeManager _pinCodeManager;
        private readonly ICityManager _CityManager;
        private readonly IBusinessUnitManager _BusinessUnitManager;
        private readonly IStateManager _stateManager;
        private readonly ICityManager _cityManager;
        private readonly RDCELERP.DAL.Entities.Digi2l_DevContext _context;
        #endregion
        public ManageModel(IBusinessPartnerManager BusinessPartnerManager, IPinCodeManager pinCodeManager, IBusinessUnitManager BusinessUnitManager, Digi2l_DevContext context, ICityManager cityManager, IStateManager stateManager, ICityManager CityManager, IOptions<ApplicationSettings> config, CustomDataProtection protector) : base(config, protector)
        {
            _BusinessPartnerManager = BusinessPartnerManager;
            _CityManager = CityManager;
            _BusinessUnitManager = BusinessUnitManager;
            _stateManager = stateManager;
            _cityManager = cityManager;
            _pinCodeManager = pinCodeManager;
            _context = context;
        }

        [BindProperty(SupportsGet = true)]
        public BusinessPartnerViewModel BusinessPartnerViewModel { get; set; }
        public enum VoucherTypeEnum { get };
        public bool IsChecked { get; set; }
        public IActionResult OnGet(string id)
        {

            var enumData = from BusinessPartnerVoucherTypeEnum e in Enum.GetValues(typeof(BusinessPartnerVoucherTypeEnum))
                           select new
                           {
                               Discount = (int)e,
                               Cash = e.ToString()
                           };
            ViewData["VoucherTypeList"] = new SelectList(enumData, "Discount", "Cash");
            if (id != null)
            {
                id = _protector.Decode(id);
                BusinessPartnerViewModel = _BusinessPartnerManager.GetBusinessPartnerById(Convert.ToInt32(id));
              
            }

            BusinessPartnerViewModel = _BusinessPartnerManager.GetBusinessPartnerById(Convert.ToInt32(id));

            if (BusinessPartnerViewModel == null)
                BusinessPartnerViewModel = new BusinessPartnerViewModel();


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
        public async Task<IActionResult> OnPostAsync()
        {
            int result = 0;
            var Statelist = _stateManager.GetAllState();
            var BusinessUnitlist = _BusinessUnitManager.GetAllBusinessUnit();
            
            var enumData = from BusinessPartnerVoucherTypeEnum e in Enum.GetValues(typeof(BusinessPartnerVoucherTypeEnum))
                           select new
                           {
                               Discount = (int)e,
                               Cash = e.ToString()
                           };
            ViewData["VoucherTypeList"] = new SelectList(enumData, "Discount", "Cash");
            var citylist = _cityManager.GetCityBYState(BusinessPartnerViewModel.State);
            result = _BusinessPartnerManager.ManageBusinessPartner(BusinessPartnerViewModel, _loginSession.UserViewModel.UserId);
            if (result > 0)
                return RedirectToPage("Index", new { id = _protector.Encode(result) });
            else
                return RedirectToPage("Manage");

        }

    
        public IActionResult OnGetAutoStateName(string term)
        {
            if (term == null)
            {
                return BadRequest();
            }
            var data = _context.TblStates
                       .Where(s => s.Name.Contains(term) && s.IsActive == true)
                       .Select(s => new SelectListItem
                       {
                           Value = s.Name,
                           Text = s.StateId.ToString()
                       })
                       .ToArray();
            return new JsonResult(data);
        }

        public IActionResult OnGetAutoCityName(string term, string term2)
        {
            if (term == null)
            {
                return BadRequest();
            }
            var list = _context.TblCities
                       .Where(e => e.Name.Contains(term) && e.StateId == Convert.ToInt32(term2) && e.IsActive == true)
                        .Select(s => new SelectListItem
                        {
                            Value = s.Name,
                            Text = s.CityId.ToString()
                        })
                       .ToArray();
            return new JsonResult(list);
        }
        public IActionResult OnGetAutoPinCode(int term, int term2)
        {
            if (term <= 0)
            {
                return BadRequest();
            }
            var list = _context.TblPinCodes
                       .Where(e => e.ZipCode!=null && e.ZipCode!.ToString()!.Contains(term.ToString()) && e.CityId == (term2) && e.IsActive == true)
                        .Select(s => new SelectListItem
                        {
                            Value = s.ZipCode.ToString(),
                            Text = s.Id.ToString()
                        })
                       .ToArray();
            return new JsonResult(list);
        }

        public IActionResult OnGetAutoBusinessUnit(string term)
        {
            if (term == null)
            {
                return BadRequest();
            }
            var list = _context.TblBusinessUnits
                       .Where(e =>!string.IsNullOrEmpty(e.Name) && e.Name.Contains(term) && e.IsActive == true)
                        .Select(s => new SelectListItem
                        {
                            Value = s.Name!.ToString(),
                            Text = s.BusinessUnitId.ToString()
                        })
                       .ToArray();
            return new JsonResult(list);
        }

    }
}
