using System;
using System.Collections.Generic;

namespace RDCELERP.DAL.Entities
{
    public partial class TblItem
    {
        public TblItem()
        {
            TblBookingItems = new HashSet<TblBookingItem>();
        }

        public int ItemId { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Sku { get; set; }
        public string? Brand { get; set; }
        public string? ImageName { get; set; }
        public bool? IsActive { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public decimal? Price { get; set; }
        public bool? IsHotDeals { get; set; }
        public int? ItemType { get; set; }

        public virtual TblUser? CreatedByNavigation { get; set; }
        public virtual TblBusinessType? ItemTypeNavigation { get; set; }
        public virtual TblUser? ModifiedByNavigation { get; set; }
        public virtual ICollection<TblBookingItem> TblBookingItems { get; set; }
    }
}
