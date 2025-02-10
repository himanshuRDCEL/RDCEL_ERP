using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using RDCELERP.BAL.Interface;
using RDCELERP.BAL.MasterManager;
using RDCELERP.Common.Helper;
using RDCELERP.Core.App.Pages.Base;
using RDCELERP.DAL.Entities;
using RDCELERP.DAL.IRepository;
using RDCELERP.Model.Base;
using RDCELERP.Model.ModelNumber;
using RDCELERP.Model.UniversalPriceMaster;

namespace RDCELERP.Core.App.Pages.ModelNumber
{
    public class IndexModel : CrudBasePageModel
    {
        private readonly RDCELERP.DAL.Entities.Digi2l_DevContext _context;
        private readonly IModelNumberManager _modelNumberManager;
        private readonly IProductCategoryManager _productCategoryManager;
        private readonly IProductTypeManager _productTypeManager;
        private readonly IModelNumberRepository _modelNumberRepository;
        public IndexModel(RDCELERP.DAL.Entities.Digi2l_DevContext context, IProductTypeManager productTypeManager, IProductCategoryManager productCategoryManager, IModelNumberManager modelNumberManager, IModelNumberRepository modelNumberRepository, IOptions<ApplicationSettings> config, CustomDataProtection protector) : base(config, protector)
        {
            _context = context;
            _modelNumberManager = modelNumberManager;
            _productTypeManager = productTypeManager;
            _productCategoryManager = productCategoryManager;
            _modelNumberRepository = modelNumberRepository;
        }

        [BindProperty(SupportsGet = true)]
        public ModelNumberViewModel ModelNumberVM { get; set; }

        [BindProperty(SupportsGet = true)]
        public IList<TblModelNumber> TblModelNumber { get; set; }
        [BindProperty(SupportsGet = true)]
        public TblModelNumber TblModelNumberObj { get; set; }
        public IActionResult OnGet()
        {
            TblModelNumberObj = new TblModelNumber();
            var ProductGroup = _productCategoryManager.GetAllProductCategory();
            if (ProductGroup != null)
            {
                ViewData["ProductGroup"] = new SelectList(ProductGroup, "Id", "Description");
            }

            if (_loginSession == null)
            {
                return RedirectToPage("/index");
            }
            else
            {
                ModelNumberVM = _modelNumberManager.GetModelNumberById(_loginSession.UserViewModel.UserId);

                return Page();
            }
        }

        public IActionResult OnPostDeleteAsync()
        {
            if (_loginSession == null)
            {
                return RedirectToPage("/index");
            }
            else
            {
                if (TblModelNumberObj != null && TblModelNumberObj.ModelNumberId > 0)
                {
                    TblModelNumberObj = _context.TblModelNumbers.Find(TblModelNumberObj.ModelNumberId);
                }

                if (TblModelNumberObj != null)
                {
                    if(TblModelNumberObj.IsActive == true)
                    {
                        TblModelNumberObj.IsActive = false;
                    }
                    else
                    {
                        TblModelNumberObj.IsActive = true;
                    }
                   
                    TblModelNumberObj.ModifiedBy = _loginSession.UserViewModel.UserId;
                    _context.TblModelNumbers.Update(TblModelNumberObj);
                    //  _context.TblRoles.Remove(TblRole);
                    _context.SaveChanges();
                }

                return RedirectToPage("./index");
            }


        }
        #region Product category type
        public JsonResult OnGetProductCategoryTypeAsync()
        {
            var productTypeList = _productTypeManager.GetProductTypeByCategoryId(Convert.ToInt32(ModelNumberVM.ProductCategoryId));
            if (productTypeList != null)
            {
                ViewData["productTypeList"] = new SelectList(productTypeList, "Id", "Description");
            }
            return new JsonResult(productTypeList);
        }

        #endregion

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
                            var BusinessUnit = _context.TblBusinessUnits.Where(x => x.Name == item.CompanyName && x.IsActive == true).FirstOrDefault();
                            var Check = _modelNumberRepository.GetSingle(x => x.ModelName == item.ModelName && x.IsActive == true && x.BusinessUnitId == BusinessUnit?.BusinessUnitId);
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
                return RedirectToPage("./index", new { id = _protector.Encode(ModelNumberVM.ModelNumberId) });
            }
        }

        #endregion

    }
}



    
