using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using RDCELERP.Model.Base;
using RDCELERP.Model.LGCMobileApp;

namespace RDCELERP.Model.DriverDetails
{
    public class DriverDetailsViewModel : BaseViewModel
    {
        public int DriverDetailsId { get; set; }
        [Required(ErrorMessage = "Please Enter Your Name")]
        public string? DriverName { get; set; }

        [Required(ErrorMessage = "Please Enter Your Phone Number")]
        [RegularExpression(@"^((?!(0))[0-9]{10})$", ErrorMessage = "Please Enter Valid 10 Digit Mobile Number.")]
        public string? DriverPhoneNumber { get; set; }

        [Required(ErrorMessage = "Please Enter Your Vehicle Number")]
        public string? VehicleNumber { get; set; }
        public string? City { get; set; }
        public string? EVCBusinessName { get; set; }
        public string[]? OrderTransId { get; set; }
        public string[]? OrderLGCId { get; set; }

        public List<SelectListItem> CityList = new List<SelectListItem>();
        public List<SelectListItem> EvcList = new List<SelectListItem>();

        //Added by VK for mobile App
        public int? ServicePartnerId { get; set; }
        public string? ServicePartnerName { get; set; }
        public int? CityId { get; set; }
        public string? DriverCity { get; set; }
        public string? VehicleRcNumber { get; set; }
        public string? VehicleRcCertificate { get; set; }
        public string? VehiclefitnessCertificate { get; set; }
        public string? DriverlicenseNumber { get; set; }
        public string? DriverlicenseImage { get; set; }
        public bool? IsApproved { get; set; }
        public int? ApprovedBy { get; set; }
        public string? ProfilePicture { get; set; }
        public string? VehicleInsuranceCertificate { get; set; }
        public string? OrderAssignedDate { get; set; }

        //Added for Assign order to Driver
        public string? LogisticIdList { get; set; }
        public int? DriverCityId { get; set; }
        public string? ServicePartnerBusinessName { get; set; }

        //Added by VK
        public string? VehicleRcCertificateString { get; set; }
        public string? VehiclefitnessCertificateString { get; set; }
        public string? ProfilePictureString { get; set; }
        public string? VehicleInsuranceCertificateString { get; set; }
        public string? DriverlicenseImageString { get; set; }
        public string? VehicleRcCertificateImgSrc { get; set; }
        public string? VehiclefitnessCertificateImgSrc { get; set; }
        public string? ProfilePictureImgSrc { get; set; }
        public string? VehicleInsuranceCertificateImgSrc { get; set; }
        public string? DriverlicenseImgSrc { get; set; }
        public int? AssignedOrdersCount { get; set; }
        public string? JourneyPlanDate { get; set; }
        public DriverListViewModel? driverlistVM { get; set; }
        public int? DriverId { get; set; }
    }

    public class DriverDetailsResponseList
    {
        public List<DriverDetailsResponseViewModal>? DriverDetailsResponse { get; set; }
    }

    public class DriverDetailsResponseViewModal
    {
        public int DriverDetailsId { get; set; }
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

    }

    public class DriverDetailsListByCityResponse
    {
        public int DriverDetailsId { get; set; }
        public string? DriverName { get; set; }
        public string? DriverPhoneNumber { get; set; }
        public string? VehicleNumber { get; set; }
        public string? City { get; set; }
        public int MaxAcceptableOrderCount { get; set; }
        public int AcceptedOrderCount { get; set; }
    }

    public class DriverResponseViewModal
    {
        public int DriverId { get; set; }
        public string? DriverName { get; set; }
        public string? DriverPhoneNumber { get; set; }       
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }     
        public string? DriverlicenseNumber { get; set; }
        public string? DriverlicenseImage { get; set; }
        public string? ProfilePicture { get; set; }       
        public int? CityId { get; set; }
        public string cityName { get; set; }
        public int? ServicePartnerId { get; set; }

    }

    public class vehicleList
    {
        public int VehicleId { get; set; }       
        public string? VehicleNumber { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string? VehicleRcNumber { get; set; }       
        public string? VehicleInsuranceCertificate { get; set; }
        public string? VehiclefitnessCertificate { get; set; }
        public string? VehicleRcCertificate { get; set; }
        public int? CityId { get; set; }
        public string? CityName { get; set; }
        public int? ServicePartnerId { get; set; }

    }

    public class vehicleListResponseList
    {
        public List<vehicleList>? vehicleLists { get; set; }
    }
    public class DriverListResponseList
    {
        public List<DriverResponseViewModal>? DriverLists { get; set; }
    }
    public class VehicleListByCityResponse
    {
        public int VehicleId { get; set; }
        public string? VehicleNumber { get; set; }
        public string? City { get; set; }
        public int CityId { get; set; }
        public int? ServicePartnerId { get; set; }
        public int MaxAcceptableOrderCount { get; set; }
        public int AcceptedOrderCount { get; set; }

        public DriverResponseViewModal DriverDetail { get; set; }
    }
}
