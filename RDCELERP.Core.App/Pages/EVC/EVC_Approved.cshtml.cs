using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using NPOI.SS.Formula.Functions;
using RDCELERP.BAL.Interface;
using RDCELERP.Common.Enums;
using RDCELERP.Common.Helper;
using RDCELERP.Core.App.Pages.Base;
using RDCELERP.DAL.Entities;
using RDCELERP.Model.Base;
using RDCELERP.Model.Company;
using RDCELERP.Model.EVC;
using RDCELERP.Model.Users;

namespace RDCELERP.Core.App.Pages.EVC
{
    public class EVC_ApprovedModel : BasePageModel
    {
        #region Variable declartion
        private readonly RDCELERP.DAL.Entities.Digi2l_DevContext _context;
        private readonly IEVCManager _EVCManager;
        #endregion

        # region Constructor
        public EVC_ApprovedModel(RDCELERP.DAL.Entities.Digi2l_DevContext context, IOptions<ApplicationSettings> config, IEVCManager EVCManager, CustomDataProtection protector) : base(config)
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

        public async Task OnGetAsync()
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
                if (TblEvcregistrations != null && TblEvcregistrations.EvcregistrationId > 0)
                {
                    TblEvcregistrations = _context.TblEvcregistrations.Find(TblEvcregistrations.EvcregistrationId);
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
                                tblPartner.IsActive= false;
                                tblPartner.IsApprove = false;
                                tblPartner.ModifiedBy = _loginSession.UserViewModel.UserId;
                                tblPartner.ModifiedDate = _currentDatetime;
                                _context.TblEvcPartners.Update(tblPartner);
                                _context.SaveChanges();

                            }
                        }
                    }
                }               
                return RedirectToPage("./EVC_Approved/");
            }
        }

        public IActionResult OnPostWalletSaveAsync()
        {
            EVCWalletAdditionViewModel EVCWalletAdditionViewModels = new EVCWalletAdditionViewModel();
            long result = 0;
            if (_loginSession == null)
            {
                return RedirectToPage("/Index");
            }
            else
            {
               
                 if (TblEvcregistrations != null && TblEvcregistrations.EvcregistrationId > 0)
                {
                    EVCWalletAdditionViewModels.EvcregistrationId = TblEvcregistrations.EvcregistrationId;
                    EVCWalletAdditionViewModels.Amount = (long)TblEvcregistrations.EvcwalletAmount;
                    EVCWalletAdditionViewModels.CreatedBy = _loginSession.UserViewModel.UserId;
                    EVCWalletAdditionViewModels.CreatedDate = _currentDatetime;
                    EVCWalletAdditionViewModels.ModifiedBy = _loginSession.UserViewModel.UserId;
                    EVCWalletAdditionViewModels.ModifiedDate = _currentDatetime;

                    result = _EVCManager.SaveEVCWalletDetails(EVCWalletAdditionViewModels, _loginSession.UserViewModel.UserId);
                    if (result != null)
                    {
                        TblEvcregistrations = _context.TblEvcregistrations.Find(TblEvcregistrations.EvcregistrationId);
                        if (TblEvcregistrations != null)
                        {
                            
                            TblEvcregistrations.EvcwalletAmount = (TblEvcregistrations.EvcwalletAmount>0|| TblEvcregistrations.EvcwalletAmount!=null? TblEvcregistrations.EvcwalletAmount:0) + result;
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
               
                return RedirectToPage("./EVC_Approved/");
            }
        }
        public JsonResult OnGetCurrentWalletRecordAsync(int id)
        {
            return new JsonResult(_EVCManager.GetEvcByEvcregistrationId(id));
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
          
            var data = _context.TblEvcregistrations.Where(x => x.IsActive == true && x.Isevcapprovrd == true && x.EvcregdNo.Contains(term))
            .Select(x => x.EvcregdNo)
            .ToArray();
            return new JsonResult(data);
        }
        #endregion

    }
}

