


using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDCELERP.Model.MobileApplicationModel;

namespace RDCELERP.Model.UniversalPriceMaster
{
    public class UniversalPriceMasterExcel
    {
        public int PriceMasterUniversalId { get; set; }
        [Required(ErrorMessage = "Company is required.")]
        public string? Company { get; set; }
        public string? StoreCode { get; set; }
        public string? NewBrand { get; set; }
        [Required(ErrorMessage = "Price master name  is required.")]
        public string? PriceMasterName { get; set; }
        [Required(ErrorMessage = "Product category  is required.")]
        [DisplayName("Product Category")]
        public string? ProductCategory { get; set; }
        [Required(ErrorMessage = "Product type  is required.")]
        [DisplayName("Product Type")]
        public string? ProductType { get; set; }
        [Required(ErrorMessage = "Product type code is required.")]
        public string? ProductTypeCode { get; set; }
        [Required(ErrorMessage = "Brand name 1 is required.")]
        public string? BrandName1 { get; set; }
        [Required(ErrorMessage = "Brand name 2 is required.")]
        public string? BrandName2 { get; set; }
        [Required(ErrorMessage = "Brand name 3 is required.")]
        public string? BrandName3 { get; set; }
        [Required(ErrorMessage = "Brand name 4 is required.")]
        public string? BrandName4 { get; set; }
        [Required(ErrorMessage = "Quote P high is required.")]
        public string? QuotePHigh { get; set; }
        [Required(ErrorMessage = "Quote Q high is required.")]
        public string? QuoteQHigh { get; set; }
        [Required(ErrorMessage = "Quote R high is required.")]
        public string? QuoteRHigh { get; set; }
        [Required(ErrorMessage = "Quote S high is required.")]
        public string? QuoteSHigh { get; set; }
        [Required(ErrorMessage = "Quote P is required.")]
        public string? QuoteP { get; set; }
        [Required(ErrorMessage = "Quote Q is required.")]
        public string? QuoteQ { get; set; }
        [Required(ErrorMessage = "Quote R is required.")]
        public string? QuoteR { get; set; }
        [Required(ErrorMessage = "Quote S is required.")]
        public string? QuoteS { get; set; }
        [Required(ErrorMessage = "Other brand is required.")]
        public string? OtherBrand { get; set; }
        public string? Remarks { get; set; }
        [Required(ErrorMessage = "Price start date is required.")]
        public string? PriceStartDate { get; set; }
        [Required(ErrorMessage = "Price end date is required.")]
        public string? PriceEndDate { get; set; }
        public string? UploadExchangePriceMaster { get; set; }
    }
}
