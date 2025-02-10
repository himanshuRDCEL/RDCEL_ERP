using System;
using System.Collections.Generic;

namespace RDCELERP.DAL.Entities
{
    public partial class TblModelNumber
    {
        public TblModelNumber()
        {
            TblAbbregistrations = new HashSet<TblAbbregistration>();
            TblModelMappings = new HashSet<TblModelMapping>();
            TblVoucherVerfications = new HashSet<TblVoucherVerfication>();
        }

        public int ModelNumberId { get; set; }
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
        public decimal? SweetenerDigi2l { get; set; }
        public int? BusinessPartnerId { get; set; }
        public bool? IsAbb { get; set; }
        public bool? IsExchange { get; set; }

        public virtual TblBrand? Brand { get; set; }
        public virtual TblBusinessPartner? BusinessPartner { get; set; }
        public virtual TblBusinessUnit? BusinessUnit { get; set; }
        public virtual TblProductCategory? ProductCategory { get; set; }
        public virtual TblProductType? ProductType { get; set; }
        public virtual ICollection<TblAbbregistration> TblAbbregistrations { get; set; }
        public virtual ICollection<TblModelMapping> TblModelMappings { get; set; }
        public virtual ICollection<TblVoucherVerfication> TblVoucherVerfications { get; set; }
    }
}
