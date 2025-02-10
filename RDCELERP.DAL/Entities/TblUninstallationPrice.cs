using System;
using System.Collections.Generic;

namespace RDCELERP.DAL.Entities
{
    public partial class TblUninstallationPrice
    {
        public int Id { get; set; }
        public decimal? UnInstallationPrice { get; set; }
        public int? ProductCatId { get; set; }
        public int? ProductTypeId { get; set; }
        public int? BusinessUnitId { get; set; }
        public int? BusinessPartnerId { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public int? CreatedBy { get; set; }
        public int? ModifiedBy { get; set; }
        public bool? IsActive { get; set; }

        public virtual TblBusinessPartner? BusinessPartner { get; set; }
        public virtual TblBusinessUnit? BusinessUnit { get; set; }
        public virtual TblProductCategory? ProductCat { get; set; }
        public virtual TblProductType? ProductType { get; set; }
    }
}
