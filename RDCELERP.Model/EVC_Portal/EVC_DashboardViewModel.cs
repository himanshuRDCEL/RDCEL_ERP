using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDCELERP.Model.Base;

namespace RDCELERP.Model.EVC_Portal
{
   public class EVC_DashboardViewModel : BaseViewModel
    {
        public int UserId { get; set; }

        public int evcRegistrationId { get; set; }
        public long? walletAmount { get; set; }
        public decimal? clearBalance { get; set; }
        public decimal? Allocation { get; set; }
        public int totalAssignOrder { get; set; }
        public int totalAllocateOrder { get; set; }
        public int totalDeliveredOrder { get; set; }
        public int totalCompleteOrder { get; set; }      
        public int evcAllocation { get; set; }
        public decimal? myApproved { get; set; }
    }
}
