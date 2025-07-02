using System;
using System.Collections.Generic;

namespace RDCELERP.DAL.Entities
{
    public partial class TblBookingItem
    {
        public TblBookingItem()
        {
            TblBtoBpayments = new HashSet<TblBtoBpayment>();
        }

        public int BookingItemId { get; set; }
        public int? CustomerId { get; set; }
        public int? ItemId { get; set; }
        public string? OrderNo { get; set; }
        public DateTime? DeliveryDate { get; set; }
        public string? ItemCode { get; set; }
        public string? EanNo { get; set; }
        public decimal? ItemMrp { get; set; }
        public decimal? BillingAmount { get; set; }
        public int? Quantity { get; set; }
        public string? Consignee { get; set; }
        public string? ConsigneeName { get; set; }
        public string? ProjectCode { get; set; }
        public string? WhCode { get; set; }
        public bool? Qccheck { get; set; }
        public bool? QcverifiedQunatity { get; set; }
        public decimal? BookingPrice { get; set; }
        public decimal? TotalPrice { get; set; }
        public bool? IsBookingPaymentConfirm { get; set; }
        public string? ItemStatus { get; set; }
        public bool? IsActive { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public bool? IsAdvcPayment { get; set; }
        public int? UnderPicking { get; set; }
        public int? PickedQty { get; set; }
        public int? DispatchedQty { get; set; }
        public int? AdjustedQty { get; set; }
        public int? PendingQty { get; set; }
        public string? RazorOrderId { get; set; }
        public string? SyncOrderNo { get; set; }
        public bool? IsPaymentDone { get; set; }
        public DateTime? PaymentDate { get; set; }
        public string? BookingStatus { get; set; }
        public decimal? B2bPrice { get; set; }

        public virtual TblBusinessCustomer? CreatedByNavigation { get; set; }
        public virtual TblBusinessCustomer? Customer { get; set; }
        public virtual TblItem? Item { get; set; }
        public virtual TblBusinessCustomer? ModifiedByNavigation { get; set; }
        public virtual ICollection<TblBtoBpayment> TblBtoBpayments { get; set; }
    }
}
