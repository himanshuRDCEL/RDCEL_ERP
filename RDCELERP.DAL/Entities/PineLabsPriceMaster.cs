using System;
using System.Collections.Generic;

namespace RDCELERP.DAL.Entities
{
    public partial class PineLabsPriceMaster
    {
        public short PriceMasterUniversalId { get; set; }
        public byte PriceMasterNameId { get; set; }
        public string PriceMasterName { get; set; } = null!;
        public byte ProductCategoryId { get; set; }
        public string ProductCategoryName { get; set; } = null!;
        public byte ProductTypeId { get; set; }
        public string ProductTypeName { get; set; } = null!;
        public string ProductTypeCode { get; set; } = null!;
        public string BrandName1 { get; set; } = null!;
        public string BrandName2 { get; set; } = null!;
        public string BrandName3 { get; set; } = null!;
        public string BrandName4 { get; set; } = null!;
        public short QuotePHigh { get; set; }
        public short QuoteQHigh { get; set; }
        public short QuoteRHigh { get; set; }
        public short QuoteSHigh { get; set; }
        public short QuoteP { get; set; }
        public short QuoteQ { get; set; }
        public short QuoteR { get; set; }
        public short QuoteS { get; set; }
        public string OtherBrand { get; set; } = null!;
        public DateTime PriceStartDate { get; set; }
        public DateTime PriceEndDate { get; set; }
        public byte IsActive { get; set; }
        public byte CreatedBy { get; set; }
        public string CreatedDate { get; set; } = null!;
        public string ModifiedBy { get; set; } = null!;
        public string ModifiedDate { get; set; } = null!;
    }
}
