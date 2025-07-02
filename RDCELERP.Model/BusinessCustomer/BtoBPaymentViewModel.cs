using RDCELERP.Model.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDCELERP.Model.BusinessCustomer
{
    public class BtoBPaymentViewModel :BaseViewModel
    {
        public int PaymentId { get; set; }
        public string? RazorpayPaymentId { get; set; }
        public string? RazorpayOrderId { get; set; }
        public string? RazorpaySignature { get; set; }
        public int? BookingItemId { get; set; }
        public decimal? Amount { get; set; }
        public string? PaymentStatus { get; set; }
     
        public string? Method { get; set; }
        public string? OrderNo { get; set; }
    }
}
