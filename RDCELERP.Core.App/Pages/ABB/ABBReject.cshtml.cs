using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;
using RDCELERP.BAL.Interface;
using RDCELERP.Core.App.Pages.Base;
using RDCELERP.DAL.Entities;
using RDCELERP.Model.AbbRegistration;
using RDCELERP.Model.Base;
using RDCELERP.Model.QCComment;
using static RDCELERP.Model.QCComment.ExchangeOrderStatusViewModel;

namespace RDCELERP.Core.App.Pages.ABB
{
    public class ABBRejectModel : BasePageModel
    {
        private readonly IAbbRegistrationManager _AbbRegistrationManager;
        private readonly IUserManager _UserManager;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly RDCELERP.DAL.Entities.Digi2l_DevContext _context;

        public ABBRejectModel(RDCELERP.DAL.Entities.Digi2l_DevContext context, IAbbRegistrationManager AbbRegistrationManager, IWebHostEnvironment webHostEnvironment, IOptions<ApplicationSettings> config, IUserManager userManager)
           : base(config)
        {
            _webHostEnvironment = webHostEnvironment;
            _AbbRegistrationManager = AbbRegistrationManager;
            _context = context;
        }
        public AbbRegistrationModel AbbRegistrationModel { get; set; }
        public List<TblAbbregistration> TblAbbregistration { get; set; }
        public TblAbbregistration TblAbbregistrations { get; set; }
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
                TblAbbregistrations.AbbReject = false;
                int ABBstatus = (int)ExchangeOrderStatusEnum.ABBRejectUndo;
                result = _AbbRegistrationManager.SaveExchangeABBHistory(TblAbbregistrations.RegdNo, Convert.ToInt32(_loginSession.UserViewModel.UserId), ABBstatus);
                _context.TblAbbregistrations.Update(TblAbbregistrations);
                await _context.SaveChangesAsync();
            }
            return RedirectToPage("ABBReject", new { ABBregistrationId = TblAbbregistrations.AbbregistrationId });
        }
    }
}
