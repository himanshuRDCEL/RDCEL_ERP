using System;
using System.Collections.Generic;

#nullable disable

namespace RDCELERP.DAL.Entities
{
    public partial class TblExchangeOrderStatus1
    {
        public int? Id { get; set; }
        public string StatusCode { get; set; }
        public string StatusDescription { get; set; }
        public string StatusName { get; set; }
        public bool? IsActive { get; set; }
    }
}
