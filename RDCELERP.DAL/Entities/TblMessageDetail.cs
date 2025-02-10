using System;
using System.Collections.Generic;

namespace RDCELERP.DAL.Entities
{
    public partial class TblMessageDetail
    {
        public int MessageDetailId { get; set; }
        public string? Code { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Message { get; set; }
        public string? ResponseJson { get; set; }
        public DateTime? SendDate { get; set; }
        public byte? MessageType { get; set; }
        public string? Email { get; set; }
        public bool? IsUsed { get; set; }
    }
}
