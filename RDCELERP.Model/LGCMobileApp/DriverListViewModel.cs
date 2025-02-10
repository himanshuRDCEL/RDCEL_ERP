using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDCELERP.Model.Base;

namespace RDCELERP.Model.LGCMobileApp
{
    public class DriverListViewModel : BaseViewModel
    {
        public int DriverId { get; set; }
        public string? DriverName { get; set; }
        public string? DriverPhoneNumber { get; set; }
        public int? UserId { get; set; }
        public int? CityId { get; set; }
        public string? CityName { get; set; }
        public int? ServicePartnerId { get; set; }
        public string? ServicePartnerName { get; set; }
        public string? DriverLicenseNumber { get; set; }
        public string? DriverLicenseImage { get; set; }
        public bool? IsApproved { get; set; }
        public int? ApprovedBy { get; set; }
        public string? ApprovedByName { get; set; }
        public string? ProfilePicture { get; set; }
       

    }
}
