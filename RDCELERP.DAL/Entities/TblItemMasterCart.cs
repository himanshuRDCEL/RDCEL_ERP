using System;
using System.Collections.Generic;

namespace RDCELERP.DAL.Entities
{
    public partial class TblItemMasterCart
    {
        public int ItemMasterCartId { get; set; }
        public int? ItemMasterId { get; set; }
        public string? ItemMastercode { get; set; }
        public string? Ean { get; set; }
        public string? ItemMasterDesc { get; set; }
        public decimal? Mrp { get; set; }
        public string? PurchaseQty { get; set; }
        public decimal? B2bPrice { get; set; }
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
        public string? ImageName { get; set; }
        public bool? IsActive { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }

        public virtual TblUser? CreatedByNavigation { get; set; }
        public virtual TblItemMaster? ItemMaster { get; set; }
    }
}
