using System;
using System.Collections.Generic;

namespace RDCELERP.DAL.Entities
{
    public partial class TblBrand
    {
        public TblBrand()
        {
            TblBrandSmartBuys = new HashSet<TblBrandSmartBuy>();
            TblExchangeOrders = new HashSet<TblExchangeOrder>();
            TblModelMappings = new HashSet<TblModelMapping>();
            TblModelNumbers = new HashSet<TblModelNumber>();
            TblOrderBasedConfigs = new HashSet<TblOrderBasedConfig>();
            TblPriceMasterMappings = new HashSet<TblPriceMasterMapping>();
            TblProdCatBrandMappings = new HashSet<TblProdCatBrandMapping>();
            TblVoucherVerfications = new HashSet<TblVoucherVerfication>();
        }

        public int Id { get; set; }
        public string? Name { get; set; }
        public bool? IsActive { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string? BrandLogoUrl { get; set; }
        public int? BusinessUnitId { get; set; }

        public virtual TblBusinessUnit? BusinessUnit { get; set; }
        public virtual ICollection<TblBrandSmartBuy> TblBrandSmartBuys { get; set; }
        public virtual ICollection<TblExchangeOrder> TblExchangeOrders { get; set; }
        public virtual ICollection<TblModelMapping> TblModelMappings { get; set; }
        public virtual ICollection<TblModelNumber> TblModelNumbers { get; set; }
        public virtual ICollection<TblOrderBasedConfig> TblOrderBasedConfigs { get; set; }
        public virtual ICollection<TblPriceMasterMapping> TblPriceMasterMappings { get; set; }
        public virtual ICollection<TblProdCatBrandMapping> TblProdCatBrandMappings { get; set; }
        public virtual ICollection<TblVoucherVerfication> TblVoucherVerfications { get; set; }
    }
}
