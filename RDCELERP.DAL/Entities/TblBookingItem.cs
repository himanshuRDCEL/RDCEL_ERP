using System;
using System.Collections.Generic;

namespace RDCELERP.DAL.Entities
{
    public partial class TblBookingItem
    {
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

        public virtual TblUser? CreatedByNavigation { get; set; }
        public virtual TblBusinessCustomer? Customer { get; set; }
        public virtual TblItem? Item { get; set; }
        public virtual TblUser? ModifiedByNavigation { get; set; }
    }
}
