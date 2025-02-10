using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDCELERP.Model.Dashboards
{
    public class ServicePartnerDasboardViewModel
    {
        public decimal TodayTotalEarning { get; set; }//Pickup done 
        public decimal TodayEstimateEarning { get; set; }
        public decimal PrevoiusEarning { get; set; }
        public decimal TillDateEarning { get; set; }//Drop Done
        
        public int TodayActiveVehicle { get; set; }
        public int TodayActiveOrders { get; set; }

        public int? TotalOrderCount { get; set; }
        public int? TotalPickedUpCount { get; set; }
    }
}
