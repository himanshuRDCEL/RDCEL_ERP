using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDCELERP.Model.Base;

namespace RDCELERP.Model.ModelNumber
{
    public class ModelMappingViewModel : BaseViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Model name is required.")]
        public string? ModelName { get; set; }
        public int ModelId { get; set; }
        [Required(ErrorMessage = "Brand name is required.")]
        [DisplayName("Brand Name")]
        public string? BrandName { get; set; }
        public int? BrandId { get; set; }
       
        public bool? IsDefault { get; set; }
        [Required(ErrorMessage = "Business unit name is required.")]
        [DisplayName("BusinessUnit Name")]
        public string? BusinessUnitName { get; set; }
        public int? BusinessUnitId { get; set; }
      
       
        [RegularExpression(@"^\d+(\.\d{1,2})?$", ErrorMessage = "Value must be a positive number")]

        public decimal? SweetenerBu { get; set; }
      
        [RegularExpression(@"^\d+(\.\d{1,2})?$", ErrorMessage = "Value must be a positive number")]

        public decimal? SweetenerBp { get; set; }
        [Required(ErrorMessage = "If nothing then enter 0")]
        [RegularExpression(@"^\d+(\.\d{1,2})?$", ErrorMessage = "Value must be a positive number")]
        public decimal? SweetenerDigi2l { get; set; }
        [Required(ErrorMessage = "Business partner name is required.")]
        [DisplayName("Business Partner Name")]
        public string? BusinessPartnerName { get; set; }
        public int? BusinessPartnerId { get; set; }
        public string? Date { get; set; }
    }
}
