using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDCELERP.Model.TechGuard
{
   public class TechGuardRequestModel
    {
        public string? repair_id { get; set; }
        public int amount { get; set; }
        public string? company_id { get; set; }
        public string? secret_key { get; set; }
    }

    public class TechGuardResponse
    {
        public bool status { get; set; }
        public string? referance_id { get; set; }
        public string? payment_status { get; set; }
        public string? remark { get; set; }
    }

    public class ProcessPaymentTechGuard
    {
        public string? CustomerName { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Address { get; set; }
        public string? Email { get; set; }
        public string? FinalPrice { get; set; }
        public string? RepairId { get; set; }
        public string? RegdNo { get; set; }
        public int RedemptoinId { get; set; }
    }
}
