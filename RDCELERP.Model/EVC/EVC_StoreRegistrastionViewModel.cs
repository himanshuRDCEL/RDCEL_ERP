using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDCELERP.Model.Base;

namespace RDCELERP.Model.EVC
{
    public class EVC_StoreRegistrastionViewModel: BaseViewModel
    {

        public int EvcPartnerId { get; set; }
        public int EvcregistrationId { get; set; }
        [Required(ErrorMessage = "Required")]
        public string? Address1 { get; set; }
        [Required(ErrorMessage = "Required")]
        public string? Address2 { get; set; }

        [Required(ErrorMessage = "Pin code is required.")]
        [RegularExpression(@"^[1-9][0-9]{5}$", ErrorMessage = "Please enter valid 6 digit pin code.")]
        [DisplayName("PinCode")]
        public string? PinCode { get; set; }

        [Required(ErrorMessage = "City is required.")]
        [DisplayName("City Name")]
        public string? City { get; set; }
        public int CityId { get; set; }

        [Required(ErrorMessage = "State is require.")]
        [DisplayName("State Name")]
        public string? State { get; set; }
        public int StateId { get; set; }
       
        [Required]
        [RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$", ErrorMessage = "Please enter a valid e-mail adress")]
        public string? EmailId { get; set; }

        [Required(ErrorMessage = "Required")]
        [RegularExpression(@"^([0-9]{10})$", ErrorMessage = "Please Enter Valid 10 Digit Mobile Number.")]
        [Display(Name = "Store Mobile Number")]
        [DataType(DataType.PhoneNumber)]
        [Phone]
        public string? ContactNumber { get; set; }
        public string? EvcStoreCode { get; set; }
        public string? ListOfPincode { get; set; }
        public int? userId { get; set; }

        public bool? IsApprove { get; set; }


    }

    public class EVCStore_SpecificationViewModel : BaseViewModel
    {
       
        public int EvcPartnerpreferenceId { get; set; }
        [Required(ErrorMessage = "Required")]
        public int EvcPartnerId { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "select product category")]
        public int productcatId { get; set; }
        [Required(ErrorMessage = "Product category Name is require.")]
        [DisplayName("Product category Name")]
        public string? ProductcatName { get; set; }
        [Required(ErrorMessage = "Quality is require.")]
        [DisplayName("Quality")]
        public string[]? Quality { get; set; }
    }

   
}
