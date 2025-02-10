using System;
using System.Collections.Generic;

namespace RDCELERP.DAL.Entities
{
    public partial class TblBuproductCategoryMapping
    {
        public int Id { get; set; }
        public int? ProductCatId { get; set; }
        public int? BusinessUnitId { get; set; }
        public bool? IsActive { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public int? CreatedBy { get; set; }
        public int? ModifiedBy { get; set; }
        public bool? IsAbb { get; set; }
        public bool? IsExchange { get; set; }
        public int? ProductTypeId { get; set; }
        public int? OldProductCatId { get; set; }

        public virtual TblBusinessUnit? BusinessUnit { get; set; }
        public virtual TblUser? CreatedByNavigation { get; set; }
        public virtual TblUser? ModifiedByNavigation { get; set; }
        public virtual TblProductCategory? OldProductCat { get; set; }
        public virtual TblProductCategory? ProductCat { get; set; }
        public virtual TblProductType? ProductType { get; set; }
    }
}
