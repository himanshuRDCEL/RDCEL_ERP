using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDCELERP.Model.AbbRegistration
{
    public class AbbRegistrationVMExcel
    {
        public string? YourRegistrationNo { get; set; }

        [Key]
        public int AbbregistrationId { get; set; }
        [Required(ErrorMessage = "Required")]
        public string? CompanyName { get; set; }
        [Required(ErrorMessage = "Required")]
        public string? Brand { get; set; }
        public string? StoreName { get; set; }
        [Required(ErrorMessage = "Required")]
        public string? StoreCode { get; set; }
        public string? StoreManagerEmail { get; set; }
        [Required(ErrorMessage = "Required")]
        public string? SponsorOrderNo { get; set; }

        //Customer Details

        public int CustomerId { get; set; }

        [Required(ErrorMessage = "Required")]
        [RegularExpression(@"^[a-zA-Z]+[a-zA-Z\s]*$", ErrorMessage = "Use letters only please")]
        [Display(Name = "Enter Customer FirstName")]
        public string? CustFirstName { get; set; }
        [Required(ErrorMessage = "Required")]
        public string? CustLastName { get; set; }

        [Required(ErrorMessage = "Required")]
        [RegularExpression(@"^([0-9]{10})$", ErrorMessage = "Please Enter Valid 10 Digit Mobile Number.")]
        [Display(Name = "Customer Mobile Number")]
        [DataType(DataType.PhoneNumber)]
        [Phone] public string? CustMobile { get; set; }

        [Required(ErrorMessage = "Required")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string? CustEmail { get; set; }


        [Required(ErrorMessage = "Required")]
        [Display(Name = "Customer Address Line 1")]
        [StringLength(maximumLength: 100)]
        public string? CustAddress1 { get; set; }

        [Display(Name = "Customer Address Line 1")]
        [StringLength(maximumLength: 100)]
        public string? CustAddress2 { get; set; }

        [Required(ErrorMessage = "Required")]
        [RegularExpression(@"^([0-9]{6})$", ErrorMessage = "Please Enter Valid 6 Digit Mobile Number.")]
        public string? CustPinCode { get; set; }
        public string? CustCity { get; set; }

        public string? CustState { get; set; }
        public int StateId { get; set; }
        public string? Location { get; set; }
        public int OrderId { get; set; }
        
        public string? NewProductCategoryName { get; set; }
        public string? NewProductType { get; set; }
        public string? Description { get; set; }
        [Required]
        public int? NewProductCategoryTypeId { get; set; }
        public string? NewSize { get; set; }
        public string? ProductSrNo { get; set; }
        public string? NewModelName { get; set; }
        
        public string? AbbplanName { get; set; }
       
        public DateTime InvoiceDate { get; set; }
        public string? InvoiceNo { get; set; }
        public decimal NewPrice { get; set; }
        public decimal Abbfees { get; set; }
        public decimal ProductNetPrice { get; set; }
        public string? AbbplanPeriod { get; set; }
        public int OrderType { get; set; }
        public string? NoOfClaimPeriod { get; set; }
        public DateTime UploadDateTime { get; set; }
        public string? Remarks { get; set; }
    }
}
