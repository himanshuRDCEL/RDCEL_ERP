using System;
using System.Collections.Generic;

namespace RDCELERP.DAL.Entities
{
    public partial class TblItemCart
    {
        public int ItemCartId { get; set; }
        public int? ItemMasterId { get; set; }
        public int? ItemId { get; set; }
        public string? Itemcode { get; set; }
        public string? Ean { get; set; }
        public string? ItemDesc { get; set; }
        public decimal? Mrp { get; set; }
        public int? PurchaseQty { get; set; }
        public decimal? B2bPrice { get; set; }
        public decimal? SubTotalPrice { get; set; }
        public decimal? TotalPrice { get; set; }
        public bool? IsActive { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public bool? IsPaymentComplete { get; set; }

        public virtual TblBusinessCustomer? CreatedByNavigation { get; set; }
        public virtual TblItem? Item { get; set; }
        public virtual TblItemMaster? ItemMaster { get; set; }
    }
}
