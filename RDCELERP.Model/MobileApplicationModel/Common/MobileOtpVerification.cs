using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDCELERP.Model.MobileApplicationModel.Common
{
    public class MobileOtpVerification
    {
        [RegularExpression(@"^([0-9]{10})$", ErrorMessage = "Please Enter Valid 10 Digit Mobile Number.")]
        [Display(Name = "Service Partner Mobile Number")]
        [DataType(DataType.PhoneNumber)]
        [Required]
        public string? mobileNumber { get; set; }

        [Required]
        [Display(Name = "Otp Request type")]
        public string? requestFor { get; set; }
    }

    public class VerifyOtp
    {
        [RegularExpression(@"^([0-9]{10})$", ErrorMessage = "Please Enter Valid 10 Digit Mobile Number.")]
        [Display(Name = "Service Partner Mobile Number")]
        [DataType(DataType.PhoneNumber)]
        [Required]
        public string? mobileNumber { get; set; }

        [Required]
        [Display(Name = "Otp Request ")]
        public string? Otp { get; set; }
    }

    public class OtpWithUserInfo
    {
        public int? UserId { get; set; }
        public string? UserRoleName { get; set; }
        //public string message { get; set; }

    }

    public class IsNumberorEmailExits
    {
        [Required]
        public bool isMobileNumber { get; set; }

        [RegularExpression(@"^([0-9]{10})$", ErrorMessage = "Please Enter Valid 10 Digit Mobile Number.")]
        [DataType(DataType.PhoneNumber)]
        [Phone]
        public string? MobileNumber { get; set; }

        [RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$", ErrorMessage = "Please enter a valid e-mail adress")]
        public string? Email { get; set; }
        public bool isServicePartner { get; set; } 
        public bool isServicePartnerDriver { get; set; }


    }

    public class OTPSent
    {
        [RegularExpression(@"^([0-9]{10})$", ErrorMessage = "Please Enter Valid 10 Digit Mobile Number.")]
        [DataType(DataType.PhoneNumber)]
        [Required]
        public string? mobileNumber { get; set; }

        
    }
}
