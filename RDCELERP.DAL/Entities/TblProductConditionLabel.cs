using System;
using System.Collections.Generic;

namespace RDCELERP.DAL.Entities
{
    public partial class TblProductConditionLabel
    {
        public int Id { get; set; }
        public string? PclabelName { get; set; }
        public int? OrderSequence { get; set; }
        public int BusinessUnitId { get; set; }
        public bool? IsActive { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? Modifieddate { get; set; }
        public int? CreatedBy { get; set; }
        public int? ModifiedBy { get; set; }
        public int? BusinessPartnerId { get; set; }
        public bool? IsSweetenerApplicable { get; set; }
        public int? ProductCatId { get; set; }

        public virtual TblBusinessPartner? BusinessPartner { get; set; }
        public virtual TblBusinessUnit BusinessUnit { get; set; } = null!;
        public virtual TblProductCategory? ProductCat { get; set; }
    }
}
