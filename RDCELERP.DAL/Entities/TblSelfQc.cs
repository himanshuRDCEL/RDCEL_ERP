using System;
using System.Collections.Generic;

namespace RDCELERP.DAL.Entities
{
    public partial class TblSelfQc
    {
        public int SelfQcid { get; set; }
        public string? ImageName { get; set; }
        public string? RegdNo { get; set; }
        public bool? IsExchange { get; set; }
        public bool? IsAbb { get; set; }
        public bool? IsActive { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string? Modifiedby { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public int? ExchangeOrderId { get; set; }
        public int? RedemptionId { get; set; }
        public int? UserId { get; set; }

        public virtual TblExchangeOrder? ExchangeOrder { get; set; }
        public virtual TblAbbredemption? Redemption { get; set; }
        public virtual TblUser? User { get; set; }
    }
}
