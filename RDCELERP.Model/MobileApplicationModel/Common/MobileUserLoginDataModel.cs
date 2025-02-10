using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDCELERP.Model.DriverDetails;
using RDCELERP.Model.MobileApplicationModel.LGC;
using RDCELERP.Model.Users;

namespace RDCELERP.Model.MobileApplicationModel.Common
{
    public class MobileUserLoginDataModel
    {
        public string? username { get; set; }
        public string? password { get; set; }
        public string? DeviceId { get; set; }
        public string? DeviceType { get; set; }
        public string? MobileNumber { get; set; }
        public string? Otp { get; set; }
        public string? UserLoginType { get; set; }
        public string? UserId { get; set; }
        public string? UserRoleName { get; set; }
        public bool loginByNumber { get; set; }
        public bool IsServicePartner { get; set; }
    }
    public class LoginByEmail
    {
        [Required]
        [RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$", ErrorMessage = "Please enter a valid e-mail adress")]
        public string? email { get; set; }
        [Required]
        public string?password { get; set; }
        public string? DeviceType { get; set; }
        public string? DeviceId { get; set; }


    }

    public class UserLoginResponseModal
    {
        public string? UserName { get; set; }
        public string? MobileNumber { get; set; }
        public string? Email { get; set; }
        public string? UserLoginTypeId { get; set; }
        public string? UserRoleName { get; set; }
    }

    public class LoginUserDetailsDataViewModal
    {
        public UserDetailsDataModel? userDetails { get; set; }
        public LGCUserViewDataModel? servicePatnerDetails { get; set; }
        public DriverResponseViewModal? driverDetails { get; set; }
    }

    public class OTPLOGIN
    {
        [Required(ErrorMessage = "Required")]
        [RegularExpression(@"^([0-9]{10})$", ErrorMessage = "Please Enter Valid 10 Digit Mobile Number.")]
        [DataType(DataType.PhoneNumber)]
        [Phone]
        public string? MobileNumber { get; set; }

        public string? OTP { get; set; }

        public string? DeviceType { get; set; }
        public string? DeviceId { get; set; }

    }

}
