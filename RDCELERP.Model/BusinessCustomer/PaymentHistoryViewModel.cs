using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDCELERP.Model.BusinessCustomer
{
    public class PaymentHistoryViewModel
    {
        public string OrderNo { get; set; }
        public decimal? Amount { get; set; }
        public string PaymentStatus { get; set; }
        public string PaymentMode { get; set; }
        public string TransactionId { get; set; }
        public DateTime PaymentDate { get; set; }
        public string PaymentDateFormatted => PaymentDate.ToString("dd-MMM-yyyy hh:mm tt");
    }
}
