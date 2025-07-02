using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDCELERP.Model.BusinessCustomer
{
    public class OrderHistoryViewModel
    {
        public string OrderId { get; set; }
        public DateTime? BookingDate { get; set; }
        public List<ItemDetail> Items { get; set; }
        public PaymentDetail Payment { get; set; }

        public string Status { get; set; }
    }

    public class ItemDetail
    {
        public string ItemName { get; set; }
        public int? Quantity { get; set; }
        public decimal? BillingAmount { get; set; }
        public string BookingStatusSummary { get; set; }  
    }
    public class PaymentDetail
    {
        public decimal? Amount { get; set; }
        public string PaymentStatus { get; set; }
        public string PaymentMode { get; set; }
    }

}
