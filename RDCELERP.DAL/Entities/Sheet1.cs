using System;
using System.Collections.Generic;

#nullable disable

namespace RDCELERP.DAL.Entities
{
    public partial class Sheet1
    {
        public string RegdNo { get; set; }
        public string SponsorOrderNo { get; set; }
        public double? ZohoSponsorOrderId { get; set; }
        public double? OrderFlag { get; set; }
        public string EMailSmsFlag { get; set; }
        public string QcFlag { get; set; }
        public string InstallationFlag { get; set; }
        public string PickupFlag { get; set; }
        public string PaymentFlag { get; set; }
        public string EvcDropFlag { get; set; }
        public string PostingFlag { get; set; }
        public string InvoiceNo { get; set; }
        public string InvoiceDate { get; set; }
        public string EvcCode { get; set; }
        public string AllocationStatus { get; set; }
        public double? FinalStatusId { get; set; }
        public double? FinalStatusCode { get; set; }
        public double? CurrentStatusId { get; set; }
        public string CurrentStatusCode { get; set; }
        public string Rnumber { get; set; }
        public double? UpdateInSystem { get; set; }
        public string StatusMatching { get; set; }
        public double? IsDataTransfered { get; set; }
        public string QcDate { get; set; }
        public string ActualProdQltyAtTimeOfQc { get; set; }
        public string QcComment { get; set; }
        public string LgcTktCreatedDate { get; set; }
        public string LogisticsStatusRemark { get; set; }
        public string PickupDate { get; set; }
        public string LgcTktNo { get; set; }
        public string LogisticBy { get; set; }
        public string EvcDropDate { get; set; }
        public string PostingDate { get; set; }
    }
}
