using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDCELERP.Model.Base;
using RDCELERP.Model.Product;

namespace RDCELERP.Model.Master
{
    public class ProductCategoryViewModel : BaseViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Product category name is required.")]
        [RegularExpression(@"^[^\u0900-\u097F]*$", ErrorMessage = "Only Alphabets and Numbers allowed.")]
        [DisplayName("Product Category Name")]
        [StringLength(40)]
        public string? Name { get; set; }
        [Required(ErrorMessage = "Description  is required.")]
        [RegularExpression(@"^[^\u0900-\u097F]*$", ErrorMessage = "Only Alphabets and Numbers allowed.")]
        [StringLength(50)]
        public string? Description { get; set; }
        [Required(ErrorMessage = "Code is required.")]
        [RegularExpression(@"^[ A-Za-z0-9 +-]*$", ErrorMessage = "Only Alphabets, Numbers and symbol (+,-) allowed.")]
        [StringLength(30)]
        public string? Code { get; set; }
        public string? ProductCategoryImageUrl { get; set; }
        public string? ProductCategoryImage { get; set; }
        public string? ProductCategoryImageLink { get; set; }
        public bool? IsAllowedForOld { get; set; }
        public bool? IsAllowedForNew { get; set; }
        [Required(ErrorMessage = "Description for ABB is required.")]
        [RegularExpression(@"^[^\u0900-\u097F]*$", ErrorMessage = "Only Alphabets and Numbers allowed.")]
        [StringLength(50)]
        public string? DescriptionForAbb { get; set; }
        public string? Date { get; set; }
        public int Buid { get; set; }
        public bool Selected { get; set; }
        public List<int?>? SelectedProduct { get; set; }
        public IList<ProductCategoryViewModel>? ProductCategoryViewModelList { get; set; }
        public IList<ProductTypeViewModel>? ProductTypeViewModelList { get; set; }

        public ProductCategoryVMExcel? ProductCategoryVM { get; set; }
        public IFormFile? UploadProductCategory { get; set; }
        public List<ProductCategoryVMExcel>? ProductCategoryVMList { get; set; }
        public List<ProductCategoryVMExcel>? ProductCategoryVMErrorList { get; set; }

    }

    public class BuProductCatDataModel
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Code { get; set; }
        public string? ProductCategoryImage { get; set; }

        public bool? IsAllowedForOld { get; set; }
        public bool? IsAllowedForNew { get; set; }

    }


}

    

