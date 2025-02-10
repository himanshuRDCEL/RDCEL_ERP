using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDCELERP.Model.MobileApplicationModel.LGC
{
    public class LogisticOrderDetailsDataModel
    {
        public string? Regdno { get; set; }
        public string? OrderId { get; set; }

    }

    public class orderRegdnolist
    {
        public string? Regdno { get; set; }
        public int driverID { get; set; }
        public string? TicketNumber { get; set; }
        public decimal? AmtPaybleThroughLgc { get; set; }

    }
}
