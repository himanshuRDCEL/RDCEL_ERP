using System;
using System.Collections.Generic;

namespace RDCELERP.DAL.Entities
{
    public partial class TblProdCatBrandMapping
    {
        public int ProdCatBrandMappingId { get; set; }
        public int BrandId { get; set; }
        public int ProductCatId { get; set; }
        public int BrandGroupId { get; set; }
        public bool? IsActive { get; set; }
        public int? Createdby { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }

        public virtual TblBrand Brand { get; set; } = null!;
        public virtual TblBrandGroup BrandGroup { get; set; } = null!;
        public virtual TblUser? CreatedbyNavigation { get; set; }
        public virtual TblUser? ModifiedByNavigation { get; set; }
        public virtual TblProductCategory ProductCat { get; set; } = null!;
    }
}
