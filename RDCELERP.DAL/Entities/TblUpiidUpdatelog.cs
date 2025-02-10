using System;
using System.Collections.Generic;

namespace RDCELERP.DAL.Entities
{
    public partial class TblUpiidUpdatelog
    {
        public int Id { get; set; }
        public string? Upiid { get; set; }
        public string? OldUpiid { get; set; }
        public string? RegdNo { get; set; }
        public string? PayLoad { get; set; }
        public bool? IsActive { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }
}
