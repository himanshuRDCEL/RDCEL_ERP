using System;
using System.Collections.Generic;

namespace RDCELERP.DAL.Entities
{
    public partial class TblAbbpriceMaster
    {
        public int PriceMasterId { get; set; }
        public int? BusinessUnitId { get; set; }
        public int? ProductCatId { get; set; }
        public int? ProductTypeId { get; set; }
        public short? FeeTypeId { get; set; }
        public string? Sponsor { get; set; }
        public string? ProductCategory { get; set; }
        public string? ProductSabcategory { get; set; }
        public int? PriceStartRange { get; set; }
        public int? PriceEndRange { get; set; }
        public string? FeeType { get; set; }
        public decimal? FeesApplicableAmt { get; set; }
        public decimal? FeesApplicablePercentage { get; set; }
        public int? PlanPeriodInMonths { get; set; }
        public bool? IsActive { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public decimal? BusinessUnitMarginAmount { get; set; }
        public decimal? BusinessUnitMarginPerc { get; set; }
        public decimal? BusinessPartnerMarginAmount { get; set; }
        public decimal? BusinessPartnerMarginPerc { get; set; }
        public decimal? Gstexclusive { get; set; }
        public decimal? Gstinclusive { get; set; }
        public decimal? GstValueForNewProduct { get; set; }

        public virtual TblBusinessUnit? BusinessUnit { get; set; }
        public virtual TblProductCategory? ProductCat { get; set; }
        public virtual TblProductType? ProductType { get; set; }
    }
}
