using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDCELERP.Model.Base;

namespace RDCELERP.Model.LGCMobileApp
{
    public class VehicleViewModel : BaseViewModel
    {
        public int VehicleId { get; set; }
        public string? VehicleNumber { get; set;}
        public string? VehicleRCNumber { get; set;}
        public string? VehicleRCCertificate { get; set;}
        public string? VehiclefitnessCertificate { get; set;}
        public string? VehicleInsuranceCertificate { get; set;}
        public int CityId { get; set;}
        public string? CityName { get; set;}
        public int ServicePartnerId { get; set;}
        public string? ServicePartnerName { get; set;}
        public bool IsApproved { get; set;}
        public int ApprovedBy { get; set;}
        public string? ApprovedByName { get; set;}

    }
}
