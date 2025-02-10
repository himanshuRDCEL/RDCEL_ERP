using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;
using RDCELERP.BAL.Interface;
using RDCELERP.Common.Helper;
using RDCELERP.Core.App.Pages.Base;
using RDCELERP.DAL.Entities;
using RDCELERP.DAL.IRepository;
using RDCELERP.Model.AbbRegistration;
using RDCELERP.Model.Base;
using RDCELERP.Model.QCComment;
using static RDCELERP.Model.QCComment.ExchangeOrderStatusViewModel;

namespace RDCELERP.Core.App.Pages.AbbRegistration
{
    public class NotApprovedModel : BasePageModel
    {
        private readonly RDCELERP.DAL.Entities.Digi2l_DevContext _context;
        private readonly IAbbRegistrationManager _AbbRegistrationManager;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IUserManager _UserManager;
        private readonly IMailManager _mailManager;
        IAbbRegistrationRepository _abbRegistrationRepository;
        ILogging _logging;

        public NotApprovedModel(IAbbRegistrationRepository abbRegistrationRepository, IMailManager mailManager, IUserManager userManager, IWebHostEnvironment webHostEnvironment, IAbbRegistrationManager AbbRegistrationManager, RDCELERP.DAL.Entities.Digi2l_DevContext context, IOptions<ApplicationSettings> config, ILogging logging)
        : base(config)
        {
            _webHostEnvironment = webHostEnvironment;
            _AbbRegistrationManager = AbbRegistrationManager;
            _context = context;
            _UserManager = userManager;
            _mailManager = mailManager;
            _abbRegistrationRepository = abbRegistrationRepository;
            _logging = logging;
        }

        public AbbRegistrationModel AbbRegistrationModel { get; set; }
        public List<TblAbbregistration> TblAbbregistration { get; set; }
        public TblAbbregistration TblAbbregistrations { get; set; }
        public List<AbbRegistrationModel> abbRegistrationModels { get; set; }

        #region ABB Approve
        public IActionResult OnGetABBApprove(int ABBregistrationId)
        {
            bool? flag = false;
            int? userId = _loginSession?.UserViewModel?.UserId;
            try
            {
                if (ABBregistrationId > 0)
                {
                    flag = _AbbRegistrationManager.ABBApproveById(ABBregistrationId, userId);
                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("NotApproved", "OnGetABBApprove", ex);
            }
            return RedirectToPage("NotApproved", new { ABBregistrationId = ABBregistrationId });
        }
        #endregion

        #region ABB Reject
        public async Task<IActionResult> OnGetAbbRejectAsync(int ABBregistrationId)
        {
            if (ABBregistrationId == null)
            {
                return NotFound();
            }
            TblAbbregistrations = await _context.TblAbbregistrations.FindAsync(ABBregistrationId);
            if (TblAbbregistrations != null)
            {
                int result = 0;
                TblAbbregistrations.AbbReject = true;
                int ABBstatus = (int)ExchangeOrderStatusEnum.ABBReject;
                result = _AbbRegistrationManager.SaveExchangeABBHistory(TblAbbregistrations.RegdNo, Convert.ToInt32(_loginSession.UserViewModel.UserId), ABBstatus);
                _context.TblAbbregistrations.Update(TblAbbregistrations);
                await _context.SaveChangesAsync();
            }
            return RedirectToPage("NotApproved", new { ABBregistrationId = TblAbbregistrations.AbbregistrationId });
        }
        #endregion

        public IActionResult OnPostDeleteAsync()
        {
            if (_loginSession == null)
            {
                return RedirectToPage("/Index");
            }
            else
            {
                if (TblAbbregistrations != null && TblAbbregistrations.AbbregistrationId > 0)
                {
                    TblAbbregistrations = _context.TblAbbregistrations.Find(TblAbbregistrations.AbbregistrationId);
                }
                if (TblAbbregistrations != null)
                {
                    TblAbbregistrations.IsActive = false;
                    TblAbbregistrations.ModifiedBy = _loginSession.UserViewModel.UserId;
                    _context.TblAbbregistrations.Update(TblAbbregistrations);
                    _context.SaveChanges();
                }
                return RedirectToPage("./NotApproved");
            }
        }

    }
}
