using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDCELERP.Model.Base;

namespace RDCELERP.Model.LGC
{
    public class ServicePartnerDashboardViewModel:BaseViewModel
    {
        public int? ServicePartnerId { get; set; }
        public int ReadyForPickup { get; set; }
        public int PickupDone { get; set; }
        public int LoadDone { get; set; }
        public int PickupDecline { get; set; }
        public int Drop { get; set; }
       
    }
}
