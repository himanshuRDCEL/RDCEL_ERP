using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDCELERP.DAL.Entities;
using RDCELERP.Model.Base;
using RDCELERP.Model.DriverDetails;
using RDCELERP.Model.EVC_Portal;
using RDCELERP.Model.LGCMobileApp;

namespace RDCELERP.Model.LGC
{
    public class LGCOrderViewModel : BaseViewModel
    {
        public int? Id { get; set; }
        //Order Details
        public int? OrderLGCId { get; set; }
        public string? RegdNo { get; set; }
        public string? ProductCategory { get; set; }
        public string? ProductType { get; set; }
        public string? BrandName { get; set; }
        public decimal AmountPayableThroughLGC { get; set; }
        public string? TicketNumber { get; set; }
        public string? SponsorName { get; set; }
        //Priyanshi
        public string? AfterQCProductQty { get; set; }

        //Customer Details
        public string? CustomerName { get; set; }

        // Addon Tables
        public TblEvcregistration? tblEvcregistration { get; set; }
        public TblCustomerDetail? tblCustomerDetail { get; set; }
        public TblCustomerDetail TblCustomerDetail = new TblCustomerDetail();
        public TblEvcregistration Tblevcregistration = new TblEvcregistration();
        public TblBrand TblBrand = new TblBrand();

        // Driver Details
        public int DriverId { get; set; }
        public string? DriverName { get; set; }
        public string? DriverPhoneNumber { get; set; }
        public string? VehicleNumber { get; set; }
        public DriverDetailsViewModel? driverDetailsVM { get; set; }
        public DriverListViewModel? driverlistVM { get; set; }
        public int? TrackingId { get; set; }

        // Others
        public string? CreatedDate { get; set; }
        public string? Commant { get; set; }
        public string? PickupScheduleDate { get; set; }
        public string? PickupScheduleTime { get; set; }
        public string? Lgccomments { get; set; }

        // Use for Reschedule
        public DateTime? RescheduleDate { get; set; }
        public int Reschedulecount { get; set; }
        public string? RescheduleComment { get; set; }

        //EVC Details
        public PODViewModel? podVM { get; set; }
        public decimal EvcAmount { get; set; }
        public int EVCRegistrationId { get; set; }
        public string? City { get; set; }
        public string? EVCBusinessName { get; set; }
        public string? InvoicePdfName { get; set; }

        //EVC Auto Allocation phase 2
        public EVC_PartnerViewModel? evcPartnerDetailsVM { get; set; }
        public int? EvcpartnerId { get; set; }
        public string? EvcStoreCode { get; set; }
        public string? EVCMobileNumber { get; set; }
        public string? EVCContactPerson { get; set; }

        public bool? IsDefaultPickupAddress { get; set; }

        public TblBusinessPartner tblBusinessPartner = new TblBusinessPartner();
    }
}
