using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDCELERP.Model.Base;

namespace RDCELERP.Model.EVC
{
    public class EVCAllocationViewModel : BaseViewModel
    {
        public int WalletTransactionId { get; set; }
        public int? EvcregistrationId { get; set; }
        public string? RegdNo { get; set; }
        public string? SponserOrderNo { get; set; }
        public string? StatusId { get; set; }
        public int? ExchangeOrderId { get; set; }
        public int? AbbregistrationId { get; set; }
        public long? OrderAmount { get; set; }
        public DateTime? OrderOfDeliverdDate { get; set; }
        public DateTime? OrderOfCompleteDate { get; set; }
        public DateTime? OrderOfInprogressDate { get; set; }
        public string? OrderType { get; set; }
    }
}
