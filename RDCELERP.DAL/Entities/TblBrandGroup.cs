using System;
using System.Collections.Generic;

namespace RDCELERP.DAL.Entities
{
    public partial class TblBrandGroup
    {
        public TblBrandGroup()
        {
            TblProdCatBrandMappings = new HashSet<TblProdCatBrandMapping>();
        }

        public int BrandGroupId { get; set; }
        public string? BrandGroupName { get; set; }
        public int ProductCatId { get; set; }
        public decimal? Weightage { get; set; }
        public bool? IsActive { get; set; }
        public int? Createdby { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }

        public virtual TblUser? CreatedbyNavigation { get; set; }
        public virtual TblUser? ModifiedByNavigation { get; set; }
        public virtual TblProductCategory ProductCat { get; set; } = null!;
        public virtual ICollection<TblProdCatBrandMapping> TblProdCatBrandMappings { get; set; }
    }
}
