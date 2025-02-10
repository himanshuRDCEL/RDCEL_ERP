using System;
using System.Collections.Generic;

namespace RDCELERP.DAL.Entities
{
    public partial class TblVehicleJourneyTrackingDetail
    {
        public int TrackingDetailsId { get; set; }
        public int? TrackingId { get; set; }
        public int? ServicePartnerId { get; set; }
        public int? DriverId { get; set; }
        public int? Evcid { get; set; }
        public int? OrderTransId { get; set; }
        public DateTime? JourneyPlanDate { get; set; }
        public string? PickupLatt { get; set; }
        public string? PickupLong { get; set; }
        public string? DropLatt { get; set; }
        public string? DropLong { get; set; }
        public DateTime? PickupStartDatetime { get; set; }
        public DateTime? PickupEndDatetime { get; set; }
        public DateTime? OrderDropTime { get; set; }
        public TimeSpan? PickupTat { get; set; }
        public TimeSpan? DropTat { get; set; }
        public int? StatusId { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public bool? IsActive { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public decimal? BasePrice { get; set; }
        public decimal? PickupInc { get; set; }
        public decimal? PackingInc { get; set; }
        public decimal? DropInc { get; set; }
        public decimal? DropImageInc { get; set; }
        public decimal? Total { get; set; }
        public decimal? EstimateEarning { get; set; }
        public bool? IsPackedImg { get; set; }
        public int? EvcpartnerId { get; set; }

        public virtual TblUser? CreatedByNavigation { get; set; }
        public virtual TblDriverDetail? Driver { get; set; }
        public virtual TblEvcregistration? Evc { get; set; }
        public virtual TblEvcPartner? Evcpartner { get; set; }
        public virtual TblUser? ModifiedByNavigation { get; set; }
        public virtual TblOrderTran? OrderTrans { get; set; }
        public virtual TblServicePartner? ServicePartner { get; set; }
        public virtual TblExchangeOrderStatus? Status { get; set; }
        public virtual TblVehicleJourneyTracking? Tracking { get; set; }
    }
}
