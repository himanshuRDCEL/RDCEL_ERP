using System;
using System.Collections.Generic;

namespace RDCELERP.DAL.Entities
{
    public partial class TblImage
    {
        public int Id { get; set; }
        public int? BizlogTicketId { get; set; }
        public int? SponsorId { get; set; }
        public string? ImageUrl { get; set; }
        public bool? IsActive { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }

        public virtual TblBizlogTicket? BizlogTicket { get; set; }
        public virtual TblExchangeOrder? Sponsor { get; set; }
    }
}
