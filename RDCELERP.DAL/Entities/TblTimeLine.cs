using System;
using System.Collections.Generic;

namespace RDCELERP.DAL.Entities
{
    public partial class TblTimeLine
    {
        public int TimeLineId { get; set; }
        public string? OrderTimeline { get; set; }
        public bool? IsActive { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }

        public virtual TblUser? CreatedByNavigation { get; set; }
    }
}
