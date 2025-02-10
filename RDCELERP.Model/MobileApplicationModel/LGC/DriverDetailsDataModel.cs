using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDCELERP.Model.Base;

namespace RDCELERP.Model.MobileApplicationModel.LGC
{
    public class DriverDetailsDataModel
    {
        public int DriverDetailsId { get; set; }
       // [Required(ErrorMessage = "Required")]
        [Display(Name = "Driver FullName")]
        [StringLength(50)]
        public string? DriverName { get; set; }

        //[Required(ErrorMessage = "Required")]
        [RegularExpression(@"^([0-9]{10})$", ErrorMessage = "Please Enter Valid 10 Digit Mobile Number.")]
        [Display(Name = "Partner Mobile Number")]
        [DataType(DataType.PhoneNumber)]
        [Phone]
        public string? DriverPhoneNumber { get; set; }
   
        public string? VehicleNumber { get; set; }
      
        public string? City { get; set; }
        public bool? IsActive { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? Modifiedby { get; set; }
        public DateTime? ModifiedDate { get; set; }
       
        public string? VehicleRcNumber { get; set; }
       
        //public string VehicleRcCertificate { get; set; }
        //public string VehiclefitnessCertificate { get; set; }
        //public string ProfilePicture { get; set; }
        //public string VehicleInsuranceCertificate { get; set; }

        public IFormFile? VehicleRcCertificate { get; set; }
        public IFormFile? VehiclefitnessCertificate { get; set; }
        public IFormFile? ProfilePicture { get; set; }
        public IFormFile? VehicleInsuranceCertificate { get; set; }
        public IFormFile? DriverlicenseImage { get; set; }
      
        public string? DriverlicenseNumber { get; set; }
        [Required]
        //public string DriverlicenseImage { get; set; }
        public bool? IsApproved { get; set; }
        public int? ApprovedBy { get; set; }
        public int? CityId { get; set; }
        public int? ServicePartnerId { get; set; }

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
    }
    public class UpdateDriverDetailsDataModel : BaseViewModel
    {
        [Required(ErrorMessage = "Required")]
        public int DriverDetailsId { get; set; }
        [Display(Name = "Driver FullName")]
        [StringLength(50)]
        public string? DriverName { get; set; }

        [RegularExpression(@"^([0-9]{10})$", ErrorMessage = "Please Enter Valid 10 Digit Mobile Number.")]
        [Display(Name = "Partner Mobile Number")]
        [DataType(DataType.PhoneNumber)]
        [Phone]
        public string? DriverPhoneNumber { get; set; }

        public string? VehicleNumber { get; set; }

        public string? City { get; set; }
        public bool? IsActive { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? Modifiedby { get; set; }
        public DateTime? ModifiedDate { get; set; }

        public string? VehicleRcNumber { get; set; }


        public IFormFile? VehicleRcCertificate { get; set; }
        public IFormFile? VehiclefitnessCertificate { get; set; }
        public IFormFile? ProfilePicture { get; set; }
        public IFormFile? VehicleInsuranceCertificate { get; set; }
        public IFormFile? DriverlicenseImage { get; set; }

        public string? DriverlicenseNumber { get; set; }
        public bool? IsApproved { get; set; }
        public int? ApprovedBy { get; set; }
        public int? CityId { get; set; }
        public int? ServicePartnerId { get; set; }
    }

    public class DriverDetailsDataModellist
    {
        public List<DriverDetailsDataModel>? driverDetailsDataModels { get; set; }
    }
    public class DriverRegisterationReponse
    {
        public int RegistrationId { get; set; }
        public string? vehicleNo { get; set; }
        public string? responseMessage { get; set; }
    }
    public class TotalVehicleCounts
    {
        public int ActiveVehicles { get; set; }
    }


    public class DriverDataModel
    {
        public int DriverDetailsId { get; set; }
        // [Required(ErrorMessage = "Required")]
        [Display(Name = "Driver FullName")]
        [StringLength(50)]
        public string? DriverName { get; set; }

        //[Required(ErrorMessage = "Required")]
        [RegularExpression(@"^([0-9]{10})$", ErrorMessage = "Please Enter Valid 10 Digit Mobile Number.")]
        [Display(Name = "Partner Mobile Number")]
        [DataType(DataType.PhoneNumber)]
        [Phone]
        public string? DriverPhoneNumber { get; set; }             
        public bool? IsActive { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? Modifiedby { get; set; }
        public DateTime? ModifiedDate { get; set; }     
        public IFormFile? ProfilePicture { get; set; }    
        public IFormFile? DriverlicenseImage { get; set; }
        public string? DriverlicenseNumber { get; set; }
        
        //public string DriverlicenseImage { get; set; }
        public bool? IsApproved { get; set; }
        public int? ApprovedBy { get; set; }
        public int? CityId { get; set; }
        public int? ServicePartnerId { get; set; }       
        public string? ProfilePictureString { get; set; }      
        public string? DriverlicenseImageString { get; set; }
       public string? ProfilePictureImgSrc { get; set; }       
        public string? DriverlicenseImgSrc { get; set; }
        public int? SPuserId { get; set; }
    }
    public class UpdateDriverDataModel : BaseViewModel
    {
        [Required(ErrorMessage = "Required")]
        public int DriverId { get; set; }
        [Display(Name = "Driver FullName")]
        [StringLength(50)]
        public string? DriverName { get; set; }

        [RegularExpression(@"^([0-9]{10})$", ErrorMessage = "Please Enter Valid 10 Digit Mobile Number.")]
        [Display(Name = "Partner Mobile Number")]
        [DataType(DataType.PhoneNumber)]
        [Phone]
        public string? DriverPhoneNumber { get; set; }
        public bool? IsActive { get; set; }
        public int? Modifiedby { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public IFormFile? ProfilePicture { get; set; }
        public IFormFile? DriverlicenseImage { get; set; }
        public string? DriverlicenseNumber { get; set; }
        public bool? IsApproved { get; set; }
        public int? CityId { get; set; }
        [Required(ErrorMessage = "Required")]
        public int? ServicePartnerId { get; set; }
        public int? SPuserId { get; set; }
    }

    public class VehicleDataModel
    {
        public int VehicleId { get; set; }
      //  [RegularExpression(@"^[A-Za-z]{2}\s[0-9]{1,2}\s[A-Za-z]{1,3}\s[0-9]{1,4}$", ErrorMessage = "Please Enter Valid Vehicle Number.")]       
        public string? VehicleNumber { get; set; }       
        public bool? IsActive { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? Modifiedby { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string? VehicleRcNumber { get; set; }

        //public string VehicleRcCertificate { get; set; }
        //public string VehiclefitnessCertificate { get; set; }
        //public string ProfilePicture { get; set; }
        //public string VehicleInsuranceCertificate { get; set; }

        public IFormFile? VehicleRcCertificate { get; set; }
        public IFormFile? VehiclefitnessCertificate { get; set; }       
        public IFormFile? VehicleInsuranceCertificate { get; set; }             
        public bool? IsApproved { get; set; }
        public int? ApprovedBy { get; set; }
        public int? CityId { get; set; }
        public int? ServicePartnerId { get; set; }
        public int? SPuserId { get; set; }

        //Added by VK
        public string? VehicleRcCertificateString { get; set; }
        public string? VehiclefitnessCertificateString { get; set; }      
        public string? VehicleInsuranceCertificateString { get; set; }       
        public string? VehicleRcCertificateImgSrc { get; set; }
        public string? VehiclefitnessCertificateImgSrc { get; set; }      
        public string? VehicleInsuranceCertificateImgSrc { get; set; }       
    }
    public class UpdateVehicleDataModel : BaseViewModel
    {
        [Required(ErrorMessage = "Required")]
        public int VehicleId { get; set; }                    
        public IFormFile? VehicleRcCertificate { get; set; }
        public IFormFile? VehiclefitnessCertificate { get; set; }        
        public IFormFile? VehicleInsuranceCertificate { get; set; }              
        public int? CityId { get; set; }
        public int? ServicePartnerId { get; set; }
        public int? SPuserId { get; set; }
    }

    public class DisableDriverDataModel 
    {       
        public int ServicePartnerId { get; set; }
        public int DriverId { get; set; }
    }
    public class DisableVehicleDataModel
    {
        public int ServicePartnerId { get; set; }
        public int vehicleId { get; set; }
    }
    public class PlanJourneyListDataModel
    {
        public int ServicePartnerId { get; set; }
        public int? DriverId { get; set; }
        public DateTime? startdate { get; set; }
        public DateTime? enddate { get; set; }
        public int? pagenumber { get; set; } = 1;
        public int? pagesize { get; set; } = 10;
    }
}
