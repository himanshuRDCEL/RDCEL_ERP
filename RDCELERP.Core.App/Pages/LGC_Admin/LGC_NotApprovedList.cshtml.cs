using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;
using RDCELERP.BAL.Interface;
using RDCELERP.Common.Helper;
using RDCELERP.Core.App.Pages.Base;
using RDCELERP.DAL.Entities;
using RDCELERP.Model.Base;
using RDCELERP.Model.EVC;
using RDCELERP.Model.Users;
using static Org.BouncyCastle.Math.EC.ECCurve;

namespace RDCELERP.Core.App.Pages.LGC_Admin
{
    public class LGC_NotApprovedListModel : BasePageModel
    {
        #region Variable declartion
        private readonly IEVCManager _EVCManager;
        private readonly RDCELERP.DAL.Entities.Digi2l_DevContext _context;
        private readonly IUserManager _UserManager;
        #endregion

        #region Constracter
        public LGC_NotApprovedListModel(RDCELERP.DAL.Entities.Digi2l_DevContext context, IOptions<ApplicationSettings> config, IEVCManager EVCManager, IUserManager userManager) : base(config)
        {
            _EVCManager = EVCManager;
            _context = context;
            _UserManager = userManager;
        }
        #endregion

        #region Model Binding
        public EVC_NotApprovedViewModel evc_NotApprovedModel { get; set; }
        [BindProperty(SupportsGet = true)]
        public TblServicePartner TblServicePartners { get; set; }
        public UserViewModel UserViewModel { get; set; }
        public UserRoleViewModel UserRoleViewModel { get; set; }
        public TblCompany tblCompany { get; set; }

        DateTime _currentDatetime = DateTime.Now.TrimMilliseconds();
        #endregion
        public async Task OnGetAsync()
        {

        }
        //this method use for Approved EVC and Genrete Role and Send ID/Password for EVC Mail
        public async Task<IActionResult> OnGetApprovedServicePartnerAsync(int ServicePartnerId)
        {
            if (ServicePartnerId == null)
            {
                return NotFound();
            }
            int EvcApproved = 0;
            int UserId = 0;

            TblServicePartners = await _context.TblServicePartners.FindAsync(ServicePartnerId);

            if (TblServicePartners != null)
            {
                #region Create new User in TblUser Table 
                UserId = _UserManager.ManageServicePartnerUser(TblServicePartners, _loginSession.UserViewModel.UserId, _loginSession.RoleViewModel.CompanyId);
                if (UserId > 0)
                {
                    TblServicePartners.ServicePartnerLoginId = _loginSession.UserViewModel.UserId;
                    TblServicePartners.ServicePartnerIsApprovrd = true;
                    TblServicePartners.UserId = UserId;
                    TblServicePartners.Modifiedby= _loginSession.UserViewModel.UserId;
                    TblServicePartners.ModifiedDate = _currentDatetime;
                    _context.TblServicePartners.Update(TblServicePartners);
                    EvcApproved = await _context.SaveChangesAsync();                   
                }
                else if (TblServicePartners != null && TblServicePartners.UserId > 0)
                {
                    TblServicePartners.ServicePartnerLoginId = _loginSession.UserViewModel.UserId;
                    TblServicePartners.ServicePartnerIsApprovrd = true;
                    TblServicePartners.Modifiedby = _loginSession.UserViewModel.UserId;
                    TblServicePartners.ModifiedDate = _currentDatetime;
                    _context.TblServicePartners.Update(TblServicePartners);
                    EvcApproved = await _context.SaveChangesAsync();
                }
                #endregion
            }
            return RedirectToPage("LGC_NotApprovedList", new { ServicePartnerId = TblServicePartners.ServicePartnerId });
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
                if (TblServicePartners != null && TblServicePartners.ServicePartnerId > 0)
                {
                    TblServicePartners = _context.TblServicePartners.Find(TblServicePartners.ServicePartnerId);
                    if (TblServicePartners != null)
                    {
                        TblServicePartners.IsActive = false;
                        TblServicePartners.ServicePartnerIsApprovrd = false;
                        // TblServicePartners.ModifiedBy = _loginSession.UserViewModel.UserId;
                        TblServicePartners.ModifiedDate = _currentDatetime;
                        _context.TblServicePartners.Update(TblServicePartners);
                        _context.SaveChanges();
                    }
                }
                return RedirectToPage("./LGC_ApprovedList/");
            }
        }
    }
}
