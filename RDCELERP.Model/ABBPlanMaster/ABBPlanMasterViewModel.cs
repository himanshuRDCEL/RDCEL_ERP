using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDCELERP.Model.Base;

namespace RDCELERP.Model.ABBPlanMaster
{
    public class ABBPlanMasterViewModel : BaseViewModel
    {
        public List<EntryViewModel> ListOfEntries { get; set; } = new List<EntryViewModel>();


        public int PlanMasterId { get; set; }
        [Required(ErrorMessage = "Business unit name is required.")]
        [DisplayName("Business Unit Name")]
        public string? BusinessUnitName { get; set; }
        public int? BusinessUnitId { get; set; }
      
        public string? Sponsor { get; set; }
        [Required(ErrorMessage = "From month is required.")]
        public int? FromMonth { get; set; }
        [Required(ErrorMessage = "To month is required.")]
        public int? ToMonth { get; set; }
        [Required(ErrorMessage = "Assured buy back percentage is required.")]
        [Range(1, 100, ErrorMessage = "Percentage value must be between 1 and 100.")]
        public int? AssuredBuyBackPercentage { get; set; }
        [RegularExpression(@"^(0|[1-9]\d*)$", ErrorMessage = "Please use numbers only.")]
        [Required(ErrorMessage = "Plan period in month is required.")]
        public int? PlanPeriodInMonth { get; set; }
        [Required(ErrorMessage = "Product category name is required.")]
        [DisplayName("Product Category Name")]
        public string? ProductCategoryName { get; set; }
        public int? ProductCatId { get; set; }
       
        [Required(ErrorMessage = "Product type name is required.")]
        [DisplayName("Product Type Name")]
        public string? ProductTypeName { get; set; }
        public int? ProductTypeId { get; set; }
       
        [Required(ErrorMessage = "Abb plan name is required.")]
        public string? AbbplanName { get; set; }
        [Range(3, 100, ErrorMessage = "No claim period must be between 3 to 100.")]
        [Required(ErrorMessage = "No claim period is required.")]
        public string? NoClaimPeriod { get; set; }
        public string? Date { get; set; }


        //ListOfEntries = new List<EntryViewModel>();

        public List<PlanDetails>? PlanList { get; set; }

        public ABBPlanMasterVMExcel? ABBPlanMasterVM { get; set; }
        public IFormFile? UploadABBPlanMaster { get; set; }
        public List<ABBPlanMasterVMExcel>? ABBPlanMasterVMList { get; set; }
        public List<ABBPlanMasterVMExcel>? ABBPlanMasterVMErrorList { get; set; }
    }

    public class Abbplandetail
    {
        public string? planprice { get; set; }
        public string? planduration { get; set; }
        public string? planName { get; set; }
        public string? NoClaimPeriod { get; set; }
        public string? Message { get; set; }
    }

    public class AbbRedemptionValue
    {
        public int RedemptionPeriod { get; set; }
        public int RedemptionPercentage { get; set; }
        public decimal RedemptionValue { get; set; }
        public int NoClaimPeriod { get; set; }

    }

    public class PlanDetails
    {
        public string PlanMasterId { get; set; }
        public string? fromMonth { get; set; }
        public string? toMonth { get; set; }
        public string? percentage { get; set; }
    }

    public class EntryViewModel
    {
        public int PlanMasterId { get; set; }
        public int? fromMonth { get; set; }
        public int? toMonth { get; set; }
        public int? percentage { get; set; }
    }

   

}
