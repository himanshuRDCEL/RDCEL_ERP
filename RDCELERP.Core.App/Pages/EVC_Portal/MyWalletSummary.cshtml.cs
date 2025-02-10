using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using RDCELERP.BAL.Interface;
using RDCELERP.Common.Helper;
using RDCELERP.Core.App.Pages.Base;
using RDCELERP.DAL.Entities;
using RDCELERP.Model.Base;
using RDCELERP.Model.EVC;
using RDCELERP.Model.EVC_Portal;

namespace RDCELERP.Core.App.Pages.EVC_Portal
{
    public class MyWalletSummaryModel : BasePageModel
    {
        private readonly RDCELERP.DAL.Entities.Digi2l_DevContext _context;
        public readonly IOptions<ApplicationSettings> _config;
        private CustomDataProtection _protector;
        private readonly IEVCManager _eVCManager;


        public MyWalletSummaryModel(RDCELERP.DAL.Entities.Digi2l_DevContext context, IOptions<ApplicationSettings> config, CustomDataProtection protector, IEVCManager eVCManager)
        : base(config)
        {
            _context = context;
            _config = config;
            _protector = protector;
            _eVCManager = eVCManager;
        }

        [BindProperty(SupportsGet = true)]
        public EVC_DashboardViewModel eVC_DashBoardViewModel { get; set; }
        [BindProperty(SupportsGet = true)]
        public List<MyWalletSummaryAdditionViewModel> myWalletSummaryAddition { get; set; }
        [BindProperty(SupportsGet = true)]
        public List<lattestAllocationViewModel> lattestAllocationViewModels { get; set; }
        [BindProperty(SupportsGet = true)]
        public int? userId { get; set; }
        public IActionResult OnGetAsync(int? UserId)
        {
           
            if (_loginSession == null)
            {
                return RedirectToPage("/index");
            }
            else
            {
                if (UserId != null)
                {
                    userId = UserId;

                }
                else
                {
                    userId = _loginSession.UserViewModel.UserId;
                }

                //int userId = _loginSession.UserViewModel.UserId;
                

                
                eVC_DashBoardViewModel = _eVCManager.EvcByUserId((int)userId);
                myWalletSummaryAddition = _eVCManager.EvcUserWalletAdditionHistory((int)userId);
                lattestAllocationViewModels = _eVCManager.GetListOFLattestAllocation((int)userId,false);
                return Page();
            }
        }
    }
}
