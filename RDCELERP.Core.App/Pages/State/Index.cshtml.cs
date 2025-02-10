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
using RDCELERP.Common.Enums;
using RDCELERP.Common.Helper;
using RDCELERP.Core.App.Pages.Base;
using RDCELERP.DAL.Entities;
using RDCELERP.DAL.IRepository;
using RDCELERP.Model.Base;
using RDCELERP.Model.State;

namespace RDCELERP.Core.App.Pages.State
{
    public class IndexModel : CrudBasePageModel
    {
        private readonly RDCELERP.DAL.Entities.Digi2l_DevContext _context;
        private readonly IStateManager _StateManager;
        private readonly IStateRepository _StateRepository;

        public IndexModel(RDCELERP.DAL.Entities.Digi2l_DevContext context, IStateRepository StateRepository, IStateManager StateManager, IOptions<ApplicationSettings> config, CustomDataProtection protector) : base(config, protector)
        {
            _context = context;
            _StateManager = StateManager;
            _StateRepository = StateRepository;
        }

        [BindProperty(SupportsGet = true)]
        public StateViewModel StateVM { get; set; }
        [BindProperty(SupportsGet = true)]
        public StateVMExcel StateVMExcel { get; set; }

        [BindProperty(SupportsGet = true)]
        public IList<TblState> TblState { get; set; }
        [BindProperty(SupportsGet = true)]
        public TblState TblStateObj { get; set; }

        public IActionResult OnGet()
        {
            TblStateObj = new TblState();

            if (_loginSession == null)
            {
                return RedirectToPage("/index");
            }
            else
            {
                StateVM = _StateManager.GetStateById(_loginSession.UserViewModel.UserId);

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
                if (TblStateObj != null && TblStateObj.StateId > 0)
                {
                    TblStateObj = _context.TblStates.Find(TblStateObj.StateId);
                }

                if (TblStateObj != null)
                {
                    if (TblStateObj.IsActive == true)
                    {
                        TblStateObj.IsActive = false;
                    }
                    else
                    {
                        TblStateObj.IsActive = true;
                    }
                    TblStateObj.ModifiedBy = _loginSession.UserViewModel.UserId;
                    _context.TblStates.Update(TblStateObj);
                    //  _context.TblRoles.Remove(TblRole);
                    _context.SaveChanges();
                }

                return RedirectToPage("./index");
            }
        }

    }
}




