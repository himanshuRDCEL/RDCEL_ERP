using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDCELERP.Model.Base;

namespace RDCELERP.Model.OrderTrans
{
    public class OrderTransViewModel : BaseViewModel
    {
        [Key]
        public int OrderTransId { get; set; }
        public int OrderType { get; set; }
        public int ExchangeId { get; set; }
        public int ABBRedemptionId { get; set; }
        public string? SponsorOrderNumber { get; set; }
        public string? RegdNo { get; set; }
        public decimal ExchangePrice { get; set; }
        public decimal QuotedPrice { get; set; }
    }
}
