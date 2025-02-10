using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Options;
using RDCELERP.BAL.Interface;
using RDCELERP.Core.App.Pages.Base;
using RDCELERP.Model;
using RDCELERP.Model.Base;
using RDCELERP.Model.ExchangeOrder;
using RDCELERP.DAL.Entities;
using RDCELERP.BAL.MasterManager;
using static RDCELERP.Model.QCCommentViewModel;
using System.IO;
using Microsoft.AspNetCore.Http;
using RDCELERP.DAL.IRepository;

namespace RDCELERP.Core.App.Pages.QCIndex
{
    public class ProductRatingModel : BasePageModel
    {
        #region Variable Declaration
        private readonly IQCCommentManager _QcCommentManager;
        private readonly IUserManager _UserManager;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IStateManager _stateManager;
        private readonly ICityManager _cityManager;
        private readonly IBrandManager _brandManager;
        private readonly IBusinessPartnerManager _businessPartnerManager;
        private readonly RDCELERP.DAL.Entities.Digi2l_DevContext _context;
        IABBRedemptionManager _aBBRedemptionManager;
        IOrderQCRepository _orderQCRepository;
        IExchangeOrderRepository _ExchangeOrderRepository;
        #endregion

        #region Constructor
        public ProductRatingModel(IExchangeOrderRepository ExchangeOrderRepository,IOrderQCRepository orderQCRepository, RDCELERP.DAL.Entities.Digi2l_DevContext context, IABBRedemptionManager aBBRedemptionManager, IBusinessPartnerManager businessPartnerManager, IBrandManager brandManager, IStateManager StateManager, ICityManager CityManager, IWebHostEnvironment webHostEnvironment, IOptions<ApplicationSettings> config, IUserManager userManager, IQCCommentManager QcCommentManager)
           : base(config)
        {
            _ExchangeOrderRepository = ExchangeOrderRepository;
            _orderQCRepository = orderQCRepository;
            _webHostEnvironment = webHostEnvironment;
            _stateManager = StateManager;
            _cityManager = CityManager;
            _brandManager = brandManager;
            _businessPartnerManager = businessPartnerManager;
            _context = context;
            _UserManager = userManager;
            _QcCommentManager = QcCommentManager;
            _aBBRedemptionManager = aBBRedemptionManager;
        }
        #endregion

        [BindProperty(SupportsGet = true)]      
        public QCCommentViewModel QCCommentViewModel { get; set; }
        public TblExchangeOrder tblExchangeorder { get; set; }
        public TblOrderQc TblOrderQc { get; set; }
        public ExchangeOrderViewModel exchangeOrderViewModel { get; set; }
        public int login_id { get; set; }
        public IActionResult OnGet(int id)
        {
            return Page();
        }

        public IActionResult OnPostAsync()
        {
            int result = 0;           
            if (ModelState.IsValid)
            {
               
            }           
            if (result > 0)
                return RedirectToPage("/QCIndex/OrdersForQC");
            else
                return RedirectToPage("/QCIndex/OrdersForQC");
        }
      
    }
}
