using System;
using System.Collections.Generic;

namespace RDCELERP.DAL.Entities
{
    public partial class TblWalletTransaction
    {
        public TblWalletTransaction()
        {
            TblCreditRequests = new HashSet<TblCreditRequest>();
        }

        public int WalletTransactionId { get; set; }
        public int? EvcregistrationId { get; set; }
        public string? RegdNo { get; set; }
        public string? SponserOrderNo { get; set; }
        public string? StatusId { get; set; }
        public decimal? OrderAmount { get; set; }
        public DateTime? OrderOfDeliverdDate { get; set; }
        public DateTime? OrderOfCompleteDate { get; set; }
        public DateTime? OrderOfInprogressDate { get; set; }
        public string? OrderType { get; set; }
        public bool? IsActive { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string? ZohoAllocationId { get; set; }
        public DateTime? OrderofAssignDate { get; set; }
        public int? OrderTransId { get; set; }
        public bool? IsPrimeProductId { get; set; }
        public int? OldEvcid { get; set; }
        public int? NewEvcid { get; set; }
        public int? ReassignCount { get; set; }
        public int? EvcpartnerId { get; set; }
        public bool? IsOrderAmtWithSweetener { get; set; }
        public int? GsttypeId { get; set; }
        public decimal? BaseValue { get; set; }
        public decimal? Cgstamt { get; set; }
        public decimal? Sgstamt { get; set; }
        public decimal? Igstamt { get; set; }
        public decimal? SweetenerAmt { get; set; }
        public int? Lgccost { get; set; }
        public decimal? PrimeProductDiffAmt { get; set; }

        public virtual TblUser? CreatedByNavigation { get; set; }
        public virtual TblEvcPartner? Evcpartner { get; set; }
        public virtual TblEvcregistration? Evcregistration { get; set; }
        public virtual TblLoV? Gsttype { get; set; }
        public virtual TblUser? ModifiedByNavigation { get; set; }
        public virtual TblOrderTran? OrderTrans { get; set; }
        public virtual ICollection<TblCreditRequest> TblCreditRequests { get; set; }
    }
}
