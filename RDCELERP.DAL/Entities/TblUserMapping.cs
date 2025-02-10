using System;
using System.Collections.Generic;

namespace RDCELERP.DAL.Entities
{
    public partial class TblUserMapping
    {
        public int UserMappingId { get; set; }
        public int? UserId { get; set; }
        public int? BusinessPartnerId { get; set; }
        public int? BusinessUnitId { get; set; }
        public bool? IsActive { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }

        public virtual TblBusinessPartner? BusinessPartner { get; set; }
        public virtual TblBusinessUnit? BusinessUnit { get; set; }
        public virtual TblUser? CreatedByNavigation { get; set; }
        public virtual TblUser? ModifiedByNavigation { get; set; }
        public virtual TblUser? User { get; set; }
    }
}
