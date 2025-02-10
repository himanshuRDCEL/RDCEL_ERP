using System;
using System.Collections.Generic;

namespace RDCELERP.DAL.Entities
{
    public partial class TblAreaLocality
    {
        public int AreaId { get; set; }
        public int? Pincode { get; set; }
        public string? AreaLocality { get; set; }
        public string? BranchCode { get; set; }
        public string? District { get; set; }
        public string? Taluk { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public int? ModifiedBy { get; set; }
    }
}
