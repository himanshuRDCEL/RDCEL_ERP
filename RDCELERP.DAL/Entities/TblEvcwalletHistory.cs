using System;
using System.Collections.Generic;

namespace RDCELERP.DAL.Entities
{
    public partial class TblEvcwalletHistory
    {
        public int WalletHistoryId { get; set; }
        public int EvcregistrationId { get; set; }
        public int? OrderTransId { get; set; }
        public decimal? CurrentWalletAmount { get; set; }
        public decimal? AddAmount { get; set; }
        public bool? AmountAdditionFlag { get; set; }
        public decimal? FinalOrderAmount { get; set; }
        public bool? AmountdeductionFlag { get; set; }
        public decimal? BalanceWalletAmount { get; set; }
        public bool? IsActive { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string? TransactionId { get; set; }
        public int? EvcpartnerId { get; set; }

        public virtual TblUser? CreatedByNavigation { get; set; }
        public virtual TblEvcPartner? Evcpartner { get; set; }
        public virtual TblEvcregistration Evcregistration { get; set; } = null!;
        public virtual TblUser? ModifiedByNavigation { get; set; }
        public virtual TblOrderTran? OrderTrans { get; set; }
    }
}
