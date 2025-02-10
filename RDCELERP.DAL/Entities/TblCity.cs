using System;
using System.Collections.Generic;

namespace RDCELERP.DAL.Entities
{
    public partial class TblCity
    {
        public TblCity()
        {
            MapServicePartnerCityStates = new HashSet<MapServicePartnerCityState>();
            TblBusinessPartners = new HashSet<TblBusinessPartner>();
            TblDriverDetails = new HashSet<TblDriverDetail>();
            TblDriverLists = new HashSet<TblDriverList>();
            TblEvcPartners = new HashSet<TblEvcPartner>();
            TblEvcregistrations = new HashSet<TblEvcregistration>();
            TblPinCodes = new HashSet<TblPinCode>();
            TblRefurbisherRegistrations = new HashSet<TblRefurbisherRegistration>();
            TblServicePartners = new HashSet<TblServicePartner>();
            TblVehicleLists = new HashSet<TblVehicleList>();
        }

        public int CityId { get; set; }
        public string Name { get; set; } = null!;
        public int? StateId { get; set; }
        public bool? IsActive { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public bool? IsMetro { get; set; }
        public string? CityLogo { get; set; }
        public string? CityCode { get; set; }

        public virtual TblState? State { get; set; }
        public virtual ICollection<MapServicePartnerCityState> MapServicePartnerCityStates { get; set; }
        public virtual ICollection<TblBusinessPartner> TblBusinessPartners { get; set; }
        public virtual ICollection<TblDriverDetail> TblDriverDetails { get; set; }
        public virtual ICollection<TblDriverList> TblDriverLists { get; set; }
        public virtual ICollection<TblEvcPartner> TblEvcPartners { get; set; }
        public virtual ICollection<TblEvcregistration> TblEvcregistrations { get; set; }
        public virtual ICollection<TblPinCode> TblPinCodes { get; set; }
        public virtual ICollection<TblRefurbisherRegistration> TblRefurbisherRegistrations { get; set; }
        public virtual ICollection<TblServicePartner> TblServicePartners { get; set; }
        public virtual ICollection<TblVehicleList> TblVehicleLists { get; set; }
    }
}
