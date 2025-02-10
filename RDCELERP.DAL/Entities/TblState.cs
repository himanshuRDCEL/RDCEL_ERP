using System;
using System.Collections.Generic;

namespace RDCELERP.DAL.Entities
{
    public partial class TblState
    {
        public TblState()
        {
            MapServicePartnerCityStates = new HashSet<MapServicePartnerCityState>();
            TblCities = new HashSet<TblCity>();
            TblEvcPartners = new HashSet<TblEvcPartner>();
            TblEvcregistrations = new HashSet<TblEvcregistration>();
            TblRefurbisherRegistrations = new HashSet<TblRefurbisherRegistration>();
            TblServicePartners = new HashSet<TblServicePartner>();
        }

        public int StateId { get; set; }
        public string Name { get; set; } = null!;
        public string Code { get; set; } = null!;
        public bool? IsActive { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }

        public virtual ICollection<MapServicePartnerCityState> MapServicePartnerCityStates { get; set; }
        public virtual ICollection<TblCity> TblCities { get; set; }
        public virtual ICollection<TblEvcPartner> TblEvcPartners { get; set; }
        public virtual ICollection<TblEvcregistration> TblEvcregistrations { get; set; }
        public virtual ICollection<TblRefurbisherRegistration> TblRefurbisherRegistrations { get; set; }
        public virtual ICollection<TblServicePartner> TblServicePartners { get; set; }
    }
}
