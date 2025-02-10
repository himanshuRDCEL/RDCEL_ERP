using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDCELERP.Model.Base;

namespace RDCELERP.Model.ExchangeOrder
{
    public class ExchangeAbbstatusHistoryViewModel : BaseViewModel
    {
        public int StatusHistoryId { get; set; }
        public int OrderType { get; set; }
        public string? SponsorOrderNumber { get; set; }
        public string? RegdNo { get; set; }
        public string? ZohoSponsorId { get; set; }
        public int? CustId { get; set; }
        public int? StatusId { get; set; }
        public int? OrderTransId { get; set; }
    }
}
