using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDCELERP.Model.Base;

namespace RDCELERP.Model.BusinessUnit
{
    public class BUViewModel : BaseViewModel
    {
        public int BusinessUnitId { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? RegistrationNumber { get; set; }
        public string? QrcodeUrl { get; set; }
        public string? LogoName { get; set; }
        public string? ContactPersonFirstName { get; set; }
        public string? ContactPersonLastName { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }
        public string? AddressLine1 { get; set; }
        public string? AddressLine2 { get; set; }
        public string? Pincode { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public int? LoginId { get; set; }
        public string? ZohoSponsorId { get; set; }
        public int? ExpectedDeliveryHours { get; set; }
        public int? VoucherExpiryTime { get; set; }
        public decimal? SweetnerForDtd { get; set; }
        public decimal? SweetnerForDtc { get; set; }
        public bool? IsSweetnerModelBased { get; set; }
        public bool? IsAbb { get; set; }
        public bool? IsExchange { get; set; }
        public bool? IsBumultiBrand { get; set; }
        public bool? IsBud2c { get; set; }
        public bool? ShowAbbPlan { get; set; }
        public bool? IsQualityWorkingNonWorking { get; set; }
        public bool? IsInvoiceDetailsRequired { get; set; }
        public bool? IsNewProductDetailsRequired { get; set; }
        public int? Gsttype { get; set; }
        public int? MarginType { get; set; }
        public bool? IsSponsorNumberRequiredOnUi { get; set; }
        public bool? IsUpiIdRequired { get; set; }
        public bool? IsPaymentThirdParty { get; set; }
        public bool? IsModelDetailRequired { get; set; }
        public bool? IsNewBrandRequired { get; set; }
        public bool? IsAreaLocality { get; set; }
        public bool? IsValidationBasedSweetner { get; set; }
    }
}
