using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDCELERP.Model.ABBPriceMaster
{
    public class ABBPriceMasterVMExcel
    {
        public int PriceMasterId { get; set; }
        [Required(ErrorMessage = "Business unit name is required.")]
        [DisplayName("Business Unit Name")]

       
        public string? CompanyName { get; set; }

        public short FeeTypeId { get; set; }
        
        public string? Sponsor { get; set; }
        [Required(ErrorMessage = "Product category is required.")]

        public string? ProductCategory { get; set; }
        [Required(ErrorMessage = "Product sabcategory is required.")]

        public string? ProductSabcategory { get; set; }
        [Required(ErrorMessage = "Price start range is required.")]
        public int PriceStartRange { get; set; }
        [Required(ErrorMessage = "Price end range is required.")]
        public int PriceEndRange { get; set; }
        [Required(ErrorMessage = "Fee type is required.")]
        [RegularExpression(@"^[A-Za-z][a-zA-Z\s]*$", ErrorMessage = "Use letters only please")]
        public string? FeeType { get; set; }
        [Required(ErrorMessage = "Fees applicable Amount is required.")]

        public int FeesApplicableAmt { get; set; }
        [Required(ErrorMessage = "Fees applicable percentage is required.")]
        [Range(1, 100, ErrorMessage = "Percentage value must be between 1 and 100.")]

        public decimal FeesApplicablePercentage { get; set; }
        [Required(ErrorMessage = "Plan period in months is required.")]
        public int PlanPeriodInMonths { get; set; }

        public decimal BusinessUnitMarginAmount { get; set; }
        
        public decimal BusinessUnitMarginPerc { get; set; }
        
        public decimal BusinessPartnerMarginAmount { get; set; }
       
        public decimal BusinessPartnerMarginPerc { get; set; }
        
        public decimal Gstexclusive { get; set; }
      
        public decimal Gstinclusive { get; set; }
        public decimal GstValueForNewProduct { get; set; }

        public string? Remarks { get; set; }

    }
}
