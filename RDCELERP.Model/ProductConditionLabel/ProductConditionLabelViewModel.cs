using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDCELERP.Model.Base;

namespace RDCELERP.Model.ProductConditionLabel
{
    public class ProductConditionLabelViewModel : BaseViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Product condition label name is required.")]
        [RegularExpression(@"^[^\u0900-\u097F]*$", ErrorMessage = "Only Alphabets and Numbers allowed.")]
        public string? PclabelName { get; set; }
        [Required(ErrorMessage = "Order sequence is required.")]
        public int? OrderSequence { get; set; }
        [Required(ErrorMessage = "Company is required.")]
        public string BusinessUnitName { get; set; }
        public int BusinessUnitId { get; set; }
        [Required(ErrorMessage = "Buisness partner is required.")]
        public string? BusinessPartnerName { get; set; }
        public int? BusinessPartnerId { get; set; }
        public bool? IsSweetenerApplicable { get; set; }
        public string? Date { get; set; }
    }
}
