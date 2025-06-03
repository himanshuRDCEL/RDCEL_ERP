using System;
using System.Collections.Generic;

namespace RDCELERP.DAL.Entities
{
    public partial class TblCatBrandSweetener
    {
        public TblCatBrandSweetener()
        {
            TblCatBrandSweetenerMappings = new HashSet<TblCatBrandSweetenerMapping>();
        }

        public int CatBrandSweetenerId { get; set; }
        public string? ModelName { get; set; }
        public string? Description { get; set; }
        public string? Code { get; set; }
        public int? BrandId { get; set; }
        public int? ProductCategoryId { get; set; }
        public int? ProductTypeId { get; set; }
        public bool? IsActive { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public decimal? SweetnerForDtd { get; set; }
        public decimal? SweetnerForDtc { get; set; }
        public bool? IsDefaultProduct { get; set; }
        public int? BusinessUnitId { get; set; }
        public decimal? SweetenerBu { get; set; }
        public decimal? SweetenerBp { get; set; }
        public decimal? SweetenerOwn { get; set; }
        public int? BusinessPartnerId { get; set; }
        public bool? IsExchange { get; set; }

        public virtual TblBrand? Brand { get; set; }
        public virtual TblBusinessPartner? BusinessPartner { get; set; }
        public virtual TblBusinessUnit? BusinessUnit { get; set; }
        public virtual TblProductCategory? ProductCategory { get; set; }
        public virtual TblProductType? ProductType { get; set; }
        public virtual ICollection<TblCatBrandSweetenerMapping> TblCatBrandSweetenerMappings { get; set; }
    }
}
