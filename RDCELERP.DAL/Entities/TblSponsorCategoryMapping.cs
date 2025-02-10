using System;
using System.Collections.Generic;

namespace RDCELERP.DAL.Entities
{
    public partial class TblSponsorCategoryMapping
    {
        public int Id { get; set; }
        public int? CategoryId { get; set; }
        public int? BucategoryId { get; set; }
        public int? BusinessUnitId { get; set; }
        public string? BucategoryName { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public int? CreatedBy { get; set; }
        public int? ModifiedBy { get; set; }
        public bool? IsActive { get; set; }

        public virtual TblBusinessUnit? BusinessUnit { get; set; }
        public virtual TblProductCategory? Category { get; set; }
    }
}
