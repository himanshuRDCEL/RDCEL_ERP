using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;
using RDCELERP.BAL.Interface;
using RDCELERP.Common.Helper;
using RDCELERP.Core.App.Pages.Base;
using RDCELERP.DAL.IRepository;
using RDCELERP.Model.Base;
using RDCELERP.Model.CashfreeModel;

namespace RDCELERP.Core.App.Pages.ABB
{

   public class CertificateNotFoundDetailsModel : BasePageModel
    {
        #region variable declaration
        //private readonly RDCELERP.DAL.Entities.Digi2l_DevContext _context;
        public readonly IOptions<ApplicationSettings> _config;
        private CustomDataProtection _protector;
        private readonly IExchangeOrderManager _exchangeOrderManager;
        private readonly IDealerManager _dashBoardManager;
        IExchangeOrderRepository _exchangeOrderRepository;
        IBusinessPartnerRepository _businessPartnerRepository;
        ICashfreePayoutCall _cashfreePayoutCall;
        public ILogging _logging;

        #region Constructor
        public CertificateNotFoundDetailsModel(IOptions<ApplicationSettings> config, CustomDataProtection _dataprotector, IExchangeOrderManager exchangeOrderManager, IDealerManager dealerDashBoardManager, ILogging logging, IBusinessPartnerRepository businessPartnerRepository, IExchangeOrderRepository exchangeOrderRepository, ICashfreePayoutCall cashfreecall) : base(config)
        {
            _config = config;
            _protector = _dataprotector;
            _exchangeOrderManager = exchangeOrderManager;
            _dashBoardManager = dealerDashBoardManager;
            _logging = logging;
            _businessPartnerRepository = businessPartnerRepository;
            _exchangeOrderRepository = exchangeOrderRepository;
            _cashfreePayoutCall = cashfreecall;
        }
        #endregion

        #endregion
        [BindProperty(SupportsGet = true)]
        public PayOutDetails messageObj { get; set; }
        public IActionResult OnGet(string message, string ReturnURL)
        {
            try
            {
                if (message != null)
                {
                    messageObj.Message = message;
                }
                else
                {
                    messageObj.Message = "Something went wrong";
                }
                var baseUrl = _config.Value.BaseURL;
                messageObj.PageRedirectionURL = baseUrl + ReturnURL;
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("DetailsModel", "OnGet", ex);
            }
            return Page();
        }
    }
}
