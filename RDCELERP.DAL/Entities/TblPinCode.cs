using System;
using System.Collections.Generic;

namespace RDCELERP.DAL.Entities
{
    public partial class TblPinCode
    {
        public TblPinCode()
        {
            MapServicePartnerCityStates = new HashSet<MapServicePartnerCityState>();
            TblBppincodeMappings = new HashSet<TblBppincodeMapping>();
        }

        public int Id { get; set; }
        public string? ZohoPinCodeId { get; set; }
        public int? ZipCode { get; set; }
        public string? Location { get; set; }
        public string? HubControl { get; set; }
        public string? State { get; set; }
        public bool? IsActive { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public bool? IsAbb { get; set; }
        public bool? IsExchange { get; set; }
        public int? CityId { get; set; }
        public string? AreaLocality { get; set; }

        public virtual TblCity? City { get; set; }
        public virtual ICollection<MapServicePartnerCityState> MapServicePartnerCityStates { get; set; }
        public virtual ICollection<TblBppincodeMapping> TblBppincodeMappings { get; set; }
    }
}
