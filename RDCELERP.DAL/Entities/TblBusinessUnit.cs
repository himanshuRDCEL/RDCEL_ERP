using System;
using System.Collections.Generic;

namespace RDCELERP.DAL.Entities
{
    public partial class TblBusinessUnit
    {
        public TblBusinessUnit()
        {
            TblAbbplanMasters = new HashSet<TblAbbplanMaster>();
            TblAbbpriceMasters = new HashSet<TblAbbpriceMaster>();
            TblAbbregistrations = new HashSet<TblAbbregistration>();
            TblBppincodeMappings = new HashSet<TblBppincodeMapping>();
            TblBrandSmartBuys = new HashSet<TblBrandSmartBuy>();
            TblBrands = new HashSet<TblBrand>();
            TblBubasedSweetnerValidations = new HashSet<TblBubasedSweetnerValidation>();
            TblBuconfigurationMappings = new HashSet<TblBuconfigurationMapping>();
            TblBuproductCategoryMappings = new HashSet<TblBuproductCategoryMapping>();
            TblBusinessPartners = new HashSet<TblBusinessPartner>();
            TblCompanies = new HashSet<TblCompany>();
            TblExchangeOrders = new HashSet<TblExchangeOrder>();
            TblModelMappings = new HashSet<TblModelMapping>();
            TblModelNumbers = new HashSet<TblModelNumber>();
            TblOrderBasedConfigs = new HashSet<TblOrderBasedConfig>();
            TblPriceMasterMappings = new HashSet<TblPriceMasterMapping>();
            TblPriceMasterQuestioners = new HashSet<TblPriceMasterQuestioner>();
            TblProductConditionLabels = new HashSet<TblProductConditionLabel>();
            TblSocieties = new HashSet<TblSociety>();
            TblSponsorCategoryMappings = new HashSet<TblSponsorCategoryMapping>();
            TblTransMasterAbbplanMasters = new HashSet<TblTransMasterAbbplanMaster>();
            TblUninstallationPrices = new HashSet<TblUninstallationPrice>();
            TblUserMappings = new HashSet<TblUserMapping>();
            TblVehicleIncentives = new HashSet<TblVehicleIncentive>();
            TblVoucherTermsAndConditions = new HashSet<TblVoucherTermsAndCondition>();
        }

        public int BusinessUnitId { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? RegistrationNumber { get; set; }
        public string? QrcodeUrl { get; set; }
        public string? LogoName { get; set; }
        public string? ContactPersonFirstName { get; set; }
        public string? ContactPersonLastName { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }
        public string? AddressLine1 { get; set; }
        public string? AddressLine2 { get; set; }
        public string? Pincode { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public int? LoginId { get; set; }
        public bool? IsActive { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string? ZohoSponsorId { get; set; }
        public int? ExpectedDeliveryHours { get; set; }
        public int? VoucherExpiryTime { get; set; }
        public decimal? SweetnerForDtd { get; set; }
        public decimal? SweetnerForDtc { get; set; }
        public bool? IsSweetnerModelBased { get; set; }
        public bool? IsAbb { get; set; }
        public bool? IsExchange { get; set; }
        public bool? IsBumultiBrand { get; set; }
        public bool? IsBud2c { get; set; }
        public bool? ShowAbbPlan { get; set; }
        public bool? IsInvoiceDetailsRequired { get; set; }
        public bool? IsNewProductDetailsRequired { get; set; }
        public int? Gsttype { get; set; }
        public int? MarginType { get; set; }
        public bool? IsSponsorNumberRequiredOnUi { get; set; }
        public bool? IsUpiIdRequired { get; set; }
        public bool? IsPaymentThirdParty { get; set; }
        public bool? IsModelDetailRequired { get; set; }
        public bool? IsNewBrandRequired { get; set; }
        public bool? IsAreaLocality { get; set; }
        public bool? IsValidationBasedSweetner { get; set; }
        public bool? IsQualityRequiredOnUi { get; set; }
        public bool? IsQualityWorkingNonWorking { get; set; }
        public bool? IsStandardPriceMaster { get; set; }
        public bool? ShowEmplyeeCode { get; set; }
        public bool? IsQcdateTimeRequiredOnD2c { get; set; }
        public bool? IsBulkOrder { get; set; }
        public bool? IsCertificateAvailable { get; set; }
        public bool? IsSweetenerIndependent { get; set; }
        public bool? IsInvoiceAvailable { get; set; }
        public bool? IsReportingOn { get; set; }
        public int? OrderPendingTimeH { get; set; }
        public string? ReportEmails { get; set; }
        public bool? IsAbbDayConfig { get; set; }
        public int? AbbDayDiff { get; set; }
        public bool? IsBpassociated { get; set; }
        public bool? IsBucatIdOn { get; set; }
        public bool? IsProductSerialNumberRequired { get; set; }
        public bool? IsSfidrequired { get; set; }

        public virtual TblLoV? GsttypeNavigation { get; set; }
        public virtual Login? Login { get; set; }
        public virtual TblLoV? MarginTypeNavigation { get; set; }
        public virtual ICollection<TblAbbplanMaster> TblAbbplanMasters { get; set; }
        public virtual ICollection<TblAbbpriceMaster> TblAbbpriceMasters { get; set; }
        public virtual ICollection<TblAbbregistration> TblAbbregistrations { get; set; }
        public virtual ICollection<TblBppincodeMapping> TblBppincodeMappings { get; set; }
        public virtual ICollection<TblBrandSmartBuy> TblBrandSmartBuys { get; set; }
        public virtual ICollection<TblBrand> TblBrands { get; set; }
        public virtual ICollection<TblBubasedSweetnerValidation> TblBubasedSweetnerValidations { get; set; }
        public virtual ICollection<TblBuconfigurationMapping> TblBuconfigurationMappings { get; set; }
        public virtual ICollection<TblBuproductCategoryMapping> TblBuproductCategoryMappings { get; set; }
        public virtual ICollection<TblBusinessPartner> TblBusinessPartners { get; set; }
        public virtual ICollection<TblCompany> TblCompanies { get; set; }
        public virtual ICollection<TblExchangeOrder> TblExchangeOrders { get; set; }
        public virtual ICollection<TblModelMapping> TblModelMappings { get; set; }
        public virtual ICollection<TblModelNumber> TblModelNumbers { get; set; }
        public virtual ICollection<TblOrderBasedConfig> TblOrderBasedConfigs { get; set; }
        public virtual ICollection<TblPriceMasterMapping> TblPriceMasterMappings { get; set; }
        public virtual ICollection<TblPriceMasterQuestioner> TblPriceMasterQuestioners { get; set; }
        public virtual ICollection<TblProductConditionLabel> TblProductConditionLabels { get; set; }
        public virtual ICollection<TblSociety> TblSocieties { get; set; }
        public virtual ICollection<TblSponsorCategoryMapping> TblSponsorCategoryMappings { get; set; }
        public virtual ICollection<TblTransMasterAbbplanMaster> TblTransMasterAbbplanMasters { get; set; }
        public virtual ICollection<TblUninstallationPrice> TblUninstallationPrices { get; set; }
        public virtual ICollection<TblUserMapping> TblUserMappings { get; set; }
        public virtual ICollection<TblVehicleIncentive> TblVehicleIncentives { get; set; }
        public virtual ICollection<TblVoucherTermsAndCondition> TblVoucherTermsAndConditions { get; set; }
    }
}
