using RDCELERP.BAL.Interface;
using RDCELERP.Core.App.Helper;
using RDCELERP.Core.App.Pages.Base;
using RDCELERP.DAL.Entities;
using RDCELERP.Model.Base;
using RDCELERP.Model.Users;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RDCELERP.Model.Dashboards;

namespace RDCELERP.Core.App.Pages.QCPortal
{
    public class QCDashboard : BasePageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly IQCCommentManager _qCCommentManager;
        private readonly RDCELERP.DAL.Entities.Digi2l_DevContext _context;
        public QCDashboard(ILogger<IndexModel> logger, IUserManager userManager, RDCELERP.DAL.Entities.Digi2l_DevContext context, IOptions<ApplicationSettings> config, IQCCommentManager qCCommentManager)
        : base(config)

        {
            _logger = logger;
            _context = context;
            _qCCommentManager = qCCommentManager;
        }

        [BindProperty(SupportsGet = true)]
        public QCDashboardViewModel QCDashboardVM { get; set; }
        [BindProperty(SupportsGet = true)]
        public UserViewModel UserViewModel { get; set; }
       
        [BindProperty(SupportsGet = true)]
        public IList<TblUserRole> TblUserRole { get; set; }
        [BindProperty(SupportsGet = true)]
        public List<UserViewModel> UserVMList { get; set; }
        [BindProperty(SupportsGet = true)]
        public UserViewModel UserVM { get; set; }
        public int RoleId { get; set; }
        public string javascriptVoidZero { get; set; }
        public IActionResult OnGet()
        {
            javascriptVoidZero = "javascript:void(0)";
            var baseUrl = _baseConfig.Value.BaseURL;
            if (_loginSession == null)
            {
                return RedirectToPage("Index");
            }
            else
            {
                if (_loginSession.RoleViewModel.CompanyId > 0)
                {
                    QCDashboardVM = _qCCommentManager.GetQCFlagBasedCount(Convert.ToInt32(_loginSession.RoleViewModel.CompanyId));
                    if (QCDashboardVM != null)
                    {
                        QCDashboardVM.OrderForQCUrl = baseUrl + "QCIndex/OrdersForQC";
                        QCDashboardVM.SelfQCOrdersUrl = baseUrl + "QCIndex/SelfQCOrders";
                        QCDashboardVM.AllResheduledQCUrl = baseUrl + "QCIndex/RescheduledQC?TabId=1";
                        QCDashboardVM.ResheduledQCAgingUrl = baseUrl + "QCIndex/RescheduledQC?TabId=2";
                        QCDashboardVM.ResheduledQC3Url = baseUrl + "QCIndex/RescheduledQC?TabId=3";
                        QCDashboardVM.ResheduledQC2Url = baseUrl + "QCIndex/RescheduledQC?TabId=4";
                        QCDashboardVM.ResheduledQC1Url = baseUrl + "QCIndex/RescheduledQC?TabId=5";
                        QCDashboardVM.PriceQuotedQCUrl = baseUrl + "QCIndex/PriceQuotedQC";
                        QCDashboardVM.CancelledOrdersUrl = baseUrl + "QCIndex/CancelOrderList";
                        QCDashboardVM.ReopenedQCUrl = baseUrl + "QCIndex/ReopenedQC";
                    }
                    else
                    {
                        QCDashboardVM = new QCDashboardViewModel();
                    }
                }
                return Page();
            }
        }
    }
}
