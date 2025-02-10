using System;
using System.Collections.Generic;

namespace RDCELERP.DAL.Entities
{
    public partial class ViewAllExchangeDatum
    {
        public string? RegdNo { get; set; }
        public int? BusinessUnitId { get; set; }
        public string? CompanyName { get; set; }
        public string? SponsorOrderNumber { get; set; }
        public DateTime? OrderDate { get; set; }
        public string? CurrentStatus { get; set; }
        public string? StatusName { get; set; }
        public string? StatusDescription { get; set; }
        public string? ProductCondition { get; set; }
        public decimal? ExchangePrice { get; set; }
        public decimal? Sweetener { get; set; }
        public string? EmployeeId { get; set; }
        public DateTime? SelfQcdate { get; set; }
        public string? Qcflag { get; set; }
        public string? ProposedQcdateTime { get; set; }
        public DateTime? Qcdate { get; set; }
        public string? Qccomments { get; set; }
        public string? Upiid { get; set; }
        public string? QualityAfterQc { get; set; }
        public decimal? PriceAfterQc { get; set; }
        public string? PreferredPickupDate { get; set; }
        public string? EvcAllocated { get; set; }
        public decimal? Evcprice { get; set; }
        public DateTime? EvcAssignedDate { get; set; }
        public string? LogisticsPartner { get; set; }
        public string? TicketNumber { get; set; }
        public DateTime? TicketGenerationDate { get; set; }
        public DateTime? CancellationDate { get; set; }
        public DateTime? Rescheduledate { get; set; }
        public DateTime? ProposedPickDate { get; set; }
        public DateTime? ActualPickupDate { get; set; }
        public DateTime? ActualDropDate { get; set; }
        public string? Lgccomments { get; set; }
        public decimal? LgcpayableAmt { get; set; }
        public string? PodName { get; set; }
        public string? BusinessPartnerName { get; set; }
        public string? BusinessUnit { get; set; }
        public string? Expr1 { get; set; }
        public string? ProductCat { get; set; }
        public string? ProdCatCode { get; set; }
        public string? ProductType { get; set; }
        public string? ProdTypeCode { get; set; }
        public string? BrandName { get; set; }
        public string? Pincode { get; set; }
        public string? City { get; set; }
        public string IsFromHome { get; set; } = null!;
        public DateTime? OrderDate1 { get; set; }
        public string? VoucherNo { get; set; }
        public DateTime? IssueDate { get; set; }
        public string VoucherStatus { get; set; } = null!;
        public decimal? VoucherAmount { get; set; }
        public decimal? OldProductPrice { get; set; }
        public decimal? SweetenerAmount { get; set; }
        public string? VoucherStatusDetail { get; set; }
        public string? CustName { get; set; }
        public string? CustEMail { get; set; }
        public string? CustMobile { get; set; }
        public string? CustCity { get; set; }
        public string? CustState { get; set; }
        public string? CustZipCode { get; set; }
        public string IsOrc { get; set; } = null!;
        public string? StoreName { get; set; }
        public string? StoreCode { get; set; }
        public DateTime? InstallationDate { get; set; }
        public string? SponsorServiceRefId { get; set; }
    }
}
