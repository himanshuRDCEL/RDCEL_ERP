using System;
using System.Collections.Generic;

namespace RDCELERP.DAL.Entities
{
    public partial class TblEvcdispute
    {
        public int EvcdisputeId { get; set; }
        public int? EvcregistrationId { get; set; }
        public int? ExchangeOrderId { get; set; }
        public int? ProductTypeId { get; set; }
        public int? ProductCatId { get; set; }
        public string? Status { get; set; }
        public string? LevelStatus { get; set; }
        public decimal? Amount { get; set; }
        public bool? IsActive { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string? Evcdisputedescription { get; set; }
        public string? Digi2Lresponse { get; set; }
        public string? Image1 { get; set; }
        public string? Image2 { get; set; }
        public string? Image3 { get; set; }
        public string? Image4 { get; set; }
        public int? OrderTransId { get; set; }
        public string? DisputeRegno { get; set; }

        public virtual TblUser? CreatedByNavigation { get; set; }
        public virtual TblEvcregistration? Evcregistration { get; set; }
        public virtual TblExchangeOrder? ExchangeOrder { get; set; }
        public virtual TblUser? ModifiedByNavigation { get; set; }
        public virtual TblOrderTran? OrderTrans { get; set; }
    }
}
