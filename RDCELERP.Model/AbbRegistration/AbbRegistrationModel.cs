using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDCELERP.Model.Base;

namespace RDCELERP.Model.AbbRegistration
{
    public class AbbRegistrationModel : BaseViewModel
    {
        public string? YourRegistrationNo { get; set; }

        [Key]
        public int AbbregistrationId { get; set; }
        public string? ZohoAbbregistrationId { get; set; }
        public int? BusinessUnitId { get; set; }
        public string? SponsorName { get; set; }
        public string? BrandName { get; set; }
        public int NewBrandId { get; set; }//
        public int? BusinessPartnerId { get; set; }//dealerid
        public string? StoreName { get; set; }
        public string? StoreCode { get; set; }
        public string? StoreManagerEmail { get; set; }
        public string? RegdNo { get; set; }
        public string? SponsorOrderNo { get; set; }

        //Customer Details

        public int CustomerId { get; set; }

        [Required(ErrorMessage = "Required")]
        [StringLength(maximumLength: 50)]
        [RegularExpression(@"^[a-zA-Z]+[a-zA-Z\s]*$", ErrorMessage = "Use letters only please")]
        [Display(Name = "Enter Customer FirstName")]
        public string? CustFirstName { get; set; }
        [StringLength(maximumLength: 50)]
        public string? CustLastName { get; set; }

        [Required(ErrorMessage = "Required")]
        [RegularExpression(@"^([0-9]{10})$", ErrorMessage = "Please Enter Valid 10 Digit Mobile Number.")]
        [Display(Name = "Customer Mobile Number")]
        [DataType(DataType.PhoneNumber)]
        [Phone] public string? CustMobile { get; set; }

        [Required(ErrorMessage = "Required")]
        [StringLength(maximumLength: 100)]

        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string? CustEmail { get; set; }

        [Required(ErrorMessage = "Required")]
        [Display(Name = "Customer Address Line 1")]
        [StringLength(maximumLength: 100)]
        public string? CustAddress1 { get; set; }

        [Display(Name = "Customer Address Line 2")]
        [StringLength(maximumLength: 100)]
        public string? CustAddress2 { get; set; }

        [Required(ErrorMessage = "Required")]
        [RegularExpression(@"^([0-9]{6})$", ErrorMessage = "Please Enter Valid 6 Digit Mobile Number.")]
        public string? CustPinCode { get; set; }
        [StringLength(50)]
        public string? CustCity { get; set; }

        public string? CustState { get; set; }
        public int StateId { get; set; }
        public string? Location { get; set; }
        public int OrderId { get; set; }

        [Required]
        public int? NewProductCategoryId { get; set; }
        public string? NewProductCategoryName { get; set; }
        public string? NewProductCategoryType { get; set; }
        public string? Description { get; set; }
        [Required]
        public int? NewProductCategoryTypeId { get; set; }
        public string? NewSize { get; set; }
        [StringLength(maximumLength: 50)]
        [RegularExpression("^[a-zA-Z0-9]*$", ErrorMessage = "Only Alphabets and Numbers allowed.")]
        public string? ProductSrNo { get; set; }
        public string? ModelName { get; set; }
        public int? ModelNumberId { get; set; }
        public string? AbbplanName { get; set; }
        public string? Hsncode { get; set; }
        public DateTime? InvoiceDate { get; set; }
        [StringLength(maximumLength: 50)]
        public string? InvoiceNo { get; set; }
        public decimal? NewPrice { get; set; }
        public decimal? Abbfees { get; set; }
        [Range(0, 9999999)]
        public decimal? ProductNetPrice { get; set; }
        public string? AbbplanPeriod { get; set; }
        public int OrderType { get; set; }
        public string? InvoiceImage { get; set; }
        public string? UploadImage { get; set; }
        public string? SponsorProgCode { get; set; }
        public string? NoOfClaimPeriod { get; set; }
        public int? AbbpriceId { get; set; }
        public DateTime? UploadDateTime { get; set; }
        public bool? OtherCommunications { get; set; }
        public bool? FollowupCommunication { get; set; }
        public bool? AbbApprove { get; set; }
        public bool? AbbReject { get; set; }
        public string? InvoiceImageWithPath { get; set; }
        public IFormFile? Image { get; set; }
        public string? FileName { get; set; }
        public string? Base64StringValue { get; set; }

        public int? StatusId { get; set; }
        public string? CustFullName { get; set; }
        public string? CompanyName { get; set; }
        public string? RegistrationDate { get; set; }
        public decimal? DealerMargin { get; set; }
        public decimal? BusinessUnitMargin { get; set; }
        public string? AreaLocality { get; set; }
        public decimal? BaseValue { get; set; }
        public decimal? Cgst { get; set; }
        public decimal? Sgst { get; set; }
        public bool IsModelDetailRequired { get; set; }


    }
}
