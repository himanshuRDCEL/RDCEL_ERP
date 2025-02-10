using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDCELERP.Model.MobileApplicationModel;

namespace RDCELERP.Model.DriverDetails
{
    public class VehiclerequestViewModel
    {
    }
    public class AcceptOrderbyVehicleRequest
    {
        public int OrdertransId { get; set; }
        public int DriverDetailsId { get; set; }
        public int? ServicePartnerId { get; set; }
        public int? EVCId { get; set; }
        public string? PickupLatt { get; set; }
        public string? PickupLong { get; set; }
        public string? DropLatt { get; set; }
        public string? DropLong { get; set; }
        public DateTime? JourneyPlanDate { get; set; }
        public int? EvcPartnerId { get; set; }


    }
    public class RejectVehicleOrderRequest
    {
        public int OrdertransId { get; set; }
        public int DriverDetailsId { get; set; }
        public string? RejectComment { get; set; }
    }

    public class StartVehicleJourney
    {
        public int DriverDetailsId { get; set; }
        public int? ServicePartnerId { get; set; }
        public DateTime? JourneyPlanDate { get; set; }
        public string? CurrentVehicleLatt { get; set; }
        public string? CurrentVehicleLong { get; set; }
    }

    public class PickupDoneOTPReq
    {
        public int OrdertransId { get; set; }
        public int DriverDetailsId { get; set; }
    }
    public class PickupDoneOTPVerficationReq
    {
        public int OrdertransId { get; set; }
        public int DriverDetailsId { get; set; }
        public string? OTP { get; set; }
    }

    public class pickupDoneReq
    {
        public int OrdertransId { get; set; }
        public int DriverDetailsId { get; set; }
        public int? ServicePartnerId { get; set; }
        public int EVCId { get; set; }
        public string? LGCComment { get; set; }
        public IFormFile? U_Image_One { get; set; }
        public IFormFile? U_Image_Two { get; set; }
        public IFormFile? U_Image_three { get; set; }
        public IFormFile? U_Image_four { get; set; }
        public IFormFile? P_Image_five { get; set; }
        public int EvcPartnerId { get; set; }

    }
    public class DropDoneReq
    {
        
        public int DriverDetailsId { get; set; }
        public int? ServicePartnerId { get; set; }
        public int EVCId { get; set; }
        public string? LGCComment { get; set; }
        public IFormFile? U_Image_One { get; set; }
        public IFormFile? U_Image_Two { get; set; }
        public IFormFile? U_Image_three { get; set; }
        public IFormFile? U_Image_four { get; set; }
        public IFormFile? P_Image_five { get; set; }
        public List<DropOrderList>? OrderTransList { get; set; }
        public int EvcPartnerId { get; set; }
        public int TrackingId { get; set; }
    }

    public class DropOrderList
    {
        public int OrdertransId { get; set; }
    }

    public class DropDoneStatusUpdateReq
    {

        public int DriverDetailsId { get; set; }
        public int? ServicePartnerId { get; set; }
        public int EVCId { get; set; }
        public string? LGCComment { get; set; }       
        public List<DropOrderList>? OrderTransList { get; set; }

    }
    public class DropDoneOTPReq
    {
        public int EVCId { get; set; }
        public int DriverDetailsId { get; set; }
        public int EvcPartnerId { get; set; }
    }
    public class DropDoneOTPVerficationReq
    {
        public int EVCId { get; set; }
        public int DriverDetailsId { get; set; }
        public string? OTP { get; set; }
        public int EvcPartnerId { get; set; }
    }
}
