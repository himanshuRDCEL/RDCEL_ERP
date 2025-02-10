using System;
using System.Collections.Generic;

namespace RDCELERP.DAL.Entities
{
    public partial class TblBrandSmartBuy
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public bool? IsActive { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string? BrandLogoUrl { get; set; }
        public int? BusinessUnitId { get; set; }
        public int? ProductCategoryId { get; set; }
        public int? BrandId { get; set; }

        public virtual TblBrand? Brand { get; set; }
        public virtual TblBusinessUnit? BusinessUnit { get; set; }
        public virtual TblProductCategory? ProductCategory { get; set; }
    }
}
