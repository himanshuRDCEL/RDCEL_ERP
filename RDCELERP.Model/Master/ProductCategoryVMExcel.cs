using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDCELERP.Model.Master
{
    public class ProductCategoryVMExcel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Product category name is required.")]
        [RegularExpression(@"^[^\u0900-\u097F]*$", ErrorMessage = "Only Alphabets and Numbers allowed.")]
        [DisplayName("Product Category Name")]
        [StringLength(40)]
        public string? Name { get; set; }
        [Required(ErrorMessage = "Description  is required.")]
        [RegularExpression(@"^[^\u0900-\u097F]*$", ErrorMessage = "Only Alphabets and Numbers allowed.")]
        [StringLength(150)]
        public string? Description { get; set; }
        [Required(ErrorMessage = "Code is required.")]
        [RegularExpression(@"^[^\u0900-\u097F]*$", ErrorMessage = "Only Alphabets and Numbers allowed.")]
        [StringLength(30)]
        public string? Code { get; set; }

        public bool? IsAllowedForOld { get; set; }
        public bool? IsAllowedForNew { get; set; }
        [Required(ErrorMessage = "Description for ABB is required.")]
        [RegularExpression(@"^[^\u0900-\u097F]*$", ErrorMessage = "Only Alphabets and Numbers allowed.")]
        public string? DescriptionForAbb { get; set; }
        public bool IsActive { get; set; }
        public string? Remarks { get; set; }
    }
}
