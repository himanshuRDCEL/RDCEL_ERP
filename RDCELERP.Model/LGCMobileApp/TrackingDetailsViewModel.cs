using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDCELERP.DAL.Entities;
using RDCELERP.Model.Base;

namespace RDCELERP.Model.LGCMobileApp
{
    public class VehiclesTrackingDetails : BaseViewModel
    {
        public int TrackingDetailsId { get; set; }
        public int? TrackingId { get; set; }
        public int? ServicePartnerId { get; set; }
        public int? DriverId { get; set; }
        public int? Evcid { get; set; }
        public int? OrderTransId { get; set; }
        public DateTime? JourneyPlanDate { get; set; }
        public DateTime? PickupStartDatetime { get; set; }
        public DateTime? PickupEndDatetime { get; set; }
        public int? StatusId { get; set; }
        public decimal? EstimateEarning { get; set; }
        public string? RegdNo { get; set; }
        public string? ServicePartnerName { get; set; }
        public string? DriverName { get; set; }
        public string? VehicleNo { get; set; }
        public string? DriverPhoneNo { get; set; }
        public string? DriverCity { get; set; }
        public string? ServicePartnerBusinessName { get; set; }
        public DateTime? orderAssignedDate { get; set; }
    }
    public class VehiclesTrackingDetailsList
    {
        public List<VehiclesTrackingDetails>? VehiclesTrackingDetailslist { get; set; }
    }
}
