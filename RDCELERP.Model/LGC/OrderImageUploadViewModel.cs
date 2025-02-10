using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDCELERP.Model.LGC
{
    public class OrderImageUploadViewModel
    {
        public int OrderImageUploadId { get; set; }
        public int? OrderTransId { get; set; }
        public string? ImageName { get; set; }
        public int? ImageUploadby { get; set; }
        public string? LgcpickDrop { get; set; }
        public bool? IsActive { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string? Modifiedby { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string? FilePath { get; set; }
        public string? ImageWithPath { get; set; }
        public string? PoDImageName { get; set; }
        public string? InvoicePDFName { get; set; }
    }
}
