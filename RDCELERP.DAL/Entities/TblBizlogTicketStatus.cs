using System;
using System.Collections.Generic;

namespace RDCELERP.DAL.Entities
{
    public partial class TblBizlogTicketStatus
    {
        public int Id { get; set; }
        public string BizlogTicketNo { get; set; } = null!;
        public string? Status { get; set; }
        public string? Remarks { get; set; }
        public bool? IsActive { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string? AwbNo { get; set; }
    }
}
