using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDCELERP.Model.Base;

namespace RDCELERP.Model.CommonModel
{
    public class EVCClearBalanceViewModel: BaseViewModel
    {
        public int evcRegistrationId { get; set; }
        public decimal? walletAmount { get; set; }
        public decimal? clearBalance { get; set; }
        public decimal? InProgresAmount { get; set; }
        public decimal? DeliverdAmount { get; set;}
    }
}
