using System;
using System.Collections.Generic;

namespace RDCELERP.DAL.Entities
{
    public partial class ViewAllExchangeDataForPinelab
    {
        public string? RegdNo { get; set; }
        public string? TransactionId { get; set; }
        public string? Utrnumber { get; set; }
        public string? SponsorOrderNumber { get; set; }
        public DateTime? OrderDate { get; set; }
        public DateTime? DateOfTrx { get; set; }
        public string? CurrentStatus { get; set; }
        public string? PickUpStatus { get; set; }
        public string? StatusName { get; set; }
        public string? StatusDescription { get; set; }
        public string? ProductCondition { get; set; }
        public decimal? ExchangePrice { get; set; }
        public decimal? Sweetener { get; set; }
        public decimal? OroembonusAmount { get; set; }
        public DateTime? ProposedPickDate { get; set; }
        public DateTime? ActualPickupDate { get; set; }
        public DateTime? DateOfDevicePickUp { get; set; }
        public DateTime? ActualDropDate { get; set; }
        public string? Lgccomments { get; set; }
        public decimal? Lgcamount1 { get; set; }
        public string? VoucherCode { get; set; }
        public string? CompanyName { get; set; }
        public decimal? Amount { get; set; }
        public decimal? OembonusAmount { get; set; }
    }
}
