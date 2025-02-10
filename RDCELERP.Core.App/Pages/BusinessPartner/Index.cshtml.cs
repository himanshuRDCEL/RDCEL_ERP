using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using RDCELERP.BAL.Interface;
using RDCELERP.BAL.MasterManager;
using RDCELERP.Common.Helper;
using RDCELERP.Core.App.Pages.Base;
using RDCELERP.DAL.Entities;
using RDCELERP.Model.Base;
using RDCELERP.Model.BusinessPartner;

namespace RDCELERP.Core.App.Pages.BusinessPartner
{
    public class IndexModel : CrudBasePageModel
    {

        private readonly RDCELERP.DAL.Entities.Digi2l_DevContext _context;
        private readonly IBusinessPartnerManager _BusinessPartnerManager;

        public IndexModel(RDCELERP.DAL.Entities.Digi2l_DevContext context, IBusinessPartnerManager BusinessPartnerManager, IOptions<ApplicationSettings> config, CustomDataProtection protector) : base(config, protector)
        {
            _context = context;
            _BusinessPartnerManager = BusinessPartnerManager;
        }

        [BindProperty(SupportsGet = true)]
        public BusinessPartnerViewModel BusinessPartnerVM { get; set; }

        [BindProperty(SupportsGet = true)]
        public IList<TblBusinessPartner> TblBusinessPartner { get; set; }
        [BindProperty(SupportsGet = true)]
        public TblBusinessPartner TblBusinessPartnerObj { get; set; }

        public FileResult OnPostExportExcel_DataError<T>(List<T> items, string filename)
        {
            bool flag = false;
            var Date = System.DateTime.Now;
            var dateTime = Date.ToString("MM-dd-yyyy_hh:mm");
            var FileName = filename + "_" + dateTime + ".xlsx";
            MemoryStream stream = ExcelExportHelper.ListExportToExcel(items);
            return (File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", FileName));
        }

        public IActionResult OnGet()
        {
            TblBusinessPartnerObj = new TblBusinessPartner();

            if (_loginSession == null)
            {
                return RedirectToPage("/index");
            }
            else
            {

                BusinessPartnerVM = _BusinessPartnerManager.GetBusinessPartnerById(_loginSession.UserViewModel.UserId);

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
                if (TblBusinessPartnerObj != null && TblBusinessPartnerObj.BusinessPartnerId > 0)
                {
                    TblBusinessPartnerObj = _context.TblBusinessPartners.Find(TblBusinessPartnerObj.BusinessPartnerId);
                }

                if (TblBusinessPartnerObj != null)
                {

                    TblBusinessPartnerObj.IsActive = false;
                    TblBusinessPartnerObj.ModifiedBy = _loginSession.UserViewModel.UserId;
                    _context.TblBusinessPartners.Update(TblBusinessPartnerObj);
                    //  _context.TblRoles.Remove(TblRole);
                    _context.SaveChanges();
                }

                return RedirectToPage("./index");
            }
        }
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
                            List<string>? labels = new List<string> { item?.LabelName_Excellent_P, item?.LabelName_Good_Q, item?.LabelName_Average_R, item?.LabelName_NonWorking_S };
                            var checklabels = labels.Where(x => !string.IsNullOrEmpty(x)).OrderBy(x => x.Length).ToList();

                            if (checklabels != null && checklabels.Count < 2)
                            {
                                var ErrorMsg = string.Join(Environment.NewLine, "You Need to add at least two product condition label");
                                item.Remarks += ErrorMsg + Environment.NewLine;
                            }
                        }
                    }
                }

                if (BusinessPartnerVM.BusinessPartnerVMList != null && BusinessPartnerVM.BusinessPartnerVMList.Count > 0)
                {
                    BusinessPartnerVM = _BusinessPartnerManager.ManageBusinessPartnerBulk(BusinessPartnerVM, _loginSession.UserViewModel.UserId);
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
                return RedirectToPage("/Index");
            }
        }

    }
}