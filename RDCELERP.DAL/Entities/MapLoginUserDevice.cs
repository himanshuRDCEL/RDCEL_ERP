using System;
using System.Collections.Generic;

namespace RDCELERP.DAL.Entities
{
    public partial class MapLoginUserDevice
    {
        public int MapLoginUserDeviceId { get; set; }
        public string? UserDeviceId { get; set; }
        public int? UserId { get; set; }
        public string? DeviceType { get; set; }
        public bool? IsActive { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public int? CreatedBy { get; set; }

        public virtual TblUser? CreatedByNavigation { get; set; }
        public virtual TblUser? ModifiedByNavigation { get; set; }
        public virtual TblUser? User { get; set; }
    }
}
