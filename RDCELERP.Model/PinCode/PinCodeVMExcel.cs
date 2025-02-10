using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDCELERP.Model.PinCode
{
    public class PinCodeVMExcel
    {
        public int Id { get; set; }
        public string? ZohoPinCodeId { get; set; }
        [Required(ErrorMessage = "Pin code is required.")]
        [RegularExpression(@"^[1-9][0-9]{5}$", ErrorMessage = "Please enter valid 6 digit pin code.")]
        public int? PinCode { get; set; }
        [Required(ErrorMessage = "Location is required.")]
        public string? Location { get; set; }
        public string? HubControl { get; set; }
        [Required(ErrorMessage = "State is required.")]
        public string? State { get; set; }
        public bool? IsAbb { get; set; }
        public bool? IsExchange { get; set; }
        public string? Remarks { get; set; }
        public bool IsActive { get; set; }



    }
}
