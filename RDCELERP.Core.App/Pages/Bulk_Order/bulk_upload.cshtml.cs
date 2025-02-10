using RDCELERP.BAL.Interface;
using RDCELERP.Core.App.Helper;
using RDCELERP.Core.App.Pages.Base;
using RDCELERP.DAL.Entities;
using RDCELERP.Model.Base;
using RDCELERP.Model.Users;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RDCELERP.Model.BusinessUnit;
using Microsoft.AspNetCore.Http;
using RDCELERP.Model.ExchangeOrder;
using RDCELERP.Common.Helper;
using System.ComponentModel.DataAnnotations;
using System.IO;
using RDCELERP.DAL.IRepository;
using RDCELERP.DAL.Repository;
using static Org.BouncyCastle.Math.EC.ECCurve;
using RDCELERP.Model.SearchFilters;
using RDCELERP.BAL.Interface;
using RDCELERP.BAL.MasterManager;

namespace RDCELERP.Core.App.Pages
{
    public class BulkUpload : CrudBasePageModel
    {
        
        private readonly IUserManager _userManager;
        private readonly IExchangeOrderManager _exchangeOrderManager;
        private readonly IUniversalPriceMasterRepository _universalPriceMasterRepository;
        private readonly IPriceMasterMappingRepository _priceMasterMappingRepository;
        IProductCategoryManager _productCategoryManager;
        IProductTypeManager _productTypeManager;
        private readonly RDCELERP.DAL.Entities.Digi2l_DevContext _context;
        ILogging _logging;
        IOptions<ApplicationSettings> _baseConfig;
        public BulkUpload(IUserManager userManager, IProductCategoryManager productCategoryManager, IProductTypeManager productTypeManager, IOptions<ApplicationSettings> baseConfig, IExchangeOrderManager exchangeOrderManager, CustomDataProtection protector, RDCELERP.DAL.Entities.Digi2l_DevContext context, IOptions<ApplicationSettings> config, IUniversalPriceMasterRepository universalPriceMasterRepository, IPriceMasterMappingRepository priceMasterMappingRepository, ILogging logging)
        : base(config, protector)

        {
           
            _userManager = userManager;
            _context = context;
            _exchangeOrderManager = exchangeOrderManager;
            _universalPriceMasterRepository = universalPriceMasterRepository;
            _priceMasterMappingRepository = priceMasterMappingRepository;
            _productCategoryManager = productCategoryManager;
            _productTypeManager = productTypeManager;
            _baseConfig = baseConfig;
            _logging = logging;
        }


        [BindProperty(SupportsGet = true)]
        public BusinessUnitViewModel BusinessUnitViewModel { get; set; }
        [BindProperty(SupportsGet = true)]
        public ExchangeBulkLiquidatioModel ExchangeVM1 { get; set; }
        [BindProperty(SupportsGet = true)]
        public SearchFilterViewModel searchFilterVM { get; set; }
        [BindProperty(SupportsGet = true)]
        public int UserId { get; set; }
        public IActionResult OnGet()
        {

            if (_loginSession == null)
            {
                return RedirectToPage("Index");
            }
            else
            {
                UserId = _loginSession.UserViewModel.UserId;
                // Get Product Category List
                var ProductGroup = _productCategoryManager.GetAllProductCategory();
                if (ProductGroup != null)
                {
                    ViewData["ProductGroup"] = new SelectList(ProductGroup, "Id", "Description");
                }
                return Page();
            }

        }
       
       

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


        //public IActionResult OnGetAutoBusinessUnit(string term)
        //{
        //    if (term == null)
        //    {
        //        return BadRequest();
        //    }
        //    var list = _context.TblBusinessUnits
        //               .Where(e => e.Name.Contains(term) && e.IsBulkOrder == true)
        //                .Select(s => new SelectListItem
        //                {
        //                    Value = s.Name.ToString(),
        //                    Text = s.BusinessUnitId.ToString()
        //                })
        //               .ToArray();
        //    return new JsonResult(list);
        //}

        #region Excel Import for Exchange
        public ActionResult OnPostExchangeExcelImport()
        {

            IFormFile ImportFile = null; FileResult fileResult = null;
            string replyMessage = string.Empty;

            if (ExchangeVM1 != null && ExchangeVM1.UploadExchange1 != null)
            {
                ImportFile = ExchangeVM1.UploadExchange1;
                ExchangeVM1.ExchangeVMList1 = new List<ExchangeBulkExcel>();
            }
            try
            {
                if (ImportFile != null)
                {
                    // DataTable datatable = ExcelImportHelper.ExcelSheetReader(ImportFile);
                    ExchangeVM1.ExchangeVMList1 = ExcelImportHelper.ExcelToList<ExchangeBulkExcel>(ImportFile, ExchangeVM1.ExchangeVMList1);
                    // Implement Model based Validations

                    if (ExchangeVM1.ExchangeVMList1 != null && ExchangeVM1.ExchangeVMList1.Count > 0)
                    {
                        DAL.Entities.TblUser tblUser = _context.TblUsers.Where(x => x.UserId == _loginSession.UserViewModel.UserId && x.IsActive == true).FirstOrDefault();
                        tblUser.Email = SecurityHelper.DecryptString(tblUser.Email, _baseConfig.Value.SecurityKey);
                        foreach (var item in ExchangeVM1.ExchangeVMList1)
                        {

                            if (tblUser.Email != null)
                            {
                                TblBusinessPartner BusinessPartner = _context.TblBusinessPartners.Where(x => x.Email == tblUser.Email && x.IsActive == true).FirstOrDefault();

                                var BusinessUnit = _context.TblBusinessUnits.Where(x => x.BusinessUnitId == BusinessPartner.BusinessUnitId && x.IsActive == true).FirstOrDefault();
                                TblProductType ProductType = _context.TblProductTypes.Where(x => x.Description + x.Size == item.ProductType && x.IsActive == true).FirstOrDefault();
                                TblProductCategory ProductCategory = _context.TblProductCategories.Where(x => x.Description == item.ProductCategory && x.IsActive == true).FirstOrDefault();
                                TblPriceMasterMapping tblPriceMasterMapping = new TblPriceMasterMapping();

                                tblPriceMasterMapping = _priceMasterMappingRepository.GetSingle(x => x.BusinessUnitId == BusinessUnit.BusinessUnitId && x.BusinessPartnerId == BusinessPartner.BusinessPartnerId && x.IsActive == true);


                                TblUniversalPriceMaster tblUniversalPriceMaster = new TblUniversalPriceMaster();
                                if (tblPriceMasterMapping != null)
                                {
                                    tblUniversalPriceMaster = _universalPriceMasterRepository.GetSingle(x =>
                                        x.PriceMasterNameId == tblPriceMasterMapping.PriceMasterNameId

                                        && x.ProductCategoryId == ProductCategory?.Id
                                        && x.ProductTypeId == ProductType?.Id
                                         && x.IsActive == true &&

                                       (x.BrandName1 == item.Brand ||
                                        x.BrandName2 == item.Brand ||
                                        x.BrandName3 == item.Brand ||
                                        x.BrandName4 == item.Brand ||
                                        x.OtherBrand == item.Brand));

                                    if (tblUniversalPriceMaster == null)
                                    {
                                        item.Remarks += "Price master not found for this order";
                                    }
                                }
                                else
                                {
                                    item.Remarks += "Price master not found for this order";
                                }
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

                if (ExchangeVM1.ExchangeVMList1 != null && ExchangeVM1.ExchangeVMList1.Count > 0)
                {
                    ExchangeVM1 = _exchangeOrderManager.ManageExchangeBulkUpload(ExchangeVM1, _loginSession.UserViewModel.UserId);
                    if (ExchangeVM1 != null)
                    {
                        if (ExchangeVM1.ExchangeVMErrorList1 != null && ExchangeVM1.ExchangeVMErrorList1.Count > 0)
                        {
                            fileResult = OnPostExportExcel_DataError(ExchangeVM1.ExchangeVMErrorList1, "ExchangeVMErrorList1");
                            replyMessage = ExchangeVM1.ExchangeVMErrorList1.Count + " Records Import Sucessfully and " + ExchangeVM1.ExchangeVMErrorList1.Count + " Records Not Import";
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
                _logging.WriteErrorToDB("Bulk_Order", "BulkUpload", ex);
            }
            if (fileResult != null)
            {
                return fileResult;
            }
            else
            {
                return RedirectToPage("./Bulk_Upload", new { id = _protector.Encode(ExchangeVM1.Id) });
            }
        }

        #endregion

        #region Product category type
        public JsonResult OnGetProductCategoryTypeAsync()
        {
            var productTypeList = _productTypeManager.GetProductTypeByCategoryId(Convert.ToInt32(searchFilterVM.ProductCatId));
            if (productTypeList != null)
            {
                ViewData["productTypeList"] = new SelectList(productTypeList, "Id", "Description");
            }
            return new JsonResult(productTypeList);
        }

        #endregion

    }
}
