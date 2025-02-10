using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDCELERP.Model.Base;
using RDCELERP.Model.PinCode;
using RDCELERP.Model.State;

namespace RDCELERP.Model.MobileApplicationModel.LGC
{
    public class RegisterServicePartnerDataModel : BaseViewModel
    {
        public int ServicePartnerId { get; set; }

        [Required(ErrorMessage = "Required")]
        [Display(Name = "Service Partner Name")]
        [StringLength(50)]
        public string? ServicePartnerName { get; set; }

        [Required(ErrorMessage = "Required")]
        [Display(Name = "Service Partner First Name")]
        [StringLength(50)]
        public string? ServicePartnerFirstName { get; set; }

        [Required(ErrorMessage = "Required")]
        [Display(Name = "Service Partner Last Name")]
        [StringLength(50)]
        public string? ServicePartnerLastName { get; set; }

        [Required(ErrorMessage = "Required")]
        [Display(Name = "Service Partner Business Name")]
        [StringLength(50)]
        public string? ServicePartnerBusinessName { get; set; }

        [Required(ErrorMessage = "Required")]
        [Display(Name = "Service Partner Description")]
        [StringLength(255)]
        public string? ServicePartnerDescription { get; set; }
        
        public bool? IsServicePartnerLocal { get; set; }
        public int? UserId { get; set; }

        public string? ServicePartnerRegdNo { get; set; }

        [Required(ErrorMessage = "Required")]
        [RegularExpression(@"^([0-9]{10})$", ErrorMessage = "Please Enter Valid 10 Digit Mobile Number.")]
        [Display(Name = "Service Partner Mobile Number")]
        [DataType(DataType.PhoneNumber)]
        [Phone]
        public string? ServicePartnerMobileNumber { get; set; }

        [Required(ErrorMessage = "Required")]
        [RegularExpression(@"^([0-9]{10})$", ErrorMessage = "Please Enter Valid 10 Digit Mobile Number.")]
        [Display(Name = "Service Partner Alternate Mobile Number")]
        [DataType(DataType.PhoneNumber)]
        [Phone]
        public string? ServicePartnerAlternateMobileNumber { get; set; }

        [Required]
        [RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$", ErrorMessage = "Please enter a valid e-mail adress")]
        public string? ServicePartnerEmailId { get; set; }

        [Required(ErrorMessage = "Required")]
        [Display(Name = "Regd Address Line 1")]
        [StringLength(maximumLength: 255)]  
        public string? ServicePartnerAddressLine1 { get; set; }

        
        [Display(Name = "Regd Address Line 2")]
        [StringLength(maximumLength: 255)]
        public string? ServicePartnerAddressLine2 { get; set; }

        //[Required(ErrorMessage = "Required")]
        [RegularExpression(@"^[1-9][0-9]{5}$", ErrorMessage = "Please Enter Valid 6 Digit Pincode.")]
        [Display(Name = "Pincode")]
        public string? ServicePartnerPinCode { get; set; }
        public int? ServicePartnerCityId { get; set; }
        public int? ServicePartnerStateId { get; set; }

        [RegularExpression(@"^[0-9]{2}[A-Z]{5}[0-9]{4}[A-Z]{1}[1-9A-Z]{1}Z[0-9A-Z]{1}$", ErrorMessage = "Please Enter Valid GSTNo")]
        public string? ServicePartnerGstno { get; set; }     
        //public string ServicePartnerGstregisteration { get; set; }

        [RegularExpression(@"^[A-Z]+[a-zA-Z\s]*$", ErrorMessage = "Use letters only please")]
        [Required(ErrorMessage = "Required")]
        [Display(Name = "Bank Name")]
        [StringLength(50)]
        public string? ServicePartnerBankName { get; set; }

        [RegularExpression(@"^[A-Z]{4}0[A-Z0-9]{6}$", ErrorMessage = "Please Enter Valid 11 Digit IFSC Code ")]
        [Required(ErrorMessage = "Required")]
        [Display(Name = "IFSC Code")]
        public string? ServicePartnerIfsccode { get; set; }

        [RegularExpression(@"^[0-9]{9,18}$", ErrorMessage = "Please Enter Valid Bank Account No")]
        [Required(ErrorMessage = "Required")]
        [Display(Name = "Bank Account No ")]
        public string? ServicePartnerBankAccountNo { get; set; }
        public int? ServicePartnerInsertOtp { get; set; }
        public int? ServicePartnerLoginId { get; set; }
        public bool? ServicePartnerIsApprovrd { get; set; }
        //public string ServicePartnerCancelledCheque { get; set; }
        public bool? IconfirmTermsCondition { get; set; }
        //public string ServicePartnerAadharfrontImage { get; set; }
        //public string ServicePartnerAadharBackImage { get; set; }
        //public string ServicePartnerProfilePic { get; set; }
        public List<AddPincode>? addPincodes { get; set; }

        public CitiesID? addCity { get; set; }

        public IFormFile? ServicePartnerCancelledCheque { get; set; }       
        public IFormFile? ServicePartnerAadharfrontImage { get; set; }
        public IFormFile? ServicePartnerAadharBackImage { get; set; }
        public IFormFile? ServicePartnerProfilePic { get; set; }
        public IFormFile? ServicePartnerGstregisteration { get; set; }

    }

    public class AddCityPincodes
    {
        public List<CityIdlist>? cityIdlists { get; set; }
    }
    public class AddPincode
    {
        //public int PincodeId { get; set; }
        public int ServicePartnerId { get; set; }
        public int CreatedBy { get; set; }
        //public int ModifiedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public bool IsActive { get; set; }
        public bool IsPrimaryPincode { get; set; }
        public int CityId { get; set; }
        public int? StateId { get; set; }
        public string? ListOfPincodes { get; set; }

    }
    public class LGCUserViewDataModel
    {
        public int ServicePartnerId { get; set; }
        public string? ServicePartnerName { get; set; }
        public string? ServicePartnerDescription { get; set; }
        //public bool? IsActive { get; set; }
       // public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        //public int? Modifiedby { get; set; }
        //public DateTime? ModifiedDate { get; set; }
        public bool? IsServicePartnerLocal { get; set; }
        public int? UserId { get; set; }
        public string? ServicePartnerRegdNo { get; set; }
        public string? ServicePartnerMobileNumber { get; set; }
        public string? ServicePartnerAlternateMobileNumber { get; set; }
        public string? ServicePartnerEmailId { get; set; }
        public string? ServicePartnerAddressLine1 { get; set; }
        public string? ServicePartnerAddressLine2 { get; set; }
        public string? ServicePartnerPinCode { get; set; }
        public int? ServicePartnerCityId { get; set; }
        public int? ServicePartnerStateId { get; set; }
        public string? ServicePartnerGstno { get; set; }
        public string? ServicePartnerGstregisteration { get; set; }
        public string? ServicePartnerBankName { get; set; }
        public string? ServicePartnerIfsccode { get; set; }
        public string? ServicePartnerBankAccountNo { get; set; }
        //public int? ServicePartnerInsertOtp { get; set; }
        //public int? ServicePartnerLoginId { get; set; }
        //public bool? ServicePartnerIsApprovrd { get; set; }
        public string? ServicePartnerCancelledCheque { get; set; }
        public bool? IconfirmTermsCondition { get; set; }
        public string? ServicePartnerAadharfrontImage { get; set; }
        public string? ServicePartnerAadharBackImage { get; set; }
        public string? ServicePartnerProfilePic { get; set; }
        public string? ServicePartnerFirstName { get; set; }
        public string? ServicePartnerLastName { get; set; }

        public int NumofVehicle { get; set; }
    }

    public class  UpdateServicePartnerDataModel : BaseViewModel
    {
        [Required(ErrorMessage = "Required")]
        public int ServicePartnerId { get; set; }
        public string? ServicePartnerName { get; set; }
        public string? ServicePartnerFirstName { get; set; }
        public string? ServicePartnerLastName { get; set; }
        public string? ServicePartnerBusinessName { get; set; }
        public string? ServicePartnerDescription { get; set; }

        public bool? IsServicePartnerLocal { get; set; }
        public int? UserId { get; set; }
        public string? ServicePartnerRegdNo { get; set; }
        [RegularExpression(@"^([0-9]{10})$", ErrorMessage = "Please Enter Valid 10 Digit Mobile Number.")]
        public string? ServicePartnerMobileNumber { get; set; }
        [RegularExpression(@"^([0-9]{10})$", ErrorMessage = "Please Enter Valid 10 Digit Mobile Number.")]
        public string? ServicePartnerAlternateMobileNumber { get; set; }
        [RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$", ErrorMessage = "Please enter a valid e-mail adress")]
        public string? ServicePartnerEmailId { get; set; }
        public string? ServicePartnerAddressLine1 { get; set; }
        public string? ServicePartnerAddressLine2 { get; set; }
        [RegularExpression(@"^[1-9][0-9]{5}$", ErrorMessage = "Please Enter Valid 6 Digit Pincode.")]
        [Display(Name = "Pincode")]
        public string? ServicePartnerPinCode { get; set; }
        public int? ServicePartnerCityId { get; set; }
        public int? ServicePartnerStateId { get; set; }

        [RegularExpression(@"^[0-9]{2}[A-Z]{5}[0-9]{4}[A-Z]{1}[1-9A-Z]{1}Z[0-9A-Z]{1}$", ErrorMessage = "Please Enter Valid GSTNo")]
        public string? ServicePartnerGstno { get; set; }
        [RegularExpression(@"^[A-Z]+[a-zA-Z\s]*$", ErrorMessage = "Use letters only please")]
        public string? ServicePartnerBankName { get; set; }

        [RegularExpression(@"^[A-Z]{4}0[A-Z0-9]{6}$", ErrorMessage = "Please Enter Valid 11 Digit IFSC Code ")]
        public string? ServicePartnerIfsccode { get; set; }

        [RegularExpression(@"^[0-9]{9,18}$", ErrorMessage = "Please Enter Valid Bank Account No")]
        public string? ServicePartnerBankAccountNo { get; set; }
        public int? ServicePartnerInsertOtp { get; set; }
        public int? ServicePartnerLoginId { get; set; }
        public bool? ServicePartnerIsApprovrd { get; set; }
        public bool? IconfirmTermsCondition { get; set; }
        public List<AddPincode>? addPincodes { get; set; }

        public CitiesID? addCity { get; set; }

        public IFormFile? ServicePartnerCancelledCheque { get; set; }
        public IFormFile? ServicePartnerAadharfrontImage { get; set; }
        public IFormFile? ServicePartnerAadharBackImage { get; set; }
        public IFormFile? ServicePartnerProfilePic { get; set; }
        public IFormFile? ServicePartnerGstregisteration { get; set; }
    }

   
}
