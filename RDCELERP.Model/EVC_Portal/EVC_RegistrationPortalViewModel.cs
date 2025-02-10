using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDCELERP.Model.Base;

namespace RDCELERP.Model.EVC_Portal
{
    public class EVC_RegistrationPortalViewModel : BaseViewModel
    {
        [Key]
        public int EvcregistrationId { get; set; }
        public int EmployeeId { get; set; }
        public int UserId { get; set; }
        public string? EmployeeName { get; set; }
        public string? EmployeeEMail { get; set; }
        [Required(ErrorMessage = "Required")]
        [Display(Name = "Bussiness Name")]
        [StringLength(50)]
        public string? BussinessName { get; set; }

        [Required(ErrorMessage = "Required")]
        [RegularExpression(@"^([0-9]{10})$", ErrorMessage = "Please Enter Valid 10 Digit Mobile Number.")]
        [Display(Name = "EVC Mobile Number")]
        [DataType(DataType.PhoneNumber)]
        [Phone] public string? EVCMobileNumber { get; set; }

        [Required(ErrorMessage = "Required")]
        [DataType(DataType.EmailAddress)]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string? EmailId { get; set; }
       

        [Required(ErrorMessage = "Required")]
        [RegularExpression(@"^([0-9]{10})$", ErrorMessage = "Please Enter Valid 10 Digit Mobile Number.")]
        [Display(Name = "Alternate Mobile Number")]
        [DataType(DataType.PhoneNumber)]
        [Phone]
        public string? AlternateMobileNumber { get; set; }

        [RegularExpression(@"^[A-Z]+[a-zA-Z\s]*$", ErrorMessage = "Use letters only please")]
        [Required(ErrorMessage = "Required")]
        [Display(Name = "Bank Name")]
        [StringLength(50)]
        public string? BankName { get; set; }

        [Required(ErrorMessage = "Required")]
        [Display(Name = "IFSC Code")]
        public string? IFSCCODE { get; set; }

        [Required(ErrorMessage = "Required")]
        [Display(Name = "Regd Address Line 1")]
        [StringLength(maximumLength: 100)]
        public string? RegdAddressLine1 { get; set; }
        public string? BankAccountNo { get; set; }

        public string? CopyofCancelledCheque { get; set; }
        public int? EntityTypeId { get; set; }
        public string? EwasteRegistrationNumber { get; set; }

        [Required(ErrorMessage = "Required")]
        [Display(Name = "Regd Address Line 2")]
        [StringLength(maximumLength: 100)]
        public string? RegdAddressLine2 { get; set; }
        [Required(ErrorMessage = "Required")]
        public string? PinCode { get; set; }
        [Required(ErrorMessage = "Required")]
        public int StateId { get; set; }
     
        public int cityId { get; set; }
        [Required(ErrorMessage = "Required")]
        public string? ContactPerson { get; set; }
        [Required(ErrorMessage = "Required")]
        public string? ContactPersonAddress { get; set; }
        [Required(ErrorMessage = "Required")]
        public string? Gstno { get; set; }
        public string? UploadGSTRegistration { get; set; }       
        public string? EWasteCertificate { get; set; }
        public string? POCName { get; set; }
        public string? POCPlace { get; set; }
        public DateTime Date { get; set; }
        public int? EvcapprovalStatusId { get; set; }
        public string? EvcregdNo { get; set; }
        public long? EvcwalletAmount { get; set; }
       
        public string? EvczohoBookName { get; set; }
        
      
    }
}
