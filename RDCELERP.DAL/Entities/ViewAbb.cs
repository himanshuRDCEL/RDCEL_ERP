using System;
using System.Collections.Generic;

namespace RDCELERP.DAL.Entities
{
    public partial class ViewAbb
    {
        public int AbbregistrationId { get; set; }
        public string? EmployeeId { get; set; }
        public int? BusinessUnitId { get; set; }
        public string? RegdNo { get; set; }
        public string? SponsorOrderNo { get; set; }
        public string? CustFirstName { get; set; }
        public string? CustLastName { get; set; }
        public string? CustMobile { get; set; }
        public string? CustEmail { get; set; }
        public string? CustAddress1 { get; set; }
        public string? CustAddress2 { get; set; }
        public string? Location { get; set; }
        public string? CustPinCode { get; set; }
        public string? CustCity { get; set; }
        public string? CustState { get; set; }
        public int? NewProductCategoryId { get; set; }
        public int? NewProductCategoryTypeId { get; set; }
        public int? NewBrandId { get; set; }
        public string? NewSize { get; set; }
        public string? ProductSrNo { get; set; }
        public int? ModelNumberId { get; set; }
        public string? AbbplanName { get; set; }
        public string? Hsncode { get; set; }
        public DateTime? InvoiceDate { get; set; }
        public string? InvoiceNo { get; set; }
        public decimal? NewPrice { get; set; }
        public decimal? Abbfees { get; set; }
        public string? OrderType { get; set; }
        public string? SponsorProdCode { get; set; }
        public int? AbbpriceId { get; set; }
        public DateTime? UploadDateTime { get; set; }
        public int? BusinessPartnerId { get; set; }
        public bool? IsActive { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string? YourRegistrationNo { get; set; }
        public string? InvoiceImage { get; set; }
        public string? AbbplanPeriod { get; set; }
        public string? NoOfClaimPeriod { get; set; }
        public decimal? ProductNetPrice { get; set; }
        public bool? OtherCommunications { get; set; }
        public bool? FollowupCommunication { get; set; }
        public int? StateId { get; set; }
        public string? NewProductCategoryName { get; set; }
        public string? NewProductCategoryType { get; set; }
        public bool? AbbApprove { get; set; }
        public int? CustomerId { get; set; }
        public int? OrderId { get; set; }
        public string? StoreManagerEmail { get; set; }
        public string? UploadImage { get; set; }
        public bool? SponsorStatus { get; set; }
        public string? MarCom { get; set; }
        public bool? AbbReject { get; set; }
        public bool? PaymentStatus { get; set; }
        public string? OtherModelNo { get; set; }
        public int? StatusId { get; set; }
        public string? Company { get; set; }
        public string? StoreName { get; set; }
        public string? StoreCode { get; set; }
        public string? StoreLocation { get; set; }
        public string? ProductGroup { get; set; }
        public string? ProductType { get; set; }
        public string? ModelName { get; set; }
        public string? Brand { get; set; }
        public string? InvoicePath { get; set; }
    }
}
