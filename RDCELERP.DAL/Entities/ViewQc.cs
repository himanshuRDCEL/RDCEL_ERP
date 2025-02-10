using System;
using System.Collections.Generic;

#nullable disable

namespace RDCELERP.DAL.Entities
{
    public partial class ViewQc
    {
        public string RegdNo { get; set; }
        public string CompanyName { get; set; }
        public string StatusCode { get; set; }
        public string StatusDescription { get; set; }
        public string QcDoneBy { get; set; }
        public string CustomerName { get; set; }
        public DateTime? OrderDate { get; set; }
        public DateTime? QcDate { get; set; }
        public DateTime? SelfQcDate { get; set; }
        public string Qccomments { get; set; }
    }
}
