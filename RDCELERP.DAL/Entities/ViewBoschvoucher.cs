using System;
using System.Collections.Generic;

#nullable disable

namespace RDCELERP.DAL.Entities
{
    public partial class ViewBoschvoucher
    {
        public string RegdNo { get; set; }
        public string PayerCode { get; set; }
        public string PayerPhone { get; set; }
        public string PayerEmail { get; set; }
        public string StoreName { get; set; }
        public string Location { get; set; }
        public string Pincode { get; set; }
        public string City { get; set; }
        public string IsFromHome { get; set; }
        public DateTime? OrderDate { get; set; }
        public string CustName { get; set; }
        public string CustEMail { get; set; }
        public string CustMobile { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string CustCity { get; set; }
        public string CustZipCode { get; set; }
        public string OldProdGroup { get; set; }
        public string OldProdType { get; set; }
        public string Brandname { get; set; }
        public string VoucherNo { get; set; }
        public DateTime? IssueDate { get; set; }
        public string VoucherStatus { get; set; }
        public decimal? VoucherAmt { get; set; }
        public decimal? OldProductPrice { get; set; }
        public decimal? SweetenerAmount { get; set; }
        public string NewProductType { get; set; }
        public string NewProductModelNo { get; set; }
        public DateTime? InvDate { get; set; }
        public string InvoiceUrl { get; set; }
        public string ProductCondition { get; set; }
        public string ZohoSponsorOrderId { get; set; }
        public string StoreCode { get; set; }
        public string SponsorOrderNumber { get; set; }
        public string EstimatedDeliveryDate { get; set; }
        public string Size { get; set; }
        public string VoucherStatusName { get; set; }
        public int? VoucherStatusId { get; set; }
        public bool? IsVoucherused { get; set; }
    }
}
