using System;
using System.Collections.Generic;

namespace RDCELERP.DAL.Entities
{
    public partial class TblItem
    {
        public TblItem()
        {
            TblBookingItems = new HashSet<TblBookingItem>();
            TblItemCarts = new HashSet<TblItemCart>();
        }

        public int ItemId { get; set; }
        public int? Sno { get; set; }
        public string? LocationCode { get; set; }
        public string? Itemcode { get; set; }
        public string? Ean { get; set; }
        public string? ItemDesc { get; set; }
        public decimal? Mrp { get; set; }
        public string? Qty { get; set; }
        public string? PendingDispatch { get; set; }
        public string? AvailableStock { get; set; }
        public string? Brand { get; set; }
        public string? SubCat { get; set; }
        public string? Size { get; set; }
        public string? Colour { get; set; }
        public string? Condition { get; set; }
        public string? Gender { get; set; }
        public string? Division { get; set; }
        public string? Section { get; set; }
        public string? Department { get; set; }
        public decimal? CostPrice { get; set; }
        public string? Rsp { get; set; }
        public string? ManageBatchItem { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public string? Uom { get; set; }
        public string? Hsn { get; set; }
        public string? Hsnid { get; set; }
        public string? ImageName { get; set; }
        public bool? IsActive { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public bool? IsHotDeals { get; set; }
        public int? ItemType { get; set; }
        public int? ItemMasterId { get; set; }

        public virtual TblBusinessCustomer? CreatedByNavigation { get; set; }
        public virtual TblItemMaster? ItemMaster { get; set; }
        public virtual TblBusinessType? ItemTypeNavigation { get; set; }
        public virtual ICollection<TblBookingItem> TblBookingItems { get; set; }
        public virtual ICollection<TblItemCart> TblItemCarts { get; set; }
    }
}
