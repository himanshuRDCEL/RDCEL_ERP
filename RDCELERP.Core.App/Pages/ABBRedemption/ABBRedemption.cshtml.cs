using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper.Configuration;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;
using RDCELERP.BAL.Interface;
using RDCELERP.Core.App.Pages.Base;
using RDCELERP.Model.Base;
using RDCELERP.Model.ABBRedemption;
using Microsoft.AspNetCore.Mvc.Rendering;
using RDCELERP.DAL.Entities;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using System.IO;
using RDCELERP.Common.Enums;
using RDCELERP.DAL.IRepository;
using RDCELERP.Model.AbbRegistration;
using AutoMapper;
using RDCELERP.Model.ABBPlanMaster;
using RDCELERP.Common.Helper;

namespace RDCELERP.Core.App.Pages.ABBRedemption
{
    public class ABBRedemptionModel : BasePageModel
    {
        #region Variable discussion
        private readonly IABBRedemptionManager _AbbRedemptionManager;
        private readonly IUserManager _UserManager;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IStateManager _stateManager;
        private readonly ICityManager _cityManager;
        private readonly IBrandManager _brandManager;
        private readonly IBusinessPartnerManager _businessPartnerManager;
        private readonly IProductCategoryManager _productCategoryManager;
        private readonly IProductTypeManager _productTypeManager;
        private readonly IStoreCodeManager _storeCodeManager;
        private readonly RDCELERP.DAL.Entities.Digi2l_DevContext _context;
        private readonly IAbbRegistrationManager _AbbRegistrationManager;
        private readonly IPinCodeManager _pinCodeManager;
        private readonly IMapper _mapper;
        private readonly IBusinessUnitRepository _businessUnitRepository;
        DateTime _currentDatetime = DateTime.Now.TrimMilliseconds();
        IAbbRegistrationRepository _abbRegistrationRepository;
        #endregion

        #region Constructor
        public ABBRedemptionModel(IPinCodeManager pinCodeManager, IStoreCodeManager storeCodeManager, IAbbRegistrationManager AbbRegistrationManager, RDCELERP.DAL.Entities.Digi2l_DevContext context, IProductTypeManager productTypeManager, IABBRedemptionManager abbRedemptionRegManager, IProductCategoryManager productCategoryManager, IBusinessPartnerManager businessPartnerManager, IBrandManager brandManager, IStateManager StateManager, ICityManager CityManager, IWebHostEnvironment webHostEnvironment, IOptions<ApplicationSettings> config, IUserManager userManager, IAbbRegistrationRepository abbRegistrationRepository, IMapper mapper, IBusinessUnitRepository businessUnitRepository)
            : base(config)
        {
            _webHostEnvironment = webHostEnvironment;
            _AbbRedemptionManager = abbRedemptionRegManager;
            _stateManager = StateManager;
            _cityManager = CityManager;
            _brandManager = brandManager;
            _businessPartnerManager = businessPartnerManager;
            _productCategoryManager = productCategoryManager;
            _productTypeManager = productTypeManager;
            _context = context;
            _storeCodeManager = storeCodeManager;
            _UserManager = userManager;
            _AbbRegistrationManager = AbbRegistrationManager;
            _pinCodeManager = pinCodeManager;
            _abbRegistrationRepository = abbRegistrationRepository;
            _mapper = mapper;
            _businessUnitRepository = businessUnitRepository;
        }
        #endregion

        #region Bind Properties
        [BindProperty(SupportsGet = true)]
        public ABBRedemptionViewModel AbbRedemptionModel { get; set; }

        public AbbRegistrationModel AbbRegistrationModel { get; set; }

        public ABBPlanMasterViewModel aBBPlanMasterViewModel { get; set; }
        #endregion


        public IActionResult OnGet(int Id)
        {
            if (Id > 0)
            {
                AbbRegistrationModel = _AbbRegistrationManager.GetABBRegistrationId(Convert.ToInt32(Id));

                if (AbbRegistrationModel != null && AbbRegistrationModel.AbbregistrationId > 0)
                {
                    AbbRedemptionModel.AbbRegistrationModel = AbbRegistrationModel;
                    AbbRedemptionModel = _mapper.Map<AbbRegistrationModel, ABBRedemptionViewModel>(AbbRegistrationModel);
                    DateTime dt = Convert.ToDateTime(AbbRegistrationModel.InvoiceDate);
                    AbbRedemptionModel.Registrationdate = dt.ToString("dd/MM/yyyy");
                    if (AbbRegistrationModel.CustomerId > 0)
                    {
                        AbbRedemptionModel.CustomerDetailsId = Convert.ToInt32(AbbRegistrationModel.CustomerId);
                    }
                }
                #region Redemption value calculatation & periods in months
                int? proCatId = Convert.ToInt32(AbbRegistrationModel.NewProductCategoryId);
                int? proTypeId = Convert.ToInt32(AbbRegistrationModel.NewProductCategoryTypeId);
                int? buId = Convert.ToInt32(AbbRegistrationModel.BusinessUnitId);

                int monthsdiff = CalculateMonthsDifference(Convert.ToDateTime(AbbRegistrationModel.InvoiceDate), DateTime.Now);

                AbbRedemptionModel.RedemptionPeriod = monthsdiff;

                if (proCatId > 0 && proTypeId > 0 && buId > 0)
                {
                    aBBPlanMasterViewModel = _AbbRedemptionManager.GetABBPlanMasterDetails(buId, proCatId, proTypeId, monthsdiff);

                    if (aBBPlanMasterViewModel != null && aBBPlanMasterViewModel.AssuredBuyBackPercentage != null && aBBPlanMasterViewModel.AssuredBuyBackPercentage > 0 && AbbRegistrationModel.ProductNetPrice > 0 && Convert.ToInt32(aBBPlanMasterViewModel.NoClaimPeriod) <= AbbRedemptionModel.RedemptionPeriod)
                    {
                        AbbRedemptionModel.RedemptionPercentage = Convert.ToInt32(aBBPlanMasterViewModel.AssuredBuyBackPercentage);

                        decimal? RedValue = (AbbRegistrationModel.ProductNetPrice / 100) * aBBPlanMasterViewModel.AssuredBuyBackPercentage;
                        AbbRedemptionModel.RedemptionValue = Convert.ToDecimal(RedValue);
                    }
                    if (aBBPlanMasterViewModel != null && !string.IsNullOrEmpty(aBBPlanMasterViewModel.NoClaimPeriod))
                    {
                        AbbRedemptionModel.NoClaimPeriod = Convert.ToInt32(aBBPlanMasterViewModel.NoClaimPeriod);
                    }

                }
                //for validation purpose
                AbbRedemptionModel.RedemptionPeriod = 0;
                AbbRedemptionModel.RedemptionValue = 0;
                AbbRedemptionModel.NoClaimPeriod = 0;
                #endregion

                #region check BU Based CustomerDetails Edit
                TblBusinessUnit tblBusinessUnit = _businessUnitRepository.GetSingle(x => x.IsActive == true && x.BusinessUnitId == AbbRegistrationModel.BusinessUnitId);
                if (tblBusinessUnit != null && tblBusinessUnit.Name == EnumHelper.DescriptionAttr(BussinessUnitEnum.Tech_Guard).ToString())
                {
                    AbbRedemptionModel.IsCustomerDetailsEdittable = true;
                }
                else
                {
                    AbbRedemptionModel.IsCustomerDetailsEdittable = false;
                }
                #endregion

                #region Redemption Date Configuration

                if (_baseConfig.Value.AllowFutureDateSelectionForRedemption)
                {
                    AbbRedemptionModel.isFutureDateAllow = _baseConfig.Value.AllowFutureDateSelectionForRedemption;
                }
                if (_baseConfig.Value.AllowPastDateSelectionForRedemption)
                {
                    AbbRedemptionModel.isPastDateAllow = _baseConfig.Value.AllowPastDateSelectionForRedemption;
                }
                AbbRedemptionModel.currentDate = _currentDatetime;
                #endregion

                #region invoice image url setup
                ViewData["InvoiceImageURL"] = _baseConfig.Value.InvoiceImageURL;
                string MVCBaseurl = _baseConfig.Value.MVCBaseURLForABBInvoice;
                string ERPABBInvoiceUrl = _baseConfig.Value.BaseURL + EnumHelper.DescriptionAttr(FileAddressEnum.ABBInvoice);

                if (AbbRegistrationModel.UploadImage != null)
                {
                    AbbRedemptionModel.InvoiceImage = ERPABBInvoiceUrl + AbbRegistrationModel.UploadImage;
                }
                else
                {
                    AbbRedemptionModel.InvoiceImage = MVCBaseurl + AbbRegistrationModel.InvoiceImage;
                }
                #endregion

            }


            if (_loginSession == null)
            {
                return RedirectToPage("/index");
            }
            else
            {
                bool auth = true;
                if (TempData["Auth"] != null)
                    auth = (Boolean)TempData["Auth"];
                if (!auth)
                {
                    ShowMessage("Invalid inFormation, please fillup again", MessageTypeEnum.error);
                }
                return Page();
            }
        }
        public IActionResult OnPostAsync()
        {
            int result = 0;
            if (ModelState.IsValid)
            {
                if (_loginSession != null)
                {
                    AbbRedemptionModel.CreatedBy = _loginSession.UserViewModel.UserId;
                }

                result = _AbbRedemptionManager.SaveABBRedemptionDetails(AbbRedemptionModel);
            }
            if (result > 0)
            {
                TempData["Auth"] = true;
                return RedirectToPage("/ABB/ABBApproved");
            }
            else
            {
                TempData["Auth"] = false;
                return RedirectToPage("/ABB/ABBApproved");
            }
        }

        //public List<ABBRedemptionModel> aBBRedemptionModels { get; set; }

        public JsonResult OnGetABBRedempDetailsByRegdNoAsync(string RegdNo)
        {
            return new JsonResult(_AbbRedemptionManager.GetABBDetailsByRegdNo(RegdNo));
        }

        public IActionResult OnGetAutoCompleteAsync(string prefix)
        {
            /* IEnumerable<SelectListItem> regdNumList = null;*/
            IEnumerable<SelectListItem> regdNumList = null;
            List<TblAbbregistration> tblAbbregistrations = null;

            tblAbbregistrations = _AbbRedemptionManager.GetAutoCompleteRegdNo(prefix);
            regdNumList = (tblAbbregistrations).Select(prodt => new SelectListItem() { Text = prodt.RegdNo, Value = prodt.RegdNo });
            regdNumList = regdNumList.OrderBy(o => o.Text).ToList();
            var result = new SelectList(regdNumList, "Value", "Text");
            return new JsonResult(regdNumList);

            //List<TblAbbredemption> tblAbbredemptions = null;
            //var rno = (from aBBRedemptionModels in _context.TblAbbregistrations
            //                 where aBBRedemptionModels.RegdNo.StartsWith(prefix)
            //                 select new
            //                 {
            //                     label = aBBRedemptionModels.RegdNo,
            //                     val = aBBRedemptionModels.RegdNo
            //                 }).ToList();

            //return new JsonResult(rno);
        }

        public JsonResult OnGetCustDetailsByRegdNoAsync()
        {
            return new JsonResult(_AbbRedemptionManager.GetABBDetailsByRegdNo(AbbRedemptionModel.RegdNo));
        }

        public int CalculateMonthsDifference(DateTime startDate, DateTime endDate)
        {
            // Calculate the difference in years
            int yearsDiff = endDate.Year - startDate.Year;

            // Calculate the difference in months accounted for by full years
            int monthsDiff = yearsDiff * 12;

            // Calculate the remaining months within the same year
            monthsDiff += endDate.Month - startDate.Month;

            // Adjust the months difference if endDate day is before startDate day
            if (endDate.Day < startDate.Day)
            {
                monthsDiff--;
            }

            return monthsDiff;
        }

        #region Get Redemption value calculatation & periods in months
        public JsonResult OnGetRedemptionValueAsync(string redemptiondate, decimal? productNetPrice, int proCatId, DateTime? startDate, int proTypeId, int BUID)
        {
            AbbRedemptionValue abbRedemptionValue = new AbbRedemptionValue();
            if (proCatId > 0 && proTypeId > 0 && BUID > 0)
            {
                //here startDate is InvoiceDate of the order
                int monthsdiff = CalculateMonthsDifference(Convert.ToDateTime(startDate), DateTime.Parse(redemptiondate));

                aBBPlanMasterViewModel = _AbbRedemptionManager.GetABBPlanMasterDetails(BUID, proCatId, proTypeId, monthsdiff);
                abbRedemptionValue.RedemptionPeriod = monthsdiff;

                if (aBBPlanMasterViewModel != null && aBBPlanMasterViewModel.AssuredBuyBackPercentage != null && aBBPlanMasterViewModel.AssuredBuyBackPercentage > 0 && productNetPrice > 0)
                {
                    abbRedemptionValue.RedemptionPercentage = Convert.ToInt32(aBBPlanMasterViewModel.AssuredBuyBackPercentage);

                    decimal? RedValue = (productNetPrice / 100) * aBBPlanMasterViewModel.AssuredBuyBackPercentage;
                    abbRedemptionValue.RedemptionValue = Convert.ToDecimal(RedValue);
                }
                if (aBBPlanMasterViewModel != null && !string.IsNullOrEmpty(aBBPlanMasterViewModel.NoClaimPeriod))
                {
                    abbRedemptionValue.NoClaimPeriod = Convert.ToInt32(aBBPlanMasterViewModel.NoClaimPeriod);
                }
            }
            return new JsonResult(abbRedemptionValue);
        }

        #endregion 
    }
}

