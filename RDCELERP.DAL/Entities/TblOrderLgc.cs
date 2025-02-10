using System;
using System.Collections.Generic;

namespace RDCELERP.DAL.Entities
{
    public partial class TblOrderLgc
    {
        public int OrderLgcid { get; set; }
        public int OrderTransId { get; set; }
        public DateTime? ProposedPickDate { get; set; }
        public string? Lgccomments { get; set; }
        public decimal? LgcpayableAmt { get; set; }
        public DateTime? ActualPickupDate { get; set; }
        public DateTime? ActualDropDate { get; set; }
        public string? SecondaryOrderFlag { get; set; }
        public int? StatusId { get; set; }
        public int? Evcpodid { get; set; }
        public bool? IsActive { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public int? LogisticId { get; set; }
        public string? CustDeclartionpdfname { get; set; }
        public int? DriverDetailsId { get; set; }
        public int? EvcregistrationId { get; set; }
        public bool? IsInvoiceGenerated { get; set; }
        public int? EvcpartnerId { get; set; }

        public virtual TblUser? CreatedByNavigation { get; set; }
        public virtual TblDriverDetail? DriverDetails { get; set; }
        public virtual TblEvcPartner? Evcpartner { get; set; }
        public virtual TblEvcpoddetail? Evcpod { get; set; }
        public virtual TblEvcregistration? Evcregistration { get; set; }
        public virtual TblLogistic? Logistic { get; set; }
        public virtual TblUser? ModifiedByNavigation { get; set; }
        public virtual TblOrderTran OrderTrans { get; set; } = null!;
    }
}
