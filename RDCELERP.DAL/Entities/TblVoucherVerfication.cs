using System;
using System.Collections.Generic;

namespace RDCELERP.DAL.Entities
{
    public partial class TblVoucherVerfication
    {
        public int VoucherVerficationId { get; set; }
        public int? CustomerId { get; set; }
        public int? ExchangeOrderId { get; set; }
        public string? InvoiceName { get; set; }
        public bool? IsDealerConfirm { get; set; }
        public string? Description { get; set; }
        public bool? IsActive { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string? VoucherCode { get; set; }
        public bool? IsVoucherused { get; set; }
        public string? InvoiceImageName { get; set; }
        public int? BusinessPartnerId { get; set; }
        public int? NewProductCategoryId { get; set; }
        public int? NewProductTypeId { get; set; }
        public int? NewBrandId { get; set; }
        public int? ModelNumberId { get; set; }
        public decimal? ExchangePrice { get; set; }
        public string? InvoiceNumber { get; set; }
        public decimal? Sweetneer { get; set; }
        public int? VoucherStatusId { get; set; }
        public int? RedemptionId { get; set; }

        public virtual TblBusinessPartner? BusinessPartner { get; set; }
        public virtual TblCustomerDetail? Customer { get; set; }
        public virtual TblExchangeOrder? ExchangeOrder { get; set; }
        public virtual TblModelNumber? ModelNumber { get; set; }
        public virtual TblBrand? NewBrand { get; set; }
        public virtual TblProductCategory? NewProductCategory { get; set; }
        public virtual TblProductType? NewProductType { get; set; }
        public virtual TblAbbredemption? Redemption { get; set; }
        public virtual TblVoucherStatus? VoucherStatus { get; set; }
    }
}
