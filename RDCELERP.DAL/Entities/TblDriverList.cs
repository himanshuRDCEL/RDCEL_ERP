using System;
using System.Collections.Generic;

namespace RDCELERP.DAL.Entities
{
    public partial class TblDriverList
    {
        public TblDriverList()
        {
            TblDriverDetails = new HashSet<TblDriverDetail>();
        }

        public int DriverId { get; set; }
        public string? DriverName { get; set; }
        public string? DriverPhoneNumber { get; set; }
        public int? UserId { get; set; }
        public int? CityId { get; set; }
        public int? ServicePartnerId { get; set; }
        public string? DriverLicenseNumber { get; set; }
        public string? DriverLicenseImage { get; set; }
        public bool? IsApproved { get; set; }
        public int? ApprovedBy { get; set; }
        public string? ProfilePicture { get; set; }
        public bool? IsActive { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }

        public virtual TblCity? City { get; set; }
        public virtual TblUser? CreatedByNavigation { get; set; }
        public virtual TblUser? ModifiedByNavigation { get; set; }
        public virtual TblServicePartner? ServicePartner { get; set; }
        public virtual TblUser? User { get; set; }
        public virtual ICollection<TblDriverDetail> TblDriverDetails { get; set; }
    }
}
