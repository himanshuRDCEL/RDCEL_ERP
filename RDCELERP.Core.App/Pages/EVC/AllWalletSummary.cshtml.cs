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

namespace RDCELERP.Core.App.Pages.EVC
{
    public class AllWalletSummaryModel : CrudBasePageModel
    {
        #region Variable declartion 
        private readonly RDCELERP.DAL.Entities.Digi2l_DevContext _context;
        private readonly IEVCManager _EVCManager;
        #endregion

        #region Constructor
        public AllWalletSummaryModel(RDCELERP.DAL.Entities.Digi2l_DevContext context, IOptions<ApplicationSettings> config, IEVCManager EVCManager, CustomDataProtection protector) : base(config, protector)
        
        {
            _EVCManager = EVCManager;
            _context = context;
        }
        #endregion

        #region Model Binding

        [BindProperty(SupportsGet = true)]
        public TblEvcregistration TblEvcregistrations { get; set; }
        [BindProperty(SupportsGet = true)]
        public IList<EVC_ApprovedModel> EVC_ApprovedModels { get; set; }
        public TblEvcwalletAddition EvcwalletAdditions { get; set; }
        public EVCWalletAdditionViewModel EVCWalletAdditionViewModels { get; set; }
        DateTime _currentDatetime = DateTime.Now.TrimMilliseconds();
        #endregion
        public void OnGet()
        {

        }
        //Method use for Add EVC Wallet Amount For Admin site 
        public IActionResult OnPostWalletSaveAsync()
        {
            EVCWalletAdditionViewModel EVCWalletAdditionViewModels = new EVCWalletAdditionViewModel();
            var result = 0;
            if (_loginSession == null)
            {              
                return RedirectToPage("/Index");
            }
            else
            {
                #region Save Data for  TblWalletaddition table and update TblEvcregistrations table 
                if (TblEvcregistrations != null && TblEvcregistrations.EvcregistrationId > 0)
                {
                    EVCWalletAdditionViewModels.EvcregistrationId = TblEvcregistrations.EvcregistrationId;
                    EVCWalletAdditionViewModels.Amount = (long)TblEvcregistrations.EvcwalletAmount;
                    EVCWalletAdditionViewModels.CreatedBy = _loginSession.UserViewModel.UserId;                   
                    EVCWalletAdditionViewModels.ModifiedDate = _currentDatetime;
                    EVCWalletAdditionViewModels.CreatedDate = _currentDatetime;
                    EVCWalletAdditionViewModels.ModifiedBy = _loginSession.UserViewModel.UserId;

                    result = _EVCManager.SaveEVCWalletDetails(EVCWalletAdditionViewModels, _loginSession.UserViewModel.UserId);
                    if (result != null)
                    {
                        TblEvcregistrations = _context.TblEvcregistrations.Find(TblEvcregistrations.EvcregistrationId);
                        if (TblEvcregistrations != null)
                        {                            
                            TblEvcregistrations.EvcwalletAmount = TblEvcregistrations.EvcwalletAmount + result;
                            TblEvcregistrations.ModifiedBy = _loginSession.UserViewModel.UserId;
                            TblEvcregistrations.ModifiedDate = _currentDatetime; 

                            _context.TblEvcregistrations.Update(TblEvcregistrations);
                            _context.SaveChanges();
                        }
                    }
                    else
                    {
                        //pass Messege  for not add wallet amount 
                    }
                }
                #endregion

                return RedirectToPage("./AllWalletSummary/");
            }
        }
    }
}
