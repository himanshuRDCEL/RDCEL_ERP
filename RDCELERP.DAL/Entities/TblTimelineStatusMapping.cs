using System;
using System.Collections.Generic;

namespace RDCELERP.DAL.Entities
{
    public partial class TblTimelineStatusMapping
    {
        public int TimelineStatusMappingId { get; set; }
        public int OrderTimeLineId { get; set; }
        public int StatusId { get; set; }
        public string? StatusCode { get; set; }
        public bool? IsActive { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }

        public virtual TblUser? CreatedByNavigation { get; set; }
        public virtual TblExchangeOrderStatus Status { get; set; } = null!;
    }
}
