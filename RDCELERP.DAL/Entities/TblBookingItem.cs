using System;
using System.Collections.Generic;

namespace RDCELERP.DAL.Entities
{
    public partial class TblBookingItem
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
        public bool? IsActive { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }

        public virtual TblUser? CreatedByNavigation { get; set; }
        public virtual TblBusinessCustomer? Customer { get; set; }
        public virtual TblItem? Item { get; set; }
        public virtual TblUser? ModifiedByNavigation { get; set; }
    }
}
