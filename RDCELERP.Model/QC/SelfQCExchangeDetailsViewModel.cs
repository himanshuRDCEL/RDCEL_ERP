using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDCELERP.Model.QC
{
    public class SelfQCExchangeDetailsViewModel
    {
        public string? CustomerName { get; set; }
        public string? ProductCategory { get; set; }
        public string? RegdNo { get; set; }
        public int? OrderTransId { get; set; }
        public int? StatusId { get; set; }
    }
}
