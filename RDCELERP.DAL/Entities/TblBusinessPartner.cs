using System;
using System.Collections.Generic;

namespace RDCELERP.DAL.Entities
{
    public partial class TblBusinessPartner
    {
        public TblBusinessPartner()
        {
            TblAbbredemptions = new HashSet<TblAbbredemption>();
            TblAbbregistrations = new HashSet<TblAbbregistration>();
            TblBppincodeMappings = new HashSet<TblBppincodeMapping>();
            TblExchangeOrders = new HashSet<TblExchangeOrder>();
            TblModelMappings = new HashSet<TblModelMapping>();
            TblModelNumbers = new HashSet<TblModelNumber>();
            TblOrderBasedConfigs = new HashSet<TblOrderBasedConfig>();
            TblPriceMasterMappings = new HashSet<TblPriceMasterMapping>();
            TblProductConditionLabels = new HashSet<TblProductConditionLabel>();
            TblUninstallationPrices = new HashSet<TblUninstallationPrice>();
            TblUserMappings = new HashSet<TblUserMapping>();
            TblVoucherVerfications = new HashSet<TblVoucherVerfication>();
        }

        public int BusinessPartnerId { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? StoreCode { get; set; }
        public string? QrcodeUrl { get; set; }
        public string? Qrimage { get; set; }
        public string? ContactPersonFirstName { get; set; }
        public string? ContactPersonLastName { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }
        public string? AddressLine1 { get; set; }
        public string? AddressLine2 { get; set; }
        public string? Pincode { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public int? BusinessUnitId { get; set; }
        public bool? IsActive { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public bool? IsAbbbp { get; set; }
        public bool? IsExchangeBp { get; set; }
        public string? FormatName { get; set; }
        public bool? IsDealer { get; set; }
        public string? Bppassword { get; set; }
        public string? AssociateCode { get; set; }
        public bool? IsOrc { get; set; }
        public bool? IsDefferedSettlement { get; set; }
        public string? LogoImage { get; set; }
        public bool? IsOtpRequired { get; set; }
        public string? SponsorName { get; set; }
        public string? StoreType { get; set; }
        public string? Gstnumber { get; set; }
        public string? BankDetails { get; set; }
        public string? AccountNo { get; set; }
        public string? Ifsccode { get; set; }
        public bool? PaymentToCustomer { get; set; }
        public string? DashBoardImage { get; set; }
        public bool? IsDefferedAbb { get; set; }
        public bool? IsD2c { get; set; }
        public bool? IsVoucher { get; set; }
        public int? VoucherType { get; set; }
        public bool? IsRedemptionSettelemtInstant { get; set; }
        public int? CityId { get; set; }
        public decimal? SweetenerBu { get; set; }
        public decimal? SweetenerBp { get; set; }
        public decimal? SweetenerDigi2l { get; set; }
        public string? Upiid { get; set; }
        public bool? IsDefaultPickupAddress { get; set; }
        public bool? IsUnInstallationRequired { get; set; }

        public virtual TblBusinessUnit? BusinessUnit { get; set; }
        public virtual TblCity? CityNavigation { get; set; }
        public virtual ICollection<TblAbbredemption> TblAbbredemptions { get; set; }
        public virtual ICollection<TblAbbregistration> TblAbbregistrations { get; set; }
        public virtual ICollection<TblBppincodeMapping> TblBppincodeMappings { get; set; }
        public virtual ICollection<TblExchangeOrder> TblExchangeOrders { get; set; }
        public virtual ICollection<TblModelMapping> TblModelMappings { get; set; }
        public virtual ICollection<TblModelNumber> TblModelNumbers { get; set; }
        public virtual ICollection<TblOrderBasedConfig> TblOrderBasedConfigs { get; set; }
        public virtual ICollection<TblPriceMasterMapping> TblPriceMasterMappings { get; set; }
        public virtual ICollection<TblProductConditionLabel> TblProductConditionLabels { get; set; }
        public virtual ICollection<TblUninstallationPrice> TblUninstallationPrices { get; set; }
        public virtual ICollection<TblUserMapping> TblUserMappings { get; set; }
        public virtual ICollection<TblVoucherVerfication> TblVoucherVerfications { get; set; }
    }
}
