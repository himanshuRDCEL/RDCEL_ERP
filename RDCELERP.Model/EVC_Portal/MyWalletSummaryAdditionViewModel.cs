using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDCELERP.Model.Base;

namespace RDCELERP.Model.EVC_Portal
{
    public class MyWalletSummaryAdditionViewModel : BaseViewModel
    {
        public int evcRegistrationId { get; set; }
        public decimal Amount { get; set; }
        public string? AddedBy { get; set; }
        //public DateTime? CreatedDate {get;set;}
        public string? FinalDate { get; set; }
        public string? type { get; set; }

    }
}
