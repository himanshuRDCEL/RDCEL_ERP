using System;
using System.Collections.Generic;

namespace RDCELERP.DAL.Entities
{
    public partial class TblBuconfiguration
    {
        public TblBuconfiguration()
        {
            TblBuconfigurationMappings = new HashSet<TblBuconfigurationMapping>();
        }

        public int ConfigId { get; set; }
        public string? Key { get; set; }
        public string? Description { get; set; }
        public bool? IsActive { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }

        public virtual TblUser? CreatedByNavigation { get; set; }
        public virtual TblUser? ModifiedByNavigation { get; set; }
        public virtual ICollection<TblBuconfigurationMapping> TblBuconfigurationMappings { get; set; }
    }
}
