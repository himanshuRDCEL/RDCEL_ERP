using System;
using System.Collections.Generic;

namespace RDCELERP.DAL.Entities
{
    public partial class TblErrorLog
    {
        public int ErrorLogId { get; set; }
        public string? ClassName { get; set; }
        public string? MethodName { get; set; }
        public string? SponsorOrderNo { get; set; }
        public string? ErrorMessage { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
}
