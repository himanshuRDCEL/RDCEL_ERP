using System;
using System.Collections.Generic;

namespace RDCELERP.DAL.Entities
{
    public partial class MapServicePartnerCityState
    {
        public int MapServicePartnerCityStateId { get; set; }
        public int? ServicePartnerId { get; set; }
        public int? PincodeId { get; set; }
        public bool? IsPrimaryPincode { get; set; }
        public int? CityId { get; set; }
        public int? StateId { get; set; }
        public bool? IsActive { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public int? CreatedBy { get; set; }
        public int? ModifiedBy { get; set; }
        public string? ListOfPincodes { get; set; }

        public virtual TblCity? City { get; set; }
        public virtual TblUser? CreatedByNavigation { get; set; }
        public virtual TblUser? ModifiedByNavigation { get; set; }
        public virtual TblPinCode? Pincode { get; set; }
        public virtual TblServicePartner? ServicePartner { get; set; }
        public virtual TblState? State { get; set; }
    }
}
