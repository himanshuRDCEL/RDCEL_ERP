using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ClosedXML.Excel;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Options;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using QRCoder;
using RDCELERP.BAL.Helper;
using RDCELERP.BAL.Interface;
using RDCELERP.Common.Enums;
using RDCELERP.Common.Helper;
using RDCELERP.Core.App.Pages.Base;
using RDCELERP.DAL.Entities;
using RDCELERP.DAL.IRepository;
using RDCELERP.Model.ABBPlanMaster;
using RDCELERP.Model.ABBPriceMaster;
using RDCELERP.Model.ABBRedemption;
using RDCELERP.Model.AbbRegistration;
using RDCELERP.Model.Base;
using RDCELERP.Model.BusinessPartner;
using RDCELERP.Model.BusinessUnit;
using RDCELERP.Model.City;
using RDCELERP.Model.Company;
using RDCELERP.Model.ExchangeOrder;
using RDCELERP.Model.ImageLabel;
using RDCELERP.Model.Master;
using RDCELERP.Model.ModelNumber;
using RDCELERP.Model.PinCode;
using RDCELERP.Model.PriceMaster;
using RDCELERP.Model.Product;
using RDCELERP.Model.ServicePartner;
using RDCELERP.Model.State;
using RDCELERP.Model.VehicleIncentive;
using CellType = NPOI.SS.UserModel.CellType;

namespace RDCELERP.Core.App.Pages.LOV_Utility
{
    public class LOV_UtilityModel : CrudBasePageModel
    {
        #region Variable Declaration
        private readonly RDCELERP.DAL.Entities.Digi2l_DevContext _context;
        private readonly ICityManager _CityManager;
        private readonly ICityRepository _CityRepository;
        private readonly IPinCodeManager _PinCodeManager;
        private readonly IPinCodeRepository _PinCodeRepository;
        private readonly IStateManager _StateManager;
        private readonly IStateRepository _StateRepository;
        private readonly IProductCategoryManager _productCategoryManager;
        private readonly IProductCategoryRepository _productCategoryRepository;
        private readonly IProductTypeRepository _productTypeRepository;
        private readonly IProductTypeManager _productTypeManager;
        private readonly IBrandRepository _brandRepository;
        private readonly IBrandManager _brandManager;
        private readonly IBusinessPartnerRepository _businessPartnerRepository;
        private readonly IBusinessPartnerManager _businesssPartnerManager;
        private readonly IABBPlanMasterRepository _abbPlanMasterRepository;
        private readonly IABBPlanMasterManager _abbPlanMasterManager;
        private readonly IABBPriceMasterRepository _abbPriceMasterRepository;
        private readonly IABBPriceMasterManager _abbPriceMasterManager;
        private readonly IModelNumberRepository _modelNumberRepository;
        private readonly IModelNumberManager _modelNumberManager;
        private readonly IPriceMasterRepository _priceMasterRepository;
        private readonly IPriceMasterManager _priceMasterManager;
        private readonly IServicePartnerManager _servicePartnerManager;
        private readonly IServicePartnerRepository _servicePartnerRepository;
        private readonly IExchangeOrderManager _exchangeOrderManager;
        private readonly IExchangeOrderRepository _exchangeOrderRepository;
        private readonly IAbbRegistrationManager _abbRegistrationManager;
        private readonly IVehicleIncentiveManager _vehicleIncentiveManager;
        private readonly IVehicleIncentiveRepository _vehicleIncentiveRepository;
        private readonly IImageLabelMasterManager _imageLabelMasterManager;
        #endregion

        public LOV_UtilityModel(RDCELERP.DAL.Entities.Digi2l_DevContext context, IOptions<ApplicationSettings> config, CustomDataProtection protector, IServicePartnerManager servicePartnerManager, IServicePartnerRepository servicePartnerRepository, IBrandManager brandManager, IBrandRepository brandRepository, IPriceMasterRepository priceMasterRepository, IModelNumberManager modelNumberManager, IModelNumberRepository modelNumberRepository, IPriceMasterManager priceMasterManager, IProductTypeManager productTypeManager, IProductTypeRepository productTypeRepository, IProductCategoryManager productCategoryManager, IProductCategoryRepository productCategoryRepository, IStateManager StateManager, IStateRepository StateRepository, ICityManager CityManager, ICityRepository CityRepository, IPinCodeManager PinCodeManager, IPinCodeRepository PinCodeRepository, IBusinessPartnerRepository businessPartnerRepository, IBusinessPartnerManager businesssPartnerManager, IABBPlanMasterRepository abbPlanMasterRepository, IABBPlanMasterManager abbPlanMasterManager, IABBPriceMasterManager abbPriceMasterManager, IABBPriceMasterRepository abbPriceMasterRepository, IExchangeOrderManager exchangeOrderManager, IExchangeOrderRepository exchangeOrderRepository, IAbbRegistrationManager abbRegistrationManager, IVehicleIncentiveManager vehicleIncentiveManager, IVehicleIncentiveRepository vehicleIncentiveRepository, IImageLabelMasterManager imageLabelMasterManager) : base(config, protector)
        {
            _CityManager = CityManager;
            _CityRepository = CityRepository;
            _PinCodeManager = PinCodeManager;
            _PinCodeRepository = PinCodeRepository;
            _StateManager = StateManager;
            _StateRepository = StateRepository;
            _productCategoryManager = productCategoryManager;
            _productCategoryRepository = productCategoryRepository;
            _productTypeRepository = productTypeRepository;
            _productTypeManager = productTypeManager;
            _brandRepository = brandRepository;
            _brandManager = brandManager;
            _businessPartnerRepository = businessPartnerRepository;
            _businesssPartnerManager = businesssPartnerManager;
            _abbPlanMasterManager = abbPlanMasterManager;
            _abbPlanMasterRepository = abbPlanMasterRepository;
            _abbPriceMasterManager = abbPriceMasterManager;
            _abbPriceMasterRepository = abbPriceMasterRepository;
            _modelNumberRepository = modelNumberRepository;
            _modelNumberManager = modelNumberManager;
            _priceMasterManager = priceMasterManager;
            _priceMasterRepository = priceMasterRepository;
            _servicePartnerManager = servicePartnerManager;
            _servicePartnerRepository = servicePartnerRepository;
            _exchangeOrderManager = exchangeOrderManager;
            _exchangeOrderRepository = exchangeOrderRepository;
            _abbRegistrationManager = abbRegistrationManager;
            _vehicleIncentiveManager = vehicleIncentiveManager;
            _vehicleIncentiveRepository = vehicleIncentiveRepository;
            _imageLabelMasterManager = imageLabelMasterManager;
            _context = context;
        }
        [BindProperty(SupportsGet = true)]
        public StateViewModel StateVM { get; set; }
        [BindProperty(SupportsGet = true)]
        public StateVMExcel StateVMExcel { get; set; }
        [BindProperty(SupportsGet = true)]
        public PinCodeViewModel PinCodeVM { get; set; }
        [BindProperty(SupportsGet = true)]
        public CityViewModel CityVM { get; set; }
        [BindProperty(SupportsGet = true)]
        public CityVMExcel CityVMExcel { get; set; }
        [BindProperty(SupportsGet = true)]
        public IList<TblCity> TblCity { get; set; }
        [BindProperty(SupportsGet = true)]
        public TblCity TblCityObj { get; set; }
        [BindProperty(SupportsGet = true)]
        public IList<TblState> TblState { get; set; }
        [BindProperty(SupportsGet = true)]
        public TblState TblStateObj { get; set; }
        [BindProperty(SupportsGet = true)]
        public IList<TblPinCode> TblPinCode { get; set; }
        [BindProperty(SupportsGet = true)]
        public TblPinCode TblPinCodeObj { get; set; }
        [BindProperty(SupportsGet = true)]
        public IList<TblProductCategory> TblProductCategory { get; set; }
        [BindProperty(SupportsGet = true)]
        public TblProductCategory TblProductCategoryObj { get; set; }
        [BindProperty(SupportsGet = true)]
        public ProductCategoryViewModel ProductCategoryVM { get; set; }
        [BindProperty(SupportsGet = true)]
        public ProductTypeViewModel ProductTypeVM { get; set; }
        [BindProperty(SupportsGet = true)]
        public TblProductType TblProductTypeObj { get; set; }
        [BindProperty(SupportsGet = true)]
        public BrandViewModel BrandVM { get; set; }
        [BindProperty(SupportsGet = true)]
        public IList<TblBrand> TblBrand { get; set; }
        [BindProperty(SupportsGet = true)]
        public TblBrand TblBrandObj { get; set; }
        [BindProperty(SupportsGet = true)]
        public BusinessPartnerViewModel BusinessPartnerVM { get; set; }
        [BindProperty(SupportsGet = true)]
        public TblBusinessPartner TblBusinessPartnerObj { get; set; }
        [BindProperty(SupportsGet = true)]
        public ABBPlanMasterViewModel ABBPlanMasterVM { get; set; }
        [BindProperty(SupportsGet = true)]
        public TblAbbplanMaster TblAbbplanMasterObj { get; set; }
        [BindProperty(SupportsGet = true)]
        public ABBPriceMasterViewModel ABBPriceMasterVM { get; set; }
        [BindProperty(SupportsGet = true)]
        public TblAbbpriceMaster TblAbbpriceMasterObj { get; set; }
        [BindProperty(SupportsGet = true)]
        public ModelNumberViewModel ModelNumberVM { get; set; }
        [BindProperty(SupportsGet = true)]
        public TblModelNumber TblModelNumberObj { get; set; }
        [BindProperty(SupportsGet = true)]
        public PriceMasterViewModel PriceMasterVM { get; set; }
        public TblPriceMaster TblPriceMasterObj { get; set; }
        [BindProperty(SupportsGet = true)]
        public ExchangeOrderViewModel ExchangeVM { get; set; }
        public TblExchangeOrder TblExchangeOrderObj { get; set; }
        [BindProperty(SupportsGet = true)]
        public ABBRegistrationViewModel AbbVM { get; set; }
        public TblAbbregistration TblAbbregistrationObj { get; set; }
        [BindProperty(SupportsGet = true)]
        public ServicePartnerViewModel ServicePartnerVM { get; set; }
        public TblServicePartner TblServicePartnerObj { get; set; }
        [BindProperty(SupportsGet = true)]
        public VehicleIncentiveViewModel VehicleIncentiveVM { get; set; }
        [BindProperty(SupportsGet = true)]
        public TblVehicleIncentive TblVehicleIncentiveObj { get; set; }
        [BindProperty(SupportsGet = true)]
        public TblImageLabelMaster TblImageLabelMasterObj { get; set; }
        [BindProperty(SupportsGet = true)]
        public ImageLabelNewViewModel ImageLabelVM { get; set; }

        #region Export Data To Excel 
        public FileResult OnPostExportExcel_DataError<T>(List<T> items, string filename)
        {
            bool flag = false;
            var Date = System.DateTime.Now;
            var dateTime = Date.ToString("MM-dd-yyyy_hh:mm");
            var FileName = filename + "_" + dateTime + ".xlsx";
            MemoryStream stream = ExcelExportHelper.ListExportToExcel(items);
            return (File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", FileName));
        }
        #endregion

        #region Excel Import for City
        public ActionResult OnPostCityExcelImport()
        {

            IFormFile ImportFile = null; FileResult fileResult = null;
            string replyMessage = string.Empty;

            if (CityVM != null && CityVM.UploadCity != null)
            {
                ImportFile = CityVM.UploadCity;
                CityVM.CityVMList = new List<CityVMExcel>();
            }
            try
            {
                if (ImportFile != null)
                {
                    // DataTable datatable = ExcelImportHelper.ExcelSheetReader(ImportFile);
                    CityVM.CityVMList = ExcelImportHelper.ExcelToList<CityVMExcel>(ImportFile, CityVM.CityVMList);
                    // Implement Model based Validations
                    if (CityVM.CityVMList != null && CityVM.CityVMList.Count > 0)
                    {
                        foreach (var item in CityVM.CityVMList)
                        {
                            var Check = _CityRepository.GetSingle(x => x.Name.Trim().ToLower() == item.Name.Trim().ToLower());
                            if (Check != null)
                            {
                                item.Remarks += "City is already exists." + Environment.NewLine;
                            }
                            var Check1 = _CityRepository.GetSingle(x => x.CityCode.Trim().ToLower() == item.CityCode.Trim().ToLower());
                            if (Check1 != null)
                            {
                                item.Remarks += "City code is already exists." + Environment.NewLine;
                            }

                            // Validate the model
                            var results = new List<ValidationResult>();
                            var context = new ValidationContext(item);
                            if (!Validator.TryValidateObject(item, context, results, true))
                            {
                                var errorMessages = string.Join(Environment.NewLine, results.Select(r => r.ErrorMessage));
                                item.Remarks += errorMessages + Environment.NewLine;
                            }
                        }
                    }
                }

                if (CityVM.CityVMList != null && CityVM.CityVMList.Count > 0)
                {
                    CityVM = _CityManager.ManageCityBulk(CityVM, _loginSession.UserViewModel.UserId);
                    if (CityVM != null)
                    {
                        if (CityVM.CityVMErrorList != null && CityVM.CityVMErrorList.Count > 0)
                        {
                            fileResult = OnPostExportExcel_DataError(CityVM.CityVMErrorList, "CityVMErrorList");
                            replyMessage = CityVM.CityVMList.Count + " Records Import Sucessfully and " + CityVM.CityVMErrorList.Count + " Records Not Import";
                        }
                        else
                        {
                            replyMessage = "All Records Import Sucessfully";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
            if (fileResult != null)
            {
                return fileResult;
            }
            else
            {
                return RedirectToPage("./LOV_Utility", new { id = _protector.Encode(CityVM.CityId) });
            }
        }

        #endregion
        public IActionResult OnPostDeleteAsync()
        {
            if (_loginSession == null)
            {
                return RedirectToPage("/ManageLOVUtility");
            }
            else
            {
                if (TblCityObj != null && TblCityObj.CityId > 0)
                {
                    TblCityObj = _context.TblCities.Find(TblCityObj.CityId);
                }

                if (TblCityObj != null)
                {
                    if (TblCityObj.IsActive == true)
                    {
                        TblCityObj.IsActive = false;
                    }
                    else
                    {
                        TblCityObj.IsActive = true;
                    }
                    TblCityObj.ModifiedBy = _loginSession.UserViewModel.UserId;
                    _context.TblCities.Update(TblCityObj);
                    //  _context.TblRoles.Remove(TblRole);
                    _context.SaveChanges();
                }

                return RedirectToPage("./LOV_Utility");
            }
        }

      

        #region Excel Import for State
        public ActionResult OnPostStateExcelImport()
        {

            IFormFile ImportFile = null; FileResult fileResult = null;
            string replyMessage = string.Empty;
            var stateVMList = new List<StateVMExcel>();
            if (StateVM != null && StateVM.UploadState != null)
            {
                ImportFile = StateVM.UploadState;
                StateVM.StateVMList = new List<StateVMExcel>();
            }
            try
            {
                if (ImportFile != null)
                {
                    // DataTable datatable = ExcelImportHelper.ExcelSheetReader(ImportFile);
                    StateVM.StateVMList = ExcelImportHelper.ExcelToList<StateVMExcel>(ImportFile, StateVM.StateVMList);
                    // Implement Model based Validations
                    if (StateVM.StateVMList != null && StateVM.StateVMList.Count > 0)
                    {
                        foreach (var item in StateVM.StateVMList)
                        {
                            var Check = _StateRepository.GetSingle(x => x.Name.Trim().ToLower() == item.Name.Trim().ToLower());
                            if (Check != null)
                            {
                                item.Remarks += "State is already exists." + Environment.NewLine;
                            }
                            var Check1 = _StateRepository.GetSingle(x => x.Code.Trim().ToLower() == item.Code.Trim().ToLower());
                            if (Check1 != null)
                            {
                                item.Remarks += "Code is already exists." + Environment.NewLine;
                            }

                            // Validate the model
                            var results = new List<ValidationResult>();
                            var context = new ValidationContext(item);
                            if (!Validator.TryValidateObject(item, context, results, true))
                            {
                                var errorMessages = string.Join(Environment.NewLine, results.Select(r => r.ErrorMessage));
                                item.Remarks += errorMessages + Environment.NewLine;
                            }
                        }
                    }
                }

                if (StateVM.StateVMList != null && StateVM.StateVMList.Count > 0)
                {
                    StateVM = _StateManager.ManageStateBulk(StateVM, _loginSession.UserViewModel.UserId);
                    if (StateVM != null)
                    {
                        if (StateVM.StateVMErrorList != null && StateVM.StateVMErrorList.Count > 0)
                        {
                            fileResult = OnPostExportExcel_DataError(StateVM.StateVMErrorList, "StateVMErrorList");
                            replyMessage = StateVM.StateVMList.Count + " Records Import Sucessfully and " + StateVM.StateVMErrorList.Count + " Records Not Import";
                        }
                        else
                        {
                            replyMessage = "All Records Import Sucessfully";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
            if (fileResult != null)
            {
                return fileResult;
            }
            else
            {
                return RedirectToPage("./LOV_Utility", new { id = _protector.Encode(StateVM.StateId) });
            }
        }

        #endregion

        public IActionResult OnPostStateDeleteAsync()
        {
            if (_loginSession == null)
            {
                return RedirectToPage("/ManageLOVUtility");
            }
            else
            {
                if (TblStateObj != null && TblStateObj.StateId > 0)
                {
                    TblStateObj = _context.TblStates.Find(TblStateObj.StateId);
                }

                if (TblStateObj != null)
                {
                    if (TblStateObj.IsActive == true)
                    {
                        TblStateObj.IsActive = false;
                    }
                    else
                    {
                        TblStateObj.IsActive = true;
                    }
                    TblStateObj.ModifiedBy = _loginSession.UserViewModel.UserId;
                    _context.TblStates.Update(TblStateObj);
                    //  _context.TblRoles.Remove(TblRole);
                    _context.SaveChanges();
                }

                return RedirectToPage("./LOV_Utility");
            }
        }


        #region Excel Import for PinCode
        public ActionResult OnPostPinCodeExcelImport()
        {

            IFormFile ImportFile = null; FileResult fileResult = null;
            string replyMessage = string.Empty;

            if (PinCodeVM != null && PinCodeVM.UploadPinCode != null)
            {
                ImportFile = PinCodeVM.UploadPinCode;
                PinCodeVM.PinCodeVMList = new List<PinCodeVMExcel>();
            }
            try
            {
                if (ImportFile != null)
                {
                    // DataTable datatable = ExcelImportHelper.ExcelSheetReader(ImportFile);
                    PinCodeVM.PinCodeVMList = ExcelImportHelper.ExcelToList<PinCodeVMExcel>(ImportFile, PinCodeVM.PinCodeVMList);
                    // Implement Model based Validations

                    if (PinCodeVM.PinCodeVMList != null && PinCodeVM.PinCodeVMList.Count > 0)
                    {
                        foreach (var item in PinCodeVM.PinCodeVMList)
                        {
                            var Check = _PinCodeRepository.GetSingle(x => x.ZipCode == item.PinCode);
                            if (Check != null)
                            {
                                item.Remarks += "Pin Code is already exists." + Environment.NewLine;
                            }


                            // Validate the model
                            var results = new List<ValidationResult>();
                            var context = new ValidationContext(item);
                            if (!Validator.TryValidateObject(item, context, results, true))
                            {
                                var errorMessages = string.Join(Environment.NewLine, results.Select(r => r.ErrorMessage));
                                item.Remarks += errorMessages + Environment.NewLine;
                            }
                        }
                    }
                }

                if (PinCodeVM.PinCodeVMList != null && PinCodeVM.PinCodeVMList.Count > 0)
                {
                    PinCodeVM = _PinCodeManager.ManagePinCodeBulk(PinCodeVM, _loginSession.UserViewModel.UserId);
                    if (PinCodeVM != null)
                    {
                        if (PinCodeVM.PinCodeVMErrorList != null && PinCodeVM.PinCodeVMErrorList.Count > 0)
                        {
                            fileResult = OnPostExportExcel_DataError(PinCodeVM.PinCodeVMErrorList, "PinCodeVMErrorList");
                            replyMessage = PinCodeVM.PinCodeVMList.Count + " Records Import Sucessfully and " + PinCodeVM.PinCodeVMErrorList.Count + " Records Not Import";
                        }
                        else
                        {
                            replyMessage = "All Records Import Sucessfully";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
            if (fileResult != null)
            {
                return fileResult;
            }
            else
            {
                return RedirectToPage("./LOV_Utility", new { id = _protector.Encode(PinCodeVM.Id) });
            }
        }

        #endregion


        public IActionResult OnPostPinCodeDeleteAsync()
        {
            if (_loginSession == null)
            {
                return RedirectToPage("/ManageLOVUtility");
            }
            else
            {
                if (TblPinCodeObj != null && TblPinCodeObj.Id > 0)
                {
                    TblPinCodeObj = _context.TblPinCodes.Find(TblPinCodeObj.Id);
                }

                if (TblPinCodeObj != null)
                {
                    if (TblPinCodeObj.IsActive == true)
                    {
                        TblPinCodeObj.IsActive = false;
                    }
                    else
                    {
                        TblPinCodeObj.IsActive = true;
                    }
                    TblPinCodeObj.ModifiedBy = _loginSession.UserViewModel.UserId;
                    _context.TblPinCodes.Update(TblPinCodeObj);
                    //  _context.TblRoles.Remove(TblRole);
                    _context.SaveChanges();
                }

                return RedirectToPage("./LOV_Utility");
            }
        }


        #region Excel Import for Product Category
        public ActionResult OnPostProductCategoryExcelImport()
        {

            IFormFile ImportFile = null; FileResult fileResult = null;
            string replyMessage = string.Empty;

            if (ProductCategoryVM != null && ProductCategoryVM.UploadProductCategory != null)
            {
                ImportFile = ProductCategoryVM.UploadProductCategory;
                ProductCategoryVM.ProductCategoryVMList = new List<ProductCategoryVMExcel>();
            }
            try
            {
                if (ImportFile != null)
                {
                    // DataTable datatable = ExcelImportHelper.ExcelSheetReader(ImportFile);
                    ProductCategoryVM.ProductCategoryVMList = ExcelImportHelper.ExcelToList<ProductCategoryVMExcel>(ImportFile, ProductCategoryVM.ProductCategoryVMList);
                    // Implement Model based Validations

                    if (ProductCategoryVM.ProductCategoryVMList != null && ProductCategoryVM.ProductCategoryVMList.Count > 0)
                    {
                        foreach (var item in ProductCategoryVM.ProductCategoryVMList)
                        {
                            var Check = _productCategoryRepository.GetSingle(x => x.Name == item.Name);
                            if (Check != null)
                            {
                                item.Remarks += "Product category is already exists." + Environment.NewLine;
                            }


                            // Validate the model
                            var results = new List<ValidationResult>();
                            var context = new ValidationContext(item);
                            if (!Validator.TryValidateObject(item, context, results, true))
                            {
                                var errorMessages = string.Join(Environment.NewLine, results.Select(r => r.ErrorMessage));
                                item.Remarks += errorMessages + Environment.NewLine;
                            }
                        }
                    }
                }

                if (ProductCategoryVM.ProductCategoryVMList != null && ProductCategoryVM.ProductCategoryVMList.Count > 0)
                {
                    ProductCategoryVM = _productCategoryManager.ManageProductCategoryBulk(ProductCategoryVM, _loginSession.UserViewModel.UserId);
                    if (ProductCategoryVM != null)
                    {
                        if (ProductCategoryVM.ProductCategoryVMErrorList != null && ProductCategoryVM.ProductCategoryVMErrorList.Count > 0)
                        {
                            fileResult = OnPostExportExcel_DataError(ProductCategoryVM.ProductCategoryVMErrorList, "ProductCategoryVMErrorList");
                            replyMessage = ProductCategoryVM.ProductCategoryVMErrorList.Count + " Records Import Sucessfully and " + ProductCategoryVM.ProductCategoryVMErrorList.Count + " Records Not Import";
                        }
                        else
                        {
                            replyMessage = "All Records Import Sucessfully";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
            if (fileResult != null)
            {
                return fileResult;
            }
            else
            {
                return RedirectToPage("./LOV_Utility", new { id = _protector.Encode(PinCodeVM.Id) });
            }
        }

        #endregion


        public IActionResult OnPostProductCategoryDeleteAsync()
        {
            if (_loginSession == null)
            {
                return RedirectToPage("/LOV_Utility");
            }
            
            else
            {
                if (TblProductCategoryObj != null && TblProductCategoryObj.Id > 0)
                {
                    TblProductCategoryObj = _context.TblProductCategories.Find(TblProductCategoryObj.Id);
                }

                if (TblProductCategoryObj != null)
                {
                    if (TblProductCategoryObj.IsActive == true)
                    {
                        TblProductCategoryObj.IsActive = false;
                    }
                    else
                    {
                        TblProductCategoryObj.IsActive = true;
                        
                    }
                    TblProductCategoryObj.ModifiedBy = _loginSession.UserViewModel.UserId;
                    _context.TblProductCategories.Update(TblProductCategoryObj);
                    //  _context.TblRoles.Remove(TblRole);
                    _context.SaveChanges();
                }

                return RedirectToPage("./LOV_Utility");
            }
        }

        #region Excel Import for Product Type
        public ActionResult OnPostProductTypeExcelImport()
        {

            IFormFile ImportFile = null; FileResult fileResult = null;
            string replyMessage = string.Empty;

            if (ProductTypeVM != null && ProductTypeVM.UploadProductType != null)
            {
                ImportFile = ProductTypeVM.UploadProductType;
                ProductTypeVM.ProductTypeVMList = new List<ProductTypeVMExcel>();
            }
            try
            {
                if (ImportFile != null)
                {
                    // DataTable datatable = ExcelImportHelper.ExcelSheetReader(ImportFile);
                    ProductTypeVM.ProductTypeVMList = ExcelImportHelper.ExcelToList<ProductTypeVMExcel>(ImportFile, ProductTypeVM.ProductTypeVMList);
                    // Implement Model based Validations

                    if (ProductTypeVM.ProductTypeVMList != null && ProductTypeVM.ProductTypeVMList.Count > 0)
                    {
                        foreach (var item in ProductTypeVM.ProductTypeVMList)
                        {
                            var Check = _productCategoryRepository.GetSingle(x => x.Name == item.Name);
                            if (Check != null)
                            {
                                item.Remarks += "Product type is already exists." + Environment.NewLine;
                            }


                            // Validate the model
                            var results = new List<ValidationResult>();
                            var context = new ValidationContext(item);
                            if (!Validator.TryValidateObject(item, context, results, true))
                            {
                                var errorMessages = string.Join(Environment.NewLine, results.Select(r => r.ErrorMessage));
                                item.Remarks += errorMessages + Environment.NewLine;
                            }
                        }
                    }
                }

                if (ProductTypeVM.ProductTypeVMList != null && ProductTypeVM.ProductTypeVMList.Count > 0)
                {
                    ProductTypeVM = _productTypeManager.ManageProductTypeBulk(ProductTypeVM, _loginSession.UserViewModel.UserId);
                    if (ProductTypeVM != null)
                    {
                        if (ProductTypeVM.ProductTypeVMErrorList != null && ProductTypeVM.ProductTypeVMErrorList.Count > 0)
                        {
                            fileResult = OnPostExportExcel_DataError(ProductTypeVM.ProductTypeVMErrorList, "ProductTypeVMErrorList");
                            replyMessage = ProductTypeVM.ProductTypeVMErrorList.Count + " Records Import Sucessfully and " + ProductTypeVM.ProductTypeVMErrorList.Count + " Records Not Import";
                        }
                        else
                        {
                            replyMessage = "All Records Import Sucessfully";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
            if (fileResult != null)
            {
                return fileResult;
            }
            else
            {
                return RedirectToPage("./LOV_Utility", new { id = _protector.Encode(ProductTypeVM.Id) });
            }
        }

        #endregion


        public IActionResult OnPostProductTypeDeleteAsync()
        {
            if (_loginSession == null)
            {
                return RedirectToPage("/LOV_Utility");
            }

            else
            {
                if (TblProductTypeObj != null && TblProductTypeObj.Id > 0)
                {
                    TblProductTypeObj = _context.TblProductTypes.Find(TblProductTypeObj.Id);
                }

                if (TblProductTypeObj != null)
                {
                    if (TblProductTypeObj.IsActive == true)
                    {
                        TblProductTypeObj.IsActive = false;
                    }
                    else
                    {
                        TblProductTypeObj.IsActive = true;

                    }
                    TblProductTypeObj.ModifiedBy = _loginSession.UserViewModel.UserId;
                    _context.TblProductTypes.Update(TblProductTypeObj);
                    //  _context.TblRoles.Remove(TblRole);
                    _context.SaveChanges();
                }

                return RedirectToPage("./LOV_Utility");
            }
        }

        #region Excel Import for Brand
        public ActionResult OnPostBrandExcelImport()
        {

            IFormFile ImportFile = null; FileResult fileResult = null;
            string replyMessage = string.Empty;

            if (BrandVM != null && BrandVM.UploadBrand != null)
            {
                ImportFile = BrandVM.UploadBrand;
                BrandVM.BrandVMList = new List<BrandVMExcel>();
            }
            try
            {
                if (ImportFile != null)
                {
                    // DataTable datatable = ExcelImportHelper.ExcelSheetReader(ImportFile);
                    BrandVM.BrandVMList = ExcelImportHelper.ExcelToList<BrandVMExcel>(ImportFile, BrandVM.BrandVMList);
                    // Implement Model based Validations

                    if (BrandVM.BrandVMList != null && BrandVM.BrandVMList.Count > 0)
                    {
                        foreach (var item in BrandVM.BrandVMList)
                        {
                            var Check = _brandRepository.GetSingle(x => x.Name == item.Name);
                            if (Check != null)
                            {
                                item.Remarks += "Brand is already exists." + Environment.NewLine;
                            }


                            // Validate the model
                            var results = new List<ValidationResult>();
                            var context = new ValidationContext(item);
                            if (!Validator.TryValidateObject(item, context, results, true))
                            {
                                var errorMessages = string.Join(Environment.NewLine, results.Select(r => r.ErrorMessage));
                                item.Remarks += errorMessages + Environment.NewLine;
                            }
                        }
                    }
                }

                if (BrandVM.BrandVMList != null && BrandVM.BrandVMList.Count > 0)
                {
                    BrandVM = _brandManager.ManageBrandBulk(BrandVM, _loginSession.UserViewModel.UserId);
                    if (BrandVM != null)
                    {
                        if (BrandVM.BrandVMErrorList != null && BrandVM.BrandVMErrorList.Count > 0)
                        {
                            fileResult = OnPostExportExcel_DataError(BrandVM.BrandVMErrorList, "BrandVMErrorList");
                            replyMessage = BrandVM.BrandVMErrorList.Count + " Records Import Sucessfully and " + BrandVM.BrandVMErrorList.Count + " Records Not Import";
                        }
                        else
                        {
                            replyMessage = "All Records Import Sucessfully";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
            if (fileResult != null)
            {
                return fileResult;
            }
            else
            {
                return RedirectToPage("./LOV_Utility", new { id = _protector.Encode(BrandVM.Id) });
            }
        }

        #endregion


        public IActionResult OnPostBrandDeleteAsync()
        {
            if (_loginSession == null)
            {
                return RedirectToPage("/LOV_Utility");
            }

            else
            {
                if (TblBrandObj != null && TblBrandObj.Id > 0)
                {
                    TblBrandObj = _context.TblBrands.Find(TblBrandObj.Id);
                }

                if (TblBrandObj != null)
                {
                    TblBrandObj.IsActive = false;
                    TblBrandObj.ModifiedBy = _loginSession.UserViewModel.UserId;
                    _context.TblBrands.Update(TblBrandObj);
                    //  _context.TblRoles.Remove(TblRole);
                    _context.SaveChanges();
                }

                return RedirectToPage("./LOV_Utility");
            }
        }


        #region Excel Import for Business Partner
        public ActionResult OnPostBusinessPartnerExcelImport()
        {

            IFormFile ImportFile = null; FileResult fileResult = null;
            string replyMessage = string.Empty;

            if (BusinessPartnerVM != null && BusinessPartnerVM.UploadBusinessPartner != null)
            {
                ImportFile = BusinessPartnerVM.UploadBusinessPartner;
                BusinessPartnerVM.BusinessPartnerVMList = new List<BusinessPartnerVMExcelModel>();
            }
            try
            {
                if (ImportFile != null)
                {
                    // DataTable datatable = ExcelImportHelper.ExcelSheetReader(ImportFile);
                    BusinessPartnerVM.BusinessPartnerVMList = ExcelImportHelper.ExcelToList<BusinessPartnerVMExcelModel>(ImportFile, BusinessPartnerVM.BusinessPartnerVMList);
                    // Implement Model based Validations

                    if (BusinessPartnerVM.BusinessPartnerVMList != null && BusinessPartnerVM.BusinessPartnerVMList.Count > 0)
                    {
                        foreach (var item in BusinessPartnerVM.BusinessPartnerVMList)
                        {
                            

                            // Validate the model
                            var results = new List<ValidationResult>();
                            var context = new ValidationContext(item);
                            if (!Validator.TryValidateObject(item, context, results, true))
                            {
                                var errorMessages = string.Join(Environment.NewLine, results.Select(r => r.ErrorMessage));
                                item.Remarks += errorMessages + Environment.NewLine;
                            }
                            List<string>? labels = new List<string> { item?.LabelName_Excellent_P, item?.LabelName_Good_Q, item?.LabelName_Average_R, item?.LabelName_NonWorking_S};
                            var checklabels = labels.Where(x=> !string.IsNullOrEmpty(x)).OrderBy(x => x.Length).ToList();

                            if(checklabels != null && checklabels.Count < 2)
                            {
                                var ErrorMsg = string.Join(Environment.NewLine, "You Need to add at least two product condition label");
                                item.Remarks += ErrorMsg + Environment.NewLine;
                            }                          
                        }
                    }
                }

                if (BusinessPartnerVM.BusinessPartnerVMList != null && BusinessPartnerVM.BusinessPartnerVMList.Count > 0)
                {
                    BusinessPartnerVM = _businesssPartnerManager.ManageBusinessPartnerBulk(BusinessPartnerVM, _loginSession.UserViewModel.UserId);
                    if (BusinessPartnerVM != null)
                    {
                        if (BusinessPartnerVM.BusinessPartnerVMErrorList != null && BusinessPartnerVM.BusinessPartnerVMErrorList.Count > 0)
                        {
                            fileResult = OnPostExportExcel_DataError(BusinessPartnerVM.BusinessPartnerVMErrorList, "BusinessPartnerVMErrorList");
                            replyMessage = BusinessPartnerVM.BusinessPartnerVMErrorList.Count + " Records Import Sucessfully and " + BusinessPartnerVM.BusinessPartnerVMErrorList.Count + " Records Not Import";
                        }
                        else
                        {
                            replyMessage = "All Records Import Sucessfully";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
            if (fileResult != null)
            {
                return fileResult;
            }
            else
            {
                return RedirectToPage("./LOV_Utility", new { id = _protector.Encode(BusinessPartnerVM.BusinessPartnerId) });
            }
        }

        #endregion


        public IActionResult OnPostBusinessPartnerDeleteAsync()
        {
            if (_loginSession == null)
            {
                return RedirectToPage("/LOV_Utility");
            }

            else
            {
                if (TblBusinessPartnerObj != null && TblBusinessPartnerObj.BusinessPartnerId > 0)
                {
                    TblBusinessPartnerObj = _context.TblBusinessPartners.Find(TblBusinessPartnerObj.BusinessPartnerId);
                }

                if (TblBusinessPartnerObj != null)
                {

                    TblBusinessPartnerObj.IsActive = false;
                    TblBusinessPartnerObj.ModifiedBy = _loginSession.UserViewModel.UserId;
                    _context.TblBusinessPartners.Update(TblBusinessPartnerObj);
                    _context.SaveChanges();
                }

                return RedirectToPage("./LOV_Utility");
            }
        }


        #region Excel Import for ABB Plan Master
        public ActionResult OnPostABBPlanMasterExcelImport()
        {

            IFormFile ImportFile = null; FileResult fileResult = null;
            string replyMessage = string.Empty;

            if (ABBPlanMasterVM != null && ABBPlanMasterVM.UploadABBPlanMaster != null)
            {
                ImportFile = ABBPlanMasterVM.UploadABBPlanMaster;
                ABBPlanMasterVM.ABBPlanMasterVMList = new List<ABBPlanMasterVMExcel>();
            }
            try
            {
                if (ImportFile != null)
                {
                    // DataTable datatable = ExcelImportHelper.ExcelSheetReader(ImportFile);
                    ABBPlanMasterVM.ABBPlanMasterVMList = ExcelImportHelper.ExcelToList<ABBPlanMasterVMExcel>(ImportFile, ABBPlanMasterVM.ABBPlanMasterVMList);
                    // Implement Model based Validations

                    if (ABBPlanMasterVM.ABBPlanMasterVMList != null && ABBPlanMasterVM.ABBPlanMasterVMList.Count > 0)
                    {
                        foreach (var item in ABBPlanMasterVM.ABBPlanMasterVMList)
                        {


                            // Validate the model
                            var results = new List<ValidationResult>();
                            var context = new ValidationContext(item);
                            if (!Validator.TryValidateObject(item, context, results, true))
                            {
                                var errorMessages = string.Join(Environment.NewLine, results.Select(r => r.ErrorMessage));
                                item.Remarks += errorMessages + Environment.NewLine;
                            }
                        }
                    }
                }

                if (ABBPlanMasterVM.ABBPlanMasterVMList != null && ABBPlanMasterVM.ABBPlanMasterVMList.Count > 0)
                {
                    ABBPlanMasterVM = _abbPlanMasterManager.ManageABBPlanMasterBulk(ABBPlanMasterVM, _loginSession.UserViewModel.UserId);
                    if (BusinessPartnerVM != null)
                    {
                        if (ABBPlanMasterVM.ABBPlanMasterVMErrorList != null && ABBPlanMasterVM.ABBPlanMasterVMErrorList.Count > 0)
                        {
                            fileResult = OnPostExportExcel_DataError(ABBPlanMasterVM.ABBPlanMasterVMErrorList, "ABBPlanMasterVMErrorList");
                            replyMessage = ABBPlanMasterVM.ABBPlanMasterVMErrorList.Count + " Records Import Sucessfully and " + ABBPlanMasterVM.ABBPlanMasterVMErrorList.Count + " Records Not Import";
                        }
                        else
                        {
                            replyMessage = "All Records Import Sucessfully";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
            if (fileResult != null)
            {
                return fileResult;
            }
            else
            {
                return RedirectToPage("./LOV_Utility", new { id = _protector.Encode(ABBPlanMasterVM.PlanMasterId) });
            }
        }

        #endregion


        public IActionResult OnPostABBPlanMasterDeleteAsync()
        {
            if (_loginSession == null)
            {
                return RedirectToPage("/LOV_Utility");
            }

            else
            {
                if (TblAbbplanMasterObj != null && TblAbbplanMasterObj.PlanMasterId > 0)
                {
                    TblAbbplanMasterObj = _context.TblAbbplanMasters.Find(TblAbbplanMasterObj.PlanMasterId);
                }

                if (TblAbbplanMasterObj != null)
                {
                    TblAbbplanMasterObj.IsActive = false;
                    TblAbbplanMasterObj.ModifiedBy = _loginSession.UserViewModel.UserId;
                    _context.TblAbbplanMasters.Update(TblAbbplanMasterObj);
                    //  _context.TblRoles.Remove(TblRole);
                    _context.SaveChanges();
                }

                return RedirectToPage("./LOV_Utility");
            }
        }


        #region Excel Import for ABB Price Master
        public ActionResult OnPostABBPriceMasterExcelImport()
        {

            IFormFile ImportFile = null; FileResult fileResult = null;
            string replyMessage = string.Empty;

            if (ABBPriceMasterVM != null && ABBPriceMasterVM.UploadABBPriceMaster != null)
            {
                ImportFile = ABBPriceMasterVM.UploadABBPriceMaster;
                ABBPriceMasterVM.ABBPriceMasterVMList = new List<ABBPriceMasterVMExcel>();
            }
            try
            {
                if (ImportFile != null)
                {
                    // DataTable datatable = ExcelImportHelper.ExcelSheetReader(ImportFile);
                    ABBPriceMasterVM.ABBPriceMasterVMList = ExcelImportHelper.ExcelToList<ABBPriceMasterVMExcel>(ImportFile, ABBPriceMasterVM.ABBPriceMasterVMList);
                    // Implement Model based Validations

                    if (ABBPriceMasterVM.ABBPriceMasterVMList != null && ABBPriceMasterVM.ABBPriceMasterVMList.Count > 0)
                    {
                        foreach (var item in ABBPriceMasterVM.ABBPriceMasterVMList)
                        {


                            // Validate the model
                            var results = new List<ValidationResult>();
                            var context = new ValidationContext(item);
                            if (!Validator.TryValidateObject(item, context, results, true))
                            {
                                var errorMessages = string.Join(Environment.NewLine, results.Select(r => r.ErrorMessage));
                                item.Remarks += errorMessages + Environment.NewLine;
                            }
                        }
                    }
                }

                if (ABBPriceMasterVM.ABBPriceMasterVMList != null && ABBPriceMasterVM.ABBPriceMasterVMList.Count > 0)
                {
                    ABBPriceMasterVM = _abbPriceMasterManager.ManageABBPriceMasterBulk(ABBPriceMasterVM, _loginSession.UserViewModel.UserId);
                    if (ABBPriceMasterVM != null)
                    {
                        if (ABBPriceMasterVM.ABBPriceMasterVMErrorList != null && ABBPriceMasterVM.ABBPriceMasterVMErrorList.Count > 0)
                        {
                            fileResult = OnPostExportExcel_DataError(ABBPriceMasterVM.ABBPriceMasterVMErrorList, "ABBPriceMasterVMErrorList");
                            replyMessage = ABBPriceMasterVM.ABBPriceMasterVMErrorList.Count + " Records Import Sucessfully and " + ABBPriceMasterVM.ABBPriceMasterVMErrorList.Count + " Records Not Import";
                        }
                        else
                        {
                            replyMessage = "All Records Import Sucessfully";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
            if (fileResult != null)
            {
                return fileResult;
            }
            else
            {
                return RedirectToPage("./LOV_Utility", new { id = _protector.Encode(ABBPriceMasterVM.PriceMasterId) });
            }
        }

        #endregion


        public IActionResult OnPostABBPriceMasterDeleteAsync()
        {
            if (_loginSession == null)
            {
                return RedirectToPage("/LOV_Utility");
            }

            else
            {
                if (TblAbbpriceMasterObj != null && TblAbbpriceMasterObj.PriceMasterId > 0)
                {
                    TblAbbpriceMasterObj = _context.TblAbbpriceMasters.Find(TblAbbpriceMasterObj.PriceMasterId);
                }

                if (TblAbbpriceMasterObj != null)
                {


                    TblAbbpriceMasterObj.IsActive = false;
                    TblAbbpriceMasterObj.ModifiedBy = _loginSession.UserViewModel.UserId;
                    _context.TblAbbpriceMasters.Update(TblAbbpriceMasterObj);
                    //  _context.TblRoles.Remove(TblRole);
                    _context.SaveChanges();
                }

                return RedirectToPage("./LOV_Utility");
            }
        }

        #region Excel Import for Model Number
        public ActionResult OnPostModelNumberExcelImport()
        {

            IFormFile ImportFile = null; FileResult fileResult = null;
            string replyMessage = string.Empty;

            if (ModelNumberVM != null && ModelNumberVM.UploadModelNumber != null)
            {
                ImportFile = ModelNumberVM.UploadModelNumber;
                ModelNumberVM.ModelNumberVMList = new List<ModelNumberVMExcel>();
            }
            try
            {
                if (ImportFile != null)
                {
                    // DataTable datatable = ExcelImportHelper.ExcelSheetReader(ImportFile);
                    ModelNumberVM.ModelNumberVMList = ExcelImportHelper.ExcelToList<ModelNumberVMExcel>(ImportFile, ModelNumberVM.ModelNumberVMList);
                    // Implement Model based Validations

                    if (ModelNumberVM.ModelNumberVMList != null && ModelNumberVM.ModelNumberVMList.Count > 0)
                    {
                        foreach (var item in ModelNumberVM.ModelNumberVMList)
                        {
                            var Check = _modelNumberRepository.GetSingle(x => x.ModelName == item.ModelName);
                            if (Check != null)
                            {
                                item.Remarks += "Model Number is already exists." + Environment.NewLine;
                            }


                            // Validate the model
                            var results = new List<ValidationResult>();
                            var context = new ValidationContext(item);
                            if (!Validator.TryValidateObject(item, context, results, true))
                            {
                                var errorMessages = string.Join(Environment.NewLine, results.Select(r => r.ErrorMessage));
                                item.Remarks += errorMessages + Environment.NewLine;
                            }
                        }
                    }
                }

                if (ModelNumberVM.ModelNumberVMList != null && ModelNumberVM.ModelNumberVMList.Count > 0)
                {
                    ModelNumberVM = _modelNumberManager.ManageModelNumberBulk(ModelNumberVM, _loginSession.UserViewModel.UserId);
                    if (ABBPriceMasterVM != null)
                    {
                        if (ModelNumberVM.ModelNumberVMErrorList != null && ModelNumberVM.ModelNumberVMErrorList.Count > 0)
                        {
                            fileResult = OnPostExportExcel_DataError(ModelNumberVM.ModelNumberVMErrorList, "ModelNumberVMErrorList");
                            replyMessage = ModelNumberVM.ModelNumberVMErrorList.Count + " Records Import Sucessfully and " + ModelNumberVM.ModelNumberVMErrorList.Count + " Records Not Import";
                        }
                        else
                        {
                            replyMessage = "All Records Import Sucessfully";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
            if (fileResult != null)
            {
                return fileResult;
            }
            else
            {
                return RedirectToPage("./LOV_Utility", new { id = _protector.Encode(ModelNumberVM.ModelNumberId) });
            }
        }

        #endregion


        public IActionResult OnPostModelNumberDeleteAsync()
        {
            if (_loginSession == null)
            {
                return RedirectToPage("/LOV_Utility");
            }

            else
            {
                if (TblModelNumberObj != null && TblModelNumberObj.ModelNumberId > 0)
                {
                    TblModelNumberObj = _context.TblModelNumbers.Find(TblModelNumberObj.ModelNumberId);
                }

                if (TblModelNumberObj != null)
                {
                    TblModelNumberObj.IsActive = false;
                    TblModelNumberObj.ModifiedBy = _loginSession.UserViewModel.UserId;
                    _context.TblModelNumbers.Update(TblModelNumberObj);
                    //  _context.TblRoles.Remove(TblRole);
                    _context.SaveChanges();
                }

                return RedirectToPage("./LOV_Utility");
            }
        }

        #region Excel Import for Price Master
        public ActionResult OnPostPriceMasterExcelImport()
        {

            IFormFile ImportFile = null; FileResult fileResult = null;
            string replyMessage = string.Empty;

            if (PriceMasterVM != null && PriceMasterVM.UploadPriceMaster != null)
            {
                ImportFile = PriceMasterVM.UploadPriceMaster;
                PriceMasterVM.PriceMasterVMList = new List<PriceMasterExcel>();
            }
            try
            {
                if (ImportFile != null)
                {
                    // DataTable datatable = ExcelImportHelper.ExcelSheetReader(ImportFile);
                    PriceMasterVM.PriceMasterVMList = ExcelImportHelper.ExcelToList<PriceMasterExcel>(ImportFile, PriceMasterVM.PriceMasterVMList);
                    // Implement Model based Validations

                    if (PriceMasterVM.PriceMasterVMList != null && PriceMasterVM.PriceMasterVMList.Count > 0)
                    {
                        foreach (var item in PriceMasterVM.PriceMasterVMList)
                        {
                            if (item.BrandName3 == item.BrandName4 || item.BrandName1 == item.BrandName4 || item.BrandName2 == item.BrandName4 || item.BrandName2 == item.BrandName3 || item.BrandName1 == item.BrandName3 || item.BrandName1 == item.BrandName2)
                            {
                                item.Remarks += "Brands are should be different to each other" + Environment.NewLine;

                            }


                            // Validate the model
                            var results = new List<ValidationResult>();
                            var context = new ValidationContext(item);
                            if (!Validator.TryValidateObject(item, context, results, true))
                            {
                                var errorMessages = string.Join(Environment.NewLine, results.Select(r => r.ErrorMessage));
                                item.Remarks += errorMessages + Environment.NewLine;
                            }
                        }
                    }
                }

                if (PriceMasterVM.PriceMasterVMList != null && PriceMasterVM.PriceMasterVMList.Count > 0)
                {
                    PriceMasterVM = _priceMasterManager.ManagePriceMasterBulk(PriceMasterVM, _loginSession.UserViewModel.UserId);
                    if (PriceMasterVM != null)
                    {
                        if (PriceMasterVM.PriceMasterVMErrorList != null && PriceMasterVM.PriceMasterVMErrorList.Count > 0)
                        {
                            fileResult = OnPostExportExcel_DataError(PriceMasterVM.PriceMasterVMErrorList, "PriceMasterVMErrorList");
                            replyMessage = PriceMasterVM.PriceMasterVMErrorList.Count + " Records Import Sucessfully and " + PriceMasterVM.PriceMasterVMErrorList.Count + " Records Not Import";
                        }
                        else
                        {
                            replyMessage = "All Records Import Sucessfully";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
            if (fileResult != null)
            {
                return fileResult;
            }
            else
            {
                return RedirectToPage("./LOV_Utility", new { id = _protector.Encode(PriceMasterVM.Id) });
            }
        }

        #endregion


        public IActionResult OnPostPriceMasterDeleteAsync()
        {
            if (_loginSession == null)
            {
                return RedirectToPage("/LOV_Utility");
            }

            else
            {
                if (TblPriceMasterObj != null && TblPriceMasterObj.Id > 0)
                {
                    TblPriceMasterObj = _context.TblPriceMasters.Find(TblPriceMasterObj.Id);
                }

                if (TblPriceMasterObj != null)
                {
                    TblPriceMasterObj.IsActive = false;
                    TblPriceMasterObj.ModifiedBy = _loginSession.UserViewModel.UserId;
                    _context.TblPriceMasters.Update(TblPriceMasterObj);
                    //  _context.TblRoles.Remove(TblRole);
                    _context.SaveChanges();
                }

                return RedirectToPage("./LOV_Utility");
            }
        }

        #region Excel Import for Exchange
        public ActionResult OnPostExchangeExcelImport()
        {

            IFormFile ImportFile = null; FileResult fileResult = null;
            string replyMessage = string.Empty;

            if (ExchangeVM != null && ExchangeVM.UploadExchange != null)
            {
                ImportFile = ExchangeVM.UploadExchange;
                ExchangeVM.ExchangeVMList = new List<ExchangeVMExcel>();
            }
            try
            {
                if (ImportFile != null)
                {
                    // DataTable datatable = ExcelImportHelper.ExcelSheetReader(ImportFile);
                    ExchangeVM.ExchangeVMList = ExcelImportHelper.ExcelToList<ExchangeVMExcel>(ImportFile, ExchangeVM.ExchangeVMList);
                    // Implement Model based Validations

                    if (ExchangeVM.ExchangeVMList != null && ExchangeVM.ExchangeVMList.Count > 0)
                    {
                        foreach (var item in ExchangeVM.ExchangeVMList)
                        {

                            // Validate the model
                            var results = new List<ValidationResult>();
                            var context = new ValidationContext(item);
                            if (!Validator.TryValidateObject(item, context, results, true))
                            {
                                var errorMessages = string.Join(Environment.NewLine, results.Select(r => r.ErrorMessage));
                                item.Remarks += errorMessages + Environment.NewLine;
                            }
                        }
                    }
                }

                if (ExchangeVM.ExchangeVMList != null && ExchangeVM.ExchangeVMList.Count > 0)
                {
                    ExchangeVM = _exchangeOrderManager.ManageExchangeOrderBulk(ExchangeVM, _loginSession.UserViewModel.UserId);
                    if (ExchangeVM != null)
                    {
                        if (ExchangeVM.ExchangeVMErrorList != null && ExchangeVM.ExchangeVMErrorList.Count > 0)
                        {
                            fileResult = OnPostExportExcel_DataError(ExchangeVM.ExchangeVMErrorList, "PriceMasterVMErrorList");
                            replyMessage = ExchangeVM.ExchangeVMErrorList.Count + " Records Import Sucessfully and " + ExchangeVM.ExchangeVMErrorList.Count + " Records Not Import";
                        }
                        else
                        {
                            replyMessage = "All Records Import Sucessfully";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
            if (fileResult != null)
            {
                return fileResult;
            }
            else
            {
                return RedirectToPage("./LOV_Utility", new { id = _protector.Encode(ExchangeVM.Id) });
            }
        }

        #endregion


      

        #region Excel Import for ABB
        public ActionResult OnPostABBExcelImport()
        {

            IFormFile ImportFile = null; FileResult fileResult = null;
            string replyMessage = string.Empty;

            if (AbbVM != null && AbbVM.UploadAbb != null)
            {
                ImportFile = AbbVM.UploadAbb;
                AbbVM.AbbVMList = new List<AbbRegistrationVMExcel>();
            }
            try
            {
                if (ImportFile != null)
                {
                    // DataTable datatable = ExcelImportHelper.ExcelSheetReader(ImportFile);
                    AbbVM.AbbVMList = ExcelImportHelper.ExcelToList<AbbRegistrationVMExcel>(ImportFile, AbbVM.AbbVMList);
                    // Implement Model based Validations

                    if (AbbVM.AbbVMList != null && AbbVM.AbbVMList.Count > 0)
                    {
                        foreach (var item in AbbVM.AbbVMList)
                        {

                            // Validate the model
                            var results = new List<ValidationResult>();
                            var context = new ValidationContext(item);
                            if (!Validator.TryValidateObject(item, context, results, true))
                            {
                                var errorMessages = string.Join(Environment.NewLine, results.Select(r => r.ErrorMessage));
                                item.Remarks += errorMessages + Environment.NewLine;
                            }
                        }
                    }
                }

                if (AbbVM.AbbVMList != null && AbbVM.AbbVMList.Count > 0)
                {
                    AbbVM = _abbRegistrationManager.ManageABBBulk(AbbVM, _loginSession.UserViewModel.UserId);
                    if (AbbVM != null)
                    {
                        if (AbbVM.AbbVMErrorList != null && AbbVM.AbbVMErrorList.Count > 0)
                        {
                            fileResult = OnPostExportExcel_DataError(AbbVM.AbbVMErrorList, "PriceMasterVMErrorList");
                            replyMessage = AbbVM.AbbVMErrorList.Count + " Records Import Sucessfully and " + AbbVM.AbbVMErrorList.Count + " Records Not Import";
                        }
                        else
                        {
                            replyMessage = "All Records Import Sucessfully";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
            if (fileResult != null)
            {
                return fileResult;
            }
            else
            {
                return RedirectToPage("./LOV_Utility", new { id = _protector.Encode(AbbVM.ABBRegistrationId) });
            }
        }

        #endregion


        public IActionResult OnPostABBDeleteAsync()
        {
            if (_loginSession == null)
            {
                return RedirectToPage("/LOV_Utility");
            }

            else
            {
                if (TblAbbregistrationObj != null && TblAbbregistrationObj.AbbregistrationId > 0)
                {
                    TblAbbregistrationObj = _context.TblAbbregistrations.Find(TblAbbregistrationObj.AbbregistrationId);
                }

                if (TblAbbregistrationObj != null)
                {
                    TblAbbregistrationObj.IsActive = false;
                    TblAbbregistrationObj.ModifiedBy = _loginSession.UserViewModel.UserId;
                    _context.TblAbbregistrations.Update(TblAbbregistrationObj);

                    _context.SaveChanges();
                }

                return RedirectToPage("./LOV_Utility");
            }
        }

        #region Excel Import for Service Partner
        public ActionResult OnPostServicePartnerExcelImport()
        {

            IFormFile ImportFile = null; FileResult fileResult = null;
            string replyMessage = string.Empty;

            if (ServicePartnerVM != null && ServicePartnerVM.UploadServicePartner != null)
            {
                ImportFile = ServicePartnerVM.UploadServicePartner;
                ServicePartnerVM.ServicePartnerVMList = new List<ServicePartnerVMExcel>();
            }
            try
            {
                if (ImportFile != null)
                {
                    // DataTable datatable = ExcelImportHelper.ExcelSheetReader(ImportFile);
                    ServicePartnerVM.ServicePartnerVMList = ExcelImportHelper.ExcelToList<ServicePartnerVMExcel>(ImportFile, ServicePartnerVM.ServicePartnerVMList);
                    // Implement Model based Validations

                    if (ServicePartnerVM.ServicePartnerVMList != null && ServicePartnerVM.ServicePartnerVMList.Count > 0)
                    {
                        foreach (var item in ServicePartnerVM.ServicePartnerVMList)
                        {
                            var Check = _servicePartnerRepository.GetSingle(x => x.ServicePartnerName == item.ServicePartnerName);
                            if (Check != null)
                            {
                                item.Remarks += "This Name is already exists." + Environment.NewLine;
                            }
                            var Check1 = _servicePartnerRepository.GetSingle(x => x.ServicePartnerMobileNumber == item.ServicePartnerMobileNumber);
                            if (Check1 != null)
                            {
                                item.Remarks += "This Mobile Number is already exists." + Environment.NewLine;
                            }
                            var Check2 = _servicePartnerRepository.GetSingle(x => x.ServicePartnerEmailId == item.ServicePartnerEmailId);
                            if (Check2 != null)
                            {
                                item.Remarks += "This Email is already exists." + Environment.NewLine;
                            }
                            // Validate the model
                            var results = new List<ValidationResult>();
                            var context = new ValidationContext(item);
                            if (!Validator.TryValidateObject(item, context, results, true))
                            {
                                var errorMessages = string.Join(Environment.NewLine, results.Select(r => r.ErrorMessage));
                                item.Remarks += errorMessages + Environment.NewLine;
                            }
                        }
                    }
                }

                if (ServicePartnerVM.ServicePartnerVMList != null && ServicePartnerVM.ServicePartnerVMList.Count > 0)
                {
                    ServicePartnerVM = _servicePartnerManager.ManageSevicePartnerBulk(ServicePartnerVM, _loginSession.UserViewModel.UserId);
                    if (ServicePartnerVM != null)
                    {
                        if (ServicePartnerVM.ServicePartnerVMErrorList != null && ServicePartnerVM.ServicePartnerVMErrorList.Count > 0)
                        {
                            fileResult = OnPostExportExcel_DataError(ServicePartnerVM.ServicePartnerVMList, "ServicePartnerVMErrorList");
                            replyMessage = ServicePartnerVM.ServicePartnerVMErrorList.Count + " Records Import Sucessfully and " + ServicePartnerVM.ServicePartnerVMErrorList.Count + " Records Not Import";
                        }
                        else
                        {
                            replyMessage = "All Records Import Sucessfully";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
            if (fileResult != null)
            {
                return fileResult;
            }
            else
            {
                return RedirectToPage("./LOV_Utility", new { id = _protector.Encode(ServicePartnerVM.ServicePartnerId) });
            }
        }

        #endregion


        public IActionResult OnPostServicePartnerDeleteAsync()
        {
            if (_loginSession == null)
            {
                return RedirectToPage("/LOV_Utility");
            }

            else
            {
                if (TblServicePartnerObj != null && TblServicePartnerObj.ServicePartnerId > 0)
                {
                    TblServicePartnerObj = _context.TblServicePartners.Find(TblServicePartnerObj.ServicePartnerId);
                }

                if (TblServicePartnerObj != null)
                {
                    TblServicePartnerObj.IsActive = false;
                    TblServicePartnerObj.Modifiedby = _loginSession.UserViewModel.UserId;
                    _context.TblServicePartners.Update(TblServicePartnerObj);

                    _context.SaveChanges();
                }

                return RedirectToPage("./LOV_Utility");
            }
        }


        #region Excel Import for Vehicle Incentive
        public ActionResult OnPostVehicleIncentiveExcelImport()
        {

            IFormFile ImportFile = null; FileResult fileResult = null;
            string replyMessage = string.Empty;

            if (VehicleIncentiveVM != null && VehicleIncentiveVM.UploadVehicleIncentive != null)
            {
                ImportFile = VehicleIncentiveVM.UploadVehicleIncentive;
                VehicleIncentiveVM.VehicleIncentiveVMList = new List<VehicleIncentiveVMExcel>();
            }
            try
            {
                if (ImportFile != null)
                {
                    // DataTable datatable = ExcelImportHelper.ExcelSheetReader(ImportFile);
                    VehicleIncentiveVM.VehicleIncentiveVMList = ExcelImportHelper.ExcelToList<VehicleIncentiveVMExcel>(ImportFile, VehicleIncentiveVM.VehicleIncentiveVMList);
                    // Implement Model based Validations

                    if (VehicleIncentiveVM.VehicleIncentiveVMList != null && VehicleIncentiveVM.VehicleIncentiveVMList.Count > 0)
                    {
                        foreach (var item in VehicleIncentiveVM.VehicleIncentiveVMList)
                        {
                            
                            // Validate the model
                            var results = new List<ValidationResult>();
                            var context = new ValidationContext(item);
                            if (!Validator.TryValidateObject(item, context, results, true))
                            {
                                var errorMessages = string.Join(Environment.NewLine, results.Select(r => r.ErrorMessage));
                                item.Remarks += errorMessages + Environment.NewLine;
                            }
                        }
                    }
                }

                if (VehicleIncentiveVM.VehicleIncentiveVMList != null && VehicleIncentiveVM.VehicleIncentiveVMList.Count > 0)
                {
                    VehicleIncentiveVM = _vehicleIncentiveManager.ManageVehicleIncentiveBulk(VehicleIncentiveVM, _loginSession.UserViewModel.UserId);
                    if (VehicleIncentiveVM != null)
                    {
                        if (VehicleIncentiveVM.VehicleIncentiveVMErrorList != null && VehicleIncentiveVM.VehicleIncentiveVMErrorList.Count > 0)
                        {
                            fileResult = OnPostExportExcel_DataError(VehicleIncentiveVM.VehicleIncentiveVMList, "VehicleIncentiveVMErrorList");
                            replyMessage = VehicleIncentiveVM.VehicleIncentiveVMErrorList.Count + " Records Import Sucessfully and " + VehicleIncentiveVM.VehicleIncentiveVMErrorList.Count + " Records Not Import";
                        }
                        else
                        {
                            replyMessage = "All Records Import Sucessfully";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
            if (fileResult != null)
            {
                return fileResult;
            }
            else
            {
                return RedirectToPage("./LOV_Utility", new { id = _protector.Encode(VehicleIncentiveVM.IncentiveId) });
            }
        }

        #endregion


        public IActionResult OnPostVehicleIncentiveDeleteAsync()
        {
            if (_loginSession == null)
            {
                return RedirectToPage("/LOV_Utility");
            }

            else
            {
                if (TblVehicleIncentiveObj != null && TblVehicleIncentiveObj.IncentiveId > 0)
                {
                    TblVehicleIncentiveObj = _context.TblVehicleIncentives.Find(TblVehicleIncentiveObj.IncentiveId);
                }

                if (TblVehicleIncentiveObj != null)
                {
                    TblVehicleIncentiveObj.IsActive = false;
                    TblVehicleIncentiveObj.ModifiedBy = _loginSession.UserViewModel.UserId;
                    _context.TblVehicleIncentives.Update(TblVehicleIncentiveObj);

                    _context.SaveChanges();
                }

                return RedirectToPage("./LOV_Utility");
            }
        }


        #region Excel Import for Image Label Master
        public ActionResult OnPostImageLabelExcelImport()
        {

            IFormFile ImportFile = null; FileResult fileResult = null;
            string replyMessage = string.Empty;

            if (ImageLabelVM != null && ImageLabelVM.UploadImageLabel != null)
            {
                ImportFile = ImageLabelVM.UploadImageLabel;
                ImageLabelVM.ImageLabelVMList = new List<ImageLabelMasterVMExcel>();
            }
            try
            {
                if (ImportFile != null)
                {
                    // DataTable datatable = ExcelImportHelper.ExcelSheetReader(ImportFile);
                    ImageLabelVM.ImageLabelVMList = ExcelImportHelper.ExcelToList<ImageLabelMasterVMExcel>(ImportFile, ImageLabelVM.ImageLabelVMList);
                    // Implement Model based Validations

                    if (ImageLabelVM.ImageLabelVMList != null && ImageLabelVM.ImageLabelVMList.Count > 0)
                    {
                        foreach (var item in ImageLabelVM.ImageLabelVMList)
                        {

                            // Validate the model
                            var results = new List<ValidationResult>();
                            var context = new ValidationContext(item);
                            if (!Validator.TryValidateObject(item, context, results, true))
                            {
                                var errorMessages = string.Join(Environment.NewLine, results.Select(r => r.ErrorMessage));
                                item.Remarks += errorMessages + Environment.NewLine;
                            }
                        }
                    }
                }

                if (ImageLabelVM.ImageLabelVMList != null && ImageLabelVM.ImageLabelVMList.Count > 0)
                {
                    ImageLabelVM = _imageLabelMasterManager.ManageImageLabelBulk(ImageLabelVM, _loginSession.UserViewModel.UserId);
                    if (ImageLabelVM != null)
                    {
                        if (ImageLabelVM.ImageLabelVMErrorList != null && ImageLabelVM.ImageLabelVMErrorList.Count > 0)
                        {
                            fileResult = OnPostExportExcel_DataError(VehicleIncentiveVM.VehicleIncentiveVMList, "ImageLabelMasterVMErrorList");
                            replyMessage = ImageLabelVM.ImageLabelVMErrorList.Count + " Records Import Sucessfully and " + ImageLabelVM.ImageLabelVMErrorList.Count + " Records Not Import";
                        }
                        else
                        {
                            replyMessage = "All Records Import Sucessfully";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
            if (fileResult != null)
            {
                return fileResult;
            }
            else
            {
                return RedirectToPage("./LOV_Utility", new { id = _protector.Encode(VehicleIncentiveVM.IncentiveId) });
            }
        }

        #endregion


        public IActionResult OnPostImageLabelDeleteAsync()
        {
            if (_loginSession == null)
            {
                return RedirectToPage("/LOV_Utility");
            }

            else
            {
                if (TblImageLabelMasterObj != null && TblImageLabelMasterObj.ImageLabelid> 0)
                {
                    TblImageLabelMasterObj = _context.TblImageLabelMasters.Find(TblImageLabelMasterObj.ImageLabelid);
                }

                if (TblImageLabelMasterObj != null)
                {
                    TblImageLabelMasterObj.IsActive = false;
                    TblImageLabelMasterObj.Modifiedby = _loginSession.UserViewModel.UserId;
                    _context.TblImageLabelMasters.Update(TblImageLabelMasterObj);

                    _context.SaveChanges();
                }

                return RedirectToPage("./LOV_Utility");
            }
        }

    }

}
