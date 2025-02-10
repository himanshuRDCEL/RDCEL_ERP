using System;
using System.Collections.Generic;

namespace RDCELERP.DAL.Entities
{
    public partial class TblExchangeOrder
    {
        public TblExchangeOrder()
        {
            TblEvcdisputes = new HashSet<TblEvcdispute>();
            TblFeedBacks = new HashSet<TblFeedBack>();
            TblHistories = new HashSet<TblHistory>();
            TblImages = new HashSet<TblImage>();
            TblOrderTrans = new HashSet<TblOrderTran>();
            TblSelfQcs = new HashSet<TblSelfQc>();
            TblVoucherVerfications = new HashSet<TblVoucherVerfication>();
        }

        public int Id { get; set; }
        public string? CompanyName { get; set; }
        public string? ZohoSponsorOrderId { get; set; }
        public string? OrderStatus { get; set; }
        public int? CustomerDetailsId { get; set; }
        public int? ProductTypeId { get; set; }
        public int? BrandId { get; set; }
        public string? Bonus { get; set; }
        public string? SponsorOrderNumber { get; set; }
        public string? EstimatedDeliveryDate { get; set; }
        public string? ProductCondition { get; set; }
        public int? LoginId { get; set; }
        public string? ExchPriceCode { get; set; }
        public bool? IsActive { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public bool? IsDtoC { get; set; }
        public int? SocietyId { get; set; }
        public string? RegdNo { get; set; }
        public int? BusinessPartnerId { get; set; }
        public string? SaleAssociateName { get; set; }
        public string? SaleAssociateCode { get; set; }
        public string? PurchasedProductCategory { get; set; }
        public string? StoreCode { get; set; }
        public bool? IsDelivered { get; set; }
        public string? VoucherCode { get; set; }
        public bool? IsVoucherused { get; set; }
        public string? SalesAssociateEmail { get; set; }
        public string? SalesAssociatePhone { get; set; }
        public string? InvoiceImageName { get; set; }
        public DateTime? VoucherCodeExpDate { get; set; }
        public decimal? ExchangePrice { get; set; }
        public string? ProductNumber { get; set; }
        public int? NewProductCategoryId { get; set; }
        public int? NewProductTypeId { get; set; }
        public int? NewBrandId { get; set; }
        public int? ModelNumberId { get; set; }
        public string? InvoiceNumber { get; set; }
        public string? Qcdate { get; set; }
        public string? StartTime { get; set; }
        public string? EndTime { get; set; }
        public int? StatusId { get; set; }
        public string? Comment1 { get; set; }
        public string? Comment2 { get; set; }
        public string? Comment3 { get; set; }
        public string? IsUnInstallationRequired { get; set; }
        public decimal? UnInstallationPrice { get; set; }
        public int? VoucherStatusId { get; set; }
        public decimal? Sweetener { get; set; }
        public bool? OtherCommunications { get; set; }
        public bool? OtherCommunications1 { get; set; }
        public bool? FollowupCommunication { get; set; }
        public bool? FollowupCommunication1 { get; set; }
        public string? SerialNumber { get; set; }
        public decimal? FinalExchangePrice { get; set; }
        public bool? IsDefferedSettlement { get; set; }
        public string? SponsorServiceRefId { get; set; }
        public int? ProductTechnologyId { get; set; }
        public bool? IsExchangePriceWithoutSweetner { get; set; }
        public bool? IsFinalExchangePriceWithoutSweetner { get; set; }
        public decimal? BaseExchangePrice { get; set; }
        public string? EmployeeId { get; set; }
        public decimal? SweetenerBu { get; set; }
        public decimal? SweetenerBp { get; set; }
        public decimal? SweetenerDigi2l { get; set; }
        public int? BusinessUnitId { get; set; }
        public int? PriceMasterNameId { get; set; }
        public bool? IsDiagnoseV2 { get; set; }

        public virtual TblBrand? Brand { get; set; }
        public virtual TblBusinessPartner? BusinessPartner { get; set; }
        public virtual TblBusinessUnit? BusinessUnit { get; set; }
        public virtual TblUser? CreatedByNavigation { get; set; }
        public virtual TblCustomerDetail? CustomerDetails { get; set; }
        public virtual TblUser? ModifiedByNavigation { get; set; }
        public virtual TblProductType? NewProductType { get; set; }
        public virtual TblPriceMasterName? PriceMasterName { get; set; }
        public virtual TblProductTechnology? ProductTechnology { get; set; }
        public virtual TblProductType? ProductType { get; set; }
        public virtual TblSociety? Society { get; set; }
        public virtual TblExchangeOrderStatus? Status { get; set; }
        public virtual TblVoucherStatus? VoucherStatus { get; set; }
        public virtual ICollection<TblEvcdispute> TblEvcdisputes { get; set; }
        public virtual ICollection<TblFeedBack> TblFeedBacks { get; set; }
        public virtual ICollection<TblHistory> TblHistories { get; set; }
        public virtual ICollection<TblImage> TblImages { get; set; }
        public virtual ICollection<TblOrderTran> TblOrderTrans { get; set; }
        public virtual ICollection<TblSelfQc> TblSelfQcs { get; set; }
        public virtual ICollection<TblVoucherVerfication> TblVoucherVerfications { get; set; }
    }
}
