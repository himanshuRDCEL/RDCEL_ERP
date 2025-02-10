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
using RDCELERP.Model;
using RDCELERP.Model.VoucherRedemption;
using RDCELERP.BAL.MasterManager;
using RDCELERP.Model.DealerDashBoard;
using RDCELERP.DAL.Repository;
using Microsoft.EntityFrameworkCore;
using RDCELERP.BAL.Enum;
using static ICSharpCode.SharpZipLib.Zip.ExtendedUnixData;

namespace RDCELERP.Core.App.Pages.Voucher
{
    public class VoucherDetails : BasePageModel
    {
        #region Variable discussion
        private readonly RDCELERP.DAL.Entities.Digi2l_DevContext _context;
        private readonly IBusinessUnitRepository _businessUnitRepository;
        private readonly IBusinessPartnerRepository _businessPartnerRepository;
        private readonly IVoucherRedemptionManager _voucherRedemptionManager;
        private readonly IProductCategoryRepository _productCategoryRepository;
        private readonly IProductTypeRepository _productTypeRepository;
        private readonly IExchangeOrderRepository _exchangeOrderRepository;
        private readonly IBrandRepository _brandRepository;
        
        DateTime _currentDatetime = DateTime.Now.TrimMilliseconds();
        #endregion

        #region Constructor
        public VoucherDetails(IOptions<ApplicationSettings> config, IBusinessUnitRepository businessUnitRepository, IVoucherRedemptionManager voucherRedemptionManager, IProductCategoryRepository productCategoryRepository, Digi2l_DevContext context, IProductTypeRepository productTypeRepository, IExchangeOrderRepository exchangeOrderRepository, IBrandRepository brandRepository, IBusinessPartnerRepository businessPartnerRepository)
            : base(config)
        {
            _businessUnitRepository = businessUnitRepository;
            _voucherRedemptionManager = voucherRedemptionManager;
            _productCategoryRepository = productCategoryRepository;
            _context = context;
            _productTypeRepository = productTypeRepository;
            _exchangeOrderRepository = exchangeOrderRepository;
            _brandRepository = brandRepository;
            _businessPartnerRepository = businessPartnerRepository;
        }
        #endregion

        #region Bind Properties
        //[BindProperty(SupportsGet = true)]
        //public VoucherDataContract voucherDataContract { get; set; }

        [BindProperty(SupportsGet = true)]
        public ExchangeOrderDataContract? exchangeOrderDataContract { get; set; }
        #endregion

        public IActionResult OnGet(VoucherDataContract? voucherDataContract)
        {
            string baseUrlforBack = _baseConfig?.Value?.BaseURL;

            string Message = TempData["VoucherCode"] as string;
            if (_loginSession == null)
            {
                return RedirectToPage("/index");
            }
            else
            {
                if(voucherDataContract.VoucherCode!=string.Empty || voucherDataContract.VoucherCode!=null&& voucherDataContract.PhoneNumber!=string.Empty || voucherDataContract.PhoneNumber != null)
                {
                    exchangeOrderDataContract = _voucherRedemptionManager.GetExchangeOrderDCByVoucherCode(voucherDataContract.VoucherCode, voucherDataContract.PhoneNumber);

                    if (exchangeOrderDataContract != null)
                    {
                        #region new product details adding

                        List<TblProductCategory> prodGroupListForBosch = GetProductCatForNew(Convert.ToInt32(exchangeOrderDataContract.BusinessUnitId));
                        ViewData["ProductGroup"] = new SelectList(prodGroupListForBosch, "Id", "Description");

                        #endregion

                        #region brand mapping
                        TblBusinessUnit BusinessUnitObj = _businessUnitRepository.GetSingle(x => x.IsActive == true && x.BusinessUnitId == Convert.ToInt32(exchangeOrderDataContract.BusinessUnitId));
                        if (BusinessUnitObj!=null)
                        {
                            exchangeOrderDataContract.IsBumultiBrand = BusinessUnitObj.IsBumultiBrand!=null? Convert.ToBoolean(BusinessUnitObj.IsBumultiBrand):false;
                            exchangeOrderDataContract.IsModelDetailRequired = BusinessUnitObj.IsModelDetailRequired != null ? Convert.ToBoolean(BusinessUnitObj.IsModelDetailRequired) : false;
                            exchangeOrderDataContract.IsProductSerialNumberRequired = BusinessUnitObj.IsProductSerialNumberRequired;
                        }
                        if (BusinessUnitObj.IsBumultiBrand == false)
                        {
                            TblBrand brandObj = _brandRepository.GetSingle(x => x.IsActive == true && x.BusinessUnitId == BusinessUnitObj.BusinessUnitId);
                            if (brandObj != null)
                            {
                                if (brandObj.Name != null)
                                {
                                    exchangeOrderDataContract.BrandName = brandObj.Name;
                                    exchangeOrderDataContract.NewBrandId = brandObj.Id;
                                }
                            }
                        }
                        else
                        {
                            List<TblBrand> brandsList = _brandRepository.GetList(x => x.IsActive == true).ToList();
                            brandsList = brandsList.OrderBy(o => o.Name).ToList();
                            if (brandsList != null && brandsList.Count > 0)
                            {
                                // ViewBag.Brand = new SelectList(brandsList, "Id", "Name");
                                ViewData["NewBrandId"] = new SelectList(brandsList, "Id", "Name");
                            }
                        }
                        #endregion

                        #region state list
                        List<TblBusinessPartner> tblBusinessPartner = _businessPartnerRepository.GetList(x => x.IsActive == true && x.BusinessUnitId == Convert.ToInt32(exchangeOrderDataContract.BusinessUnitId)
                        && (x.IsExchangeBp != null && x.IsExchangeBp == true)).ToList();

                        List<string> states = tblBusinessPartner.OrderBy(o => o.City)
                                                                    .Select(x => x.State.Trim())
                                                                    .Distinct().ToList();
                        List<SelectListItem> stateListItems = states.Select(x => new SelectListItem
                        {
                            Text = x,
                            Value = x
                        }).ToList();

                        ViewData["stateList"] = new SelectList(stateListItems, "Text", "Text");

                        #endregion
                    }
                }
                else
                {
                    TempData["URLredirection"] = baseUrlforBack + "Voucher/VoucherVerfication";

                    TempData["Message"] = "Not allowed to access this page";
                    return RedirectToPage("/Voucher/Thankyou");
                }

                return Page();
            }
        }
        public IActionResult OnPostAsync()
        {
            string baseUrlforBack = _baseConfig?.Value?.BaseURL;

            int result = 0;

            if (exchangeOrderDataContract != null)
            {
                result = _voucherRedemptionManager.AddVouchertoDB(exchangeOrderDataContract);
            }


            if (result > 0)
            {
                TempData["URLredirection"] = baseUrlforBack + "Voucher/VoucherVerfication";

                TempData["Auth"] = true;
                TempData["Message"] = "Succeffully Redeemed your voucher";
                return RedirectToPage("/Voucher/Thankyou");
            }
            else
            {
                TempData["URLredirection"] = baseUrlforBack + "Voucher/VoucherVerfication";

                TempData["Auth"] = false;
                return RedirectToPage("/Voucher/Thankyou");

            }
        }


        #region get new product cat & type
        public List<TblProductCategory> GetProductCatForNew(int buid)
        {
            List<TblProductCategory> tblProductCategoryList = new List<TblProductCategory>();
            List<TblBuproductCategoryMapping> tblBUProdCateMapList = new List<TblBuproductCategoryMapping>();
            try
            {
                if (buid > 0)
                {
                    
                    //linq
                    var productcatlist = _context.TblBuproductCategoryMappings.Where(mapping => mapping.BusinessUnitId == buid
                              && mapping.IsActive == true
                              && mapping.IsExchange == true
                              && mapping.IsActive == true)
                                .Select(mapping => mapping.ProductCatId)
                                .Distinct()
                                .ToList();
                    //end linq


                    if (productcatlist != null && productcatlist.Count > 0)
                    {
                        foreach (var productCategory in productcatlist)
                        {
                            TblProductCategory productObj = _productCategoryRepository.GetSingle(x => x.IsActive == true
                            && x.Id == productCategory.Value && x.Id != Convert.ToInt32(ProductCatEnum.CookTop));
                            if (productObj != null)
                            {
                                tblProductCategoryList.Add(productObj);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //LibLogging.WriteErrorToDB("VoucherDetails", "GetProductCatForNew", ex);
            }

            return tblProductCategoryList;
        }

        public List<TblProductCategory> GetProductTypeForNew(int buid,int proCatId)
        {
            List<TblProductCategory> tblProductCategoryList = new List<TblProductCategory>();
            List<TblBuproductCategoryMapping> tblBUProdCateMapList = new List<TblBuproductCategoryMapping>();
            try
            {
                if (buid > 0)
                {

                    //linq
                    var productcatlist = _context.TblBuproductCategoryMappings.Where(mapping => mapping.BusinessUnitId == buid
                              && mapping.IsActive == true
                              && mapping.IsExchange == true
                              && mapping.IsActive == true)
                                .Select(mapping => mapping.ProductCatId)
                                .Distinct()
                                .ToList();
                    //end linq


                    if (productcatlist != null && productcatlist.Count > 0)
                    {
                        foreach (var productCategory in productcatlist)
                        {
                            TblProductCategory productObj = _productCategoryRepository.GetSingle(x => x.IsActive == true
                            && x.Id == productCategory.Value && x.Id != Convert.ToInt32(ProductCatEnum.CookTop));
                            if (productObj != null)
                            {
                                tblProductCategoryList.Add(productObj);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //LibLogging.WriteErrorToDB("VoucherDetails", "GetProductCatForNew", ex);
            }

            return tblProductCategoryList;
        }


        #endregion


        #region check product category availbility
        public JsonResult OnGetCheckExistenceAsync(int productCatId, int BuiD,int ExchangeId)
        {
            List<TblProductType> tblProductCategoryList = new List<TblProductType>();

            int proCatId = exchangeOrderDataContract.NewProductCategoryId;
            int BUId =Convert.ToInt32( exchangeOrderDataContract.BusinessUnitId);
            int echngId =Convert.ToInt32( exchangeOrderDataContract.Id);
            bool flag = false;
            if (proCatId > 0 && BUId > 0 && echngId>0)
            {
                TblExchangeOrder ExchangeObj = _exchangeOrderRepository.GetSingle(x => x.Id == echngId && x.IsActive == true);

                if (ExchangeObj != null && (ExchangeObj.NewProductCategoryId != null || ExchangeObj.NewProductCategoryId == proCatId))
                {
                    flag = true;
                }
                else
                {
                    if(ExchangeObj != null && (ExchangeObj.NewProductCategoryId ==null || ExchangeObj.NewProductCategoryId ==0))
                    {
                        flag = true;
                    }
                    else
                    {
                        flag = false;
                    }
                }

                if (flag)
                {
                    var productcattypelist = _context.TblBuproductCategoryMappings.Where(mapping => mapping.BusinessUnitId == BUId
                              && mapping.IsActive == true
                              && mapping.IsExchange == true
                              && mapping.ProductCatId == proCatId
                              && mapping.IsActive == true)
                                .Select(mapping => mapping.ProductTypeId)
                                .Distinct()
                                .ToList();

                    if (productcattypelist != null && productcattypelist.Count > 0)
                    {
                        foreach (var productCategory in productcattypelist)
                        {
                            TblProductType producttypeObj = _productTypeRepository.GetSingle(x => x.IsActive == true
                            && x.Id == productCategory.Value);
                            if (producttypeObj != null)
                            {
                                tblProductCategoryList.Add(producttypeObj);
                            }
                        }

                        if (tblProductCategoryList != null)
                        {
                            ViewData["productTypeList"] = new SelectList(tblProductCategoryList, "Id", "Description");
                        }
                    }

                }


            }
            else
            {
                flag = false;
            }
            return new JsonResult(tblProductCategoryList);
        }

        public JsonResult OnGetBrandListForNewAsync(int productCatId, int BuiD, int ExchangeId)
        {
            List<TblProductType> tblProductCategoryList = new List<TblProductType>();

            int proCatId = exchangeOrderDataContract.NewProductCategoryId;
            int BUId = Convert.ToInt32(exchangeOrderDataContract.BusinessUnitId);
            int echngId = Convert.ToInt32(exchangeOrderDataContract.Id);
            bool flag = false;
            if (proCatId > 0 && BUId > 0 && echngId > 0)
            {
                TblExchangeOrder ExchangeObj = _exchangeOrderRepository.GetSingle(x => x.Id == echngId && x.IsActive == true);

                if (ExchangeObj != null && (ExchangeObj.NewProductCategoryId != null || ExchangeObj.NewProductCategoryId == proCatId))
                {
                    flag = true;
                }
                else
                {
                    if (ExchangeObj != null && (ExchangeObj.NewProductCategoryId == null || ExchangeObj.NewProductCategoryId == 0))
                    {
                        flag = true;
                    }
                    else
                    {
                        flag = false;
                    }
                }

                if (flag)
                {
                    var productcattypelist = _context.TblBuproductCategoryMappings.Where(mapping => mapping.BusinessUnitId == BUId
                              && mapping.IsActive == true
                              && mapping.IsExchange == true
                              && mapping.ProductCatId == proCatId
                              && mapping.IsActive == true)
                                .Select(mapping => mapping.ProductTypeId)
                                .Distinct()
                                .ToList();

                    if (productcattypelist != null && productcattypelist.Count > 0)
                    {
                        foreach (var productCategory in productcattypelist)
                        {
                            TblProductType producttypeObj = _productTypeRepository.GetSingle(x => x.IsActive == true
                            && x.Id == productCategory.Value);
                            if (producttypeObj != null)
                            {
                                tblProductCategoryList.Add(producttypeObj);
                            }
                        }

                        if (tblProductCategoryList != null)
                        {
                            ViewData["productTypeList"] = new SelectList(tblProductCategoryList, "Id", "Description");
                        }
                    }

                }


            }
            else
            {
                flag = false;
            }
            return new JsonResult(tblProductCategoryList);
        }

        #endregion

        #region  get city list  and store list
        public JsonResult OnGetCitylistAsync(int BuiD,string statename)
        {
            List<TblBusinessPartner> tblBusinessPartnerList = new List<TblBusinessPartner>();

            string Statename1 = exchangeOrderDataContract.StateName.ToString();
            int BUId1 = Convert.ToInt32(exchangeOrderDataContract.BusinessUnitId);

            //int BUId = Convert.ToInt32(exchangeOrderDataContract.BusinessUnitId);
            if(Statename1!=string.Empty && BUId1>0)
            {
                #region state list
                List<TblBusinessPartner> tblBusinessPartner = _businessPartnerRepository.GetList(x => x.IsActive == true && x.BusinessUnitId == BUId1
                                                               && x.State.ToLower() == Statename1.ToString().ToLower() && (x.IsExchangeBp != null && x.IsExchangeBp == true)).ToList();

                List<string> states = tblBusinessPartner.OrderBy(o => o.State)
                                                            .Select(x => x.City.Trim())
                                                            .Distinct().ToList();
                List<SelectListItem> stateListItems = states.Select(x => new SelectListItem
                {
                    Text = x,
                    Value = x
                }).ToList();

                ViewData["cityList"] = new SelectList(stateListItems, "Text", "Text");

                #endregion
                return new JsonResult(stateListItems);
            }

            return new JsonResult(tblBusinessPartnerList);
        }

        public JsonResult OnGetStorelistAsync(int BuiD, string statename)
        {
            List<TblBusinessPartner> tblBusinessPartnerList = new List<TblBusinessPartner>();

            string Statename1 = exchangeOrderDataContract.StateName.ToString();
            string Cityname1 = exchangeOrderDataContract.CityName.ToString();
            int BUId = Convert.ToInt32(exchangeOrderDataContract.BusinessUnitId);

            //int BUId = Convert.ToInt32(exchangeOrderDataContract.BusinessUnitId);
            if (Statename1 != string.Empty && BUId > 0)
            {
                #region state list
                List<TblBusinessPartner> tblBusinessPartner = _businessPartnerRepository.GetList(x => x.IsActive == true && x.BusinessUnitId == BUId
                                                               && x.State.ToLower() == Statename1.ToLower().ToString() &&x.City== Cityname1.ToString() && (x.IsExchangeBp != null && x.IsExchangeBp == true)).ToList();

                List<string> states = tblBusinessPartner.OrderBy(o => o.StoreCode)
                                                            .Select(x => x.Name.Trim())
                                                            .Distinct().ToList();
                List<SelectListItem> stateListItems = states.Select(x => new SelectListItem
                {
                    Text = x,
                    Value = x
                }).ToList();

                ViewData["storeList"] = new SelectList(stateListItems, "Text", "Text");
                return new JsonResult(stateListItems);

                #endregion

            }

            return new JsonResult(tblBusinessPartnerList);
        }

        #endregion


        #region  autocomplete

        public IActionResult OnGetAutoStateName(string term,int buid)
        {
            if (term == null)
            {
                return BadRequest();
            }
            var data = _context.TblBusinessPartners
                       .Where(s => s.State.Contains(term) && s.IsActive == true && s.BusinessUnitId == buid)
                       .Select(s => new SelectListItem
                       {
                           Value = s.State,
                           Text = s.State.ToString()
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

