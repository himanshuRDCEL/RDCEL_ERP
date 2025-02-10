
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
using RDCELERP.Common.Helper;
using RDCELERP.Core.App.Pages.Base;
using RDCELERP.DAL.Entities;
using RDCELERP.DAL.IRepository;
using RDCELERP.Model.Base;
using RDCELERP.Model.City;
using RDCELERP.Model.State;
using RDCELERP.Model.UniversalPriceMaster;

namespace RDCELERP.Core.App.Pages.UniversalPriceMaster
{
    public class IndexModel : CrudBasePageModel
    {
        private readonly RDCELERP.DAL.Entities.Digi2l_DevContext _context;
        private readonly IUniversalPriceMasterManager _universalPriceMasterManager;
        private readonly IBusinessPartnerRepository _businessPartnerRepository;

        public IndexModel(RDCELERP.DAL.Entities.Digi2l_DevContext context, IUniversalPriceMasterManager universalPriceMasterManager, IBusinessPartnerRepository businessPartnerRepository, IOptions<ApplicationSettings> config, CustomDataProtection protector) : base(config, protector)
        {
            _context = context;
            _universalPriceMasterManager = universalPriceMasterManager;
            _businessPartnerRepository = businessPartnerRepository;
        }

        [BindProperty(SupportsGet = true)]
        public UniversalPriceMasterViewModel   UniversalPriceMasterVM { get; set; }
        [BindProperty(SupportsGet = true)]
        public UniversalPriceMasterExcel UniversalPriceMasterExcel { get; set; }
        [BindProperty(SupportsGet = true)]
        public StateVMExcel StateVMExcel { get; set; }
        [BindProperty(SupportsGet = true)]
        public IList<RDCELERP.DAL.Entities.TblUniversalPriceMaster> TblUniversalPriceMaster { get; set; }
        [BindProperty(SupportsGet = true)]
        public RDCELERP.DAL.Entities.TblUniversalPriceMaster UniversalPriceMasterObj { get; set; }
        public IActionResult OnGet()
        {
            UniversalPriceMasterObj = new RDCELERP.DAL.Entities.TblUniversalPriceMaster();
            var BusinessUnit = _context.TblBusinessUnits.Where(x => x.IsActive == true);

            if (BusinessUnit != null)
            {
                ViewData["BusinessUnit"] = new SelectList(BusinessUnit, "BusinessUnitId", "Name");

            }

            if (_loginSession == null)
            {
                return RedirectToPage("/index");
            }
            else
            {
                UniversalPriceMasterVM = _universalPriceMasterManager.GetUniversalPriceMasterById  (_loginSession.UserViewModel.UserId);

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
                if (UniversalPriceMasterObj != null && UniversalPriceMasterObj.PriceMasterUniversalId > 0)
                {
                    UniversalPriceMasterObj = _context.TblUniversalPriceMasters.Find(UniversalPriceMasterObj.PriceMasterUniversalId);
                }

                if (UniversalPriceMasterObj != null)
                {
                    if(UniversalPriceMasterObj.IsActive == true)
                    {
                        UniversalPriceMasterObj.IsActive = false;
                    }
                    else
                    {
                        UniversalPriceMasterObj.IsActive = true;
                    }
                    UniversalPriceMasterObj.ModifiedBy = _loginSession.UserViewModel.UserId;
                    _context.TblUniversalPriceMasters.Update(UniversalPriceMasterObj);
                    //  _context.TblRoles.Remove(TblRole);
                    _context.SaveChanges();
                }

                return RedirectToPage("./index");
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
                return RedirectToPage("./index", new { id = _protector.Encode(UniversalPriceMasterVM.PriceMasterUniversalId) });
            }
        }

        #endregion

    }
}

