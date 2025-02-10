using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using RDCELERP.BAL.Interface;
using RDCELERP.BAL.MasterManager;
using RDCELERP.Common.Constant;
using RDCELERP.Common.Helper;
using RDCELERP.Core.App.Pages.Base;
using RDCELERP.DAL.Entities;
using RDCELERP.Model.Base;
using RDCELERP.Model.EVC;
using RDCELERP.Model.Users;


namespace RDCELERP.Core.App.Pages.EVC
{
    public class EVC_PartnerListforAdminModel : BasePageModel
    {
        #region Variable declartion
        private readonly RDCELERP.DAL.Entities.Digi2l_DevContext _context;
        private readonly IEVCManager _EVCManager;
        private DateTime? _currentDatetime;
        #endregion

        #region Constructor
        public EVC_PartnerListforAdminModel(RDCELERP.DAL.Entities.Digi2l_DevContext context, IOptions<ApplicationSettings> config, IEVCManager EVCManager, CustomDataProtection protector) : base(config)
        {
            _EVCManager = EVCManager;
            _context = context;
             _currentDatetime = DateTime.Now.TrimMilliseconds();
        }
        #endregion
        [BindProperty(SupportsGet = true)]
        public TblEvcPartner TblEvcPartners { get; set; }
        public void OnGet()
        {
        }

        //This Method use for Delet EVC
        public IActionResult OnPostDeleteAsync()
        {
            if (_loginSession == null)
            {
                return RedirectToPage("/Index");
            }
            else
            {
                if (TblEvcPartners != null && TblEvcPartners.EvcPartnerId > 0)
                {
                    TblEvcPartners = _context.TblEvcPartners.Find(TblEvcPartners.EvcPartnerId);
                    if (TblEvcPartners != null)
                    {
                        TblEvcPartners.IsActive = false;
                        TblEvcPartners.IsApprove = false;
                        TblEvcPartners.ModifiedBy = _loginSession.UserViewModel.UserId;
                        TblEvcPartners.ModifiedDate = _currentDatetime;
                        _context.TblEvcPartners.Update(TblEvcPartners);
                        _context.SaveChanges();
                    }
                }
                return RedirectToPage("./EVC_Approved/");
            }
        }

        public IActionResult OnPostCheckOrder(int evcregistrationId)
        {
            bool isValid = _context.TblWalletTransactions
                .Any(x => x.EvcpartnerId == evcregistrationId && x.IsActive == true && x.StatusId != "32");
            return new JsonResult(isValid);
        }

    }
}
