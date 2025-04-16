using System;
using System.Collections.Generic;

namespace RDCELERP.DAL.Entities
{
    public partial class TblBusinessTypeMapping
    {
        public int BusinessTypeMappingId { get; set; }
        public int? BussinessCustomerId { get; set; }
        public int? BusinessTypeId { get; set; }
        public bool? IsActive { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }

        public virtual TblBusinessType? BusinessType { get; set; }
        public virtual TblUser? CreatedByNavigation { get; set; }
        public virtual TblUser? ModifiedByNavigation { get; set; }
    }
}
