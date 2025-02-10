using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;
using RDCELERP.BAL.Interface;
using RDCELERP.Core.App.Pages.Base;
using RDCELERP.Model.Base;

namespace RDCELERP.Core.App.Pages.EVC_Dispute
{
    public class ShowDisputeListModel : BasePageModel
    {
        private readonly RDCELERP.DAL.Entities.Digi2l_DevContext _context;
        private readonly IEVCManager _EVCManager;
        public ShowDisputeListModel(RDCELERP.DAL.Entities.Digi2l_DevContext context, IOptions<ApplicationSettings> config, IEVCManager EVCManager, CustomDataProtection protector) : base(config)

        {
            _EVCManager = EVCManager;
            _context = context;
        }
        [BindProperty(SupportsGet = true)]
        public int loginUserid { get; set; }
        public void OnGet()
        {
            loginUserid = _loginSession.UserViewModel.UserId;
        }
    }
}
