using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDCELERP.Model.ModelNumber
{
    public class ModelNumberVMExcel
    {
        public int ModelNumberId { get; set; }
        [Required(ErrorMessage = "Model name is required.")]
        [RegularExpression("^[ A-Za-z0-9 -]*$", ErrorMessage = "Only Alphabets and Numbers allowed.")]
        public string ModelName { get; set; }
        [RegularExpression("^[ A-Za-z0-9 -]*$", ErrorMessage = "Only Alphabets and Numbers allowed.")]
        [Required(ErrorMessage = "Description is required.")]
        public string Description { get; set; }
        [RegularExpression("^[ A-Za-z0-9 -]*$", ErrorMessage = "Only Alphabets and Numbers allowed.")]
        [Required(ErrorMessage = "Model number is required.")]
        public string Code { get; set; }
        [Required(ErrorMessage = "Brand name is required.")]
        [DisplayName("Brand Name")]
        public string BrandName { get; set; }
        public int BrandId { get; set; }
        [Required(ErrorMessage = "Product category is required.")]
        [DisplayName("Product Category Name")]
        public string ProductCategory { get; set; }
        public int ProductCategoryId { get; set; }
        [Required(ErrorMessage = "Product type is required.")]
        [DisplayName("Product Type Name")]
        public string ProductType { get; set; }
        public bool IsDefaultProduct { get; set; }
        [Required(ErrorMessage = "Company name is required.")]
        public string CompanyName { get; set; }
        public bool IsAbb { get; set; }
        public bool IsExchange { get; set; }
        [Required(ErrorMessage = "If nothing then enter 0")]
        [RegularExpression(@"^\d+(\.\d{1,2})?$", ErrorMessage = "Value must be a positive number")]
        public decimal SweetenerBu { get; set; }
        [Required(ErrorMessage = "If nothing then enter 0")]
        [RegularExpression(@"^\d+(\.\d{1,2})?$", ErrorMessage = "Value must be a positive number")]
        public decimal SweetenerBp { get; set; }
        [Required(ErrorMessage = "If nothing then enter 0")]
        [RegularExpression(@"^\d+(\.\d{1,2})?$", ErrorMessage = "Value must be a positive number")]
        public decimal SweetenerDigi2l { get; set; }
        public string? Remarks { get; set; }
    }
}
