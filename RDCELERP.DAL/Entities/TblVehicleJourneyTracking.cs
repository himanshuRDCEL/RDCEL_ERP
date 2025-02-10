using System;
using System.Collections.Generic;

namespace RDCELERP.DAL.Entities
{
    public partial class TblVehicleJourneyTracking
    {
        public TblVehicleJourneyTracking()
        {
            TblVehicleJourneyTrackingDetails = new HashSet<TblVehicleJourneyTrackingDetail>();
        }

        public int TrackingId { get; set; }
        public int? ServicePartnerId { get; set; }
        public int? DriverId { get; set; }
        public DateTime? JourneyPlanDate { get; set; }
        public string? CurrentVehicleLatt { get; set; }
        public string? CurrentVehicleLong { get; set; }
        public DateTime? JourneyStartDatetime { get; set; }
        public DateTime? JourneyEndTime { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public bool? IsActive { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }

        public virtual TblUser? CreatedByNavigation { get; set; }
        public virtual TblDriverDetail? Driver { get; set; }
        public virtual TblUser? ModifiedByNavigation { get; set; }
        public virtual TblServicePartner? ServicePartner { get; set; }
        public virtual ICollection<TblVehicleJourneyTrackingDetail> TblVehicleJourneyTrackingDetails { get; set; }
    }
}
