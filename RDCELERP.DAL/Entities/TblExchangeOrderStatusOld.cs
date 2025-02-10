using System;
using System.Collections.Generic;

namespace RDCELERP.DAL.Entities
{
    public partial class TblExchangeOrderStatusOld
    {
        public int Id { get; set; }
        public string? StatusCode { get; set; }
        public string? StatusDescription { get; set; }
        public string? StatusName { get; set; }
        public bool? IsActive { get; set; }
    }
}
