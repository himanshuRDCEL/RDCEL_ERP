using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDCELERP.Model.Base;

namespace RDCELERP.Model.ABBPriceMaster
{


    public class ABBPriceMasterViewModel : BaseViewModel
    {
        public int PriceMasterId { get; set; }
        [Required(ErrorMessage = "Company name is required.")]
        [DisplayName("Business Unit Name")]
        public string? BusinessUnitName { get; set; }
        public int? BusinessUnitId { get; set; }
        public short? FeeTypeId { get; set; }
        public string? Sponsor { get; set; }

        [Required(ErrorMessage = "Product category is required.")]
        public string? ProductCategory { get; set; }

        [Required(ErrorMessage = "Product sabcategory is required.")]
        public string? ProductSabcategory { get; set; }

        [RegularExpression(@"^\d+(\.\d{1,2})?$", ErrorMessage = "Value must be a positive number")]
        [Required(ErrorMessage = "Price start range is required.")]
        public int? PriceStartRange { get; set; }

        [Required(ErrorMessage = "Price end range is required.")]
        [RegularExpression(@"^\d+(\.\d{1,2})?$", ErrorMessage = "Value must be a positive number")]
        public int? PriceEndRange { get; set; }
       
        [Required(ErrorMessage = "Fee type is required.")]
        [RegularExpression(@"^[A-Za-z][a-zA-Z\s]*$", ErrorMessage = "Use letters only please")]
        public string? FeeType { get; set; }

        [Required(ErrorMessage = "Fees applicable amount is required.")]
        [RegularExpression(@"^\d+(\.\d{1,2})?$", ErrorMessage = "Value must be a positive number")]
        public int? FeesApplicableAmt { get; set; }

        [Required(ErrorMessage = "Fees applicable percentage is required.")]
        [Range(1, 100, ErrorMessage = "Percentage value must be between 1 and 100.")]
        public decimal? FeesApplicablePercentage { get; set; }

        [Required(ErrorMessage = "Plan period in months is required.")]
        public int? PlanPeriodInMonths { get; set; }

        [DisplayName("business unit margin amount")]
        [RegularExpression(@"^\d+(\.\d{1,2})?$", ErrorMessage = "Value must be a positive number")]
        public decimal? BusinessUnitMarginAmount { get; set; }

        [Range(1, 100, ErrorMessage = "Percentage value must be between 1 and 100.")]
        [DisplayName("business unit margin percentage")]
        public decimal? BusinessUnitMarginPerc { get; set; }

        [DisplayName("business partner margin amount")]
        [RegularExpression(@"^\d+(\.\d{1,2})?$", ErrorMessage = "Value must be a positive number")]
        public decimal? BusinessPartnerMarginAmount { get; set; }

        [Range(1, 100, ErrorMessage = "Percentage value must be between 1 and 100.")]
        [DisplayName("business partner pargin percentage")]
        public decimal? BusinessPartnerMarginPerc { get; set; }

        [RegularExpression(@"^\d+(\.\d{1,2})?$", ErrorMessage = "Value must be a positive number")]
        [DisplayName("Gst exclusive")]
        public decimal? Gstexclusive { get; set; }

        [RegularExpression(@"^\d+(\.\d{1,2})?$", ErrorMessage = "Value must be a positive number")]
        [DisplayName("Gst inclusive")]
        public decimal? Gstinclusive { get; set; }
            
        public decimal? GstValueForNewProduct { get; set; }
        public int? ProductCategoryId { get; set; }
        public string? Date { get; set; }
        public IFormFile? UploadABBPlanMaster { get; set; }
        public IFormFile? UploadABBPriceMaster { get; set; }
        public ABBPriceMasterVMExcel? ABBPriceMasterVM { get; set; }
        public List<ABBPriceMasterVMExcel>? ABBPriceMasterVMList { get; set; }
        public List<ABBPriceMasterVMExcel>? ABBPriceMasterVMErrorList { get; set; }
    }
}
