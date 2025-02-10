using System;
using System.Collections.Generic;

namespace RDCELERP.DAL.Entities
{
    public partial class TblOrderImageUpload
    {
        public int OrderImageUploadId { get; set; }
        public int? OrderTransId { get; set; }
        public string? ImageName { get; set; }
        public int? ImageUploadby { get; set; }
        public string? LgcpickDrop { get; set; }
        public bool? IsActive { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? Modifiedby { get; set; }
        public DateTime? ModifiedDate { get; set; }

        public virtual TblUser? CreatedByNavigation { get; set; }
        public virtual TblUser? ModifiedbyNavigation { get; set; }
    }
}
