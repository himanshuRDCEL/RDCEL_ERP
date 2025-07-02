using System;
using System.Collections.Generic;

namespace RDCELERP.DAL.Entities
{
    public partial class TblBtoBpayment
    {
        public int PaymentId { get; set; }
        public string? RazorpayPaymentId { get; set; }
        public string? RazorpayOrderId { get; set; }
        public string? RazorpaySignature { get; set; }
        public int? BookingItemId { get; set; }
        public decimal? Amount { get; set; }
        public string? PaymentStatus { get; set; }
        public bool? IsActive { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string? Method { get; set; }
        public string? OrderNo { get; set; }

        public virtual TblBookingItem? BookingItem { get; set; }
        public virtual TblBusinessCustomer? CreatedByNavigation { get; set; }
    }
}
