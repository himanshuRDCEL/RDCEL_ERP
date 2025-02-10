using System;
using System.Collections.Generic;

namespace RDCELERP.DAL.Entities
{
    public partial class TblLoginMobile
    {
        public int Id { get; set; }
        public string Username { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string? UserRoleName { get; set; }
        public string? UserDeviceId { get; set; }
        public int? UserId { get; set; }
        public string? DeviceType { get; set; }
        public string? MobileNumber { get; set; }
        public bool? IsActive { get; set; }
        public int? UserLoginTypeId { get; set; }

        public virtual TblUser? User { get; set; }
    }
}
