using System;
using System.Collections.Generic;

#nullable disable

namespace RDCELERP.DAL.Entities
{
    public partial class ViewBoschDeferred
    {
        public string RegdNo { get; set; }
        public string CompanyName { get; set; }
        public string SponsorOrderNumber { get; set; }
        public string StoreName { get; set; }
        public string StoreCode { get; set; }
        public DateTime? OrderDate { get; set; }
        public string CustName { get; set; }
        public string CustEMail { get; set; }
        public string CustMobile { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string CustCity { get; set; }
        public string State { get; set; }
        public string CustZipCode { get; set; }
        public string OldProdGroup { get; set; }
        public string OldProdType { get; set; }
        public string Size { get; set; }
        public string Brandname { get; set; }
        public string ProductCondition { get; set; }
        public string VoucherCode { get; set; }
        public decimal? VoucherAmt { get; set; }
        public decimal? ExchangeAmt { get; set; }
        public decimal? OldProductPrice { get; set; }
        public decimal? SweetenerAmount { get; set; }
        public decimal? PriceAfterQc { get; set; }
        public DateTime? InvDate { get; set; }
        public string InvoiceUrl { get; set; }
        public string EstimatedDeliveryDate { get; set; }
        public string NewProdGroup { get; set; }
        public string NewProdType { get; set; }
        public string NewProductModelNo { get; set; }
    }
}
