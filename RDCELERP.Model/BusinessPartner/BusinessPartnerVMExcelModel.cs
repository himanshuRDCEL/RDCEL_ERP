using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDCELERP.Model.BusinessPartner
{
    public class BusinessPartnerVMExcelModel
    {
        public int BusinessPartnerId { get; set; }
        [Required(ErrorMessage = "Email is required.")]
        [RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$", ErrorMessage = "Please enter a valid e-mail adress")]
        public string? Email { get; set; }
        [Required(ErrorMessage = "Store code is required.")]
        [RegularExpression("^[ A-Za-z0-9 +-]*$", ErrorMessage = "Only Alphabets and Numbers allowed.")]
        public string? StoreCode { get; set; }
        [Required(ErrorMessage = "Associate code is required.")]
        public string? AssociateCode { get; set; }
        
        public string? StoreType { get; set; }

        [Required(ErrorMessage = "Pin code is required.")]
        [RegularExpression(@"^[1-9][0-9]{5}$", ErrorMessage = "Please enter valid 6 digit pin code.")]
        [DisplayName("PinCode")]
        public string? Pincode { get; set; }

        [Required(ErrorMessage = "Sponsor name is required.")]
        [DisplayName("Sponsor Name")]
        public string? CompanyName { get; set; }
        public string? ContactPersonFirstName { get; set; }
        public string? ContactPersonLastName { get; set; }

        [Required(ErrorMessage = "Phone number is required.")]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$",
                   ErrorMessage = "Entered phone format is not valid.")]
        [Display(Name = "Phone Number")]
        public string? PhoneNumber { get; set; }

        [Required(ErrorMessage = "Address line 1 is required.")]
        [DisplayName("Address Line 1")]

        public string? AddressLine1 { get; set; }
        [DisplayName("Address Line 2")]

        public string? AddressLine2 { get; set; }
        [Required(ErrorMessage = "State is require.")]
        [DisplayName("State Name")]

        public string? State { get; set; }
        public int StateId { get; set; }
        [Required(ErrorMessage = "City is required.")]
        [DisplayName("City Name")]

        public string? City { get; set; }
        public int CityId { get; set; }
        public bool Selected { get; set; }
        public int BusinessUnitId { get; set; }
        [DisplayName("Is ABB BP")]
        public bool IsAbbbp { get; set; }
        [DisplayName("Is Exchange BP")]
        public bool IsExchangeBp { get; set; }
        [DisplayName("Is ORC")]
        public bool IsORC { get; set; }

        [Required(ErrorMessage = "Format name is required.")]
        public string? FormatName { get; set; }
        [DisplayName("GST Number")]
        public string? Gstnumber { get; set; }

        [DisplayName("Bank Details")]
        public string? BankDetails { get; set; }

        [DisplayName("Account Number")]
        public string? AccountNo { get; set; }

        [DisplayName("IFSC Code")]
        public string? Ifsccode { get; set; }

        [DisplayName("Payment To Customer")]
        public bool? PaymentToCustomer { get; set; }
        [Required(ErrorMessage = "Name is required.")]
        [RegularExpression(@"^[A-Za-z][a-zA-Z\s]*$", ErrorMessage = "Use letters only please")]
        [DisplayName("Name")]
        public string? Name { get; set; }
        public string? Description { get; set; }
      
        [DisplayName("Is Dealer")]
        public bool IsDealer { get; set; }
        [Required(ErrorMessage = "Business partner's password is required.")]
        public string? Bppassword { get; set; }
        [DisplayName("Is Deffered Settlement")]
        public bool IsDefferedSettlement { get; set; }
        [DisplayName("Is Deffered Abb")]
        public bool IsDefferedAbb { get; set; }
        [DisplayName("Is D2C")]
        public bool IsD2c { get; set; }
        [DisplayName("Is Voucher")]
        public bool IsVoucher { get; set; }
        [Required(ErrorMessage = "Voucher type is required.")]
        public string? VoucherType { get; set; }
        public string? Date { get; set; }
        public string? Remarks { get; set; }       
        public string? LabelName_Excellent_P { get; set; }
        public string? LabelName_Good_Q { get; set; }
        public string? LabelName_Average_R { get; set; }
        public string? LabelName_NonWorking_S { get; set; }
        public bool IsSweetenerApplicable_P { get; set; }
        public bool IsSweetenerApplicable_Q { get; set; }
        public bool IsSweetenerApplicable_R { get; set; }
        public bool IsSweetenerApplicable_S { get; set; }
        public string? Upiid { get; set; }
    }
}
