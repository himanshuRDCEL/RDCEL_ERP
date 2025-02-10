using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;
using RDCELERP.BAL.Interface;
using RDCELERP.Common.Helper;
using RDCELERP.Core.App.Pages.Base;
using RDCELERP.DAL.Entities;
using RDCELERP.DAL.IRepository;
using RDCELERP.Model.Base;
using RDCELERP.Model.CashfreeModel;

namespace RDCELERP.Core.App.Pages.PayOut
{
    public class AddBeneficiaryModel : BasePageModel
    {
        private readonly Digi2l_DevContext _context;
        private readonly IOptions<ApplicationSettings> _config;
        private readonly ICashfreePayoutCall _cashfreePayoutCall;

        public AddBeneficiaryModel(IOptions<ApplicationSettings> config, Digi2l_DevContext context) : base(config)
        {
            _context = context;
            _config = config;
        }


        public IActionResult OnPost(string RegdNumber)
        {
            if (!string.IsNullOrEmpty(RegdNumber))
            {
                return Redirect(_config.Value.BaseURL + "PayOut/GetBeneficiaryDetails?RegdNumber=" + RegdNumber);
            }
            return Redirect(_config.Value.BaseURL + "PayOut/RePaymentRecords");
        }
    }
}
