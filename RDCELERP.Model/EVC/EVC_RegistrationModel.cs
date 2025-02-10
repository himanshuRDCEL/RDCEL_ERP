using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDCELERP.Model.Base;

namespace RDCELERP.Model.EVC
{
    public class EVC_RegistrationModel : BaseViewModel
    {
        [Key]
        public int EvcregistrationId { get; set; }
        public int EmployeeId { get; set; }      
        public string? EmployeeName { get; set; }     
        public string? EmployeeEMail { get; set; }
      
        [Required(ErrorMessage = "Bussiness name is required")]
        [Display(Name = "Bussiness Name")]
        [StringLength(50)]
        public string? BussinessName { get; set; }
        public int? EntityTypeId { get; set; }

        [Required(ErrorMessage = "Contact person is required")]
        [RegularExpression(@"^[A-Z]+[a-zA-Z\s]*$", ErrorMessage = "First letter should be Capital")]
        public string? ContactPerson { get; set; }

        [Required(ErrorMessage = "Person address is required")]
        public string? ContactPersonAddress { get; set; }

        [Required(ErrorMessage = "EVC mobile number is required")]
        [RegularExpression(@"^([0-9]{10})$", ErrorMessage = "Please Enter Valid 10 Digit Mobile Number.")]
        [Display(Name = "EVC Mobile Number")]
        [DataType(DataType.PhoneNumber)]
        [Phone] 
        public string? EVCMobileNumber { get; set; }


        [Required(ErrorMessage = " Email Id is required")]
        [RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$", ErrorMessage = "Please enter a valid e-mail adress")]
        public string? EmailId { get; set; }
         
        [RegularExpression(@"^([0-9]{10})$", ErrorMessage = "Please Enter Valid 10 Digit Mobile Number.")]
        [Display(Name = "Alternate Mobile Number")]
        [DataType(DataType.PhoneNumber)]
        [Phone]
        public string? AlternateMobileNumber { get; set; }

        [Required(ErrorMessage = "Address is required")]
        [Display(Name = "Regd Address Line 1")]
        [StringLength(maximumLength: 100)]
        public string? RegdAddressLine1 { get; set; }

        [Required(ErrorMessage = " Address are required")]
        [Display(Name = "Regd Address Line 2")]
        [StringLength(maximumLength: 100)]
        public string? RegdAddressLine2 { get; set; }

        [Required(ErrorMessage = "Bank name is required")]
        [RegularExpression(@"^[A-Z]+[a-zA-Z\s]*$", ErrorMessage = "First letter should be Capital")]
        [Display(Name = "Bank Name")]
        [StringLength(50)]
        public string? BankName { get; set; }

        [Required(ErrorMessage = " IFSC code is required")]
        [RegularExpression(@"^[A-Z]{4}0[A-Z0-9]{6}$", ErrorMessage = "Please Enter Valid 11 Digit IFSC Code ")]
        [Display(Name = "IFSC Code")]
        public string? IFSCCODE { get; set; }
       
        [RegularExpression(@"^[0-9]{9,18}$", ErrorMessage = "Please Enter Valid Bank Account No")]
        [Required(ErrorMessage = " Bank account no is required")]
        [Display(Name = "Bank Account No ")]
        public string? BankAccountNo { get; set; }

        public string? CopyofCancelledCheque { get; set; }
        public string? CopyofCancelledChequeLinkURL { get; set; }
       
        public string? CopyofCancelledChequeLinkURLBase64string { get; set; }


        public string? EwasteRegistrationNumber { get; set; }
      
        [Required(ErrorMessage = "Pin code is required")]
       
        [RegularExpression(@"^[1-9][0-9]{5}$", ErrorMessage = "Please Enter Valid 6 Digit Pincode.")]
        [Display(Name = "Pincode")]      
        public string? PinCode { get; set; }
        [Required(ErrorMessage = "State is required")]
        public int StateId { get; set; }
        [Required(ErrorMessage = "City is required")]
        public int cityId { get; set; }

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

        public string? ProfilePicLinkURLBase64string { get; set; }
        public string? POCName { get; set; }
        public string? POCPlace { get; set; }
        public DateTime Date { get; set; }
        public int? EvcapprovalStatusId { get; set; }
        public string? EvcregdNo { get; set; }
        public long? EvcwalletAmount { get; set; }
        public int UserId { get; set; }
        public bool IconfirmTermsCondition { get; set; }
        public bool? Isevcapprovrd { get; set; }
        public string? EvczohoBookName { get; set; }
        public string? EmployeeIdName { get; set; }
        [Required(ErrorMessage = "State is required")]
        public string? StateIdName { get; set; }
        [Required(ErrorMessage = "City is required")]
        public string? CityIdName { get; set; }
        public bool? IsInHouse { get; set; }
    }
}
