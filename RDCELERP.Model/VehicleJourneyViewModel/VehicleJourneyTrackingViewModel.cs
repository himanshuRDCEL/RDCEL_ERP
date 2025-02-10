using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDCELERP.Model.Base;

namespace RDCELERP.Model.VehicleJourneyViewModel
{
    public class VehicleJourneyTrackingViewModel : BaseViewModel
    {
        public int TrackingId { get; set; }
        public int ServicePartnerID { get; set; }
        public int DriverId { get; set; }
        public DateTime JourneyPlanDate { get; set; }
        public string? CurrentVehicleLatt { get; set; }
        public string? CurrentVehicleLong { get; set; }
        public string? DriverName { get; set; }
        public DateTime JourneyEndTime { get; set; }
        public string? JourneyStartDatetime { get; set; }
        public string? ServicePartnerName { get; set; }
        public string? VehicleNo { get; set; }
        public string? DriverPhoneNo { get; set; }
        public string? DriverCity { get; set; }
    }

    public class VehicleJourneyTrackDetailsModel : VehicleJourneyTrackingViewModel
    {
        public int TrackingDetailsId { get; set; }
        public int? ServicePartnerId { get; set; }
        public int? Evcid { get; set; }
        public int? OrderTransId { get; set; }
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
        public decimal? BasePrice { get; set; }
        public decimal? PickupInc { get; set; }
        public decimal? PackingInc { get; set; }
        public decimal? DropInc { get; set; }
        public decimal? DropImageInc { get; set; }
        public decimal? Total { get; set; }
        public decimal? EstimateEarning { get; set; }
        public bool? IsPackedImg { get; set; }

        //Added by VK
        public string? RegdNo { get; set; }
        public string? EVCName { get; set; }
    }
}
