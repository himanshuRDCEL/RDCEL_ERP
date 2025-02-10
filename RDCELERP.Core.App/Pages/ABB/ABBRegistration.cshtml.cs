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
using RDCELERP.Model.Base;
using RDCELERP.Model.AbbRegistration;
using RDCELERP.DAL.IRepository;
using Microsoft.AspNetCore.Mvc.Rendering;
using RDCELERP.Model.City;
using System.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Data;
using ExcelDataReader;
using RDCELERP.Model.PinCode;
using RDCELERP.BAL.Helper;
using RDCELERP.Common.Helper;
using RDCELERP.Common.Enums;

namespace RDCELERP.Core.App.Pages.ABB
{
    public class ABBRegistrationModel : BasePageModel
    {
        #region Variable Declaration
        private readonly IAbbRegistrationManager _AbbRegistrationManager;
        private readonly IUserManager _UserManager;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IStateManager _stateManager;
        private readonly ICityManager _cityManager;
        private readonly IBrandManager _brandManager;
        private readonly IBusinessPartnerManager _businessPartnerManager;
        private readonly IProductCategoryManager _productCategoryManager;
        private readonly IProductTypeManager _productTypeManager;
        private readonly IStoreCodeManager _storeCodeManager;
        private readonly IConfiguration configuration;
        private readonly RDCELERP.DAL.Entities.Digi2l_DevContext _context;
        private readonly IPinCodeManager _pinCodeManager;
        #endregion

        #region Constructor
        public ABBRegistrationModel(IPinCodeManager pinCodeManager, IStoreCodeManager storeCodeManager, RDCELERP.DAL.Entities.Digi2l_DevContext context, IConfiguration _configuration, IProductTypeManager productTypeManager, IAbbRegistrationManager AbbRegistrationManager, IProductCategoryManager productCategoryManager, IBusinessPartnerManager businessPartnerManager, IBrandManager brandManager, IStateManager StateManager, ICityManager CityManager, IWebHostEnvironment webHostEnvironment, IOptions<ApplicationSettings> config, IUserManager userManager)
            : base(config)
        {
            _webHostEnvironment = webHostEnvironment;
            _AbbRegistrationManager = AbbRegistrationManager;
            _stateManager = StateManager;
            _cityManager = CityManager;
            _brandManager = brandManager;
            _businessPartnerManager = businessPartnerManager;
            _productCategoryManager = productCategoryManager;
            _productTypeManager = productTypeManager;
            configuration = _configuration;
            _context = context;
            _storeCodeManager = storeCodeManager;
            _UserManager = userManager;
            _pinCodeManager = pinCodeManager;
        }
        #endregion

        [BindProperty(SupportsGet = true)]
        public AbbRegistrationModel AbbRegistrationModel { get; set; }
        public List<TblAbbregistration> TblAbbregistration { get; set; }

        public List<CityViewModel> citylist = new List<CityViewModel>();
        [BindProperty(SupportsGet = true)]
        public bool RedemptionButtonActive { get; set; }
        public IActionResult OnGet(int id)
        {
            if (id != 0 && id > 0)
            {
                AbbRegistrationModel = _AbbRegistrationManager.GetABBRegistrationId(Convert.ToInt32(id));
            }
            #region configure redemption button logic
            if (AbbRegistrationModel.InvoiceDate != null)
            {
                RedemptionButtonActive = _baseConfig.Value.AllowFutureDateSelectionForRedemption;
                int monthCount = CalculateMonthsDifference(Convert.ToDateTime(AbbRegistrationModel.InvoiceDate), DateTime.Now);
                if (monthCount > 6)
                {
                    RedemptionButtonActive = true;
                }
                else
                {
                    if (RedemptionButtonActive == true)
                    {
                        RedemptionButtonActive = true;
                    }
                    else
                    {
                        RedemptionButtonActive = false;
                    }
                }
            }
            #endregion

            if (_loginSession == null)
            {
                return RedirectToPage("/index");
            }
            else
            {
                ViewData["InvoiceImageURL"] = _baseConfig.Value.InvoiceImageURL;
                string MVCBaseurl = _baseConfig.Value.MVCBaseURLForABBInvoice;

                string ERPABBInvoiceUrl = _baseConfig.Value.BaseURL + EnumHelper.DescriptionAttr(FileAddressEnum.ABBInvoice);

                if (AbbRegistrationModel.UploadImage != null)
                {
                    AbbRegistrationModel.InvoiceImageWithPath = ERPABBInvoiceUrl + AbbRegistrationModel.UploadImage;
                }
                else
                {
                    AbbRegistrationModel.InvoiceImageWithPath = MVCBaseurl + AbbRegistrationModel.InvoiceImage;
                }

                return Page();
            }
        }

        public IActionResult OnPostAsync()
        {
            int result = 0;
            if (AbbRegistrationModel != null)
            {
                result = _AbbRegistrationManager.SaveABBRegDetails(AbbRegistrationModel);
            }
            if (result > 0)
                return RedirectToPage("NotApproved");

            else
                return RedirectToPage("NotApproved");
        }
        public JsonResult OnGetABBDetailsByCustMobAsync()
        {
            return new JsonResult(_AbbRegistrationManager.GetCustDetailsByMob(AbbRegistrationModel.CustMobile));
        }
        public JsonResult OnGetStoreManageEmailAsync()
        {
            return new JsonResult(_AbbRegistrationManager.GetStoremanageemail(AbbRegistrationModel.BusinessPartnerId.ToString()));
        }
        public JsonResult OnGetABBEditByIdAsync()
        {
            return new JsonResult(_UserManager.GetUserById(AbbRegistrationModel.AbbregistrationId));
        }

        #region Product category type
        public JsonResult OnGetProductCategoryTypeAsync()
        {
            var productTypeList = _AbbRegistrationManager.GetProductTypeBycategory(Convert.ToInt32(AbbRegistrationModel.NewProductCategoryId));
            if (productTypeList != null)
            {
                ViewData["productTypeList"] = new SelectList(productTypeList, "Id", "Description");
            }
            return new JsonResult(productTypeList);
        }
        #endregion 

        #region GetStateByPincode
        public JsonResult OnGetStatebyPincodeAsync()
        {
            var statelist = _AbbRegistrationManager.GetStatebyPincode(Convert.ToInt32(AbbRegistrationModel.CustPinCode));
            if (statelist != null)
            {
                ViewData["statelist"] = new SelectList(statelist, "ZipCode", "State");
            }
            return new JsonResult(statelist);
        }
        #endregion

        #region OnGetPincodeByCity
        public JsonResult OnGetPincodeByCityAsync()
        {
            var cityList = _AbbRegistrationManager.GetStatebyPincode(Convert.ToInt32(AbbRegistrationModel.CustPinCode));
            if (cityList != null)
            {
                ViewData["pincodeList"] = new SelectList(cityList, "ZipCode", "Location");
            }
            return new JsonResult(cityList);
        }
        #endregion

        #region Autocomplete for RegdNo
        public IActionResult OnGetRegdNoAutoCompleteAsync(string prefix)
        {
            IEnumerable<SelectListItem> regdNumList = null;
            List<TblAbbregistration> tblAbbregistrations = null;
            tblAbbregistrations = _AbbRegistrationManager.GetRnoAutoComplete(prefix);
            regdNumList = (tblAbbregistrations).AsEnumerable().Select(prodt => new SelectListItem() { Text = prodt.RegdNo, Value = prodt.RegdNo });
            regdNumList = regdNumList.OrderBy(o => o.Text).ToList();
            var result = new SelectList(regdNumList, "Value", "Text");
            return new JsonResult(result);
        }
        #endregion

        #region Autocomplete for Model Number
        public IActionResult OnGetModelNoAutoCompleteAsync(string prefix)
        {
            IEnumerable<SelectListItem> ModelNumList = null;

            List<TblModelNumber> tblModelNumbers = null;
            tblModelNumbers = _AbbRegistrationManager.GetModelNoAutoComplete(prefix);
            ModelNumList = (tblModelNumbers).AsEnumerable().Select(item => new SelectListItem() { Text = item.ModelName, Value = item.ModelName });
            ModelNumList = ModelNumList.OrderBy(o => o.Text).ToList();
            var result = new SelectList(ModelNumList, "Value", "Text");
            return new JsonResult(result);
        }
        #endregion

        #region Cascading Dropdown for Model Number
        public IActionResult OnGetModelNoDropdownListAsync()
        {
            List<SelectListItem> ModelNumList = null;

            List<TblModelNumber> tblModelNumbers = null;
            if (AbbRegistrationModel != null && AbbRegistrationModel.NewProductCategoryId != null && AbbRegistrationModel.NewProductCategoryTypeId != null)
            {
                if (AbbRegistrationModel.NewProductCategoryId > 0 && AbbRegistrationModel.NewProductCategoryTypeId > 0)
                {
                    tblModelNumbers = _AbbRegistrationManager.GetModelNumListByProdCatAndProdType(AbbRegistrationModel.NewProductCategoryId, AbbRegistrationModel.NewProductCategoryTypeId, Convert.ToInt32(AbbRegistrationModel.BusinessUnitId));
                    if (tblModelNumbers != null && tblModelNumbers.Count > 0)
                    {
                        ModelNumList = tblModelNumbers.ConvertAll(a =>
                        {
                            return new SelectListItem()
                            {
                                Text = a.ModelName,
                                Value = a.ModelNumberId.ToString(),
                                Selected = false
                            };
                        });
                        ModelNumList = ModelNumList.OrderBy(o => o.Text).ToList();
                    }
                }
            }
            if (ModelNumList == null)
            {
                ModelNumList = new List<SelectListItem>();
            }
            var result = new SelectList(ModelNumList, "Value", "Text");
            return new JsonResult(result);
        }
        #endregion

        #region  method to identify redemptionbuttonConfiguration
        public int CalculateMonthsDifference(DateTime startDate, DateTime endDate)
        {
            // Calculate the difference in years
            int yearsDiff = endDate.Year - startDate.Year;

            // Calculate the difference in months accounted for by full years
            int monthsDiff = yearsDiff * 12;

            // Calculate the remaining months within the same year
            monthsDiff += endDate.Month - startDate.Month;

            return monthsDiff;
        }
        #endregion

    }
}