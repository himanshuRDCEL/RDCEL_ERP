using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using RDCELERP.Model.DriverDetails;
using RDCELERP.Model.MobileApplicationModel;
using RDCELERP.Model.MobileApplicationModel.LGC;

namespace RDCELERP.Model.ServicePartner
{
    public class ServicepartnerrequestViewModel
    {

    }
    public class RejectLGCOrderRequest
    {
        public int OrdertransId { get; set; }
        public int LGCId { get; set; }
        public string? RejectComment { get; set; }
    }
    public class AssignOrderRequest
    {
        public List<int>? OrdertransId { get; set; }
        public int LGCId { get; set; }
        public int DriverDetailsId { get; set; }
        public int DriverId { get; set; }
        public int vehicleId { get; set; }
        public bool isJourneyPlannedForToday { get; set; }
    }
    public class AssignOrderResponse
    {
        public bool Status { get; set; }
        public HttpStatusCode Status_Code { get; set; }
        public string? message { get; set; }
        public int SucssesCount { get; set; }
        public List<ResponseResult>? Data { get; set; }
    }
    public class AssignOrderfailedRespone
    {
        public int OrdertransId { get; set; }
    }

    public class StartJournyVehicleListbyServicePResponse
    {
        public int TrackingId { get; set; }
        public int? ServicePartnerId { get; set; }
        public int? DriverDetailsId { get; set; }
        public DateTime? JourneyPlanDate { get; set; }
        public string? CurrentVehicleLatt { get; set; }
        public string? CurrentVehicleLong { get; set; }
        public DateTime? JourneyStartDatetime { get; set; }
        public DateTime? JourneyEndTime { get; set; }
        public DriverDetailsModel? driverDetails { get; set; }

    }
    public class StartJournyVehicleList
    {
        public List<StartJournyVehicleListbyServicePResponse>? startJournyVehicleList { get; set; }
    }

    public class ALLStartJournyVehicleList
    {
        public List<StartJournyVehicleListbyServicePResponse>? AllJournyList { get; set; }
    }
    public class DriverDetails
    {
        public int DriverId { get; set; }
        public int VehicleId { get; set; }
        public string? DriverName { get; set; }
        public string? DriverPhoneNumber { get; set; }
        public string? VehicleNumber { get; set; }
        public string? City { get; set; }
        public bool? IsActive { get; set; }
    }
    public class PickupDeclineOrderRequest
    {
        public int OrdertransId { get; set; }
        public int LGCId { get; set; }
        public int DriverDetailsId { get; set; }
        public string? DeclineComment { get; set; }
    }

    public class PickupRescheduleOrderRequest
    {
        public int OrdertransId { get; set; }
        public int LGCId { get; set; }
        public int DriverDetailsId { get; set; }        
        public DateTime? RescheduleDate { get; set; }
        public string? RescheduleComment { get; set; }

    }
}
