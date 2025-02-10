using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDCELERP.Model.Base;

namespace RDCELERP.Model.MobileApplicationModel.EVC
{
    public class EVCResellerRegisterationModel: BaseViewModel
    {
        
        public string? EmployeeName { get; set; }
       
        [Required(ErrorMessage = "Required")]
        [Display(Name = "Bussiness Name")]
        [StringLength(50)]
        public string? BussinessName { get; set; }
        //public int? EntityTypeId { get; set; }

        [Required(ErrorMessage = "Required")]
        [RegularExpression(@"^[A-Z]+[a-zA-Z\s]*$", ErrorMessage = "Use letters only please")]
        public string? ContactPerson { get; set; }

       // [Required(ErrorMessage = "Required")]
        public string? ContactPersonAddress { get; set; }

        
        [Required(ErrorMessage = "Required")]
        [RegularExpression(@"^([0-9]{10})$", ErrorMessage = "Please Enter Valid 10 Digit Mobile Number.")]
        [Display(Name = "EVC Mobile Number")]
        [DataType(DataType.PhoneNumber)]
        [Phone]
        public string? EVCMobileNumber { get; set; }


        [Required]
        [RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$", ErrorMessage = "Please enter a valid e-mail adress")]
        // [Remote("IsAlreadySignedUpStudent", "Register", ErrorMessage = "EmailId already exists in database.")]
        public string? EmailId { get; set; }

        [RegularExpression(@"^([0-9]{10})$", ErrorMessage = "Please Enter Valid 10 Digit Mobile Number.")]
        [Display(Name = "Alternate Mobile Number")]
        [DataType(DataType.PhoneNumber)]
        [Phone]
        public string? AlternateMobileNumber { get; set; }

        [Required(ErrorMessage = "Required")]
        [Display(Name = "Regd Address Line 1")]
        [StringLength(maximumLength: 100)]
        public string? RegdAddressLine1 { get; set; }

        [Required(ErrorMessage = "Required")]
        [Display(Name = "Regd Address Line 2")]
        [StringLength(maximumLength: 100)]
        public string? RegdAddressLine2 { get; set; }

        [RegularExpression(@"^[A-Z]+[a-zA-Z\s]*$", ErrorMessage = "Use letters only please")]
        //[Required(ErrorMessage = "Required")]
        [Display(Name = "Bank Name")]
        [StringLength(50)]
        public string? BankName { get; set; }

        [RegularExpression(@"^[A-Z]{4}0[A-Z0-9]{6}$", ErrorMessage = "Please Enter Valid 11 Digit IFSC Code ")]
        //[Required(ErrorMessage = "Required")]
        [Display(Name = "IFSC Code")]
        public string? IFSCCODE { get; set; }

        [RegularExpression(@"^[0-9]{9,18}$", ErrorMessage = "Please Enter Valid Bank Account No")]

        //[Required(ErrorMessage = "Required")]
        [Display(Name = "Bank Account No ")]
        public string? BankAccountNo { get; set; }

        
        public string? CopyofCancelledCheque { get; set; }
        public string? CopyofCancelledChequeLinkURL { get; set; }

        public string? CopyofCancelledChequeLinkURLBase64string { get; set; }


        public string? EwasteRegistrationNumber { get; set; }

        [Required(ErrorMessage = "Required")]

        [RegularExpression(@"^[1-9][0-9]{5}$", ErrorMessage = "Please Enter Valid 6 Digit Pincode.")]
        [Display(Name = "Pincode")]
        public string? PinCode { get; set; }
        [Required(ErrorMessage = "Required")]
        public int StateId { get; set; }
        //[Required(ErrorMessage = "Required")]
        public int cityId { get; set; }



        // [Required(ErrorMessage = "Required")]
        [RegularExpression(@"^[0-9]{2}[A-Z]{5}[0-9]{4}[A-Z]{1}[1-9A-Z]{1}Z[0-9A-Z]{1}$", ErrorMessage = "Please Enter Valid GSTNo")]
        public string? GSTNo { get; set; }

        public string? UploadGSTRegistration { get; set; }
        public string? UploadGSTRegistrationLinkURL { get; set; }

        public string? UploadGSTRegistrationLinkURLBase64string { get; set; }

        public string? EWasteCertificate { get; set; }
        public string? EWasteCertificateLinkURL { get; set; }

        public string? EWasteCertificateLinkURLBase64string { get; set; }

        public string? AadharfrontImage { get; set; }
        public string? AadharfrontImageLinkURL { get; set; }

        public string? AadharfrontImageLinkURLBase64string { get; set; }


        public string? AadharBackImage { get; set; }
        public string? AadharBackImageLinkURL { get; set; }

        public string? AadharBackImageLinkURLBase64string { get; set; }


        public string? ProfilePic { get; set; }
        public string? ProfilePicLinkURL { get; set; }

        public string ? ProfilePicLinkURLBase64string { get; set; }



        public string? POCName { get; set; }
        public string? POCPlace { get; set; }
        public DateTime Date { get; set; }
        //public int? EvcapprovalStatusId { get; set; }
        public string ?EvcregdNo { get; set; }
        public long? EvcwalletAmount { get; set; }
        public int UserId { get; set; }

        public bool IconfirmTermsCondition { get; set; }
        public bool? Isevcapprovrd { get; set; }
        public string? EvczohoBookName { get; set; }
    }
}
