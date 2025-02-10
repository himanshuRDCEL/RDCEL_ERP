using System;
using System.Collections.Generic;

namespace RDCELERP.DAL.Entities
{
    public partial class TblWhatsAppMessage
    {
        public int Id { get; set; }
        public string? PhoneNumber { get; set; }
        public string? TemplateName { get; set; }
        public bool? IsActive { get; set; }
        public DateTime? SendDate { get; set; }
        public string? MsgId { get; set; }
        public string? Code { get; set; }
    }
}
