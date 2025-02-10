using System;
using System.Collections.Generic;

#nullable disable

namespace RDCELERP.DAL.Entities
{
    public partial class TblOrderQcOld
    {
        public int OrderQcid { get; set; }
        public int OrderTransId { get; set; }
        public DateTime? ProposedQcdate { get; set; }
        public string Qccomments { get; set; }
        public string QualityAfterQc { get; set; }
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
        public string FrontViewImage { get; set; }
        public string SideViewImage { get; set; }
        public string SrNumberImage { get; set; }
        public string InsideViewImage { get; set; }
        public string PaymentImage { get; set; }
        public string ShortVideoName { get; set; }
        public bool? IsPaymentConnected { get; set; }
        public decimal? CollectedAmount { get; set; }
        public int? Reschedulecount { get; set; }
        public bool? IsUpino { get; set; }
        public string RescheduleNote { get; set; }

        public virtual TblUser CreatedByNavigation { get; set; }
        public virtual TblUser ModifiedByNavigation { get; set; }
    }
}
