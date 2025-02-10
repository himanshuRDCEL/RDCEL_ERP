using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDCELERP.DAL.Entities;
using RDCELERP.Model.Base;

namespace RDCELERP.Model.ServicePartner
{
    public class ServicePartnerViewModel : BaseViewModel
    {
        public int ServicePartnerId { get; set; }
        public string? ServicePartnerName { get; set; }
        public int Map_ServicePartnerCityStateId { get; set; }
        [Required(ErrorMessage = "Service partner description name is required.")]
        public string? ServicePartnerDescription { get; set; }
        public int? UserId { get; set; }
        public string? UserName { get; set; }
        public string? Action { get; set; }
        public string? Date { get; set; }
        public bool? IsServicePartnerLocal { get; set; }
        public string? ServicePartnerRegdNo { get; set; }
        [Required(ErrorMessage = "Mobile number is required.")]
        [RegularExpression(@"^([0-9]{10})$", ErrorMessage = "Please Enter Valid 10 Digit Mobile Number.")]
        public string? ServicePartnerMobileNumber { get; set; }
        [RegularExpression(@"^([0-9]{10})$", ErrorMessage = "Please Enter Valid 10 Digit Mobile Number.")]
        public string? ServicePartnerAlternateMobileNumber { get; set; }
        [Required(ErrorMessage = "Email is required.")]
        [RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$", ErrorMessage = "Please enter a valid e-mail adress")]
        public string? ServicePartnerEmailId { get; set; }
        [Required(ErrorMessage = "Address line 1 is required.")]
        public string? ServicePartnerAddressLine1 { get; set; }
        public string? ServicePartnerAddressLine2 { get; set; }
        [Required(ErrorMessage = "Pincode is required.")]
        public string? ServicePartnerPinCode { get; set; }
        [Required(ErrorMessage = "Pincode is required.")]
        public string[]? ServicePartnerPinCodes { get; set; }
        [Required(ErrorMessage = "City is required.")]
        public int? ServicePartnerCityId { get; set; }
        [Required(ErrorMessage = "City is required.")]
        public string[]? ServicePartnerCities {get; set; }
        [Required(ErrorMessage = "State is required.")]
        public int? ServicePartnerStateId { get; set; }
        [Required(ErrorMessage = "State is required.")]
        public string[]? ServicePartnerStates { get; set; }
        
        [RegularExpression(@"^[0-9]{2}[A-Z]{5}[0-9]{4}[A-Z]{1}[1-9A-Z]{1}Z[0-9A-Z]{1}$", ErrorMessage = "Please Enter Valid GSTNo")]
        public string? ServicePartnerGstno { get; set; }
       
        public string? ServicePartnerGstregisteration { get; set; }
        [RegularExpression(@"^[A-Z]+[a-zA-Z\s]*$", ErrorMessage = "Use letters only please")]
        [Required(ErrorMessage = "Bank name is required")]
        public string? ServicePartnerBankName { get; set; }
        [RegularExpression(@"^[A-Z]{4}0[A-Z0-9]{6}$", ErrorMessage = "Please Enter Valid 11 Digit IFSC Code ")]
        [Required(ErrorMessage = " IFSC code is required")]
        public string? ServicePartnerIfsccode { get; set; }
        [RegularExpression(@"^[0-9]{9,18}$", ErrorMessage = "Please Enter Valid Bank Account No")]
        [Required(ErrorMessage = "Bank account no. is Required")]
        [Display(Name = "Bank Account No ")]
        public string? ServicePartnerBankAccountNo { get; set; }
        public int? ServicePartnerInsertOtp { get; set; }
        public int? ServicePartnerLoginId { get; set; }
        public bool? ServicePartnerIsApprovrd { get; set; }
        [Required(ErrorMessage = "Cancelled cheque is required.")]
        
        public string? ServicePartnerCancelledCheque { get; set; }

        public string? ServicePartnerState { get; set; }
        public bool? IconfirmTermsCondition { get; set; }
        [Required(ErrorMessage = "Aadhar front image is required.")]
        public string? ServicePartnerAadharfrontImage { get; set; }
        [Required(ErrorMessage = "Aadhar back image is required.")]
        public string? ServicePartnerAadharBackImage { get; set; }
        [Required(ErrorMessage = "Profile pic is required.")]
        public string? ServicePartnerProfilePic { get; set; }
        public string? ProfilePicURL { get; set; }
        public string? CancelledChequeURL { get; set; }
        public string? GSTURL { get; set; }
        public string? AadharFrontURL { get; set; }
        public string? AadharBackURL { get; set; }
        [Required(ErrorMessage = "Service partner first name is required.")]
        [RegularExpression("^[ A-Za-z0-9 +-]*$", ErrorMessage = "Only Alphabets and Numbers allowed.")]
        public string? ServicePartnerFirstName { get; set; }
        [Required(ErrorMessage = "Service partner last name is required.")]
        [RegularExpression("^[ A-Za-z0-9 +-]*$", ErrorMessage = "Only Alphabets and Numbers allowed.")]

        public string? ServicePartnerLastName { get; set; }
        
        [Required(ErrorMessage = "Service partner business name is required.")]
        [RegularExpression("^[ A-Za-z0-9 +-]*$", ErrorMessage = "Only Alphabets and Numbers allowed.")]
        public string? ServicePartnerBusinessName { get; set; }
        public bool Selected { get; set; }

        public List<ServicePartnerViewModel>? PinCodeList { get; set; }
        public List<TblCity>? CityList { get; set; }

        public ServicePartnerVMExcel? ServicePartnerVM { get; set; }
        public IFormFile? UploadServicePartner { get; set; }
        public List<ServicePartnerVMExcel>? ServicePartnerVMList { get; set; }
        public List<ServicePartnerVMExcel>? ServicePartnerVMErrorList { get; set; }
    }
}
