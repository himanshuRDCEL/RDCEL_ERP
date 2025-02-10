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

namespace RDCELERP.Model.ModelNumber
{
    public class ModelNumberViewModel : BaseViewModel
    {
        public int ModelNumberId { get; set; }
        [Required(ErrorMessage = "Model name is required.")]
        [RegularExpression("^[ A-Za-z0-9 -]*$", ErrorMessage = "Only Alphabets and Numbers allowed.")]
        [StringLength(50)]
        public string? ModelName { get; set; }
        [RegularExpression("^[ A-Za-z0-9 -]*$", ErrorMessage = "Only Alphabets and Numbers allowed.")]
        [Required(ErrorMessage = "Description is required.")]
        [StringLength(80)]
        public string? Description { get; set; }
        [RegularExpression("^[ A-Za-z0-9 -]*$", ErrorMessage = "Only Alphabets and Numbers allowed.")]
        [Required(ErrorMessage = "Model number is required.")]
        [StringLength(50)]
        public string? Code { get; set; }
        [Required(ErrorMessage = "Brand name is required.")]
        [DisplayName("Brand Name")]
        [StringLength(50)]
        public string? BrandName { get; set; }
        public int? BrandId { get; set; }
        [Required(ErrorMessage = "Product category name is required.")]
        [DisplayName("Product Category Name")]
        public string? ProductCategoryName { get; set; }
        public int? ProductCategoryId { get; set; }
        [Required(ErrorMessage = "Product type name is required.")]
        [DisplayName("Product Type Name")]

        public string? ProductTypeName { get; set; }
        public int? ProductTypeId { get; set; }

       
        [RegularExpression(@"^\d+(\.\d{1,2})?$", ErrorMessage = "Value must be a positive number")]
        [StringLength(10)]
        public decimal? SweetnerForDtd { get; set; }
        
        [RegularExpression(@"^\d+(\.\d{1,2})?$", ErrorMessage = "Value must be a positive number")]
        [StringLength(10)]
        public decimal? SweetnerForDtc { get; set; }
        public bool? IsDefaultProduct { get; set; }
        [Required(ErrorMessage = "Business unit name is required.")]
        [DisplayName("BusinessUnit Name")]
        public string? BusinessUnitName { get; set; }
        public int? BusinessUnitId { get; set; }
        public bool? IsAbb { get; set; }
        public bool? IsExchange { get; set; }
        [Required(ErrorMessage = "If nothing then enter 0")]
        [RegularExpression(@"^\d+(\.\d{1,2})?$", ErrorMessage = "Value must be a positive number")]
      
        public decimal? SweetenerBu { get; set; }
        [Required(ErrorMessage = "If nothing then enter 0")]
        [RegularExpression(@"^\d+(\.\d{1,2})?$", ErrorMessage = "Value must be a positive number")]
      
        public decimal? SweetenerBp { get; set; }
        [Required(ErrorMessage = "If nothing then enter 0")]
        [RegularExpression(@"^\d+(\.\d{1,2})?$", ErrorMessage = "Value must be a positive number")]
       
        public decimal? SweetenerDigi2l { get; set; }
       
        [DisplayName("Business Partner Name")]
     
        public string? BusinessPartnerName { get; set; }
        public int? BusinessPartnerId { get; set; }

        public string? Date { get; set; }
        public List<TblModelNumber>? ModelList { get; set; }

        public IFormFile? UploadModelNumber { get; set; }
        public ModelNumberVMExcel? ModelNumberVM { get; set; }
        public List<ModelNumberVMExcel>? ModelNumberVMList { get; set; }
        public List<ModelNumberVMExcel>? ModelNumberVMErrorList { get; set; }
    }
}
