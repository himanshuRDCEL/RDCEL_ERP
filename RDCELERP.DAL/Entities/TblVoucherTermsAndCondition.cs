using System;
using System.Collections.Generic;

namespace RDCELERP.DAL.Entities
{
    public partial class TblVoucherTermsAndCondition
    {
        public int Id { get; set; }
        public string? TermsandCondition { get; set; }
        public int? BusinessUnitId { get; set; }
        public bool? IsActive { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public int? ModifiedBy { get; set; }
        public bool? IsDeffered { get; set; }

        public virtual TblBusinessUnit? BusinessUnit { get; set; }
    }
}
