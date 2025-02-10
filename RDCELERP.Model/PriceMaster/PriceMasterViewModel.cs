using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDCELERP.Model.Base;

namespace RDCELERP.Model.PriceMaster
{
    public class PriceMasterViewModel : BaseViewModel
    {
        public int Id { get; set; }

        public string? ZohoPriceMasterId { get; set; }
        [Required(ErrorMessage = "Exch price code is required.")]
       
        public string? ExchPriceCode { get; set; }
        [Required(ErrorMessage = "Product category description is required.")]
        [DisplayName("Product Category Description")]
        public string? ProductCategoryDiscription { get; set; }
        public int? ProductCategoryId { get; set; }
        
        
        [Required(ErrorMessage = "Product category is required.")]
       
        [DisplayName("Product Category Code")]
        
        public string? ProductCat { get; set; }

        public int? ProductTypeId { get; set; }
        public string? ProductTypeName { get; set; }
        [Required(ErrorMessage = "Product type is required.")]
        [DisplayName("Product Type")]
        
        public string? ProductType { get; set; }
        [Required(ErrorMessage = "Product type code is required.")]
        [DisplayName("Product Type Code")]
     
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
        [RegularExpression(@"^[1-9]\d*$", ErrorMessage = "Quote P high should be a positive integer.")]
        public string? QuotePHigh { get; set; }
        [Required(ErrorMessage = "Quote Q high is required.")]
        [RegularExpression(@"^[1-9]\d*$", ErrorMessage = "Quote Q high should be a positive integer.")]
        public string? QuoteQHigh { get; set; }
        [Required(ErrorMessage = "Quote R high is required.")]
        [RegularExpression(@"^[1-9]\d*$", ErrorMessage = "Quote R high should be a positive integer.")]
        public string? QuoteRHigh { get; set; }
        [Required(ErrorMessage = "Quote S high is required.")]
        [RegularExpression(@"^[1-9]\d*$", ErrorMessage = "Quote S high should be a positive integer.")]
        public string? QuoteSHigh { get; set; }
        [Required(ErrorMessage = "Quote P is required.")]
        [RegularExpression(@"^[1-9]\d*$", ErrorMessage = "Quote P should be a positive integer.")]
        public string? QuoteP { get; set; }
        [Required(ErrorMessage = "Quote Q is required.")]
        [RegularExpression(@"^[1-9]\d*$", ErrorMessage = "Quote Q should be a positive integer.")]
        public string? QuoteQ { get; set; }
        [Required(ErrorMessage = "Quote R is required.")]
        [RegularExpression(@"^[1-9]\d*$", ErrorMessage = "Quote R should be a positive integer.")]
        public string? QuoteR { get; set; }
        [Required(ErrorMessage = "Quote S is required.")]
        [RegularExpression(@"^[1-9]\d*$", ErrorMessage = "Quote S should be a positive integer.")]
        public string? QuoteS { get; set; }
      
        public string? OtherBrand { get; set; }
        public string? Date { get; set; }
        [Required(ErrorMessage = "Price start date is required.")]
        public string? PriceStartDate { get; set; }
        [Required(ErrorMessage = "Price end date is required.")]
        public string? PriceEndDate { get; set; }
        public string? UploadExchangePriceMaster { get; set; }
        public IFormFile? UploadPriceMaster { get; set; }
        public PriceMasterExcel? PriceMasterVM { get; set; }
        public List<PriceMasterExcel>? PriceMasterVMList { get; set; }
        public List<PriceMasterExcel>? PriceMasterVMErrorList { get; set; }
    }
}
