using System;
using System.Collections.Generic;

namespace RDCELERP.DAL.Entities
{
    public partial class TblCreditRequest
    {
        public int CreditRequestId { get; set; }
        public int? WalletTransactionId { get; set; }
        public bool? IsCreditRequest { get; set; }
        public bool? IsCreditRequestApproved { get; set; }
        public int? CreditRequestUserId { get; set; }
        public int? CreditRequestApproveUserId { get; set; }
        public bool? IsActive { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }

        public virtual TblUser? CreatedByNavigation { get; set; }
        public virtual TblUser? CreditRequestApproveUser { get; set; }
        public virtual TblUser? CreditRequestUser { get; set; }
        public virtual TblUser? ModifiedByNavigation { get; set; }
        public virtual TblWalletTransaction? WalletTransaction { get; set; }
    }
}
