using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDCELERP.Model.ImageLabel
{
    public class ImageLabelMasterVMExcel
    {
        public int ImageLabelid { get; set; }
       
       
        public string? ProductName { get; set; }
        [RegularExpression(@"^[A-Za-z][a-zA-Z\s]*$", ErrorMessage = "Use letters only please")]
        [Required(ErrorMessage = "Product image label is required.")]
        public string? ProductImageLabel { get; set; }
        [Required(ErrorMessage = "Product image description is required.")]
        [RegularExpression(@"^[A-Za-z][a-zA-Z\s]*$", ErrorMessage = "Use letters only please")]
        public string? ProductImageDescription { get; set; }
        [RegularExpression(@"^(?!-)\w*$", ErrorMessage = "Pattern should be a positive integer.")]
        public int Pattern { get; set; }
        [Required(ErrorMessage = "Product category name is required.")]
        public string? ProductCategory { get; set; }
        [Required(ErrorMessage = "Product type is required.")]
        public string? ProductType { get; set; }
        public string? Remarks { get; set; }
    }
}
