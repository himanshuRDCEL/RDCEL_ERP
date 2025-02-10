using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDCELERP.Model.Base;

namespace RDCELERP.Model.ExchangeOrder
{
  public   class CustomerDetailViewModel : BaseViewModel
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Required")]
        //[RegularExpression(@"^\w+\s\w+$", ErrorMessage = "Use letters only please")]
        [Display(Name = "Enter Customer Name")]
        public string? CustName { get; set; }
        [Display(Name = "Enter Customer FirstName")]
        public string? FirstName { get; set; }
        public string? LastName { get; set; }

        [Required(ErrorMessage = "Required")]
        [RegularExpression(@"^([0-9]{10})$", ErrorMessage = "Please Enter Valid 10 Digit Mobile Number.")]
        [Display(Name = "Customer Mobile Number")]
        [DataType(DataType.PhoneNumber)]
        [Phone] public string? PhoneNumber { get; set; }

        [Required(ErrorMessage = "Required")]
        [RegularExpression(@"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string? Email { get; set; }

        //[Required(ErrorMessage = "Required")]
        [Display(Name = "Customer Address Line 1")]        
        public string? Address2 { get; set; }
        [Required(ErrorMessage = "Required")]
        [Display(Name = "Customer Address Line 2")]       
        public string? Address1 { get; set; }

        [Required(ErrorMessage = "Required")]
        //[RegularExpression(@"^([0-9]{10})$", ErrorMessage = "Please Enter Valid Pincode Number")]
        //[RegularExpression(@"^([0-9]{8})$", ErrorMessage = "Please Enter Valid Pincode Number.")]
        public string? ZipCode { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? AreaLocality { get; set; }
        public string? CustomerName { get; set; }
        public int? AreaLocalityId { get; set; }
    }


}
