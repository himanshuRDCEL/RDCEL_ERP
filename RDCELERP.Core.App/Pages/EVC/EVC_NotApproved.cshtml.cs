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
    public class EVC_NotApprovedModel : BasePageModel
    {
        #region Variable declartion
        private readonly IEVCManager _EVCManager;
        private readonly RDCELERP.DAL.Entities.Digi2l_DevContext _context;
        private readonly IUserManager _UserManager;
        #endregion

        #region Constracter
        public EVC_NotApprovedModel(RDCELERP.DAL.Entities.Digi2l_DevContext context, IOptions<ApplicationSettings> config, IEVCManager EVCManager, IUserManager userManager) : base(config)
        {
            _EVCManager = EVCManager;
            _context = context;
            _UserManager = userManager;
        }
        #endregion

        #region Model Binding
        public EVC_NotApprovedViewModel evc_NotApprovedModel { get; set; }
        [BindProperty(SupportsGet = true)]
        public TblEvcregistration TblEvcregistrations { get; set; }
        public UserViewModel UserViewModel { get; set; }
        public UserRoleViewModel UserRoleViewModel { get; set; }
        public TblCompany tblCompany { get; set; }
        DateTime _currentDatetime = DateTime.Now.TrimMilliseconds();
        #endregion
        public async Task OnGetAsync()
        {

        }
        //this method use for Approved EVC and Genrete Role and Send ID/Password for EVC Mail
        public async Task<IActionResult> OnGetApprovedEVCAsync(int RegistionId)
        {
            if (RegistionId == null)
            {
                return NotFound();
            }
            int EvcApproved = 0;
            int UserId = 0;

            TblEvcregistrations = await _context.TblEvcregistrations.FindAsync(RegistionId);

            if (TblEvcregistrations != null)
            {
                #region Create new User in TblUser Table 
                UserId = _UserManager.ManageEvcUser(TblEvcregistrations, _loginSession.UserViewModel.UserId, _loginSession.RoleViewModel.CompanyId);
                if (UserId > 0)
                {
                    TblEvcregistrations.EvcapprovalStatusId = _loginSession.UserViewModel.ModifiedBy;
                    TblEvcregistrations.Isevcapprovrd = true;
                    TblEvcregistrations.UserId = UserId;
                    TblEvcregistrations.ModifiedBy = _loginSession.UserViewModel.ModifiedBy;
                    TblEvcregistrations.ModifiedDate = _currentDatetime;
                    _context.TblEvcregistrations.Update(TblEvcregistrations);
                    EvcApproved = await _context.SaveChangesAsync();
                }
                #endregion
            }
            return RedirectToPage("EVC_NotApproved", new { RegistionId = TblEvcregistrations.EvcregistrationId });
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
                if (TblEvcregistrations != null && TblEvcregistrations.EvcregistrationId > 0)
                {
                    TblEvcregistrations = _context.TblEvcregistrations.Find(TblEvcregistrations.EvcregistrationId);
                }

                if (TblEvcregistrations != null)
                {
                    TblEvcregistrations.IsActive = false;
                    TblEvcregistrations.Isevcapprovrd = false;                  
                    TblEvcregistrations.ModifiedBy = _loginSession.UserViewModel.UserId;
                    TblEvcregistrations.ModifiedDate = _currentDatetime;
                    _context.TblEvcregistrations.Update(TblEvcregistrations);
                    _context.SaveChanges();

                    List<TblEvcPartner> tblEvcPartnerlist = _context.TblEvcPartners.Where(x => x.EvcregistrationId == TblEvcregistrations.EvcregistrationId).ToList();
                    if (tblEvcPartnerlist != null)
                    {
                        foreach (var tblPartner in tblEvcPartnerlist)
                        {
                            tblPartner.IsActive = false;
                            tblPartner.IsApprove = false;
                            tblPartner.ModifiedBy = _loginSession.UserViewModel.UserId;
                            tblPartner.ModifiedDate = _currentDatetime;
                            _context.TblEvcPartners.Update(tblPartner);
                            _context.SaveChanges();

                        }
                    }
                }

                return RedirectToPage("./EVC_NotApproved/");
            }
        }

        public IActionResult OnPostCheckOrder(int evcregistrationId)
        {
            bool isValid = _context.TblWalletTransactions
                .Any(x => x.EvcregistrationId == TblEvcregistrations.EvcregistrationId && x.IsActive == true && x.StatusId != "32");
            return new JsonResult(isValid);
        }


        #region Autopopulate Search Filter for search by EVCRegdNo
        public IActionResult OnGetSearchEVCRegdNo(string term)
        {
            if (term == null)
            {
                return BadRequest();
            }

           var data = _context.TblEvcregistrations.Where(x => x.IsActive == true /*&& x.Isevcapprovrd == false*/ && x.EvcregdNo.Contains(term))
            .Select(x => x.EvcregdNo)
            .ToArray();
            return new JsonResult(data);
        }
        #endregion

        public IActionResult OnPostCheckListofPincode(int evcregistrationId)
        {
            bool isValid = _context.TblEvcPartners
                .Any(x => x.EvcregistrationId == evcregistrationId && x.IsActive == true &&x.IsApprove==false);
            return new JsonResult(isValid);
        }
    }
}
