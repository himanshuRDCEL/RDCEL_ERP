using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using RDCELERP.BAL.Interface;
using RDCELERP.Core.App.Pages.Base;
using RDCELERP.DAL.Entities;
using RDCELERP.Model.ABBPlanMaster;
using RDCELERP.Model.Base;

namespace RDCELERP.Core.App.Pages.ABBPlanMaster
{
    public class IndexModel : CrudBasePageModel
    {
       
            private readonly RDCELERP.DAL.Entities.Digi2l_DevContext _context;
            private readonly IABBPlanMasterManager _ABBPlanMasterManager;

            public IndexModel(RDCELERP.DAL.Entities.Digi2l_DevContext context, IABBPlanMasterManager ABBPlanMasterManager, IOptions<ApplicationSettings> config, CustomDataProtection protector) : base(config, protector)
            {
                _context = context;
            _ABBPlanMasterManager = ABBPlanMasterManager;
            }

            [BindProperty(SupportsGet = true)]
            public ABBPlanMasterViewModel ABBPlanMasterVM { get; set; }

            [BindProperty(SupportsGet = true)]
            public IList<TblAbbplanMaster> TblAbbplanMaster { get; set; }
            [BindProperty(SupportsGet = true)]
            public TblAbbplanMaster TblAbbplanMasterObj { get; set; }
            public IActionResult OnGet()
            {
               TblAbbplanMasterObj = new TblAbbplanMaster();

                if (_loginSession == null)
                {
                    return RedirectToPage("/index");
                }
                else
                {

                ABBPlanMasterVM = _ABBPlanMasterManager.GetABBPlanMasterById(_loginSession.UserViewModel.UserId);

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
                    if (TblAbbplanMasterObj != null && TblAbbplanMasterObj.PlanMasterId > 0)
                    {
                    TblAbbplanMasterObj = _context.TblAbbplanMasters.Find(TblAbbplanMasterObj.PlanMasterId);
                    }

                    if (TblAbbplanMasterObj != null)
                    {


                       TblAbbplanMasterObj.IsActive = false;
                       TblAbbplanMasterObj.ModifiedBy = _loginSession.UserViewModel.UserId;
                        _context.TblAbbplanMasters.Update(TblAbbplanMasterObj);
                        //  _context.TblRoles.Remove(TblRole);
                        _context.SaveChanges();
                    }

                    return RedirectToPage("./index");
                }
            }
        }
}
