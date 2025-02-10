using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static RDCELERP.Model.Whatsapp.WhatsappRescheduleViewModel;
using RDCELERP.Model.ServicePartner;
using RDCELERP.Model.Base;

namespace RDCELERP.Model.LGCMobileApp
{
    public class MobileAppLogisticViewModel : BaseViewModel
    {
        public int? LogisticId { get; set; }
        public string? RegdNo { get; set; }
        public int? StatusId { get; set; }
        public int? ServicePartnerId { get; set; }
        public int? driverDetailsId { get; set; }
        public int? OrderTransId { get; set; }    
        public decimal? AmtPaybleThroughLGC { get; set; }        
        public string? TicketNumber { get; set; }
        public string? StatusCode { get; set; }
        public string? ServicePartnerName { get; set; }
        public string? DriverName { get; set; }
        public string? DriverCity { get; set; }
        public string? DriverPhoneNo { get; set; }
        public string? RescheduleComment { get; set; }
        public string? VehicleNo { get; set; }
        public bool? isOrderAcceptedByDriver { get; set; }
        public bool? isOrderRejectedByDriver { get; set; }
        public DateTime? Rescheduledate { get; set; }
        public int? RescheduleCount { get; set; }
        // Added By VK
        public string? OrderAssignedDate { get; set; }
        public string? ProductCategory { get; set; }
        public string? ProductType { get; set; }
        public string? CustCity { get; set; }
        public string? StatusDesc { get; set; }
        public string? OrderDate { get; set; }
        public decimal? Earning { get; set; }
        public decimal? EstimatedEarning { get; set; }
        public string? LastUpdation { get; set; }
        public string? ServicePartnerBusinessName { get; set; }
        public int? TrackingDetailsId { get; set; }
        public int? PickupCount { get; set; }
        public int? DropCount { get; set; }
    } 
    public class MobileAppLogisticViewModelList
    {
        public List<MobileAppLogisticViewModel>? logisticViewModellist { get; set; }


    }
}
