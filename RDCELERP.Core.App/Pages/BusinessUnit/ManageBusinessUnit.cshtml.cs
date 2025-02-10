


using System;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Threading.Tasks;
using ClosedXML.Excel;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Hosting;
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
using RDCELERP.BAL.MasterManager;
using RDCELERP.Common.Enums;
using RDCELERP.Common.Helper;
using RDCELERP.Core.App.Pages.Base;
using RDCELERP.DAL.Entities;
using RDCELERP.DAL.IRepository;
using RDCELERP.DAL.Repository;
using RDCELERP.Model.ABBPriceMaster;
using RDCELERP.Model.Base;
using RDCELERP.Model.BusinessPartner;
using RDCELERP.Model.BusinessUnit;
using RDCELERP.Model.Company;
using RDCELERP.Model.Master;
using RDCELERP.Model.ModelNumber;
using RDCELERP.Model.PriceMaster;
using RDCELERP.Model.ServicePartner;
using CellType = NPOI.SS.UserModel.CellType;
using Newtonsoft.Json;
using RDCELERP.Model.Product;
using RDCELERP.Model.UniversalPriceMaster;
using DocumentFormat.OpenXml.Office2010.Excel;

namespace RDCELERP.Core.App.Pages.BusinessUnit
{
    public class ManageBusinessUnitModel : CrudBasePageModel
    {
        #region Variable Declaration
        private readonly IBusinessUnitManager _BusinessUnitManager;
        private readonly IModelNumberManager _modelNumberManager;
        private readonly IModelNumberRepository _modelNumberRepository;
        private readonly ICompanyManager _CompanyManager;
        private readonly IProductCategoryManager _productCategoryManager;
        private readonly IBusinessPartnerManager _BusinessPartnerManager;
        private readonly IBUProductCategoryMapping _bUProductCategoryMapping;
        private readonly IProductCategoryRepository _ProductCategoryRepository;
        private readonly IBusinessPartnerRepository _businessPartnerRepository;
        private readonly IUniversalPriceMasterManager _universalPriceMasterManager;
        private IWebHostEnvironment _webHostEnvironment;
        private ILovRepository _lovRepository;
        ILogging _logging;
        private readonly Digi2l_DevContext _context;

        
        #endregion

        public ManageBusinessUnitModel(IBUProductCategoryMapping bUProductCategoryMapping, IUniversalPriceMasterManager universalPriceMasterManager, IBusinessPartnerRepository businessPartnerRepository, IProductCategoryRepository ProductCategoryRepository, IModelNumberRepository modelNumberRepository, IModelNumberManager modelNumberManager, IBusinessUnitManager BusinessUnitManager, ILogging logging, IOptions<ApplicationSettings> config, CustomDataProtection protector, ICompanyManager companyManager, IProductCategoryManager productCategoryManager, IBusinessPartnerManager businessPartnerManager, IWebHostEnvironment webHostEnvironment, ILovRepository lovRepository, Digi2l_DevContext context) : base(config, protector)
        {
            _BusinessUnitManager = BusinessUnitManager;
            _CompanyManager = companyManager;
            _productCategoryManager = productCategoryManager;
            _BusinessPartnerManager = businessPartnerManager;
            _webHostEnvironment = webHostEnvironment;
            _lovRepository = lovRepository;
            _logging = logging;
            _modelNumberManager = modelNumberManager;
            _modelNumberRepository = modelNumberRepository;
            _context = context;
            _bUProductCategoryMapping = bUProductCategoryMapping;
            _ProductCategoryRepository = ProductCategoryRepository;
            _businessPartnerRepository = businessPartnerRepository;
            _universalPriceMasterManager = universalPriceMasterManager;

        }
        [BindProperty(SupportsGet = true)]
        public ModelNumberViewModel ModelNumberVM { get; set; }
        [BindProperty(SupportsGet = true)]
        public BusinessPartnerVMExcel businessPartnerVM { get; set; }
        [BindProperty(SupportsGet = true)]
        public PriceMasterVMExcel exchangePriceMasterVM { get; set; }
        [BindProperty(SupportsGet = true)]
        public UniversalPriceMasterViewModel UniversalPriceMasterVM { get; set; }
        [BindProperty(SupportsGet = true)]
        public BusinessUnitViewModel BusinessUnitVM { get; set; }
        
       
        public bool IsChecked { get; set; }
        public CountBUOnboardingListDataVM CountBUOnboardingListDataVM { get; set; }
        public IActionResult OnGet(string id)
        {
            var GSTType = _lovRepository.GetList(x => x.ParentId == Convert.ToInt32(LoVEnum.GSTType) && x.IsActive == true);
            if (GSTType != null)
            {
                ViewData["GstTypeList"] = new SelectList(GSTType, "ParentId", "LoVname");
            }

            if (id != null)
                id = _protector.Decode(id);

            BusinessUnitVM = _BusinessUnitManager.GetBusinessUnitById(Convert.ToInt32(id));
            if (BusinessUnitVM != null && BusinessUnitVM.BusinessUnitId > 0)
            {
                if (GSTType != null)
                {
                    ViewData["GstTypeList"] = new SelectList(GSTType, "ParentId", "LoVname");
                }

                BusinessUnitVM.CompanyVM = _CompanyManager.GetCompanyByBUId(BusinessUnitVM.BusinessUnitId);
                if (BusinessUnitVM.CompanyVM != null && BusinessUnitVM.CompanyVM.BusinessUnitId > 0)
                {
                    var BusinessUnit = _context.TblBusinessUnits.Where(x => x.BusinessUnitId == BusinessUnitVM.CompanyVM.BusinessUnitId && x.IsActive == true).FirstOrDefault();
                    if (BusinessUnit != null)
                    {
                        BusinessUnitVM.CompanyVM.BusinessUnitName = BusinessUnit.Name;
                    }
                }

               
                BusinessUnitVM.BuLoginVM = _BusinessUnitManager.GetBULoginCredentials(Convert.ToInt32(id));
                #region BU Login VM
                if (BusinessUnitVM.BuLoginVM == null)
                {
                    BusinessUnitVM.BuLoginVM = new BusinessUnitLoginVM();
                }
                if (!string.IsNullOrEmpty(BusinessUnitVM.LogoName))
                {

                    BusinessUnitVM.LogoUrlLink = _baseConfig.Value.BaseURL + "/DBFiles/Sponsor/" + BusinessUnitVM.LogoName;
                }



                #region Display QR Code Image
                string baseUrl = _baseConfig.Value.BaseURL;
                string fileAddress = EnumHelper.DescriptionAttr(FileAddressEnum.QRImage);
                BusinessUnitVM.BuLoginVM.QrCodeNameBU = BusinessUnitVM.QrcodeUrl;
                BusinessUnitVM.BuLoginVM.QrCodeUrlBU = baseUrl + fileAddress + BusinessUnitVM.QrcodeUrl;
                BusinessUnitVM.BuLoginVM.QrCodeNameBPBU = BusinessUnitVM.BuLoginVM.QrCodeNameBPBU;
                BusinessUnitVM.BuLoginVM.QrCodeUrlBPBU = baseUrl + fileAddress + BusinessUnitVM.BuLoginVM.QrCodeNameBPBU;
                #endregion
                var businessPartnerList = _BusinessPartnerManager.GetListofBusinessPartnerByBUId(Convert.ToInt32(id));
                if (businessPartnerList != null)
                {
                    BusinessUnitVM.BuLoginVM.BusinessPartnerSelectList = new SelectList(businessPartnerList, "BusinessPartnerId", "Name");
                }
                #endregion

              
                var PriceMasterList = _universalPriceMasterManager.GetListofpriceMasterByBUId(Convert.ToInt32(id));
                if (PriceMasterList != null)
                {
                    BusinessUnitVM.BuLoginVM.PriceMasterNameList = new SelectList(PriceMasterList, "PriceMasterNameId", "Name");
                }
               

                CountBUOnboardingListDataVM = _BusinessUnitManager.GetCountBUOnboardingListData(BusinessUnitVM.BusinessUnitId);
                if (BusinessUnitVM.CompanyVM == null)
                {
                    BusinessUnitVM.CompanyVM = new CompanyViewModel();
                    BusinessUnitVM.ActiveTabId = 2;
                }
                else if (CountBUOnboardingListDataVM != null)
                {
                    if (CountBUOnboardingListDataVM.BPListCount == 0)
                    {
                        BusinessUnitVM.ActiveTabId = 3;
                    }
                    else if (CountBUOnboardingListDataVM.ABBPlanMasterListCount == 0 || CountBUOnboardingListDataVM.ABBPriceMasterListCount == 0)
                    {
                        BusinessUnitVM.ActiveTabId = 5;
                    }
                    else if (CountBUOnboardingListDataVM.ExchPriceMasterListCount == 0)
                    {
                        BusinessUnitVM.ActiveTabId = 6;
                    }
                    /*else if()
                    {
                        BusinessUnitVM.ActiveTabId = 7;
                    }*/
                    else
                    {
                        BusinessUnitVM.ActiveTabId = 6;
                    }
                }
            }
            else
            {
                BusinessUnitVM = new BusinessUnitViewModel();
                BusinessUnitVM.CompanyVM = new CompanyViewModel();
                BusinessUnitVM.BusinessPartnerVM = new BusinessPartnerVMExcelModel();
                BusinessUnitVM.ABBPriceMasterVM = new ABBPriceMasterViewModel();
                BusinessUnitVM.BuLoginVM = new BusinessUnitLoginVM();
                BusinessUnitVM.ExchangePriceMasterVM = new PriceMasterViewModel();
                BusinessUnitVM.ActiveTabId = 1;
            }
            if (!string.IsNullOrEmpty(BusinessUnitVM.LogoName))
            {

                BusinessUnitVM.LogoUrlLink = _baseConfig.Value.BaseURL + "/DBFiles/Sponsor/" + BusinessUnitVM.LogoName;
            }

            #region Common for Edit and Create
            string json = null;
            JsonSerializerOptions options = new JsonSerializerOptions
            {
                ReferenceHandler = ReferenceHandler.Preserve,
                // Other options as needed
            };

            BusinessUnitVM.ProductCategoryVMList = _BusinessUnitManager.GetAllProductCategoryBuMapping(Convert.ToInt32(id));
            var result = _bUProductCategoryMapping.GetBUProdCatList(Convert.ToInt32(id));
            var iteration = result.Where(x => x.ProductCatId != null).Select(x => x.ProductCatId).Distinct().ToList();
            List<int?> selectedProductIds = null;
            List<TblProductType?> tblProductType = null;
            // Iterate through existing ProductCategoryViewModelList
            for (int i = 0; i < BusinessUnitVM.ProductCategoryVMList?.Count && i < iteration.Count; i++)
            {
                selectedProductIds = result
                    .Where(x => x.ProductCatId == iteration[i])
                    .Select(x => x.ProductTypeId)
                    .ToList();

                // Set the SelectedProduct property for the current ProductCategoryViewModel

                BusinessUnitVM.ProductCategoryVMList[i].SelectedProduct = selectedProductIds;

            }


            //string json = null;
            //JsonSerializerOptions options = new JsonSerializerOptions
            //{
            //    ReferenceHandler = ReferenceHandler.Preserve,
            //    // Other options as needed
            //};
            //List<ProductTypeNameDescription>? ProductlistS = null;

            //if (tblProductType.Count > 0)
            //{


            //    ProductlistS = tblProductType
            //        .Select(x => new ProductTypeNameDescription { Id = x.Id, Description = x.Description + x.Size }).ToList();
            //}

            //if (ProductlistS != null)
            //{
            //    json = System.Text.Json.JsonSerializer.Serialize(ProductlistS, options);
            //}

            #endregion


            //#region Common for Edit and Create
            //BusinessUnitVM.ProductCategoryVMList = _BusinessUnitManager.GetDataForCategoryAndType(Convert.ToInt32(id));
            //#endregion

            if (BusinessUnitVM.ProductCategoryVMList == null)
            {
                BusinessUnitVM.ProductCategoryVMList = new List<ProductCategoryViewModel>();
            }
            if (BusinessUnitVM.BuLoginVM == null)
            {
                BusinessUnitVM.BuLoginVM = new BusinessUnitLoginVM();
            }

            if (_loginSession == null)
            {
                return RedirectToPage("/index");
            }
            else
            {
                return Page();
            }
        }

        #region Manage BusinessUnit
        public async Task<IActionResult> OnPostManageBusinessUnitAsync(IFormFile SponsorLogo)
        {
            int result = 0;
            var GSTType = _lovRepository.GetList(x => x.ParentId == Convert.ToInt32(LoVEnum.GSTType) && x.IsActive == true);
            if (GSTType != null)
            {
                ViewData["GstTypeList"] = new SelectList(GSTType, "ParentId", "LoVname");
            }

            if (BusinessUnitVM != null)
            {
                if (SponsorLogo != null)
                {
                    string fileName = Guid.NewGuid().ToString("N") + SponsorLogo.FileName;
                    var filePath = string.Concat(_webHostEnvironment.WebRootPath, "\\", @"\DBFiles\Sponsor");
                    var fileNameWithPath = string.Concat(filePath, "\\", fileName);
                    using (var stream = new FileStream(fileNameWithPath, FileMode.Create))
                    {
                        SponsorLogo.CopyTo(stream);
                        BusinessUnitVM.LogoName = fileName;
                    }

                }


                result = _BusinessUnitManager.ManageBusinessUnit(BusinessUnitVM, _loginSession.UserViewModel.UserId);
                if (result > 0)
                {
                    ViewData["Message"] = "This Business Unit is Saved Sucessfully";
                }
                else
                {
                    ViewData["Message"] = "This Business Unit is Not Saved";
                }
            }
            return RedirectToPage("./ManageBusinessUnit", new { id = _protector.Encode(result) });
        }
        #endregion


        #region Excel Import for Model Number/ Model Mapping Master
        public ActionResult OnPostModelMasterExcelImport()
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
                            var Check = _modelNumberRepository.GetSingle(x => x.ModelName?.Trim().ToLower() == item.ModelName?.Trim().ToLower() && x.IsActive == true && x.BusinessUnit?.Name == item.CompanyName);
                            if (Check != null)
                            {
                                item.Remarks += "This model is already exist for this company" + Environment.NewLine;
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
                    if (ModelNumberVM != null)
                    {
                        if (ModelNumberVM.ModelNumberVMErrorList != null && ModelNumberVM.ModelNumberVMErrorList.Count > 0)
                        {
                            fileResult = OnPostExportExcel_DataError(ModelNumberVM.ModelNumberVMErrorList, "ModelNumberVMErrorList");
                            replyMessage = ModelNumberVM.ModelNumberVMList.Count + " Records Import Sucessfully and " + ModelNumberVM.ModelNumberVMErrorList.Count + " Records Not Import";
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
                return RedirectToPage("./ManageBusinessUnit", new { id = _protector.Encode(BusinessUnitVM.BusinessUnitId) });
            }
        }

        #endregion

        #region Manage Company Details
        public async Task<IActionResult> OnPostManageCompanyAsync()
        {
            int result = 0;
            if (BusinessUnitVM.BusinessUnitId > 0 && BusinessUnitVM.CompanyVM != null)
            {
                result = _CompanyManager.ManageCompany(BusinessUnitVM.CompanyVM, _loginSession.UserViewModel.UserId);
                if (result > 0)
                {
                    ViewData["Message"] = "This Company is Saved Sucessfully";
                }
                else
                {
                    ViewData["Message"] = "This Company is Not Saved";
                }
            }
            return RedirectToPage("./ManageBusinessUnit", new { id = _protector.Encode(BusinessUnitVM.BusinessUnitId) });
        }
        #endregion

        #region Manage BU Credentials for API and Dashboard
        public async Task<IActionResult> OnPostManageBULoginAsync()
        {
            int result = 0;

            if (BusinessUnitVM != null)
            {
                result = _BusinessUnitManager.SaveBUCredentials(BusinessUnitVM, _loginSession.UserViewModel.UserId);
                if (result > 0)
                {
                    ViewData["Message"] = "Business Unit Credentials Saved Sucessfully";
                }
                else
                {
                    ViewData["Message"] = "Business Unit Credentials Not Saved";
                }
            }
            return RedirectToPage("./ManageBusinessUnit", new { id = _protector.Encode(BusinessUnitVM.BusinessUnitId) });
        }
        #endregion

        #region Excel Import and Export Business Partner and Exchange Price master
        #region Excel Import Business Partner
        public ActionResult OnPostBPExcelImport()
        {
            int result = 0; IFormFile ImportFile = null; FileResult fileResult = null;
            var businessPartnerVMList = new List<BusinessPartnerVMExcelModel>();
            if (BusinessUnitVM != null && BusinessUnitVM.UploadBusinessPartner != null)
            {
                ImportFile = BusinessUnitVM.UploadBusinessPartner;
                BusinessUnitVM.BusinessPartnerVMList = new List<BusinessPartnerVMExcelModel>();
            }
            try
            {
                if (ImportFile != null)
                {
                    // DataTable datatable = ExcelImportHelper.ExcelSheetReader(ImportFile);
                    BusinessUnitVM.BusinessPartnerVMList = ExcelImportHelper.ExcelToList<BusinessPartnerVMExcelModel>(ImportFile, BusinessUnitVM.BusinessPartnerVMList);
                    // Implement Model based Validations
                    if (BusinessUnitVM.BusinessPartnerVMList != null && BusinessUnitVM.BusinessPartnerVMList.Count > 0)
                    {
                        foreach (var item in BusinessUnitVM.BusinessPartnerVMList)
                        {
                            // Validate the model
                            var results = new List<ValidationResult>();
                            var context = new ValidationContext(item);
                            if (!Validator.TryValidateObject(item, context, results, true))
                            {
                                // throw new Exception($"Validation failed for row {row}: {string.Join(", ", results.Select(r => r.ErrorMessage))}");
                                //item.Remarks += results.Select(r => r.ErrorMessage);
                                var exceptions = new Exception($"{string.Join(", ", results.Select(r => r.ErrorMessage))}");
                                item.Remarks += exceptions.Message;
                            }
                        }
                    }
                }
                 
                if (BusinessUnitVM.BusinessPartnerVMList != null && BusinessUnitVM.BusinessPartnerVMList.Count > 0)
                {
                    BusinessPartnerViewModel businessPartnerViewModel = new BusinessPartnerViewModel(); 
                    businessPartnerViewModel.BusinessPartnerVMList = BusinessUnitVM.BusinessPartnerVMList;
                    var res = _BusinessPartnerManager.ManageBusinessPartnerBulk(businessPartnerViewModel, _loginSession.UserViewModel.UserId);
                    BusinessUnitVM.BusinessPartnerVM = res.BusinessPartnerVM;

                    if (BusinessUnitVM != null)
                    {
                        if (BusinessUnitVM.BusinessPartnerVMErrorList != null && BusinessUnitVM.BusinessPartnerVMErrorList.Count > 0)
                        {
                            fileResult = OnPostExportExcel_DataError(BusinessUnitVM.BusinessPartnerVMErrorList, "BusinessPartnerListError");
                            ViewData["Message"] = BusinessUnitVM.BusinessPartnerVMList.Count + " Records Import Sucessfully and " + BusinessUnitVM.BusinessPartnerVMErrorList.Count + " Records Not Import";
                        }
                        else
                        {
                            ViewData["Message"] = "All Records Import Sucessfully";
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
                return RedirectToPage("./ManageBusinessUnit", new { id = _protector.Encode(BusinessUnitVM.BusinessUnitId) });
            }
        }

        /*public async Task<IActionResult> OnPostExcelImportAsync()
        {
            
            IFormFile fileName = BusinessUnitVM.BusinessPartnerVM.UploadBusinessPartner;
            // Open the document as read-only.
            using (SpreadsheetDocument spreadsheetDocument =
                SpreadsheetDocument.Open(fileName.FileName, false))
            {
                // Code removed here.
                WorkbookPart workbookPart = spreadsheetDocument.WorkbookPart;
                WorksheetPart worksheetPart = workbookPart.WorksheetParts.First();
                SheetData sheetData = worksheetPart.Worksheet.Elements<SheetData>().First();
                string text;
                foreach (Row r in sheetData.Elements<Row>())
                {
                    foreach (Cell c in r.Elements<Cell>())
                    {
                        text = c.CellValue.Text;
                        Console.Write(text + " ");
                    }
                }
            }

            
            return Page();
        }
*/
        #endregion

        #region Excel Import for Universal Price Master
        public ActionResult OnPostUniversalPriceMasterExcelImport()
        {

            IFormFile ImportFile = null; FileResult fileResult = null;
            string replyMessage = string.Empty;

            if (UniversalPriceMasterVM != null && UniversalPriceMasterVM.UploadUniversalPriceMaster != null)
            {
                ImportFile = UniversalPriceMasterVM.UploadUniversalPriceMaster;
                UniversalPriceMasterVM.UniversalPriceMasterVMList = new List<UniversalPriceMasterExcel>();
            }
            try
            {
                if (ImportFile != null)
                {
                    // DataTable datatable = ExcelImportHelper.ExcelSheetReader(ImportFile);
                    UniversalPriceMasterVM.UniversalPriceMasterVMList = ExcelImportHelper.ExcelToList<UniversalPriceMasterExcel>(ImportFile, UniversalPriceMasterVM.UniversalPriceMasterVMList);
                    // Implement Model based Validations
                    if (UniversalPriceMasterVM.UniversalPriceMasterVMList != null && UniversalPriceMasterVM.UniversalPriceMasterVMList.Count > 0)
                    {
                        foreach (var item in UniversalPriceMasterVM.UniversalPriceMasterVMList)
                        {
                            var Check = _businessPartnerRepository.GetSingle(x => x.StoreCode?.Trim().ToLower() == item.StoreCode?.Trim().ToLower() && x.IsActive == true);
                            if (Check == null)
                            {
                                item.Remarks += "This store code is not exist in any store" + Environment.NewLine;
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

                if (UniversalPriceMasterVM.UniversalPriceMasterVMList != null && UniversalPriceMasterVM.UniversalPriceMasterVMList.Count > 0)
                {
                    UniversalPriceMasterVM = _universalPriceMasterManager.ManageUniversalPriceMasterBulk(UniversalPriceMasterVM, _loginSession.UserViewModel.UserId);
                    if (UniversalPriceMasterVM != null)
                    {
                        if (UniversalPriceMasterVM.UniversalPriceMasterVMErrorList != null && UniversalPriceMasterVM.UniversalPriceMasterVMErrorList.Count > 0)
                        {
                            fileResult = OnPostExportExcel_DataError(UniversalPriceMasterVM.UniversalPriceMasterVMErrorList, "UniversalPriceMasterVMErrorList");
                            replyMessage = UniversalPriceMasterVM.UniversalPriceMasterVMList.Count + " Records Import Sucessfully and " + UniversalPriceMasterVM.UniversalPriceMasterVMErrorList.Count + " Records Not Import";
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
                return RedirectToPage("./ManageBusinessUnit", new { id = _protector.Encode(BusinessUnitVM.BusinessUnitId) });
            }
        }

        #endregion

        #region Export Data To Excel Business Partner and Exchange Price Master
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
        #endregion

        #region Save BU Product Cat Mapping
        public async Task<IActionResult> OnPostSaveBUProductCatMappingAsync()
        {
            int result = 0;

            if (BusinessUnitVM != null)
            {
               
                result = _BusinessUnitManager.SaveBuProductCategoriesForABBandExch(BusinessUnitVM, _loginSession.UserViewModel.UserId);
                var list = BusinessUnitVM.ProductCategoryVMList.Select(x => x.Id).ToList();
                if (result > 0)
                {
                    ViewData["Message"] = "Product Category Saved Sucessfully";
                }
                else
                {
                    ViewData["Message"] = "Product Category is Not Saved";
                }
            }
            return RedirectToPage("./ManageBusinessUnit", new { id = _protector.Encode(BusinessUnitVM.BusinessUnitId) });
        }
        #endregion

        #region Generate QR for Business Unit and Business Partner with BU
        public async Task<IActionResult> OnPostGenerateQRForBUAsync()
        {
            int result = 0;

            if (BusinessUnitVM != null)
            {
                string url = _baseConfig.Value.MVCEntryPoint + BusinessUnitVM.BusinessUnitId;
                bool QRIsSaved = _BusinessUnitManager.GenerateBUQRCode(BusinessUnitVM.BuLoginVM, url, "BusinessUnitQR");
                if (QRIsSaved)
                {
                    ViewData["Message"] = "QR Code Generated Sucessfully";
                }
                else
                {
                    ViewData["Message"] = "QR Code Generated is Not Generated";
                }
            }
            return RedirectToPage("./ManageBusinessUnit", new { id = _protector.Encode(BusinessUnitVM.BusinessUnitId) });
        }

        public async Task<IActionResult> OnPostGenerateQRForBPWithBUAsync()
        {
            int result = 0;
            try
            {
                if (BusinessUnitVM != null && BusinessUnitVM.BuLoginVM.BusinessPartnerId != null && BusinessUnitVM.BuLoginVM.BusinessPartnerId > 0)
                {
                    string url = _baseConfig.Value.MVCEntryPoint + BusinessUnitVM.BusinessUnitId + "&&bpid=" + BusinessUnitVM.BuLoginVM.BusinessPartnerId;
                    bool QRIsSaved = _BusinessUnitManager.GenerateBPBUQRCode(BusinessUnitVM.BuLoginVM, url, "BusinessPartnerQR");
                    if (QRIsSaved)
                    {
                        ViewData["Message"] = "QR Code Generated Sucessfully";
                    }
                    else
                    {
                        ViewData["Message"] = "QR Code Generated is Not Generated";
                    }
                }
            }



            catch (Exception ex)
            {
                _logging.WriteErrorToDB("ManageBusinessUnitModel", "GenerateQRForBPWithBU", ex);
            }
            return RedirectToPage("./ManageBusinessUnit", new { id = _protector.Encode(BusinessUnitVM.BusinessUnitId) });
        }
        #endregion


        public IActionResult OnGetSearchBUName(string term)
        {
            if (term == null)
            {
                return BadRequest();
            }
            var data = _context.TblBusinessUnits
                .Where(p => p.Name.Contains(term) && p.IsActive == true)
                 .Select(s => new SelectListItem
                 {
                     Value = s.Name,
                     Text = s.BusinessUnitId.ToString()
                 })
                .ToArray();
            return new JsonResult(data);
            //}

        }

        //       public IActionResult OnGetSelectProductTypes(string? Description)
        //       {

        //           List<TblProductCategory> category = _context.TblProductCategories
        //.Where(x => x.IsActive == true && x.Description == Description)
        //.ToList();

        //           //List<TblProductType> ProductlistS = null;
        //           List<ProductTypeNameDescription>? ProductlistS = null;

        //           if (category.Count > 0)
        //           {
        //               int categoryId = category[0].Id;

        //               ProductlistS = _context.TblProductTypes
        //                   .Where(x => x.IsActive == true && x.ProductCatId == categoryId).Select(x => new ProductTypeNameDescription { Id = x.Id, Description = x.Description + x.Size }).ToList();
        //           }

        //           if (ProductlistS != null)
        //           {
        //               ViewData["ProductlistS"] = new SelectList(ProductlistS, "Id", "Description");
        //           }

        //           return new JsonResult(ProductlistS);

        //       }


        public IActionResult OnGetSelectProductTypes(string? Description, int? Id)
        {
            string json = null;
            JsonSerializerOptions options = new JsonSerializerOptions
            {
                ReferenceHandler = ReferenceHandler.Preserve,
                // Other options as needed
            };

            List<TblProductCategory> category = _context.TblProductCategories
                .Where(x => x.IsActive == true && x.Description == Description)
                .ToList();

            List<ProductTypeNameDescription>? ProductlistS = null;

            if (category.Count > 0)
            {
                int categoryId = category[0].Id;

                ProductlistS = _context.TblProductTypes
                    .Where(x => x.IsActive == true && x.ProductCatId == categoryId).Select(x => new ProductTypeNameDescription { Id = x.Id, Description = x.Description + x.Size }).ToList();
            }

            if (ProductlistS != null)
            {
                json = System.Text.Json.JsonSerializer.Serialize(ProductlistS, options);
            }
            
            return Content(json, "application/json");
        }
    }
}
