using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDCELERP.Model.Base;

namespace RDCELERP.Model.BusinessPartner
{
    public class BusinessPartnerVMExcel 
    {
        [NotMapped]
        public int BusinessPartnerId { get; set; }
        [Required]
        [RegularExpression(@"^[A-Za-z][a-zA-Z\s]*$", ErrorMessage = "Use letters only please")]
        [DisplayName("Name")]
        public string? Name { get; set; }
        public string? Description { get; set; }
        [Required]
        public string? StoreCode { get; set; }
        public string? ContactPersonFirstName { get; set; }

        public string? ContactPersonLastName { get; set; }

        [DataType(DataType.PhoneNumber)]
        [Display(Name = "Phone Number")]
        [Required(ErrorMessage = "Phone Number Required!")]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$",
                   ErrorMessage = "Entered phone format is not valid.")]
        public string? PhoneNumber { get; set; }

        [Required]
        [DataType(DataType.EmailAddress, ErrorMessage = "E-mail is not valid")]
        public string? Email { get; set; }

        [Required]
        [DisplayName("Address Line 1")]
        public string? AddressLine1 { get; set; }
        [DisplayName("Address Line 2")]
        public string? AddressLine2 { get; set; }
        [Required]
        [DisplayName("PinCode")]
        public string? Pincode { get; set; }
        [Required]
        [DisplayName("City Name")]
        public string? City { get; set; }
        [Required]
        [DisplayName("State Name")]
        public string? State { get; set; }
        public bool IsActive { get; set; }

        [DisplayName("Is ABB BP")]
        public bool IsAbbbp { get; set; }

        [DisplayName("Is Exchange BP")]
        public bool IsExchangeBp { get; set; }

        [Required]
        [DisplayName("Formate Name")]
        public string? FormatName { get; set; }

        [DisplayName("Is Dealer")]
        public bool IsDealer { get; set; }
        [Required]
        public string? AssociateCode { get; set; }
        [Required]
        public string? Bppassword { get; set; }

        [DisplayName("Is ORC")]
        public bool IsORC { get; set; }

        [DisplayName("Is Deffered Settlement")]
        public bool IsDefferedSettlement { get; set; }
        [Required]
        [DisplayName("Sponsor Name")]
        public string? SponsorName { get; set; }

        [DisplayName("Is Deffered Abb")]
        public bool IsDefferedAbb { get; set; }

        [DisplayName("Is D2C")]
        public bool IsD2c { get; set; }

        [DisplayName("Is Voucher")]
        public bool IsVoucher { get; set; }
        public int VoucherType { get; set; }
        public string? Remarks { get; set; }
}
}
