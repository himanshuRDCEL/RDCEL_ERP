
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
    public class UniversalPriceMasterVMExcel/* : IValidatableObject*/
    {

        public int PriceMasterUniversalId { get; set; }
        public int? PriceMasterNameId { get; set; }


        [Required(ErrorMessage = "PriceMaster Name  is required.")]

        public string? PriceMasterName { get; set; }
      

        [Required]
        [DisplayName("Product Category Code")]
        public string? ProductCategory { get; set; }
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
        public string? QuotePHigh { get; set; }
        [Required]
        public string? QuoteQHigh { get; set; }
        [Required]
        public string? QuoteRHigh { get; set; }
        [Required]
        public string? QuoteSHigh { get; set; }
        [Required]
        public string? QuoteP { get; set; }
        [Required]
        public string? QuoteQ { get; set; }
        [Required]
        public string? QuoteR { get; set; }
        [Required]
        public string? QuoteS { get; set; }
        [Required]
        public string? PriceStartDate { get; set; }
        [Required]
        public string? PriceEndDate { get; set; }
        public string? OtherBrand { get; set; }
        public bool IsActive { get; set; }
        public string? Remarks { get; set; }
       
    }
}
