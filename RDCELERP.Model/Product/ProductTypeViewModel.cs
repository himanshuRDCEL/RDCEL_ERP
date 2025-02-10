using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDCELERP.Model.Base;

namespace RDCELERP.Model.Product
{
    public class ProductTypeViewModel : BaseViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Product Type Name is required.")]
        [RegularExpression(@"^[^\u0900-\u097F]*$", ErrorMessage = "Only Alphabets and Numbers allowed.")]
       
        [DisplayName("Product Type Name")]
        [StringLength(50)]
        public string? Name { get; set; }
        [Required(ErrorMessage = "Description is required")]
        [RegularExpression(@"^[^\u0900-\u097F]*$", ErrorMessage = "Only Alphabets and Numbers allowed.")]
        [StringLength(150)]
        public string? Description { get; set; }
        [StringLength(150)]
        [Required(ErrorMessage = "Description For ABB is required")]
        [RegularExpression(@"^[^\u0900-\u097F]*$", ErrorMessage = "Only Alphabets and Numbers allowed.")]
        public string? DescriptionForAbb { get; set; }
        [Required(ErrorMessage = "Code is required")]
        [RegularExpression(@"^[ A-Za-z0-9 +-]*$", ErrorMessage = "Only Alphabets, Numbers and symbol (+,-) allowed.")]
        [StringLength(30)]
        public string? Code { get; set; }
        
        [RegularExpression(@"^[^\u0900-\u097F]*$", ErrorMessage = "Only Alphabets and Numbers allowed.")]
        [StringLength(150)]
        public string? Size { get; set; }

        [Required(ErrorMessage = "Product Category is required")]
        [DisplayName("Product Category")]
        public string? ProductCategoryName { get; set; }
        public int? ProductCatId { get; set; }

        public string? ProductTypeImageUrl { get; set; }
        public string? ProductTypeImage { get; set; }
        public string? ProductTypeImageLink { get; set; }
        public bool? IsAllowedForOld { get; set; }
        public bool? IsAllowedForNew { get; set; }
        public string? Date { get; set; }

       
        public ProductTypeVMExcel? ProductTypeVM { get; set; }
        public IFormFile? UploadProductType { get; set; }
        public List<ProductTypeVMExcel>? ProductTypeVMList { get; set; }
        public List<ProductTypeVMExcel>? ProductTypeVMErrorList { get; set; }

    }

    public class ProductTypeDataResponseModel
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Size { get; set; }
        public string? Code { get; set; }
        public int? ProductCatId { get; set; }
        public string? ProductTypeImage { get; set; }

    }
    public class ProductTypeNameDescription
    {
        public int Id { get; set; }
        public string? Description { get; set; }

    }
}
