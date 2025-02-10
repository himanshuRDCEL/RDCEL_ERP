using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDCELERP.Model.VehicleIncentive
{
    public class VehicleIncentiveVMExcel
    {
        public int IncentiveId { get; set; }
        [Required(ErrorMessage = "Product category is required.")]
        public string? ProductCategory { get; set; }
        [Required(ErrorMessage = "Product type is required.")]
        public string? ProductType { get; set; }
        [Required(ErrorMessage = "Business unit is required.")]
        public string? Company { get; set; }
        [Required(ErrorMessage = "Base price is required.")]
        public decimal BasePrice { get; set; }
        [Required(ErrorMessage = "Pickup tat is required.")]
        public string? PickupTatinHr { get; set; }
        [Required(ErrorMessage = "Pickup inc amount is required.")]
        public decimal PickupIncAmount { get; set; }
        [Required(ErrorMessage = "Drop tat is required.")]
        public string? DropTatinHr { get; set; }
        [Required(ErrorMessage = "Drop inc amount is required.")]
        public decimal DropIncAmount { get; set; }
        public decimal PackagingIncentive { get; set; }
        public decimal DropImageIncentive { get; set; }
        public string? Remarks { get; set; }
    }
}
