using System;
using System.Collections.Generic;

#nullable disable

namespace RDCELERP.DAL.Entities
{
    public partial class Table2
    {
        public int BusinessPartnerId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string StoreCode { get; set; }
        public string QrcodeUrl { get; set; }
        public string Qrimage { get; set; }
        public string ContactPersonFirstName { get; set; }
        public string ContactPersonLastName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string Pincode { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public int? BusinessUnitId { get; set; }
        public bool? IsActive { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public bool? IsAbbbp { get; set; }
        public bool? IsExchangeBp { get; set; }
        public string FormatName { get; set; }
        public string SponsorName { get; set; }
        public string StoreType { get; set; }
        public string Gstnumber { get; set; }
        public string BankDetails { get; set; }
        public string AccountNo { get; set; }
        public string Ifsccode { get; set; }
        public string PaymentToCustomer { get; set; }
    }
}
