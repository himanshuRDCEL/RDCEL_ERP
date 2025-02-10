using System;
using System.Collections.Generic;

#nullable disable

namespace RDCELERP.DAL.Entities
{
    public partial class AllAbbTempTablesDatum
    {
        public int AbbdataId { get; set; }
        public bool? IsDataTransfered { get; set; }
        public string SponsorName { get; set; }
        public string SponsorOrderNo { get; set; }
        public string RegdNo { get; set; }
        public string CustomerName { get; set; }
        public string CustomerMobile { get; set; }
        public string CustomerEMail { get; set; }
        public string CustomerAddress1 { get; set; }
        public string CustomerAddress2 { get; set; }
        public double? CustomerPinCode { get; set; }
        public string CustomerCity { get; set; }
        public string CustomerState { get; set; }
        public string NewProdGroup { get; set; }
        public string NewProdType { get; set; }
        public string NewBrand { get; set; }
        public string NewSize { get; set; }
        public string ProdSrNo { get; set; }
        public string ModelNo { get; set; }
        public string AbbPlanName { get; set; }
        public string HsnCodeForAbbFees { get; set; }
        public DateTime? InvoiceDate { get; set; }
        public string InvoiceNo { get; set; }
        public string NewPrice { get; set; }
        public double? AbbFees { get; set; }
        public double? AbbPlanPeriodMonths { get; set; }
        public string OrderType { get; set; }
        public string SponsorProgCode { get; set; }
        public string AbbPriceId { get; set; }
        public string UploadDateTime { get; set; }
        public DateTime? UploadDate { get; set; }
        public string SponsorStatus { get; set; }
        public string StoreCode { get; set; }
        public string StoreName { get; set; }
        public string Location { get; set; }
        public string InvoiceImage { get; set; }
        public string MarCom { get; set; }
        public string Approved { get; set; }
        public string Id { get; set; }
        public double? ProductNetPriceInclOfGst { get; set; }
        public string IsAbbPaymentDone { get; set; }
        public string PaymentRemark { get; set; }
        public string TransactionId { get; set; }
        public string OrderId { get; set; }
    }
}
