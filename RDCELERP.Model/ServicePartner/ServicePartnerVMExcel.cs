using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDCELERP.Model.ServicePartner
{
    public class ServicePartnerVMExcel
    {
        public int ServicePartnerId { get; set; }
        
        public string? ServicePartnerName { get; set; }
      
        [Required(ErrorMessage = "Service partner description name is required.")]
        public string? ServicePartnerDescription { get; set; }
        public bool IsServicePartnerLocal { get; set; }
       
        [Required(ErrorMessage = "Mobile number is required.")]
        [RegularExpression(@"^([0-9]{10})$", ErrorMessage = "Please Enter Valid 10 Digit Mobile Number.")]
        public string? ServicePartnerMobileNumber { get; set; }
        [RegularExpression(@"^([0-9]{10})$", ErrorMessage = "Please Enter Valid 10 Digit Mobile Number.")]
        public string? ServicePartnerAlternateMobileNumber { get; set; }
        [Required(ErrorMessage = "Email is required.")]
        [RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$", ErrorMessage = "Please enter a valid e-mail adress")]
        public string? ServicePartnerEmailId { get; set; }
        [Required(ErrorMessage = "Address line 1 is required.")]
        public string? ServicePartnerAddressLine1 { get; set; }
        public string? ServicePartnerAddressLine2 { get; set; }
        [Required(ErrorMessage = "City is required.")]
        public string? ServicePartnerCities { get; set; }
        [Required(ErrorMessage = "State is required.")]
        public string? ServicePartnerStates { get; set; }
       
        [RegularExpression(@"^[0-9]{2}[A-Z]{5}[0-9]{4}[A-Z]{1}[1-9A-Z]{1}Z[0-9A-Z]{1}$", ErrorMessage = "Please Enter Valid GSTNo")]
     
        public string? ServicePartnerGstno { get; set; }
        [Required(ErrorMessage = "Service partner first name is required.")]
        [RegularExpression(@"^[A-Za-z][a-zA-Z\s]*$", ErrorMessage = "Use letters only please")]
        public string? ServicePartnerFirstName { get; set; }
        [Required(ErrorMessage = "Service partner last name is required.")]
        [RegularExpression(@"^[A-Za-z][a-zA-Z\s]*$", ErrorMessage = "Use letters only please")]
        public string? ServicePartnerLastName { get; set; }
        [Required(ErrorMessage = "Service partner business name is required.")]
        [RegularExpression(@"^[A-Za-z][a-zA-Z\s]*$", ErrorMessage = "Use letters only please")]
        public string? ServicePartnerBusinessName { get; set; }

        public bool IconfirmTermsCondition { get; set; }
       
        public bool IsServicePartnerEVCalso { get; set; }
        public string? Remarks { get; set; }
        public bool IsActive { get; set; }

    }
}
