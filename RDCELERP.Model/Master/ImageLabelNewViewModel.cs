using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDCELERP.Model.Base;
using RDCELERP.Model.ImageLabel;

namespace RDCELERP.Model.Master
{
    public class ImageLabelNewViewModel : BaseViewModel
    {
        public int ImageLabelid { get; set; }
        //[Required(ErrorMessage = "Product name is required.")]
       
        public string? ProductName { get; set; }

        public string? Action { get; set; }
        [RegularExpression(@"^[A-Za-z][a-zA-Z\s]*$", ErrorMessage = "Use letters only please")]
        [Required(ErrorMessage = "Product image label is required.")]
        public string? ProductImageLabel { get; set; }
        [RegularExpression(@"^[A-Za-z][a-zA-Z\s]*$", ErrorMessage = "Use letters only please")]
        [Required(ErrorMessage = "Product image description is required.")]
        public string? ProductImageDescription { get; set; }
        [Required(ErrorMessage = "Order sequence(pattern) is required.")]
        [RegularExpression(@"^(?!-)\w*$", ErrorMessage = "Pattern should be a positive integer.")]
        public int? Pattern { get; set; }
        [Required(ErrorMessage = "Product category  is required.")]
        public string? ProductCategoryName { get; set; }
        public int? ProductCatId { get; set; }
        [Required(ErrorMessage = "Product type is required.")]
        public string? ProductTypeName { get; set; }
        public int? ProductTypeId { get; set; }
       
        public string? RegdNo { get; set; }
        
        public string? BusinessType { get; set; }
       
        public string? Base64StringValue { get; set; }
        public IFormFile? Image { get; set; }
       
        public string? FileName { get; set; }
        //[Required(ErrorMessage = "Image place holder is required.")]
        public string? ImagePlaceHolder { get; set; }
        public string? Date { get; set; }
        public string? FullPlaceHolderImageUrl { get; set; }

        public ImageLabelMasterVMExcel? ImageLabelVM { get; set; }
        public IFormFile? UploadImageLabel { get; set; }
        public List<ImageLabelMasterVMExcel>? ImageLabelVMList { get; set; }
        public List<ImageLabelMasterVMExcel>? ImageLabelVMErrorList { get; set; }
        public bool? IsMediaTypeVideo { get; set; }
    }
}
