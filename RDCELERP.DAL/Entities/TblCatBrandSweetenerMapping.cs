using System;
using System.Collections.Generic;

namespace RDCELERP.DAL.Entities
{
    public partial class TblCatBrandSweetenerMapping
    {
        public int Id { get; set; }
        public int CatBrandSweetenerId { get; set; }
        public int? BusinessUnitId { get; set; }
        public int? BusinessPartnerId { get; set; }
        public int? BrandId { get; set; }
        public decimal? SweetenerBu { get; set; }
        public decimal? SweetenerBp { get; set; }
        public decimal? SweetenerOwn { get; set; }
        public bool? IsActive { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public bool? IsDefault { get; set; }

        public virtual TblBrand? Brand { get; set; }
        public virtual TblBusinessPartner? BusinessPartner { get; set; }
        public virtual TblBusinessUnit? BusinessUnit { get; set; }
        public virtual TblCatBrandSweetener CatBrandSweetener { get; set; } = null!;
    }
}
