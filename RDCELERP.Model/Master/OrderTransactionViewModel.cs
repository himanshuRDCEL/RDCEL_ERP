using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDCELERP.Model.Base;

namespace RDCELERP.Model.Master
{
    public  class OrderTransactionViewModel : BaseViewModel
    {
        public int OrderTransId { get; set; }
        public int OrderType { get; set; }
        public int? ExchangeId { get; set; }
        public int? StatusId { get; set; }
        public int? AbbredemptionId { get; set; }
        public string? SponsorOrderNumber { get; set; }
        public string? RegdNo { get; set; }
        public decimal? ExchangePrice { get; set; }
        public decimal? QuotedPrice { get; set; }
        public decimal? Sweetner { get; set; }
        public decimal? FinalPriceAfterQc { get; set; }
        public int? EvcpriceMasterId { get; set; }
        public decimal? Evcprice { get; set; }
        public bool? AmountPaidToCustomer { get; set; }

    }
}
