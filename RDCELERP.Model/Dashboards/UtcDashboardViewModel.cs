using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDCELERP.Model.Dashboards
{
    public class UtcDashboardViewModel
    {
        public string? ABBOrders { get; set; }
        public string? ExchangeOrders { get; set; }
        public string? ABBRedemptions { get; set; }
        public string? EVCDisputes { get; set; }
        public string? PendingABBforApprovals { get; set; }
        public string? EVCAllocations { get; set; }
        public string? QCDone { get; set; }
        public string? PickUpDone { get; set; }
        public string? DropDone { get; set; }
        public string? ExchangeInProgress { get; set; }
        public string? AbbInProgress { get; set; }
    }
}

