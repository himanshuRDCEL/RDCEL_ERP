using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDCELERP.Model.BusinessCustomer
{
    public class BookingItemViewModel
    {
        public int BookingItemId { get; set; }
        public int? CustomerId { get; set; }
        public int? ItemId { get; set; }
        public int? Quantity { get; set; }
        public bool? Qccheck { get; set; }
        public bool? QcverifiedQunatity { get; set; }
        public decimal? BookingPrice { get; set; }
        public decimal? TotalPrice { get; set; }
        public bool? IsBookingPaymentConfirm { get; set; }
    }
}
