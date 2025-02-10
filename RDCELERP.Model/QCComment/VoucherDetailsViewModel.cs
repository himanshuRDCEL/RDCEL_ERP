using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDCELERP.Model.Base;
using RDCELERP.Model.BusinessPartner;
using RDCELERP.Model.Product;
using RDCELERP.Model.ProductQuality;

namespace RDCELERP.Model.QCComment
{
    public class VoucherDetailsViewModel : BaseViewModel
    {
        public int VoucherVerficationId { get; set; }
        public int CustomerId { get; set; }
        public int ExchangeOrderId { get; set; }
        public string? InvoiceName { get; set; }
        public bool IsDealerConfirm { get; set; }
        public string? Description { get; set; }
        public string? VoucherCode { get; set; }
        public bool IsVoucherused { get; set; }
        public string? InvoiceImageName { get; set; }
        public string? NewProductCategoryName { get; set; }
        public string? NewProductTypeName { get; set; }       
        public int BusinessPartnerId { get; set; }
        public int NewProductCategoryId { get; set; }
        public int NewProductTypeId { get; set; }

        public string? VoucherStatusName { get; set; }

        public int NewBrandId { get; set; }
        public int ModelNumberId { get; set; }
        public string? ModelNumber { get; set; }
        public decimal ExchangePrice { get; set; }
        public string? InvoiceNumber { get; set; }
        public decimal Sweetneer { get; set; }
        public int VoucherStatusId { get; set; }
        public BusinessPartnerViewModel? BusinessPartnerViewModel { get; set; }
        public ProductTypeViewModel? ProductTypeViewModel { get; set; }
        public ProductQualityIndexViewModel? ProductQualityIndexViewModel { get; set; }

        public string? Brandname { get; set; }
        public string? BrandLogo{ get; set; }
        public string? UTClogo{ get; set; }
        public string? NewProductSize { get; set; }
        public string? NewBrandName { get; set; }
    }

    public class ABBVoucherDetailsViewModel : BaseViewModel
    {
        public int RedemptionId { get; set; }
        public string? VoucherCode { get; set; }
        public bool IsVoucherused { get; set; }
        public int VoucherVerficationId { get; set; }          
        public string? VoucherStatusName { get; set; }       
        public decimal RedemptionPrice { get; set; }       
        public int VoucherStatusId { get; set; }       
       // public DateTime VoucherCodeExpDate { get; set; }
        public string? VoucherCodeExpDate { get; set; }

        public string? VoucherRedemptionby { get; set; }

    }
}
