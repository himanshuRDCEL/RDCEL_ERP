using System;
using System.Collections.Generic;

namespace RDCELERP.DAL.Entities
{
    public partial class TblProductCategory
    {
        public TblProductCategory()
        {
            TblAbbplanMasters = new HashSet<TblAbbplanMaster>();
            TblAbbpriceMasters = new HashSet<TblAbbpriceMaster>();
            TblAbbregistrations = new HashSet<TblAbbregistration>();
            TblBrandGroups = new HashSet<TblBrandGroup>();
            TblBrandSmartBuys = new HashSet<TblBrandSmartBuy>();
            TblBuproductCategoryMappingOldProductCats = new HashSet<TblBuproductCategoryMapping>();
            TblBuproductCategoryMappingProductCats = new HashSet<TblBuproductCategoryMapping>();
            TblEvcPartnerPreferences = new HashSet<TblEvcPartnerPreference>();
            TblImageLabelMasters = new HashSet<TblImageLabelMaster>();
            TblModelNumbers = new HashSet<TblModelNumber>();
            TblOrderQcratings = new HashSet<TblOrderQcrating>();
            TblPriceMasterQuestioners = new HashSet<TblPriceMasterQuestioner>();
            TblPriceMasters = new HashSet<TblPriceMaster>();
            TblProdCatBrandMappings = new HashSet<TblProdCatBrandMapping>();
            TblProductConditionLabels = new HashSet<TblProductConditionLabel>();
            TblProductQualityIndices = new HashSet<TblProductQualityIndex>();
            TblProductTechnologies = new HashSet<TblProductTechnology>();
            TblProductTypes = new HashSet<TblProductType>();
            TblQuestionerLovmappings = new HashSet<TblQuestionerLovmapping>();
            TblSponsorCategoryMappings = new HashSet<TblSponsorCategoryMapping>();
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
        public bool? IsActive { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string? DescriptionForAbb { get; set; }
        public bool? IsAllowedForOld { get; set; }
        public bool? IsAllowedForNew { get; set; }
        public string? CommentForWorking { get; set; }
        public string? ProductCategoryImage { get; set; }
        public decimal? Asppercentage { get; set; }

        public virtual ICollection<TblAbbplanMaster> TblAbbplanMasters { get; set; }
        public virtual ICollection<TblAbbpriceMaster> TblAbbpriceMasters { get; set; }
        public virtual ICollection<TblAbbregistration> TblAbbregistrations { get; set; }
        public virtual ICollection<TblBrandGroup> TblBrandGroups { get; set; }
        public virtual ICollection<TblBrandSmartBuy> TblBrandSmartBuys { get; set; }
        public virtual ICollection<TblBuproductCategoryMapping> TblBuproductCategoryMappingOldProductCats { get; set; }
        public virtual ICollection<TblBuproductCategoryMapping> TblBuproductCategoryMappingProductCats { get; set; }
        public virtual ICollection<TblEvcPartnerPreference> TblEvcPartnerPreferences { get; set; }
        public virtual ICollection<TblImageLabelMaster> TblImageLabelMasters { get; set; }
        public virtual ICollection<TblModelNumber> TblModelNumbers { get; set; }
        public virtual ICollection<TblOrderQcrating> TblOrderQcratings { get; set; }
        public virtual ICollection<TblPriceMasterQuestioner> TblPriceMasterQuestioners { get; set; }
        public virtual ICollection<TblPriceMaster> TblPriceMasters { get; set; }
        public virtual ICollection<TblProdCatBrandMapping> TblProdCatBrandMappings { get; set; }
        public virtual ICollection<TblProductConditionLabel> TblProductConditionLabels { get; set; }
        public virtual ICollection<TblProductQualityIndex> TblProductQualityIndices { get; set; }
        public virtual ICollection<TblProductTechnology> TblProductTechnologies { get; set; }
        public virtual ICollection<TblProductType> TblProductTypes { get; set; }
        public virtual ICollection<TblQuestionerLovmapping> TblQuestionerLovmappings { get; set; }
        public virtual ICollection<TblSponsorCategoryMapping> TblSponsorCategoryMappings { get; set; }
        public virtual ICollection<TblUninstallationPrice> TblUninstallationPrices { get; set; }
        public virtual ICollection<TblUniversalPriceMaster> TblUniversalPriceMasters { get; set; }
        public virtual ICollection<TblVehicleIncentive> TblVehicleIncentives { get; set; }
        public virtual ICollection<TblVoucherVerfication> TblVoucherVerfications { get; set; }
        public virtual ICollection<UniversalPriceMaster> UniversalPriceMasters { get; set; }
    }
}
