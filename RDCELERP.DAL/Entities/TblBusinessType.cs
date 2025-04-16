using System;
using System.Collections.Generic;

namespace RDCELERP.DAL.Entities
{
    public partial class TblBusinessType
    {
        public TblBusinessType()
        {
            TblBusinessTypeMappings = new HashSet<TblBusinessTypeMapping>();
            TblItems = new HashSet<TblItem>();
        }

        public int BusinessTypeId { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public bool? IsActive { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }

        public virtual ICollection<TblBusinessTypeMapping> TblBusinessTypeMappings { get; set; }
        public virtual ICollection<TblItem> TblItems { get; set; }
    }
}
