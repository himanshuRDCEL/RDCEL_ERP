using System;
using System.Collections.Generic;

#nullable disable

namespace RDCELERP.DAL.Entities
{
    public partial class Flag12UpdateRequired
    {
        public string EvcAllocated { get; set; }
        public string RegdNo { get; set; }
        public string PodGenerated { get; set; }
        public string InvoiceNo { get; set; }
        public string DebitNoteNo { get; set; }
        public string Date { get; set; }
        public double? DnAmount { get; set; }
        public double? InvoiceAmount { get; set; }
        public string CompanyName { get; set; }
        public string SponsorOrderNumber { get; set; }
        public string StoreCode { get; set; }
        public string StoreName { get; set; }
        public DateTime? OrderDate { get; set; }
        public string CurrentStatus { get; set; }
        public double? FlagUpdate { get; set; }
        public string StatusName { get; set; }
        public string StatusDescription { get; set; }
        public string ProductCat { get; set; }
        public string ProductType { get; set; }
        public string ProductCondition { get; set; }
        public double? ExchangePrice { get; set; }
        public double? Sweetener { get; set; }
        public DateTime? SelfQcdate { get; set; }
        public DateTime? ProposedQcdateTime { get; set; }
        public DateTime? Qcdate { get; set; }
        public string Qccomments { get; set; }
        public string QualityAfterQc { get; set; }
        public double? PriceAfterQc { get; set; }
        public DateTime? EvcAssignedDate { get; set; }
        public DateTime? ProposedPickDate { get; set; }
        public string LogisticsPartner { get; set; }
        public string TicketNumber { get; set; }
        public DateTime? TicketGenerationDate { get; set; }
        public string Lgccomments { get; set; }
        public string LgcpayableAmt { get; set; }
        public DateTime? ActualPickupDate { get; set; }
        public DateTime? ActualDropDate { get; set; }
        public string CustName { get; set; }
        public string CustMobile { get; set; }
        public string CustEMail { get; set; }
        public string CustCity { get; set; }
        public string CustState { get; set; }
        public string CustZipCode { get; set; }
        public string BrandName { get; set; }
        public string IsFromHome { get; set; }
        public string IsOrc { get; set; }
        public double? VoucherAmount { get; set; }
        public string VoucherNo { get; set; }
        public string VoucherStatus { get; set; }
        public string VoucherStatusDetail { get; set; }
        public string PodName { get; set; }
    }
}
