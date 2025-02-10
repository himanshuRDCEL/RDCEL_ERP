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
using static Org.BouncyCastle.Math.EC.ECCurve;
using RDCELERP.BAL.MasterManager;
using RDCELERP.Model.EVC;
using RDCELERP.Model.Users;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using RDCELERP.Model.DealerDashBoard;
using RDCELERP.DAL.Repository;
using System.Data;
using RDCELERP.DAL.Helper;
using Newtonsoft.Json;
using NPOI.SS.Formula.Functions;
using RDCELERP.Model.Company;
using RDCELERP.Model.UniversalPriceMaster;
using RDCELERP.Model.Sweetener;
using RDCELERP.Model.QCComment;
using DocumentFormat.OpenXml.Wordprocessing;

namespace RDCELERP.Core.App.Pages.ExchangeRegistration
{
    public class ProductDetails : BasePageModel
    {
        #region Variable discussion
        private readonly IABBRedemptionManager _AbbRedemptionManager;
        private readonly IUserManager _UserManager;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IStateManager _stateManager;
        private readonly ICityManager _cityManager;
        private readonly IBrandManager _brandManager;
        private readonly IBusinessPartnerManager _businessPartnerManager;
        private readonly IProductTypeManager _productTypeManager;
        private readonly IProductCategoryManager _productCategoryManager;

        private readonly IStoreCodeManager _storeCodeManager;
        private readonly RDCELERP.DAL.Entities.Digi2l_DevContext _context;
        private readonly IAbbRegistrationManager _AbbRegistrationManager;
        private readonly IPinCodeManager _pinCodeManager;
        private readonly IMapper _mapper;
        private readonly IBusinessUnitRepository _businessUnitRepository;
        DateTime _currentDatetime = DateTime.Now.TrimMilliseconds();
        IAbbRegistrationRepository _abbRegistrationRepository;

        public readonly IOptions<ApplicationSettings> _config;
        public readonly IOptions<ConnectionStrings> _dbConnectionString;

        private readonly IPriceMasterMappingRepository _priceMasterMappingRepository;
        public readonly ADOHelper _adoHelper;
        private readonly IProductCategoryRepository _productCategoryRepository;
        private readonly IProductTypeRepository _productTypeRepository;
        private readonly IProductConditionLabelRepository _productConditionLabelRepository;
        private readonly IQCCommentManager _qCCommentManager;
        private readonly ISweetenerManager _sweetenerManager;
        private readonly IExchangeOrderManager _exchangeOrderManager;
        private readonly IBusinessPartnerRepository _businessPartnerRepository;

        ILogging _logging;


        #endregion

        #region Constructor
        public ProductDetails(IPinCodeManager pinCodeManager, IStoreCodeManager storeCodeManager, IAbbRegistrationManager AbbRegistrationManager, RDCELERP.DAL.Entities.Digi2l_DevContext context, IProductTypeManager productTypeManager, IABBRedemptionManager abbRedemptionRegManager, IProductCategoryManager productCategoryManager, IBusinessPartnerManager businessPartnerManager, IBrandManager brandManager, IStateManager StateManager, ICityManager CityManager, IWebHostEnvironment webHostEnvironment, IOptions<ApplicationSettings> config, IUserManager userManager, IAbbRegistrationRepository abbRegistrationRepository, IMapper mapper, IBusinessUnitRepository businessUnitRepository,  IPriceMasterMappingRepository priceMasterMappingRepository, ADOHelper adoHelper, IProductCategoryRepository productCategoryRepository, ILogging logging, IProductTypeRepository productTypeRepository, IProductConditionLabelRepository productConditionLabelRepository, IQCCommentManager qCCommentManager, ISweetenerManager sweetenerManager, IExchangeOrderManager exchangeOrderManager, IOptions<ConnectionStrings> dbConnectionString, IBusinessPartnerRepository businessPartnerRepository)
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
            _config = config;
            _priceMasterMappingRepository = priceMasterMappingRepository;
            _adoHelper = adoHelper;
            _productCategoryRepository = productCategoryRepository;
            _logging = logging;
            _productTypeRepository = productTypeRepository;
            _productConditionLabelRepository = productConditionLabelRepository;
            _qCCommentManager = qCCommentManager;
            _sweetenerManager = sweetenerManager;
            _exchangeOrderManager = exchangeOrderManager;
            _dbConnectionString = dbConnectionString;
            _businessPartnerRepository = businessPartnerRepository;
        }

        //public ProductDetails(IOptions<ApplicationSettings> config)
        //{
        //    _config = config;
        //}
        #endregion

        #region Bind Properties
        [BindProperty(SupportsGet = true)]
        public ABBRedemptionViewModel AbbRedemptionModel { get; set; }

        public AbbRegistrationModel AbbRegistrationModel { get; set; }

        public ABBPlanMasterViewModel aBBPlanMasterViewModel { get; set; }

        [BindProperty(SupportsGet = true)]
        public UserViewModel UserViewModel { get; set; }

        [BindProperty(SupportsGet = true)]
        public ExchangeOrderDataContract ExchangeOrderDataContract { get; set; }

        #endregion

        public IActionResult OnGet()
        {
            int userCompanyId = 0;
            try
            {
                if (_loginSession == null)
                {
                    return RedirectToPage("/PermissionDenied");
                }
                else
                {
                    #region company details & BusinessUnit details
                    var sessionLoginobj = _loginSession.UserViewModel;
                    if (_loginSession.RoleViewModel != null && _loginSession.RoleViewModel.CompanyId > 0)
                    {
                        userCompanyId = Convert.ToInt32(_loginSession.RoleViewModel.CompanyId);
                        int UserId = Convert.ToInt32(_loginSession.UserViewModel.UserId);
                        TblCompany? tblCompany = new TblCompany();
                        tblCompany = _context.TblCompanies.Where(x => x.IsActive == true && x.CompanyId == userCompanyId).FirstOrDefault();

                        if (tblCompany != null && tblCompany.BusinessUnitId > 0)
                        {
                            TblPriceMasterMapping? mapping = new TblPriceMasterMapping();

                            ExchangeOrderDataContract.BusinessUnitId = tblCompany.BusinessUnitId;
                            ExchangeOrderDataContract.CompanyName = tblCompany.CompanyName != string.Empty ? tblCompany.CompanyName : string.Empty;

                            #region add mapping for user with business unit  and price master mapping details

                            TblUserMapping? tblUserMapping = new TblUserMapping();
                            tblUserMapping = _context.TblUserMappings.Where(x => x.IsActive == true && x.UserId == UserId).FirstOrDefault();

                            if (tblUserMapping != null && tblUserMapping.UserId > 0 && tblUserMapping.BusinessPartnerId > 0)
                            {
                                ExchangeOrderDataContract.BusinessPartnerId = Convert.ToInt32(tblUserMapping.BusinessPartnerId);
                                mapping = _priceMasterMappingRepository.GetSingle(x => x.IsActive == true && x.BusinessUnitId == tblCompany.BusinessUnitId && x.BusinessPartnerId == Convert.ToInt32(tblUserMapping.BusinessPartnerId) && x.BrandId == null);
                            }

                            if (mapping == null)
                            {
                                mapping = _priceMasterMappingRepository.GetSingle(x => x.IsActive == true && x.BusinessUnitId == tblCompany.BusinessUnitId && x.BusinessPartnerId == null && x.BrandId == null);
                            }

                            #endregion

                            #region PRODUCT GROUP MAPPING
                            if (mapping != null)
                            {
                                ExchangeOrderDataContract.PriceMasterNameId = Convert.ToInt32(mapping.PriceMasterNameId);
                                IList<TblProductCategory> productCategories = new List<TblProductCategory>();
                                productCategories = GetTblProductCategories(Convert.ToInt32(mapping.PriceMasterNameId));

                                productCategories = productCategories.OrderBy(o => o.DescriptionForAbb).ToList();

                                if (productCategories != null && productCategories.Count > 0)
                                {
                                    ViewData["ProductGroup"] = new SelectList(productCategories, "Id", "Description");
                                }

                            }
                            #endregion

                            #region product condition label mapping
                            List<TblProductConditionLabel> conditionLabel = new List<TblProductConditionLabel>();

                            conditionLabel = _productConditionLabelRepository.GetList(x => x.BusinessUnitId == ExchangeOrderDataContract.BusinessUnitId && x.BusinessPartnerId == ExchangeOrderDataContract.BusinessPartnerId && x.IsActive == true).ToList();

                            if (conditionLabel.Count > 0)
                            {
                                //ExchangeOrderDataContract.ProductConditionCount = conditionLabel.Count;
                                ExchangeOrderDataContract.QualityCheckList = conditionLabel.Select(x => new SelectListItem
                                {
                                    Text = x.PclabelName,
                                    Value = x.OrderSequence.ToString()
                                }).ToList();

                                ViewData["QualityCheckList"] = new SelectList(ExchangeOrderDataContract.QualityCheckList, "Value", "Text");

                            }
                            else
                            {
                                conditionLabel = _productConditionLabelRepository.GetList(x => x.BusinessUnitId == ExchangeOrderDataContract.BusinessUnitId && x.BusinessPartnerId == null && x.IsActive == true).ToList();

                                ExchangeOrderDataContract.QualityCheckList = conditionLabel.Select(x => new SelectListItem
                                {
                                    Text = x.PclabelName,
                                    Value = x.OrderSequence.ToString()
                                }).ToList();

                                ViewData["QualityCheckList"] = new SelectList(ExchangeOrderDataContract.QualityCheckList, "Value", "Text");

                            }

                            #endregion
                        }
                        else
                        {
                            return RedirectToPage("/PermissionDenied");
                        }

                    }
                    else
                    {
                        return RedirectToPage("/PermissionDenied");
                    }
                    #endregion

                }
            }
            catch (Exception ex)
            {

            }
            return Page();

        }
        public IActionResult OnPostAsync()
        {
            string result = string.Empty;
            string message = string.Empty;
            string urlReturn = "ExchangeRegistration/ProductDetails";
            if (_loginSession != null)
            {
                ExchangeOrderDataContract.CreatedBy = _loginSession.UserViewModel.UserId;
            }
            result = _exchangeOrderManager.AddExchangeOrders(ExchangeOrderDataContract);

            if (result != string.Empty || result != "")
            {
                string redirection = _config?.Value?.BaseURL;
                TempData["URLredirection"] = redirection;
                TempData["Message"] = "Your Exchange details have been received at Digi2L. Our product registration referance no. " + result + " Our quality check team will connect with you soon.";
                return RedirectToPage("/ExchangeRegistration/ThankYou");
            
            }
            else
            {

                TempData["Message"] = "Order not recorded due to some error please try again later";
                return RedirectToPage("/ExchangeRegistration/ThankYou");
            }

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
        }

        #region Get Redemption value calculatation & periods in months
        public JsonResult OnGetRedemptionValueAsync(string redemptiondate, decimal? productNetPrice, int proCatId, DateTime? startDate, int proTypeId, int BUID)
        {
            AbbRedemptionValue abbRedemptionValue = new AbbRedemptionValue();
            if (proCatId > 0 && proTypeId > 0 && BUID > 0)
            {
                //here startDate is InvoiceDate of the order
                int monthsdiff = 0;//CalculateMonthsDifference(Convert.ToDateTime(startDate), DateTime.Parse(redemptiondate));

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

        #region call stored procedure for getting product details and price from db - by ashwin

        #region product gategory list by priceMasterNameId
        public List<TblProductCategory> TblProductCategories { get; set; }

        public IList<TblProductCategory> GetTblProductCategories(int priceMasterNameId)
        {
            DataTable dt = new DataTable();
            var results = new List<TblProductCategory>();
            IList<TblProductCategory> oldProductCategoriesList = new List<TblProductCategory>();
            string dbConnectionString = string.Empty;
            try
            {
                dbConnectionString = _dbConnectionString.Value.Digi2l_DevContext.ToString();
                
                if (priceMasterNameId > 0)
                {
                    #region stored procedure call for categorylist
                    SqlParameter[] sqlParam =  {
                        new SqlParameter("@PriceMasterNameId", priceMasterNameId)
                        };
                    dt = _adoHelper.ExecuteDataTable("sp_GetProductCategoryByPriceMasterNameId",dbConnectionString, sqlParam);

                    List<TblUniversalPriceMaster>? list = null;
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        string JSONresult;
                        JSONresult = JsonConvert.SerializeObject(dt);
                        list = JsonConvert.DeserializeObject<List<TblUniversalPriceMaster>>(JSONresult);

                        if (list.Count > 0)
                        {
                            foreach (var productCat in list)
                            {
                                TblProductCategory objTblProductCategory = new TblProductCategory();

                                objTblProductCategory = _productCategoryRepository.GetSingle(x => x.IsActive == true && x.IsAllowedForOld == true && x.Id == productCat.ProductCategoryId);
                                if (objTblProductCategory != null)
                                {
                                    oldProductCategoriesList.Add(objTblProductCategory);
                                }
                                else
                                {
                                    continue;
                                }
                            }


                        }
                        else
                        {
                            oldProductCategoriesList = null;
                        }
                    }
                    else
                    {
                        oldProductCategoriesList = null;
                    }
                    #endregion
                }
                else
                {
                    oldProductCategoriesList = null;
                }


                return oldProductCategoriesList;
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("ProductDetailsPage", "GetTblProductCategories", ex);

                oldProductCategoriesList = null;
            }
            return oldProductCategoriesList;
        }
        #endregion

        #region product type list by priceMasterNameId and ProductCategoryId
        public JsonResult OnGetProductTypelist(int priceMasterNameId, int productCatId)
        {
            DataTable dt = new DataTable();
            var results = new List<TblProductType>();
            IList<TblProductType> oldProductTypeList = new List<TblProductType>();
            try
            {
                string dbConnectionString = _dbConnectionString.Value.Digi2l_DevContext.ToString();

                if (priceMasterNameId > 0 && productCatId > 0)
                {
                    #region stored procedure call for categorylist
                    SqlParameter[] sqlParam =  {
                        new SqlParameter("@PriceMasterNameId", priceMasterNameId),
                        new SqlParameter("@catid", productCatId)
                        };
                    dt = _adoHelper.ExecuteDataTable("sp_GetProductTypeByPriceMasterNameId", dbConnectionString, sqlParam);

                    List<TblUniversalPriceMaster>? list = null;
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        string JSONresult;
                        JSONresult = JsonConvert.SerializeObject(dt);
                        list = JsonConvert.DeserializeObject<List<TblUniversalPriceMaster>>(JSONresult);

                        if (list.Count > 0)
                        {
                            foreach (var productCat in list)
                            {
                                TblProductType objTblProductType = new TblProductType();

                                objTblProductType = _productTypeRepository.GetSingle(x => x.IsActive == true && x.IsAllowedForOld == true && x.Id == productCat.ProductTypeId);
                                if (objTblProductType != null)
                                {
                                    if(objTblProductType.Size!=null && objTblProductType.Size != " ")
                                    {
                                        objTblProductType.Description = objTblProductType.Description + " ( " + objTblProductType.Size + " )";
                                    }
                                    oldProductTypeList.Add(objTblProductType);
                                }
                                else
                                {
                                    continue;
                                }
                            }


                        }
                        else
                        {
                            oldProductTypeList = null;
                        }
                    }
                    else
                    {
                        oldProductTypeList = null;
                    }
                    #endregion
                }
                else
                {
                    oldProductTypeList = null;
                }


                return new JsonResult(oldProductTypeList);
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("ProductDetailsPage", "OnGetProductTypelist", ex);

                oldProductTypeList = null;
            }
            return new JsonResult(oldProductTypeList);
        }

        #endregion

        #region product Brand list by priceMasterNameId and ProductCategoryId
        public JsonResult OnGetProductBrandlist(int priceMasterNameId, int productCatId, int producttypeId, int BuId)
        {
            DataTable dt = new DataTable();
            List<BrandViewModels> brandViewModels = new List<BrandViewModels>();
            List<SelectListItem> BrandList = null;
            List<SelectListItem> BrandListFinal = null;
            try
            {
                string dbConnectionString = _dbConnectionString.Value.Digi2l_DevContext.ToString();

                if (priceMasterNameId > 0 && productCatId > 0 && producttypeId > 0 && BuId > 0)
                {
                    #region stored procedure call for categorylist
                    SqlParameter[] sqlParam =  {
                        new SqlParameter("@PriceMasterNameId", priceMasterNameId),
                        new SqlParameter("@catid", productCatId),
                      new SqlParameter("@typeId",producttypeId)
                        };
                    dt = _adoHelper.ExecuteDataTable("sp_GetBrandsByPriceMasterNameId", dbConnectionString, sqlParam);

                    List<TblBrand>? list = new List<TblBrand>();
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        string JSONresult;
                        JSONresult = JsonConvert.SerializeObject(dt);
                        list = JsonConvert.DeserializeObject<List<TblBrand>>(JSONresult);

                        if (list.Count > 0)
                        {
                            brandViewModels = _mapper.Map<List<TblBrand>, List<BrandViewModels>>(list).ToList();

                            TblBusinessUnit businessUnit = _businessUnitRepository.GetSingle(x => x.BusinessUnitId == BuId);
                            if (businessUnit != null)
                            {
                                BrandViewModels? buBrand = brandViewModels.FirstOrDefault(o => o.Name.ToLower().Equals(businessUnit.Name.ToLower()));
                                BrandViewModels? otherBrand = brandViewModels.FirstOrDefault(o => o.Name.ToLower().Equals("others"));

                                brandViewModels.RemoveAll(x => x.Name.ToLower().Equals(businessUnit.Name.ToLower()));
                                brandViewModels.RemoveAll(x => x.Name.ToLower().Equals("others"));

                                if (buBrand != null)
                                    brandViewModels.Add(buBrand);

                                if (otherBrand != null)
                                    brandViewModels.Add(otherBrand);
                            }
                        }
                        else
                        {
                            brandViewModels = null;
                        }
                    }
                    else
                    {
                        brandViewModels = null;
                    }
                    #endregion
                }
                else
                {
                    brandViewModels = null;
                }

                return new JsonResult(brandViewModels);
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("ProductDetailsPage", "OnGetProductBrandlist", ex);

                brandViewModels = null;
            }
            return new JsonResult(brandViewModels);
        }

        #endregion

        #region get product price 
        public JsonResult OnGetProductPrice(int priceMasterNameId, int productCatId, int producttypeId, int BuId, int productCondtionId, int productBrandId, int BpId)
        {
            UniversalPMViewModel? universalPMViewModel = null;
            GetSweetenerDetailsDataContract? getsweetenerDataViewModel = new GetSweetenerDetailsDataContract();
            TblOrderBasedConfig tblOrderBasedConfig = null;
            ProductPriceDetailsDataContract productPriceDetailsData = new ProductPriceDetailsDataContract();
            QCProductPriceDetails qCProductPriceDetails = new QCProductPriceDetails();
            SweetenerDataViewModel? sweetenerDataVM = null;
            try
            {
                if (priceMasterNameId > 0 && productCatId > 0 && producttypeId > 0 && BuId > 0 && productCondtionId > 0)
                {
                    productPriceDetailsData.ProductCatId = Convert.ToInt32(productCatId);
                    qCProductPriceDetails.ProductCatId = Convert.ToInt32(productCatId);

                    productPriceDetailsData.ProductTypeId = Convert.ToInt32(producttypeId);
                    qCProductPriceDetails.ProductTypeId = Convert.ToInt32(producttypeId);

                    productPriceDetailsData.PriceNameId = Convert.ToInt32(priceMasterNameId);
                    qCProductPriceDetails.PriceNameId = Convert.ToInt32(priceMasterNameId);

                    productPriceDetailsData.BusinessUnitId = Convert.ToInt32(BuId);
                    qCProductPriceDetails.BusinessUnitId = Convert.ToInt32(BuId);

                    productPriceDetailsData.conditionId = Convert.ToInt32(productCondtionId);
                    qCProductPriceDetails.FinalProdQualityId = Convert.ToInt32(productCondtionId);

                    productPriceDetailsData.BrandId = Convert.ToInt32(productBrandId);
                    qCProductPriceDetails.BrandId = Convert.ToInt32(productBrandId);

                    universalPMViewModel = _qCCommentManager.GetBasePrice(qCProductPriceDetails);

                    TblProductConditionLabel tblProductConditionLabel = _productConditionLabelRepository.GetSingle(x => x.IsActive == true && x.BusinessUnitId == BuId && x.BusinessPartnerId == null && x.OrderSequence == productCondtionId);
                    if (tblProductConditionLabel != null)
                    {
                        getsweetenerDataViewModel.BusinessUnitId = Convert.ToInt32(BuId);
                        getsweetenerDataViewModel.BusinessPartnerId = Convert.ToInt32(BpId);
                        getsweetenerDataViewModel.BrandId = Convert.ToInt32(productBrandId);

                        if (tblProductConditionLabel.IsSweetenerApplicable == true)
                        {
                            sweetenerDataVM = _sweetenerManager.GetSweetenerAmtExchange(getsweetenerDataViewModel);

                        }

                        if (sweetenerDataVM != null && sweetenerDataVM.SweetenerTotal > 0)
                        {
                            universalPMViewModel.BaseValue = universalPMViewModel.BaseValue;
                            universalPMViewModel.ExchangePrice = universalPMViewModel.BaseValue + sweetenerDataVM.SweetenerTotal;
                            universalPMViewModel.SweetenerBU = sweetenerDataVM.SweetenerBu != null ? sweetenerDataVM.SweetenerBu : 0;
                            universalPMViewModel.SweetenerBP = sweetenerDataVM.SweetenerBP != null ? sweetenerDataVM.SweetenerBP : 0;
                            universalPMViewModel.SweetenerDigi2l = sweetenerDataVM.SweetenerDigi2L != null ? sweetenerDataVM.SweetenerDigi2L : 0;
                            universalPMViewModel.TotalSweetener = sweetenerDataVM.SweetenerTotal != null ? sweetenerDataVM.SweetenerTotal : 0;

                        }
                        else if (universalPMViewModel != null && universalPMViewModel.BaseValue > 0)
                        {
                            universalPMViewModel.BaseValue = universalPMViewModel.BaseValue;
                            universalPMViewModel.ExchangePrice = universalPMViewModel.BaseValue;
                            universalPMViewModel.SweetenerBU = 0;
                            universalPMViewModel.SweetenerBP = 0;
                            universalPMViewModel.SweetenerDigi2l = 0;
                            universalPMViewModel.TotalSweetener = 0;

                        }
                        else
                        {
                            universalPMViewModel.BaseValue = universalPMViewModel.BaseValue;
                            universalPMViewModel.ExchangePrice = universalPMViewModel.BaseValue;
                            universalPMViewModel.SweetenerBU = 0;
                            universalPMViewModel.SweetenerBP = 0;
                            universalPMViewModel.SweetenerDigi2l = 0;
                            universalPMViewModel.TotalSweetener = 0;
                        }
                    }
                }
                else
                {
                    universalPMViewModel = null;
                }
                return new JsonResult(universalPMViewModel);
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("ProductDetailsPage", "OnGetProductPrice", ex);

            }
            return new JsonResult(universalPMViewModel);
        }
        #endregion

        #region AUTOCOMPLETE BY PINCODE


        #endregion

        #endregion


        #region  autocomplete

        public IActionResult OnGetAutoStateName(string term)
        {
            if (term == null)
            {
                return BadRequest();
            }
            var data = _context.TblStates
                       .Where(s => s.Name.Contains(term) && s.IsActive == true)
                       .Select(s => new SelectListItem
                       {
                           Value = s.Name,
                           Text = s.StateId.ToString()
                       })
                       .ToArray();
            return new JsonResult(data);
        }
        public IActionResult OnGetAutoCityName(string term, string term2)
        {
            if (term == null)
            {
                return BadRequest();
            }
            var list = _context.TblCities
                       .Where(e => e.Name.Contains(term) && e.StateId == Convert.ToInt32(term2) && e.IsActive == true)
                        .Select(s => new SelectListItem
                        {
                            Value = s.Name,
                            Text = s.CityId.ToString()
                        })
                       .ToArray();
            return new JsonResult(list);
        }
        public IActionResult OnGetAutoPinCode(int term, int term2)
        {
            if (term == null)
            {
                return BadRequest();
            }
            var list = _context.TblPinCodes
                       .Where(e => e.ZipCode.ToString().Contains(term.ToString()) && e.CityId == (term2) && e.IsActive == true)
                        .Select(s => new SelectListItem
                        {
                            Value = s.ZipCode.ToString(),
                            Text = s.Id.ToString()
                        })
                       .ToArray();
            return new JsonResult(list);
        }

        #endregion
    }


}

