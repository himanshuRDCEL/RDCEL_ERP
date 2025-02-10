using System;
using System.Collections.Generic;

namespace RDCELERP.DAL.Entities
{
    public partial class TblEvcwalletAddition
    {
        public int EvcwalletAdditionId { get; set; }
        public int EvcregistrationId { get; set; }
        public decimal Amount { get; set; }
        public bool? IsActive { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string? TransactionId { get; set; }
        public DateTime? TransactionDate { get; set; }
        public string? InvoiceImage { get; set; }
        public string? Comments { get; set; }
        public int? Rechargeby { get; set; }
        public int? RecipetNumber { get; set; }
        public bool? IsCreaditNote { get; set; }

        public virtual TblUser? CreatedByNavigation { get; set; }
        public virtual TblUser? ModifiedByNavigation { get; set; }
    }
}
