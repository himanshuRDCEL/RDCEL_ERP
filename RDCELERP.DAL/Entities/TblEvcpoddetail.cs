using System;
using System.Collections.Generic;

namespace RDCELERP.DAL.Entities
{
    public partial class TblEvcpoddetail
    {
        public TblEvcpoddetail()
        {
            TblOrderLgcs = new HashSet<TblOrderLgc>();
        }

        public int Id { get; set; }
        public string? RegdNo { get; set; }
        public int? ExchangeId { get; set; }
        public int? Evcid { get; set; }
        public string? Podurl { get; set; }
        public int? AbbredemptionId { get; set; }
        public bool? IsActive { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string? DebitNotePdfName { get; set; }
        public string? InvoicePdfName { get; set; }
        public int? InvSrNum { get; set; }
        public DateTime? InvoiceDate { get; set; }
        public decimal? InvoiceAmount { get; set; }
        public DateTime? DebitNoteDate { get; set; }
        public decimal? DebitNoteAmount { get; set; }
        public int? DnsrNum { get; set; }
        public int? OrderTransId { get; set; }
        public string? FinancialYear { get; set; }
        public int? EvcpartnerId { get; set; }

        public virtual TblUser? CreatedByNavigation { get; set; }
        public virtual TblEvcregistration? Evc { get; set; }
        public virtual TblEvcPartner? Evcpartner { get; set; }
        public virtual TblUser? ModifiedByNavigation { get; set; }
        public virtual TblOrderTran? OrderTrans { get; set; }
        public virtual ICollection<TblOrderLgc> TblOrderLgcs { get; set; }
    }
}
