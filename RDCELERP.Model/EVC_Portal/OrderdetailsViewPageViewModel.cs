using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDCELERP.Model.EVC_Portal
{
    public class OrderdetailsViewPageViewModel
    {
        public int EvcregistrationId { get; set; }
        public int OrderTransId { get; set; }
        public string? RegdNo { get; set; }
        public string? ExchProdGroup { get; set; }
        public string? OldProdType { get; set; }
        public string? EVCRate { get; set; }
        public string? ActualProdQltyAtQc { get; set; }     
        

    }
}
