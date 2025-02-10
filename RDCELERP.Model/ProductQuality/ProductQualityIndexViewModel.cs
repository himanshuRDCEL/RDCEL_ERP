using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDCELERP.Model.Base;

namespace RDCELERP.Model.ProductQuality
{
   public class ProductQualityIndexViewModel : BaseViewModel
    {
        public int? ProductQualityIndexId { get; set; }
        [RegularExpression(@"^[^\u0900-\u097F]*$", ErrorMessage = "Only Alphabets and Numbers allowed.")]
        [Required(ErrorMessage = "Name is required.")]
        [DisplayName("Name")]
        [StringLength(50)]
        public string? Name { get; set; }
        [Required(ErrorMessage = "Product category is required.")]
        [DisplayName("Product Category")]
        public string? ProductCategoryName { get; set; }
        public int? ProductCategoryId { get; set; }

       
        public string? Date { get; set; }


        [Required(ErrorMessage = "Excellent description is required.")]
        [RegularExpression(@"^[^\u0900-\u097F]*$", ErrorMessage = "Only Alphabets and Numbers allowed.")]
        [StringLength(200)]
        [DisplayName("Excellent")]
        
        public string? ExcellentDesc { get; set; }
        [Required(ErrorMessage = "Good description is required.")]
        [RegularExpression(@"^[^\u0900-\u097F]*$", ErrorMessage = "Only Alphabets and Numbers allowed.")]
        [StringLength(200)]
        [DisplayName("Good")]
       
        public string? GoodDesc { get; set; }
        [Required(ErrorMessage = "Average description is required.")]
        [RegularExpression(@"^[^\u0900-\u097F]*$", ErrorMessage = "Only Alphabets and Numbers allowed.")]
        [StringLength(200)]
        [DisplayName("Average")]
       
        public string? AverageDesc { get; set; }
        [Required(ErrorMessage = "Non working description is required.")]
        [RegularExpression(@"^[^\u0900-\u097F]*$", ErrorMessage = "Only Alphabets and Numbers allowed.")]
        [StringLength(200)]
        [DisplayName("Non Working")]
        
        public string? NonWorkingDesc { get; set; }
    }
}
