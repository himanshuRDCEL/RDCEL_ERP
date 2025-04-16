using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDCELERP.Model.BusinessCustomer
{
    public class DashboardViewModel
    {
        public string CustomerName { get; set; }
        public int? BusinessCustomerId { get; set; }
        public int? TotalItemCount { get; set; }
        public int? HotDealsCount { get; set; }
        public int? BookingOrderCount { get; set; }

    }
}
