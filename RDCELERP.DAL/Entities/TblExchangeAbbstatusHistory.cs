using System;
using System.Collections.Generic;

namespace RDCELERP.DAL.Entities
{
    public partial class TblExchangeAbbstatusHistory
    {
        public int StatusHistoryId { get; set; }
        public int OrderType { get; set; }
        public string? SponsorOrderNumber { get; set; }
        public string? RegdNo { get; set; }
        public string? ZohoSponsorId { get; set; }
        public int? CustId { get; set; }
        public int? StatusId { get; set; }
        public bool? IsActive { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public int? OrderTransId { get; set; }
        public string? JsonObjectString { get; set; }
        public string? Comment { get; set; }
        public int? Evcid { get; set; }
        public int? ServicepartnerId { get; set; }
        public int? DriverDetailId { get; set; }

        public virtual TblUser? CreatedByNavigation { get; set; }
        public virtual TblDriverDetail? DriverDetail { get; set; }
        public virtual TblUser? ModifiedByNavigation { get; set; }
        public virtual TblOrderTran? OrderTrans { get; set; }
        public virtual TblServicePartner? Servicepartner { get; set; }
        public virtual TblExchangeOrderStatus? Status { get; set; }
    }
}
