using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDCELERP.Model.Dashboards
{
    public class ServicePartnerDashboardList
    {
        public List<ServicePartnerDashboardListViewModel>? AllSPOrderlistViewModel { get; set; }
    }
    public class ServicePartnerDashboardListViewModel
    {
        public int? OrderTrans { get; set; }
        public string? RegdNo { get; set; }
        public string? EVCName { get; set; }
        public decimal? EstimateEarning { get; set; }
        public DateTime? JourneyPlanDate { get; set; }
        public DateTime? PickupStartDatetime { get; set; }
        public DateTime? PickupEndDatetime { get; set; }
        public DateTime? OrderDropTime { get; set; }
        public TimeSpan? PickupTatinHr { get; set; }
        public TimeSpan? DropTatinHr { get; set; }
        public decimal? BasePrice { get; set; }
        public decimal? PickupIncAmount { get; set; }
        public decimal? PackagingIncentive { get; set; }
        public decimal? DropIncAmount { get; set; }      
        public decimal? DropImageInc { get; set; }
        public decimal? Total { get; set; }
        public string? EvcStoreCode { get; set; }
    }
}
