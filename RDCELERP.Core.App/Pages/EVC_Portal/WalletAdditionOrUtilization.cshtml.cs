using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;
using RDCELERP.BAL.Interface;
using RDCELERP.Common.Enums;
using RDCELERP.Common.Helper;
using RDCELERP.Core.App.Pages.Base;
using RDCELERP.Core.App.Pages.EVC;
using RDCELERP.DAL.Entities;
using RDCELERP.Model.Base;
using RDCELERP.Model.Company;
using RDCELERP.Model.EVC;
using RDCELERP.Model.Users;
namespace RDCELERP.Core.App.Pages.EVC_Portal
{
    public class WalletAdditionOrUtilizationModel : BasePageModel
    {
        private readonly RDCELERP.DAL.Entities.Digi2l_DevContext _context;
        private readonly IEVCManager _EVCManager;
        [BindProperty(SupportsGet = true)]
        public int userId { get; set; }
        public WalletAdditionOrUtilizationModel(RDCELERP.DAL.Entities.Digi2l_DevContext context, IOptions<ApplicationSettings> config, IEVCManager EVCManager, CustomDataProtection protector) : base(config)

        {
            _EVCManager = EVCManager;
            _context = context;
        }
        [BindProperty(SupportsGet = true)]
        public TblEvcregistration TblEvcregistrations { get; set; }
        [BindProperty(SupportsGet = true)]
        public IList<EVC_ApprovedModel> EVC_ApprovedModels { get; set; }
        public TblEvcwalletAddition EvcwalletAdditions { get; set; }
        public EVCWalletAdditionViewModel EVCWalletAdditionViewModels { get; set; }
        DateTime _currentDatetime = DateTime.Now.TrimMilliseconds();
        public async Task OnGetAsync(int? UserId)
        {
            if (UserId != null)
            {
                userId = (int)UserId;

            }
            else
            {
                userId = _loginSession.UserViewModel.UserId;
            }
           // userId = _loginSession.UserViewModel.UserId;
        }
        //public IActionResult OnPostDeleteAsync()
        //{
        //    if (_loginSession == null)
        //    {
        //        return RedirectToPage("/Index");
        //    }
        //    else
        //    {
        //        if (TblEvcregistrations != null && TblEvcregistrations.EvcregistrationId > 0)
        //        {
        //            TblEvcregistrations = _context.TblEvcregistrations.Find(TblEvcregistrations.EvcregistrationId);
        //        }

        //        if (TblEvcregistrations != null)
        //        {
        //            TblEvcregistrations.IsActive = false;
        //            TblEvcregistrations.ModifiedBy = _loginSession.UserViewModel.UserId;
        //            _context.TblEvcregistrations.Update(TblEvcregistrations);
        //            _context.SaveChanges();
        //        }
        //        return RedirectToPage("./EVC_Approved/");
        //    }
        //}

        //public IActionResult OnPostWalletSaveAsync()
        //{
        //    EVCWalletAdditionViewModel EVCWalletAdditionViewModels = new EVCWalletAdditionViewModel();
        //    long result = 0;
        //    if (_loginSession == null)
        //    {
        //        return RedirectToPage("/Index");
        //    }
        //    else
        //    {
        //        //Amount = TblEvcregistrations.EvcwalletAmount;
        //        if (TblEvcregistrations != null && TblEvcregistrations.EvcregistrationId > 0)
        //        {
        //            EVCWalletAdditionViewModels.EvcregistrationId = TblEvcregistrations.EvcregistrationId;
        //            EVCWalletAdditionViewModels.Amount = (long)TblEvcregistrations.EvcwalletAmount;
        //            EVCWalletAdditionViewModels.CreatedBy = _loginSession.UserViewModel.UserId;

        //            // EVCWalletAdditionViewModels.ModifiedBy = _loginSession.UserViewModel.UserId;
        //            EVCWalletAdditionViewModels.ModifiedDate = _currentDatetime;

        //            result = _EVCManager.SaveEVCWalletDetails(EVCWalletAdditionViewModels);
        //            if (result != null)
        //            {
        //                TblEvcregistrations = _context.TblEvcregistrations.Find(TblEvcregistrations.EvcregistrationId);
        //            }
        //            else
        //            {
        //                //pass Messege  for not add wallet amount 
        //            }
        //        }
        //        if (TblEvcregistrations != null)
        //        {
        //            // TblEvcregistrations.IsActive = false;
        //            TblEvcregistrations.EvcwalletAmount = TblEvcregistrations.EvcwalletAmount + result;
        //            TblEvcregistrations.ModifiedBy = _loginSession.UserViewModel.UserId;
        //            // TblEvcregistrations.ModifiedDate = _currentDatetime; 

        //            _context.TblEvcregistrations.Update(TblEvcregistrations);
        //            _context.SaveChanges();
        //        }
        //        return RedirectToPage("./EVC_Approved/");
        //    }
        //}
        //public JsonResult OnGetCurrentWalletRecordAsync(int id)
        //{
        //    return new JsonResult(_EVCManager.GetEvcByEvcregistrationId(id));
        //}
    }
}
