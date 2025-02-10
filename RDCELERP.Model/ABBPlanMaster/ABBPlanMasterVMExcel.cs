using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDCELERP.Model.ABBPlanMaster
{
    public class ABBPlanMasterVMExcel
    {
        public int PlanMasterId { get; set; }
        [Required(ErrorMessage = "Business unit name is required.")]
        [DisplayName("Business Unit Name")]
       
        public string? CompanyName { get; set; }
       
        public string? Sponsor { get; set; }
        [Required(ErrorMessage = "From month is required.")]
        public int FromMonth { get; set; }
        [Required(ErrorMessage = "To month is required.")]
        public int ToMonth { get; set; }
        [Required(ErrorMessage = "Assured buy back percentage is required.")]
        [Range(1, 100, ErrorMessage = "Percentage value must be between 1 and 100.")]
        public int AssuredBuyBackPercentage { get; set; }
        [Required(ErrorMessage = "Plan period in month is required.")]
        public int PlanPeriodInMonth { get; set; }
        [Required(ErrorMessage = "Product category name is required.")]
        [DisplayName("Product Category Name")]
       
        public string? ProductCategoryName { get; set; }
        [Required(ErrorMessage = "Product type name is required.")]
        [DisplayName("Product Type Name")]
       
        public string? ProductTypeName { get; set; }
        [Required(ErrorMessage = "Abb plan name is required.")]
        public string? AbbplanName { get; set; }
        [Required(ErrorMessage = "No claim period is required.")]
        public string? NoClaimPeriod { get; set; }
       
        public string? Remarks { get; set; }

       

    }
}
