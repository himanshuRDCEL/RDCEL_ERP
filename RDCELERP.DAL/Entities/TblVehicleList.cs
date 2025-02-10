using System;
using System.Collections.Generic;

namespace RDCELERP.DAL.Entities
{
    public partial class TblVehicleList
    {
        public TblVehicleList()
        {
            TblDriverDetails = new HashSet<TblDriverDetail>();
        }

        public int VehicleId { get; set; }
        public string? VehicleNumber { get; set; }
        public string? VehicleRcNumber { get; set; }
        public string? VehicleRcCertificate { get; set; }
        public string? VehiclefitnessCertificate { get; set; }
        public string? VehicleInsuranceCertificate { get; set; }
        public int? CityId { get; set; }
        public int? ServicePartnerId { get; set; }
        public bool? IsApproved { get; set; }
        public int? ApprovedBy { get; set; }
        public bool? IsActive { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }

        public virtual TblCity? City { get; set; }
        public virtual TblUser? CreatedByNavigation { get; set; }
        public virtual TblUser? ModifiedByNavigation { get; set; }
        public virtual TblServicePartner? ServicePartner { get; set; }
        public virtual ICollection<TblDriverDetail> TblDriverDetails { get; set; }
    }
}
