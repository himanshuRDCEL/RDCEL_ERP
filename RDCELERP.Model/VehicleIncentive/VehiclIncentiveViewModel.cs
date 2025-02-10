using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDCELERP.Model.Base;

namespace RDCELERP.Model.VehicleIncentive
{
    public class VehicleIncentiveViewModel : BaseViewModel
    {
        public int IncentiveId { get; set; }
        public int? ProductCategoryId { get; set; }
        [Required(ErrorMessage = "Product category is required.")]
        public string? ProductCategoryName { get; set; }
        public int? ProductTypeId { get; set; }
        [Required(ErrorMessage = "Product type is required.")]
        public string? ProductTypeName { get; set; }
        [Required(ErrorMessage = "Company is required.")]
        public string? BusinessUnitName { get; set; }
        public int? BusinessUnitId { get; set; }
        [Required(ErrorMessage = "Base price is required.")]
        [RegularExpression(@"^\d+(\.\d{1,2})?$", ErrorMessage = "Value must be a positive number")]
        [StringLength(10)]
        public decimal? BasePrice { get; set; }
        [Required(ErrorMessage = "Pickup time is required.")]
        [RegularExpression(@"^\d+(\.\d{1,2})?$", ErrorMessage = "Value must be a positive number")]
        [StringLength(10)]
        public TimeSpan? PickupTatinHr { get; set; }
        [Required(ErrorMessage = "Pickup incentive amount is required.")]
        [RegularExpression(@"^\d+(\.\d{1,2})?$", ErrorMessage = "Value must be a positive number")]
        [StringLength(3)]
        public decimal? PickupIncAmount { get; set; }
        [Required(ErrorMessage = "Drop time is required.")]
        [RegularExpression(@"^\d+(\.\d{1,2})?$", ErrorMessage = "Value must be a positive number")]
        [StringLength(3)]
        public TimeSpan? DropTatinHr { get; set; }
        [Required(ErrorMessage = "Drop incentive amount is required.")]
        [RegularExpression(@"^\d+(\.\d{1,2})?$", ErrorMessage = "Value must be a positive number")]
        [StringLength(10)]
        public decimal? DropIncAmount { get; set; }
        [RegularExpression(@"^\d+(\.\d{1,2})?$", ErrorMessage = "Value must be a positive number")]
        [StringLength(10)]
        [DisplayName ("Packaging Incentive")]
        public decimal? PackagingIncentive { get; set; }
        [RegularExpression(@"^\d+(\.\d{1,2})?$", ErrorMessage = "Value must be a positive number")]
        [StringLength(10)]
        [DisplayName("Drop Image Incentive")]
        public decimal? DropImageIncentive { get; set; }
        public string? Date { get; set; }
        
        public string? PickupTat { get; set; }
       
        public string? DropTat { get; set; }
        public IFormFile? UploadVehicleIncentive { get; set; }
        public VehicleIncentiveVMExcel? VehicleIncentiveVM { get; set; }
        public List<VehicleIncentiveVMExcel>? VehicleIncentiveVMList { get; set; }
        public List<VehicleIncentiveVMExcel>? VehicleIncentiveVMErrorList { get; set; }
    }
}
