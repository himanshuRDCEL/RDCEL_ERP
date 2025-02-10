using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Options;
using RDCELERP.BAL.Interface;
using RDCELERP.Common.Helper;
using RDCELERP.Core.App.Pages.Base;
using RDCELERP.DAL.Entities;
using RDCELERP.Model.Base;
using RDCELERP.Model.State;

namespace RDCELERP.Core.App.Pages.State
{
    public class ManageModel : CrudBasePageModel
    {
        #region Variable Declaration
        private readonly IStateManager _StateManager;
        private readonly RDCELERP.DAL.Entities.Digi2l_DevContext _context;
        #endregion

        public ManageModel(IStateManager StateManager, Digi2l_DevContext context, IOptions<ApplicationSettings> config, CustomDataProtection protector) : base(config, protector)
        {
            _StateManager = StateManager;
            _context = context;
        }

        [BindProperty(SupportsGet = true)]
        public StateViewModel StateViewModel { get; set; }
        [BindProperty(SupportsGet = true)]
        public StateVMExcel StateVM { get; set; }

        public IActionResult OnGet(string id)
        {
            if (id != null)
                id = _protector.Decode(id);
            StateViewModel = _StateManager.GetStateById(Convert.ToInt32(id));
            if (StateViewModel == null)
                StateViewModel = new StateViewModel();
            if (_loginSession == null)
            {
                return RedirectToPage("./Index");
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
            result = _StateManager.ManageState(StateViewModel, _loginSession.UserViewModel.UserId);
            
            if (result > 0)
                return RedirectToPage("./Index", new { id = _protector.Encode(result) });
            else
                return RedirectToPage("./Manage", new { id = _protector.Encode(result) });
        }
        public IActionResult OnPostCheckName(string? name, int? stateId)
        {
            string? nameTrimmed = string.Empty;
            if (stateId > 0)
            {
                TblState TblState = new TblState();
                nameTrimmed = name?.Trim(); // Trim the Name parameter
                bool isValid = !_context.TblStates.ToList().Exists(p => p.Name.Trim() == nameTrimmed && p.StateId != stateId);               
                return new JsonResult(isValid);
            }
            else
            {
                TblState TblState = new TblState();
                nameTrimmed = name?.Trim(); // Trim the Name parameter
                bool isValid = !_context.TblStates.ToList().Exists(p => p.Name.Trim().Equals(nameTrimmed, StringComparison.CurrentCultureIgnoreCase));
                return new JsonResult(isValid);
            }          
        }
        public IActionResult OnPostCheckCode(string? Code, int? stateId)
        {
            string? codeTrimmed = string.Empty;
            if (stateId > 0)
            {
                TblState TblState = new TblState();
                codeTrimmed = Code?.Trim(); // Trim the Name parameter
                bool isValid = !_context.TblStates.ToList().Exists(p => p.Code.Equals(codeTrimmed, StringComparison.CurrentCultureIgnoreCase) && p.StateId != stateId);      
                return new JsonResult(isValid);
            }
            else
            {
                TblState TblState = new TblState();
                codeTrimmed = Code?.Trim();
                bool isValid = !_context.TblStates.ToList().Exists(p => p.Code.Equals(codeTrimmed, StringComparison.CurrentCultureIgnoreCase));
                return new JsonResult(isValid);
            }            
        }  
    }
}
