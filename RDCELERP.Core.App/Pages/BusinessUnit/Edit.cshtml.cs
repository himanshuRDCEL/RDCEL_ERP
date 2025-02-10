using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using RDCELERP.BAL.Interface;
using RDCELERP.Core.App.Pages.Base;
using RDCELERP.DAL.Entities;
using RDCELERP.Model.Base;
using RDCELERP.Model.BusinessUnit;

namespace RDCELERP.Core.App.Pages.BusinessUnit
{
    public class EditModel : CrudBasePageModel
    {
        #region Variable Declaration
        private readonly IBusinessUnitManager _BusinessUnitManager;
        private readonly RDCELERP.DAL.Entities.Digi2l_DevContext _context;
        #endregion

        public EditModel(IBusinessUnitManager BusinessUnitManager, Digi2l_DevContext context, IOptions<ApplicationSettings> config, CustomDataProtection protector) : base(config, protector)
        {
            _BusinessUnitManager = BusinessUnitManager;
            _context = context;
        }

        [BindProperty(SupportsGet = true)]
        public BusinessUnitViewModel BusinessUnitViewModel { get; set; }

       
            public IActionResult OnGet(string id)
            {
                if (id != null)
                    id = _protector.Decode(id);
                BusinessUnitViewModel = _BusinessUnitManager.GetBusinessUnitById(Convert.ToInt32(id));

                if (BusinessUnitViewModel == null)
                BusinessUnitViewModel = new BusinessUnitViewModel();

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
                    result = _BusinessUnitManager.ManageBusinessUnit(BusinessUnitViewModel, _loginSession.UserViewModel.UserId);
                    if (result == 0)
                    {

                        ViewData["Message"] = "This Business Unit is already exist";

                        return Page();

                    }
                }
            

                if (result > 0)
                    return RedirectToPage("./Index", new { id = _protector.Encode(result) });
                else

                    return RedirectToPage("./Edit");
            }
        public IActionResult OnPostCheckName(string Name)
        {
            TblBusinessUnit TblBusinessUnit = new TblBusinessUnit();
            string nameTrimmed = Name?.Trim(); // Trim the Name parameter
            bool isValid = !_context.TblBusinessUnits.ToList().Exists(p => p.Name.Trim().Equals(nameTrimmed, StringComparison.CurrentCultureIgnoreCase));
            return new JsonResult(isValid);

        }

    }
    }

