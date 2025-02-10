using System;
using System.Collections.Generic;

namespace RDCELERP.DAL.Entities
{
    public partial class TblPriceMaster
    {
        public int Id { get; set; }
        public string? ZohoPriceMasterId { get; set; }
        public string? ExchPriceCode { get; set; }
        public int? ProductCategoryId { get; set; }
        public string? ProductCat { get; set; }
        public int? ProductTypeId { get; set; }
        public string? ProductType { get; set; }
        public string? ProductTypeCode { get; set; }
        public string? BrandName1 { get; set; }
        public string? BrandName2 { get; set; }
        public string? BrandName3 { get; set; }
        public string? BrandName4 { get; set; }
        public string? QuotePHigh { get; set; }
        public string? QuoteQHigh { get; set; }
        public string? QuoteRHigh { get; set; }
        public string? QuoteSHigh { get; set; }
        public string? QuoteP { get; set; }
        public string? QuoteQ { get; set; }
        public string? QuoteR { get; set; }
        public string? QuoteS { get; set; }
        public string? PriceStartDate { get; set; }
        public string? PriceEndDate { get; set; }
        public string? OtherBrand { get; set; }
        public bool? IsActive { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }

        public virtual TblProductCategory? ProductCategory { get; set; }
        public virtual TblProductType? ProductTypeNavigation { get; set; }
    }
}
