using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDCELERP.DAL.Entities;
using RDCELERP.Model.ExchangeOrder;
using RDCELERP.Model.LGC;
using RDCELERP.Model.MobileApplicationModel.EVC;

namespace RDCELERP.Model.MobileApplicationModel.LGC
{
    public class customerOrderDetailsDataModel
    {
        public int? OrderId { get; set; }
        public string? TicketNumber { get; set; }
        public string? RegdNo { get; set; }
        public string? CustomerName { get; set; }
        public string? MobileNumber { get; set; }
        public string? Location { get; set; }
        public string? responseMessage { get; set; }
    }
    public class EVCResellerModel
    {
        public int? EvcregistrationId { get; set; }
        public string? BussinessName { get; set; }
        public string? ContactPerson { get; set; }
        public string? EvcmobileNumber { get; set; }
        public string? AlternateMobileNumber { get; set; }
        public string? EmailId { get; set; }
        public string? RegdAddressLine1 { get; set; }
        public string? RegdAddressLine2 { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? PinCode { get; set; }
        public int? EvcPartnerId { get; set; }
        public string? EVCStoreCode { get; set; }

    }

    public class DriverDetailsModel
    {
        public int? DriverDetailsId { get; set; }
        public string? DriverName { get; set; }
        public string? DriverPhoneNumber { get; set; }
        public string? VehicleNumber { get; set; }
        public string? City { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string? VehicleRcNumber { get; set; }
        public string? DriverlicenseNumber { get; set; }
        public string? DriverlicenseImage { get; set; }
        public string? ProfilePicture { get; set; }
        public string? VehicleInsuranceCertificate { get; set; }
        public string? VehiclefitnessCertificate { get; set; }
        public string? VehicleRcCertificate { get; set; }
        public int? CityId { get; set; }
        public int? ServicePartnerId { get; set; }
        public int? DriverId { get; set; }
        public int? VehicleId { get; set; }
        public DateTime JourneyPlannedDate { get; set; }

    }

    public class customerDetailsviewModel
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Address1 { get; set; }
        public string? Address2 { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? ZipCode { get; set; }
    }
    public class ServicePartnerOrderList
    {
        public CustomerDetailViewModel? customerDetail { get; set; }
        public ExchangeOrderViewModel? orderViewModel { get; set; }
        public LGCOrderViewModel? LGCOrderViewModal { get; set; }
        //public ExchangeOrderViewModel orderViewModel { get; set; }
    }

    public class OrderDetail
    {
        public string? RegdNo { get; set; }
        public string? ProductCategory { get; set; }
        public string? ProductType { get; set; }
        public string? SponsorName { get; set; }
        public string? BrandName { get; set; }
        public int? OrderTransId { get; set; }
        public string? CreatedDate { get; set; }
        public string? CustomerName { get; set; }
        public string? TicketNumber { get; set; }
        public string? PickupScheduleDate { get; set; }
        public bool? IsDefferedSettlement { get; set; }
        public int? AmtPaybleThroughLGC { get; set; }
        public customerDetailsviewModel? customerData { get; set; }
        public EVCResellerModel? EvcData { get; set; }
        public DriverDetailsModel? DriverData { get; set; }

    }

    

    public class OrderList
    {
        public OrderDetail? orderDetail { get; set; }


        // public TblEvcregistration tblEvcregistration { get; set; }
        public List<OrderImageUploadViewModel>? OrderPickupImages { get; set; }
        public List<OrderImageUploadViewModel>? OrderDropImages { get; set; }
        public List<OrderImageUploadViewModel>? OrderQCImages { get; set; }

    }
    //use for All Order list API
    public class AllOrderList
    {
        public List<AllOrderlistViewModel>? AllOrderlistViewModels { get; set; }
    }
    public class AllOrderlistViewModel
    {
        public int? OrderTransId { get; set; }
        public string? RegdNo { get; set; }
        public string? ProductCategory { get; set; }
        public string? ProductType { get; set; }
        public string? CustomerName { get; set; }
        public string? TicketNumber { get; set; }
        public string? PickupScheduleDate { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string? City { get; set; }
        public string? ZipCode { get; set; }
        public string? Address1 { get; set; }
        public string? Address2 { get; set; }
        public string? PhoneNumber { get; set; }
        public string? State { get; set; }
        public bool? IsDefferedSettlement { get; set; }
        public int? AmtPaybleThroughLGC { get; set; }
        public int? StatusId { get; set; }
        public string? StatusDesc { get; set; }
        public EVCResellerModel? EvcData { get; set; }
        public DriverDetailsModel? DriverData { get; set; }
    }
    #region Assign Order List by vehicle
    public class AllAssignOrderlistByVehicle
    {
        public List<AllAssignOrderlistByVehicleViewModel>? AllOrderlistViewModels { get; set; }
    }
    public class AllAssignOrderlistByVehicleViewModel
    {
        public int? OrderTransId { get; set; }
        public string? RegdNo { get; set; }
        public string? ProductCategory { get; set; }
        public string? ProductType { get; set; }
        public string? CustomerName { get; set; }
        public string? TicketNumber { get; set; }
        public string? PickupScheduleDate { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string? City { get; set; }
        public string? ZipCode { get; set; }
        public string? Address1 { get; set; }
        public string? Address2 { get; set; }
        public string? PhoneNumber { get; set; }
        public int? DriverId { get; set; }
        public int? servicePartnerId { get; set; }
        public string? State { get; set; }
       // public DateTime? JourneyPlanDate { get; set; }
        public EVCResellerModel? EvcData { get; set; }
        public DriverDetailsModel? DriverData { get; set; }
    }
    #endregion
    #region Accept Order list by vehicle
    public class AllAcceptOrderlistByVehicle
    {
        public int? TotalOrderCount { get; set; }
        public int? TotalPickedUpCount { get; set; }
        public DateTime? StartJourneyTime { get; set; }
        public List<AllAcceptOrderlistByVehicleViewModel>? AllAcceptOrderlistViewModels { get; set; }
        public List<AllEVCDetails>? AllEVCDetail { get; set; }

    }
   

    public class AllAcceptOrderlistByVehicleViewModel
    {
        public int? OrderTransId { get; set; }
        public string? RegdNo { get; set; }
        public string? ProductCategory { get; set; }
        public string? ProductType { get; set; }      
        public string? TicketNumber { get; set; }
        public string? PickupScheduleDate { get; set; }       
        public int? DriverDetailsId { get; set; }
        public int? servicePartnerId { get; set; }
        public DateTime? JourneyPlanDate { get; set; }
        public PickupDetails? pickupDetails { get; set; }
        public DropDetails? dropDetails { get; set; }
        public bool? IsDefferedSettlement { get; set; }
        public int? AmtPaybleThroughLGC { get; set; }
        public int? TrackingId { get; set; }


    }
    public class PickupDetails
    {
        public string? CustomerName { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string? City { get; set; }
        public string? ZipCode { get; set; }
        public string? Address1 { get; set; }
        public string? Address2 { get; set; }
        public string? PhoneNumber { get; set; }
        public string? PickupLatt { get; set; }
        public string? PickupLong { get; set; }
        public string? State { get; set; }
    }
    public class DropDetails
    {
        public int? EvcregistrationId { get; set; }
        public string? BussinessName { get; set; }
        public string? ContactPerson { get; set; }
        public string? EvcmobileNumber { get; set; }
        public string? AlternateMobileNumber { get; set; }
        public string? EmailId { get; set; }
        public string? RegdAddressLine1 { get; set; }
        public string? RegdAddressLine2 { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? PinCode { get; set; }
        public string? DropLatt { get; set; }
        public string? DropLong { get; set; }
        public int? EvcPartnerId { get; set; }
        public string? EvcStoreCode { get; set; }
    }

    public class AllEVCDetails
    {
        public int? EvcregistrationId { get; set; }
        public string? BussinessName { get; set; }
        public string? ContactPerson { get; set; }
        public string? EvcmobileNumber { get; set; }
        public string? AlternateMobileNumber { get; set; }
        public string? EmailId { get; set; }
        public string? RegdAddressLine1 { get; set; }
        public string? RegdAddressLine2 { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? PinCode { get; set; }
        public string? DropLatt { get; set; }
        public string? DropLong { get; set; }

    }
    #endregion

    public class ReqUpdateDriverforVehicle
    {
        public int DriverId { get; set; }
        public int servicePartnerId { get; set; }
        public int vehicleId { get; set; }
        public bool isJourneyPlannedForToday { get; set; }
    }

    public class AllDriverDetailslist
    {
        public List<DriverDetailsModel>? AllDriverDetailslists{ get; set; }
    }
}
