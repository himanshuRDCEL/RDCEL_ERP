using DocumentFormat.OpenXml.Drawing.Charts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Options;
using RDCELERP.BAL.Interface;
using RDCELERP.Common.Helper;
using RDCELERP.Core.App.Pages.Base;
using RDCELERP.DAL.Entities;
using RDCELERP.DAL.IRepository;
using RDCELERP.DAL.Repository;
using RDCELERP.Model.ABBRedemption;
using RDCELERP.Model.Base;
using RDCELERP.Model.EVC;

namespace RDCELERP.Core.App.Pages.ABBRedemption
{
    public class InstantVoucharModel : PageModel
    {
        private readonly IABBRedemptionManager _AbbRedemptionManager;
        private readonly ITermsAndConditionsForVoucherRepository _termsAndConditionsForVoucherRepository;
        public readonly IOptions<ApplicationSettings> _baseConfig;

        ILogging _logging;

        public InstantVoucharModel(IABBRedemptionManager AbbRedemptionManager,ITermsAndConditionsForVoucherRepository termsAndConditionsForVoucherRepository,IOptions<ApplicationSettings> baseConfig, ILogging logging) 

        {
            _AbbRedemptionManager = AbbRedemptionManager;
            _termsAndConditionsForVoucherRepository= termsAndConditionsForVoucherRepository;
            _logging = logging;
            _baseConfig = baseConfig;
        }
        [BindProperty(SupportsGet = true)]
        public VoucherDataContract voucherDC { get; set; }
        [BindProperty(SupportsGet = true)]
        public RedemptionDataContract abbredemptionDc { get; set; }
        [BindProperty(SupportsGet = true)]
        public List<TblVoucherTermsAndCondition> termsandconditionList { get; set; }
        public IActionResult OnGet(int id)
        {           
            string response = string.Empty;           
            try
            {
                abbredemptionDc = _AbbRedemptionManager.GetOrderData(id);
                if (abbredemptionDc.BusinessUnitId > 0 && abbredemptionDc.BULogoName != null)
                {
                    abbredemptionDc.BULogoName = _baseConfig.Value.BaseURL + "wwwroot/DBFiles/Sponsor/" + abbredemptionDc.BULogoName;
                }
                abbredemptionDc.VoucherCodeExpDateString = abbredemptionDc.VoucherCodeExpDate != null ? Convert.ToDateTime(abbredemptionDc.VoucherCodeExpDate).ToString("dd/MM/yyyy") : string.Empty;
                termsandconditionList = _termsAndConditionsForVoucherRepository.GetList(x => x.BusinessUnitId == abbredemptionDc.BusinessUnitId && x.IsDeffered == false).ToList();
                abbredemptionDc.TermsandCondition = new List<SelectListItem>();
                if (termsandconditionList.Count > 0)
                {
                    abbredemptionDc.TermsandCondition = termsandconditionList.Select(x => new SelectListItem
                    {
                        Text = x.TermsandCondition,
                        Value = x.Id.ToString()
                    }).ToList();
                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("VoucherController", "GetVoucher", ex);
            }
            return Page();
        }
    }
}
