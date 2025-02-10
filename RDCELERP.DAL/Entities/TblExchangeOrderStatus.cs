using System;
using System.Collections.Generic;

namespace RDCELERP.DAL.Entities
{
    public partial class TblExchangeOrderStatus
    {
        public TblExchangeOrderStatus()
        {
            TblAbbredemptions = new HashSet<TblAbbredemption>();
            TblAbbregistrations = new HashSet<TblAbbregistration>();
            TblExchangeAbbstatusHistories = new HashSet<TblExchangeAbbstatusHistory>();
            TblExchangeOrders = new HashSet<TblExchangeOrder>();
            TblLogistics = new HashSet<TblLogistic>();
            TblOrderTrans = new HashSet<TblOrderTran>();
            TblTempData = new HashSet<TblTempDatum>();
            TblTimelineStatusMappings = new HashSet<TblTimelineStatusMapping>();
            TblVehicleJourneyTrackingDetails = new HashSet<TblVehicleJourneyTrackingDetail>();
        }

        public int Id { get; set; }
        public string? StatusCode { get; set; }
        public string? StatusDescription { get; set; }
        public string? StatusName { get; set; }
        public bool? IsActive { get; set; }

        public virtual ICollection<TblAbbredemption> TblAbbredemptions { get; set; }
        public virtual ICollection<TblAbbregistration> TblAbbregistrations { get; set; }
        public virtual ICollection<TblExchangeAbbstatusHistory> TblExchangeAbbstatusHistories { get; set; }
        public virtual ICollection<TblExchangeOrder> TblExchangeOrders { get; set; }
        public virtual ICollection<TblLogistic> TblLogistics { get; set; }
        public virtual ICollection<TblOrderTran> TblOrderTrans { get; set; }
        public virtual ICollection<TblTempDatum> TblTempData { get; set; }
        public virtual ICollection<TblTimelineStatusMapping> TblTimelineStatusMappings { get; set; }
        public virtual ICollection<TblVehicleJourneyTrackingDetail> TblVehicleJourneyTrackingDetails { get; set; }
    }
}
