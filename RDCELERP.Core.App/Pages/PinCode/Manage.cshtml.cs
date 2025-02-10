using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Options;
using RDCELERP.BAL.Interface;
using RDCELERP.Common.Helper;
using RDCELERP.Core.App.Pages.Base;
using RDCELERP.DAL.Entities;
using RDCELERP.Model.Base;

using RDCELERP.Model.PinCode;

namespace RDCELERP.Core.App.Pages.PinCode
{
    public class ManageModel : CrudBasePageModel
    {
        #region Variable Declaration
        private readonly IPinCodeManager _PinCodeManager;
        private readonly IStateManager _stateManager;
        private readonly ICityManager _cityManager;
        private readonly RDCELERP.DAL.Entities.Digi2l_DevContext _context;
        ILogging _logging;
        #endregion

        public ManageModel(ILogging logging, IPinCodeManager PinCodeManager, ICityManager cityManager, Digi2l_DevContext context, IStateManager stateManager, IOptions<ApplicationSettings> config, CustomDataProtection protector) : base(config, protector)
        {
            _PinCodeManager = PinCodeManager;
            _stateManager = stateManager;
            _cityManager = cityManager;
            _context = context;
            _logging = logging;
        }

        [BindProperty(SupportsGet = true)]
        public PinCodeViewModel PinCodeViewModel { get; set; }

        public IActionResult OnGet(string id)
        {
           
            if (id != null)
            {
                id = _protector.Decode(id);
                PinCodeViewModel = _PinCodeManager.GetPinCodeById(Convert.ToInt32(id));
                var state = _context.TblStates.Where(x => x.IsActive == true && x.Name == PinCodeViewModel.State).FirstOrDefault();
                if (state != null)
                {
                    PinCodeViewModel.StateId = state.StateId;
                }
               

            }

          

            if (PinCodeViewModel == null)
                PinCodeViewModel = new PinCodeViewModel();

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
            if (ModelState.IsValid)
            {
                result = _PinCodeManager.ManagePinCode(PinCodeViewModel, _loginSession.UserViewModel.UserId);
                if (result == 0)
                {

                    ViewData["Message"] = "This Pincode is already exist";

                    return Page();

                }
            }

            if (result > 0)
                return RedirectToPage("./Index", new { id = _protector.Encode(result) });
            else

                return RedirectToPage("./Manage");
        }

        public JsonResult OnGetCityByStateIdAsync()
        {

            var citylistS = _cityManager.GetCityBYState(PinCodeViewModel.State);
            if (citylistS != null)
            {
                ViewData["citylistS"] = new SelectList(citylistS, "Name", "Name");
            }
            return new JsonResult(citylistS);
        }

     
        public IActionResult OnPostCheckName(int zipcode, int? Id)
        {
            if (Id > 0)
            {

                bool isValid = !_context.TblPinCodes.ToList().Exists(p => p.ZipCode == zipcode && p.Id != Id);
               
                return new JsonResult(isValid);
            }
            else
            {
                bool isValid = !_context.TblPinCodes.ToList().Exists
                      (p => p.ZipCode == zipcode);

                return new JsonResult(isValid);
            }

        }




        //public IActionResult OnPostCheckName(string zipcode)
        //{


        //    #region Variable
        //    bool isValid = true;
        //    #endregion
        //    try
        //    {
        //        if (zipcode != string.Empty && zipcode != null)
        //        {
        //            TblPinCode TblPinCode = new TblPinCode();
        //            isValid = !_context.TblPinCodes.ToList().Exists
        //               (p => p.ZipCode.Equals(zipcode));

        //            if (isValid)
        //            {
        //                return new JsonResult(isValid);
        //            }
        //            else
        //            {
        //                return new JsonResult(true);
        //            }
        //        }
        //        else
        //        {
        //            return new JsonResult(true);
        //        }
        //    }
        //    catch(Exception ex)
        //    {
        //        _logging.WriteErrorToDB("ManageModel", "OnPostCheckName", ex);
        //        isValid = true;
        //    }

        //    return new JsonResult(isValid);

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

    }


}

