using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDCELERP.Model.OrderImageUpload;

namespace RDCELERP.Model.ImagLabel
{
    public class ImageLabelViewModel
    {
        [Key]
        public int ImageLabelid { get; set; }
        public string? ProductName { get; set; }
        public string? Action { get; set; }
        public string? ProductImageLabel { get; set; }
        public string? ProductImageDescription { get; set; }
        public int? Pattern { get; set; }
        public bool? IsActive { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string? Modifiedby { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public int? ProductCatId { get; set; }
        public string? ProductCategoryName { get; set; }
        public int? ProductTypeId { get; set; }
        public string? RegdNo { get; set; }
        public string? BusinessType { get; set; }
        public string? Base64StringValue { get; set; }
        public IFormFile? Image { get; set; }
        public string? FileName { get; set; }
        public string? ImagePlaceHolder { get; set; }
        public string? Date { get; set; }
        public string? FullPlaceHolderImageUrl { get; set; }
        public bool? IsMediaTypeVideo { get; set; }
    }
    
}
