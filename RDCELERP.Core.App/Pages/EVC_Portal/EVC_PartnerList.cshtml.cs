using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;
using RDCELERP.BAL.Interface;
using RDCELERP.Common.Helper;
using RDCELERP.Core.App.Pages.Base;
using RDCELERP.Core.App.Pages.EVC;
using RDCELERP.DAL.Entities;
using RDCELERP.Model.Base;
using RDCELERP.Model.EVC;

namespace RDCELERP.Core.App.Pages.EVC_Portal
{
    public class EVC_PartnerListModel : BasePageModel
    {
        #region Variable declartion
        private readonly RDCELERP.DAL.Entities.Digi2l_DevContext _context;
        private readonly IEVCManager _EVCManager;
        #endregion

        # region Constructor
        public EVC_PartnerListModel(RDCELERP.DAL.Entities.Digi2l_DevContext context, IOptions<ApplicationSettings> config, IEVCManager EVCManager, CustomDataProtection protector) : base(config)
        {
            _EVCManager = EVCManager;
            _context = context;
        }
        #endregion

        #region Model Binding
        [BindProperty(SupportsGet = true)]
        public TblEvcPartner TblEvcPartners { get; set; }
        [BindProperty(SupportsGet = true)]
        public IList<EVC_ApprovedModel> EVC_ApprovedModels { get; set; }
        public TblEvcwalletAddition EvcwalletAdditions { get; set; }
        
        DateTime _currentDatetime = DateTime.Now.TrimMilliseconds();

        [BindProperty(SupportsGet = true)]
        public int? userId { get; set; }
        #endregion

        public async Task OnGetAsync(int? UserId)
        {
            if (UserId != null)
            {
                userId = UserId;
            }
            else
            {
                userId = _loginSession.UserViewModel.UserId;
            }
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
                        TblEvcPartners.ModifiedBy = _loginSession.UserViewModel.UserId;
                        TblEvcPartners.ModifiedDate = _currentDatetime;
                        _context.TblEvcPartners.Update(TblEvcPartners);
                        _context.SaveChanges();
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
                .Any(x => x.EvcregistrationId == TblEvcPartners.EvcregistrationId &&x.EvcpartnerId== TblEvcPartners.EvcPartnerId && x.IsActive == true && x.StatusId != "32");
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
