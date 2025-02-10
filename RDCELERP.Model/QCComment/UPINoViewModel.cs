using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDCELERP.Model.Base;

namespace RDCELERP.Model.QCComment
{
   public class UPINoViewModel 
    {
        //[Required(ErrorMessage = "Required UPI ID")]
        [RegularExpression(@"^[\w.-]+@[\w.-]+$", ErrorMessage = "Invalid UPI Id")] 
        [Display(Name = "Customer UPI Number")]
        public string? UPIId { get; set; }
        //public int UPINo { get; set; }

        [Required(ErrorMessage = "Required Preferred Pickup Date")]
        public string? PreferredPickupDate { get; set; }

        [Required(ErrorMessage = "Required Preferred Pickup Time")]
        public string? PreferredPickupTime { get; set; }
        public string? Regdno { get; set; }
        public int? StatusId { get; set; }
        public int? Userid { get; set; }
        public int payId { get; set; }
        public string? PickupStartTime { get; set; }
        public string? PickupEndTime { get; set; }
        public string? VoucherType { get; set; }
        public string? CustomerName { get; set; }
        
        [RegularExpression(@"^[^\W\d_]+$", ErrorMessage = "Please Enter Valid 10 Digit Mobile Number.")]
        [Display(Name = "Customer Name")]
        public string? CustomerFirstName { get; set; }
        public string? CustomerLastName { get; set; }
        public int CustomerId { get; set; }
    }
}
