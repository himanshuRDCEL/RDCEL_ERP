using System;
using System.Collections.Generic;

namespace RDCELERP.DAL.Entities
{
    public partial class TblApicall
    {
        public int ApicallId { get; set; }
        public string? Url { get; set; }
        public string? MethodType { get; set; }
        public string? RequestBody { get; set; }
        public string? ResponseBody { get; set; }
        public bool? IsActive { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }

        public virtual TblUser? CreatedByNavigation { get; set; }
        public virtual TblUser? ModifiedByNavigation { get; set; }
    }
}
