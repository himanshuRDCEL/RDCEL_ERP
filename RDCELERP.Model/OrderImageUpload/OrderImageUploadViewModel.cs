using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDCELERP.Model.OrderImageUpload
{
    public class OrderImageUploadViewModel
    {
        [Key]
        public int OrderImageUploadId { get; set; }
        public int OrderTransId { get; set; }
        public string? ImageName { get; set; }
        public int ImageUploadby { get; set; }
        public string? LGCPickDrop { get; set; }
      
    }
}
