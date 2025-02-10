using System;
using System.Collections.Generic;

#nullable disable

namespace RDCELERP.DAL.Entities
{
    public partial class ViewExchangWithQc
    {
        public int OrderTransId { get; set; }
        public int? OrderType { get; set; }
        public int? ExchangeId { get; set; }
        public int? AbbredemptionId { get; set; }
        public string SponsorOrderNumber { get; set; }
        public string RegdNo { get; set; }
        public decimal? ExchangePrice { get; set; }
        public decimal? QuotedPrice { get; set; }
        public decimal? Sweetner { get; set; }
        public decimal? FinalPriceAfterQc { get; set; }
        public int? EvcpriceMasterId { get; set; }
        public decimal? Evcprice { get; set; }
        public bool? IsActive { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public int? StatusId { get; set; }
        public int? OrderQcid { get; set; }
        public int? Expr1 { get; set; }
        public DateTime? ProposedQcdate { get; set; }
        public string Qccomments { get; set; }
        public string QualityAfterQc { get; set; }
        public decimal? PriceAfterQc { get; set; }
        public DateTime? Expr2 { get; set; }
        public int Id { get; set; }
        public string CompanyName { get; set; }
        public int? CustomerDetailsId { get; set; }
        public int? ProductTypeId { get; set; }
        public int? BrandId { get; set; }
        public string Bonus { get; set; }
        public string Expr3 { get; set; }
        public string EstimatedDeliveryDate { get; set; }
        public string ProductCondition { get; set; }
        public int? LoginId { get; set; }
        public string ExchPriceCode { get; set; }
        public bool? Expr4 { get; set; }
        public int? Expr5 { get; set; }
        public DateTime? Expr6 { get; set; }
        public DateTime? Expr7 { get; set; }
        public bool? IsDtoC { get; set; }
        public string Expr8 { get; set; }
        public int? BusinessPartnerId { get; set; }
        public string SaleAssociateName { get; set; }
        public string SaleAssociateCode { get; set; }
        public string PurchasedProductCategory { get; set; }
        public bool? IsDelivered { get; set; }
        public string StoreCode { get; set; }
        public string VoucherCode { get; set; }
        public bool? IsVoucherused { get; set; }
        public string SalesAssociateEmail { get; set; }
        public string SalesAssociatePhone { get; set; }
        public string InvoiceImageName { get; set; }
        public DateTime? VoucherCodeExpDate { get; set; }
        public decimal? Expr9 { get; set; }
        public string ProductNumber { get; set; }
        public int? NewProductCategoryId { get; set; }
        public int? NewProductTypeId { get; set; }
        public int? NewBrandId { get; set; }
        public int? ModelNumberId { get; set; }
        public string InvoiceNumber { get; set; }
        public string Qcdate { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public int? Expr10 { get; set; }
        public string Comment1 { get; set; }
        public string Comment2 { get; set; }
        public string Comment3 { get; set; }
        public string IsUnInstallationRequired { get; set; }
    }
}
