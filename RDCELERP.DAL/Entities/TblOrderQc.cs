using System;
using System.Collections.Generic;

namespace RDCELERP.DAL.Entities
{
    public partial class TblOrderQc
    {
        public int OrderQcid { get; set; }
        public int? OrderTransId { get; set; }
        public DateTime? ProposedQcdate { get; set; }
        public string? Qccomments { get; set; }
        public string? QualityAfterQc { get; set; }
        public decimal? PriceAfterQc { get; set; }
        public DateTime? Qcdate { get; set; }
        public int? BonusType { get; set; }
        public decimal? BonusbyQc { get; set; }
        public decimal? AdditionalBonus { get; set; }
        public int? PromotionalVoucherId { get; set; }
        public int? StatusId { get; set; }
        public bool? IsActive { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string? FrontViewImage { get; set; }
        public string? SideViewImage { get; set; }
        public string? SrNumberImage { get; set; }
        public string? InsideViewImage { get; set; }
        public string? PaymentImage { get; set; }
        public string? ShortVideoName { get; set; }
        public bool? IsPaymentConnected { get; set; }
        public decimal? CollectedAmount { get; set; }
        public int? Reschedulecount { get; set; }
        public bool? IsUpino { get; set; }
        public string? RescheduleNote { get; set; }
        public DateTime? PreferredPickupDate { get; set; }
        public string? Upiid { get; set; }
        public decimal? AverageSellingPrice { get; set; }
        public decimal? ExcellentPriceByAsp { get; set; }
        public decimal? QuotedPrice { get; set; }
        public decimal? Sweetner { get; set; }
        public decimal? QuotedWithSweetner { get; set; }
        public decimal? BonusPercentQc { get; set; }
        public decimal? BonusPercentAdmin { get; set; }
        public decimal? FinalCalculatedWeightage { get; set; }
        public string? PickupStartTime { get; set; }
        public string? PickupEndTime { get; set; }
        public string? DagnosticPdfName { get; set; }
        public bool? IsInvoiceValidated { get; set; }
        public bool? IsInstallationValidated { get; set; }
        public decimal? SweetenerBu { get; set; }
        public decimal? SweetenerBp { get; set; }
        public decimal? SweetenerDigi2l { get; set; }
        public decimal? Sweetener { get; set; }

        public virtual TblUser? ModifiedByNavigation { get; set; }
        public virtual TblOrderTran? OrderTrans { get; set; }
    }
}
