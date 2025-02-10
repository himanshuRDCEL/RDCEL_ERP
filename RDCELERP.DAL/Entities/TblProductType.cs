using System;
using System.Collections.Generic;

namespace RDCELERP.DAL.Entities
{
    public partial class TblProductType
    {
        public TblProductType()
        {
            TblAbbplanMasters = new HashSet<TblAbbplanMaster>();
            TblAbbpriceMasters = new HashSet<TblAbbpriceMaster>();
            TblAbbregistrations = new HashSet<TblAbbregistration>();
            TblBuproductCategoryMappings = new HashSet<TblBuproductCategoryMapping>();
            TblExchangeOrderNewProductTypes = new HashSet<TblExchangeOrder>();
            TblExchangeOrderProductTypes = new HashSet<TblExchangeOrder>();
            TblImageLabelMasters = new HashSet<TblImageLabelMaster>();
            TblModelNumbers = new HashSet<TblModelNumber>();
            TblPriceMasterQuestioners = new HashSet<TblPriceMasterQuestioner>();
            TblPriceMasters = new HashSet<TblPriceMaster>();
            TblUninstallationPrices = new HashSet<TblUninstallationPrice>();
            TblUniversalPriceMasters = new HashSet<TblUniversalPriceMaster>();
            TblVehicleIncentives = new HashSet<TblVehicleIncentive>();
            TblVoucherVerfications = new HashSet<TblVoucherVerfication>();
            UniversalPriceMasters = new HashSet<UniversalPriceMaster>();
        }

        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Code { get; set; }
        public string? Size { get; set; }
        public int? ProductCatId { get; set; }
        public bool? IsActive { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string? DescriptionForAbb { get; set; }
        public bool? IsAllowedForOld { get; set; }
        public bool? IsAllowedForNew { get; set; }
        public string? ProductTypeImage { get; set; }
        public bool? IsusedNew { get; set; }
        public bool? Isusedold { get; set; }

        public virtual TblProductCategory? ProductCat { get; set; }
        public virtual ICollection<TblAbbplanMaster> TblAbbplanMasters { get; set; }
        public virtual ICollection<TblAbbpriceMaster> TblAbbpriceMasters { get; set; }
        public virtual ICollection<TblAbbregistration> TblAbbregistrations { get; set; }
        public virtual ICollection<TblBuproductCategoryMapping> TblBuproductCategoryMappings { get; set; }
        public virtual ICollection<TblExchangeOrder> TblExchangeOrderNewProductTypes { get; set; }
        public virtual ICollection<TblExchangeOrder> TblExchangeOrderProductTypes { get; set; }
        public virtual ICollection<TblImageLabelMaster> TblImageLabelMasters { get; set; }
        public virtual ICollection<TblModelNumber> TblModelNumbers { get; set; }
        public virtual ICollection<TblPriceMasterQuestioner> TblPriceMasterQuestioners { get; set; }
        public virtual ICollection<TblPriceMaster> TblPriceMasters { get; set; }
        public virtual ICollection<TblUninstallationPrice> TblUninstallationPrices { get; set; }
        public virtual ICollection<TblUniversalPriceMaster> TblUniversalPriceMasters { get; set; }
        public virtual ICollection<TblVehicleIncentive> TblVehicleIncentives { get; set; }
        public virtual ICollection<TblVoucherVerfication> TblVoucherVerfications { get; set; }
        public virtual ICollection<UniversalPriceMaster> UniversalPriceMasters { get; set; }
    }
}
