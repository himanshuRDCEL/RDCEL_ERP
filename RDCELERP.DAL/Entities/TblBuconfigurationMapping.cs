using System;
using System.Collections.Generic;

namespace RDCELERP.DAL.Entities
{
    public partial class TblBuconfigurationMapping
    {
        public int Id { get; set; }
        public int? BusinessUnitId { get; set; }
        public int? ConfigId { get; set; }
        public string? Value { get; set; }
        public bool? IsActive { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }

        public virtual TblBusinessUnit? BusinessUnit { get; set; }
        public virtual TblBuconfiguration? Config { get; set; }
        public virtual TblUser? CreatedByNavigation { get; set; }
        public virtual TblUser? ModifiedByNavigation { get; set; }
    }
}
