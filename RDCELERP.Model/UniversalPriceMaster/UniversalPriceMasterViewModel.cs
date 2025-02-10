using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDCELERP.DAL.Entities;
using RDCELERP.Model.Base;
using RDCELERP.Model.PriceMaster;

namespace RDCELERP.Model.UniversalPriceMaster
{
    public class UniversalPriceMasterViewModel : BaseViewModel
    {

        public int PriceMasterUniversalId { get; set; }
        public int? PriceMasterNameId { get; set; }


        [Required(ErrorMessage = "PriceMaster Name  is required.")]

        public string? PriceMasterName { get; set; }
        [Required(ErrorMessage = "Product category is required")]
        [DisplayName("Product Category")]
        public string? ProductCategoryDiscription { get; set; }

        public int? ProductCategoryId { get; set; }
        public string? ProductCategoryName { get; set; }

       
       
        [Required(ErrorMessage = "Product type is required.")]
        public string? ProductTypeName { get; set; }
        public int? ProductTypeId { get; set; }

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

        public virtual TblUser? CreatedByNavigation { get; set; }
        public virtual TblUser? ModifiedByNavigation { get; set; }
        public virtual PriceMasterName? PriceMasterNameNavigation { get; set; }
        public virtual TblProductCategory? ProductCategory { get; set; }
        public virtual TblProductType? ProductTypeNavigation { get; set; }
        public string? UploadExchangeUniversalPriceMaster { get; set; }
        public IFormFile? UploadUniversalPriceMaster { get; set; }
        public UniversalPriceMasterExcel? UniversalPriceMasterVM { get; set; }
        public List<UniversalPriceMasterExcel>? UniversalPriceMasterVMList { get; set; }
        public List<UniversalPriceMasterExcel>? UniversalPriceMasterVMErrorList { get; set; }
    }
}
