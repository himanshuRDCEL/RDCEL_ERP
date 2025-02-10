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
    public class PriceMasterVMExcel/* : IValidatableObject*/
    {
        [Required]
        [DisplayName("Product Category Code")]
        public string? ProductCat { get; set; }
        [Required]
        [DisplayName("Product Type")]
        public string? ProductType { get; set; }
        [Required]
        [DisplayName("Product Type Code")]
        public string? ProductTypeCode { get; set; }
        [Required]
        public string? BrandName1 { get; set; }
        [Required]
        public string? BrandName2 { get; set; }
        public string? BrandName3 { get; set; }
        public string? BrandName4 { get; set; }
        [Required]
        public int QuotePHigh { get; set; }
        [Required]
        public int QuoteQHigh { get; set; }
        [Required]
        public int QuoteRHigh { get; set; }
        [Required]
        public int QuoteSHigh { get; set; }
        [Required]
        public int QuoteP { get; set; }
        [Required]
        public int QuoteQ { get; set; }
        [Required]
        public int QuoteR { get; set; }
        [Required]
        public int QuoteS { get; set; }
        [Required]
        public string? PriceStartDate { get; set; }
        [Required]
        public string? PriceEndDate { get; set; }
        public string? OtherBrand { get; set; }
        public bool IsActive { get; set; }
        public string? Remarks { get; set; }
/*
        IEnumerable<ValidationResult> IValidatableObject.Validate(ValidationContext validationContext)
        {
            throw new NotImplementedException();
        }*/
    }
}
