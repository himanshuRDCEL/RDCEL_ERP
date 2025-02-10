using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDCELERP.Model.Base;

namespace RDCELERP.Model.VehicleJourneyViewModel
{
    public class StartvehicleJourneyViewModel : BaseViewModel
    {
        public int? TrackingId { get; set; }
        public int? ServicePartnerID { get; set; }
        public int? DriverId { get; set; }
        public DateTime? JourneyPlanDate { get; set; }
        public string? CurrentVehicleLatt { get; set; }
        public string? CurrentVehicleLong { get; set; }
        public string? DriverName { get; set; }
        public DateTime? JourneyEndTime { get; set; }
        public string? JourneyStartDatetime { get; set; }
        public string? ServicePartnerName { get; set; }
        public string? VehicleNo { get; set; }
        public string? DriverPhoneNo { get; set; }
        public string? DriverCity { get; set; }
        public string? RegdNo { get; set; }
        public int? OrderTransId { get; set; }
        public string? JourneyPlanDatetime { get; set; }
        public string? ServicePartnerBusinessName { get; set; }
    }
}
