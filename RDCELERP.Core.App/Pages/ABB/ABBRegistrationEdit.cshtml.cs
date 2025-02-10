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
using RDCELERP.BAL.MasterManager;
using RDCELERP.Model.ABBPlanMaster;

namespace RDCELERP.Core.App.Pages.AbbReg
{
    public class ABBRegistrationEditModel : BasePageModel
    {
        #region Variable Declaration
        private readonly IAbbRegistrationManager _AbbRegistrationManager;
        private readonly IUserManager _UserManager;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IStateManager _stateManager;
        private readonly ICityManager _cityManager;
        private readonly IBrandManager _brandManager;
        private readonly IBusinessPartnerManager _businessPartnerManager;
        private readonly IABBPlanMasterManager _abbPlanMasterManager;
        private readonly IBusinessUnitRepository _businessUnitRepository;
        private readonly IProductCategoryManager _productCategoryManager;
        private readonly IProductTypeManager _productTypeManager;
        private readonly IStoreCodeManager _storeCodeManager;
        private readonly IConfiguration configuration;
        private readonly RDCELERP.DAL.Entities.Digi2l_DevContext _context;
        private readonly IPinCodeManager _pinCodeManager;
        IExcelDataReader reader;
        #endregion

        #region Constructor
        public ABBRegistrationEditModel(IPinCodeManager pinCodeManager, IStoreCodeManager storeCodeManager, RDCELERP.DAL.Entities.Digi2l_DevContext context, IConfiguration _configuration, IProductTypeManager productTypeManager, IAbbRegistrationManager AbbRegistrationManager, IProductCategoryManager productCategoryManager, IBusinessPartnerManager businessPartnerManager, IBrandManager brandManager, IStateManager StateManager, ICityManager CityManager, IWebHostEnvironment webHostEnvironment, IOptions<ApplicationSettings> config, IUserManager userManager, IBusinessUnitRepository businessUnitRepository, IABBPlanMasterManager abbPlanMasterManager)
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
            _businessUnitRepository = businessUnitRepository;
            _abbPlanMasterManager = abbPlanMasterManager;
        }
        #endregion

        [BindProperty(SupportsGet = true)]
        public AbbRegistrationModel AbbRegistrationModel { get; set; }

        public List<TblAbbregistration> TblAbbregistration { get; set; }

        public List<CityViewModel> citylist = new List<CityViewModel>();

        public IActionResult OnGet(int id)
        {
            if (id != 0 && id > 0)
            {
                AbbRegistrationModel = _AbbRegistrationManager.GetABBRegistrationId(Convert.ToInt32(id));
                AbbRegistrationModel.IsModelDetailRequired = false;

                TblBusinessUnit tblBusinessUnit = _businessUnitRepository.GetSingle(x => x.BusinessUnitId == Convert.ToInt32(AbbRegistrationModel.BusinessUnitId));
                if (tblBusinessUnit != null)
                {
                    if (tblBusinessUnit.IsModelDetailRequired == true)
                    {
                        AbbRegistrationModel.IsModelDetailRequired = true;
                        var ModelNumList = _AbbRegistrationManager.GetModelNumListByProdCatAndProdType(AbbRegistrationModel.NewProductCategoryId, AbbRegistrationModel.NewProductCategoryTypeId, Convert.ToInt32(AbbRegistrationModel.BusinessUnitId));
                        if (ModelNumList != null && ModelNumList.Count > 0)
                        {
                            ViewData["ModelNumList"] = new SelectList(ModelNumList, "ModelNumberId", "ModelName");
                        }
                    }
                    else
                    {
                        AbbRegistrationModel.IsModelDetailRequired = false;

                    }
                }

            }

            var StoreCode = _businessPartnerManager.GetAllBusinessPartner(AbbRegistrationModel.BusinessUnitId);
            if (StoreCode != null)
            {
                ViewData["StoreCode"] = new SelectList(StoreCode, "StoreCode", "StoreCode");
                ViewData["Storename"] = new SelectList(StoreCode, "BusinessPartnerId", "Name");
                ViewData["Storeemail"] = new SelectList(StoreCode, "BusinessPartnerId", "Email");
            }

            //var Storename= _businessPartnerManager.GetAllBusinessPartner();
            //if (Storename != null)
            //{
            //    ViewData["Storename"] = new SelectList(Storename, "BusinessPartnerId", "Name");
            //}

            //var Storeemail = _businessPartnerManager.GetAllBusinessPartner();
            //if (Storeemail != null)
            //{
            //    ViewData["Storeemail"] = new SelectList(Storeemail, "BusinessPartnerId", "Email");
            //}

            var ProductGroup = _productCategoryManager.GetAllProductCategoryByAbbPlanMaster(Convert.ToInt32(AbbRegistrationModel.BusinessUnitId));
            if (ProductGroup != null)
            {
                ViewData["ProductGroup"] = new SelectList(ProductGroup, "Id", "Description");
            }

            var ProductType = _productTypeManager.GetAllProductTypeByAbbPlanMaster(Convert.ToInt32(AbbRegistrationModel.NewProductCategoryId), Convert.ToInt32(AbbRegistrationModel.BusinessUnitId));
            if (ProductType != null)
            {
                ViewData["ProductType"] = new SelectList(ProductType, "Id", "Description");
            }

            var NewBrandId = _brandManager.GetAllBrandForAbb(Convert.ToInt32(AbbRegistrationModel.NewProductCategoryId), Convert.ToInt32(AbbRegistrationModel.BusinessUnitId));
            if (NewBrandId != null)
            {
                ViewData["NewBrandId"] = new SelectList(NewBrandId, "Id", "Name");
            }

            if (_loginSession == null)
            {
                return RedirectToPage("/index");
            }
            else

            {
                ViewData["InvoiceImageURL"] = _baseConfig.Value.InvoiceImageURL;
                string MVCBaseurl = _baseConfig.Value.MVCBaseURLForABBInvoice;
                string ERPABBInvoiceUrl = _baseConfig.Value.BaseURL + EnumHelper.DescriptionAttr(FileAddressEnum.ABBInvoice);

                //AbbRegistrationModel.InvoiceImageWithPath = url + "DB_Files/ABB/InvoiceImage/" + AbbRegistrationModel.InvoiceImage;

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
            if (ModelState.IsValid)
            {
                result = _AbbRegistrationManager.SaveABBRegDetails(AbbRegistrationModel);
            }
            if (result > 0)
                return RedirectToPage("NotApproved");

            else
                return RedirectToPage("Manage");
        }
        public JsonResult OnGetABBDetailsByCustMobAsync()
        {
            return new JsonResult(_AbbRegistrationManager.GetCustDetailsByMob(AbbRegistrationModel.CustMobile));
        }
        public JsonResult OnGetStoreManageEmailAsync()
        {
            return new JsonResult(_AbbRegistrationManager.GetStoremanageemail(AbbRegistrationModel.StoreCode.ToString()));
        }
        public JsonResult OnGetABBEditByIdAsync()
        {
            return new JsonResult(_UserManager.GetUserById(AbbRegistrationModel.AbbregistrationId));
        }

        #region Product category type
        public JsonResult OnGetProductCategoryTypeAsync()
        {
            var productTypeList = _productTypeManager.GetAllProductTypeByAbbPlanMaster(Convert.ToInt32(AbbRegistrationModel.NewProductCategoryId), Convert.ToInt32(AbbRegistrationModel.BusinessUnitId));
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

        #region Get Brand by Product category from tblbrandsmartbuy
        public JsonResult OnGetBrandAsync()
        {
            //incomplete code need to change
            var brandList = _brandManager.GetAllBrandForAbb(Convert.ToInt32(AbbRegistrationModel.NewProductCategoryId), Convert.ToInt32(AbbRegistrationModel.BusinessUnitId));
            if (brandList != null)
            {
                ViewData["NewBrandId"] = new SelectList(brandList, "Id", "Name");
            }

            return new JsonResult(brandList);
        }
        #endregion 


        #region AbbPlanPrice 
        public JsonResult OnGetAbbPlanPriceAsync(string amount, int cat, int type)
        {
            Abbplandetail Abbplandetail = new Abbplandetail();

            //Abbplandetail = _abbPlanMasterManager.GetabbPlanPrice(Convert.ToInt32(cat), Convert.ToInt32(type), amount.ToString(), UserD2C);

            return new JsonResult(Abbplandetail);
        }

        #endregion
    }
}